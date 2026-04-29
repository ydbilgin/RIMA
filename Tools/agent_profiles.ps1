param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet("init", "codex-login", "codex", "claude-login", "claude", "status")]
    [string]$Command,

    [Parameter(Position = 1)]
    [ValidateSet("a", "b")]
    [string]$Profile = "a",

    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Args
)

$ErrorActionPreference = "Stop"

$CodexProfileRoot = Join-Path $env:USERPROFILE ".codex-profiles"
$CodexProfileMap = @{
    a = "codex-a"
    b = "codex-b"
}
$ClaudeProfileMap = @{
    a = "claude-a"
    b = "claude-b"
}

function Get-CodexHome {
    param([string]$Key)
    return Join-Path $CodexProfileRoot $CodexProfileMap[$Key]
}

function Initialize-CodexProfile {
    param([string]$Key)

    $target = Get-CodexHome $Key
    New-Item -ItemType Directory -Force -Path $target | Out-Null

    $source = Join-Path $env:USERPROFILE ".codex"
    if (Test-Path $source) {
        foreach ($name in @("config.toml", "rules", "skills", "memories")) {
            $src = Join-Path $source $name
            $dst = Join-Path $target $name
            if ((Test-Path $src) -and -not (Test-Path $dst)) {
                Copy-Item -LiteralPath $src -Destination $dst -Recurse
            }
        }
    }

    Write-Host "Codex profile ready: $target"
}

function Invoke-CodexProfile {
    param(
        [string]$Key,
        [string[]]$ForwardArgs
    )

    $env:CODEX_HOME = Get-CodexHome $Key
    New-Item -ItemType Directory -Force -Path $env:CODEX_HOME | Out-Null
    & codex @ForwardArgs
}

function Invoke-ClaudeProfile {
    param(
        [string]$Key,
        [string[]]$ForwardArgs
    )

    $profileName = $ClaudeProfileMap[$Key]
    & ccs $profileName @ForwardArgs
}

switch ($Command) {
    "init" {
        Initialize-CodexProfile "a"
        Initialize-CodexProfile "b"
        Write-Host ""
        Write-Host "Next Claude setup commands:"
        Write-Host "  .\Tools\agent_profiles.ps1 claude-login a"
        Write-Host "  .\Tools\agent_profiles.ps1 claude-login b"
        Write-Host ""
        Write-Host "Next Codex setup commands:"
        Write-Host "  .\Tools\agent_profiles.ps1 codex-login a"
        Write-Host "  .\Tools\agent_profiles.ps1 codex-login b"
    }
    "codex-login" {
        Initialize-CodexProfile $Profile
        Invoke-CodexProfile $Profile @("login")
    }
    "codex" {
        Initialize-CodexProfile $Profile
        Invoke-CodexProfile $Profile $Args
    }
    "claude-login" {
        $profileName = $ClaudeProfileMap[$Profile]
        & ccs auth create $profileName --share-context --context-group rima
    }
    "claude" {
        Invoke-ClaudeProfile $Profile $Args
    }
    "status" {
        Write-Host "Claude profiles:"
        & ccs auth list
        Write-Host ""
        foreach ($key in @("a", "b")) {
            $profileHome = Get-CodexHome $key
            Write-Host "Codex $key ($profileHome):"
            $env:CODEX_HOME = $profileHome
            & codex login status
            Write-Host ""
        }
    }
}
