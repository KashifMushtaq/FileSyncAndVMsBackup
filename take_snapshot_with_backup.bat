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


