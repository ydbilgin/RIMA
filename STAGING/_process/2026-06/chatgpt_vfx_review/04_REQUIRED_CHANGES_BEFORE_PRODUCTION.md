# Three Required Changes Before Production

## 1. Fix Lightning color taxonomy

### Problem
Spec says:
- Lightning = `#FFD24A`

Taxonomy says:
- Crit = `#FFD24A`
- Lightning = `#FFE600`

### Required change
Update the spec's VFX color table:

```text
Physical = #E89020
Fire = #FF6A1F
Frost = #7FE0FF
Lightning = #FFE600
Void = #7B3FA8
Ability/Magic/Arcane = #00FFCC
Light/Holy = #FFF0B0
Poison = #7BC043
Crit = #FFD24A, reserved for crit feedback only
```

### Why
Crit and Lightning must not share the same color identity. Damage taxonomy already fixed this. The VFX spec must obey it.

---

## 2. Convert `SkillVfx` from pure static hardcode to static façade + profiles

### Problem
The proposed static API is convenient:

```csharp
SkillVfx.Play(archetype, element, pos, dir);
```

But if everything behind it is hardcoded, tuning becomes code surgery.

### Required change
Keep the static façade, but back it with a registry/profile layer:

```csharp
public enum VfxElement
{
    Physical,
    Fire,
    Frost,
    Lightning,
    Void,
    AbilityMagic,
    Light,
    Poison
}

public enum VfxArchetype
{
    CastFlash,
    ImpactBurst,
    ProjectileTrail,
    GroundCrack,
    MeleeArc,
    ChainBolt,
    DashTrail,
    FrostShatter
}

[CreateAssetMenu(menuName = "RIMA/VFX/VfxProfile")]
public sealed class VfxProfile : ScriptableObject
{
    public VfxArchetype archetype;
    public VfxElement element;
    public GameObject prefab;
    public float lifetime;
    public float scale;
    public int sortingOrder;
    public int maxParticles;
    public bool attachToCaster;
}
```

Then:

```csharp
SkillVfx.Play(VfxArchetype.ImpactBurst, VfxElement.Fire, hitPos, dir);
```

### Why
This preserves simple skill code but avoids tuning hell. Humanity has already suffered enough from magic numbers hidden in seven scripts.

---

## 3. Add hard pixel-art implementation rules

### Problem
The spec says “pixel-snapped chunky particles,” but not enough concrete enforcement exists.

### Required change
Add this to the spec:

```text
Pixel VFX rules:
- Particle texture must be chunky square/diamond shard sprites, not default soft circle.
- Texture import: Point filter, no compression artifacts, no mipmaps for pixel textures.
- Particle counts stay low: 8-20 for most bursts, 24 max for major cast/impact.
- Lifetimes stay short: 0.08-0.35s for burst/spark, longer only for attached aura/glow.
- No `new Material(...)` inside cast/hit loops.
- VFX objects are pooled or return-to-pool via ParticleSystem stop callback.
- All VFX render on sorting layer `VFX` with explicit order.
- PPU must match project convention: 64. Existing PPU-32 runtime placeholder sprites are temporary only.
- Shape-critical effects use sprites/flipbooks instead of soft particle blobs.
```

### Why
Without explicit rules, Shuriken quickly produces soft modern glow soup. Delicious in the wrong game. Bad here.
