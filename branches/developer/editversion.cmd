@ECHO OFF
SETLOCAL enableextensions
SET ERRORLEVEL=

SET DIR=%~dp0
IF %DIR:~-1%==\ SET DIR=%DIR:~0,-1%
cd %DIR%

subl "%DIR%\src\SolutionItems\AssemblyVersionInfo.xml"

exit /b %ERRORLEVEL%
ENDLOCAL