param (
    [Parameter(Position = 0)]
    [string] $BackupPath,
    [Parameter(Position = 1)]
    [string] $TempBackupPath,
    [Parameter(Position = 2)]
    [string[]] $DatabaseNames = @( 'WebManagerAppDataDevelopment', 'WebManagerAppDataTesting', 'WebManagerAppData' )
)

$currentFolder = Split-Path ($MyInvocation.MyCommand.Path) -Parent
$toolsPath = Join-Path $currentFolder 'tools'
$sqlScriptName = Join-Path $currentFolder 'backup_database.sql'
$7zip = Join-Path $toolsPath '7za.exe'
if (-not $BackupPath) { $BackupPath = Join-Path $currentFolder 'bak' }
if (-not $TempBackupPath) { $TempBackupPath = Join-Path $backupPath 'temp' }

if (-not (Test-Path $BackupPath)) { $null = New-Item $BackupPath -ItemType Directory }
if (-not (Test-Path $TempBackupPath)) { $null = New-Item $TempBackupPath -ItemType Directory }

Write-Verbose "currentFolder '$currentFolder'"
Write-Verbose "toolsPath '$toolsPath'"
Write-Verbose "sqlScriptName '$sqlScriptName'"
Write-Verbose "7zip '$7zip'"
Write-Verbose "backupPath '$BackupPath'"
Write-Verbose "tempBackupPath '$TempBackupPath'"

function Get-Quoted
{
    [CmdletBinding(DefaultParameterSetName = 'Path')]
    param (
        [parameter(Position = 0, Mandatory = $true)]
        [string] $Path
    )

    if ((Test-Path $Path) -and (Get-Item $Path).PsIsContainer -and -not $Path.EndsWith('\'))
    {
        return "`"$Path\`""
    }

    return "`"$Path`""
}

function Update-Database
{
    [CmdletBinding(DefaultParameterSetName = 'DatabaseName')]
    param (
        [parameter(Position = 0, Mandatory = $true)]
        [string] $DatabaseName
    )

    #& sqlcmd -S mssql -I -E -i "`"$sqlScriptName`"" -v DatabaseName="`"$DatabaseName`"" DestinationDirectory="`"$TempBackupPath\`""
    & sqlcmd -S mssql -I -E -i (Get-Quoted $sqlScriptName) -v DatabaseName=(Get-Quoted $DatabaseName) DestinationDirectory=(Get-Quoted $TempBackupPath)

    Write-Host "Backup database '$DatabaseName' successfully."
    Write-Host
}

function Compress-File
{
    $file = Get-Item (Join-Path $TempBackupPath '*.bak')
    if ($file) {
        $fileName = $file.BaseName

        #& $7zip a -mx=9 -ms=on -mmt=on (Join-Path $BackupPath ($fileName + '.7z')) (Join-Path $TempBackupPath ($fileName + '.bak'))
        & $7zip a -mx=9 -ms=on -mmt=on (Get-Quoted (Join-Path $BackupPath ($fileName + '.7z'))) (Get-Quoted (Join-Path $TempBackupPath ($fileName + '.bak')))
        Remove-Item (Join-Path $TempBackupPath ($fileName + '.bak')) -Force

        Write-Host "Compress backup '$fileName' successfully."
        Write-Host
    }
}

#$databaseNames = @( 'WebManagerAppDataDevelopment', 'WebManagerAppDataTest', 'WebManagerAppData' )

# Main
foreach ($databaseName in $DatabaseNames)
{
    Update-Database $databaseName
    Compress-File
}
