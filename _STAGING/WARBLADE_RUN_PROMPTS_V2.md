# Warblade Run — V3 Manual Prompts (Enhance OFF)

**Hedef:** Gerçek run cycle. Battle stance shift / weight shift DEĞİL.
**Tool:** PixelLab Custom Animation V3, Pro New mode.
**Mode:** Enhance Action **KAPALI** (description tamamen bizden).
**Frame count:** **8** (loop run cycle: 2 stride × 4 faz).
**Keep First Frame:** **ON** (loop kapansın, stride kırılmasın).
**Start frame:** her yön için ilgili `warbladeNEW#.png`.
**End frame:** BOŞ.
**Style image:** `warrior_idle_128.png`.
**Canvas:** 128×128.

---

## Run Anatomy (8 Frame Loop)

| Frame | Pose | Anatomi |
|---|---|---|
| F1 | Right contact | Sağ topuk yere değer, sol ayak arkada havada (push-off bitti). |
| F2 | Right down | Sağ ayak yere tam basar, ağırlık üstünde, pelvis en alt. |
| F3 | Right pass | Sol diz sağ dizi geçer, pelvis yükselmeye başlar. |
| F4 | Right push-off / Left high | Sağ parmak ucu yerde itiyor, sol diz tepe (knee-up), **iki ayak havada (airborne)**, pelvis tepe. |
| F5 | Left contact | Sol topuk yere değer, sağ ayak arkada havada. F1 mirror. |
| F6 | Left down | Sol ayak yere tam basar. F2 mirror. |
| F7 | Left pass | Sağ diz sol dizi geçer. F3 mirror. |
| F8 | Left push-off / Right high | **İki ayak havada**. F4 mirror. → loop F1. |

**Kural:** En az F4 + F8'de iki ayak da havada. Asla iki ayak aynı anda yere basmaz (run = aerial phase var).

---

## Tek-Paragraf Description (Her Yöne Tek Cümle Akışı)

PixelLab "Description" kutusuna her yön için **ilgili paragrafı aynen yapıştır**. Paragraf tamamen self-contained.

---

