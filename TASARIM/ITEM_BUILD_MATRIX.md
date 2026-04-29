# RIMA — ITEM BUILD MATRIX
> Full reference: Component → Combined → Legendary build paths
> 2026-04-12 | Tüm 10 class, 30 legendary, 11 combined, 7 component

---

## COMPONENT LİSTESİ (7 — Faz 1)

| Kısa | Component | Pasif | Ağırlık |
|------|-----------|-------|---------|
| IS | **Iron Shard** | +6% Phys Dmg | Phys class'lar |
| VF | **Void Fragment** | +6% Ability Power | AP class'lar |
| CL | **Chain Links** | +8% Armor | Tank/bruiser |
| SV | **Shadow Veil** | +6% Dodge | Mobility |
| BG | **Blood Gem** | +6% Life Steal | Sustain |
| RS | **Rift Stone** | +6% Crit | DPS |
| SE | **Soul Ember** | +8% Attack Speed | Speed/combo |

> Faz 2 eklentisi: Bone Dust (+15% Elite dmg) + Mana Shard (+1 Skill charge)

---

## COMBINED LİSTESİ (11 — Faz 1)

Tüm combined **genel** (class kilidi yok). Bazıları belirli legendary'leri daha iyi besler — bu afiniteyi Build Matrix'te göster.

**Craft:** Forge Tab 3 (visit başına 1 işlem)

| # | Formül | Sonuç | Stat Profili | Sinerji Efekti |
|---|--------|-------|-------------|---------------|
| A | IS + BG | **Vampiric Blade** | Phys + LS | Overkill → anlık full heal burst |
| B | VF + SV | **Phantom Weave** | AP + Dodge | Dodge → void burst explosion proc |
| C | RS + SE | **Frenzy Core** | Crit + AS | Crit → 0.5s haste |
| D | IS + CL | **Warlord's Plate** | Phys + Armor | Hit taken → +Rage/resource |
| E | IS + RS | **Rift Piercer** | Phys + Crit | RiftMark stack başına armor ignore |
| F | BG + SE | **Soul Tap** | LS + AS | Kill → skill charge |
| G | VF + RS | **Fracture Amp** | AP + Crit | RiftMark'lı düşmana +20% bonus dmg |
| H | SV + SE | **Ghost Step** | Dodge + AS | Dodge → phantom strike |
| I | CL + BG | **Iron Will** | Armor + LS | Skill kullan → kısa shield burst |
| J | VF + SE | **Surge Catalyst** | AP + AS | Her skill cast → 0.4s hareket hızı |
| K | VF + CL | **Arcane Bastion** | AP + Armor | Hasar al → AP kadarlık damage shield |

**Not:** J ve K AP class'lar için eklendi (Elem/Summoner/Hexer). Diğer class'lar da kullanabilir ama legendary sinerji AP class'larda.

---

## LEGENDARY BUILD MATRIX

### Nasıl Okunur

```
Legendary Adı (Build Anchor)
  Recipe   : Combined X + Combined Y → Legendary
  Path     : Component A + Component B + Component C + Component D
  Stat Core: Bu 4 componentten gelen toplam stat profili
  L-Efekt  : Legendary'nin özel efekti (MAP_ITEM_SYSTEM.md'den)
  Fill Slot: Kalan 3 slot için önerilen combined (zorunlu değil)
```

Legendary 1 slot kaplar. Kalan 3 slot: dilediğin combined/component.

---

## ── WARBLADE ──

### 1. Berserker's Gospel *(Rage management)*
```
Recipe   : Warlord's Plate (IS+CL) + Rift Piercer (IS+RS)
Path     : IS×2 + CL + RS
Stat Core: Phys×2 + Armor + Crit + rage on hit + RiftMark armor ignore
L-Efekt  : Fury/Bloodrage threshold'a girerken 2s invincibility + skill CD -%30
Fill Slot: Vampiric Blade (sustain) · Frenzy Core (daha fazla crit) · Iron Will (armor stack)
```

### 2. Crimson Covenant *(Aggression/sustain)*
```
Recipe   : Vampiric Blade (IS+BG) + Soul Tap (BG+SE)
Path     : IS + BG×2 + SE
Stat Core: Phys + LS×2 + AS + overkill heal + kill→skill recharge
L-Efekt  : LS cap kaldırılır. Overkill dmg → Rage. Her 10 kill → uzun CD skill sıfırlanır
Fill Slot: Frenzy Core (crit→haste sinerji) · Rift Piercer (armor ignore) · Warlord's Plate
```

