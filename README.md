<p><h1>VMware VMs, Files and Directories Synchronization (Mirroring) Project (C#)</h1></p>
<p>&nbsp;</p>
<p>The project uses Microsoft's <a href="https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/robocopy">Robust Copy (RoboCopy)</a>
and VMWare Workstation's <a href="https://techdocs.broadcom.com/us/en/vmware-cis/desktop-hypervisors/fusion-pro/13-0/using-vmware-fusion/using-the-vmrun-command-to-control-virtual-machines/running-vmrun-commands/syntax-of-vmrun-commands.html">vmrun</a> utility to create mirror backups at desired location.</p>
<p>&nbsp;</p>
<p>Files and directories can be synchronized from one source folder to multiple destinations (network shares etc).</p>
<p>&nbsp;</p>
<p>VMWare Virtual Machines, however, can be backed-up only at one destination (network share etc).</p>
<p>Tool can suspend, shutdown, take a snapshot and delete an old snapshot before using RoboCopt to synchronize VM's directory. It then starts the VM as well which were running backup is triggered.</p>

<img src="https://github.com/KashifMushtaq/FileSyncAndVMsBackup/blob/main/sync1.jpg" />

<p><h2>Sub projects:</h2></p>

* SynchService (Windows Service x64)
* SynchServiceManager (.Net Framework Windows Forms Application)
* SynchSetup (Visual Studio Setup Project)

<p>&nbsp;</p>
<p><h2>Dependencies:</h2></p>

* Microsoft Visual Studio (2019 or later)
* Minimum .Net Framework 3.5

<p>&nbsp;</p>
<p><h2>Prebuilt Installer:</h2></p>
<p>MSI installer can be downloaded from <b>SynchSetup\Release</b> folder. It installs the required dependencies and on uninstall, it also removes them.</p>

<p>&nbsp;</p>
<p><h2>Manual Steps After Installation:</h2></p>
<p>After installation, you must configure the service to run under some local admin user account who has access to all the network folder (without prompting for password) which you may want to synchronize (source and destinations). If you synchronize folders between local and network destination, the service context must have read and write access to source and destination folders. Please access the network path using \\HOST\NetworkShare and when you are prompted by the Password Window, please choose to save the password. Then you can use the same user account to run the Synchronization Service.</p>

<p>&nbsp;</p>
<p><h2>Synchronization Capabilities:</h2></p>
<p>The service and client program uses Microsoft's RoboCopy to synchronize/mirror single source to multiple destinations:</p>
<p>&nbsp;</p>

1.	Local to multiple local destinations
2.	Local to multiple local and network destinations
3.	Local to multiple network destinations
4.	Network location to single or multiple local destination
5.	Network location to single or multiple network destinations

<p>Infect, It can synchronize from any location to any location provided it has access and read and write permissions.</p>

<img src="https://github.com/KashifMushtaq/FileSyncAndVMsBackup/blob/main/sync2.jpg" />

<p>The SynchService, can be configured to run the synchronization at preset time or after a certain interval. Just configure and forget service. It will keep the source and destination synchronized by itself.</p>

<p>It produces a log file and log levels could be set via management interface as well as at the job details.</p>
<p>You can add as many jobs you want.</p>


<p>&nbsp;</p>
<p><h2>VMWare Workstation's VMs Synchronization Capabilities:</h2></p>
<p>The service and client program uses VMWare Workstation's <b>vmrun</b> and Microsoft's <b>RoboCopy</b> utilities to synchronize/mirror VMs loaded in Workstation Manager:</p>
<p>&nbsp;</p>


1.	Generates a list of all the running VMs in the selected working directory.
2.	It can also load pre-compiled list of all the VMs (Registered VMs in Workstation Management Interface)  required to be backed up. Create a file named  <b>vmfulllist.txt</b> in the picked working directory and enable its use on VMs backup tab.
	<p>Sample File:</p>
 	<p><img src="https://github.com/KashifMushtaq/FileSyncAndVMsBackup/blob/main/samplefile.jpg"/></p>
3.	VMs in the list will be suspended / stopped.
4.	A snapshot named <b>auto-snapshot</b> can be deleted (if exists).
4.	A new snapshot named <b>auto-snapshot</b> can be taken.
5.	VM's directory can be mirrored at destination location.
6.	VM will be restarted after backup if it was running previously.

<p>The SynchService, can be configured to run the VMs back at a preset time after a certain interval. It will keep all the running or or full list, synchronized at the destination directory</p>
<p>VM's .vmx files key DisplayName will be used as backup directory.</p>

<p>Project’s source code can be cloned and compiled under GNU license. I do not take any responsibility what so ever.</p>

