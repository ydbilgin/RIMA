---
name: pixellab-create-image-pro-format
description: "PixelLab Create Image Pro V3'te negative prompt ayrı field DEĞİL, ana prompt içinde 'Negative Prompt :' inline yazılır. Tüm batch'lar tek block format."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 20cf7214-b515-4cce-814f-df3b0dd176f2
---

# PixelLab Create Image Pro V3 — Prompt Format LOCK

**Rule:** Create Image Pro V3'te negative prompt için **ayrı field YOK**. Tek prompt kutusu var.

**Why:** Kullanıcı doğrudan test etti ve onayladı (2026-05-18 S87) — negative prompt'u ana prompt'un sonunda `Negative Prompt :` satırıyla inline yazıyor, PixelLab parse ediyor.

**How to apply:** Tüm gelecek batch prompt'larında (prop / mob / character / variant) format şu:

```
[Main prompt content — full description]

Negative Prompt : 
[negative term 1], [negative term 2], [negative term 3], ...
```

**Sonuç:** User tek block kopyalar → tek input field'a yapıştırır → generate. **Ayrı negative field arama**.

## Banned anti-patterns
- ❌ Ayrı "negative prompt:" header'lı kod block önermek
- ❌ "negative prompt field'ına yapıştır" demek
- ❌ Multi-block format ("MAIN PROMPT" + "NEGATIVE PROMPTS" ayrı code block)

## Required pattern
- ✅ Tek code block, sonunda `Negative Prompt :` satırı
- ✅ "Tek block'u kopyala → prompt kutusuna yapıştır" talimatı

## PixelLab Variant Grid Mantığı (S87 LOCK)

Output size → variant count mapping (standard + Custom Size Beta):

| Output Size | Variant Count | Total Image | Use case |
|---|---|---|---|
| 32×32 | 32 variant | 256×256 mixed | Small detail (L5 cracks/scatter) |
| **64×64** | **16 variant** | **256×256 (4×4 grid)** | ★ **Best for prop/floor/chibi/Mob T1** |
| **64×96 CUSTOM (Beta)** | **4 variant** | **256×384** | ★ Native wall Wang16 — NO CROP |
| **64×128 CUSTOM (Beta)** | **4 variant** | **256×512** | Tall props, banners, chains |
| **96×96 CUSTOM (Beta)** | **4 variant** | **384×384** | ★ Native Mob Tier 2 — no scaling artifacts |
| 128×128 | 4 variant | 256×256 (2×2 grid) | Accent L6, Mob T3, Boss |
| 256×256+ | 1 variant | aynı boyut | Tek shot, no variant pick |

**Custom Size Beta avantajı:** Non-standard native sizes manuel crop YOK = doğal look korunur, no resampling artifacts.

**KILIT KURAL:** Tüm variant'lar AYNI PROMPT'un farklı yorumları. Farklı asset (örn. Wang16'nın 16 farklı edge config) → 16 ayrı prompt + her birinde 4-16 variant.

## Cross-link
- Live prop guide: `STAGING/map_prop_production_guide_v5.md`
- Live asset size guide: `STAGING/map_asset_size_guide_LAYERS.md`
- Character batch örneği: `STAGING/character_production_prompts.md` (v11 — main + negative inline format)
