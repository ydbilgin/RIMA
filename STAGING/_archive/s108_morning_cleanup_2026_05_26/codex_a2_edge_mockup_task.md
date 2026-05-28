ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — A2 Edge Mockup PNG'leri (init image için)

**Amaç:** A2 Edge için 4 placeholder/mockup PNG çiz (Python PIL). User bu mockup'ları PixelLab Create S-XL "new" tool'a **init image** olarak vererek pixel art versiyonlarını üretecek (1 credit/asset).

Sen YALNIZCA mockup üretiyorsun — PixelLab MCP çağırma. User manuel yapacak.

Önemli:
- Boyut DOĞRU olmalı (S-XL init image dimension lock yapar)
- Canvas DOĞRU olmalı (transparent/neutral BG)
- Composition DOĞRU olmalı (PixelLab "şuna benzet" diyecek)

## 4 Mockup spec

### Mockup 1: edge_n_horizontal
- **Boyut:** 128×64 pixel
- **Canvas:** RGBA transparent background
- **Composition:**
  - Yatay alt kısımda (y=32-64) granite taş blok dizisi
  - 4-6 adet düzensiz dikdörtgen taş bloku (her biri ~20-30 px wide, ~24-32 px tall)
  - Üst kenar çatlak/kırık (zigzag silhouette)
  - Alt kenar düz baseline
  - Renk: charcoal grey (#404550 civarı), light grey highlight üst kenarda (#5a5f6a)
  - Üst yarı (y=0-32) transparent (gökyüzü değil, sadece alpha 0)
  - Çok hafif siyah gölge altta (y=58-64) blokların altında (~alpha 100)

### Mockup 2: edge_w_vertical
- **Boyut:** 64×128 pixel
- **Canvas:** RGBA transparent
- **Composition:**
  - Sağ kısımda (x=32-64) dikey granite taş blok dizisi
  - 4-6 adet düzensiz taş bloku üst üste
  - Sol kenar (x=0-32) transparent
  - İç kenar (x=32) chipped/kırık
  - Renk + highlight + shadow Mockup 1 ile aynı palette
  - Bu sprite mirror-compatible olacak (E için flipX)

### Mockup 3: edge_corner_outer
- **Boyut:** 64×64
- **Canvas:** RGBA transparent
- **Composition:**
  - L-shape: dikey segment (right side, x=32-64, y=0-64) + yatay segment (bottom, x=0-64, y=32-64) birleşik
  - İç köşe x=32, y=32 civarı broken/chipped
  - Granite blok dokulu
  - Renk: aynı palette
  - Üst-sol kısım (x=0-32, y=0-32) transparent

### Mockup 4: edge_rubble_cluster
- **Boyut:** 64×64
- **Canvas:** RGBA transparent
- **Composition:**
  - Merkezde scattered granite chunks (5-8 küçük düzensiz şekil)
  - Çeşitli boyutlar: bazı 8×8, bazı 12×6, bazı 6×10
  - Alt kısımda yoğunlaş (y=32-64)
  - Üst kısım sparser
  - Renk: aynı palette + couple darker shadows
  - Low silhouette, dağılmış görünüm

## Genel kurallar (4 mockup için)

- **Sadece şekil ve baseline color** (taş bloklar grup halinde, basic light/shadow)
- **DETAY YOK** (texture, crack desenleri PixelLab'a bırakılacak)
- **Anti-aliasing YOK** (nearest neighbor pure pixel art baseline)
- **Color palette:**
  - Base granite: #404550
  - Highlight (top edges): #5a5f6a
  - Shadow (bottom): #2a2d35
  - Transparent: alpha 0
- **Pivot reference:** Bottom-center (Unity Pivot Bottom için uyumlu)
- **PNG export:** PIL `image.save(path, 'PNG')` standard, RGBA mode

## Çıktı dosyaları

```
STAGING/concepts/fractured_chamber/edge_mockups/
├─ edge_n_horizontal_mockup_128x64.png
├─ edge_w_vertical_mockup_64x128.png
├─ edge_corner_outer_mockup_64x64.png
└─ edge_rubble_cluster_mockup_64x64.png
```

## User için kullanım rehberi (Codex bunu da yazsın)

`STAGING/a2_edge_user_pixellab_guide.md`:

```markdown
# A2 Edge — PixelLab Üretim Rehberi (User için)

## Workflow
1. PixelLab web UI'a git
2. **Create S-XL Image (new)** tool seç (1 credit/gen)
3. Settings:
   - Init image: yüklenecek mockup PNG
   - Size: mockup ile aynı (init image dimension lock)
   - Remove Background: AÇIK (transparent BG çıktı için)
   - Style image: (opsiyonel) STAGING/concepts/fractured_chamber/a1_floor_wang16_FINAL.png

## 4 Asset için Prompt + Init Image

### Asset 1: North-facing low edge (yatay)
- **Init image:** STAGING/concepts/fractured_chamber/edge_mockups/edge_n_horizontal_mockup_128x64.png
- **Size:** 128×64
- **Prompt:**
> [copy-paste prompt — Codex hazırlayacak, RIMA Shattered Keep style + low edge NOT wall + 3/4 thickness + cyan crack subtle]

### Asset 2: West-facing low edge (dikey, E için mirror)
- **Init image:** edge_w_vertical_mockup_64x128.png
- **Size:** 64×128
- **Prompt:** [copy-paste]

### Asset 3: Outer corner low edge cap
- **Init image:** edge_corner_outer_mockup_64x64.png
- **Size:** 64×64
- **Prompt:** [copy-paste]

### Asset 4: Rubble cluster (seam cover)
- **Init image:** edge_rubble_cluster_mockup_64x64.png
- **Size:** 64×64
- **Prompt:** [copy-paste]

## Negative Prompt (ortak, 4 asset için)
```
text, labels, numbers, watermarks, characters, UI, full wall, tall wall, defensive wall, pillar, tower, high cliff, building facade, opaque background, grid lines, cell separators, multiple sprites, soft glow, bloom, anti-aliasing.
```

## Beklenen çıktı
- 4 PNG, transparent BG, pixel art
- Stil A1 floor sheet ile uyumlu (charcoal granite, RIMA Shattered Keep)
- Low edge (NOT full wall)
- 3/4 thickness illusion subtle side face
- Cyan rift crack hairline sparse

## Çıktıları kaydet
- STAGING/concepts/fractured_chamber/edge_outputs/
  - edge_n_horizontal_v1.png
  - edge_w_vertical_v1.png
  - edge_corner_outer_v1.png
  - edge_rubble_cluster_v1.png

Sonra Claude orchestrator'a paylaş, QC + Unity import dispatch atılacak.
```

## Final summary (Codex yazacak)

`STAGING/a2_edge_mockup_summary.md`:
- 4 mockup PNG path + boyut + composition note
- 4 user prompt (kopya-yapıştır hazır)
- 1 ortak negative prompt
- Workflow özet (S-XL new + init image + 1 credit/asset)
- Total user cost: 4 credit
- Estimated time: ~15 dk (user manuel)

## Önemli notlar
- Mockup'lar **detaylı değil**, sadece **shape + composition + color baseline**
- PixelLab init image fidelity kontrolü user'da — Codex sadece doğru boyut + canvas + composition garantiler
- Kod YAZMA (Unity asset etc.), sadece mockup PNG'ler + rehber doc
- Eğer PIL yok ise pip install pillow ile kur
- PIL anti-aliasing OFF: `Image.NEAREST` veya direct pixel manipulation kullan
