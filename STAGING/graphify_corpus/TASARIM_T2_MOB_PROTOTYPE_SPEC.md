---
status: LOCKED
faz: 1
tarih: 2026-05-14
ozet: "3 T2 mob spec ve 17 tuning Q (Kararlar #82+84)"
---
# T2_MOB_PROTOTYPE_SPEC.md
**LOCKED 2026-05-13 -- Karar #82 + #84 -- Faz 1 Combat Expansion**

---

## Niyet

Faz 1 combat expansion kapsaminda 3 adet T2 mob prototipi tanimlanmaktadir. Bu dokuman, her mob icin rol, davranis agaci, T1 base attack, T2 ability, counterplay ve grup kompozisyon bilgilerini standart 6 alan formatiyla belgelemektedir. Sayisal degerler playtest tuning asamasinda ayarlanmak uzere placeholder olarak birakilmistir.

Seckilen 3 mob -- Shard Walker, Penitent Bruiser, Fracture Imp -- Faz 1 arena butcesiyle sinirlandirilmistir: normal odada max 2 T2 mob, elite odada max 3 T2 mob. T3 yetenekler Faz 1'de tamamen devre disi. Grup kompozisyon kurallari Karar #61 (mob infighting yok) ve Karar #84 (butce limitleri) ile uyumludur.

---

## Tech / Balance Constraints