### 3. Ironclad Edict *(Punish/counter)*
```
Recipe   : Warlord's Plate (IS+CL) + Iron Will (CL+BG)
Path     : IS + CL×2 + BG
Stat Core: Phys + Armor×2 + LS + rage on hit + shield on skill
L-Efekt  : Hasar alınca Rage gain ×3. Alınan hasarın %25'i counter olarak geri döner. Counter → Sunder Mark
Fill Slot: Rift Piercer (armor ignore on crit) · Vampiric Blade (overkill) · Frenzy Core
```

---

## ── ELEMENTALIST ──

### 4. Convergence Matrix *(Multi-element combo)*
```
Recipe   : Fracture Amp (VF+RS) + Surge Catalyst (VF+SE)
Path     : VF×2 + RS + SE
Stat Core: AP×2 + Crit + AS + bonus dmg on RiftMark + cast speed
L-Efekt  : 3 farklı elementten vuruş → fusion explosion. AP scaling +%15
Fill Slot: Phantom Weave (void burst+dodge) · Arcane Bastion (AP+armor) · Frenzy Core
```

### 5. Arcane Singularity *(Mono-element mastery)*
```
Recipe   : Phantom Weave (VF+SV) + Fracture Amp (VF+RS)
Path     : VF×2 + SV + RS
Stat Core: AP×2 + Dodge + Crit + void burst on dodge + bonus dmg
L-Efekt  : Tüm aktif skill'ler aynı element → hasar +%40. Element değişince 5s bekleme
Fill Slot: Surge Catalyst (cast speed) · Arcane Bastion (shield) · Ghost Step (dodge chain)
```

### 6. Mirror Cascade *(Summon/explosion)*
```
Recipe   : Arcane Bastion (VF+CL) + Surge Catalyst (VF+SE)
Path     : VF×2 + CL + SE
Stat Core: AP×2 + Armor + AS + damage shield + cast speed
L-Efekt  : Mirror Image +1 kopya. Kopya ölünce chain elemental explosion. Kopya sayısı → passive AP
Fill Slot: Phantom Weave (void burst) · Fracture Amp (crit+bonus dmg) · Iron Will
```

---

## ── SHADOWBLADE ──

### 7. Void Predator *(Burst)*
```
Recipe   : Phantom Weave (VF+SV) + Rift Piercer (IS+RS)
Path     : VF + SV + IS + RS
Stat Core: AP + Dodge + Phys + Crit + void burst + armor ignore
L-Efekt  : Stealth first hit → guaranteed crit + poison max stack. Stealth +1s
Fill Slot: Ghost Step (dodge chain) · Vampiric Blade (sustain) · Soul Tap (kill→recharge)
```

### 8. Hemorrhage *(Stacking DoT)*
```
Recipe   : Vampiric Blade (IS+BG) + Ghost Step (SV+SE)
Path     : IS + BG + SV + SE
Stat Core: Phys + LS + Dodge + AS + overkill heal + phantom strike
L-Efekt  : Poison stack'ler aktif skill ile bleed'e dönüşür. DoT composite hasar artar
Fill Slot: Soul Tap (on-kill sustain) · Frenzy Core (crit tetikleme) · Rift Piercer
```

### 9. Phantom Protocol *(Mobility loop)*
```
Recipe   : Ghost Step (SV+SE) + Frenzy Core (RS+SE)
Path     : SV + RS + SE×2
Stat Core: Dodge + AS×2 + Crit + phantom strike + haste on crit
L-Efekt  : Her dodge = 0.5s mini-stealth. Stealth içinde kill → stealth CD sıfırlanır
Fill Slot: Phantom Weave (AP+void burst) · Soul Tap (kill chain) · Vampiric Blade
```

---

## ── RANGER ──

### 10. Eagle's Requiem *(Precision)*
```
Recipe   : Rift Piercer (IS+RS) + Frenzy Core (RS+SE)
Path     : IS + RS×2 + SE
Stat Core: Phys + Crit×2 + AS + armor ignore + haste on crit
L-Efekt  : Focus max → sonraki ok guaranteed headshot (crit + bonus dmg)
Fill Slot: Soul Tap (kill sustain) · Vampiric Blade (LS) · Ghost Step (dodge)
```

### 11. Volley Tyrant *(Sustained DPS)*
```
Recipe   : Frenzy Core (RS+SE) + Soul Tap (BG+SE)
Path     : RS + BG + SE×2
Stat Core: Crit + AS×2 + LS + haste + kill→skill recharge
L-Efekt  : Multi-shot +2 ok. Her ok ayrı wound stack sayar. Wound stack → hasar artar
Fill Slot: Rift Piercer (armor ignore) · Phantom Weave (dodge) · Fracture Amp
```

