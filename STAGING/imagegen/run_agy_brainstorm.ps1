# Dispatch the modular-pipeline visual-decomposition brainstorm to agy (Gemini 3.1 Pro High)
$ErrorActionPreference = 'Continue'
$rima = 'F:\Antigravity Projeler\2d roguelite\RIMA'
$log  = Join-Path $rima 'STAGING\imagegen\agy_brainstorm_log.txt'
$task = Join-Path $rima 'STAGING\agy_task_modular_pipeline_brainstorm.md'
$settings = 'C:\Users\ydbil\.gemini\antigravity-cli\settings.json'

"AGY BRAINSTORM START $(Get-Date -Format o)" | Set-Content $log -Encoding UTF8

# Set model to Gemini 3.1 Pro (High) for a strong analysis; remember old value to restore
$old = $null
try {
  $json = Get-Content $settings -Raw | ConvertFrom-Json
  $old = $json.model
  $json.model = 'Gemini 3.1 Pro (High)'
  $json | ConvertTo-Json -Depth 20 | Set-Content $settings -Encoding UTF8
  Add-Content $log "model: $old -> Gemini 3.1 Pro (High)"
} catch { Add-Content $log "model set FAILED: $_" }

# Dispatch (round-robin + failover; long analysis -> generous timeout)
& ax dispatch --task-file $task --print-timeout 360 *>> $log 2>&1

# Restore model
if ($old) {
  try {
    $json = Get-Content $settings -Raw | ConvertFrom-Json
    $json.model = $old
    $json | ConvertTo-Json -Depth 20 | Set-Content $settings -Encoding UTF8
    Add-Content $log "model restored -> $old"
  } catch { Add-Content $log "model restore FAILED: $_" }
}

$out = Join-Path $rima 'STAGING\MODULAR_PIPELINE_AGY.md'
if (Test-Path $out) { Add-Content $log "OK -> $out" } else { Add-Content $log "MISSING -> $out" }
Add-Content $log "AGY BRAINSTORM DONE $(Get-Date -Format o)"
