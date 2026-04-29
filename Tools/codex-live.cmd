@echo off
setlocal
pwsh -NoProfile -ExecutionPolicy Bypass -File "%~dp0codex_live.ps1" %*