### 12. Tether Sovereign *(CC/AoE)*
```
Recipe   : Phantom Weave (VF+SV) + Soul Tap (BG+SE)
Path     : VF + SV + BG + SE
Stat Core: AP + Dodge + LS + AS + void burst + kill→skill recharge
L-Efekt  : Tethering Arrow süresi ×2. Tethered düşmanlar hasar havuzu paylaşır
Fill Slot: Fracture Amp (AP+crit) · Ghost Step (dodge chain) · Iron Will
```

---

## ── RONIN ──

### 13. Void Resonance *(Tension burst)*
```
Recipe   : Rift Piercer (IS+RS) + Phantom Weave (VF+SV)
Path     : IS + RS + VF + SV
Stat Core: Phys + Crit + AP + Dodge + armor ignore + void burst
L-Efekt  : Max tension release = 3 phantom strike. Void Cleave cone +%30 hasar
Fill Slot: Frenzy Core (haste) · Vampiric Blade (sustain) · Soul Tap (kill chain)
```

### 14. Sheathed Storm *(Speed iaido)*
```
Recipe   : Frenzy Core (RS+SE) + Ghost Step (SV+SE)
Path     : RS + SV + SE×2
Stat Core: Crit + Dodge + AS×2 + haste + phantom strike
L-Efekt  : Draw hızı ×2. Sheath/draw döngüsü her 3 kez = bedava skill cast
Fill Slot: Rift Piercer (armor ignore) · Soul Tap (kill→recharge) · Phantom Weave
```

### 15. Cursed Edge *(Persistent tension)*
```
Recipe   : Vampiric Blade (IS+BG) + Rift Piercer (IS+RS)
Path     : IS×2 + BG + RS
Stat Core: Phys×2 + LS + Crit + overkill heal + armor ignore
L-Efekt  : Her hit tension decay dondurur. Max tension'da kalıcı. Max tension → bleed stack ekler
Fill Slot: Frenzy Core (haste) · Warlord's Plate (armor+rage) · Soul Tap
```

---

## ── GUNSLINGER ──

### 16. Overheat Protocol *(Controlled overheat)*
```
Recipe   : Warlord's Plate (IS+CL) + Frenzy Core (RS+SE)
Path     : IS + CL + RS + SE
Stat Core: Phys + Armor + Crit + AS + rage on hit + haste
L-Efekt  : Overheat = AoE explosion + Heat 50'ye reset (sıfırlanmaz artık)
Fill Slot: Vampiric Blade (sustain) · Rift Piercer (armor ignore) · Ghost Step
```

### 17. Rift Archer *(Mobility)*
```
Recipe   : Ghost Step (SV+SE) + Rift Piercer (IS+RS)
Path     : SV + SE + IS + RS
Stat Core: Dodge + AS + Phys + Crit + phantom strike + armor ignore
L-Efekt  : Rift Dash ×1.5 mesafe. Dash sonrası 2s tüm atışlar armor-piercing
Fill Slot: Frenzy Core (crit chain) · Soul Tap (kill recharge) · Phantom Weave
```

### 18. Chrome Dominion *(Heat accumulation)*
```
Recipe   : Frenzy Core (RS+SE) + Soul Tap (BG+SE)
Path     : RS + BG + SE×2
Stat Core: Crit + AS×2 + LS + haste + kill→skill recharge
L-Efekt  : Her 10 Heat = +%1 hasar (max +%15). Ricochet hit başına +5 Heat
Fill Slot: Rift Piercer (armor ignore) · Warlord's Plate (armor) · Vampiric Blade
```

> Not: Chrome Dominion ve Volley Tyrant (Ranger) aynı 4 komponente dayanıyor.
> Aynı "Crit+Speed+Sustain" stat temeliyle farklı class mekanikleri besleniyor — kasıtlı.

---

## ── RAVAGER ──

### 19. Ruin Eternal *(Infinite berserk)*
```
Recipe   : Warlord's Plate (IS+CL) + Vampiric Blade (IS+BG)
Path     : IS×2 + CL + BG
Stat Core: Phys×2 + Armor + LS + rage on hit + overkill heal
L-Efekt  : Berserk kill-gate CD reset limiti kaldırılır. Kill başına berserk +0.5s
Fill Slot: Frenzy Core (crit haste) · Iron Will (armor+shield) · Soul Tap
```

### 20. Warlord's Grip *(Mass aggro)*
```
Recipe   : Warlord's Plate (IS+CL) + Iron Will (CL+BG)
Path     : IS + CL×2 + BG
Stat Core: Phys + Armor×2 + LS + rage on hit + shield on skill
L-Efekt  : Taunted düşman başına hasar +%10 (max +%40). Taunt süresi ×1.5
Fill Slot: Vampiric Blade (sustain) · Rift Piercer (armor ignore) · Frenzy Core
```

