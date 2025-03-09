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

<p><h2>Batch Version of VMWare Workstation VMs Backup:</h2></p>
<p>Similar functionality as the C# tool but have to properly configure it to run via Task Scheduler.</p>
<code>
@echo off

cls

setlocal



REM 0 or 1 (1 == shutdown instead of suspending)
SET ShutDown=0
REM 0 or 1 (1 == backup VM dir to network location - no use of " or ' in path)
SET BackupVm=1

REM 0 or 1 (1 == use full VM list) id selected, the VMs in the list will be targeted
REM To use this switch create vmfulllist.txt file in working directory like VMRun list command creates.
SET UseFullList=0


REM Specify where vmrun.exe can be located
SET WSPath="C:\Program Files (x86)\VMware\VMware Workstation\vmrun.exe"
SET VMList=D:\vmware-automation\working\vmlist.txt
SET VMFullList=D:\vmware-automation\working\vmfulllist.txt
SET RoboLogDir=D:\vmware-automation\working
SET BackupDir=\\192.168.1.22\public\AutomatedVmsBackup



for /f "tokens=1-3 delims=/ " %%a in ('date /t') do (
	SET _date=%%a%%b%%c
)

SET RoboCopyLogFile=%RoboLogDir%/%_date%.log
SET RoboCopyLogFile=%RoboCopyLogFile:\=/%

REM replace \ with /
SET WSPath=%WSPath:\=/%
SET VMList=%VMList:\=/%
SET VMFullList=%VMFullList:\=/%
SET LOG=%RoboCopyLogFile%
SET RoboLogDir=%RoboLogDir:\=/%
SET BackupDir=%BackupDir:\=/%


echo This script takes a snapshot of all the running VMs after susending them.  > %LOG%
echo Deletes the old one first then takes a new snapshot. >> %LOG%
echo Restarts susended VM. Should run as a task under a user account. >> %LOG%

echo Started on [%date% at %time%] >> %LOG%
echo. >> %LOG%
echo VMware's vmrun path [%WSPath%] >> %LOG%


echo Started on [%date% at %time%]
echo.
echo VMware's vmrun path [%WSPath%]

echo.
echo. >> %LOG%

echo Generating running VMs list >> %LOG%
echo Generating running VMs list

%WSPath% list > %VMList%

echo. >> %LOG%

echo Running VMs list [%RunVMList%] >> %LOG%
echo Exit code from list command = %ERRORLEVEL% >> %LOG%
echo Exit code from list command = %ERRORLEVEL%
if %ERRORLEVEL% NEQ 0 GOTO End

if %UseFullList% EQU 1 (
	REM Use full VM list
	SET RunVMList=%VMFullList%
) else (
	REM Use generated list
	SET RunVMList=%VMList%
)



echo. >> %LOG%

FOR /F "tokens=* delims=" %%x in (%RunVMList%) DO (echo %%x >> %LOG%)
echo. >> %LOG%

FOR /F "tokens=* delims=" %%x in (%RunVMList%) DO (echo %%x)
echo.



FOR /F "delims=* skip=1" %%v IN (%RunVMList%) DO (
	CALL :SuspendVM "%%v"
	CALL :DleteSnapshot "%%v"
	CALL :TakeSnapshot "%%v"
	CALL :TakeBackup "%%v"
	
	REM restart only if running VM list was used. Otherwise, vmware autostart service will restart required VMs
	if %UseFullList% EQU 0 (
		CALL :StartVM "%%v"
	)
)


REM restart running VMs
if %UseFullList% EQU 1 (
	FOR /F "delims=* skip=1" %%v IN (%VMList%) DO (
		CALL :StartVM "%%v"
	)
)


REM FOR /F "delims=* skip=1" %%v IN (%VMList%) DO (CALL :DleteSnapshot "%%v")

REM FOR /F "delims=* skip=1" %%v IN (%VMList%) DO (CALL :TakeSnapshot "%%v")

REM FOR /F "delims=* skip=1" %%v IN (%VMList%) DO (CALL :StartVM "%%v")


GOTO theEnd









:SuspendVM
SET CurrentVM=%1
SET CurrentVM=%CurrentVM:\=/%

for /F "delims=" %%i in (%CurrentVM%) do (SET dirname=%%~dpi)
for /F "delims=" %%i in (%CurrentVM%) do (SET filename=%%~nxi)
for /F "delims=" %%i in (%CurrentVM%) do (SET basename=%%~ni)

SET dirname=%dirname:\=/%
echo Dir Name [%dirname%]
echo File Name [%filename%]
echo Base Name [%basename%]

echo Dir Name [%dirname%] >> %LOG%
echo File Name [%filename%] >> %LOG%
echo Base Name [%basename%] >> %LOG%

SET SOURCE=%dirname%
SET DEST=%BackupDir%/%basename%

if exist %CurrentVM% (
	
	if %ShutDown% EQU 1 (
	
		echo Stopping VM [%CurrentVM%] >> %LOG%
		echo Stopping VM [%CurrentVM%]
		
		%WSPath% stop %CurrentVM% hard >> %LOG%
		
		echo Exit code from stop command = %ERRORLEVEL% >> %LOG%
		echo Exit code from stop command = %ERRORLEVEL%
		
		REM if %ERRORLEVEL% NEQ 0 GOTO End
		
	) else (
		echo Suspending VM [%CurrentVM%] >> %LOG%
		echo Suspending VM [%CurrentVM%]
		
		%WSPath% suspend %CurrentVM% hard >> %LOG%
		
		echo Exit code from suspend command = %ERRORLEVEL% >> %LOG%
		echo Exit code from suspend command = %ERRORLEVEL%
		
		REM if %ERRORLEVEL% NEQ 0 GOTO End
	)
	
	echo. >> %LOG%
	
	echo Waiting for 30 seconds ... >> %LOG%
	timeout /t 30 /nobreak
	
	echo. >> %LOG%

)


GOTO :EOF






:DleteSnapshot
SET CurrentVM=%1
SET CurrentVM=%CurrentVM:\=/%

if exist %CurrentVM% (
	
	echo Deleting  auto-snapshot from VM [%CurrentVM%] >> %LOG%
	echo Deleting  auto-snapshot from VM [%CurrentVM%]
	
	%WSPath% deleteSnapshot %CurrentVM% auto-snapshot >> %LOG%
	
	echo Exit code from deleteSnapshot command = %ERRORLEVEL% >> %LOG%
	echo Exit code from deleteSnapshot command = %ERRORLEVEL%

	REM if %ERRORLEVEL% NEQ 0 GOTO End

	echo. >> %LOG%
	
	echo Waiting for 60 seconds ... >> %LOG%
	timeout /t 60 /nobreak

	echo. >> %LOG%
)

GOTO :EOF



:TakeSnapshot
SET CurrentVM=%1
SET CurrentVM=%CurrentVM:\=/%

if exist %CurrentVM% (
	
	echo Taking auto-snapshot of VM [%CurrentVM%] >> %LOG%
	echo Taking auto-snapshot of VM [%CurrentVM%]
	
	%WSPath% snapshot %CurrentVM% auto-snapshot >> %LOG%
	
	echo Exit code from snapshot command = %ERRORLEVEL% >> %LOG%
	echo Exit code from snapshot command = %ERRORLEVEL%
	
	REM if %ERRORLEVEL% NEQ 0 GOTO End

	echo. >> %LOG%
	
	echo Waiting for 30 seconds ... >> %LOG%
	timeout /t 30 /nobreak
	
	echo. >> %LOG%
)

GOTO :EOF




:TakeBackup
SET CurrentVM=%1
SET CurrentVM=%CurrentVM:\=/%

if %BackupVm% EQU 1 (

	echo Taking backup of VM using robocopy [%CurrentVM%] >> %LOG%
	echo Source [%SOURCE%]  >> %LOG%
	echo Dest [%DEST%]  >> %LOG%

	echo Taking backup of VM using robocopy [%CurrentVM%]
	echo Source [%SOURCE%]
	echo Dest [%DEST%]
	
	robocopy.exe "%SOURCE%" "%DEST%" /MIR /xa:H /xo /ts /x /fp /xf *.log *.lck *.scoreboard /xd caches *.lck /tee /eta /np /njh /njs /log+:%RoboCopyLogFile%
	
	echo Exit code from Robocopy = %ERRORLEVEL% >> %LOG%
	echo Exit code from Robocopy = %ERRORLEVEL%

	REM if %ERRORLEVEL% NEQ 0 GOTO End
	
	echo backup of VM completed [%CurrentVM%] >> %LOG%
	echo backup of VM completed [%CurrentVM%]
	
) else (
	echo Not taking backup of VM [%CurrentVM%] >> %LOG%
)

GOTO :EOF




:StartVM
SET CurrentVM=%1
SET CurrentVM=%CurrentVM:\=/%

if exist %CurrentVM% (
	
	echo Starting VM [%CurrentVM%] >> %LOG%
	echo Starting VM [%CurrentVM%]
	
	%WSPath% start %CurrentVM% nogui >> %LOG%
	
	echo Exit code from start command = %ERRORLEVEL% >> %LOG%
	echo Exit code from start command = %ERRORLEVEL%
	
	REM if %ERRORLEVEL% NEQ 0 GOTO End

	echo. >> %LOG%
	
	echo Waiting for 30 seconds ... >> %LOG%
	timeout /t 30 /nobreak
	
	echo. 
)

GOTO :EOF










:End

IF %ERRORLEVEL% NEQ 0 (

	echo. >> %LOG%
	
	
	if %ShutDown% EQU 1 (
		echo Starting stopped VM [%CurrentVM%] after error >> %LOG%
		echo Starting stopped VM [%CurrentVM%] after error
	) else (
		echo Starting suspended VM [%CurrentVM%] after error >> %LOG%
		echo Starting suspended VM [%CurrentVM%] after error
	)
		
	%WSPath% start %CurrentVM% nogui >> %LOG%
	
	echo. >> %LOG%

	
	if %ShutDown% EQU 1 (
		echo Wait a little bit for the suspended VM to start ... >> %LOG%
		echo Wait a little bit for the suspended VM to start ...
	)
	
	echo Waiting for 30 seconds ... >> %LOG%
	timeout /t 30 /nobreak
	
	echo. >> %LOG%
)

GOTO :EOF









:theEnd

	
echo Finished on [%date% at %time%] >> %LOG%
echo. >> %LOG%

echo Finished on [%date% at %time%]
echo.

if %BackupVm% EQU 1 (
	echo All running VM's old snapshot deleted. >> %LOG%
	echo All running VM's snapshot taken successfuly. >> %LOG%
	echo All running VM's backedup. >> %LOG%
	echo All running started again. >> %LOG%
	
	echo All running VM's old snapshot deleted.
	echo All running VM's snapshot taken successfuly.
	echo All running VM's backedup.
	echo All running started again.
) else (
	echo All running VM's old snapshot deleted. >> %LOG%
	echo All running VM's snapshot taken successfuly. >> %LOG%
	echo All running VM's NOT backedup. >> %LOG%
	echo All running started again. >> %LOG%
	
	echo All running VM's old snapshot deleted.
	echo All running VM's snapshot taken successfuly.
	echo All running VM's NOT backedup.
	echo All running started again.
)

endlocal

Exit

</code>

<p>Project’s source code can be cloned and compiled under GNU license. I do not take any responsibility what so ever.</p>

