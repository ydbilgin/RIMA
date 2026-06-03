# RIMA — Clean Structure + Phased Roadmap (S6, Opus synthesis)

> 4-source synthesis: **NLM canon** (direction) + **ax** (vision/risk) + **cx** (feasibility/critical-path) +
> **Sonnet 9-subsystem audit** (93 findings, 10 blockers). Opus decides. This is the **single roadmap**; additions
> layer on top of this base order. Direction lock = `RIMA_DIRECTION_LOCK_S6.md`.

---

## PART A — THE CLEAN STRUCTURE (resolve every competing system → ONE of each)
The #1 risk (all sources): the demo is split across overlapping systems; the team can polish a path the live slice
doesn't run. Scene audit confirms the LIVE spine = `Systems.Map.RoomLoader`. So we standardize on it and retire the rest.

| Concern | ✅ KEEP (the ONE) | ❌ RETIRE / MERGE | Care |
|---|---|---|---|
| Room flow | `Systems.Map.RoomLoader` (+ `RoomSequenceData[]`) | `RuntimeRoomManager`, `Map/Runtime/RoomLoader` (static) | ⚠️ boss-death routes THROUGH `RuntimeRoomManager.NotifyBossDefeated` — **rewire to RoomLoader BEFORE retiring** |
| Map fragment | `RIMA.Environment.MapFragment` | `RIMA.MapFragment` (`Core/MapFragment.cs`) | split event buses; consolidate to Environment |
| Settings menu | `SettingsMenuUI` (+ `UIManager`) | `Core/SettingsMenu.cs` | duplicate ESC + timeScale writes |
| Camera follow | the scene-live one (+ make it read `ScreenShakeDriver`) | the other CameraFollow | shake only lands on the one reading the driver |
| Camera shake | `Combat/Juice/ScreenShakeDriver` (offset-only) | `VFX/ScreenShake` (rotates cam = breaks pixel-perfect!), `Core/CameraShake` | delete rotation-writer |
| Boss script | the prefab-wired one (confirm) | the other `PenitentSovereign` twin | 684-line vs 297-line; keep prefab's, delete twin |
| Enemy spawn authority | `EncounterController` | `RuntimeRoomManager.SpawnEnemies` (dies with RRM) | split `aliveEnemies` counters → OnRoomCleared may never fire |
| Dead orphans | — | `SkillDraftSystem.cs`, `Combat/Skills/SkillData.cs` | namespace collision risk; delete |
| Duplicate Systems GO | active one | inactive duplicate (HitStop+CameraShake) | scene delete |

**Result:** one room spine, one fragment, one settings owner, one camera-shake (offset-only), one boss, one spawn authority.

---

## PART B — THE PHASED ROADMAP (base order; do in sequence)

### Phase 0 — CONSOLIDATE + UNBLOCK (make the loop actually play E2E) ⬅️ START HERE
*Goal: one clean spine + the demo runs start→finish. This IS the "temizlik + birleştir".*
- **Code consolidation** (Part A): rewire boss-death → `RoomLoader.RaiseDemoComplete`, then [Obsolete]/retire RRM +
  static RoomLoader; consolidate MapFragment to Environment; delete dead orphans; delete rotation camera-shake.
- **The 10 blockers** (B1-B10 below) → demo plays: spawn → kill wave → fragment drops → pickup (cyan beam) → draft →
  gate unlock → fade → … → boss 50% → DemoComplete + Wishlist CTA.
- **Exit gate:** ONE PlayMode smoke test proving the full loop end-to-end.

### Phase 1 — COMBAT FEEL LOCK (graybox-first, canon pivot — BEFORE art)
*Goal: freeze the 30-sec loop feel. Art production stays HALTED until this is frozen.*
- Tune `BasicAttackProfile` timing (startup/commit/cancel), verify `MeleeChain` deferred strike, attack/dash buffer.
- Juice tune: hitstop 0.04/0.07/0.12/0.18 tiers, directional shake, white flash, damage numbers. **Fire the finisher
  CommitBeat** (`CombatHandler.OnCommitBeat` → `CombatEventBus.PublishCommitBeat`). Wire `VFXRouter.entries`.
- **F5 playtest = the real feel gate** (your call).

