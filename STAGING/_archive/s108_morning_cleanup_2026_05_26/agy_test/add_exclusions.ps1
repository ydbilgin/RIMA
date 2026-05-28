#!/usr/bin/env pwsh
# Defender exclusions for agy CLI test infrastructure.
# Must run ELEVATED. Writes self-report to exclusion_result.txt so the calling
# (non-elevated) process can verify success.

$ErrorActionPreference = "Continue"
$resultFile = Join-Path $PSScriptRoot "exclusion_result.txt"

$paths = @(
  'F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\agy_test',
  'F:\Antigravity Projeler\2d roguelite\RIMA\agychange.ps1',
  'C:\Users\ydbil\AppData\Local\agy\bin\agy.exe',
  'C:\Users\ydbil\.gemini\antigravity-cli'
)
$procs = @('agy.exe')
$lines = @("=== Defender Exclusion Add @ $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') ===")

foreach ($p in $paths) {
  try { Add-MpPreference -ExclusionPath $p -ErrorAction Stop; $lines += "OK   path: $p" }
  catch { $lines += "FAIL path: $p -> $($_.Exception.Message)" }
}
foreach ($pr in $procs) {
  try { Add-MpPreference -ExclusionProcess $pr -ErrorAction Stop; $lines += "OK   proc: $pr" }
  catch { $lines += "FAIL proc: $pr -> $($_.Exception.Message)" }
}

$lines += ""
$lines += "=== Current state (elevated view) ==="
try {
  $prefs = Get-MpPreference
  $lines += "ExclusionPath ($($prefs.ExclusionPath.Count) total):"
  $prefs.ExclusionPath | ForEach-Object { $lines += "  EP: $_" }
  $lines += "ExclusionProcess ($($prefs.ExclusionProcess.Count) total):"
  $prefs.ExclusionProcess | ForEach-Object { $lines += "  ER: $_" }
} catch {
  $lines += "Get-MpPreference failed: $($_.Exception.Message)"
}

$lines | Out-File -FilePath $resultFile -Encoding utf8
