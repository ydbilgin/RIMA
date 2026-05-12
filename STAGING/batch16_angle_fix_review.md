## Task
Review the GLOBAL STYLE BLOCK below. The 16-character batch prompt already produces
the correct visual style. The only problem: some characters came out south-east facing
instead of true south.

A new reference image was produced (described below as REF IMAGE) that has the correct
true south angle. We want to use this image in Create Image Pro to lock the angle for
all 16 characters.

Answer these questions:
1. What minimal text changes to the GLOBAL STYLE BLOCK would best enforce true south
   facing and prevent south-east drift?
2. In PixelLab Create Image Pro: should the new correct-angle image be added as
   "style reference" (aesthetic/texture influence) or "reference image" (pose/content
   influence)? Which is more effective for locking a specific camera angle?
3. Should the reference image be added once globally, or per-variation?
4. Output: corrected GLOBAL STYLE BLOCK only (minimal changes, do not touch individual
   variation descriptions).

---

## REF IMAGE description (Image 6)
A single 64x64 chibi male character with dual short swords, true south facing,
correct high top-down ~35 degree camera angle, face and both eyes clearly visible,
symmetric left-right pose, chunky chibi proportions, thick dark outline,
hard pixel edges, dark fantasy palette. This is the angle we want for all 16.

---

## CURRENT GLOBAL STYLE BLOCK

GLOBAL STYLE BLOCK (apply to ALL 16 variations):
64x64 pixel art chibi character, top-down ARPG camera angle ~30-35 degrees overhead
(Hades / Hyper Light Drifter match), south-facing idle pose. Chibi proportions:
oversized head (~40% of total height), short legs, broad shoulders, fits entirely
within 64x64 canvas. Outline: 1px dark pixel outline, no anti-aliasing, no gradients,
no painterly shading. Environment palette: muted desaturated tones #4A4A4A / #2A2E35 /
#1A2B1A as neutral base. Neon accent usage: #FFB000 amber / #FFF000 yellow / #00FFCC
rift cyan — used sparingly as class accent highlight only. Visual reference: Into
Samomor (Sang Hendrix, RPG Maker MZ) character style. Each variation is a standalone
character sprite on transparent background. Weapon is integrated into the sprite as
1-piece (no separate attachment). Silhouette must be readable at 64x64 and at 16px
thumbnail. No 3D render, no painterly gradients, no realistic proportions, no
isometric projection.
