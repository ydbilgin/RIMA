# Scene V5 Report

STATUS: DONE

## Confirmed Tweaks
- Global Light 2D: 0.22 -> 0.38
- CentralPortal_CyanGlow: 2.5 -> 5
- Brazier_NW_WarmLight: 2.2 -> 4.5
- Brazier_NE_WarmLight: 2.2 -> 4.5
- Brazier_SW_WarmLight: 2.2 -> 4.5
- Brazier_SE_WarmLight: 2.2 -> 4.5
- Bloom override: Intensity 0.7, Threshold 0.9, Scatter 0.7, Tint white.
- Global Volume: created at `Global Volume`.
- Volume profile: created `Assets/Settings/PlayableArena_V5_BloomProfile.asset`.
- Main Camera post-processing: false -> true (UniversalAdditionalCameraData added).
- URP HDR support: true; Renderer2D PostProcessData: present.

## HDR / Bloom Notes
- HDR emission: Skipped: CentralPortal sprite material has no _EmissionColor/_Emission property; no clean HDR material path without adding/replacing materials.
- Bloom path: enabled through scene Global Volume + dedicated VolumeProfile asset; no URP asset mutation was required because Renderer2D already has PostProcessData and the camera now renders post-processing.

## Screenshot Outputs
- `STAGING/s106_overnight/scene_v5_match_attempt.png` (1280x720)
- `STAGING/s106_overnight/scene_v5_vs_M3.png` (2560x720)
- Average luminance v4 -> v5: 0.029 -> 0.053

## Honest Comparison vs M3
Score: 6.5/10. The final numeric polish materially improves readability: floor, central cyan focal, and corner warm pools now read stronger than v4. It still cannot fully match M3's authored boss silhouette, dense lightning forms, and bespoke arena decoration without new art, but this is the strongest version achievable within the final no-new-art micro-polish constraint.
