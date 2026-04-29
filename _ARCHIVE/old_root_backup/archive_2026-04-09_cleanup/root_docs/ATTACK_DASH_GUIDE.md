# RIMA — Attack & Dash Animasyon Üretim Rehberi
> **Araç:** PixelLab Site → Animate with text PRO  
> **Hedef:** 4 karakter × (3 attack + 1 dash) × 8 yön  
> **Son güncelleme:** 2026-04-08

---

## Özet: Ne Üretilecek

| Animasyon | Klasör adı | Frame sayısı | Loop |
|---|---|---|---|
| Birinci vuruş | `attack-1` | 10 | Hayır |
| İkinci vuruş | `attack-2` | 10 | Hayır |
| Final vuruş | `attack-3` | 10 | Hayır |
| Kaçış/Dash | `dash` | 8 | Hayır |

Her animasyon → **8 yön** → her yön ayrı klasör.

---

## Klasör Yapısı — Nereye Kaydet

```
F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\
│
├── Shadowblade\
│   └── animations\
│       ├── attack-1\
│       │   ├── south\        ← frame_000.png ... frame_009.png
│       │   ├── south-east\
│       │   ├── east\
│       │   ├── north-east\
│       │   ├── north\
│       │   ├── north-west\
│       │   ├── west\
│       │   └── south-west\
│       ├── attack-2\         ← aynı yapı
│       ├── attack-3\         ← aynı yapı
│       └── dash\             ← aynı yapı (8 frame)
│
├── Elementalist\  (aynı yapı)
├── Ranger\        (aynı yapı)
└── Warblade\      (aynı yapı)
```

**Frame isimlendirme:**
```
frame_000.png
frame_001.png
frame_002.png
...
frame_009.png   ← attack için (10 frame)
frame_007.png   ← dash için (8 frame)
```

---

## Referans Görseller — Nereden Al

Her animasyonu üretmeden önce o yönün **reference** görselini alacaksın.  
Bu görsel, karakterin renk, stil ve silah tutarlılığını kilitler.

```
F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\{Karakter}\reference\

Örnek:
  Shadowblade\reference\south.png       ← south yönü için kullan
  Shadowblade\reference\east.png        ← east yönü için kullan
  Shadowblade\reference\north-west.png  ← north-west için kullan
  (8 yönün hepsi mevcut)
```

> Her yön için o yönün reference görselini kullan.  
> south animasyonu üretiyorsan → `reference/south.png`  
> north-east üretiyorsan → `reference/north-east.png`

---

## Araç: PixelLab Site — Animate with text PRO

### Nereye Git
1. `https://www.pixellab.ai/create` aç
2. Sol panelde **"Animate"** sekmesine geç
3. **"Animate with text PRO"** butonuna tıkla  
   *(Açıklaması: "Add animation to existing image")*

### Ekranda Ne Göreceksin
- **Üst alan:** Resim yükleme alanı — "Paste from clipboard" butonu var
- **View dropdown:** `None / Side / Low top-down / High top-down / Oblique (3/4 view)`
- **Direction dropdown:** `None / South / South-East / East / North-East / North / North-West / West / South-West`
- **Text alanı:** Animasyon tanımını yazacağın yer
- **Generate** butonu

### Her Yön İçin Adımlar

```
1. reference/{yön}.png dosyasını kopyala
2. Sayfadaki "Paste from clipboard" butonuna tıkla → resim yüklenir
3. View → "Low top-down" seç
4. Direction → ürettiğin yönü seç (örn: "South")
5. Text alanına aşağıdaki promptlardan uygun olanı yaz
6. Generate → sonucu izle
7. Beğendiysen: sağ tıkla → Farklı kaydet
8. Beğenmediysen: promptu değiştir → tekrar Generate
```

### Kaç Yönde Üret? Önerilen Sıra

Önce **south** → beğenince diğerlerine geç.  
South iyi çıkmazsa promptu düzeltmek en ucuz bu aşamada.

```
Üretim sırası:
south → east → north → west → south-east → south-west → north-east → north-west
```

