# USER TASK — DASH ANIMATIONS
*Güncelleme: 2026-04-09 | Sen yaparsın. Bitince Claude'a "X dash hazır" de.*

---

## ARAÇ KARARI

Tüm karakterler için: **pixellab.ai Edit Image PRO + Aseprite Interpolate (new)**

---

## WORKFLOW — 2 Adım

```
ADIM 1 — pixellab.ai → Edit Image PRO
  → O yönün base sprite'ını yükle
  → SHORT prompt → "Enhance with AI" dene
  → Beğenmezsen LONG prompt direkt dene
  → Peak dash pozu (maksimum uzanma/lean anı) PNG olarak kaydet

ADIM 2 — Aseprite → Animate > Pro Tools → Interpolate (new)
  → Set start image = base/{char}_{yön}.png (idle/ayakta)
  → Set end image   = ADIM 1 peak dash pozu
  → Action description = aşağıdaki Interpolate prompt
  → Number of frames: 8
  → Generate
```

**Edit Image PRO ayarları (sabit):**
- Output size: 128x128
- Remove background: ✓ açık

---

## ASEPRITE KURULUM

1. Aseprite aç → `Edit → PixelLab → Open Plugin` (veya `Ctrl+Space+P`)
2. Sol panelde **Animate → Pro Tools → Interpolate (new)**

---

## BASE SPRITE YOLLARI

```
Warblade:     F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Warblade\base\warblade_{yön}.png
Elementalist: F:\...\Elementalist\base\elementalist_{yön}.png
Ranger:       F:\...\Ranger\base\ranger_{yön}.png
Shadowblade:  F:\...\Shadowblade\base\shadowblade_{yön}.png
```

---

## YÖNLER

3 yön üret: **S / N / W**
East = West mirror → Unity'de Sprite Renderer flipX yapılır, ayrı üretme.

---

## KAYIT YERLERİ

```
Peak dash frame (geçici):
  RIMA\Assets\Sprites\Characters\{Char}\dash_frames\dash_{yön}.png

Animasyon çıktıları:
  Warblade:     animations\iron-surge\south\frame_000.png
                animations\iron-surge\north\frame_000.png
                animations\iron-surge\west\frame_000.png
  Elementalist: animations\blink-dash\south\frame_000.png
                animations\blink-dash\north\frame_000.png
                animations\blink-dash\west\frame_000.png
  Ranger:       animations\vault-dash\south\frame_000.png
                animations\vault-dash\north\frame_000.png
                animations\vault-dash\west\frame_000.png
  Shadowblade:  animations\shadow-step\south\frame_000.png
                animations\shadow-step\north\frame_000.png
                animations\shadow-step\west\frame_000.png
```

