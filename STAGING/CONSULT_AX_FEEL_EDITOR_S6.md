# AX (Gemini) CONSULT — "feel inside a room" (no camera tilt) + detailed pause-editor UX

You are a game-design/UX researcher. Two focused questions. Output concise markdown, practical, ranked. NO code.

## SHARED CONTEXT
RIMA = 2D top-down 3/4 action-roguelite (Unity URP, PPU64). 
**HARD CONSTRAINTS (cannot change):**
- Camera is FLAT orthographic, **NO perspective tilt** (tilt breaks cursor-aim + sprite scale + Y-sort — locked).
- Character sprites (10 classes × 8 directions = 80 sprites) are drawn at **~70-80° high top-down 3/4** and CANNOT be redrawn at a lower/more-frontal angle (multi-week cost, rejected).
- So any "inside a room" fix MUST come from ART / LAYOUT / ZOOM / LIGHTING — NOT camera angle.

Current room: connected masonry wall-runs already built — North = continuous wall + cyan arch gate; East/West = lit wall runs with void gaps; **South/front = OPEN VOID (floating-island edge)**. Walls are flat-front sprites ~3 cells (192px) tall, bottom-center pivot. Floor is 19×14 cells (1 cell = 1 world unit). Gameplay zoom ≈ 8 world-units tall view → player at center sees mostly **open floor**; walls only frame the screen edges.

Target aesthetic: Diablo/Hades-like enclosed lit dungeon.

**USER FEEDBACK (verbatim, translated):** "the walls still aren't what I wanted — I feel like I'm looking from too far overhead; I can't feel like I'm INSIDE a room."

## Q1 — "FEEL INSIDE A ROOM" (the priority)
How do top-down 3/4 action games create the visceral "I'm standing INSIDE an enclosed hall" feeling — WITHOUT tilting the camera or redrawing characters at a lower angle? Survey + rank the levers, with concrete refs:
- **Wall height & proportion** — how tall (relative to screen / player) must walls read? Do Hades/Children of Morta/Diablo/CrossCode/Tunic/Moonlighter exaggerate wall height?
- **Foreground / near-wall occlusion** — the South/front wall drawn IN FRONT of the player, occluding the bottom of the screen (player walks "behind" the bottom wall). How much does this sell "inside"? (Hades does this heavily.) Tradeoff vs the current open-void floating-island front.
- **Room size vs camera zoom** — is the current room too big / camera too zoomed-out, so walls leave the frame? Tighter room or closer walls?
- **Ceiling / overhang hints, top-wall thickness, vignette/edge-darkening, light cone from above.**
- The **floating-island (open front, void edges) vs enclosed-room (walls all around)** tension — the canon had open front; the user now wants enclosed. How to reconcile (e.g., enclosed back/sides + a low foreground front wall + a couple of void gaps that still read "floating")?

Deliver a RANKED list (highest feel-per-effort first) of concrete changes, each with a ref and the tradeoff.

## Q2 — DETAILED IN-GAME PAUSE EDITOR (UX)
We have an F2 in-play map-paint overlay (currently a plain text-button IMGUI palette). The user wants: **press F2 → game PAUSES ("remote edit mode") → a DETAILED editor screen where the placeable ITEMS/TYPES are shown VISUALLY (thumbnail of each tile/prop sprite, categories, selected-item preview)** — not just text buttons.
Survey UX patterns for good in-game pause level editors / placement palettes (RPG Maker, Super Mario Maker 2, Core/Crayta, Townscaper, Dreams, Eternal/Tiled-style, any ARPG with a build/edit mode). What makes the palette readable & fast: thumbnail grid layout, category tabs, search, hover-preview, selected-highlight, brush-size, paused-dimmed-game backdrop, hotkeys. Give a concise dos/don'ts + a recommended layout for a thumbnail-based pause-edit palette.

Keep it tight and decision-useful. Opus will make the final call.
