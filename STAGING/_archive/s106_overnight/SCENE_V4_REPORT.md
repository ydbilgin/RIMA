# Scene V4 Report

STATUS: DONE

## Changes
- Fixed Light2D target sorting layers on 13 Light2D components.
- Added/updated 4 brazier Flame child sprites from `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/act1_prop_brazier_orange_flame_v01.png`.
- Brightened `L1_Nebula` to `(0.65, 0.30, 0.95)` and `L0_Void` to `(0.18, 0.12, 0.25)`.
- Raised `LightningStreaks` to Ground order -350, set 5/s emission, bright magenta start color, additive material, and cyan-to-magenta trails.
- Scaled 4 pillars to 1.5x, moved them outward by 0.5u, and added warm amber pillar lights.
- Tone check average luminance before/after: 0.026 -> 0.029.
- Global Light 2D intensity before/after: 0.15 -> 0.22 (raised for readability).

## Light2D Target Sorting Layer Verification
Applied target IDs to every Light2D component found after pillar-light creation:
- Default = 0
- Ground = 2024493761
- Floor = 1843609376
- Decals = 1200000001
- Walls = 593505845
- Entities = 1293760285
- BackwallLandmark = 657081444
- Characters = 1200000003
- Props = 1200000004

Light2D objects updated:
- Brazier_NE_WarmLight
- Brazier_NW_WarmLight
- Brazier_SE_WarmLight
- Brazier_SW_WarmLight
- CentralPortal_CyanGlow
- Global Light 2D
- Pillar_AmberLight
- Pillar_AmberLight
- Pillar_AmberLight
- Pillar_AmberLight
- RimLight_East_Cyan
- RimLight_South_Cyan
- RimLight_West_Cyan

## Deliverables
- `Assets/Scenes/Test/PlayableArena.unity`
- `STAGING/s106_overnight/scene_v4_match_attempt.png`
- `STAGING/s106_overnight/scene_v4_vs_M3.png`
- `STAGING/s106_overnight/SCENE_V4_REPORT.md`

## Self Assessment vs M3
The critical sorting-layer fix is applied across every Light2D component, and the requested polish objects are now present: visible flame sprites, brighter BG tints, higher lightning sorting, scaled pillars, and added pillar lights. The capture is still very dark compared with M3 even after the allowed Global Light 2D bump to 0.22, so the remaining gap is tonal readability plus authored asset density: the reference still has a much brighter floor, stronger storm lightning, a larger central boss focal, and richer brazier/pillar silhouettes.
