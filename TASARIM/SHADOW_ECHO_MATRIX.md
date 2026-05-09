# RIMA Shadow Echo Matrix
# Tarih: 2026-05-09 | Sistem LOCKED, içerik DESIGN PASS PENDING

Bu dosya 50 Shadow Echo havuzunu ve 90 cross-class combo rating'ini tanımlar.
Sistem kararı: `TASARIM/SKILL_SYSTEM_v2.md` Karar 5

---

## SİSTEM ÖZET

- **10 sınıf × 5 Shadow Echo = 50 Echo**
- ~30 Echo mevcut sınıf skill'lerinden reuse (anim hazır)
- ~20 Echo yeni shadow-spesifik (anim üretilecek)
- 90 cross-class combo rating: Strong / Neutral / Weak

---

## SHADOW POZİSYON KURALI (LOCKED)

| Skill Tipi | Pozisyon | Örnek |
|---|---|---|
| Melee STRIKE | Hedefin üstünde | Drawn Edge, Iron Charge, Veil Strike |
| Ranged STRIKE | Player'ın yanında (~24px omuz arkası) | Hex Bolt, Pinning Shot, Fireball |
| ZONE | Hedef noktada (cursor/yer) | Bone Trap, Foul Wave, Frost Wall |
| Self-buff | Player'ın üstünde | Iaido Stance shadow |

**Görsel spek:** alpha 0.3 max | cyan tint #00FFCC | süre 0.4s | Z-sort spawn Y'sinde
**UI:** Ekran kenarı 1-frame icon + 12px "{Class} Echo!" text

---

## TYPE DAĞILIMI (Sınıf Başına 5 Echo Tahmini)

| Sınıf | Melee | Ranged | Zone | Buff |
|---|---|---|---|---|
| Warblade | 4 | 0 | 1 | 0 |
| Ronin | 4 | 0 | 0 | 1 |
| Shadowblade | 4 | 0 | 1 | 0 |
| Brawler | 5 | 0 | 0 | 0 |
| Ravager | 5 | 0 | 0 | 0 |
| Ranger | 1 | 3 | 1 | 0 |
| Gunslinger | 0 | 4 | 1 | 0 |
| Elementalist | 0 | 3 | 2 | 0 |
| Hexer | 0 | 3 | 2 | 0 |
| Summoner | 1 | 1 | 2 | 1 |

**Toplam:** 24 melee + 14 ranged + 10 zone + 2 buff = 50 Echo

---

## SHADOW ECHO HAVUZU (DESIGN PASS PENDING)

Aşağıdaki 50 Echo'nun her biri için tanımlanması gereken alanlar:
- **İsim** (genelde kaynak skill ismi)
- **Pozisyon tipi** (melee/ranged/zone/buff)
- **Anim** (mevcut reuse VEYA yeni)
- **Tag bırakır mı?** (Cross-Class Proc için)
- **Hasar/etki** (player'ın skill'ine ek)
- **Cooldown overlay** (skill ile sync mi, ayrı CD mi)

### Warblade (5 Echo)
- [ ] Iron Combo Slam (melee, anim reuse)
- [ ] Crippling Blow (melee, anim reuse)
- [ ] Sunder Mark (melee, anim reuse)
- [ ] Gravity Cleave (melee, anim reuse)
- [ ] Earthsplitter (zone, anim reuse)

### Elementalist (5 Echo)
- [ ] Fireball (ranged, anim reuse)
- [ ] Glacial Spike (ranged, anim reuse)
- [ ] Living Bomb (ranged, anim reuse)
- [ ] Frost Wall (zone, anim reuse)
- [ ] Solar Flare (zone, anim reuse)

### Shadowblade (5 Echo)
- [ ] Veil Strike (melee, anim reuse)
- [ ] Phase Step (melee, anim reuse)
- [ ] Backstab Mark (melee, anim reuse)
- [ ] Death Mark (melee, anim reuse)
- [ ] Smoke Veil (zone, anim reuse)

### Ranger (5 Echo)
- [ ] Pinning Shot (ranged, anim reuse)
- [ ] Sweep Volley (ranged, anim reuse)
- [ ] Marked Detonate (ranged, anim reuse)
- [ ] Bone Trap (zone, anim reuse)
- [ ] Predator's Mark (melee close, anim reuse)

