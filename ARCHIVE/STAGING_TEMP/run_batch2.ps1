$ErrorActionPreference = "Stop"

# Set CODEX_HOME for yasinderyabilgin profile
$env:CODEX_HOME = "$env:USERPROFILE\.codex-profiles\yasinderyabilgin"

$npmRoot = Join-Path $env:APPDATA "npm"
$codexBin = Join-Path $npmRoot "node_modules\@openai\codex\bin\codex.js"
$node = Join-Path $npmRoot "node.exe"
if (-not (Test-Path $node)) { $node = "node.exe" }

$taskFile = "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_task_batch2.txt"
$outputFile = "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_batch2_output.txt"

New-Item -ItemType Directory -Force -Path $env:CODEX_HOME | Out-Null

$nodeStr = if ($node -match " ") { "`"$node`"" } else { $node }
$binStr = if ($codexBin -match " ") { "`"$codexBin`"" } else { $codexBin }

cmd /c "$nodeStr $binStr exec - -s danger-full-access -C `"F:\Antigravity Projeler\2d roguelite\RIMA`" < `"$taskFile`"" 2>&1 | Out-File -FilePath $outputFile -Encoding utf8
Write-Host "EXIT: $LASTEXITCODE"
