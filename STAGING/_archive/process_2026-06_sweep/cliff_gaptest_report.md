# Cliff Gap Stress Test Report

Copy scene: Assets/Scenes/_IsoGame_cliffgaptest.unity
Final active variant in copy: VAR1 (interior-push), per task instruction.
Live scenes modified: no. SHA256 hashes for _IsoGame, _IsoGame_Map02, and _IsoGame_Map03 matched before/after.

## Removed Hole Cells

Hole A, western 3-cell line: (5,14), (5,15), (5,16)
Hole B, eastern 2x2: (21,3), (22,3), (21,4), (22,4)
Constraint note: Satisfied western/eastern third, 3-cell boundary clearance, and 6-cell separation constraints.

## Primary Screenshots

- Assets/Screenshots/cliff_gaptest_var0_game.png
- Assets/Screenshots/cliff_gaptest_var0_scene.png
- Assets/Screenshots/cliff_gaptest_var1_game.png
- Assets/Screenshots/cliff_gaptest_var1_scene.png
- Assets/Screenshots/cliff_gaptest_var2_game.png
- Assets/Screenshots/cliff_gaptest_var2_scene.png

## Tight Hole Screenshots

- Assets/Screenshots/cliff_gaptest_var0_holeA.png
- Assets/Screenshots/cliff_gaptest_var0_holeB.png
- Assets/Screenshots/cliff_gaptest_var1_holeA.png
- Assets/Screenshots/cliff_gaptest_var1_holeB.png
- Assets/Screenshots/cliff_gaptest_var2_holeA.png
- Assets/Screenshots/cliff_gaptest_var2_holeB.png

## Variant Metrics

### VAR0 current

Cliff count: 136
Overflow heuristic count: 136
Overflow samples by DIR:
- S: cliff_0_0_S, cliff_1_0_S, cliff_2_0_S, cliff_3_0_S, cliff_4_0_S, cliff_5_0_S, cliff_6_0_S, cliff_7_0_S
- SE: cliff_15_2_SE, cliff_21_5_SE, cliff_30_13_SE, cliff_5_17_SE
- SW: cliff_23_3_SW, cliff_2_11_SW, cliff_6_14_SW, cliff_17_22_SW
- E: cliff_14_1_E, cliff_31_1_E, cliff_14_2_E, cliff_31_2_E, cliff_31_3_E, cliff_20_4_E, cliff_31_4_E, cliff_20_5_E
- W: cliff_22_2_W, cliff_23_2_W, cliff_1_10_W, cliff_2_10_W, cliff_6_13_W, cliff_16_21_W, cliff_17_21_W, cliff_1_23_W

### VAR1 interior-push

Cliff count: 136
Overflow heuristic count: 136
Overflow samples by DIR:
- S: cliff_0_0_S, cliff_1_0_S, cliff_2_0_S, cliff_3_0_S, cliff_4_0_S, cliff_5_0_S, cliff_6_0_S, cliff_7_0_S
- SE: cliff_15_2_SE, cliff_21_5_SE, cliff_30_13_SE, cliff_5_17_SE
- SW: cliff_23_3_SW, cliff_2_11_SW, cliff_6_14_SW, cliff_17_22_SW
- E: cliff_14_1_E, cliff_31_1_E, cliff_14_2_E, cliff_31_2_E, cliff_31_3_E, cliff_20_4_E, cliff_31_4_E, cliff_20_5_E
- W: cliff_22_2_W, cliff_23_2_W, cliff_1_10_W, cliff_2_10_W, cliff_6_13_W, cliff_16_21_W, cliff_17_21_W, cliff_1_23_W

### VAR2 edge-anchored

Cliff count: 136
Overflow heuristic count: 61
Overflow samples by DIR:
- S: none
- SE: none
- SW: none
- E: cliff_14_1_E, cliff_31_1_E, cliff_14_2_E, cliff_31_2_E, cliff_31_3_E, cliff_20_4_E, cliff_31_4_E, cliff_20_5_E
- W: cliff_22_2_W, cliff_23_2_W, cliff_6_13_W, cliff_16_21_W, cliff_17_21_W, cliff_1_23_W, cliff_2_23_W, cliff_3_23_W

## Read

VAR2 has the fewest overflow hits and looks most tucked in the tight hole screenshots. VAR1 was left active in the saved copy as requested, but based on this stress test VAR2 is the stronger geometry candidate for Opus review.
