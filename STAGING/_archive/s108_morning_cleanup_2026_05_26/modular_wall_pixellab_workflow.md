# RIMA Modular Wall — PixelLab Asset Pack + Inpaint Workflow

**Date:** 2026-05-23
**Goal:** chatgpt_ref tarzı odalar üret. Modular base pieces + junction asset packs + user inpaint refinement = composite RoomTemplate sprites.
**Architecture fit:** Procgen logic burada Wang-constrained random pick olarak uygulanır; tüm sistem hâlâ Option C (Template + Decor Overlay) çatısı altında.

---

## 0. Mantık özeti (3 satır)

1. PixelLab'da **4 asset pack** üret (2×2 grid sheet, her sheet'te 4 piece) → **16 base wall piece**
2. Her piece **identical edge profile** (üst/alt/yan kenar pikselleri birbirine geçer)
3. Sen Aseprite'ta piece'leri yan yana koyup **junction'ları inpaint et** → composite room sprite çıkar → Unity'de template olarak kullan

---

## 1. Asset pack listesi (4 sheet, 16 piece)

Her sheet **PixelLab create_image_pro 2×2 grid**, **256×256 per cell, 512×512 toplam**.

### Sheet 1 — Base wall directions (4 piece)
| Cell | Piece | Açıklama |
|---|---|---|
| 1.1 | **NW-SE straight** | Diamond oda SOL kenarı (45° downhill) |
| 1.2 | **NE-SW straight** | Diamond oda SAĞ kenarı (45° downhill) |
| 2.1 | **Apex ∧ (top corner)** | Diamond oda TEPESİ (NW-SE + NE-SW buluşur) |
| 2.2 | **Apex ∨ (bottom corner)** | Diamond oda DİBİ (front, oyuncu girer) |

### Sheet 2 — Door / arch variants (4 piece)
| Cell | Piece | Açıklama |
|---|---|---|
| 1.1 | NW-SE + arch (open) | Sol kenarda taş arch, içi siyah |
| 1.2 | NE-SW + arch (open) | Sağ kenarda taş arch, içi siyah |
| 2.1 | NW-SE + wooden door (closed) | Sol kenarda kapı |
| 2.2 | NE-SW + wooden door (closed) | Sağ kenarda kapı |

### Sheet 3 — Junction pieces (4 piece)
| Cell | Piece | Açıklama |
|---|---|---|
| 1.1 | **Inner corner NW (girinti)** | İçeri kıvrılan köşe — L-shape oda için |
| 1.2 | **Inner corner NE** | Mirror |
| 2.1 | **T-junction NW-SE-side** | Yan odaya bağlantı, NW-SE duvardan dışarı açılış |
| 2.2 | **T-junction NE-SW-side** | Mirror |

### Sheet 4 — Decor wall variants (4 piece)
| Cell | Piece | Açıklama |
|---|---|---|
| 1.1 | NW-SE + banner (intact red) | Decor variant |
| 1.2 | NW-SE + torch sconce (lit) | Decor variant |
| 2.1 | NW-SE + alcove statue | Decor variant |
| 2.2 | NW-SE + crack (cyan rift) | Decor variant |

**Toplam: 16 piece, 4 sheet, 4 PixelLab gen (her gen 256-credit civarı veya 1 sub-gen).**

---

## 2. Edge profile kuralı (KRİTİK)

**Tüm 16 piece'in kenar pikselleri AYNI** olmalı. Bu olmadan modular çalışmaz.

PixelLab prompt'una zorunlu ekle (her sheet için):
```
CRITICAL: All four cells must have IDENTICAL edge pixels at the top edge,
bottom edge, and shared internal edges. Stone block pattern, brick mortar
lines, height, and shadow at edges must be PIXEL-PERFECT IDENTICAL across
cells so adjacent cells tile seamlessly. Same dark stone color, same brick
size, same top cap profile.
```

**Reference image:** master_room PNG veya Sheet 1'in çıktısını sonraki sheet'lere reference olarak ver (style + edge chain).

---

## 3. Junction strategy (apex meetings)

Diamond oda 4 kenar buluşur:
- **Top apex ∧:** Sheet 1 cell 2.1 (Apex ∧) parça
- **Bottom apex ∨:** Sheet 1 cell 2.2 (Apex ∨) parça
- **L-shape için inner corner:** Sheet 3 cell 1.1/1.2

**Bu apex'ler ÖZEL üretilmiş tek parçalardır.** Iki straight piece kendi başına buluşturmaya çalışmazsın, apex parça'sını koyarsın. Inpaint sadece **çok küçük gap** varsa devreye girer (1-3 pixel mismatch).

