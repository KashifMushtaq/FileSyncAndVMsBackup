using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SynchServiceNS
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            //BeforeUninstall += new InstallEventHandler(BeforeUninstallEventHandler);
            //BeforeInstall += new InstallEventHandler(BeforeInstallEventHandler);

        }

        //public void BeforeInstallEventHandler(object sender, InstallEventArgs e)
        //{

        //    string dir = AssemblyDirectory + @"\packages";
        //    string batchFileName = dir + @"\install.bat";

        //    //MessageBox.Show(batchFileName);

        //    var startInfo = new ProcessStartInfo();
        //    startInfo.WorkingDirectory = dir;
        //    startInfo.FileName = batchFileName;
        //    startInfo.CreateNoWindow = true;
        //    startInfo.UseShellExecute = false;

        //    Process.Start(startInfo);
        //}

        //public void BeforeUninstallEventHandler(object sender, InstallEventArgs e)
        //{
        //    string dir = AssemblyDirectory + @"\packages";
        //    string batchFileName = dir + @"\uninstall.bat";

        //    //MessageBox.Show(batchFileName);

        //    var startInfo = new ProcessStartInfo();
        //    startInfo.WorkingDirectory = dir;
        //    startInfo.FileName = batchFileName;
        //    startInfo.CreateNoWindow = true;
        //    startInfo.UseShellExecute = false;

        //    Process.Start(startInfo);
        //}

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
