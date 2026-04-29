# GÖRSEL STİL VE ÜRETİM REHBERİ
*2D Roguelite — Kesin Stil Kararları + Tüm Araç Workflow'ları*
*Aseprite · PixelLab · Gemini · Ollama Araştırma Sentezi*

---

## BÖLÜM 0 — OYUNUN GÖRSEL KİMLİĞİ (Kesin Kararlar)

Bu kararlar tasarım araştırmalarından çıktı. Değiştirme.

### Stil Referansları

| Referans | Ne Alıyoruz |
|----------|-------------|
| **Children of Morta** | Büyük sprite (48-64px), URP 2D Lighting, sıcak fener ışığı karanlık ortamda, normal map derinliği |
| **Enter the Gungeon** | Sade silüet okunabilirliği, net hitbox hissi, abartılı animasyon feedback |
| **Darkest Dungeon** | Renk paleti felsefesi — soğuk ortam, izole sıcak renkler |
| **Hades** | VFX okunabilirliği — efektler büyük, renkli, anlık |

**Tek cümle stil:** *"Children of Morta'nın ışığı, Enter the Gungeon'ın netliği, Darkest Dungeon'ın renk soğukluğu."*

---

### Kesin Boyutlar

| Varlık | Boyut | Neden |
|--------|-------|-------|
| Oyuncu karakteri | **64×64px** | PixelLab animate zorunluluğu (16/32/64/128/256 kare). Children of Morta da bu boyutu kullanıyor |
| Elite düşman | **64×64px** | Aynı sebep — oyuncuyla aynı boyut sistemi |
| Boss | **128×128px** | PixelLab'ın desteklediği en yakın büyük boyut |
| Grunt (temel düşman) | **32×32px** | Sürü halinde — performans + PixelLab uyumlu |
| Zemin tile | **16×16px** | Standart, Unity tile palette ile uyumlu |
| Duvar tile | **16×32px** | Üst kısmı görünür — top-down perspektif hissi |
| Prop (küçük) | **16×16px** | Fıçı, taş, kemik vb. |
| Prop (büyük) | **32×32px** | Sandık, sütun, kapı |
| Skill ikonu | **32×32px** | UI'da net okunabilir minimum boyut |
| Portre (class seçim) | **64×64px** | Seçim ekranında omuz üstü yüz — PixelLab uyumlu |
| VFX (küçük) | **32×32px** | Vuruş kıvılcımı, kan sıçraması |
| VFX (orta) | **64×64px** | Skill patlaması — PixelLab uyumlu |
| VFX (büyük/ultimate) | **128×128px** | Ultimate alanı, boss efekti — PixelLab uyumlu |

**Boyut Kuralı:** PixelLab animasyon araçları sadece **16, 32, 64, 128, 256** kare boyutlarını destekler.
Bu boyutların dışında üretim yapabilirsin ama animasyon araçları çalışmaz.

---

### Renk Paleti — KESIN DEĞERLER

```
─── ORTAM (Arkaplan, Zemin, Duvarlar) ──────────────────────────────
  #0a0a14   Neredeyse siyah — en koyu gölge
  #12121e   Temel zemin rengi
  #1e1e32   Açık zemin / duvar taban rengi
  #2e2e4a   Duvar yüzeyi, orta ton
  #3e3e5e   Açık taş, aydınlatılmış yüzey kenarı

─── KARAKTERLER (Ortamdan NET AYRIŞMALI) ───────────────────────────
  #c8a84b   Altın zırh, vurgu rengi (Warblade, Paladin)
  #e05252   Kırmızı enerji, kan, tehlike (Brawler, Berserker)
  #4a8fd4   Soğuk mavi, buz, büyü (Elementalist)
  #9e4fe0   Mor, karanlık büyü, zehir (Hexer, Summoner)
  #52c87a   Zehir yeşili, doğa, ok (Ranger, zehir VFX)
  #e09a30   Turuncu, ateş, sıcak ışık (meşale, Elementalist ateş)
  #d4d4f0   Soğuk beyaz-mor, kutsal ışık (Paladin, holy VFX)

─── DÜŞMANLAR (Oyuncudan FARKLI ama okunabilir) ────────────────────
  #3a1a1a   Koyu et tonu — zombi/veba kurbanı
  #6a4a3a   Orta et tonu
  #4a2a2a   Kan/yara rengi
  #1a2a1a   Zehirli düşman tonu
  #2a1a3a   Büyülü/sihirli düşman

─── SKİLL EFEKTLERİ (EN PARLAK — her şeyden önce dikkat çeksin) ────
  #ffffff   Beyaz impact flash (HIT anı — tüm saldırılarda)
  #ffe566   Altın enerji (Rage, Warblade burst)
  #ff4422   Ateş (Elementalist fire, Berserker rage)
  #22aaff   Buz/soğuk (Elementalist ice, Ranger frozen arrow)
  #cc22ff   Mor büyü (Hexer, Summoner)
  #22ff88   Zehir/biyolojik (Hexer DoT, Summoner)
  #ffaa22   Kutsal/holy (Paladin, Holy Power)

─── UI ──────────────────────────────────────────────────────────────
  #0d0d1a   UI arkaplan (slot, kart)
  #c8a84b   Altın çerçeve (normal kalite)
  #aa44ff   Mor çerçeve (rare kalite)
  #ff8822   Turuncu çerçeve (ultimate)
  #e05252   Kırmızı — HP düşük uyarı
  #52c87a   Yeşil — pozitif (heal, buff)
```

