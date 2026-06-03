ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed scope (4) BLOCKED if new tiles missing.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: User pointed out b340684f tileset generated at view_angle=90 (flat) instead of 35° (high top-down). Antigravity vision eval confirmed: chatgpt_ref tiles have 3-4px vertical y-offset perspective baked in, our 90° tiles are flat circle blobs. Regenerated at angle=35° as PixelLab tileset `451bbfd8-bb7c-4778-8643-caa95ffddf97`. This task downloads new tiles, archives old, repaints PlayableArena.unity with new 35° tiles, applies Stream N planned fixes (weighted random + camera follow + even resolution).

---

# STREAM O — ISO TILE 35° INTEGRATION + ARENA FINALIZE

## ⚠️ Phase 0 — Verify new tileset exists (5 min, MANDATORY)

Call PixelLab API to confirm new tileset is complete:
```bash
# From shell/MCP (if accessible) OR document the curl call needed
curl https://api.pixellab.ai/mcp/tiles-pro/451bbfd8-bb7c-4778-8643-caa95ffddf97/download
```

OR use UnityMCP `mcp__pixellab__get_tiles_pro` tool (if available in your env).

Verify:
- Status: completed
- 16 tile URLs returned
- view_angle in metadata = 35.0 (confirm not the old 90°)

If tileset not ready yet → WAIT 30-60s and retry. If after 5 min still processing → write WAITING status, do not proceed.

## Phase 1 — Download 16 new tiles (10 min)

### 1A. Create target directory
`Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/`

### 1B. Download all 16 PNGs
URL pattern: `https://backblaze.pixellab.ai/file/pixellab-tiles/f587b47a-7c0e-4f37-a6c9-7d311a2c935f/451bbfd8-bb7c-4778-8643-caa95ffddf97/tile_{N}.png` for N=0..15

Use PowerShell `Invoke-WebRequest` in a single batch (16 calls).

### 1C. Configure sprite imports
For each PNG:
- Texture Type: Sprite (2D and UI)
- PPU: 32 (since 64×64 PNG with diamond at 64×32 effective = 2 cells wide → 32 PPU = 2 units = 2 cells)
- Filter Mode: Point (no filter)
- Compression: None
- Pivot: Center
- AssetDatabase.Refresh at end

### 1D. Create Unity Tile assets
At `Assets/ScriptableObjects/Floor/IsoTiles35/tile_{N}.asset` for each:
- ScriptableObject.CreateInstance<Tile>()
- tile.sprite = imported sprite
- tile.colliderType = Tile.ColliderType.None

## Phase 2 — Archive old 90° tiles (5 min)

Move (NOT delete) old assets to archive:
- `Assets/Sprites/AssetPackV3/floor_iso_pixellab/` → `Assets/_archive~/pre_s106_stream_o_tiles90/floor_iso_pixellab/`
- `Assets/ScriptableObjects/Floor/IsoTiles/` → `Assets/_archive~/pre_s106_stream_o_tiles90/IsoTiles/`

This preserves rollback path without leaving stale assets in active asset tree.

## Phase 3 — Repaint PlayableArena.unity (15 min)

Open `Assets/Scenes/Test/PlayableArena.unity`. Apply:

### 3A. Clear existing tilemap
`tilemap.ClearAllTiles();`

### 3B. Weighted repaint with new 35° tiles
Per Phase 1 inspection, identify tile theme mapping (per b340684f description pattern):
- Tiles 0-3: cobblestone base (clean)
- Tiles 4-7: cyan veins variant
- Tiles 8-11: dirt variant
- Tiles 12-15: ritual rune variant

(Verify by visual inspection — themes may be in different order.)

Weighted random:
- **80% base** (tiles 0-3 random)
- **15% dirt** (tiles 8-11 random)
- **3% cyan veins** (tiles 4-7 random)
- **2% ritual** (tiles 12-15 random)

Use `Random.InitState(2026)` for deterministic.

22×16 cell rectangle (same as Stream M). SetTiles batch API in single AssetDatabase batch wrap.

### 3C. Verify floor renders correctly
- Diamond pattern visible
- Tiles continuous (no gaps)
- Random distribution matches weight rule

