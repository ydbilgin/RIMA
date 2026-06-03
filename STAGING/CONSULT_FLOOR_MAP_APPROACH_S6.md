# CONSULT — RIGHT floor/map approach for RIMA: tile-grid vs hand-painted map vs hybrid

Concise, decisive markdown. The user is unhappy: our floor "still doesn't look like a proper map" — it reads as a repetitive tile GRID.

## CONTEXT
RIMA = 2D top-down 3/4 action-roguelite (Unity URP 2D, PPU64, camera flat-ortho high-top-down). We generated a PixelLab `create_topdown_tileset` (16-tile Wang, 32px, "high top-down", "selective outline", "detailed shading") for the dungeon floor and painted the floor Tilemap with the single SOLID tile, repeated. Result: a visible repetitive grid (every cell has a selective-outline border → grid lines everywhere), featureless beyond the repeat. User reaction: "not a proper map, I don't understand — you made it a tileset, is this how we were supposed to do it?"

KEY TENSION: the user ALSO wants a real in-engine **map EDITOR** where they place objects/tiles to BUILD a map (modular). A pure single hand-painted map image is NOT tile-placeable; a tile grid is editable but looks grid-y. Past research: the team liked Sang Hendrix's "Realtime Parallax Map Builder" (hand-painted layered parallax maps, RPG-Maker-style, NOT obvious grids) and the chatgpt_ref dungeon look (cohesive, painted, no visible grid).

## QUESTIONS (be decisive)
1. **Which approach for RIMA room floors — and is "tileset" what we should be doing?**
   - (A) Tile-grid done RIGHT: which PixelLab params kill the grid look — "lineless" outline (no per-tile border)? multiple floor VARIANTS (4-8) randomly scattered? larger tiles? Plus a DECAL layer (cracks/stains/rubble/moss) painted on top to break repetition. Does this get to "proper map"?
   - (B) Hand-painted whole-room map (Sang Hendrix / parallax style): one large painted floor image (or few layers) per room, no grid — cohesive "real map" look. How does this coexist with a tile/object PLACEMENT editor (it doesn't tile)? Is it authored differently (paint base, place props on top)?
   - (C) HYBRID (likely answer): a low-contrast tiled or painted BASE (lineless, varied) + heavy decal/prop/overlay layer placed by the editor + unified lighting/color grade. This is what most "real" top-down games (Hades, CoM, Moonlighter) actually do. Confirm or correct.
   Give a clear RECOMMENDATION + WHY, reconciling "proper-map look" WITH "place-objects-to-build-map editor."
2. **Concrete PixelLab workflow for the chosen approach:** which PixelLab tool(s) + exact settings (outline=lineless? detail? view? tile_size? how many floor variants? a separate decal/prop set?) to produce a floor that reads as a proper map, not a grid. Minimal gen count (the user is cost-conscious — we spent only 8 gens total so far, keep it lean).
3. **The grid-line problem specifically:** is the "selective outline" the main culprit (per-tile borders)? Should floor tiles be "lineless" + rely on shading/decals for definition?

Keep it tight + decision-useful. Opus makes the final call + implements.
