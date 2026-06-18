**VERDICT: PASS-WITH-FIXES**

Static audit of the playtest bug-fix batch. READ-ONLY (no files modified, no git writes). The builder's
fresh-Play verification is accepted as-is; this review is static + scope-diff against the report.

## Summary
- ANOMALY-2 Director gate: SAFE — DirectorMode bypass is real and robust (NOT a P0). Confirmed below.
- BUG-1 ArcaneBlast: correct, matches Fireball pattern, no null-deref.
- BUG-2 passive toast: correct, distinct, no leak, skill path not regressed.
- Telemetry: format correct BUT 4 of the 6 sites DOUBLE-LOG (P1) — contradicts the report's "bypass" claim.
- SCOPE: large undocumented scope expansion (P1) — a full VFX/instrumentation pass + a multi-file
  "DEMO SAFETY" hardening pass landed alongside the 5 reported fixes. None appears broken, but the report
  under-documents what shipped.

| Sev | File:Line | Issue | Fix |
|-----|-----------|-------|-----|
| P1 | WarStomp.cs:43, Blink.cs:52, GlacialSpike.cs:67, DeepWound.cs:53 | DOUBLE-LOG. These call `SkillRuntime.DealDamage(hp, dmg, this)` (3-arg). Skill components live on the Player root (controllers added via `player.AddComponent<...SkillController>()`; skills via `host.gameObject.AddComponent`). Player root prefab is tagged `Player` (Player.prefab:182). So `attacker.CompareTag("Player")` is TRUE → SkillRuntime.cs:197 ALSO emits `[Damage] {n} -> {name} (skill)`. Result: every hit logs twice — the descriptive manual tag PLUS a generic `(skill)` line. Report claims these "bypass the Player-tagged DealDamage log" — FALSE for the 3-arg path. | Either (a) drop the manual log in these 4 sites and instead route the descriptive element through `DealDamage(hp, dmg, popup:false, this.gameObject, dir, element:"warstomp")` so SkillRuntime's single log carries the right tag, or (b) keep manual logs and pass `attacker:null` so SkillRuntime stays silent. (DeepWound.cs:38 main-hit and Earthsplitter.cs:41 use the 2-arg null-attacker form → single log, CORRECT — leave them.) |
| P1 | (scope) CharacterSelectScreen.cs (+43), CharacterSelectController.cs:158, ChamberSelectBootstrap.cs:294/1974, Loc.cs:112/294, ClassUnlockPolicy.cs:9-18, all 14 Elementalist skills (VFX), SkillBase.cs:85 `[Cast]`, SkillRuntime.cs:145/196 `[Damage]` | UNDOCUMENTED SCOPE. The report describes 5 fixes touching ~8 files; the diff touches 34 scripts. Two extra workstreams shipped silently: (1) a "DEMO SAFETY / IN DEVELOPMENT" select-screen hardening pass routed through new `ClassUnlockPolicy.IsDemoPlayable`; (2) a broad SkillVfx CastFlash/ImpactBurst/ChainBolt pass across Blizzard/ChainLightning/Combustion/FrostWall/FrozenOrb/LivingBomb/Meteor/MirrorImage/PrismBeam/SolarFlare/ArcaneSurge + the `[Cast]`/`[Damage]` instrumentation in SkillBase/SkillRuntime. Each change individually looks sound (APIs exist: CastFlash/ImpactBurst/ProjectileTrail/ChainBolt all in SkillVfx.cs; gate logic consistent), but this violates the "surgical, only expected files" constraint and the report's own "Surgical fixes only" header. | Document these two passes (likely from sibling _done_elementalist_vfx / _done_playtest_instrumented reports) so the reviewer/orchestrator knows they shipped; or split them into their own labeled commits. No code change required if intentional — this is a traceability defect, not a logic defect. |
| P2 | SkillRuntime.cs:197 | The auto-`[Damage]` element on the double-logged 3-arg path resolves to `element ?? packet.damageType` → "skill" (generic), so even after de-dup the descriptive tag must come from the explicit `element:` arg, not the manual log. Tie this off when fixing the P1 above. | Pass explicit `element:` in the 4 fixed call sites. |

## Detail — checks PASSED

1. BUG-1 ArcaneBlast (ArcaneBlast.cs:52-90): CORRECT. `FireProjectile` now falls back to
   `CreateRuntimeProjectile()` when `projectilePrefab==null` (early-out removed). Mirrors Fireball's
   `CreateRuntimeFireball` (Rigidbody2D gravity 0, trigger CircleCollider2D r=0.18, SpriteRenderer on "VFX"
   layer order 20, PlayerProjectile). `proj.Init(... attacker:player.gameObject, element:"arcane")` matches
   PlayerProjectile.Init signature (named args valid). `proj`/`player` null-guarded. Sprite reuses
   `ElementalistRuntimeVisuals.GetCircleSprite()` (procedural, self-caching, no new art). Damage uses the
   skill's escalating `dmg`. No null-deref. NOTE: because attacker=Player here, the projectile hit will emit
   the single SkillRuntime `[Damage] ... (arcane)` line — correct, no manual log added (good, no dup).

2. BUG-2 passive toast (HUDController.cs:514-561, DraftManager.cs:519): CORRECT. `ShowToast` is null/empty
   guarded, starts `ToastRoutine`. Routine creates its own `HudToast` GO parented to HUD, unscaled-time fade
   (works during the paused draft), null-checks tmp/go each frame, and `Destroy(go)` at end → NO LEAK, no
   reuse collision (one GO per toast). Center-screen violet TMP is visibly distinct from a skill-bar grant.
   `HUDController.Instance?.` is null-safe. Skill-grant path NOT regressed (the added `[Grant]` log at
   DraftManager.cs:752 is additive logging only).

3. ANOMALY-2 Director gate (PlayerClassManager.cs:53-58 + ClassUnlockPolicy.cs:9-18): SAFE — NOT a P0.
   Gate now rejects unless `DirectorBypassClassUnlock || (IsUnlocked && IsDemoPlayable)`. The `!DirectorBypass`
   term short-circuits FIRST. DirectorMode.cs:2113-2121 sets `DirectorBypassClassUnlock=true` in a try/finally
   around `SetPrimaryClass(type)`, so the Director can still set ANY class. (DirectorMode also independently
   pre-gates on `IsDirectorClassImplemented`.) Normal player selection IS still gated. Director-break CONFIRMED
   ABSENT.

4. Telemetry format: tag format matches existing `[Damage] {amount} -> {name} ({element})`
   (SkillRuntime.cs:146/197). All sites are per-hit (inside hit loops / single-target), DeepWound bleed is
   per-tick (1/s) as labeled. Format = PASS; duplication = the P1 above.

5. Surgical/locked systems: no GATE/Boss/reward-bleed/Build-core/weaponless-anim/branching-seed edits seen.
   No new abstractions beyond `IsDemoPlayable` (single small policy method, reasonable) and `ShowToast`
   (minimal). The concern is scope breadth (P1) + traceability, not locked-system breakage.

## Not verified (out of static scope)
- Runtime double-log was not re-observed (the builder's session stalled before end-of-run console re-check,
  per the report's "Console / known issues"). The double-log is inferred from code + prefab tag, high
  confidence. A 10s fresh Play casting WarStomp once will confirm: expect TWO `[Damage]` lines per hit.
- PENDING housekeeping from the report still open: restore `playModeStartScene` to MainMenu.unity; delete
  test PlayerPrefs `AB_TEST_HP_BEFORE`/`AB2_BEFORE`. Confirm before any demo capture.