**Kural:** Bir karakter veya efekt tasarlarken en az **3 value adımı** fark olmalı (0-255 skalasında). Aksi halde arka plana karışır.

---

### Animasyon Timing — KULLANILACAK DEĞERLER
*Ollama araştırması (deepseek-r1:14b) + ETG/CoM/Hades analizi sentezi*

| Animasyon | FAZ 1 Min. | FAZ 5 Hedef | FPS | ms/frame | Kritik not |
|-----------|-----------|------------|-----|----------|------------|
| Idle | 4 | **6** | 6 | 167ms | Çok sallama — hafif nefes yeterli |
| Walk/Run | 6 | **8** | 10 | 100ms | 6 ucuz görünür, 8 çok daha akıcı |
| Attack (hafif) | 6 | **8** | 12 | 83ms | Windup+impact+recovery net görünmeli |
| Attack (ağır) | 8 | **10** | 8 | 125ms | Uzun windup = "telegraphed" = oyuncu kaçar |
| Dash/Dodge | 4 | **6** | 16 | 62ms | Hızlı + motion blur frame'i |
| Hit (hasar alınca) | **6** | **6** | 10 | 100ms | **3 frame çok az!** Frame 1: beyaz flash |
| Death | 8 | **10** | 8 | 125ms | Yavaş = dramatik |
| Skill Cast | 8 | **10** | 10 | 100ms | Windup görünür olmalı |
| Rage Activate | 6 | **8** | 10 | 100ms | Buildup + patlama + kararlı duruş |
| Boss Idle | 4 | **6** | 4 | 250ms | Çok ağır, tehditkar — neredeyse duruyor |
| Boss Attack (ağır) | 10 | **12** | 8 | 125ms | Uzun windup zorunlu |
| Boss Special/Charge | 12 | **16** | 8 | 125ms | Şarj aşaması net görünmeli |
| Boss Death | 12 | **16** | 6 | 167ms | En dramatik — en uzun |
| VFX Impact | 5 | **5** | 16 | 62ms | Hızlı = snappy his |
| VFX Aura/Glow | 8 | **8** | 10 | 100ms | Döngüsel |
| Ateş (meşale) | 4 | **6** | 8 | 125ms | Döngüsel |

**FPS mantığı:**
- 4-5 FPS → Çok ağır, kasıtlı yavaş (boss idle, sinematik an)
- 6-8 FPS → Ağır hareket (boss attack, ölüm, yavaş walk)
- 10-12 FPS → Standart oyuncu animasyonu (walk, attack, skill)
- 14-16 FPS → Hızlı, snappy (dash, hit flash, VFX impact)

**Solo dev stratejisi:**
FAZ 1 → "FAZ 1 Min." sütununu kullan (hızlı prototip)
FAZ 5 polishing → "FAZ 5 Hedef" sütununa yükselt

---

## BÖLÜM 1 — ASEPRİTE WORKFLOW (Düzeltilmiş)

### Genel Kurulum

```
Aseprite açılınca:
  Edit → Preferences → Editor:
    "Show Grid" aktif et (G tuşu toggle)

  Edit → Preferences → New Sprite:
    Genellikle ihtiyaca göre açıyorsun ama kısayol:
    File → New (Ctrl+N) → boyut gir

  Canvas boyutu her sprite için farklı — yukarıdaki tabloya bak.
  Arka plan: TRANSPARENT (şeffaf)
  Color mode: RGBA

  Grid: View → Grid → Grid Settings → 16×16px
  (Tile üzerinde çalışırken referans için)
```

### Temel Aseprite Araçları

```
Kalem (B): Piksel çizim — en çok kullanacağın
Silgi (E): Silme
Renk seçici (Alt+tık): Canvastan renk al
Dörtgen seçim (M): Alan seç, taşı, kopyala
Bucket fill (G): Alan doldur (dikkatli — AA yapar)
  → Preferences'ta "Pixel Art" modunda kullan (anti-aliasing YOK)

Önemli: Edit → Preferences → Defaults:
  Stroke: 1px
  Anti-aliasing: KAPALI
```

### Aseprite'ta Animasyon Workflow

```
Timeline (Ctrl+T ile aç/kapat):
  - Her animasyon frame'i ayrı bir "frame" (sütun)
  - Her layer ayrı görsel katman (satır)
  - Tag: frame grupları için (idle, walk, attack — Unity'de ayrı clip olacak)

Frame oluşturma:
  Sağ tık → "New Frame" veya F tuşu
  Frame kopyala: Alt+sürükle (bir önceki frame'i kopyalar, üstüne çiz)

Layer organizasyonu (önerilen):
  Layer "HITBOX"   → kırmızı dikdörtgen — Unity referansı için, export'ta kapat
  Layer "body"     → ana karakter
  Layer "weapon"   → silah (ayrı layer'da animasyon kolaylaşır)
  Layer "shadow"   → zemine düşen gölge (opsiyonel, küçük oval)

Onion skin (soğan zarı görünümü):
  View → Show Onion Skin (önceki/sonraki frame'i soluk gösterir)
  Animasyon yaparken AÇIK tut
```

