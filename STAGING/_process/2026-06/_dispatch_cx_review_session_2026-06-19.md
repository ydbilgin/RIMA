# Codex Review — 2026-06-19 session VFX/feel + fix changes (READ-ONLY)

ACTIVE RULES: (1) think before reviewing (2) flag only real issues, no nitpick spam (3) surgical scope — only the files listed (4) BLOCKED if unclear.

**READ-ONLY:** Do NOT modify any file. Do NOT `git add` / `git commit` / stage anything. This is a review — report findings only; the orchestrator triages fixes.

UNITY ERROR CHECK: not required (no Unity edits) — this is static code review. A Sonnet agent wrote these; you are the independent reviewer (writer != reviewer).

## What to review (correctness, regressions, Unity-API correctness, perf, cleanup)

### PRIMARY — new VFX/feel code (this session)
1. **`Assets/Scripts/VFX/SkillVfx.cs`** — new methods `ProjectileBlaze(GameObject, VfxElement)` + `ImpactExplosion(Vector3, VfxElement)`.
   - A REAL bug was fixed in ProjectileBlaze: `TrailRenderer.widthCurve` assignment RESETS `startWidth`/`endWidth` (Unity API), so the old `trail.startWidth = 0.10f` was silently overwritten to ~1.0 (curve value at t=0) → trail rendered 10× too wide. Fix uses `trail.widthMultiplier = 0.10f` set AFTER `widthCurve`. **Verify this is correct and that no other TrailRenderer/LineRenderer in the file has the same widthCurve-then-width ordering bug.**
   - Review: glow flicker coroutine lifetime, ember ParticleSystem config + cleanup (child dies with projectile?), additive material reuse, null-guards, per-projectile allocations.
2. **`Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs`** — orb scale (0.28/0.32), `ProjectileBlaze` + `ImpactExplosion` wiring in ExecuteBolt / SetOnHit. Verify element-awareness preserved (Fire/Frost/Light/Arcane), no double-VFX, no leaks.
3. **`Assets/Scripts/VFX/StatusEffectTint.cs`** (NEW) — tints enemy SpriteRenderers by active status (Chill=blue, Burning=red w/ DoT pulse, apply-flash). Review: per-frame allocations, original-color cache + smooth restore correctness, multi-SpriteRenderer handling, event subscribe/unsubscribe in OnDestroy, `LateUpdate` + `[DefaultExecutionOrder]` ordering vs mob behavior, behavior when no StatusEffectSystem present.
4. **`Assets/Scripts/Systems/StatusEffects/StatusEffectSystem.cs`** — 1-line auto-attach in `Start`: `if (GetComponent<StatusEffectTint>() == null) gameObject.AddComponent<StatusEffectTint>();`. Verify no double-add, no Awake/Start ordering hazard.
5. **`Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Elementalist.asset`** — `projectileCooldown` 0.34→0.42. Confirm it is the Elementalist-only profile (not shared across classes).

### SECONDARY — hardening (already auditor-reviewed; confirm only)
6. **`Assets/Scripts/Combat/Juice/HitPauseDriver.cs`** ~line 124 — `previousTimeScale = Time.timeScale > 0.0001f ? Time.timeScale : 1f;` (never adopt a paused baseline). Confirm correct + legit slow-mo baselines (tab-open 0.1, DeathScreen) preserved.

## Output
Write findings to `STAGING/_process/2026-06/_review_cx_session_vfx_2026-06-19.md`. Then a <=10-line summary: per-item PASS / ISSUE, each real finding as `file:line — severity — what`. Do NOT fix anything.
