param (
    [Parameter(Mandatory=$false, Position = 0)]
    [string[]]  $Paths        = $null,
    [Parameter(Mandatory=$false, Position = 1)]
    [int]       $FaultRetries   = 255,
    [Parameter(Mandatory=$false, Position = 2)]
    [int]       $WaitTimeOut    = 75
)

$ErrorActionPreference = "Stop"

$scriptRoot = (Split-Path -Parent $MyInvocation.MyCommand.Definition)
Import-Module (Join-Path $scriptRoot '..\..\..\lib\msbuild\Gapon.MSBuild.Tasks.dll')


$inProgress = '[IN PROGRESS]'
$success    = '[DELETED]    '
$failed     = '[FAILED]     '
$hasMatched = $false

foreach ($path in $Paths)
{
    $items = Find-Path -SearchPath $path -IncludeEmptyDirectories -SearchFilter FindDirectories
    foreach ($item in $items)
    {
        if (-not (Test-Path -Path $item -ErrorAction SilentlyContinue))
        {
            Continue
        }

        $cursorPos = $Host.UI.RawUI.CursorPosition

        $counter = 0
        while (($counter++ -lt $FaultRetries) -and (Test-Path -Path $item -ErrorAction SilentlyContinue))
        {
            $Host.UI.RawUI.CursorPosition = $cursorPos

            if ($counter -gt 1)
            {
                Start-Sleep -Milliseconds (2 * $counter)
            }

            Write-Host ("Try delete folder '{0}'... [{1:D3}] " -f $item, $counter) -NoNewline
            Write-Host $inProgress -ForegroundColor Yellow

            try
            {
                Remove-Item -Path $item -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
                $hasMatched = $true
            }
            catch
            {
            }
        }

        $Host.UI.RawUI.CursorPosition = $cursorPos
        Write-Host ("Try delete folder '{0}'... [{1:D3}] " -f $item, --$counter) -NoNewline

        if (-not (Test-Path -Path $item -ErrorAction SilentlyContinue))
        {
            Write-Host $success -ForegroundColor Green
        }
        else
        {
            Write-Host $failed -ForegroundColor Red
        }
    }
}

if ($hasMatched -and ($WaitTimeOut -gt 0))
{
    for ($i = 0; $i -lt 5; ++$i)
    {
        Write-Host '.' -NoNewline
        Start-Sleep -Milliseconds $WaitTimeOut
    }

    Write-Host
}
