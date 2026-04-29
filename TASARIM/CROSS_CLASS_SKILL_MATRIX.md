# RIMA — Cross-Class Skill Matrix (Tam Tasarım)

> **[STALE]** Eski skill isimleri (Kidney Shot, Flare, Aimed Shot, Fan of Knives, Vanish, Evasion, Ambush, Hemorrhage, Battle Cry, Dead Eye, Critical Shot, Bullet Rain, Rally Cry, Soul Bargain, Whirlwind, Mille Feuille Cut, Blade Veil, Rush Combo, Momentum Strike vb.) geçersiz. `SINIF_VE_SKILL_KARAR_BELGESI.md` S41 sync bittikten sonra sıfırdan rebuild edilecek. **Şu an bu dosyayı skill referansı olarak kullanma.**

*Güncelleme: 2026-04-12 | ChatGPT + Gemini + Claude final karar*

> Her kombinasyon için: Secondary pool (6 skill) + Auto pasif + Sinerji skill
> A primary + B secondary ≠ B primary + A secondary — farklı kimlik, farklı ritim
> Faz 1-2: Tam detay | Faz 3-4: Prensip + taslak

---

## ONAYLANAN KURALLAR (review sonrası)

**Sinerji skill tier:**
- Tüm sinerji skilleri **Epic** tier — çıkma şansı %12, Oda 3+ depth lock.
- Sinerji skill sistemi Epic tier içinde kaldığı için Legendary veya Mythic tier'a girmez.
- (Eski "Heroic" etiketi kaldırıldı — "Heroic" artık RIMA'da tier adı değil.)

**Her sinerji skill bir "bridge verb" taşır:**
```
Iron Phantom      = dash → stealth punish
Molten Wrath      = rage → ignite
Predator's Advance = root → execute
Ghost Arrow       = stealth → invisible snipe
Crimson Surge     = stealth → rage burst
Hunter's Mark     = trap → assassination
Storm Shot        = slow → elemental arrow
Elemental Arrow   = root → element cast
Phantom Inferno   = stealth → detonation
Shadow Flame      = bleed → yanma birleşim
War Hunter        = kite → melee punish
Arcane Fury       = melee → arcane nova
```

**Auto pasif kuralı:**
Her auto pasifin en az 1 davranış köprüsü olacak.
Format: "Primary aksiyonu tetikler → Secondary buna cevap verir."
Salt "+stat" olmaz.

**Stealth-burst overlap ayrıştırması:**
Aynı "saklan+vur" ailesinden görünen 4 skill ayrıştırılır:
- Iron Phantom = **kimlik: zincir dash** (iki hareket birleşimi)
- Crimson Surge = **kimlik: Rage'den güç alan stealth çıkışı**
- Ghost Arrow = **kimlik: görünmez ranged tek atış**
- Hunter's Mark = **kimlik: çevre kontrolü + tuzak assassination**
VFX renkleri de farklı: Iron Phantom=kırmızı/gri | Crimson Surge=kırmızı/mor | Ghost Arrow=yeşil/transparan | Hunter's Mark=koyu yeşil/toprak

---

## FORMAT

```
### [PRIMARY] + [SECONDARY]
Kimlik: "tek cümle"
Secondary pool: hangi 6 skill Z/X'e girer + neden
Auto pasif: PlayerClassManager'ın eklediği bridge pasif
Sinerji skill: Koşul + efekt
```

---

# ═══ FAZ 1-2 KOMBİNASYONLARI ═══

---

## WB PRIMARY — 3 COMBO

---

### ⚔️+🔥 WARBLADE primary + ELEMENTALİST secondary
**Kimlik:** "Yaklaş, yakıt."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Fireball | WB_Elem pasifi ile Rage+ateş sinerji | ✅ Core |
| Glacial Spike | Slow → Death Blow setup, WB'nin CC zinciri | ✅ Core |
| Blink | Kaçış + reposition, WB'nin tek mobility eksikliği | ✅ Core |
| Arcane Blast | War Stomp knockup sonrası AoE burst | ✅ Core |
| Chain Lightning | Temizlik, standalone güçlü | ⚠️ Utility |
| Mirror Image | Tank/distraction, son seçenek | ⚠️ Utility |

**Auto pasif — CrossClassPassive_WB_Elem:**
"Ateşli hasar veya alınca +5 Mana. Mana 50+ iken Rage kazanımı +%20."
→ İki kaynağı birbirine bağlar.

**Sinerji skill — MOLTEN WRATH (Epic):**
Koşul: WB'den ≥1 aktif + Elem'den ≥1 aktif alınmış olacak.
Efekt: Rage 50+ iken herhangi Ateş skill kullanılırsa ekstra 5m lav patlaması (2s yanma + knockback). Yanmakta olan hedefe Iron Charge → yanma süresi ×2 + Rage +30.

---

### ⚔️+🗡️ WARBLADE primary + SHADOWBLADE secondary
**Kimlik:** "Gücü var, ustalığı da öğrendi."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Backstab | Iron Charge arkaya geçince %200 hasar — ideal sinerji | ✅ Core |
| Shadow Step | Ek gap-close, Blade Rush'ın yedek/tamamlayıcısı | ✅ Core |
| Hemorrhage | Bleed + Rage sinerji (WB_Shadow pasifi) | ✅ Core |
| Kidney Shot | 5s stun → Death Blow execute setup | ✅ Core |
| Evasion | WB'nin savunma açığını kapatır | ✅ Core |
| Fan of Knives | AoE debuff yayımı, Cleave alternatifi | ⚠️ Utility |

**Auto pasif — CrossClassPassive_WB_Shadow:**
"Bleed aktif düşmanı öldürünce +15 Rage. Iron Charge ile bleed hedefe ulaşınca stun +0.5s."

**Sinerji skill — IRON PHANTOM (Epic):**
Koşul: WB'den ≥1 aktif + Shadow'dan ≥1 aktif alınmış.
Efekt: Iron Charge kullandıktan 2s içinde Shadow Step kullanılırsa → sonraki saldırı backstab bonusu (%150) + 3s mini stealth girer. Bleed aktif hedefe Iron Charge → stun süresi +1s.

---

### ⚔️+🏹 WARBLADE primary + RANGER secondary
**Kimlik:** "Yaklaşmadan önce hazırlar, yaklaştıktan sonra bitirir."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Aimed Shot | Range opener → WB approach, CC setup | ✅ Core |
| Concussive Arrow | Knockback + root → Death Blow, Iron Charge sinerji | ✅ Core |
| Disengage | WB'nin yokluğu olan kaçış mekanizması | ✅ Core |
| Barbed Net Shot | Root + bleed, Iron Charge sinerji | ✅ Core |
| Explosive Trap | Zemin kontrolü, WB'nin approach'ını güvenli yapar | ✅ Core |
| Volley | AoE temizlik, standalone güçlü | ⚠️ Utility |

**Auto pasif — CrossClassPassive_WB_Ranger:**
"Root/CC altındaki düşmana verilen hasar +%15. Aimed Shot Ranger secondary ile Focus üretir, WB hasarı artırır."

**Sinerji skill — PREDATOR'S ADVANCE (Epic):**
Koşul: WB'den ≥1 aktif + Ranger'dan ≥1 aktif alınmış.
Efekt: Root/CC altındaki hedefe Iron Charge → hasar ×2 + Rage +50 (normal +20 yerine). Gravity Cleave kullandıktan sonra 4s Aimed Shot CD yok, Focus maliyeti 0.

---

## ELEMENTALİST PRIMARY — 3 COMBO

---

### 🔥+⚔️ ELEMENTALİST primary + WARBLADE secondary
**Kimlik:** "Kontrol eder, yaklaşırsa cezalandırır."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Iron Charge | Power move olarak — Blink ile kombine gap-close/escape | ✅ Core |
| War Stomp | Elem'in AoE setup'ı için mükemmel — düşmanları toplar, Blizzard sinerji | ✅ Core |
| Iron Counter | Elem'in yakın dövüş savunmasızlığını kapatır | ✅ Core |
| Gravity Cleave | Çeker + Blizzard/Frozen Orb sinerji | ✅ Core |
| Crippling Blow | Yavaşlatma → spell hit garantisi | ✅ Core |
| Bladestorm | Rage doluyken AoE spin — Elem'in nova anı | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Elem_WB:**
"Melee kill başına +10 Mana. War Stomp sonrası 3s Elemental State birikimi +1."

**Sinerji skill — ARCANE FURY (Epic):**
Koşul: Elem'den ≥1 aktif + WB'den ≥1 aktif.
Efekt: Mana 80+ iken War Stomp veya Iron Counter kullanılırsa → melee hit sonraki 4s boyunca Elemental State +2 uygular (hit başına). Gravity Cleave çektikten sonra Meteor cast süresi yok, instant.

---

### 🔥+🗡️ ELEMENTALİST primary + SHADOWBLADE secondary
**Kimlik:** "Görünmez ol, büyü yağdır."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Vanish | Yakın düşmandan anında kaçış — Elem'in en büyük açığı | ✅ Core |
| Shadow Step | Reposition — Blink'in alternatifi farklı kaynakla | ✅ Core |
| Backstab | Stealth çıkışında instant spell değil, yakın bonus | ✅ Core |
| Evasion | Dodge + Elem'in cast koruması | ✅ Core |
| Fan of Knives | AoE debuff + Elemental State ile combo | ✅ Core |
| Hemorrhage | Bleed DoT + Elemental DoT stack'lenebilir | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Elem_Shadow:**
"Stealth'ten çıkınca sonraki spell +%30 hasar. Bleed aktif hedefe spell +1 Elemental State."

**Sinerji skill — PHANTOM INFERNO (Epic):**
Koşul: Elem'den ≥1 aktif + Shadow'dan ≥1 aktif.
Efekt: Vanish sonrası cast edilen ilk spell patlama yarıçapı ×2 + 0 cast süresi. Bleed ve yanma aynı anda aktif hedefe Fireball → iki DoT birleşip patlama yapar (%200 tek hasar).

---

### 🔥+🏹 ELEMENTALİST primary + RANGER secondary
**Kimlik:** "Mesafe kraldır. İkisi de uzakta savaşır."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Aimed Shot | Tek hedef burst — Elem'in AoE'sine ST tamamlayıcı | ✅ Core |
| Disengage | Yaklaşan düşmandan çekilme — Blink ile çift kaçış | ✅ Core |
| Concussive Arrow | Knockback → Spell hit garantisi, Meteor combo | ✅ Core |
| Flare | Stealth açığa çıkar + slow alanı — mixed combat | ✅ Core |
| Barbed Net Shot | Root → Glacial Spike, Meteor, Frozen Orb setup | ✅ Core |
| Tethering Arrow | Uzaklaştıkça hasar → Elem'in kite döngüsüyle uyumlu | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Elem_Ranger:**
"Ranger skill kullanınca +5 Mana. 4m+ mesafedeki hedefe spell +%10 hasar (Ranger'ın Focus distance mantığı)."

**Sinerji skill — STORM SHOT (Epic):**
Koşul: Elem'den ≥1 aktif + Ranger'dan ≥1 aktif.
Efekt: Root altındaki hedefe cast edilen spell Elemental State +2 uygular (yerine +1). Aimed Shot hold → bırakınca mermi Elemental State'e göre element kazanır (Fire: 2s yanma, Frost: 1s slow).

---

## SHADOWBLADE PRIMARY — 3 COMBO

---

### 🗡️+⚔️ SHADOWBLADE primary + WARBLADE secondary
**Kimlik:** "Gizlenip gelir. Ama bazen eziyor."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Iron Charge | Power gap-close, assassin'ın ani hamle seçeneği | ✅ Core |
| Sunder Mark | Backstab + zırh -%40 = brutal execute | ✅ Core |
| War Stomp | Stealth'ten çıkış AoE, kalabalık kontrolü | ✅ Core |
| Crippling Blow | Yavaşlatma → Kidney Shot zinciri | ✅ Core |
| Battle Surge | HP regen — Shadow'un iyileşme eksikliğini kapatır | ✅ Core |
| Death Blow | HP<%30 execute — finisher katmanı | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Shadow_WB:**
"Stealth'ten çıkınca +20 Rage. Rage 50+ iken backstab bonusu +%30."

**Sinerji skill — CRIMSON SURGE (Epic):**
Koşul: Shadow'dan ≥1 aktif + WB'den ≥1 aktif.
Efekt: Vanish veya Evasion aktifken Iron Charge kullanılırsa → hedef 3s görünmez işaretlenir (Haunt gibi), Iron Charge hasarı ×1.5. İşaretli hedefe ilk Backstab → garantili %300 + Rage anında +40.

---

### 🗡️+🔥 SHADOWBLADE primary + ELEMENTALİST secondary
**Kimlik:** "Zehir ve ateş. İkisi de öldürür, ikisi de görünmez."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Fireball | Stealth çıkışı + AoE, bleed+yanma combo | ✅ Core |
| Blink | Shadowstep + Blink = çift reposition | ✅ Core |
| Living Bomb | Bleed + Living Bomb = çift DoT stack | ✅ Core |
| Glacial Spike | Slow → Backstab, Kidney Shot setup | ✅ Core |
| Arcane Blast | Çevre temizliği, Evasion sonrası burst | ✅ Core |
| Combustion | Tüm Fire spell instant cast — Shadow'un hız ritmiyle uyumlu | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Shadow_Elem:**
"Bleed aktif düşmana spell +1 Elemental State. Stealth'ten spell cast edince +10 Mana."

**Sinerji skill — SHADOW FLAME (Epic):**
Koşul: Shadow'dan ≥1 aktif + Elem'den ≥1 aktif.
Efekt: Backstab ile bleed uygulandığında 3s içinde Fireball kullanılırsa → bleed + yanma birleşir, patlama yapar (%250 instant). Vanish içinden cast edilen Fireball silent (ses yok, enemy alert tetiklenmez) + hasar +%50.

---

### 🗡️+🏹 SHADOWBLADE primary + RANGER secondary
**Kimlik:** "Yakın veya uzak, ikisi de tuzak."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Aimed Shot | Stealth'ten çıkmadan uzaktan açılış | ✅ Core |
| Disengage | Shadowblade'in ikinci kaçış seçeneği (Shadowstep + Disengage) | ✅ Core |
| Barbed Net Shot | Root + bleed (bleed Shadow'un ana mekanizması) | ✅ Core |
| Explosive Trap | Vanish sonrası tuzak yerleştirme + kaçış combo | ✅ Core |
| Flare | Düşman stealth'i engelle (PvP veya özel mob) | ⚠️ Utility |
| Concussive Arrow | Knockback → Kidney Shot setup mesafeden | ✅ Core |

**Auto pasif — CrossClassPassive_Shadow_Ranger:**
"Root altındaki hedefe backstab +%20 hasar. Disengage kullanınca CP +1."

**Sinerji skill — HUNTER'S MARK (Epic):**
Koşul: Shadow'dan ≥1 aktif + Ranger'dan ≥1 aktif.
Efekt: Vanish içindeyken Explosive Trap veya Barbed Net Shot kullanılırsa → trap/net stealth'te görünmez olur (düşman görmez), tetiklenince %200 bonus hasar + otomatik Backstab mesafesi oluşur. Bleed aktif hedefe Aimed Shot hold → guaranteed crit + bleed süresi +4s.

---

## RANGER PRIMARY — 3 COMBO

---

### 🏹+⚔️ RANGER primary + WARBLADE secondary
**Kimlik:** "Uzakta savaşır ama gerekince kırıp geçer."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Iron Charge | Düşman yaklaşınca gap punish | ✅ Core |
| War Stomp | Kalabalığı dağıt — Volley combo | ✅ Core |
| Gravity Cleave | Çeker → Ranger menzile sokar (ironi: çeker ki uzak kalsın) | ✅ Core |
| Iron Counter | Yakın dövüş savunması | ✅ Core |
| Deep Wound | Bleed + Ranger'ın Multi-Shot wound sinerji | ✅ Core |
| Crippling Blow | Yavaşlatma → Ranger'ın kite döngüsü kolaylaşır | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Ranger_WB:**
"4m altı düşmana melee verince Focus +10. Iron Charge hedefe Aimed Shot mark koyar (2s içinde +%30 hasar)."

**Sinerji skill — WAR HUNTER (Epic):**
Koşul: Ranger'dan ≥1 aktif + WB'den ≥1 aktif.
Efekt: Tethering Arrow ile bağlanan hedefe Iron Charge → zincir anında kopar, %400 kritik hasar + Focus +50 + Rage +30. Disengage geri atlarken Gravity Cleave aktifse → geri atlama sırasında düşmanları çeker (her iki yönde çalışır).

---

### 🏹+🔥 RANGER primary + ELEMENTALİST secondary
**Kimlik:** "Ok + büyü. Her şeye cevap var."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Fireball | Yakın grubu temizle, Ranger'ın melee açığı | ✅ Core |
| Glacial Spike | Slow → Aimed Shot combo, mükemmel sinerji | ✅ Core |
| Blink | Disengage + Blink = çift kaçış/reposition | ✅ Core |
| Arcane Blast | Yaklaşan kalabalığa AoE | ✅ Core |
| Frozen Orb | Yavaş hareketli, Ranger kite path'ına yayılır | ✅ Core |
| Chain Lightning | Multi-Shot + Chain Lightning = uzaktan AoE | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Ranger_Elem:**
"Ranged skill ile hit edilen hedefe +1 Elemental State. Elemental State 3+ hedefe Aimed Shot +%20 hasar."

**Sinerji skill — ELEMENTAL ARROW (Epic):**
Koşul: Ranger'dan ≥1 aktif + Elem'den ≥1 aktif.
Efekt: Glacial Spike slow altındaki hedefe Aimed Shot hold → mermi element kazanır (Frost: %300 hasar + Freeze 2s). Blink'ten sonra 3s içinde atılan oklar Fireball DoT uygular (her ok).

---

### 🏹+🗡️ RANGER primary + SHADOWBLADE secondary
**Kimlik:** "Görünmez avcı."

**Secondary pool (6):**
| # | Skill | Neden seçildi |
|---|-------|--------------|
| Vanish | Yüklü Aimed Shot'ı stealth'ten hazırla | ✅ Core |
| Shadowstep | Yaklaşan düşmanın arkasına geç, Backstab değil — Disengage upgrade | ✅ Core |
| Hemorrhage | Bleed + Barbed Net Shot kanama stack | ✅ Core |
| Evasion | Dodge + Focus birikimi, Ranger'ın hayatta kalması | ✅ Core |
| Ambush | Stealth'ten %300 hasar — Ranger'ın en güçlü ani açılışı | ✅ Core |
| Fan of Knives | Grupları bleed ile yakala, Multi-Shot ile devam | ⚠️ Utility |

**Auto pasif — CrossClassPassive_Ranger_Shadow:**
"Stealth'ten ilk ok +%50 hasar. Bleed aktif düşmana Aimed Shot crit şansı +%20."

**Sinerji skill — GHOST ARROW (Epic):**
Koşul: Ranger'dan ≥1 aktif + Shadow'dan ≥1 aktif.
Efekt: Vanish içindeyken Aimed Shot hold edilirse → bırakıldığında ok görünmez (düşman dodge etmez) + %400 hasar + guaranteed bleed (4s). Ambush stealth'ten çıkışında Disengage'in Focus maliyeti 0 (anında kaçış imkanı).

---

# ═══ FAZ 3 KOMBİNASYONLARI (TASLAK) ═══
*Tam detay Faz 3 yaklaşınca yazılacak. Prensipler ve secondary havuz seçim mantığı burada.*

---

### PRENSİP: FAZ 3 CLASS'LARI İÇİN SECONDARY HAVUZ SEÇİMİ

**Ravager secondary olarak:**
- Seçilecek 6 skill: Bloodlust Strike, Frenzied Leap, Reckless Swing, Battle Cry, Undying Tenacity, Death Wish
- Prensip: Rage benzeri Fury → cross-class bridge. Her primary class'ın kaynağı Fury ile bağlanmalı.
- Auto pasif mantığı: "Kill başına +X Fury. Primary skill kullanınca +Y Fury."

**Ronin secondary olarak:**
- Seçilecek 6 skill: Quickdraw Slash, Haste Dash, Wind Step, Counter Draw, Blade Veil, Flash Draw
- Prensip: Mobility + burst — her primary'e reposition ve tekli hasar ekler.
- Auto pasif mantığı: "Dash/movement sonrası sonraki skill +%20 hasar."

**Gunslinger secondary olarak:**
- Seçilecek 6 skill: Rift Dash, Quickdraw, Critical Shot, Dead Eye, Smoke Grenade, Ricochet
- Prensip: Mesafe + burst. Yakın sınıflara range seçeneği.
- Auto pasif mantığı: "Ranged hit başına Heat +5 (dummy resource, ya da primary resource'a bridge)."

**Brawler secondary olarak:**
- Seçilecek 6 skill: Mach Punch, Rush Combo, Counter Blow, Guard Break, Repulse, Aerial Rave
- Prensip: Charge mekanizması → her hit daha güçlü. Combo builder.
- Auto pasif mantığı: "Her aktif skill kullanımı +1 Brawler Charge. 5 Charge = sonraki LMB +%50."

---

### FAZ 3 CROSS-CLASS TASLAK (Sinerji skill prensipleri)

| Primary | Secondary | Sinerji Tema |
|---------|-----------|-------------|
| Ravager | Warblade | "Zırh kır + Fury biriktir" — iki execute sistemi |
| Ravager | Shadowblade | "HP düşünce stealth" — son stand + kaçış |
| Ravager | Elementalist | "Hasar al → ateş nova" — acı = güç |
| Ronin | Warblade | "Katana + kılıç" — iki yüksek hasar burst |
| Ronin | Shadowblade | "Phantasm" — illusion + assassination |
| Ronin | Ranger | "Kite cutter" — kaçan düşmanı keser |
| Gunslinger | Warblade | "Run and gun + melee finisher" |
| Gunslinger | Ranger | "Double ranged" — mesafe sahipliği total |
| Gunslinger | Shadowblade | "Smoke cover + assassination" |
| Brawler | Warblade | "Pure brute" — en yüksek ham melee hasar |
| Brawler | Ravager | "Unstoppable" — tüm CC immune |
| Brawler | Ranger | "Rush and shoot" — approach + range |

---

# ═══ FAZ 4 KOMBİNASYONLARI (KONSEPT) ═══

*Summoner ve Hexer benzersiz kaynak mekanizmaları nedeniyle cross-class en karmaşık.*

### Summoner Cross-Class Prensibi:
- Summoner primary olunca Z/X'teki secondary skills minyonlarla etkileşmeli
- "Summoner + Warblade" → WB skill'leri minyonlara komuta eder gibi hissettirmeli
- "Summoner + Hexer" → en sinerjik combo: minyon + stack = köle + lanет

### Hexer Cross-Class Prensibi:
- Hexer secondary olunca Z/X'e giren 6 skill: stack biriktiren, yayan, patlatan
- Her primary sınıfının hasarı Hex Stack biriktirir → Hexblast finisher
- "Hexer secondary" = her sınıfa patience/burst layer ekler

---

# ═══ ÖZET TABLO ═══

| Kombinasyon | Sinerji Skill | Tema |
|-------------|--------------|------|
| WB + Elem | Molten Wrath | Rage + Ateş |
| WB + Shadow | Iron Phantom | Dash + Stealth |
| WB + Ranger | Predator's Advance | CC → Execute |
| Elem + WB | Arcane Fury | Büyü + Melee nova |
| Elem + Shadow | Phantom Inferno | Stealth cast |
| Elem + Ranger | Storm Shot | Elemental arrow |
| Shadow + WB | Crimson Surge | Stealth + Rage burst |
| Shadow + Elem | Shadow Flame | Bleed + Yanma |
| Shadow + Ranger | Hunter's Mark | Görünmez tuzak |
| Ranger + WB | War Hunter | Kite + Melee punch |
| Ranger + Elem | Elemental Arrow | Elemental ok |
| Ranger + Shadow | Ghost Arrow | Görünmez ok |
| Ravager + * | TBD Faz 3 | — |
| Ronin + * | TBD Faz 3 | — |
| Gunslinger + * | TBD Faz 3 | — |
| Brawler + * | TBD Faz 3 | — |
| Summoner + * | TBD Faz 4 | — |
| Hexer + * | TBD Faz 4 | — |

---

## REVIEW SORULARI (ChatGPT + Gemini için)

1. **Sinerji skill dengesi:** 12 Faz 1-2 sinerji skill'in hepsi "Epic tier" — bazıları Common/Rare yapılmalı mı?
2. **Auto pasif kalitesi:** Köprü pasifleri (CrossClassPassive) yeterince anlamlı mı yoksa sadece "bonus sayı" mu?
3. **Secondary havuz seçimi:** Hangi kombinasyonun secondary havuzu zayıf/güçlü görünüyor?
4. **Overlap var mı?** Bazı sinerji skilleri birbirine çok benzemiyor mu? (Ghost Arrow vs Iron Phantom — ikisi de stealth+hasar)
5. **Faz 3 class'ları için** hangi cross-class en ilgi çekici olur?

---

*Bu belge ChatGPT + Gemini review sonrası güncellenecek.*
*Kabul edilen değişiklikler → SkillDatabase.cs + SINIF_VE_SKILL_KARAR_BELGESI.md*
