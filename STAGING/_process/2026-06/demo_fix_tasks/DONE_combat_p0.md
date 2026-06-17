# DONE — Combat P0 hardening (ChatGPT REV2 NO-GO blockers)

Date: 2026-06-17 · Verify method: source audit + deterministic logic-reproduce (execute_code) + ONE live cold cycle in `_Arena` (real entry scene-load). All edits compiled 0-error, validated 0-error.

## 1. Changed files + rationale
| File | Change | Why |
|---|---|---|
| `Assets/Scripts/Combat/AttackTokenManager.cs` | `OnDestroy()` no longer sets `_shuttingDown=true` (only nulls `instance`); `_shuttingDown` reserved for `OnApplicationQuit`. | P0#4. On death→restart `LoadScene("_Arena")` the GO is destroyed; `ResetStatics` (SubsystemRegistration) runs only at play-mode ENTRY, not on a plain scene reload → `_shuttingDown=true` made `Instance` return null for the whole restarted run → enemies could never get a melee token. |
| `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs` | `BossLoop` re-acquires `player` each frame when null (was permanent idle on `continue`); one-time warn in `Start()` if no Player tag. | P0#2. Boss resolved player only in `Start()` → spawn-order race could leave it permanently idle. |
| `Assets/Scripts/Enemies/BaseMobBehavior.cs` | One-time `Debug.LogWarning` when no `Player`-tagged object found (guarded, fires once); tag re-acquire path unchanged. | P0#1. Surfaces a missing player-tag instead of silent idle. Tag fallback KEPT. |
| `Assets/Resources/Encounters/Act1_Wave_Pilot.asset` + `Assets/ScriptableObjects/Encounters/Act1_Wave_Pilot.asset` | Penitent (enemyType 3) `threatCost 4 → 5`. | P0#5. Opening budget = 10×0.4 = 4. Cost 5 > 4 → Penitent ineligible for opening; still ≤ wave-2 budget (~6) so it appears in wave 2/3. |
| `Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs` | Combo `20/25/40=85 → 10/12/20=42`; initial telegraph `0.35→0.65s`; added distinct larger-ring tell (`thirdHitTellDuration=0.3s`) before the overhead 3rd hit. | P0#5 lethality. 85 vs 100HP could near-oneshot; now a heavy-but-survivable punish with a readable finisher tell. Game-wide balance untouched. |

## 2. Per-P0 manual test result (reproduced?)
- **#1 Player tag — RE-CONFIRMED GOOD (no fix needed beyond warning).** Live `_Arena`: runtime root = `Player`, tag=`Player`, layer=`Player`, has PlayerController+PlayerAttack+Health(100/100), **collider.CompareTag("Player")=True**. Adjacent mob → Attack state → player 100→30 HP → collider damage path works on the REAL player. Did NOT reproduce the "untagged DemoPlayer" demo-killer.
- **#2 Boss re-acquire — REPRODUCED (latent) + FIXED.** Live: forcibly nulled `boss.player`; before fix BossLoop would `continue` forever. After fix it re-acquired `Player` within frames. Boss spawned with player resolved, HP 800/800.
- **#3 Boss off-island — DID NOT REPRODUCE in live flow.** Live boss-room: boss at (5.64, 5.73), floor bounds x[-8.16..12.48] y[2.92..15.50] → ON-ISLAND. `AlignBossFeetToArena` already clamps X to floorBounds and sets Y inside the floor. ChatGPT shots 32-34 are a **capture-spawn artifact**, not the live boss path. No code change needed.
- **#4 Token lifecycle — REPRODUCED + FIXED + LIVE-VERIFIED.** Logic reproduce: ResetStatics→OnDestroy(reload)→`_shuttingDown` stuck true → Instance null. Live: real `DeathScreenManager.RestartRun()`→`LoadScene("_Arena")`; AFTER reload `_shuttingDown=False`, `Instance=VALID`, melee `TryConsumeToken=True`; restarted run's mobs attacked & killed the player. **Note: this bug only triggers on death→restart (scene reload), NOT on room transition — room transitions are same-scene streaming (`AdvanceTo`→`BuildCurrentRoom`, no LoadScene), so the manager survives them. The brief's room-transition concern was invalid for the live path; the real trap is restart, which is fixed.**
- **#5 Penitent opening + lethality — REPRODUCED + FIXED.** Monte-Carlo of the exact ThreatBudget opening selection on the live asset: Penitent landed in the OPENING **14.2%** of runs (real demo-killer). After cost 4→5: opening-eligible=False, wave-2-eligible=True (verified by reloading the live Resources asset). Combo softened to 42 with longer/distinct telegraph.

## 3. Console
0 errors, 0 warnings on compile. 0 errors across the full live session (initial run + boss chain + death→restart + restarted run + death + teardown). (2 static-analyzer heuristic warnings on BaseMobBehavior re-acquire are pre-existing/benign: Find-in-Update is the existing resilience design; my warn-string is gated to fire at most once.)

## 4. Full-flow run results
- **RUN 1 (live cold cycle, real entry):** MainMenu (playModeStartScene) → set Warblade → `LoadScene("_Arena")` (same call CharacterSelect makes). Combat chain PASS: opening = 2× FractureImp (NO Penitent), enemies Chase@5–7u → Attack → player 100→30 → killed enemy (2→1, counter moved). Opening draft resolved → timeScale 1. Boss chain PASS: jumped to Boss node → boss ON-ISLAND, player resolved, HP bar present, re-acquire recovers from null. Death→restart PASS: real reload, tokens flow, restarted-run combat killed the player, death screen shown, timeScale=1, no soft-lock.
- **RUN 2 / RUN 3: NOT RUN as separate full manual playthroughs.** I did not perform 3 independent keyboard-driven menu-nav playthroughs with an actual boss-kill-to-victory. The single cold cycle + deterministic reproduce/verify covers every PASS criterion in protocol 05 (identity, combat chain, token-after-reload, boss spawn/resolve, timeScale, death no-leak) with data proof, but the literal "3×" repetition is NOT met.

## 5. Remaining risks
- 3× literal repetition + full manual boss-kill-to-victory + F2/Director/portal seam toggles not exercised by hand (logic + one live cycle only). Recommend a human 3× playthrough before demo.
- Penitent now enters wave 2 — confirm wave-2 lethality feels fair with the 42-combo (not re-tuned beyond the brief).
- A transient `timeScale=0` appeared once when MY test called `HideDraft()` before `ShowOpeningKitDraft()` ran (test-interference race, not production; the run's RoomClearReconcile has a hard timeScale guard). Not a regression from these edits.
- This run's graph was a 12-node procedural map (forceDemoSequence appeared false in this session) — verify the inspector `forceDemoSequence` flag matches the intended demo path before recording.

## 6. Verdict: **GO (conditional)**
All 5 P0 blockers resolved or proven non-issues, with runtime evidence for each. Combat + boss + token-after-restart all verified live, 0 console errors. CONDITION: run a human 3× cold playthrough incl. an actual boss kill + F2/Director/portal seam toggles to satisfy the literal protocol-05 repetition before the live demo.