### Eğer iki straight piece buluşması gerekiyorsa (L-shape, T-junction)
1. İki piece'i yan yana koy (Aseprite)
2. Buluşma noktasında **2-4 pixel gap** olabilir
3. **Inpaint:** PixelLab `create_image_pro` mask ile o gap bölgesini regenerate et — prompt: "smooth dark stone wall continuation, match left and right edges exactly"
4. Çıkan sonucu kompozit sprite'a paste et

---

## 4. PixelLab production prompts (her sheet için)

### Sheet 1 prompt (base directions)
```
2x2 grid asset pack. Pixel art dark fantasy dungeon walls in isometric
top-down 85 degree view. Each cell 256x256.

CELL 1.1: NW-to-SE direction wall, weathered dark stone bricks with cyan
rift cracks, mortar lines visible, 90 degree top profile.

CELL 1.2: NE-to-SW direction wall, mirror of cell 1.1, same brick pattern
and palette.

CELL 2.1: Top apex corner where NW-SE and NE-SW walls converge upward,
forming a peaked ∧ shape, same brick pattern continues across the junction.

CELL 2.2: Bottom apex corner where NW-SE and NE-SW walls converge downward,
forming a ∨ shape (room front, player view).

CRITICAL: All four cells share IDENTICAL stone color, brick size, mortar
spacing, top cap height. Edges where cells would touch must be pixel-perfect
identical. RIMA dark fantasy palette: dark stone (#2a2a3a base), warm
torch accent absent here, cyan rift cracks as accent only.

Style: hand-pixeled, hard edges, no anti-aliasing, max 4 tones per color
region, Shattered Keep aesthetic.
```

### Sheet 2 prompt (door/arch variants)
```
2x2 grid asset pack. Same dark stone wall style as Sheet 1 reference image.

CELL 1.1: NW-SE wall with stone archway opening at center, archway interior
is pitch black void (no door, just opening).

CELL 1.2: NE-SW wall with stone archway opening, mirror of cell 1.1.

CELL 2.1: NW-SE wall with closed wooden door, dark wood planks with iron
bands, lock visible, stone arch frame around door.

CELL 2.2: NE-SW wall with closed wooden door, mirror of cell 2.1.

CRITICAL: Edge profile (top, bottom, left, right edges where wall meets
empty) must be PIXEL-PERFECT IDENTICAL to Sheet 1 base wall edges.
Reference: Sheet 1 NW-SE wall cell. Same brick pattern, same color, same
top cap.
```

### Sheet 3 prompt (junctions)
```
2x2 grid asset pack. Same dark stone wall style as Sheet 1 reference.

CELL 1.1: Inner corner where wall turns inward (concave, alcove-style),
NW corner version.

CELL 1.2: Inner corner mirror (NE).

CELL 2.1: T-junction where main NW-SE wall has a side opening pointing
SW (so a perpendicular wall branches off).

CELL 2.2: Mirror (NE-SW wall with NW-side branch).

CRITICAL: Edge profile matches Sheet 1 PERFECTLY. Stone color, brick size,
top cap all identical. These junction pieces must tile seamlessly with
Sheet 1 straight pieces.
```

### Sheet 4 prompt (decor variants)
```
2x2 grid asset pack. NW-SE direction walls with built-in decor.

CELL 1.1: NW-SE wall with intact red banner hanging from upper portion,
gold trim, slightly tattered edges.

CELL 1.2: NW-SE wall with iron torch sconce mounted, lit warm orange flame
with light bloom subtle glow on adjacent stones.

CELL 2.1: NW-SE wall with carved alcove containing small stone statue of
armored figure, statue partially weathered.

CELL 2.2: NW-SE wall with prominent cyan rift crack running diagonally
across the wall, glowing cyan energy seeping from crack.

CRITICAL: Underlying wall edge profile matches Sheet 1 NW-SE wall EXACTLY.
Only the wall CENTER changes — top, bottom, left, right edges identical
to base wall. Decor is overlaid, not protruding off the wall area.
```

---

## 5. Inpaint workflow (kullanıcı tarafı)

### Workflow: Composite oda inşası
1. **Aseprite (veya Photoshop) aç**
2. **Boş canvas:** 1024×1024 (target oda boyutu)
3. **Place pieces:**
   - Apex ∧ üst orta
   - NW-SE straight'leri tepe ∧'nin altına dizilim
   - NE-SW straight'leri sağ tarafa dizilim
   - Apex ∨ alt orta
   - Door/arch piece'leri yerine koy
   - Decor wall variant'larını istediğin slot'a swap et
4. **Junction check:** İki piece arasında pixel mismatch var mı bak (zoom 4x)
5. **Inpaint gerek olan yerler:**
   - PixelLab create_image_pro mask ile o bölgeyi seç
   - Prompt: "smooth stone wall continuation, match adjacent stones exactly"
   - Output küçük patch, kompozit'e paste
