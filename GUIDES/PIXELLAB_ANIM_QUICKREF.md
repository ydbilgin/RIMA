# PixelLab Animasyon Quickref
*Hızlı karar ağacı: PATH A (Animate with Text) vs PATH B (KF + Interpolate). Master Pipeline'a tamamlayıcı.*
*Master pipeline (kapsamlı): [`RIMA_MASTER_ART_PIPELINE.md`](RIMA_MASTER_ART_PIPELINE.md)*

---

## PROMPT YAZMA KURALI

- Tek cümle, tek aksiyon
- Kamera / açı yazma (referanstan geliyor)
- Reference image her zaman açık
- Gereksiz sıfat yok

**Şablon:**
`[karakter kimliği], [tek aksiyon], [1-2 zorunlu teknik detay], pixel art sprite`

---

## KARAR AĞACI

```
PATH A dene:
  Animate with text → tek adım, hızlı
  Beğenilmezse Retry → sonra reuse seed → sonra reduce colors
  Hâlâ istediğin çıkmıyorsa → PATH B'ye geç

PATH B:
  Kaç hareket noktası var?
    Basit (tek geçiş)    → 2 nokta: A → B
    Karmaşık (iki geçiş) → 3 nokta: A → B → C
```

---

## PATH A — Animate with Text
**Ne zaman:** Idle, Run, Death — doğrudan döngü veya düşüş

**Adımlar:**
1. Animate → base sprite yükle
2. Action description → şablon ile tek satır yaz
3. Generate → Retry → reuse seed → reduce colors
4. ✅ Export

**Örnek promptlar:**
```
Idle:   male warrior, breathing slowly with subtle weight shift, sword resting on shoulder, pixel art sprite
Run:    male warrior, dragging massive greatsword behind while running, pixel art sprite
Death:  male warrior, collapsing forward and falling limp to the ground, pixel art sprite
```

---

## PATH B — Keyframe + Interpolate

### 2 Nokta — A → B
**Ne zaman:** Attack, Dash, basit skill (tek geçiş yeterli)

```
A = base/idle (mevcut sprite)
B = hedef poz → Edit Image ile üret
```

**Adımlar:**
1. Edit Image → base sprite yükle → B pozu için prompt yaz → Generate
2. Interpolate → Start = A | End = B | Steps = 1-2 → Generate
3. Aseprite: A + aralar + B → export

**Örnek:**
```
Attack B: male warrior, both hands driving greatsword downward at full extension, pixel art sprite
Dash B:   male warrior, full forward lunge with blade trailing low, pixel art sprite
```

---

### 3 Nokta — A → B → C
**Ne zaman:** Karmaşık skill, V Burst, dönüşüm (iki ayrı geçiş noktası var)

```
A = base/idle
B = ara poz (windup, charge, dönüşümün ortası)
C = peak veya son poz → genellikle base'e dönüş, ya da yeni poz
```

**Adımlar:**
1. Edit Image → B için prompt yaz → Generate → B keyframe
2. Edit Image → C için prompt yaz → Generate → C keyframe (ya da base kullan)
3. Interpolate → A → B | Steps = 1-2
4. Interpolate → B → C | Steps = 1-2
5. Aseprite: A + aralar + B + aralar + C → export
   - B overlap'i bir kez say

**Örnek:**
```
V Burst B (charge): male warrior, both arms pulling back, energy gathering at blade, pixel art sprite
V Burst C (release): male warrior, both arms extending forward, massive burst at blade tip, pixel art sprite
```

---

## Animasyon → Path Haritası

| Animasyon | Path | Nokta | Frame |
|-----------|:----:|:-----:|:-----:|
| Idle | A | — | 2 KF |
| Run | A | — | 4 f |
| Death | A | — | 4-5 f |
| Attack (basit) | B | A→B | ~4 f |
| Dash | B | A→B | ~3 f |
| Attack (combo) | B | A→B→C | ~5 f |
| V Burst | B | A→B→C | ~5 f |
| Karmaşık skill | B | A→B→C | ~6 f |

---

## Yasak

- `"eyes visible"` — kamera açısını kırar
- `"dark fantasy"` — genre etiketi
- Kamera / açı tanımı — reference image'dan geliyor
