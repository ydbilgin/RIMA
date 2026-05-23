ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethayday.md AS THE VERY LAST STEP.

# Codex Task — HD-2D Hybrid Unity Technical Review

## ACTIVE RULES
(1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## NLM ACCESS
If you need RIMA design context, query NLM first via:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## Amaç
User wants to pivot RIMA from pure 2D pixel art pipeline to **HD-2D Hybrid pattern** (like Octopath Traveler, Triangle Strategy, HD-2D Dragon Quest III):
- **Characters:** stay 2D pixel art sprites (PixelLab pipeline locked — Karar #100 chibi 64px)
- **Environment (walls + floor):** modeled in 3D (Blender or Unity ProBuilder), textured with 2D pixel art
- **Camera:** orthographic at iso angle (~85-90° tilt), Pixel Perfect Camera
- **Lighting:** 3D scene lights interact with 2D sprites

Evaluate Unity technical feasibility, complexity, and provide implementation outline.

This runs IN PARALLEL with Opus design verdict (rima-design agent handles design judgment). Your job: pure Unity tech.

## Files for context
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Rooms/` — existing scaffolded room scripts (RoomTemplate, OverlayAnchor, DecorCategory, RoomDecorationSpawner). Will any of this still apply?
- `F:/Antigravity Projeler/2d roguelite/RIMA/CURRENT_STATUS.md` — S102 status with Option C lock
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/architecture_decision.md` — current locked Option C
- `F:/LaurethStudio/05_RESEARCH/3d_pipeline_tooling.md` — studio 3D tooling research (TRELLIS, Blender MCP, etc.)

## Your task — Unity Tech Review

### 1. Camera setup for HD-2D
- Orthographic vs Perspective camera?
- Tilt angle for iso top-down (85-90°)?
- Pixel Perfect Camera component compatibility with 3D scene?
- Multiple cameras (e.g., one for 3D bg, one for 2D UI)?
- URP 2D Renderer (currently in use) vs URP 3D Renderer — can we use both? Switch?

### 2. Sprite-on-Mesh / 2D character in 3D world
- Sprite Renderer with `Sorting Order` for character?
- How does Y-sorting work with 3D environment? (Sprite must sort against 3D objects in front/behind)
- Billboard shader OR fixed-rotation sprite?
- Shadow casting: do 2D sprites cast shadows on 3D walls? (Octopath Traveler does this)
- Lighting: how does a 2D sprite receive light from a 3D point light? (custom shader vs built-in URP 2D)

### 3. 3D wall mesh + 2D pixel art texture pipeline
- Blender → FBX → Unity vs Unity ProBuilder native?
- Texture filter mode: Point (no filter) for pixel art preservation
- Texel density at iso angle (texture stretches/squashes when wall is tilted)
- Seamless/tileable texture pipeline (existing PixelLab Wang tilesets relevant?)
- Trim sheet vs full-wall texture
- UV mapping considerations for irregular dungeon shapes

### 4. Lighting system
- URP 2D lights (currently locked S59) vs URP 3D forward+ renderer
- Can we mix? Or full switch to 3D renderer with 2D-style materials?
- Real-time vs baked lighting for dungeon torches
- Sprite lighting: how Octopath handles "sprite catches light from 3D source"

### 5. Pipeline complexity for solo dev
- Estimated LOC for new pipeline (vs existing 2D pipeline)
- Blender learning curve (assume basic) vs ProBuilder (in-Unity)
- Asset production: 3D wall kit + 2D texture (number of unique assets needed)
- Iteration speed: change a wall shape — how many steps?

### 6. Integration with existing scaffolded scripts
- `Assets/Scripts/Rooms/RoomTemplate.cs` — does it still apply? Modify how?
- `OverlayAnchor.cs` and decor placement — still valid for 2D decor in 3D world?
- Anchor positions in 3D space (Vector3 vs Vector2)?
- RoomDecorationSpawner — adapt for 3D scene?

### 7. Performance & polish
- Draw call estimation per dungeon room
- Mesh count for typical dungeon
- Z-fighting risks at wall joins
- Pixel art crispness at iso angle (mipmap, scaling)
- Common HD-2D gotchas (research Octopath / Triangle Strategy postmortems briefly)

### 8. Implementation outline (if pivot ADOPT)
5-7 concrete Unity steps to validate this approach in a 1-day proof slice:
1. Setup URP 3D renderer or hybrid
2. Build 1 modular wall in ProBuilder
3. Apply tileable PixelLab texture
4. Setup orthographic camera at iso angle with Pixel Perfect
5. Place test 2D character sprite (existing warblade)
6. Add 1 dynamic light (point light = torch)
7. Test: looks like Octopath? Looks like chatgpt_ref?

### 9. Verdict
- **ADOPT** / **SKIP** / **RESEARCH_MORE** from pure tech feasibility perspective
- Top 3 tech risks
- Top 3 tech wins

## Output

Write report to: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/codex_hd2d_tech_review.md`

Structure:
```
# Codex HD-2D Tech Review

## Verdict
Choice: ADOPT/SKIP/RESEARCH_MORE
Confidence: low/med/high
Top 3 wins / Top 3 risks

## Tech analysis
[Per question 1-7, brief but concrete]

## Implementation outline (5-7 Unity steps)
[Concrete, with file paths and component names]

## Code-level risks
[What could go wrong from engineering side]
```

Target 500-800 words. No actual code in the report — just analysis + outline.

## Constraints
- Do NOT write code or modify Unity files
- Do NOT contradict character lock
- Output ONE report file only
- Don't redo design (Opus handles), focus on Unity tech


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethayday.md AS THE VERY LAST STEP.