# RIMA — Logo Tasarımı ve Görsel Kimlik
*Son güncelleme: 2026-03-29 | Aseprite + PixelLab + Oyun Teması*

---

## BÖLÜM 1 — LOGO KONSEPTİ

### RIMA Neden Bu Logo?

RIMA = "dar derin çatlak, yarık." Logo da tam bunu yansıtmalı.

### Final Logo Tasarımı

```
R  I          M  A
      ↘ft           ↘rch

Gizli okuma: RI+ft = RIFT | MA+rch = MARCH
```

- `RIMA` büyük, bold, tek bakışta okunur — ana marka bu
- `ft` → "I" harfinin sağ-alt köşesinden kırılıp sarkmış, aşağı-sağa eğimli
- `rch` → "A" harfinin sağ-alt köşesinden kırılıp sarkmış, aynı açı
- Her iki sarkık parça da küçük, kırık dokulu, hafif dönemli — düşen moloz gibi
- Kırılma noktalarında `#FFD700` altın glow — kopan yerde ışık sızıyor
- Dikkatli bakan `RIFT MARCH`'ı görür — easter egg değil, katman

**Kimlik cümlesi:** *"Kırık ama duran. Yarık ama var."*

**ÖNEMLİ:** "ft" ve "rch" altbilgi/subtitle değil — ana harflerin kırık parçaları.
"RI" ile "ft" arasında BOŞLUK YOKTUR, break point'te harfler bitişir, oradan ayrılır.

### Logo Versiyonları

| Versiyon | Kullanım | Boyut |
|----------|----------|-------|
| Ana Logo | Menü, splash screen, Steam page | 320×80px |
| Kompakt | UI köşesi, loading screen | 160×40px |
| İkon | Steam küçük ikon, uygulama ikonu | 64×64px (sadece çatlak motifi) |
| Tek Renk | Dark background üzerinde white | Her boyut |

---

## BÖLÜM 2 — ASEPRİTE'TA LOGO YAPIMI

### Adım 1: Canvas Kurulumu

```
File → New
  Width:  320px
  Height: 80px
  Color mode: RGBA
  Background: Transparent

View → Grid → Grid Settings → 8×8px
(Harf tasarımında alignment için)
```

### Adım 2: Harf Geometrisi

Her harf **60×64px**, harfler arası **8px** boşluk.
Toplam: (60×4) + (8×3) = 264px — 320px canvas'ta ortala (28px kenar).

**Harf tasarım kuralları:**
```
Stroke: 6px kalınlık (bold, ağır hissettirmeli)
Serif: YOK — geometrik, keskin, sert
Corner: Düz köşeler (rounded kesinlikle yok)
Style: Slab-pixel — Dead Cells / Darkest Dungeon logo ruhunda
```

**R harfi (60×64px):**
```
Dikey gövde: sol kenar, 6px kalın, tüm yükseklik
Üst yatay: en üstten 6px yükseklik, 36px uzunluk
Orta yatay: tam orta, 36px uzunluk
Bacak: orta yataydan sağ-alt köşeye diagonal (6px kalın)
```

**I harfi (24×64px):**
```
Tek dikey çubuk: 6px kalın
Üst serif: 6px yüksek, 24px geniş
Alt serif: 6px yüksek, 24px geniş
```

**M harfi (72×64px):**
```
Sol dikey: 6px kalın
Sağ dikey: 6px kalın
Orta V: tepeden başlar, ortaya kadar iner (6px kalın çizgi)
Yatay bağlantı YOK — ters V şeklinde iki diagonal
```

**A harfi (60×64px):**
```
Sol diagonal: sol-alt'tan tepeye, 6px kalın
Sağ diagonal: sağ-alt'tan tepeye, 6px kalın
Yatay çizgi: tam orta, 48px uzunluk, 6px yüksek
Alt köşeler açık (A'nın altında delik olacak)
```

