# RIMA — Image-Gen Pack (S6, demo screens) — copy-paste ready

> Derived from `DESIGN_LOCK_DEMO_S6.md` §4. **Brand:** cyan rift `#00FFCC` (the seal's energy) over deep-purple
> `#3A1A4A`→black void; slate structure; tarnished gold `#F2BC3D` = reward-only. Style: **pixel art, PPU 64, 640×360
> ref-res, integer upscale.** "Ashen Glyph" UI = no hard rectangular borders, translucent fractured-stone/iron, things
> pulse/fade. Every full-screen bg = a framed view of the floating seal-keep island in the void.
> **Sizing law:** full-screen art EXACTLY 640×360 (or clean 1280×720 master). Icon source max 2× footprint. NEVER 1920×1080.
> **Generation:** Codex image-gen / PixelLab / Sora — any. Drop output at the listed Resources path; a later wiring batch
> binds them into the self-building screens (currently procedural/RimaUITheme placeholders).

## Common style suffix (append to every prompt)
`, 2D pixel art, limited palette, deep-purple #3A1A4A to black void background, cyan #00FFCC rift energy glow as the only saturated accent, slate-grey stone, no text, no watermark, crisp pixels, PPU64 scale`

## Priority A — Menu + Conversion screens (highest visible impact)
| File (Resources path) | Size px | Prompt (+ append common suffix) |
|---|---|---|
| `Resources/UI/RIMA/menu_bg_island.png` | 640×360 | "A lone fractured floating stone keep-fragment suspended in a vast dark abyss, cyan rift cracks bleeding light along its torn cliff edges, one small warm ember brazier, slow drifting dust, cinematic low-angle hero shot, somber lonely mood" |
| `Resources/UI/RIMA/menu_bg_island@2x.png` | 1280×720 | (same as above, 2× master) |
| `Resources/UI/RIMA/logo_rima_glyph.png` | 256×96 | "RIMA wordmark logo, eroded engraved stone letters, a single cyan #00FFCC rift glyph fracture through the letters, transparent background" |
| `Resources/UI/RIMA/death_overlay.png` | 640×360 | "near-black full-screen vignette, faint cyan #00FFCC embers rising, transparent center fading to dark edges, mournful" |
| `Resources/UI/RIMA/wishlist_cta_btn.png` | 256×48 | "dark fractured-stone pill button, glowing cyan #00FFCC edge, small Steam logo glyph on left, 9-slice friendly, transparent background, no text" |
| `Resources/UI/RIMA/wishlist_cta_btn_lg.png` | 320×64 | (same, larger) |
| `Resources/UI/RIMA/victory_bg_bloom.png` | 640×360 | "the floating seal-keep island seen from afar, amplified radiant cyan #00FFCC rift bloom answering briefly, triumphant-but-somber, the seal momentarily holding" |
| `Resources/UI/RIMA/next_class_silhouette.png` | 128×192 | "dark featureless humanoid silhouette of a robed elemental mage, faint cyan #00FFCC rim light, mysterious teaser, transparent background" |
| `Resources/UI/RIMA/steam_glyph_cyan.png` | 24×24 | "minimal Steam logo glyph, cyan #00FFCC line art, transparent" |

## Priority B — HUD + Draft frames
| File | Size px | Prompt |
|---|---|---|
| `Resources/UI/RIMA/minimap_frame_stone.png` | 80×80 | "broken-stone square frame, 4px thick eroded border, transparent center, faint cyan #00FFCC cracks at corners" |
| `Resources/UI/RIMA/hex_slot_mask.png` | 32×32 | "white hexagon silhouette on transparent, clean edges (mask)" |
| `Resources/UI/RIMA/lowhp_vignette.png` | 640×360 | "radial blood-red to transparent vignette, transparent center, additive overlay, no text" |
| `Resources/UI/RIMA/card_frame_stone.png` | 160×224 | "translucent fractured-stone trading-card frame, eroded iron corners, 9-slice, transparent center, no text" |
| `Resources/UI/RIMA/rarity_glow_common.png` | 128×128 | "soft gray radial glow, transparent" |
| `Resources/UI/RIMA/rarity_glow_rare.png` | 128×128 | "soft teal #339999 radial glow, transparent" |
| `Resources/UI/RIMA/rarity_glow_epic.png` | 128×128 | "soft purple #8033A0 radial glow, transparent" |
| `Resources/UI/RIMA/rarity_glow_legendary.png` | 128×128 | "soft gold #CCA626 radial glow, transparent" |
| `Resources/UI/RIMA/icon_frame_hex.png` | 64×64 | "hexagonal icon frame, eroded stone + thin cyan #00FFCC inner edge, transparent center" |
| `Resources/UI/RIMA/banner_underline_rune.png` | 320×48 | "faint horizontal cyan #00FFCC rift-rune underline streak, transparent, additive" |
| `Resources/UI/RIMA/boss_skull_glyph.png` | 32×32 | "cyan #00FFCC line-art skull glyph, transparent" |

## Priority C — Particles / caps (tiny additive)
| File | Size px | Prompt |
|---|---|---|
| `Resources/UI/RIMA/particle_ember.png` | 8×8 | "single soft cyan #00FFCC ember dot, additive, transparent" |
| `Resources/UI/RIMA/dust_mote.png` | 4×4 | "single faint grey dust speck, transparent" |
| `Resources/UI/RIMA/card_select_flash.png` | 160×224 | "pure white soft flash card shape, additive, transparent" |

## Skill icons (20+, white-silhouette + cyan accent) — 64×64 each
`Resources/UI/RIMA/skill_icon_<name>.png` — "white pixel-art silhouette icon of <skill concept>, single cyan #00FFCC accent detail, flat, transparent background, readable at 20px". Drives `SkillBase.icon` on `Data/Skills/*.asset`. List of skills = `Assets/Scripts/Skills/{Warblade,Elementalist,Ranger,Shadowblade}/*`.

## Import settings (all)
Texture Type Sprite, **Filter = Point (no filter)**, Compression = None, PPU = 64, no mipmaps. Full-screen bg = no 9-slice; frames/pills = 9-slice border.
