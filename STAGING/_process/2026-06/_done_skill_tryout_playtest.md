# Skill Try-Out Playtest — Log-Instrumented (Elementalist) — 2026-06-18

Method: Play `_Arena`, Director-bypass → Elementalist primary, kit skills bound via production `DraftManager.TryDirectorAssignSkill`, casts driven via `SkillBase.TryActivate()` / `PlayerAttack.InvokeBasicAttackForTest()`. Damage observed on a controlled dummy (Health) + real enemies. Log lines captured via `Application.logMessageReceived` (same source the F3 overlay reads). Exited Play clean, scene not dirty, 0 console errors.

| Scenario | Result | Log evidence + HP delta |
|---|---|---|
| LMB basic attack (RiftBolt) | PASS | bolt vel=(15,0) east → hit dummy; HP 99813→99789, **delta 24**; projectile despawned on hit (DealDamage `basic_lmb`) |
| Q/slot0 Fireball (projectile) | PASS | fired projectile east, collided dummy; HP 99881→99848, **delta 33** (30 + burn tick) |
| E/slot1 Glacial Spike (instant line) | PASS | `[Cast] Glacial Spike (Player)` + `[Damage] 45 -> PLAYTEST_Dummy (skill)`, **delta 45** |
| R/slot2 Chain Lightning (instant) | PASS | `[Cast] Chain Lightning (Player)` + `[Damage] 40 -> PLAYTEST_Dummy (skill)`, **delta 40** |
| F/slot3 Arcane Blast (projectile, ex-"dead") | PASS | fired projectile, collided dummy; HP 99848→99813, **delta 35** = baseDamage; confirms ArcaneBlast no-op fix |
| Grant instrumentation | PASS | `[Grant] Meteor -> slot 3` on Director assign |
| Char-select skill try-out | PASS (shipped, no interactive preview) | Confirm gate `IsUnlocked && IsDemoPlayable` → Elementalist confirmable, locked classes block start; selected class's skills cast+hit in-run (rows above). NO in-menu try-out-cast feature exists — never shipped; not a regression. |

Notes: All 4 Q/E/R/F skills + LMB CAST and HIT the dummy with [Cast]/[Damage]/[Grant] logging live. Two test-harness conditions (NOT bugs): (1) game auto-paused `timeScale=0` blocked projectile physics until set to 1; (2) Elementalist default aim = TowardsMouse, so casts aimed at the editor cursor (SW) until AttackAimMode set to CharacterFacing. Both restored on exit (timeScale=1, AttackAimMode=TowardsMouse, DirectorBypass=false, dummy destroyed). Verified vs Elementalist; Warblade not separately driven (kit/flow identical via same SkillBase.TryActivate path).
