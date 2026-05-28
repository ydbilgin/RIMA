# Kit A+B+C Logical Layout Analysis — Codex (gpt-5.5)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Kit A (floor) + Kit B (cliff face) + Kit C (parallax BG) bir araya gelirken Unity engineering doğru mu? PPU, sorting, parallax math, prefab strategy, performance budget. Antigravity (Gemini 3) paralel visual/design verdict veriyor. Ben (Opus) sentezleyeceğim.

## RIMA TECHNICAL CONTEXT
- Unity 6, URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- PPU=64, Grid cellSize=(1, 0.5, 1), CellLayout=Isometric
- Camera: orthographic, high top-down 3/4 ~70-80°
- Active scene: `Assets/Scenes/Test/PlayableArena.unity`
- **OVERLAP BUG:** Kit A floor diamond footprint is 62×39 px not 64×32 dimetric. See `STAGING/s106_overnight/iso_overlap_test.png`.
- Existing 3-Kit BG architecture verdict: `STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md` — sorting/parallax baseline

## ASSETS

### Kit A floor (Unity-imported, LIVE)
- `Assets/ScriptableObjects/Floor/IsoTiles35/tile_*.asset` (16 tiles)
- Sprites: `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/tile_*.png` (64×64, PPU=64)

### Kit B cliff face (ref only, NOT yet in Unity)
- `STAGING/s106_overnight/ref_kit_b/cliff_{N,S,E,W,NE,NW,SE,SW,cyan_glow}.png` — all 1024×1536 HD

### Kit C parallax BG (ref only, NOT yet in Unity)
- `STAGING/s106_overnight/ref_kit_c/bg_{L0_void,L1_nebula}.png` — 1254×1254
- `bg_{L2_ruins_A,L2_ruins_B,L4_fog}.png` — 1672×941
- `bg_{L3_island_small,L3_island_large}.png` — 1254×1254 with alpha

### Reference composition
- `STAGING/s106_overnight/kit_abc_logical_mockup.png` — Python composite, 1280×720, all 3 kits layered for scale/placement reference

## ANSWER ALL 8

### 1. PPU per kit (target import setting)
Concrete numbers + rationale.
- Kit A: PPU=64 ✓ locked
- Kit B (each cliff face): recommend PPU value + final Unity sprite size + world units. Show math.
- Kit C (each BG layer): recommend PPU per asset + world units. Note: parallax layers usually have very low PPU to render large.

### 2. World-space size & placement
For an 12×8 cell arena (12 wide, 8 deep iso):
- Arena footprint in world units (X, Y)
- Cliff face placement: how many cliff sprites per edge? Spacing? World position offsets?
- BG layer Z-depth values (parallax factor)
- Camera orthographic size + position

### 3. Sorting & parallax math (Unity-specific)
Provide table:
```
Layer            | Sorting Layer | Order in Layer | Z position | Parallax factor
Kit C L0 void    |               |                |            |
Kit C L1 nebula  |               |                |            |
Kit C L2 ruins   |               |                |            |
Kit C L3 islands |               |                |            |
Kit C L4 fog     |               |                |            |
Kit A floor      |               |                |            |
Kit B cliff face |               |                |            |
Player/enemies   |               |                |            |
```
Plus parallax script formula (camera Δ → layer Δ).

### 4. Overlap fix — Unity-side
Options:
- A) Change Grid cellSize to (1, 0.61, 1) — keep tile content, adjust math
- B) Re-import tile sprites with new pivot to compensate
- C) Regenerate Kit A tiles at proper 2:1 dimetric (64×32 footprint via PixelLab)
Which is fastest, which is correct long-term? Code snippet for fastest fix.

### 5. Pixel filter — should we PIXELIFY Kit B/C HD refs?
HD ChatGPT-style refs vs pixel art floor → 2 paths:
- a) Pixelify Kit B/C through PixelLab Style Reference (init image, AI Freedom=0) — pixel-perfect match
- b) Keep HD as parallax BG, intentional Octopath-style mixed pipeline
Engineering trade-offs: file size, draw calls, sprite atlas packing, Pixel Perfect Camera interaction.

### 6. Prefab strategy
Should each Kit B/C asset be a prefab? Tile asset? Sprite directly?
- Cliff face: GameObject with SpriteRenderer or part of a Tilemap?
- BG layers: large quad sprite or ScrollingBackground component?
Code structure recommendation.

### 7. Performance budget
- Draw calls per room (Combat/Ritual/Boss)
- Sprite Atlas grouping plan
- Pixel Perfect Camera target res + ref res

### 8. Production order (concrete steps)
1. Fix overlap → which option, code snippet
2. Import Kit B/C → which sprites first, into which folders
3. Set up parallax — which script, where attached
4. Build sample arena — which scene, which prefab order
5. Test → which checks to run

## DELIVERABLE
- Single response file: `STAGING/s106_overnight/KIT_ABC_LAYOUT_VERDICT_CODEX.md`
- 800-1200 words
- Final line: `VERDICT: <CONFIG_VALUES_SUMMARY>` (e.g., `VERDICT: KitB PPU=192 / KitC PPU=8 / cellSize y=0.61 / Sorting Group per room`)
