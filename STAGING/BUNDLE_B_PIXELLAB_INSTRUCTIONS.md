# Bundle B — PixelLab Create Image Pro V3 (Manual)

L2b Macro Floor Patch kaynak görseli üretimi. Sen elinle yaparsın, ben sonra Python ile crop + alpha mask + RIMA palette quantize edip pool kuracağım.

---

## 1. PixelLab Web UI

1. **pixellab.ai** aç, login.
2. Sol menüden **Create Image** → **Pro V3** seç.
3. Yeni job oluştur.

## 2. Ayarlar (sırayla)

| Ayar | Değer | Neden |
|---|---|---|
| **Size** | **512×512** | Pro max boyutu (memory: reference_pixellab_create_image_pro.md). 512px → 64px chunk = 8×8 = 64 candidate, 128px chunk = 4×4 = 16 candidate. |
| **Detail** | **Low detailed** | 64-128px crop hedefli, low detail = clean pixel art (memory: feedback_pixellab_sxl_low_detail.md). |
| **View** | **Low top-down** | RIMA 30-35° LOCK (memory: project_camera_angle_revisit_s43.md, S86 update). High Top-Down BANNED. |
| **Outline** | **No outline** veya **Single color (light)** | Floor texture'ın etrafına dark outline olmasın. Eğer "No outline" yoksa "Single color" → light gray |
| **Style image** | **BOŞ BIRAK** (ilk deneme) | Drift olursa 2. denemede `ANCHORS/characters/08_elementalist.png` style ref olarak verilebilir (camera/palette baseline) ama karakter bias riski var. İlk denemeyi temiz prompt-driven yap. |
| **Enhance with AI** | **KAPALI** | Memory: feedback_enhance_pixellab_anchor.md — Enhance BANNED. Long-format prompt yeterli. |
| **Negative Prompt field** | YOK (V3'te ayrı field yok) | Memory: feedback_pixellab_create_image_pro_format.md — negative prompt prompt'un sonunda inline. |

## 3. PROMPT — birebir kopyala-yapıştır

```
Large seamless dark slate stone floor area, viewed from a low top-down 30 to 35 degree perspective angle, painterly pixel-art-compatible style with fractured-epic mood. The surface is covered in irregular natural cobblestone shapes of varied sizes ranging from small fist-sized stones to large flagstones, organically packed together without any straight grid lines or repeating patterns. Each stone has subtle worn weathered grain, soft painterly highlights catching cool ambient light from the upper-left, and gentle shadowed edges where stones meet. The dominant palette is muted slate blue-gray (hex around 3A4250) with subtle cooler shadow pockets (hex around 2A323C) and faint warm amber undertones (hex around 6B5840) hinting in deep crack lines and dust accumulations. Stones bleed into neighbors with soft painterly transitions, no hard borders between stones, no decorative frame around the image. The entire 512 by 512 area is a continuous floor surface, edges extending past the canvas naturally as if the floor continues beyond. Plain undecorated stones, no focal markings, no symbols, no glowing elements, no magical effects, no props, no characters, no plants, no debris, no blood, no rune carvings, no painted decoration. Pure low-contrast atmospheric stone floor texture suitable for a dark fantasy ARPG dungeon arena ground. Negative Prompt : gridlines, tile borders, square decoration, repeating pattern, painted symbols, runes, glowing elements, bright orange, bright red, neon colors, blood splatters, rift scars, ritual circles, magical effects, photorealistic noise, high detail clutter, top-down 90 degree, isometric view, side view, perspective lines, characters, props, plants, debris, decorative border, frame, vignette, color drift, dark fantasy genre label, grimdark label
```

## 4. Generate + Download

1. **Generate** bas, ~30-60s bekle.
2. Çıkan görseli incele:
   - Köşelerde gridline/border var mı? → varsa REJECT, prompt'a "no decorative border" tekrarla, re-generate
   - Bright orange/red lekeleri var mı? → REJECT
   - Floor angle düz tepeden mi (90° top-down)? → REJECT, view "Low top-down" doğru seçili mi kontrol et
   - Stones uniform mu (hepsi aynı boy)? → varsa OK ama irregular daha iyi
   - Genel ton slate blue-gray + amber crack accent mi? → OK
3. **Onayladığın görseli indir** (download PNG).
4. **Kaydet:** `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\Phase1A_L2b_Source\floor_source_v1.png`
   - Eğer birden fazla iyi varyant ürettiysen: `floor_source_v1.png`, `floor_source_v2.png`, ...

## 5. Bana haber ver

"floor_source_v1 hazır" de, ben hemen şunları yapacağım:

1. **Python crop:** 64×64 (×64 candidate), 128×128 (×16 candidate), 256×256 (×4 candidate) interior chunk'lar
2. **Irregular alpha mask** her chunk'a (Perlin-blob mask, kare silhouette yok)
3. **RIMA palette quantize** (slate+amber 16-color clamp)
4. **Visual grid render** → sana inceleme için sun
5. **Top 10-20 chunk pick** → MacroFloorPatchAtlasSO pool

## 6. Tahmini süre

- PixelLab generation: 30-60s
- Sen download + save: 1 dk
- Python crop + filter + grid render: 2-3 dk
- **Toplam:** ~5 dk

## Risk + fallback

| Risk | Belirti | Fallback |
|---|---|---|
| Style drift (bright orange/blood) | Çıktıda parlak/dini sembol var | Negative prompt'u güçlendir, re-gen |
| Top-down 90° | Stone'lar düz daire/kare, depth yok | View "Low top-down" mı kontrol, gerekirse "tile_view_angle: 60" override |
| Genre-confused (cartoon/photoreal) | Çok soft veya çok keskin | Detail "Low detailed" doğru mu kontrol |
| Çıktı çok dark | Detay görünmüyor | "subtle warm amber accent" vurgula, "cool ambient light from upper-left" tut |
| Çıktı çok bright | Cream/krem ton | "muted slate blue-gray dominant" vurgula, "no bright colors" ekle |

## Style image alternatifi (Plan B)

İlk deneme drift olursa: style_image olarak `F:\Antigravity Projeler\2d roguelite\RIMA\ANCHORS\characters\08_elementalist.png` ver. Memory `feedback_pixellab_style_ref_instruction.md`: "camera/scale only, not identity" — Elementalist'in camera angle + palette baseline'ı style ref olarak kullanılır, identity bias riskine rağmen RIMA tone'u tutabilir.

Ama bu plan B. İlk denemeyi clean prompt ile yap.