### 21. Gore Tide *(Sustain berserk)*
```
Recipe   : Vampiric Blade (IS+BG) + Iron Will (CL+BG)
Path     : IS + CL + BG×2
Stat Core: Phys + Armor + LS×2 + overkill heal + shield on skill
L-Efekt  : Berserk'te hasar alınca flat heal. Berserk'te 1 kez ölmeden survive (run/1)
Fill Slot: Warlord's Plate (rage+phys) · Soul Tap (kill chain) · Frenzy Core
```

---

## ── BRAWLER ──

### 22. Knockout Theory *(Precision combo)*
```
Recipe   : Frenzy Core (RS+SE) + Warlord's Plate (IS+CL)
Path     : RS + SE + IS + CL
Stat Core: Crit + AS + Phys + Armor + haste on crit + rage on hit
L-Efekt  : Perfect timing window +0.15s. Perfect hit → +2 Charge. Kopmayan seri → hasar artar
Fill Slot: Rift Piercer (armor ignore) · Vampiric Blade (sustain) · Soul Tap
```

### 23. Unstoppable Mass *(Bulldozer)*
```
Recipe   : Warlord's Plate (IS+CL) + Soul Tap (BG+SE)
Path     : IS + CL + BG + SE
Stat Core: Phys + Armor + LS + AS + rage on hit + kill→skill recharge
L-Efekt  : Charge hızı ×2. Charge hit = 0.5s stun. Mesafeye göre hasar artar
Fill Slot: Frenzy Core (crit haste) · Iron Will (armor stack) · Rift Piercer
```

### 24. Iron Knuckle *(Sustained combo)*
```
Recipe   : Rift Piercer (IS+RS) + Warlord's Plate (IS+CL)
Path     : IS×2 + RS + CL
Stat Core: Phys×2 + Crit + Armor + armor ignore + rage on hit
L-Efekt  : Combo sayacı interrupt sonrası 3s korunur. Max combo threshold bonusları güçlenir
Fill Slot: Frenzy Core (haste) · Vampiric Blade (sustain) · Soul Tap
```

---

## ── SUMMONER ──

### 25. Lich Ascension *(Lich sustain)*
```
Recipe   : Arcane Bastion (VF+CL) + Phantom Weave (VF+SV)
Path     : VF×2 + CL + SV
Stat Core: AP×2 + Armor + Dodge + damage shield + void burst
L-Efekt  : Lich Form süresi ×2. Lich Form'da minyon drain → enemy drain
Fill Slot: Surge Catalyst (cast speed) · Fracture Amp (AP+crit) · Iron Will
```

### 26. Swarm Intelligence *(Minion army)*
```
Recipe   : Surge Catalyst (VF+SE) + Arcane Bastion (VF+CL)
Path     : VF×2 + SE + CL
Stat Core: AP×2 + AS + Armor + cast speed + damage shield
L-Efekt  : Minyon limit +2. Proximity minyon buff (+%10 hasar/komşu)
Fill Slot: Phantom Weave (dodge) · Fracture Amp (crit) · Ghost Step
```

### 27. Soul Nexus *(Sacrifice engine)*
```
Recipe   : Fracture Amp (VF+RS) + Surge Catalyst (VF+SE)
Path     : VF×2 + RS + SE
Stat Core: AP×2 + Crit + AS + bonus dmg on mark + cast speed
L-Efekt  : Her minyon ölümünde 5s +%15 hasar (max 3 stack). Sacrifice CD -%50
Fill Slot: Phantom Weave (dodge+void burst) · Arcane Bastion (shield) · Ghost Step
```

---

## ── HEXER ──

### 28. Mirror Abyss *(Reflection)*
```
Recipe   : Phantom Weave (VF+SV) + Ghost Step (SV+SE)
Path     : VF + SV×2 + SE
Stat Core: AP + Dodge×2 + AS + void burst + phantom strike
L-Efekt  : Empathy reflection %150. Reflection AoE patlama yapar
Fill Slot: Fracture Amp (AP+crit) · Arcane Bastion (shield) · Soul Tap
```

### 29. Hex Convergence *(Multi-hex)*
```
Recipe   : Fracture Amp (VF+RS) + Arcane Bastion (VF+CL)
Path     : VF×2 + RS + CL
Stat Core: AP×2 + Crit + Armor + bonus dmg + damage shield
L-Efekt  : 3 farklı hex aktifken hasar aura (passive, yakın düşmanlar hasar alır)
Fill Slot: Phantom Weave (dodge) · Surge Catalyst (cast speed) · Iron Will
```

