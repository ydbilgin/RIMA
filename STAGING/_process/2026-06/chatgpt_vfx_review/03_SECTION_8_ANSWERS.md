# Answers to Spec §8 Open Questions

## 1. Pixel-snapped chunky particle, flipbook sprite-sheet, or hybrid?

**Answer: Hybrid.**

Use chunky pixel particles for:
- sparks;
- embers;
- dust puffs;
- frost chips;
- tiny lightning sparks;
- cast hand glows;
- short hit bursts.

Use flipbook or hand-drawn sprites for:
- frost shatter;
- melee swing arcs;
- ground cracks;
- Earthsplitter fissures;
- major impact silhouettes;
- any effect whose readability depends on shape rather than particle scatter.

Why:
- ParticleSystem is fastest and reusable for small burst noise.
- Flipbook/sprite-sheet is visually cleaner for iconic shapes.
- Pure ParticleSystem risks soft Unity look.
- Pure flipbook for all 8 VFX is slower and overkill for demo.

Final rule:
> Particle for texture/noise. Sprite/flipbook for shape.

## 2. Static `SkillVfx` API or SO-based `VfxProfile`?

**Answer: Static façade backed by lightweight profiles.**

Recommended shape:

```csharp
public static class SkillVfx
{
    public static void Play(VfxArchetype archetype, VfxElement element, Vector3 pos, Vector2 dir = default, Transform attachTo = null);
}
```

Internally:
- `SkillVfxDatabase` or `VfxRegistry` maps `(archetype, element)` to a prefab/profile.
- `VfxProfile` holds prefab, lifetime, sorting order, particle cap, color override, scale, attach mode, optional material.
- Skill-specific override profiles are only added when the archetype profile is not enough.

Why not pure static hardcoded?
- Hardcoded static config becomes painful when tuning VFX sizes/colors/lifetimes.
- Artists/designers cannot tweak prefab mappings without code changes.
- It risks scattering magic numbers through skill scripts, the ancient curse of Unity projects.

Why not full SO-per-skill immediately?
- Overkill for 8 demo VFX.
- The spec is right that SO should be used when variation is real.

## 3. Chain Lightning: LineRenderer or particle trail?

**Answer: LineRenderer, but pixel-styled.**

Use LineRenderer for the main bolt because the gameplay is target-to-target chain logic and LineRenderer cleanly communicates connection.

Rules:
- 3 to 5 jagged points per segment.
- Snap points to 1/64 world increments or at least pixel-ish increments.
- Use a shared material, not `new Material` per cast.
- Short lifetime: 0.08 to 0.14 seconds.
- Add tiny particle sparks at hit points if desired.
- Main bolt color: Lightning `#FFE600`, not Crit gold `#FFD24A`.

Particle trail alone is worse here because it reads like lingering energy smoke rather than an instant chain connection.

## 4. Melee swing arc: procedural mesh/trail or hand-drawn arc sprite?

**Answer: hand-drawn/pixel arc sprite for demo, optional procedural placement.**

Use a small set of arc sprites:
- light swing arc;
- heavy/cleave arc;
- maybe wide Gravity Cleave arc.

Then rotate/flip/place according to facing direction.

Why:
- Melee arc silhouette matters a lot.
- TrailRenderer often looks smooth and modern unless aggressively constrained.
- Hand-drawn arc keeps style consistent and avoids “transparent banana smear” syndrome.

Acceptable demo fallback:
- A simple procedural mesh arc with a pixelated material is acceptable only if no art sprite exists yet.
- Do not use soft TrailRenderer as the final demo visual.

## 5. Are 8 VFX realistic in demo time?

**Answer: Yes, but only with strict tiering.**

8 VFX are realistic if they are not all treated as bespoke mini-cinematics.

Recommended priority:

### Tier 1: Must polish first
1. Fireball
2. Warblade basic swing
3. Gravity Cleave

These are frequent and define class feel.

### Tier 2: Next
4. Chain Lightning
5. Iron Charge
6. Earthsplitter

These need strong readability but can be built from reusable archetypes.

### Tier 3: Last polish
7. Glacial Spike
8. Elementalist basic bolt

Glacial Spike already has sprite cluster direction; Elementalist basic can reuse mini-bolt + mini-impact.

Production rule:
- Do not attempt 8 unique bespoke VFX from scratch.
- Build 5 reusable archetypes, then tune per skill.

Reusable archetypes:
- CastFlash
- ImpactBurst
- ProjectileTrail
- MeleeArc
- GroundCrack / DashTrail / ChainBolt variant set
