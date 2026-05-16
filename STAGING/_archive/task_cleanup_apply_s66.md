# Codex Cleanup Apply (S66)

**Tarih:** 2026-05-13
**Tip:** MECHANICAL — sil + taşı
**Effort:** medium
**Çıktı:** CODEX_DONE.md'ye summary

## Görev

rima-sonnet'in çıkardığı cleanup envanterine göre dosya operasyonları yap. Hiçbir judgment yok — sadece komutları çalıştır.

## SİL Listesi (kesin sil — içerik başka yerde, revoked pipeline)

```bash
# 1. PROMPTS_S43 klasörü — S43 dönemi skill animation contracts, 2.5D revoked pipeline
rm -rf "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/PROMPTS_S43"

# 2. _ANCHOR_QC_MASTER_S43.md — 2.5D dönemi anchor QC, klasörden gözden kaçmış
rm "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ANCHOR_QC_MASTER_S43.md"

# 3. _DISCARDED concept_art klasörü — REASON.md ile zaten DISCARDED işaretli
rm -rf "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concept_art/_DISCARDED_2026-05-10_painterly"
```

## ARŞİVLE Listesi (tarihsel değer, sil değil taşı)

Önce hedef klasörleri oluştur (yoksa):
```bash
mkdir -p "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12"
mkdir -p "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive"
```

Sonra taşı:
```bash
# 1. ANIMATION_REDESIGN.md (ARCHIVED notu var) → 2.5D archive
mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/ANIMATION_REDESIGN.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"

# 2. LAST_EPOCH_RIMA_TASARIM_NOTLARI.md → STAGING archive
mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/LAST_EPOCH_RIMA_TASARIM_NOTLARI.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

# 3. codex_revoke_cleanup_2026-05-12.md → STAGING archive
mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/codex_revoke_cleanup_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

# 4. PRERENDERED_2D_DECISION.md (REJECTED) → 2.5D archive
mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/PRERENDERED_2D_DECISION.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"
```

## Doğrulama

Operasyonlar bitince:
```bash
ls "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/PROMPTS_S43" 2>&1 | head -2  # No such file
ls "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ANCHOR_QC_MASTER_S43.md" 2>&1 | head -2
ls "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/ANIMATION_REDESIGN.md"
ls "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"
```

## Çıktı (CODEX_DONE.md'ye append)

```markdown
# S66 Cleanup Apply — TAMAMLANDI

**Tarih:** 2026-05-13
**Silinen:**
- STAGING/PROMPTS_S43/ (klasör, ~60 dosya)
- TASARIM/_ANCHOR_QC_MASTER_S43.md
- STAGING/concept_art/_DISCARDED_2026-05-10_painterly/ (klasör)

**Arşivlenen:**
- TASARIM/ANIMATION_REDESIGN.md → TASARIM/_ARCHIVE_2.5D_2026-05-12/
- STAGING/LAST_EPOCH_RIMA_TASARIM_NOTLARI.md → STAGING/_archive/
- STAGING/codex_revoke_cleanup_2026-05-12.md → STAGING/_archive/
- TASARIM/PRERENDERED_2D_DECISION.md → TASARIM/_ARCHIVE_2.5D_2026-05-12/

**Doğrulama:** ✓ Tüm operasyonlar başarılı

**Manuel review pending:** BIG_DESIGN_DECISIONS, room design, pixellab pipeline duplicate, skill audit — orchestrator değerlendirecek.
```

## Kısıtlar
- git commit YOK
- Manuel review'a girme — sadece sil + taşı
- Hata olursa belirt, hangi komut başarısız oldu
- Effort: medium
