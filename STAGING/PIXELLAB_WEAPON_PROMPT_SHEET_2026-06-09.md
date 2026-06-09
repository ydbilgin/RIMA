# RIMA Silah Prompt Sayfası — PixelLab create_1_direction_object
Hazırlık: 2026-06-09

## Kullanım Özeti
1. Her blokta `size` kare (örn. `size: 64`). Native üret, küçültme/scale YOK.
2. Birden fazla aday gelir; review edip en iyisini seç (seçim kriteri her blokta belirtildi).
3. Üretimi sen yaparsın; ben mount/pivot/mirror'ı sonradan oturtacağım.
4. `style_images` kullanılırsa `size` parametresi girilmez (boyutu style görseli belirler). Bu sayfada style_images YOK.

---

## Batch A — Küçük (32 px)

### Gunslinger — Rift-Tech Tabanca
- `size:` 32
- `view:` sidescroller
- `beklenen aday:` 64
- **prompt:** `a single rift-tech pistol, sleek futuristic sci-fi frame with glowing orange-red rift energy vents, accent color #FF4400, sidescroller game asset, no character, no hands, no environment, transparent background, horizontal-right (handle left tip right), no western, no revolver, no flintlock, no holster`
- **seçim kriteri:** Gövde slim/teknolojik görünmeli; rift detayı (parlayan oyuk/çizgi) olmalı; western/kovboy hissi SIFIRLANDI mı kontrol et.
- **Unity pivot:** Bottom-center (x=0.5, y=0.0)
- **mount notu:** Tek sprite üret; L-el offhand = runtime flipX ile mirror. Mount-profil yaması gerekecek.

---

### Shadowblade — Reverse-Grip Hançer
- `size:` 32
- `view:` sidescroller
- `beklenen aday:` 64
- **prompt:** `a single slender dagger, thin sharp blade with clean void-purple edge tint, accent color #5A2A8A, reverse-grip orientation (tip pointing down-left), sidescroller game asset, no character, no hands, no environment, transparent background, horizontal-right (handle left tip right), no glow, no aura, no magical effect`
- **seçim kriteri:** Bıçak ince ve sert görünmeli; void moru sadece metal kenar tonu olmalı (parlayan emission YOK); reverse-grip yönü "tip down/left" olmalı.
- **Unity pivot:** Center (x=0.5, y=0.5)
- **mount notu:** Tek sprite üret; offhand = runtime flipX. Mount-profil yaması gerekecek.

---

## Batch B — Orta (48 px)

### Elementalist — Floating Golden Rune Disc
- `size:` 48
- `view:` top-down
- `beklenen aday:` 16
- **prompt:** `a single flat circular rune amulet disc, golden engraved rune glyphs on surface, glowing yellow accent color #FFF000, centered game asset, isolated floating object, no character, no hands, no environment, transparent background, centered, no staff, no wand, no rod, no handle`
- **seçim kriteri:** Tamamen yuvarlak/disk şeklinde olmalı; asa veya sap YOK; altın rün sembolleri net; parlama yumuşak (emission değil, metalik parıltı).
- **Unity pivot:** Center (x=0.5, y=0.5)
- **mount notu:** Avuç-üstü hover — karakter elinin bilek noktasına mount edilecek, offset +0.1y. Mount-profil yaması gerekecek.

---

