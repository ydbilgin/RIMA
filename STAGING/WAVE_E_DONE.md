# Wave E DONE Report (2026-05-19 S94)

Execution agent: Sonnet UnityMCP
Spec: `STAGING/OPUS_WALL_FINAL_DECISION.md` Phase 1-10
Scene mutated: `Assets/Scenes/Demo/RoomPipelineTest.unity`

## Phase completion checklist

| Phase | Status | Note |
|---|---|---|
| 1. Asset import (PPU 64, Point, Pivot) | OK | 4 files reimported (pixellab_wall_section_horizontal BottomCenter; pixellab_wall_arch_section BottomLeft; gate_arch BottomLeft; painterly_prop_06_burning_brazier BottomCenter). Compression set Uncompressed on 3 wall sprites. |
| 2. Scene cleanup (Wave 4 painterly_wall delete) | OK | 108 stale child GameObjects removed from L3_Walls under both Spawn rooms (Wave 4 `TopEdge_02_*`, `BottomEdge_07_*`, `LeftEdge_04_*`, `RightEdge_05_*`, `EntryArch_11`, `AlcoveNiche_12`, `VinesAccent`, etc.). Gate_Entry + Gate_Exit kept and moved into new `Gates/` subfolder. |
| 3. Spawn_01 composition | OK | Built per spec §3 + §4 tables. Hierarchy: Top(4) Bottom(4) Left(6) Right(6) Corners(4) Gates(2) Braziers(4) SeamDecals(18) = 48 SR. |
| 4. Warblade scale 0.85 -> 0.5 | OK | `Warblade_Player` (scene root, single instance shared by both rooms) localScale set to (0.5, 0.5, 1.0). |
| 5. Cyan brazier conversion (Light2D + FlameAdditive + Animator) | OK | 6 braziers converted (4 Spawn_01 + 2 Spawn_02). Each: child `CyanLight` (Light2D Point, color #00FFCC, intensity 1.2, outer 2.0, inner 0.5, falloff 0.8), child `FlameAdditive` SR (cyan alpha 0.7, SO 111), Animator with `Assets/Animations/Braziers/BrazierBreath.controller` (Idle state, intensity 1.0->1.4->1.0 + alpha 0.6->0.8->0.6 over 2.0s loop, ClampedAuto tangents). |
| 6. Spawn_02 variant | OK | Built per spec deltas: stacked gate_arch replaces LG (left at y=3.91), 2 braziers right-side only, V3/V4 bottom vines at (5, -0.3) and (13, -0.3) replace top vines, mirror flipX maintained for right side. 46 SR total. |
| 7. File pack cleanup | OK | `wall_edge_stone.png` -> `Assets/Art/_archive_faz1/walls_superseded/wall_edge_stone.png` via AssetDatabase.MoveAsset (GUID preserved). 12x `painterly_wall_*.png` tagged with `WARM_TONE_VARIANT`. STAGING test PNGs deleted (`spawn01_faz1_polish_v1.png`, `v2.png`); `v3.png`, `test_pixellab.png`, `test_api_dl.png` never existed (no-op). |
| 8. Screenshot | OK | Temp orthographic camera capture (18u x 14u frame, 720p). Files: `STAGING/walls_v3_spawn01.png` (543 KB), `STAGING/walls_v3_spawn02.png` (564 KB). Captures show wall composition isolated; L1_BaseFloor tilemap is empty in scene (separate work), so floor reads black in capture. |
| 9. Test pass | PARTIAL | EditMode run: 421 total, 5 failures, all unrelated to wall work (3x MCP transport disposal log assertion noise, 2x SubRoomSequenceController Destroy-in-edit-mode in newly added Codex Step 1 tests). RoomFlowTests (PlayMode, 16 tests) all fail at SetUp because `_IsoGame` scene missing from build profile — pre-existing environment issue, not wall-related. WallCompositionTests.cs deferred (time pressure, per spec optional). |
| 10. Visual fidelity gate | FLAGGED <80% | See score table below. ~68% — below 80% threshold. Phase 2 mini-regen flagged for orchestrator decision. |

## SR count per Spawn

- Spawn_01 L3_Walls total: 48 GameObjects (Top 4 + Bottom 4 + Left 6 + Right 6 + Corners 4 + Gates 2 + Braziers 4 + SeamDecals 18). Spec target was "~30 SR" but that figure excluded the 16 seam decals from §4; including them brings the engineering target to ~46-48.
- Spawn_02 L3_Walls total: 46 GameObjects (same as Spawn_01 minus 2 left-side braziers, with L_LG_stack replacing LG arch).

## read_console summary

Zero compile errors after all phases. Only ambient MCP-FOR-UNITY "Client handler exited" infos (transport noise, not project warnings). No URP / Light2D / Animator validation errors.

## Test run result

| Mode | Total | Pass | Fail | Notes |
|---|---|---|---|---|
| EditMode | 421 | 416 | 5 | All 5 failures unrelated to Wave E (3 MCP transport log noise, 2 SubRoomSequenceController pre-existing edit-mode Destroy violation in Codex Step 1 code). |
| PlayMode RoomFlowTests | 16 | 0 | 16 | All fail at SetUp: `_IsoGame` scene missing from build profile (environment, not code). |

## Fidelity score (Spec §7 table — Phase 1 prediction was 80-85%)

| Dimension | Spec Phase 1 prediction | Sonnet observation | Achieved % |
|---|---|---|---|
| Wall tone | 90% | Hero `pixellab_wall_section_horizontal` matches Act 1 dark slate well; visually consistent with concept | 88% |
| Wall verticality | 75% | Warblade 0.5 scale not in screenshot (only walls captured). 35-row wall sprite at 6.0×3.375u with Warblade 1.2u = 2.8x ratio as planned | 70% (untested in capture, math holds) |
| Cyan braziers | 95% | Light2D + cyan tint + additive overlay visible; gates also pick up cyan glow from existing setup | 78% (single capture frame doesn't show breath animation) |
| Gate arches | 90% | LG/RG `pixellab_wall_arch_section` reads as gate cutouts; vertical alignment OK | 72% (arch perspective slightly mismatched to horizontal wall row) |
| Seam invisibility | 85% | Top edge: 4 walls @ 1.5u overlap should hide; visually the **repeating brick pattern is identical between segments → seams legible due to texture sameness** rather than gap. Bottom: same issue. Side: gate_arch stacks have visible vertical gaps between segments (each is 2u tall but extends to 2.36u of sprite, so y-spacing of 2.0 gives some overlap but stack reads as separate doorways). | 55% |
| Side wall fidelity | 60% | gate_arch columns read as 5 isolated archways per side — NOT continuous buttressed wall. Concept ref shows monolithic block walls with chains/banners. This is the spec's explicit Phase 1 limitation. | 50% |
| Corner ornamentation | 85% | C-TL/TR/BL/BR `gate_arch` at SO 102 placed correctly; flipY for bottom corners gives reasonable orientation | 75% |
| Floor-wall transition | 75% | Capture has empty floor (L1 tilemap empty in scene); cannot validate transition fidelity from this screenshot alone. The wall sprites' baked rubble lines DO extend below their pivots so transition will read once floor populates. | n/a (deferred to integrated capture with patches/scatter) |

**Weighted average (excluding floor-wall N/A): ~68%.**

## Open issues / BLOCKED items

1. **Phase 2 mini-regen flagged.** Side wall fidelity (50%) and seam invisibility (55%) drag overall fidelity to ~68%, below 80% commit gate. Per spec §2 Phase 2 the recommended remedy is 2-3 PixelLab gens of vertical hero wall sprites (424×632, style ref `b2703abf`) to replace `gate_arch` column stacks on both left and right edges of both Spawn rooms. **Orchestrator decision needed.**
2. **Top/bottom hero wall seam visibility.** Even with mathematical 1.5u overlap, the identical brick pattern repeats and seams are still legible. Possible Phase 2 mitigation: regenerate 2-3 horizontal wall **variants** so adjacent segments don't share pattern, OR add larger seam decals.
3. **Capture limitation.** Screenshots taken with only L3_Walls visible (L1 tilemap empty, L4_Patches and L5_Scatter not in frame focus); a full integrated capture should be redone after Wave G (L4_Patches rebuild) for a fair side-by-side fidelity check vs concept.
4. **WallCompositionTests.cs deferred** (optional per spec).
5. **`_IsoGame` PlayMode test build profile gap** — pre-existing, but worth flagging since it makes runtime RoomFlow regression testing impossible without manual scene load.
6. **2 SubRoomSequenceController edit-mode Destroy failures** in newly added Codex Step 1 tests — needs `DestroyImmediate` swap, separate cleanup task.

## Hard-rule compliance

- DID read OPUS_WALL_FINAL_DECISION.md fully before any tool call. Positions match §3 + §4 tables exactly.
- Used `read_console` after asset reimport, scene mutations, animation creation; no errors.
- Saved scene with `manage_scene save` after composition + Warblade scale + brazier conversion batches.
- DID NOT regenerate any PixelLab asset.
- DID NOT modify Warblade prefab — only the scene instance localScale.
- DID NOT touch `pixellab_wall_corner.png` (rejected purple-tinted).
- DID NOT call `EditorUtility.DisplayDialog` (only `Debug.Log` indirectly via UnityMCP execute_code).
- DID NOT git commit (orchestrator gate).
- Used spec positions verbatim — no improvisation.

## Estimated total time

~50 minutes Sonnet UnityMCP execution (faster than spec's 90 min estimate because asset reimports + scene composition batched into 2 large execute_code calls instead of per-SR manage_gameobject calls). Phase 9 lost ~6 min waiting on a stuck external test job that had to be cleared via reflection.

## Recommendation to orchestrator

Decision required at the 80% gate. Two paths:
- **Path R (recommended, per spec):** Dispatch Phase 2 mini-regen — 2-3 vertical wall PixelLab gens + 1-2 horizontal wall variants (4-5 gens total, within 2150 reserve). Re-paint sides + alternate top/bottom segments. Re-capture and re-score. Target 90%+.
- **Path C (commit-as-is):** Accept 68% as a milestone snapshot, commit Wave E, move to Wave F/G in parallel. Phase 2 regen deferred to a later wave once Wave G floor/patches give a fair integrated comparison.

Files touched (orchestrator commit scope when authorized):
- `Assets/Scenes/Demo/RoomPipelineTest.unity` (Spawn_01 + Spawn_02 L3_Walls rebuilt; Warblade_Player scale)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pixellab_wall_section_horizontal.png.meta` (PPU 64, pivot, point)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pixellab_wall_arch_section.png.meta` (PPU 64, pivot)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_arch.png.meta` (PPU 64, pivot)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/painterly_prop_06_burning_brazier.png.meta` (PPU 64)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/wall_edge_stone.png` MOVED to `Assets/Art/_archive_faz1/walls_superseded/wall_edge_stone.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painterly_wall_01..12.png.meta` (added WARM_TONE_VARIANT label)
- New: `Assets/Animations/Braziers/BrazierBreath.anim` + `Assets/Animations/Braziers/BrazierBreath.controller` (+ folder.meta files)
- Deleted: `STAGING/spawn01_faz1_polish_v1.png`, `STAGING/spawn01_faz1_polish_v2.png`
- New: `STAGING/walls_v3_spawn01.png`, `STAGING/walls_v3_spawn02.png`, `STAGING/WAVE_E_DONE.md`
