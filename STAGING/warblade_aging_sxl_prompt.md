# Warblade Aging Prompt — PixelLab Create Image S-XL (init image based)

## Workflow

1. **Init image:** `ANCHORS/characters/01_warblade.png` (Image #16 v11 batch'inden mevcut Warblade)
2. **Mode:** PixelLab → Create Image S-XL → **"new"** option
3. **Image-to-image strength:** ~0.4-0.55 (yüksek değer = init image'den daha az sapma; aging için orta)
4. **Style:** "Low detailed" (memory: S-XL detail = Low detailed for 64×64 sprites)
5. **Output:** 256×256 → downsample to 64×64 nearest-neighbor (memory: S-XL upscale + downsample workflow)
6. **High Top-Down dropdown:** ⚠️ S-XL'de **BANNED** — kamera açısı init image'den miras alınır (init image zaten 30-35° tilt)

---

## MAIN PROMPT (S-XL init image için kısa odaklı)

```
Same chibi 64x64 character from the init image, kept identical: same body proportions (3 to 4 head chibi), same dark brown leather armor with brass buckle accents, same dark short messy hair, same calm idle pose with weapon-free hands, same high top-down camera angle from the init image.

Make ONLY the face older and weathered, like a tired veteran in his late 40s. Add subtle pixel-level aging: light wrinkle pixels under the eyes, slightly sunken tired eye sockets with a 1-pixel shadow under each eye, slightly deeper crow's feet at the outer eye corners, more grizzled and unkempt beard with the beard slightly longer at the chin and 2 or 3 small grey pixel streaks at the chin, weathered tan skin with one faint 1-pixel scar across the right brow, calm patient slightly downturned mouth corners showing fatigue not anger.

Keep the chibi big-head readable face — eyes and eyebrows and nose and mouth are still distinct clean pixel clusters readable at 64x64. Hard pixel edges, no anti-aliasing, no soft gradient, max 2 tones per color, 1px solid black outline.

The body is fully chibi — only the face features show subtle aging, the body proportions and armor and pose stay exactly as in the init image.
```

---

## NEGATIVE PROMPT

```
young clean face, smooth baby skin, clean shave, child face, teenager face, anime cute face, anime style, beautiful pretty face, idealized hero face, realistic adult head proportions, 5 head tall body, 6 head tall body, slim adult body, tall lean body, smooth gradient on skin, anti-aliasing, soft edges, blur, painterly style, 3d render, weapons of any kind, sword, axe, hilt, scabbard, holster, shield, drawn weapon, weapon in hand, weapon on back, weapon on hip, weapon on belt, ghostly weapon glow, text, words, letters, captions, labels, character names, watermark, signatures, numbers, side view, side profile, three-quarter view, character portrait shot, pure front view, looking up at camera, isometric projection
```

---

## Beklenen Sonuç

- Aynı Warblade anchor body (Image #16 (1,1) slot ile birebir aynı)
- **Yüz:** Yorgun veteran ifadesi, hafif kırışık + sunken eyes + grizzled beard + 1-pixel cheek/brow scar
- Chibi 3-4 head proportion korunur
- 30-35° high top-down açı init image'den korunur
- Silahsız body kuralı korunur

---

## Önemli notlar (S-XL spesifik)

- **"High Top-Down" UI dropdown S-XL'de YASAK** — init image'den miras alınır
- S-XL daha "raw" output verir, Pixelorama cleanup 5-15 dk gerekebilir
- Strength 0.4-0.55 yeterli: 0.7+ = init'i fazla bozar, 0.3- = neredeyse hiç değişmez
- Çıktı 256×256 üretilir → nearest-neighbor downsample 64×64 = sharp pixel output

---

## Eğer S-XL aging başarısız olursa fallback

- **Plan B:** Create Image Pro V3 + state prompt: "make him older" (Karar #145 Use #6) — yeni anchor değil, state-level aging
  - Bu workflow `02_elementalist.png` veya diğer sınıflarda da test edilebilir
- **Plan C:** v12 prompt tweak — sadece Warblade için drift-spesifik aging keywords ekle (canonical v11'in tek-sınıf revize hali)
