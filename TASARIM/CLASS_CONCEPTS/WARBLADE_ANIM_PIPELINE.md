# WARBLADE ANÄ°MASYON PÄ°PELINE
> Idle + Run animasyon Ã¼retim kÄ±lavuzu.
> Concept image: `Characters/Warblade/warrior_idle_128.png`
> Ã‡Ä±ktÄ± klasÃ¶rÃ¼: `Assets/Sprites/Characters/Warblade/`

---

## PixelLab Sabit Ayarlar (Run keyframe Ã¼retimi iÃ§in)

| Alan | DeÄŸer |
|------|-------|
| Tool | Create from Reference |
| Concept Image | `Characters/Warblade/warrior_idle_128.png` |
| Camera View | high top-down (= 35Â°) â€” UI'dan ver, prompta YAZMA |
| Size | 128px |
| Preset | male human |
| Outline | single color black |
| Shading | medium |
| Detail | medium |
| AI Freedom | 0 |

**YÃ¶n sÄ±rasÄ± (clockwise):** S â†’ SE â†’ E â†’ NE â†’ N â†’ NW â†’ W â†’ SW
Dosyalar oluÅŸturma zamanÄ±na gÃ¶re sÄ±ralanÄ±r: 1. dosya = S, 8. dosya = SW

---

## IDLE ANÄ°MASYON

**PixelLab Ã¼retimi yok** â€” idle base zaten hazÄ±r. Sadece Aseprite.

### Pose ReferansÄ± (her yÃ¶nde aynÄ± kural)
- KÄ±lÄ±Ã§ saÄŸ omuzda dÃ¼z yatÄ±yor, aÄŸzÄ± arkaya bakÄ±yor
- SaÄŸ el kabzayÄ± crossguard'a yakÄ±n tutuyor
- Sol kol yanÄ±nda rahat sarkÄ±yor
- DuruÅŸ: aÄŸÄ±r, sakin, hazÄ±r

### Aseprite AdÄ±mlarÄ± (her yÃ¶n iÃ§in tekrarla)

1. Ä°lgili yÃ¶nÃ¼n idle base PNG'sini Aseprite'ta aÃ§
2. Frame 1'i Ã§oÄŸalt â†’ 2 Ã¶zdeÅŸ frame
3. **Frame 2'de kÃ¼Ã§Ã¼k nefes deÄŸiÅŸikliÄŸi boyat:**
   - Omuzlar 1-2px aÅŸaÄŸÄ± kayar
   - Omuzda duran kÄ±lÄ±Ã§ 1-2px Ã¶ne/aÅŸaÄŸÄ± kayar
   - BaÅŸka hiÃ§bir ÅŸey deÄŸiÅŸmez
4. Frame 1 â†’ saÄŸ tÄ±k â†’ **Set as Loop Start (First)**
5. Frame 2 â†’ saÄŸ tÄ±k â†’ **Set as Loop End (Last)**
6. Ãœst menÃ¼: **Cel â†’ Interpolate Cels** â†’ Between frames: **2** â†’ OK
7. Toplam 4 frame: `F1 â†’ I1 â†’ I2 â†’ F2` â†’ loop baÅŸa dÃ¶ner
8. Export: `warblade_idle_[yÃ¶n]_anim.png` (sprite sheet)

---

## RUN ANÄ°MASYON

### Keyframe A â€” Sol ayak Ã¶ne

```
short dark hair, grey steel full plate armor with cold blue rift crack scars on left shoulder pauldron, left leg striding forward with foot firmly planted, right leg pushing off behind, both hands gripping two-handed hilt with right hand near crossguard left hand near pommel, massive blade dragging angled behind right hip, body aggressively forward-leaning, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette
```

### Keyframe B â€” SaÄŸ ayak Ã¶ne

```
short dark hair, grey steel full plate armor with cold blue rift crack scars on left shoulder pauldron, right leg striding forward with foot firmly planted, left leg pushing off behind, both hands gripping two-handed hilt with right hand near crossguard left hand near pommel, massive blade dragging angled behind left hip, body aggressively forward-leaning, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette
```

### PixelLab Ãœretim AdÄ±mlarÄ±

1. Ã–nce **South** yÃ¶nÃ¼nde Keyframe A Ã¼ret â†’ QC geÃ§
2. South Keyframe B Ã¼ret â†’ QC geÃ§
3. Ä°kisi onaylandÄ±ktan sonra kalan 7 yÃ¶n iÃ§in tekrarla (A+B)

### Aseprite Montaj AdÄ±mlarÄ± (her yÃ¶n iÃ§in)

1. Yeni 128Ã—128 Aseprite dosyasÄ± aÃ§
2. Keyframe A â†’ Frame 1'e kopyala/yapÄ±ÅŸtÄ±r
3. Frame 3 ekle â†’ Keyframe B'yi yapÄ±ÅŸtÄ±r (Frame 2 ÅŸimdilik boÅŸ)
4. Frame 1 = First, Frame 3 = Last olarak iÅŸaretle
5. **Interpolate Cels** â†’ Between frames: **1** â†’ OK
   â†’ A + interp + B (3 frame)
6. Frame 4 ekle: Frame 1'in kopyasÄ±nÄ± yapÄ±ÅŸtÄ±r (loop kapanmasÄ±)
7. Frame 3 ve Frame 4 arasÄ± â†’ **Interpolate Cels** â†’ Between: **1**
   â†’ Toplam: A + I + B + I = **4 frame** loop
8. Export: `warblade_run_[yÃ¶n]_anim.png`

---

## QC Kontrol (Her yÃ¶n, her frame)

**Idle:**
- [ ] KÄ±lÄ±Ã§ saÄŸ omuzda kaldÄ± (kayÄ±p yok)
- [ ] Nefes hareketi 1-2px â€” fazla deÄŸil
- [ ] Loop seamless (son frame â†’ ilk frame boÅŸluk yok)

**Run:**
- [ ] A ve B'de kÄ±lÄ±Ã§ zÄ±t tarafta sÃ¼rÃ¼nÃ¼yor (A=saÄŸ arka, B=sol arka)
- [ ] Her iki elde aynÄ± anda hilt tutuluyor (tek el bÄ±rakmÄ±yor)
- [ ] Ä°leri lean her iki frame'de sabit
- [ ] Blade yerde sÃ¼rÃ¼nÃ¼yor â€” omuz seviyesine Ã§Ä±kmÄ±yor

---

## Ã‡Ä±ktÄ± KlasÃ¶r YapÄ±sÄ±

```
Assets/Sprites/Characters/Warblade/
  base/          â† mevcut idle base'ler (8 yÃ¶n, statik)
  idle/          â† animasyon Ã§Ä±ktÄ±sÄ± (8 yÃ¶n, 4 frame)
  run/           â† animasyon Ã§Ä±ktÄ±sÄ± (8 yÃ¶n, 4 frame)
```


