# 04 — Kilitli Stat Modeli (Stats sekmesi için)

## Model (Opus kilit)
`ClassStatProfile` (SO) → `ClassStatRuntime` (kopya, asset kirlenmez) → `DamagePacket` → `DamageCalculator` → Health.
**Phys/AP ayrı stat** (canon). damageMult sadece debug override.

**Formül:** `final = baseDamage × (physPower veya abilityPower)/100 × cap(identityBuild,3.0) × cap(situational,2.0) × debugMult × incomingMult`. Overflow (>3.0) → posture (demo'da log).

## Stats sekmesi slider'ları (canlı uygula)
maxHP · physPower · abilityPower · attackSpeedMult · moveSpeed · debugGlobalDamageMult · (düşman HP/damage ×). Reset/Preset save-load/Build export butonları.

## 10 class değerleri (v0.1)
| Class | Tip | maxHP | physPwr | abilityPwr | atkSpd× | move |
|---|---|---|---|---|---|---|
| Warblade | Phys | 115 | 110 | 70 | 0.90 | 4.35 |
| Elementalist | AP | 80 | 65 | 125 | 1.00 | 4.45 |
| Shadowblade | Phys | 80 | 95 | 80 | 1.35 | 4.75 |
| Ranger | Phys | 85 | 105 | 80 | 1.05 | 4.65 |
| Ravager | Phys | 125 | 115 | 65 | 0.85 | 4.35 |
| Ronin | Phys | 85 | 100 | 75 | 1.00 | 4.60 |
| Gunslinger | Phys | 85 | 100 | 80 | 1.25 | 4.75 |
| Brawler | Phys | 130 | 95 | 65 | 1.20 | 4.45 |
| Summoner | AP | 75 | 60 | 105 | 0.95 | 4.40 |
| Hexer | AP | 75 | 60 | 115 | 0.90 | 4.35 |

## Kritik notlar
- HP gerçek kaynağı `PlayerStats.maxHP` (Health.cs değil) — slider oraya yazmalı
- attackSpeedMult sadece cooldown/comboWindow'a (commitment'a DOKUNMA)
- moveSpeed farkı dar (4.35-4.75) — final ağırlık animasyondan