### SOUTH (start: `warbladeNEW1.png`)
```
Heavy greatsword warrior running directly toward the viewer in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Both legs alternate visibly in front of the body with wide athletic stride; knees lift toward the camera on the airborne frames; pelvis bobs vertically and rotates slightly side-to-side; torso leans forward 5 to 10 degrees; sword is carried in the right hand at hip level across the right side, blade angled outward and swinging lightly forward and back; left arm swings freely across the front of the body in counter-balance to the leg cycle; beard, hair and any cloth elements trail back from forward momentum. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### SOUTH-EAST (start: `warbladeNEW8.png`)
```
Heavy greatsword warrior running diagonally toward the lower-right of the frame, body in 3/4 front view, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Right shoulder leads the motion; stride reads diagonally along the ground plane; right leg cycles in the foreground while left leg is partially occluded behind the body on the high frames; pelvis bobs vertically and rotates with the stride; torso leans forward 5 to 10 degrees in the diagonal direction; sword is carried in the right hand on the foreground side, blade angled outward, swinging lightly forward of the hip; left arm swings across the body in counter-balance; beard, hair and any cloth trail back from forward momentum. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### EAST (start: `warbladeNEW7.png`)
```
Heavy greatsword warrior running to the right in pure side profile, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. This is the most athletic-reading direction: maximum stride length on screen, full knee lift visible, both legs swing through their complete arc; pelvis bobs vertically with clear up-down rhythm; torso leans forward 5 to 10 degrees; sword is held in the right hand between viewer and body, must remain visible and not clipped behind torso, blade swings lightly forward and back at hip level; left arm swings behind the body on contact frames and in front on push-off frames in counter-balance; beard, hair and any cloth trail back from forward momentum. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### NORTH-EAST (start: `warbladeNEW6.png`)
```
Heavy greatsword warrior running diagonally away from the viewer toward the upper-right of the frame, body in 3/4 back view, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Back of the right shoulder leads the motion; we see the back-right of the torso, top of the head and partial right side of the face; right leg cycles in the foreground, left leg behind the body on the high frames; pelvis bobs vertically and rotates with the stride; torso leans forward 5 to 10 degrees in the diagonal direction; sword is carried in the right hand on the foreground (right) side, blade angled outward from the body, swinging lightly forward and back; hair and any cloak trail strongly back toward the viewer because of forward momentum away from us. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### NORTH (start: `warbladeNEW5.png`)
```
Heavy greatsword warrior running directly away from the viewer in full back view, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Both legs alternate visibly behind the torso, soles of feet briefly readable on the push-off frames; knees lift away from the camera on the airborne frames; pelvis bobs vertically with clear up-down rhythm and rotates slightly with the stride; torso leans forward 5 to 10 degrees away from the viewer; sword is carried in the right hand, visible to the right of the body silhouette, blade angled outward, swinging lightly forward and back; shoulder counter-rotation visible across the upper back; hair and any cloak trail strongly back toward the viewer from the forward momentum away from us. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### NORTH-WEST (start: `warbladeNEW4.png`)
```
Heavy greatsword warrior running diagonally away from the viewer toward the upper-left of the frame, body in 3/4 back view, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Back of the left shoulder leads the motion; we see the back-left of the torso, top of the head and partial left side of the face; left leg cycles in the foreground, right leg behind the body on the high frames; pelvis bobs vertically and rotates with the stride; torso leans forward 5 to 10 degrees in the diagonal direction; sword is held in the right hand which is now on the FAR side of the body — the blade tip must remain visible past the right hip silhouette, do not fully hide the weapon behind the torso; hair and any cloak trail strongly back toward the viewer from the forward momentum away from us. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### WEST (start: `warbladeNEW3.png`)
```
Heavy greatsword warrior running to the left in pure side profile, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Maximum stride length on screen, full knee lift visible, both legs swing through their complete arc; pelvis bobs vertically with clear up-down rhythm; torso leans forward 5 to 10 degrees in the leftward direction; sword is held in the right hand which is on the FAR side of the body — the blade must still read past the torso silhouette, do not fully hide the weapon behind the body; left arm (the foreground arm) swings clearly in counter-balance to the leg cycle; beard, hair and any cloth trail back from forward momentum. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

### SOUTH-WEST (start: `warbladeNEW2.png`)
```
Heavy greatsword warrior running diagonally toward the lower-left of the frame, body in 3/4 front view, in a full 8-frame run cycle: right-foot contact, right down, right pass, airborne with left knee lifted high, left-foot contact, left down, left pass, airborne with right knee lifted high. Left shoulder leads the motion; stride reads diagonally along the ground plane; left leg cycles in the foreground while right leg is partially occluded behind the body on the high frames; pelvis bobs vertically and rotates with the stride; torso leans forward 5 to 10 degrees in the diagonal direction; sword is held in the right hand which is on the FAR side of the body — keep the blade tip visible past the right hip silhouette so the weapon reads, do not fully hide it behind the torso; left arm (foreground) swings in counter-balance; beard, hair and any cloth trail back from forward momentum. Both feet must clearly leave the ground on the two airborne frames. This is a full RUN CYCLE with airborne phase, knee lift, alternating leg swing and forward momentum — NOT a stance shift, NOT a weight shift, NOT a guarded advance, NOT a walk, NOT a battle idle. Do not keep both feet on the ground. Do not freeze the upper body. Do not micro-step. Stride must be wide and athletic. Preserve exact character identity from the reference: same proportions, same partial-armor-over-cloth, same dark steel and brown leather palette, same beard, same skin tone, same greatsword silhouette and size. Top-down ARPG camera at roughly 35 degrees high angle, 128x128 pixel art canvas, black background, character centered, no camera drift, no scale change, no redesign, grounded fantasy with muted desaturated weathered look — not dark fantasy genre, not grimdark.
```

---

## Üretim Akışı

Her yön için PixelLab UI'da:
1. Custom Animation V3.
2. Mode: **Pro New**.
3. Style image: `warrior_idle_128.png`.
4. Reference / start frame: ilgili `warbladeNEW#.png`.
5. End frame: **BOŞ**.
6. Frame count: **8**.
7. **Enhance Action: KAPALI**.
8. **Keep First Frame: AÇIK** (loop için).
9. Description kutusuna ilgili yönün **tek paragrafını** aynen yapıştır.
10. Generate.

**QC kriterleri:**
- F4 + F8'de iki ayak da havada mı? → değilse FAIL.
- Stride genişliği önceki üretimden belirgin daha geniş mi? → değilse FAIL.
- Pelvis vertical bobbing var mı? → düz çizgi → FAIL.
- Sword 8 frame boyunca tutarlı mı (size + el)? → bozuksa FAIL.
- Kimlik ilk frame'le aynı mı (sakal/zırh/palet)? → kayma → FAIL.
- 8 frame loop mu? (F8 → F1 stride break yok) → keep first frame ON kontrolü.

**Test sırası:** EAST → PASS sonrası WEST + SOUTH (kritik gameplay yönleri) → kalan 5.
