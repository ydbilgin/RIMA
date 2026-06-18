# Instrumented Playtest — Warblade + Elementalist (2026-06-18)

Scene: `_Arena` (Warblade pre-placed; switched to Elementalist via `PlayerClassManager.SetPrimaryClass`).
Driving method: **programmatic** via `execute_code` (skill `TryActivate`/`ForceReady`, melee behavior `OnLMBInput`+`OnUpdate`, projectile `OnTriggerEnter2D` reflection, `DraftManager.OnOfferSelected` reflection). Dummy enemy `PT_Dummy` (Health 5000, Enemy layer) spawned in melee/projectile range.

## ⚠️ Environment / methodology caveats (NOT game bugs)
1. **`read_console` (MCP) returns 0 entries for runtime logs** in this session. Verified the instrumentation fires by hooking `Application.logMessageReceived` IN-PROCESS inside each `execute_code` run — that captured every `[Cast]`/`[Damage]`/`[Grant]` line. All log evidence below is from in-process capture.
2. **Play-mode frames do NOT advance between MCP calls** (`Time.frameCount` frozen at 39907, `Time.time` frozen at 62.95, editor unfocused). Consequence: projectiles don't travel and `Destroy(obj, life)` never fires. Projectile-skill damage was therefore proven by manually invoking the spawned projectile's `OnTriggerEnter2D(dummyCollider)`; coroutine/DoT/landing-delay effects could not be time-stepped.
3. **The running play session predates the "just added" instrumentation code** (long-running, frame 39907). So `DebugLogOverlay`'s `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` auto-bootstrap had not fired → no live overlay instance. I created an instance manually to verify its capture + F3 toggle (see Scenario 7). A fresh Play-mode entry is required for F3 to auto-work.

## Scenario Matrix

