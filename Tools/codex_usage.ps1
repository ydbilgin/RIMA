param(
    [switch]$Json,
    [switch]$Compact,
    [switch]$NoColor,
    [int]$WatchSeconds = 0,
    [int]$ParentPid = 0,
    [string]$StopFile = ""
)

$ErrorActionPreference = "Stop"

function Get-CodexHome {
    if ($env:CODEX_HOME -and (Test-Path -LiteralPath $env:CODEX_HOME)) {
        return $env:CODEX_HOME
    }

    return Join-Path $env:USERPROFILE ".codex"
}

function Read-CodexAuth {
    $codexHome = Get-CodexHome
    $authPath = Join-Path $codexHome "auth.json"
    if (-not (Test-Path -LiteralPath $authPath)) {
        throw "Codex auth not found at $authPath. Run: codex login"
    }

    $auth = Get-Content -LiteralPath $authPath -Raw | ConvertFrom-Json
    if (-not $auth.tokens -or -not $auth.tokens.access_token) {
        throw "Codex auth.json does not contain an access token. Run: codex logout; codex login"
    }

    if (-not $auth.tokens.account_id) {
        throw "Codex auth.json does not contain an account_id. Run: codex logout; codex login"
    }

    return $auth
}

function Convert-ResetTime {
    param([object]$UnixSeconds)

    if ($null -eq $UnixSeconds) {
        return $null
    }

    return [DateTimeOffset]::FromUnixTimeSeconds([int64]$UnixSeconds).LocalDateTime
}

function Format-Window {
    param(
        [string]$Name,
        [object]$Window
    )

    if ($null -eq $Window) {
        return "${Name}: unavailable"
    }

    $used = [int]$Window.used_percent
    $left = [Math]::Max(0, 100 - $used)
    $reset = Convert-ResetTime $Window.reset_at
    $minutes = [Math]::Ceiling(([int]$Window.reset_after_seconds) / 60)

    return ("{0}: {1}% left ({2}% used), resets {3} (~{4} min)" -f $Name, $left, $used, $reset.ToString("yyyy-MM-dd HH:mm"), $minutes)
}

function Get-WindowStats {
    param([object]$Window)

    if ($null -eq $Window) {
        return $null
    }

    $used = [Math]::Min(100, [Math]::Max(0, [int]$Window.used_percent))
    $left = [Math]::Max(0, 100 - $used)
    $reset = Convert-ResetTime $Window.reset_at
    $minutes = [Math]::Ceiling(([int]$Window.reset_after_seconds) / 60)

    [pscustomobject]@{
        Used    = $used
        Left    = $left
        Reset   = $reset
        Minutes = $minutes
    }
}

function Get-UsageColor {
    param([int]$Left)

    if ($NoColor) {
        return $null
    }

    if ($Left -le 15) {
        return "Red"
    }

    if ($Left -le 35) {
        return "Yellow"
    }

    return "Green"
}

function Write-Part {
    param(
        [string]$Text,
        [string]$Color,
        [switch]$NoNewline
    )

    $args = @{
        Object    = $Text
        NoNewline = $NoNewline
    }

    if ($Color) {
        $args.ForegroundColor = $Color
    }

    Write-Host @args
}

function Write-UsageBar {
    param(
        [string]$Label,
        [object]$Window,
        [int]$BarWidth = 26
    )

    $stats = Get-WindowStats $Window
    if ($null -eq $stats) {
        Write-Host ("{0,-6} unavailable" -f $Label)
        return
    }

    $filled = [int][Math]::Round(($stats.Left / 100.0) * $BarWidth)
    $filled = [Math]::Min($BarWidth, [Math]::Max(0, $filled))
    $empty = $BarWidth - $filled
    $color = Get-UsageColor $stats.Left
    $emptyColor = if ($NoColor) { $null } else { "DarkGray" }
    $resetText = if ($stats.Reset) { $stats.Reset.ToString("yyyy-MM-dd HH:mm") } else { "unknown" }

    Write-Part ("{0,-6} [" -f $Label) $null -NoNewline
    if ($filled -gt 0) {
        Write-Part ("#" * $filled) $color -NoNewline
    }
    if ($empty -gt 0) {
        Write-Part ("-" * $empty) $emptyColor -NoNewline
    }
    Write-Part ("] {0,3}% left / {1,3}% used | reset {2} (~{3}m)" -f $stats.Left, $stats.Used, $resetText, $stats.Minutes) $null
}

