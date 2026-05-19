v15e-B secondary cluster cap DONE

Implemented:
- Added BlueprintZoneTypeSO.secondaryClusterCap with default 4.
- Moved L5 small scatter in the composition-budget path to capped secondary clusters.
- Preserved cap=0 as legacy unbounded L5 density placement.
- Added secondary cluster metrics and budget validation.
- Updated v15e-B composer metric/screenshot paths.
- Set zone_stone secondaryClusterCap=4, zone_grass secondaryClusterCap=3, other root zone assets=4.
- Added two EditMode tests for secondary cap and cap=0 legacy behavior.

Verification:
- git diff --check: clean except pre-existing CRLF warnings in unrelated files.
- Unity batchmode shell command was attempted and refused because the project is already open in Unity.
- Active Unity editor EditMode run: 411 passed, 0 failed, 0 skipped, state Passed.
- RimaV15cSceneComposer.Build() completed in the active editor.

Artifacts:
- STAGING/CODEX_DONE_v15e_B_secondary_cluster_cap_metrics.txt
- Assets/Screenshots/PlayableRoom_combat_v15e_B_secondary_cap_LIVE.png

Metrics:
- Secondary clusters: 11 / 11 cap -- OK
- L8 atmospheric: 30 / 56 cap -- OK
- Budget failures: none

No commit was made.
