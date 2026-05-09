# PixelLab Production Guide — RIMA Act 1 (v2)
*Tarih: 2026-05-10 | Opus + NLM cross-check ile revize | v2 LOCKED*

> **🎯 TEK GİRİŞ DOSYASI:** Bu doküman **authoritative** — PixelLab üretiminde önce buraya bak.
> Detaylı prompt'lar lazımsa Bölüm 11'deki PLAYBOOK adım numaralarına git.
> v1 (`PIXELLAB_PRODUCTION_GUIDE_v1.md`) **deprecated** — bu dosya onun yerini alıyor.

Bu guide RIMA'nın tüm PixelLab üretim **kanonik kurallarını**, **referans araçlarını** ve **adım adım üretim sırasını** tek yerde toplar.

> **v1 → v2 ne değişti:**
> v3 Pixflux engine zorunluluğu, 252×252 canvas standardı, pixel budget formülü (524,288), animate_character MCP kalıcı yasak, Web App vs MCP karar matrisi, Animation-to-Animation Bridging Mode alternatifi, **adım adım üretim sırası bölümü**.

**İlişkili dokümanlar:**
- Detaylı prompt'lar (51 adım): `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md`
- Tool docs (57 dosya): `PixelLabDocs/`
- Style anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
- Animation Bible: `TASARIM/ANIMATION_BIBLE.md`

---

## 0. KANONİK KURALLAR (Geri Kalan Her Şeyin Üstünde)

### 0.1 Tool Versiyon Disiplini
- **"NEW" tag'li v3 araçlar zorunlu.** Eski v2 araçlar üretim sırasında %49 oranında çöküyor.
- **"Interpolate NEW"** = Interpolate **v2** (252×252 destekli). Eski 64×64 sınırlı Interpolation **ÖLÜ**.
- **"Animation-to-Animation Bridging Mode"** alternatif — KF1 + KF3 → 2 ara frame, geniş silahlı sınıflarda kırpma riskinde.

### 0.2 Canvas Standardı
**Karakter ve animasyon: 252×252** (v3 dayatması, RIMA seçimi değil)
- Geniş silahların kanvas dışına taşmasını engeller (Warblade greatsword örneği)
- Unity'ye **128×128 frame** olarak kırpılır (PPU=64)
- Aseprite'ta merkezden 128×128 crop → Unity multiple sprite

**Tile/obje boyutları (üretim):** Bölüm 2'de tablo halinde.

### 0.3 Pixel Budget Formülü — ZORUNLU KONTROL
```
width × height × frame_count ≤ 524,288
```

| Canvas | Max Frame | Kullanım |
|---|---|---|
| 252×252 | **8** | Standart (silahlı karakterler) |
| 160×160 | 16 | Silahsız karakterler için minimum safe |
| 128×128 | 16 | Silah taşıyan karakterde kırpma — KULLANMA |

**Bütçe aşımı = generation hatası VEYA kalite düşüşü.** Frame sayısını planla.

### 0.4 animate_character MCP — KALICI YASAK (2026-05-02)
Tüm karakter animasyonları için MCP kullanılmaz.
- **Sebep:** 4-frame limit + sprite frame'lere VFX gömülmesi + bozuk run animasyonu
- **char_id'ler memory'den kasıtlı silindi**
- Claude prompt dökümanı hazırlar, kullanıcı PixelLab UI'da uygular

### 0.5 Attack Animation Budget Kuralı
**Eski plan:** START + PEAK + END = 1 + 4 + 4 = 9 frame → bütçe aşımı.
**Yeni format (zorunlu):** PEAK frame'i windup'ın son + follow'un ilk frame'i olarak paylaş → toplam **8 unique frame**.
- Alternatif: 2 ayrı Unity clip (Attack_Windup 4-frame + Attack_Follow 4-frame, Animator chain)

### 0.6 Web App vs MCP Karar Matrisi

