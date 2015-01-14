Param (
    [alias('f')]
    [string] $BuildFile     = (Join-Path (Split-Path -Parent $MyInvocation.MyCommand.Definition) 'project.msbuild'),
    [alias('p')]
    [string] $BuildParams   = $null,
    [alias('t')]
    [string] $BuildTarget   = 'Build',  #'WebPackage', #
    [alias('c')]
    [switch] $ForceCleanup  = $false,
	[alias('co')]
    [string] $Configuration   = $null
)

# Core refs
$rootPath           = Get-Location

# Refs
$sln   = (Join-Path $rootPath 'src\project.sln') 
$publishProfile   = 'Production'
#$publishProfile   = (Join-Path $rootPath 'ext\scripts\msbuild\web.package.pubxml')
$cleanBuildScript   = (Join-Path $rootPath 'ext\scripts\ps\RemoveBuildDirectories.ps1')
$msbuildExecutable  = (Join-Path (Get-Content -Path Env:\SystemRoot) 'Microsoft.NET\Framework\v4.0.30319\MSBuild.exe')
$buildFolder        = (Join-Path $rootPath '_build')
$buildLogFile       = (Join-Path $buildFolder 'msbuild.log')
$buildLogFileParam  = "/fileloggerparameters:LogFile=${buildLogFile};Verbosity=diagnostic;Encoding=UTF-8"

if ($BuildTarget)
{
    $BuildTarget = "/target:${BuildTarget}"
}

if ($BuildParams)
{
    $BuildParams = "/property:${BuildParams}"
}

# Main
if ($ForceCleanup)
{
    & $cleanBuildScript -FaultRetries 100 -WaitTimeOut 0
}

if (-not (Test-Path -Path $buildFolder))
{
    New-Item -Path $buildFolder -ItemType Directory | Out-Null
}

& $msbuildExecutable $BuildFile $BuildTarget $BuildParams /maxcpucount /fileLogger $buildLogFileParam /nodeReuse:false
# & $msbuildExecutable $sln /p:DeployOnBuild=true /p:PublishProfile=$publishProfile /p:Configuration=$publishProfile
if($LASTEXITCODE -ne 0)
{
    Throw 'Build failed'
    Exit 1
}