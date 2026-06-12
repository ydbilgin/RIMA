# Fix combat core wiring after overnight Director/stat run

## Problem

Overnight autonomous run added stat/damage core and Director Mode. Review found several correctness risks in combat wiring:

1. Melee finisher is likely passed as `DamagePacket.isCrit`, causing implicit 1.5x crit multiplier.
2. `DamageCalculator` supports defender armor/MR, but `SkillRuntime` production path does not pass defender stats.
3. Ranger `ShotCadenceBehavior` projectile bypasses `DamagePacket/DamageCalculator`, unlike Elementalist.
4. Zero-damage/status-only packets can become 1 chip damage.
5. Director TEST mode may keep overlay raycasts active and block gameplay input.
6. PlayerStats and Health are separate HP authorities; needs decision or bridge.

## Scope

Correctness-only fix pass. No visual polish, no C4/C5 work, no elemental resist matrix.

## Files to inspect first

- `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`
- `Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs`
- `Assets/Scripts/Skills/SkillRuntime.cs`
- `Assets/Scripts/Skills/PlayerProjectile.cs`
- `Assets/Scripts/Balance/DamageCalculator.cs`
- `Assets/Scripts/Core/Health.cs`
- `Assets/Scripts/UI/DirectorMode.cs`
- `Assets/Scripts/Systems/PlayerClassManager.cs`

## Acceptance

- Finisher != crit.
- Ranger projectile uses DamagePacket path.
- Defender stats applied where available.
- Zero damage remains zero.
- Director TEST mode does not eat LMB/RMB/mouse aim.
- CombatContract still green.