Diagonal yönler (NE/NW/SE/SW) bazen siyah/boş çıkabilir — alt kısımdaki sorun giderme bölümüne bak.

---

## Prompt Şablonları

### Genel Kural
Her promptta 3 şey zorunlu:
1. **Karakterin görünüşü** (kıyafet, silah)
2. **Tam aksiyon** (ne yapıyor, belirsiz değil)
3. **Yön** (direction kelimesini directiona göre değiştir)

| Direction | Prompt'a yaz |
|---|---|
| south | "toward the viewer, facing downward" |
| north | "away from the viewer, facing upward" |
| east | "facing right" |
| west | "facing left" |
| south-east | "toward the lower-right diagonal" |
| south-west | "toward the lower-left diagonal" |
| north-east | "toward the upper-right diagonal" |
| north-west | "toward the upper-left diagonal" |

---

### WARBLADE — Ağır zırhlı savaşçı, büyük iki elli kılıç

**attack-1** (hızlı yatay slash):
```
heavily armored warrior in dark plate armor, swings large two-handed greatsword 
in a fast horizontal slash [YÖN YAZI], 
quick windup → sharp strike → brief follow-through, attack sequence, 
10 frames, no loop, dark fantasy pixel art
```

**attack-2** (diagonal aşağı vuruş):
```
heavily armored warrior in dark plate armor, powerful diagonal downward sword swing 
[YÖN YAZI], 
overhead windup → heavy downward slash → impact pose, 
10 frames, no loop, dark fantasy pixel art
```

**attack-3** (güçlü final, zemin vuruşu):
```
heavily armored warrior in dark plate armor, devastating ground-slam with greatsword 
[YÖN YAZI], 
wide windup → slam into ground → shockwave impact, finishing blow, 
10 frames, no loop, dark fantasy pixel art
```

**dash**:
```
heavily armored warrior in dark plate armor, explosive forward charge dash 
[YÖN YAZI], 
burst of motion, armor trailing, aggressive lunge forward, 
8 frames, no loop, dark fantasy pixel art
```

---

### SHADOWBLADE — Karanlık suikastçı, çift kısa kılıç / hançer

**attack-1** (hızlı jab):
```
dark assassin in black leather armor with dual short blades, 
quick darting jab-slash [YÖN YAZI], 
explosive fast strike, single clean cut motion, 
10 frames, no loop, dark fantasy pixel art
```

**attack-2** (çapraz çift vuruş):
```
dark assassin in black leather armor with dual short blades, 
cross-slash combo [YÖN YAZI], 
two rapid diagonal cuts in quick succession, 
10 frames, no loop, dark fantasy pixel art
```

**attack-3** (spinning final):
```
dark assassin in black leather armor with dual short blades, 
spinning slash finisher [YÖN YAZI], 
spins and delivers powerful dual blade strike, shadow trail visible, 
10 frames, no loop, dark fantasy pixel art
```

**dash**:
```
dark assassin in black leather armor, shadow blink dash [YÖN YAZI], 
vanishes in shadow burst and reappears forward, ghost trail behind, 
8 frames, no loop, dark fantasy pixel art
```

---

### ELEMENTALIST — Büyücü, asa, elemental güç

**attack-1** (hızlı enerji atışı):
```
robed elemental mage holding glowing staff, 
quick energy bolt blast [YÖN YAZI], 
staff swings forward → bright energy release → recoil, 
10 frames, no loop, dark fantasy pixel art
```

**attack-2** (alan büyüsü):
```
robed elemental mage holding glowing staff, 
ground-burst elemental attack [YÖN YAZI], 
staff slams toward ground → elemental eruption, magical impact, 
10 frames, no loop, dark fantasy pixel art
```

**attack-3** (güçlü kanal saldırısı):
```
robed elemental mage holding glowing staff, 
charged overloaded elemental beam [YÖN YAZI], 
staff raised → energy charges → powerful release blast, 
10 frames, no loop, dark fantasy pixel art
```

**dash**:
```
robed elemental mage, arcane blink teleport [YÖN YAZI], 
disappears in flash of magical energy and reappears forward, 
arcane particles trail, 8 frames, no loop, dark fantasy pixel art
```