### Phase 2 — CONTROLS / HUD CONSISTENCY
*Goal: no input/UI lies.* (per `CONTROL_SCHEME_SYNTHESIS_S6.md`)
- Expand `KeyBindManager` → registry + persistence + `RebuildBindings()`; repoint controllers; rebind actually works.
- Fix 3 bugs: SkillBar labels binding-driven (Q/E/R/F not 1-5); unify to one `SettingsMenuUI`; no fake slots.
- Interact key: route the 4 hardcoded `Key.G` pickups through one rebindable action.

### Phase 3 — CONVERSION (the wishlist payoff)
- Replace `/app/0/` Steam URL (Death + Victory + BuildSeed). Victory `timeScale 0.2→0`. Run-stats + next-class teaser.
- T4 test: boss kill → DemoComplete → CTA active.

### Phase 4 — ART INTEGRATION (only after Phase 1 frozen)
- Swap placeholders → real (imagegen menu/hero already done; Groups B/C next). **Slash-arc = painterly flipbook**
  (canon) replacing the LineRenderer visual; hide weapon during swing. Boss sprite. Skill icons (19 exist, wire to
  `SkillIconRegistry`). Player layer 10 fix.

### Phase 5 — AUDIO
- Move 21 wavs `Assets/Audio/SFX/` → `Resources/Audio/` enum-named; extend `Sfx` enum (16 missing); wire BossIntro/
  Cast/RoomClear/RageFull hooks. (Music deferred.)

### Phase 6 — READABILITY + FINAL POLISH (ax's #1 risk)
- **Color budget:** cyan #00FFCC EXCLUSIVELY for player actions / active rifts / critical telegraphs; enemies+bg muted.
- **Juice scaling:** big shake/hitstop on finishers/rifts only; keep basic strikes readable. Final QC + E2E.

---

## PART C — THE 10 DEMO BLOCKERS (Phase 0 work; CODE vs SCENE)
| # | Blocker | Type | Owner |
|---|---|---|---|
| B1 | DraftManager.offerUI/offerGenerator null in scene → draft never opens | SCENE | Unity |
| B2 | MapFragmentSpawner.fragmentPrefab null → no fragment | SCENE | Unity |
| B3 | Combat rooms skip fragment drop (`RoomLoader:285` gates on isRewardRoom) | CODE | Opus |
| B4 | Two MapFragment classes, split event buses | CODE | Opus |
| B5 | Boss phase 0.33→0.5 + delete boss twin | CODE | Opus |
| B6 | Boss→Victory route via RRM (no E2E test) | CODE | Opus |
| B7 | `_IsoGame` scene missing → kills test suites (BootstrapContract) | CODE | Opus |
| B8 | 21 SFX wavs unreachable (wrong path) | CODE+ASSET | Opus |
| B9 | Draft blocked by `DraftDrivenByRoomLoader` flag + Gate-in-room req | CODE+SCENE | Opus+Unity |
| B10 | Duplicate inactive Systems GO | SCENE | Unity |

**CODE blockers (Opus can fix now, dotnet-build-verifiable):** B3, B4, B5, B6, B7, B8. **SCENE blockers (need Unity
Editor):** B1, B2, B9(part), B10. → I execute the CODE half now (writer Opus → cx/agy review → build); the SCENE half
is a guided Unity pass (UnityMCP when stable, or you).

---

## Execution order for the cleanup (safe → risky)
1. **Truly-dead deletes** (zero refs): `SkillDraftSystem.cs`, `Combat/Skills/SkillData.cs`, rotation `VFX/ScreenShake.cs`.
2. **One-liners:** boss threshold 0.33→0.5, Steam URL, victory timeScale, `[Obsolete]` on static RoomLoader.
3. **Rewire-then-retire:** boss-death → RoomLoader.RaiseDemoComplete (B6) → then retire `RuntimeRoomManager`.
4. **MapFragment consolidation** (B4) → Environment only.
5. **Fragment-in-combat-room** (B3) + draft flag (B9 code half).
6. **Test fixes** (B7): BootstrapContract scene name + asmdef ref.
Each step: Opus writes → `dotnet build RIMA.Runtime` → cx/agy review → next. SCENE half batched for a Unity pass.