### Hexer — Grimoire / Cursed Totem
- `size:` 48
- `view:` sidescroller
- `beklenen aday:` 16
- **prompt:** `a single cursed grimoire tome, cracked leather cover with dark red blood rune markings, iron clasps and bone-fragment binding, accent color #8B0000, sidescroller game asset, no character, no hands, no environment, transparent background, centered, no whip, no academic decoration, no bright colors`
- **seçim kriteri:** Karanlık/lanetli estetik; akademik temiz kitap DEĞİL (Elementalist'ten ayrı olmalı); koyu kırmızı kemik/deri detayı olmalı.
- **Unity pivot:** Center (x=0.5, y=0.5)
- **mount notu:** Tutan el pozisyonuna göre mount-profil yaması gerekecek.

---

### Summoner — Soul Lantern
- `size:` 48
- `view:` sidescroller
- `beklenen aday:` 16
- **prompt:** `a single ornate soul lantern with cold cyan spectral glow emanating from within, dark iron frame with soul energy wisps, accent color #00FF88, sidescroller game asset, isolated floating object, no character, no hands, no environment, transparent background, centered, no staff, no swing handle`
- **seçim kriteri:** Soğuk/ruhsal cyan ışık; demir çerçeve rustik/gotik görünmeli; staff veya saplı obje DEĞİL — asılı/süzülen fener.
- **Unity pivot:** Center (x=0.5, y=0.5)
- **mount notu:** Sol el hover — avuç yanı mount. Karakter "tutar" gibi görünmez; floating offset. Mount-profil yaması gerekecek.

---

## Batch C — Büyük (64–96 px)

### Ronin — Katana (Çekili)
- `size:` 64
- `view:` sidescroller
- `beklenen aday:` 16
- **prompt:** `a single drawn katana, clean polished steel blade with faint hamon temper line, pale gold brass tsuba guard and wrapped tsuka handle, accent color #C8A87A, sidescroller game asset, no character, no hands, no environment, transparent background, horizontal-right (handle left tip right), no glow, no fantasy effect`
- **seçim kriteri:** Klasik katana proporsiyon (uzun ince bıçak, kısa kabza); tsuba (guard) belirgin; hamon çizgisi olursa bonus; soluk altın/pirinç ton hakim.
- **Unity pivot:** Bottom-center (x=0.5, y=0.0)
- **mount notu:** Sol bel kını (scabbard) ayrı sprite olacak — bu sadece çekili bıçak. Mount-profil yaması gerekecek.

---

### Ranger — Compound Bow (Mekanik, Sol El)
- `size:` 64
- `view:` sidescroller
- `beklenen aday:` 16
- **prompt:** `a single tactical compound bow, asymmetric cam and pulley system, cold blue accent limb details, accent color #7BA7BC, sidescroller game asset, no character, no hands, no environment, transparent background, horizontal-right (riser left limbs extending right and up), no forest decorations, no natural wood curves, no quiver`
- **seçim kriteri:** Mekanik/taktiksel kam sistemi görünür olmalı; asimetrik profil; doğal orman okçusu estetiği SIFIRLANDI mı kontrol et.
- **Unity pivot:** Center (x=0.5, y=0.5)
- **mount notu:** Sol el tutar (riser kolu); ok ayrı sprite. Mount-profil yaması gerekecek.

---

### Ravager — Kısa Balta (Hatchet)
- `size:` 64
- `view:` sidescroller
- `beklenen aday:` 16
- **prompt:** `a single crude short hatchet, wide heavy chopper blade with blood red weathered nicks, short thick wooden handle wrapped in leather cord, accent color #D43F3F, sidescroller game asset, no character, no hands, no environment, transparent background, horizontal-right (handle left blade right), no long handle, no poleaxe, no elegant finish`
- **seçim kriteri:** KISA sap + büyük/ağır balta kafası; kaba/vahşi işçilik; uzun saplı balta DEĞİL; kan kırmızısı (#D43F3F) Hexer'in koyu kırmızısından (#8B0000) ayrışmalı.
- **Unity pivot:** Bottom-center (x=0.5, y=0.0)
- **mount notu:** Tek sprite üret; offhand ikinci balta = runtime flipX. Mount-profil yaması gerekecek.

---

### Warblade — İki Elli Greatsword (Cyan Rift)
- `size:` 96
- `view:` sidescroller
- `beklenen aday:` 4
- **prompt:** `a single massive two-handed greatsword, dark steel blade with subtle cyan rift energy crack running along the flat, plain brass crossguard, brown leather-wrapped long grip, accent color #C09455 brass with cyan rift detail, low-guard horizontal resting pose, sidescroller game asset, no character, no hands, no environment, transparent background, horizontal-right (handle left tip right), no ornate decoration, no glowing blade, no magical flames`
- **seçim kriteri:** Büyük/ağır greatsword proporsiyon; süssüz işçi estetiği; cyan çatlak/rift ince bir detay olmalı (tüm bıçak parlamamalı); pirinç guard belirgin ama sade.
- **Unity pivot:** Bottom-center (x=0.5, y=0.0)
- **mount notu:** Mount sistemi HAZIR (Warblade mount-profil mevcutta çalışıyor). Diğer sınıflar için baz referans olarak kullanılabilir.

---

## Brawler — Silah Yok

Brawler silah üretimi YOK. Yumruk/deri sargı tamamen kozmetik; isteğe bağlı olarak ayrı bir seansta 32px deri el sargısı cosmetic üretilebilir (o zaman `no gloves, no boxing wraps branding, plain leather hand wrap strip` prompt kullan).

---

## Üretim Sırası Önerisi

1. **Elementalist diski** (48px, Batch B) — demo'nun tek görsel eksiği; avuç-hover sistemi test edilecek.
2. **Gunslinger + Shadowblade** (Batch A, 32px) — küçük ve hızlı; 64 aday → rahat seçim.
3. **Hexer + Summoner** (Batch B, 48px) — Elementalist diskiyle aynı batch mantığı.
4. **Ronin + Ranger + Ravager** (Batch C, 64px) — orta süre.
5. **Warblade** (96px, 4 aday) — en büyük; mount zaten hazır, son kontrole bırak.
