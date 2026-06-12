# 02 — Gemini Genre Balance Research (roguelite/ARPG normları)

## Minimal stat seti = 4
`maxHP` · `moveSpeed` · `attackSpeed`(çarpan) · `damageMult`(birleşik)
- Gemini: attackPower/magicPower ayrımını terk et — hasar türü = yetenek etiketi (Melee/Magic/Ranged tag), stat değil (Hades/Brotato).
- crit/armor = taban stat değil, **item katmanı** (Dead Cells, RoR2).
> NOT: Bu NLM canon (Phys+AP ayrı stat) ile ÇELİŞİYOR — ChatGPT/Opus çözecek.

## Arketip → oranlar (base=100)
| Arketip | Örnek | HP | Hasar× | Hız | AtkSpd× | Crit |
|---|---|---|---|---|---|---|
| Tank | Brawler | 150 | 0.90 | 0.85 | 0.80 | 0% |
| Bruiser | Warblade/Ravager | 120 | 1.00 | 0.95 | 0.90 | 5% |
| Mage | Elementalist/Hexer | 70 | 1.30 | 0.95 | 1.00 | 5% |
| Assassin | Shadowblade/Ronin | 65 | 1.20 | 1.15 | 1.50 | 15% |
| Ranged | Ranger/Gunslinger | 80 | 1.10 | 1.05 | 1.10 | 10% |

## Attack speed vs damage (DPS pariteli örnek, hedef ~133 DPS)
| Sınıf | Taban Hasar | Atk/s | DPS |
|---|---|---|---|
| Warblade (ağır) | 100 | 1.33 | ~133 |
| Shadowblade (hızlı) | 25 | 5.33 | ~133 |
AtkSpeed çarpan aralığı: yavaş 0.7-0.9× · normal 1.0× · hızlı 1.3-1.8× · çok hızlı 2.0-3.0×.
Hızlı sınıflar on-hit (zehir/kanama) hızlı biriktirir → yavaşlara doğuştan AoE/Stagger ver.

## Damage formülü — Additive Multiplicative (Hades/Brotato)
`FinalDamage = (baseDamage + flatItem) × (1 + totalBonus%/100)`
Örn: (25+5)×(1+0.40)=42. Sayılar <1000, okunur, dengelenebilir. Exponansiyel (1.15^level) → KAÇIN.

## Base-100 referans modeli
En iyi pratik. Her class'a `StatProfile` (5 çarpan), engine çarpan uygular. Item/difficulty scaling base=100 üzerinden, tüm class'larda otomatik orantılı.
