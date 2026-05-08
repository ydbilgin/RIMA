# PixelLab Üretim Kılavuzu — Warblade Animasyonları
Date: 2026-05-07 | Status: READY TO GENERATE

---

## TOOL DURUMU (kontrol tarihi: 2026-05-07)

| Eski Ad | Güncel Ad | Not |
|---------|-----------|-----|
| animate-with-text-v3 | **Animate with Text (New)** | Bu tool'u kullan |
| — | **Interpolate (New)** | Frame arası üretim — beat tekniğinde kullan |
| Create Tiles PRO | **Tileset Generation** | İsometric seçeneği var |
| Create Image PRO | **Create Image Pro (Pixflux)** | Pixflux motoru |

> PixelLab sürekli güncellenir. Adında "New" geçen tool varsa onu tercih et.
> Periyodik kontrol: Gemini veya Codex'e `pixellab.ai güncel tool listesi` sor.

---

## GLOBAL AYARLAR

| Ayar | Değer |
|------|-------|
| Tool | **Animate with Text (New)** |
| Canvas | **252 × 252 px** |
| Camera | Low Top-Down (35° — Diablo 2 / Hades stili) |
| Yön sırası | South önce → East → West = East'i yatay flip → North en son |

**YASAKLI (hiçbir promptta kullanma):**
Mor tonlar, el parlamaları, alev efektleri, darbe kıvılcımları, mavi vuruş parlamaları, swoosh trail, baked VFX, sprite içine gömülü durum efektleri.

**Silah kuralı:** Body-only üret → silah second pass (Edit Image Pro ile üstüne ekle).

---

## WARBLADE CHARACTER DEFINITION

Her animasyon promptunda CHARACTER bloğunu kullan.

```
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, angular faceplate, no face visible
BODY: broad-shouldered heavily armored torso, upright combat-ready stance
LIMBS: full plate arms, gauntlets, full plate legs, heavy sabatons
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC
CLOTHING: dark steel plate armor #2e3040 main body, cold blue trim #7BA7BC on pauldron edges and breastplate seams
HANDS: empty (weapon added as separate pass)
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves tapering to sabatons
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps, muted cool palette
```

---

## GLOBAL CONSTRAINTS

Her animasyon promptuna bu bloğu ekle:

```
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.
```

---

## ÜRETİM SIRASI

| Sıra | Animasyon | Frame | Teknik |
|------|-----------|-------|--------|
| 1 | Idle | 8 | Standard |
| 2 | Run — South | 6 | Extreme Pose → Interpolate (New) |
| 3 | LMB Beat 1 (Low Sweep) | 4 | Peak önce → Interpolate (New) |
| 4 | LMB Beat 2 (Overhead Cut) | 5 | Peak önce → Interpolate (New) |
| 5 | LMB Beat 3 / Commit Beat | 5-6 | Peak önce → Interpolate (New) |
| 6 | RMB (Crossguard Bash) | 4-5 | Standard |
| 7 | Hit React | 3 | Standard |
| 8 | Death | 6 | Standard |
| 9 | Run — East | 6 | South ile aynı prompt |
| 10 | North (sırta bakış) | — | En son |

---

## ANİM 1 — Idle

**Frame sayısı:** 8 | **Teknik:** Standard generation

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, angular faceplate, no face visible
BODY: broad-shouldered heavily armored torso, upright at-rest stance, weight evenly distributed
LIMBS: full plate arms hanging relaxed at sides, gauntlets, full plate legs, heavy sabatons flat on ground
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC on pauldron edges and breastplate seams
HANDS: empty, gauntlets open
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves tapering to sabatons
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps, muted cool palette

[ACTION]
Standing still, breathing heavily, slight rhythmic armor sway, helmet tilts 1-2px each cycle, pauldrons rise and fall with breath.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 2 — Run Cycle

**Frame sayısı:** 6 (yön başına)

