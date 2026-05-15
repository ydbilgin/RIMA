# Pro UI Pair E/F — Raggedness Pro Versiyon

**Amac:** Yeni `rubble<->cliff_drop` ve `rubble<->rift_pool` tileset'lerini Pro UI raggedness 50% ile uretmek. Pair A/B'den sonra Faz 1+ hedefi — vertical traversal + magical hazard water.

**Uretim noktasi:** PixelLab Pro UI (Maps > Tileset Pro mode, NOT MCP — MCP'de Pro yok)

**Toplam credit:** ~12 (2 Pro tileset x ~6 credit)

**Onemli:** Her iki Pair icin Upper base ID yeni — bu tileset'lerin oncesinde PixelLab'da cliff_drop ve rift_pool icin birer standalone base sprite uretilmeli (Standard mode, tek tile). Uretilen ID'ler asagidaki "[YENI_ID]" alanina yazilir.

---

## Pair E — rubble<->cliff_drop Pro

**Mevcut Standard:** YOK — cliff_drop base henuz mevcut degil (Codex Phase 1 kapsami disinda). Bu run ile ilk kez uretilecek.

**Canonical base IDs (chain icin):**
- Lower (rubble): `2165fb86`
- Upper (cliff_drop): `[YENI_ID]` — oncesinde standalone cliff_drop base sprite uret, ID'yi buraya yaz

**Pro UI ayarlari:**
- Mode: **Top-down tileset Pro**
- Lower base tile ID: `2165fb86` (rubble)
- Upper base tile ID: `[YENI_ID]` (cliff_drop)
- Transition size: **0.30**
- Raggedness: **50%** (ucurum kenari maksimum ragged — erozyon ve kirilma simule)
- Tile size: 32x32
- Style: muted desaturated Salt-and-Sanctuary ravine drop gritty

**Lower description (rubble — yapistir):**
```
Cracked grey-brown rubble ground with scattered small loose stones, dust patches, hairline fractures, eroded debris texture. Muted desaturated palette #5A4F45 / #3F362E / #2A241F. Top-down 35 degree view. No grass, no vegetation, no metal, no clean edges. Worn ancient fortress aftermath.
```

**Upper description (cliff_drop — yapistir):**
```
Dark rocky cliff face dropping into shadow and void below. Deep shadow gradient from dark grey #2A2520 at the rim to near-black #1F1A15 at depth. Rough vertical stone texture with visible rock strata lines. Occasional small dark plant outgrowth clinging to cracks #2A3520. Top-down 35 degree view looking slightly over the drop edge. No water, no light sources, no safe ground — pure vertical hazard depth. The bottom is implied by darkness, not shown.
```

**Transition description (yapistir):**
```
Rubble ground crumbling at the cliff edge. Loose stones rolling and falling off the eroded lip. The rimstone is broken and uneven — no clean edge, only fractured rock progressively giving way to open void. Occasional pebble mid-fall implied by a shadow blur beneath the rim. The boundary is an aggressive eroded lip, irregular and dangerous-looking.
```

---

## Pair E — cliff_drop base sprite (on adim, Pro oncesi)

Bu base sprite'i Pro'dan ONCE Standard single-tile olarak uret. ID'yi not al.

**PixelLab Standard base sprite description (yapistir):**
```
Top-down 35 degree view of a rocky cliff drop. Dark weathered stone rim at top, deep shadow falling into void below. Muted dark palette #2A2520 / #1F1A15 / #2A3520. Small dark mossy plant growth in rock cracks. Rough stone texture, no smooth surfaces. No water, no light, no creatures. Hazard terrain tile for top-down ARPG dungeon. 32x32 pixel art.
```

---

## Pair F — rubble<->rift_pool Pro

**Mevcut Standard:** YOK — rift_pool base henuz mevcut degil. Pair B rift overlay (`6e5e6639`) rift FRACTURE idi; rift_pool durgun havuz farkli estetik.

**Canonical base IDs (chain icin):**
- Lower (rubble): `2165fb86`
- Upper (rift_pool): `[YENI_ID]` — oncesinde standalone rift_pool base sprite uret, ID'yi buraya yaz

**Pro UI ayarlari:**
- Mode: **Top-down tileset Pro**
- Lower base tile ID: `2165fb86` (rubble)
- Upper base tile ID: `[YENI_ID]` (rift_pool)
- Transition size: **0.40** (rift sivisi yayiliyor — gecisin daha genis olmasi gerekiyor)
- Raggedness: **50%** (rift kenari organik — sivi ve korozyon efekti)
- Tile size: 32x32
- Style: Karar #98 palette zorla — cyan #00FFCC + violet #5A2A8A

**Lower description (rubble — yapistir):**
```
Cracked grey-brown rubble ground with scattered small loose stones, dust patches, hairline fractures, eroded debris texture. Muted desaturated palette #5A4F45 / #3F362E / #2A241F. Top-down 35 degree view. No grass, no vegetation, no clean edges.
```

**Upper description (rift_pool — yapistir):**
```
Glowing violet rift pool surface, top-down 35 degree view. Swirling violet liquid core #5A2A8A with slow spiral motion implied by surface texture. Bright cyan energy tendrils #00FFCC radiating outward across the surface in thin fracture lines. Pool reflects nothing — the surface shows only its own inner void glow, otherworldly and lightless. No stones, no ground, pure hazardous magical liquid. Faint foam-like cyan fizzle at the very edge of the pool surface.
```

**Transition description (yapistir):**
```
Rubble ground being slowly dissolved and consumed by encroaching rift pool liquid. The pool edge fizzes with cyan energy foam where the liquid meets solid ground. Stone cracks are stained violet as rift energy seeps into them before full dissolution. The boundary is active and irregular — the pool is expanding, eating into rubble. No clean geometric border, only chemical-magical corrosion in progress.
```

---

## Pair F — rift_pool base sprite (on adim, Pro oncesi)

Bu base sprite'i Pro'dan ONCE Standard single-tile olarak uret. ID'yi not al.

**PixelLab Standard base sprite description (yapistir):**
```
Top-down 35 degree view of a magical rift pool hazard tile. Swirling violet liquid surface #5A2A8A core with cyan energy fractures #00FFCC across it. Otherworldly glowing pool, reflects nothing. Faint cyan fizzle at edges. No stones, no creatures, no vegetation. Hazardous magical water terrain for top-down ARPG dungeon. 32x32 pixel art.
```

---

## Workflow

1. **Pair E — on adim:** Standard mode'da cliff_drop base sprite uret (description yukarda). ID'yi kaydet.
2. **Pair F — on adim:** Standard mode'da rift_pool base sprite uret (description yukarda). ID'yi kaydet.
3. Her iki yeni ID'yi bu dosyadaki `[YENI_ID]` alanina yaz.
4. Pro UI'a gec (Maps > Tileset Pro mode).
5. Pair E yap → Generate → 16 tile cikar → JSON + PNG kaydet → `STAGING/pixellab_tilesets_dump/` altina yeni ID ile.
6. Pair F ayni sekilde.
7. INDEX.md'yi guncelle (yeni 2 satir Pair E Pro + Pair F Pro + cliff_drop base + rift_pool base).
8. Faz 1+ BiomePreset update: cliff_drop + rift_pool terrain ID'leri ShatteredKeep preset'e ekle (Codex dispatch ayri).

---

## QC her tile icin

- [ ] Tile kosesi dogal kivircimli (raggedness etkisi gorunur — kare kose degil)
- [ ] Pair E: transition zone ince ragged ~3-4px (ucurum kenarindan asagi goz kaymasi his)
- [ ] Pair F: transition zone genis aura ~6-7px (rift sivisi yayilma etkisi)
- [ ] Palette spec Pair E: muted earth tonlari, cliff kisminda koyu #1F1A15 void hakim
- [ ] Palette spec Pair F: Karar #98 cyan #00FFCC + violet #5A2A8A ZORLU — baska renk kabul edilmez
- [ ] 16 tile hepsi 4x4 grid Wang positions tutarli (corner tipi dogru)
- [ ] 32x32 tile size
- [ ] Spritesheet downloadable (`tileset_data.spritesheet_url` public CDN)
- [ ] Pair E cliff kisminda "bottom void" hissi var — sadece karanlik, zemin gorsel yok
- [ ] Pair F pool kisminda "reflection of nothing" — parlak ama yansimasiz gorsel

---

## V1 farki (Pair A/B ile karsilastirma)

- Pair E/F icin base sprite on adim ZORUNLU — A/B mevcut ID'leri kullaniyordu, E/F yeni ID gerektirir
- Pair F transition size 0.40 (A/B'den daha genis) — rift sivisi daha agresif yayilma
- Pair E raggedness 50% (A'nin 47%'sinden biraz daha fazla) — ucurum kenari cok duzensiz
- Reference image kullanilmiyor (V8 dersi: tile'larda reference image bias riski yok)
- Raggedness 50% bilerek ust sinir — cliff ve rift kenarlari dogal olarak maksimum duzensiz
