# RIMA — Menu / UI Image-Gen Spec (S6)

> Replace the current Python-drawn placeholders under `Assets/Resources/UI/RIMA/` with **hand-authored
> mythic pixel art** via Codex `$imagegen` (gpt-image-2 for full-screens/hero; `generate2dsprite` magenta
> sheet + chroma-bbox for the small discrete pack — see `IMAGEGEN_ASSET_PACK_PLAN_S6.md` Path A / Path B).
> This doc gives, per asset: target file, exact canvas px, a ready-to-paste prompt, and a transparency note.

## Locked art direction (NLM canon — paste once, applies to all)
Dark mythic pixel art UI. **Dark slate stone + tarnished gold `#F2BC3D` frames** (gold = reward/structure
accent, sparing). **Pale cyan spirit light `#00FFCC` as a MINORITY accent** (rift seal energy, never floods the
frame). Deep-purple `#3A1A4A` → black void backdrop. **Subtle red/cyan rift cracks** bleeding faint light.
Premium roguelite, somber-mythic. This is **NOT an RPG inventory grid** — no hard rectangular borders, no
beveled-button kit; frames are eroded fractured-stone/iron that pulse and fade. Combat HUD stays **quiet**;
rich ornamentation lives in **overlays** (menu / select / death / victory / draft cards).

**Common style suffix (append to EVERY prompt):**
`, 2D pixel art, limited palette, dark slate-grey stone + tarnished gold #F2BC3D frame accents, deep-purple #3A1A4A to black void, pale cyan #00FFCC spirit-rift light as a minority accent only, subtle red/cyan rift cracks, somber mythic premium roguelite mood, NOT an RPG inventory grid, crisp pixels, PPU64 scale, no text, no watermark`

**Import settings (all):** Texture Type Sprite, Filter = **Point**, Compression = None, **PPU 64**, no mipmaps.
Full-screen bg = no 9-slice; stone frames/pills/cards = 9-slice border.

---

## A — Full-screen overlays (Path A: gpt-image-2, image-to-image off the placeholder; 1280×720 master → 640×360 ½)

| Target file (`Resources/UI/RIMA/`) | Canvas px | `$imagegen` prompt (+append common suffix) | Transparency / chroma |
|---|---|---|---|
| `menu_bg_island.png` ✅ **EXISTS (286KB, real)** | 640×360 | "A lone fractured floating stone keep-fragment suspended in a vast dark abyss, tarnished-gold rune banding along a ruined parapet, pale cyan rift cracks bleeding faint light down the torn cliff edges, one small warm ember brazier, slow drifting dust, cinematic low-angle hero shot, somber lonely mythic mood" | Opaque RGBA, full-bleed, no alpha |
| `char_select_bg.png` 🆕 **NEW — generate (currently flat `RimaUITheme.BackgroundDark`)** | 640×360 | "Interior of a ruined floating keep hall used as a class-selection chamber, three faint gold-runed stone alcoves/plinths receding into darkness, pale cyan spirit-light pooling on a cracked slate floor, dust motes, symmetrical balanced composition leaving the center-and-thirds clear for class cards, somber mythic vault" | Opaque RGBA, leave thirds visually quiet for card overlay |
| `death_overlay.png` ✅ **EXISTS (31KB, real)** | 640×360 | "near-black full-screen vignette, faint pale cyan #00FFCC embers rising from below, transparent clear center fading to dark fractured-stone edges, faint red rift hairline cracks, mournful" | **Transparent center**, alpha fade to opaque-dark edges (additive over scene) |
| `victory_bg_bloom.png` ✅ **EXISTS (209KB, real)** | 640×360 | "the floating seal-keep island seen from afar across the void, an amplified radiant pale cyan #00FFCC rift bloom answering briefly, tarnished-gold rune light catching the parapet, triumphant-but-somber, the seal momentarily holding" | Opaque RGBA, full-bleed |
| `lowhp_vignette.png` ⛔ **KEEP PYTHON** (procedural radial, S6 spec) | 640×360 | (do NOT regen — flat red radial; Python placeholder is correct & cheap. Listed for completeness.) | Transparent center → red edges; **stays Python** |

> Note on `RIMA_MenuDungeonBackground.png`: separate legacy menu-bg sprite referenced by `RimaUITheme`; out
> of this pass's scope (main menu uses `menu_bg_island`). Do not touch unless a wiring batch repaths it.

---

## B — Hero / glyph (Path A: gpt-image-2, transparent)

| Target file | Canvas px | `$imagegen` prompt (+append common suffix) | Transparency / chroma |
|---|---|---|---|
| `logo_rima_glyph.png` ✅ **EXISTS (34KB, real)** | 256×96 | "RIMA wordmark logo, eroded engraved slate-stone letters with a thin tarnished-gold inlay, a single pale cyan #00FFCC rift fracture splitting through the letters" | **Transparent background** (RGBA, alpha cut to letter bounds) |
| `next_class_silhouette.png` ✅ **EXISTS (30KB, real)** | 256 (256×256, transparent) | "single dark featureless humanoid silhouette of a robed elemental mage, faint pale cyan #00FFCC rim light tracing the contour, one tarnished-gold ember at the chest, mysterious teaser unlock pose, centered" | **Transparent background**; spec asks 256px sq — generate at 256×256, alpha to silhouette. (Existing file is 128×192; regen to 256² if the 256px target is enforced, else reuse.) |
| `boss_skull_glyph.png` ❌ **PYTHON (226B) — generate** | 64×64 (gen) → 32×32 | "menacing skull glyph, eroded slate-stone carving with a thin tarnished-gold rim and pale cyan #00FFCC rift light glowing in the eye sockets, mythic boss-marker sigil, centered" | **Transparent**; gen at 64² on magenta, chroma-bbox → 32×32 (matches existing footprint) |