Base path: `F:\Antigravity Projeler\2d roguelite\RIMA\`

---

# ══════════════════════════════
# WARBLADE — "Iron Surge" (Ağır Şarj)
# ══════════════════════════════

Warblade dashes forward in a short explosive charge — low center of gravity, heavy forward lean.

---

## ADIM 1 — Edit Image PRO (Peak Dash Pose)

### South

**SHORT**
```
Heavily armored warrior explosive forward charge — body nearly horizontal, extreme forward lean, armor momentum forward, boots barely touching ground.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, facing downward toward camera.
Peak forward charge moment: body dropped to extremely low center of gravity, nearly horizontal forward lean. Arms pumping forward. Heavy armor plates shifted forward from momentum. Boots barely touching ground at full sprint. Maximum speed burst frame.
```

### North

**SHORT**
```
Heavily armored warrior explosive charge away from camera — body nearly horizontal, extreme forward lean away, armor momentum forward.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, facing upward away from camera.
Peak forward charge away from camera: body at extremely low angle, nearly horizontal forward lean away. Arms pumping. Armor plates shifted forward from momentum. Maximum speed burst frame.
```

### West

**SHORT**
```
Heavily armored warrior explosive charge to the left — body nearly horizontal left lean, armor momentum left, boots barely touching.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, facing left.
Peak leftward charge: body at extremely low angle, nearly horizontal lean to the left. Arms pumping leftward. Armor plates shifted from leftward momentum. Maximum speed burst frame.
```

---

## ADIM 2 — Interpolate (Dash Motion)

**South / North / West için aynı prompt (yön kelimesini değiştir):**

**SHORT**
```
Armored warrior explosive charge dash — drops weight, full speed burst forward, plants to stop. No loop.
```

**LONG**
```
Heavily armored plate-clad warrior. Drops weight low — explosive burst forward at full sprint — momentum carries to planted landing stance. Heavy armor shakes from impact. No loop. Dash animation, 8 frames.
```

---

# ══════════════════════════════
# ELEMENTALIST — "Blink" (Teleport Dash)
# ══════════════════════════════

Elementalist vanishes in a burst of magical fire energy — teleport dissolve effect.

---

## ADIM 1 — Edit Image PRO (Peak Dissolve Frame)

Peak frame = karakter neredeyse tamamen çözülmüş, sadece enerji parçacıkları kaldı.

### South

**SHORT**
```
Robed mage nearly fully dissolved in magical fire energy — body almost gone, only scattered glowing orange particles and faint ghost outline remain.
```

**LONG**
```
Robed battle mage, 128x128 pixel art, low top-down view, facing downward toward camera.
Teleport vanish at peak: body has nearly fully dissolved into magical fire energy. Only a faint semi-transparent ghost outline visible. Bright orange-gold fire energy particles scattered outward in a burst pattern. Body form almost completely gone.
```

### North

**SHORT**
```
Robed mage nearly fully dissolved in magical fire energy — body almost gone, only scattered glowing orange particles and faint ghost outline remain, facing away.
```

**LONG**
```
Robed battle mage, 128x128 pixel art, low top-down view, facing upward away from camera.
Teleport vanish at peak: body nearly dissolved into magical fire energy. Faint semi-transparent outline. Orange-gold fire particles scattered outward. Body form almost completely gone.
```

### West

**SHORT**
```
Robed mage nearly fully dissolved leftward in magical fire energy — body almost gone, glowing particles scattered left, faint ghost outline.
```

**LONG**
```
Robed battle mage, 128x128 pixel art, low top-down view, facing left.
Teleport vanish at peak: body dissolving leftward into magical fire energy. Faint ghost outline. Orange-gold particles scattered outward to the left. Body form nearly gone.
```

---

## ADIM 2 — Interpolate (Blink Dissolve Motion)

**SHORT**
```
Robed mage body dissolves into magical fire energy particles — fades from solid to nearly invisible ghost. No loop.
```

**LONG**
```
Robed battle mage teleport vanish: body begins dissolving — fire energy particles peel off edges, body becomes semi-transparent, then nearly invisible ghost outline, then almost completely gone. No loop. Blink disappear animation, 8 frames.
```

---

# ══════════════════════════════
# RANGER — "Vault" (Athletic Leap)
# ══════════════════════════════

Ranger does a fast athletic vault leap — backward leap away from danger.

---

## ADIM 1 — Edit Image PRO (Peak Airborne Frame)

Peak frame = karakter havada, bacaklar toplu, tam hava anı.

### South

**SHORT**
```
Archer airborne in athletic backward leap — legs tucked up cleanly, bow held to side, body compact midair, peak of arc.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view, facing downward toward camera.
Peak of backward vault leap: fully airborne, legs tucked up cleanly. Bow held at side. Body compact in midair. Slight backward lean from leap direction. At highest point of arc — maximum height, maximum tuck.
```

### North

**SHORT**
```
Archer airborne in athletic forward leap — legs tucked up, bow held to side, body compact midair, peak of arc.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view, facing upward away from camera.
Peak of forward vault leap: fully airborne, legs tucked cleanly. Bow held at side. Body compact. Forward momentum visible. At highest point of arc.
```

### West

**SHORT**
```
Archer airborne in athletic sideward leap left — legs tucked up, bow held, body compact midair, peak of arc.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view, facing left.
Peak of sideward vault leap to the left: fully airborne, legs tucked. Bow held at side. Body compact, moving leftward. At highest point of arc.
```

---

## ADIM 2 — Interpolate (Vault Motion)

**SHORT**
```
Archer pushes off ground in athletic vault leap — rises to peak airborne tuck — descends landing crouched. No loop.
```

**LONG**
```
Archer ranger performs athletic vault leap: pushes hard off ground, launches into air, legs tuck at peak of arc, bow swings at side. Descends to crouched landing with knees bent absorbing impact. Light agile movement. No loop. Dash animation, 8 frames.
```

---

# ══════════════════════════════
# SHADOWBLADE — "Shadow Step" (Shadow Vanish)
# ══════════════════════════════

Shadowblade dissolves into dark shadow energy — shadow teleport effect.

---

## ADIM 1 — Edit Image PRO (Peak Shadow Dissolve Frame)

Peak frame = karakter neredeyse tamamen karanlık dumana dönmüş.

### South

**SHORT**
```
Dual-blade assassin nearly fully dissolved into dark shadow smoke — body almost gone, only dark wisps and faint ghost outline remain.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view, facing downward toward camera.
Shadow vanish at peak: body has nearly fully dissolved into dark shadow smoke and wisps. Only a faint ghost silhouette outline visible. Dark smoke and shadow particles scattered outward. Body form almost completely gone, darkness taking over.
```

### North

**SHORT**
```
Dual-blade assassin nearly fully dissolved into dark shadow smoke — body almost gone, dark wisps scattered, facing away.
```

**LONG**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down view, facing upward away from camera.
Shadow vanish at peak: body nearly dissolved into dark shadow smoke. Faint ghost silhouette. Dark smoke scattered outward. Body form almost gone.
```

