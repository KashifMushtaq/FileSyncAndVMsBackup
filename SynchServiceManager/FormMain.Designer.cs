namespace SynchServiceNS
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControl_Log = new System.Windows.Forms.TabControl();
            this.tabPage_Log = new System.Windows.Forms.TabPage();
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.label_ThreadStatus = new System.Windows.Forms.Label();
            this.buttonStopService = new System.Windows.Forms.Button();
            this.button_StartService = new System.Windows.Forms.Button();
            this.label_Refresh = new System.Windows.Forms.Label();
            this.button_Clear = new System.Windows.Forms.Button();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.tabPage_Jobs = new System.Windows.Forms.TabPage();
            this.groupBox_SavedJobs = new System.Windows.Forms.GroupBox();
            this.button_ManualRun = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.button_AddNew = new System.Windows.Forms.Button();
            this.button_Enable = new System.Windows.Forms.Button();
            this.button_Disable = new System.Windows.Forms.Button();
            this.button_Remove = new System.Windows.Forms.Button();
            this.listView_SavedJobs = new System.Windows.Forms.ListView();
            this.tabPage_BackupVMs = new System.Windows.Forms.TabPage();
            this.label_UseAutoService = new System.Windows.Forms.Label();
            this.checkBox_UseVmwareAuto = new System.Windows.Forms.CheckBox();
            this.label_RunningVMs = new System.Windows.Forms.Label();
            this.richTextBox_RunningVMs = new System.Windows.Forms.RichTextBox();
            this.label_BackupTimeHHmm = new System.Windows.Forms.Label();
            this.button_RunBackup = new System.Windows.Forms.Button();
            this.button_CancelBackup = new System.Windows.Forms.Button();
            this.button_SaveBackup = new System.Windows.Forms.Button();
            this.label_BackupTime = new System.Windows.Forms.Label();
            this.dateTimePicker_BackupTime = new System.Windows.Forms.DateTimePicker();
            this.label_BackuIntervalDays = new System.Windows.Forms.Label();
            this.numericUpDown_BackupInterval = new System.Windows.Forms.NumericUpDown();
            this.label_BackupInterval = new System.Windows.Forms.Label();
            this.label_UseFullList = new System.Windows.Forms.Label();
            this.checkBox_UseFullList = new System.Windows.Forms.CheckBox();
            this.label_CreateSnapshot = new System.Windows.Forms.Label();
            this.checkBox_CreateSnapshot = new System.Windows.Forms.CheckBox();
            this.label_DeleteSnapshot = new System.Windows.Forms.Label();
            this.checkBox_DeleteSnapshot = new System.Windows.Forms.CheckBox();
            this.label_BackupDir = new System.Windows.Forms.Label();
            this.button_BackupDir = new System.Windows.Forms.Button();
            this.textBox_BackupDir = new System.Windows.Forms.TextBox();
            this.label_BackupVMs = new System.Windows.Forms.Label();
            this.checkBox_BackuVMs = new System.Windows.Forms.CheckBox();
            this.label_ShutdownVMs = new System.Windows.Forms.Label();
            this.checkBox_ShutdownVMs = new System.Windows.Forms.CheckBox();
            this.label__LogDir = new System.Windows.Forms.Label();
            this.button_WorkingDir = new System.Windows.Forms.Button();
            this.textBox_WorkingDir = new System.Windows.Forms.TextBox();
            this.button_VmRun = new System.Windows.Forms.Button();
            this.textBox_VmRun = new System.Windows.Forms.TextBox();
            this.label_VmRun = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Timer_Vms = new System.Windows.Forms.Timer(this.components);
            this.label_JobExeTime = new System.Windows.Forms.Label();
            this.label_NextJobExeTime = new System.Windows.Forms.Label();
            this.tabControl_Log.SuspendLayout();
            this.tabPage_Log.SuspendLayout();
            this.tabPage_Jobs.SuspendLayout();
            this.groupBox_SavedJobs.SuspendLayout();
            this.tabPage_BackupVMs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_BackupInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_Log
            // 
            this.tabControl_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_Log.Controls.Add(this.tabPage_Log);
            this.tabControl_Log.Controls.Add(this.tabPage_Jobs);
            this.tabControl_Log.Controls.Add(this.tabPage_BackupVMs);
            this.tabControl_Log.Location = new System.Drawing.Point(6, 7);
            this.tabControl_Log.Name = "tabControl_Log";
            this.tabControl_Log.SelectedIndex = 0;
            this.tabControl_Log.Size = new System.Drawing.Size(1147, 676);
            this.tabControl_Log.TabIndex = 0;
            // 
            // tabPage_Log
            // 
            this.tabPage_Log.Controls.Add(this.richTextBox_Status);
            this.tabPage_Log.Controls.Add(this.label_ThreadStatus);
            this.tabPage_Log.Controls.Add(this.buttonStopService);
            this.tabPage_Log.Controls.Add(this.button_StartService);
            this.tabPage_Log.Controls.Add(this.label_Refresh);
            this.tabPage_Log.Controls.Add(this.button_Clear);
            this.tabPage_Log.Controls.Add(this.richTextBox_Log);
            this.tabPage_Log.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Log.Name = "tabPage_Log";
            this.tabPage_Log.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Log.Size = new System.Drawing.Size(1139, 650);
            this.tabPage_Log.TabIndex = 0;
            this.tabPage_Log.Text = "Log";
            this.tabPage_Log.UseVisualStyleBackColor = true;
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Status.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBox_Status.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBox_Status.Location = new System.Drawing.Point(17, 19);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.ReadOnly = true;
            this.richTextBox_Status.Size = new System.Drawing.Size(1106, 288);
            this.richTextBox_Status.TabIndex = 7;
            this.richTextBox_Status.TabStop = false;
            this.richTextBox_Status.Text = "";
            this.toolTip.SetToolTip(this.richTextBox_Status, "Log Created by Synch Service. Refreshes after every 5 seconds");
            this.richTextBox_Status.WordWrap = false;
            // 
            // label_ThreadStatus
            // 
            this.label_ThreadStatus.AutoSize = true;
            this.label_ThreadStatus.Location = new System.Drawing.Point(14, 3);
            this.label_ThreadStatus.Name = "label_ThreadStatus";
            this.label_ThreadStatus.Size = new System.Drawing.Size(122, 13);
            this.label_ThreadStatus.TabIndex = 6;
            this.label_ThreadStatus.Text = "Manual - Thread Activity";
            // 
            // buttonStopService
            // 
            this.buttonStopService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStopService.Location = new System.Drawing.Point(128, 621);
            this.buttonStopService.Name = "buttonStopService";
            this.buttonStopService.Size = new System.Drawing.Size(105, 23);
            this.buttonStopService.TabIndex = 5;
            this.buttonStopService.Text = "Stop Service";
            this.buttonStopService.UseVisualStyleBackColor = true;
            this.buttonStopService.Click += new System.EventHandler(this.buttonStopService_Click);
            // 
            // button_StartService
            // 
            this.button_StartService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_StartService.Location = new System.Drawing.Point(17, 621);
            this.button_StartService.Name = "button_StartService";
            this.button_StartService.Size = new System.Drawing.Size(105, 23);
            this.button_StartService.TabIndex = 4;
            this.button_StartService.Text = "Start Service";
            this.button_StartService.UseVisualStyleBackColor = true;
            this.button_StartService.Click += new System.EventHandler(this.button_StartService_Click);
            // 
            // label_Refresh
            // 
            this.label_Refresh.AutoSize = true;
            this.label_Refresh.Location = new System.Drawing.Point(14, 310);
            this.label_Refresh.Name = "label_Refresh";
            this.label_Refresh.Size = new System.Drawing.Size(166, 13);
            this.label_Refresh.TabIndex = 3;
            this.label_Refresh.Text = "Log - Refreshes Every 5 Seconds";
            // 
            // button_Clear
            // 
            this.button_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Clear.Location = new System.Drawing.Point(1058, 621);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(75, 23);
            this.button_Clear.TabIndex = 2;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Log.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBox_Log.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBox_Log.Location = new System.Drawing.Point(17, 326);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.ReadOnly = true;
            this.richTextBox_Log.Size = new System.Drawing.Size(1106, 289);
            this.richTextBox_Log.TabIndex = 1;
            this.richTextBox_Log.TabStop = false;
            this.richTextBox_Log.Text = "";
            this.toolTip.SetToolTip(this.richTextBox_Log, "Log Created by Synch Service. Refreshes after every 5 seconds");
            this.richTextBox_Log.WordWrap = false;
            // 
            // tabPage_Jobs
            // 
            this.tabPage_Jobs.Controls.Add(this.groupBox_SavedJobs);
            this.tabPage_Jobs.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Jobs.Name = "tabPage_Jobs";
            this.tabPage_Jobs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Jobs.Size = new System.Drawing.Size(1139, 650);
            this.tabPage_Jobs.TabIndex = 1;
            this.tabPage_Jobs.Text = "Synch Jobs";
            this.tabPage_Jobs.UseVisualStyleBackColor = true;
            // 
            // groupBox_SavedJobs
            // 
            this.groupBox_SavedJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_SavedJobs.Controls.Add(this.button_ManualRun);
            this.groupBox_SavedJobs.Controls.Add(this.button_Edit);
            this.groupBox_SavedJobs.Controls.Add(this.button_AddNew);
            this.groupBox_SavedJobs.Controls.Add(this.button_Enable);
            this.groupBox_SavedJobs.Controls.Add(this.button_Disable);
            this.groupBox_SavedJobs.Controls.Add(this.button_Remove);
            this.groupBox_SavedJobs.Controls.Add(this.listView_SavedJobs);
            this.groupBox_SavedJobs.Location = new System.Drawing.Point(15, 17);
            this.groupBox_SavedJobs.Name = "groupBox_SavedJobs";
            this.groupBox_SavedJobs.Size = new System.Drawing.Size(1105, 627);
            this.groupBox_SavedJobs.TabIndex = 7;
            this.groupBox_SavedJobs.TabStop = false;
            this.groupBox_SavedJobs.Text = "Saved Jobs";
            // 
            // button_ManualRun
            // 
            this.button_ManualRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ManualRun.Location = new System.Drawing.Point(1011, 598);
            this.button_ManualRun.Name = "button_ManualRun";
            this.button_ManualRun.Size = new System.Drawing.Size(75, 23);
            this.button_ManualRun.TabIndex = 12;
            this.button_ManualRun.Text = "Manual Run";
            this.button_ManualRun.UseVisualStyleBackColor = true;
            this.button_ManualRun.Click += new System.EventHandler(this.button_ManualRun_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Edit.Location = new System.Drawing.Point(662, 598);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(75, 23);
            this.button_Edit.TabIndex = 11;
            this.button_Edit.Text = "Edit";
            this.toolTip.SetToolTip(this.button_Edit, "Edit Selected Job");
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // button_AddNew
            // 
            this.button_AddNew.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_AddNew.Location = new System.Drawing.Point(500, 598);
            this.button_AddNew.Name = "button_AddNew";
            this.button_AddNew.Size = new System.Drawing.Size(75, 23);
            this.button_AddNew.TabIndex = 10;
            this.button_AddNew.Text = "Add";
            this.toolTip.SetToolTip(this.button_AddNew, "Add New Job");
            this.button_AddNew.UseVisualStyleBackColor = true;
            this.button_AddNew.Click += new System.EventHandler(this.button_AddNewJob_Click);
            // 
            // button_Enable
            // 
            this.button_Enable.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Enable.Location = new System.Drawing.Point(338, 598);
            this.button_Enable.Name = "button_Enable";
            this.button_Enable.Size = new System.Drawing.Size(75, 23);
            this.button_Enable.TabIndex = 9;
            this.button_Enable.Text = "Enable";
            this.toolTip.SetToolTip(this.button_Enable, "Enable Selected Job");
            this.button_Enable.UseVisualStyleBackColor = true;
            this.button_Enable.Click += new System.EventHandler(this.button_Enable_Click);
            // 
            // button_Disable
            // 
            this.button_Disable.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Disable.Location = new System.Drawing.Point(419, 598);
            this.button_Disable.Name = "button_Disable";
            this.button_Disable.Size = new System.Drawing.Size(75, 23);
            this.button_Disable.TabIndex = 8;
            this.button_Disable.Text = "Disable";
            this.toolTip.SetToolTip(this.button_Disable, "Disable Selected Job");
            this.button_Disable.UseVisualStyleBackColor = true;
            this.button_Disable.Click += new System.EventHandler(this.button_Disable_Click);
            // 
            // button_Remove
            // 
            this.button_Remove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Remove.Location = new System.Drawing.Point(581, 598);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(75, 23);
            this.button_Remove.TabIndex = 7;
            this.button_Remove.Text = "Remove";
            this.toolTip.SetToolTip(this.button_Remove, "Remove Selected Job");
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // listView_SavedJobs
            // 
            this.listView_SavedJobs.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView_SavedJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_SavedJobs.FullRowSelect = true;
            this.listView_SavedJobs.GridLines = true;
            this.listView_SavedJobs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_SavedJobs.HideSelection = false;
            this.listView_SavedJobs.Location = new System.Drawing.Point(11, 19);
            this.listView_SavedJobs.MultiSelect = false;
            this.listView_SavedJobs.Name = "listView_SavedJobs";
            this.listView_SavedJobs.ShowGroups = false;
            this.listView_SavedJobs.Size = new System.Drawing.Size(1075, 559);
            this.listView_SavedJobs.TabIndex = 1;
            this.listView_SavedJobs.UseCompatibleStateImageBehavior = false;
            this.listView_SavedJobs.View = System.Windows.Forms.View.Details;
            this.listView_SavedJobs.SelectedIndexChanged += new System.EventHandler(this.listView_SavedJobs_SelectedIndexChanged);
            this.listView_SavedJobs.DoubleClick += new System.EventHandler(this.button_Edit_Click);
            // 
            // tabPage_BackupVMs
            // 
            this.tabPage_BackupVMs.Controls.Add(this.label_NextJobExeTime);
            this.tabPage_BackupVMs.Controls.Add(this.label_JobExeTime);
            this.tabPage_BackupVMs.Controls.Add(this.label_UseAutoService);
            this.tabPage_BackupVMs.Controls.Add(this.checkBox_UseVmwareAuto);
            this.tabPage_BackupVMs.Controls.Add(this.label_RunningVMs);
            this.tabPage_BackupVMs.Controls.Add(this.richTextBox_RunningVMs);
            this.tabPage_BackupVMs.Controls.Add(this.label_BackupTimeHHmm);
            this.tabPage_BackupVMs.Controls.Add(this.button_RunBackup);
            this.tabPage_BackupVMs.Controls.Add(this.button_CancelBackup);
            this.tabPage_BackupVMs.Controls.Add(this.button_SaveBackup);
            this.tabPage_BackupVMs.Controls.Add(this.label_BackupTime);
            this.tabPage_BackupVMs.Controls.Add(this.dateTimePicker_BackupTime);
            this.tabPage_BackupVMs.Controls.Add(this.label_BackuIntervalDays);
            this.tabPage_BackupVMs.Controls.Add(this.numericUpDown_BackupInterval);
            this.tabPage_BackupVMs.Controls.Add(this.label_BackupInterval);
            this.tabPage_BackupVMs.Controls.Add(this.label_UseFullList);
            this.tabPage_BackupVMs.Controls.Add(this.checkBox_UseFullList);
            this.tabPage_BackupVMs.Controls.Add(this.label_CreateSnapshot);
            this.tabPage_BackupVMs.Controls.Add(this.checkBox_CreateSnapshot);
            this.tabPage_BackupVMs.Controls.Add(this.label_DeleteSnapshot);
            this.tabPage_BackupVMs.Controls.Add(this.checkBox_DeleteSnapshot);
            this.tabPage_BackupVMs.Controls.Add(this.label_BackupDir);
            this.tabPage_BackupVMs.Controls.Add(this.button_BackupDir);
            this.tabPage_BackupVMs.Controls.Add(this.textBox_BackupDir);
            this.tabPage_BackupVMs.Controls.Add(this.label_BackupVMs);
            this.tabPage_BackupVMs.Controls.Add(this.checkBox_BackuVMs);
            this.tabPage_BackupVMs.Controls.Add(this.label_ShutdownVMs);
            this.tabPage_BackupVMs.Controls.Add(this.checkBox_ShutdownVMs);
            this.tabPage_BackupVMs.Controls.Add(this.label__LogDir);
            this.tabPage_BackupVMs.Controls.Add(this.button_WorkingDir);
            this.tabPage_BackupVMs.Controls.Add(this.textBox_WorkingDir);
            this.tabPage_BackupVMs.Controls.Add(this.button_VmRun);
            this.tabPage_BackupVMs.Controls.Add(this.textBox_VmRun);
            this.tabPage_BackupVMs.Controls.Add(this.label_VmRun);
            this.tabPage_BackupVMs.Location = new System.Drawing.Point(4, 22);
            this.tabPage_BackupVMs.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_BackupVMs.Name = "tabPage_BackupVMs";
            this.tabPage_BackupVMs.Size = new System.Drawing.Size(1139, 650);
            this.tabPage_BackupVMs.TabIndex = 2;
            this.tabPage_BackupVMs.Text = "Backup VMs";
            this.tabPage_BackupVMs.UseVisualStyleBackColor = true;
            // 
            // label_UseAutoService
            // 
            this.label_UseAutoService.AutoSize = true;
            this.label_UseAutoService.Location = new System.Drawing.Point(16, 241);
            this.label_UseAutoService.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_UseAutoService.Name = "label_UseAutoService";
            this.label_UseAutoService.Size = new System.Drawing.Size(118, 13);
            this.label_UseAutoService.TabIndex = 68;
            this.label_UseAutoService.Text = "Use Auto Start Service:";
            // 
            // checkBox_UseVmwareAuto
            // 
            this.checkBox_UseVmwareAuto.AutoSize = true;
            this.checkBox_UseVmwareAuto.Checked = true;
            this.checkBox_UseVmwareAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_UseVmwareAuto.Location = new System.Drawing.Point(146, 239);
            this.checkBox_UseVmwareAuto.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_UseVmwareAuto.Name = "checkBox_UseVmwareAuto";
            this.checkBox_UseVmwareAuto.Size = new System.Drawing.Size(285, 17);
            this.checkBox_UseVmwareAuto.TabIndex = 67;
            this.checkBox_UseVmwareAuto.Text = "(Use VMWare Auto Service To Start VM After Backup)";
            this.checkBox_UseVmwareAuto.UseVisualStyleBackColor = true;
            this.checkBox_UseVmwareAuto.CheckedChanged += new System.EventHandler(this.checkBox_UseVMWareAuto_CheckedChanged);
            // 
            // label_RunningVMs
            // 
            this.label_RunningVMs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_RunningVMs.AutoSize = true;
            this.label_RunningVMs.Location = new System.Drawing.Point(66, 335);
            this.label_RunningVMs.Name = "label_RunningVMs";
            this.label_RunningVMs.Size = new System.Drawing.Size(74, 13);
            this.label_RunningVMs.TabIndex = 66;
            this.label_RunningVMs.Text = "Running VMs:";
            // 
            // richTextBox_RunningVMs
            // 
            this.richTextBox_RunningVMs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_RunningVMs.Location = new System.Drawing.Point(146, 335);
            this.richTextBox_RunningVMs.Name = "richTextBox_RunningVMs";
            this.richTextBox_RunningVMs.ReadOnly = true;
            this.richTextBox_RunningVMs.Size = new System.Drawing.Size(957, 224);
            this.richTextBox_RunningVMs.TabIndex = 65;
            this.richTextBox_RunningVMs.Text = "";
            this.richTextBox_RunningVMs.WordWrap = false;
            // 
            // label_BackupTimeHHmm
            // 
            this.label_BackupTimeHHmm.AutoSize = true;
            this.label_BackupTimeHHmm.Location = new System.Drawing.Point(277, 291);
            this.label_BackupTimeHHmm.Name = "label_BackupTimeHHmm";
            this.label_BackupTimeHHmm.Size = new System.Drawing.Size(107, 13);
            this.label_BackupTimeHHmm.TabIndex = 64;
            this.label_BackupTimeHHmm.Text = "(yyyy-MM-dd HH:mm)";
            // 
            // button_RunBackup
            // 
            this.button_RunBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_RunBackup.Location = new System.Drawing.Point(795, 618);
            this.button_RunBackup.Name = "button_RunBackup";
            this.button_RunBackup.Size = new System.Drawing.Size(96, 23);
            this.button_RunBackup.TabIndex = 63;
            this.button_RunBackup.Text = "Backup Now";
            this.toolTip.SetToolTip(this.button_RunBackup, "Manually backup now");
            this.button_RunBackup.UseVisualStyleBackColor = true;
            this.button_RunBackup.Click += new System.EventHandler(this.button_RunBackup_Click);
            // 
            // button_CancelBackup
            // 
            this.button_CancelBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CancelBackup.Location = new System.Drawing.Point(999, 618);
            this.button_CancelBackup.Name = "button_CancelBackup";
            this.button_CancelBackup.Size = new System.Drawing.Size(96, 23);
            this.button_CancelBackup.TabIndex = 62;
            this.button_CancelBackup.Text = "Cancel";
            this.toolTip.SetToolTip(this.button_CancelBackup, "Cancel change");
            this.button_CancelBackup.UseVisualStyleBackColor = true;
            this.button_CancelBackup.Click += new System.EventHandler(this.button_CancelBackup_Click);
            // 
            // button_SaveBackup
            // 
            this.button_SaveBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SaveBackup.Location = new System.Drawing.Point(897, 618);
            this.button_SaveBackup.Name = "button_SaveBackup";
            this.button_SaveBackup.Size = new System.Drawing.Size(96, 23);
            this.button_SaveBackup.TabIndex = 61;
            this.button_SaveBackup.Text = "Save";
            this.toolTip.SetToolTip(this.button_SaveBackup, "Save the change");
            this.button_SaveBackup.UseVisualStyleBackColor = true;
            this.button_SaveBackup.Click += new System.EventHandler(this.button_SaveBackup_Click);
            // 
            // label_BackupTime
            // 
            this.label_BackupTime.AutoSize = true;
            this.label_BackupTime.Location = new System.Drawing.Point(31, 291);
            this.label_BackupTime.Name = "label_BackupTime";
            this.label_BackupTime.Size = new System.Drawing.Size(103, 13);
            this.label_BackupTime.TabIndex = 60;
            this.label_BackupTime.Text = "Job Execution Time:";
            // 
            // dateTimePicker_BackupTime
            // 
            this.dateTimePicker_BackupTime.CustomFormat = "yyyy-MMM-dd HH:mm";
            this.dateTimePicker_BackupTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_BackupTime.Location = new System.Drawing.Point(146, 287);
            this.dateTimePicker_BackupTime.Name = "dateTimePicker_BackupTime";
            this.dateTimePicker_BackupTime.ShowUpDown = true;
            this.dateTimePicker_BackupTime.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_BackupTime.TabIndex = 58;
            this.toolTip.SetToolTip(this.dateTimePicker_BackupTime, "Select day and time of the day to launch VMs backup job");
            this.dateTimePicker_BackupTime.Value = new System.DateTime(2025, 3, 4, 0, 0, 0, 0);
            this.dateTimePicker_BackupTime.ValueChanged += new System.EventHandler(this.dateTimePicker_BackupTime_ValueChanged);
            // 
            // label_BackuIntervalDays
            // 
            this.label_BackuIntervalDays.AutoSize = true;
            this.label_BackuIntervalDays.Location = new System.Drawing.Point(277, 265);
            this.label_BackuIntervalDays.Name = "label_BackuIntervalDays";
            this.label_BackuIntervalDays.Size = new System.Drawing.Size(37, 13);
            this.label_BackuIntervalDays.TabIndex = 57;
            this.label_BackuIntervalDays.Text = "(Days)";
            // 
            // numericUpDown_BackupInterval
            // 
            this.numericUpDown_BackupInterval.Location = new System.Drawing.Point(146, 261);
            this.numericUpDown_BackupInterval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown_BackupInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_BackupInterval.Name = "numericUpDown_BackupInterval";
            this.numericUpDown_BackupInterval.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_BackupInterval.TabIndex = 56;
            this.toolTip.SetToolTip(this.numericUpDown_BackupInterval, "Job Run Interval in days.");
            this.numericUpDown_BackupInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown_BackupInterval.ValueChanged += new System.EventHandler(this.numericUpDown_BackupInterval_ValueChanged);
            // 
            // label_BackupInterval
            // 
            this.label_BackupInterval.AutoSize = true;
            this.label_BackupInterval.Location = new System.Drawing.Point(52, 265);
            this.label_BackupInterval.Name = "label_BackupInterval";
            this.label_BackupInterval.Size = new System.Drawing.Size(82, 13);
            this.label_BackupInterval.TabIndex = 55;
            this.label_BackupInterval.Text = "Job run interval:";
            // 
            // label_UseFullList
            // 
            this.label_UseFullList.AutoSize = true;
            this.label_UseFullList.Location = new System.Drawing.Point(48, 220);
            this.label_UseFullList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_UseFullList.Name = "label_UseFullList";
            this.label_UseFullList.Size = new System.Drawing.Size(86, 13);
            this.label_UseFullList.TabIndex = 49;
            this.label_UseFullList.Text = "Use Full VM List:";
            // 
            // checkBox_UseFullList
            // 
            this.checkBox_UseFullList.AutoSize = true;
            this.checkBox_UseFullList.Location = new System.Drawing.Point(146, 218);
            this.checkBox_UseFullList.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_UseFullList.Name = "checkBox_UseFullList";
            this.checkBox_UseFullList.Size = new System.Drawing.Size(343, 17);
            this.checkBox_UseFullList.TabIndex = 48;
            this.checkBox_UseFullList.Text = "(Reads fullvmlist.txt file from the working directory for the list of VMs)";
            this.checkBox_UseFullList.UseVisualStyleBackColor = true;
            this.checkBox_UseFullList.CheckedChanged += new System.EventHandler(this.checkBox_UseFullList_CheckedChanged);
            // 
            // label_CreateSnapshot
            // 
            this.label_CreateSnapshot.AutoSize = true;
            this.label_CreateSnapshot.Location = new System.Drawing.Point(45, 199);
            this.label_CreateSnapshot.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CreateSnapshot.Name = "label_CreateSnapshot";
            this.label_CreateSnapshot.Size = new System.Drawing.Size(89, 13);
            this.label_CreateSnapshot.TabIndex = 47;
            this.label_CreateSnapshot.Text = "Create Snapshot:";
            // 
            // checkBox_CreateSnapshot
            // 
            this.checkBox_CreateSnapshot.AutoSize = true;
            this.checkBox_CreateSnapshot.Checked = true;
            this.checkBox_CreateSnapshot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_CreateSnapshot.Location = new System.Drawing.Point(146, 197);
            this.checkBox_CreateSnapshot.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_CreateSnapshot.Name = "checkBox_CreateSnapshot";
            this.checkBox_CreateSnapshot.Size = new System.Drawing.Size(142, 17);
            this.checkBox_CreateSnapshot.TabIndex = 46;
            this.checkBox_CreateSnapshot.Text = "(Name -> auto-snapshot)";
            this.checkBox_CreateSnapshot.UseVisualStyleBackColor = true;
            this.checkBox_CreateSnapshot.CheckedChanged += new System.EventHandler(this.checkBox_CreateSnapshot_CheckedChanged);
            // 
            // label_DeleteSnapshot
            // 
            this.label_DeleteSnapshot.AutoSize = true;
            this.label_DeleteSnapshot.Location = new System.Drawing.Point(45, 178);
            this.label_DeleteSnapshot.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_DeleteSnapshot.Name = "label_DeleteSnapshot";
            this.label_DeleteSnapshot.Size = new System.Drawing.Size(89, 13);
            this.label_DeleteSnapshot.TabIndex = 45;
            this.label_DeleteSnapshot.Text = "Delete Snapshot:";
            // 
            // checkBox_DeleteSnapshot
            // 
            this.checkBox_DeleteSnapshot.AutoSize = true;
            this.checkBox_DeleteSnapshot.Checked = true;
            this.checkBox_DeleteSnapshot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DeleteSnapshot.Location = new System.Drawing.Point(146, 176);
            this.checkBox_DeleteSnapshot.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_DeleteSnapshot.Name = "checkBox_DeleteSnapshot";
            this.checkBox_DeleteSnapshot.Size = new System.Drawing.Size(142, 17);
            this.checkBox_DeleteSnapshot.TabIndex = 44;
            this.checkBox_DeleteSnapshot.Text = "(Name -> auto-snapshot)";
            this.checkBox_DeleteSnapshot.UseVisualStyleBackColor = true;
            this.checkBox_DeleteSnapshot.CheckedChanged += new System.EventHandler(this.checkBox_DeleteSnapshot_CheckedChanged);
            // 
            // label_BackupDir
            // 
            this.label_BackupDir.AutoSize = true;
            this.label_BackupDir.Location = new System.Drawing.Point(47, 90);
            this.label_BackupDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BackupDir.Name = "label_BackupDir";
            this.label_BackupDir.Size = new System.Drawing.Size(90, 13);
            this.label_BackupDir.TabIndex = 43;
            this.label_BackupDir.Text = "Backup directory:";
            // 
            // button_BackupDir
            // 
            this.button_BackupDir.Location = new System.Drawing.Point(1020, 85);
            this.button_BackupDir.Name = "button_BackupDir";
            this.button_BackupDir.Size = new System.Drawing.Size(75, 23);
            this.button_BackupDir.TabIndex = 42;
            this.button_BackupDir.Text = "Browse";
            this.button_BackupDir.UseVisualStyleBackColor = true;
            this.button_BackupDir.Click += new System.EventHandler(this.button_BackupDir_Click);
            // 
            // textBox_BackupDir
            // 
            this.textBox_BackupDir.Location = new System.Drawing.Point(146, 87);
            this.textBox_BackupDir.MaxLength = 200;
            this.textBox_BackupDir.Name = "textBox_BackupDir";
            this.textBox_BackupDir.ReadOnly = true;
            this.textBox_BackupDir.Size = new System.Drawing.Size(857, 20);
            this.textBox_BackupDir.TabIndex = 41;
            this.toolTip.SetToolTip(this.textBox_BackupDir, "Source Directory");
            // 
            // label_BackupVMs
            // 
            this.label_BackupVMs.AutoSize = true;
            this.label_BackupVMs.Location = new System.Drawing.Point(63, 157);
            this.label_BackupVMs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BackupVMs.Name = "label_BackupVMs";
            this.label_BackupVMs.Size = new System.Drawing.Size(71, 13);
            this.label_BackupVMs.TabIndex = 40;
            this.label_BackupVMs.Text = "Backup VMs:";
            // 
            // checkBox_BackuVMs
            // 
            this.checkBox_BackuVMs.AutoSize = true;
            this.checkBox_BackuVMs.Checked = true;
            this.checkBox_BackuVMs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_BackuVMs.Location = new System.Drawing.Point(146, 155);
            this.checkBox_BackuVMs.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_BackuVMs.Name = "checkBox_BackuVMs";
            this.checkBox_BackuVMs.Size = new System.Drawing.Size(82, 17);
            this.checkBox_BackuVMs.TabIndex = 39;
            this.checkBox_BackuVMs.Text = "(RoboCopy)";
            this.checkBox_BackuVMs.UseVisualStyleBackColor = true;
            this.checkBox_BackuVMs.CheckedChanged += new System.EventHandler(this.checkBox_BackuVMs_CheckedChanged);
            // 
            // label_ShutdownVMs
            // 
            this.label_ShutdownVMs.AutoSize = true;
            this.label_ShutdownVMs.Location = new System.Drawing.Point(52, 136);
            this.label_ShutdownVMs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ShutdownVMs.Name = "label_ShutdownVMs";
            this.label_ShutdownVMs.Size = new System.Drawing.Size(82, 13);
            this.label_ShutdownVMs.TabIndex = 38;
            this.label_ShutdownVMs.Text = "Shutdown VMs:";
            // 
            // checkBox_ShutdownVMs
            // 
            this.checkBox_ShutdownVMs.AutoSize = true;
            this.checkBox_ShutdownVMs.Checked = true;
            this.checkBox_ShutdownVMs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ShutdownVMs.Location = new System.Drawing.Point(146, 134);
            this.checkBox_ShutdownVMs.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_ShutdownVMs.Name = "checkBox_ShutdownVMs";
            this.checkBox_ShutdownVMs.Size = new System.Drawing.Size(166, 17);
            this.checkBox_ShutdownVMs.TabIndex = 37;
            this.checkBox_ShutdownVMs.Text = "( Unchecked - Suspend VMs)";
            this.checkBox_ShutdownVMs.UseVisualStyleBackColor = true;
            // 
            // label__LogDir
            // 
            this.label__LogDir.AutoSize = true;
            this.label__LogDir.Location = new System.Drawing.Point(44, 66);
            this.label__LogDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label__LogDir.Name = "label__LogDir";
            this.label__LogDir.Size = new System.Drawing.Size(93, 13);
            this.label__LogDir.TabIndex = 36;
            this.label__LogDir.Text = "Working directory:";
            // 
            // button_WorkingDir
            // 
            this.button_WorkingDir.Location = new System.Drawing.Point(1020, 61);
            this.button_WorkingDir.Name = "button_WorkingDir";
            this.button_WorkingDir.Size = new System.Drawing.Size(75, 23);
            this.button_WorkingDir.TabIndex = 35;
            this.button_WorkingDir.Text = "Browse";
            this.button_WorkingDir.UseVisualStyleBackColor = true;
            this.button_WorkingDir.Click += new System.EventHandler(this.button_WorkingDir_Click);
            // 
            // textBox_WorkingDir
            // 
            this.textBox_WorkingDir.Location = new System.Drawing.Point(146, 63);
            this.textBox_WorkingDir.MaxLength = 200;
            this.textBox_WorkingDir.Name = "textBox_WorkingDir";
            this.textBox_WorkingDir.ReadOnly = true;
            this.textBox_WorkingDir.Size = new System.Drawing.Size(857, 20);
            this.textBox_WorkingDir.TabIndex = 34;
            this.toolTip.SetToolTip(this.textBox_WorkingDir, "Source Directory");
            // 
            // button_VmRun
            // 
            this.button_VmRun.Location = new System.Drawing.Point(1020, 31);
            this.button_VmRun.Name = "button_VmRun";
            this.button_VmRun.Size = new System.Drawing.Size(75, 23);
            this.button_VmRun.TabIndex = 33;
            this.button_VmRun.Text = "Browse";
            this.button_VmRun.UseVisualStyleBackColor = true;
            this.button_VmRun.Click += new System.EventHandler(this.button_VmRun_Click);
            // 
            // textBox_VmRun
            // 
            this.textBox_VmRun.Location = new System.Drawing.Point(146, 32);
            this.textBox_VmRun.MaxLength = 200;
            this.textBox_VmRun.Name = "textBox_VmRun";
            this.textBox_VmRun.ReadOnly = true;
            this.textBox_VmRun.Size = new System.Drawing.Size(857, 20);
            this.textBox_VmRun.TabIndex = 32;
            this.toolTip.SetToolTip(this.textBox_VmRun, "Source Directory");
            // 
            // label_VmRun
            // 
            this.label_VmRun.AutoSize = true;
            this.label_VmRun.Location = new System.Drawing.Point(16, 37);
            this.label_VmRun.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_VmRun.Name = "label_VmRun";
            this.label_VmRun.Size = new System.Drawing.Size(124, 13);
            this.label_VmRun.TabIndex = 0;
            this.label_VmRun.Text = "VMWare vmrun location:";
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Timer_Vms
            // 
            this.Timer_Vms.Enabled = true;
            this.Timer_Vms.Interval = 10000;
            this.Timer_Vms.Tick += new System.EventHandler(this.Timer_Vms_Tick);
            // 
            // label_JobExeTime
            // 
            this.label_JobExeTime.AutoSize = true;
            this.label_JobExeTime.Location = new System.Drawing.Point(455, 291);
            this.label_JobExeTime.Name = "label_JobExeTime";
            this.label_JobExeTime.Size = new System.Drawing.Size(108, 13);
            this.label_JobExeTime.TabIndex = 69;
            this.label_JobExeTime.Text = "Next Execution Time:";
            // 
            // label_NextJobExeTime
            // 
            this.label_NextJobExeTime.AutoSize = true;
            this.label_NextJobExeTime.Location = new System.Drawing.Point(569, 291);
            this.label_NextJobExeTime.Name = "label_NextJobExeTime";
            this.label_NextJobExeTime.Size = new System.Drawing.Size(0, 13);
            this.label_NextJobExeTime.TabIndex = 70;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 688);
            this.Controls.Add(this.tabControl_Log);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Synch Manager";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControl_Log.ResumeLayout(false);
            this.tabPage_Log.ResumeLayout(false);
            this.tabPage_Log.PerformLayout();
            this.tabPage_Jobs.ResumeLayout(false);
            this.groupBox_SavedJobs.ResumeLayout(false);
            this.tabPage_BackupVMs.ResumeLayout(false);
            this.tabPage_BackupVMs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_BackupInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_Log;
        private System.Windows.Forms.TabPage tabPage_Log;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.TabPage tabPage_Jobs;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label_Refresh;
        private System.Windows.Forms.GroupBox groupBox_SavedJobs;
        private System.Windows.Forms.ListView listView_SavedJobs;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button button_Enable;
        private System.Windows.Forms.Button button_Disable;
        private System.Windows.Forms.Button button_Remove;
        private System.Windows.Forms.Button button_AddNew;
        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.Button buttonStopService;
        private System.Windows.Forms.Button button_StartService;
        private System.Windows.Forms.Button button_ManualRun;
        private System.Windows.Forms.Label label_ThreadStatus;
        private System.Windows.Forms.RichTextBox richTextBox_Status;
        private System.Windows.Forms.TabPage tabPage_BackupVMs;
        private System.Windows.Forms.Label label_VmRun;
        private System.Windows.Forms.Button button_VmRun;
        private System.Windows.Forms.TextBox textBox_VmRun;
        private System.Windows.Forms.Label label__LogDir;
        private System.Windows.Forms.Button button_WorkingDir;
        private System.Windows.Forms.TextBox textBox_WorkingDir;
        private System.Windows.Forms.Label label_ShutdownVMs;
        private System.Windows.Forms.CheckBox checkBox_ShutdownVMs;
        private System.Windows.Forms.Label label_BackupDir;
        private System.Windows.Forms.Button button_BackupDir;
        private System.Windows.Forms.TextBox textBox_BackupDir;
        private System.Windows.Forms.Label label_BackupVMs;
        private System.Windows.Forms.CheckBox checkBox_BackuVMs;
        private System.Windows.Forms.Label label_DeleteSnapshot;
        private System.Windows.Forms.CheckBox checkBox_DeleteSnapshot;
        private System.Windows.Forms.Label label_CreateSnapshot;
        private System.Windows.Forms.CheckBox checkBox_CreateSnapshot;
        private System.Windows.Forms.Label label_UseFullList;
        private System.Windows.Forms.CheckBox checkBox_UseFullList;
        private System.Windows.Forms.Label label_BackupTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_BackupTime;
        private System.Windows.Forms.Label label_BackuIntervalDays;
        private System.Windows.Forms.NumericUpDown numericUpDown_BackupInterval;
        private System.Windows.Forms.Label label_BackupInterval;
        private System.Windows.Forms.Button button_CancelBackup;
        private System.Windows.Forms.Button button_SaveBackup;
        private System.Windows.Forms.Button button_RunBackup;
        private System.Windows.Forms.Label label_BackupTimeHHmm;
        private System.Windows.Forms.Label label_RunningVMs;
        private System.Windows.Forms.RichTextBox richTextBox_RunningVMs;
        private System.Windows.Forms.Label label_UseAutoService;
        private System.Windows.Forms.CheckBox checkBox_UseVmwareAuto;
        private System.Windows.Forms.Timer Timer_Vms;
        private System.Windows.Forms.Label label_NextJobExeTime;
        private System.Windows.Forms.Label label_JobExeTime;
    }
}

