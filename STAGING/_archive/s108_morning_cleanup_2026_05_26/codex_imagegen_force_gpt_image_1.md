# Codex Task — Forced gpt-image-1 Single Floor Tile Test

## ZORUNLU DİREKTİFLER (HARD CONSTRAINTS)

**Bu task'in temel amacı:** Codex CLI'nin `gpt-image-1` backend'inin RIMA pixel-art ile uyumlu floor tile üretip üretemediğini **gerçekten** test etmek.

1. **MUST USE `gpt-image-1` model.** Image generation MCP/CLI tool çağır.
2. **MUST NOT use Python/Pillow/numpy/PIL** veya **herhangi bir procedural shape generator**. Önceki dispatch (`bxtnqfrjl`) Pillow'a düşmüştü — bu denemede **kesinlikle yasak**.
3. **MUST NOT use ImageMagick, Aseprite CLI, GIMP CLI** veya başka deterministic CLI image tool.
4. Eğer `gpt-image-1` mevcut değilse veya tool call başarısız olursa: **DURUR, DONE.md'ye "gpt-image-1 unavailable" yazar, ÇIKAR.** Fallback üretme.

## Hedef Output

**Tek bir floor tile, 64×64 pixel art.**

Dosya: `Assets/Sprites/Environment/Codex_Test_v1/floor_tile_gpt_image_1.png`

## Style Reference Target

**Alabaster Dawn / CrossCode 16-32-bit pixel art tradition.**
- Top-down ~30-35° perspective, NOT pure 90°, NOT isometric
- Hard pixel edges, NO anti-aliasing, max 2-3 tones per color region
- 64×64 native pixel art (NOT 256px scaled down)
- 1px subtle silhouette outline (if any)

## Prompt for gpt-image-1

```
A single 64x64 native pixel art floor tile, top-down view at 30-35 degree angle,
weathered dark slate gray stone with subtle dusty blue mineral residue and faint
hairline cracks, deep brown undertone, 1px subtle outline, hard pixel edges,
max 3 tone palette per color region, no anti-aliasing, seamless tileable,
ancient ritual temple atmosphere, Alabaster Dawn / CrossCode pixel art tradition,
NOT painterly, NOT illustrator vector style, NOT 3D render, NOT cartoon
```

## Validation Codex'in yapacağı

DONE.md'ye **kanıt yaz**:

1. **Hangi tool çağrıldı?** Tool name + exact parameters (model, size, prompt).
2. **Çıktı boyutu** (PNG header okumadan native 64×64 değilse fail).
3. **Pixel discipline check:** Output görüntüsünde
   - Anti-aliasing var mı? (1 = soft gradient, 0 = hard edge)
   - Renk sayısı (palette quantize sonrası unique color count)
   - Pixel-grid hizalı mı?
4. **Auto-verdict:** PASS (hard pixel + 64px native + Alabaster Dawn-uyumlu) veya FAIL (soft/blurry/illustrator drift).

## Tek dosya, 1 tool call

Bu görev MİNİMAL. 1 imagegen call, 1 PNG çıktı, 1 DONE.md report. Komplikasyon yok.

## Done file

`STAGING/codex_imagegen_force_gpt_image_1_DONE.md`

İçeriği:
- Tool used + parameters
- Output path + dimensions
- Pixel discipline verdict (PASS/FAIL with evidence)
- Reasoning: gpt-image-1 RIMA pixel-art üretebilir mi? Yan yana PixelLab ile karşılaştırmaya değer mi?

## Output Beklenen

```
Assets/Sprites/Environment/Codex_Test_v1/
  └── floor_tile_gpt_image_1.png    (64×64 RGBA)
STAGING/codex_imagegen_force_gpt_image_1_DONE.md
```
