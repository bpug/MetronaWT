@ECHO OFF
SETLOCAL enableextensions
SET ERRORLEVEL=

ECHO Comparing two files: %1 with %2

IF NOT EXIST %1 GOTO file1notfound
IF NOT EXIST %2 GOTO file2notfound

fc %1 %2
IF %ERRORLEVEL%==0 GOTO nocopy

ECHO Files are not the same. Copying %1 over %2
copy %1 %2 /y & GOTO end

:nocopy
ECHO Files are the same. Did nothing
GOTO end


:file1notfound
ECHO %1 not found.
GOTO end


:file2notfound
copy %1 %2 /y
GOTO end


:end
ECHO Done.
exit /b %ERRORLEVEL%
ENDLOCAL