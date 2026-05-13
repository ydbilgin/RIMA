# Karar #129 — F1 Shattered Keep BiomePreset SO — execute every step, commit at end

## Context

Karar #129 Faz 1 P0: RimaBiomeType enum'unu ScriptableObject preset sistemine yükselt. Karar #128 TileAssetMetadata SO tamamlandıktan sonra bu dispatch gelir (bağımlılık: #128 ✓ olmalı).

**Dependency:** Karar #128 TileAssetMetadata.cs ✓

## STEP 1 — RimaBiomePreset ScriptableObject

Read `Assets/Scripts/Systems/Map/` — RimaBiomeType enum nerede? (muhtemelen RimaRoomBaselineTemplate.cs veya RimaBiomeType.cs içinde)

Create `Assets/Scripts/Systems/Map/RimaBiomePreset.cs`:

```csharp
[CreateAssetMenu(fileName = "BiomePreset", menuName = "RIMA/Biome Preset")]
public class RimaBiomePreset : ScriptableObject
{
    public RimaBiomeType biomeType;
    public string biomeName;
    [TextArea] public string description;

    // Tile metadata references
    public TileAssetMetadata[] allowedFloorTiles;
    public TileAssetMetadata[] allowedWallTiles;
    public TileAssetMetadata[] transitionTiles;
    public TileAssetMetadata[] decalTiles;

    // Scatter configuration (Karar #121)
    public string[] allowedScatterTags;   // e.g. "Stone", "Moss", "Rubble", "Dirt"

    // Visual mood
    public Color paletteBaseColor = Color.gray;
    public Color paletteShadowColor = Color.black;

    // Density ranges
    [Range(0f, 1f)] public float decalDensity = 0.3f;
    [Range(0f, 1f)] public float scatterDensity = 0.2f;

    // Cliff style (Antigravity 4 P0.3)
    public bool useCliffFront = true;
    public bool useCliffTop = true;
}
```

## STEP 2 — F1 Shattered Keep preset asset

Create `Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset`:
- biomeType: F1_ShatteredRuins (or closest existing enum value — check RimaBiomeType)
- biomeName: "Shattered Keep"
- allowedFloorTiles: leave empty array (will be filled when wang assets have metadata SO companions)
- allowedWallTiles: leave empty array
- allowedScatterTags: ["Stone", "Moss", "Rubble"]
- paletteBaseColor: #3D3230 (charcoal stone)
- paletteShadowColor: #1A1210
- decalDensity: 0.3
- scatterDensity: 0.2
- useCliffFront: true
- useCliffTop: true

## STEP 3 — Compile check

`read_console` — 0 errors required.

## STEP 4 — Commit

```bash
git add Assets/Scripts/Systems/Map/RimaBiomePreset.cs Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset
git commit -m "[karar129] RimaBiomePreset SO + F1 Shattered Keep preset

- RimaBiomePreset ScriptableObject (biome/tiles/scatter/density/cliff style)
- Shattered_Keep_F1_BiomePreset.asset (F1 MVP preset)
- Karar #128 TileAssetMetadata integration (allowedFloorTiles/WallTiles refs)"
```

## STEP 5 — Report

Write `STAGING/karar_129_biome_preset_report.md`:
```
# Karar #129 BiomePreset Report

## RimaBiomePreset SO
[fields Y/N]

## F1 Shattered Keep asset
[created Y/N, biomeType mapped correctly Y/N]

## Console
[0 errors Y/N]
```

Append `CODEX_DONE_laurethgame.md`:
```
## [2026-05-14] Karar #129 F1 BiomePreset
- RimaBiomePreset.cs: Y/N
- Shattered_Keep preset asset: Y/N
- Compile clean: Y/N
- Commit: [hash]
```

## Constraints

- DO NOT create new RimaBiomeType enum values unless strictly needed — use existing ones
- allowedFloorTiles/WallTiles may be empty arrays (metadata SOs populated in subsequent dispatch)
- Karar #117 Portable Core: RimaBiomePreset → Game layer (depends on TileAssetMetadata which is Core)

## Source References

1. `Assets/Scripts/Systems/Map/` — mevcut map scriptler (RimaBiomeType enum bul)
2. `Assets/Scripts/Systems/Map/TileAssetMetadata.cs` — Karar #128 output (ref)
3. `Assets/Art/Templates/F1_ShatteredRuins.asset` — F1 template referans
