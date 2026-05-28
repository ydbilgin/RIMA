# Codex Task — Image Gen: Task B v2 Pack 5-6 Dungeon Composition

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NO PIXELLAB GEN.** Codex imagegen (gpt-image-1) only. Concept reference, not asset.

---

## Amaç

User locks (2026-05-22):
- Asset pack v2 GO (34 entry, wall + floor focused)
- Wall numbered list 4 item/batch method confirmed
- **Task B dispatch:** v2 sheet ile 5-6 farklı dungeon tipi compose, "sonsuz oda v2 ile" görsel kanıt + wall 4-yön coverage uygulaması

---

## Output

`STAGING/concepts/v2_dungeons_compose_v1.png` (5-6 panel large)

After generation: `STAGING/concepts/v2_dungeons_compose_v1_NOTES.md`

---

## Reference materials (mandatory)

1. **`STAGING/concepts/asset_pack_sheet_v2.png`** — 34-entry v2 inventory. Bu task'ta TÜM sprite'lar v2 sheet'inden gelecek.
2. **`STAGING/asset_pack_v2_proposal.md`** — v2 inventory tablo + dungeon type combo formulas (Section 5)
3. **`STAGING/concepts/chatgpt ref/*.png`** (8 image) — atmosphere quality target, Hades-iso ~70-75° tilt
4. **`STAGING/concepts/multi_room_compose_v1.png`** + **`rima_rooms_with_characters_v1.png`** — earlier compose style baseline
5. RIMA canonical character roster: Warblade (greatsword), Ronin (katana + sheath on body), Ranger (bow + ivory hair) — placement optional per panel

---

## Hedef — 6-panel dungeon variety

Layout: 2 row × 3 column (6 panel) OR vertical stack — Codex'in tercih ettiği komp.

### Panel 1 — Combat Hall (Broken Slab Hall)
- Hades-iso ~70-75°, geniş atmospheric
- Assets per v2: F01/F02/F04/F07 + W01-W09 + P01/P02/P04/P05 + D02/D03/D04
- Warblade center, 3 enemy spread (close/mid/far depth cue)
- Wall 4-yön coverage demonstrate: N native + S native (if visible foreground) + W + E flipX
- Lighting: warm torches + cyan rift accent

### Panel 2 — Ritual Chamber
- Hades-iso ~70-75°, dramatic center focus
- Assets per v2: F06 (ritual radial) + F05 + H02 (altar pillar) + W01/W05/W07 + P05 + D05 (glyph)
- Central altar with cyan crystal glow, radial floor pattern
- Optional: 1-2 figure circling altar (player + companion or solo + ritual scene)
- Lighting: dominant cyan center, dim warm corners

### Panel 3 — Narrow Corridor (Hairline Rift)
- Hades-iso ~70-75°, vertical tight composition
- Assets per v2: F03 (walkway) + W03/W04 (W+E flipX) + W05-W08 corners + P04 + D02/D04
- Ranger solo walking north
- Torch rhythm side-by-side
- Cyan rift accent at far end (next room hint)

### Panel 4 — Boss Arena (Rift Court)
- Hades-iso ~70-75°, wide dramatic
- Assets per v2: F08 (polished) + F05 + F07 (blood) + W01/W02/W05-W08 + P01/P05 + H03 (throne dais)
- Boss silhouette on throne dais (large 256-384px proxy)
- Warblade + Ronin team approach from south entry
- Lighting: dominant cyan boss core + 2 warm brazier flanking

### Panel 5 — Treasure Vault
- Hades-iso ~70-75°, intimate small room
- Assets per v2: F08 (polished) + F01 + W01/W05-W08 + P06/P07 + P05 (brazier) + D04
- Chest/treasure at center (proxy, may use P05 brazier as wealth glow source)
- Warm gold-like lighting (warm brazier emphasis)
- Solo player optional or no character

