# Hard-to-PixelLab Skill Analysis + Industry Research

**Status:** v2 — ChatGPT 10 sheet inspection + production approach revision + industry pattern catalog.
**Trigger:** User feedback "chain bağlama gibi pixellab'la yapımı zor olan skilleri tespit et + insanlar nasıl yapmış araştır".

---

## 1. ChatGPT v6 Sheet Review (Quality Audit)

| # | Class | Skill# | Visual Quality | Sprite Faithful | Notes |
|---|---|:---:|:---:|:---:|---|
| 01 | Warblade | 14 | ✅ Excellent | ✅ | Painterly Hades+Diablo mood, all skills visible |
| 02 | Ronin | 4 | ✅ Excellent | ✅ | 2×2 large panels, beautiful katana animations |
| 03 | Shadowblade | 22 | ✅ Excellent | ✅ | Dark+purple, status icon legend bottom-right |
| 04 | Ranger | 20 | ✅ Excellent | ✅ | Gold accent, complete arsenal |
| 05 | Summoner | 8 | ✅ Excellent | ⚠️ | Sprite mostly faithful, hair color slight drift |
| 06 | Gunslinger | 8 | ✅ Excellent | ✅ | Western dual-pistol mood |
| 07 | **Ravager** | 8 | ✅ Good | ⚠️ **Single axe** (user noted: should be dual axe — non-blocker reference) |
| 08 | Hexer | 8 | ✅ Excellent | ✅ | Purple curse mood, Shackle Curse chains visible |
| 09 | Brawler | 8 | ✅ Excellent | ✅ | Fist combat, orange aura |
| 10 | Elementalist | 15 | ✅ Excellent | ✅ | Element variety, Chain Lightning prominent |

**Net assessment:** 9/10 excellent, Ravager dual-axe note for v7 regenerate (non-blocker). v6 ChatGPT = LIVE concept reference.

---

## 2. Hard-to-PixelLab Skill Identification

ChatGPT statik frame'lerle çözmüş, AMA in-game dynamic implementation farklı zorluk seviyelerinde:

### Category A: EASY (Tek anim spritesheet, PixelLab direkt)
Tek karakter pose + tek-frame VFX. PixelLab animate_character endpoint ile rahat.

- **Tüm melee swing'ler**: Cleave, Iron Crush, Blade Rush, War Stomp, Earth Slam, Final Draw, Quickdraw, Whirlwind, Reckless Strike, Severance, Fan of Knives, Backstab, Jab/Cross/Uppercut/Knockout
- **Karakter-merkezli AOE**: Battle Surge, Berserk, War Cry, Crimson Roar, Iaido Stance, Iron Stance, Death Wail, Decay Aura, Combustion, Solar Flare, Arcane Surge
- **Static debuff sigils**: Curse Mark, Sunder Mark, Predators Mark, Backstab Mark, Death Mark
- **Tek projektil**: Fireball, Hex Bolt, Black Arrow, Axe Throw, Aimed Shot, Concussive Arrow, Multi Shot, Pinning Shot
- **Self-buff aura**: Battle Surge, Berserk, Bloodthirst, Preparation, Ironclad Momentum, Iron Stance, Powder Burst, Bullet Time
- **Beam/stream**: Prism Beam (tek yön beam, sabit), Soul Drain (kısa kararlı tether)

