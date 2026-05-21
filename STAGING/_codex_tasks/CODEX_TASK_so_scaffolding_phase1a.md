# Codex Task — Phase 1A SO Scaffolding

Generate minimal C# ScriptableObject contracts per ChatGPT FINAL verdict §12 (`STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md`). These are MUST-have before asset library growth. Field-additive, no breaking changes to existing 321/321 EditMode tests.

## Critical constraint

**Renderer-agnostic interface.** SO definitions hold only data — Sprite, Texture2D, primitives. NO `SpriteRenderer`, `Tilemap`, `MonoBehaviour` references in SO public API. Concrete rendering happens in spawner/importer classes (e.g., `BrushAtlasImporter`, `ScatterBrushPainter`).

This is enforced because the architecture must port to HD-2D / sprite-in-3D in the future (per memory `project_3d_portability_strategy.md`).

## Files to create

### 1. `Assets/Scripts/Rima/MapDesigner/SO/TerrainDefinitionSO.cs`

```csharp
using UnityEngine;

namespace RIMA.MapDesigner.SO {
  [CreateAssetMenu(menuName = "RIMA/MapDesigner/TerrainDefinition")]
  public class TerrainDefinitionSO : ScriptableObject {
    public string terrainId;
    public string displayName;
    public Sprite[] baseTileVariants;  // pool, picked seeded
    public bool walkable = true;
    public bool blocksMovement = false;
    public VisualCategory visualCategory;
    public Color averageColor = new Color(0.23f, 0.26f, 0.31f); // slate blue-gray default
    [Range(0f, 1f)] public float defaultDecalDensity = 0.10f;
    [Range(0f, 1f)] public float defaultScatterDensity = 0.18f;
  }

  public enum VisualCategory { Stone, Dirt, Grass, Sand, Water, Lava, Wood, Metal, RiftFloor, Custom }
}
```

### 2. `Assets/Scripts/Rima/MapDesigner/SO/PatchAtlasSO.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO {
  [CreateAssetMenu(menuName = "RIMA/MapDesigner/PatchAtlas")]
  public class PatchAtlasSO : ScriptableObject {
    public string atlasId;
    public PatchRole role;
    public List<string> validTerrainIds;  // resolve by id, not by SO ref
    public Sprite[] variants;
    [Range(0f, 1f)] public float density = 0.10f;
    [Min(0f)] public float minDistance = 2f;
    public bool edgeBiased = false;
    public bool wallProximityBiased = false;
    public AllowedTransforms allowedTransforms;
  }

  public enum PatchRole { BaseFloor, MacroPatch, OrganicDecal, DetailScatter, Accent }

  [System.Serializable]
  public struct AllowedTransforms {
    public bool flipX, flipY;
    public bool rotate90, rotate180, rotate270;
    public Vector2 scaleRange;       // e.g., (0.75, 1.25)
    public Vector2 alphaRange;       // e.g., (0.75, 1.0)
  }
}
```

### 3. `Assets/Scripts/Rima/MapDesigner/SO/PropDefinitionSO.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO {
  [CreateAssetMenu(menuName = "RIMA/MapDesigner/PropDefinition")]
  public class PropDefinitionSO : ScriptableObject {
    public string propId;
    public Sprite visual;               // sprite asset only, not a renderer
    public Vector2Int footprint = Vector2Int.one;
    public bool hasCollision = false;
    public Vector2 ySortPivot = new Vector2(0.5f, 0f);
    public List<string> validTerrainIds;
    public bool isFeatureAnchor = false;
    public Sprite shadowSprite;          // optional, for L9
    public Vector2 shadowOffset = new Vector2(0f, -0.08f);
    public Vector2 shadowScale = new Vector2(1.1f, 0.4f);
    [Range(0f, 1f)] public float shadowAlpha = 0.35f;
    public string lightingProfileId;     // resolves to EmitterProfile by id
  }
}
```

### 4. `Assets/Scripts/Rima/MapDesigner/SO/RoomVisualProfileSO.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO {
  [CreateAssetMenu(menuName = "RIMA/MapDesigner/RoomVisualProfile")]
  public class RoomVisualProfileSO : ScriptableObject {
    public string profileId;
    public RoomVisualMode visualMode;
    public bool usesWallKit = true;
    public bool usesTierBBackground = false;
    public Color floorTone = new Color(0.16f, 0.14f, 0.18f);
    public List<PatchAtlasSO> allowedPatchAtlases;
    public List<PropDefinitionSO> allowedProps;
    public string lightingProfileId;
  }

  public enum RoomVisualMode { DungeonEnclosed, HadesArena, RuinedCourtyard, Shrine, RiftChamber, BossArena }
}
```

### 5. `Assets/Scripts/Rima/MapDesigner/SO/ImportAssetRole.cs`

```csharp
namespace RIMA.MapDesigner.SO {
  public enum ImportAssetRole {
    Terrain32,
    MacroPatch64_128,
    OrganicDecal,
    DetailScatter,
    Accent,
    Prop,
    Character,
    TierBBackground,
    LightSource
  }
}
```

Match exactly the enum names ChatGPT specified. Do not rename, do not add fields.

## Tests to add

`Assets/Tests/EditMode/MapDesigner/SO/Phase1ASoContractsTests.cs`:

```csharp
using NUnit.Framework;
using RIMA.MapDesigner.SO;
using UnityEngine;

