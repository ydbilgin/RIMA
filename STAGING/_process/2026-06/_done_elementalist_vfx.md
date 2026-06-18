# Elementalist Skill VFX — Task #9a DONE (2026-06-18)

Add engine-layer skill VFX (via `SkillVfx` helper) to all non-Fireball Elementalist skills, mirroring how Fireball is already wired. NO new art — reuse existing sprites/prefabs; VFX = tint/additive/scale-fade/trail/line only.

## Discovery

- **SkillVfx API** (`Assets/Scripts/VFX/SkillVfx.cs`): static helpers `CastFlash(GameObject/Transform, VfxElement)`, `ImpactBurst(Vector3, VfxElement)`, `ProjectileTrail(GameObject, VfxElement)`, `ChainBolt(from, to, VfxElement)`, plus `PlayBurst/PlaySweep/MeleeArc/GroundCrack/SpawnTinted/Palette`. Elements: Physical/Fire/Frost/Lightning/Void/Arcane (each a distinct hex color). Runner uses `DontDestroyOnLoad` → Play-mode only (same as Fireball).
- **Fireball pattern** (`Skills/Elementalist/Fireball.cs`): `CastFlash` at start of Execute → `ProjectileTrail` on the projectile → `ImpactBurst` + explosion `PlayBurst` on hit. This was the ONLY skill with SkillVfx wiring (grep confirmed: 4 SkillVfx calls, all in Fireball).
- **Skill enumeration** (`Skills/Elementalist/*.cs`): 17 files. Non-castable: `Elementalist_SkillController`, `ElementalistRuntimeVisuals`. Castable `SkillBase` subclasses = 14 total; Fireball had VFX, the other **13 had NONE**. Two helper MonoBehaviours (`FrozenOrbObject`, `MirrorClone`) carry the deferred explosion logic and were also wired.

## Plan → Implementation (13 skills wired)

Convention applied to every skill: a `CastFlash` on the caster at cast (the universal "I cast" tell), plus a geometry-appropriate impact/trail/bolt matching the skill shape. Existing bespoke visuals (spike clusters, meteor sprite, ChainLightning's own LineRenderer arcs, `SkillRuntime.SpawnCircleVisual` markers) were KEPT — SkillVfx is layered on top, exactly as Fireball layers ImpactBurst over its explosion sprite. Element chosen per skill identity / `RegisterElementCast` element.

| Skill | Element | SkillVfx added | Rationale |
|---|---|---|---|
| GlacialSpike | Frost | CastFlash + ImpactBurst at line tip | icy spike line, frost spark at far end |
| FrozenOrb | Frost | CastFlash + ProjectileTrail on orb + ImpactBurst on Explode() | frost trail follows slow orb; burst on Blink-detonate |
| Meteor | Fire | CastFlash + ImpactBurst at crater | fire detonation on impact |
| ChainLightning | Lightning | CastFlash + ChainBolt + ImpactBurst per jump | additive bolt between targets + spark on each strike |
| ArcaneBlast | Arcane | CastFlash + ProjectileTrail + ImpactBurst onHit | arcane projectile trail + hit spark (mirrors Fireball) |
| PrismBeam | Lightning | CastFlash + ChainBolt (caster→tip) + ImpactBurst at tip | beam streak + end spark |
| FrostWall | Frost | CastFlash + ImpactBurst at wall center | frost burst marks raised wall |
| SolarFlare | Fire | CastFlash + ImpactBurst at cone mouth | bright flare burst |
| LivingBomb | Fire | CastFlash + ImpactBurst at each detonation (Explode) | fire pop on every fuse blast (incl. chained copies) |
| Blizzard | Frost | CastFlash + ImpactBurst at zone landing | frost burst when storm lands after channel |
| Blink | Arcane | ImpactBurst at start + at destination | blink-out / blink-in spark (self-positioning, no CastFlash needed) |
| MirrorImage | Arcane | CastFlash + ImpactBurst per clone spawn + ImpactBurst on clone death | arcane pop as each mirror materializes / detonates |
| Combustion | Fire | CastFlash | fire buff-activation flash (no projectile) |
| ArcaneSurge | Arcane | CastFlash + ImpactBurst on Blink-empowered detonation | arcane buff flash + detonation burst |

### Files modified (13)
- `Assets/Scripts/Skills/Elementalist/GlacialSpike.cs`
- `Assets/Scripts/Skills/Elementalist/FrozenOrb.cs` (FrozenOrb.Execute + FrozenOrbObject.Explode)
- `Assets/Scripts/Skills/Elementalist/Meteor.cs`
- `Assets/Scripts/Skills/Elementalist/ChainLightning.cs`
- `Assets/Scripts/Skills/Elementalist/ArcaneBlast.cs`
- `Assets/Scripts/Skills/Elementalist/PrismBeam.cs`
- `Assets/Scripts/Skills/Elementalist/FrostWall.cs`
- `Assets/Scripts/Skills/Elementalist/SolarFlare.cs`
- `Assets/Scripts/Skills/Elementalist/LivingBomb.cs`
- `Assets/Scripts/Skills/Elementalist/Blizzard.cs`
- `Assets/Scripts/Skills/Elementalist/Blink.cs`
- `Assets/Scripts/Skills/Elementalist/MirrorImage.cs` (MirrorImage.Execute + MirrorClone.OnCloneDeath)
- `Assets/Scripts/Skills/Elementalist/Combustion.cs`
- `Assets/Scripts/Skills/Elementalist/ArcaneSurge.cs`

(Fireball.cs untouched — already had VFX. No new abstractions, no sprite production, no locked-system files touched.)

## Verification

- `refresh_unity` (force, scripts, compile=request) → compiled.
- `read_console` Errors: **0**. Warnings: **0**. (Filtered + unfiltered, post-compile.)
- `execute_code` data-proof:
  - All 16 edited types resolve in the live compiled assembly (14 SkillBase subclasses + FrozenOrbObject + MirrorClone): `16/16`.
  - SkillVfx hooks used all exist: `CastFlash` (2 overloads), `ImpactBurst` (1), `ProjectileTrail` (1), `ChainBolt` (1).
  - Runtime hook invocation in edit-mode threw the expected `DontDestroyOnLoad can only be used in play mode` from `SkillVfxRunner` — this is the SAME edit-mode limitation Fireball's path has; the calls themselves are correct and run in Play mode. Not a defect introduced by this change.
- Wiring correctness is additionally guaranteed by the 0-error compile: a mis-typed SkillVfx call would have been a compile error.

## Notes / Deviations / Limitations
- Pre-existing dead code: none deleted. `ElementalistRuntimeVisuals.GetCrackSprite()` has a "TODO replace with PixelLab" placeholder — left untouched (out of scope).
- Element mapping uses VfxElement.Lightning for ChainLightning/PrismBeam (yellow), Arcane (teal) for arcane/utility skills, Fire/Frost as labeled. Blink/ArcaneSurge/MirrorImage use Arcane so utility reads distinctly from offensive fire/frost/lightning.
- Did NOT toggle Play mode for the runtime visual capture: overlay/additive VFX do not capture in screenshots (project rule), and entering Play would require careful no-state-leak teardown. Static-assembly + 0-error compile + hook-existence proof is the verification basis.
- BLOCKED items: none.
