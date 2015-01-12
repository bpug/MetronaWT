# Core refs
$ScriptRoot                 = (Split-Path -Parent $MyInvocation.MyCommand.Definition)
$ScriptPath                 = (Split-Path $MyInvocation.MyCommand.Path)

# Import common utility
. (Join-Path $ScriptRoot '_common.ps1')

# Core refs
$ToolsPath                  = (Get-AbsolutePath $ScriptRoot, 'tools')
$7zTool                     = (Get-AbsolutePath $ToolsPath, '7za.exe')
$StoreAdm                   = (Get-AbsolutePath $ToolsPath, 'StoreAdm.exe')
$XmlDeployedFileName        = 'deployed.xml'
$XmlDeployedFileTemplate    = (Get-AbsolutePath $ScriptRoot, $XmlDeployedFileName)
$ReleasesFileExtension      = 'zip'

# Project specific refs
$ReleaseDirectory           = (Get-AbsolutePath $ScriptRoot, '..\releases')
$IISDirectory               = (Get-AbsolutePath $ScriptRoot, '..\..\inetpub')
$StagingAreaKey             = 'portal:Staging'
$StagingAreas               = New-Object object[] 3
$ConnectionStrings          = New-Object object[] 3
$StagingAreas[0]            = 'Portal'
$ConnectionStrings[0]       = 'WebManagerAppDataProduction'
$StagingAreas[1]            = 'Portaltest'
$ConnectionStrings[1]       = 'WebManagerAppDataTesting'
$StagingAreas[2]            = 'Portaldevelopment'
$ConnectionStrings[2]       = 'WebManagerAppDataDevelopment'
$RelativeConfigFilePath     = 'web.config'
$ConnectionStringName       = 'Brunata.WebManager.Web'
$ConnectionStringTemplate   = 'Data Source=mssql;Initial Catalog=[PARAMETER1];Integrated Security=SSPI'


# Private
function Get-LatestDeployedFileName
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea
    )

    $xmlFileName = (Get-AbsolutePath $ReleaseDirectory, $StagingArea, $XmlDeployedFileName)
    Test-FileOrAdd -File $xmlFileName -OriginalFile $XmlDeployedFileTemplate

    Print-Message "Get latest deployed filename in: '${xmlFileName}'..."

    $xmlDeployed = [xml](Get-Content -Path $xmlFileName)
    $latestDeployedFileName = $xmlDeployed.deployed.latest

    Print-Info "Latest deployed filename: '${latestDeployedFileName}'"

    Print-Success "Get latest deployed filename in: '${xmlFileName}'...Done"
    Print-Success

    return $latestDeployedFileName
}

function Set-LatestDeployedFileName
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea,
        [Parameter(Mandatory=$true, Position = 1)]
        [string] $LatestReleaseFileName
    )

    $xmlFileName = (Get-AbsolutePath $ReleaseDirectory, $StagingArea, $XmlDeployedFileName)
    Test-FileOrAdd -File $xmlFileName -OriginalFile $XmlDeployedFileTemplate

    Print-Message "Set latest deployed filename '${LatestReleaseFileName}' in '${xmlFileName}'..."

    $xmlDeployed = [xml](Get-Content -Path $xmlFileName)
    $xmlDeployed.deployed.latest = $LatestReleaseFileName
    $xmlDeployed.save((Resolve-Path -Path $xmlFileName))

    Print-Success "Set latest deployed filename '${LatestReleaseFileName}' in '${xmlFileName}'...Done"
    Print-Success
}

function Get-LatestRelease
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea
    )

    $releaseStagingAreaDirectory = (Get-AbsolutePath $ReleaseDirectory, $StagingArea)

    Print-Message "Get latest release in: '${releaseStagingAreaDirectory}'..."

    $latestReleaseFileName = (Get-ChildItem -Path $releaseStagingAreaDirectory -Filter ("*." + $ReleasesFileExtension) | Where-Object { -not $_.PSIsContainer } | Sort-Object -Property LastWriteTime | Select-Object -Last 1)

    Print-Info "Latest release: '${latestReleaseFileName}'"

    Print-Success "Get latest release in: '${releaseStagingAreaDirectory}'...Done"
    Print-Success

    return $latestReleaseFileName
}

