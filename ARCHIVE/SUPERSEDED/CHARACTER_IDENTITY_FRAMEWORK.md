﻿﻿﻿﻿﻿﻿﻿﻿# Karakter Kimliği Framework — PixelLab Üretim Kuralları
> **Source of truth.** Tüm class'lar için kimlik ilkeleri + kilitlenmiş class'lar için PixelLab promptları.
> Kilitlenmemiş class'lar: kimlik özeti + QC eklenince tamamlanır.
> Güncelleme: session 19 (Create from Template Pro format, 4 yeni class lock)

---

## Bu Tur — Adım Adım (Gemini Ref → PixelLab)

Her class için sırayla yap:

```
1. pixellab.ai → Create → "Create from Template" Pro seç
2. Char Type: humanoid
3. Character Size: 128px
4. Camera View: high top-down  ← dropdown'dan seç
5. Concept Image: ilgili *_south_lock.png yükle
   (F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/)
6. Description: aşağıdan ilgili class'ın "Description" metnini kopyala yapıştır
7. Style Image: boş bırak (şimdilik)
8. Generate → en iyi varyasyonu seç
9. QC listesini geç → onayla → kaydet: <class>_S.png
```

**Sira:**
Warblade → Elementalist → Gunslinger → Ravager → Brawler → Ranger → Ronin → Shadowblade → Summoner → Hexer


---

## Temel İlkeler

### Katman 1 — Karakter Kimliği (ÖNCELİK)
- Bu karakter bu dünyada kimdir? Ne yaşadı, nereden geliyor?
- Zırhı/kıyafeti nereden geldi, ne kadar kullandı, ne kadar yıprandı?
- Silhouette güçlü ama abartısız — güç kütleden, kalabalıktan değil
- Oranlar olgun — chibi yok, toy-like yok
- Fonksiyonel zırh/kıyafet — malzeme mantığı gerçekçi
- Büyü/efekt seviyesi sınıf kimliğine göre belirlenir, varsayılan minimal

### Katman 2 — Kamera (ARAÇ)
**Kilit açı — Hero Siege style.**
- **Create from Template Pro kullanılıyorsa:** Camera View → `high top-down` UI'dan seç. Proportion/visibility hints description'a eklenebilir.
- **Create Image Pro kullanılıyorsa (fallback):** Aşağıdaki metni prompta ekle:
```
high overhead top-down camera, steep bird's eye view, around 75-80 degree downward angle, top of head clearly visible, body foreshortened from above, full body readable, not isometric, not side-view, not flat, not paper-thin, volumetric body forms
```

**Yasaklar:**
- `"3/4"` yazma
- `"slightly tilted"` veya `"60-65 degree"` yazma — bunlar Hero Siege değil Hades açısı
- Oyun adı referansı yazma

---

## Global Negative Prompt

Her üretimde ekle:
```
pure top-down flat sprite, paper cutout, sticker-like, no depth, chibi proportions, oversized weapon, exaggerated heroic pose, glowing armor, arcade energy effects, shiny pristine plate, dramatic billowing cape, splash art composition, flashy particle effects, toy-like armor
```

---

## Ton Hedefi

| İstenen | İstenmeyen |
|---------|-----------|
| Grounded dark fantasy | Arcade / oyunlaştırılmış |
| Hikayesel, dünyaya ait | Splash-art hero pose |
| Fonksiyonel, yıpranmış malzeme | Pristine parade armor |
| Hacimli, boyutlu form | Flat / paper-thin sprite |
| Olgun oranlar | Chibi / toy-like |
| Sessiz güç, silhouette netliği | Abartılı silah, aşırı efekt |

**Referans hissi:** Dark Souls / Blasphemous / Darkest Dungeon — Diablo 3 arcade değil.

---

## Araç Seçimi

**Create from Template Pro** (ana araç) — Gemini ref ile üretimde kullanılan format.

### Create from Template Pro — UI Alanları

| Alan | Değer |
|------|-------|
| **Char Type** | `humanoid` |
| **Description** | class promptu (aşağıdaki format) |
| **Character Size** | `128px` |
| **Camera View** | `high top-down` ← UI'dan seç, prompta yazma |
| **Style Image** | Genel pixel art render dili — RIMA tonuna uygun ref varsa; yoksa boş |
| **Concept Image** | Gemini reference PNG (`*_south_lock.png`) — kimlik anchor |