| Görev | Tool | Sebep |
|---|---|---|
| Karakter animasyon (her clip) | **Web App ZORUNLU** | MCP yasak (0.4) |
| Karakter prototip (style/iter) | Web App | Görsel feedback |
| Batch base 4-yön (10 sınıf loop) | MCP OK | Programmatic |
| Tile / static prop | MCP OK | Batch hızlı |
| Animasyon polish | Aseprite | Pixel kontrol |
| Cinematic frame | Aseprite + Edit Image Pro | Hand-paint |

---

## 1. TOOL HARİTASI

| Asset Tipi | Tool | PixelLabDocs Referansı | Kanal |
|---|---|---|---|
| Floor tiles (F1/F2/F3) | **Create Tiles Pro** → Shape: Isometric | `create-tiles-pro.md` | Map |
| Wall tiles (W1/W2) | **Create Tileset Pro** → Transition Height: 1.0 | `create-tileset.md` | Map |
| Wall tiles alternatif | **Create Isometric Tile** (bireysel) | `create-isometric-tile.md` | Map |
| F1↔F2 / F2↔F3 geçiş | **Create Tileset Standard** → Upload Image | `create-tileset.md` | Map |
| Oda objeleri (pillar, rubble…) | **Create Image S-XL (new)** | `create-image-flux.md` | Ana |
| Karakter base 4-yön | **Create Character Pro New** | `create-character-pro` (mcp_docs.md) | Ana |
| Karakter idle/hit/death | **Animate with Text NEW** (v3) | `animate-with-text-new.md` | Ana |
| Run cycle / 3-segment dolgu | **Interpolate NEW** (v2 252px) | `interpolation.md` | Ana |
| Geniş silah anim alternatif | **Animation-to-Animation Bridging Mode** | `animation-to-animation.md` | Ana |
| Karmaşık özel saldırı | **Animate with Skeleton** | `animate-with-skeleton.md` | Ana |
| Sprite düzeltme + silah ekle | **Edit Image Pro** | `edit-image-pro.md` | Ana |
| 8-yön rotasyon (özel ihtiyaç) | **Create 8 Rotations Pro** | `create-8-rotations-pro.md` | Ana |

---

## 2. BOYUT / VARİYASYON KURALI

### 2.1 Karakter ve Animasyon (v3 dayatması)
| Asset | Üretim Boyutu | Unity Frame | Frame Sayısı | Notlar |
|---|---|---|---|---|
| Karakter base 4-yön | 252×252 | 128×128 (crop) | 1 frame × 4 yön | Body-only, silahsız |
| Idle | 252×252 | 128×128 | 6-8 frame | Subtle breathing |
| Hit react | 252×252 | 128×128 | 3 frame | Flinch |
| Death | 252×252 | 128×128 | 6 frame | Collapse |
| Run cycle | 252×252 | 128×128 | 6 frame | Brian's Extreme Pose |
| Attack (LMB/RMB) | 252×252 | 128×128 | **8 frame (PEAK paylaşılır)** | Bütçe kuralı (0.5) |
| Dash | 252×252 | 128×128 | 4 frame | Anticipation→push→air→land |
| Weapon Pass | 252×252 | 128×128 | Static + propagate | Edit Image Pro |

### 2.2 Tile ve Obje
| Boyut | Varyasyon | Açıklama |
|---|---|---|
| 64px | 16 var | F1/F2/F3 floor (Create Tiles Pro) |
| 32×64px → 2x upscale → 64×128 | 8-23 var | W1/W2 wall (Create Tileset Pro Transition Full) |
| 64×128 (direkt) | 4-8 var | Create Isometric Tile (alternatif) |
| 128px | 4 var | Büyük hero objeler (chest, altar) |
| 256px | 4 var | Pillar, large altar (Create Image S-XL) |
| 128px | 8-16 var | Scatter objeler (rubble, barrel, bone) |
| 64px | 8 var | Wall torch |
| 64px | 16 var | Floor crack decal |

