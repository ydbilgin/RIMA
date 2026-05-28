ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — PPU + repaint only (4) BLOCKED if PlayableArena scene missing.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: User reported visual bug — "saçma bir birleşim olmuş floorda" + "bazıları üst üste binmiş". Root cause: Stream O imported 16 tile PNGs at PPU 32, but iso Grid cellSize is (1, 0.5, 1). Math: 64px tile / PPU 32 = 2 units wide. Cell is 1 unit wide. Tiles render at 2× cell size → 100% overlap into neighbor cells. Ritual rune tiles (centered circles) overlap into adjacent cells creating "scattered weird circle" look.

**Fix:** Re-import the 16 tile PNGs at **PPU 64** (each 64px tile = 1 unit wide, matching cellSize.x=1). NO scene structural changes, just PPU import setting per asset + AssetDatabase.Refresh + Tilemap ClearAllTiles + repaint with same weighted random pattern + screenshot.

---

# STREAM O.1 — PPU CONFIGURATION FIX (Quick)

## ⚠️ Phase 0 — Verify state (3 min)

Check:
- `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/b340684f_tile_<N>.png` (16 PNGs) exist
- Their `.meta` files have `spritePixelsToUnits: 32` (current bug)
- `Assets/ScriptableObjects/Floor/IsoTiles35/tile_<N>.asset` (16 Tile assets) exist
- `Assets/Scenes/Test/PlayableArena.unity` has iso Grid cellSize (1, 0.5, 1) and 352 tiles painted

If any missing → write WAITING.

## Phase 1 — Re-import tile PNGs at PPU 64 (10 min)

### 1A. Edit each `.meta` file

For each of 16 `b340684f_tile_<N>.png.meta`:
- Find line `spritePixelsToUnits: 32`
- Change to `spritePixelsToUnits: 64`
- Save

Use shell/PowerShell to batch-edit OR Unity API:
```csharp
foreach (var path in tilePaths) {
    var importer = AssetImporter.GetAtPath(path) as TextureImporter;
    importer.spritePixelsPerUnit = 64;
    importer.SaveAndReimport();
}
```

### 1B. AssetDatabase.Refresh ONCE at end of all 16 reimports

Don't refresh per-file — batch.

### 1C. Verify

After refresh, in Unity Editor, click one of the tile PNG assets → Inspector should show "Pixels Per Unit: 64".

## Phase 2 — Repaint scene (5 min)

Open `Assets/Scenes/Test/PlayableArena.unity`:

```csharp
tilemap.ClearAllTiles();
// Same weighted random pattern as Stream O:
// 80% base (tiles 0-3), 15% dirt (8-11), 3% cyan (4-7), 2% ritual (12-15)
// Same seed 2026 for reproducibility
Random.InitState(2026);

Vector3Int[] positions = new Vector3Int[352];
TileBase[] tiles = new TileBase[352];
int idx = 0;
for (int x = -11; x <= 10; x++) {
    for (int y = -8; y <= 7; y++) {
        positions[idx] = new Vector3Int(x, y, 0);
        // weighted theme selection per cell:
        float r = Random.value;
        int themeStart;
        if (r < 0.80f) themeStart = 0;        // cobblestone 0-3
        else if (r < 0.95f) themeStart = 8;    // dirt 8-11
        else if (r < 0.98f) themeStart = 4;    // cyan 4-7
        else themeStart = 12;                  // ritual 12-15
        int variant = themeStart + Random.Range(0, 4);
        tiles[idx] = tileAssets[variant];
        idx++;
    }
}

AssetDatabase.StartAssetEditing();
try {
    tilemap.SetTiles(positions, tiles);
    EditorSceneManager.MarkSceneDirty(scene);
} finally {
    AssetDatabase.StopAssetEditing();
    AssetDatabase.Refresh();
}
EditorSceneManager.SaveScene(scene);
```

## Phase 3 — Verify visual fix (5 min)

Take new screenshot: `STAGING/s106_overnight/playable_arena_35deg_v2.png` (1280×720)

In play mode, verify:
- No tile overlap (each tile occupies exactly 1 cell)
- Diamond tiles align with iso grid
- Rune circles don't bleed into neighbor cells

## Phase 4 — Report

```
# STREAM O.1 - PPU FIX - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 0 — State verified
- 16 PNGs PPU 32 before: y/n
- 16 Tile assets exist: y/n
- Scene has 352 tiles painted: y/n

## Phase 1 — Re-import PPU 64
- All 16 .meta updated to spritePixelsToUnits 64: y/n
- AssetDatabase batch reimport completed: y/n

## Phase 2 — Repaint
- Tiles cleared: y
- 352 cells re-painted with same weighted seed 2026: y/n
- Distribution: 298/38/9/7 (or report actual): ...
- Scene saved: y

## Phase 3 — Visual verification
- Screenshot path: STAGING/s106_overnight/playable_arena_35deg_v2.png (1280x720)
- Overlap fixed (no rune bleed): y/n
- Rune circles aligned 1-per-cell: y/n

## Compile check
- 0 errors, 0 warnings

## Time: N min
```

## Safety constraints (HARD)
- ❌ Don't change cellSize (1, 0.5, 1) — keep as Stream O set
- ❌ Don't touch tile_<N>.asset files (those just reference sprites — PPU change auto-propagates)
- ❌ Don't modify CameraFollow.cs, Player, lighting (those are correct per Stream O)
- ❌ Don't touch other scenes
- ✅ Single batch AssetDatabase.Refresh
- ✅ Same seed 2026 for reproducible compare

## Estimated total: 20-30 min
