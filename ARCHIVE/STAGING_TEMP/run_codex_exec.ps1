# Run codex exec for batch1 sign-off task using laurethgame profile
# Sets CODEX_HOME to the laurethgame profile directory so auth is used

$profileHome = "$env:USERPROFILE\.codex-profiles\laurethgame"
$env:CODEX_HOME = $profileHome

$promptFile = "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_task_wrapped.txt"
$outputFile = "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_batch1_output.txt"
$workDir = "F:\Antigravity Projeler\2d roguelite\RIMA"

Write-Host "CODEX_HOME set to: $($env:CODEX_HOME)"
Write-Host "Running codex exec with stdin from prompt file..."
Write-Host "Working dir: $workDir"

$output = Get-Content -Path $promptFile -Raw | codex exec --dangerously-bypass-approvals-and-sandbox -C $workDir - 2>&1

$exitCode = $LASTEXITCODE
Write-Host "Exit code: $exitCode"
Write-Host "Output lines: $($output.Count)"

$output | Out-File -FilePath $outputFile -Encoding UTF8
Write-Host "Output written to: $outputFile"

# Show last 100 lines
$output | Select-Object -Last 100
