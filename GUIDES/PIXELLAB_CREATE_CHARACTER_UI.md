# PixelLab — Create Character UI Kullanım Rehberi
> RIMA | Son güncelleme: 2026-04-23
> Hedef: 10 class için 128px base sprite, 8 yön, Pro mod

---

## Önce Hazırla

Üretime geçmeden şunlar hazır olmalı:

| Materyal | Kaynak | Nerede |
|----------|--------|--------|
| Concept image | Seçilmiş ChatGPT çıktısı | `C:/Users/ydbil/Downloads/chatgpt chars/SELECTED/` |
| Style image | Warblade PixelLab çıktısı (ilk class bittikten sonra) | `Assets/Sprites/Characters/Warblade/base/warblade_base_S.png` |
| Character Description | Per-class prompt | `GUIDES/CHARACTER_BASE_PRODUCTION_GUIDE.md` → Adım 2 tablosu |

> **İlk class (Warblade) için style image YOK** — üret, kaydet, sonra diğer 9 class için kullan.

---

## Adım Adım UI

### 1 — Siteye Gir
`pixellab.ai` → sol menüden **Create Character**

---

### 2 — Tab Seç
Üstte iki tab var:
- **Create from Template** ← bunu seç
- Create from Reference (farklı akış, bizim için değil)

---

### 3 — Generation Mode
```
○ Standard
● Pro  [NEW]
```
**Pro** seç. Style image ve Concept image slotları sadece Pro'da aktif olur.

> Maliyet: UI'da "This tool costs 20-40 generations depending on size" yazar. 128px = üst sınıra yakın. **Generate butonundaki sayıya bak**, bas öncesi bakiyeyi kontrol et.

---

### 4 — Character Type
```
● Humanoid   ○ Quadruped [EXPERIMENTAL]
```
**Humanoid** seç. Tüm RIMA class'ları bipedal.

---

### 5 — Character Description
`CHARACTER_BASE_PRODUCTION_GUIDE.md` → Adım 2 → per-class prompt tablosundan ilgili class'ın promptunu **tek satır** olarak yapıştır.

Örnek (Warblade):
```
full body centered, same scale as reference, no zoom-in, top-down ARPG view, male mercenary warrior greatsword, dark grey cloth tunic clearly distinct from black leather armor pieces, cold blue uniform glow along full blade length no bright tip flare, short dark brown hair, scarred jaw
```

> **Kural:** Prompt içine oyun adı, "dark fantasy", "isometric", "3/4 view" YAZMA. "High Top-Down" camera zaten UI'da seçili, tekrar yazma.

---

### 6 — Character Size
Preset butonları: 32 / 48 / 56 / 64 / 80 / 96 / **128** / 168

**128px** seç (turuncu highlight).

> UI notu: "Approximate height of your character in pixels. The canvas will be ~40% larger to make room for animations."
> → Çıktı canvas boyutu 128px DEĞİL ~179px olacak. Bu normal — Unity'de 128×128 tile olarak slice ederiz.

---

### 7 — Camera View
```
Sidescroller | Low Top-Down | [High Top-Down] | Oblique Projection (UNAVAILABLE)
```
**High Top-Down** seç (turuncu highlight). RIMA'nın ~60-65° overhead kamerası buna karşılık geliyor.

> Pro mode bilgi kutusu: "Pro mode generates 8 rotations using advanced AI. Supports sizes 32×32 to 168×168."
> → 8 yön otomatik üretilecek. Ayrı ayrı yön üretmeye gerek yok.

---

### 8 — Style Image (optional)
**"Used as a style reference for the generated character"**

| Class | Style Image | Concept Image |
|-------|-------------|---------------|
| Warblade | `warrior_idle_128.png` ← `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/` | `SELECTED/01_warblade.png` |
| Diğer 9 class | `warblade_base_S.png` ← onaylı PixelLab çıktısı | `SELECTED/0X_[class].png` |

> **Not:** Style image ZORUNLU — boş bırakılırsa chibi oranlar çıkıyor, her yönde farklı grip.

