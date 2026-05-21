# Pilot A Visual Review + Walls Diagnose — Codex Report

## Bölüm 1: Walls 55 vs 52 Mismatch
### Enumerate Result
- Total Walls renderer: 55
- Unity summary:
``text
scenePath=Assets/Scenes/Demo/PathC_BaseTest.unity
sceneDirty=False
pilotATestExists=False
layerCounts=Entities:8, Walls:55
wallsByRoot=Grid:43, Props_Root:12
gridChildren=52
propsRootChildren=12
``
- GameObject list:
  - 1. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-78.89,-41.795,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 2. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-78.42,-41.56,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 3. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-77.48,-42.03,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 4. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-76.54,-42.03,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 5. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-75.13,-41.795,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 6. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-74.19,-41.795,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 7. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-73.25,-41.795,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 8. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-72.31,-41.795,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 9. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-71.84,-41.56,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 10. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-70.9,-43.91,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 11. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-70.43,-41.795,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 12. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-69.96,-39.21,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 13. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-67.906,-40.714,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 14. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-66.2,-44.85,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 15. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-65.821,-39.927,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 16. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-64.32,-44.85,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 17. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-63.85,-41.325,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 18. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-61.97,-41.325,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 19. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-61.03,-44.615,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 20. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Grid): pos=(-60.09,-44.615,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 21. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-77.95,-41.325,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 22. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-77.01,-41.325,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 23. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-77.01,-41.325,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 24. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-77.01,-41.325,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 25. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-76.54,-41.09,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 26. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-76.54,-41.09,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 27. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-76.54,-41.09,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 28. wall_01_8530799c-41f8-4df5-a76f-28f49e6d71c7 (Grid): pos=(-61.97,-44.615,0), prefabGUID=c80818f3996bf5545804b2a10679258e
  - 29. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-77.95,-40.855,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 30. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-77.01,-41.325,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 31. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-77.01,-40.855,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 32. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-71.37,-41.325,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 33. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-63.38,-44.85,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 34. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-63.38,-44.38,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 35. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-62.91,-44.615,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 36. wall_02_a52f6711-e1e6-434d-a178-9eebd8521f91 (Grid): pos=(-61.5,-44.85,0), prefabGUID=4427eebe5f5ef2941a8f18931a033686
  - 37. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-77.95,-41.325,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 38. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-77.95,-41.325,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 39. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-77.01,-41.325,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 40. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-77.01,-41.325,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 41. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-77.01,-41.325,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 42. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-77.01,-41.325,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 43. wall_03_abf9c178-4a5b-4cb2-a3e7-3729eeaceec3 (Grid): pos=(-63.85,-44.615,0), prefabGUID=f63c940199fa2f84c9933fbbe4285dbb
  - 44. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-68.976,-42.293,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 45. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-68.55,-43.205,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 46. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-68.08,-39.68,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 47. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-67.14,-43.44,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 48. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-66.156,-41.823,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 49. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-65.73,-43.675,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 50. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.79,-40.385,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 51. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.79,-38.975,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 52. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.32,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 53. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.32,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 54. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.276,-42.293,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 55. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-63.38,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c

### Antigravity Cross-Check
- Antigravity raporu 52; live Unity count is 55 active SpriteRenderer on sortingLayerName Walls.
- Cross-check by root: Grid has 43 Walls SpriteRenderer; Props_Root has 12 Walls SpriteRenderer; all 55 are active and none are hidden/disabled.
- Grid.childCount is 52, but that is not a Walls-renderer count: it includes Floor_Tilemap, 43 wall SpriteRenderers, and 8 Entity SpriteRenderers.
- The 12 Props_Root Walls candidates are:
  - 44. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-68.976,-42.293,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 45. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-68.55,-43.205,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 46. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-68.08,-39.68,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 47. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-67.14,-43.44,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 48. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-66.156,-41.823,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 49. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-65.73,-43.675,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 50. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-64.79,-40.385,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 51. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-64.79,-38.975,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 52. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-64.32,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 53. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-64.32,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 54. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-64.276,-42.293,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 55. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f: pos=(-63.38,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
- If the current deterministic sorted list is compared directly against Antigravity's claimed 52, the +3 tail entries are:
  - 53. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.32,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 54. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-64.276,-42.293,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
  - 55. wall_00_65c99904-12b8-4b98-9e5f-fe2f280f6a2f (Props_Root): pos=(-63.38,-40.15,0), prefabGUID=1f9527adb7c6df441b1678146669eb5c
- Most likely cause: Antigravity's 52 was a stale/simple hierarchy count, not the current live Walls SpriteRenderer count. The scene has 55 live active Walls SpriteRenderers; the mismatch is not caused by inactive objects or TilemapRenderer/SpriteRenderer mixing.
- Verdict: needs cleanup/reconciliation if 52 is intended, because Props_Root contains 12 active wall instances and the scene has no metadata identifying exactly three canonical extras.

## Bölüm 2: Pilot A Visual Review
### PNG Download
- 4 frame downloaded: FAIL / BLOCKED
- Shell Invoke-WebRequest failed: unreachable network to ackblaze.pixellab.ai.
- Shell curl.exe -L --retry 3 failed with exit 7: could not connect to ackblaze.pixellab.ai:443.
- Test-NetConnection backblaze.pixellab.ai -Port 443: TcpTestSucceeded=False.
- UnityWebRequest failed: ConnectionError, responseCode  , error Cannot connect to destination host.
- Sizes: not available.
- STAGING/pilot_a_candidates/ klasörü oluşturuldu; no PNG files were downloaded.

### Unity Import
- SKIPPED: blocked by PNG download access failure.
- No files copied to Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/.
- Import setting verify: not run.

### Scene Placement
- SKIPPED: blocked by PNG download access failure.
- No PilotATest_S95v2 instances were created.

### Screenshot
- SKIPPED: blocked by PNG download access failure.
- STAGING/pilot_a_visual_review.png not written.

### Cleanup
- PilotATest_S95v2 exists: NO
- Pilot test hierarchy delete needed: NO
- PNG asset'ler korundu: no asset PNGs were created.
- Scene dirty: NO

## Açık Sorular
- ackblaze.pixellab.ai is unreachable from both shell and Unity in this environment; Pilot A visual placement cannot proceed until the PNGs are locally available or the host is reachable.
- For Walls cleanup: decide whether Props_Root 12 active wall instances are intentional. Without a baseline marker, there is no reliable way to isolate exactly three extras from the 55 live renderers.
