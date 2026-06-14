# MAP PLACEMENT DIAGNOSE + CORRECT-LAYOUT SPEC (ax / Gemini)

ACTIVE RULES: (1) think before answering (2) concrete values, no fluff (3) answer ONLY what's asked (4) say UNKNOWN if unsure.
NLM ACCESS: query RIMA design canon first if needed:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

## Context
RIMA = 2D top-down 3/4 ARPG (Hades/Children-of-Morta camera, NOT isometric, NOT true 45deg diamond). PPU 64.
We are building a Townscaper-2D MAP TOOL: select -> place -> auto-connect walls (Wang). A wall cell resolves to a
shape (Single/End/Straight/Corner/T/Cross) + a rotation via a Wang 4-neighbor resolver. The ROTATION MATH was just
fixed and verified (CCW table matches Unity Quaternion.Euler(0,0,+theta); 20/20 unit tests + 12/12 runtime checks pass).

PROBLEM (user, frustrated): even with the math fixed, composing a room still does NOT produce a proper-looking
connected MAP LAYOUT on screen. We need to find WHY the visual map is wrong and get exact placement rules.

## Known facts that may be the root cause
- `RoomConfig` (scene MonoBehaviour) defaults to `GridLayout.CellLayout.Isometric` and `cellSize = (0.94, 0.94, 1)`.
  The game art is TOP-DOWN 3/4, not iso. Walls are placed at grid cell centers. Suspicion: an ISOMETRIC grid
  lays cells out in a diamond, so a "rectangular room" of walls comes out skewed/diamond instead of top-down rect.
- Wall pieces are placeholders (grey-box squares) until real directional PixelLab sprites exist. Squares can't
  show rotation, so "are the angles right" is invisible with placeholders.
- Wall sprite pivot must be bottom-center; rotation is applied about the foot origin after positioning.

## Deliverable (tight, concrete — this becomes an execution spec for the Unity side)
1. TOP-3 most-likely root causes of "no proper map layout appears", ranked, each with the fix.
   Explicitly rule IN or OUT the Isometric-grid suspicion: for a TOP-DOWN 3/4 game, what should the Grid
   `cellLayout` and `cellSize` be so a rectangular wall ring reads as a proper room (Rectangle? what cellSize)?
2. The minimal correct WALL SPRITE SET for Townscaper-style auto-connect (how many distinct sprites, which shapes
   must be authored vs which are rotations/mirrors), and the canonical 0deg orientation each sprite should be drawn in
   so the CCW resolver rotates them correctly (End connects South@0, Corner connects S+E@0, T opens North@0 — confirm
   or correct these against standard top-down wall-tiling references).
3. A concrete PLACEMENT/COMPOSITION spec for a readable top-down 3/4 room: floor layer vs wall layer ordering,
   wall height/foreshortening, pivot, sort (this project uses Camera Transparency Sort = Custom Axis (0,1,0),
   single "Entities" layer, SpriteSortPoint.Pivot). What makes it feel "you are inside a room" vs "scattered tiles".
4. 2-3 reference games/tools for top-down 2D map/wall auto-tiling we should match (Townscaper is 3D; what is the
   correct 2D top-down analog — e.g. RPG Maker auto-tile, Tiled Wang sets, Enter the Gungeon / Hades room editors).

Keep it to specifics we can act on. No code. End with a one-paragraph "DO THIS FIRST" recommendation.
