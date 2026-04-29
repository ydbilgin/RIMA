param(
    [string]$Version = "1.24.10921.0"
)

$ErrorActionPreference = "Stop"

$asset = "Microsoft.WindowsTerminal_${Version}_x64.zip"
$url = "https://github.com/microsoft/terminal/releases/download/v$Version/$asset"
$base = Join-Path $env:LOCALAPPDATA "Programs\WindowsTerminalPortable"
$zip = Join-Path $env:TEMP $asset

New-Item -ItemType Directory -Path $base -Force | Out-Null

Write-Output "Downloading $url"
Invoke-WebRequest -Uri $url -OutFile $zip -UseBasicParsing -TimeoutSec 180

Write-Output "Extracting to $base"
Expand-Archive -LiteralPath $zip -DestinationPath $base -Force

$terminal = Get-ChildItem -LiteralPath $base -Recurse -File -Filter WindowsTerminal.exe |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if (-not $terminal) {
    throw "WindowsTerminal.exe was not found after extraction."
}

Write-Output "Installed portable Windows Terminal:"
Write-Output $terminal.FullName
