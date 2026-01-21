using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace SynchServiceNS
{
    public partial class FormMain : Form
    {
        private const string SPLIT_CHAR_MAIN = "|";
        private const string SPLIT_CHAR = ",";
        private const string SPLIT_CHAR_EQUAL = "=";
        static private object threadLock = new object();
        private static ServiceController serviceController = new ServiceController();

        private Logging m_logger = new Logging();
        private static INI m_INI = new INI();

        public delegate void SetTextCallback(string text);

        public FormMain()
        {
            InitializeComponent();
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            try
            {
                this.richTextBox_Log.Text = "";
                m_logger.ClearLogFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LogMessage(string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.richTextBox_Log.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(LogMessage);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox_Status.AppendText(  text );
                Application.DoEvents();
                Thread.Sleep(500);
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            serviceController.Refresh();

            richTextBox_Log.Text = m_logger.getLogText();
            try
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    button_StartService.Enabled = false;
                    buttonStopService.Enabled = true;
                }
                else if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    button_StartService.Enabled = true;
                    buttonStopService.Enabled = false;
                }
            }
            catch (Exception)
            {

                button_StartService.Enabled = false;
                buttonStopService.Enabled = false;
            }
        }

        private void UpdateTotalVMsRunning()
        {
            string VMRun = m_INI.IniReadValue("VM-BACKUP", "VMRun");

            if (File.Exists(VMRun))
            {
                double JobRunInterval = double.Parse(m_INI.IniReadValue("VM-BACKUP", "JobRunInterval"));
                DateTime JobRunTime = DateTime.Parse(m_INI.IniReadValue("VM-BACKUP", "JobRunTime"));
                this.label_NextJobExeTime.Text = JobRunTime.AddDays(JobRunInterval).ToString("yyyy-MMM-dd HH-mm");

                string WorkingDir = m_INI.IniReadValue("VM-BACKUP", "WorkingDir");

                Process process = new Process();
                process.StartInfo.WorkingDirectory = WorkingDir;
                process.StartInfo.FileName = string.Format("\"{0}\"", VMRun);
                process.StartInfo.Arguments = "list";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                //* Start process and handlers
                process.Start();

                List<string> vmList = new List<string>();

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    vmList.Add(line);
                }

                process.WaitForExit();

                this.richTextBox_RunningVMs.ResetText();
                foreach (string ln in vmList)
                {
                    this.richTextBox_RunningVMs.AppendText(ln + "\n");
                }
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            m_logger.EnableLogBuffer = true;

            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Job ID";
            columnHeader1.TextAlign = HorizontalAlignment.Center;
            columnHeader1.Width = 150;

            ColumnHeader columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "Job Description";
            columnHeader2.TextAlign = HorizontalAlignment.Left;
            columnHeader2.Width = 400;

            ColumnHeader columnHeader3 = new ColumnHeader();
            columnHeader3.Text = "Job Status";
            columnHeader3.TextAlign = HorizontalAlignment.Center;
            columnHeader3.Width = 150;

            this.listView_SavedJobs.Columns.Add(columnHeader1);
            this.listView_SavedJobs.Columns.Add(columnHeader2);
            this.listView_SavedJobs.Columns.Add(columnHeader3);


            loadAllJobs();

            if (listView_SavedJobs.Items.Count > 0) this.listView_SavedJobs.Items[0].Selected = true;

            try
            {
                if (DoesServiceExist("SynchService", "."))
                {
                    serviceController.MachineName = ".";
                    serviceController.ServiceName = "SynchService";

                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        button_StartService.Enabled = false;
                        buttonStopService.Enabled = true;
                    }
                    else if (serviceController.Status == ServiceControllerStatus.Stopped)
                    {
                        button_StartService.Enabled = true;
                        buttonStopService.Enabled = false;
                    }
                    timer.Enabled = true;
                }
                else
                {
                    button_StartService.Enabled = false;
                    buttonStopService.Enabled = false;
                    timer.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button_StartService.Enabled = false;
                buttonStopService.Enabled = false;
            }




            #region ------------------- Backup -------------------
            LoadBackSettings();
            #endregion
        }

        #region ------------------- Backup -------------------
        private void LoadBackSettings()
        {
            try
            {
                this.textBox_VmRun.Text = m_INI.IniReadValue("VM-BACKUP", "VMRun");
                this.textBox_WorkingDir.Text = m_INI.IniReadValue("VM-BACKUP", "WorkingDir");
                this.textBox_BackupDir.Text = m_INI.IniReadValue("VM-BACKUP", "BackupDest");

                this.checkBox_ShutdownVMs.Checked = bool.Parse(m_INI.IniReadValue("VM-BACKUP", "ShutdownVms"));
                this.checkBox_BackuVMs.Checked = bool.Parse(m_INI.IniReadValue("VM-BACKUP", "BackupVms"));
                this.checkBox_DeleteSnapshot.Checked = bool.Parse(m_INI.IniReadValue("VM-BACKUP", "DeleteSnapshot"));
                this.checkBox_CreateSnapshot.Checked = bool.Parse(m_INI.IniReadValue("VM-BACKUP", "CreateSnapshot"));
                this.checkBox_UseFullList.Checked = bool.Parse(m_INI.IniReadValue("VM-BACKUP", "UseFullVMsList"));
                this.numericUpDown_BackupInterval.Value = decimal.Parse(m_INI.IniReadValue("VM-BACKUP", "JobRunInterval"));
                this.dateTimePicker_BackupTime.Value = DateTime.Parse(m_INI.IniReadValue("VM-BACKUP", "JobRunTime"));
                this.checkBox_UseVmwareAuto.Checked = bool.Parse(m_INI.IniReadValue("VM-BACKUP", "UseVMWareAuto"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        bool DoesServiceExist(string serviceName, string machineName)
        {
            bool bR = false;
            ServiceController[] services = ServiceController.GetServices(machineName);
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == serviceName)
                {
                    bR = true;
                    break;
                }
            }
            return bR;
        }
        private void loadAllJobs()
        {
            listView_SavedJobs.Items.Clear();

            string[] iniSECTION;

            lock (threadLock)
            {
                //INI File
                //Job_Name=Job Description|Source|Destination 1...n|FileFilterIn 1...n|FileFilterEx 1...n|DirFilterEx 1...n|SynchType|JobStatus|LastRun|RunAfter

                //read all jobs from ini file
                iniSECTION = m_INI.ReadSection();
            }


            List<string> Jobs = new List<string>();

            if (iniSECTION != null)
            {
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

                        try
                        {
                            clsArguments arg = new clsArguments();
                            arg.LoadData(Job, JobData);

                            addJob(Job, JobData[0], (JobData[7] == "1") ? "Enabled" : "Disabled", arg);
                        }
                        catch (Exception Ex)
                        {
                            //error in this job parameters, move to next job
                            MessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            continue;
                        }

                    }
                }

            }
        }
        private bool addJob(string JobID, string JobDescription, string Enabled, clsArguments arg)
        {
            bool bR = false;

            if (!checkDuplicate(JobID, listView_SavedJobs))
            {
                ListViewItem item = new ListViewItem(JobID, 0);
                ListViewItem.ListViewSubItem sub1 = item.SubItems.Add(JobDescription);
                ListViewItem.ListViewSubItem sub2 = item.SubItems.Add(Enabled);


                ListViewItem thisItem = listView_SavedJobs.Items.Add(item);
                thisItem.Tag = arg;
                bR = true;
            }
            return bR;
        }





        private bool checkDuplicate(String FindItem, ListView listBox)
        {

            if (listBox.Items.Count > 0)
            {
                for (int iC = 0; iC < listBox.Items.Count; iC++)
                {
                    string sVal = listBox.Items[iC].SubItems[0].Text;
                    if (sVal.Contains(FindItem) && sVal.Equals(FindItem, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

            }
            return false;
        }


        private void button_AddNewJob_Click(object sender, EventArgs e)
        {
            FormJobDetails fd = new FormJobDetails();
            fd.ShowDialog();
            fd.m_JobID = string.Empty;
            loadAllJobs();
        }

        private void button_Disable_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView_SavedJobs.SelectedIndices;
            foreach (int index in indexes)
            {
                if (index != -1)
                {
                    string JobID = listView_SavedJobs.Items[index].SubItems[0].Text;

                    listView_SavedJobs.Items[index].SubItems[2].Text = "Disabled";
                    clsArguments arg = (clsArguments)listView_SavedJobs.Items[index].Tag;
                    arg.JobStatus = "0";

                    try
                    {
                        m_INI.IniWriteValue(JobID, arg.getValueStringForINI());
                        button_Enable.Enabled = true;
                        button_Disable.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button_Enable_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView_SavedJobs.SelectedIndices;
            foreach (int index in indexes)
            {
                if (index != -1)
                {
                    string JobID = listView_SavedJobs.Items[index].SubItems[0].Text;

                    listView_SavedJobs.Items[index].SubItems[2].Text = "Enabled";
                    clsArguments arg = (clsArguments)listView_SavedJobs.Items[index].Tag;
                    arg.JobStatus = "1";

                    try
                    {
                        m_INI.IniWriteValue(JobID, arg.getValueStringForINI());
                        button_Enable.Enabled = false;
                        button_Disable.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void listView_SavedJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView_SavedJobs.SelectedIndices;
            foreach (int index in indexes)
            {
                if (index != -1)
                {
                    clsArguments arg = (clsArguments)listView_SavedJobs.Items[index].Tag;
                    if (arg.JobStatus == "1")
                    {
                        button_Disable.Enabled = true;
                        button_Enable.Enabled = false;
                    }
                    else
                    {
                        button_Disable.Enabled = false;
                        button_Enable.Enabled = true;
                    }
                }
            }
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView_SavedJobs.SelectedIndices;
            foreach (int index in indexes)
            {
                if (index != -1)
                {
                    string JobID = listView_SavedJobs.Items[index].SubItems[0].Text;
                    try
                    {
                        m_INI.IniWriteValue(JobID, null);
                        loadAllJobs();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button_Edit_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView_SavedJobs.SelectedIndices;
            foreach (int index in indexes)
            {
                if (index != -1)
                {
                    string JobID = listView_SavedJobs.Items[index].SubItems[0].Text;
                    FormJobDetails fd = new FormJobDetails();
                    fd.m_JobID = JobID;
                    fd.m_arg = (clsArguments)listView_SavedJobs.Items[index].Tag;
                    fd.ShowDialog();
                    loadAllJobs();
                }
            }
        }

        private void button_StartService_Click(object sender, EventArgs e)
        {
            try
            {
                serviceController.Refresh();
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromTicks(DateTime.Now.AddMinutes(1).Ticks));
                button_StartService.Enabled = false;
                buttonStopService.Enabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonStopService_Click(object sender, EventArgs e)
        {
            try
            {
                serviceController.Refresh();
                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromTicks(DateTime.Now.AddMinutes(1).Ticks));
                button_StartService.Enabled = true;
                buttonStopService.Enabled = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_ManualRun_Click(object sender, EventArgs e)
        {
            this.richTextBox_Status.Text = string.Empty;

            ListView.SelectedIndexCollection indexes = listView_SavedJobs.SelectedIndices;
            foreach (int index in indexes)
            {
                if (index != -1)
                {
                    try
                    {
                        string JobID = listView_SavedJobs.Items[index].SubItems[0].Text;
                        clsArguments arg = (clsArguments)listView_SavedJobs.Items[index].Tag;

                        this.tabControl_Log.SelectedTab = tabPage_Log;
                        this.richTextBox_Status.Focus();
                        this.richTextBox_Status.ScrollToCaret();
                        clsCopy.RunJob(arg, this);
                        Application.DoEvents();
                        string LastMessage = string.Empty;
                        string NewMessage = string.Empty;

                        richTextBox_Status.AppendText(m_logger.getLogText());
                        this.richTextBox_Status.AppendText("Job completed.\r\n");
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Select Job to Run?", "Manual Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #region ------------------------- backup -------------------------

        private void button_VmRun_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(this.textBox_VmRun.Text);
                openFileDialog.Filter = "VM Run|vmrun.exe";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    if (!filePath.ToLower().EndsWith("vmrun.exe") || IsNetworkPath(filePath))
                    {
                        MessageBox.Show("Invalid vmrun location. Please select only vmrun.exe?", "VM Run Utility Location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        this.textBox_VmRun.Text = filePath;
                    }
                }
            }
        }

        private bool IsNetworkPath(string path)
        {
            if (!path.StartsWith(@"/") && !path.StartsWith(@"\"))
            {
                try
                {
                    string rootPath = System.IO.Path.GetPathRoot(path); // get drive's letter
                    System.IO.DriveInfo driveInfo = new System.IO.DriveInfo(rootPath); // get info about the drive
                    return driveInfo.DriveType == DriveType.Network; // return true if a network drive
                }
                catch (Exception) {}
            }

            return true; // is a UNC path
        }

        private void button_WorkingDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Please select working folder location";
            DialogResult dR = folderBrowserDialog.ShowDialog();

            if (dR == DialogResult.OK)
            {
                string workingFolder = folderBrowserDialog.SelectedPath;

                if (IsNetworkPath(workingFolder))
                {
                    MessageBox.Show("Invalid location. Please select local working location?", "Working folder location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.textBox_WorkingDir.Text = workingFolder;
                    m_INI.IniWriteValue("VM-BACKUP", "VMList", Path.Combine(workingFolder, "vmlist.txt")); 
                }
            }
        }

        private void button_BackupDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Please select a backup destination";
            DialogResult dR = folderBrowserDialog.ShowDialog();

            if (dR == DialogResult.OK)
            {
                string backupDir = folderBrowserDialog.SelectedPath;
                this.textBox_BackupDir.Text = backupDir;
            }
        }

        private void checkBox_BackuVMs_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_DeleteSnapshot_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_CreateSnapshot_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_UseFullList_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_BackupInterval_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_BackupTime_ValueChanged(object sender, EventArgs e)
        {
            
        }
        private void checkBox_UseVMWareAuto_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_RunBackup_Click(object sender, EventArgs e)
        {
            this.button_RunBackup.Enabled = false;
            
            this.tabControl_Log.SelectedTab = tabPage_Log;

            clsCopy.RunBackupJob(this);
            Application.DoEvents();
            string LastMessage = string.Empty;
            string NewMessage = string.Empty;

            richTextBox_Status.AppendText(m_logger.getLogText());
            this.richTextBox_Status.AppendText("Job completed.\r\n");

            this.button_RunBackup.Enabled = true;
        }

        private void button_SaveBackup_Click(object sender, EventArgs e)
        {
            m_INI.IniWriteValue("VM-BACKUP", "VMRun", this.textBox_VmRun.Text);
            m_INI.IniWriteValue("VM-BACKUP", "WorkingDir", this.textBox_WorkingDir.Text);
            m_INI.IniWriteValue("VM-BACKUP", "BackupDest", this.textBox_BackupDir.Text);

            m_INI.IniWriteValue("VM-BACKUP", "ShutdownVms", this.checkBox_ShutdownVMs.Checked ? "True":"False");
            m_INI.IniWriteValue("VM-BACKUP", "BackupVms", this.checkBox_BackuVMs.Checked ? "True" : "False");
            m_INI.IniWriteValue("VM-BACKUP", "DeleteSnapshot", this.checkBox_DeleteSnapshot.Checked ? "True" : "False");
            m_INI.IniWriteValue("VM-BACKUP", "CreateSnapshot", this.checkBox_CreateSnapshot.Checked ? "True" : "False");
            m_INI.IniWriteValue("VM-BACKUP", "UseFullVMsList", this.checkBox_UseFullList.Checked ? "True" : "False");
            m_INI.IniWriteValue("VM-BACKUP", "JobRunInterval", this.numericUpDown_BackupInterval.Value.ToString());
            m_INI.IniWriteValue("VM-BACKUP", "JobRunTime", this.dateTimePicker_BackupTime.Value.ToString("yyyy-MMM-dd HH:mm:ss"));
            m_INI.IniWriteValue("VM-BACKUP", "UseVMWareAuto", this.checkBox_UseVmwareAuto.Checked ? "True" : "False");

            MessageBox.Show("Settings are saved", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void button_CancelBackup_Click(object sender, EventArgs e)
        { 
            try
            {
                LoadBackSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer_Vms_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateTotalVMsRunning();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Refresh_VMsList_Click(object sender, EventArgs e)
        {
            try
            {
                listBox_VMsList.Items.Clear();
                string userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string VMListPath = userProfileFolder + @"\VMware\inventory.vmls";

                if (File.Exists(VMListPath))
                {
                    using (StreamReader sr = File.OpenText(VMListPath))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(line) && line.Contains(".config = ") && line.Contains(".vmx")) // only add paths to vmx files
                            {
                                string vmPath = line.Substring(line.IndexOf("=") + 2);
                                vmPath = vmPath.Replace("\"", "");
                                FileInfo file = new FileInfo(vmPath);
                                if (file.Exists)
                                {
                                    listBox_VMsList.Items.Add(file.FullName);
                                }
                            }
                        }
                    }

                    if (listBox_VMsList.Items.Count > 0)
                    {
                        MessageBox.Show(string.Format("List refreshed. Total VMs {0}", listBox_VMsList.Items.Count), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("No VMs List at {0}", VMListPath), "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_VMsList_SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox_VMsList.Items.Count; i++)
            {
                listBox_VMsList.SelectedIndices.Add(i);
            }
        }

        private void button_VMsList_UnselectAll_Click(object sender, EventArgs e)
        {
            listBox_VMsList.SelectedIndices.Clear();
        }

        private void button_VMsList_Backup_Click(object sender, EventArgs e)
        {
            if (listBox_VMsList.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select VM(s) to backup.", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string WorkingDir = m_INI.IniReadValue("VM-BACKUP", "WorkingDir");
            string VmFullList = Path.Combine(WorkingDir, "vmfulllist.txt");

            try
            {
                if (File.Exists(VmFullList))
                { 
                    File.Delete(VmFullList);
                }

                StreamWriter streamWriter = File.CreateText(VmFullList);
                foreach (string selectedVM in listBox_VMsList.SelectedItems)
                {
                    streamWriter.WriteLine(selectedVM);
                }
                streamWriter.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            this.button_RunBackup.Enabled = false;
            this.button_VMsList_Backup.Enabled = false;

            //temporary save flage in INI for thread
            if (!this.checkBox_UseFullList.Checked)
            {
                m_INI.IniWriteValue("VM-BACKUP", "UseFullVMsList", "True");
            }

            this.tabControl_Log.SelectedTab = tabPage_Log;

            clsCopy.RunBackupJob(this);
            Application.DoEvents();
            string LastMessage = string.Empty;
            string NewMessage = string.Empty;

            richTextBox_Status.AppendText(m_logger.getLogText());
            this.richTextBox_Status.AppendText("Selected VM(s) Backup Job completed.\r\n");

            //temporary save flage in INI for thread
            if (!this.checkBox_UseFullList.Checked)
            {
                m_INI.IniWriteValue("VM-BACKUP", "UseFullVMsList", "False");
            }

            this.button_RunBackup.Enabled = true;
            this.button_VMsList_Backup.Enabled = true;
        }

        private void button_SaveList_Click(object sender, EventArgs e)
        {
            if (listBox_VMsList.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select VM(s) to backup.", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string WorkingDir = m_INI.IniReadValue("VM-BACKUP", "WorkingDir");
            string VmFullList = Path.Combine(WorkingDir, "vmfulllist.txt");

            try
            {
                if (File.Exists(VmFullList))
                {
                    File.Delete(VmFullList);
                }

                StreamWriter streamWriter = File.CreateText(VmFullList);
                foreach (string selectedVM in listBox_VMsList.SelectedItems)
                {
                    streamWriter.WriteLine(selectedVM);
                }
                streamWriter.Close();

                MessageBox.Show(string.Format("Selected VM(s) list saved at [{0}]", VmFullList), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}