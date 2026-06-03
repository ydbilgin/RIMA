# RIMA Image-Gen Asset Pack — Production Plan (S6)

> Goal (user): replace the Python placeholders with **real** assets via **Codex imagegen** (gpt-image-2 /
> generate2dsprite), produced so cuts are **clean and lossless**, especially the menu. Keep sizes **PixelLab-standard**
> so a later PixelLab refine drops in. The 24 Python placeholders (`Assets/Resources/UI/RIMA/*` + boss) are the
> "şimdilik geçici" layer and the **layout/brand guide** for this real pass.

## ⚠️ The clean-cut reality (why naive grid-slicing fails)
AI image gen (gpt-image-2/DALL·E-class) does **NOT** snap elements to an exact pixel grid. A prompted "sprite sheet"
drifts: cell sizes vary, gutters wander, elements aren't pixel-aligned. So **do not trust a fixed grid and slice
blindly** — you'll cut through art ("kayıp"). Two robust paths instead:

### Path A — Individual image-to-image (NO slicing) → full-screens + hero assets
Feed each Python placeholder as the **init/reference image** to gpt-image-2 and prompt "upgrade to finished pixel art,
same composition/size/palette." Output stays its own file at its exact size → **zero cutting**, brand+layout preserved.
Use for: `menu_bg_island`, `victory_bg_bloom`, `death_overlay`, `lowhp_vignette`, `logo_rima_glyph`,
`next_class_silhouette`, `PenitentSovereign_placeholder` (boss). This is the **cleanest** route and the one for the menu.

### Path B — Chroma-sheet + bounding-box extraction (lossless slice) → small discrete UI pack
For the many small elements (icons/frames/glyphs/glows/particles), generate them on a **solid magenta (#FF00FF)
background, well-separated**, then extract each element by **alpha/chroma bounding box** (not by rigid grid). This is
exactly what the `generate2dsprite` local processor does (chroma-key → frame extraction → align → transparent export).
The cut follows the art's true bounds → **lossless, clean edges**, regardless of AI drift. Group by uniform target size
so each extracted sprite lands on a PixelLab-standard canvas.

> If a true uniform grid IS wanted, generate with **explicit thick gutters + cell separators** and a stated cell size,
> then still extract per-cell via chroma bbox (not raw grid math). Belt-and-suspenders.

## Size groupings (PixelLab-standard — so PixelLab refine drops in)
| Group | Assets | Gen size | Final canvas |
|---|---|---|---|
| Full-screen (Path A) | menu/victory/death/lowhp | 1280×720 master | 640×360 (integer ½) |
| Hero (Path A) | logo, next-class, boss | native (256×96 / 128×192 / 192²) | same |
| 64² icon pack (Path B) | icon_frame_hex, hex_slot_mask, boss_skull, steam_glyph(→64 then downscale 24) | one magenta sheet, 64² cells + 32px gutters | 64×64 each |
| 128² glow pack (Path B) | rarity_glow ×4 | one sheet, 128² cells | 128×128 each |
| Frame pack (Path B) | minimap_frame_stone(80), card_frame_stone(160×224), banner_underline(320×48), wishlist btns | individual (varied AR) | exact |
| Particles (Path A, keep Python) | particle_ember(8), dust_mote(4), card_select_flash | tiny — **Python placeholder is already fine**, low ROI to regen | — |

All imports: Sprite, **Point** filter, Compression None, **PPU 64**, no mipmaps. Full-screen = no 9-slice; frames/pills = 9-slice border.

## Skill selection
- **gpt-image-2** (`/gpt-image-2`): Path A image-to-image (init = the placeholder). Best for full-screens + hero.
- **generate2dsprite**: Path B magenta sheets + the chroma/bbox processor for lossless extraction (icon/glow packs).
- (Per #2 shorts: PixelLab **Object Creator** may pack-generate these too — verify live; an alternative to Path B.)

## Per-asset prompts
Reuse `STAGING/IMAGEGEN_PACK_S6.md` prompts verbatim (they already encode brand + size + the common style suffix).
For Path A add the image-to-image preamble: *"Refine this exact image into finished pixel art, identical composition,
size and palette; cyan #00FFCC rift accents over deep-purple #3A1A4A→black void; crisp PPU64 pixels; no text."*

## Execution / quota
- Codex image gen shares the codex account quota. **3 accounts are spent (resetting); only yekta has headroom now.**
- **Now:** probe gpt-image-2 on `menu_bg_island` (Path A) to prove the pipeline + check availability. If it works,
  do the 7 Path-A hero/full-screens (highest visible impact). 
- **On 3-account reset (bulk):** run the Path-B packs + any remaining, per the user's "bol resim" plan.
- Drop outputs at the SAME `Resources/UI/RIMA/...` paths (overwrite placeholders) → self-building UI picks them up.

## Order of value (when generating)
1. `menu_bg_island` (first thing players see) → 2. `victory_bg_bloom` + `death_overlay` (conversion) →
3. `logo_rima_glyph` → 4. boss → 5. frame/icon packs (Path B) → 6. particles (skip — Python fine).
