# Multi-Resolution Scaling Research: Pixel-Art 2D Action RPGs
**Date:** 2026-05-28  
**Model:** gemini-3.1-pro-preview (default) — ran with --yolo after auth-prompt bypass  
**Context:** RIMA Unity URP 2D, Pixel Perfect Camera, PPU 64, 120px chibi canvas, painterly VFX, cyan Hades-Elysium style. Candidate config: Pixel Perfect Camera refResolution 640x360 (integer-scale 3x@1080p / 4x@1440p / 6x@4K).

---

## Per-Game Findings

### Alabaster Dawn (Crowabout Games)
- **Integer-scale or HD-upscale?** NOT strict pixel-perfect. Uses skeleton-to-pixel-art converter with rotation/skew — breaks global pixel grid. Optional low-res mode at 640x360 for classic aesthetic.
- **4K framing:** Fixed 16:9. Playing at 4K upscales the image — you do NOT see more world. Same FOV scaled up.
- **Known base resolution:** 640x360 optional low-res mode; default is higher-res.
- **Sources:** Steam/Reddit community discussions, WSGF (Widescreen Gaming Forum). No official dev postmortem cited.
- **Confidence:** MEDIUM — community-sourced, no first-party technical doc.

### Colossus - Eternal Blight (Rustic Panda Games)
- **Integer-scale or HD-upscale?** HD-upscale approach, NOT strict integer grid. Built in GameMaker. Developer explicitly avoided strict pixel-lock to support arbitrary monitor sizes.
- **4K framing:** Fixed 16:9 at 4K (upscales base view). Ultrawide/super-ultrawide EXPANDS horizontal FOV — more world visible on X-axis.
- **Known base resolution:** No specific base resolution cited publicly.
- **Sources:** Developer patch notes and Steam Community hub updates.
- **Confidence:** MEDIUM — dev patch notes cited but no direct link.

### Children of Morta (Dead Mage / 11 bit studios)
- **Integer-scale or HD-upscale?** NO pixel-perfect integer scale. Internal native base ~1600x900. Dynamic camera zoom (zoom in/out during combat + cutscenes) causes intentional pixel crawl/shimmer — accepted as cinematic style.
- **4K framing:** Maintains fixed 16:9 framing at 4K — upscales, does NOT show more dungeon. Ultrawide requires community hex-edit workaround.
- **Known base resolution:** ~1600x900 internal.
- **Sources:** Dead Mage Steam forum discussions; PCGamingWiki resolution breakdowns.
- **Confidence:** MEDIUM — PCGamingWiki + Steam forums, no official dev doc.

### Hades (Supergiant Games)
- **Art type confirmation:** Hand-painted 2D HD environments + pre-rendered 3D characters (Maya → Bink video textures → V-Ray toon shader). NOT pixel art. No pixel-perfect constraint.
- **Native render resolution:** 1080p (1920x1080). At 1440p/4K the 1080p assets are upscaled — character sprites appear slightly softer than native-4K 3D games.
- **4K framing:** Fixed framing, upscaled. Not a pixel-art game — no integer-scale consideration.
- **Sources:** Supergiant Noclip documentary; GDC/YouTube technical breakdowns of their 3D-to-2D pipeline.
- **Confidence:** HIGH — Noclip documentary is primary source.

---

## General Industry Trend 2023–2026

### Dominant approach: **Option B** — High-res render + flexible orthographic camera

Modern pixel-art Action RPGs overwhelmingly choose high-res rendering (1080p+) with sub-pixel movement and Upscale Render Texture, NOT strict integer-scale Pixel Perfect Camera.

**Three reasons cited by developers:**

1. **Combat fluidity:** Integer-scale locks movement to pixel grid — causes jitter on diagonals and dodge-rolls at 60/144fps. Sub-pixel rendering + nearest-neighbor filter keeps art chunky while allowing silky physics.
2. **Camera shake & VFX:** Screen shake on a pixel-locked camera produces jagged jumps. High-res camera observing low-res assets (or UpscaleRenderTexture) allows smooth shake + lighting (e.g., Sea of Stars).
3. **Unity UpscaleRenderTexture:** Unity's Pixel Perfect Camera "Upscale Render Texture" toggle has become the practical industry standard — renders pixel art cleanly while enabling sub-pixel transforms and smooth camera tracking.

### Referenced talks / posts:
- **GDC:** Tyriq Plummer "Modern Pixel Art" — shift from hardware-restriction integers to "HD-2D" approach
- **GDC:** Kyle Bunk "Hi-Bit Age" — retro sprites married to sub-pixel engine physics + modern lighting
- **Dead Cells pipeline:** 3D-to-pixel via shaders — inherently requires high-res render + non-integer scale
- **Unity Pixel Perfect Camera docs** — UpscaleRenderTexture toggle specifically designed for this hybrid approach

**Note:** GDC talk titles are cited by Gemini from model knowledge — specific year/URL not confirmed. Verify at gdcvault.com before using as authoritative reference.

---

## RIMA-Specific Recommendation

**RIMA profile:** 120px chibi sprites + painterly VFX + dynamic camera + Hades-Elysium cyan aesthetic.

**Verdict: Option A (640x360 + integer-scale UpscaleRenderTexture) is viable but niche. Option B hybrid is better fit.**

Reasoning:
- Painterly VFX (particle systems, screen shake, cyan hitsparks, dash trails) will look degraded on strict pixel-lock — shake jitters, particles lose smoothness.
- 120px sprites at PPU 64 on 640x360 base = characters ~1.87 units tall, fills screen reasonably. But at 4K (6x scale) the chunky pixel look will be very pronounced — may conflict with painterly VFX layer.
- **Recommended hybrid:** Pixel Perfect Camera with **UpscaleRenderTexture ON + Pixel Snapping OFF**. This gives integer-clean sprite rendering while allowing sub-pixel camera movement, smooth VFX, and fluid combat. Reference resolution 640x360 is correct for 1080p=3x / 1440p=4x / 4K=6x integer ladder.
- UI on separate Screen Space Overlay Canvas with Scale With Screen Size (1920x1080 ref) — never run UI through UpscaleRenderTexture.

---

## Confidence Summary

| Finding | Confidence | Reason |
|---|---|---|
| Hades = hand-drawn HD, ~1080p native | HIGH | Noclip documentary |
| Children of Morta ~1600x900, no integer-scale | MEDIUM | PCGamingWiki + Steam forums |
| Alabaster Dawn 640x360 optional low-res | MEDIUM | Community sources, no first-party doc |
| Colossus - Eternal Blight HD-upscale | MEDIUM | Dev patch notes, no direct link |
| Industry trend: Option B dominant | MEDIUM-HIGH | Model knowledge + Unity docs pattern; GDC talk titles unverified |

**GAPS:** No first-party developer blogs or GDC vault links confirmed for Alabaster Dawn or Colossus. GDC talk titles (Plummer, Bunk) cited from model knowledge — year and URL not verified.
