# Research: Yudou Tweet — Painted Background Prototype
**Source:** https://x.com/Yudou/status/2056564099681493138
**Date fetched:** 2026-05-19
**Method:** yt-dlp (video + info.json) + ffmpeg frame extraction

---

## Tweet Content

**Text:** "okay *now* we're starting to get somewhere #pixelart #gamedev"
**Author:** @Yudou (yudou.art) — pixel art indie game dev
**Media:** 15-second video clip

### What the Video Shows

A 2D side-scrolling combat prototype (NOT top-down — different projection than RIMA). Key observations frame by frame:

- **Frame 1-2:** Two pixel-art characters on a plain gray background — a teal/cyan dual-sword figure and a pink-haired fighter. Dark horizontal floor line. Placeholder/blocked-out state.
- **Frame 3-5:** Combat VFX visible — large white slash arc, sparkle burst. Highly readable against dark BG. Characters remain clearly separated from background.
- **Frame 6-8:** The "getting somewhere" reveal — a wipe/slide transition exposes a fully painted background replacing the gray: dark atmospheric forest with twisted trees, large rock formations, mushrooms, layered depth. The layout geometry does not change — only the art layers underneath are being revealed.

The painted background uses:
- Deep value range (near-black to dark gray-brown)
- Large organic silhouette shapes for far depth
- Mid-ground detail: rock texture, twisted tree trunks, mushroom caps
- No grid/tile pattern visible — fully hand-painted feel
- Color palette: muted desaturated earthy tones + very dark blues/purples for atmosphere

---

## Gemini RIMA Takeaways (4 Concrete Actions)

**Model used:** gemini-3.1-pro-preview (default, two 429 rate-limit retries before success — same model, capacity exhaustion not model error)

### 1. Strict Value/Saturation Isolation: Characters vs. Floor

Yudou keeps characters at high saturation (cyan, pink) against a desaturated dark background. RIMA action: compress Shattered Keep floor and wall palettes into a darker, lower-saturation range (moody blues, dark gray-browns). Reserve highest saturation exclusively for character sprites. Consider bright colored outlines or rim-light edges on characters instead of dark outlines, which disappear into dark floors.

### 2. Pure-White VFX Cores for Readability

Large white slash arcs and sparkle bursts read instantly because of extreme luminance contrast. RIMA action: VFX slash impacts and telegraphs should prioritize pure-white or extremely bright cores. Avoid dark/transparent VFX (dark smoke, shadowy magic) without a bright glowing edge. In a dark Hades-style environment, readability depends entirely on peak brightness.

### 3. Blueprint-to-Organic Stamp: Break the Grid Deliberately

The wipe transition validates the Blueprint-first approach — functional layout defined first (gray placeholder), then art painted on top that ignores grid rigidity. RIMA action: treat the Blueprint layer as strictly functional (invisible engine boundaries). When placing BackgroundLayerData brush-stamps, deliberately break the grid — use large, asymmetrical, organic rock formations and irregular grass patches that bleed over Blueprint zone edges. This hides the grid and sells the hand-painted illusion.

### 4. Detail Reduction for Depth (Silhouetting)

Background depth achieved by reducing far elements to dark featureless silhouettes, reserving texture for mid-ground. RIMA translation to 35-degree top-down: use detail reduction for vertical depth via sorting layers. Abyss / deep water / chasm layers (absolute bottom) = low-detail dark silhouettes. High-frequency pixel detail (stone cracks, grass blades, pebbles) = strictly the playable floor layer. Creates natural focal plane keeping eyes on navigable space.

---

## Honest Read: How Directly Useful Is This?

**Directly useful: YES, with projection caveat.**

The core finding — a wipe transition from gray placeholder to fully painted background — is strong external validation for RIMA's exact pipeline (Blueprint semantic zones → BackgroundLayerData painted overlay). This is the same architectural decision arriving independently from another dev.

The specific techniques (value isolation, VFX white cores, silhouette depth, grid-breaking stamps) all translate cleanly to a top-down 35-degree projection. The projection difference (side-scroll vs top-down) changes *where* depth is expressed (horizontal vs. vertical axis) but the visual principles transfer 1:1.

The one thing NOT transferable: Yudou's floor is a simple horizontal line (platformer). RIMA needs actual floor art coverage. But the dark value palette principle still applies — dark, compressed floor tones let characters pop.

**RIMA paint prompt impact:** Shattered Keep F1 stone palette should trend darker and more desaturated than current test. Characters should not use dark outlines against a dark floor.

---

## Assets Downloaded

- `STAGING/yudou_tweet_thumbnail.jpg` — video thumbnail (hero frame, both characters visible)
- `STAGING/yudou_tweet_video.mp4` — full 480x270 video (15s)
- `STAGING/yudou_frame_01.jpg` through `yudou_frame_08.jpg` — extracted frames

Temp file `*.info.json` removed from root.