| Class | Scenario | PASS/ANOMALY/BUG | Evidence (log / HP delta) | Note |
|---|---|---|---|---|
| Warblade | 1. 8-dir facing | PASS | 8 distinct real sprites: idle_east/NE/north/NW/west/SW/south/SE, all tex=True | Animator=Warblade; no fallback |
| Warblade | 2. Basic attack (LMB) | PASS | `[Damage] 30 -> PT_Dummy (Physical)`; HPd 28/30/33 over 3 swings | MeleeChain defers hit by attackStartup (OnUpdate resolves) |
| Warblade | 3. Skills (full kit) | PASS | `[Cast]` for all 13; dmg: WarStomp30 Cleave22 DeepWound20 Earthsplitter34 CripplingBlow45 DeathBlow240 GravityCleave55; utility: SunderMark/IronCrush/BattleSurge/IroncladMomentum/IronCounter/IronCharge HPd0 | No exceptions on any cast |
| Warblade | 3a. Rage-gated skill | PASS (expected) | Cleave `act=False` at 0 rage → no `[Cast]`; with full rage `act=True [Damage]22` | Cost gate working, not a bug |
| Warblade | 4. VFX | PASS | MeleeArc/SkillVfx objects spawn on swing | |
| Warblade | 7. F3 overlay | PASS* | overlay captures `[Cast]/[Damage]/[Grant]`, F3 toggles `_visible`→OnGUI | *verified on manual instance; see caveat 3 |
| Elementalist | 0. Class apply | PASS | WB disabled, EL enabled, Mana 100/100, Animator=Elementalist, Body=elementalist_idle_south tex=True, 0 warnings | |
| Elementalist | 1. 8-dir facing (JUST FIXED) | **PASS** | 8 DISTINCT real sprites: idle_east/NE/north/NW/west/SW/south/SE, all tex=True | Fix CONFIRMED — not fallback (fallback = south×8) |
| Elementalist | 2. Basic attack (RiftBolt LMB) | PASS | spawns moving RiftBolt; manual-trigger `[Damage] 22 -> PT_Dummy (Ability)` | Projectile travel needs frames (caveat 2) |
| Elementalist | 3. Skills (15 cast) | PASS | every `[Cast]` fired, ZERO exceptions; immediate dmg GlacialSpike45/ChainLightning40/SolarFlare46/PrismBeam78/Blink20 | Fireball manual-trigger HPd30 |
| Elementalist | 3b. ArcaneBlast | **BUG** | `[Cast]` fires, spends 20 mana, spawns ONLY cast-flash, **no projectile, HPd0** | projectilePrefab=NULL + no runtime fallback → dead offensive skill |
| Elementalist | 3c. FrozenOrb | PASS (expected) | spawns FrozenOrb_Runtime; applies Chill/Frozen status, HPd0 by design | utility/control, not direct damage |
| Elementalist | 4. Skill VFX (#9a) | PASS | non-Fireball skills spawn VFX: Fireball +SR+Trail, FrozenOrb +SR+Trail, GlacialSpike +3SR, ChainLightning/SolarFlare/PrismBeam/FrostWall +SR | #9a working |
| Both | 5. Reward→Draft→grant (SKILL) | PASS | `[Grant] Fireball -> slot 0`; lands in EL slot 0, skill bar polls slots → shows | |
| Both | 5b. Reward→Draft→grant (PASSIVE) | PASS (mechanics) | `[Grant] Ironclade Momentum -> passive Lv 1`; PassiveBase attached, NOT in any slot | mechanically correct |
| Both | 5c. Passive player-facing feedback | **BUG (UX)** | passive pick → only generic DraftSelect SFX (== skill pick) + panel closes; NO HUD entry, NO toast/banner/text | matches user complaint exactly |
| Both | 6. Class-select gate | PASS | IsDemoPlayable true ONLY Warblade+Elementalist; `SetPrimaryClass(Shadowblade/Ranger)` → `Locked primary class rejected`, class unchanged; Warblade accepted | hard gate holds |

## Prioritized BUG / ANOMALY list

### BUG-1 (MEDIUM) — Elementalist `ArcaneBlast` is a non-functional offensive skill
- **Repro:** Primary=Elementalist → cast ArcaneBlast (`skill.TryActivate()`).
- **Observed:** `[Cast] Arcane Blast (Player)`, mana spent 20, but `ArcaneBlast.projectilePrefab == NULL` and `FireProjectile()`/`FireBarrage()` early-return on null prefab (`if (projectilePrefab == null) return;`). Result: only the `SkillVfx.CastFlash` glow spawns; **no projectile, no `[Damage]`, HP delta 0**. The 4th-cast "Barrage" path is also dead (same null guard).
- **Evidence:** `ArcaneBlast: manaSpent=20 GOdelta=1` (the 1 GO = cast-flash only); `+SR=0 +Trail=0 HPd=0`.
- **Root cause:** unwired `projectilePrefab` reference + (unlike `Fireball`) no `CreateRuntime*` fallback in code.
- **Fix options (orchestrator):** wire a projectile prefab on the ArcaneBlast component/prefab, OR add a runtime-projectile fallback mirroring `Fireball.CreateRuntimeFireball`. File: `Assets/Scripts/Skills/Elementalist/ArcaneBlast.cs:52-63`.

### BUG-2 (MEDIUM, UX) — Passive draft pick has no player-facing feedback ("don't know if skill or passive")
- **Repro:** Trigger a draft, pick a PASSIVE card.
- **Observed:** `[Grant] <name> -> passive Lv N` logged and `PassiveBase` applied, but the player sees only the **same `DraftSelect` SFX as a skill pick** and the panel closes. The passive does NOT appear in the skill bar (correct by design), and there is **no toast/banner/HUD level-up text** anywhere. A skill pick visibly fills a slot; a passive pick is visually indistinguishable from "nothing happened."
- **Evidence:** `HandlePassivePick` → `FinishPick` (`offerUI.Hide()` + `AudioManager.Play(DraftSelect)` + `OnSkillPicked.Invoke`). `OnSkillPicked` has exactly one subscriber (`MapFragmentBridge`, run-flow, not UI). `SkillBarUI` only renders active slots. Verified: passive `in skill slot? False`, `passive Lv now=1`, `PassiveBase attached? True`.
- **Fix options (orchestrator):** add a short on-pick passive confirmation (floating "PASİF: <name> Lv N" text / HUD passive-tray pip / distinct SFX). Files: `DraftManager.cs:499-520` (HandlePassivePick/FinishPick), a HUD feedback hook.

### ANOMALY-1 (LOW) — `DraftManager.skillController` caches Warblade even when primary is Elementalist
- **Observed:** After switching primary to Elementalist, `DraftManager.skillController` field still resolved to `Warblade_SkillController`. The skill grant itself was routed CORRECTLY to the Elementalist controller (slots 0-3 use `ResolvePrimarySlotHost()`, which picks the live primary host), so no wrong grant occurred. The cached field is only an internal inconsistency / latent risk if any code path reads `skillController` directly for slot 0-3 routing.
- **Note:** Likely also a session artifact (class was switched mid-play). Worth a glance but low priority.

### ANOMALY-2 (LOW, defense-in-depth) — `PlayerClassManager.SetPrimaryClass` gates on `IsUnlocked` only, not `IsDemoPlayable`
- **Observed:** Several non-demo classes report `IsUnlocked=true` (Ravager/Gunslinger/Brawler/Summoner/Hexer — from prior-session echo-unlock PlayerPrefs). `SetPrimaryClass` rejects only `!IsUnlocked`, so it would NOT block a non-demo-playable-but-unlocked class. The **hard demo gate is enforced upstream** by the chamber (`IsDemoSelectable` = `IsDemoPlayable` blocks confirm/StartRun/AttuneRoutine), so in normal flow these classes still cannot start a run. But `SetPrimaryClass` itself lacks the `IsDemoPlayable` guard — a defense-in-depth gap if any other caller invokes it directly.
- **Evidence:** policy truth-table above; `SetPrimaryClass(Shadowblade/Ranger)` rejected because they are also `!IsUnlocked`.

### INSTRUMENTATION NOTE (not a bug) — some skills deal damage without a `[Damage]` log
- WarStomp/DeepWound/Earthsplitter/Blink/GlacialSpike show HP deltas but no `[Damage]` line, because the `[Damage]` tag lives only in `SkillRuntime.DealDamage`/`DealDamageRaw` (SkillRuntime.cs:146/197). Skills that call `Health.TakeDamage` directly (or apply DoT) bypass the tag. Damage-via-HP-delta is still proven; this is just `[Damage]`-tag coverage, useful if the orchestrator wants full telemetry parity.

## Cleanup / exit
- Destroyed 51 test/leaked runtime objects (PT_Dummy, PT_Marker, SkillDatabase_PT, leaked RiftBolt/Fireball/FrozenOrb runtimes + SkillVfx instances — leaked only because frames weren't ticking to run `Destroy(,life)`).
- Exited Play mode cleanly. `_Arena` reverted to 12 roots, `isDirty=false` — **no scene leak**. Final console: 0 errors.
- Screenshots: `STAGING/_process/2026-06/playtest_shots/wb_arena.png`, `elem_facing_se_skillbar.png` (Elementalist real SE sprite + populated skill bar + HP bar).