### Adım 3: Çatlak Çizgisi

```
Yeni layer: "crack"
Renk: #000000 (siyah — harfleri keser)

Çatlak rotası (diagonal, ~20° eğim):
  Başlangıç: R harfinin üst-sağ köşesinden (canvas ~105px, 4px)
  → I harfinin ortasından geçer (canvas ~150px, 30px)
  → I ve M arası boşluktan geçer (canvas ~185px, 38px)
  → M harfinin alt-sol köşesine doğru (canvas ~220px, 64px)
  Bitiş: M harfinin dışına taşar (canvas ~235px, 80px)

Çatlak genişliği: 4px (2px siyah + 2px glow)
Kalem ile elle çiz — düz olmasın, hafif kıvrımlar ekle (doğal çatlak hissi)
Çatlak harfleri keserek geçmeli — harfler bu noktada "kırılmış" görünür
```

### Adım 4: Offset (Kırılma Etkisi)

```
Yeni layer: "crack_offset"

Çatlağın SOL-ÜST tarafındaki harf pikselleri:
  Selection → seç → 2px yukarı + 1px sola kaydır

Çatlağın SAĞ-ALT tarafındaki harf pikselleri:
  Selection → seç → 2px aşağı + 1px sağa kaydır

Bu iki yarım artık tam oturmuyor — aralarında çatlak var.
```

### Adım 5: Çatlak Işığı (Glow)

```
Yeni layer: "crack_glow" (crack layer'ının altına)

Çatlak boyunca 1px mesafede:
  İç glow: #FFD700 (altın sarı) — 100% opacity
  Dış glow 1: #C8A832 — 60% opacity
  Dış glow 2: #A07820 — 30% opacity
  Dış glow 3: #604010 — 10% opacity (çok ince yayılım)

Teknik: "çatlak" pathini izleyerek her iki yanına 1'er pixel ekle, opacity düşür.

Sonuç: Çatlak sanki içeriden aydınlatılıyor gibi görünür.
Kintsugi'nin karanlık versiyonu — altın ama heal eden değil, sızan.
```

### Adım 6: Ana Harf Rengi

```
Harf rengi seçenekleri (konsept test et):

Seçenek A — Koyu Çelik (önerilen):
  Ana dolgu:  #1E1E32
  Highlight:  #3E3E5E (üst kenar — hafif aydınlık)
  Shadow:     #0A0A14 (alt kenar — çok koyu)

Seçenek B — Kemik Beyazı (alternatif):
  Ana dolgu:  #D4D4F0
  Shadow:     #8A8AB0

Seçenek C — Void Siyahı (minimal):
  Ana dolgu:  #080808
  Edge:       #2E2E4A

Test: Her iki versiyonu da siyah ve koyu arkaplan üzerinde dene.
Koyu çelik (#1E1E32) genellikle en iyi kontrast verir.
```

### Adım 7: Arkaplan Çatlak Motifi (Opsiyonel)

```
Yeni layer: "bg_cracks" (en alta)

Logunun arkasına hafif, çok ince çatlak çizgileri:
  Renk: #1A1A2E (çok koyu, neredeyse görünmez)
  Kalınlık: 1px
  Sayı: 5-8 ince çatlak, çeşitli açılardan geçiyor
  Opacity: 20-30%

Bu arkaplan dokusu logun "boşlukta" değil "kırılmış bir yüzeyde" durduğunu hissettirir.
```

### Adım 8: Export

```
Logo export (tam versiyon):
  File → Export Sprite Sheet
  Sheet type: Single frame
  Trim: Kapalı (sabit boyut istiyoruz)
  Output: PNG, transparent background

Boyutlar üret:
  rima_logo_320x80.png   (ana versiyon)
  rima_logo_160x40.png   (File → Scale → 50%)
  rima_icon_64x64.png    (sadece çatlak motifi — harf yok)
```

---

