Param (
    [alias('t')]
    [string] $BuildTriggerType  = $null,
    [alias('p')]
    [string] $BuildParams       = $null,
    [alias('o')]
    [switch] $OnlySetVersion    = $false
)

# Core refs
$scriptRoot             =   (Split-Path -Parent $MyInvocation.MyCommand.Definition)

# Import common utility
. (Join-Path $scriptRoot 'common.ps1')

# Refs
$buildScript            =   (Join-Path $scriptRoot 'build.ps1')
$setVcsInfoBuildTarget  =   'SetVcsInfo'
$defaultBuildTarget     =   'Build'
$caption                =   'Select build type'
$message                =   'Do you want to: '
$choices                =   @(
                                '&Auto',
                                '&Ci',
                                '&Nightly',
                                '&PreRelease',
                                '&Stable',
                                '&Quit'
                            )
$helpMessages           =   @(
                                'Auto         - Automatic select versioning scheme from current branch name',
                                'Ci           - Versioning scheme e.g. 1.0.0+beta.1.abcd012.0001',
                                'Nightly      - Versioning scheme e.g. 1.0.0+beta.1.abcd012',
                                'PreRelease   - Versioning scheme e.g. 1.0.0-beta.1',
                                'Stable       - Versioning scheme e.g. 1.0.0',
                                'Quit'
                            )


# Private
function Get-BuildTriggerType
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position = 0)]
        [string] $BuildTriggerType
    )

    switch ($BuildTriggerType)
    {
        'A' { return $null }
        'C' { return 'BuildTriggerType=Ci' }
        'N' { return 'BuildTriggerType=Nightly' }
        'P' { return 'BuildTriggerType=PreRelease' }
        'S' { return 'BuildTriggerType=Stable' }
        'Q' { Exit 1 }
    }
}

if ($BuildParams)
{
    $BuildParams += ';'
}


# Main
if ($BuildTriggerType)
{
    $BuildParams += (Get-BuildTriggerType $BuildTriggerType)
}
else
{
    switch (Select-Item -Caption $caption -Message $message -ChoiceList $choices -HelpMessages $helpMessages -Default 1)
    {
        0 { $BuildParams += (Get-BuildTriggerType 'A') }
        1 { $BuildParams += (Get-BuildTriggerType 'C') }
        2 { $BuildParams += (Get-BuildTriggerType 'N') }
        3 { $BuildParams += (Get-BuildTriggerType 'P') }
        4 { $BuildParams += (Get-BuildTriggerType 'S') }
        5 { $BuildParams += (Get-BuildTriggerType 'Q') }
    }
}

& $buildScript -BuildTarget $setVcsInfoBuildTarget -BuildParams $BuildParams -ForceCleanup

if (-Not $OnlySetVersion)
{
    & $buildScript -BuildTarget $defaultBuildTarget -BuildParams $BuildParams
}