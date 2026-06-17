# DONE A1 - PixelLab Props Import

## Summary
- Imported 10 PixelLab REVIEW-status prop frames by direct public URL download.
- Created sprites under `Assets/Art/Props/Pixellab/`.
- Created `PropDefinitionSO` assets under `Assets/Data/Props/Pixellab/`.
- Appended all 10 new props to `Assets/Resources/Props/PropRegistry.asset`.
- `select_object_frames` called: no.

## Prop Imports
| Prop | Frame | Sprite path | propId | Block |
|---|---:|---|---|---|
| Barrel (weathered RIMA) | 0 | `Assets/Art/Props/Pixellab/BarrelWeatheredRIMA.png` | `e558d9a41f521ed46bb6798b97c6a724` | YES |
| Crate (weathered RIMA) | 12 | `Assets/Art/Props/Pixellab/CrateWeatheredRIMA.png` | `fe41d18234ef76c40be957456ba532d4` | YES |
| Ornate Chest | 6 | `Assets/Art/Props/Pixellab/OrnateChest.png` | `200bff9029c843c49bd368928e125468` | YES |
| Iron Brazier | 0 | `Assets/Art/Props/Pixellab/IronBrazier.png` | `2bc7d949c1d31014caf06a604a010cd1` | YES |
| Broken Pillar | 0 | `Assets/Art/Props/Pixellab/BrokenPillar.png` | `2d67073f92e353a4a8257ea99fb2f366` | YES |
| Cloth Banner | 0 | `Assets/Art/Props/Pixellab/ClothBanner.png` | `c96d65a4ecd03b648a7fab2f5f629812` | NO |
| Statue Fragment | 0 | `Assets/Art/Props/Pixellab/StatueFragment.png` | `b4c1f9aead9037845a44edbab2a27e56` | YES |
| Rubble Pile | 6 | `Assets/Art/Props/Pixellab/RubblePile.png` | `985a6193361a0ce4999aef2755c56e0c` | NO |
| Wall Torch (RIMA) | 4 | `Assets/Art/Props/Pixellab/WallTorch.png` | `70fab5094f5fa6f4ea3ed52ea5767037` | YES |
| Burlap Sack | 10 | `Assets/Art/Props/Pixellab/BurlapSack.png` | `1cbb5e4bfe92f2742855095dea1013b0` | NO |

## Registry And Catalog Verification
- PropRegistry `AllProps` count: 9 -> 19.
- New props with `worldSprite != null`: 10/10.
- BuildModeAssetCatalog verification: `AllProps=19; iconEligible=18; propsEntries=18; newSprites=10/10; newCatalogNames=10/10`.
- Props group entry count equals icon-eligible registry props: 18 == 18.
- All 10 new display names are present in the F2 Props catalog.

## Console
- Unity console Error+Warning check: 0 entries.

## Notes
- Import settings applied through Unity editor API: Sprite Single, PPU 64, Point filter, uncompressed, max size 256, mipmaps off, alpha transparency on, fallback physics shape off.
- PixelLab fallback was not needed because all selected frame URLs downloaded directly.