### Export Ayarları

```
File → Export Sprite Sheet (Ctrl+Alt+Shift+S):

  Layout sekmesi:
    Sheet type: "Packed" veya "By Rows"
    → "By Rows" kullan: tek satır, tüm frame'ler soldan sağa
    → Columns: frame sayısına eşit

  Sprite sekmesi:
    Layers: "Visible layers" (HITBOX layer'ını KAPALI bırak)
    Frames: "All frames" veya Tag seç

  Borders sekmesi:
    Shape padding: 1px (sprite'lar arası boşluk — Unity'de bleeding önler)
    Inner padding: 0
    Trim: "Trim sprite" İŞARETLE (boş kenarları keser)

  Output sekmesi:
    Output file: ✅ — PNG olarak kaydet
    JSON data: ✅ — Hash formatı (Unity için)

  Save butonuna bas.
```

---

## BÖLÜM 2 — PIXELLAB ASEPRITE EKSTENSİYONU (Resmi Dokümana Dayalı)

> Kaynak: pixellab.ai resmi API dokümantasyonu + OpenAPI spec
> Bu bilgiler doğrudan resmi kaynaktan alındı.

---

### ⚠️ KRİTİK: CANVAS BOYUTU DEĞİŞİYOR

Önceki rehberde karakter boyutu 48×48px olarak belirlenmişti.
PixelLab animasyon araçları SADECE şu kare boyutları destekliyor:
**16, 32, 64, 128, 256px**

**48×48 çalışmıyor.** Karakter boyutunu **64×64px** olarak güncelliyoruz.

| Varlık | Eski | YENİ | Neden |
|--------|------|------|-------|
| Oyuncu karakteri | 48×48 | **64×64** | PixelLab animate zorunluluğu |
| Elite düşman | 48×48 | **64×64** | Aynı sebep |
| Grunt | 32×32 | **32×32** | Zaten doğruydu |
| Boss | 80×80 | **128×128** | En yakın desteklenen boyut |
| Skill ikonu | 32×32 | **32×32** | Zaten doğruydu |

64×64 aslında daha iyi — Children of Morta da bu boyutu kullanıyor.

---

### PixelLab'ın İki Modeli

```
PIXFLUX — Büyük sprite üretimi için
  Boyut: 32×32'den 400×400'e kadar
  Karakter: ~48×64 (portrait), ortam ve geniş sahneler için iyi
  Stil desteği: yok

BİTFORGE — Küçük-orta sprite + stil tutarlılığı için
  Boyut: max 80×80 (Tier 1) veya 140×140 (Tier 2+)
  Stil referansı: var (style_image parametresi)
  Pose kontrol: var (skeleton_keypoints)
  Küçük sprite'lar için tercih et
```

**Bizim kullanımımız:**
- Karakter sprite (64×64): Bitforge veya Pixflux
- Environment tile (16×16): Bitforge
- Boss (128×128): Pixflux

---

### PixelLab Generate — DOĞRU Prompt Formatı

```
❌ YANLIŞ (Stable Diffusion tarzı — PixelLab'da çalışmaz):
"pixel art, warrior knight, 16-bit, rpg character, full body, masterpiece,
best quality, high detail, transparent background, 64x64"

✅ DOĞRU (Doğal dil cümlesi — PixelLab'ın beklediği format):
"A heavily armored warrior knight with a greatsword and shield"

PixelLab ZATEN pixel art üretiyor — "pixel art" yazman gürültü ekler.
Boyutu (64x64) prompta yazma — arayüzdeki boyut seçiciden ayarla.
"masterpiece", "best quality" gibi SD token'ları işe yaramaz.
```

**Parametreler (prompt yerine arayüzden ayarlanır):**

| Parametre | Seçenekler | Bizim oyunumuz için |
|-----------|-----------|---------------------|
| `outline` | single color black, single color, selective, lineless | **single color black outline** |
| `shading` | flat, basic, medium, detailed, highly detailed | **medium** |
| `detail` | low, medium, highly detailed | **medium** |
| `view` | side, low top-down, high top-down | **low top-down** |
| `direction` | N, NE, E, SE, S, SW, W, NW | **S (south = kameraya bakıyor)** |
| `no_background` | true/false | **true** |

**Her karakter için prompt örnekleri:**

