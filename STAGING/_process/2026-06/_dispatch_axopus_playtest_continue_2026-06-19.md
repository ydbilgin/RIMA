# ax_opus Playtest Continuation — Human-Like Demo Loop Sweep (2026-06-19)

ACTIVE RULES: (1) think before acting (2) min footprint, no speculation (3) surgical — touch nothing outside scope (4) BLOCKED if unclear, do not silently partial.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

UNITY ERROR CHECK: when done, read_console (Error+Warning); a stray MCP `ExecuteCode.cs` "objects not cleaned up" error is a known tooling artifact (ignore but report it). Report final console state explicitly. Do NOT "fix" engine code — this is a TEST pass.

GRAPHIFY: for any cross-file/architecture question use graphify query FIRST (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`), ~71x cheaper than bulk-read.
  e.g. `graphify query "how does RoomClearSequence restore time scale and open the exit door" --graph STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`

---

## 🔒 SCOPE — TEST ONLY (HARD)
You are continuing an in-progress **playtest sweep**. You are the **single** Unity-driving agent (no parallel Unity work).

**ABSOLUTELY FORBIDDEN:**
- ❌ NO `git add` / `git commit` / `git push` / `git checkout` / `git restore` / `git stash`. Touch git in NO way.
- ❌ NO reverting / editing / "cleaning up" the working tree. It is **intentionally dirty** (Warblade mount tool + 8-way facing fix + earlier playtest patches are uncommitted on purpose).
- ❌ NO saving scenes / prefabs / assets / ScriptableObjects to disk. No `manage_scene save`. No prefab apply.
- ❌ NO permanent C# edits. If a runtime repair is needed to get PAST a blocker, do it **in-memory at runtime only** (e.g. recreate a component live) and **label it clearly as a runtime workaround** in the report — never persist it.

**ALLOWED:** read scripts, run/stop Play Mode, `EditorApplication.Step()`, execute_code probes (read state / lethal-damage shortcut to clear rooms / invoke buttons), screenshots, console reads, graphify/NLM queries.

If something can ONLY be verified by editing code or saving an asset → mark that item **BLOCKED** and report it; do not do it.

---

## CONTEXT — where the previous (Codex) account left off
Read these first (direct-read OK): `CURRENT_STATUS.md` (PLAYTEST TRACKER T0–T10 block) and `STAGING/_process/2026-06/CODEX_HANDOFF_PLAYTEST_2026-06-18.md`.

Already verified PASS by Codex: **T0** compile/console, **T1** MainMenu→CharacterSelect, **T2** chamber skill-bar, **T3** Arena opening draft (dbCount=111, 3 cards), **T4** opening skill select, **T5** combat/facing (W/W+D/D body+weapon synced), **T6** room clear→reward draft, **T8** reset→fresh opening draft (death path NOT tested).

Patches already applied (uncommitted, in working tree — DO NOT revert, test WITH them):
`MainMenuController.cs` (button listener), `SkillBarUI.cs` (slot ref recovery), `SkillDatabase.cs` (EnsureRuntime/IsUsable + `Build()` guard `if (built && db.Count>0) return;`), `DraftManager.cs` + `SkillOfferGenerator.cs` (EnsureRuntime).

**Two unresolved bugs (root-cause already hypothesized — your job is to CONFIRM rigorously, not fix):**
- 🔴 **T7 — 2nd reward draft does NOT pause.** In a 2-room run, the second `RewardPickup` reward draft opened with `Time.timeScale=1` (no slow-mo/pause), `lifecycle=DoorOpen`. Hypothesis: exit door / `DoorOpen` lifecycle advances BEFORE reward is collected, and `RoomClearSequence`'s `finally RestoreGameplayTimeScale()` sets `timeScale=1` while `SkillOfferUI` is still open, overwriting the UIManager overlay pause state.
- 🔴 **T9 — DraftManager singleton dies on scene reload.** `DraftManager.OnDestroy()` sets static `_shuttingDown=true`; the Instance getter then treats a freshly-loaded instance as null → non-Warblade class runs get NO opening draft. (`DraftManager_Auto` exists in scene but `DraftManager.Instance==null`.)
- 🟡 **T6 — DESIGN question (do NOT decide/fix):** exit door opened while reward `collected=false`; player can leave without taking the reward. Just confirm whether this reproduces and document it for a human design call.

---

## YOUR TASKS (priority order — demo is imminent, ~today)

### P1 — Code health gate
- `refresh_unity(scope="scripts", compile="request", wait_for_ready=true)` → `read_console(types=["error","warning"])`. If real (non-MCP-artifact) errors exist, STOP and report; do not proceed.

### P2 — Fresh full golden path (re-verify patches hold)
Fresh Play from `MainMenu`. Prefer real UI interaction; if MCP synthetic keyboard/click is unreliable (Codex saw `moveAction.ReadValue` stay zero despite pressed keys, and `onClick.Invoke()` needed instead of physical click) → fall back to `onClick.Invoke()` / direct field set and **say so explicitly** in the report (mark as shortcut).
Path: MainMenu → click Başla → CharacterSelect → pick **Warblade** → chamber (Q/E/R/F icons+names filled, `SkillBarUI.slots[]` non-null) → enter `_Arena` → opening draft auto-opens (3 skill cards, `timeScale=0`, NOT gold/heal fallback) → select Iron Charge → draft closes, `timeScale=1`, Q slot fills → use skill on enemy (cooldown starts, countdown renders) → clear room → `RewardPickup` spawns → collect → reward draft opens. Screenshot each milestone.

### P3 — T7 freeze root-cause (HIGHEST demo risk)
Determine whether the frozen 2nd-reward-draft (`timeScale=1`) reproduces in **NORMAL** play, or only when using the `ForceCollect()` / forced-lethal-damage shortcut. Trace the real ordering with graphify + targeted probes:
- When does `lifecycle` become `DoorOpen` relative to reward collect? Is `DoorOpen` before collect a forced-kill artifact or real?
- Does `RestoreGameplayTimeScale()` run while `UIManager.IsSkillOfferOpen==true`? Does it overwrite the overlay pause?
Run at least 2 reward loops in one run. Capture state values (`draftActive`, `UIManager.IsSkillOfferOpen`, `Time.timeScale`, `lifecycle`) at the moment the 2nd reward draft opens. Screenshot. Verdict: **real bug in normal play / shortcut-only artifact / inconclusive** — with evidence.

### P4 — T8 death / reset path
Trigger player death (or run reset) if reachable at runtime. Verify on return to MainMenu/CharacterSelect/fresh run: no leaked draft UI, no stale `RewardPickup`, `SkillDatabase` still usable, fresh opening draft appears. If death isn't reachable without code/asset changes, mark BLOCKED.

### P5 — T9 singleton + class smoke
Confirm the `_shuttingDown` root cause: after a scene reload, is `DraftManager.Instance==null` while a `DraftManager` component exists in scene? Then run class smoke for **Elementalist, Ranger, Shadowblade**: CharacterSelect → Arena → does opening draft appear and a first skill select work? (Use runtime workaround to unblock if needed — label it.) Document which classes are broken purely by T9 vs other causes.

### P6 — Evidence pack + report
Screenshots via `manage_camera` (`include_image=false` for file evidence). Frame-step deterministically:
```csharp
UnityEditor.EditorApplication.isPaused = true;
for (int i = 0; i < 60; i++) UnityEditor.EditorApplication.Step();
UnityEditor.EditorApplication.isPaused = false;
return UnityEngine.Time.frameCount.ToString();
```
(Shell sleep does NOT advance Unity frames in this MCP setup.)

---

## OUTPUT (E1 — write to file, return short)
Write the full report to: `STAGING/_process/2026-06/PLAYTEST_REPORT_AXOPUS_2026-06-19.md`
Per phase report: **PASS / FAIL / BLOCKED**, exact scene + frame count, screenshot path, console error/warning count, the runtime state values that prove each claim, and any shortcut used (name it, e.g. `onClick.Invoke()` instead of physical click).
Your final returned message = **≤10 lines**: per-phase verdict line + the report path + the single most important finding (esp. P3 T7 verdict). Do NOT inline the full report into the return.
End with explicit confirmation: "No git operations performed, no assets/scenes/prefabs saved, working tree unchanged except runtime-only state."
