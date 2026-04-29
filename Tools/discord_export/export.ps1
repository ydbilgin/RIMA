# PixelLab Discord exporter
# Usage:
#   .\export.ps1                                # default: son 180 gun
#   .\export.ps1 -DaysBack 90                   # son 90 gun
#   .\export.ps1 -DaysBack 0                    # tum tarih (dikkat: cok uzun)
#   .\export.ps1 -OutDir "..\..\_STAGING\test"  # baska klasor

param(
    [string]$DceCli = ".\dce\DiscordChatExporter.Cli.exe",
    [string]$ChannelsFile = ".\channels.txt",
    [string]$TokenFile = ".\token.txt",
    [string]$OutDir = "..\..\_STAGING\discord_pixellab",
    [int]$DaysBack = 60
)

$ErrorActionPreference = "Stop"

# Pre-flight
if (-not (Test-Path $DceCli)) {
    Write-Error "DCE bulunamadi: $DceCli`nREADME bolum 1: zip'i .\dce\ altina ac."
    exit 1
}
if (-not (Test-Path $TokenFile)) {
    Write-Error "Token yok: $TokenFile`nREADME bolum 2: tarayicidan token al, token.txt'ye yapistir."
    exit 1
}
if (-not (Test-Path $ChannelsFile)) {
    Write-Error "Channels yok: $ChannelsFile"
    exit 1
}

$token = (Get-Content $TokenFile -Raw).Trim()
if ([string]::IsNullOrWhiteSpace($token)) {
    Write-Error "token.txt bos."
    exit 1
}

# Channel listesi
$channels = @()
foreach ($line in Get-Content $ChannelsFile) {
    $trimmed = $line.Trim()
    if (-not $trimmed -or $trimmed.StartsWith("#")) { continue }
    $parts = $trimmed -split '\s+', 2
    $id = $parts[0]
    $label = if ($parts.Count -gt 1) { $parts[1].Trim() } else { $id }
    $safe = $label -replace '[^\w\-]', '_'
    $channels += [pscustomobject]@{ Id = $id; Label = $safe }
}

if ($channels.Count -eq 0) {
    Write-Error "channels.txt'de aktif kanal yok (hepsi yorumda mi?)."
    exit 1
}

# Out dir
if (-not (Test-Path $OutDir)) {
    New-Item -ItemType Directory -Path $OutDir -Force | Out-Null
}
$absOut = (Resolve-Path $OutDir).Path

# Tarih filtresi
$afterArg = @()
if ($DaysBack -gt 0) {
    $after = (Get-Date).AddDays(-$DaysBack).ToString("yyyy-MM-dd")
    $afterArg = @("--after", $after)
    Write-Host "Tarih filtresi: $after sonrasi" -ForegroundColor Cyan
} else {
    Write-Host "Tarih filtresi: YOK (tum tarih)" -ForegroundColor Yellow
}

Write-Host "Kanal sayisi: $($channels.Count)" -ForegroundColor Cyan
Write-Host "Cikis: $absOut" -ForegroundColor Cyan
Write-Host ""

$summary = @()
foreach ($ch in $channels) {
    $jsonPath = Join-Path $absOut "$($ch.Label).json"
    $mediaDir = Join-Path $absOut "$($ch.Label)_media"
    $tmpDir   = Join-Path $absOut "_tmp_$($ch.Label)"

    Write-Host "==> $($ch.Label) ($($ch.Id))" -ForegroundColor Green

    # Thread'li kanallarda DCE tek dosyaya yazamaz — gecici klasore export et, sonra birlestir
    if (Test-Path $tmpDir) { Remove-Item $tmpDir -Recurse -Force }
    New-Item -ItemType Directory -Path $tmpDir -Force | Out-Null

    $args = @(
        "export",
        "-t", $token,
        "-c", $ch.Id,
        "-f", "Json",
        "-o", (Join-Path $tmpDir "%{channel}.json"),
        "--media",
        "--media-dir", $mediaDir,
        "--reuse-media",
        "--include-threads", "All"
    ) + $afterArg

    $start = Get-Date
    & $DceCli @args
    $code = $LASTEXITCODE
    $dur = ((Get-Date) - $start).TotalSeconds

    # Gecici klasordeki tum JSON'lari tek dosyaya birlestir
    $tmpJsons = Get-ChildItem $tmpDir -Filter "*.json" -ErrorAction SilentlyContinue
    if ($tmpJsons.Count -eq 1) {
        # Tek dosya — direkt tasi
        Copy-Item $tmpJsons[0].FullName $jsonPath -Force
    } elseif ($tmpJsons.Count -gt 1) {
        # Birden fazla (kanal + thread'ler) — mesajlari birlestir
        $allMsgs = @()
        foreach ($tj in $tmpJsons) {
            try {
                $data = Get-Content $tj.FullName -Raw | ConvertFrom-Json
                if ($data.messages) { $allMsgs += $data.messages }
            } catch { Write-Host "    WARN: $($tj.Name) parse hatasi" -ForegroundColor Yellow }
        }
        # Ana kanaldan metadata al
        $mainData = Get-Content $tmpJsons[0].FullName -Raw | ConvertFrom-Json
        $mainData.messages = $allMsgs
        $mainData | ConvertTo-Json -Depth 20 -Compress | Set-Content $jsonPath -Encoding UTF8
        Write-Host "    Merged: $($tmpJsons.Count) dosya, $($allMsgs.Count) mesaj" -ForegroundColor Cyan
    }
    # Temizle
    Remove-Item $tmpDir -Recurse -Force -ErrorAction SilentlyContinue

    $summary += [pscustomobject]@{
        Channel = $ch.Label
        ExitCode = $code
        DurationSec = [math]::Round($dur, 1)
        Json = if (Test-Path $jsonPath) { (Get-Item $jsonPath).Length } else { 0 }
    }
    Write-Host ""
}

Write-Host "==== Ozet ====" -ForegroundColor Cyan
$summary | Format-Table -AutoSize
Write-Host ""
Write-Host "Cikti: $absOut"
Write-Host "Sonra: Claude'a 'Discord export hazir' de."
