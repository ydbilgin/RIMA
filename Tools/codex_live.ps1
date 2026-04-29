param(
    [int]$RefreshSeconds = 60,
    [double]$UsagePaneSize = 0.16,
    [switch]$DryRun,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$CodexArgs
)

$ErrorActionPreference = "Stop"

function Quote-Arg {
    param([string]$Value)

    if ($Value -match '[\s";]') {
        return '"' + ($Value -replace '"', '\"') + '"'
    }

    return $Value
}

function Join-Args {
    param([string[]]$Values)

    return ($Values | ForEach-Object { Quote-Arg $_ }) -join " "
}

$rawCodexScript = Join-Path $PSScriptRoot "codex_raw.ps1"
if (-not (Test-Path -LiteralPath $rawCodexScript)) {
    throw "Missing raw Codex script: $rawCodexScript"
}

$pwsh = (Get-Command pwsh -ErrorAction Stop).Source
$rawArgs = @("-NoProfile", "-ExecutionPolicy", "Bypass", "-File", $rawCodexScript) + $CodexArgs

if ($DryRun) {
    Write-Output "Mode: plain Codex launcher (usage bar disabled)"
    Write-Output "Ignored compatibility args: RefreshSeconds=$RefreshSeconds UsagePaneSize=$UsagePaneSize"
    Write-Output "Codex: $pwsh $(Join-Args $rawArgs)"
    return
}

& $pwsh @rawArgs
exit $LASTEXITCODE
