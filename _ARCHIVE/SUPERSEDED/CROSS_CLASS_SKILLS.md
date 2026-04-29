# CROSS-CLASS SKILL SİSTEMİ
> Oluşturulma: 2026-04-16 | v3 — Ghost Attack Option C kilitlendi (2026-04-17)

---

## KURAL
- Her class'ın **8 exportable skilli** var (total 80 cross-class skill)
- Bir runda max **2 cross-class slot**
- Run içi sunum: pool'dan random **3 teklif** → 1 seçim × 2 kez
- **Kısıtsız:** Her class her cross-class skilli seçebilir
- Cross-class skiller alıcı class'ın resource'unu üretmez/kullanmaz — CD tabanlı veya pasif
- Kullanılınca: **Ghost VFX** (kaynak class'ın rengiyle hayalet görüntüsü, 0.6s)

---

## GHOST ATTACK SİSTEMİ — OPSİYON C KİLİTLENDİ (2026-04-17)

### Trigger Noktaları (İKİSİ BİRDEN)
1. **Cross-class skill havuzu** (80 skill, 2 slot/run) → aktive edilince / pasif proc edilince
2. **Z/X secondary skilleri** → kullanıldığında

Her ikisi de "farklı classtan güç ödünç alma" anı. Ghost = o anın görsel kilidi.

### Ghost Attack Animasyon Spec
- **12 frame**, 2-segment (6f windup→peak + 6f peak→settle)
- **4 yön** (S/N/W/E)
- **Kaynak class karakteri** kendi imza saldırısını yapıyor — 0.6s içinde tamamlar
- Sprite **nötr üretilir** → Unity runtime'da MaterialPropertyBlock ile class rengi tint
- Material: Additive blend, alpha 0.5 → 0 (fade out)

### Class Renkleri
| Class | Renk | Ghost Attack İmzası |
|-------|------|---------------------|
| Warblade | `#66AAFF` | Geniş yatay kılıç sweep |
| Elementalist | `#FF6600` | Çift el thrust, enerji patlaması |
| Shadowblade | `#9933CC` | Çapraz X dual-blade kesiş |
| Ranger | `#44CC44` | Hızlı yay çek-bırak |
| Ravager | `#FF3322` | Yukarıdan aşağı balta slam |
| Ronin | `#FFFFFF` | Iaido kın çekişi, tek horizontal kesik |
| Gunslinger | `#FFB800` | Quick-draw, silah omuz hizasında ateş |
| Brawler | `#FF8800` | Rising uppercut |
| Summoner | `#22FF88` | Aşağı-yukarı çağırma jesti |
| Hexer | `#CCFF00` | Parmak lanet fırlatması |

**Tam üretim spec:** `GUIDES/GHOST_ATTACK_SPEC.md`

---

## WARBLADE → export (ghost renk: `#66AAFF` soğuk mavi)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Iron Fragment** | Pasif | Her vuruşta %20 şans: hedef 0.4s micro-stagger |
| **Rage Infusion** | Pasif | Hasar aldığında class resource +8 |
| **Sunder Pulse** | Pasif | Skill kullanımı sonrası en yakın düşman 3s savunma -%20 |
| **Iron Will** | Pasif | HP %40 altındayken alınan hasar -%15 |
| **Cleave Echo** | Aktif (CD 8s) | Önde 2m yay: çevredeki düşmanlara 40 flat hasar |
| **Momentum Slam** | Pasif | Dash sonrası 0.5s içinde vuruş: +50% hasar |
| **Battle Hardened** | Pasif | Her oda başında 1 vuruş absorbe eden demir zırh katmanı |
| **Execution Sense** | Pasif | HP %25 altındaki düşmanlara +20% hasar |

---

## ELEMENTALİST → export (ghost renk: `#FF6600` ateş turuncu)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Ember Touch** | Pasif | Melee vuruşlar 2s ateş DoT (8/sn) |
| **Frost Step** | Pasif | Dash sonrası buz patı (1.5m, 1.5s, %35 slow) |
| **Static Arc** | Pasif | Skill kullanımından 0.3s sonra en yakın düşmana 25 lightning |
| **Arcane Surge** | Pasif | 4 skill kullanımından sonra: sonraki skill +35% hasar (sayaç) |
| **Mana Shield** | Aktif (CD 12s) | 2s: gelen hasarın %40'ını engeller |
| **Elemental Weakness** | Pasif | DoT uygulanan düşman tüm hasardan +10% alır |
| **Blink Reflex** | Pasif | HP %20 altına düşünce: otomatik 2m blink (45s CD) |
| **Overcharge** | Pasif | Üst üste 5 skill kullanımında 6. skill CD'siz |

---

## SHADOWBLADE → export (ghost renk: `#9933CC` void mor)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Shadow Linger** | Pasif | Dash +2m mesafe, +0.15s iframe |
| **Bleeding Edge** | Pasif | Crit vuruşlar 3s bleed (12/sn) |
| **Void Trace** | Pasif | Düşman öldürünce 1.5s görünmezlik |
| **Phase Echo** | Pasif | Dash sonrası 1s içinde alınan hasar -%25 |
| **Shadow Mark** | Aktif (CD 10s) | Hedefe 5s shadow mark: işaretliye arka taraftan vuruş +40% |
| **Rift Sense** | Pasif | Ekran dışındaki düşmanların konumu HUD'da gösterilir |
| **Void Siphon** | Pasif | Görünmezlik kırılınca: class resource +15 |
| **Phantom Step** | Pasif | Dash'te düşmandan geçilirse: o düşman 0.5s confusion (saldırmaz) |

---

## RANGER → export (ghost renk: `#44CC44` doğa yeşili)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Kite Bonus** | Pasif | 5m+ mesafede: hasar +15% |
| **Hunter's Mark** | Pasif | Skill kullanımında en yakın düşmana 5s mark (+12% hasar) |
| **Trap Sense** | Pasif | 6m+ düşmanlardan alınan hasar -%18 |
| **Eagle Eye** | Pasif | Crit şansı +8% (flat) |
| **Tactical Reload** | Aktif (CD 15s) | Tüm skill CD'leri -2s |
| **Predator's Focus** | Pasif | Aynı düşmana 3. ardışık saldırı: +30% hasar |
| **Terrain Read** | Pasif | Oda başında: düşmanların initial pozisyonları 2s görünür |
| **Arrow Volley Echo** | Aktif (CD 14s) | 3m önde küçük yağmur: 3 düşmana 30 hasar |

---

## RAVAGER → export (ghost renk: `#FF3322` kan kırmızı)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Bloodfuel** | Pasif | Alınan hasarın %8'i lifesteal |
| **Primal Hunger** | Pasif | Class resource %50+ iken hasar +12% |
| **War Cry Fragment** | Aktif (CD oda başı) | 2s: düşmanlar sana yönelir, savunma +30% |
| **Pain to Power** | Pasif | Hasar alınca 3s içindeki saldırı +20% hasar |
| **Berserker's Edge** | Pasif | HP %50 altında: hareket hızı +10% |
| **Wound Frenzy** | Pasif | Bleed uygulanmış düşmana vuruş: bleed süresini +1s uzatır |
| **Undying Rage** | Pasif | Ölümcül darbe alınca 1 kez: 1 HP ile kal (oda başına 1 kez) |
| **Savage Instinct** | Pasif | 3 üst üste vuruş sonrası: sonraki vuruş +35% hasar |

---

## RONİN → export (ghost renk: `#FFFFFF` saf beyaz)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Still Mind** | Pasif | 0.6s hareketsiz: sonraki skill +35% hasar |
| **Afterimage Decoy** | Pasif | Dash'te 1 hit absorbe eden afterimage (2s) |
| **First Blood** | Pasif | Odada ilk skill kullanımı: guaranteed crit |
| **Void Cut** | Pasif | Crit vuruşlar kısa (0.2s) hitstop yaratır (juice) |
| **Draw Reflex** | Pasif | Hasar alınca 0.3s içinde saldırırsan: +25% hasar |
| **Tension Transfer** | Pasif | Dash kullanımı: bir sonraki saldırıya +20% hasar stacks (max 3) |
| **Clean Strike** | Pasif | Tek hedefe saldırırken (başka düşman 3m içinde yok): +18% hasar |
| **Phantom Blade** | Aktif (CD 12s) | Önde 4m void kesim: %80 hasar, geçilen düşmanlar 0.3s stun |

---

## GUNSLİNGER → export (ghost renk: `#FFB800` sıcak sarı)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Trick Shot** | Pasif | Her 3. skill kullanımında sonraki saldırı en yakın 2. hedefe sekiyor |
| **Quick Reload** | Pasif | Düşman öldürünce tüm skill CD'leri -1.5s |
| **Muzzle Flash** | Pasif | Skill kullanımı sonrası 0.8m AoE 15 flat hasar |
| **Suppressing Fire** | Pasif | Aynı hedefe 3 saniye içinde 2 skill isabet: hedef 1.5s slow |
| **Hip Fire Reflex** | Pasif | Dash sırasında alınan hasar -%30 |
| **Overheated** | Pasif | 5 saniye içinde 3+ skill kullanılırsa: 4. skill CD'siz |
| **Ricochet Rounds** | Pasif | Projectile skilleri: ilk hedeften sekip 1 ek hedefe %50 hasar |
| **Last Stand Shot** | Aktif (CD 20s) | Ani geri çekilme (2m) + tek %200 hasar atış |

---

## BRAWLER → export (ghost renk: `#FF8800` turuncu amber)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Rhythm Strike** | Pasif | Her 4. ardışık vuruşta: o vuruş +45% hasar |
| **Momentum Burst** | Pasif | Dash sonrası 0.4s içinde saldırı: +55% hasar |
| **Counter Instinct** | Pasif | Hasar aldıktan 0.5s içinde saldırı: +30% hasar |
| **Ground Pound Echo** | Aktif (CD 10s) | Zeminde 2m şok dalgası: yakın düşmanları 0.5s iter |
| **Combo Memory** | Pasif | 3 saniye içinde 4 ayrı skill kullanılırsa: sonraki skill CD'siz |
| **Iron Fist** | Pasif | Melee range'deki (1.5m) düşmanlara hasar +12% |
| **Juggle Master** | Pasif | Havada/knockup'taki düşmana vuruş: +40% hasar |
| **Adrenaline Rush** | Pasif | Skill kill'den sonra 3s: hareket hızı +20%, CD -0.5s/sn |

---

## SUMMONER → export (ghost renk: `#22FF88` nekro yeşil)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Soul Shard** | Pasif | Öldürünce %15 şans: 4s ruh çağrılır (basit melee) |
| **Grave Pact** | Pasif | Oda başına 1 kez: ilk ölümü engeller, %30 HP ile devam |
| **Undead Grit** | Pasif | Max HP +18% |
| **Bone Wall** | Aktif (CD 18s) | Önde 1.5m bariyer: 5s sürer, düşmanları bloklar |
| **Minion Surge** | Pasif | Skill her isabet ettiğinde: aktif minyon/tur varsa +10% hasar |
| **Death Pulse** | Pasif | Düşman öldürünce 1m çevresine 20 hasar AoE |
| **Soul Drain** | Pasif | Öldürdüğün her düşman: 1.5s içinde bir sonraki skill +8% hasar (max 3 stack) |
| **Corpse Step** | Pasif | Oda içinde her düşman ölümünde: kısa hız artışı (0.8s, +15%) |

---

## HEXER → export (ghost renk: `#CCFF00` hasta sarı)

| İsim | Tür | Efekt |
|------|-----|-------|
| **Wither Touch** | Pasif | Senin DoT'ların düşmanın hareket hızını -%20 azaltır |
| **Doom Sight** | Pasif | HP %30 altındaki düşmanlara +10% hasar; HP barları duvar üzerinden görünür |
| **Hex Leech** | Pasif | Skill isabet ettirince: HP +4 |
| **Curse Mark** | Pasif | Skill kullanımında: hedefe 4s curse (debuff süreleri +25% uzar) |
| **Plague Carrier** | Pasif | Senin DoT'ların bitişik düşmanlara %30 oranında zincir yapar |
| **Psychic Drain** | Pasif | Düşman hasar verince (sana): CD'lerin 0.3s azalır |
| **Entropy** | Pasif | Her saldırı: hedefe -2% savunma yığar (max -%20, 8s süre) |
| **Fragile Curse** | Aktif (CD 16s) | Hedefe 5s kırılganlık: ilk hasar %200 — sonra normal |

---

## SUNUM SİSTEMİ

```
Oda temizlenir
      ↓
Pool'dan 3 random cross-class skill teklif (farklı class'lardan garantili)
      ↓
Oyuncu seçer → slot 1 doldu
      ↓
Orta ilerlemede (5. oda veya mini-boss sonrası) slot 2 teklifi
      ↓
Run boyunca 2 cross-class skill aktif
```

**Not:** Aynı class'tan ikinci skill almak mümkün değil (variety zorunlu).

---

## KOMBO ÖRNEKLERİ

| Ana Class | Cross-Class Kombo | Sinerji |
|-----------|-------------------|---------|
| Warblade | Void Trace + Grave Pact | Kill → görünmez, ölüm koruması |
| Shadowblade | Frost Step + Predator's Focus | Her dash buz + aynı hedefe 3. vuruş bonusu |
| Ranger | Momentum Burst + Trick Shot | Tactical Roll → shot = burst + sekme |
| Gunslinger | Still Mind + Eagle Eye | 0.6s dur → +35% + %8 crit = sniper moment |
| Ravager | Undying Rage + Grave Pact | İki katmanlı ölüm koruması |
| Brawler | Rhythm Strike + Juggle Master | 4. vuruş + air target = %85 bonus |
| Hexer | Plague Carrier + Wither Touch | DoT zincir yayar + slow = kite loop |
| Summoner | Soul Drain + Death Pulse | Kill → AoE + hasar stack → zincirleme |

---

*Onaylanınca MASTER_KARAR_BELGESI.md #22 olarak işlenir.*