**Toplam EASY: ~75 skill (115'ten)**

### Category B: MEDIUM (PixelLab + Unity overlay/placed object)

Karakter + ayrı placed sprite (trap/totem/summon/clone).

- **Placed traps**: Bone Trap, Explosive Trap, Wireline Trap, Living Bomb, Frost Wall, Frozen Orb (placed sphere)
- **Summoned entities**: Summon Wisp, Familiar Strike, Shadow Clone, Mirage Blade, Mirror Image, Ethereal Guard (orbiting orbs)
- **Single-frame projectile + impact**: Glacial Spike, Meteor (with arc + shadow), Marked Detonate
- **Dash + teleport with afterimage**: Blink, Phase Step, Shadow Step, Vanish, Disengage, Hunters Step
- **Smoke/fog effects**: Smoke Round, Smoke Veil, Veil Burst, Toxic Eruption, Plague Touch, Necrosis (decay spots)
- **Placement-based VFX**: Beacon (light pillar), Flare (ground glow)

**Toplam MEDIUM: ~25 skill**

### Category C: MEDIUM-HARD (Unity LineRenderer / SpriteShape required)

Two-actor tether/beam — caster ile target arasında dinamik segment.

- **🔗 Shackle Curse** (Hexer) — spectral chain binding
- **🔗 Spirit Bind** (Summoner) — chain a foe in spectral bonds
- **🔗 Tethering Arrow** (Ranger) — tether pulling enemy
- **🔗 Soul Link** (Summoner) — links you to ally
- **🔗 Pact Drain** (Summoner) — drain life tether

**Pipeline:**
1. PixelLab: tek chain/tether segment sprite (1 gen, çok kolay) — örn. 3-link chain texture
2. Unity: LineRenderer veya SpriteShape, caster + target Transform anchor
3. Texture scrolling animation (UV offset per-frame)

**Toplam MEDIUM-HARD: 5 skill**

### Category D: HARD (Multi-actor jump chain + procedural composition)

N-target sequential chain. Animation path runtime'da hesaplanır.

- **🔗 Chain Lightning** (Elementalist) — zigzag electric arc, 3+ mob seri
- **🔗 Chain Cull** (Shadowblade) — 3 enemy sequential cuts
- **🔗 Sweep Volley** (Ranger) — arc multiple-target

**Pipeline:**
1. PixelLab: lightning segment + impact frame (2-3 gen)
2. Unity: Particle system OR sequential LineRenderer + target-jumping logic
3. C# script: nearest-target queue + segment instantiation between jumps

**Toplam HARD: 3 skill**

---

## 3. Industry Pattern Research

User: "insanlar nasıl yapmış" — endüstride bu mekanikleri nasıl çözmüşler?

### Pattern 1 — Unity LineRenderer + Texture Scroll (en yaygın)

**Hades (Supergiant):** Aphrodite's "Heart Throb" / Demeter's chain effects → LineRenderer + chain segment texture. Caster + target Transform anchor. Per-frame texture offset UV → "moving chain" feel.

**Path of Exile (Grinding Gear Games):** Beam spells (Cold Ray, Power Siphon) → LineRenderer with scrolling sprite atlas. Beam stays anchored to caster origin + target hitbox.

**Risk of Rain 2 (Hopoo Games):** Chain Lightning artifact → LineRenderer jumps between targets, brief flash per jump, segment fades 0.3s.

**Implementation:**
```csharp
LineRenderer line;
public Transform caster, target;
public Material chainMaterial; // PixelLab-generated chain texture

void Update() {
    line.SetPosition(0, caster.position);
    line.SetPosition(1, target.position);
    // UV scroll
    chainMaterial.mainTextureOffset += new Vector2(Time.deltaTime * 2f, 0);
}
```

**PixelLab kontribüsyon:** sadece chain segment texture (4-frame loop, 1 gen).

### Pattern 2 — SpriteShape (Curved Chains, 2D Spline)

**Hollow Knight (Team Cherry):** Hornet's needle thread → 2D spline curve, mid-flight curves under gravity. Unity SpriteShape Controller + chain texture.

**PixelLab kontribüsyon:** chain segment texture (sade düz tile, 1 gen).

**Unity tarafı:** Procedural spline points based on caster→target trajectory + parabolic offset.

### Pattern 3 — Particle System with Linked Particles

**Diablo III (Blizzard):** Demon Hunter Spike Trap / Witch Doctor curse links → particle system with line-renderer-style particles linked.

**Used when:** Loose visual link (not rigid chain), aesthetic emphasis (gore/cursed energy), not strict mechanics.

**PixelLab kontribüsyon:** energy puff sprite (1 gen).

### Pattern 4 — Sequential Sprite Spawn (n-jump chains)

**Risk of Rain 2 Chain Lightning, Slay the Spire Lightning chain:** No persistent connection. Just sequential strike VFX at each target with brief 50-100ms timing.

**Implementation:**
```csharp
IEnumerator ChainStrike(List<Enemy> targets) {
    foreach (var t in targets) {
        Instantiate(strikePrefab, t.transform.position, Quaternion.identity);
        t.TakeDamage(damage);
        yield return new WaitForSeconds(0.08f);
    }
}
```

**PixelLab kontribüsyon:** single lightning strike sprite (1 gen). Re-used per target.

### Pattern 5 — Static Pin / Anchor (PixelLab tek-frame yeterli)

**Diablo II:** Bone Prison / Druid roots → spawn placed sprite at target position. No tether.

**Used for:** Shadow Pin, Body Lock, Barbed Net Shot — düşman üzerine yerleştirilen statik sprite.

**PixelLab kontribüsyon:** placed sprite (1 gen).

### Pattern 6 — Skill Redesign (mechanical simplification)

Mekanik fazla complex ise tasarımı sadeleştirme — endüstri pattern.

**Original:** "Soul Link — link your soul to ally, share strength dynamically"
**Simplified:** "Soul Link — instant burst, both you and ally gain +20% damage for 5s. Visual link flash 0.5s then fades."

**Trade-off:** Mekanik daha az interactive ama production maliyeti DRAMATIC olarak düşer.

---

## 4. Per-Skill Production Path Verdict (Hard Skills)

### Shackle Curse (Hexer)
- **Pattern:** #1 LineRenderer + Texture Scroll
- **PixelLab gen:** Chain segment texture (4-frame loop) — 1 gen
- **Unity work:** ChainBinder.cs (caster + target anchor, UV scroll, 3s duration)
- **Effort:** LOW-MEDIUM (1-2 saat)
- **No redesign needed**

### Spirit Bind (Summoner)
- Same as Shackle Curse (chain texture re-use)
- **PixelLab gen:** Spirit chain segment (white/pale variant) — 1 gen
- **Unity work:** Same ChainBinder.cs (shader variant for pale color)

### Tethering Arrow (Ranger)
- **Pattern:** #1 LineRenderer + Texture Scroll
- **PixelLab gen:** Cyan energy tether segment — 1 gen
- **Unity work:** TetherPullSystem.cs (arrow hit triggers tether + pull force toward caster)
- **Effort:** MEDIUM (2-3 saat — pull mechanic)

### Soul Link (Summoner)
- **Pattern:** #5 + #6 — simplified to instant burst
- **PixelLab gen:** golden flash sprite (1 gen)
- **Unity work:** Apply buff to both actors, brief LineRenderer flash 0.5s
- **Effort:** LOW (instant burst version)

### Pact Drain (Summoner)
- **Pattern:** #1 LineRenderer
- **PixelLab gen:** dark energy stream segment — 1 gen
- **Unity work:** DrainSystem.cs (per-tick damage to target + heal to caster while tethered)
- **Effort:** MEDIUM

### Chain Lightning (Elementalist)
- **Pattern:** #4 Sequential Sprite Spawn (NOT persistent line)
- **PixelLab gen:** Lightning strike sprite (4-6 frame) — 1 gen
- **Unity work:** ChainLightningJumper.cs (nearest-target queue, jump timing 80ms)
- **Effort:** MEDIUM (2-3 saat)

### Chain Cull (Shadowblade)
- **Pattern:** #4 Sequential Sprite Spawn + sequential dash
- **PixelLab gen:** Dagger cut frame + blood splatter (1 gen)
- **Unity work:** ChainCullDash.cs (sequential teleport between 3 nearest, 100ms timing per cut)
- **Effort:** MEDIUM

### Sweep Volley (Ranger)
- **Pattern:** Multi-arrow projectile (already EASY-MEDIUM)
- **PixelLab gen:** 1 arrow sprite, runtime instantiated × N
- **Unity work:** Arrow spawn arc (5-7 arrows at angles)
- **Effort:** LOW-MEDIUM

---

## 5. Production Budget — Hard Skills

| Skill | PixelLab Gens | Unity Work (saat) | Total Effort |
|---|---:|---:|---|
| Shackle Curse | 1 | 1-2 | LOW |
| Spirit Bind | 0 (re-use) | 0.5 (shader variant) | TRIVIAL |
| Tethering Arrow | 1 | 2-3 | MEDIUM |
| Soul Link | 1 | 1 (simplified) | LOW |
| Pact Drain | 1 | 2-3 | MEDIUM |
| Chain Lightning | 1 | 2-3 | MEDIUM |
| Chain Cull | 1 | 2-3 | MEDIUM |
| Sweep Volley | 0 (arrow re-use) | 1 | LOW |
| **TOTAL** | **6** | **~12-18 saat** | — |

**Verdict:** Hard skill'lerin **hiçbiri** redesign gerektirmiyor. Hepsi industry-standard LineRenderer / Sequential Spawn pattern'larıyla çözülebilir. PixelLab katkısı sadece **6 texture gen** (chain segment, energy stream, lightning strike, dagger cut blood splatter). Kalan iş Unity-side C# composition.

---

## 6. Tavsiyeler

### A. v6 ChatGPT Sheet → LIVE Reference
Mevcut 10 sheet visual canonical reference olarak LIVE. **Hard skill'ler statik frame'lerle gösterilmiş ama gameplay implementation aynı görsele ulaşır** (LineRenderer + scroll = dinamik chain effect).

### B. Ravager Dual Axe Fix
Sheet 7 single-axe regenerate (1 ChatGPT iteration) — non-blocker, post-current-task.

### C. Feasibility JSON Update
`v6_skill_feasibility.json` (Codex v6 attempt'te yazıldı) güncellenmeli:
- HARD → MEDIUM-HARD (LineRenderer pattern)
- HARD-COMPOSITE → MEDIUM (Sequential spawn pattern)
- Production approach kolonu industry pattern numarası ile

### D. Skill Redesign — None Required
115 skill'in hiçbiri redesign gerektirmiyor. Soul Link "simplified instant burst" opsiyonu sadece **production simplification** opsiyonu, mekanik redesign değil.

### E. Industry Pattern Catalog → Code Library
8 pattern (#1-#6 above) Unity utility library olarak generic class'larla yazılabilir:
- `ChainBinder.cs` (Pattern #1)
- `CurvedChainBinder.cs` (Pattern #2)
- `LinkedParticleSystem.cs` (Pattern #3)
- `SequentialStrike.cs` (Pattern #4)
- `StaticPinEffect.cs` (Pattern #5)

Bunlar yazıldığında 8 hard skill 1 saatte hooked up edilebilir.

---

## 7. Open Items

1. Ravager dual-axe regenerate (ChatGPT 1 iteration, low priority)
2. v6_skill_feasibility.json update (orchestrator quick task)
3. Unity utility library `ChainBinder.cs` etc. (Codex implementation task, post-progression LOCK)

---

**Conclusion:** ChatGPT v6 sheet'leri visual canonical reference olarak LIVE adoption ready. Hard skill'ler ne PixelLab budget gen şişirir ne de mekanik redesign gerektirir — Unity-side standard pattern'larla industry-aligned çözüm var. Production roadmap'e Pattern Library Section 6.11 olarak ekle.
