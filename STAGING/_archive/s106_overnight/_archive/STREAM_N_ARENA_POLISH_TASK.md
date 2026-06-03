ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — single scene + camera follow (4) BLOCKED if scene corrupt.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: User feedback (verbatim): "saçma sapan bi görüntü var burada da daha güzel bağlayabiliyordum world painter la bağlayınca" — random tile mixing causes visual chaos (16 tiles span 4 themes: cobblestone/cyan-veins/dirt/runes; random = patchwork). PLUS "unitymcp ile hata alıyorum önce odd numbered resoulution var ayrıca oyun gibi biraz daha yakında olsun kamera oyuncuyu takip etsin" — camera too far, no follow, odd resolution.

**Stream M (`buhwvuhj0`) was KILLED** before this dispatch — scene `Assets/Scenes/Test/PlayableArena.unity` exists (saved 15:41) but may be in intermediate state. Stream N fixes everything.

---

# STREAM N — ARENA POLISH (Tile Weighting + Camera Follow + Resolution Fix)

## ⚠️ Phase 0 — Verify scene state (5 min, MANDATORY FIRST)

Open `Assets/Scenes/Test/PlayableArena.unity`. Check what's already there:
- Floor + Grid + Tilemap?
- Painted tiles (how many)?
- Lighting GameObjects?
- Player instantiated?
- Camera setup?

Write a 100-word state summary at top of CODEX_DONE. Then DECIDE:
- If scene has Floor + Tilemap + Player but tiles are randomly mixed → fix (Phase 1-4)
- If scene incomplete (e.g. no Player) → complete the missing parts AS WELL AS fix
- If scene corrupt → recreate from scratch using Stream M task spec as guide

## Phase 1 — Tile re-paint with WEIGHTED variant selection (10-15 min)

### 1A. Inspect tile_0..15 visually
Read each `Assets/Sprites/AssetPackV3/floor_iso_pixellab/b340684f_tile_<N>.png` (16 PNG total).

**Heuristic per b340684f description (from PixelLab API metadata):**
- 4 themes × 4 sub-variants:
  - Theme A: cobblestone (base, clean)
  - Theme B: cracked + cyan veins
  - Theme C: packed dirt + stone fragments  
  - Theme D: ritual rune + cyan accent

Likely mapping (verify by visual inspection):
- Tiles 0-3 → Theme A (cobblestone, base)
- Tiles 4-7 → Theme B (cyan veins)
- Tiles 8-11 → Theme C (dirt)
- Tiles 12-15 → Theme D (ritual)

(If actual mapping differs after inspection, document it in report.)

### 1B. Weighted selection rule
For each cell, pick tile with probabilities:
- **80% base** (cobblestone, tiles 0-3 — random within group)
- **15% dirt** (tiles 8-11 — adds organic variation)
- **3% cyan veins** (tiles 4-7 — accent, rare)
- **2% ritual** (tiles 12-15 — very rare accent)

This produces predominantly clean cobblestone floor with occasional dirt patches and very sparse cyan/rune accents — matches chatgpt_ref visual rhythm (not 25% cyan everywhere).

### 1C. Clear existing tiles + repaint
```csharp
tilemap.ClearAllTiles();
// Build positions + tiles arrays with weighted selection per cell
// SetTiles batch call (single API call)
AssetDatabase.StartAssetEditing();
try { tilemap.SetTiles(positions, tiles); EditorSceneManager.MarkSceneDirty(scene); }
finally { AssetDatabase.StopAssetEditing(); AssetDatabase.Refresh(); }
```

Cell range: stay with prior 22×16 rectangle (or whatever Stream M painted). Don't change grid size.

### 1D. Use deterministic seed
`Random.InitState(2026)` before painting so result is reproducible.

---

## Phase 2 — Camera (8-12 min)

### 2A. Reduce ortho size for closer view
- Main Camera ortho size: **5** (was 8 per Stream M spec — too far). 
- Adjust if visible viewport too tight; aim to see ~7-9 cells in each direction.

