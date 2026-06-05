ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect için modüler pixel-art asset üretim + dilimleme + wire pipeline'ının FEASIBILITY/REUSE lensi. ANALİZ ONLY — kod/asset değiştirme. Sonucu CODEX_DONE.md'ye yaz.

# Görev
READ this brief: STAGING/ASSET_PIPELINE_BRIEF_2026-06-04.md (içinde örnek sheet içeriği + in-game reality + 7 soru).

Answer from FEASIBILITY/REUSE lens, concrete (cite real paths/methods/import-settings):
1. Sheet-then-slice vs separate-gen: hangisi RIMA PPU64 pixel-art için daha temiz import verir? Unity multiple-sprite slicing mekaniği (grid vs automatic, Sprite Editor / TextureImporter spritesheet metadata), naming, ve programmatic slice mümkün mü (AssetDatabase/TextureImporter API)?
2. Import settings reçetesi: PPU 64, FilterMode.Point, compression none, alpha, 9-slice border (spriteBorder) — frame parçaları için programmatic border set edilebilir mi?
3. REUSE haritası: örnek sheet'teki hangi parça MEVCUT asset'i duplikliyor — Pack/button_9slice, card_frame_9slice, panel_frame_9slice, bar_frame_9slice, pedestal_seal, bg_seal_keep? Hangileri GERÇEKTEN yeni (framed skill-card composite, Ashen Glyph pedestal, VFX, class icons)?
4. Class icons: idle_south crop reuse mümkün mü (RectMask2D / programmatic Sprite.Create sub-rect)? "characters = PixelLab ONLY" kuralı UI-icon için ne diyor (memory feedback_imagegen_onbrand_not_realistic_s6 + project rules)?
5. Üretilen imagegen asset'i in-game pixel sprite'larla tutarlı yapma: pixel_cleanup skill (STAGING) + quantize/snap adımı var mı, nasıl uygulanır?
6. En az-friction GENERATE listesi + import pipeline adımları (registry'ye placeholder log dahil — feedback_imagegen_asset_pack_clean_cell_split).

Terse, cite paths/methods.
