# Codex Review -- Sprint 10 Implementation DONE

**Date:** 2026-05-16
**Reviewer:** Codex (laurethgame)
**Verdict:** PASS-WITH-CONDITIONS (90% compliance)

## EC Matrix

| EC | Criterion | Status | Evidence |
|---|---|---|---|
| EC-1 | dotnet build PASS | ASSUMED-VERIFIED | Not run per task. `STAGING/codex_review_sprint10_impl.md` states Opus confirmed build PASS. |
| EC-2 | Brush V1 tests unchanged | ASSUMED-VERIFIED | Review spec states the 3 failures are pre-existing: Karar143K, EraseAllDecorative NRE, GridTileExecutor NRE. Not Sprint 10-caused. |
| EC-3 | Save->reload roundtrip | PARTIAL | Test exists at `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs:33`. It saves and reloads at lines 42-48, but assertions at lines 49-62 do not cover all populated fields from lines 146-159. Missing assertions include player spawn `socketId`/`facing`, door socket `socketId`/`position`/`direction`/`widthInTiles`, enemy socket positions and `tierHint`, `cameraBounds.tileRect`, `difficultyTags`, and `blockerTags`. |
| EC-4 | GUID stability | VERIFIED | `RoomTemplateSaver` uses `EditorUtility.CopySerialized(template, existing)` for existing assets at `Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs:83`; new assets use `AssetDatabase.CreateAsset` at lines 92 and 97. No `AssetDatabase.DeleteAsset` in saver. Test exists at `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs:71`. |
| EC-5 | Pick deterministic | VERIFIED | `RoomBankSO.Pick` returns null for empty lists at `Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs:19-21`, uses `unchecked(seed * 1103515245 + 12345)` at line 24 and `(hashed & 0x7FFFFFFF) % list.Count` at line 25. Test exists at `Assets/Tests/EditMode/Room/RoomBankPickTests.cs:43`. |
| EC-6 | PlayMode spawn | VERIFIED | Test exists at `Assets/Tests/PlayMode/Room/RoomBankRuntimeSpawnTests.cs:77`; player/enemy distance checks are within `0.1f` at lines 93-96. `RoomBankRuntimeTester.RunTest` spawns player and enemy from socket positions at `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs:62-82` and validates exit socket at lines 90-94. |
| EC-7 | Validator returns List | VERIFIED | `RoomTemplateValidator.Validate` returns `List<RoomValidationIssue>` at `Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs:9-42`; null template path returns structured issues at lines 11-17. `ValidateBank` returns structured lists at lines 44-55. |
| EC-8 | No editor ref from runtime | VERIFIED | `SaveLoadResults`, `RoomTemplateSaver`, `RoomTemplateLoader`, and `RoomTemplateMenu` are `#if UNITY_EDITOR` gated. Runtime tester has no references to saver, loader, or save/load result classes. |
| EC-9 | No non-integer scale | VERIFIED | `rg "localScale|transform.localScale"` across the listed Sprint 10 files returned no matches. |
| EC-10 | Sorting layers clean | ASSUMED-VERIFIED | No sorting-layer code appears in Sprint 10 source files; task says mark assumed verified. |

## Spec Drift Findings

- `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs:33-62` does not fully satisfy EC-3's "all fields in RoomTemplateSO" verification despite the test name. The implementation may roundtrip the data, but the test does not prove it for every field and nested socket property.
- Listed reference file `memory/project_s86_opus_signoff_decisions.md` was not present at the requested path, so this review could not directly verify that signoff document. The two other reference specs were read.
- Optional fixture `Assets/Data/Rooms/ShatteredKeep/combat_shatteredkeep_test_001.asset` is absent. This is acceptable per the task because tests create assets at runtime.

## Forbidden List

ALL CLEAR -- no forbidden items found. Explicit search across the listed Sprint 10 files found no `CompositionRoleMap`, Natural Engine composition logic, Bridson/Poisson sampler, `PropDefinitionSO`, Props Mode, `SpriteAtlas`, per-biome packing, AI tag suggestion, Auto-Dress/AutoDressExecutor, Markov clustering, or sub-template room references.

## Risk List

R1 (P1): Roundtrip coverage gap. The save/load test populates fields that it never asserts after reload, so future serialization regressions could pass unnoticed.

R2 (P2): The signoff memory reference was missing at the requested path, so review traceability to that document is incomplete.

R3 (P2): `RoomBankSO` exposes public direct-reference lists at `Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs:10-14`. The `Pick` and `AllRooms` methods provide a reasonable adapter surface, but callers that directly consume those public lists would break if the fields are later swapped to Addressable `AssetReference` lists.

## Recommended Fixes

- `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs:47`: after loading `reloaded`, assert every field populated in `MakePopulatedTemplate`, including nested socket IDs, directions/facing, widths, positions, enemy `tierHint`, `cameraBounds.tileRect`, `difficultyTags`, and `blockerTags`.
- `memory/project_s86_opus_signoff_decisions.md`: restore the missing reference file or update the review prompt to the correct path.
- `Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs:10`: before Sprint 11 Addressables work, either document that callers must use `Pick`/`AllRooms` instead of public lists or add accessor methods that can survive a storage swap.

## Sprint 11 Forward-Compat

The RoomBankSO + RoomTemplateSO API is sufficient for Sprint 11 CompositionRoleMap to query door sockets and player/enemy spawn positions without restructuring the room template data. `RoomTemplateSO` exposes `doorSockets`, `playerSpawn`, and `enemySpawnSockets` directly, and `RoomBankSO` exposes deterministic `Pick`, typed room lists, and `AllRooms`.

The Addressables path is conditionally safe: callers that use `Pick` or an adapter over `AllRooms` can keep the same high-level flow, but direct public list access is a weak point because replacing `List<RoomTemplateSO>` fields with `AssetReference` fields would be a source-level breaking change for those callers.

---