### 2B. Even resolution
- Camera Pixel Perfect settings: target reference resolution **1280×720** (16:9, both even) — NOT 1400×900
- Game window aspect: 16:9 fixed
- This avoids the odd-resolution UnityMCP screenshot capture issue user flagged

### 2C. CameraFollow script
Create `Assets/Scripts/Camera/CameraFollow.cs`:

```csharp
using UnityEngine;

namespace RIMA.CameraSystem
{
    [DefaultExecutionOrder(100)] // run after player movement
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

        void OnValidate()
        {
            if (worldOffset.z == 0) worldOffset = new Vector3(worldOffset.x, worldOffset.y, -10f);
        }
    }
}
```

Attach to Main Camera. Set `target` = Player transform (drag in inspector OR set via editor script).

### 2D. Test camera follow
- In play mode, move player with WASD
- Camera should smoothly follow with ~0.15s damp
- No jitter, no overshoot
- Player should stay roughly centered (small offset OK)

---

## Phase 3 — Player verification (5 min)

If Stream M didn't complete Player setup, do it now:
- Instantiate `Assets/Prefabs/Player.prefab` at (0, 0, 0)
- SpriteRenderer material → `Sprite-Lit-Default`
- Sorting layer = Default, sortingOrder = 0
- Verify Rigidbody2D + Collider2D present
- Verify WASD movement works (check PlayerMovementController.cs script attached)

If Player.prefab won't move with WASD, check:
- Does PlayerMovementController script have public speed > 0?
- Is FixedUpdate or Update reading Input.GetAxis("Horizontal/Vertical")?
- Old Input Manager axes "Horizontal"/"Vertical" exist in Project Settings?

DO NOT modify Player.prefab itself — only fix scene-level setup.

---

## Phase 4 — Re-screenshot at even resolution (5 min)

UnityMCP screenshot with EVEN dimensions:
- 1280×720 OR 1024×768 OR 1600×900
- NOT 1400×900 (Stream J/M had odd issues with this — user reported)

Path: `STAGING/s106_overnight/playable_arena_v2.png`

Take screenshot:
- In play mode (lighting active)
- Camera in current ortho size 5 view
- Player at center mid-frame

---

## Phase 5 — Report (5 min)

```
# STREAM N - ARENA POLISH - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 0 — Scene state on entry
<100 words>

## Phase 1 — Tile weighted re-paint
- Tile theme mapping confirmed:
  - Cobblestone base: tiles X, Y, Z, W
  - Cyan veins: ...
  - Dirt: ...
  - Ritual: ...
- Cells painted: N (22×16 = 352 expected)
- Weighted distribution actual: <breakdown>
- Visual chaos reduced: y/n (subjective check)

## Phase 2 — Camera + Follow
- Ortho size: 5
- Resolution target: 1280×720 (or whatever even)
- CameraFollow.cs created: y/n + line count
- Smooth damping verified in play mode: y/n

## Phase 3 — Player
- Prefab instantiated: y/n
- SpriteRenderer lit: y/n
- WASD moves player in play mode: y/n
- Camera follows correctly: y/n

## Phase 4 — Screenshot
- Path: STAGING/s106_overnight/playable_arena_v2.png
- Resolution: <WxH>
- Captured in play mode: y/n

## Compile check
- 0 errors, 0 warnings

## Time: N min
```

---

## Safety constraints (HARD)
- ❌ Don't touch other 4 PainterTestE scenes
- ❌ Don't modify Player.prefab itself (scene-level setup only)
- ❌ Don't change Tilemap CellLayout (Isometric stays)
- ❌ NO walls, NO room generator, NO objects beyond Player
- ✅ AssetDatabase batch wrap for SetTiles
- ✅ Single Refresh at end
- ✅ Even resolution screenshot ONLY
- ✅ DETERMINISTIC seed (2026) for reproducible tile mix

## Estimated total: 35-50 min
