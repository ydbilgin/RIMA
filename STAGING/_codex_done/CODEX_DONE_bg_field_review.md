# Codex Review: RoomTemplate backgroundSprite Field - VERDICT

## Verdict: FAIL

## Checklist
1. Diff surgical: FAIL (evidence: `git diff --stat` reports 36 changed tracked files, not only the 2 target files. Target files show `RoomTemplateSO.cs | 4 +` and `RoomBankRuntimeTester.cs | 24 +`.)
2. schemaVersion: PASS (`Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:9` remains `public string schemaVersion = "1.0";`; `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs:50` still asserts `"1.0"`.)
3. Asset deserialization: PASS (UnityMCP `AssetDatabase.LoadAssetAtPath<RoomTemplateSO>` loaded 10/10 assets from `Assets/Data/Rooms/Library/*.asset`.)
4. EditMode test suite: PASS (UnityMCP EditMode run succeeded, 419/419 completed.)
5. Walkable grid: PASS (`IsWalkable(Vector2Int)` remains at `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:36`; `git diff` for this file only adds the background field block.)
6. Runtime null safety: PASS (`Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs:63-84` checks `picked.backgroundSprite != null`, logs both branches, and parents via `result.roomInstance != null ? result.roomInstance.transform : transform`.)
7. SpriteRenderer params: PASS (`Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs:75-78` assigns sprite, `sortingOrder = -100`, and `drawMode = SpriteDrawMode.Simple`; z position is set to `1f` at line 71.)
8. Test addition needed: NO

## Risk findings
1. Scope risk: repository diff is not surgical. `git diff --stat` includes 36 changed tracked files, including `.claude/PROJECT_RULES.md`, `.gitignore`, multiple `Assets/Data/Blueprint/*` assets, editor scripts, scene files, docs, and the two target files. This fails the requested "only these 2 files changed" check even though the target code diffs themselves match the requested implementation.

## Recommendation
FIX FIRST
