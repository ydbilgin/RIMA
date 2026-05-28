---
name: active-ai-asset-qa-gate-v2-rima
description: RIMA AI Asset QA Gate v2 — 10 test çift katman (5 technical + 5 game-artist). Production'a giren her AI sprite bu gate'ten geçer. Studio STUDIO_KARAR_005 v2 RIMA adaptasyonu. PixelLab/AI Freedom çıktısı doğrudan import edilmez.
metadata:
  type: project
  source: F:/LaurethStudio/01_PIPELINE/2D/ai_asset_qa_gate.md
  origin_session: studio_s15_close_2026_05_27_transfer_to_rima
---

RIMA AI Asset QA Gate v2 — Studio'dan adapte (2026-05-27 transfer).

## Kapsam
Production'a giren her AI sprite/tile/asset 10 testi geçer. Studio QA Gate v2 ile aynı çatı, RIMA değerleri:
- **Palette referans:** cyan `#00FFCC` + violet `#5A2A8A` (Karar #98), `Art/Palettes/rima_palette.gpl` (16-color)
- **Character scale:** 120×120 (S100 Warblade verified)
- **Tile scale:** 32×32 top-down
- **VFX scale:** 64-128 mix (küçük 64-80, ultimate 96-128)
- **PPU:** 64

## Katman A — Technical (Studio orijinal 5)
1. **Seam Check** (tile only) — 3×3 grid preview, Wang corner consistency (Karar #131 16-key)
2. **Alpha Channel** — transparent edge gri halo / fringe → fail
3. **Scale Match** — character 120px referans, weapon 24-56px, tile 32px ±50% sapma fail
4. **Silhouette Consistency** — batch içinde 2+ farklı stil → fail (anchor set discipline)
5. **Palette Match** — `rima_palette.gpl` dışı pixel >%5 → fail

## Katman B — Game-Artist (Studio v2 agy ekledi)
6. **Silhouette Readability** — siyah fill (`#000000`) sonrası 0.1sn'de okunur mu, kol/bacak çamuru var mı
7. **Color Depth** — Aseprite Color Count → 16/32 sınır aşıyor mu, AI uydurma 100+ ara renk
8. **Mixel Detection** — 1×1 piksel boyut sahnede eşit mi, AI 0.5px detay üretti mi (PPU mismatch)
9. **Anti-Aliasing Cleanup** — kenarlarda yarı şeffaf "haze" + jaggies, temizlenebilir mi
10. **Pose Readability** — duruş net mi (idle/attack/walk), eklem fiziken mümkün, ağırlık merkezi var mı

İkisi de zorunlu. Katman A geçip B'yi geçmeyebilir.

## Workflow
1. PixelLab/AI çıktı → `STAGING/asset_inbox/`
2. **Palette-Lock Daemon** (`Tools/palette_lock_daemon.py`) Aseprite CLI auto-remap → 16-color force
3. Manuel QA 10 test (~3-5 dk per sprite)
4. PASS → `asset_index.json` `qa_pass: true` → Unity import
5. FAIL → `STAGING/asset_reject_reasons.md` log + re-gen veya manuel cleanup

## Mandatory (production'a girer)
- Karakter anchor pose, weapon sprite, hero prop, wall overlay brush, tile (base + variation), VFX particle (sparse exempt)

## Optional (skip OK)
- STAGING test sprite, proof-of-concept, 4-candidate selection reject'leri

## RIMA-spesifik fail pattern (S82 weapon QC dersleri)
PixelLab interpolate uzun blade için silhouette drift %20-30:
- Greatsword candidate 4/5: scimitar shape (silhouette drift)
- Greatsword 6: weird pommel loop
- Greatsword 10: spiked jagged
- Greatsword 14: wing-shape (anatomy error)

**Mitigation:** Anchor reference + tighter prompt (negative: "wings, spikes, organic curves").

## Cleanup Tools (RIMA Tools/)
- **Tier 1:** Aseprite (alpha + palette + seam best), Krita
- **Tier 2:** Python PIL (`Tools/pixel_cleanup/`), `palette_lock_daemon.py` (Aseprite CLI batch remap)

## Studio kaynak otorite
[[F:/LaurethStudio/01_PIPELINE/2D/ai_asset_qa_gate.md]] — Studio bu pattern'in kaynağı. RIMA versiyon = uygulama doc. Studio update'lerse RIMA orchestrator memory sync.

## Cross-link
[[wang-tile-build-workflow-rima]] [[reference-pixellab-production-knowledge]] [[feedback-pixellab-mcp-halt-strict]] [[project-canonical-character-roster-v2]]
