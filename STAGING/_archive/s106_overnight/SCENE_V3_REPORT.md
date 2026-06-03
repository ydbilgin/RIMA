# Scene V3 Match Attempt Report

## Asset paths used

| Role | Status | Path |
|---|---|---|
| Brazier / mounting apparatus | Found exact ID 41342e20 | `Assets/Sprites/Environment/ShatteredKeep_PixelLab/Props/mounting_14_41342e20-39a8-4482-9775-48abf9f05262.png` |
| Central portal / decal | Found exact ID 5ccc5721 | `Assets/Sprites/Environment/PixelLab_Selected_Assets/alabaster_decal_5ccc5721-3007-4d9a-8fce-86e99bc6a078.png` |
| Corner pillar | Exact PixelLab ID 6b52751d found in catalog only; used local keyword match | `Assets/Art/AssetPacks/Act1_ShatteredKeep/pillars/act1_pillar_broken_granite_v01.png` |
| Cyan cliff variant | Found local asset | `Assets/Sprites/Environment/KitB_Cliff/cliff_cyan_glow.png` |

## Change checklist

| Change | Status | Notes |
|---|---|---|
| 1. Four corner braziers | Completed | Placed 4 `Brazier_*` objects at arena corners, Floor sorting order 5, each with warm Point Light2D intensity 2.2 and radius 1.5. Removed old standalone NW/NE warm lights and SW/SE cyan crystal lights. |
| 2. Central portal and glow | Completed | Added `CentralPortal` at origin with 5ccc5721 decal, Floor sorting order 1, scaled to about 2x2 world units, plus cyan Point Light2D intensity 2.5 radius 2.0. |
| 3. Corner framing pillars | Completed | Placed 4 broken granite pillars just outside the arena bounds, Floor sorting order 8. |
| 4. High-contrast lighting | Completed | Global Light 2D set to 0.15, braziers boosted to 2.2, S/E/W cyan rim lights added at intensity 0.8 radius 1.0, LightPulse attached to the SW brazier light. |
| 5. Purple storm BG | Completed | Tinted L1_Nebula, L0_Void, and L2_Ruins to violet/dark-violet/purple-grey. Added `LightningStreaks` particle system under `RoomBackgroundRig`. |
| 6. Arena slight expansion | Completed | Repainted floor from 41 cells to 59 cells using the requested oval footprint, with central 5 cells biased toward rune tile variants. |
| 7. Cliff variety | Completed | Offset cliff ring positions, scaled cliffs to 0.92, and swapped 5 south cliff sprites to `cliff_cyan_glow`. |

## Output files

| File | Size |
|---|---|
| `STAGING/s106_overnight/scene_v3_match_attempt.png` | 1280x720 |
| `STAGING/s106_overnight/scene_v3_vs_M3.png` | 2560x720 |

## Self-assessment vs M3

Score: 7.0 / 10.

V3 is materially closer than V2: the arena is wider, the center has a clear glow focal, the corners now have warm light anchors, and the background reads more purple/storm-like. Remaining gap is mainly asset fidelity: the imported mounting apparatus and local broken pillar are usable but still less rich than the M3 reference, and the lightning particles are a light atmospheric pass rather than a fully authored storm backdrop.

## Console verify

Unity console after apply/check: 0 errors, 0 warnings.
