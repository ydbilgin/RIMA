# S75-B — Multi-variant per Wang Key

**Effort:** high
**Prereq:** S75-A merged (Map Designer UX deep)

---

## GOAL

Aynı Wang corner key için 2-4 alternatif tile destekle → deterministic hash ile cell pozisyonuna göre variant seç → tile-grid tekrarı kırılır, PixelLab-like organik visual.

PixelLab Map Tool kalitesi tile çeşitlilğinden geliyor — bizim mimari de bunu desteklemeli.

---

## DATA MODEL

### CornerWangTileSetSO (extend)
**File:** `Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs`

```csharp
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public class CornerWangTileSetSO : ScriptableObject
    {
        [Header("Terrain Labels")]
        public string lowerTerrainLabel = "lower (floor)";
        public string upperTerrainLabel = "upper (wall)";

        // LEGACY (kept for backward compat)
        public TileBase[] tiles = new TileBase[16];

        // NEW multi-variant
        [System.Serializable]
        public class WangVariants
        {
            public TileBase[] variants = new TileBase[1];
        }
        public WangVariants[] variantsByKey = new WangVariants[16];

        public TileBase GetTile(int nw, int ne, int sw, int se, int hashSeed = 0)
        {
            int key = (nw << 3) | (ne << 2) | (sw << 1) | se;
            return GetTileForKey(key, hashSeed);
        }

        public TileBase GetTileForKey(int key, int hashSeed = 0)
        {
            if (key < 0 || key >= 16) return null;
            if (variantsByKey != null && variantsByKey.Length == 16 && variantsByKey[key] != null && variantsByKey[key].variants != null && variantsByKey[key].variants.Length > 0)
            {
                var arr = variantsByKey[key].variants;
                if (arr.Length == 1) return arr[0];
                int idx = (int)((uint)hashSeed % (uint)arr.Length);
                return arr[idx] ?? arr[0];
            }
            // Fallback to legacy
            return (tiles != null && key < tiles.Length) ? tiles[key] : null;
        }

        [ContextMenu("Sync Variants From Legacy Tiles")]
        public void SyncFromLegacy()
        {
            if (variantsByKey == null || variantsByKey.Length != 16)
                variantsByKey = new WangVariants[16];
            for (int i = 0; i < 16; i++)
            {
                if (variantsByKey[i] == null)
                    variantsByKey[i] = new WangVariants { variants = new TileBase[1] };
                if (variantsByKey[i].variants == null || variantsByKey[i].variants.Length == 0)
                    variantsByKey[i] = new WangVariants { variants = new TileBase[] { tiles != null && i < tiles.Length ? tiles[i] : null } };
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
}
```

### CornerWangPainter (use hashSeed)
**File:** `Assets/Scripts/Systems/Map/CornerWangPainter.cs`

`ResolveTile` ekstra position param + hash:
```csharp
public static TileBase ResolveTile(RimaBiomePreset biome, int nw, int ne, int sw, int se, int x = 0, int y = 0)
{
    // ... existing single/dual unique terrain logic ...
    int seed = (x * 73856093) ^ (y * 19349663);
    return pairing.tileSet.GetTile(nwBit, neBit, swBit, seBit, seed);
}
```

`Paint(...)` içinde cell loop'unda `ResolveTile(biome, ...,  x, y)` çağır (position'ı geçir).

### RimaMapDesignerWindow.DrawLiveTilePreviewCells
Aynı pattern: `CornerWangPainter.ResolveTile(activeBiome, nw, ne, sw, se, x, y)` ile preview render.

---

## VARIANT IMPORT (RebuildAllWangTilesets extend)

**File:** `Assets/Editor/RebuildAllWangTilesets.cs`

Mevcut akış: spritesheet.png → 16 Tile + 16 CornerWangTileSetSO.tiles.
**EKLE:** variants directory check:

```csharp
private static void CreateTileSet(TilesetMeta meta)
{
    // ... existing tiles[] population ...
    
    // NEW: scan for variants
    string folder = $"{TilesetRoot}/{meta.name}";
    string variantsFolder = $"{folder}/variants";
    so.variantsByKey = new CornerWangTileSetSO.WangVariants[16];
    for (int cornerKey = 0; cornerKey < 16; cornerKey++) {
        var variants = new List<TileBase>();
        variants.Add(AssetDatabase.LoadAssetAtPath<TileBase>($"{GeneratedFolder}/wang_{meta.name}_tile_{cornerKey}.asset"));
        // Variant pattern: wang_{name}_tile_{key}_v1.asset, _v2, etc
        for (int v = 1; v <= 5; v++) {
            var variantTile = AssetDatabase.LoadAssetAtPath<TileBase>($"{GeneratedFolder}/wang_{meta.name}_tile_{cornerKey}_v{v}.asset");
            if (variantTile != null) variants.Add(variantTile);
        }
        so.variantsByKey[cornerKey] = new CornerWangTileSetSO.WangVariants { variants = variants.ToArray() };
    }
}
```

---

## STUB VARIANT GENERATOR (placeholder until real PixelLab Pro gen)

**Yeni dosya:** `Assets/Editor/WangVariantStubGenerator.cs`

Menu: `RIMA > Tools > Generate Wang Variant Stubs (rotated)`

```csharp
public static class WangVariantStubGenerator
{
    [MenuItem("RIMA/Tools/Generate Wang Variant Stubs (rotated)")]
    public static void Generate()
    {
        // For each existing wang_*_tile_*.asset (legacy):
        //   Create 3 variants by rotating sprite 90/180/270 degrees
        //   Save as wang_{name}_tile_{key}_v1.asset, _v2, _v3
        //   Note: This is just a STUB. Real PixelLab Pro variants would be visually distinct.
        // Use RuntimeImageUtility to copy and rotate Texture2D.
    }
}
```

**Önemli:** Bu STUB sadece test için. Production kullanıcı PixelLab Pro Web UI'da raggedness 40-55% ile gerçek variant üretir, manuel import eder.

---

## VALIDATION

1. `dotnet build RIMA.slnx` PASS
2. Existing F1 biome: `variantsByKey` empty veya 1-element → görsel davranış AYNI kalmalı (fallback to legacy tiles[])
3. Run "Sync Variants From Legacy Tiles" context menu on FloorWall_CornerWangTileSet → variantsByKey populated with single-variant
4. Run "Generate Wang Variant Stubs" → 3 ek variant per key (rotated). Map Designer preview'de farklı cell'lerde farklı rotation görmeli.
5. Backwards: silinsin variantsByKey assets → legacy tiles[] fallback çalışmalı

**Screenshot:** STAGING/s75b_multivariant_preview.png — variants generate edildikten sonra Map Designer canvas'ta wall blok visual variation.

---

## COMMIT MESAJI

```
[S75-B] Multi-variant per Wang key

- CornerWangTileSetSO.variantsByKey (16 keys, 1+ variants each)
- GetTile(...) accepts hashSeed, deterministic position-based variant pick
- CornerWangPainter passes (x,y) hash to variant selection
- RebuildAllWangTilesets imports {tile}_v{N}.asset variants if present
- WangVariantStubGenerator menu: rotate-based stub variants (test only)
- Legacy tiles[] fallback maintained (backward compat)
```