### Panel 6 — Crypt Corridor (mossy/abandoned)
- Hades-iso ~70-75°, narrow with archway end
- Assets per v2: F09 (mossy overgrowth) + F03 + F04 + W03/W04 + W10 (archway) + P06/P07 + D01 (moss decal)
- Atmospheric: dim, mossy floor, archway portal at end
- Dust accumulation, organic decay feel
- Optional 1 enemy or empty atmospheric

---

## Style — overall multi-panel

- Each panel concept-art quality
- **CRITICAL:** Use ONLY v2 asset pack sheet sprites (consistent across all 6 panels)
- Hades-iso ~70-75° tilt — **stronger emphasis** than earlier mid-tilt mistake (Step 2 görseli mid-tilt 75-85 çıkmıştı, bu task lock 70-75)
- Wall 4-yön coverage visible per panel where applicable
- 4-yön coverage demonstrate: at least 2 panel (örn 1 Combat + 4 Boss) S wall AND N wall görsel olarak ayırt edilebilir, flipY YASAK Hades-iso lock kanıtı
- Pixel art aesthetic with painterly polish
- Dark unified background separating panels
- Small label/title at top of each panel

### Size
- Önemsiz (test reference). 1536×1024 OR 2048×1536. Native res preferred over upscale.

---

## Negative directives

- NO new asset invention — only v2 sheet sprites
- NO mid-tilt drift — Hades-iso ~70-75° LOCK, not 80-85°
- NO pure top-down (90°)
- NO single-scene composition (must be 5-6 distinct panels)
- NO bright cartoon palette
- NO UI elements (except subtle enemy HP bars OK)

---

## After image gen — NOTES.md

`STAGING/concepts/v2_dungeons_compose_v1_NOTES.md`:

### Per-panel v2 asset usage
| Panel | Wall coverage (N/S/W/E/corners visible) | Floor materials used (F01-F09) | Hero used (H01-H03) | Props (P01-P07) | Decals (D01-D05) |
|---|---|---|---|---|---|

### Hades-iso tilt verification
- Per panel tilt estimate (target ~70-75°)
- Any panel drifted to 80-85° mid-tilt? Note for re-gen if so

### Modular reuse verification (CRITICAL)
- Same sprite visible in multiple panels? Comprehensive list
- Same wall sprite + flipX correctly shows as opposite-direction wall in different panel? PASS/FAIL
- Floor F06 ritual radial unique to Panel 2? F09 mossy unique to Panel 6? Or accidentally reused inappropriately?

### Hero usage check
- H02 ritual altar (Panel 2): visible focal point? PASS/TWEAK/FAIL
- H03 throne dais (Panel 4 boss): visible dramatic anchor? PASS/TWEAK/FAIL
- H01 archway (Panel 6 crypt end): visible portal hint? PASS/TWEAK/FAIL

### vs ChatGPT_TOPDOWN reference quality
- Atmosphere match: AT/NEAR/BELOW
- Polish match: AT/NEAR/BELOW
- Hades-iso tilt accuracy: PASS/TWEAK/FAIL

### Recommendation
- v2 pack ile 6 dungeon tipi modular kanıt READABLE? YES/NO
- v2 ile gerçek PixelLab production'a geçilebilir mi? YES/NO/REGEN-X

---

## Tool

Codex imagegen skill (gpt-image-1 force). Use v2 sheet PNG as init_image/reference_image if supported.

Save to: `STAGING/concepts/v2_dungeons_compose_v1.png`

---

## Commit

```
[Codex] [S98 IMAGEGEN] Task B v2 dungeons compose — 6 panel modular proof

- Codex imagegen 6-panel concept (Combat/Ritual/Corridor/Boss/Treasure/Crypt)
- Uses v2 asset pack (34 sprite, wall 4-yön coverage)
- Hades-iso ~70-75° tilt lock (stronger than Step 2 mid-tilt)
- v2 modular reuse evidence + dungeon type variety
- PNG: STAGING/concepts/v2_dungeons_compose_v1.png
```

Wall clock: ~15-25 min.
