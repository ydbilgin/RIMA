# Unity — Class Hand VFX Sistemi
**Tarih:** 2026-04-29 | Karar: Sprite'a bake etme, runtime particle kullan

## Neden Runtime?
128px sprite'ta renk/animasyon kontrolü zor. Unity Particle System:
- Exact hex renk (#CCFF00 gibi)
- Animasyonlu (pulse, curl, flicker)
- Ability state'e bağlanabilir (idle=küçük, skill cast=büyük)
- Reusable prefab sistemi

---

## Per-Class Hand Effects

| Class | El | Renk | Efekt tipi |
|---|---|---|---|
| Hexer | Sağ | #CCFF00 | Curse tendrils — ince kıvrılan ışık çizgileri parmak uçlarından |
| Summoner | Sağ | #22FF88 | Necro palm glow — avuç içinden yükselen yumuşak yeşil ışık |
| Elementalist | Sol | #FFD97D | Orb ambient glow — sprite orbun etrafına küçük warm halo |
| Ranger | — | — | Ok çekince bow glow (skill VFX, idle'da yok) |
| Warblade | — | #FF4400 | Rage %100'de kılıç kenarında kırmızı enerji |

---

## Uygulama Planı (Unity — Codex görevi)

### Prefab yapısı
```
Player (sınıf prefabı)
  └── VFX_HandEffect (child GO, sağ/sol el pozisyonunda)
        └── ParticleSystem (HandGlow component)
```

### Script: `HandGlowVFX.cs`
- `[SerializeField] Color glowColor`
- `[SerializeField] float idleIntensity = 0.3f`
- `[SerializeField] float castIntensity = 1.0f`
- `public void SetCastState(bool casting)` → emission rate değişir
- `SkillController.OnSkillExecuted` event'ine bağlan

### Particle ayarları (Hexer curse tendrils örnek)
- Shape: Cone, angle=15, radius=0.05
- Start speed: 0.3–0.8
- Start size: 0.04–0.08
- Lifetime: 0.4–0.7s
- Color over lifetime: #CCFF00 → transparent
- Emission rate idle: 8/s | cast: 40/s
- Gravity: -0.1 (hafif yukarı kıvrılma hissi)

### Summoner (#22FF88 palm glow)
- Shape: Sphere, radius=0.15
- Start speed: 0.1–0.3 (daha durgun, avuçta toplanır)
- Emission rate idle: 5/s | cast: 30/s
- Color: #22FF88 → transparent

---

## Codex Görevi Notu
Bu sistemi Faz 1 loop testi öncesinde kur.
Öncelik sırası: Hexer → Summoner → Warblade (rage VFX) → diğerleri.
Script + prefab, sprite'tan bağımsız — sprite yokken placeholder GO ile test edilebilir.

---

## BÖLÜM 2 — Rift Crack Glow System (Runtime Evaluation)
Tarih: 2026-04-29

---

### SECTION A — Technical Verdict

**CONFIRMED.** The bake-line / runtime-glow split is technically sound for this project.

The codebase uses frame-by-frame sprite animation (Unity Animator + SpriteRenderer, confirmed via PlayerAnimator.cs — no Spine, no PSD2D IK, no SkeletonAnimation found anywhere in Assets/Scripts/). This means there are no bone transforms to attach glow anchors to, but it also means the glow does not need to track fine per-frame weapon positions: a static local-offset child GameObject parented to the character root is the correct approach.

The soft, diffuse nature of the glow (inner light, not a sharp edge highlight) tolerates 3–5px positional drift across animation frames without producing a readable mismatch — the glow radius at runtime (0.15–0.4 Unity units) is significantly larger than any expected frame-to-frame weapon wobble at 128px/PPU=128 scale. The baked crack line anchors the viewer's eye to the geometry; the runtime glow layer adds the color and pulse without needing pixel-perfect alignment.

The one genuine edge case is the Ravager: the scar channels are on the body chest surface, not a held weapon, meaning they move with the character root by definition — origin anchor works perfectly here. No class has a crack location that is so animation-frame-coupled (e.g., rotating tip of a spinning weapon) that the overlay would produce consistent misalignment. The split is confirmed for all 10 classes.

---

### SECTION B — Per-Class Glow Anchor Table

| Class | Crack location | Glow anchor strategy | Local offset estimate | Accent hex |
|---|---|---|---|---|
| Warblade | Armor / blade edge | Fixed offset | `(0.15, 0.1)` right-side weapon | #7BA7BC |
| Elementalist | Leather fingerless glove (non-orb hand, right) | Fixed offset | `(-0.12, 0.0)` right-hand low | #FFD97D (gold-cream, matches orb) |
| Shadowblade | Left dagger blade | Fixed offset | `(-0.15, 0.05)` left-weapon | #9933CC |
| Ranger | Bow limb (grip to upper tip) | Fixed offset | `(-0.1, 0.25)` bow mid-span | #7BA7BC |
| Ravager | Sternum scar channels | Origin | `(0.0, 0.05)` chest center | #FF3322 |
| Ronin | Scabbard edge | Fixed offset | `(-0.18, -0.05)` left hip scabbard | #FFFFFF |
| Gunslinger | Both pistol barrels / cylinders | Fixed offset x2 | `(-0.12, -0.05)` and `(0.12, -0.05)` (dual) | #FFB800 |
| Brawler | Left shoulder / forearm fracture lines | Fixed offset | `(-0.12, 0.15)` left arm mid | #FF8800 |
| Summoner | Staff shaft | Fixed offset | `(-0.2, 0.1)` staff mid | #22FF88 |
| Hexer | Iron lantern frame | Fixed offset | `(-0.18, 0.05)` lantern center | #CCFF00 |

**Notes:**
- Bone-attachment strategy ELIMINATED — no skeletal 2D animation in use (confirmed by codebase scan: no Spine, no PSD2D, no IKManager found).
- Gunslinger uses two child GOs (VFX_RiftGlow_L and VFX_RiftGlow_R) under the character root — both driven by a single RiftGlowVFX component.
- All offsets are starting estimates for inspector tuning; final values set per-prefab by Codex during implementation.

---

### SECTION C — State Behavior Table

| State | Glow behavior | Emission rate | Intensity multiplier | Pulse speed |
|---|---|---|---|---|
| Idle | Slow breathe — always visible, minimal | 3/s | 0.25 | 0.8 Hz |
| Moving | Same as idle — no change | 3/s | 0.25 | 0.8 Hz |
| Skill casting | Surge — bright flash then settle | 25/s (burst 0.15s) → 8/s | 1.0 (burst) → 0.4 (settle) | 2.5 Hz |
| Rage at 100% (RageSystem.RagePercent >= 1.0) | Intense pulse — driven by RageSystem.OnRageChanged | 15/s | 0.8 | 3.0 Hz |
| Hit / damage received (Health.OnDamageTaken) | Single spike flash | 40/s burst 0.08s | 1.2 spike | no change |
| Death (Health.OnDeath) | Fade out over 1.2s → off | ramp to 0 | ramp to 0 | — |

**Hook points confirmed in codebase:**
- `RageSystem.OnRageChanged` (UnityEvent<int,int>) — drives Rage@100% state
- `RageSystem.OnBloodrageStateChanged` (UnityEvent<bool>) — cleaner bool for threshold crossing
- `Health.OnDamageTaken` (UnityEvent<int>) — drives hit spike
- `Health.OnDeath` (UnityEvent) — drives death fade
- `SkillFlowTracker.NotifySkillUsed` — no public event exposed directly; use `SkillFlowTracker.IsChainedToBasic` poll or add `public event Action OnSkillUsed` stub in RiftGlowVFX if per-skill flash is needed
- `PlayerController.IsMoving` — polled in Update (no event needed)

**No `SkillController` monolithic class found** — each class has its own `[Class]_SkillController` (Elementalist_SkillController, Shadowblade_SkillController, Ranger_SkillController confirmed; others assumed same pattern). RiftGlowVFX should use `SkillFlowTracker.NotifySkillUsed` event (requires adding `public event Action<SkillBase> OnSkillUsed` to SkillFlowTracker, or poll `IsChainedToSkill` in Update).

---

### SECTION D — Architecture Decision

**Option B — Separate `RiftGlowVFX.cs`** is the correct choice.

Rationale:
- **Against A (extend HandGlowVFX):** HandGlowVFX is hand-positioned and per-class-hand-specific. Rift glow anchors are on weapons/body parts — different positions, different trigger logic (rage threshold vs skill cast). Merging produces inspector bloat and a script that does two unrelated jobs.
- **Against C (shared VFXController):** Premature generalization. HandGlowVFX and RiftGlowVFX share only Color and emission-rate concepts — not enough coupling to justify a unified controller at this stage. State coupling complexity increases significantly; debugging which system drove which behavior becomes harder.
- **For B:** Clean single responsibility. Both scripts follow the same prefab pattern (child GO under player, ParticleSystem + script). Code reuse achieved through shared particle settings convention and the existing `LightPulse` / `RageSystem` / `Health` event infrastructure — no shared base class needed yet.

```csharp
// sketch only — not full implementation
namespace RIMA
{
    public class RiftGlowVFX : MonoBehaviour
    {
        [Header("Glow Color")]
        [SerializeField] private Color glowColor = Color.white;

        [Header("Intensity Levels")]
        [SerializeField] private float idleIntensity    = 0.25f;
        [SerializeField] private float castIntensity    = 1.0f;
        [SerializeField] private float ragePeakIntensity = 0.8f;
        [SerializeField] private float hitSpikeIntensity = 1.2f;

        [Header("Pulse")]
        [SerializeField] private float idlePulseSpeed  = 0.8f;
        [SerializeField] private float ragePulseSpeed  = 3.0f;

        [Header("References — auto-find on Awake")]
        [SerializeField] private RageSystem   rageSystem;
        [SerializeField] private Health       health;
        [SerializeField] private SkillFlowTracker skillFlow;

        // Optional: second particle system for Gunslinger dual-barrel
        [SerializeField] private ParticleSystem secondaryPS;

        private ParticleSystem ps;
        private ParticleSystem.EmissionModule emission;
        private bool isDead;

        private void Awake() { /* find refs, cache emission module */ }
        private void OnEnable()  { /* subscribe Health.OnDamageTaken, Health.OnDeath, RageSystem.OnBloodrageStateChanged */ }
        private void OnDisable() { /* unsubscribe */ }
        private void Update()    { /* poll RagePercent + IsChainedToSkill; drive idle breath pulse via Sin(Time.time * pulseSpeed) */ }

        private void OnDamageTaken(int _) { /* spike coroutine */ }
        private void OnDeath()            { /* fade out coroutine */ }
        private void OnBloodrageChanged(bool active) { /* swap to rage pulse params */ }

        public void SetCastState(bool casting) { /* emission rate + intensity swap */ }
        public void SetColor(Color c)          { /* runtime color override for Elementalist element-swap */ }
    }
}
```

**Elementalist special case:** `SetColor()` method allows runtime color swap between Fire (#FF6633), Frost (#88CCFF), and Lightning (#FFEE44) based on `Elementalist_SkillController.FireState` / `FrostState` — keeping class identity rules intact.

---

### SECTION E — PixelLab Edit Prompt Updates

The runtime glow decision means PixelLab Edit prompts must NOT bake inner glow as a light/bloom effect — only the hairline crack geometry. Removing "faint inner glow" language from Edit prompts would be **incorrect**: the crack must appear as a visible fracture line with a faint pixel-level color fill inside it (i.e., the crack cavity reads as colored at the sprite level). This is NOT the same as a bloom/soft-glow effect, which is what Unity provides.

**The correct action is to clarify language, not remove it.** The crack should read as "colored interior pixels" (baked), while "glow cloud / aura / soft bloom" remains Unity-only.

Prompts requiring clarification or minor edit:

| Prompt | Current language | Issue | Action |
|---|---|---|---|
| **C1 — Ronin** | "faint white inner light visible inside the crack" | Ambiguous — could be interpreted as soft bloom | KEEP — "inner light" at 128px scale = colored pixels, acceptable |
| **C2 — Gunslinger** | "faint brass-yellow inner light inside" | Same as C1 | KEEP — no change needed |
| **C3 — Ranger** | "faint cold blue inner glow inside the crack" | "glow" language — PixelLab may interpret as bloom blur | MODIFY: replace "inner glow" with "faint cold blue color fill visible inside the crack line" |
| **C4 — Shadowblade** | "faint purple inner glow inside the crack" | Same as C3 | MODIFY: replace "faint purple inner glow inside the crack" with "faint void purple color fill visible inside the hairline crack" |
| **C5 — Elementalist** | "faint gold-cream inner glow matching the orb light" | "glow matching the orb light" may trigger ambient bloom effect | MODIFY: replace "faint gold-cream inner glow matching the orb light" with "faint gold-cream color fill visible inside the hairline crack only — no bloom, no ambient effect" |
| **C6 — Summoner** | "faint green inner glow inside the crack" | "inner glow" language | MODIFY: replace "faint green inner glow inside the crack" with "faint necro green (#22FF88) color fill visible inside the hairline crack only" |
| **C7 — Hexer** | "faint yellow-green inner glow inside the crack" | "inner glow" language | MODIFY: replace "faint yellow-green inner glow inside the crack" with "faint cursed yellow-green (#CCFF00) color fill inside the iron lantern crack line only" |

**Total prompts needing modification: 5 (C3, C4, C5, C6, C7).**
C1 and C2 use "inner light" language which is unambiguous at pixel art scale — no change required.

**Ravager special note (B2 description):** B2 already correctly states "Blood red (#FF3322) in scar channels and axe edge stains ONLY — no glow, no aura." Ravager's blood-red scar channels are the one class where baking some color into the wound channels is correct (dried blood fill, not a luminous glow), and the runtime RiftGlowVFX handles any pulsed emission at the origin anchor separately. No change to B2.

**Brawler (C8 / B1 description):** Already specifies "amber inner glow only in crack and at gauntlet contact points" — this should be clarified to "faint amber (#FF8800) color fill inside the hairline fracture-crack lines only" if re-run, but C8 is a Create (not Edit) so PixelLab has more freedom — acceptable as-is.

---

### SECTION F — Codex Implementation Task (next step)

**Task title:** Implement RiftGlowVFX.cs + per-class prefab anchors

**Scope:** No PixelLab work. No Unity scene structural changes. Script + prefab only.

**Scripts to write:**

1. `Assets/Scripts/VFX/RiftGlowVFX.cs`
   - Namespace: RIMA
   - Component placed on a child GO under each class player prefab
   - Drives a Unity ParticleSystem for the crack glow
   - Supports optional `secondaryPS` for Gunslinger dual-barrel

2. SkillFlowTracker.cs — add one line:
   `public event Action<SkillBase> OnSkillUsed;`
   and fire it in `NotifySkillUsed()`. (1-line addition, stays backward compatible.)

**Prefabs to create:**

One per class, following this structure:
```
VFX_RiftGlow_[ClassName] (Prefab)
  └── ParticleSystem
      └── RiftGlowVFX component
```
Priority order: Warblade → Elementalist → Shadowblade → Ravager → Hexer → Summoner → Ronin → Ranger → Gunslinger → Brawler.
Gunslinger prefab has two child ParticleSystems (L and R barrels).

**Inspector fields required per prefab:**

| Field | Type | Value |
|---|---|---|
| glowColor | Color | Per class hex (Section B table) |
| idleIntensity | float | 0.25 |
| castIntensity | float | 1.0 |
| ragePeakIntensity | float | 0.8 (Ravager/Warblade) / 0.0 (non-rage classes) |
| hitSpikeIntensity | float | 1.2 |
| idlePulseSpeed | float | 0.8 |
| ragePulseSpeed | float | 3.0 |
| rageSystem | RageSystem | Assign in Inspector (null = not rage class) |
| health | Health | Assign in Inspector |
| skillFlow | SkillFlowTracker | Assign in Inspector |
| secondaryPS | ParticleSystem | Gunslinger only |

**Particle system base settings (all classes):**
- Shape: Sphere, radius = 0.08
- Start speed: 0.0 (stationary glow, not directional particles)
- Start size: 0.03–0.06
- Lifetime: 0.5–0.9s
- Color over lifetime: glowColor → transparent
- Emission rate idle: 3/s
- Render mode: Billboard
- Sorting Layer: matches character SpriteRenderer layer, Order +1

**Acceptance criteria:**
- [ ] `RiftGlowVFX.cs` compiles without errors in namespace RIMA
- [ ] Idle glow visible on placeholder GO at runtime with correct accent color
- [ ] `SetCastState(true)` raises emission visibly; `SetCastState(false)` returns to idle
- [ ] `Health.OnDamageTaken` produces a single spike flash
- [ ] `Health.OnDeath` fades glow to zero over 1.2s
- [ ] RageSystem.OnBloodrageStateChanged switches to high-pulse mode (Warblade and Ravager prefabs only)
- [ ] Gunslinger dual-PS fires identically from both anchors
- [ ] All 10 class prefabs created with correct color and local offset per Section B table
- [ ] No changes to any existing VFX scripts (HandGlowVFX, LightPulse, RageVisualFeedback) except the 1-line SkillFlowTracker addition

---

## BÖLÜM 3 — Prefab Wiring Plan
Tarih: 2026-04-29

**Script output:**
- `Assets/Scripts/VFX/HandGlowVFX.cs`
- `Assets/Scripts/VFX/RiftGlowVFX.cs`
- `Assets/Scripts/Systems/SkillFlowTracker.cs` now exposes `public event Action<SkillBase> OnSkillUsed;`

**General wiring convention:**
```
[CharacterOrEnemyRoot]
  ├── SpriteRoot / Renderer child
  └── VFX
      ├── VFX_HandEffect_[Class]     (ParticleSystem + HandGlowVFX)
      └── VFX_RiftGlow_[Class]       (ParticleSystem + RiftGlowVFX)
```

All VFX child objects stay local-offset based. No scene or prefab asset was modified by Codex in this pass; this is an inspector wiring plan for Claude/Unity MCP later.

### Player/Class Prefab Fields

`HandGlowVFX`:
- `glowColor`: class hand/orb color (`#CCFF00`, `#22FF88`, `#FFD97D`, etc.)
- `idleEmissionRate`: 5-8 for hand/orb idle, depending class
- `castEmissionRate`: 30-40
- `skillFlow`: parent `SkillFlowTracker`

`RiftGlowVFX`:
- `glowColor`: per-class crack color from Section B table
- `rageSystem`: parent `RageSystem` only for Warblade/Ravager; leave null for other classes
- `health`: parent `Health`
- `skillFlow`: parent `SkillFlowTracker`
- `secondaryPS`: Gunslinger second pistol particle system only

### BossAI_PenitentSovereign.cs Prefab Plan

Target script path: `Assets/Scripts/Enemies/BossAI_PenitentSovereign.cs`

Suggested hierarchy:
```
PenitentSovereign
  ├── SpriteRoot
  ├── Hurtbox / Collider
  └── VFX
      ├── VFX_Penitent_ShackleWindup
      ├── VFX_Penitent_SurgeCore
      └── VFX_Penitent_DeathFade
```

Required existing components on root:
- `Health`
- `Rigidbody2D`
- `KnockbackReceiver`
- `BossAI_PenitentSovereign`

Inspector wiring:
- `BossAI_PenitentSovereign.bossHealthBar`: scene HUD boss bar or auto-find fallback.
- `VFX_Penitent_ShackleWindup`: ParticleSystem child, disabled by default; later triggered during `ShackleThrow()` windup.
- `VFX_Penitent_SurgeCore`: ParticleSystem child at root/local `(0,0,0)`, radius matching `surgeRadius`; later triggered during `PenitentSurge()`.
- `VFX_Penitent_DeathFade`: ParticleSystem child; connect to `Health.OnDeath` via inspector or small enemy VFX bridge later.

### HollowMite.cs Prefab Plan

Target script path: `Assets/Scripts/Enemies/HollowMite.cs`

Suggested hierarchy:
```
HollowMite
  ├── SpriteRoot
  ├── Collider2D
  └── VFX
      ├── VFX_Mite_ZigzagDust
      └── VFX_Mite_DeathPop
```

Required existing components on root:
- `Health`
- `Rigidbody2D`
- `HollowMite`

Inspector wiring:
- `VFX_Mite_ZigzagDust`: small dust ParticleSystem behind feet; local offset `(0,-0.15,0)`, emission low while moving.
- `VFX_Mite_DeathPop`: short burst ParticleSystem; bind to `Health.OnDeath` via inspector or future enemy VFX bridge.
- No `RiftGlowVFX` required unless a fractured/elite variant is made.

### TheWound.cs Prefab Plan

Target script path: `Assets/Scripts/Enemies/TheWound.cs`

Suggested hierarchy:
```
TheWound
  ├── SpriteRoot
  ├── Collider2D
  └── VFX
      ├── VFX_Wound_HealAura
      ├── VFX_Wound_CorePulse
      └── VFX_Wound_DeathBurst
```

Required existing components on root:
- `Health`
- `Rigidbody2D`
- `TheWound`

Inspector wiring:
- `TheWound.enemyLayer`: must include all enemy layers that should receive healing and death burst damage.
- `VFX_Wound_HealAura`: looping radial ParticleSystem; local origin, visual radius should match `healRadius`.
- `VFX_Wound_CorePulse`: idle pulse ParticleSystem on body core; low emission, red/pink wound color.
- `VFX_Wound_DeathBurst`: one-shot ParticleSystem; trigger alongside `DeathSequence()` or bind via `Health.OnDeath`.

### Acceptance Checklist
- [ ] Player class prefabs use `HandGlowVFX` only for hand/orb idle identity effects.
- [ ] Player class prefabs use `RiftGlowVFX` only for baked crack glow anchors.
- [ ] Enemy prefabs above use simple child ParticleSystems; no player class VFX scripts are required unless a crack-glow elite variant is introduced.
- [ ] No VFX child contains gameplay collider or damage logic.
- [ ] All VFX children sit under a `VFX` parent for hierarchy clarity.
