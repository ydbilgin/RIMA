# LayeredRoomPainter Biome Wiring — execute every step, commit at end

## Context

LayeredRoomPainter.cs currently paints 2 tilemaps (floor + 1 wall) with a single TileBase each.
Karar #118b added 4-layer tilemap stack (Base/Decal/WallsFront/WallsTop). Karar #128/129 added WangTileResolver + RimaBiomePreset.
This task wires them together so GenerateLayered() paints all 4 layers using biome data.

## Key files (DO NOT read others)

- `Assets/Scripts/Systems/Map/LayeredRoomPainter.cs`
- `Assets/Scripts/Systems/Map/WangTileResolver.cs`
- `Assets/Scripts/Systems/Map/RimaBiomePreset.cs`
- `Assets/Scripts/Systems/Map/TileAssetMetadata.cs`
- `Assets/Scripts/Demo/RoomPipelineTestController.cs`

## STEP 1 — Read the key files

Read all 5 files listed above.

## STEP 2 — Upgrade LayeredRoomPainter.cs

Add new static method (keep old Paint() intact for backward compat):

```csharp
public static void PaintBiome(
    Tilemap baseTm,
    Tilemap decalTm,
    Tilemap wallFrontTm,
    Tilemap wallTopTm,
    byte[] floor,
    byte[] wall,
    int w,
    int h,
    RimaBiomePreset biome,
    TileAssetMetadata[] wangLibrary,
    int seed)
{
    if (baseTm == null || wallFrontTm == null || biome == null) return;

    baseTm.ClearAllTiles();
    if (decalTm) decalTm.ClearAllTiles();
    wallFrontTm.ClearAllTiles();
    if (wallTopTm) wallTopTm.ClearAllTiles();

    var rng = new System.Random(seed);

    // Floor + decal pass
    for (int y = 0; y < h; y++)
    {
        for (int x = 0; x < w; x++)
        {
            int idx = y * w + x;
            if (floor[idx] != 1) continue;
            var cell = new Vector3Int(x, y, 0);

            // Floor tile (weight-based deterministic)
            TileAssetMetadata floorMeta = PickWeighted(biome.allowedFloorTiles, seed, x, y);
            if (floorMeta?.tile != null) baseTm.SetTile(cell, floorMeta.tile);

            // Decal pass
            if (decalTm != null && biome.decalTiles != null && biome.decalTiles.Length > 0)
            {
                int hash = seed ^ (x * 1000003) ^ (y * 999983);
                float r = (Mathf.Abs(hash % 10000) / 10000f);
                if (r < biome.decalDensity)
                {
                    TileAssetMetadata decalMeta = PickWeighted(biome.decalTiles, seed + 1, x, y);
                    if (decalMeta?.tile != null) decalTm.SetTile(cell, decalMeta.tile);
                }
            }
        }
    }

    // Wall pass — compute NSEW Wang mask, route to front/top tilemaps
    for (int y = 0; y < h; y++)
    {
        for (int x = 0; x < w; x++)
        {
            int idx = y * w + x;
            if (wall[idx] != 1) continue;
            var cell = new Vector3Int(x, y, 0);

            bool north = IsWall(wall, x, y + 1, w, h);
            bool east  = IsWall(wall, x + 1, y, w, h);
            bool south = IsWall(wall, x, y - 1, w, h);
            bool west  = IsWall(wall, x - 1, y, w, h);
            int mask = WangTileResolver.ComputeWangMask(north, east, south, west);

            TileAssetMetadata meta = ResolveWang(wangLibrary, mask, cell, seed);
            if (meta == null && biome.allowedWallTiles != null && biome.allowedWallTiles.Length > 0)
                meta = PickWeighted(biome.allowedWallTiles, seed, x, y);
            if (meta?.tile == null) continue;

            // Antigravity 4 P0: all walls → WallsFront; cliff-top tiles also → WallsTop
            wallFrontTm.SetTile(cell, meta.tile);
            if (wallTopTm != null && meta.isCliffTop)
                wallTopTm.SetTile(cell, meta.tile);
        }
    }

    baseTm.CompressBounds();
    wallFrontTm.CompressBounds();
}

private static TileAssetMetadata PickWeighted(TileAssetMetadata[] pool, int seed, int x, int y)
{
    if (pool == null || pool.Length == 0) return null;
    float total = 0f;
    foreach (var t in pool) if (t != null) total += Mathf.Max(0f, t.weight);
    if (total <= 0f) return pool[0];
    int hash = seed ^ (x * 73856093) ^ (y * 19349663);
    float pick = (Mathf.Abs(hash % 10000) / 10000f) * total;
    float cumul = 0f;
    foreach (var t in pool)
    {
        if (t == null) continue;
        cumul += Mathf.Max(0f, t.weight);
        if (pick <= cumul) return t;
    }
    return pool[pool.Length - 1];
}

private static bool IsWall(byte[] wall, int x, int y, int w, int h)
{
    if (x < 0 || y < 0 || x >= w || y >= h) return true;
    return wall[y * w + x] == 1;
}

private static TileAssetMetadata ResolveWang(TileAssetMetadata[] lib, int mask, Vector3Int cell, int seed)
{
    if (lib == null || lib.Length == 0) return null;
    var candidates = System.Array.FindAll(lib, t => t != null && t.wangMask == mask);
    if (candidates.Length == 0) return null;
    if (candidates.Length == 1) return candidates[0];
    int hash = seed ^ (cell.x * 73856093) ^ (cell.y * 19349663);
    float total = 0f;
    foreach (var c in candidates) total += Mathf.Max(0f, c.weight);
    float pick = (Mathf.Abs(hash % 10000) / 10000f) * total;
    float cumul = 0f;
    foreach (var c in candidates) { cumul += Mathf.Max(0f, c.weight); if (pick <= cumul) return c; }
    return candidates[candidates.Length - 1];
}
```