**Teknik:**
1. **Animate with Text (New)** ile extreme pose üret: yüksek diz kaldırma (left knee up, right foot planted)
2. **Interpolate (New)** ile ara frame'leri doldur
3. Karşı yön için flip

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, angular faceplate, no face visible
BODY: broad-shouldered heavily armored torso, strong forward lean into direction of travel
LIMBS: full plate arms and legs in alternating running motion, heavy sabatons striking ground
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC, pauldrons shifting with arm swing
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC
HANDS: empty gauntlets, arms driving forward and back
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves, sabatons
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
Running fast. Left foot forward and planted on ground, right knee driving up, left arm swinging back, right arm driving forward, torso leaning 15 degrees into movement, armor plates shifting with momentum.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 3 — LMB Beat 1 (Low Sweep)

**Frame sayısı:** 4 | Hasar: 3. frame

**Teknik:**
1. **Animate with Text (New)** ile PEAK frame üret (sweep ortası — kılıç yatay pozisyonda)
2. **Interpolate (New)** ile START ve END frame'leri doldur
3. Dash-cancel penceresi: frame 2-3

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, no face visible
BODY: broad-shouldered heavily armored torso, explosive rotational torque from hips
LIMBS: full plate arms driving wide horizontal sweep, legs planted wide for stability
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC, left pauldron driving forward
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC
HANDS: empty gauntlets gripping weapon
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
PEAK FRAME: Low wide horizontal sword sweep. Both feet planted wide, right foot anchored, left foot pivoting. Torso rotated 45 degrees, right shoulder driving forward and down. Arms fully extended at waist height, mid-sweep position. Center of gravity low and forward.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 4 — LMB Beat 2 (Overhead Cut)

**Frame sayısı:** 5 | Hasar: 4. frame

**Teknik:**
1. **Animate with Text (New)** ile PEAK frame üret (kılıç en yüksek noktada, aşağı inerken)
2. **Interpolate (New)** ile START (kılıç kalkıyor) ve END (kılıç aşağıda) doldur
3. Dash-cancel penceresi: frame 3-4

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, helmet tilted slightly down with downward strike momentum
BODY: broad-shouldered heavily armored torso, maximum downward momentum, shoulders dropping with strike
LIMBS: both arms driving blade downward from above head, right leg stepped forward absorbing impact, left leg pushing from behind
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC, pauldrons dropping with arm motion
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC
HANDS: empty gauntlets, both hands driving down
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
PEAK FRAME: Brutal overhead vertical chop, blade descending. Arms at 45-degree downward angle, shoulders at lowest point of arc. Right foot stepped forward 8px, left foot back and planted. Spine bent 10 degrees forward with strike momentum. Entire body weight behind the blow.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 5 — LMB Beat 3 / Commit Beat (Shoulder Ram)

**Frame sayısı:** 5-6 | Hasar: 4. frame | Bu beat iptal edilemez — proc tetikler

**Teknik:**
1. **Animate with Text (New)** ile PEAK frame üret (forward thrust zirve)
2. **Interpolate (New)** ile doldurun
3. Dash-cancel: YOK (commit beat)

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, helmet driving forward with body weight
BODY: broad-shouldered heavily armored torso, entire body mass surging forward aggressively
LIMBS: right leg driving forward as lunge, left leg fully extended and pushing off, right arm bracing crossguard forward, left arm pulling back for counterbalance
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC, right pauldron driving forward as battering ram
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC
HANDS: empty gauntlets
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
PEAK FRAME: Explosive forward shoulder thrust lunge. Right foot planted forward, left foot fully extended behind pushing off ground, body at 20-degree forward lean. Right shoulder and pauldron leading the charge, maximum forward extension. Weight fully committed, no recovery position.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 6 — RMB (Crossguard Bash)

