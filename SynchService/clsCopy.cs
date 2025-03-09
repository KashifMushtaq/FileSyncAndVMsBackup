using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using File = System.IO.File;
using SynchServiceNS.Properties;
using System.ServiceProcess;


namespace SynchServiceNS
{
    public static class clsCopy
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string LongPath, StringBuilder ShortPath, int BufferSize);

        private static Logging m_Logger = new Logging();
        private static clsArguments m_arg;

        public static FormMain.SetTextCallback LogMessageToForm;

        public enum SynchType : int
        {
            SourceToDestinationSynch = 1,
            BothWaysSynch = 2,
            DestinationsToSourceSynch = 3
        }

        /// <summary>
        /// Called from service only
        /// </summary>
        /// <param name="arg"></param>
        public static void RunJob(clsArguments arg)
        {
            WriteLine(LOG.INFORMATION, string.Format("Creating Thread for args[{0}]", arg.getValueStringForINI()), true);

            Thread m_Thread = new Thread(new ParameterizedThreadStart(SynchThreadCopy));
            m_Thread.Name = arg.JobName;

            m_Logger.LogLevel = int.Parse(arg.LogLevel);
            m_Logger.EnableLogBuffer = false;
            m_Thread.Priority = ThreadPriority.Lowest;
            m_Thread.Start(arg);

            WriteLine(LOG.INFORMATION, string.Format("Thread Started [{0}] ", m_Thread.ManagedThreadId), true);
        }
        /// <summary>
        /// Called from Main Form
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="t"></param>
        public static void RunJob(clsArguments arg, SynchServiceNS.FormMain t)
        {
            WriteLine(LOG.INFORMATION, string.Format("Creating Thread for args[{0}]", arg.getValueStringForINI()), true);

            Thread m_Thread = new Thread(new ParameterizedThreadStart(SynchThreadCopy));

            m_Thread.Name = arg.JobName;

            m_Logger.LogLevel = int.Parse(arg.LogLevel);
            m_Logger.EnableLogBuffer = true;

            m_Thread.Priority = ThreadPriority.Lowest;
            m_Thread.Start(arg);

            WriteLine(LOG.INFORMATION, string.Format("VMs backup thread started [{0}] ", m_Thread.ManagedThreadId), true);
            FormMain.SetTextCallback d = new FormMain.SetTextCallback(t.LogMessage);

            while (m_Thread.IsAlive)
            {
                t.Invoke(d, new object[] { m_Logger.LastLoggedMessage });

                Application.DoEvents();
            }

            //cleanup anything left
            string s = m_Logger.LastLoggedMessage;

        }

        public static void RunBackupJob()
        {
            WriteLine(LOG.INFORMATION, "Creating Thread for VMs Backup", true);

            Thread m_Thread = new Thread(new ParameterizedThreadStart(VMBackupThread));

            m_Thread.Name = "VMs-Backup-Thread";

            m_Logger.LogLevel = m_Logger.LogLevel;
            m_Logger.EnableLogBuffer = true;

            m_Thread.Priority = ThreadPriority.Lowest;
            m_Thread.Start(Settings.Default);

            WriteLine(LOG.INFORMATION, string.Format("VMs backup thread started [{0}] ", m_Thread.ManagedThreadId), true);
        }

        public static void RunBackupJob(SynchServiceNS.FormMain t)
        {
            WriteLine(LOG.INFORMATION, "Creating Thread for VMs Backup", true);

            Thread m_Thread = new Thread(new ParameterizedThreadStart(VMBackupThread));

            m_Thread.Name = "VMs-Backup-Thread";

            m_Logger.LogLevel = m_Logger.LogLevel;
            m_Logger.EnableLogBuffer = true;

            m_Thread.Priority = ThreadPriority.Lowest;
            m_Thread.Start(Settings.Default);

            WriteLine(LOG.INFORMATION, string.Format("Thread Started [{0}] ", m_Thread.ManagedThreadId), true);
            FormMain.SetTextCallback d = new FormMain.SetTextCallback(t.LogMessage);

            while (m_Thread.IsAlive)
            {
                t.Invoke(d, new object[] { m_Logger.LastLoggedMessage });

                Application.DoEvents();
            }

            //cleanup anything left
            string s = m_Logger.LastLoggedMessage;

        }

        private static void VMBackupThread(object arguments)
        {
            int TotalRunningVMs = 0;

            List<string> vmsBackupist = new List<string>();
            List<string> vmsRunningList = new List<string>();
            List<string> vmsFullList = new List<string>();

            Settings settings = (Settings)arguments;

            WriteLine(LOG.INFORMATION, string.Format("Generating VMs list at [{0}]", settings.VMList), true);

            #region --------------- Process VMs Backup ---------------

            try
            {
                if (File.Exists(settings.VMList))
                {
                    File.Delete(settings.VMList);
                }

                #region --------------- Get VM List ---------------

                //* Create process to get VM's lists
                Process process = new Process();
                process.StartInfo.WorkingDirectory = settings.WorkingDir;
                process.StartInfo.FileName = settings.VMRun;
                process.StartInfo.Arguments = string.Format("list");

                int exitCode = RunProcess(process, ListVMOputHandler);
                WriteLine(LOG.INFORMATION, string.Format("Exit code [{0}] for [{1}]", exitCode, process.StartInfo.Arguments), true);

                SleepWithDoEvents(2);


                if (File.Exists(settings.VMList))
                {
                    using (StreamReader sr = File.OpenText(settings.VMList))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(line) && line.Contains(@"\")) // only add paths to vmx files
                            {
                                vmsRunningList.Add(line);
                                WriteLine(LOG.INFORMATION, string.Format("Running VM [{0}]", line), true);
                            }
                        }

                        TotalRunningVMs = vmsRunningList.Count;
                        vmsBackupist = vmsRunningList.GetRange(0, vmsRunningList.Count);
                        WriteLine(LOG.INFORMATION, string.Format("Total running VMs [{0}]", TotalRunningVMs), true);
                    }

                    if (settings.UseFullVMsList && File.Exists(settings.VMFullList))
                    {
                        using (StreamReader sr = File.OpenText(settings.VMFullList))
                        {
                            string line = string.Empty;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (!string.IsNullOrEmpty(line) && line.Contains(@"\")) // only add paths to vmx files
                                {
                                    vmsFullList.Add(line);
                                    WriteLine(LOG.INFORMATION, string.Format("VM in vmfulllist.txt [{0}]", line), true);
                                }
                            }
                        }
                        vmsBackupist = new List<string>();
                        vmsBackupist = vmsFullList.GetRange(0, vmsFullList.Count);
                        WriteLine(LOG.INFORMATION, string.Format("Total vmfulllist.txt VMs [{0}]", vmsFullList.Count), true);
                    }

                }


                #endregion


                #region --------------- stop or suspend ---------------
                foreach (string vmPath in vmsBackupist)
                {
                    string sourceVMPath = vmPath.Replace("\"", "");

                    FileInfo fi = new FileInfo(sourceVMPath);
                    string sourceDir = fi.Directory.FullName;
                    string destinationDir = Path.Combine(settings.BackupDest, GetBackupDirNameFromVM(sourceVMPath));

                    

                    if (File.Exists(sourceVMPath))
                    {
                        process = new Process();
                        process.StartInfo.WorkingDirectory = settings.WorkingDir;
                        process.StartInfo.FileName = settings.VMRun;

                        if (settings.ShutdownVms)
                        {
                            process.StartInfo.Arguments = string.Format("stop \"{0}\"", sourceVMPath);

                            WriteLine(LOG.INFORMATION, string.Format("Stopping VM [{0}]", sourceVMPath), true);
                        }
                        else
                        {
                            if (settings.SuspendOption)
                            {
                                process.StartInfo.Arguments = string.Format("suspend \"{0}\" hard", sourceVMPath);
                            }
                            else
                            {
                                process.StartInfo.Arguments = string.Format("suspend \"{0}\" soft", sourceVMPath);
                            }

                            WriteLine(LOG.INFORMATION, string.Format("suspending VM [{0}]", sourceVMPath), true);
                        }

                        exitCode = RunProcess(process);
                        WriteLine(LOG.INFORMATION, string.Format("Exit code [{0}] for [{1}]", exitCode, process.StartInfo.Arguments), true);
                    }

                    if (exitCode == 0) SleepWithDoEvents(10);

                }

                #endregion


                #region --------------- Delete snapshot ---------------

                foreach (string vmPath in vmsBackupist)
                {
                    string sourceVMPath = vmPath.Replace("\"", "");

                    FileInfo fi = new FileInfo(sourceVMPath);
                    string sourceDir = fi.Directory.FullName;
                    string destinationDir = Path.Combine(settings.BackupDest, GetBackupDirNameFromVM(sourceVMPath));

                    

                    if (File.Exists(sourceVMPath))
                    {
                        if (settings.DeleteSnapshot)
                        {
                            process = new Process();
                            process.StartInfo.WorkingDirectory = settings.WorkingDir;
                            process.StartInfo.FileName = settings.VMRun;
                            process.StartInfo.Arguments = string.Format("deleteSnapshot \"{0}\" auto-snapshot", sourceVMPath);

                            WriteLine(LOG.INFORMATION, string.Format("deleting auto-snapshot from VM [{0}]", sourceVMPath), true);

                            exitCode = RunProcess(process);
                            WriteLine(LOG.INFORMATION, string.Format("Exit code [{0}] for [{1}]", exitCode, process.StartInfo.Arguments), true);

                            if(exitCode == 0) SleepWithDoEvents(60);
                        }


                    }
                }

                #endregion


                #region --------------- Create snapshot ---------------

                foreach (string vmPath in vmsBackupist)
                {
                    string sourceVMPath = vmPath.Replace("\"", "");

                    FileInfo fi = new FileInfo(sourceVMPath);
                    string sourceDir = fi.Directory.FullName;
                    string destinationDir = Path.Combine(settings.BackupDest, GetBackupDirNameFromVM(sourceVMPath));


                    if (settings.CreateSnapshot)
                    {
                        process = new Process();
                        process.StartInfo.WorkingDirectory = settings.WorkingDir;
                        process.StartInfo.FileName = settings.VMRun;
                        process.StartInfo.Arguments = string.Format("snapshot \"{0}\" auto-snapshot", sourceVMPath);

                        WriteLine(LOG.INFORMATION, string.Format("Creating auto-snapshot for VM [{0}]", sourceVMPath), true);

                        exitCode = RunProcess(process);
                        WriteLine(LOG.INFORMATION, string.Format("Exit code [{0}] for [{1}]", exitCode, process.StartInfo.Arguments), true);

                        if (exitCode == 0) SleepWithDoEvents(30);
                    }
                }

                #endregion


                #region --------------- Take backup ---------------

                foreach (string vmPath in vmsBackupist)
                {
                    string sourceVMPath = vmPath.Replace("\"", "");

                    FileInfo fi = new FileInfo(sourceVMPath);
                    string sourceDir = fi.Directory.FullName;
                    string destinationDir = Path.Combine(settings.BackupDest, GetBackupDirNameFromVM(sourceVMPath));

                    if (settings.BackupVms)
                    {
                        CleanupSourceDir(sourceDir);

                        WriteLine(LOG.INFORMATION, string.Format("RoboCopy Source [{0}] Destination [{1}]", sourceDir, destinationDir), true);

                        process = new Process();
                        process.StartInfo.WorkingDirectory = settings.WorkingDir;

                        process.StartInfo.FileName = "robocopy.exe";
                        process.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" /MIR /xa:H /xo /ts /x /fp /tee /eta /np /njs /njh /xf *.log *.lck *.scoreboard /xd caches *.lck", sourceDir, destinationDir);

                        WriteLine(LOG.INFORMATION, string.Format("Creating backup of VM [{0}]", sourceVMPath), true);

                        exitCode = RunProcess(process);
                        WriteLine(LOG.INFORMATION, string.Format("Exit code [{0}] for [{1}]", exitCode, process.StartInfo.Arguments), true);

                        SleepWithDoEvents(5);
                    }
                }

                #endregion


                #region --------------- Kill process vmware-vmx ---------------

                // kill all vmware-vmx vmx instances
                var processes = Process.GetProcessesByName("vmware-vmx");
                foreach (var proc in processes)
                {
                    WriteLine(LOG.INFORMATION, string.Format("VMBackupThread -> Killing Process [{0}] Id [{1}]", proc.ProcessName, proc.Id), true);
                    proc.Kill();
                    SleepWithDoEvents(1);
                }

                #endregion


                #region --------------- Start VMs ---------------

                foreach (string vmPath in vmsRunningList)
                {
                    string sourceVMPath = vmPath.Replace("\"", "");

                    FileInfo fi = new FileInfo(sourceVMPath);
                    string sourceDir = fi.Directory.FullName;
                    string destinationDir = Path.Combine(settings.BackupDest, GetBackupDirNameFromVM(sourceVMPath));


                    // start by vmrun hangs VMS amd wont start correctly when
                    // backup is launched manually.
                    // for my setup all the running VMs are in auto start
                    // starting autostart service brings VM UP
                    if (Properties.Settings.Default.UseVMWareAuto)
                    {
                        WriteLine(LOG.INFORMATION, string.Format("Restarting VmwareAutostartService to bring up VM [{0}]", sourceVMPath), true);

                        SleepWithDoEvents(5);

                        RestartVMWareAutoStartService();

                        SleepWithDoEvents(5);

                        if (!VerifyVMRunning(sourceVMPath))
                        {
                            WriteLine(LOG.INFORMATION, "", true);
                            WriteLine(LOG.ERROR, string.Format("VM [{0}] is Down. Restart of VmwareAutostartService did not bring it UP", sourceVMPath), true);
                            WriteLine(LOG.INFORMATION, "", true);
                        }
                        else
                        {
                            int nowRunning = GetNowRunningVMs();
                            if (nowRunning == TotalRunningVMs)
                            {
                                WriteLine(LOG.INFORMATION, "", true);
                                WriteLine(LOG.INFORMATION, string.Format("VmwareAutostartService started all required [{0}] VMs", nowRunning), true);
                                WriteLine(LOG.INFORMATION, "", true);

                                break; //no need to keep restarting service
                            }
                        }
                    }
                    else
                    {
                        process = new Process();
                        process.StartInfo.WorkingDirectory = settings.WorkingDir;
                        process.StartInfo.FileName = settings.VMRun;
                        process.StartInfo.Arguments = string.Format("start \"{0}\" nogui", sourceVMPath);

                        WriteLine(LOG.INFORMATION, string.Format("Starting VM [{0}]", sourceVMPath), true);

                        exitCode = RunProcess(process);
                        WriteLine(LOG.INFORMATION, string.Format("Exit code [{0}] for [{1}]", exitCode, process.StartInfo.Arguments), true);

                    }

                    SleepWithDoEvents(5);
                }

                #endregion

                CheckAllVmsRunningAgain(TotalRunningVMs);

            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, ex.Message);
                WriteLine(LOG.ERROR, ex.StackTrace);
            }

            #endregion
        }

        private static void SleepWithDoEvents(int seconds)
        {
            WriteLine(LOG.INFORMATION, string.Format("Sleeping for [{0}] seconds", seconds), true);

            for (int i = 0; i < seconds; i++)
            {
                Application.DoEvents();
                Thread.Sleep(1000);
            }
        }

        private static string GetBackupDirNameFromVM(string vmPath)
        {
            string dirName = string.Empty;

            FileInfo fi = new FileInfo(vmPath);
            dirName = fi.Directory.Name;

            using (StreamReader sr = File.OpenText(vmPath))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.Contains("displayName")) // only add paths to vmx files
                    {
                        dirName = ((line.Split('=')[1]).Trim()).Replace("\"", "");
                        break;
                    }
                }
            }

            WriteLine(LOG.INFORMATION, string.Format("VM [{0}] Display Name [{1}]", vmPath, dirName), true);

            return dirName;
        }


        private static int RunProcess(Process process)
        {
            WriteLine(LOG.INFORMATION, string.Format("RunProcess -> [{0}]", process.StartInfo.Arguments), true);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);

            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

            return process.ExitCode;
        }

        private static int RunProcess(Process process, DataReceivedEventHandler listVMOputHandler)
        {
            WriteLine(LOG.INFORMATION, string.Format("List RunProcess -> [{0}]", process.StartInfo.Arguments), true);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += new DataReceivedEventHandler(listVMOputHandler);

            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

            return process.ExitCode;
        }

        private static void CheckAllVmsRunningAgain(int totalRunningInStart)
        {
            var processes = Process.GetProcessesByName("vmware-vmx");

            int runningVMs = processes.Length;

            if (runningVMs != totalRunningInStart)
            {
                foreach (var proc in processes)
                {
                    WriteLine(LOG.INFORMATION, string.Format("CheckAllVmsRunningAgain -> Killing Process [{0}] Id [{1}]", proc.ProcessName, proc.Id), true);
                    proc.Kill();
                }

                ServiceController serviceController = new ServiceController("VmwareAutostartService");
                try
                {
                    if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                    {
                        serviceController.Stop();
                    }
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running);

                    WriteLine(LOG.INFORMATION, "Re-started VmwareAutostartService", true);
                }
                catch(Exception ex)
                {
                    WriteLine(LOG.ERROR, string.Format("Failed to start VmwareAutostartService [{0}] ", ex.Message));
                }
            }
        }

        private static int GetNowRunningVMs()
        {
            int result = 0;

            Process process = new Process();
            process.StartInfo.WorkingDirectory = Settings.Default.WorkingDir;
            process.StartInfo.FileName = Settings.Default.VMRun;
            process.StartInfo.Arguments = string.Format("list");

            WriteLine(LOG.INFORMATION, string.Format("GetNowRunningVMs -> List RunProcess -> [\"{0}\"]", process.StartInfo.Arguments), true);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            //* Start process and handlers
            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                WriteLine(LOG.INFORMATION, string.Format("{0}", line), true);

                if (line.ToLower().Contains("total")) //first line -> Total running VMs: 3
                {
                    result = int.Parse(line.Split(':')[1].Trim());
                    WriteLine(LOG.INFORMATION, "", true);
                    WriteLine(LOG.INFORMATION, string.Format("Total VMs running now [{0}]", result), true);
                    WriteLine(LOG.INFORMATION, "", true);
                    break;
                }
            }

            process.WaitForExit();

            return result;
        }

        private static bool VerifyVMRunning(string sourceVMPath)
        {
            bool result = false;

            Process process = new Process();
            process.StartInfo.WorkingDirectory = Settings.Default.WorkingDir;
            process.StartInfo.FileName = Settings.Default.VMRun;
            process.StartInfo.Arguments = string.Format("list");

            WriteLine(LOG.INFORMATION, string.Format("VerifyVMRunning -> List RunProcess -> [\"{0}\"]", process.StartInfo.Arguments), true);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            //* Start process and handlers
            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                WriteLine(LOG.INFORMATION, string.Format("{0}", line), true);

                if (line.Contains(sourceVMPath))
                {
                    WriteLine(LOG.INFORMATION, "", true);
                    WriteLine(LOG.INFORMATION, string.Format("VM [{0}] is UP", line), true);
                    WriteLine(LOG.INFORMATION, "", true);
                    result = true;
                }
            }

            process.WaitForExit();

            return result;
        }

        private static void CleanupSourceDir(string sourceDir)
        {
            DirectoryInfo di = new DirectoryInfo(sourceDir);

            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.Contains(".log") || fi.Name.Contains(".scoreboard"))
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception ex)
                    {
                        WriteLine(LOG.INFORMATION, string.Format("CleanupSourceDir -> Error [{0}] ", ex.Message));
                    }
                }
            }
        }

        private static void RestartVMWareAutoStartService()
        {
            ServiceController serviceController = new ServiceController("VmwareAutostartService");
            try
            {
                if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    serviceController.Stop();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);

                WriteLine(LOG.INFORMATION, "Re-started VmwareAutostartService", true);
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, string.Format("Failed to start VmwareAutostartService [{0}] ", ex.Message));
            }
        }


        private static void SynchThreadCopy(object arguments)
        {
            try
            {
                clsArguments arg = (clsArguments)arguments;
                m_arg = arg;

                string jobSource = arg.JobSource;

                if (int.Parse(arg.SynchType) == (int)SynchType.SourceToDestinationSynch) // source to destination
                {
                    foreach (string destination in arg.JobDestinations)
                    {
                        if (!Directory.Exists(jobSource))
                        {
                            WriteLine(LOG.INFORMATION, string.Format("Source [{0}] could not be found. Can not continue.", jobSource), true);
                            continue;
                        }

                        DoWork(jobSource, destination);
                        WriteLine(LOG.INFORMATION, string.Format("Source [{0}] --> Destination [{1}] Completed", jobSource, destination), true);
                    }
                }
                else if (int.Parse(arg.SynchType) == (int)SynchType.BothWaysSynch) // both ways
                {
                    foreach (string destination in arg.JobDestinations)
                    {
                        if (!Directory.Exists(jobSource))
                        {
                            WriteLine(LOG.INFORMATION, string.Format("Source [{0}] could not be found. Can not continue.", jobSource), true);
                        }
                        else
                        {
                            DoWork(jobSource, destination);
                        }

                        if (!Directory.Exists(destination))
                        {
                            WriteLine(LOG.INFORMATION, string.Format("Destination [{0}] could not be found. Can not continue.", destination), true);
                        }
                        else
                        {
                            DoWork(destination, jobSource);
                        }

                        WriteLine(LOG.INFORMATION, string.Format("Two way [{0}] <--> [{1}] Sync Completed", jobSource, destination), true);
                    }
                }
                else if (int.Parse(arg.SynchType) == (int)SynchType.DestinationsToSourceSynch) // Destinations to source
                {
                    foreach (string destination in arg.JobDestinations)
                    {
                        if (!Directory.Exists(destination))
                        {
                            WriteLine(LOG.INFORMATION, string.Format("Destination [{0}] could not be found. Can not continue.", destination), true);
                            continue;
                        }

                        DoWork(destination, jobSource);
                        WriteLine(LOG.INFORMATION, string.Format("Destination [{0}] --> Source [{1}] Completed", destination, jobSource), true);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, String.Format("SynchThreadCopy -> {0}", ex.Message));
            }
        }

        private static void DoWork(string source, string destination)
        {
            try
            {
                
                RoboCopyMirror(source, destination);



                ////Remove empty directories
                //RemoveEmptyDirectories(source);

                //DirectoryInfo rootSource = new DirectoryInfo(source);
                //rootSource.Refresh();

                //string parPath = string.Empty;
                //string relPath = string.Empty;
                //if ((object)rootSource.Parent != null)
                //{
                //    parPath = rootSource.Parent.FullName;
                //    relPath = rootSource.FullName.Replace(parPath, "");
                //    if (isExcludedDir(relPath, rootSource.FullName))
                //    {
                //        //dir in exclusion list no need to look for sub directories and files
                //        return;
                //    }
                //    else
                //    {
                //        CopyFiles(rootSource, destination);
                //    }
                //}
                //else
                //{
                //    CopyFiles(rootSource, destination);
                //}



                //foreach (DirectoryInfo directoryInfo in rootSource.GetDirectories())
                //{
                //    Application.DoEvents();

                //    string ParentPath = directoryInfo.Parent.FullName;
                //    string thisDirRelPath = directoryInfo.FullName.Replace(ParentPath, "");
                //    if (!isExcludedDir(thisDirRelPath, directoryInfo.FullName))
                //    {
                //        string FullDestinationPath = destination + thisDirRelPath;

                //        DoWork(directoryInfo.FullName, FullDestinationPath);
                //        WriteLine(LOG.INFORMATION, String.Format("DoWork -> directoryInfo.FullName [{0}] and FullDestinationPath [{1}]", directoryInfo.FullName, FullDestinationPath));
                //    }
                //}


                ////remove files which do not exist at source anymore
                //if (int.Parse(m_arg.SynchType) == (int)SynchType.SourceToDestinationSynch)
                //{
                //    DirectoryInfo destinationDirectoryInfo = new DirectoryInfo(destination);

                //    foreach (FileInfo destFileInfo in destinationDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
                //    {
                //        string destFilePath = destFileInfo.FullName;

                //        string sourceFilePath = destFilePath.Replace(destination, source);

                //        if (!File.Exists(sourceFilePath))
                //        {
                //            //file at source does not exist. Should delete it from here to complete synchronization
                //            try
                //            {
                //                FileOperationAPIWrapper.MoveToRecycleBin(destFilePath);
                //                WriteLine(LOG.WARNING, String.Format("DoWork -> MoveToRecycleBin -> orphaned file [{0}]", destFilePath));
                //            }
                //            catch (Exception e)
                //            {
                //                WriteLine(LOG.ERROR, String.Format("DoWork -> MoveToRecycleBin Error \r\n{0}", e.ToString()));
                //            }

                //        }
                //    }
                //}

                ////Remove empty directories
                //RemoveEmptyDirectories(destination);
            }
            catch (Exception ex)
            {

                WriteLine(LOG.ERROR, String.Format("DoWork -> Error \r\n{0}", ex.ToString()));
            }
        }

        private static void RemoveEmptyDirectories(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                RemoveEmptyDirectories(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        static DateTime GetExplorer_LastAccessTimeUtc(string filename)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return System.IO.File.GetLastAccessTimeUtc(filename).Add(localOffset);
        }
        static DateTime GetExplorer_LastWriteTimeUtc(string filename)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return System.IO.File.GetLastWriteTimeUtc(filename).Add(localOffset);
        }
        static DateTime GetExplorer_CreationTimeUtc(string filename)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return System.IO.File.GetCreationTimeUtc(filename).Add(localOffset);
        }
        private static String getHash<T>(string FullName) where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                using (FileStream fStream = System.IO.File.OpenRead(FullName))
                {
                    byte[] hashBytes = crypt.ComputeHash(fStream);
                    foreach (byte bt in hashBytes)
                    {
                        sb.Append(bt.ToString("x2"));
                    }
                }
            }
            return sb.ToString();
        }
        static bool FilesAreNotEqual(FileInfo first, FileInfo second)
        {
            Boolean bR = false;

            if (!System.IO.File.Exists(second.FullName))
            {
                return true;
            }

            try
            {
                long fileLen_1 = first.Length;
                long fileLen_2 = second.Length;
                DateTime creationDate_1 = GetExplorer_CreationTimeUtc(first.FullName);
                DateTime creationDate_2 = GetExplorer_CreationTimeUtc(second.FullName);

                DateTime lastWriteTime_1 = GetExplorer_LastWriteTimeUtc(first.FullName);
                DateTime lastWriteTime_2 = GetExplorer_LastWriteTimeUtc(second.FullName);

                DateTime creationDate_2_1 = first.CreationTimeUtc;
                DateTime creationDate_2_2 = second.CreationTimeUtc;

                DateTime lastWriteTime_2_1 = first.LastWriteTimeUtc;
                DateTime lastWriteTime_2_2 = second.LastWriteTimeUtc;


                if ((fileLen_1.Equals(fileLen_2) && creationDate_1.Equals(creationDate_2) && lastWriteTime_1.Equals(lastWriteTime_2)) ||
                (fileLen_1.Equals(fileLen_2) && creationDate_2_1.Equals(creationDate_2_2) && lastWriteTime_2_1.Equals(lastWriteTime_2_2)))
                {
                    // files not changed
                    Application.DoEvents();

                    WriteLine(LOG.DEBUG, String.Format("FILES [{0}] == [{1}], NOT CHANGED", first.Name, second.Name));
                }
                else
                {
                    if (getHash<MD5>(first.FullName).Equals(getHash<MD5>(second.FullName)))
                    {
                        WriteLine(LOG.DEBUG, String.Format("Hash Compare -> FILES [{0}] == [{1}], NOT CHANGED", first.Name, second.Name));
                    }
                    else
                    {
                        WriteLine(LOG.INFORMATION, String.Format("Compare Hash -> Files are different -> first [{0}] and second [{1}]", lastWriteTime_1, lastWriteTime_2));
                        bR = true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, String.Format("FilesAreEqual_OneByte -> Error \r\n{0}", ex.ToString()));
            }
            return bR;
        }

        private static void CopyFiles(DirectoryInfo thisDir, string Destination)
        {
            thisDir.Refresh();

            foreach (FileInfo sourceFile in thisDir.GetFiles())
            {
                Application.DoEvents();

                string sourceFilePath = sourceFile.FullName;

                FileInfo sF = new FileInfo(sourceFilePath);
                string destinationFilePath = Destination + @"\" + sourceFile.Name;
                string sourceFileRelPath = @"\" + sF.Name;

                if (!isExcludedFile(sourceFileRelPath) || isIncludedFile(sourceFileRelPath))
                {
                    FileInfo destinationFile = null;
                    bool bFilesAreNotEqual;

                    if (System.IO.File.Exists(destinationFilePath))
                    {
                        destinationFile = new FileInfo(destinationFilePath);
                        destinationFile.Refresh();
                        sF.Refresh();

                        bFilesAreNotEqual = FilesAreNotEqual(sF, destinationFile);

                        destinationFile.Refresh();
                        sourceFile.Refresh();
                        sF.Refresh();
                    }
                    else
                    {
                        bFilesAreNotEqual = true;
                    }

                    if (bFilesAreNotEqual)
                    {
                        bool destExists = System.IO.File.Exists(destinationFilePath);

                        if (!destExists)
                        {
                            if (sF.Length == 0)
                            {
                                WriteLine(LOG.ERROR, String.Format("Source FILE [{0}] has 0 bytes. File may be corrupt", sF.FullName));
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    if (!Directory.Exists(Destination))
                                    {
                                        WriteLine(LOG.INFORMATION, String.Format("Destination Directory Path [{0}] Created", Destination));
                                        Directory.CreateDirectory(Destination);
                                    }

                                    CopyFileExactly(sF.FullName, destinationFilePath);

                                    WriteLine(LOG.INFORMATION, string.Format("File copied From [{0}] to [{1}]", sourceFile.FullName, destinationFilePath), true);
                                }
                                catch (Exception ex)
                                {
                                    WriteLine(LOG.ERROR, String.Format("CopyFiles -> File not at destination -> Error \r\n{0}", ex.ToString()));
                                }
                            }
                        }
                        else if (destinationFile.LastWriteTimeUtc < sF.LastWriteTimeUtc)
                        {
                            if (sF.Length == 0)
                            {
                                WriteLine(LOG.ERROR, String.Format("Source FILE [{0}] has 0 bytes. File may be corrupt", sF.FullName));
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    CopyFileExactly(sF.FullName, destinationFilePath);

                                    WriteLine(LOG.INFORMATION, string.Format("File UPDATED From [{0}] to [{1}]", sourceFile.FullName, destinationFile.FullName), true);
                                }
                                catch (Exception ex)
                                {
                                    WriteLine(LOG.ERROR, String.Format("CopyFiles -> File present at destination -> Error \r\n{0}", ex.ToString()));
                                }
                            }
                        }
                        else if (destinationFile.LastWriteTimeUtc > sourceFile.LastWriteTimeUtc)
                        {
                            WriteLine(LOG.WARNING, string.Format("Destination[{1}] file is NEWER THAN Source[{0}]", sourceFile.FullName, destinationFile.FullName), true);
                        }
                    }
                    else
                    {
                        WriteLine(LOG.INFORMATION, string.Format("FILES NOT CHANGED - Source [{0}] Destination [{1}]", sourceFile.FullName, destinationFile.FullName));
                        Application.DoEvents();
                    }
                }
            }
        }

        private static void CopyFileExactly(string copyFromPath, string copyToPath)
        {
            try
            {
                if (System.IO.File.Exists(copyToPath))
                {
                    var target = new FileInfo(copyToPath);
                    if (target.IsReadOnly)
                        target.IsReadOnly = false;
                }

                var origin = new FileInfo(copyFromPath);
                origin.CopyTo(copyToPath, true);

                var destination = new FileInfo(copyToPath);
                if (destination.IsReadOnly)
                {
                    destination.IsReadOnly = false;
                    destination.CreationTime = origin.CreationTime;
                    destination.LastWriteTime = origin.LastWriteTime;
                    destination.LastAccessTime = origin.LastAccessTime;
                    destination.IsReadOnly = true;
                }
                else
                {
                    destination.CreationTime = origin.CreationTime;
                    destination.LastWriteTime = origin.LastWriteTime;
                    destination.LastAccessTime = origin.LastAccessTime;
                }
            }
            catch (Exception ex)
            {
                WriteLine(LOG.ERROR, String.Format("CopyFiles -> CopyFileExactly -> Error {0}", ex.Message));
            }
        }

        private static void WriteLine(LOG Level, string Message, bool bIgnoreLevel)
        {
            try
            {
                m_Logger.WriteToLog(Level, Message, bIgnoreLevel);
            }
            catch (Exception) { }
        }
        private static void WriteLine(LOG Level, string Message)
        {
            WriteLine(Level, Message, false);
        }
        private static bool isExcludedDir(string DirNameWithRelativePath, string FullPath)
        {
            bool bR = false;

            if (Directory.GetFiles(FullPath).Length == 0 && Directory.GetDirectories(FullPath).Length == 0)
            {
                WriteLine(LOG.INFORMATION, string.Format("Dir [{0}] has no files and sub dir, will return as it is excluded", FullPath));
                return true; // no files and sub dir in this dir, just return as it is excluded
            }

            if (m_arg.DirFiltersEx == null) return bR;

            try
            {
                foreach (string sFilter in m_arg.DirFiltersEx)
                {
                    Application.DoEvents();
                    //Thread.Sleep(100);
                    Regex regex = new Regex(sFilter + "$", RegexOptions.IgnoreCase);
                    string sDirName = "/" + sFilter.Replace("\\", "");
                    string sFullPath = FullPath.Replace("\\", "/");
                    if (regex.IsMatch(DirNameWithRelativePath) || sFullPath.IndexOf(sDirName, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        WriteLine(LOG.INFORMATION, string.Format("Dir [{0}] is in exclusion list", sFullPath));
                        bR = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return bR;
        }
        private static bool isExcludedFile(string FileNameWithRelativePath)
        {
            bool bR = false;

            if (m_arg.FileFiltersEx == null && m_arg.FileFiltersIn != null)
            {
                m_arg.FileFiltersEx = "*.*".Split(',');
            }

            if (m_arg.FileFiltersEx != null)
            {

                if (!string.IsNullOrEmpty(m_arg.FileFiltersEx[0]))
                {
                    foreach (string sFil in m_arg.FileFiltersEx)
                    {
                        if (sFil.Contains("*.*")) return true;
                    }
                }
            }
            else
            {
                return false;
            }

            try
            {
                foreach (string sFilter in m_arg.FileFiltersEx)
                {
                    Application.DoEvents();
                    //Thread.Sleep(100);
                    if (string.IsNullOrEmpty(sFilter)) continue;

                    //. $ ^ { [ ( | ) * + ? \
                    string Filter = sFilter.Replace(@"\", @"\\");
                    Filter = Filter.Replace("^", @"\^");
                    Filter = Filter.Replace(".", @"\.");
                    Filter = Filter.Replace("?", @"\?");
                    Filter = Filter.Replace("[", @"\[");
                    Filter = Filter.Replace("]", @"\]");
                    Filter = Filter.Replace("{", @"\{");
                    Filter = Filter.Replace("}", @"\}");
                    Filter = Filter.Replace("(", @"\(");
                    Filter = Filter.Replace(")", @"\)");
                    Filter = Filter.Replace("+", @"\+");
                    Filter = Filter.Replace("|", @"\|");
                    Filter = Filter.Replace("$", @"\$");
                    Filter = Filter.Replace("*", @"\.*");


                    Regex regex = new Regex("" + Filter + "$", RegexOptions.IgnoreCase);
                    bR = regex.IsMatch(FileNameWithRelativePath);
                    if (bR)
                    {
                        WriteLine(LOG.INFORMATION, string.Format("File [{0}] is in exclusion list", FileNameWithRelativePath));
                        bR = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return bR;
        }
        private static bool isIncludedFile(string FileNameWithRelativePath)
        {
            bool bR = false;

            if (m_arg.FileFiltersIn == null)
            {
                if (m_arg.FileFiltersEx == null)
                    return true;
                else
                    return false;
            }

            if (!string.IsNullOrEmpty(m_arg.FileFiltersIn[0]))
            {
                foreach (string sFil in m_arg.FileFiltersIn)
                {
                    if (sFil.Contains("*.*")) return true;
                }
            }

            try
            {
                foreach (string sFilter in m_arg.FileFiltersIn)
                {
                    if (string.IsNullOrEmpty(sFilter)) continue;

                    //. $ ^ { [ ( | ) * + ? \
                    string Filter = sFilter.Replace(@"\", @"\\");
                    Filter = Filter.Replace("^", @"\^");
                    Filter = Filter.Replace(".", @"\.");
                    Filter = Filter.Replace("?", @"\?");
                    Filter = Filter.Replace("[", @"\[");
                    Filter = Filter.Replace("]", @"\]");
                    Filter = Filter.Replace("{", @"\{");
                    Filter = Filter.Replace("}", @"\}");
                    Filter = Filter.Replace("(", @"\(");
                    Filter = Filter.Replace(")", @"\)");
                    Filter = Filter.Replace("+", @"\+");
                    Filter = Filter.Replace("|", @"\|");
                    Filter = Filter.Replace("$", @"\$");
                    Filter = Filter.Replace("*", @"\.*");


                    Regex regex = new Regex("" + Filter + "$", RegexOptions.IgnoreCase);
                    bR = regex.IsMatch(FileNameWithRelativePath);
                    if (bR)
                    {
                        WriteLine(LOG.INFORMATION, string.Format("File [{0}] is in inclusion list", FileNameWithRelativePath));
                        bR = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return bR;
        }

        /// <summary>
        /// Function use a process call to execute a batch command to synchronize source folder with destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private static void RoboCopyMirror(string source, string destination)
        {
            //* Create your Process
            Process process = new Process();
            process.StartInfo.FileName = "robocopy.exe";

            process.StartInfo.Arguments = string.Format("\"{0}\"" + " " + "\"{1}\"" + " " + "{2}", source, destination, "/MIR /xa:H /xo /ts /x /fp /tee /eta /np /njs /njh");

            if (m_arg != null && m_arg.FileFiltersIn != null)
            {
                if (m_arg.FileFiltersIn.Length > 0)
                {
                    foreach (string filter in m_arg.FileFiltersIn)
                    {
                        process.StartInfo.Arguments += " " + filter;
                    }
                }
            }

            if (m_arg != null && m_arg.FileFiltersEx != null)
            {
                if (m_arg.FileFiltersEx.Length>0) process.StartInfo.Arguments += " /xf";
                foreach (string filter in m_arg.FileFiltersEx)
                {
                    process.StartInfo.Arguments += " " + filter;
                }
            }

            if (m_arg != null && m_arg.DirFiltersEx != null)
            {
                if (m_arg.FileFiltersEx.Length > 0) process.StartInfo.Arguments += " /xd";
                foreach (string filter in m_arg.DirFiltersEx)
                {
                    process.StartInfo.Arguments += " " + filter;
                }
            }

            WriteLine(LOG.INFORMATION, string.Format("Command : robocopy.exe [{0}]", process.StartInfo.Arguments), true);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);

            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            WriteLine(LOG.INFORMATION, string.Format("{0}", outLine.Data));
        }

        static void ListVMOputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            TextWriter tw = new StreamWriter(Settings.Default.VMList, true);
            tw.WriteLine(string.Format("{0}", outLine.Data));
            tw.Close();
        }
    }
}


/*
class Program
{
    static void Main(string[] args)
    {
        string sourcePath = @"C:\Users\artio\Desktop\FASassignment\root\dir1";
        string destinationPath = @"C:\Users\artio\Desktop\FASassignment\root\dir2";
        var source = new DirectoryInfo(sourcePath);
        var destination = new DirectoryInfo(destinationPath);

        CopyFolderContents(sourcePath, destinationPath, "", true, true);
        DeleteAll(source, destination);
    }

    public static void CopyFolderContents(string sourceFolder, string destinationFolder, string mask, Boolean createFolders, Boolean recurseFolders)
    {
        try
        {

            var exDir = sourceFolder;
            var dir = new DirectoryInfo(exDir);
            var destDir = new DirectoryInfo(destinationFolder);

            SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (string sourceFile in Directory.GetFiles(dir.ToString(), mask, so))
            {
                FileInfo srcFile = new FileInfo(sourceFile);
                string srcFileName = srcFile.Name;

                // Create a destination that matches the source structure
                FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.Replace(sourceFolder, ""));

                if (!Directory.Exists(destFile.DirectoryName) && createFolders)
                {
                    Directory.CreateDirectory(destFile.DirectoryName);
                }

                //Check if src file was modified and modify the destination file
                if (srcFile.LastWriteTime > destFile.LastWriteTime || !destFile.Exists)
                {
                    File.Copy(srcFile.FullName, destFile.FullName, true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
        }
    }

    private static void DeleteAll(DirectoryInfo source, DirectoryInfo target)
    {
        if (!source.Exists)
        {
            target.Delete(true);
            return;
        }

        // Delete each existing file in target directory not existing in the source directory.
        foreach (FileInfo fi in target.GetFiles())
        {
            var sourceFile = Path.Combine(source.FullName, fi.Name);
            if (!File.Exists(sourceFile)) //Source file doesn't exist, delete target file
            {
                fi.Delete();
            }
        }

        // Delete non existing files in each subdirectory using recursion.
        foreach (DirectoryInfo diTargetSubDir in target.GetDirectories())
        {
            DirectoryInfo nextSourceSubDir = new DirectoryInfo(Path.Combine(source.FullName, diTargetSubDir.Name));
            DeleteAll(nextSourceSubDir, diTargetSubDir);
        }
    }
} 
 */