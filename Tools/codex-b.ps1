param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Args
)

$ErrorActionPreference = "Stop"
$env:CODEX_HOME = Join-Path $env:USERPROFILE ".codex-profiles\codex-b"
New-Item -ItemType Directory -Force -Path $env:CODEX_HOME | Out-Null

if ($Args.Count -gt 0 -and $Args[0] -eq "login") {
    & codex login
} else {
    & codex @Args
}
