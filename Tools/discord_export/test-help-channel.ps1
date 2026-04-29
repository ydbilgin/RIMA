# help-questions-support forum kanali test export
# Son 7 gun, sadece bu kanal, ayri test klasoru
# Forum = her thread ayri dosya. DCE klasor cikisi + sonra merge.

$ErrorActionPreference = "Stop"

$token = ([IO.File]::ReadAllText((Resolve-Path .\token.txt))).Trim()
$channelId = "1115580783663521862"
$testDir = "..\..\_STAGING\discord_pixellab_test"

# Threads klasoru (her thread ayri JSON) + merged final dosya
$threadsDir = Join-Path $testDir "help-questions-support_threads"
New-Item -ItemType Directory -Path $threadsDir -Force | Out-Null
$absThreads = (Resolve-Path $threadsDir).Path
$absTest = (Resolve-Path $testDir).Path

$mediaDir = Join-Path $absTest "help-questions-support_media"
$mergedPath = Join-Path $absTest "help-questions-support.json"
$after = (Get-Date).AddDays(-7).ToString("yyyy-MM-dd")

# DCE template: her thread %C (channel/thread name) ile adlanir
$outTemplate = Join-Path $absThreads "%C.json"

Write-Host "Test export basliyor..." -ForegroundColor Cyan
Write-Host "Kanal: help-questions-support ($channelId)"
Write-Host "Tarih: $after sonrasi (son 7 gun)"
Write-Host "Threads cikti: $absThreads"
Write-Host ""

.\dce\DiscordChatExporter.Cli.exe export `
  -t $token `
  -c $channelId `
  -f Json `
  -o $outTemplate `
  --media `
  --media-dir $mediaDir `
  --reuse-media `
  --include-threads All `
  --after $after

Write-Host ""
Write-Host "==== EXPORT BITTI, MERGE ====" -ForegroundColor Cyan

# Tum thread JSON'larini birlestir
$threadFiles = Get-ChildItem -Path $absThreads -Filter "*.json"
Write-Host "Bulunan thread JSON: $($threadFiles.Count)"

if ($threadFiles.Count -eq 0) {
    Write-Host "HATA: thread dosyasi yok" -ForegroundColor Red
    exit 1
}

$allMessages = @()
$firstGuild = $null
$firstChannel = $null

foreach ($tf in $threadFiles) {
    $raw = [IO.File]::ReadAllText($tf.FullName)
    $data = $raw | ConvertFrom-Json
    if (-not $firstGuild) { $firstGuild = $data.guild }
    if (-not $firstChannel) { $firstChannel = $data.channel }
    if ($data.messages) {
        $allMessages += $data.messages
    }
}

# Birlesik JSON
$merged = [PSCustomObject]@{
    guild = $firstGuild
    channel = $firstChannel
    messages = $allMessages
    messageCount = $allMessages.Count
}

$merged | ConvertTo-Json -Depth 100 | Out-File -FilePath $mergedPath -Encoding utf8

Write-Host ""
Write-Host "==== SONUC ====" -ForegroundColor Green
$mergedSize = [math]::Round((Get-Item $mergedPath).Length / 1KB, 1)
Write-Host "Thread sayisi : $($threadFiles.Count)" -ForegroundColor Yellow
Write-Host "Toplam mesaj  : $($allMessages.Count)" -ForegroundColor Yellow
Write-Host "Merged JSON   : $mergedPath ($mergedSize KB)"
Write-Host ""

if ($allMessages.Count -eq 0) {
    Write-Host "UYARI: 0 mesaj." -ForegroundColor Red
} else {
    Write-Host "OK: Forum kanali calisiyor. Tam exporta gec." -ForegroundColor Green
}