### Camera View Seçimi — KİLİTLENDİ (session 17)
- **Camera View alanı:** `high top-down` — her class, her turda sabit
- ~~Low Top-Down~~ — TERK EDİLDİ (Hades açısı, hedef değil)
- **Kamera açı metni Description'a yazılmaz** — UI seçimi yeterli. Proportion hints izinli.

### Description Prompt Kuralı
- Kamera açı metni prompta yazılmaz — UI'dan seçilir
- Promptlar kısa ve odaklı — Concept Image geri kalanı taşır
- Proportion sorunu varsa: `full body visible head to boots`, `longer legs` ekle
- Tek satır yaz — çok satırlı format kopyalamayı zorlaştırır

---

## Production Pipeline

```
1. South üret → onayla
2. <class>_S.png'yi reference olarak yükle (strength ~80-100)
3. North / East / West yönlerini approved South reference ile üret
4. Diagonaller: adjacent cardinal'lardan türet — NE = N+E referansı, South alone değil
5. Yönler arası karakter yeniden tasarlanmaz
```

---

## Identity Summary Formatı

Her sınıf için doldur:
```
Role in the world:    [kim, neden savaşıyor, nereden geliyor]
Silhouette basis:     [neye dayanıyor, nasıl okunuyor]
Armor / clothing:     [malzeme, durum, renk paleti]
Effect level (0-10):  [ne kadar görünür, ne şekilde]
Grounded / fantasy:   [oran, tek fantastik unsur nedir]
```

---

## QC Checklist Formatı

Her sınıf üretimi sonrası:
```
[ ] Silhouette tutarlı — 4 yön aynı karakteri temsil ediyor mu?
[ ] Sınıf kimliği okunuyor — kim olduğu anında anlaşılıyor mu?
[ ] Malzeme okunuyor — metal / kumaş / deri farkı net mi?
[ ] Hacim var — karakter flat değil, volumetric formlar görünüyor mu?
[ ] Kamera doğru — isometric değil, side-view değil, overhead doğru mu?
```

## Teknik Kabul Kriterleri (PixelLab Output)

PixelLab'den gelen sprite **bu kriterleri geçmeden** pipeline'a girmez:

| Kriter | Kabul | Red |
|--------|-------|-----|
| Çözünürlük | 128×128 px | Başka boyut |
| Renk modu | RGBA (4 kanal, alpha var) | RGB (alpha yok) |
| Arkaplan | Şeffaf (transparent) | Düz renk/opak |
| Piksel stili | Point-like net kenarlar | Yumuşak gradient/painterly blur |
| Stil | Native pixel art | Downscale edilmiş illustration |

> **Not:** Gemini concept referans dosyaları (1024px, RGB, opak) bu kriterlere tabi DEĞİLDİR — onlar PixelLab'e girdi, asıl sprite değil.



---

## HEXER

> **Kimlik kilidi:** Lanet ve çürüme büyücüsü. Ritüel, kontrol ve tükeniş — doğrudan saldırı değil.

**Role in the world:** Rift bozulmasının yarattığı lanet enerjisini kullanan bir kahin-arcanist. Gücü acımasız ama sessiz; aura değil, sızma.

**Silhouette basis:** Uzun tattered kırmızı-bordo robe, bir elde karanlık tahta asa, diğer elde yeşil-mor alevli demir fener. Çürüme sarmallıkları ayak çevresinde.

**Armor / clothing:** Floor-length dark crimson tattered robes, worn and fraying at hem, dark leather belt, iron lantern accessory, dark wooden staff. Zırh yok; tamamen kumaş ve ritüel aksesuar.

**Effect level:** 4/10 — demir fenerin içindeki yeşil-mor alev ve ayak çevresindeki çürüme sarmalı. Gövde üzerinde glow yok.

**Grounded / fantasy:** 60% grounded. Fantastik unsur: lanet feneri, çürüme sarmalı ve dual yeşil-mor renk aksan.