### 2.3 2x Upscale Kuralı (Create Tileset)
Create Tileset çıktısı max 32px. Unity'de:
- Filter Mode = **Point** (nearest-neighbor)
- Compression = **None**
- Scale 2x → 64px — kalite kaybı yok

---

## 3. CREATE TILESET STANDARD — TERRAIN GEÇİŞ SETİ

**Ne üretir:** İki terrain tipi arasında seamless geçiş tile ailesi (Wang set).
**Çıktı formatları:** Wang 16-tile, dual-grid 15-tile, 3×3 tileset
**Boyut:** 32×32 (→ 2x = 64×64) veya Transition Full ile 32×64 (→ 64×128)

### Kullanım (F1↔F2 geçiş):
1. Lower Terrain → **Upload Image**: F1 approved floor tile yükle
2. Upper Terrain → **Upload Image**: F2 approved floor tile yükle
3. Transition: **Full (100%)** — 32×64 çıktı (duvar yüksekliği için)
4. Transition Description: "scattered stone rubble, cracked floor"
5. Map orientation: **Top-down**
6. Generate → export Wang set
7. 2x nearest-neighbor upscale → Unity import

### Unity Import (Wang set):
- Sprite Mode: Multiple, 4×4 grid (veya Wang layout'una göre)
- 2D Tilemap Extras → **Terrain Tile** (Rule Tile değil)
- Filter Mode: Point, Compression: None

---

## 4. CREATE TILESET PRO — DUVAR TİLE AİLESİ

**Ne üretir:** Transition Height ile duvar yüzü dahil 23-tile set (köşe + junction varyantları otomatik).
**Maliyet:** ~20 generation.

### Kullanım (W1 duvar):
1. Tile Size: **32×32**
2. Lower Terrain: `"cold grey granite stone floor, isometric pixel art dungeon"`
3. Upper Terrain: `"dark stone brick wall face, vertical masonry, dungeon"`
4. Shape Controls: **angular/kare preset** seç (dungeon köşeleri yumuşak değil)
5. Transition Height: **1.0** (tam tile yüksekliği duvar yüzü)
6. Advanced Options → AI Border Freedom: 0.3 (düşük = daha tutarlı kenar)
7. Generate Pro → export
8. 2x upscale → 64×128 duvar tile

### W2 için:
- Aynı ayarlar, Upper Terrain'e W1 output tile'ını style reference olarak gir
- Daha koyu/daha bozuk görünüm için Upper description'a "cracked, weathered" ekle

---

## 5. CREATE TILES PRO — ZEMİN TİLE SETİ

**Şekil:** Isometric (Shape dropdown'dan)
**Boyut:** 64px → 16 varyasyon
**View:** High top-down (zemin için tepeden bakış)
**Style Reference:** Önceki approved tile'ı her session'da yükle

### Üretim sırası:
1. F1 üret → 4 var test → QC → 16 var tam üretim
2. F1 approved → style reference olarak yükle → F2 üret
3. F2 approved → F3 üret
4. Hepsi onaylandıktan sonra → Standard ile F1↔F2 ve F2↔F3 geçiş seti

---

## 6. CREATE IMAGE S-XL (NEW) — ODA OBJELERİ

**Transparent background:** ✅ ON — chromakey pipeline GEREKMİYOR
**Direction:** None (non-character)
**Outline:** Single color (pixel art kontur)
**Detail:** Highly detailed

| Obje | View | Boyut |
|---|---|---|
| Pillar, altar | **Low top-down** | 256px |
| Barrel, crate, chest | **Low top-down** | 128px |
| Rubble cluster | **High top-down** | 128px |
| Bone pile | **High top-down** | 128px |
| Wall torch | **Low top-down** | 64px |
| Floor crack decal | **High top-down** | 128px |

**Low top-down:** ~20° elevation — yüksek/3D objeler için, daha fazla ön yüz görünür
**High top-down:** ~35° elevation — zemine yapışık, flat objeler için

### Style Consistency:
Her obje setinden birini approve et → sonraki objelerde style reference olarak yükle.
**Palette LOCKED:** `#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575`

---

## 7. KARAKTER ÜRETİM PIPELINE (4 Sınıf v1 Sprint)

**Sınıf sırası:** Warblade → Ranger → Shadowblade → Elementalist

### 7.1 Adım Akışı (her sınıf için)
1. **Base 4-yön** (Create Character Pro New) — body-only, silahsız, 252×252, 4 yön ayrı (S/E/N/W)
2. **Idle** (Animate with Text NEW) — 6-8 frame, "subtle breathing motion"
3. **Hit react** (Animate with Text NEW) — 3 frame, "flinch backwards"
4. **Death** (Animate with Text NEW) — 6 frame, "collapse to ground"
5. **Run cycle** (Animate with Text NEW + Interpolate NEW)
   - 12 frame al → en uç poz seç (Pose A)
   - Aseprite'ta horizontal flip → Pose B
   - Interpolate NEW: A → B arası 4-6 frame doldur
6. **Attack LMB** (Animate with Text NEW PEAK + Interpolate NEW segments)
   - PEAK frame üret (silahla)
   - START → PEAK: 4 frame windup (PEAK = son frame)
   - PEAK → END: 4 frame follow (PEAK = ilk frame, paylaşılır)
   - **Toplam: 8 unique frame** (bütçe kuralı 0.5)
7. **Attack RMB** — Aynı 3-segment, farklı PEAK
8. **Dash** — 4 frame: crouch → push → air → land
9. **Weapon Pass** (Edit Image Pro) — body-only set'e silah ekle, propagate

### 7.2 Sınıf-Spesifik Notlar
| Sınıf | Tip | Aksent | Silah | Notlar |
|---|---|---|---|---|
| Warblade | Simetrik | Cold blue #7BA7BC | Greatsword | W = E flipX, geniş silah → Bridging Mode değerlendir |
| Ranger | Asimetrik | Cold blue #7BA7BC | Compound bow (sol el) | 4 yön ayrı |
| Shadowblade | Asimetrik | Void purple #5A2A8A | Twin blades | 4 yön ayrı, embedded glow YASAK |
| Elementalist | Asimetrik | Element-agnostic robe | YOK | Weapon Pass YOK, VFX engine-side |

### 7.3 Hero Anchor Zorunlu
Her gen'de yükle: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
Kamera açısı + boyut tutarlılığı için kritik.

---

## 8. GENEL TUTARLILIK KURALLARI

1. **Style reference zorunlu** — her yeni üretimde approved tile/sprite yükle
2. **Palette hex kodları prompt'a gir** — "dark grey" gibi belirsiz terim kullanma
3. **4 var test → QC → 16 var** — bulk üretim öncesi küçük test
4. **Create Tileset max 32px** → nearest-neighbor 2x upscale → Unity Point filter
5. **Transparent bg** S-XL objeler için her zaman ON — `process_tiles.py` gerekmez
6. **Chromakey #00FF00** sadece Create Tiles Pro / Create Tile Isometric çıktıları için
7. **PixelArt Mode ON, Upscale OFF, Anti-aliasing OFF** — Web App ortak ayarlar
8. **Pixel budget formülü her animasyonda kontrol** (Bölüm 0.3)

---

## 9. QC KONTROL LİSTESİ

Her üretim sonrası:
- [ ] Palette tutarlı? (Hex kodu gözle kontrol)
- [ ] 3D render görünümü yok? (Gradient, soft shadow — varsa yeniden üret)
- [ ] Isometric açı doğru? (Hepsi aynı perspektifte mi?)
- [ ] Transparent/chromakey clean? (Artık piksel kalmamış)
- [ ] Unity'de yan yana test (filter Point, adjacent tile gap yok)
- [ ] Varyasyonlar tutarlı mı? (Biri çok farklı görünüyorsa ayıkla)
- [ ] Karakter anim için: pixel budget formülü tutuyor mu?
- [ ] Karakter anim için: silah kanvas dışına taşmıyor mu?

---

## 10. PROMPT DOSYALARI

| Dosya | İçerik |
|---|---|
| `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md` | 51 adımlık üretim recipe'si — detaylı prompt'lar |
| `STAGING/PIXELLAB_PROMPT_FLOORS_v3.md` | F1/F2/F3 + transition prompts |
| `STAGING/PIXELLAB_PROMPT_WALLS_v3.md` | W1/W2/OBW prompts (64×128) |
| `STAGING/PIXELLAB_PROMPT_OBSTACLES_v1.md` | Pillar/rubble/torch/barrel/bone prompts |

---

## 11. 🎬 ADIM ADIM ÜRETİM SIRASI — Tıklama Rehberi

**Kullanım:** Bu bölüm seni baştan sona götürür. Her adım için **(1) ne tool'una tıkla, (2) ne ayar yap, (3) detaylı prompt için PLAYBOOK'un hangi adımına bak** belirtilir.

> ⚠️ Her üretimden önce `PixelLab Web App'te ortak ayarlar`: **Pixel Art Mode ON, Upscale OFF, Anti-aliasing OFF**.

### FAZ A — Tile Üretimi (Önce bunu bitir, sonra karaktere geç)

**🟢 Adım A1: F1 Floor (16 var) — ~1 saat**
1. PixelLab Web App → **Map** bölümü → **Create Tiles Pro** tool'una tıkla
2. Ayarlar:
   - Shape dropdown → **Isometric**
   - Size → **64×64**
   - Variation → **16**
   - View → **High top-down**
   - Background → **#00FF00** (saf yeşil)
3. Prompt için: `PRODUCTION_PLAYBOOK.md` **Adım 4** (F1 Floor)
4. İlk seferde 4 var test → QC → 16 var tam üretim
5. Kaydet: `STAGING/PIXELLAB/02_NEXT_floors/outputs/f1/f1_sheet_v1.png`
6. Process: `python STAGING/process_tiles.py --source ... --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_`

**🟢 Adım A2: F2 Floor (16 var)** — F1 approved → style reference olarak yükle → PLAYBOOK Adım 5

**🟢 Adım A3: F3 Floor (16 var)** — F2 approved → style reference → PLAYBOOK Adım 6

**🟢 Adım A4: F1↔F2 Transition (8 var)**
1. PixelLab → **Map** → **Create Tileset Standard** tool
2. Lower Terrain → **Upload Image** → F1 approved
3. Upper Terrain → **Upload Image** → F2 approved
4. Transition: **Full (100%)**, Map orientation: **Top-down**
5. Detay: PLAYBOOK Adım 7

**🟢 Adım A5: F2↔F3 Transition (8 var)** — PLAYBOOK Adım 8

**🟢 Adım A6: W1 Wall (8 var) — ~30 dk**
1. PixelLab → **Map** → **Create Tile — Isometric** tool
2. Boyut: **64×128**, Variation: **8**, Background: **#00FF00**
3. Prompt: PLAYBOOK Adım 1
4. Kaydet: `STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_sheet_v1.png`

**🟢 Adım A7: W2 Wall (8 var)** — W1 approved → style ref → PLAYBOOK Adım 2

**🟢 Adım A8: OBW Wall (4 var)** — Yüksek arch wall → PLAYBOOK Adım 3

**🟢 Adım A9-A16: Obstacles (8 üretim) — ~2-3 saat**
- **Tool:** PixelLab → Ana tool listesi → **Create Image S-XL (new)**
- **Background: Transparent ON** (chromakey GEREKMIYOR — process_tiles.py atlanır)
- Sırayla: Pillar → Rubble → Torch → Crack Decal → Barrel/Crate → Bone → Stump → Altar
- Detay: PLAYBOOK Adım 9-16

**Faz A kontrol:** Tüm tile + obstacle Unity'ye import edildi. `DemoRoomPainter` ile görsel QC yap.

---

### FAZ B — Karakter Animasyonu (4 sınıf, sırayla)

> 🚫 **animate_character MCP YASAK** — sadece Web App!
> 📐 Tüm karakter işleri **252×252 canvas**'ta.

#### B.1 Warblade (Simetrik, 3 yön + W flip)

**Adım B1.1: Base 4-yön (Body-only)**
1. PixelLab Web App → Ana tool listesi → **Create Character Pro New** tool
2. Canvas: **252×252**
3. Hero Anchor yükle: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
4. 3 ayrı gen call: South / East / North (W üretme — Unity flipX)
5. Body-only prompt — silah YOK
6. Detay + prompt: PLAYBOOK Adım 17
7. Kaydet: `STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/`

**Adım B1.2: Idle (6-8 frame)**
1. PixelLab → **Animate with Text NEW** (v3 Pixflux) tool
2. Input: Adım B1.1'in S/E/N sprite'ları
3. Frame: 6-8, Yön sayısı: 3 (S, E, N) → 3 gen call
4. Prompt: PLAYBOOK Adım 18

**Adım B1.3: Hurt (3 frame)** — PLAYBOOK Adım 19
**Adım B1.4: Death (6 frame)** — PLAYBOOK Adım 20
**Adım B1.5: Walk Cycle (Brian's Extreme Pose)**
1. **Animate with Text NEW** ile 12 frame walk al → en uç poz seç (Pose A)
2. Aseprite'ta Pose A'yı yatay flip → Pose B
3. **Interpolate NEW** (v2, 252×252) tool → Input: Pose A + Pose B → Output: 4-6 frame
4. ⚠️ Geniş silah (greatsword) kırpma yaşarsan: **Animation-to-Animation Bridging Mode** kullan
5. Detay: PLAYBOOK Adım 21

**Adım B1.6: Attack LMB (3-segment, 8 frame total)**
1. **Animate with Text NEW** ile PEAK frame üret (silahla)
2. **Interpolate NEW** ile START → PEAK: 4 frame windup (PEAK = son frame)
3. **Interpolate NEW** ile PEAK → END: 4 frame follow (PEAK = ilk frame, paylaşılır)
4. **Toplam 8 unique frame** (pixel budget kuralı, Bölüm 0.5)
5. Detay: PLAYBOOK Adım 22

**Adım B1.7: Attack RMB (Heavy Slam)** — PLAYBOOK Adım 23
**Adım B1.8: Dash (4 frame)** — PLAYBOOK Adım 24
**Adım B1.9: Weapon Pass**
1. PixelLab → **Edit Image Pro** tool
2. Input: Adım B1.1 base sprite (body-only)
3. Prompt: "add greatsword on right shoulder..." (PLAYBOOK Adım 25)
4. 3 ayrı pass (S, E, N)
5. Aseprite'ta tüm idle/walk/dash/hurt/death frame'lerine silah katmanı propagate et

**Warblade kontrol:** ~7 anim x 3 yön = 21 anim sheet + base + weapon = ~25 dosya.

#### B.2 Ranger (Asimetrik, 4 yön ayrı)

Akış aynı ama 4 yön ayrı (W flipX değil, ayrı çek). Yay tek elde, asimetri.
- Base: PLAYBOOK Adım 26
- Idle/Hurt/Death: 27/28/29
- Walk: 30 (4 yön, Bridging Mode geniş silahta)
- Attack LMB (Bow): 31
- Attack RMB (Aim): 32
- Dash (Roll): 33
- Weapon Pass: 34

#### B.3 Shadowblade (Asimetrik, 4 yön)

Twin blades, void purple #5A2A8A aksent. Embedded glow YASAK (VFX engine-side).
- Base: PLAYBOOK Adım 35
- Idle/Hurt/Death: 36/37/38
- Walk: 39
- Attack LMB (Twin Blade): 40
- Attack RMB (Shadow Step): 41
- Dash (Shadow Dash): 42
- Weapon Pass: 43

#### B.4 Elementalist (Asimetrik, 4 yön, **Weapon Pass YOK**)

Eller serbest, cool neutral robe (element-agnostic, VFX engine-side).
- Base: PLAYBOOK Adım 44
- Idle/Hurt/Death: 45/46/47
- Walk: 48
- Attack LMB (CastRhythm): 49
- Attack RMB (Element Ranged): 50
- Dash (Mage Step): 51

---

### FAZ C — Final Kontrol & İleri Faz

✅ Tüm tile + obstacle Unity'de import edildi
✅ 4 sınıf base 4-yön + 7 anim seti tamamlandı
✅ Pixel budget her anim için doğrulandı (≤ 524,288)
✅ Style consistency QC (Bölüm 9)

**Sonraki adım:** Mob/Boss üretimi → Cinematic frames → Polish.

---

## 📌 HANGİ DOSYAYI İZLEMELİ?

**1️⃣ Bu dosya (`PIXELLAB_PRODUCTION_GUIDE_v2.md`) — TEK GİRİŞ NOKTASI**
   - Önce **Bölüm 0**'ı oku (kanonik kurallar — bunları unutursan kalite düşer)
   - **Bölüm 11**'deki adım sırasına gir (A1 → A2 → ... → B4)

**2️⃣ Detaylı prompt lazımsa → `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md`**
   - Bölüm 11'deki "PLAYBOOK Adım X" referansından spesifik adıma git
   - Kopyala-yapıştır prompt + kaydet path + process komutu

**3️⃣ Tool detayı lazımsa → `PixelLabDocs/<tool>.md`**
   - Spesifik bir tool'un tüm parametreleri için (ör. `create-tiles-pro.md`)

**Diğer dosyalar (sadece referans):**
- `GUIDES/PIXELLAB_ANIM_QUICKREF.md` — animasyon hızlı referans
- `GUIDES/RIMA_MASTER_ART_PIPELINE.md` — genel art pipeline
- `TASARIM/ANIMATION_BIBLE.md` — animasyon tasarım kuralları
- `TASARIM/CLASS_CONCEPTS/` — sınıf konsept ve style anchor

---

## LOCKED KARARLAR ÖZETİ

### v1'den (2026-05-09) — KORUNUYOR
- Create Tileset Standard/Pro max 32px → 2x nearest-neighbor upscale → 64px
- W1/W2: Create Tileset Pro, Transition Height 1.0, Transition Full → 64×128
- F1↔F2/F2↔F3: Create Tileset Standard, Upload Image, Wang export
- Obje tool: Create Image S-XL (new), transparent bg, Low/High top-down
- Floor tool: Create Tiles Pro, Isometric shape, 64px
- Shared palette Act 1: `#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575`

### v2 yeni (2026-05-10) — EKLENDİ
- **Karakter/anim canvas: 252×252** (v3 zorunlu, Unity'ye 128×128 crop)
- **Pixel budget formülü:** w × h × frames ≤ 524,288
- **Tool versiyon disiplini:** "NEW" v3 zorunlu, eski v2 %49 çöküyor
- **Interpolate NEW = v2 252×252** (eski 64×64 ÖLÜ)
- **Animation-to-Animation Bridging Mode** alternatifi
- **animate_character MCP KALICI YASAK** (2026-05-02)
- **Attack 8-frame zorunlu** (PEAK paylaşımı, eski 9 bütçe aşımı)
- **Web App vs MCP karar matrisi**
- v1 sprint sınıfları: Warblade / Ranger / Shadowblade / Elementalist (kalan 6 → v2 sprint)
