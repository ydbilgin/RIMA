# CODEX TASK — Painter Paint Regression FIX

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun

Diagnose raporu: `STAGING/CODEX_DONE_painter_diagnose.md`.
Özet: `targetParent = targetTilemap.transform.parent` (Grid) atanıyor → PaintWallWithConnections doğrudan Grid altına yapıştırıyor → S95 scene Walls_Root/Props_Root organization bozuluyor → paint fail veya yanlış yere paint.

## Görev — Fix

`Assets/Editor/RimaUnifiedPainterWindow.cs`:

### Fix 1 — PeekTargetParent fallback (lines 3203-3206)
Eğer `targetParent` Grid veya Tilemap GameObject ise (yani Floor_Tilemap.transform.parent), targetParent return etme — Grid/Tilemap fallback path'i çalıştır. Şu an non-null Grid'i hemen return ediyor.

### Fix 2 — Auto-init guard (lines 752-755)
`targetParent = targetTilemap.transform.parent` atamasını conditional yap:
- Eğer parent.GetComponent<Grid>() var ise atama YAPMA
- Eğer parent.GetComponent<Tilemap>() var ise atama YAPMA
- Sadece "normal" parent (Walls_Root, Props_Root gibi empty container) atanmalı

### Fix 3 — Wall auto-connect canonical path (lines 3511-3546)
`PaintWallWithConnections()` doğrudan `GetTargetParent()` kullanıyor → `GetOrCreateGroupParent()` route'una yönlendir. Diğer prefab paint nasıl group classify ediyorsa wall da aynı yoldan geçsin.

## Allowed File Writes

- **MODIFY:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (sadece 3 fix bölgesi)

## Forbidden

- Diğer painter logic'ine dokunma
- CollisionRulesSO, sortingLayer scripts'lere dokunma (zaten doğru)
- Sahne dosyalarına dokunma

## Test

- Compile error 0 verify (`read_console`)
- IsoShowcaseRoom_S95.unity sahnesini aç, painter window'unu aç
- Wall sekmesi açık, palette'te pilot_a_wall_* görünüyor
- Bir wall paint dene, doğru parent altına gitsin (Walls_Root)

## Rapor

`STAGING/CODEX_DONE_painter_fix.md` — fix'lenen line'lar + read_console + manuel test sonucu.

## Effort

medium — 3 yer fix + test, ~30 dakika.
