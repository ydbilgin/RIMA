# PixelLab AI YouTube Shorts — Research Report (S6, ax/Antigravity)

> Source: ax research on https://www.youtube.com/@PixelLab_AI/shorts (2026-05-30).
> ⚠️ Provenance limit: ax has no headless-browser/video-playback tool, so it could NOT watch the shorts frame-by-frame.
> It reconstructed the techniques via targeted web search on the channel's titles/metadata/discussion. Treat feature
> claims (esp. "Object Creator", dates, credit costs) as **to-verify** against the live PixelLab app before relying on them.

## Shorts → technique → RIMA use
| Short topic | Technique | RIMA use | Priority |
|---|---|---|---|
| Character States | Generate poses/variants (mid-walk, combat, hurt) BEFORE animating | Make `hurt/casting/attacking` states as animation anchors → prevents AI limb-glitch | **High** |
| GBA-style anim | **"Mid-stride trick"**: start walk cycles from a legs-spread mid-step frame, not standing | Smoother 8-dir walk loops (S/SW/W/NW/N bakes) | **High** |
| Auto-rotate | 4/8-dir sheets from 1 input via Image-Guidance-Weight + optional Force-Symmetry | Auto-gen the 5 core angles from 1 front concept → faster 8-dir bakes | **High** |
| Object Creator (claims Apr 2026) | Group-generate/catalog/rotate/animate object PACKS w/ style refs | Batch dungeon props + weapons in one UI w/ cyan palette — **may supersede create_1_direction_object batching** | **High (verify)** |
| Loot generation | Style-Reference → tiered common/rare/epic/legendary from a base | Weapon variants (HandAnchor) + loot tiers, uniform canvas | High |
| Create Tiles Pro | Square/Hex/Iso/Octagon tiles, non-square (16×32…64×128), adj. thickness/angle | Iso/topdown tile transitions at PPU64 (grass→dirt, cliffs) | Med |
| Interpolate (2 frames) | AI interpolates between a START and END image (pixel-cluster aware) | Controlled swing/VFX timing — **you supply start+end**, AI fills (still user-gated animate) | Med |
| Destructible (inpaint) | Iterative inpainting → damaged/broken frames | Breakable props (vases/chests/crates/pillars) | Med |
| Image→Pixel art | Downscale + palette-map converter | Concept/photo → clean pixel base | Low |
| Aseprite plugin | PixelLab inside Aseprite (Ctrl+Space+P), mask-paint inpaint | Edit weapon/armor overlays on canvas, validate offsets | Med |

## Top 5 to adopt/test (ROI order)
1. **Mid-stride trick** for all walk cycles — biggest smoothness win, zero extra cost.
2. **Character States as anim anchors** — gen `weapon-drawn / mid-swing` states, animate from those → frame stability.
3. **Object Creator** for weapon/prop packs — verify it's live; if so, migrate weapon batching here (pack throughput).
4. **Tiered loot via Style Reference** — base weapon → epic/legendary variants, uniform canvas.
5. **Aseprite plugin** (v1.3.7+ paid, needs "full trust") — kills export/import round-trips for masks/offsets.

## Updates / contradictions to our current PixelLab KB (VERIFY before acting)
- **Weapon workflow:** `create_1_direction_object` batch → possibly superseded by **Object Creator** (pack + states + rotation in one). [[reference_pixellab_knowledge_base_s114]] update candidate.
- **Animation:** **Interpolation (2-frame)** = controlled start/end → AI in-between. Fits the user rule "I may make the start/end frame but NEVER animate without your approval" → interpolation is the *controlled* path, still gated ([[feedback_never_animate_without_approval]]).
- **Tiles:** Create Tiles Pro supports non-square iso (16×32 … 64×128) = PPU64 fit. Claimed cost ~20-25 gens (S/M/L), 20-40 with style refs.
- **Canvas cap:** auto-rotate/gen ~**128×128** standard ceiling (matches our [[reference_pixellab_knowledge_base_s114]]).

## Honest access limit
ax could not crawl the raw shorts video/frames (no JS/video tool). Findings = web-search reconstruction of the
channel's topics. **Action:** before relying on "Object Creator" / dates / costs, confirm in the live PixelLab app or docs.