```
Warblade:
  "A heavily armored plague-era warrior knight with a greatsword and shield,
   dark medieval armor with golden trim"

Elementalist:
  "A dark-robed mage with a glowing fire orb in one hand and frost crystals
   forming in the other, arcane runes on the robes"

Rogue:
  "A hooded assassin in dark leather armor with dual daggers,
   shadowy cloak, lower face masked"

Ranger:
  "A lean archer in worn leather armor with a longbow and quiver,
   hood and bone accents, forest scout"

Brawler:
  "A martial arts monk with no weapons, hands wrapped in glowing chi bandages,
   battle-scarred muscular build"

Paladin:
  "A holy knight in gleaming silver armor with a sun symbol on the chest,
   faint divine light radiating from the breastplate"

Summoner:
  "A necromancer in dark robes with a skull-topped staff,
   three ghostly spirits floating around, gaunt hollow-eyed face"

Hexer:
  "A plague doctor with a long-beaked mask, dark coat,
   alchemical vials on the belt with green vapors, syringe in hand"
```

---

### PixelLab Animate — 3 Farklı Araç (Hangisini Kullanacaksın?)

#### Araç 1: Animate with Text (Standard)
```
Canvas: ZORUNLU 64×64px
Frame: 4 frame üretir
Kullanım: Karakter + aksiyon tanımı → animasyon

Workflow:
1. 64×64 canvas'ta karakterini üret (Generate ile)
2. Animate sekmesine geç
3. Character description: "heavily armored warrior knight" (karakteri tanımla)
4. Action: "walk", "run", "attack", "idle" gibi doğal dil
5. Direction: south, north, east, west
6. View: low top-down
7. Generate → 4 frame gelir

Action örnekleri (doğal dil yaz):
  idle:   "standing idle, breathing slowly"
  walk:   "walking forward"
  attack: "swinging sword horizontally"
  dash:   "dashing forward quickly"
  death:  "falling down and collapsing"
  hit:    "staggering backward from impact"
```

#### Araç 2: Animate with Text Pro
```
Canvas: 32×32 veya 64×64 → 16 frame üretir (4×4 grid)
        65-128px → 4 frame
Referans: Var olan karakter görselini referans olarak verebilirsin

16 frame = FAZ 5 polishing için ideal (daha akıcı animasyon)
FAZ 1 için 4 frame yeterli → Standard kullan
FAZ 5'te Pro'ya geç → aynı karakteri 16 frame'e yükselt
```

#### Araç 3: Animate with Skeleton (En Fazla Kontrol)
```
Canvas: 16, 32, 64, 128 veya 256px (kare)
Skeleton: Named joint'lar (NOSE, NECK, SHOULDER, ELBOW, vb.)

Workflow:
1. 64×64 karakterini canvas'ta aç
2. Animate with Skeleton sekmesi
3. "Set reference" butonuna bas → PixelLab skeleton'ı otomatik tahmin eder
4. Skeleton joint'larını kontrol et, yanlış ise sürükleyerek düzelt
5. "Rescale" bas (boyuta göre ayarla)
6. Animasyon için: ilk frame'i idle olarak ayarla → "Freeze 1 → Generate 2 frames"
7. Skeleton bir kez kaydedildi mi → diğer karakterler için yeniden kullanılabilir

Ne zaman kullan:
  → Özel pose istiyorsun (standart walk/attack dışı)
  → Diğer araçların verdiği animasyon tutarsız
  → Boss için (büyük sprite, daha fazla kontrol gerekir)
```

#### Hangi Araç — Ne Zaman?

```
FAZ 1 (hızlı prototip):
  → Animate with Text Standard
  → 64×64, 4 frame, doğal dil action

FAZ 4 (tüm sınıflar):
  → Animate with Text Standard (hız için)
  → Tutarsız olanları Skeleton ile düzelt

FAZ 5 (polishing):
  → Animate with Text Pro (16 frame)
  → Boss → Animate with Skeleton

Boss animasyonu:
  → Her zaman Skeleton kullan (128×128, kontrol şart)
```

---

### Negatif Prompt — HER ARAÇ İÇİN (PixelLab, Gemini, SD)

```
PixelLab'da "negative_description" (advanced seçenek) varsa:
  "blurry, anti-aliased, 3d render, realistic, low contrast, extra limbs"

NOT: PixelLab bu parametreyi "advanced" olarak işaretlemiş —
çoğu durumda sadece positif prompt yeterli.
Sonuç beğenilmezse negative ekle.

Gemini/Imagen için negatif prompt yok — başka araç gerekir.
SD/Midjourney için tam negative prompt listesi:
  "anti-aliased, smooth gradients, blurry, 3d render, realistic photo,
   low contrast, white background, watermark, text, extra limbs,
   deformed anatomy, modern style, cartoonish, sketch"
```

---

### PixelLab İnit Image (Referans Görsel ile Üretim)

```
Her araçta init_image (başlangıç görseli) verme imkânı var.
Bu önemli — referans vermek sonucu çok iyileştirir.

init_image_strength değerleri:
  0-300:   Renk tonu rehberliği (çok özgür)
  300-400: Kaba şekil + renk (blob gibi taslaktan başla)
  400-600: Orta — var olan sprite'ın varyasyonunu üret
  600-900: Detaylı — neredeyse bitmiş işe küçük düzeltme

Workflow (önerilen):
  1. Kaba bir iskelet/siluet çiz Aseprite'ta (renkli bloblar yeterli)
  2. Init image olarak ver → strength: 350
  3. Generate → beğendiğini al
  4. Beğendiğini yeni init image olarak ver → strength: 500
  5. Tekrar generate → daha rafine sonuç
  ⚠️ guidance_scale'i çok yüksek koyma (1-20 arası, default 8 iyi)
     Çok yüksek → over-saturation ve artifactlar
```