function Show-CodexUsageCompact {
    param([object]$Usage)

    $allowedColor = if ($Usage.rate_limit.allowed) { "Green" } else { "Red" }
    Write-Part "Codex Usage" "Cyan" -NoNewline
    Write-Part " | plan " $null -NoNewline
    Write-Part ([string]$Usage.plan_type) "White" -NoNewline
    Write-Part " | allowed " $null -NoNewline
    Write-Part ([string]$Usage.rate_limit.allowed) $allowedColor

    Write-UsageBar "5h" $Usage.rate_limit.primary_window
    Write-UsageBar "Week" $Usage.rate_limit.secondary_window

    if ($Usage.code_review_rate_limit) {
        Write-UsageBar "Review" $Usage.code_review_rate_limit.primary_window
    }

    if ($Usage.rate_limit_reached_type) {
        Write-Part ("Limit reached: {0}" -f $Usage.rate_limit_reached_type) "Red"
    }
}

function Get-CodexUsage {
    $auth = Read-CodexAuth
    $headers = @{
        Authorization        = "Bearer $($auth.tokens.access_token)"
        "ChatGPT-Account-ID" = $auth.tokens.account_id
        "User-Agent"         = "codex-cli"
        Accept               = "application/json"
    }

    Invoke-RestMethod `
        -Uri "https://chatgpt.com/backend-api/wham/usage" `
        -Headers $headers `
        -Method Get `
        -TimeoutSec 8
}

function Show-CodexUsageData {
    param([object]$Usage)
    if ($Json) {
        $Usage | ConvertTo-Json -Depth 12
        return
    }

    if ($Compact) {
        Show-CodexUsageCompact $Usage
        return
    }

    Write-Output "Codex Usage"
    Write-Output ("Plan: {0}" -f $Usage.plan_type)
    Write-Output ("Allowed: {0}" -f $Usage.rate_limit.allowed)
    Write-Output (Format-Window "5h limit" $Usage.rate_limit.primary_window)
    Write-Output (Format-Window "Weekly limit" $Usage.rate_limit.secondary_window)

    if ($Usage.code_review_rate_limit) {
        Write-Output (Format-Window "Code review" $Usage.code_review_rate_limit.primary_window)
    }

    if ($Usage.credits) {
        if ($Usage.credits.unlimited) {
            Write-Output "Credits: unlimited"
        } elseif ($Usage.credits.has_credits -and $null -ne $Usage.credits.balance) {
            Write-Output ("Credits: {0}" -f $Usage.credits.balance)
        }
    }

    if ($Usage.rate_limit_reached_type) {
        Write-Output ("Limit reached type: {0}" -f $Usage.rate_limit_reached_type)
    }
}

function Show-CodexUsage {
    try {
        $usage = Get-CodexUsage
    } catch {
        Write-Error "Failed to fetch Codex usage: $($_.Exception.Message)"
        return
    }

    Show-CodexUsageData $usage
}

function Test-ShouldStop {
    if ($StopFile -and (Test-Path -LiteralPath $StopFile)) {
        return $true
    }

    if ($ParentPid -gt 0 -and -not (Get-Process -Id $ParentPid -ErrorAction SilentlyContinue)) {
        return $true
    }

    return $false
}

function Move-ToTop {
    try {
        [Console]::SetCursorPosition(0, 0)
        Write-Host "`e[0J" -NoNewline
    } catch {
        Clear-Host
    }
}

if ($WatchSeconds -gt 0) {
    while (-not (Test-ShouldStop)) {
        try {
            $usage = Get-CodexUsage
            Move-ToTop
            Show-CodexUsageData $usage
        } catch {
            Move-ToTop
            Write-Part "Codex Usage" "Cyan" -NoNewline
            Write-Part (" | unavailable: {0}" -f $_.Exception.Message) "Yellow"
        }

        for ($i = 0; $i -lt $WatchSeconds; $i++) {
            if (Test-ShouldStop) {
                break
            }
            Start-Sleep -Seconds 1
        }
    }

    exit 0
}

Show-CodexUsage
