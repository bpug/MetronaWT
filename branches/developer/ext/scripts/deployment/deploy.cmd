@echo off
@setlocal enableextensions

set dir=%~dp0
if %dir:~-1%==\ set dir=%dir:~0,-1%

"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Unrestricted -File "%dir%\deploy.ps1"
