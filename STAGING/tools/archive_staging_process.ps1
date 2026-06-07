# archive_staging_process.ps1 — STAGING üst-seviye süreç artifact'lerini _process/<YYYY-MM>/ altına süpürür.
# Konvansiyon: .claude/PROJECT_RULES.md "Süreç Artifact Konvansiyonu" (2026-06-07).
# GÜVENLİ: silme yok, sadece Move-Item; whitelist-desenli; idempotent; klasörlere dokunmaz.

$ErrorActionPreference = "Stop"
$staging = Split-Path $PSScriptRoot -Parent   # .../STAGING
$month   = Get-Date -Format "yyyy-MM"
$dest    = Join-Path $staging "_process\$month"

# Taşınacak desenler (SADECE üst-seviye dosyalar)
$patterns = @(
    "_council_*.md", "cx_task_*.md", "_task_*.md",
    "_done_*.md", "_review_*.md", "_research_*.md",
    "_nlm_*.md", "_nlm_*.json", "_ax_*.md",
    "RESEARCH_*.md", "QUEUE_*.md", "AGY_*.md", "agy_*.md",
    "CX_*.md", "codex_*.md"
)
# Dokunulmayacak istisnalar (aktif/canlı dosyalar buraya eklenir)
$keep = @(
    "TASK_portalpack_production_2026-06-07.md"
)

$moved = @{}
foreach ($pat in $patterns) {
    Get-ChildItem -Path $staging -Filter $pat -File -ErrorAction SilentlyContinue | ForEach-Object {
        if ($keep -contains $_.Name) { return }
        if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Force $dest | Out-Null }
        Move-Item $_.FullName (Join-Path $dest $_.Name)
        $moved[$pat] = ($moved[$pat] ?? 0) + 1
    }
}

if ($moved.Count -eq 0) {
    Write-Host "Temiz — taşınacak süreç artifact'i yok."
} else {
    $total = ($moved.Values | Measure-Object -Sum).Sum
    Write-Host "Taşınan: $total dosya → $dest"
    $moved.GetEnumerator() | Sort-Object Value -Descending | ForEach-Object { Write-Host ("  {0,4}  {1}" -f $_.Value, $_.Key) }
    Write-Host "`nHatırlatma: git add + commit gerekir (dosyalar tracked, rename olarak algılanır)."
}