---

## C — Draft-card + slot frames (Path B: one magenta `#FF00FF` sheet, chroma-bbox extract; transparent)

| Target file | Canvas px | `$imagegen` prompt (+append common suffix) | Transparency / chroma |
|---|---|---|---|
| `card_frame_stone.png` ❌ **PYTHON (1.5KB) — generate** | 160×224 | "translucent fractured slate-stone trading-card frame, eroded tarnished-gold iron corner-brackets, a faint pale cyan #00FFCC rift hairline along the inner edge, ornate-but-restrained, 9-slice friendly thick border, hollow transparent center, mythic reward-card" | **Transparent center + outer**; magenta bg, chroma-bbox; **mark 9-slice border** on import |
| `icon_frame_hex.png` ❌ **PYTHON (1.7KB) — generate** | 64×64 | "hexagonal icon frame, eroded slate-stone ring with a thin tarnished-gold edge and a single pale cyan #00FFCC inner rift glint, hollow transparent center, clean readable at small size" | **Transparent center**; magenta bg, chroma-bbox |
| `hex_slot_mask.png` ❌ **PYTHON (186B) — generate (or keep)** | 32×32 | "solid white filled hexagon silhouette on transparent background, clean hard edges, used as an alpha mask" | **Pure white-on-transparent MASK** — no color/detail; cheap, Python is also fine. Generate only if batching with C. |

---

## D — Rarity glow set (Path B: one magenta sheet, 128² cells, chroma-bbox; additive transparent)

| Target file | Canvas px | `$imagegen` prompt (+append common suffix) | Transparency / chroma |
|---|---|---|---|
| `rarity_glow_common.png` ❌ **PYTHON (4KB) — generate** | 128×128 | "soft diffuse radial glow, desaturated slate-grey, faint, additive bloom, transparent" | **Transparent**, additive; center bright → edges fade to 0 alpha |
| `rarity_glow_rare.png` ❌ **PYTHON (4KB) — generate** | 128×128 | "soft diffuse radial glow, pale cyan #00FFCC, additive bloom, transparent" | same — additive |
| `rarity_glow_epic.png` ❌ **PYTHON (4KB) — generate** | 128×128 | "soft diffuse radial glow, deep mystic purple #8033A0, additive bloom, transparent" | same — additive |
| `rarity_glow_legendary.png` ❌ **PYTHON (4KB) — generate** | 128×128 | "soft diffuse radial glow, radiant tarnished-gold #F2BC3D with a faint pale cyan #00FFCC inner core, strongest bloom, additive, transparent" | same — additive |

> Glows are pure radial bloom (no stone/frame) — generate on magenta, chroma-bbox to clean transparent circles.
> Keep the brand hierarchy: common=grey, rare=cyan, epic=purple, legendary=gold (gold is the rarest = the only
> place gold dominates over cyan).

---

## E — Already-fine placeholders (do NOT regen this pass)
`banner_underline_rune.png` (320×48, additive cyan streak — Python fine) · `minimap_frame_stone.png` (80×80 — HUD-quiet, low ROI) · `steam_glyph_cyan.png` (24×24) · `particle_ember.png` (8×8) · `dust_mote.png` (4×4) · `card_select_flash.png` (160×224 white flash) · `wishlist_cta_btn.png` (256×48) · `wishlist_cta_btn_lg.png` (320×64). Regen later only if a polish batch has spare quota.

---

## Tally — generate vs reuse
**REUSE (already real hand-gen art, 5):** `menu_bg_island`, `victory_bg_bloom`, `death_overlay`, `logo_rima_glyph`, `next_class_silhouette` (reuse as-is, or optional 256² regen if the 256px target is enforced) — matches the "5/7 hero imagegen done" note (boss + char-select were the 2 not done; here `boss_skull_glyph` and the new `char_select_bg` carry that).

**GENERATE NEW / UPGRADE FROM PYTHON (8):**
1. `char_select_bg.png` (NEW, 640×360)
2. `boss_skull_glyph.png` (Python → 32×32)
3. `card_frame_stone.png` (Python → 160×224)
4. `icon_frame_hex.png` (Python → 64×64)
5. `rarity_glow_common.png` (Python → 128×128)
6. `rarity_glow_rare.png` (Python → 128×128)
7. `rarity_glow_epic.png` (Python → 128×128)
8. `rarity_glow_legendary.png` (Python → 128×128)
   *(+ optional: `hex_slot_mask` 32² if batched — mask, low value; `next_class_silhouette` 256² regen if 256px enforced.)*

**STAY PYTHON (intentional, 1 called out + section E):** `lowhp_vignette.png` (procedural red radial) + the section-E set.

**Counts: 8 to generate · 5 to reuse · 1 stays Python (lowhp_vignette) + 8 already-fine placeholders untouched.**