---

### PixelLab Img2Img — Inpainting

```
Her üretim aracı init_image destekliyor = img2img.
Ayrıca tam Inpainting özelliği var:

Inpainting workflow:
  1. Üretilmiş sprite'ı aç
  2. Yeni paint layer ekle
  3. Değiştirmek istediğin alanı siyahla boya (mask)
  4. "Modify current layer" ile generate → sadece o alan yeniden üretilir

Ne zaman kullan:
  → Sprite'ın tamamı güzel ama bir kol/silah yanlış çıktı
  → Yüz/maske bölümünü değiştirmek istiyorsun
  → Rengi bir bölgede değiştirmek istiyorsun
```

### PixelLab Animate — Nasıl Kullanılır

```
1. Canvas'ta HAZIR bir sprite olmalı (tek frame, base pose)
2. PixelLab → Animate sekmesini aç
3. Animasyon tipini seç veya describe et
4. Generate bas → frame'ler oluşur
5. Frame'leri incele, kötü olanları sil/düzelt

GERÇEKÇI BEKLENTI:
- PixelLab'ın animasyonları her zaman mükemmel olmaz
- Genellikle 2-3 frame iyi olur, 1-2 frame bozuk
- Bozuk frame'leri Aseprite'ta elle düzelt (silgi + kalem ile)
- Tam otomatik mükemmel animasyon bekleme — hibrit çalışma gerekir
```

### Negatif Prompt — HER ARAÇ İÇİN (PixelLab, Gemini, SD)

```
Negatif prompt olmadan AI istemediğin şeyler üretir.
Ollama araştırması bu listeyi doğruladı.

PIXEL ART ÜRETMEK İÇİN HER ZAMAN EKLE:
  "anti-aliased, smooth gradients, blurry, 3d render, realistic photo,
   low contrast, white background, watermark, text, extra limbs,
   deformed anatomy, modern style, cartoonish, sketch"

Her terimin neden gerekli olduğu:
  anti-aliased      → kenar piksellerinin yumuşatılmasını önler
  smooth gradients  → geçişlerin bulanık olmasını önler (pixel art'ta yasak)
  blurry            → genel bulanıklık önlenir
  3d render         → 3D görüntü üretilmesini önler
  realistic photo   → fotoğraf gerçekçiliği yerine pixel art ister
  low contrast      → küçük sprite'ta okunabilirlik şart
  white background  → şeffaf arka plan istiyoruz
  modern style      → ortaçağ/dark fantasy dışına çıkmasın

PixelLab'da "negative prompt" alanı varsa buraya yaz.
Yoksa (bazı versiyonlarda yok) → ana prompt'a "avoid realistic style" ekle.
```

### PixelLab Img2Img — Ne Zaman Kullanılır?

```
Kullanım senaryoları:
  ✅ Gemini'den gelen referans görseli → pixel art'a dönüştür
  ✅ Mevcut sprite'ın farklı renk varyasyonu (aynı şekil, farklı palette)
  ✅ Düşmanın "elite" versiyonu (temel grunt'tan oluştur, biraz farklılaştır)
  ✅ Silah değişimi (Warblade Greatsword → Shield Stance)
```

---

### PixelLab Alternatifi: Creature2D (Ücretsiz)

```
Ollama araştırması bunu önerdi — ücretsiz, Aseprite entegrasyonlu.

Creature2D nedir:
  Skeleton-based animasyon aracı
  Aseprite ile entegre çalışıyor
  Öğrenmesi kolay, ücretsiz
  Çıktı: spritesheet veya ayrı frame'ler

Ne zaman kullan:
  PixelLab'ın animasyon sonuçlarından memnun değilsen
  Daha fazla kontrol istiyorsan (kemik yerleşimini kendin ayarlarsın)
  PixelLab'ı almadan önce test etmek istiyorsan

İndir: creature.kestrelmoon.com veya GitHub'da ara "Creature2D"

PixelLab vs Creature2D:
  PixelLab    → AI prompt tabanlı → sonuç daha hızlı ama öngörülemez
  Creature2D  → Kemikleri kendin kurarsın → daha fazla kontrol, biraz daha zaman
  Öneri: PixelLab'ı dene, sonuç beğenmediğin animasyonlar için Creature2D'ye geç
```

---

## BÖLÜM 3 — GEMİNİ ENTEGRASYONU (Konsept + Referans Üretimi)

### Gemini Ne İşe Yarar Bu Projede?

Gemini direkt olarak pixel art üretmiyor (veya çok iyi değil).
Ama şu işler için **mükemmel**:

