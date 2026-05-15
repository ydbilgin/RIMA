# Ravager Tek-Sprite Regenerate — Baltasız agresif

**Sebep:** V8 batch Ravager'da subtle hatchet kabza-formları kaldı. Edit Image mask-based silme çalışmadı. Tek-sprite Pro UI regenerate ile agresif silahsız prompt.

**Init image:** `Characters/idle_batch_v8/05_ravager.png` (mevcut sprite reference, identity koru)
**Mode:** PixelLab Pro UI Create Image Pro **single character** (variations = 1) veya Create Image S-XL New
**Canvas:** 64×64
**Credit:** ~1-2

---

## Reference image description (init image field)

```
Match this character identity exactly — same wild red hair, bare scarred arms, dark blood-red hide armor, crimson torn cloth wraps, bulky hunched build, predator crouch. Change only one thing: remove any weapon or axe-shape from both hands entirely. Hands must be bare clenched fists with visible knuckles, nothing held, no axe handle, no blade, no metal, no wood, no weapon outline of any kind. Empty hand grip pose, but the hands themselves are pure bare fists.
```

---

## YAPIŞTIRMA — Main prompt

```
ABSOLUTE RULE: the generated image must contain NO weapon, NO axe, NO hatchet, NO blade, NO handle, NO weapon shape of any kind. Both hands must be bare clenched fists with visible bare knuckles — completely empty, holding nothing.

Bulky muscular hunched male berserker with wild dark-red shoulder-length hair, scarred bare arms, dark blood-red rough hide armor with crimson torn cloth wraps at the waist. Aggressive forward predator crouch, shoulders rolled forward, weight low. Both fists clenched tight and held at hip level in aggressive empty-handed stance — knuckles bare and clearly visible, fingers curled into pure fist form, no object held in either hand, no weapon outline, no metal axe-head, no wooden handle, no leather grip, no blade, no shadow that could read as a weapon.

Pixel art chibi style, 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients. Chibi proportions: oversized head ~40% of total height, broad shoulders, stubby legs ~25%. Top-down 35° view, south-facing front, both eyes visible.

Negative prompt: axe, hatchet, dual hatchets, axe head, axe blade, wooden handle in hand, weapon in hand, weapon handle, weapon grip, brown object in fist, brown shape near hand, metal blade, weapon outline, ghostly weapon, transparent weapon, weapon shadow, anything held in hand, anything in fist, text, words, letters, captions, labels.
```

---

## Workflow

1. Pro UI **single mode** (variations = 1) — Pro veya S-XL New ikisi de OK
2. Init image: `Characters/idle_batch_v8/05_ravager.png` ekle
3. Init description'a yukarıdaki metni yapıştır (ravager identity + sadece silah sil)
4. Main prompt'u prompt field'a yapıştır
5. Canvas: **64×64**
6. Generate (~1-2 credit)
7. Yumruklar bare visible mı kontrol et
8. Drift varsa "Generate again" (~1 credit/iter, 2-3 deneme yeterli)

---

## QC checklist

- [ ] **İki yumrukta da hiçbir silah/balta formu YOK** — pure bare fist
- [ ] Wild red hair ✓
- [ ] Scarred bare arms ✓
- [ ] Dark blood-red armor ✓
- [ ] Bulky hunched predator crouch ✓
- [ ] Chibi proportions
- [ ] Transparent BG
- [ ] No text

---

## Eğer regenerate de çalışmazsa

PixelLab Edit Image **Inpaint** mode'unu dene (mask atmayan, prompt-based area edit):
1. Edit Image > Inpaint
2. Init: `05_ravager.png`
3. Mask BRUSH ile her iki yumruğun **etrafındaki kahverengi balta-formlarını** kabaca işaretle (overdraw OK)
4. Inpaint prompt: `bare clenched fist knuckles, no weapon, empty hand, skin tone matching arms`
5. Generate, hand area sadece bare fist olarak fill edilir

Eğer Inpaint da fail ederse → manual pixel edit (Aseprite/GIMP) — kahverengi pixeller'i sil, fist outline'ı doldur. ~5 dk manual iş.

---

## Output

Final sprite → `Characters/idle_batch_v8/05_ravager.png` overwrite (eskisini `_archive/v8_initial/05_ravager_with_axes.png` arşivle)
