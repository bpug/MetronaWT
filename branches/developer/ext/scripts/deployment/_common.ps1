function Require-Param {
    [CmdletBinding()]
    param (
        [string] $Value,
        [string] $ParamName
    )

    if ($Value -eq $null)
    {
        Print-Error "The parameter -${ParamName} is required."
        exit 1
    }
}

function Print-Error {
    [CmdletBinding()]
    param (
        [string] $Value,
        [switch] $Light = $false
    )

    if ($Light)
    {
        Write-Host $Value -ForeGroundColor White -BackgroundColor Red
    }
    else
    {
        Write-Host $Value -ForeGroundColor Red
    }
}

function Print-Warning {
    [CmdletBinding()]
    param (
        [string] $Value,
        [switch] $Light = $false
    )

    if ($Light)
    {
        Write-Host $Value -ForeGroundColor Red -BackgroundColor Yellow
    }
    else
    {
        Write-Host $Value -ForeGroundColor Yellow
    }
}

function Print-Success {
    [CmdletBinding()]
    param (
        [string] $Value,
        [switch] $Light = $false
    )

    if ($Light)
    {
        Write-Host $Value -ForeGroundColor Black -BackgroundColor Green
    }
    else
    {
        Write-Host $Value -ForeGroundColor Green
    }
}

function Print-Message {
    [CmdletBinding()]
    param (
        [string] $Value,
        [switch] $Light = $false
    )

    if ($Light)
    {
        Write-Host $Value -ForeGroundColor Black -BackgroundColor White
    }
    else
    {
        Write-Host $Value -ForeGroundColor White
    }
}

function Print-Info {
    [CmdletBinding()]
    param (
        [string] $Value,
        [switch] $Light = $false
    )

    if ($Light)
    {
        Write-Host $Value
    }
    else
    {
        Write-Host $Value
    }
}

function Require-Module {
    [CmdletBinding()]
    param (
        [string] $Name
    )

    if (-not(Get-Module -Name $Name))
    {
        if (Get-Module -ListAvailable | Where-Object { $_.name -eq $Name })
        {
            Print-Message 'Module is available and will be loaded.'
            Import-Module -Name $Name
        }
        else
        {
            Print-Error "The module '${Name}' is required."
            exit 1
        }
    }
}

function Programfiles-Dir {
    if (Is64bit -eq $true)
    {
        (Get-Item 'Env:ProgramFiles(x86)').Value
    }
    else
    {
        (Get-Item 'Env:ProgramFiles').Value
    }
}

function Is64bit {
    return ([IntPtr]::Size -eq 8)
}

function Get-QuotedString
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [string] $Value
    )

    if (-not $Value.StartsWith('"') -and -not $Value.EndsWith('"'))
    {
        return "`"$Value`""
    }

    return $Value
}

function Get-QuotedPath
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [string] $Path
    )

    if ((Test-Path $Path) -and (Get-Item $Path).PsIsContainer -and -not $Path.EndsWith('\'))
    {
        return "`"$Path\`""
    }

    return "`"$Path`""
}

function Get-AbsolutePath {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string[]] $Paths
    )

    $completePath = $Paths[0]

    for ($i = 1; $i -lt $Paths.Length; ++$i)
    {
        $path = $Paths[$i]
        $completePath = Join-Path $completePath $path
    }

    return $executionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($completePath)
}

function Set-KeyValue
{
    [CmdletBinding()]
    param (
        [string] $path,
        [string] $key,
        [string] $value
    )

    $settings = [xml](Get-Content -Path $path)
    $setting = $settings.configuration.appSettings.add | Where-Object { $_.key -eq $key }
    $setting.value = "${value}"
    $resolvedPath = Resolve-Path -Path $path
    $settings.save($resolvedPath)
}

function Set-ConnectionStringInternal
{
    [CmdletBinding()]
    param (
        [string] $path,
        [string] $name,
        [string] $value
    )

    $settings = [xml](Get-Content -Path $path)
    $setting = $settings.configuration.connectionStrings.add | Where-Object { $_.name -eq $name }
    $setting.connectionString = "${value}"
    $setting.providerName = 'System.Data.SqlClient'
    $resolvedPath = Resolve-Path -Path $path
    $settings.save($resolvedPath)
}

function Test-FileOrAdd
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [string] $File,
        [Parameter(Mandatory = $true, Position = 1)]
        [string] $OriginalFile
    )

    if (-not (Test-Path -Path $File))
    {
        Print-Warning "File not exist: '${File}'"

        if (Test-Path -Path $OriginalFile)
        {
            Copy-Item $OriginalFile $File
            Print-Info "File '${File}' created from '${OriginalFile}'"
            Print-Info
        }
        else
        {
            Print-Error "Original file '${OriginalFile}' not exist"
            Print-Error
        }
    }
    else
    {
        Print-Message "File already exist: '${File}'"
        Print-Message
    }
}

function Select-Item
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $false, Position = 0)]
        [string] $Caption = $null,
        [Parameter(Mandatory = $false, Position = 1)]
        [string] $Message = $null,
        [Parameter(Mandatory = $true, Position = 2)]
        [string[]] $ChoiceList,
        [Parameter(Mandatory = $false, Position = 3)]
        [string[]] $HelpMessages = $null,
        [Parameter(Mandatory = $false, Position = 4)]
        [int] $Default = 0
    )

    $choices = New-Object System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]

    for($i = 0; $i -lt $ChoiceList.Length; ++$i)
    {
        $choice = New-Object Management.Automation.Host.ChoiceDescription $ChoiceList[$i]

        if($HelpMessages -and $HelpMessages[$i])
        {
            $choice.HelpMessage = $HelpMessages[$i]
        }

        $choices.Add($choice)
    }

    $Host.UI.PromptForChoice($Caption, $Message, $choices, $Default)
}