namespace RIMA.MapDesigner.Tests.SO {
  public class Phase1ASoContractsTests {
    [Test] public void TerrainDef_Defaults_Sensible() {
      var t = ScriptableObject.CreateInstance<TerrainDefinitionSO>();
      Assert.IsTrue(t.walkable);
      Assert.IsFalse(t.blocksMovement);
      Assert.That(t.defaultDecalDensity, Is.InRange(0f, 1f));
      Object.DestroyImmediate(t);
    }

    [Test] public void PatchAtlas_Roles_Cover_AllSlots() {
      // assert enum count is 5 (BaseFloor, MacroPatch, OrganicDecal, DetailScatter, Accent)
      Assert.AreEqual(5, System.Enum.GetValues(typeof(PatchRole)).Length);
    }

    [Test] public void ImportAssetRole_Matches_ChatGPTLock() {
      var required = new[] { "Terrain32", "MacroPatch64_128", "OrganicDecal",
                             "DetailScatter", "Accent", "Prop", "Character",
                             "TierBBackground", "LightSource" };
      foreach (var n in required) {
        Assert.IsTrue(System.Enum.IsDefined(typeof(ImportAssetRole), n),
                      $"ImportAssetRole missing: {n}");
      }
    }

    [Test] public void RoomVisualProfile_Modes_Cover_AllChatGPTModes() {
      Assert.AreEqual(6, System.Enum.GetValues(typeof(RoomVisualMode)).Length);
    }

    [Test] public void PropDef_Shadow_Defaults() {
      var p = ScriptableObject.CreateInstance<PropDefinitionSO>();
      Assert.That(p.shadowAlpha, Is.InRange(0f, 1f));
      Assert.AreEqual(new Vector2(0f, -0.08f), p.shadowOffset);
      Object.DestroyImmediate(p);
    }
  }
}
```

## Constraints

- Do NOT modify existing `BrushPackSO`, `PatchAtlasSO` (if it exists already at different path), or any Brush V1 SO.
- Do NOT change existing namespace organization; new files go under `RIMA.MapDesigner.SO`.
- Do NOT use `EditorUtility.DisplayDialog` anywhere (memory `feedback_no_dialog.md`).
- Use `[CreateAssetMenu(menuName = "RIMA/MapDesigner/<X>")]` consistently.
- All public fields require either default initialization or remain serializable.
- New tests must pass alongside existing 321 — total = 321 + 5 = 326 minimum.

## Report

Write `STAGING/CODEX_TASK_so_scaffolding_phase1a_DONE.md` with:
- 5 .cs files created (paths)
- 1 test file created (path)
- Compile status (Unity must compile cleanly)
- Test count delta (should be +5)
- Any deviation from this spec, with reason

If you cannot compile due to a missing namespace dep, list the dep and stop — do not add stub fallbacks.
