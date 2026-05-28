# Codex Review: Image #14 — 16 Chibi Karakter Diversity Question

## Görev
User Image #14'te 16 chibi karakter grid'i gösterdi (4×4). RIMA 10 sınıf canonical kimlikleri ile karşılaştırıp:
1. Her slot **hangi sınıfa** en yakın eşleşir?
2. **(4,2) mavi/yeşil saçlı karakter** ve **(4,4) siyahi kadın karakteri** RIMA'ya nasıl entegre edilebilir?
3. **Diversity expansion** (skin variant per class) Karar #74/#100/#143-E/#144/#145 ile uyumlu mu?

## RIMA Canonical Class Identity (NLM verified)

| Sınıf | Saç | Ten/Outfit | Silah/Aksent |
|---|---|---|---|
| Warblade | dark short messy hair + dark masculine beard | beyaz ten + dark brown leather armor | brass buckle, claymore |
| Elementalist | honey-blonde low bun | beyaz ten + dusty indigo crop top + cream sash + deep teal skirt | rune disk (sol elinde) |
| Shadowblade | dark hair (short/medium) | soluk ten + dark robe + soft purple aksent | phase blade |
| Ranger | bleached-ivory uzun saç | beyaz ten + forest tones (green/khaki/brown) | uzun yay + sadak |
| Ravager | dark messy hair | beyaz ten + dark blood-red armor + leather harness + iron studs | iki elli balta |
| Ronin | dark short hair (samurai topknot/tail) | beyaz ten + dark navy haori | katana |
| Gunslinger | dark short hair | beyaz ten + grey-purple trench coat | iki tabanca, kemer |
| Brawler | bald / shaved | dark skin + simple wrap top + bandage hands | yumruk |
| Summoner | dark hair (long/loose) | beyaz ten + dark teal robe | rune staff |
| Hexer | dark hood + uzun siyah saç | soluk ten + dark cloak + curse-themed aksent | curse focus |

Notlar:
- Karar #100: chibi 64×64, 3-4 head proportion, high top-down 30-35°
- Karar #144: silahsız body + WeaponSR child SR (silahlar runtime child)
- Karar #145: PixelLab Character States workflow
- Memory: Brawler "REGEN" candidate (dark-skin already canonical), Image #12'de 3 sınıf drift (Warblade beard yok, Elementalist robe, Ravager shirtless)

## Image #14 Detaylı Slot-by-Slot Tanımı (Opus tarafından çıkarılmış)

Image 4×4 grid (16 karakter), her chibi yaklaşık 64×64 px. Slotlar:

### Row 1 (üst)
- **(1,1):** Beyaz ten + dark short hair + hafif beard/stubble. Dark teal/black coat with belt. Masculine. **Olası eşleşme:** Warblade (eğer beard görülürse) veya Ronin
- **(1,2):** Beyaz ten + reddish-honey kısa saç (yarım toplu) + female. Dark teal/forest sleeveless top + dark pants. **Olası eşleşme:** Elementalist (honey-blonde drift?) veya Ranger
- **(1,3):** Beyaz ten + black short hair + masculine. Dark navy/black robe. **Olası eşleşme:** Ronin veya Shadowblade
- **(1,4):** Beyaz ten + dark messy hair + masculine. **Dark red/maroon armor + leather harness + iron studs** (Ravager canonical match!) **Olası eşleşme:** Ravager (CANONICAL FIT)

