param (
    [Parameter(Mandatory=$false, Position = 1)]
    [int]       $FaultRetries   = 255,
    [Parameter(Mandatory=$false, Position = 2)]
    [int]       $WaitTimeOut    = 75
)

# Core refs
$scriptRoot = (Split-Path -Parent $MyInvocation.MyCommand.Definition)

# Refs
$paths = @(
                'artifacts',
                '_artifacts',
                'bin',
                'build',
                '_build',
                'debug',
                'release',
                'obj'
            )

& (Join-Path $scriptRoot 'removedirectories.ps1') -Paths $paths -FaultRetries $FaultRetries -WaitTimeOut $WaitTimeOut
