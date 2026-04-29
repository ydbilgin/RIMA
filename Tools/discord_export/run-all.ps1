# PixelLab Discord -> Ollama digest pipeline (tek tus)
#
# Default: export son 60 gun + text-only digest
#
# Ornekler:
#   .\run-all.ps1                          # text-only (hizli, ~dakikalar)
#   .\run-all.ps1 -WithImages              # + image vision (yavas, saatler)
#   .\run-all.ps1 -WithImages -WithVideos  # tam pipeline (cok yavas, ffmpeg gerek)
#   .\run-all.ps1 -SkipExport              # export atla, sadece analyze tekrar
#   .\run-all.ps1 -ImageLimit 50           # kanal basina 50 image cap

param(
    [switch]$WithImages,
    [switch]$WithVideos,
    [switch]$SkipExport,
    [int]$DaysBack = 60,
    [int]$ImageLimit = 0,
    [int]$VideoLimit = 0,
    [string]$TextModel = "qwen2.5:14b",
    [string]$VisionModel = "qwen2.5vl:7b"
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

# Pre-flight
if (-not $SkipExport) {
    if (-not (Test-Path ".\dce\DiscordChatExporter.Cli.exe")) {
        Write-Error "DCE yok. README bolum 1: zip'i .\dce\ altina ac."
        exit 1
    }
    if (-not (Test-Path ".\token.txt")) {
        Write-Error "token.txt yok. README bolum 2."
        exit 1
    }
}

if (-not (Test-Path ".\channels.txt")) {
    Write-Error "channels.txt yok."
    exit 1
}

# Ollama health
try {
    $r = Invoke-WebRequest -Uri "http://localhost:11434/api/tags" -UseBasicParsing -TimeoutSec 5
    $tags = ($r.Content | ConvertFrom-Json).models.name
} catch {
    Write-Error "Ollama erisilemedi (http://localhost:11434). Baska terminalde 'ollama serve' calistir."
    exit 1
}

# Model varligi
$missing = @()
if ($tags -notcontains $TextModel) { $missing += $TextModel }
if (($WithImages -or $WithVideos) -and ($tags -notcontains $VisionModel)) { $missing += $VisionModel }
if ($missing.Count -gt 0) {
    Write-Host "Eksik Ollama modeli: $($missing -join ', ')" -ForegroundColor Yellow
    foreach ($m in $missing) {
        Write-Host "  Pulling: ollama pull $m" -ForegroundColor Cyan
        & ollama pull $m
        if ($LASTEXITCODE -ne 0) { Write-Error "ollama pull $m basarisiz"; exit 1 }
    }
}

# Python
$py = Get-Command python -ErrorAction SilentlyContinue
if (-not $py) { $py = Get-Command python3 -ErrorAction SilentlyContinue }
if (-not $py) { Write-Error "Python yok (analyze.py icin gerek)"; exit 1 }

# ffmpeg uyari
if ($WithVideos -and -not (Get-Command ffmpeg -ErrorAction SilentlyContinue)) {
    Write-Warning "ffmpeg yok. Video phase ffmpeg gerek (https://ffmpeg.org veya 'winget install Gyan.FFmpeg'). Atlanacak."
}

# Step 1: Export
if (-not $SkipExport) {
    Write-Host "`n=== Step 1: Discord Export (son $DaysBack gun) ===" -ForegroundColor Cyan
    & .\export.ps1 -DaysBack $DaysBack
    if ($LASTEXITCODE -ne 0) { Write-Error "Export basarisiz"; exit 1 }
} else {
    Write-Host "`n=== Step 1: SKIP (--SkipExport) ===" -ForegroundColor Yellow
}

# Step 2: Analyze
Write-Host "`n=== Step 2: Ollama Analyze ===" -ForegroundColor Cyan
$analyzeArgs = @("analyze.py", "--text-model", $TextModel, "--vision-model", $VisionModel)
if ($WithImages) { $analyzeArgs += "--with-images" }
if ($WithVideos) { $analyzeArgs += "--with-videos" }
if ($ImageLimit -gt 0) { $analyzeArgs += @("--image-limit", $ImageLimit) }
if ($VideoLimit -gt 0) { $analyzeArgs += @("--video-limit", $VideoLimit) }

& $py.Source @analyzeArgs
if ($LASTEXITCODE -ne 0) { Write-Error "Analyze basarisiz"; exit 1 }

Write-Host "`n=== BITTI ===" -ForegroundColor Green
$digest = Resolve-Path "..\..\_STAGING\discord_pixellab\digest" -ErrorAction SilentlyContinue
if ($digest) {
    Write-Host "Digest: $digest"
    Write-Host "Acmak icin: explorer `"$digest`""
    Write-Host "Master ozet: $digest\MASTER.md"
}