```
1. KONSEPT ART:
   Karakterin genel görünümünü kafanda netleştirmek için
   → Gemini'de gerçekçi/semi-realistic görsel üret
   → Aseprite'ta bu görsele bakarak pixel art olarak çiz

2. RENK PALETI REFERANSI:
   Karakterin renk şemasını görmek için
   → "Color palette for a plague doctor character, dark fantasy, 5 colors" gibi
   → Hex değerleri al, Aseprite paletine ekle

3. ENVIRONMENT MOOD BOARD:
   Zindan oda görünümü, atmosfer referansı
   → Tek bir görsel üret, tonalite kararları için kullan

4. SKİLL EFEKT REFERANSI:
   "Hexblast explosion effect" gibi prompt → yüksek kalite VFX referansı
   → PixelLab'da Img2Img ile pixel art'a dönüştür

5. PROTOTİP HIZLANMASI:
   Bir sınıfı hızlıca test etmek istiyorsun → Gemini'de portreyi üret
   → Doğrudan oyuna placeholder olarak koy (yüksek çözünürlük)
   → Sonra pixel art versiyonunu üret
```

### Gemini'ye Nasıl Erişirsin?

```
Seçenek 1 — Gemini Web (En Kolay):
  → gemini.google.com gidin
  → Sohbet kutusuna yaz: "Generate an image of..."
  → (Görsel üretim özelliği aktif olmalı — Gemini 2.0 gerekiyor)

Seçenek 2 — Google ImageFX (En İyi Kalite):
  → labs.google/fx/tools/image-fx
  → Imagen 3 modeli — yüksek kalite
  → Sadece görsel üretim

Seçenek 3 — Gemini API (Kod ile):
  → Şu an için gerekli değil
```

### Gemini Prompt'ları — Oyun İçin Hazır Liste

---

#### KARAKTER KONSEPT GÖRSELLERİ (Aseprite'ta çizmeden önce)

> **GEMİNİ/IMAGEN PROMPT KURALI (Ollama doğruladı):**
> Kısa doğal dil cümleleri — keyword listesi değil.
> 1-2 cümle yeterli. "8K resolution" gibi SD modifier'ları ekleme.
> Stil için: concept art veya illustration → pixel art'a çevirmek en kolay.

```
WARBLADE:
"Dark fantasy warrior knight in plague-era medieval armor, heavy plate with golden trim,
greatsword at the back, front-facing full body concept art, illustration style,
dark navy and gold tones, dramatic torch lighting."

ELEMENTALIST:
"Dark fantasy elementalist mage, flowing dark robes with glowing runes,
fire orb in one hand frost crystals in the other, front-facing full body concept art,
purple and blue tones with orange fire accents, illustration style."

ROGUE:
"Dark fantasy hooded assassin, tight dark leather armor, dual daggers at hips,
shadow cloak, lower face masked, front-facing full body concept art,
black and deep purple tones, illustration style."

RANGER:
"Dark fantasy ranger archer, worn leather armor with bone accents, longbow in hand,
hood and quiver, front-facing full body concept art,
dark green and brown tones, illustration style."

BRAWLER:
"Dark fantasy brawler monk, no weapons, hands wrapped in glowing chi bandages,
battle-scarred muscular build, front-facing full body concept art,
dark copper and earth tones with golden energy glow, illustration style."

PALADIN:
"Dark fantasy holy knight paladin, gleaming silver armor with sun holy symbol,
faint divine light radiating from chest, front-facing full body concept art,
silver and white with warm golden glow against dark background, illustration style."

SUMMONER:
"Dark fantasy necromancer summoner, dark robes, skull-topped staff,
three floating ghostly spirits surrounding, gaunt hollow-eyed face,
front-facing full body concept art, deep purple and sickly green, illustration style."

HEXER:
"Dark fantasy plague doctor, iconic long-beaked plague mask, long dark coat,
alchemical vials on belt with toxic green vapors, syringe in hand,
front-facing full body concept art, black coat with green and amber accents, illustration style."
```

---

#### ENVIRONMENT MOOD BOARD

```
ZINDAN ODA:
"Dark medieval dungeon room seen from above, worn stone floor and cracked walls,
wall-mounted torch casting amber light, bones and rusted armor scattered,
concept art illustration style, dark navy and stone grey tones."

HUB ALANI:
"Plague-era medieval safehouse interior from top-down view, campfire at center,
wooden crates and barrels, makeshift workbench, dark stone walls,
warm firelight against cold surroundings, concept art illustration style."

BOSS ODASI:
"Massive dark dungeon boss chamber top-down view, circular room with broken pillars,
plague-infected altar at center, dim torchlight from walls,
ominous atmosphere, concept art illustration style, dark fantasy."
```

---

#### SKİLL EFEKTİ REFERANSLARI (PixelLab Img2Img için)

> Gemini ile VFX referansı üretirken kısa tut, efekti tanımla, tarzı belirt.

