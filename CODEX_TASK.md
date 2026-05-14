ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.

# Karar #118a — TileImportWizard script only — execute every step, commit at end

## Context

Karar #118 büyük olduğu için 3 parçaya bölündü. Bu #118a: sadece TileImportWizard Editor script + RuleTile template asset.

#118b (tilemap layers) ve #118c (brush mode) ayrı dispatch'te gelecek.

## STEP 1 — TileImportWizard Editor script

Create `Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs`:

- EditorWindow, menu: `RIMA > Tile Import Wizard`
- Folder picker button: `EditorUtility.OpenFolderPanel("Select PixelLab export folder", "Assets/Art/Tiles", "")`
- JSON parse: scan folder for `*.json` (PixelLab batch export metadata). Use `JsonUtility` (no Newtonsoft required).
- Expected JSON schema:
  ```json
  { "tile_size": 32, "tile_type": "topdown_wang", "tiles": [{"index": 0, "wang_mask": "0000", "sprite_x": 0, "sprite_y": 0, "width": 32, "height": 32}] }
  ```
- If JSON missing or malformed: show EditorUtility.DisplayDialog error, do not crash.
- Sheet slice: for each texture (.png) in folder, call `TextureImporter` set `spriteImportMode = SpriteImportMode.Multiple`, generate `SpriteMetaData[]` from JSON tile entries, call `AssetDatabase.ImportAsset`.
- RuleTile auto-create: for each wang tileset found, create a new `RuleTile` asset at `Assets/Art/Tiles/F1/Generated/wang_{name}_RuleTile.asset`. Do NOT overwrite existing assets with same name (append `_new` suffix).
- Use 16-tile NSEW Wang mapping (Karar #116 baseline): neighbors = N, S, E, W bitmask 0-15.
- Progress bar: `EditorUtility.DisplayProgressBar` during slice + import.

## STEP 2 — RuleTile template asset

Create via `AssetDatabase.CreateAsset` in STEP 1 code or separate script:
`Assets/Art/Tiles/F1/Generated/RuleTile_F1_Wang_Template.asset`

- Standard 16-rule RuleTile with NSEW neighbors
- All sprite refs null (placeholder template — wizard clones this and fills refs)
- No RIMA biome enum references in this template (Karar #117 Portable Core compliance)

## STEP 3 — Compile check

After creating the scripts:
- Use Unity MCP `read_console` to check for compilation errors
- If errors: fix them. Do not commit broken code.

## STEP 4 — Test

Open TileImportWizard (RIMA > Tile Import Wizard).
Test with existing `Assets/Art/Tiles/F1/Generated/` folder (wang tile PNGs from S67).
Verify: no crash, progress bar shows, output .asset files generate.

## STEP 5 — Commit

```bash
git add Assets/Editor/RoomDesigner/Tools/TileImportWizard.cs Assets/Art/Tiles/F1/Generated/RuleTile_F1_Wang_Template.asset
git commit -m "[karar118a] TileImportWizard Editor script + RuleTile_F1 template

- Folder picker → JSON parse → sheet slice → RuleTile auto-create
- 16-tile NSEW Wang mapping (Karar #116)
- Karar #117 portable core compliance (no biome refs in template)
- Test: existing F1 Generated folder import OK"
```

## STEP 6 — Report

Write `STAGING/karar_118a_wizard_report.md`:
```
# Karar #118a TileImportWizard Report

## TileImportWizard
[functional Y/N]
[test result on existing wang assets]
[any issues]

## RuleTile Template
[created Y/N]

## Console
[0 errors Y/N]
```

Append CODEX_DONE.md:
```
## [2026-05-14] Karar #118a TileImportWizard
- TileImportWizard.cs created: Y/N
- RuleTile template: Y/N
- Compile errors: none/list
- Test: pass/fail
```

## Constraints

- DO NOT overwrite existing wang_*_RuleTile.asset files (append _new suffix)
- DO NOT implement brush mode or tilemap layers (that is #118b/#118c)
- Karar #117: TileImportWizard goes in Editor layer, template asset has NO game-layer dependencies
- Use JsonUtility (built-in), NOT Newtonsoft

## Source References

1. `Assets/Editor/RoomDesigner/` — mevcut Room Designer kod (referans)
2. `Assets/Art/Tiles/F1/Generated/` — S67 wang importer output (test input)
3. `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #116/#117/#118 referans (sadece ilgili satırlar)


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.