## BÖLÜM 3 — LOGO REFERANSI İÇİN PİXELLAB PROMPT

Logo'yu Aseprite'da elle yapmadan önce concept referans üretmek için:

```
PixelLab Generate (Pixflux, 320×80 veya 256×64):

"Dark fantasy pixel art game logo: large bold uppercase RIMA in dark steel,
 small broken lowercase ft drooping below-right of the I letter at an angle,
 small broken lowercase rch drooping below-right of the A letter at same angle,
 ft and rch look like cracked debris falling from the main letters,
 gold glowing light #FFD700 bleeding at the break points,
 void black background #080808"

Parametreler:
  outline: single color black
  shading: detailed
  no_background: false (siyah bg istiyoruz)

⚠️ Bu sadece referans içindir. Final logo Aseprite'da elle yapılmalı.
Gemini ile konsept üret → Aseprite'ta precise pixel çalışması yap.
```

---

## BÖLÜM 4 — RIMA GÖRSEL KİMLİĞİ

### Tema Felsefesi: "Kırılmış Ama Duran"

RIMA = dar çatlak. Oyunun her görsel elemanı bu temayı taşımalı:

- **Her yüzeyde çatlak var** — taş, zırh, boss, UI çerçevesi
- **Çatlaklar cansız değil** — ışık sızıyor, enerji akıyor
- **Kırılma sahnelerin DNA'sı** — zemin çatlakları sadece dekor değil, düşmanlar oradan çıkıyor
- **Karakterler de çatlamış** — savaşçının zırhı kırık, ama terk etmemiş

**Referans sanat terimi:** Kintsugi (金継ぎ) — ama karanlık versiyonu.
Japonlarda kırık seramiği altınla onarırlar. Bizde dünya kırıldı, onarılmadı.
Altın çatlaklar iyileşmenin değil, yarığın kendi enerjisinin işareti.

### Brand Renkleri (RIMA Identity)

```
─── RIMA MARKA PALETİ ─────────────────────────────────────────────
  #080808   Void Black — logo arkaplanı, UI arka planı
  #1E1E32   Deep Steel — ana harf rengi, panel arka planı
  #FFD700   Crack Gold — çatlak ışığı, aktif/selected state
  #C8A832   Aged Gold — secondary vurgu, non-active crack
  #D4D4F0   Ghost White — metin rengi, secondary
  #604010   Deep Amber — shadow/glow falloff

─── AKT BAĞLANTISI ─────────────────────────────────────────────────
  Act 1 crack glow:  #7BA7BC  (soğuk mavi — taze çatlak)
  Act 2 crack glow:  #9E4FE0  (mor — derinleşmiş rift)
  Act 3 crack glow:  #FFD700  (altın — çekirdeğe yaklaşım)
  Hub crack glow:    #D4956A  (mum sıcağı — mühürlü ama canlı)
```

### Görsel Dil Kuralları (RIMA Temasına Göre)

**1. Zemin ve Tile'lar:**
```
Her akt zemin tile'ında çatlak texture olmalı:
- Act 1: İnce, yeni çatlaklar. Soğuk mavi glow.
- Act 2: Geniş çatlaklar, içlerinden organik şeyler çıkıyor.
- Act 3: Zeminlerin yarısı yokluk — sadece çatlak çizgileri kalmış.
```

**2. UI Çerçeveleri:**
```
Skill slot çerçevesi: Tek düz dikdörtgen değil — köşelerde kırık çizgiler
Skill kartları: Kartın kenarında çatlak motifi
HP bar çerçevesi: Her çeyrekte bir çatlak var — HP düştükçe çatlaklar derinleşiyor
Rarity renkleri değişiyor:
  Common:    Gri çatlak (#808080)
  Rare:      Mavi çatlak (#4A8FD4)
  Epic:      Mor çatlak (#9E4FE0)
  Legendary: Altın çatlak (#FFD700) — logo ile aynı renk
```