## STEP 3 — Update RoomPipelineTestController

Read `Assets/Scripts/Demo/RoomPipelineTestController.cs`.

Add fields:
```csharp
[SerializeField] private Tilemap wallsFrontTilemap;
[SerializeField] private Tilemap wallsTopTilemap;
[SerializeField] private RimaBiomePreset biomePreset;
[SerializeField] private TileAssetMetadata[] wangTileLibrary;
```

Add new ContextMenu method alongside existing `GenerateLayered()`:
```csharp
[ContextMenu("0b. Generate Biome Layered")]
public void GenerateBiomeLayered()
{
    if (!EnsureTemplateAndTilemaps()) return;
    if (biomePreset == null)
    {
        Debug.LogError("RoomPipelineTestController: assign biomePreset.");
        return;
    }

    var rng = new System.Random(seed);
    int width  = rng.Next(Mathf.Max(3, template.minWidth),  Mathf.Max(template.minWidth  + 1, template.maxWidth  + 1));
    int height = rng.Next(Mathf.Max(3, template.minHeight), Mathf.Max(template.minHeight + 1, template.maxHeight + 1));

    var input    = new GenerationInput(seed, template.biome.ToString(), template.archetypeId, width, height, template.generatorVersion);
    var gen      = new LayeredRoomGenerator();
    GridSnapshot snapshot = gen.Generate(input);

    LayeredRoomPainter.PaintBiome(
        baseTilemap, decalsTilemap,
        wallsFrontTilemap, wallsTopTilemap,
        snapshot.floorMask, snapshot.wallMask,
        snapshot.width, snapshot.height,
        biomePreset, wangTileLibrary, seed);

    Debug.Log($"RoomPipelineTestController: BiomePaint {snapshot.width}x{snapshot.height} (seed={seed}, biome={biomePreset.biomeName})");
}
```

## STEP 4 — Compile check

`read_console` — 0 errors required. Fix any compiler errors.

## STEP 5 — Commit

```bash
git add Assets/Scripts/Systems/Map/LayeredRoomPainter.cs Assets/Scripts/Demo/RoomPipelineTestController.cs
git commit -m "[biome-paint] LayeredRoomPainter.PaintBiome — 4-layer Wang+biome wiring

- PaintBiome: floor weight-based pick, decal density pass, Wang mask wall routing
- Wall → WallsFront always; isCliffTop → WallsTop also (Antigravity 4 P0)
- RoomPipelineTestController: GenerateBiomeLayered ContextMenu added
- Karar #128 WangTileResolver + Karar #129 RimaBiomePreset integration"
```

## STEP 6 — Report

Write `STAGING/layered_room_biome_wiring_report.md`:
```
# LayeredRoomPainter Biome Wiring Report

## PaintBiome method
[created Y/N]

## Wang mask routing
[wallFront Y/N, wallTop for isCliffTop Y/N]

## Decal pass
[density-based Y/N]

## RoomPipelineTestController
[GenerateBiomeLayered added Y/N, new fields Y/N]

## Compile
[0 errors Y/N]
```

Append `CODEX_DONE_yasinderyabilgin.md`:
```
## [2026-05-14] LayeredRoomPainter Biome Wiring
- PaintBiome: Y/N
- Wang routing: Y/N
- Compile: Y/N
- Commit: [hash]
```

## Constraints

- DO NOT remove old LayeredRoomPainter.Paint() — keep for backward compatibility
- DO NOT add PropContainer wiring yet — that's T4 (separate task)
- wangLibrary may be empty (null-safe) — fallback to biome.allowedWallTiles
- NAMESPACE: RIMA.Systems.Map for LayeredRoomPainter

## Source References

1. `Assets/Scripts/Systems/Map/LayeredRoomPainter.cs` — current static Paint()
2. `Assets/Scripts/Systems/Map/WangTileResolver.cs` — Resolve() + ComputeWangMask()
3. `Assets/Scripts/Systems/Map/RimaBiomePreset.cs` — allowedFloorTiles/allowedWallTiles/decalTiles/decalDensity
4. `Assets/Scripts/Systems/Map/TileAssetMetadata.cs` — tile/weight/isCliffTop/isCliffFront/wangMask
5. `Assets/Scripts/Demo/RoomPipelineTestController.cs` — GenerateLayered() pattern to follow
