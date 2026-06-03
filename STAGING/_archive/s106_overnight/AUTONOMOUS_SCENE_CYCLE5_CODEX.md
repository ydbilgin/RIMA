# Scene Composition Cycle 5 — Ground-Shaped Lighting + Collision + Parallax Wire

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: User user feedback (Turkish):
> "çok karanlık. ground'umuz olduğu kadar bi aydınlık yapalım. ground yani tile'lar harici bir yere gidilemeyecek. layerları doğru ayarla, parallax'lar her map'te türüne göre olacak. Ana bileşen ground olacak, onun altında derinlik hissini Kit B ile yapacağız."

Translation: Too dark. Brightness should match WHERE the ground exists (painted area lit, void beyond stays dark — spotlight effect). Player CANNOT leave the painted floor area. Layer architecture correct: ground primary, Kit B (cliff) below for depth feel, Kit C parallax behind.

**PREREQUISITE STATE:** Cycle 4 (task `bfgu1gvqz`) is running in parallel. It applies: Global 0.22→0.38, Portal 2.5→5.0, Brazier 2.2→4.5, Bloom enable. You will pick up scene AFTER Cycle 4 saves. If Cycle 4 didn't run or state is unclear, apply its tweaks first OR check current values.

## 3 PRIMARY TASKS

### Task 1 — Ground-shaped area lighting (PRIORITY)
The user wants the playable area LIT BRIGHT, void/beyond DARK. Like a Hades spotlight on the arena.

**Implementation:**
1. Set `Global Light 2D` intensity to **0.08** (very dim, void darkness)
2. Create new GameObject "ArenaAreaLight" with `Light2D` component:
   - **Type: Freeform Light2D** (URP 2D)
   - **Shape:** polygon outlining the painted floor area (use TilemapCollider2D from Task 2 if generated, OR compute from painted cell perimeter)
   - **Color:** warm white (1.0, 0.95, 0.85)
   - **Intensity:** 1.2-1.5
   - **Falloff:** 0.3 (sharp edge, slight feather)
   - **Target Sorting Layers:** all gameplay layers (Default, Ground, Floor, Decals, Walls, Entities, BackwallLandmark, Characters, Props)
3. Existing brazier + portal + rim lights stay (they boost on top of the area light)
4. Result: only the painted floor is lit; beyond is pitch dark with brazier/portal hot spots glowing

**Alternative if Freeform Light2D not feasible:** Use Sprite Light2D with a polygon sprite mask matching the painted area, OR multiple overlapping Point Light2Ds across painted area (4-6 covering all corners).

### Task 2 — Floor boundary collision
Player cannot walk OFF the painted floor (no falling into void).

**Implementation:**
1. On the Tilemap GameObject (currently named "Tilemap" under "Floor"), add:
   - `TilemapCollider2D` — but in INVERSE mode (or use a `CompositeCollider2D` strategy)
   - `Rigidbody2D` (BodyType: Static)
   - `CompositeCollider2D` (CompositionType: Outline, GenerationType: Synchronous)
   - Set `TilemapCollider2D.usedByComposite = true`
2. The TilemapCollider blocks the PAINTED cells (player blocked by tiles) — we don't want this. We want INVERSE: void blocked, ground walkable.
3. **Approach:** create a SECOND TilemapCollider2D on a NEW "VoidBlocker" Tilemap:
   - Iterate the bounding rect of painted cells, expand by 2 cells in each direction
   - For each cell in expanded rect, if the original Floor tilemap has NO tile there, paint a `VoidBlocker` tile (simple invisible/transparent tile)
   - Add TilemapCollider2D + CompositeCollider2D + Rigidbody2D Static
   - The Composite Outline collider will form a wall along the painted area's perimeter
   - VoidBlocker's tiles can be invisible (use any TileBase asset — color tinted transparent), OR use a custom Tile with `null` sprite but enabled collider
4. Verify: Player at painted area center, try to move outside → should hit wall

### Task 3 — Wire ParallaxLayer.cs to Kit C children
The script exists at `Assets/Scripts/Background/ParallaxLayer.cs`. Wire it to RoomBackgroundRig children with per-layer factors per Codex verdict.

**Implementation:**
For each child of RoomBackgroundRig, add `RIMA.Background.ParallaxLayer` component (use `AddComponent<>` or type lookup if class assembly not auto-detected):

| Child | Factor.x | Factor.y |
|---|---|---|
| L0_Void | 0.03 | 0.02 |
| L1_Nebula | 0.05 | 0.04 |
| L2_Ruins | 0.08 | 0.05 |
| L3_Island_Small | 0.14 | 0.08 |
| L3_Island_Large | 0.14 | 0.08 |
| L4_Fog | 0.10 | 0.06 |

- Set `target` = Camera.main
- Set `snapToPixel` = true
- Set `pixelsPerUnit` = 64
- ExecuteAlways means parallax works in Editor too — Codex can verify by moving camera in Scene view

## OTHER NOTES

- Layer architecture is correct already (per user):
  - Ground (Kit A floor) = primary, on "Floor" sortingLayer
  - Kit B (cliff face) = under ground on "Ground" sortingLayer (order -50) — deepens visual
  - Kit C (parallax BG) = even deeper on "Ground" sortingLayer (order -800..-260)
  - Player = "Characters" sortingLayer
  - Per-map parallax means user will vary L0-L4 factors per arena type later
- Per-map variation = scope creep, NOT this dispatch. Just wire the script with the default verdict factors.

## PROCESS

1. Phase 0: Read prereq + current scene state
2. Phase 1: Task 2 first (collision) — generates VoidBlocker tilemap, gives perimeter polygon
3. Phase 2: Task 1 (lighting) — use the perimeter polygon for Freeform Light2D shape
4. Phase 3: Task 3 (parallax wire)
5. Phase 4: Tone check; if scene still too dark, raise ArenaAreaLight intensity to 1.8
6. Phase 5: Save scene, screenshots, report

## DELIVERABLES

- Modified `Assets/Scenes/Test/PlayableArena.unity`
- New `Assets/Scenes/Test/PlayableArena.unity` with:
  - VoidBlocker Tilemap GameObject (with Composite collider)
  - ArenaAreaLight GameObject (Freeform Light2D)
  - ParallaxLayer components on all 6 BG rig children
- `STAGING/s106_overnight/scene_v6_match_attempt.png` (1280×720)
- `STAGING/s106_overnight/scene_v6_collision_test.png` — Scene view showing collider gizmos (Scene view screenshot with gizmos visible)
- `STAGING/s106_overnight/SCENE_V6_REPORT.md`:
  - Confirm 3 tasks done with metrics:
    - Lighting: ArenaAreaLight type, intensity, polygon point count
    - Collision: VoidBlocker tilemap cell count, composite collider path count
    - Parallax: 6 ParallaxLayer components attached, factors verified
  - Console 0/0
  - Self-assessment vs M3 (1-10)

## CONSTRAINTS
- NO PixelLab API calls
- 0 error 0 warning required
- Don't disturb other Cycle 1-4 outputs (CliffRing, BG layer positions, lighting setup beyond the spec)
- The Freeform Light2D shape should follow the painted floor outline; if too complex, use Sprite Light2D with a generated polygon sprite OR multiple Point lights
- Single scene save at end

## TIME ESTIMATE
~60-90 min at xhigh.

After DONE: Opus reviews. This may be the final SHIP-cycle (lighting + collision + parallax all done, brightness sorted, design philosophy applied).
