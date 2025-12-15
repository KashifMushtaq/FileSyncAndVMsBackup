using SynchServiceNS.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace SynchServiceNS
{

    public partial class SynchService : ServiceBase
    {
        private static INI m_INI = new INI();
        private static Logging m_Logger = new Logging();
        private static object threadLock=new object();

        private const string SPLIT_CHAR_MAIN = "|";
        private const string SPLIT_CHAR = ",";
        private const string SPLIT_CHAR_EQUAL = "=";


        AutoResetEvent autoEventMonitor = new AutoResetEvent(false);
        AutoResetEvent autoEventMonitorVMBackup = new AutoResetEvent(false);
        AutoResetEvent autoEventMonitorCleanLogs = new AutoResetEvent(false);

        private static Timer m_monitorThread;
        private static Timer m_monitorThreadVMBackup;
        private static Timer m_monitorLogFilesThread;

        public SynchService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
            try
            {
                //create a timer thread which ticks every minutes
                m_monitorThread = new Timer(new TimerCallback(Timer_Tick), autoEventMonitor, 10000, 60000);
                m_monitorThreadVMBackup = new Timer(new TimerCallback(Timer_Tick_VMBackup), autoEventMonitorVMBackup, 10000, int.Parse(TimeSpan.FromHours(1).TotalMilliseconds.ToString()));
                m_monitorLogFilesThread = new Timer(new TimerCallback(Timer_Tick_CleanLogs), autoEventMonitorCleanLogs, 10000, 60000);
                m_Logger.EnableLogBuffer = false;
                WriteLine(LOG.INFORMATION, "Synch Service Started");
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, string.Format("OnStart -> Error [{0}]", ex.Message));
            }
        }

        protected override void OnStop()
        {
            try
            {
                WriteLine(LOG.INFORMATION, "Synch Service Stopped");
                m_monitorThread.Dispose(autoEventMonitor);
                m_monitorThreadVMBackup.Dispose(autoEventMonitorVMBackup);
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, string.Format("OnStop -> Error [{0}]", ex.Message));
            }
        }

        /// <summary>
        /// Timer to trigger VM Backup Jobs
        /// </summary>
        /// <param name="stateInfo"></param>
        public static void Timer_Tick_VMBackup(Object stateInfo)
        {
            try
            {
                double JobRunInterval = double.Parse(m_INI.IniReadValue("VM-BACKUP", "JobRunInterval"));
                DateTime JobRunTime = DateTime.Parse(m_INI.IniReadValue("VM-BACKUP", "JobRunTime"));

                if (DateTime.Now.Ticks >= JobRunTime.AddDays(JobRunInterval).Ticks)
                {
                    if (DateTime.Now.Hour >= JobRunTime.AddDays(JobRunInterval).Hour && DateTime.Now.Minute >= JobRunTime.AddDays(JobRunInterval).Minute)
                    {
                        WriteLine(LOG.INFORMATION, string.Format("Backup is due. Date Now [{0}], Scheduled [{1}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), JobRunTime.AddDays(JobRunInterval).ToString("yyyy-MM-dd HH:mm")));

                        DateTime NextBackupTime = JobRunTime.AddDays(JobRunInterval); // Next run DateTime
                        m_INI.IniWriteValue("VM-BACKUP", "JobRunTime", NextBackupTime.ToString("yyyy-MMM-dd HH:mm:ss"));

                        clsCopy.RunBackupJob();
                    }
                    else
                    {
                        WriteLine(LOG.INFORMATION, string.Format("Backup job is due today [{0}] at time [{1}]. Remaining time [{2}]", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm"), (JobRunTime - DateTime.Now).Hours + ":" + (JobRunTime - DateTime.Now).Minutes));
                    }
                }
                else
                {
                    WriteLine(LOG.DEBUG, string.Format("Backup job is not due. Date Now [{0}], Interval [{1}], Scheduled [{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), JobRunInterval, JobRunTime.AddDays(JobRunInterval).ToString("yyyy-MM-dd HH:mm")));
                }
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, string.Format("Timer_Tick_VMBackup -> Error [{0}]", ex.Message));

            }
        }

        public static void Timer_Tick_CleanLogs(Object stateInfo)
        {
            String logFileRootDir = Path.GetDirectoryName(m_Logger.LogFilePath);
            int daysOlderThan = 07;
            DateTime thresholdDate = DateTime.Now.AddDays(-daysOlderThan);

            DirectoryInfo directory = new DirectoryInfo(logFileRootDir);
            // Retrieve files older than X days
            var oldFiles = directory.GetFiles()
                .Where(file => file.LastWriteTime < thresholdDate)
                .ToList();

            foreach (var file in oldFiles)
            {
                try
                {
                    if (file.Extension.Equals(".log", StringComparison.OrdinalIgnoreCase))
                    {
                        file.Delete();
                        WriteLine(LOG.INFORMATION, string.Format("Timer_Tick_CleanLogs -> Log File [{0}] removed", file.FullName));
                    }
                }            
                catch (Exception ex)
                {
                    WriteLine(LOG.ERROR, string.Format("Timer_Tick_CleanLogs -> Error [{0}]", ex.Message));
                }
            }
        }


        public static void Timer_Tick(Object stateInfo)
        {
            string[] iniSECTION;

            lock (threadLock)
            {
                //INI File
                //Job_Name=Job Description|Source|Destination 1...n|FileFilterIn 1...n|FileFilterEx 1...n|DirFilterEx 1...n|SynchType|JobStatus|LastRun|RunAfter

                WriteLine(LOG.DEBUG, "INI Path " + m_INI.m_PATH);

                //read all jobs from INI file
                iniSECTION = m_INI.ReadSection();

                WriteLine(LOG.DEBUG, "Timer_Tick -> after read");
            }



            if (iniSECTION == null)
            {
                WriteLine(LOG.INFORMATION, "No Jobs in INI File.");
                return;
            }

            List<string> Jobs = new List<string>();

            WriteLine(LOG.DEBUG, string.Format("Total Jobs [{0}]", iniSECTION.Length));

            //make job list
            foreach (string iniValue in iniSECTION)
            {
                string JobName = iniValue.Split(SPLIT_CHAR_EQUAL[0])[0];
                if (!string.IsNullOrEmpty(JobName))
                {
                    Jobs.Add(JobName.Trim());
                }
            }

            //get job data
            foreach (string Job in Jobs)
            {
                string JobValue = m_INI.IniReadValue(Job);

                if (!string.IsNullOrEmpty(JobValue))
                {
                    //INI File
                    //Job_Name=Job Description|Source|Destination 1...n|FileFilterIn 1...n|FileFilterEx 1...n|DirFilterEx 1...n|SynchType|JobStatus|LastRun|RunAfter
                    string[] JobData = JobValue.Split(SPLIT_CHAR_MAIN[0]);

                    WriteLine(LOG.DEBUG, string.Format("Parsing Job [{0}]", Job));

                    try
                    {
                        clsArguments arg=new clsArguments();
                        arg.LoadData(Job, JobData);
                        
                        m_Logger.LogLevel = int.Parse(arg.LogLevel);

                        if (arg.JobStatus == "1")
                        {
                            WriteLine(LOG.DEBUG, string.Format("Job [{0}] is Active", Job));
                            if (arg.UseRunAt == "1")
                            {
                                TimeSpan lastRun = TimeSpan.FromTicks(long.Parse(arg.JobLastRun));
                                TimeSpan timeSpanNow = TimeSpan.FromTicks(DateTime.Now.Ticks);
                                double intervalMinutes = double.Parse("60");
                                int hr = int.Parse(arg.RunAt.Split(':')[0]);
                                int min = int.Parse(arg.RunAt.Split(':')[1]);
                                
                                WriteLine(LOG.DEBUG, string.Format("TimeSpan Total Minutes Now[{0}])", timeSpanNow.TotalMinutes));
                                WriteLine(LOG.DEBUG, string.Format("TimeSpan Total Minutes Last Run[{0}]", lastRun.TotalMinutes));
                                WriteLine(LOG.DEBUG, string.Format("Now - Last Run[{0}] Minutes", (timeSpanNow.TotalMinutes - lastRun.TotalMinutes)));
                                WriteLine(LOG.DEBUG, string.Format("Job [{0}] Interval[{1}] Minutes)", Job, intervalMinutes));
                                WriteLine(LOG.DEBUG, string.Format("Job [{0}] Hr[{1}])", Job, hr));
                                WriteLine(LOG.DEBUG, string.Format("Job [{0}] Min[{1}])", Job, min));

                                if ((timeSpanNow.TotalMinutes - lastRun.TotalMinutes) > intervalMinutes && DateTime.Now.Hour == hr && DateTime.Now.Minute==min)
                                {
                                    WriteLine(LOG.DEBUG, string.Format("Thread Created for Job[{0}], Arguments [{1}]", Job, arg.getValueStringForINI()));
                                    clsCopy.RunJob(arg);
                                    arg.JobLastRun = timeSpanNow.Ticks.ToString();
                                    m_INI.IniWriteValue(Job, arg.getValueStringForINI());
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(arg.JobLastRun) || arg.JobLastRun == "0")
                                    {
                                        arg.JobLastRun = timeSpanNow.Ticks.ToString();
                                        m_INI.IniWriteValue(Job, arg.getValueStringForINI());
                                    }
                                    WriteLine(LOG.DEBUG, string.Format("Job[{0}] run is not due now", Job));
                                }
                            }
                            else
                            {
                                TimeSpan lastRun = TimeSpan.FromTicks(long.Parse(arg.JobLastRun));
                                double intervalMinutes = double.Parse(arg.RunAfter);
                                TimeSpan timeSpaninterval = TimeSpan.FromTicks(DateTime.Now.AddMinutes(intervalMinutes).Ticks);
                                TimeSpan timeSpanNow = TimeSpan.FromTicks(DateTime.Now.Ticks);

                                WriteLine(LOG.DEBUG, string.Format("TimeSpan Total Minutes Now[{0}])", timeSpanNow.TotalMinutes));
                                WriteLine(LOG.DEBUG, string.Format("TimeSpan Total Minutes Last Run[{0}]", lastRun.TotalMinutes));
                                WriteLine(LOG.DEBUG, string.Format("Now - Last Run[{0}] Minutes", (timeSpanNow.TotalMinutes - lastRun.TotalMinutes)));
                                WriteLine(LOG.DEBUG, string.Format("Job [{0}] Interval[{1}] Minutes)", Job, intervalMinutes));

                                if ((timeSpanNow.TotalMinutes - lastRun.TotalMinutes) > intervalMinutes)
                                {
                                    WriteLine(LOG.DEBUG, string.Format("Thread Created for Job[{0}], Arguments [{1}]", Job, arg.getValueStringForINI()));

                                    clsCopy.RunJob(arg);

                                    arg.JobLastRun = timeSpanNow.Ticks.ToString();
                                    m_INI.IniWriteValue(Job, arg.getValueStringForINI());
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(arg.JobLastRun) || arg.JobLastRun == "0")
                                    {
                                        arg.JobLastRun = timeSpanNow.Ticks.ToString();
                                        m_INI.IniWriteValue(Job, arg.getValueStringForINI());
                                    }
                                    WriteLine(LOG.DEBUG, string.Format("Job[{0}] run is not due now", Job));
                                }
                            }
                        }
                        else
                        {
                            WriteLine(LOG.DEBUG, string.Format("Job [{0}] is Disabled", Job));
                        }
                    }
                    catch (Exception Ex)
                    {
                        //error in this job parameters, move to next job
                        WriteLine(LOG.ERROR, string.Format("Error in Running Job [{0}]", Job));
                        WriteLine(LOG.ERROR, string.Format("[{0}]", Ex.Message));
                        continue;
                    }
 
                }
            }

        }

        private static void WriteLine(LOG Level, string Message)
        {
            m_Logger.WriteToLog(Level, Message);
        }
    }
}
