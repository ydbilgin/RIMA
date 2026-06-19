# Codex Review - session VFX/feel changes - 2026-06-19

Scope reviewed:
- Assets/Scripts/VFX/SkillVfx.cs
- Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs
- Assets/Scripts/VFX/StatusEffectTint.cs
- Assets/Scripts/Systems/StatusEffects/StatusEffectSystem.cs
- Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Elementalist.asset
- Assets/Scripts/Combat/Juice/HitPauseDriver.cs

Findings:

1. Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs:102 - Medium - The new on-hit lambda explicitly handles a null hit at line 98 by falling back to the projectile position for VFX placement, but then immediately calls hit.GetComponent<StatusEffectSystem>(). If PlayerProjectile ever invokes the callback with null or a destroyed target, the impact VFX will play and then the handler will throw before status application. Guard before GetComponent or return when hit is null.

Review notes:

- SkillVfx.ProjectileBlaze uses trail.widthCurve followed by trail.widthMultiplier = 0.10f, which is the correct ordering for the Unity TrailRenderer widthCurve reset behavior. The only other TrailRenderer path in this file, ProjectileTrail, does not assign widthCurve; ChainBolt uses LineRenderer.widthMultiplier only. No other widthCurve/startWidth ordering issue found in SkillVfx.cs.
- ProjectileBlaze child cleanup is acceptable for the current new-projectile callsite: embers and glow are parented under the projectile, and BlazeGlowFlicker exits when the SpriteRenderer is destroyed. Shared additive material reuse is centralized. Per-projectile allocations exist but are bounded to this visual feature and not a leak in the reviewed code.
- ImpactExplosion creates a temporary runtime sprite template, delegates lifetime to PlayBurst, then destroys the template. No leaked template object found.
- CastRhythmBehavior preserves element-aware mapping through GetVfxElement: Fire -> Fire, Frost -> Frost, Light -> Lightning, fallback -> Arcane. VFX wiring is single-call on projectile spawn and two intentional impact calls on hit.
- StatusEffectTint has no obvious per-frame managed allocations in LateUpdate. It caches renderer arrays and original colors once, handles multiple SpriteRenderers, unsubscribes OnEffectApplied in OnDestroy, and is inert when no StatusEffectSystem is present. Restore is immediate to cached original colors when no dominant effect remains.
- StatusEffectSystem auto-attach is guarded with GetComponent<StatusEffectTint>() == null, so no double-add is present. Runtime-added StatusEffectTint can wire in Start or first LateUpdate.
- BasicAttackProfile_Elementalist.asset is named BasicAttackProfile_Elementalist, has classType: 2, and has projectileCooldown: 0.42. Within the reviewed asset, this is the Elementalist profile.
- HitPauseDriver now records previousTimeScale only when Time.timeScale > 0.0001f; non-zero slow-mo baselines such as 0.1 are preserved, while a paused zero baseline is intentionally not adopted.

Summary:
1. PASS - SkillVfx ProjectileBlaze/ImpactExplosion width ordering, cleanup, material reuse, and renderer scan.
2. ISSUE - Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs:102 - Medium - null-aware hitPos fallback is followed by hit.GetComponent dereference.
3. PASS - StatusEffectTint lifecycle, multi-renderer handling, no per-frame allocation hotspot, event cleanup.
4. PASS - StatusEffectSystem guarded auto-attach, no double-add, no reviewed Start ordering break.
5. PASS - BasicAttackProfile_Elementalist.asset is Elementalist-named classType 2 and projectileCooldown is 0.42.
6. PASS - HitPauseDriver paused-baseline guard preserves positive slow-mo baselines.