### West

**SHORT**
```
Dual-blade assassin nearly fully dissolved leftward into dark shadow smoke — body almost gone, dark wisps scattered left.
```

**LONG**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down view, facing left.
Shadow vanish at peak: body dissolving leftward into dark shadow smoke. Faint ghost silhouette. Dark smoke and wisps scattered to the left. Body form nearly gone.
```

---

## ADIM 2 — Interpolate (Shadow Step Dissolve Motion)

**SHORT**
```
Dual-blade assassin body dissolves into dark shadow smoke — fades from solid to nearly invisible dark wisps. No loop.
```

**LONG**
```
Dual-wielding dark assassin shadow teleport vanish: dark energy particles begin peeling off body edges, body becomes semi-transparent shadowy form, then barely visible dark silhouette outline, then almost completely dissolved into shadow smoke. No loop. Shadow step disappear animation, 8 frames.
```

---

# ══════════════════════════════
# QC — Kalite Kontrol
# ══════════════════════════════

```
PASS kriterleri:
  ✓ Başlangıç frame = base sprite ile eşleşiyor
  ✓ Peak frame okunabilir (lean, dissolve, airborne — netçe görünüyor)
  ✓ Hareket temiz ve dinamik
  ✓ Karakter kimliği korunmuş

FAIL çözümleri:
  Hareketsiz / static → Edit Image PRO'da daha uç (extreme) poz seç
  Dissolve görünmüyor  → "body becoming transparent, semi-transparent, dissolving" ekle
  Airborne görünmüyor  → "fully off ground, feet not touching ground" ekle
  Peak frame zayıf     → Edit Image PRO'da farklı varyasyon seç (SHORT dene)
  Interpolate bozuk    → Peak frame'i daha net farklı üret
```

## RETRY STRATEJİSİ

| Deneme | Değişiklik |
|--------|-----------|
| 1 | SHORT prompt → Enhance with AI |
| 2 | LONG prompt direkt |
| 3 | Edit Image PRO'da farklı varyasyon seç |
| 4 | Claude'a ilet |

---

## BİTİNCE

"Warblade dash hazır" / "Elementalist dash hazır" / vs. de.
Her karakter ayrı bildirilebilir.

Claude şunları yapar:
1. Sprite import (PPU=64, Point, No compression)
2. .anim clip build (loop=false)
3. PlayerAnimator'a Dash state + trigger ekle
4. DashController script ile bağla