```
HEXBLAST:
"Toxic magic explosion from above, green and purple energy burst with skull motifs,
circular blast pattern, dark background, game VFX concept art illustration."

HOLY LIGHT:
"Divine holy light beam from above, golden white rays descending, glowing ground circle,
sacred rune patterns, dark background, game VFX concept art illustration."

FIREBALL:
"Fireball explosion from above, orange-red-yellow spherical burst, dark smoke edges,
white hot core, dark background, game VFX concept art illustration."

SHADOW STRIKE:
"Shadow teleport assassination effect from above, dark purple-black smoke burst,
shadow tendrils, afterimage silhouette, dark background, game VFX concept art."

RAGE ACTIVATE:
"Rage power-up aura from above, circular red-orange energy burst from center,
crackling electricity, intense glow, dark background, game VFX concept art."

GRUDGE BADGE SET:
"Set of small circular resistance badge icons for dark fantasy game:
fire skull, ice crystal, slash mark, dark eye, holy cross, poison drop,
pixel art style, dark background, icon design."
```

---

#### BOSS KONSEPT GÖRSELİ

```
BOSS 01 — VEBA LORDU:
"A massive plague lord boss character for a top-down 2D action roguelite.
Enormous dark corrupted armor, a cracked plague doctor mask fused into the helmet,
multiple decayed limbs or plague-infected appendages, glowing toxic green veins
running through the armor. Towering and intimidating. Semi-realistic digital concept art,
front-facing full body, dark fantasy plague medieval aesthetic."
```

---

### Gemini → Aseprite Workflow (Adım Adım)

```
SENARYO: Warblade'i çizmeden önce konsept oluştur.

ADIM 1 — Gemini'de üret:
  gemini.google.com aç → "Generate an image of..." → yukarıdaki Warblade prompt'u yaz
  → En beğendiğin görseli indir (sağ tık → "Resmi farklı kaydet")
  → Kaydet: ART/referanslar/warblade_konsept_ref.jpg

ADIM 2 — Aseprite'ta referans olarak aç:
  Aseprite'ta yeni dosya aç: 48×48px transparent
  File → Import → referans görselini seç
  VEYA: Edit → Paste from Clipboard
  → Görseli ayrı bir layer'a koy: "REF" adını ver
  → Bu layer'ı yeniden boyutlandır: Image → Scale → 48×48
  → Layer opacity'i %40'a düşür (çok baskın olmasın)

ADIM 3 — Üstüne çiz:
  Yeni layer oluştur: "body" (REF layer'ının üstünde)
  Referansa bakarak 48×48 alanda pixel art olarak çiz
  → Renkler için yukarıdaki paleti kullan, referanstan direkt renk alma
  → Silueti önce çiz (1px outline), sonra içini doldur

ADIM 4 — REF layer'ını kapat/sil:
  Bitince REF layer'ını kapat veya sil
  Sadece çizdiğin sprite kalacak

ADIM 5 — PixelLab Img2Img ile alternatif:
  Gemini'den gelen görseli PixelLab → Img2Img'e ver
  "Convert to pixel art, 48x48, top-down 2D character, dark fantasy" yaz
  → Otomatik pixel art versiyonu üretilir
  → Beğenirsen kullan, beğenmezsen elle çiz
```

---

## BÖLÜM 4 — ARAÇ SEÇİM REHBERİ

Her varlık için hangi araca başvuracaksın?

```
─── OYUNCU KARAKTERLERİ (Warblade vb.) ─────────────────────────────

  ÖNCE: Gemini → konsept görsel üret (Warblade prompt'u)
  SONRA seç:
    Seçenek A (Öğrenme): Gemini referansına bakarak Aseprite'ta elle çiz
      → Daha uzun sürer ama sonuç en çok senin
    Seçenek B (Hız): PixelLab Generate → 48x48 sprite üret, düzelt
      → Daha hızlı ama sonuç jenerik olabilir
    Seçenek C (Hibrit / ÖNERİLEN): Gemini → PixelLab Img2Img → Aseprite'ta düzelt
      → Gemini konsepti verince PixelLab bunu pixel art'a çevirir
      → Aseprite'ta küçük düzeltmeler yaparsın

─── ANİMASYON (Tüm karakterler) ────────────────────────────────────

  PixelLab → Animate ile otomatik üret
  → Bozuk frame'leri Aseprite'ta elle düzelt
  → Beklenti: Her animasyonun %60-70'i PixelLab'dan gelecek,
    %30-40'ı elle düzeltme

─── DÜŞMANLAR (Grunt, Elite) ───────────────────────────────────────

  PixelLab Generate → direkt üret (referans gerek yok)
  Animasyon: PixelLab Animate

─── BOSS ────────────────────────────────────────────────────────────

  Gemini → yüksek kalite konsept görsel
  PixelLab Img2Img → pixel art'a dönüştür (80x80 veya 96x96)
  Aseprite → detay ekle, palette uyarla

─── ENVIRONMENT / TILESET ──────────────────────────────────────────

  PixelLab Generate → tek tile üret, prompt:
  "seamless tileable dungeon floor, pixel art, 16x16, dark stone, no border"
  Aseprite → 4 varyasyon oluştur (hafif farklılıklar el ile)

─── SKİLL İKONLARI ─────────────────────────────────────────────────

  PixelLab Generate → 32x32, icon formatında
  Referans: Gemini'den icon konsept alabilirsin ama direkt PixelLab daha hızlı

─── VFX / EFEKTLER ─────────────────────────────────────────────────

  Seçenek A: Gemini → VFX referans → PixelLab Img2Img → pixel art
  Seçenek B: Aseprite'ta elle çiz (VFX genellikle soyut — elle yapmak kolay)
  ÖNERİ: Çoğu VFX'i Aseprite'ta elle yap, karmaşık olanları Gemini referansla

─── PORTRELER (Class seçim ekranı) ─────────────────────────────────

  Gemini → yüksek kalite portre görsel (64x64 için uygun)
  PixelLab Img2Img → pixel art versiyonu
  Aseprite → ince ayar
```