### Row 2 (üst-orta)
- **(2,1):** Soluk ten + dark hood + dark cloak (yüz hafif gölgede). Feminine. **Olası eşleşme:** Hexer
- **(2,2):** **Dark skin** + bald/shaved kafa + masculine. Plain light wrap top + bare arms. **Olası eşleşme:** Brawler (CANONICAL FIT — memory'deki "REGEN" candidate)
- **(2,3):** Beyaz ten + dark hair (slightly longer) + masculine. Dark coat with belt. **Olası eşleşme:** Shadowblade veya farklı Ronin
- **(2,4):** Beyaz ten + blonde/honey hair (medium length) + female. **Green/khaki forest outfit** with brown belt. **Olası eşleşme:** Ranger (CANONICAL FIT — bleached-ivory + forest tones)

### Row 3 (alt-orta)
- **(3,1):** Beyaz ten + uzun siyah saç + female. Dark teal/black robe. **Olası eşleşme:** Summoner (canonical dark teal robe + dark hair) veya Hexer female variant
- **(3,2):** **Dark skin** + red/auburn kısa saç + female. Dark armor with belt. **Olası eşleşme:** New variant — Ravager dark-skin female? Veya yeni karakter
- **(3,3):** Beyaz ten + **gri/silver hair** + beard + masculine. Dark coat. **Olası eşleşme:** NPC mentor (Cinematic Layer V1?) veya Gunslinger drift
- **(3,4):** Beyaz ten + **red hair** (uzun) + female. Dark outfit with belt. **Olası eşleşme:** Elementalist red-hair drift (Image #12'deki drift!) veya Ravager female variant

### Row 4 (alt — user'ın spesifik sorduğu)
- **(4,1):** **Dark skin** + bald/shaved + neutral/feminine. **Beige/tan simple robe** (monk-like). **Olası eşleşme:** Brawler alt variant veya NPC monk
- **(4,2):** **Soluk ten + teal/blue/green hair** (medium) + feminine. **Purple/lavender outfit** with brown belt. **EŞLEŞME YOK** — hiçbir canonical RIMA sınıfına uymaz. Yeni karakter/NPC potansiyeli
- **(4,3):** Beyaz ten + **beyaz/silver hair** + beard + masculine. Dark coat. **Olası eşleşme:** Mentor NPC veya Gunslinger gri-saç drift
- **(4,4):** **Dark skin** + female + dark short hair + dark outfit (sleek). **Olası eşleşme:** Yeni karakter veya class skin variant (Ranger dark-skin? Ravager dark-skin?)

## User'ın Sorusu (Spesifik)

**(4,2) mavi/yeşil saçlı karakter** + **(4,4) siyahi kadın karakter** RIMA'ya eklenebilir mi? **"Aktif classlara da eklenebilir"** dedi.

## 3 Olası Yol

### Yol 1: Class Skin Variant System (Hades-style)
- 10 sınıf lock'u korunur
- Her sınıfa **2-3 skin variant** eklenir (cinsiyet + etnisite çeşitliliği)
- Örn:
  - Ranger: Beyaz-blonde (canonical) + Dark-skin female (4,4 olabilir)
  - Brawler: Dark-skin male (canonical) + Dark-skin female (4,4) + Dark-skin monk (4,1)
  - Elementalist: Honey-blonde (canonical) + Teal/blue hair (4,2 — Frost variant olarak rebrand)
- Skin **tamamen kozmetik** — mekanik/skill etkilenmez
- PixelLab Character States workflow ile **yarı-otomatik üretim** (Karar #145)

### Yol 2: NPC Hub Characters
- (4,1), (4,2), (4,3), (4,4) **NPC olarak Hub'da** (Rift Break) yer alır
- Vendor, mentor, lore-keeper karakterler
- Cinematic Layer V1 (TASARIM/CINEMATIC_LAYER_v1.md) entegrasyonu

### Yol 3: 11-12 Class Expansion (RIMA scope-creep riski)
- (4,2) blue-haired = yeni Frost/Ice Mage class
- (4,4) dark-skin female = yeni Assassin/Rogue class
- 10-class lock **bozulur** — Karar #100 ihlali
- 80 skill pool → 96-110 skill expansion
- **RED FLAGGED — scope explosion**

## Codex'ten İstenen

### Q1: 16 slot eşleştirmesi
- Her slot için "hangi RIMA sınıfına en yakın" verdict (Opus tahminlerini doğrula/düzelt)
- Hangi slot **canonical fit**, hangi slot **drift/regenerate**?
- Image #12 ile cross-check: (1,1) Warblade beard düzeldi mi? (3,4) Elementalist red-hair drift devam mı?

### Q2: (4,2) blue/teal-haired karakter
- RIMA 10 sınıftan **hangisine** rebrand edilebilir (Frost Elementalist mantıklı mı)?
- Yoksa NPC olarak Hub'da kullanılmalı mı?
- 11. class eklemek ihlal mi (Karar #100)?

### Q3: (4,4) siyahi kadın karakter
- Ranger / Ravager / Brawler dark-skin female variant olarak kullanılabilir mi?
- Diversity expansion için en uygun sınıf hangisi?

### Q4: Skin Variant System (Yol 1) verdict
- PASS / MODIFY / FAIL
- Karar #74/#100/#143-E/#144/#145 uyum kontrolü
- Üretim maliyeti tahmini (PixelLab Character States ile kaç batch?)
- Sprint hedef önerisi

### Q5: NPC Hub Characters (Yol 2) verdict
- PASS / MODIFY / FAIL
- Cinematic Layer V1 entegrasyonu mantıklı mı?
- Hub karakterleri kaç tane gerekir (vendor + mentor + lore-keeper)?

### Q6: 11-12 Class Expansion (Yol 3) verdict
- Beklenen: FAIL (scope-creep)
- Ama doğrulama gerekir — Path C olabilir mi?

### Q7: Diversity stratejisi final önerisi
- Yol 1 / Yol 2 / Yol 3 / kombinasyon
- Sayısal hedef (kaç skin variant, kaç NPC, vs.)
- Sprint sıralaması

## Çıktı
`STAGING/codex_review_image14_diversity_DONE.md` — 1000-1800 kelime. ASCII-only.
