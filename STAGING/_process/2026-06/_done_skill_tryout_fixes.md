# Skill try-out fixes — DONE (2026-06-18)

All 3 fixes applied surgically. Compile clean, 0 console errors after each. FrameRateGuard loaded; playModeStartScene=MainMenu preserved; no Play session used (edit-mode data-proof only); no leftover temp objects.

## Fix 1 [BUG] — Elementalist LMB (Rift Bolt) VFX
`Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs`
- Added `GetVfxElement(...)` helper after `GetElementTag` (~L129): Fire→Fire, Frost→Frost, Light→Lightning, default→Arcane.
- `ExecuteBolt`: `SkillVfx.CastFlash(owner.gameObject, vfx)` after empowered/damage calc (~L60); `SkillVfx.ProjectileTrail(go, vfx)` before `AddComponent<PlayerProjectile>` (~L82); `SkillVfx.ImpactBurst(hit pos, vfx)` as first line of existing `SetOnHit` lambda (~L92).
- Data-proof: all 3 SkillVfx(GameObject/Vector3, VfxElement) signatures + GetVfxElement helper present via reflection.

## Fix 2 [BUG] — Skills cast/hit dummy (root cause = empty chamber loadout) + Fix 3 [FEATURE] — try-out
`Assets/Scripts/UI/ChamberSelectBootstrap.cs` (chamber-only; run-start empty-loadout design lock untouched)
- New `PracticeKits` dict (Warblade: Iron Charge/Gravity Cleave/Earthsplitter/Cleave; Elementalist: Fireball/Glacial Spike/Chain Lightning/Frozen Orb — first 3 mirror DraftManager.ClassKits).
- `GrantPracticeLoadout(cls)`: resolves active controller, binds Q/E/R/F via SkillDatabase→AddComponent(skillType)→SetSlot (mirrors DraftManager.AssignActive); called from `SpawnPlayer` (~L990) and `ApplySelectedClassToPlayer` (~L1745).
- `TopUpPracticeResource()`: class-aware (Elementalist=ManaSystem else RageSystem; player carries both after attune); periodic refill in `Update` (0.5s cadence, ~L233) since Rage decays.
- Field `practiceRefillTimer` (~L99); dummy prompt updated to "Dummy — LMB + Q/E/R/F ile dene  [G] Sınıf Seç" (~L348).
- Data-proof: all 8 kit skills resolve non-null/implemented SkillBase types; end-to-end edit-mode test filled Elementalist slots 0-3 (TryUse null-guard now passes).

BLOCKED: none. Fix 3 implemented (no risk to select/play flow — chamber-only, additive).