### 30. Soul Debt *(Corruption escalation)*
```
Recipe   : Surge Catalyst (VF+SE) + Phantom Weave (VF+SV)
Path     : VF×2 + SE + SV
Stat Core: AP×2 + AS + Dodge + cast speed + void burst
L-Efekt  : Soul Bargain acceleration stack'leri kalıcı. Stack başına AP +%2 (run boyunca birikiyor)
Fill Slot: Fracture Amp (crit+bonus dmg) · Arcane Bastion (armor shield) · Ghost Step
```

---

## COMPONENT DEMAND MATRİKSİ

Hangi component kaç legendary build path'inde gerekli:

| Component | Legendary Sayısı | Kullanan Class'lar |
|-----------|-----------------|-------------------|
| **Iron Shard (IS)** | 13 | Warblade, Shadowblade, Ronin, Gunslinger, Ravager, Brawler |
| **Void Fragment (VF)** | 12 | Elementalist, Ranger(1), Ronin(1), Summoner, Hexer |
| **Soul Ember (SE)** | 12 | Shadowblade, Ranger, Ronin, Gunslinger, Brawler, Summoner, Hexer |
| **Chain Links (CL)** | 10 | Warblade, Gunslinger, Ravager, Brawler, Summoner, Hexer |
| **Rift Stone (RS)** | 10 | Warblade, Ranger, Ronin, Gunslinger, Elem, Summoner, Hexer |
| **Blood Gem (BG)** | 9 | Warblade, Shadowblade, Ranger(1), Ronin, Ravager, Brawler |
| **Shadow Veil (SV)** | 9 | Elem, Shadowblade, Ranger(1), Ronin, Summoner, Hexer |

**Sonuç:** IS ve VF en çekişmeli componentler — her ikisi de birden fazla class için kritik. Elite drop seçiminde "IS mi VF mi" kararı gerçek prioritize zorlar.

---

## COMBINED KULLANIM TABLOSU

Hangi combined kaç legendary'de kullanılıyor:

| Combined | Legendary Sayısı | Öne Çıkan Class'lar |
|----------|-----------------|---------------------|
| **Warlord's Plate (D)** | 8 | Warblade, Gunslinger, Ravager, Brawler |
| **Frenzy Core (C)** | 8 | Shadowblade, Ranger, Ronin, Gunslinger, Brawler |
| **Vampiric Blade (A)** | 7 | Warblade, Shadowblade, Ronin, Ravager |
| **Phantom Weave (B)** | 7 | Elem, Shadowblade, Ranger, Ronin, Summoner, Hexer |
| **Rift Piercer (E)** | 6 | Warblade, Shadowblade, Ronin, Gunslinger, Brawler |
| **Soul Tap (F)** | 6 | Ranger, Gunslinger, Ravager, Brawler |
| **Surge Catalyst (J)** | 5 | Elem, Summoner, Hexer |
| **Arcane Bastion (K)** | 5 | Elem, Summoner, Hexer |
| **Ghost Step (H)** | 5 | Shadowblade, Ronin, Gunslinger, Hexer |
| **Fracture Amp (G)** | 5 | Elem, Ranger, Summoner, Hexer |
| **Iron Will (I)** | 4 | Warblade, Ravager |

**Warlord's Plate ve Frenzy Core** en evrensel combined'lar — fiziksel build için Warlord, hız build için Frenzy. Her ikisi de 4 slot içinde "fill" olarak da değerli.

---

## CROSS-CLASS KURAL

Combined'lar class kilidi yok. Herkes her combined'ı kullanabilir.  
**Ama:** Legendary recipe spesifik combined gerektirir → o combined'ı takmak yerine "legendary için biriktir" kararı doğar.

Örnek: Shadowblade, Warlord's Plate'i ekleyebilir (IS+CL → Phys+Armor). Ama Warlord's Plate + Iron Will = Warlord's Grip legendary recipe'si → Ravager için optimal. Cross-class Shadowblade Warlord's Grip alabilir: işlevsel ama Ravager'a göre %70 verimli.

---

## ÖNERİLEN KURAL: "2 + 1 + slot"

Sağlıklı bir run itemizasyonu:
```
1 Legendary (Boss'tan veya craft)          [1 slot]
1 Ana combined (legendary tamamlayıcı)     [1 slot]
1 Utility combined (sustain/haste/shield)  [1 slot]
1 Component veya fill combined             [1 slot]
```
4 slot doldu, her slot işlevsel, legendary anchor build'i belirliyor.
