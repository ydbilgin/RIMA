# Pro UI Pair A/B — Raggedness Pro Versiyon

**Amaç:** Mevcut Standard `rubble↔wall` ve `rubble↔rift` tileset'lerini Pro UI raggedness 45-50% ile yeniden üretmek. Codex Phase 1 dispatch'i background'da çalışırken paralel iş.

**Üretim noktası:** PixelLab Pro UI (Maps > Tileset Pro mode, NOT MCP — MCP'de Pro yok)

**Toplam credit:** ~12 (2 Pro tileset × ~6 credit)

---

## Pair A — rubble↔wall Pro

**Mevcut Standard:** `9ffbb4d1-79d0-441d-8c23-e1df62e25644` (`STAGING/pixellab_tilesets_dump/9ffbb4d1-...png` referans)
**Canonical base IDs (chain için):**
- Lower (rubble): `2165fb86`
- Upper (wall): `02586a60`

**Pro UI ayarları:**
- Mode: **Top-down tileset Pro**
- Lower base tile ID: `2165fb86` (rubble)
- Upper base tile ID: `02586a60` (wall)
- Transition size: **0.30**
- Raggedness: **47%** (45-50% range içinde)
- Tile size: 32×32
- Style: muted desaturated Salt-and-Sanctuary Shattered Keep dark gritty

**Lower description (rubble — yapıştır):**
```
Cracked grey-brown rubble ground with scattered small loose stones, dust patches, hairline fractures, eroded debris texture. Muted desaturated palette #5A4F45 / #3F362E / #2A241F. Top-down 35° view. No grass, no vegetation, no metal, no clean edges. Worn ancient fortress aftermath.
```

**Upper description (wall — yapıştır):**
```
Dark weathered stone keep wall with deep mortar lines, vertical block pattern, faint moss crawl at base, occasional chipped corner. Muted cold grey-blue palette #4A4F55 / #2F3338 / #1A1D20. Dense solid mass, no openings, no decorative carvings. Salt-and-Sanctuary fortress wall aesthetic.
```

**Transition description (yapıştır):**
```
Rubble debris piling up against and crumbling away from the wall base. Loose stones spilling from broken wall sections. Organic ragged edge where wall structure has eroded into ground rubble. No clean geometric border — the boundary is naturally crumbled and irregular.
```

---

## Pair B — rubble↔rift Pro

**Mevcut Standard chain:** `04633962` (S76 canonical rubble + rift base, dump'ta yok ama yapay üretilmişti)
**Canonical base IDs:**
- Lower (rubble): `2165fb86`
- Upper (rift overlay): `6e5e6639`

**Pro UI ayarları:**
- Mode: **Top-down tileset Pro**
- Lower base tile ID: `2165fb86` (rubble)
- Upper base tile ID: `6e5e6639` (rift overlay)
- Transition size: **0.35** (rift daha çok yayılır)
- Raggedness: **50%** (üst sınır — rift kenarı doğal kıvılcımlı olsun)
- Tile size: 32×32
- Style: Karar #98 palette zorla — cyan #00FFCC + violet #5A2A8A

**Lower description (rubble — yapıştır):**
```
Cracked grey-brown rubble ground with scattered small loose stones, dust patches, hairline fractures, eroded debris texture. Muted desaturated palette #5A4F45 / #3F362E / #2A241F. Top-down 35° view. No grass, no vegetation, no clean edges.
```

**Upper description (rift — yapıştır):**
```
Glowing violet rift fracture surface emanating cyan energy at the cracks. Deep void purple base #5A2A8A with bright cyan #00FFCC fractures running through, faint warmth glow at the edges. Top-down 35° view. Otherworldly hazardous terrain — looks unsafe to walk on. No vegetation, no stones, pure rift corruption.
```

**Transition description (yapıştır):**
```
Rubble ground being CONSUMED by encroaching violet rift corruption. Cyan energy tendrils crawling outward from the rift core into the rubble cracks. Small floating cyan particles drift upward from the boundary. The edge is jagged, irregular, organic — like the rift is actively eating into the ground. No clean geometric border.
```

---

## Workflow

1. Pro UI'a giriş yap (PixelLab Maps mode)
2. Pair A yap → Generate → 16 tile çıkar → JSON + PNG kaydet → `STAGING/pixellab_tilesets_dump/` altına yeni ID ile (ör. `<new_id_A>.png` + `.json`)
3. Pair B aynı şekilde
4. INDEX.md'yi güncelle (yeni 2 satır Pair A Pro + Pair B Pro)
5. Codex Phase 1 dispatch bitince → yeni Pro ID'leri F1 Shattered Keep BiomePreset'te Standard ID'lerle swap (one-line edit)

---

## QC her tile için

- [ ] Tile köşesi doğal kıvılcımlı (raggedness etkisi görünür — kare köşe değil)
- [ ] Transition zone kalın değil (Pair A: ince ragged ~3-4px, Pair B: kalın aura ~5-6px)
- [ ] Palette spec: Pair A muted earth, Pair B Karar #98 cyan+violet zorlu
- [ ] 16 tile hepsi 4×4 grid Wang positions tutarlı (corner tipi doğru)
- [ ] 32×32 tile size
- [ ] Spritesheet downloadable (`tileset_data.spritesheet_url` public CDN)

---

## V1 farkı

- Reference olmadan üretiliyor (V5 character batch'in tersine, tile'larda reference image bias riski yok)
- Raggedness 45-50% bilinçli üst-orta band (40% yetersiz organik, 55%+ kaotik)
- Chain base ID'leri kanonikal (Standard'la stil tutarlı)
