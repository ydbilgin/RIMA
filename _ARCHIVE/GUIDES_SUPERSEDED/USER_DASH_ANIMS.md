# USER TASK — DASH ANIMATIONS
> Güncelleme: 2026-04-14 | Sen yaparsın. Bitince Claude'a "X dash hazır" de.
> **Toplam: 8 frame | 2 segment (4f burst + 4f landing)**

---

## FRAME KARARI

**Neden 8 frame / 2 segment:**
- Dash HIZLI hissettirmeli — fazla frame dashı yavaşlatır
- Landing keyframe olmadan karakter peak pose'da donuyor
- Her segment 4 frame → AI drift yok, tutarlı çıkış

```
Segment 1: Base → PEAK LEAN    | 4 frame | patlayıcı burst
Segment 2: PEAK LEAN → LANDING | 4 frame | ağırlık absorbe
```

---

## WORKFLOW — Her Karakter İçin

### ADIM 1 — Peak Lean Keyframe (Edit Image PRO)
- Input: `base/{char}_{yön}.png`
- Output size: 128x128, Remove background: ✓
- Kaydet: `dash_frames\peak_{yön}.png`

### ADIM 2 — Landing Keyframe (Edit Image PRO)
- Input: `dash_frames\peak_{yön}.png` *(zincirleme — tutarlılık için)*
- Output size: 128x128, Remove background: ✓
- Kaydet: `dash_frames\landing_{yön}.png`

### ADIM 3 — Segment 1: Base → Peak Lean (Interpolate, 4 frame)
- Start: `base/{char}_{yön}.png`
- End: `peak_{yön}.png`
- Number of frames: **4**

### ADIM 4 — Segment 2: Peak Lean → Landing (Interpolate, 4 frame)
- Start: `peak_{yön}.png`
- End: `landing_{yön}.png`
- Number of frames: **4**

### ADIM 5 — Aseprite Birleştirme
```
1. Yeni dosya: 128×128, 8 frame
2. Seg1: frame 1-4 (Base→Peak)
3. Seg2: frame 5-8 (Peak→Landing)
4. Frame delay: 55ms tümü (hızlı)
5. Export → {anim_name}_{yön}.png
```

---

## KAYIT YERLERİ

```
Keyframeler (geçici):
  Assets\Sprites\Characters\{Char}\dash_frames\peak_{yön}.png
  Assets\Sprites\Characters\{Char}\dash_frames\landing_{yön}.png

Animasyon çıktıları:
  Warblade:     animations\iron-surge\{yön}\frame_000.png
  Elementalist: animations\blink-dash\{yön}\frame_000.png
  Ranger:       animations\vault-dash\{yön}\frame_000.png
  Shadowblade:  animations\shadow-step\{yön}\frame_000.png
```