Upload: "Click or drag to upload" alanına sürükle veya tıkla → dosya seç.

---

### 9 — Concept Image (optional)
**"Upload concept art to generate a pixel art version"**

`SELECTED/` klasöründen ilgili class dosyasını yükle:

| Class | Dosya |
|-------|-------|
| Warblade | `SELECTED/01_warblade.png` |
| Shadowblade | `SELECTED/02_shadowblade.png` |
| Brawler | `SELECTED/03_brawler.png` |
| Ravager | `SELECTED/04_ravager.png` |
| Ronin | `SELECTED/05_ronin.png` |
| Elementalist | `SELECTED/06_elementalist.png` |
| Ranger | `SELECTED/07_ranger.png` |
| Gunslinger | `SELECTED/08_gunslinger.png` |
| Hexer | `SELECTED/09_hexer.png` |
| Summoner | `SELECTED/10_summoner.png` |

---

### 10 — Generate Character
**Generate Character** butonuna bas.

Pro mod: 8 yön sprite sheet üretir. Tamamlanınca indir.

**8 yön sırası (API V2):**

| Index | Yön | RIMA suffix |
|-------|-----|-------------|
| 0 | South (kameraya bakıyor) | `_S` |
| 1 | South-West | `_SW` |
| 2 | West | `_W` |
| 3 | North-West | `_NW` |
| 4 | North (kameradan uzak) | `_N` |
| 5 | North-East | `_NE` |
| 6 | East | `_E` |
| 7 | South-East | `_SE` |

İlk frame (index 0) = `_S` — base lock olarak kaydet.

---

## Çıktı QC

İndirmeden önce ekranda kontrol et:

| Kriter | Olması gereken |
|--------|---------------|
| Karakter yüksekliği | Canvas'ın ~%60-65'i — ne çok küçük ne çok büyük |
| Kamera açısı | Başın tepesi görünür, yüz aşağıya bakıyor, bacaklar foreshortened |
| Renk | Kıyafet rengi prompttaki renge uyuyor — kahverengi tona kaçmamış |
| Accent | Class'ın accent rengi doğru yerde (Brawler: knuckles, Warblade: blade, vs.) |
| Silhouette | 8 yön arasında tutarlı scale — biri diğerinden belirgin büyük/küçük değil |

Fail varsa → yeniden üret (aynı ayarlar, aynı concept image).

---

## Kaydetme ve Klasörleme

İndirilen sprite sheet:
```
Assets/Sprites/Characters/[Class]/base/
  [class]_base_spritesheet.png   ← PixelLab çıktısı (ham)
  [class]_base_S.png             ← S yönü ayrı kesilmiş (Unity import için golden source)
```

**Boyut notu:**
PixelLab "128px = karakter yüksekliği" olarak tanımlıyor — çıktı canvas ~%40 daha büyük (~179px) olabilir.
- Eğer çıktı **128×128 ise**: Unity'de direkt slice et.
- Eğer çıktı **daha büyükse (~179px)**: Aseprite'da 128×128 olarak crop/export et, sonra Unity'e aktar. İki boyutu asla karıştırma.

**S yönü:**
- PixelLab'ın 8 yönünden index 0 = "kameraya bakan" yön = bizim `_S`
- Bu dosya animasyon için base lock — overwrite YAPMA

**İlk class (Warblade) sonrası:**
`warblade_base_S.png` → style anchor olarak kaydet, diğer 9 class üretiminde Style Image slotuna yükle.

---

## Özet Ayarlar (Her Class İçin Aynı)

| Alan | Değer |
|------|-------|
| Tab | Create from Template |
| Generation Mode | Pro |
| Character Type | Humanoid |
| Character Size | 128px |
| Camera View | High Top-Down |
| Style Image | Warblade PixelLab çıktısı (Warblade hariç) |
| Concept Image | `SELECTED/0X_[class].png` |
| Character Description | Per-class prompt (tek satır, guide'dan kopyala) |