**3. Karakterler:**
```
Tüm class sprite'larında:
- Zırh/giysi üzerinde en az bir görünür çatlak/yırtık
- Çatlaktan class'ın rengi sızıyor (Warblade: kırmızı, Elementalist: mavi/turuncu vs.)
- "Savaşa girmiş, ama yıkılmamış" hissi
```

**4. Boss'lar:**
```
Her boss üzerinde büyük çatlaklar:
- Iron Warden: Zırhın göğsünde dev bir çatlak, soğuk mavi enerji
- Fractured King: Tüm vücudu çatlak haritası gibi, her çatlak farklı renk
- Hollow Sovereign: Vücut yokluk, çatlaklar onun şeklini çiziyor (çatlak = form)
- Nexus Core: Sadece çatlaklar var — gövdesi tamamen kırık cam gibi
```

**5. Efektler:**
```
Skill hit efektleri: Her vuruşta küçük çatlak deseni yayılıyor (vurgu)
Death efekti: Düşman ölünce çatlaklar yayılıyor, sonra dissolve
Boss faz geçişi: Ekranda dev çatlak geçiyor (screen crack efekti)
Run başlangıcı: Siyah ekranda tek bir çatlak büyüyor, oyun başlıyor
```

---

## BÖLÜM 5 — ACT GÖRSEL KİMLİKLERİ (RIMA Temalı Güncelleme)

### Act 1 — "Yüzey Çatlakları" (The First Rift)

```
Hikaye: Dünyanın kırıldığı yer. Taze, ham çatlaklar.
        Henüz tam açılmamış, ama kapanmayacağı belli.

Renk:   Soğuk, gri-mavi. Toz. Enkaz.
        Çatlak rengi: #7BA7BC (buz mavisi)

Çevre:  Taş yapılar yıkılıyor ama ayakta duruyor.
        Zeminlerde ince çatlaklar — genişlemiyor henüz.
        Çatlak kenarları keskin, yeni.

Işık:   Global düşük, soğuk. Meşaleler sönmüş.
        Sadece çatlaklardan sızan soluk mavi glow.

PixelLab ortam referans prompt:
  "Top-down view of freshly cracked dungeon stone floor,
   thin blue glowing fissures running through grey stone,
   cold atmosphere, dust particles, collapsed architecture visible,
   dark fantasy environment"
```

### Act 2 — "Derin Yarıklar" (The Bleeding Rift)

```
Hikaye: Çatlaklar çok açıldı. Rift'in öte tarafından
        varlıklar sızıyor. Organik, büyüyen bir bozulma.

Renk:   Derin mor ve yıpranmış yeşil.
        Çatlak rengi: #9E4FE0 (void moru)

Çevre:  Çatlaklar artık geniş — içlerinden organik dokular fışkırıyor.
        Mantar, köklere benzer şeyler, biyolüminesans.
        Zemin bazı yerlerde tamamen yok — sadece void.

Işık:   Mor ambiyans. Çatlaklar parlak ve aktif.
        Organik yapılar zayıf yeşil glow.

PixelLab ortam referans prompt:
  "Top-down view of corrupted cavern floor, wide glowing purple rifts,
   organic tendrils and mushroom growth emerging from cracks,
   bioluminescent green patches, dark void visible in deepest cracks,
   abandoned dark fantasy dungeon"
```

### Act 3 — "Rima'nın Özü" (The Core Fracture)

```
Hikaye: Orijinal kırılma noktasına yaklaşıyorsun.
        Burada gerçeklik neredeyse yok. Sadece çatlaklar var.

Renk:   Neredeyse siyah. Sadece altın çatlak çizgileri.
        Çatlak rengi: #FFD700 (saf altın)

Çevre:  Zemin "çatlak haritası" — taş değil, void üzerinde çatlak deseni.
        Yerçekimi yok hissi. Bazı tile'lar havada asılı.
        Her şey geometrik parçalara bölünmüş.

Işık:   Çok koyu. Sadece çatlak glow'u aydınlatıyor.
        Oyuncu ışığı çatlak rengine dönüşüyor (altın).

PixelLab ortam referans prompt:
  "Top-down view of fractured void floor, golden glowing cracks on
   near-black surface, floating stone fragments, geometric shard patterns,
   no solid ground just fracture lines over darkness,
   end-game dark fantasy realm"
```