### Lantern + Staff Lock — KESİN (session 19)
- **Demir fener** (cursed green-purple flame) zorunlu
- **Karanlık tahta asa** zorunlu
- **Dual aksan: decay green + void purple** — ikisi birlikte, birini çıkarma
- **Gövde üzerinde yaygın glow YASAK** — enerji yalnızca fenerde ve ayaklarda

### Create from Template Pro

| Alan | Değer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `hexer_south_lock.png` |

**Description:**
`dark wooden staff in one hand, iron lantern with green-purple flame in other hand, dark crimson tattered robes, pixel art 128px transparent background`

### QC
```
[ ] Demir fener net okunuyor mu — yeşil-mor alev ikisi birlikte mi?
[ ] Karanlık tahta asa görünür mü?
[ ] Çürüme sarmalı ayak çevresinde mi — gövdede yaygın glow yok mu?
[ ] Tattered crimson robe okunaklı hem ile mi?
[ ] Kamera overhead doğru, isometric/side-view kayması yok mu?
```

---

## RAVAGER

> **Kimlik kilidi:** Kızgın kan berserkeri. Öfke ve ham güç — no void, no magic, pure rage.

**Role in the world:** Çatlak savaşlarından yüzü dövme ve bedenine kazınmış izlerle çıkmış bir barbar. Gücü void değil, kandan gelen ısı ve öfke.

**Silhouette basis:** Tüm roster'ın en geniş ve en büyük insan gövdesi. Gömlek yok — tribal scarification ve çentikli balta bu figürü tanımlar.

**Armor / clothing:** Shirtless, minimal dark iron bracers only, dark heavy trousers and boots. Zırh yok; kimlik tamamıyla bedenden ve aksesuvarsızlıktan geliyor.

**Effect level:** 3/10 — tribal scarification'da derin kan kırmızısı iç ışıma (#8B1A1A), vücut çevresinde hafif kızıl ısı dalgası. Mavi veya mor YOK.

**Grounded / fantasy:** 75% grounded. Fantastik unsur: scarification'ın iç parıltısı ve çok hafif ısı distorsiyon aurası.

