---
name: juice-features-v1
description: "Top 10 small-but-pleasing juice features for RIMA (footprints, dust kick, ember motes, etc.) — Phase 2 polish scope."
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

# Juice Features V1 — Top 10 for Phase 2 Polish

**Status:** Backlog (not LOCKED). Brainstorm from Bandit Knight + Hades + HLD reference + research takeaways. User to pick final cut at Phase 2 entry.

**Why:** RIMA's "Fractured Epic" tone (vivid + dramatic, not grimdark) demands constant moment-to-moment delight at the micro level. At 64×64 top-down, feet + ground reactions are direct camera-readable — high ROI.
**How to apply:** Phase 2 combat feel sprint + Phase 3 polish. Each item is Animator + ParticleSystem + simple shader; asset cost minimal.

## Top 10 by value/cost ratio

| # | Feature | Cost | Visibility | Source/Reference |
|---|---|---|---|---|
| 1 | **Footprints per surface** (sand/dirt/snow/blood/wet) | Düşük | Yüksek (top-down feet direct) | Bandit Knight insight |
| 2 | **Dust kick** koşmaya başlarken + landing'de | Çok düşük | Yüksek | Hades signature |
| 3 | **Skill cooldown sweep + ready-glow** | Düşük | Yüksek (UX must-have) | Modern roguelite standard |
| 4 | **Coin bounce + magnetism** | Çok düşük | Yüksek | Hades dopamine signature |
| 5 | **Hit number weight scaling** (büyük dmg = büyük pop, yavaş fade) | Düşük | Yüksek | Combat feel |
| 6 | **Critical hit slow-mo 50ms** | Çok düşük | Çok yüksek | Bandit Knight + ReBlade combined |
| 7 | **Floating embers in rift areas** | Düşük | Yüksek | Fractured Epic tone signature |
| 8 | **Resource aura glow** (rage/heat/tension build-up) | Orta | Yüksek | Class identity |
| 9 | **Wall crack persist on heavy hits** | Düşük | Yüksek | Karar #143 paint integration uyumlu |
| 10 | **Düşük HP vignette + heart pulse** | Düşük | Yüksek | Hades danger feel |

## Other candidates (deferred, V2)

**Environmental reaction:**
- Grass bend under feet + spring back
- Su sıçraması + ripple (sığ su)
- Yapraklar (autumn biome) arkanda dönerek savrulur
- Örümcek ağı parçalanma (eski oda geçerken)
- Cape/cloak hareketle hafif geri savrulur

**Combat micro:**
- Floor tile crack ground slam çevresinde
- Particle ceiling fall (boss heavy hit'te toz/taş)
- Sword trail / motion arc (Karar #59 slash trail OK)
- After-image (Shadowblade dash)

**Player state:**
- Idle breath cycle (göğüs hafif kalk-in)
- Eye glow on rage (Ravager/Warblade signature)

**Loot:**
- Chest 2-stage animation (kapak kalkar → içerik glow)
- Rare drop slow-mo 100ms + sparkle ring
- Pickup pulse light + sfx

**Atmospheric (passive):**
- Dust motes in light shafts
- Distant lightning flash (boss intro)
- Bird flyover silhouette (hub)
- Crackling fire embers (torch/forge)

## Pixel art uygunluk notları
- **Çok küçük detay (eye blink, sweat drops)** 64×64'te kaybolur → SKIP
- **Reflection in water** expensive shader → SKIP V1
- **Footprints + dust kick** kamera top-down olduğu için **PERFECT VISIBILITY** → ⭐⭐⭐⭐⭐
- **Floating embers** "Fractured Epic" tone'una BIRE BIR uyar (rift theme)

## Cross-links
[[combat-feel-research-combined]] [[visual-quality]] [[feel-toggles]] [[karar-143-layered-pipeline]]
