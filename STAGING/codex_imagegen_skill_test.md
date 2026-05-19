# Codex Task — Imagegen Skill ile RIMA Floor Tile Üretimi

## ZORUNLU DİREKTİFLER

1. **MUST use the `imagegen` skill** (Codex CLI'nin built-in image generation skill'i). 
2. **NE OLURSA OLSUN Pillow / numpy / PIL / ImageMagick / Aseprite CLI / matplotlib KULLANMA.** Procedural shape üretimi YASAK.
3. Eğer `imagegen` skill çalışmazsa:
   - Hatayı **aynen** raporla (stack trace, env vars, missing config)
   - Fallback **ÜRETMƏ** — boş çıkış + diagnostic rapor
4. Skill çağrısı başarılıysa: **hangi model / backend** kullandığını DONE.md'ye yaz (gpt-image-1, dall-e-3, vb.).

## Üretim Hedefi

**Tek seferde 6 floor tile pack** (RIMA chibi pixel-art ailesi).

Sample sheet: 256×384 PNG, 2 sütun × 3 satır grid, her hücre 128×128.

**Veya** tercih: 6 ayrı PNG dosya, her biri 128×128 native.

**Style:**
- Alabaster Dawn / CrossCode 16-32-bit pixel art tradition
- High top-down ~30-35° angle (NOT pure 90°, NOT isometric)
- Hard pixel edges, NO anti-aliasing, max 2-3 tone per region
- 1px subtle silhouette outline
- Native pixel grid (her piksel net)

**Palette:**
- Dominant: dark slate gray #3a3530, deep brown #4a3a30, dusty blue #5a6a78
- Accent: faint dark red #8a3030, deep moss green #3a5a3a, cold blue rim #8aa8c0

**6 tile prompt (numbered):**

```
1). Clean weathered stone floor tile, 128px top-down 35 degree, dark slate gray base with deep brown undertone, slight grain variation, ancient ritual temple atmosphere, hard pixel edges, max 3 tones, no anti-aliasing

2). Stone floor tile with sparse moss patch, 128px top-down 35 degree, deep moss green organic spot blending into dark stone, weathered ancient feel, hard pixel edges, max 3 tones

3). Cracked stone floor tile, 128px top-down 35 degree, thin hairline fractures with darker shadow lines, subtle dust accumulation, hard pixel edges, max 3 tones

4). Worn smooth stone floor tile, 128px top-down 35 degree, polished by foot traffic, faint cold blue rim highlight, hard pixel edges, max 3 tones

5). Stained stone floor tile, 128px top-down 35 degree, dusty blue mineral residue, faint abstract sigil-like discoloration, hard pixel edges, max 3 tones

6). Hairline-cracked stone floor tile, 128px top-down 35 degree, cold blue glow at crack edges, weathered ancient temple, hard pixel edges, max 3 tones
```

**Negative anchors (prompt suffix or skill negative field, what skill supports):**
- NOT painterly gradient
- NOT illustrator vector style
- NOT 3D render
- NOT cartoon
- NOT anti-aliased
- NOT isometric

## Output Locations

**Eğer üretildi:**
```
Assets/Sprites/Environment/Codex_Test_v2/
  ├── floor_set_a.png             (256×384 contact sheet, 2×3 grid 128×128)
  └── tiles/
      ├── floor_01_clean.png      (128×128)
      ├── floor_02_moss.png
      ├── floor_03_cracked.png
      ├── floor_04_worn.png
      ├── floor_05_stained.png
      └── floor_06_hairline.png
```

**Eğer ÜRETİLEMEDİ:** boş klasör + DONE.md hatalı yaz.

## DONE.md (Codex zorunlu yazar)

`STAGING/codex_imagegen_skill_test_DONE.md`

İçeriği:

1. **Skill kullanıldı mı?** Evet/Hayır.
2. **Hangi backend?** (gpt-image-1, dall-e-3, sd, vb. — skill ne döndürdüyse yaz).
3. **Tool call parameters** — exact model, size, prompt, seed.
4. **Output dimensions** (PNG header'dan oku — Pillow ile DEĞİL, file inspect ile).
5. **Pixel discipline auto-verdict:**
   - PASS: hard pixel + 128px native + Alabaster Dawn-uyumlu
   - FAIL: soft/blurry/illustrator drift
6. **Conclusion:** RIMA tile production için Codex imagegen skill viable mi? PixelLab ile A/B karşılaştırmaya değer mi?

## Tek hedef

**Eğer imagegen skill mevcut ve çalışıyorsa:** 6 floor tile üret + DONE.md yaz.

**Eğer skill mevcut değil veya başarısız:** Boş çıkış + diagnostic DONE.md.

**Komplikasyon yok.** Procedural fallback yok. Pillow yok.
