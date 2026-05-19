# Codex Task — Phase 1.5 Data-First Decal Migration

3-verdict consensus locked (`memory/project_multi_projection_architecture_lock.md`): critical path is data-first decal pattern. Brush V1 currently spawns 1 GameObject per decal (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs:224-273`), which at 40×25 room × density 0.16 × 3 layers = ~480 decoration GameObjects per room, ~1500 at peak (2-room streaming). This is the bleed.

Goal: add a data-first path **alongside** the existing GameObject path, behind a feature flag. Existing 328/328 EditMode tests must stay green. Existing executors must NOT be modified — new executors are registered as alternatives.

## Files to create

### 1. `Assets/Scripts/MapDesigner/Brush/Data/RoomDecalDataSO.cs`

Per-room SO holding decal placement data. Renderer-agnostic (no `SpriteRenderer`/`Tilemap`/`Mesh` refs in public API).

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "RoomDecalData", menuName = "RIMA/MapDesigner/RoomDecalData")]
    public class RoomDecalDataSO : ScriptableObject
    {
        public string roomId;
        public List<DecalPlacement> placements = new List<DecalPlacement>();
    }

    [System.Serializable]
    public struct DecalPlacement
    {
        public Vector2 worldPos;
        public int spriteId;       // resolves to Sprite via PatchAtlasSO.variants[]
        public byte layer;         // 4 (organic) / 5 (detail) / 6 (accent)
        public byte rotationStep;  // 0/1/2/3 = 0/90/180/270 degrees
        public byte flags;         // bit 0=flipX, bit 1=flipY, bit 2=locked
        public short tintPackedRGB; // 5-6-5 packed tint, or 0 for white
    }
}
```

### 2. `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalDataExecutor.cs`

Mirror of `FreeformDecalExecutor` but writes to `RoomDecalDataSO` instead of spawning GameObject. Registers as **alternative** for `PaintMode.FreeformDecal`. Selected when `BrushPipelineConfigSO.useDataFirstDecals == true`.

Required behavior:
- Same density/seed/walkable/minDistance logic as existing
- Apply same allowedTransforms (flipX/Y, rotation 0/90/180/270, scale clamped to 1.0 per ImportAssetRole lock)
- Append `DecalPlacement` to active `RoomDecalDataSO.placements`
- Mark SO dirty via `Undo.RecordObject` + `EditorUtility.SetDirty`
- Return `BrushExecutorResult { success=true, spawnedCount=N, spawnedObjects=null, modifiedAssets=[soRef] }`
- Honor existing seed pipeline (`stroke.seed` → System.Random) for replay determinism

### 3. `Assets/Scripts/MapDesigner/Brush/Executors/Editor/ScatterAlongStrokeDataExecutor.cs`

Same pattern as #2 but for `PaintMode.ScatterAlongStroke`. Samples along stroke path, writes placements.

### 4. `Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs`

MonoBehaviour that reads `RoomDecalDataSO` and builds a `Mesh` per layer at room load. Disposes on room unload.

Required behavior:
- One `MeshFilter` + `MeshRenderer` child per layer (L4, L5, L6)
- Mesh = quad per `DecalPlacement` with UV = sprite atlas rect lookup
- Material = `Sprites/Default` or `Sprites-Lit` for URP 2D
- Sorting layer / order matches the SO's `BrushLayerOperation.targetLayer` mapping (existing convention in `BrushExecutorRouter.cs:262-275`)
- Dirty-chunk rebuild API: `RebuildDirty(RectInt cells)` rebuilds only intersecting placements
- Sprite atlas resolution: requires SpriteAtlas asset per PatchAtlasSO (atlas tag = atlas's `atlasId`)

### 5. `Assets/Scripts/MapDesigner/Brush/Data/BrushPipelineConfigSO.cs`

Feature flag SO.

```csharp
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "BrushPipelineConfig", menuName = "RIMA/MapDesigner/BrushPipelineConfig")]
    public class BrushPipelineConfigSO : ScriptableObject
    {
        public bool useDataFirstDecals = false; // default off until benchmarked
        public bool useDataFirstScatter = false;
    }
}
```

### 6. Integration with existing `BrushExecutorRouter`

Modify `BrushExecutorRouter.RegisterIfAvailable` (existing reflection-based registration at `BrushExecutorRouter.cs:20-25`) to also try `*DataExecutor` types. Selection logic in `Dispatch`: if `BrushPipelineConfigSO.useDataFirstDecals == true` AND mode is `FreeformDecal`/`ScatterAlongStroke`, route to `*DataExecutor` instead of legacy.

**Critical:** do NOT delete or modify existing `FreeformDecalExecutor` / `ScatterAlongStrokeExecutor`. Add registry priority so DataExecutor wins when flag is on.

## Tests to add

`Assets/Tests/EditMode/Brush/BrushDataFirstExecutorTests.cs`:

```csharp
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEngine;

namespace RIMA.MapDesigner.Tests.Brush
{
    public class BrushDataFirstExecutorTests
    {
        [Test] public void FreeformDecalData_Appends_Placement_NotGameObject()
        {
            // Setup: RoomDecalDataSO + BrushPipelineConfigSO.useDataFirstDecals=true + AssetPool with 1 sprite
            // Action: Dispatch FreeformDecal at world (5, 3)
            // Assert: SO.placements.Count == 1, no scene GameObject spawned
        }

        [Test] public void ScatterAlongStrokeData_Samples_Along_Path()
        {
            // Setup: stroke from (0,0) to (10,0), density 0.2, minDistance 2
            // Action: Dispatch ScatterAlongStroke
            // Assert: SO.placements.Count in expected range (3-5), no GameObjects
        }

        [Test] public void Seed_Determinism_Same_Seed_Same_Placements()
        {
            // Action: Run same stroke twice with same seed
            // Assert: placement positions byte-equal
        }

        [Test] public void Flag_Off_Uses_Legacy_GameObject_Path()
        {
            // BrushPipelineConfigSO.useDataFirstDecals = false
            // Assert: existing GameObject executor runs, scene GameObject spawned (legacy behavior preserved)
        }

        [Test] public void Chunk_Renderer_Builds_Mesh_From_SO()
        {
            // Setup: SO with 10 placements
            // Action: Renderer.Build()
            // Assert: 3 MeshRenderer children (L4/L5/L6), correct vertex count
        }
    }
}
```

5 new tests. Existing 328 stay green = total 333.

## Atlas requirement

For chunk renderer to work, each `PatchAtlasSO` needs an associated `SpriteAtlas` asset for atlas-based UV lookup. Phase 1.5 scope INCLUDES generating a SpriteAtlas asset per PatchAtlasSO if missing — `AssetDatabase`-driven Editor utility.

Suggested file: `Assets/Editor/MapDesigner/PatchAtlasSpriteAtlasBuilder.cs` with menu item `RIMA → MapDesigner → Build SpriteAtlas from PatchAtlas`.

## Constraints

- Renderer-agnostic discipline: no `SpriteRenderer`/`Tilemap` types in public SO API (LOCKED by `memory/project_multi_projection_architecture_lock.md` rule #2)
- Determinism: all sub-grid placements seeded through `System.Random` per stroke (LOCKED by tweet review Risk 7)
- Field-additive only — no breaking changes to `BrushExecutorResult`, `BrushStroke`, `PatchAtlasSO`
- No `EditorUtility.DisplayDialog` (memory `feedback_no_dialog.md`)
- All new files under `RIMA.MapDesigner.Brush.*` namespace
- New tests must pass alongside existing 328 → 333 minimum
- Unity must compile cleanly after each file added
- DO NOT touch ChunkedSpriteLayer / any 3D mesh path — pure 2D mesh for low top-down renderer

## Reports

Write `STAGING/CODEX_TASK_PHASE_1_5_DATA_FIRST_DECALS_DONE.md` with:
- 6 files created (paths)
- 5 new tests created
- Unity compile status
- Test count delta (328 → 333+)
- BrushExecutorRouter modification scope (additive only)
- Any deviation with reason
- Performance benchmark snippet (optional bonus): 100 decals placed via DataExecutor vs LegacyExecutor, measure paint time + GO count

## Scope guardrails

- Do NOT migrate existing executors (additive parallel only)
- Do NOT change BrushStroke struct
- Do NOT touch L2 Tilemap / L3 walls / L7 props paths
- Do NOT introduce DOTS/ECS
- Do NOT introduce 3D rendering
- This is the V1 ship-readiness pivot per `STAGING/CODEX_TWEET_REVIEW_xhigh.md` + Opus overnight verdict + Codex multi-projection §3
