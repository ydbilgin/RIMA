# CX TASK — RIMA top-down 3/4 room style: CAMERA-DEPTH + WALLED Ruined-Keep rooms + edges (concrete Unity)

ACTIVE RULES: (1) think (2) concrete settings/numbers (3) respect locked constraints (4) note assumptions.

## CONTEXT (LOCKED)
RIMA = top-down 3/4 action-roguelite. Camera = flat 2D **orthographic**, rotation (0,0,0), **PixelPerfectCamera** assetsPPU=64, ref 640x360, upscaleRT on, **orthographicSize=4**. Cursor-aim. PPC overrides orthoSize (zoom via refResolution).

## TWO LOOK REFERENCES (view both)
- `STAGING/floor_perspective_concepts/03_wallless_improved.png` — wall-less floating-island edge, cyan rim-light.
- `STAGING/concepts/chatgpt_ref/` (esp. `ChatGPT Image 22 May 2026 16_12_46 (1).png` and `ChatGPT Image 25 May 2026 00_18_45 (2).png`) — these are RIMA HUD mockups of **WALLED, semi-enclosed broken-masonry "Shattered Keep" rooms** in top-down 3/4: stone perimeter walls + columns + banners + torches + central cyan rift-altar, dark floor. Also subfolders `blueprint_room/`, `new_chatgpt/`.

Canon = **Ruined Keep HYBRID**: floating-island foundation BUT semi-enclosed broken masonry (Hades Asphodel / Bastion). So "wall-less" is not absolute — rooms can have broken perimeter walls/columns with open gaps to the void.

User goal (verbatim intent): "get DEPTH via camera closeness/zoom; get the boundaries/walls right for top-down 3/4 but placed LOGICALLY; make my game beautiful."

## ANSWER (concrete) — these 5
1. **CAMERA for depth:** best zoom to feel close+dramatic+readable like the refs. Pick a concrete orthographicSize OR refResolution (state which lever is right under PixelPerfectCamera). Is a SUBTLE perspective FOV worth it vs pure ortho (cursor-aim impact)? Recommend a number.
2. **WALLED Ruined-Keep rooms in top-down 3/4 — CAN we, and HOW:** how to render perimeter walls/columns in top-down 3/4 (tall wall sprites with a visible front face, like the 128x192 cliff sprites? Y-sorted on "Entities"?), where the wall base sits vs the walkable collider, how to mix solid broken walls WITH open void edges in one room. Confirm feasibility + the sprite/sort/collider recipe.
3. **LOGICAL placement:** rules so walls/columns/altars read as a coherent room not random scatter — perimeter framing, entrances/gate gaps, central focal altar, symmetry vs organic, prop density. How RoomLoader/RoomPainter should place them.
4. **EDGES/boundaries:** where broken walls vs open void edges go; cyan rim-light on open edges; how the play-area limit stays clear.
5. **FLOOR look:** value/brightness (current placeholder tiles too dark), foreshortening, light-pools, decals.

Output concise markdown with NUMBERS + Unity component settings + a clear YES/NO + recipe for walled rooms. No code changes.
