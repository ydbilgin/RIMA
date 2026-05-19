# Codex Review — RIMA Prop Production Prompts Quality Check

## Görev
**Gen budget bugün kısıtlı** — hatalı üretim önleme. Opus 12 prop için PixelLab Create Image Pro V3 prompt'ları yazdı (`STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md`). Codex bu prompt'ları **mantık + clarity + PixelLab uyumluluk** açısından review etsin.

## Context — PixelLab Create Image Pro V3 Mantığı (LOCK)

| Output Size (UI dropdown) | Variant Count | Layout | UI'ın söylediği |
|---|---|---|---|
| 32×32 | 32 variant | mixed grid | "32 variation" |
| **64×64** | **16 variant** | 4×4 grid 256×256 | "16 variation" |
| 96×96 CUSTOM (Beta) | 4 variant | 2×2 grid 384×384 | "4 variation 96×96" |
| **128×128** | 4 variant | 2×2 grid 256×256 | "**2×2 grid 4 variation**" |
| 64×96 CUSTOM (Beta) | 4 variant | 2×2 grid | "4 variation" |
| 64×128 CUSTOM (Beta) | 4 variant | 2×2 grid | "4 variation" |
| 256×256+ | 1 variant | single image | "1 variation" |

**Kritik kural:** PixelLab UI **zaten** size + variant count'u söylüyor (örnek: 128×128 seçince UI "2×2 grid 4 variation" diyor). **Prompt içine bunu yazmak GEREKSIZ + KAFA KARIŞTIRICI.**

Tüm variant'lar AYNI PROMPT'un farklı yorumları (1 prop, N variant) — variant'lar farklı prop'lar değil.

**User feedback:** Prompt'larda "2x2 grid 4 variation" tarzı bilgi VAR şu an — bu gereksiz, kaldırılmalı. Variant separation gerekirse `-` notation kullanılabilir daha net.

## Format reminder — PixelLab Create Image Pro V3

- Negative prompt için **ayrı field YOK** — main prompt'un sonunda inline `Negative Prompt :` satırı
- Tek prompt kutusuna **tek block** yapıştırılır
- Style ref panel **boş** (RIMA convention)
- Camera UI dropdown **High top-down** (RIMA convention)

## Mevcut prompt örneği — STEP 1 (Wooden Crate)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top plane is visible as a flat plane, the front face is visibly angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits completely within with at least 4 pixels transparent padding on all sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood, Vivid Vulnerability palette. Soft oval ground shadow beneath the prop.

A small wooden crate, weathered and chipped, dark brown stained planks with iron banding strips at the top and bottom of the crate body, metal nail rivets visible at the corners. Subtle wear marks and a few pixel-sized scuff marks on the side panels. Approximate body size 36x36 pixels centered within the 64x64 canvas. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple crates in one image
```

## CODEX'TEN İSTENEN

### Q1: Genel Mantık + Format Check

Bu prompt format'ı PixelLab Create Image Pro V3 ile uyumlu mu?
- "ABSOLUTE CANVAS" satırında size mention etmek **gereksiz** mi? (UI dropdown zaten size'ı belirliyor)
- "Approximate body size 36x36 pixels centered within the 64x64 canvas" instruction PixelLab'a nasıl yansıyor? (Pixel-level talimat etkili mi yoksa bağlayıcı değil mi?)
- "Single isolated prop" sufficient mi yoksa daha net direktif gerek mi (örn. "**1 prop only, full body view**")?
- `Negative Prompt :` inline format doğru mu?

### Q2: Risk Faktörleri — Hatalı Üretim Tehlikeleri

Her bir prompt'taki potansiyel problem noktaları:
- Çoklu sigil/text yorumlanabilir keywords var mı? (örn. "sigil-like engraved lines" — gerçek text'e dönüşür mü?)
- "Dripping wax frozen in pixel droplets" benzeri ifadeler ambiguous mu?
- "Fits within the 64x64 canvas with at least 4 pixels transparent padding" PixelLab tarafından parse ediliyor mu?
- "Approximate body size XxY pixels" instruction etkili mi yoksa hayalî sayılar mı?

### Q3: Output Size Stratejisi — Doğru mu?

12 prop için STEP-by-STEP output size kararları:

| Step | Prop | Önerilen Output | Variant | Logic |
|---|---|---|---|---|
| 1 | Wooden Crate | 64×64 standard | 16 | Direct native, simple geometric |
| 2 | Stone Urn | 64×64 standard | 16 | Direct native |
| 3 | Candle | 64×64 standard | 16 | Direct native, small |
| 4 | Debris Pile | 64×64 OR 256→64 | 16 OR 1 | Organic — opsiyonel high-res |
| 5 | Column intact | 64×128 CUSTOM | 4 | Native tall |
| 6 | Column broken | 64×128 CUSTOM | 4 | Native tall |
| 7 | Brazier | 256→64 önerilen | 1 | Organic ember glow |
| 8 | Banner torn | 64×128 CUSTOM | 4 | Native tall |
| 9 | Stone Altar | 256→64 önerilen | 1 | Organic rune glow |
| 10 | Treasure Pile | 256→64 önerilen | 1 | Organic gem detail |
| 11 | Hanging Chains | 64×128 CUSTOM | 4 | Native tall |
| 12 | Kneeling Statue | 256→64 önerilen | 1 | Organic fracture detail |

- Bu output size stratejisi her prop için doğru mu?
- 256→64 downsample tekniği gerçekten "doğal/oval görünüm" katar mı yoksa overrated mi?
- Hangi prop için **MUTLAKA** 256→64 (geometric tile'lar için zararlı olur), hangileri için optional?

### Q4: Prompt Bloat Check

Her prompt ~250-350 token uzunlukta. PixelLab bunu tam parse ediyor mu yoksa son satırlar drop ediyor mu?
- Hangi kısımlar **kesilebilir** (CANVAS rule redundancy gibi)?
- Hangi kısımlar **MUTLAKA korunmalı** (camera angle, style discipline)?
- Negative prompt list'i yeterince spesifik mi yoksa genişletilmeli mi?

### Q5: PASS/MODIFY/FAIL Verdict per Step

Her 12 step için verdict:
- **PASS** — prompt yeterli, üret
- **MODIFY** — küçük tweak gerek (spesifik öner)
- **FAIL** — yeniden yazılmalı (yeni prompt öner)

### Q6: Variant Separation Önerisi

User dedi: "variantları daha net ayırabilirsin - şeklinde 4 variant belli olabilir daha güzel yapabilirsin"

Variant'lar PixelLab tarafından otomatik üretiliyor (1 prompt → N variant). Variant separation NASIL daha net olur:
- Prompt içinde `-` notation mı?
- `Variation A: dark wood / Variation B: light wood` gibi ipucu mu?
- Yoksa sadece prompt'u net yazıp PixelLab'ın doğal variation'ına güvenmek mi?

### Q7: Eksiklikler

Bu 12 prop prompt'unda eksik olan, eklenmeli olan kritik direktifler var mı?
- "seamless tile" (tile'lar için)
- "wear pattern asymmetric" (variation için)
- "no border lines" (sprite borders)
- "centered" (already var)

## Önemli Constraint

- Gen budget bugün kısıtlı — 5000 gen yarın geliyor
- User bu gece 4 P0 prop üretecek
- Hatalı prompt = wasted gen = bugün yapamama

## Çıktı format

`STAGING/codex_review_prop_prompts_DONE.md` — 1500-2500 kelime. Q1-Q7 her birine net verdict. Master MD'ye uygulanacak revisions list (örn. "STEP 1'de X satırı sil, Y ekle").