### Build + Color Lock — KESİN (session 19)
- **En geniş ve en büyük insan silhouette** zorunlu
- **Blood red (#8B1A1A) scarification** — mor veya mavi YASAK
- **Dual büyük çentikli balta** (notched, dried blood, one in each hand) zorunlu
- **Mavi/mor enerji YASAK** — yalnızca kırmızı-kızıl aksan

### Create from Template Pro

| Alan | Değer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `ravager_south_lock.png` |

**Description:**
`dual large axes one in each hand, shirtless, blood-red glowing tribal scarification, pixel art 128px transparent background`

### QC
```
[ ] En büyük/geniş silhouette mi — tüm roster içinde readable mi?
[ ] Scarification blood red mu — mor veya mavi yok mu?
[ ] Büyük notched balta, dried blood detayı okunuyor mu?
[ ] Gövde efekti minimal mi — büyük yaygın aura yok mu?
[ ] Kamera overhead doğru, sticker-flat görünüm yok mu?
```

---

## Class Lock Durumu

| Class | PixelLab Prompt | QC | Durum |
|-------|----------------|-----|-------|
| Warblade | ✅ | ✅ | **LOCKED** |
| Ranger | ✅ | ✅ | **LOCKED** |
| Gunslinger | ✅ | ✅ | **LOCKED** |
| Shadowblade | ✅ | ✅ | **LOCKED** |
| Summoner | ✅ | ✅ | **LOCKED** |
| Elementalist | ✅ | ✅ | **LOCKED** |
| Brawler | ✅ | ✅ | **LOCKED** |
| Ronin | ✅ | ✅ | **LOCKED** |
| Hexer | ✅ | ✅ | **LOCKED** |
| Ravager | ✅ | ✅ | **LOCKED** |

---

## Class Combat Identities

> **Kural:** Shared camera â‰  shared combat identity.
> Her sÄ±nÄ±fÄ±n kendi animasyon ritmi ve savaÅŸ dili vardÄ±r. Kamera aynÄ± olsa da karakter fantezisi aynÄ± deÄŸildir.

| SÄ±nÄ±f | SavaÅŸ KimliÄŸi |
|-------|--------------|
| **Warblade** (E) | AÄŸÄ±r, kararlÄ±, committed vuruÅŸlar â€” aÄŸÄ±r ritim, mesafe kapatan, geri Ã§ekilmez |
| **Shadowblade** (E) | HÄ±zlÄ±, kesin, kaÃ§Ä±nan â€” kÄ±sa Ã¶lÃ¼mcÃ¼l pencereler, pozisyon Ã¶ncelikli |
| **Ronin** (E) | HÄ±zlÄ±, hassas, dual-blade â€” keskin ritim, akÄ±ÅŸ hali |
| **Ranger** (K) | Mesafe kontrolÃ¼, takip, sÃ¼rekli yeniden konumlanma |
| **Gunslinger** (K) | Kadans tabanlÄ± menzilli saldÄ±rÄ±, mobilite, patlayÄ±cÄ± burst zamanlamasÄ± |
| **Hexer** (K) | Lanet odaklÄ± casting, kontrol, ritÃ¼el tonu â€” doÄŸrudan deÄŸil dolaylÄ± |
| **Summoner** (K) | Ã‡aÄŸrÄ±lar Ã¼zerinden dolaylÄ± baskÄ±, alan ve pozisyon yÃ¶netimi |
| **Elementalist** (K) | BÃ¼yÃ¼ zamanlamasÄ±, elemental patlama, alan kontrolÃ¼ |
| **Brawler** (E) | VÃ¼cut aÄŸÄ±rlÄ±ÄŸÄ± etkisi, yakÄ±n baskÄ±, agresif baÄŸlanma â€” geri durmuyor |
| **Ravager** (E) | VahÅŸi momentum, kaba dengesizlik â€” kontrol kaybetmek gÃ¼Ã§ |

---

## WARBLADE

**Role in the world:** Son savaÅŸÃ§Ä±lardan biri. Ã‡Ã¶kmekte olan bir savaÅŸ geleneÄŸinin kalÄ±ntÄ±sÄ±. SavaÅŸÄ±yor Ã§Ã¼nkÃ¼ baÅŸka bildiÄŸi yok.

**Silhouette basis:** GeniÅŸ omuzlar, alÃ§ak ve aÄŸÄ±r duruÅŸ. GÃ¼Ã§ kÃ¼tleden geliyor. KÄ±lÄ±Ã§ bÃ¼yÃ¼k ama iÅŸlevsel â€” "oyun silahÄ±" deÄŸil.

**Armor / clothing:** Yamanan zÄ±rh parÃ§alarÄ± â€” gÃ¶ÄŸÃ¼s plakasÄ± + omuzluklar gerÃ§ek metal, altÄ±nda kalÄ±n gambeson + zincir. Bare head — kask yok, yüz açık. Pelerin: koyu kÄ±zÄ±l, yÄ±pranmÄ±ÅŸ, kirli, pratik. Renk: soluk siyah, paslanmÄ±ÅŸ demir, mat altÄ±n (eski soyluluk kalÄ±ntÄ±sÄ± â€” parlak deÄŸil).

**Effect level:** 1/10 â€” kÄ±lÄ±Ã§ metalinde ince soÄŸuk mavi Ã§atlaklar. Glow yok, yayÄ±lan enerji yok. Metalin iÃ§inden sÄ±zan eski bir kusur gibi.

**Grounded / fantasy:** 85% grounded. Tek fantastik unsur kÄ±lÄ±cÄ±n Ã§atlaÄŸÄ±.

### Create from Template Pro

| Alan | DeÄŸer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `warblade_south_lock.png` |

**Description:**
`two-handed greatsword held low, dark iron armor with crimson cloak, bare head no helmet no visor, mature adult proportions longer legs normal head-to-body ratio, sword length around 1.2x body height, full body visible head to boots, pixel art 128px transparent background`

### QC
```
[ ] Bare head — kask/visor/miğfer yok mu?
[ ] Pelerin yıpranmış/pratik, dramatik değil mi?
[ ] Kılıç çatlakları ince ve içsel — glowing değil?
[ ] Bacaklar görünüyor mu — squat/cüce değil mi?
[ ] Karakter hacimli — flat/sticker değil?
```

---

## RANGER

> **Kimlik kilidi:** "Tactical rift hunter (ruins/dungeon), NOT forest archer."

**Role in the world:** Eski bir izci deÄŸil â€” rift yarÄ±klarÄ±nÄ± takip eden, tuzak kuran, mesafeyi silah olarak kullanan bir avcÄ±. KÃ¶keni belirsiz, ekipmanÄ± toplanmÄ±ÅŸ.

**Silhouette basis:** Asimetrik utility yÃ¼kÃ¼ â€” bir kalÃ§ada kÄ±sa bolt-pack / trap canisters, kemerde tether spool. Yay uzun ama ince, rift-etched. Omuz hattÄ± Ã¶ne eÄŸik stalker posturu â€” heroic wide-leg duruÅŸu yok.

**Armor / clothing:** Charcoal / slate deri (yeÅŸil yok). DÃ¼ÅŸÃ¼k taktik cowl (bÃ¼yÃ¼k fantasy hood deÄŸil) + alt yÃ¼z sargÄ±sÄ±. Fonksiyonel katmanlar, yÄ±pranmÄ±ÅŸ.

**Effect level:** 2/10 â€” cold-blue ok ucu glow (sadece ok ucu, yay deÄŸil) + Tethering Arrow aktifken gerilen zincir Ã§izgisi (tether line). BÃ¼yÃ¼ dili deÄŸil, rift tool dili.

**Grounded / fantasy:** 80% grounded. Tek fantastik unsur ok ucunun cold-blue rift yÃ¼kÃ¼.

**Skill identity vurgusu:** kite-control > sniper. Tethering Arrow (8m snap) + Disengage/Tactical Roll kombini gÃ¶rsel imza. Trap canisters / tether spool gÃ¶rsel = Explosive Trap + Tethering Arrow mekanik.

### Silah Lock â€” KESÄ°N (session 19)
- **Tek elde yay + diÄŸer elde tek nocked arrow** (cold-blue tip)
- Elde serbest yÃ¼zen birden fazla ok **YASAK** â€” tutarsÄ±zlÄ±k yaratÄ±r
- SÄ±rtÄ±nda quiver: OK

### Create from Template Pro

| Alan | DeÄŸer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `ranger_south_lock.png` |

**Description:**
`bow fully drawn, single glowing arrow nocked, dark leather armor with hood and face wrap, pixel art 128px transparent background`

### QC
```
[ ] YeÅŸil ton var mÄ±? â†’ HAYIR olmalÄ±
[ ] Hood bÃ¼yÃ¼k mÃ¼? â†’ HAYIR, dÃ¼ÅŸÃ¼k taktik cowl
[ ] Elde kaÃ§ ok? â†’ SADECE BÄ°R nocked arrow, baÅŸka serbest ok yok
[ ] Asimetrik utility okunuyor mu? (belt items)
[ ] Ok ucu cold-blue â€” yay/vÃ¼cut parlak deÄŸil?
[ ] Stalker Ã¶ne-eÄŸik duruÅŸ â€” heroic pose deÄŸil?
```

---

## GUNSLINGER

> **Kimlik kilidi (Karar #38):** Rift-tech dual-pistol duelist. Western/kovboy arketip YASAK.

**Role in the world:** Rift enerjisini silaha dÃ¶nÃ¼ÅŸtÃ¼ren, koÅŸarken ateÅŸ eden bir dÃ¼elist. Taktik deÄŸil kinetik â€” duraklama yok.

**Silhouette basis:** Asimetrik duelist kÄ±yafet, aÃ§Ä±k saÃ§ akÄ±ÅŸÄ± overhead'de okunabilir. Ä°ki tabanca geniÅŸ duruÅŸ.

**Armor / clothing:** Uzun asimetrik duelist coat, altÄ±nda taktik harness + kemer kÄ±lÄ±flarÄ±. Copper-orange uzun saÃ§ â€” top-down aÃ§Ä±dan kimlik iÅŸareti. Dark teal / gunmetal palette, copper aksanlar.

**Effect level:** 2/10 â€” tabanca namlusunda ince rift-mavi enerji hattÄ±. Glow yok, spread yok.

**Grounded / fantasy:** 75% grounded. Tek fantastik unsur rift-enerjili tabancalar.

### SaÃ§ + Kimlik Lock â€” KESÄ°N (session 19)
- **Uzun copper-orange saÃ§** â€” tied back veya side flow, top-down aÃ§Ä±dan gÃ¶rÃ¼nÃ¼r olmalÄ±
- **Duelist coat** (asymmetric, uzun) â€” askeri spandex deÄŸil
- Western/kovboy arketip referansÄ± **YASAK**

### Create from Template Pro

| Alan | DeÄŸer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `gunslinger_south_lock.png` |

**Description:**
`long vivid orange flowing hair, dual energy pistols ready at hip, pixel art 128px transparent background`

### QC
```
[ ] SaÃ§ copper-orange ve top-down'dan gÃ¶rÃ¼nÃ¼r mÃ¼?
[ ] KÄ±yafet duelist coat â€” askeri/spandex deÄŸil mi?
[ ] Ä°ki tabanca okunuyor mu?
[ ] Western kovboy elementi var mÄ±? â†’ HAYIR olmalÄ±
[ ] Olgun oranlar â€” bacak uzunluÄŸu readable mÄ±?
```

---

## SHADOWBLADE

> **Kimlik kilidi:** HÄ±z ve kesinlik. Pozisyon her ÅŸey â€” kÄ±sa Ã¶lÃ¼mcÃ¼l pencereler.

**Role in the world:** GÃ¶lgede yaÅŸayan, rift-void enerjisini silah olarak kullanan bir suikastÃ§Ä±. KaynaÄŸÄ± bilinmiyor.

**Silhouette basis:** GeniÅŸ combat crouch, her iki elde uzatÄ±lmÄ±ÅŸ void bÄ±Ã§aklar. Asimetrik duruÅŸ.

**Armor / clothing:** Dark purple-black segmented tactical armor, pauldronlar ve diz korumalarÄ±. YÃ¼z: alt kÄ±sÄ±m dark shadow wrap cloth ile Ã¶rtÃ¼lÃ¼ â€” gÃ¶zler aÃ§Ä±k, gÃ¶zlerde soluk mor parÄ±ltÄ±.

**Effect level:** 4/10 â€” void bÄ±Ã§aklarÄ±ndan sÃ¼zÃ¼len ince mor alev wisp'leri. GÃ¶vde/zÄ±rh parlak deÄŸil.

**Grounded / fantasy:** 60% grounded. Fantastik unsurlar: void bÄ±Ã§aklar + gÃ¶zler.

### YÃ¼z Lock â€” KESÄ°N (session 19)
- **Full mask YASAK**
- **Ä°zin verilen istisna:** lower-face shadow wrap cloth â€” gÃ¶zler aÃ§Ä±k, mor parÄ±ltÄ± gÃ¶rÃ¼nÃ¼r
- "No face-concealing headgear" kuralÄ± **Gemini referans Ã¼retim fazÄ±na aittir** (yÃ¼z okunurluÄŸu iÃ§in)
- PixelLab Ã¼retim fazÄ±nda istisna: Shadowblade â lower-face wrap intentional. Warblade â bare head (helmet yok).
- DiÄŸer class'larda Gemini fazÄ±nda yÃ¼z aÃ§Ä±k kalÄ±r; PixelLab fazÄ±nda class tasarÄ±mÄ±na gÃ¶re karar verilir
- Neden: gÃ¶zler kimlik noktasÄ± â€” tam maske silÃ¼eti anonim yapar, class okunurluÄŸunu Ã¶ldÃ¼rÃ¼r

### Create from Template Pro

| Alan | DeÄŸer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `shadowblade_south_lock.png` |

**Description:**
`lower face wrapped in dark cloth eyes visible, dual short void blades, dark purple armor, pixel art 128px transparent background`

### QC
```
[ ] Alt yÃ¼z shadow wrap var mÄ± â€” tam maske deÄŸil mi?
[ ] GÃ¶zler gÃ¶rÃ¼nÃ¼r mÃ¼ â€” mor parÄ±ltÄ± okunuyor mu?
[ ] Ä°ki void bÄ±Ã§ak okunuyor mu?
[ ] GÃ¶vde/zÄ±rh parlak deÄŸil â€” sadece bÄ±Ã§ak wisps mÄ±?
[ ] Olgun oranlar, bacak readable mÄ±?
```

---

## SUMMONER

> **Kimlik kilidi:** Komutan. DoÄŸrudan savaÅŸmaz â€” varlÄ±klarÄ± yÃ¶netir, sahayÄ± kontrol eder.

**Role in the world:** Rift aracÄ±lÄ±ÄŸÄ±yla elementleri Ã§aÄŸÄ±ran bir savaÅŸ komutanÄ±. GÃ¼Ã§ doÄŸrudan deÄŸil, dolaylÄ±.

**Silhouette basis:** Bir elde ornate scepter, diÄŸer elde aÃ§Ä±k avuÃ§ â€” komuta jesti. Tiara + purple-gold robe soyluluk okumasÄ±.

**Armor / clothing:** Rich purple robe with gold trim, armored shoulderpiece. Kahverengi saÃ§ + mor gemstone tiara. TÃ¼m bÃ¼yÃ¼ efektleri Unity VFX katmanÄ±nda â€” sprite'ta bÃ¼yÃ¼k runic circle OLMAZ.

**Effect level:** 1/10 (sprite'ta) â€” sadece scepter crystal hafif parlar. Runic circle, Ã§aÄŸÄ±rma efekti, portal hepsi Unity VFX.

**Grounded / fantasy:** 65% grounded. Fantastik unsur: scepter + tiara.

### Efekt Lock â€” KESÄ°N (session 19)
- **SaÄŸ elde bÃ¼yÃ¼k runic circle YASAK** â€” Unity'de VFX olarak yapÄ±lacak, sprite'a iÅŸlenmez
- **Scepter + aÃ§Ä±k avuÃ§** yeterli â€” komutan hissi buradan gelir

### Create from Template Pro

| Alan | DeÄŸer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `summoner_south_lock.png` |

**Description:**
`golden scepter raised in one hand, other hand open commanding gesture, purple robes with gold trim, pixel art 128px transparent background`

### QC
```
[ ] SaÄŸ elde bÃ¼yÃ¼k runic circle var mÄ±? â†’ HAYIR olmalÄ±
[ ] Scepter net okunuyor mu?
[ ] Komutan pozu â€” pasif deÄŸil aktif mi?
[ ] Tiara gÃ¶rÃ¼nÃ¼yor mu?
[ ] Olgun oranlar, bacak readable mÄ±?
```

---

## ELEMENTALIST

> **Kimlik kilidi:** Kontrollü fırtına büyücüsü. Gösteriş değil ritim, alan ve zamanlama.

**Role in the world:** Rift fırtınalarını dizginleyip savaş alanını yöneten bir arkanist. Gücü ham patlama değil, doğru anda boşaltılan odaklı enerji.

**Silhouette basis:** Akışkan robe katmanları, küçük açık hood ve tek elde tutulan yıldırım orb'u ile okunur. Staff yok; kimlik eliyle taşıdığı enerji çekirdeğinden gelir.

**Armor / clothing:** Flowing blue-purple robe with cyan rune trims, small hood that does not cover the face, modest slightly open neckline, layered cloth belts and sleeves. Kumaş ağırlıklı, metal minimum.

**Effect level:** 3/10 — tek elde yoğun cyan-blue lightning orb, kısa kontrollü kıvılcım dili. Gövde ve robe üzerinde yaygın glow yok.

**Grounded / fantasy:** 70% grounded. Fantastik unsur: elde taşınan yıldırım orb'u ve rune trim enerjisi.

### Orb + Hood Lock — KESİN (session 20)
- **Tek elde lightning orb** zorunlu
- **Staff/wand YASAK**
- **Hood küçük ve yüz açık** — yüzü örten başlık YASAK

### Create from Template Pro

| Alan | Değer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `elementalist_south_lock.png` |

**Description:**
`open neckline, bare forearms visible, crackling lightning orb in one hand, pixel art 128px transparent background`

### QC
```
[ ] Tek elde lightning orb net okunuyor mu?
[ ] Staff veya wand var mı? → HAYIR olmalı
[ ] Hood küçük ve yüz açık mı?
[ ] Cyan rune trim robe üzerinde okunuyor mu?
[ ] Kamera overhead doğru, karakter flat değil mi?
```

---

## BRAWLER

> **Kimlik kilidi:** Yumrukla kıran yakın baskı sınıfı. Kütle ve tempo ile savaşır.

**Role in the world:** Çatlak bölgelerde çıplak kuvvetle hayatta kalmış bir ön hat dövüşçüsü. Silaha güvenmez; bedenini ve ivmesini silah yapar.

**Silhouette basis:** Geniş omuz, uzun dik gövde, belirgin çıplak kollar ve yumruk pozisyonu. Powerlifter squat değil, uzun ve ileri baskı yapan yapı.

**Armor / clothing:** Dark olive and worn brown leather harness setup, wrapped forearms, reinforced trousers and boots. Torso büyük ölçüde açık; purple rift-scar tattoos kas yapısı üzerinde okunur.

**Effect level:** 2/10 — dövme izlerinde düşük mor çatlak parıltısı, sadece yakın bölgede. Büyük aura/particle yok.

**Grounded / fantasy:** 80% grounded. Fantastik unsur: mor rift-scar dövme izlerinin içsel ışığı.

### Build + Weapon Lock — KESİN (session 20)
- **Tall muscular fighter silhouette** zorunlu
- **Squat/powerlifter oranı YASAK**
- **Silah YASAK** — yalnızca fists

### Create from Template Pro

| Alan | Değer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `brawler_south_lock.png` |

**Description:**
`both fists raised bare, leather harness, purple glowing tattoos on arms, pixel art 128px transparent background`

### QC
```
[ ] Silhouette uzun ve kaslı mı — squat değil mi?
[ ] Silah var mı? → HAYIR, sadece fists olmalı
[ ] Purple rift-scar tattoos okunuyor mu?
[ ] Harness/leather malzemesi net mi?
[ ] Kamera overhead doğru, sticker-flat görünüm yok mu?
```

---

## RONIN

> **Kimlik kilidi:** Sessiz ama ölümcül gezgin kılıç ustası. Keskin disiplin, temiz hat.

**Role in the world:** Dağılan düzenin ardından kendi koduyla yaşayan bir sürgün savaşçı. Çatışmaya hızla girip aynı hızla çıkar.

**Silhouette basis:** Clean hem robe formu, top-knot baş silüeti ve dual katana dili. Bir kılıç çekili, diğeri kında; asimetrik ama düzenli okunur.

**Armor / clothing:** Layered worn blue-green hakama robes, weathered cloth wraps, asymmetric single shoulder guard, practical belt and scabbard rig. Yıpranmış ama bakımlı, gereksiz aksesuar yok.

**Effect level:** 1/10 — neredeyse sıfır; metal kenarda çok hafif soğuk vurgu dışında enerji efekti yok.

**Grounded / fantasy:** 90% grounded. Fantastik unsur: yok denecek kadar az; kimlik tamamen disiplinli siluet ve hareketten gelir.

### Blade Readability Lock — KESİN (session 20)
- **Dual katana** zorunlu: biri drawn, biri sheathed
- **Top-knot görünür** olmalı
- **Robe hem temiz ve okunur** — dağınık/abartılı kumaş YASAK

### Create from Template Pro

| Alan | Değer |
|------|-------|
| Char Type | humanoid |
| Character Size | 128px |
| Camera View | high top-down |
| Concept Image | `ronin_south_lock.png` |

**Description:**
`dark indigo hakama, one katana drawn forward, one sheathed at waist, half-open chest wrap with scarred torso, pixel art 128px transparent background`

### QC
```
[ ] Dual katana doğru mu — biri çekili biri kında mı?
[ ] Top-knot top-down açıdan okunuyor mu?
[ ] Asymmetric shoulder guard görünür mü?
[ ] Blue-green hakama katmanları ve clean hem silhouette net mi?
[ ] Kamera overhead doğru, isometric/side-view kayması yok mu?
```
