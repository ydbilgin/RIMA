---
name: combat-feel-research-combined
description: Bandit Knight + ReBlade research distilled into concrete hitstop/screenshake/slow-mo tuning matrix for RIMA combat feel.
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

# Combat Feel — Bandit Knight + ReBlade Combined Tuning Matrix

**Status:** Research findings, ready for Phase 2 combat feel sprint implementation.
**Sources:** `STAGING/research_bandit_knight.md`, `STAGING/research_reblade_death_spiral.md` (2026-05-16).
**Why:** RIMA's 64×64 chibi sprite limits pose-read. Combat feel must come from timing + audio + flash + freeze layering, not from per-pixel sprite detail.
**How to apply:** Phase 2 combat feel sprint. `BasicAttackProfile` ScriptableObject extends with `criticalHitstopMs`, `parryHitstopMs`, `criticalSlowMoMs`. `EnemyTelegraph` extended with white-flash 0.2-0.3s pre-attack tween.

## Tuning Matrix (per event)

| Event | Hitstop | Screen Shake | Slow-mo | Audio | Source |
|---|---|---|---|---|---|
| Normal hit | 40ms | hafif | yok | generic | Bandit Knight |
| Heavy hit | 80-100ms | orta | yok | weighted | Bandit Knight |
| Critical hit | 80ms + 50ms micro-freeze | yüksek | 50-80ms | crack/clang | ReBlade + Bandit Knight |
| Perfect parry | 50ms micro-freeze | flash | yok | distinct clang | ReBlade |
| Projectile rebound | 50ms micro | white flash | yok | rebound zing | ReBlade |
| Killing blow (elite/boss) | 50ms freeze | yüksek | 100ms | death cue | Bandit Knight |
| Brawler punch | 80ms + body 4-8px step-in | orta | yok | meaty thump | Bandit Knight |
| Brawler crit punch | 80ms + 50ms micro + step-in 6-10px | yüksek | 50ms | extra heavy | combined |

## 3 Lifted Mechanics (LOCK candidates for Phase 2)

### M1 — Projectile Rebound (Warblade / Brawler signature)
**What:** Düşman projectile active frame'inde timed parry → velocity reverse + allegiance flip "player".
**Why:** Karar #57 Counter Arketipleri (Warblade=absorb/break, Brawler=whiff-evade) somut mekaniğe bağlar.
**How to apply:**
- Warblade variant: absorb-reflect (geri yollar, full damage)
- Brawler variant: catch-and-throw (yakalar, manuel atar)
- White sprite flash 100ms confirms rebound
- Cost: ~30 line code per class, 0 art

### M2 — Parryable Sprite Flash (system-wide telegraph)
**What:** Heavy attack windup'ta enemy SR white/red flash 0.2-0.3s, hitbox active OLMADAN ÖNCE.
**Why:** 64×64 chibi pose-read sınırlı — bu art-cost-free telegraph compensation. Karar #65 3-Layer Feedback Hierarchy ile uyumlu (Normal/Commit/Break = white-flash/red-flash/screen-pulse).
**How to apply:**
- `EnemyTelegraph` script extend
- MaterialPropertyBlock white-fill tween, 2-frame
- Per attack tier farklı renk (light=hafif glow, heavy=full white, super=red pulse)
- Karar #66 boolean `hasInterruptArmor` ile sync (armored = farklı flash)

### M3 — Micro-Hitstop 0.05s (crit + perfect parry distinct)
**What:** Normal hit'lerden ayrı, crit + perfect parry'de `Time.timeScale = 0` 50ms.
**Why:** ReBlade somut sayı veriyor (50ms sweet spot). Karar #64 `ActionCommitProfile.hitstopMs` field zaten var — sadece bracket tuning ile crit branching gerekli.
**How to apply:**
- `BasicAttackProfile.criticalHitstopMs = 50`
- `BasicAttackProfile.parryHitstopMs = 50`
- Distinct SFX bank: `crit_clang_*`, `parry_clang_*` (general hit bank'tan ayrı)

## Brawler Step-In (Bandit Knight insight)
64×64'te yumruğun reach'i kısa → impact frame'inde body 4-8px forward translate = "step-in" feel.
- Crit version: 6-10px translate
- Movement is animator-driven (Animator parameter `attackStepInPx`)
- Bandit Knight anti-pattern: light arcade tap → RIMA anti-bandit: weight over speed

## Anti-patterns (what NOT to copy)

**Bandit Knight reject:**
- Light arcade hit feel
- Cute/comedic tone overlay (RIMA Fractured Epic vs Bandit Knight comedic-stealth)
- Soft rounded slash arcs

**ReBlade reject:**
- 3D camera control / free-look / behind-camera dodge / vertical attacks — RIMA 2D top-down scope dışı
- Stealth pacing (RIMA Hades-style rapid room flow)
- Multi-button parry timing (RIMA 1-button dash + per-class counter)

## Cross-links
[[bandit-knight-research]] [[reblade-research]] (these were merged here; standalone files NOT created)
[[combat-architecture]] [[anchor-selections-s43]] [[8dir-mirror-production-strategy]]
