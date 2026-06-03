<#
nlm_relogin.ps1 — one-command fix for the NotebookLM "Authentication expired" loop.

ROOT CAUSE (diagnosed S6, 2026-05-30): NotebookLM session cookies live only ~10h. When they
expire, `nlm login` finds the STALE profiles/<profile>/cookies.json, tries to validate it, fails,
and prints "Authentication expired" instead of re-opening Chrome -> infinite loop. `nlm login --clear`
does NOT fix it (it wipes chrome-profiles but leaves the stale cookies.json that causes the choke).

THIS SCRIPT: deletes ONLY the stale NLM cookie profile (profiles/<profile>) and KEEPS
chrome-profiles/<profile> (your persistent Google session) — so the follow-up `nlm login` is a
SILENT re-auth if your Google session is still alive, or a normal one-time sign-in if not.

USAGE (run it yourself in your terminal so Chrome can open):
  ! pwsh -File "F:\Antigravity Projeler\2d roguelite\RIMA\nlm_relogin.ps1"
Optionally a non-default profile:
  ! pwsh -File ".\nlm_relogin.ps1" -ProfileName default
#>
param([string]$ProfileName = "default")

$dir  = Join-Path $env:USERPROFILE ".notebooklm-mcp-cli"
$pdir = Join-Path $dir "profiles\$ProfileName"

if (Test-Path $pdir) {
    $stamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $bak   = Join-Path $dir "profiles\_stale_${ProfileName}_$stamp"
    Move-Item -LiteralPath $pdir -Destination $bak
    Write-Host "Cleared stale NLM cookies for '$ProfileName'  ->  $bak" -ForegroundColor Yellow
} else {
    Write-Host "No existing profile '$ProfileName' (already clean)." -ForegroundColor DarkGray
}

# Resolve the nlm command (direct on PATH, else via uvx).
$nlm = Get-Command nlm -ErrorAction SilentlyContinue
Write-Host "`nOpening Chrome for re-auth..." -ForegroundColor Cyan
if ($nlm) {
    & nlm login --profile $ProfileName
} else {
    & uvx --from notebooklm-mcp-cli nlm login --profile $ProfileName
}

Write-Host "`nVerifying..." -ForegroundColor Cyan
if ($nlm) { & nlm login --check --profile $ProfileName }
else      { & uvx --from notebooklm-mcp-cli nlm login --check --profile $ProfileName }