---

### RANGER — Hafif zırhlı okçu, yay

**attack-1** (hızlı ok atışı):
```
light-armored archer with bow, quick draw and fire [YÖN YAZI], 
fast draw → aim → release, arrow leaving the bow clearly visible, 
10 frames, no loop, dark fantasy pixel art
```

**attack-2** (güçlü gerilmiş atış):
```
light-armored archer with bow, power shot [YÖN YAZI], 
full draw back → held tension → powerful arrow release, heavy impact implied, 
10 frames, no loop, dark fantasy pixel art
```

**attack-3** (çoklu ok):
```
light-armored archer with bow, multi-arrow volley [YÖN YAZI], 
draws multiple arrows simultaneously → releases fanned spread of arrows, 
10 frames, no loop, dark fantasy pixel art
```

**dash**:
```
light-armored archer, agile combat roll dodge [YÖN YAZI], 
rolls forward quickly while staying low, bow in hand throughout, 
8 frames, no loop, dark fantasy pixel art
```

---

## QC: Hangi Sonuç Kabul, Hangisi Ret

**Kabul (PASS):**
- Karakter referans görselle aynı stil/renk
- Silah görünüyor ve tutarlı
- Aksiyon net okunuyor (windup → strike anlaşılıyor)
- Siyah/boş frame yok
- Yön doğru (south üretildiyse bize bakıyor)

**Ret (FAIL) — Promptu değiştir, tekrar Generate:**
- Farklı karakter çıktı (Init Image yanlış yüklenmiş)
- Siyah veya tamamen boş frameler
- Yanlış yön (east üretildi ama west bakıyor)
- Aksiyon anlaşılmıyor (sadece ayakta duruyor)
- Silah yanlış (kalkan yerine kılıç çıktı gibi)

---

## Sorun Giderme

| Sorun | Çözüm |
|---|---|
| Siyah/boş frame çıktı (özellikle diagonal) | Prompta yön kelimesini 2 kez ekle: `"facing lower-left diagonal, motion directed lower-left"` |
| Yanlış karakter stili | Init Image'ı tekrar yükle, View=Low top-down seçili mi kontrol et |
| Aksiyon çok belirsiz | Daha spesifik yaz: "fast windup, sharp 2-frame strike, follow-through" |
| Silah tutarsız | Kıyafeti ve silahı promptun başında tekrar tarif et |
| Animasyon çok yavaş/hızlı | Frame sayısını değiştir (8→10 veya 10→8) |
| Stil kaçıyor yönden yöne | Her yön için o yönün reference/ görselini kullan |

---

## Export ve Kaydetme

### Site'den PNG olarak indir
Her frame'i ayrı PNG olarak kaydet:
- Sağ tıkla → Farklı kaydet
- Veya frame'lerin altındaki indirme butonu varsa onu kullan

### Klasöre yerleştir
```
Örnek: Shadowblade attack-1 south için:
F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Shadowblade\animations\attack-1\south\

Dosyalar:
  frame_000.png  ← ilk frame (windup başlangıcı)
  frame_001.png
  ...
  frame_009.png  ← son frame (follow-through sonu)
```

> Frame sırasına dikkat: 000 = başlangıç, sonraki = devamı.

---

## Bana Ne Zaman Haber Ver

| Ne söyle | Ben ne yaparım |
|---|---|
| **"Warblade south hazır"** | O 4 animasyonu import edip test ederim, onay veririm, diğer yönlere geçersin |
| **"Warblade tüm yönler hazır"** | Tüm 32 clip'i import + wirer yaparım, sonraki karaktere geçersin |
| **"bir şeyle sorun var"** | Promptu düzeltir veya teknik sorunu çözerim |

**Önerilen iş sırası:**
```
1. Warblade → south → 4 animasyon (attack-1, 2, 3, dash) → bana göster → onay
2. Warblade → kalan 7 yön → hepsini tamamla → bana bildir
3. Shadowblade → aynı sıra
4. Elementalist → aynı sıra
5. Ranger → aynı sıra
```