function Stop-AppPool
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $AppPoolName
    )

    Print-Message "Stop app pool: '${appPoolName}'..."

    $executable = 'C:\Windows\System32\inetsrv\appcmd.exe'

    $quotedAppPoolString = (Get-QuotedString $AppPoolName)
    $quotedAppPoolParam = "/apppool.name:`"$quotedAppPoolString`""

    & $executable stop apppool $quotedAppPoolParam

    Print-Success "Stop app pool: '${appPoolName}'...Done"
    Print-Success
}

function Clear-IsolatedStorage
{
    Print-Message "Clear IsolatedStorage ..."

    & $StoreAdm /REMOVE | Out-Null
    & $StoreAdm /REMOVE /ROAMING | Out-Null
    & $StoreAdm /REMOVE /MACHINE | Out-Null

    Print-Success "Clear IsolatedStorage ...Done"
    Print-Success
}

function Delete-IISFiles
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea
    )

    $iisStagingAreaDirectory = (Get-AbsolutePath $IISDirectory, $StagingArea, '*')

    Print-Message "Delete IIS Files: '${iisStagingAreaDirectory}'..."

    Remove-Item -Path ${iisStagingAreaDirectory} -Recurse -Force

    Print-Success "Delete IIS Files: '${iisStagingAreaDirectory}'...Done"
    Print-Success
}

function Extract-LatestRelease
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea,
        [Parameter(Mandatory=$true, Position = 1)]
        [string] $LatestReleaseFileName
    )

    $releaseStagingAreaDirectory = (Get-AbsolutePath $ReleaseDirectory, $StagingArea)
    $latestRelease = (Get-AbsolutePath $releaseStagingAreaDirectory, $LatestReleaseFileName)
    $iisStagingAreaDirectory = (Get-AbsolutePath $IISDirectory, $StagingArea)

    Print-Message "Extract latest release from: '${latestRelease}'"
    Print-Message "Extract latest release to: '${iisStagingAreaDirectory}'..."

    $quotedLatestReleaseString = (Get-QuotedString $latestRelease)
    $quotedLatestReleaseParam = "`"$quotedLatestReleaseString`""
    $quotedOutputString = (Get-QuotedString $iisStagingAreaDirectory)
    $quotedoutputParam = "-o`"$quotedOutputString`""

    & $7zTool x $quotedLatestReleaseParam $quotedoutputParam -y | Out-Null

    Print-Success "Extract latest release from: '${latestRelease}'"
    Print-Success "Extract latest release to: '${iisStagingAreaDirectory}'...Done"
    Print-Success
}

function Set-StaticStaging
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea
    )

    $iisStagingAreaDirectory = (Get-AbsolutePath $IISDirectory, $StagingArea)
    $configFile = (Get-AbsolutePath $iisStagingAreaDirectory, $RelativeConfigFilePath)
    if ($configFile -and (Test-Path -Path $configFile))
    {
        Print-Message "Set portal:Staging in: '${configFile}'..."

        Set-KeyValue -path $configFile -key $StagingAreaKey -value $StagingArea

        Print-Success "Set portal:Staging in: '${configFile}'...Done"
        Print-Success
    }
    else
    {
        Print-Error "Set portal:Staging in: '${configFile}'...Not found"
        Print-Error
    }
}

function Set-ConnectionString
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $StagingArea,
        [Parameter(Mandatory=$true, Position = 1)]
        [string] $ConnectionStringName,
        [Parameter(Mandatory=$true, Position = 2)]
        [string] $ConnectionStringParameter
    )

    $iisStagingAreaDirectory = (Get-AbsolutePath $IISDirectory, $StagingArea)
    $configFile = (Get-AbsolutePath $iisStagingAreaDirectory, $RelativeConfigFilePath)
    if ($configFile -and (Test-Path -Path $configFile))
    {
        Print-Message "Set ConnectionString in: '${configFile}'..."

        $connectionString = $ConnectionStringTemplate.Replace('[PARAMETER1]', $ConnectionStringParameter)

        Set-ConnectionStringInternal -path $configFile -name $ConnectionStringName -value $connectionString

        Print-Success "Set ConnectionString in: '${configFile}'...Done"
        Print-Success
    }
    else
    {
        Print-Error "Set ConnectionString in: '${configFile}'...Not found"
        Print-Error
    }
}

function Start-AppPool
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $AppPoolName
    )

    Print-Message "Start app pool: '${appPoolName}'..."

    $executable = 'C:\Windows\System32\inetsrv\appcmd.exe'

    $quotedAppPoolString = (Get-QuotedString $AppPoolName)
    $quotedAppPoolParam = "/apppool.name:`"$quotedAppPoolString`""

    & $executable start apppool $quotedAppPoolParam

    Print-Success "Start app pool: '${appPoolName}'...Done"
    Print-Success
}


# Main
for ($i = 0; $i -lt $StagingAreas.Length; ++$i)
{
    $stagingArea = $StagingAreas[$i]
    $connectionString = $ConnectionStrings[$i]

    Print-Message "Process staging area: '${stagingArea}'..." -Light
    Print-Message

    $latestRelease = (Get-LatestRelease $stagingArea)
    if ($latestRelease -and (Test-Path -Path $latestRelease.FullName))
    {
        if ((Get-LatestDeployedFileName $stagingArea) -ne $latestRelease.Name)
        {
            Stop-AppPool $stagingArea
            Clear-IsolatedStorage
            Delete-IISFiles $stagingArea
            Extract-LatestRelease $stagingArea $latestRelease
            Set-StaticStaging $stagingArea
            Set-ConnectionString $stagingArea $ConnectionStringName $connectionString
            Start-AppPool $stagingArea
            Set-LatestDeployedFileName $stagingArea $latestRelease.Name
        }
    }

    Print-Success "Process staging area: '${stagingArea}'...Done" -Light
    Print-Message
    Print-Message
}
