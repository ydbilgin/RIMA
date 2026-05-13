# Karar #128 — TileAssetMetadata SO + WangTileResolver — execute every step, commit at end

## Context

Karar #128 Faz 1 P0: TileImportWizard (#118a — commit 1b99080) tamamlandı. Şimdi Unity-side metadata SO + autotile resolver implementasyonu. Karar #115 deterministic seed compliance zorunlu.

**Dependency satisfied:** Karar #118a TileImportWizard ✓

## STEP 1 — TileAssetMetadata ScriptableObject

Create `Assets/Scripts/Systems/Map/TileAssetMetadata.cs`:

```csharp
[CreateAssetMenu(fileName = "TileAssetMetadata", menuName = "RIMA/Tile Asset Metadata")]
public class TileAssetMetadata : ScriptableObject
{
    public string tileId;
    public RimaBiomeType biomeType;      // existing enum
    public RimaTerrainType terrainType;  // existing enum or create
    public TileBase tile;                // TileBase reference
    [Range(0f, 1f)] public float weight = 1f;
    public bool supportsCollision;
    public bool isCliffFront;
    public bool isCliffTop;
    public bool isTransition;
    public bool decalAllowed = true;
    public bool scatterAllowed = true;
    public bool shadowRequired;
    // Wang edge signatures (16-bit NSEW mask)
    public int wangMask;                 // 0-15 for 16-tile NSEW
    public string variantGroup;          // for weighted variant selection
}
```

Also create `RimaTerrainType` enum if it doesn't already exist (check `Assets/Scripts/Systems/Map/` first):
```csharp
public enum RimaTerrainType { Floor, Wall, WallTop, Decal, Prop, Void }
```

## STEP 2 — WangTileResolver

Create `Assets/Scripts/Systems/Map/WangTileResolver.cs`:

```csharp
// Deterministic seed-based Wang tile resolver (Karar #115 compliance)
public class WangTileResolver : MonoBehaviour
{
    [SerializeField] private TileAssetMetadata[] tileLibrary;

    // Returns deterministic tile given neighbor mask (0-15) and world position
    public TileAssetMetadata Resolve(int wangMask, Vector3Int cellPos, int seed)
    {
        // Filter by wangMask
        var candidates = System.Array.FindAll(tileLibrary, t => t.wangMask == wangMask);
        if (candidates.Length == 0) return null;
        if (candidates.Length == 1) return candidates[0];

        // Weighted random — deterministic via seed + position hash
        int hash = seed ^ (cellPos.x * 73856093) ^ (cellPos.y * 19349663);
        float totalWeight = 0f;
        foreach (var c in candidates) totalWeight += c.weight;
        float rand = (Mathf.Abs(hash % 10000) / 10000f) * totalWeight;
        float cumulative = 0f;
        foreach (var c in candidates)
        {
            cumulative += c.weight;
            if (rand <= cumulative) return c;
        }
        return candidates[candidates.Length - 1];
    }

    // Compute 16-tile NSEW wang mask from neighbor existence
    // bitmask: N=1, E=2, S=4, W=8
    public static int ComputeWangMask(bool north, bool east, bool south, bool west)
    {
        return (north ? 1 : 0) | (east ? 2 : 0) | (south ? 4 : 0) | (west ? 8 : 0);
    }
}
```

## STEP 3 — TileImportWizard integration

In `Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs`, add metadata auto-create:
- After RuleTile auto-create (existing logic), create a companion TileAssetMetadata SO for each tile
- Set `wangMask` from JSON tile data, `biomeType` = F1 (default), `terrainType` from tile_type field
- Save to `Assets/Art/Tiles/F1/Generated/metadata_{name}.asset`

## STEP 4 — RimaTerrainType check

Read `Assets/Scripts/Systems/Map/` directory to check if RimaTerrainType already exists. If yes, extend it. If no, create standalone file.

## STEP 5 — Compile check

`read_console` — 0 errors required. Fix any compile errors before continuing.

## STEP 6 — Unit test (execute_code)

Run via UnityMCP execute_code:
```csharp
var resolver = new GameObject("TestResolver").AddComponent<WangTileResolver>();
// Test determinism: same mask + pos + seed = same result (call Resolve twice, compare)
var result1 = resolver.Resolve(5, new UnityEngine.Vector3Int(3, 7, 0), 42);
var result2 = resolver.Resolve(5, new UnityEngine.Vector3Int(3, 7, 0), 42);
return result1 == result2 ? "DETERMINISM: PASS" : "DETERMINISM: FAIL";
```

## STEP 7 — Commit

```bash
git add Assets/Scripts/Systems/Map/TileAssetMetadata.cs Assets/Scripts/Systems/Map/WangTileResolver.cs Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs
git commit -m "[karar128] TileAssetMetadata SO + WangTileResolver (16-tile NSEW)

- TileAssetMetadata ScriptableObject (biome/terrain/wang mask/weights)
- WangTileResolver: deterministic seed-based 16-tile NSEW resolver (Karar #115)
- TileImportWizard: companion metadata SO auto-create on import
- RimaTerrainType enum (Floor/Wall/WallTop/Decal/Prop/Void)"
```

## STEP 8 — Report

Write `STAGING/karar_128_tile_metadata_report.md`:
```
# Karar #128 Report

## TileAssetMetadata SO
[fields implemented Y/N]

## WangTileResolver
[determinism test: PASS/FAIL]
[16-tile NSEW mask: Y/N]

## TileImportWizard integration
[metadata auto-create Y/N]

## Console
[0 errors Y/N]
```

Append `CODEX_DONE_laurethgame.md`:
```
## [2026-05-14] Karar #128 TileAssetMetadata + WangTileResolver
- TileAssetMetadata SO: Y/N
- WangTileResolver determinism: PASS/FAIL
- TileImportWizard integration: Y/N
- Commit: [hash]
```

## Constraints

- Karar #115 deterministic seed compliance ZORUNLU (same seed + pos = same tile)
- Karar #117 Portable Core: TileAssetMetadata + WangTileResolver → Core/Game layer ayrımına uy (map sistemi Core'a girer, biome-specific SO Game'de)
- RimaTerrainType: eğer zaten varsa extend et, yeniden oluşturma

## Source References

1. `Assets/Scripts/Systems/Map/` — mevcut map scriptleri (RimaBiomeType, LargeDungeonMapPainterBase vs.)
2. `Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs` — #118a output, entegrasyon noktası
3. `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #115/#117/#128 spec referans
