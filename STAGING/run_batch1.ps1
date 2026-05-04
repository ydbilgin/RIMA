$ErrorActionPreference = "Stop"

# Set CODEX_HOME for laurethgame profile
$env:CODEX_HOME = "$env:USERPROFILE\.codex-profiles\laurethgame"

# Sync statusline config (copy from codex_profile.ps1 logic)
$configPath = Join-Path $env:CODEX_HOME "config.toml"
$statusLine = 'status_line = ["model-with-reasoning", "context-remaining", "context-used", "five-hour-limit", "weekly-limit", "git-branch", "current-dir"]'
# Minimal sync -- just ensure config exists
New-Item -ItemType Directory -Force -Path $env:CODEX_HOME | Out-Null

$npmRoot = Join-Path $env:APPDATA "npm"
$codexBin = Join-Path $npmRoot "node_modules\@openai\codex\bin\codex.js"
$node = Join-Path $npmRoot "node.exe"
if (-not (Test-Path $node)) { $node = "node.exe" }

$taskFile = "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_task_batch1.txt"
$outputFile = "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_batch1_output.txt"

# Run codex exec with stdin from file, output to file
# Use cmd.exe for reliable stdin redirect
$nodeStr = if ($node -match " ") { "`"$node`"" } else { $node }
$binStr = if ($codexBin -match " ") { "`"$codexBin`"" } else { $codexBin }

cmd /c "$nodeStr $binStr exec - -s danger-full-access -C `"F:\Antigravity Projeler\2d roguelite\RIMA`" < `"$taskFile`"" 2>&1 | Out-File -FilePath $outputFile -Encoding utf8
Write-Host "EXIT: $LASTEXITCODE"
