# Pilot Prompt — Warblade South (Create Image S-XL New)

**Amaç:** 1 sprite referans pilot. Açı + stil + 64px kalite gate. Onaylanırsa batch'e geçilir.
**Tool:** PixelLab Web App → Create Image **S-XL New** (Pro değil)
**Canvas:** 64x64
**Background:** Transparent ON
**Style preset (eğer varsa):** "Pixel Art Chunky" veya benzeri (varsayılan)

---

## PROMPT (kopyala-yapıştır)

```
TYPE: humanoid pixel art warrior, 64x64 chibi proportions
STYLE: 16-bit chunky pixel art, hard edges, clean pixel clusters, no anti-aliasing, no gradients, no embedded glow
VIEW: 35° high top-down 3/4 perspective (Hades-style, Into Samomor reference), character facing south — looking directly at camera/viewer, feet planted toward viewer

HEAD: medium dark hair, fierce eyes, stoic jaw, no helmet
BODY: heavy build, broad shoulders, upright stance, planted legs
LIMBS: muscular arms gripping greatsword with BOTH hands, knuckles forward
WEAPON: greatsword held vertical at center chest, blade pointing down, hilt visible at chest level, dark steel blade, brown leather wrap on hilt
CLOTHING: heavy plate armor (dark steel grey), DEEP RED surcoat over chest, dark iron pauldrons, brown belt, iron boots
HANDS: both hands firmly gripping greatsword hilt
SILHOUETTE: tall vertical with sword down, broad shoulder plates, recognizable knight profile, asymmetric weapon profile
COLOR: dark steel grey #333333 armor, deep red #8B0000 surcoat, brown leather #4A3520, 2-3 shade steps only
POSE: neutral idle, weight evenly on both feet, weapon held vertical at center, alert but calm

PALETTE: desaturated cool environment tones (this character on dark background), no neon/bloom on character itself (skill VFX is engine-side, NOT in sprite)
LAYOUT: single south-facing frame, character centered, ~60% of canvas height (padding for VFX trails)
RULES: pixel art only, no painterly, no anti-alias, no noise, no embedded glow, hard pixel edges only
```

---

## QC Kontrol Listesi (pilot onayı için)

Pilot çıktısı onaylanmadan önce şu kontrolleri yap:

- [ ] **64x64 boyutu doğru** (Web App çıktısı tam 64x64)
- [ ] **Açı 35° High Top-Down** (zemine basma hissi var, paper Mario flat değil)
- [ ] **South facing** (karakter sana bakıyor, gözler görünür, yan profil değil)
- [ ] **Silah 1-piece** (greatsword karakterin elinde, ayrı sprite gibi durmuyor)
- [ ] **Palette doğru** (dark steel + deep red surcoat — gri/kahve değil)
- [ ] **Padding korunmuş** (karakter canvas'ın %60-70'i, üst/alt boşluk var)
- [ ] **No embedded glow** (sprite üzerinde parlama efekti yok)
- [ ] **Anti-aliasing yok** (kenarlar hard pixel, blurry değil)
- [ ] **Chibi proporsiyon** (büyük kafa, kompakt vücut, oversized DEĞİL)
- [ ] **Silüet okunabilir** (Warblade-knight kimliği net)

## Onay Sonrası Plan

✅ **Onaylanırsa:** Bu sprite "Warblade_south_v1.png" olarak kaydedilir → `PIXELLAB_OUTPUTS/warblade/south_64.png` → batch prompt'ta `style_reference_images` parametresine verilir → 15 sprite Create Image Pro batch çalıştırılır.

❌ **Reddedilirse:** Prompt revize edilir (hangi madde başarısızsa onun directive'i sertleştirilir), pilot tekrar denenir. Max 3 iterasyon.

## Tahmini Maliyet

- Pilot: 1 generation (Create Image S-XL New ~20 gen)
- Batch: 15 generation (Create Image Pro ~16 gen kapasite)
- **Toplam pilot+batch:** ~17 gen (PixelLab 5000 budget'tan minimal)
