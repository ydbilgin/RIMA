# Karar #122 — Echo Resonance Multi-Tier Trigger System (LOCKED 2026-05-14)

**Locked by:** Opus orchestrator decision, S69
**Extends:** Karar #5 (Skill System v2 / Shadow Echo) + Karar #7 (Resonance Altar)
**Conflicts:** NONE (canon-compatible extension)

## 4 Tier Trigger System

| Tier | Trigger | Chance | ICD | Echo Dmg | Source |
|---|---|---|---|---|---|
| T1 Commit-Beat | LMB Beat 3 (combo final) | 100% | 1.2 sn | 35% | Cross-class seçimi (free) |
| T2 Resonance Hit | Her LMB vuruşu | 15-25% (Altar scale) | 0.8 sn ayrı | 25% | Altar pasifleri (Karar #7) |
| T3 Empowered Skill | Belirli Q/E/R/F cast | 100% | Skill CD | 50% | Skill Evolution draft (Karar #5) |
| T4 Rift Proc Bond | 3 farklı Family Tag stack | 100% | yok | 100% + armor pen | Family Tag meta |

## Primary Skill Enhancement (Echo proc'unda primary buff)

| Tier | Primary Buff |
|---|---|
| T1 | Commit-Beat dmg +20% |
| T2 | Primary attack 1 Family Tag uygular (random) |
| T3 | Primary skill cyan VFX trail + cooldown -10% |
| T4 | Primary full crit + 50% armor pen |

Feedback loop: Echo proc → primary buff → easier Commit-Beat → more Echo.

## Universal 3-Beat Combo Foundation

| Beat | Profil |
|---|---|
| Beat 1 | Light, 0.2sn recovery, repositioning |
| Beat 2 | Medium, 0.3sn recovery, chain opener |
| Beat 3 (COMMIT-BEAT) | Heavy, 0.5sn recovery, knockback, ECHO TRIGGER |

**Rules:**
- Whiff penalty: Beat 3 miss → ICD still triggers
- Reset window: 0.8sn between beats → combo resets
- Dash-cancel: Beat 1-2 cancellable, Beat 3 LOCKED (commit cost)
- Class personality: Each class's Beat 3 = signature (Warblade ram, Ranger aimed, Shadowblade phase strike, etc.)

## Spawn Position (Karar #5 append)

| Skill Type | Spawn |
|---|---|
| Melee STRIKE | Target's position |
| Ranged STRIKE | **Player facing-relative front-flank, ±45°, ~24px offset** (REFINED 2026-05-14) |
| Zone | Cursor position |
| Buff | Player position |

## Phantom Visual

- Alpha 0.3 + cyan #00FFCC + 0.4 sn duration (canon)
- Tier color intensity:
  - T1: standard cyan
  - T2: lighter cyan + faint glow
  - T3: bright cyan + radial flash
  - T4: white-cyan + screen flash

## Damage Cap

- Max 2 Echo proc per enemy per frame (visual chaos prevention)
- Boss execute prohibited (canon, %50-70 burst only)

## MVP Implementation (25-day school deadline)

**Faz 1 (MVP):**
- T1 only
- Warblade 3-Beat Iron Combo
- Iron Combo Beat 3 → Elementalist Fireball Echo proc
- Primary enhancement T1 (+20% Commit-Beat dmg)
- Phantom shader + facing-relative spawn
- ICD 1.2 sn

**Faz 2 (post-MVP polish):**
- T2 (Altar Resonance pasifleri)
- T3 (Skill Evolution draft UI)
- T4 (Family Tag system + Rift Proc)
- Class-specific Commit-Beat personality polish

**MVP cost:** ~7-10 saat code + 1-3 gen

## Integration with Yol A (weapon decouple)

- Player primary = Yol A decouple (body silahsız + weapon ayrı + Unity attach)
- Phantom Echo = self-contained (weapon-baked OK, 0.4sn brief instance)
- Skill Evolution potential visual: T3 empowered skill cast → primary weapon sprite swap (örn. Iron Combo + Fireball Echo → Iron Combo Slam'in greatsword sprite'ı brief cyan glow shader applied)
- Tüm shader/transition Unity tarafı

## Karar #5 + #7 Append Notları

- **Karar #5 (Skill System v2 append):** "Ranged STRIKE phantom spawn = facing-relative front-flank (±45° from player facing direction), ~24px offset. Echo damage scaling 4-tier (T1-T4) per Karar #122."
- **Karar #7 (Resonance Altar append):** "Altar pasifleri T2 Resonance Hit trigger'ı scale eder: base %15 chance, max %25 chance Altar upgrades ile."

## Conflicts Resolved

- ~~Önceki Karar #122 önerisi (Rift Echo Universal Spawn 2-ghost SW+SE)~~ — REVOKED 2026-05-14, canon ile çakışıyordu
- Mevcut Karar #5/#7 cross-class spec'i KORUNDU, sadece tier system + spawn refinement extension

## Production Order

1. **Memory commit** (this file, sonra MEMORY/project_karar_122_echo_resonance.md formatına dönüştürülecek)
2. **rima-doc dispatch** (sonradan): MASTER_KARAR_BELGESI.md #122 entry + #5/#7 append
3. **Codex dispatch:** Unity 3-Beat combo state machine + Commit-Beat detection + phantom spawner + facing-relative spawn calculator
4. **PixelLab dispatch:** Elementalist Fireball anim (MVP — V2'de primary asset)

## Lock Status

LOCKED 2026-05-14, S69. Değişiklik için karar reset gerekli.