### Hub — "Eşik" (The Threshold)

```
Hikaye: Bir riftin içinde mühürlü yer. Güvenli.
        Çatlaklar var ama altın harçla doldurulmuş (Kintsugi).
        Burası hem en güvenli hem en kırılgan yer.

Renk:   Sıcak. Mum ışığı. Altın ve kahve tonları.
        Çatlak rengi: #8B6914 (eski altın, mühürlü)

Çevre:  Taş duvarlar — ama çatlaklar altın dolguyla kapatılmış.
        Meşaleler yanıyor. Halı, ahşap mobilya.
        Her köşede küçük Kintsugi objeleri var (yıkık ama onarlı şeyler).

PixelLab ortam referans prompt:
  "Top-down view of ancient stone sanctuary, walls with golden-sealed cracks
   in Kintsugi style, warm candlelight atmosphere, worn rug on stone floor,
   mysterious alcoves with figures, dark wood furniture,
   safe haven dark fantasy hub room"
```

---

## BÖLÜM 6 — OLLAMA İLE GÖRSEL AÇIKLAMALAR

Bu prompt'lar Ollama (llama3.2) ile oyun içi art açıklamaları üretmek için:

### Sanat Yönü Açıklaması Üretici

```python
system_prompt = """Sen RIMA oyununun sanat yönetmenisin.
RIMA, dar çatlak anlamına gelir. Oyunun dünyası kırılmış gerçekliğin parçalarından oluşuyor.
Her görsel eleman şu temayı taşıyor: "Kırılmış ama duran — çatlaktan ışık sızıyor."

Verilen sprite/asset açıklaması için:
1. Oyun içi codex girişi (2-3 cümle, birinci kişi gözlemci)
2. Görsel özellikler listesi (bullet points)
3. PixelLab için doğal dil prompt (tek paragraf, natural language, SD token yok)

Ton: Melankolik, gözlemci, kısa. Şiirsel ama aşırıya kaçma."""

user_prompt = """Asset: Shard Walker
Tip: Grunt düşman, 32×32px, Act 1-3
Tanım: Kırık taş ve kemik parçalarından oluşan insansı form, parçalar koyu enerjiyle
birbirine bağlı, aralarından soğuk mavi ışık sızıyor."""
```

### Hızlı Prompt Batch (Tüm Mob'lar İçin)

```python
mob_list = [
    "Shard Walker — kırık parçalardan oluşan insansı form",
    "Seam Crawler — zemin çatlağında yaşayan, sadece pençeleri görünür",
    "Echo Hound — yarı transparan kopyalar bırakan yırtıcı",
    "Void Thrall — göğsünden void sızan, ölünce ikiye bölünen insansı",
    "Hollow Mite — küçük içi boş sürü yaratığı",
    "The Twice-Born — bir ipliğe bağlı iki kimlikli varlık",
    "Fracture-Born — zemin çatlağından tırmanarak çıkan",
    "Spore Hollow — çatlaklarından mantar yapılar fışkıran boş kabuk",
    "Rift Maw — duvara yapışık çatlak ağzı, çeker ve spawn eder",
    "Class Mimic — oyuncunun class siluetini taklit eden",
    "Remnant Host — üç ruh barındıran, kontrol sürekli değişen beden",
    "The Wound — havada asılı kayan gerçeklik yarası"
]

for mob in mob_list:
    # Her mob için yukarıdaki system_prompt ile çalıştır
    pass
```
