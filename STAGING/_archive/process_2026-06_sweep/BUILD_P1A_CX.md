ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear. Unity is OPEN — use UnityMCP. After edits run read_console; report compile status. Do NOT self-certify; Opus+agy will review.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" (only if needed).

# BUILD — PHASE 1 Batch A: demo-loop blockers + VFXRouter (design-independent)

You are the WRITER. Implement the 3 items below in the EXISTING codebase. These are demo blockers,
design-independent (no story/lighting/art dependency). Scene = `Assets/Scenes/Test/PlayableArena_Test01.unity`.
⚠️ TWO RoomLoaders exist: use the LIVE one `Assets/Scripts/Systems/Map/RoomLoader.cs` (NOT `Assets/Scripts/Map/Runtime/RoomLoader.cs`).

## A1 — Boss-death → class-select race (softlock fix)
- Problem: `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs:562-572` triggers class-selection on death, while
  `RoomLoader.cs:357-374` raises DemoComplete from boss Health death → race/softlock.
- Fix: in the DEMO boss flow, BYPASS the class-select trigger so DemoComplete/Victory owns the end. Prefer a guard
  (e.g. a serialized `bool suppressClassSelectOnDeath = true` on the boss, or skip when running the demo room flow).
  Do NOT delete the class-select code (other flows may use it) — guard it.
- Verify: boss death path reaches DemoComplete WITHOUT opening class-select.

## A2 — Death-screen scale-0 (death screen invisible)
- Problem: scene `DeathScreenCanvas` root localScale is {0,0,0} (`PlayableArena_Test01.unity:20838-20839`) and
  `DeathScreenManager` fields are null / rely on GameObject.Find (`:20759-20766`).
- Fix: ensure the death screen actually SHOWS when the player dies — correct the root scale (to 1) and/or make
  DeathScreenManager set active+scale on show. Confirm DeathScreenManager wires its named children correctly.
- Verify: trigger player death (or simulate) → death screen renders, not zero-scaled.

## A3 — VFXRouter.entries populate
- Problem: `VFXRouter.entries` is empty (`PlayableArena_Test01.unity:10726`), so wired hit-confirm VFX don't all fire.
- Fix: populate entries mapping CombatEventBus tags → existing pooled prefabs:
  hit_default → `Assets/Prefabs/VFX/HitSpark.prefab`, kill_default → `Assets/Prefabs/VFX/DeathBurst.prefab`,
  dash_default → existing dash/trail prefab (find best match; if none, leave dash out and NOTE it),
  status_default → best existing match or omit + NOTE. Use prefabs that already exist; do not create art.
- Verify: entries non-empty; a hit produces a spark in play mode if testable.

## DELIVER (write to DONE file)
Per item: files changed (file:line), what changed, compile status (read_console: 0 errors required), play-verify result
or why not verifiable. List any item you marked BLOCKED and why. Keep it concise and factual.
