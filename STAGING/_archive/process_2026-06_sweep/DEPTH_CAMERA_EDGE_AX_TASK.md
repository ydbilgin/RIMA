# AX (Gemini) TASK — RIMA top-down 3/4: camera-depth + WALLED Ruined-Keep rooms + beautiful edges

You are an art-director + Unity 2D rendering consultant. VIEW BOTH look references:
- `STAGING/floor_perspective_concepts/03_wallless_improved.png` (wall-less floating-island edge, cyan rim-light).
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` and `.../ChatGPT Image 25 May 2026 00_18_45 (2).png` — RIMA HUD mockups of **WALLED, semi-enclosed broken-masonry top-down 3/4 dungeon rooms** (stone perimeter walls, columns, torches, central cyan rift-altar, dark floor). Subfolders `blueprint_room/`, `new_chatgpt/` have more.

## CONTEXT (LOCKED)
RIMA = top-down 3/4 action-roguelite, flat 2D ortho camera (PixelPerfect assetsPPU=64, ref 640x360, orthoSize=4), cursor-aim. Canon = **Ruined Keep hybrid**: floating-island base + broken semi-enclosing masonry (so walls ARE allowed, broken/partial, with open void gaps).
User goal: "DEPTH via camera closeness; walls/boundaries right for top-down 3/4 but placed LOGICALLY; make it beautiful."

## ANSWER (concrete, reference what you SEE)
1. **Camera closeness → depth:** how does zooming closer (smaller ortho/ref) boost the dramatic dimensional feel like the refs? Recommend a zoom level for close-but-readable. Subtle perspective tilt worth it? Trade-off vs cursor-aim.
2. **WALLED rooms in top-down 3/4 (the chatgpt_ref look):** how are those perimeter walls/columns drawn to read correctly in top-down 3/4 (visible front face + height, Y-sorted so the player walks behind/in front)? How do those mockups place walls LOGICALLY (perimeter framing, entrances, central altar focal point)? What makes them look coherent not cluttered?
3. **Edges/boundaries:** mixing solid broken walls with open void edges + cyan rim-light — where each goes, keeping the play-area limit clear + beautiful.
4. **Floor beauty:** current placeholder floor too dark — value range, light-pools, decals, foreshortening for readable beauty.
5. **Depth without (only) walls:** tall props, Y-sort overlap, parallax void bg, contact shadows.
6. One-paragraph recommendation + dos/don'ts to make RIMA rooms look like the chatgpt_ref mockups (beautiful + depthful + logically built).
Keep it concrete.
