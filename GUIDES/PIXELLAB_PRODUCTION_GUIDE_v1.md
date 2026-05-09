# PixelLab Production Guide — RIMA Act 1
# Tarih: 2026-05-09 | Opus onaylı | v1 LOCKED

Bu guide RIMA'nın tüm PixelLab üretim kararlarını tek yerde toplar.
Prompt dosyaları: `STAGING/PIXELLAB_PROMPT_*.md`

---

## 1. TOOL HARİTASI

| Asset Tipi | Tool | Nerede |
|---|---|---|
| Floor tiles (F1/F2/F3) | **Create Tiles Pro** → Shape: Isometric | Map bölümü |
| Wall tiles (W1/W2) | **Create Tileset Pro** → Transition Height: 1.0 | Map bölümü |
| F1→F2 / F2→F3 geçiş | **Create Tileset Standard** → Upload Image | Map bölümü |
| Oda objeleri (pillar, rubble…) | **Create Image S-XL (new)** | Ana tool listesi |
| Karakter/mob sprite | **Create Character Pro New** | Ana tool listesi |
| Karakter animasyonu | **Animate with Text NEW v3** | Ana tool listesi |
| Ara kare / smooth | **Interpolate NEW** | Ana tool listesi |
| Sprite düzeltme | **Edit Image Pro** | Ana tool listesi |

---

## 2. BOYUT / VARİYASYON KURALI

| Boyut | Varyasyon | Açıklama |
|---|---|---|
| 64px | 16 var | F1/F2/F3 floor (Create Tiles Pro) |
| 32×64px → 2x upscale → 64×128 | 8-23 var | W1/W2 wall (Transition Full + 2x) |
| 128px | 4 var | Büyük hero objeler |
| 256px | 4 var | Pillar, altar (Create Image S-XL) |
| 128px | 8-16 var | Scatter objeler (rubble, barrel, bone) |
| 64px | 8 var | Wall torch |

**2x Upscale Kuralı:** Create Tileset çıktısı max 32px. Unity'de:
- Filter Mode = **Point** (nearest-neighbor)
- Compression = **None**
- Scale 2x → 64px — kalite kaybı yok

---

## 3. CREATE TILESET STANDARD — TERRAIN GEÇİŞ SETİ

**Ne üretir:** İki terrain tipi arasında seamless geçiş tile ailesi (Wang set).
**Çıktı formatları:** Wang 16-tile, dual-grid 15-tile, 3x3 tileset
**Boyut:** 32×32 (→ 2x = 64×64) veya Transition Full ile 32×64 (→ 64×128)

### Kullanım (F1→F2 geçiş):
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

**Ne üretir:** Transition Height ile duvar yüzü dahil 23-tile set (köşe+junction varyantları otomatik).
**Maliyet:** 20 generation, Gemini kalitesi.

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

## 5. CREATE TILES PRO — ZEMIN TİLE SETİ

**Şekil:** Isometric (Shape dropdown'dan)
**Boyut:** 64px → 16 varyasyon
**View:** High top-down (zemin için tepeden bakış)
**Style Reference:** Önceki approved tile'ı her session'da yükle

### Üretim sırası:
1. F1 üret → 4 var test → QC → 16 var tam üretim
2. F1 approved → style reference olarak yükle → F2 üret
3. F2 approved → F3 üret
4. Hepsi onaylandıktan sonra → Standard ile F1→F2 ve F2→F3 geçiş seti

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
Her obje setinden birini approve et → sonraki objelerde style reference olarak yükle
Palette sabitle: `#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575`

---

## 7. GENEL TUTARLILIK KURALLARI

1. **Style reference zorunlu** — her yeni üretimde approved tile/sprite yükle
2. **Palette hex kodları prompt'a gir** — "dark grey" gibi belirsiz terim kullanma
3. **4 var test → QC → 16 var** — bulk üretim öncesi küçük test
4. **Create Tileset max 32px** → nearest-neighbor 2x upscale → Unity Point filter
5. **Transparent bg** S-XL objeler için her zaman ON — process_tiles.py gerekmez
6. **Chromakey #00FF00** sadece Create Tiles Pro / Create Tile Isometric çıktıları için

---

## 8. QC KONTROL LİSTESİ

Her üretim sonrası:
- [ ] Palette tutarlı? (Hex kodu gözle kontrol)
- [ ] 3D render görünümü yok? (Gradient, soft shadow — varsa yeniden üret)
- [ ] Isometric açı doğru? (Hepsi aynı perspektifte mi?)
- [ ] Transparent/chromakey clean? (Artık piksel kalmamış)
- [ ] Unity'de yan yana test (filter Point, adjacent tile gap yok)
- [ ] Varyasyonlar tutarlı mı? (Biri çok farklı görünüyorsa ayıkla)

---

## 9. PROMPT DOSYALARI

| Dosya | İçerik |
|---|---|
| `STAGING/PIXELLAB_PROMPT_FLOORS_v3.md` | F1/F2/F3 + transition prompts |
| `STAGING/PIXELLAB_PROMPT_WALLS_v3.md` | W1/W2/OBW prompts (64x128) |
| `STAGING/PIXELLAB_PROMPT_OBSTACLES_v1.md` | Pillar/rubble/torch/barrel/bone prompts |

---

## LOCKED KARARLAR (2026-05-09)

- Create Tileset Standard/Pro max 32px → 2x nearest-neighbor upscale → 64px: **LOCKED**
- W1/W2 üretim yöntemi: Create Tileset Pro, Transition Height 1.0, Transition Full → 64×128: **LOCKED**
- F1→F2/F2→F3: Create Tileset Standard, Upload Image, Wang export: **LOCKED**
- Obje tool: Create Image S-XL (new), transparent bg, Low/High top-down: **LOCKED**
- Floor tool: Create Tiles Pro, Isometric shape, 64px: **LOCKED**
- Shared palette Act 1: #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575: **LOCKED**
