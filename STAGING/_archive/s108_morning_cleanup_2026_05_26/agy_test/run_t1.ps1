#!/usr/bin/env pwsh
# S106 prep — test 1: agy --print with TERM=xterm-256color env override (untried in S105)
# Goal: see if setting a real terminal type defeats the altScreen=true hang.
# Safety: Hidden launch, hard 75s WaitForExit, kill on timeout, output redirected to files.

$ErrorActionPreference = "Continue"
$agy     = "C:\Users\ydbil\AppData\Local\agy\bin\agy.exe"
$testDir = $PSScriptRoot
$out     = Join-Path $testDir "out_t1.txt"
$err     = Join-Path $testDir "err_t1.txt"
$logFile = Join-Path $testDir "agy_t1.log"
Remove-Item $out, $err, $logFile -ErrorAction SilentlyContinue

$prompt = @"
You are running in print mode. Reply with EXACTLY three short lines and nothing else.
Line1: AGY_ONLINE
Line2: MCP_SERVERS=<comma-separated names of MCP servers you have access to, or NONE>
Line3: UNITY_CHECK=<if UnityMCP is in your MCP list, call its read_console tool with last 1 entry; reply CONNECTED if the call returned data, NOT_CONNECTED if the call failed/errored, ABSENT if UnityMCP is not in your tool list>
"@

# Critical env: S105 log showed `TERM="" altScreen=true` -> hang. Force a real terminal type.
$env:TERM      = "xterm-256color"
$env:COLORTERM = "truecolor"

$agyArgs = @(
  "--print", $prompt,
  "--dangerously-skip-permissions",
  "--print-timeout", "60s",
  "--log-file", $logFile
)

Write-Host "Launching agy hidden (TERM=$env:TERM)..."
$start = Get-Date
$proc = Start-Process -FilePath $agy -ArgumentList $agyArgs `
  -WindowStyle Hidden -PassThru `
  -RedirectStandardOutput $out -RedirectStandardError $err
Write-Host "PID=$($proc.Id) waiting up to 75s..."
$exited = $proc.WaitForExit(75000)
$dur = [int]((Get-Date) - $start).TotalSeconds

if (-not $exited) {
  Write-Host "STATUS: HANG after ${dur}s, killing PID $($proc.Id)"
  try { $proc.Kill($true) } catch {}
  Start-Sleep -Milliseconds 500
} else {
  Write-Host "STATUS: EXITED in ${dur}s, exit code $($proc.ExitCode)"
}

$outLen = if (Test-Path $out)     { (Get-Item $out).Length }     else { 0 }
$errLen = if (Test-Path $err)     { (Get-Item $err).Length }     else { 0 }
$logLen = if (Test-Path $logFile) { (Get-Item $logFile).Length } else { 0 }
Write-Host "stdout=${outLen}B  stderr=${errLen}B  log=${logLen}B"

Write-Host "`n--- STDOUT ---"
if ($outLen -gt 0) { Get-Content $out -Raw } else { Write-Host "(empty)" }

Write-Host "`n--- STDERR ---"
if ($errLen -gt 0) { Get-Content $err -Raw } else { Write-Host "(empty)" }

Write-Host "`n--- LOG TAIL (last 40 lines) ---"
if ($logLen -gt 0) { Get-Content $logFile -Tail 40 } else { Write-Host "(empty)" }