---

## BÖLÜM 5 — NORMAL MAP WORKFLOW (Netleştirildi)

### Laigter Kullanımı (Ücretsiz, En Kolay)

```
İndir: github.com/azagaya/laigter/releases

Kullanım:
1. Laigter'ı aç
2. Sol üst köşe → "Open Image" → sprite PNG'yi aç
3. Sol panelde ayarlar:
     "Depth" slider: 0.7 (karakter için iyi başlangıç)
     "Bumpiness": 0.5
     "Soft": 0.3
   → Canvas'ta normal map önizlemesini görürsün (mavi-yeşil-kırmızı)
4. Ayarları sprite'ın siluetine göre düzenle
5. Sol üst → "Export" → PNG olarak kaydet
6. Dosya adı: warblade_front_idle_NormalMap.png (aynı klasöre)

Unity'de atama:
  Project'te sprite'ı seç
  Inspector → "Secondary Textures" bölümü
  "+" butonuna bas
  Name: _NormalMap  (Tam bu şekilde — büyük/küçük harf önemli!)
  Texture: NormalMap PNG'yi sürükle
  "Apply" bas
```

### Hangi Sprite'lara Normal Map Şart?

```
✅ ŞART:          Oyuncu (tüm sınıflar)
✅ ŞART:          Boss
✅ Önemli:        Elite düşmanlar
✅ Önemli:        Büyük çevre props (sütun, sandık, kapı)
⚠️ Opsiyonel:    Grunt düşmanlar
❌ Gerekmez:      Zemin tile'ları
❌ Gerekmez:      Küçük props (fıçı, taş)
❌ Gerekmez:      VFX
❌ Gerekmez:      UI
```

---

## BÖLÜM 6 — UNITY'E AKTARMA (Hızlı Referans)

```
Tüm sprite import ayarları:
  Texture Type: Sprite (2D and UI)
  Sprite Mode: Multiple (spritesheet) / Single (tek sprite)
  Pixels Per Unit:
    Karakter (48px): 48 PPU
    Tile (16px): 16 PPU
    Büyük props (32px): 32 PPU
    → Kural: PPU = sprite'ın pixel genişliği = Unity'de 1 unit
  Filter Mode: Point (No Filter)  ← UNUTURSAN bulanık görünür
  Compression: None

Spritesheet dilim:
  Sprite Editor → Slice → Grid By Cell Size
  Cell size: sprite boyutuna göre (48×48 karakter için 48×48)

```

---

## BÖLÜM 7 — FAZ 1 BAŞLANGIÇ SIRASI (Yeni — Önce Bunları Yap)

```
[ ] ADIM 1: Warblade konsept görseli → Gemini'de üret (Bölüm 3 prompt'u)
[ ] ADIM 2: warblade_front_BASE → PixelLab Generate veya Gemini Img2Img
[ ] ADIM 3: warblade_front_idle (4 frame) → PixelLab Animate
[ ] ADIM 4: warblade_front_walk (6 frame) → PixelLab Animate
[ ] ADIM 5: warblade_front_attack1 (5 frame) → PixelLab Animate
[ ] ADIM 6: Normal map → Laigter ile
[ ] ADIM 7: Unity'e aktar, PPU ayarla, animasyon test et
    → Eğer güzel görünüyorsa back ve left yönleri de yap
    → Güzel görünmüyorsa Aseprite'ta düzelt, tekrar export et

[ ] ADIM 8: Grunt → PixelLab Generate (32×32) + Animate
[ ] ADIM 9: Dungeon floor tile (16×16) → PixelLab Generate
[ ] ADIM 10: Dungeon wall tile (16×32) → PixelLab Generate
[ ] ADIM 11: Torch prop (4 frame anim) → PixelLab Generate + Animate
[ ] ADIM 12: vfx_hit_sparks (5 frame) → Aseprite'ta elle çiz (basit)
[ ] ADIM 13: ui_hpbar + ui_ragebar → Aseprite'ta elle çiz

→ Bu 13 adım tamamlanınca Unity'de ilk playtest yapılabilir.
```

---

*Bu rehber Ollama araştırması (PIXELART_ARASTIRMA_CIKTISI.md) tamamlanınca güncellenecek.*
*Araştırma çalışıyor — tamamlanınca frame sayıları ve timing doğrulanacak.*
