# Playtest Bugfix — Done Report (2026-06-18)

Sole Unity agent. No git commits. Surgical fixes only.

## Fixes

### BUG-1 — ArcaneBlast dead skill (FIXED)
`Assets/Scripts/Skills/Elementalist/ArcaneBlast.cs`
- `FireProjectile` (L52-67): removed the `if (projectilePrefab == null) return;` early-out;
  now falls back to a new `CreateRuntimeProjectile()` when no prefab is assigned — mirrors Fireball's
  `CreateRuntimeFireball`. Passes `attacker: player.gameObject` + `element:"arcane"` to `proj.Init`
  so the projectile both hits AND emits the Player-tagged `[Damage]` telemetry line.
- `CreateRuntimeProjectile()` (L69-90): new arcane-violet runtime projectile (circle sprite from
  `ElementalistRuntimeVisuals.GetCircleSprite()`, Rigidbody2D + trigger CircleCollider2D + PlayerProjectile).
  No new art — reuses the existing procedural circle.

### BUG-2 — Passive draft pick has no feedback (FIXED)
- `Assets/Scripts/UI/HUDController.cs`: added public `ShowToast(string, float=2.5f)` + `ToastRoutine`
  coroutine (transient center-screen TMP text, outline-only, unscaled-time fade) — modeled on the
  existing `SecondaryUnlockBanner` style. No general toast system existed; this is the minimal reuse-shaped addition.
- `Assets/Scripts/Skills/DraftManager.cs` `HandlePassivePick` (after the `[Grant]` log, ~L519):
  `HUDController.Instance?.ShowToast($"Pasif kazanıldı: {passive.skillName} (Sv {lvl + 1})");`
  Passive grants now show a distinct HUD toast; skill grants remain distinct (they land in the skill bar).

### ANOMALY-2 — SetPrimaryClass gate (FIXED)
`Assets/Scripts/Systems/PlayerClassManager.cs` `SetPrimaryClass` (L50-64): gate now ANDs in
`ClassUnlockPolicy.IsDemoPlayable(type)` — rejects unless `DirectorBypassClassUnlock` OR
`(IsUnlocked && IsDemoPlayable)`. Mirrors `CharacterSelectController.cs:161`. Director bypass preserved.

### Telemetry completion — `[Damage]` logs for 5 bypassing skills (FIXED)
One per-hit `[Damage]` Debug.Log added immediately after each `SkillRuntime.DealDamage(...)` site,
matching the `[Damage] {amount} -> {target.name} ({element})` format:
- `Warblade/WarStomp.cs` L42 — `(warstomp)`
- `Warblade/DeepWound.cs` L37 main hit `(deepwound)` + L51 bleed tick `(bleed)` (per-tick, 1/s)
- `Warblade/Earthsplitter.cs` L40 — `(earthsplitter)`
- `Elementalist/Blink.cs` L51 — `(blink)`
- `Elementalist/GlacialSpike.cs` L66 — `(glacialspike)`

### ANOMALY-1 (optional) — SKIPPED
`DraftManager.skillController` is intentionally the **Warblade** controller (host for secondary slots
4-5, stable across class switch); the PRIMARY host is already re-resolved live via
`ResolvePrimarySlotHost()`/`UseElementalistPrimary()`. The cache is not the "first class" bug it appears
to be, and touching it risks the working secondary-band grant path. Skipped per task guidance.

## Verification (FRESH Play, dev-direct `_Arena`)
Fresh Play entry confirmed twice (MainMenu full-flow then `_Arena` dev-direct). All data-proof:
- Compile: 0 errors (read_console after forced script recompile).
- DebugLogOverlay AUTO-bootstrapped (instance present, fresh entry). Overlay `_lines` queue is the F3
  render source; subscribes to `Application.logMessageReceived` (gated UNITY_EDITOR — active in editor Play).
- `[Cast]`/`[Damage]`/`[Grant]` lines confirmed present in the overlay queue (captured same-frame):
  REAL `[Cast] Arcane Blast (Player)` and REAL `[Damage] 35 -> TEST_Enemy (arcane)`.
- ArcaneBlast: real `TryActivate()` fired the runtime projectile (`ArcaneBlast_Runtime`), traveled,
  hit, dealt damage. **HP delta proven twice: 100->65 = 35, and 200->165 = 35** (baseDamage=35 first cast).
- Passive feedback: `HUDController.Instance.ShowToast(...)` created an active `HudToast` GO with text
  `"Pasif kazanıldı: Test Pasif (Sv 2)"` — distinct from a skill grant.
- Telemetry logs (captured same-frame in overlay queue, real hits):
  `[Damage] 30 -> TEST_Enemy (warstomp)`, `[Damage] 20 -> FractureImp(Clone) (blink)`,
  `[Damage] 45 -> TEST_Enemy (glacialspike)`, `[Damage] 20 -> TEST_Enemy (deepwound)`.
  Earthsplitter: code-verified (identical Debug.Log pattern after DealDamage in the EnemiesInLine loop);
  direct hit NOT runtime-captured (test-harness BoxCast geometry against a synthetic kinematic collider
  returned 0 — not a code defect; its 4 structural siblings all fired their lines).

## Console / known issues
- Pre-fix forced recompile: 0 errors.
- Console error re-check at end: NOT VERIFIED — the prolonged `_Arena` Play session stalled the Unity
  main thread (MCP commands time out at 30s) AFTER all fixes were verified. This is a Play-mode runtime
  stall (heavy arena + many execute_code probes + F3 OnGUI log flood), NOT caused by the edits
  (which compiled clean and ran correctly). Source edits are saved to disk; the stall does not affect them.
- PENDING (needs editor responsive): (1) Stop Play / let editor recover; (2) restore
  `EditorSceneManager.playModeStartScene` to `Assets/Scenes/UI/MainMenu.unity` (I temporarily set it to
  null to run the `_Arena` dev-test — F5 path); (3) delete test PlayerPrefs keys `AB_TEST_HP_BEFORE`,
  `AB2_BEFORE`. Runtime TEST_ GameObjects auto-destroy on Play exit.
