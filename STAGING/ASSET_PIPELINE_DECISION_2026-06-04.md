# DECISION — CharacterSelect Asset Pipeline
Date: 2026-06-04 · Council: cx (feasibility+NLM canon) + ax-3.1-Pro (art) + ax-3.5-Flash (lean) → Opus
Brief: STAGING/ASSET_PIPELINE_BRIEF_2026-06-04.md

## ONE LINE
Build CharSelect with REUSE first (idle_south sprites + existing Pack frames/pedestal/bg). Generate ONLY a few premium "atmospheric" pieces (abstract skill glyphs + VFX, optional pedestal/backdrop variant) and drop them in LATER. Class roster avatars = our REAL canonical sprites, NOT generated.

## UNANIMOUS (cx NLM-backed + both ax)
- **Class icons → REUSE canonical `idle_south` sprites** (all 10 classes have them, PPU64). NOT generated. NLM canon: "characters/portraits = PixelLab ONLY; imagegen portraits = rejected as realistic/off-style." Live code already does this (`CharacterSelectScreen.LoadCanonicalSprite` → `RimaUITheme.AnchorPath`).
- **Structural UI → REUSE existing Pack** (these DUPLICATE example-sheet pieces): button_9slice (border 16), card_frame_9slice (28), panel_frame_9slice (32), bar_frame_9slice (12/8/12/8), pedestal_seal, bg_seal_keep. Regenerating 9-slice chrome = zero value + slicing/border overhead.
- **Ship order:** wire CharSelect RAW with reuse → operational first → drop-in generated glyphs/pedestal later. Movement/juice hides average assets; don't block on art.

## RESOLVED: sheet vs separate (cx settled 3.1-vs-3.5)
- **SEPARATE transparent PNGs** for the mixed UI pieces (different sizes/borders/pivots/alpha → a single mixed sheet = slicing pain + bleed).
- **SHEET-then-slice ONLY for uniform-grid assets** (VFX frames, icon packs — equal cells). Precedents: `BrushAtlasImporter` (programmatic Multiple sprite import), `RimaUITheme.PassiveIcon` (Sprite.Create sub-rects), `AnimationImportHelper` (ISpriteEditorDataProvider).
- → Get both: cohesion via locked palette/prompt, clean imports via separates.

## GENERATE LIST (reuse-first, premium-only)
1. **Skill glyphs** — 3–10 standalone transparent abstract glyphs (96–128px), NO frame/text (framed by reused card_frame_9slice). Abstract = OK per NLM (sword/rune/portal silhouette OK; chibi portrait NOT).
2. **VFX sprites** — uniform grid sheet (4×4 / 8-cell, 64–96px): cyan sparkle/swoosh/lightning + orange embers.
3. *(optional)* Ashen-Glyph pedestal variant — only if visibly better than `pedestal_seal`; NAMED variant, not a replacement.
4. *(optional)* CharSelect backdrop variant — only as a deliberate upgrade over `bg_seal_keep`.
5. **DO NOT generate:** buttons, card/panel frames, resource bars, class chibi portraits.

## PRODUCTION PIPELINE (imagegen → in-game-consistent)
1. Raw PNGs → `STAGING/imagegen/character_select_raw/`.
2. `Tools/pixel_cleanup/pixel_cleanup.py` report → then `--remove_stray --snap_to_palette --fix_color_noise --apply_cleanup` (palette `palette_examples/rima_shattered_keep.json`; `--keep_alpha` for glow VFX). Optional `Tools/palette_lock_daemon.py` + `Art/Palettes/rima_palette.gpl` for strict lock.
3. Import: textureType Sprite, **PPU 64**, FilterMode.Point, no mipmap, Uncompressed, alphaIsTransparency, FullRect; pivot center for UI / bottom-center for props. 9-slice via `importer.spriteBorder`. UI `Image.type = Sliced`.
4. Manifest log (per feedback_imagegen_asset_pack_clean_cell_split): `STAGING/imagegen/character_select/PLACEMENT_MANIFEST.md` (raw/clean/Resources path, cell names, PPU, border, pivot, owner screen).
5. Wire by Resources.Load in RimaUITheme / CharacterSelectScreen. Keep text/layout/bars/identity/selection/9-slice scaling RUNTIME.

## ART-DIRECTION (3.1 Pro) — imagegen prompt discipline
Palette lock hex: void-purple #3A1A4A, rift-cyan #00FFCC, abyssal-stone #1A1A24, pale-parchment #E8DCC8. 1px hard outlines, dithered/block shading, NO anti-alias/soft gradients. Prompt core: "strict 16-bit pixel art, no anti-aliasing, flat colors, dithered shading, [palette], pure black bg for transparency keying." Mandatory post: quantize to palette + hard-clip alpha (0/100%).

## INTERPRETATION OF "characters = PixelLab ONLY" (for the icon question)
- CHARACTER sprites + portraits → **PixelLab ONLY**.
- ABSTRACT UI symbols (skill glyphs, runes, weapon silhouettes) → **imagegen on-brand OK** (cheaper, gating-free) + pixel_cleanup. Not required to be PixelLab.
