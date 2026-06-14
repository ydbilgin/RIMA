# CX (Codex) CONSULT — Unity-implementable "inside a room" feel + detailed pause-editor plan

ACTIVE RULES: (1) think before answering (2) concrete & minimal (3) flag tradeoffs vs the floating-island canon (4) BLOCKED if unclear.
You are a senior Unity 2D engineer. Two questions. Output concise markdown with concrete, implementable steps + a ranked recommendation. You MAY read the files named below. Do NOT write code files — this is a PLAN/consult; Opus implements.

## SHARED CONTEXT
RIMA = 2D top-down 3/4 action-roguelite (Unity URP 2D Renderer, PPU64, Custom-Axis (0,1,0) Y-sort, SpriteRenderer Sort Point = Pivot, single "Entities" sorting layer).
**HARD CONSTRAINTS:** camera FLAT orthographic, NO tilt (breaks cursor-aim/sprite-scale/Y-sort — locked); 80 character sprites drawn at ~70-80° top-down 3/4, CANNOT redraw lower. So "inside a room" must come from ART/LAYOUT/ZOOM/LIGHTING.
READ to ground yourself: `STAGING/RUINED_KEEP_ROOM_LOOK_LOCK_S6.md`, `STAGING/RUINED_KEEP_SEGMENT_DATA_S6.md`, `Assets/Scripts/DevTools/WallRunBuilder.cs`, `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`.

Current state: connected wall-runs built via `WallRunBuilder.BuildRun` — N continuous + cyan arch, E/W lit runs + void gaps, **S = open void (floating-island front)**. Walls = flat-front sprites 64×192 (3 cells tall), bottom-center pivot. Floor 19×14 cells (1u each). Gameplay camera ≈ ortho 4 (8u tall view), PixelPerfectCamera currently disabled. Player centered → sees mostly open floor; walls only at edges.
**USER FEEDBACK (translated):** "walls still not right — feels too top-down/overhead, can't feel INSIDE a room."

## Q1 — Concrete Unity changes to feel ENCLOSED / "inside a room"
Given the locked flat-ortho top-down 3/4, give a RANKED list of implementable changes (highest feel-per-effort first), each with the exact Unity mechanism + tradeoff vs the floating-island canon. Evaluate at least:
1. **Taller walls** — scale the run sprites taller (e.g. ×1.3-1.6) / generate taller variants? Effect on Y-sort + occlusion.
2. **Tighter playable area / closer walls** — shrink the wall perimeter inward (e.g. ~13×10 instead of 19×14) so walls frame the player at gameplay zoom, without re-painting the full floor (or shrink floor too). Mechanism via the existing segment data / BuildRun cells.
3. **FRONT (south) foreground wall + occlusion** — add a low S wall drawn IN FRONT of the player (higher sortingOrder / lower Y) so the bottom of the screen is framed and the player walks "behind" it. How to do this with Custom-Axis Y-sort (a wall whose pivot is BELOW the player's Y, or a dedicated foreground sort). Tradeoff vs open-void front.
4. **Camera zoom** — is ortho 4 too zoomed-out or too-in? Recommend a value (note PixelPerfectCamera 576×324 vs 480×270 lock). Should PPC be re-enabled?
5. **Edge vignette / lighting** — darken screen edges + a top-down light cone to imply walls/ceiling.
Resolve the floating-island-vs-enclosed tension concretely (e.g. enclosed back+sides + low foreground S wall + 1-2 void gaps that still read "floating").

## Q2 — Upgrade F2 overlay → DETAILED PAUSE editor with VISUAL palette
The user wants: F2 → **pause the game** (Time.timeScale=0, restore on exit) → a **DETAILED editor** where placeable types are shown as **sprite THUMBNAILS** (not text buttons), with categories + selected preview.
`InPlayMapPaintOverlay.cs` is currently IMGUI text-button palette (already has drag-place, Prop mode, undo, Compose button). Give an implementation plan:
- **Pause:** set `Time.timeScale=0` on F2-open, restore previous on close; what to watch (Input System unaffected by timeScale? audio? physics).
- **Thumbnail palette — pick the path:** (a) stay IMGUI but draw sprite thumbnails via `GUI.DrawTextureWithTexCoords` (sprite.texture + sprite.rect/texture-size for atlas UV) in a grid of toggles; (b) migrate to **uGUI** (Canvas + GridLayoutGroup + Image buttons); (c) **UI Toolkit** (UXML/USS). Recommend ONE with reasoning (this is a dev tool; IMGUI-thumbnail is lowest-friction — confirm or override). 
- **Layout:** category tabs (Floor / Cliff / Wall / Prop), scrollable thumbnail grid, selected-highlight, larger preview of the selected type, keep the existing drag-place/undo/Compose. 
- Concrete change list to `InPlayMapPaintOverlay.cs` (which methods: `OnGUI`, palette structs — currently `_palette`/`_propPalette` hold the assets, so thumbnails just need the sprite which is already there).

Keep it implementable & ranked. Opus makes the final call + implements.
