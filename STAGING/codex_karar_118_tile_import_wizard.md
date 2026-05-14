# Karar #118 TileImportWizard + 4-layer tilemap stack — execute every step, commit at end

## Context

Karar #118 (Hybrid Tile Composition Multi-Layer + create_tiles_pro Primary) Faz 1.0 ZORUNLU. Mevcut RIMA Wang RuleTile importer var (`Assets/Editor/...` görüldü S67 commit 5017622) ama 4-layer (Base/Decal/Wall/Prop) full pipeline yok. TileImportWizard PixelLab Export Parser dispatch BEKLİYOR.

User Alabaster Dawn doğallık standardı istiyor. Karar #118 4-layer composition = doğallığın motoru (Base + organik Decal overlay + Wang Wall + Prop scatter).

## 4-Layer Tilemap Stack (Karar #118 spec)

### Layer 1 — Base (RuleTile + RandomTile)
- Floor + path + variants (autoconnect)
- Weighted RandomTile (3+ variant per terrain — Karar #116 zorunlu)
- Tilemap collision: OFF (walkable)

### Layer 2 — Decal (transparent overlay)
- Moss patches, rift cracks, dust, grime, small debris
- Tilemap collider: OFF
- Sorting order: between Base ve Wall (Layer 1.5)
- Asset source: `create_object` transparent BG (PixelLab)

### Layer 3 — Wall (Wang autotile + Front/Top split)
- Wang 47-tile or 3x3 transition pattern
- Tilemap collider: ON
- Sub-split per Antigravity 4 P0.3: WallsTilemap_Front (collision) + WallsTilemap_Top (walkable)

### Layer 4 — Prop (discrete sprites)
- Pillar, rubble, brazier, large decor (>32px)
- Individual GameObject prefabs, not tilemap tiles
- Drop shadow child (per Antigravity 4 P0.2)

## TileImportWizard (Codex Faz 1.0 zorunlu)

Workflow:
1. **Folder select dialog** → PixelLab batch export folder (`Assets/Art/Tiles/F1/Generated/` veya user-provided)
2. **`asset_000.json` parse** → tile metadata (size, type, palette ref)
3. **Sheet auto-slice** → TextureImporter sprite mode Multiple + grid slice based on metadata
4. **Standard Wang mapping** → 16-tile NSEW mask (Karar #116) OR 47-tile 3x3 (Alabaster Dawn level)
5. **RuleTile auto-create** → Asset/Art/Tiles/F1/Generated/wang_*_RuleTile.asset (mevcut pattern S67 importer'da var)
6. **Multi-layer tilemap iskelet** → Demo scene'de Base/Decal/Wall_Front/Wall_Top/Prop tilemap GameObject create
7. **Brush mode wiring** → Room Designer brush mode dropdown: Paint Base / Paint Decal / Paint Wall / Paint Prop

## Task — Execute all steps

### STEP 1 — TileImportWizard Editor script

Create `Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs`:
- EditorWindow with menu `RIMA > Tile Import Wizard`
- Folder picker → `EditorUtility.OpenFolderPanel`
- JSON parse via `JsonUtility` or Newtonsoft (manifest.json check `com.unity.nuget.newtonsoft-json` mevcut mu)
- Sheet slice: read texture, generate sprite metadata, write to .meta yamls programatik
- RuleTile auto-create: load template `RuleTile_F1_Wang_Template.asset` (Codex create), populate sprite refs per Wang mask
- Output asset path: `Assets/Art/Tiles/F1/Generated/wang_*_RuleTile.asset`

### STEP 2 — RuleTile template asset

Create `Assets/Art/Tiles/F1/Generated/RuleTile_F1_Wang_Template.asset`:
- 16-tile Wang NSEW mask (Karar #116 baseline) — TileImportWizard yeni RuleTile asset'leri bu template'i clone ederek üretir
- Karar #117 Portable Core uyumlu — RIMA palette/biome enum referansı YOK template'te (game-layer concrete sınıfı override eder)

### STEP 3 — Multi-layer tilemap iskelet (Demo scene)

`Assets/Scenes/Demo/RoomPipelineTest.unity` (mevcut, git status M):
- Add 5 new tilemap GameObject hierarchy:
  ```
  Grid
    BaseTilemap (sortingOrder 0)
    DecalTilemap (sortingOrder 1)
    WallsTilemap_Front (sortingOrder 2, TilemapCollider2D ON)
    WallsTilemap_Top (sortingOrder 3)
    PropContainer (empty, prop prefab instantiate here)
  ```
- Mevcut WallsTilemap → WallsTilemap_Front + WallsTilemap_Top split (Antigravity 4 P0.3 alignment)
- Mevcut FloorTilemap → BaseTilemap rename + DecalTilemap eklenmeli

### STEP 4 — Room Designer brush mode extension

`Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs`:
- Toolbar dropdown: brush mode = Base / Decal / Wall / Prop
- Selected layer = active tilemap target for brush controller
- BrushController.cs: target tilemap parameter (currently hardcoded — pass mode)
- CircleBrush + SoftBrush: apply to selected layer (Base only paints autotile, Decal only paints overlay, etc.)
- Karar #117 Portable Core compliance: brush mode enum in Core, layer→mode mapping in Game layer

### STEP 5 — PixelLab Export schema validation

In TileImportWizard, validate JSON schema before sheet slice:
```json
{
  "tile_size": 32,
  "tile_type": "topdown_wang|object|sidescroller",
  "tiles": [
    {"index": 0, "wang_mask": "0000", "sprite_x": 0, "sprite_y": 0, "width": 32, "height": 32}
  ]
}
```
- Bilinmeyen schema → user-friendly error, no crash
- Karar #118 compliance: create_topdown_tileset Pro + create_tiles_pro her ikisi import desteklenmeli (different JSON)

### STEP 6 — TileImportWizard test run

- Mevcut `Assets/Art/Tiles/F1/Generated/wang_floor_wall_tile_*.asset` (git status'ta görüldü) ile test
- Wizard'ı aç, folder select, JSON parse, RuleTile generate
- Existing wang_cold_wall_RuleTile.asset ile comparison — yeni wizard sonucu eşit veya iyi

### STEP 7 — Documentation

Update `TASARIM/RIMA_MASTER_ART_PIPELINE.md` (varsa) veya yeni `TASARIM/MAP_TILE_PIPELINE.md`:
- 4-layer tilemap stack explanation
- TileImportWizard usage
- PixelLab export format expected
- Karar #116/#117/#118 cross-refs

### STEP 8 — Commit

```bash
git add Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs Assets/Editor/RoomDesigner/**/*.cs Assets/Art/Tiles/F1/Generated/RuleTile_F1_Wang_Template.asset Assets/Scenes/Demo/RoomPipelineTest.unity TASARIM/MAP_TILE_PIPELINE.md
git commit -m "[karar118] TileImportWizard + 4-layer tilemap stack (Base/Decal/Wall/Prop)

- TileImportWizard Editor: folder→JSON parse→sheet slice→RuleTile auto
- 4-layer tilemap iskelet (Base + Decal + Wall_Front + Wall_Top + Prop)
- Brush mode dropdown (Karar #117 portable core compliant)
- RuleTile_F1_Wang_Template seed asset
- Existing wang_*_RuleTile.asset migration test pass"
```

### STEP 9 — Report

Write `STAGING/karar_118_tile_import_wizard_report.md`:
```markdown
# Karar #118 TileImportWizard Implementation Report

## TileImportWizard
[functional Y/N, test result on existing wang assets]

## 4-Layer Tilemap Stack
[Base/Decal/Wall_Front/Wall_Top/Prop tilemaps in Demo scene Y/N]

## Brush Mode Extension
[Room Designer dropdown Y/N, layer→mode mapping working Y/N]

## PixelLab Export Schema
[JSON validation Y/N, edge cases handled]

## Test (existing wang assets)
[migration result: pass/fail, comparison notes]

## Pending (next iter)
[Karar #119 AI ASCII Parser entegrasyon, Karar #116 PixelLab Pro re-gen user-side]
```

## Constraints

- Execute every step
- DO NOT touch Karar # locks
- DO NOT delete existing wang_*_RuleTile.asset (S67 commit, korunmalı — wizard test çıktısı NEW asset olsun ayrı path)
- Karar #117 Portable Core compliance — Core/Game layer ayrımı ZORUNLU (TileImportWizard Core'a girer mi tartış: brush mode enum Core'da, RIMA biome RuleTile Game'de)
- Newtonsoft.Json eksikse Packages/manifest.json'a ekleme — JsonUtility kullan (Unity built-in)

## Source References

1. `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #115/#116/#117/#118 spec
2. `Assets/Editor/RoomDesigner/**` — mevcut Room Designer kodu (Antigravity AutoCliff dahil)
3. `Assets/Art/Tiles/F1/Generated/wang_*_RuleTile.asset` — S67 wang importer çıktısı, migration test base
4. `Assets/Scenes/Demo/RoomPipelineTest.unity` — Demo scene (4-layer iskelet eklenecek)
5. `STAGING/PIXELLAB_PRODUCTION_STEPS.md` — PixelLab batch JSON format expectations
