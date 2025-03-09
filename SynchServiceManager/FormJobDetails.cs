﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SynchServiceNS
{
    public partial class FormJobDetails : Form
    {
        public string m_JobID = string.Empty;
        private const string SPLIT_CHAR_MAIN = "|";
        private const string SPLIT_CHAR = ",";
        private const string SPLIT_CHAR_EQUAL = "=";
        private bool m_bDataChanged = false;
        private bool m_bIsLoading = false;

        private object threadLock = new object();

        private INI m_INI = new INI();
        public clsArguments m_arg;

        public FormJobDetails()
        {
            InitializeComponent();
        }

        private void FormJobDetails_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_JobID))
            {
                try
                {
                    m_bIsLoading = true;

                    ShowJobData(m_JobID);
                    textBox_JobID.ReadOnly = true;
                    m_bIsLoading = false;
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_bIsLoading = false;
                }
            }
            else 
            {
                int newId = m_INI.GetNextJobId();

                m_arg = new clsArguments();
                m_arg.JobLastRun = "0";
                m_arg.JobStatus = "1";
                m_arg.SynchType = "1";
                m_arg.RunAfter = numericUpDown_Interval.Value.ToString();
                m_arg.LogLevel = "5";

                comboBox_LogLevel.SelectedIndex = 4;
                comboBox_SynchType.SelectedIndex = 0;

                textBox_JobID.Text = newId.ToString();
            }
        }

        private void button_Source_Click(object sender, EventArgs e)
        {
            //folderBrowserDialog.SelectedPath = this.textBox_Source.Text;
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Select Source Files Folder";
            DialogResult dR = folderBrowserDialog.ShowDialog();
            if (dR == DialogResult.OK)
            {
                string sPath = folderBrowserDialog.SelectedPath;
                if (sPath != this.textBox_Source.Text)
                {
                    this.textBox_Source.Text = sPath;
                    m_bDataChanged = true;
                    m_arg.JobSource = textBox_Source.Text;
                }
            }
        }

        private void numericUpDown_Interval_ValueChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
 
            m_arg.RunAfter = numericUpDown_Interval.Value.ToString();
            m_bDataChanged = true;
        }
        private void saveJob()
        {
            if (m_bDataChanged)
            {
                if (string.IsNullOrEmpty(m_JobID))
                {
                    string JobID = textBox_JobID.Text.Trim();
                    if (!checkDuplicateJob(JobID) && !string.IsNullOrEmpty(JobID))
                    {
                        m_JobID = JobID;
                        m_arg.JobName = JobID;
                        m_bDataChanged = true;
                    }
                    else
                    {
                        MessageBox.Show("Can not add duplicate job name. Job name must be unique.");
                        textBox_JobID.Text = string.Empty;
                        m_JobID = "";
                        m_arg.JobName = "";
                        return;
                    }
                }
                
                if (string.IsNullOrEmpty(m_arg.JobSource))
                {
                    MessageBox.Show("Sourse can not be empty.");
                    return;
                }

                if(string.IsNullOrEmpty(getItemsPiped(listBox_Destinations)))
                {
                    MessageBox.Show("There should be atleast on destination.");
                    return;
                }

                m_INI.IniWriteValue(m_JobID, m_arg.getValueStringForINI());
                m_bDataChanged = false;
                MessageBox.Show("Changes Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nothing Changed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ShowJobData(string Job)
        {
            string JobValue = m_INI.IniReadValue(Job);

            if (!string.IsNullOrEmpty(JobValue))
            {
                //INI File
                //Job_Name=Job Description|Source|Destination 1...n|FileFilterIn 1...n|FileFilterEx 1...n|DirFilterEx 1...n|SynchType|JobStatus|LastRun|RunAfter
                string[] JobData = JobValue.Split(SPLIT_CHAR_MAIN[0]);

                this.textBox_JobID.Text = Job;
                this.textBox_JobDescription.Text = JobData[0];
                this.textBox_Source.Text = JobData[1];

                listBox_Destinations.Items.Clear();
                listBox_Destinations.Items.AddRange(JobData[2].Split(SPLIT_CHAR[0]));

                this.numericUpDown_Interval.Value = decimal.Parse(JobData[9]);

                this.textBox_FileExFilter.Text = JobData[4];
                this.textBox_FileIncFilter.Text = JobData[3];
                this.textBox_SubDirExcFilter.Text = JobData[5];


                if (JobData[6] == "1")
                {
                    comboBox_SynchType.SelectedIndex = 0;
                }
                else if (JobData[6] == "2")
                {
                    comboBox_SynchType.SelectedIndex = 1;
                }
                else
                {
                    comboBox_SynchType.SelectedIndex = 2;
                }

                if (JobData[10] == "1")
                {
                    comboBox_LogLevel.SelectedIndex = 0;
                }
                else if (JobData[10] == "2")
                {
                    comboBox_LogLevel.SelectedIndex = 1;
                }
                else if (JobData[10] == "3")
                {
                    comboBox_LogLevel.SelectedIndex = 2;
                }

                else if (JobData[10] == "4")
                {
                    comboBox_LogLevel.SelectedIndex = 3;
                }
                else
                {
                    comboBox_LogLevel.SelectedIndex = 4;
                }

                if (JobData[11] == "1")
                {
                    checkBox_UseSynshFramework.Checked=true;
                }
                else
                {
                    checkBox_UseSynshFramework.Checked = false;
                }
                if (JobData[12] == "1")
                {
                    checkBox_RunAt.Checked = true;
                }
                else
                {
                    checkBox_RunAt.Checked = false;
                }
                dateTimePicker_RunAt.Text = JobData[13];
            }
        }

        private void textBox_JobDescription_TextChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.JobDescription = textBox_JobDescription.Text;
            m_bDataChanged = true;
        }

        private void button_AddDest_Click(object sender, EventArgs e)
        {
            //folderBrowserDialog.SelectedPath = this.textBox_Source.Text;
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Select Destination Folder";
            DialogResult dR = folderBrowserDialog.ShowDialog();
            if (dR == DialogResult.OK)
            {
                string sPath = folderBrowserDialog.SelectedPath;
                if (!checkDuplicate(sPath, this.listBox_Destinations))
                {
                    this.listBox_Destinations.Items.Add(sPath);
                    m_bDataChanged = true;
                    m_arg.JobDestinations = getItemsPiped(listBox_Destinations).Split(SPLIT_CHAR[0]);
                    
                }
            }
        }

        private void button_RemoveDest_Click(object sender, EventArgs e)
        {
            if (this.listBox_Destinations.SelectedIndex != -1)
            {
                this.listBox_Destinations.Items.RemoveAt(this.listBox_Destinations.SelectedIndex);
                m_bDataChanged = true;
                if (listBox_Destinations.Items.Count > 0)
                {
                    m_arg.JobDestinations = getItemsPiped(listBox_Destinations).Split(SPLIT_CHAR[0]);
                }
                else
                {
                    m_arg.JobDestinations = null;
                }
            }
        }

        private void comboBox_SynchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.SynchType = (comboBox_SynchType.SelectedIndex + 1).ToString();
            m_bDataChanged = true;
        }

        private void textBox_FileExFilter_TextChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.FileFiltersEx = textBox_FileExFilter.Text.Split(SPLIT_CHAR[0]);
            m_bDataChanged = true;
        }

        private void textBox_FileIncFilter_TextChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.FileFiltersIn = textBox_FileIncFilter.Text.Split(SPLIT_CHAR[0]);
            m_bDataChanged = true;
        }

        private void textBox_SubDirExcFilter_TextChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_bDataChanged = true;

            if (textBox_SubDirExcFilter.Text.IndexOf("*") != -1)
            {
                MessageBox.Show("Can not have Wildcard i.e * in Sub Dir Filters", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            m_arg.DirFiltersEx = textBox_SubDirExcFilter.Text.Split(SPLIT_CHAR[0]);
            
        }

        private void comboBox_LogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.LogLevel = (comboBox_LogLevel.SelectedIndex + 1).ToString();
            m_bDataChanged = true;
        }

        private void button_SaveJobDetails_Click(object sender, EventArgs e)
        {
            if (m_bDataChanged)
            {
                try
                {
                    saveJob();
                    m_bDataChanged = false;
                    this.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nothing Changed");
            }
        }


        private bool checkDuplicateJob(string thisJob)
        {
            bool bR = false;
            try
            {
                INI ini = new INI();
                string[] Jobs = ini.ReadSection();
                foreach (string iniValue in Jobs)
                {
                    string JobName = iniValue.Split(SPLIT_CHAR_EQUAL[0])[0];
                    if (thisJob.Trim().ToLower() == JobName.Trim().ToLower())
                    {
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





        private bool checkDuplicate(String FindItem, ComboBox comboBox)
        {

            if (comboBox.Items.Count > 0)
            {
                for (int iC = 0; iC < comboBox.Items.Count; iC++)
                {
                    string sVal = comboBox.Items[iC].ToString();
                    if (sVal.Contains(FindItem) && sVal.Equals(FindItem, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        private bool checkDuplicate(String FindItem, ListBox listBox)
        {

            if (listBox.Items.Count > 0)
            {
                for (int iC = 0; iC < listBox.Items.Count; iC++)
                {
                    string sVal = listBox.Items[iC].ToString();
                    if (sVal.Contains(FindItem) && sVal.Equals(FindItem, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        private bool checkDuplicate(String FindItem, ListView listBox)
        {

            if (listBox == null) return false;

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

        private String getItemsPiped(ListView oObj)
        {
            if (oObj.Items.Count > 0)
            {
                String sPiped = "";
                for (int iC = 0; iC < oObj.Items.Count; iC++)
                {
                    if (sPiped != "") sPiped += SPLIT_CHAR;
                    sPiped += (String)oObj.Items[iC].SubItems[0].Text;
                }
                if (sPiped != "")
                {
                    return sPiped.ToLower();
                }
            }
            return "";
        }

        private String getItemsPiped(ListBox oObj)
        {
            if (oObj.Items.Count > 0)
            {
                String sPiped = "";
                for (int iC = 0; iC < oObj.Items.Count; iC++)
                {
                    if (sPiped != "") sPiped += SPLIT_CHAR;
                    sPiped += (String)oObj.Items[iC];
                }
                if (sPiped != "")
                {
                    return sPiped.ToLower();
                }
            }
            return "";
        }
        private String getItemsPiped(ComboBox oObj)
        {
            if (oObj.Items.Count > 0)
            {
                String sPiped = "";
                for (int iC = 0; iC < oObj.Items.Count; iC++)
                {
                    if (sPiped != "") sPiped += SPLIT_CHAR;
                    sPiped += (String)oObj.Items[iC];
                }
                if (sPiped != "")
                {
                    return sPiped.ToLower();
                }
            }
            return "";
        }

        private void textBox_JobID_TextChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_bDataChanged = true;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if (m_bDataChanged)
            {
                if (MessageBox.Show("Job data has been changed. Discard changes?", "Detected Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    m_bDataChanged = false;
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void FormJobDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bDataChanged)
            {
                if (MessageBox.Show("Job data has been changed. Discard changes?", "Detected Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                }
                else
                {
                    e.Cancel=true;
                }
            }
        }

        private void textBox_Source_TextChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            if (Directory.Exists(textBox_Source.Text))
            {
                //MessageBox.Show("Directory does not exist or is in-accessible. If it's a network location, make sure it's a valid path", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                m_arg.JobSource = textBox_Source.Text;
                m_bDataChanged = true;
            }                
        }

        private void checkBox_UseSynshFramework_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.UseSynchFramework = (checkBox_UseSynshFramework.Checked)?"1":"0";
            m_bDataChanged = true;
        }

        private void checkBox_RunAt_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.UseRunAt = (checkBox_RunAt.Checked) ? "1" : "0";
            m_bDataChanged = true;
        }

        private void dateTimePicker_RunAt_ValueChanged(object sender, EventArgs e)
        {
            if (m_bIsLoading) return;
            m_arg.RunAt = dateTimePicker_RunAt.Text;
            m_bDataChanged = true;
        }

   }
}