## Phase 4 — Camera + CameraFollow (10 min)

If Stream M completed Phase 2 partially, verify state. Else create fresh.

### 4A. Camera config
- Main Camera ortho size: **5** (close, game-like)
- Background: dark navy `RGB(8, 10, 16)`
- Pixel Perfect Camera: PPU 32 (matches new tile import PPU)
- Reference Resolution: **1280×720** (16:9 EVEN — no more odd resolution)

### 4B. CameraFollow.cs
Create `Assets/Scripts/Camera/CameraFollow.cs` if not exists:
```csharp
using UnityEngine;
namespace RIMA.CameraSystem
{
    [DefaultExecutionOrder(100)]
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        [Range(0.05f, 1f)] public float smoothTime = 0.15f;
        public Vector3 worldOffset = new Vector3(0f, 0f, -10f);
        private Vector3 currentVelocity;
        void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + worldOffset;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref currentVelocity, smoothTime);
        }
    }
}
```

Attach to Main Camera. Drag Player transform as `target` (via editor script setting since this runs in batch mode).

## Phase 5 — Player verification (5 min)

If Player not in scene:
- Instantiate `Assets/Prefabs/Player.prefab` at (0, 0, 0)
- SpriteRenderer.material = `Sprite-Lit-Default`
- SortingLayer = Default, sortingOrder = 0

Verify WASD movement works (check PlayerMovementController.cs is attached and `speed > 0`).

## Phase 6 — URP 2D Lights (5 min)

If not already set up:
- Global Light 2D: color `#1A2030`, intensity 0.35
- 4 Point Lights:
  - 2 warm torch `#FFA040` intensity 0.9 range 4 at NW + NE
  - 2 cyan crystal `#00FFCC` intensity 0.7 range 3 at SW + SE

## Phase 7 — Screenshot (5 min)

Take play-mode screenshot at **1280×720 (even)**:
- `STAGING/s106_overnight/playable_arena_35deg_v1.png`

Take SceneView gizmo screenshot (lights visible):
- `STAGING/s106_overnight/playable_arena_35deg_gizmo_v1.png`

## Phase 8 — Report

Write CODEX_DONE_<profile>.md:

```
# STREAM O - ISO TILE 35° INTEGRATION - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 0 — Tileset verification
- ID 451bbfd8: status=completed y/n
- view_angle confirmed 35: y/n
- 16 URLs received: y/n

## Phase 1 — Download + import
- 16 PNGs in floor_iso_pixellab_35deg/: y
- 16 Tile assets in IsoTiles35/: y
- PPU 32 + Point filter + No compression: y

## Phase 2 — Archive
- Old 90° tiles moved to _archive~/pre_s106_stream_o_tiles90/: y/n

## Phase 3 — Repaint
- Tiles cleared: y
- Weighted repaint 80/15/3/2: y
- Cells painted: N (target 22×16 = 352)

## Phase 4 — Camera
- Ortho size 5: y
- CameraFollow.cs created/exists: y/n
- Target = Player: y/n
- Reference resolution 1280×720: y/n

## Phase 5 — Player
- In scene: y
- Sprite-Lit-Default: y
- WASD verified in play mode: y/n

## Phase 6 — Lighting
- Global Light 2D: y
- 4 Point Lights: y

## Phase 7 — Screenshots
- playable_arena_35deg_v1.png (1280×720): y/n
- playable_arena_35deg_gizmo_v1.png: y/n

## Self-rating
- chatgpt_ref floor likeness: N/10 (target 7+ with new 35° tiles)
- What's different vs Stream J (90° tiles, 5.6/10): <list>

## Compile check
- 0 errors, 0 warnings

## Time: N min
```

---

## Safety constraints (HARD)
- ❌ Don't modify Player.prefab or movement scripts
- ❌ Don't touch other PainterTestE scenes
- ❌ NO walls, NO room generator, NO objects beyond Player
- ❌ Don't DELETE old 90° tiles, MOVE to archive
- ✅ AssetDatabase batch wrap
- ✅ Single Refresh per phase
- ✅ Even resolution screenshots (1280×720)
- ✅ Deterministic seed (2026)

## Estimated total: 50-70 min
