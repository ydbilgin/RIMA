# Warblade Weapon — Anchor Extraction (S-XL New)

**Init image:** `Characters/anchors/warblade.png`
**Mode:** PixelLab Create Image S-XL New (image-to-image, init driven)
**Canvas:** **56×20** (S74 LOCK, horizontal layout)
**Credit:** ~1-2

---

## Reference image description (init image field)

```
Use this image as the source. Extract only the sword the character is holding. Keep the sword's exact pixel style, scale, color palette, and detail. Remove everything else.
```

---

## YAPIŞTIRMA — Main prompt

```
Isolated longsword centered on a fully transparent background, no character, no body, no arm, no hand, no clothing, no armor, no shadow. The sword is the only object visible. Pixel art style matching the reference: dark steel blade with subtle edge highlight, dark leather-wrapped grip, simple crossguard. Single-edged single-handed sword silhouette, blade horizontal across the canvas, point on the right side and grip on the left. Worn weathered field-used steel, no rust, no decoration. Top-down 35° view to match the player sprite render.

Negative prompt: character, body, arm, hand, fingers, glove, sleeve, shoulder, torso, head, face, hair, armor, clothing, cape, shadow under sword, background scenery, text, words, letters, captions, watermark, 3d render, soft shading, blur, anti-aliasing, painterly, photorealistic.
```

---

## Workflow

1. PixelLab Pro UI > **Create Image S-XL New** (image-to-image mode)
2. Init image: `Characters/anchors/warblade.png` upload
3. Init image description'a yukarıdaki kısa metni yapıştır
4. Main prompt'u prompt field'a yapıştır
5. Canvas: **56×20** set
6. Generate (~1-2 credit)
7. Sonuç kontrol: **sadece silah** çıktı mı, karakter parçaları (kol/el) bulaştı mı
8. Drift varsa "Generate again" 1-2 kez (~1 credit/iter)

---

## QC checklist

- [ ] **Sadece silah** görünür — karakter, kol, el, parmak YOK
- [ ] **Transparent background** — solid renk yok
- [ ] **Scale anchor karakterle uyumlu** (warblade.png'deki silah ne ise aynı boyutta)
- [ ] **Stil uyumu** — pixel boyutu, palette, outline anchor ile match
- [ ] Blade orientation: point right, grip left (yatay layout 56×20)
- [ ] No shadow, no glow

---

## Sonuç

Final sprite → `Characters/weapons/warblade_longsword.png` kaydet (veya `Assets/Sprites/Weapons/Warblade/longsword.png` Unity proje structure'ına göre).

Sonra Unity'de WeaponDatabase'e ekle (Codex Phase 2 dispatch HandAnchorAttach Level 2 ile birlikte gelecek).

---

## Spec revize notu

S74 LOCK'taki "Warblade Greatsword 56×20" → Faz 1 için **Longsword 56×20** olarak revize edilebilir (anchor'a sadık). Greatsword spec Karar #80 NLM canon'da two-hand — Faz 2'de **Warblade T2 form variant** olarak büyük greatsword eklenir (Karar #124 yapıyı destekler: "Warblade Base + T2 Rift" form variation).

Faz 1 Warblade silah: **Longsword** (anchor scale + style).
Faz 2 Warblade silah: **Greatsword T2** (canon revival + Rift variant).

Bu yorumu kullanıcı onaylarsa S74 LOCK dosyası "Base = Longsword, T2 = Greatsword Rift" olarak güncellenir.