### Ravager (5 Echo)
- [ ] Brutal Swing (melee, anim reuse)
- [ ] Bloodlust Strike (melee, anim reuse)
- [ ] Carnage Spin (melee, anim reuse)
- [ ] Wild Hack (melee, anim reuse)
- [ ] Bone Crack (melee, anim reuse)

### Ronin (5 Echo)
- [ ] Drawn Edge (melee, anim reuse)
- [ ] Quickdraw Slash (melee, anim reuse)
- [ ] Sōken-giri (melee, anim reuse)
- [ ] Crescent Arc (melee, anim reuse)
- [ ] Iaido Stance (buff, anim reuse — 0.5s static pose)

### Gunslinger (5 Echo)
- [ ] Dual Fire Burst (ranged, anim reuse)
- [ ] Quickdraw (ranged, anim reuse)
- [ ] Deadshot (ranged, anim reuse)
- [ ] Fan the Hammer (ranged, anim reuse)
- [ ] Smoke Grenade (zone, anim reuse)

### Brawler (5 Echo)
- [ ] Jab Combo (melee, anim reuse)
- [ ] Bully (melee, anim reuse)
- [ ] Crackjaw (melee, anim reuse)
- [ ] Cyclone Drive (melee, anim reuse)
- [ ] Curbstomp (melee, anim reuse)

### Summoner (5 Echo)
- [ ] Bone Spike (ranged, anim reuse)
- [ ] Raise Skeleton (zone — minyon spawn, anim reuse)
- [ ] Corpse Explosion (zone, anim reuse)
- [ ] Death Nova (zone, anim reuse)
- [ ] Bone Shield (buff, anim reuse)

### Hexer (5 Echo)
- [ ] Hex Bolt (ranged, anim reuse)
- [ ] Hexblast (ranged, anim reuse)
- [ ] Spitback (ranged, anim reuse)
- [ ] Pandemic (zone, anim reuse)
- [ ] Foul Wave (zone, anim reuse)

**Not:** Tüm 50 Echo şu an mevcut skill reuse. Eğer playtest "shadow çeşitliliği yetersiz" derse, sınıf başına 1-2 yeni shadow-spesifik Echo eklenir (toplam ~20 ek anim).

---

## COMBO MATRIX (90 Çift, DESIGN PASS PENDING)

Her cross-class çifti için Shadow Echo seçimine bağlı sinerji rating'i.
Format: **Ana Sınıf → Shadow Sınıf:** Strong / Neutral / Weak (gerekçe)

### Örnek Doldurma (Warblade Ana, Shadow Seçenekleri):

| Ana | Shadow | Rating | Gerekçe |
|---|---|---|---|
| Warblade | Hexer | **Strong** | Sunder + Hex stack birikim sinerjisi |
| Warblade | Ronin | **Strong** | Tension Echo + Verdict Ledger ritim eşleşir |
| Warblade | Elementalist | Neutral | Burn katmanı eklenir, ekstra sinerji yok |
| Warblade | Ranger | Neutral | Pierce ekstra hasar, taktik fark az |
| Warblade | Shadowblade | Weak | İkisi de melee burst, redundant |
| Warblade | Brawler | Weak | Combo overlap, hız ritmi çakışır |
| Warblade | Ravager | Weak | İkisi de Bleed-adjacent, redundant |
| Warblade | Gunslinger | Neutral | Range fark, mekanik birleşmiyor |
| Warblade | Summoner | **Strong** | Verdict Ledger + minyon proc'u sinerjisi |

**Hedef:** 90 çiftin her biri rating'lenecek. Hiçbir çift "must-take" olmamalı, hiçbir çift "useless" olmamalı.

**Doldurma planı:**
- Faz 1 (şimdi): Warblade × 9 (yukarıdaki örnek tamamlandı)
- Faz 2: Diğer 9 sınıfın Ana matrisi (81 ek satır)
- Faz 3: Playtest'te denge ayarlaması

---

## NEXT STEPS

1. Her sınıf için 5 Shadow Echo'nun mekanik detayı (hasar/CD/tag)
2. 90 çift combo rating tablosu doldur
3. Shadow render shader prototipi (Unity)
4. Phantom spawn logic (object pool, position rule)
5. UI anchor flash prefab
6. Particle aura 10 cross-class variant

**Sahiplik:** rima-design (mekanik) + rima-codex (Unity prototip) + rima-asset (UI flash + particle)