**Frame sayısı:** 4-5 | **Teknik:** Standard — geniş kılıç sallama DEĞİL, kısa omuz darbesi

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, head tucked slightly down in defensive bracing posture
BODY: broad-shouldered heavily armored torso, weight shifted forward but feet planted, defensive aggression
LIMBS: arms bent and drawn in close to chest bracing crossguard, legs wide and planted for stability
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC
HANDS: empty gauntlets bracing weapon crossguard
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
Short aggressive forward shoulder-check. Feet planted shoulder-width apart, right foot 4px forward. Both arms drawn tight to chest, crossguard braced at sternum level driving forward. Torso leaning 10 degrees forward into bash. Knees slightly bent absorbing own impact force.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 7 — Hit React (Flinch)

**Frame sayısı:** 3 | **Teknik:** Standard

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit, snapping left with impact force
BODY: broad-shouldered heavily armored torso, lurching left under impact, momentarily destabilized
LIMBS: right arm thrown outward for balance, left arm pulling in, right knee slightly buckling, left foot planting hard
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC, right pauldron driven back by impact
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC, armor plate seams widening 2px under impact displacement
HANDS: empty gauntlets splayed
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
Frame 1 (impact): Body lurches left 6px, head snaps left, right shoulder driven back 8px, left arm thrown outward for balance, right pauldron gap widens showing underlayer.
Frame 2 (recoil peak): Body displaced furthest left, spine bent backward 5 degrees, both knees slightly buckled, left boot planting hard, maximum displacement.
Frame 3 (recovery start): Body returning toward center, still 3px left of neutral, head lifting, shoulders beginning to resettle, armor plates realigning.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. 3 frames horizontal strip 756x252px total. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## ANİM 8 — Death

**Frame sayısı:** 6 | **Teknik:** Standard

**Prompt:**
```
[CHARACTER]
TYPE: heavy humanoid warrior
HEAD: full closed steel helmet, narrow dark visor slit
BODY: broad-shouldered heavily armored torso, losing structural integrity frame by frame
LIMBS: full plate arms and legs going limp progressively
EXTRA: wide angular pauldrons with cold blue accent lines #7BA7BC
CLOTHING: dark steel plate armor #2e3040, cold blue trim #7BA7BC
HANDS: empty gauntlets opening as grip fails
SILHOUETTE: wide pauldrons, tall angular helmet, heavy greaves
COLOR: dark steel #2e3040, cold blue accent #7BA7BC, 2-3 shade steps

[ACTION]
Frame 1 (stagger): Body staggers forward 4px, knees beginning to buckle, head dropping 3px, arms falling loose at sides.
Frame 2 (kneel): Right knee contacts ground, left hand reaching forward instinctively, torso pitched 15 degrees forward, helmet visor tilting down.
Frame 3 (collapse): Both knees on ground, torso at 45 degrees forward, arms splayed to sides, pauldrons rotating outward with body mass.
Frame 4 (falling): Torso at 70 degrees, face toward ground, arms no longer bracing, full gravity weight pulling down.
Frame 5 (impact): Full prone contact with ground, armor plate displacement, 1-2px debris chips #1a1c28 scatter from impact points, limbs at final rest position.
Frame 6 (settled): Completely prone and still, slight dust stipple #323446 around body silhouette, shadow ellipse #0e0e18 beneath mass.

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical 252x252px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row for frames 1-4. Frame 5-6 prone — body center aligns to same horizontal axis.

7D Fantasy RPG spritesheet layout. 6 frames horizontal strip 1512x252px total. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## INTERPOLATE (NEW) KULLANIM REHBERİ

Beat animasyonları (LMB 1-2-3) ve Run Cycle için:

1. **Animate with Text (New)** ile sadece PEAK frame'i üret
2. PEAK frame'i kaydet
3. **Interpolate (New)** tool'unu aç
4. Başlangıç pozu (START) ve bitiş pozu (END) olarak idle veya recovery frame'i ver
5. Kaç ara frame istediğini ayarla → tool doldurur
6. Sonuçları QC et — anatomik olmayan kaymaları at