6. **Floor patch:** Mevcut Wang topdown tilesetlerden birini kullan, oda zeminini doldur (PixelLab inventory'sinde 25 hazır var)
7. **Save:** RoomTemplate_001.png olarak Unity'ye import

### Inpaint best practices
- Inpaint **sadece junction noktalarında** yapılır, ana piece'lere dokunma
- Mask area küçük tut (16×16 veya 32×32 max)
- Output Aseprite ile pixel-perfect copy-paste, AI'ya tüm parçayı yeniden çizdirme

---

## 6. Procgen logic uygulaması

User's procgen mantığı buraya nasıl oturur?

### Wang-constrained random pick (Boris the Brave 16-tile mantığı)
Her wall slot'a hangi piece gelecek? Random pick + constraint:

| Slot tipi | Allowed pieces | Constraint |
|---|---|---|
| Side wall (NW-SE/NE-SW) | Plain / Banner / Torch / Alcove / Crack | "Iki banner peş peşe YASAK" |
| Door slot | Arch / Wooden door | "Min 1 arch per room" |
| Apex | Apex ∧ / Apex ∨ only | "Sabit (top/bottom)" |
| Junction | Inner corner / T-junction | "Slot rolüne göre" |

### Implementation
- Unity'de `RoomTemplate.cs` (zaten Codex tarafından scaffolded) içine **WallSlot[]** array ekle
- Her WallSlot: position + allowed piece types + constraint
- Runtime: `RoomDecorationSpawner` benzeri `RoomWallAssembler` script ile slot'lara piece pick et
- Yoksa: Aseprite'ta MANUEL kompozit yap (önerilen başlangıç, sonra automate)

### Önemli karar
Şu an **MANUEL kompozit** (Aseprite + inpaint) ile başla. 5-10 oda üret. Bunlar **template** olur, RoomTemplate ScriptableObject'e atanır. Decor overlay sistemi (zaten yapılmış) üzerinde çalışır.

Sonra İSTERSEN automated assembler ekleyebilirsin — ama önce manuel composite yeterli mi gör.

---

## 7. Final flow

```
PixelLab gen × 4 sheet (16 piece)
       ↓
Aseprite manuel composite (yan yana yerleştir)
       ↓
Inpaint pass junction'larda (gerekirse)
       ↓
1024×1024 composite room sprite çıkar
       ↓
Unity'ye RoomTemplate.baseImage olarak import
       ↓
OverlayAnchor system (Codex scaffolded) decor spawn
       ↓
Game runtime: room sprite + decor overlay = final oda
```

---

## 8. Maliyet/Süre tahmini

| Adım | Süre | Maliyet |
|---|---|---|
| 4 PixelLab sheet gen | 30-60 dk (web UI) | 4 × create_image_pro ≈ ~40-80 credit |
| Aseprite composite per room | 15-30 dk | 0 |
| Inpaint per junction (avg 2-3 per room) | 5-10 dk | ~5-10 credit per inpaint |
| 5 oda toplam | ~3-5 saat user + ~100-150 credit | |

**LoRA training paralel** ~5-7 saat → ikisi aynı anda biter.

---

## 9. Avantajlar (LoRA-only vs hybrid)

| Boyut | LoRA-only | Hybrid (LoRA + modular) |
|---|---|---|
| Style consistency | LoRA çıkışına bağımlı | Mevcut PixelLab style direkt |
| Iterate hızı | LoRA-gen + cleanup | Compose + inpaint daha hızlı |
| Risk | LoRA'nın kalitesine bel bağla | Iki yol, biri kesin işler |
| Asset reuse | Her gen yeni | 16 piece sonsuz oda yapar |
| Procgen fit | Template-only seçim | Wall slot constraint = gerçek procgen |

---

## 10. Önerilen sıra

### Bu turda (LoRA training çalışırken paralel)
1. ✅ Bu workflow doc'unu onayla
2. **Sen PixelLab web UI'da Sheet 1'i üret** (2-3 prompt iterasyon, ~30 dk)
3. **Edge match doğrula** (Aseprite'ta 4 cell'i extract et, yan yana koy)
4. Sheet 1 PASS → Sheet 2/3/4 üret
5. Sheet 1 FAIL → prompt revize, tekrar

### Sonra (LoRA bittiğinde)
- LoRA çıkışını **template painting** için kullan (full oda 1024×1024)
- Modular kit **template extension** (oda genişletme, koridor, special variants)
- İki sistem birbirini tamamlar

### Risk hedge
- LoRA başarısızsa → modular kit zaten elinde, **fallback ready**
- LoRA başarılıysa → her ikisini kombinle (template ana + modular ekler)

---

**Onaylarsan:** Sheet 1 PixelLab prompt'unu sana hazır metin olarak verebilirim, web UI'a paste edip üretirsin. Sheet 1 PASS olunca diğer 3'ü ardı ardına üretirsin. Ben paralel Aseprite composite script'i hazırlarım (otomasyon için).
