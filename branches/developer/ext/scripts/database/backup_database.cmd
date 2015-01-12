@echo off
@setlocal enableextensions

"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Unrestricted -File "%~dp0backup-database.ps1"
