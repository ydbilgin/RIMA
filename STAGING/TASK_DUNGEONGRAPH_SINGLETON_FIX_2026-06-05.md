# TASK: DungeonGraph singleton destroys Systems GO + sparse-room re-seed + SeamCrawler tag

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
Pre-playtest probe found a BLOCKING bug + 2 smaller issues. User presents soon — fix fast, verify in play mode, commit. Unity Editor is OPEN (UnityMCP available).

## Issue 1 — BLOCKER: Systems GO self-destructs in _IsoGame scenes
`RIMA.DungeonGraph` (Core MonoBehaviour, `Assets/Scripts/Core/DungeonGraph.cs` ~line 58) singleton guard calls `Destroy(gameObject)` on duplicate. The CharacterSelect/Chamber scene leaves `DungeonGraph.Instance` set; when any `_IsoGame*` scene loads, its "Systems" GO (which carries DungeonGraph AND RoomClearVictoryTrigger and possibly more) destroys ITSELF entirely → room clear never fires, doors never unlock.
- Fix surgically: prefer `Destroy(this)` (component only) over `Destroy(gameObject)`, OR clear the stale static Instance on scene unload (e.g. `if (Instance == this) Instance = null;` in OnDestroy + accept the new scene's instance). Think about which is correct given how Instance is consumed elsewhere (rg the usages first). Do NOT restructure the singleton; minimum change that keeps both paths working.
- NOTE: there are TWO DungeonGraph classes (old `RIMA.DungeonGraph` MonoBehaviour in Core — this one — and new `RIMA.MapDesigner.Room.Runtime.DungeonGraph` pure class). Only touch the Core MonoBehaviour.

## Issue 2 — Sparse rooms after prop prune (feel, but presentation-visible)
After the QC re-seed (commit 90c84995), these have too few props:
- combatlarge_twin_basins_01: 1 prop / 599 floor cells (worst)
- combat_large_lshape_01: 1 prop / 351
- combat_large_cross_01: 2 props / 281
Use the RoomQCFixMenu / BridsonPoissonAutoPlacer pipeline you built to RE-SEED just these 3 with a higher density target — aim ~1 prop per 25-40 floor cells (twin_basins ~15-20 props, lshape ~9-12, cross ~7-10), all on-island validated (propOutsideFloor=0), CleanCenter respected.

## Issue 3 — SeamCrawler tag
SeamCrawler enemy is tagged "Untagged" instead of "Enemy". Find where SeamCrawler is defined (prefab or runtime spawn) and fix the tag at the source. Check whether any code filters by tag "Enemy" that would have missed it (report findings, fix the tag regardless).

## Verification (MANDATORY, play mode)
1. PATH A (classic/_IsoGame): Play MainMenu → invoke BAŞLA → in Chamber, programmatically run the classic select path (SelectClass + OnStartRun like the previous probe did) → _IsoGame loads → confirm "Systems" GO ALIVE with RoomClearVictoryTrigger present → kill all enemies via Health.TakeDamage → confirm clear fires (reward/door unlock chain reachable). 
2. PATH B (primary/chamber rift → _Arena): from Chamber, invoke the rift-door confirm path (ChamberSelectBootstrap) → _Arena loads → RoomRunDirector encounter starts → kill enemies → clear → reward → door advance at least 1 room. This is the USER'S PRESENTATION PATH — it must be green end-to-end.
3. Rebuild the 3 re-seeded rooms in edit mode, programmatic on-island check, screenshot each to Assets/Screenshots/RoomQC_v2/ (overwrite).
4. Console 0 errors. Do NOT save scenes. Stop play mode and leave editor on Assets/Scenes/UI/MainMenu.unity.

## Commit
Only the changed files. Conventional message, e.g. `fix(core): DungeonGraph duplicate guard destroyed Systems GO + re-seed sparse rooms + SeamCrawler tag`. Identity ydbilgin, NO Co-Authored-By.

## Output
CODEX_DONE: root-cause choice for #1 (and why), per-room new prop counts, PATH A + PATH B evidence, commit hash.
