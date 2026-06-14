ACTIVE RULES: be honest about quality. Use your real image_gen tool (confirmed working). No faking.

# IMAGE-GEN — Priority A screen art (4 highest-impact, de-risk quality first)

Generate these 4 PNGs with your image_gen tool, resize to the EXACT pixel size, save to the listed path under
`Assets/Resources/UI/RIMA/`. Brand: cyan rift `#00FFCC` (the seal's energy) over deep-purple `#3A1A4A`→black void;
slate stone structure; pixel-art / limited-palette feel; NO text in the art; transparent where noted.

| File (under Assets/Resources/UI/RIMA/) | Size px | Prompt |
|---|---|---|
| `menu_bg_island.png` | 640×360 | "A lone fractured floating stone keep-fragment suspended in a vast dark abyss, cyan #00FFCC rift cracks bleeding light along its torn cliff edges, one small warm ember, slow dust, cinematic somber lonely hero shot, 2D pixel art, deep-purple #3A1A4A to black void" |
| `wishlist_cta_btn.png` | 256×48 | "dark fractured-stone pill button shape, glowing cyan #00FFCC edge, transparent background, no text, 9-slice friendly, pixel art" |
| `death_overlay.png` | 640×360 | "near-black full-screen vignette, faint cyan #00FFCC embers rising from below, transparent center fading to dark edges, mournful, pixel art" |
| `next_class_silhouette.png` | 128×192 | "dark featureless silhouette of a robed elemental mage, faint cyan #00FFCC rim light, transparent background, mysterious teaser, pixel art" |

## Import .meta (best-effort)
For each PNG also write a Unity `.meta` marking it as a **Sprite** with **filterMode Point (0)**, **textureCompression
None**, **spritePixelsToUnit 64**, a fresh GUID. If you are NOT confident of the exact current TextureImporter YAML
format, SKIP the .meta and note it (Unity will import with defaults; settings fixed later) — do NOT write a malformed .meta.

## DELIVER (write to DONE file)
For each: saved path, final dimensions (verify with a tool), and an HONEST 1-line quality note (is it on-brand /
usable as a placeholder?). State the image_gen tool used. If any generation looks off-brand or low quality, say so —
these are placeholders the user may regenerate in PixelLab. List BLOCKED + why.