**Yönler:** S / N / W (East = West mirror, Unity'de flipX)

---

---

# WARBLADE — "Iron Surge" (Ağır Şarj)

*Warblade ağır ama patlayıcı — düşük ağırlık merkezi, forward lean, armor momentum.*

---

## PEAK LEAN keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Heavily armored warrior explosive forward charge — body nearly horizontal, extreme forward lean, armor momentum forward, boots barely touching ground.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, {facing direction}.
Peak of explosive forward charge. Body dropped to extremely low center of gravity, nearly horizontal forward lean. Arms pumping forward aggressively. Heavy armor plates shifted forward from momentum. Boots barely contacting ground at full sprint. Maximum speed burst frame — no recovery yet.
```

---

## LANDING keyframe — Edit Image PRO
*Input: PEAK frame (zincirleme)*

**SHORT**
```
Heavily armored warrior planting hard after charge — one foot forward planted firmly, body absorbing impact, slight forward hunch from stopping force.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, {facing direction}.
Landing pose after forward charge. One foot planted hard forward, body absorbing impact momentum. Slight forward hunch from stopping force. Armor plates settling from deceleration. Weight fully on planted front foot, back foot stabilizing. Combat-ready despite impact.
```

---

## Interpolate Prompts

**Segment 1 — Base → Peak Lean (4 frame)**
```
Heavily armored warrior drops weight low and bursts forward in explosive charge. Body goes nearly horizontal from momentum. Fast aggressive acceleration.
```

**Segment 2 — Peak Lean → Landing (4 frame)**
```
Heavily armored warrior decelerates from explosive charge, plants foot hard, body absorbs stopping impact. Weight shifts to front foot.
```

---

---

# ELEMENTALIST — "Blink" (Teleport Dissolve)

*Elementalist yanıp söner — vücut ateş enerjisine dönüşür, yeniden oluşur.*

**Not: Bu dash 2 ayrı animasyondur.**
- **Blink Out:** Base → peak dissolve (kaybolma)
- **Blink In:** peak dissolve → base (yeniden oluşma, tersten kullanılabilir)

---

## PEAK DISSOLVE keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Robed mage nearly fully dissolved in magical fire energy — body almost gone, only scattered glowing orange particles and faint ghost outline remain.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view, {facing direction}.
Teleport vanish at peak: body nearly fully dissolved into magical fire energy. Only a faint semi-transparent ghost outline visible. Bright orange-gold fire energy particles scattered outward in a burst pattern. Body form almost completely gone — peak dissolve moment.
```

---

## LANDING keyframe — Edit Image PRO
*Input: PEAK frame (zincirleme)*

*Landing = karakter yeniden maddeleşiyor. Peak'ten base'e doğru tersine oluşum.*

**SHORT**
```
Robed mage re-materializing from magical fire energy — body half-formed, still partially translucent, orange-gold particles condensing back into solid form.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view, {facing direction}.
Re-materialization at midpoint: body half-solidified from magical fire energy. Figure partially translucent, still showing orange-gold glow through the form. Fire particles condensing inward. More solid than peak dissolve but not yet fully formed.
```

---

## Interpolate Prompts

**Segment 1 — Base → Peak Dissolve (4 frame)**
```
Robed mage body dissolves into magical fire energy — solid form breaks apart into glowing orange-gold particles, becoming nearly invisible ghost outline.
```

**Segment 2 — Peak Dissolve → Landing (4 frame)**
```
Magical fire energy particles condense and re-solidify, robed mage body re-materializes from ghost form back toward solid shape.
```

*Not: Unity'de blink-in = aynı spritesheet tersten çal (reverse), ayrı animasyon asset gerekmez.*

---

---

# RANGER — "Vault" (Athletic Leap)

*Ranger hızlı atletik sıçrama — tehlikeden geri kaçış, akrobatik.*

---

## PEAK AIRBORNE keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Archer airborne in athletic backward leap — legs tucked up cleanly, bow held at side, body compact midair at peak of arc.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view, {facing direction}.
Peak of backward vault leap. Fully airborne, legs tucked up cleanly at chest. Bow held at side. Body compact in midair at highest point of arc. Slight backward lean from leap direction. Maximum height — feet completely off ground, tuck at peak.
```

---

## LANDING keyframe — Edit Image PRO
*Input: PEAK frame (zincirleme)*

**SHORT**
```
Archer landing from vault — both feet hitting ground, knees bent absorbing impact, slight forward lean from landing force, bow at side.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view, {facing direction}.
Landing after vault leap. Both feet hitting ground simultaneously, knees deeply bent absorbing impact force. Slight forward lean from landing momentum. Bow arm steady at side, quiver settled. Light agile landing — body ready to continue moving.
```

---

## Interpolate Prompts

**Segment 1 — Base → Peak Airborne (4 frame)**
```
Archer pushes hard off ground, launches into backward vault, legs tuck at peak of arc. Fast upward burst into airborne tuck.
```

**Segment 2 — Peak Airborne → Landing (4 frame)**
```
Archer descends from peak airborne tuck, extends legs toward ground, lands with knees bent absorbing impact. Light agile descent to landing.
```

---

---

# SHADOWBLADE — "Shadow Step" (Shadow Dissolve)

*Shadowblade karanlık dumana dönüşür — ateş değil gölge, soğuk ve sessiz.*

**Not: Elementalist gibi 2 ayrı animasyon (out + in).**

---

## PEAK SHADOW DISSOLVE keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Dual-blade assassin nearly fully dissolved into dark shadow smoke — body almost gone, only dark wisps and faint ghost outline remain.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view, {facing direction}.
Shadow vanish at peak: body nearly fully dissolved into dark shadow smoke and wisps. Only a faint ghost silhouette outline visible. Dark smoke and shadow particles scattered outward. Body form almost completely gone — darkness taking over at peak dissolve.
```

---

## LANDING keyframe — Edit Image PRO
*Input: PEAK frame (zincirleme)*

**SHORT**
```
Dual-blade assassin re-solidifying from dark shadow smoke — body half-formed, still partially shadowy and translucent, dark wisps condensing inward.
```

**LONG**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down view, {facing direction}.
Re-materialization midpoint: body half-solidified from shadow smoke. Figure still partially translucent with dark shadow energy visible through the form. Dark wisps condensing inward toward the solidifying body. More substantial than peak dissolve but not yet fully solid.
```

---

## Interpolate Prompts

**Segment 1 — Base → Peak Shadow Dissolve (4 frame)**
```
Dual-blade assassin body dissolves into dark shadow smoke — solid form breaks apart into dark wisps and shadow particles, becoming near-invisible ghost silhouette.
```

**Segment 2 — Peak Shadow Dissolve → Landing (4 frame)**
```
Dark shadow particles and smoke condense and re-solidify, assassin body re-materializes from ghost shadow form back toward solid shape.
```

---

---

# QC

```
DASH PASS:
  ✓ Segment 1: burst/lean clearly readable — karakter yön değişimi net
  ✓ Segment 2: landing hissediliyor (ağırlık absorbe, plant veya solidify)
  ✓ Peak keyframe okunabilir — lean extreme veya dissolve tam görünür
  ✓ Karakter kimliği korunmuş (silah, kıyafet, renk)
  ✓ Dissolve karakterleri: ghost outline görünür (tamamen transparan değil)

FAIL çözümleri:
  Burst yok / static      → Peak frame'i daha extreme yap (daha horizontal lean veya daha dissolved)
  Landing yok             → Landing keyframe'i daha "absorbed impact" yap
  Dissolve görünmüyor     → "nearly completely gone, only faint outline" güçlendir
  Segment geçişi sert     → Landing keyframe'i peak'e görsel olarak daha yakın üret
  Karakter şekil değişti  → Zincirleme bozulmuş — landing'i peak input'tan yeniden üret
```

## RETRY

| Sorun | Çözüm |
|-------|-------|
| Peak frame zayıf | SHORT → Enhance with AI, beğenmezsen LONG direkt |
| Landing tutarsız (silah farklı) | Peak'i input alarak zincirleme yeniden üret |
| Interpolate ortaları bozuk | Daha extreme peak seç, kontrast artır |
| 3 denemede çözülmedi | Claude'a ilet |

---

## BİTİNCE

"Warblade dash hazır" / "Ranger dash hazır" vs. de. Her karakter ayrı bildirilebilir.

Claude şunları yapar:
1. Sprite import (PPU=64, Point, No compression)
2. .anim clip build (loop=false)
3. PlayerAnimator'a Dash state + trigger ekle
4. DashController ile bağla