| Alan | Deger |
|---|---|
| Faz | 1 (T3 disabled) |
| Normal room T2 cap | 2 |
| Elite room T2 cap | 3 (T3 disabled = 3 T2 only) |
| Boss room T3 | disabled Faz 1 |
| Mob infighting | HAYIR (Karar #61) |
| Bruiser aura | faction-blind, %50 heal azaltma, 3m radius |
| Sayisal degerler | placeholder + playtest tuning |

---

## 1. Shard Walker -- Crystal Bloom

### 6 Alan

**1. Role + Stats**
- Role: Skirmisher (alan kontrol, pozisyon dayatma)
- Size class: 64px
- HP: [placeholder: 80 -- playtest tuning]
- Speed: [placeholder: 2.5 tile/s -- playtest tuning]
- Base attack range: [placeholder: 1.5 tile -- playtest tuning]
- Armor: none
- XP value: [placeholder: 15 -- playtest tuning]

**2. Behavior Tree**
- States: Idle / Aware / Chase / Telegraph_T2 / Attack_T1 / Cooldown / Hurt / Death
- Idle -> Aware: player enter agro radius [placeholder: 6 tile]
- Aware -> Chase: line-of-sight confirmed
- Chase -> Attack_T1: player range <= [placeholder: 1.5 tile]
- Chase -> Telegraph_T2: T2 cooldown ready AND player range <= [placeholder: 4 tile] (AOE range)
- Telegraph_T2 -> cast_t2: after telegraph duration
- Attack_T1 / cast_t2 -> Cooldown: attack complete
- Cooldown -> Chase: cooldown elapsed
- Any state -> Hurt: damage received (interrupt Telegraph_T2 cancels cast)
- Any state -> Death: HP <= 0

**3. T1 Base Attack**
- Cooldown: [placeholder: 1.8s -- playtest tuning]
- Telegraph: [placeholder: 300ms -- playtest tuning] (melee windup flash)
- Damage: [placeholder: 10 -- playtest tuning]
- Hitbox: melee arc [placeholder: 1 tile radius, 90 deg front]
- Animation slot: attack_t1

**4. T2 Ability -- Crystal Bloom**
- Cooldown: [placeholder: 12s -- playtest tuning]
- Telegraph: [placeholder: 1.4s -- playtest tuning] ground glow (kristal cyan pulse, belirgin)
- Cast animation: cast_t2
- Effect: yerden kristal dikenleri filizlenir, AOE zone olusturur
- AOE radius: [placeholder: 2.5 tile -- playtest tuning]
- Duration: [placeholder: 5s -- playtest tuning]
- Damage (tick): [placeholder: 8/s -- playtest tuning] (overlap tick)
- Visual cue (active): AOE outline pulse (cyan)
- Visual cue (end): 0.3s fade, kristal kiriliyor parcacik efekti
- Note: kristal zone pozisyon denial -- oyuncu rota degistirmek zorunda

**5. Counterplay**
- Dodge i-frame window: [placeholder: 0.5s -- playtest tuning] (dodge slayd kristal zone gecisi)
- Parry: T1 melee parry edilebilir; T2 cast parry etmez (alan efekti)
- Safe zone: kristal AOE disinda kalmak (zone kenar belirgin gorusel)
- Alternatif: kristal aktifken uzak mesafe varsa diger mob'a focus, kristal bittikten sonra Shard'a don
- Telegraphın interrupt edilmesi: Shard'a hasar verilirse T2 cast iptal (Hurt interrupt)

**6. Group Composition**
- Normal room: max 2 Shard Walker (T2 cap = 2)
- Elite room: max 3 (kombo ile kullanim -- bkz. Group Composition Matrix)
- Synergy: Bruiser aura + Shard kristal zone = oyuncu hem heal azalmis hem hareket kisitli
- Synergy: Fracture Imp summon + Shard zone = imp'lar zonedan drive eder, oyuncu sikisir
- Priority hedef: Shard Walker orta oncelik -- zone aktifken diger moblar ileri iter

### Animation Slots
- idle / walk / telegraph_t2 / attack_t1 / cast_t2 / hurt / death

### Telegraph Spec
- T2 pre-cast: [placeholder: 1.4s -- playtest tuning] ground glow (kristal cyan)
- Active: AOE outline pulse (cyan, [placeholder: 0.5s -- playtest tuning] interval)
- End: 0.3s fade + kiriliyor parcacik

### Open Tuning Questions
1. Crystal Bloom AOE radius -- 2m odak mi 4m avoid mi?
2. Telegraph suresi 1.2s / 1.8s -- quick reaction tier?
3. Hurt interrupt T2 cast -- kolayca kesilebiliyor mu yoksa daha uzun interrupt window mi?

---

## 2. Penitent Bruiser -- Self-Mortification

### 6 Alan

**1. Role + Stats**
- Role: Tank / Brawler (yakin, guclu vuruslar, alan heal debuff)
- Size class: 64px
- HP: [placeholder: 150 -- playtest tuning]
- Speed: [placeholder: 1.8 tile/s -- playtest tuning]
- Base attack range: [placeholder: 1.2 tile -- playtest tuning]
- Armor: [placeholder: %15 damage reduction -- playtest tuning]
- XP value: [placeholder: 25 -- playtest tuning]

**2. Behavior Tree**
- States: Idle / Aware / Chase / Telegraph_T1 / Attack_T1 / Telegraph_T2 / Self_Mortification / Buff_Active / Cooldown / Hurt / Death
- Idle -> Aware: player enter agro radius [placeholder: 5 tile]
- Aware -> Chase: confirmed
- Chase -> Telegraph_T1: player range <= [placeholder: 1.2 tile]
- Chase -> Telegraph_T2: T2 cooldown ready (self-cast, range bagimsiz)
- Telegraph_T2 -> Self_Mortification: telegraph complete
- Self_Mortification -> Buff_Active: self-damage applied
- Buff_Active -> Chase/Attack: buff duration [placeholder: 5s]
- Buff_Active expire -> Cooldown_T2
- Any -> Hurt: damage (T2 self-cast NOT interrupted by hurt -- intentional)
- Any -> Death: HP <= 0

**3. T1 Base Attack**
- Cooldown: [placeholder: 2.2s -- playtest tuning]
- Telegraph: [placeholder: 500ms -- playtest tuning] (heavy windup, iri vurus pozu)
- Damage: [placeholder: 18 -- playtest tuning]
- Hitbox: melee slam [placeholder: 1.2 tile radius, 120 deg front]
- Animation slot: attack_t1
- Note: yavas ama sert -- telegraphi okunamaz ise sert ceza

**4. T2 Ability -- Self-Mortification**
- Cooldown: [placeholder: 10s -- playtest tuning]
- Telegraph: [placeholder: 1.6s -- playtest tuning] Bruiser kendine vurma animasyonu, kirmizi flas
- Cast animation: cast_t2 (self-flagellation pozu)
- Mekanism:
  - Bruiser kendine hasar verir: HP -[placeholder: %10 maks HP -- playtest tuning]
  - Sonraki [placeholder: 5s -- playtest tuning] boyunca damage +[placeholder: %30 -- playtest tuning] buff kazanir
  - Anti-cheese: buff stack etmez, sadece refresh (tekrar kullanirsa sadece sure uzar)
- Visual cue: kirmizi body flash + hasar sayisi kendi uzerinde
- Buff aktif gorusel: kirmizi auraCosmic pulse (belirgin, oyuncu uyarisi)
- Note: Bruiser buff aktifken ozellikle tehlikeli -- T1 hasarini arttirir

**5. Counterplay**
- Dodge i-frame window: [placeholder: 0.5s -- playtest tuning]
- Parry: T1 slam parry edilebilir (buyuk telegraph = parry firsat)
- Buff aktifken: kacinma on planda -- buff'li T1 slam max hasar
- Aura pasif karsi: [placeholder: 3m] disinda kalmak heal kesintisini onler
- Alternatif: Bruiser'i T2 telegraphinda interrupt etme olanagi YOK (hurt self-cast interrupt etmez) -- kacinma zorunlu
- Oncelik: Bruiser buff aktifken en tehlikeli mob -- focus veya disengage

**6. Group Composition**
- Normal room: max 1 Bruiser (tek Bruiser bile aura + buff tehditkar)
- Elite room: max 2 Bruiser (2 Bruiser aura overlap, zone-wide heal azaltma)
- Synergy: Shard Walker kristal zone + Bruiser aura = oyuncu hem zone'da hem heal azalmis
- Synergy: Fracture Imp summon + Bruiser brawl = oyuncu mikro-dalga + buyuk vurus ayni anda
- Priority hedef: Bruiser buff aktifken yuksek oncelik

### Faction-Blind Aura Notu (Karar #61 detay)
- %50 heal azaltma, [placeholder: 3m -- playtest tuning] radius, faction-blind
- Bruiser kendisi dahil radius icindeki TUM birimler etkili (oyuncu + mob + Bruiser)
- Mob infighting HAYIR -- Bruiser diger moblara saldirmaz, sadece oyuncuya odakli
- Visual: kirmizi pulsing ring, 3m kenar belirgin (player heal debuff gorusel uyarisi kritik)
- Oyuncu etkisi: potions / regen / item heali [placeholder: %50] azalir

### Self-Mortification Mekaniği
- Bruiser kendine vurur (HP -[placeholder: %10 maks HP -- playtest tuning])
- Sonraki [placeholder: 5s -- playtest tuning] boyunca damage +[placeholder: %30 -- playtest tuning] buff
- Anti-cheese: ayni buff stack etmez (refresh)
- Self-damage minimum HP koruması: [placeholder: %10 alt sinir -- olmeyecek sekilde -- playtest tuning]

### Animation Slots
- idle / walk / telegraph_t1 / attack_t1 / telegraph_t2 / cast_t2 / hurt / death

### Telegraph Spec
- T1: [placeholder: 500ms -- playtest tuning] heavy windup flas
- T2 pre-cast: [placeholder: 1.6s -- playtest tuning] self-flagellation animasyonu, kirmizi flas
- Buff aktif: kirmizi aura pulse (buff sure boyunca devam)

### Open Tuning Questions
1. Self-damage %10 -- risk/odul dengesi? %20 daha cesur hissi?
2. Buff +%30 damage -- T1 ile komboda cok mu oldurucu?
3. Aura radius 3m -- kucuk arena icin asiri genis mi?

---

## 3. Fracture Imp -- Imp Tide

### 6 Alan

**1. Role + Stats**
- Role: Swarm / Harasser (summon, kalabalik baski, dikkat dagitma)
- Size class: 64px (Fracture Imp kendisi)
- Sub-imp size: 32px
- HP: [placeholder: 60 -- playtest tuning]
- Speed: [placeholder: 3.0 tile/s -- playtest tuning]
- Base attack range: [placeholder: 1.0 tile -- playtest tuning]
- XP value: [placeholder: 20 -- playtest tuning] (sub-impler ayri XP vermez)

**2. Behavior Tree**
- States: Idle / Aware / Chase / Telegraph_T1 / Attack_T1 / Telegraph_T2 / Imp_Tide_Cast / Cooldown / Hurt / Death
- Sub-Imp States: Spawn / Chase_Player / Contact_Attack / Dead
- Idle -> Aware: player enter agro radius [placeholder: 7 tile]
- Aware -> Chase: confirmed
- Chase -> Attack_T1: range <= [placeholder: 1.0 tile]
- Chase -> Telegraph_T2: T2 ready AND arena sub-imp count < cap [placeholder: 8]
- Telegraph_T2 -> Imp_Tide_Cast: telegraph complete
- Imp_Tide_Cast: spawn [placeholder: 3] sub-imp
- Sub-imp spawn -> Chase_Player: immediate
- Sub-imp Contact_Attack: temas = patlama (AOE hasar), sub-imp olur
- Any -> Hurt (Fracture Imp): interrupt T2 cast (telegraph iptal)
- Any -> Death: HP <= 0

**3. T1 Base Attack**
- Cooldown: [placeholder: 1.5s -- playtest tuning]
- Telegraph: [placeholder: 250ms -- playtest tuning] (kisa tirmik ataği pozu)
- Damage: [placeholder: 8 -- playtest tuning]
- Hitbox: melee claw [placeholder: 1 tile radius, 80 deg front]
- Animation slot: attack_t1

**4. T2 Ability -- Imp Tide**
- Cooldown: [placeholder: 14s -- playtest tuning]
- Telegraph: [placeholder: 1.2s -- playtest tuning] portalcik acma animasyonu, mor enerji pulse
- Cast animation: cast_t2
- Effect: [placeholder: 3 -- playtest tuning] sub-imp spawn eder
- Sub-imp AI: basit chase + suicide contact patlama
- Sub-imp patlama hasari: [placeholder: 12 -- playtest tuning]
- Sub-imp lifetime: [placeholder: 8s -- playtest tuning] (sure bitince kendi kendine olur)
- Anti-cheese: arena max sub-imp cap [placeholder: 8 -- playtest tuning] (cap asilmissa T2 kullanmaz)
- Visual cue: portalcik mor, sub-imp spawn parcacik, sub-imp uzerinde kirmizi goz (hedef gorusel)

**5. Counterplay**
- Dodge i-frame window: [placeholder: 0.4s -- playtest tuning]
- Sub-imp patlama: temas oncesi 0.1s flas (minimal uyari -- hizli hedef)
- Parry: T1 claw parry edilebilir
- Sub-imp counter: AOE saldiri ile toplu temizleme mumkun
- Safe zone: Fracture Imp T2 telegraphinda hurt = cast iptal -- Fracture Imp'e focus ile tide onlenir
- Alternatif: sub-imp'lari yoksay + Fracture Imp'e focus (lifetime beklenmeden kaynagi kes)

**6. Group Composition**
- Normal room: max 1 Fracture Imp (sub-imp kalabaligi zaten doldurucu)
- Elite room: max 2 Fracture Imp (arena cap [placeholder: 8] sub-imp ile sinirli)
- Synergy: Shard Walker kristal zone + Imp Tide = sub-impler zonedan drive, oyuncu sikisir
- Synergy: Bruiser brawl + Imp Tide = buyuk vuruslar arasinda kucuk patlamalar, focus dagitir
- Priority hedef: Fracture Imp T2 telegraphinda en yuksek oncelik (cast iptal = tide onlenir)

### Summon Sub-Spec
- Spawn count: [placeholder: 3 imp -- playtest tuning]
- Sub-imp stats:
  - HP: [placeholder: 15 -- playtest tuning]
  - Damage (patlama): [placeholder: 12 -- playtest tuning]
  - Lifetime: [placeholder: 8s -- playtest tuning]
  - Speed: [placeholder: 4.0 tile/s -- playtest tuning] (hizli -- baski hissi)
  - Size: 32px
- Sub-imp AI: basit chase_player -> contact -> patlama -> death
- Contact patlama: [placeholder: 0.5 tile -- playtest tuning] AOE radius
- Anti-cheese: arena max imp cap [placeholder: 8 -- playtest tuning]

### Animation Slots
- idle / walk / telegraph_t2 / attack_t1 / cast_t2 / hurt / death
- Sub-imp: spawn / chase / explode / death

### Telegraph Spec
- T2 pre-cast: [placeholder: 1.2s -- playtest tuning] portalcik acma, mor enerji pulse
- Sub-imp spawn: parcacik burst (kolay taninan)
- Sub-imp contact: [placeholder: 0.1s -- playtest tuning] flas oncesi patlama

### Open Tuning Questions
1. Imp Tide 3 spawn dengeli mi? 4 swarm hissi daha mi iyi?
2. Sub-imp speed 4.0 tile/s -- asiri hizli mi, kacis imkani yeterli mi?
3. Arena cap 8 -- buyuk arenalarda yetersiz mi?

---

## Group Composition Matrix

| Room Type | Comp 1 | Comp 2 | Comp 3 |
|---|---|---|---|
| Normal | 2x Shard Walker | 1x Bruiser + 1x Shard Walker | 1x Fracture Imp + 1x Shard Walker |
| Elite | 1x Bruiser + 2x Shard Walker | 1x Fracture Imp + 2x Shard Walker | 2x Bruiser + 1x Fracture Imp |

**Kombo detayi:**

- **Normal -- 2x Shard Walker:** Iki kristal zone aralik dagitir, oyuncu hareket alani kisalir. Priority: tek tek saldiri, zone aktifken kacin.
- **Normal -- 1x Bruiser + 1x Shard Walker:** Bruiser aura + Shard zone = heal azalmis + hareket kisitli. Priority: Bruiser buff aktif olmadan Shard'i al.
- **Normal -- 1x Fracture Imp + 1x Shard Walker:** Sub-impler zonedan kacmaya iter, Shard yakin mesafede vuracak. Priority: Fracture Imp telegraphinda cast iptal et.
- **Elite -- 1x Bruiser + 2x Shard Walker:** Bruiser merkez tutar, iki kristal zone cikisi kapatir. Priority: Bruiser oncelik, zone ile ugrasmadan once.
- **Elite -- 1x Fracture Imp + 2x Shard Walker:** Tide + iki zone = minimum guvenli alan. Priority: Fracture Imp T2 iptal kritik, sonra Shard.
- **Elite -- 2x Bruiser + 1x Fracture Imp:** Iki aura overlap = arena genelinde heal azaltma. Priority: Fracture Imp (tide = kalabalik artar), sonra Bruiser.

---

## Animation Frame Counts (placeholder)

| Slot | Frames @ 10-12fps |
|---|---|
| idle | [placeholder: 4-6 -- playtest tuning] |
| walk | [placeholder: 6-8 -- playtest tuning] |
| telegraph_t2 | [placeholder: 8-12 -- playtest tuning] |
| attack_t1 | [placeholder: 4-6 -- playtest tuning] |
| cast_t2 | [placeholder: 6-10 -- playtest tuning] |
| hurt | [placeholder: 2-3 -- playtest tuning] |
| death | [placeholder: 6-10 -- playtest tuning] |
| sub-imp chase | [placeholder: 4 -- playtest tuning] |
| sub-imp explode | [placeholder: 3 -- playtest tuning] |

---

## Cross-References
- Karar #61 (mob infighting yok, Bruiser aura faction-blind)
- Karar #82 (3-Tier Skill System, T3 disabled Faz 1)
- Karar #84 (T2/T3 staged room budget)
- Bagimli belgeler: MOB_COMPOSITION_RULES, DAMAGE_CALCULATION
- BOSS_PHASE2_RIFT_TEAR_SPEC (mob + boss arena sinerjisi, Faz 2 hazirlik)

---

## Open Tuning Questions (playtest oncesi)

1. Crystal Bloom AOE radius -- 2m odak / 4m avoid? (Shard Walker zone boyutu)
2. Bruiser self-damage %10 az mi / %20 cesaret testi? (risk/odul denklemi)
3. Imp Tide 3 spawn dengeli mi / 4 swarm hissi? (kalabalik basinci)
4. Telegraph timing 1.2s - 1.6s -- quick reaction tier mi? (oynanabilirlik siniri)
5. Crystal Bloom Hurt interrupt -- kolayca kesilebiliyor mu? (anti-cheese kontrol)
6. Sub-imp speed 4.0 tile/s -- kacis imkani yeterli mi?
7. Elite arena cap 8 sub-imp -- buyuk arenalarda yetersiz?
8. 2x Bruiser aura overlap -- arena genelinde heal azaltma cok guclu mu?

---

## Production Notes

- Sayisal degerler playtest tuning -- placeholder ile yazildi (kasitli)
- T2 telegraph 1.2-1.6s clear visual -- dodge edebilir olmali (oynanabilir tasarim gerekliligi)
- Faz 1 butce limitleri kesin: Normal max 2 T2, Elite max 3, T3 disabled
- PixelLab Create Character: Shard Walker + Bruiser 64px chibi, Fracture Imp 64px (sub-imp 32px)
- 4 yon + flipX, 10-12 fps anim
- Sub-imp ayri karakter seti gerektirir (32px, minimal animasyon: spawn/chase/explode/death)
- Bruiser aura visual (kirmizi pulsing ring) UI overlay olarak ayri layer -- shader gerekebilir

