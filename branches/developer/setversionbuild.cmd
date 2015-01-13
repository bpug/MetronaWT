@ECHO OFF
SETLOCAL enableextensions
SET ERRORLEVEL=

SET DIR=%~dp0
IF %DIR:~-1%==\ SET DIR=%DIR:~0,-1%
cd %DIR%

SET PSEXE="%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe"
%PSEXE% -NoProfile -ExecutionPolicy Unrestricted -File "%DIR%\ext\scripts\msbuild\buildchoice.ps1" -p Configuration=Production%*

exit /b %ERRORLEVEL%
ENDLOCAL