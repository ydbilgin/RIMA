# WARBLADE — İsometric Sprite & Animasyon Workflow
> **Başlangıç noktası:** `warblade_S.png` mevcut.
> Her adım sırayla — bir sonraki adım bir öncekinin çıktısını kullanır.

---

## ARAÇ REHBERİ (Aseprite Extension)

| Araç | Ne zaman | Nasıl açılır |
|------|----------|--------------|
| **Create Image (M/L)** | Yön üretimi — direction dropdown burada | Ctrl+Space+S → sol üstten "Create" seç |
| **Edit Image PRO** | Animasyon keyframe — mevcut frame'i değiştirir | Ctrl+Space+S → sol üstten "Edit PRO" seç |
| **Interpolate Pro** | Frame arası doldur — 2 frame → N frame | Ctrl+Space+S → sol üstten "Interpolate" seç |

> Canvas 160×160 GENİŞLETME: sadece Interpolate Pro'dan ÖNCE yapılır. Edit Image PRO'ya 128×128 direkt verilir.

---

## AŞAMA 1 — SE YÖN ÜRETİMİ

**Hedef:** warblade_S.png → warblade_SE.png (tüm animasyonların init anchor'ı)

1. Aseprite'ta `warblade_S.png` aç
2. `Ctrl+Space+S` → **Create Image (M veya L)** seç
3. Şu ayarları gir:

| Alan | Değer |
|------|-------|
| Camera view | high top-down |
| Direction | southeast |
| Isometric | ON |
| Init image | YES |
| Strength | **220** |
| Remove BG | ON |
| Output | New frame |

4. Prompt (tek satır):
```
heavy warrior, black iron plate armor with gold trim, long black cape, massive greatsword with blue glowing cracks, vibrant Hades Supergiant Games art style, Fractured Epic
```

5. Beğenilen frame → `File → Export As → warblade_SE.png`

---

## AŞAMA 2 — SW (flipX)

1. `warblade_SE.png` aç
2. `Sprite → Flip Horizontal`
3. `File → Export As → warblade_SW.png`

**Faz 1 minimum tamamlandı: S + SE + SW.**

---

## AŞAMA 3 — ANİMASYONLAR (SE yönü)

Tüm animasyonlar `warblade_SE.png` üzerinden üretilir.

---

### IDLE — 1 Segment, 8f

1. Aseprite'ta `warblade_SE.png` aç
2. `Sprite → Canvas Size → 160×160` (padding: eşit dağıt)
3. `Ctrl+Space+S` → **Interpolate Pro** seç:

| Alan | Değer |
|------|-------|
| Start frame | warblade_SE.png |
| End frame | warblade_SE.png (aynı!) |
| Action | heavy armored warrior breathing, subtle weight shift, isometric southeast, idle loop |
| Frames | 8 |

4. Export: `warblade_idle_SE_f00-07.png`

---

### WALK — 2 Segment, 10f

**ADIM 1 — MID-STRIDE keyframe:**
1. `warblade_SE.png` aç (128×128, canvas genişletme YOK)
2. `Ctrl+Space+S` → **Edit Image PRO** seç:

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, mid-stride walking left leg forward right leg back, weight on front foot, isometric southeast, vibrant Hades art style, pixel art |

3. Export: `warblade_SE_midstride.png`

**ADIM 2 — Interpolate:**
4. Her iki frame'i 160×160'a canvas genişlet
5. **Interpolate Pro:**

| | Seg 1 | Seg 2 |
|-|-------|-------|
| Start | warblade_SE.png | warblade_SE_midstride.png |
| End | warblade_SE_midstride.png | warblade_SE.png |
| Action | warrior walking stride | warrior walking stride return |
| Frames | 5 | 5 |

6. Seg1 + Seg2 birleştir. Export: `warblade_walk_SE_f00-09.png`

---

### ATTACK — 3 Segment, 12f

**ADIM 1 — WINDUP keyframe** (Base'den üret):
1. `warblade_SE.png` aç (128×128)
2. **Edit Image PRO:**

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, both hands raising greatsword high overhead, body arched back weight on rear foot, attack windup pose, isometric southeast, vibrant Hades art style, pixel art |

3. Export: `warblade_SE_windup.png`

**ADIM 2 — PEAK keyframe** (Windup'tan zincirleme üret):
4. `warblade_SE_windup.png` aç (128×128)
5. **Edit Image PRO:**

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, greatsword slammed fully downward arms extended down body bent forward at waist, impact complete, isometric southeast, vibrant Hades art style, pixel art |

6. Export: `warblade_SE_peak.png`

**ADIM 3 — Interpolate (3 ayrı pass):**
7. Tüm frame'leri 160×160 canvas'a genişlet
8. **Interpolate Pro:**

| | Seg 1 — Anticipation | Seg 2 — Strike | Seg 3 — Recovery |
|-|---------------------|----------------|------------------|
| Start | warblade_SE.png | warblade_SE_windup.png | warblade_SE_peak.png |
| End | warblade_SE_windup.png | warblade_SE_peak.png | warblade_SE.png |
| Action | warrior winds up overhead slam | powerful overhead slam strike | recovery to combat stance |
| Frames | **3** | **6** | **3** |

9. Seg1+Seg2+Seg3 birleştir = 12f. Export: `warblade_attack_SE_f00-11.png`

---

### DASH — 2 Segment, 8f

**ADIM 1 — PEAK keyframe:**
1. `warblade_SE.png` aç (128×128)
2. **Edit Image PRO:**

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, body nearly horizontal mid-air lunge, both feet off ground legs extended behind, sword arm reaching forward, explosive dash peak, isometric southeast, vibrant Hades art style, pixel art |

3. Export: `warblade_SE_dashpeak.png`

**ADIM 2 — LANDING keyframe** (Peak'ten zincirleme üret):
4. `warblade_SE_dashpeak.png` aç (128×128)
5. **Edit Image PRO:**

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, skidding to stop after dash, front foot planted hard, body decelerating, sword raised, isometric southeast, vibrant Hades art style, pixel art |

6. Export: `warblade_SE_dashlanding.png`

**ADIM 3 — Interpolate:**
7. Frame'leri 160×160'a genişlet
8. **Interpolate Pro:**

| | Seg 1 | Seg 2 |
|-|-------|-------|
| Start | warblade_SE.png | warblade_SE_dashpeak.png |
| End | warblade_SE_dashpeak.png | warblade_SE_dashlanding.png |
| Action | explosive forward dash | dash landing deceleration |
| Frames | 4 | 4 |

9. Birleştir = 8f. Export: `warblade_dash_SE_f00-07.png`

---

### DEATH — 2 Segment, 12f

**ADIM 1 — STAGGER keyframe:**
1. `warblade_SE.png` aç (128×128)
2. **Edit Image PRO:**

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, staggering backward reeling from fatal blow, off-balance weight shifting backward, knees buckling, sword slipping, isometric southeast, vibrant Hades art style, pixel art |

3. Export: `warblade_SE_stagger.png`

**ADIM 2 — FLAT DEAD keyframe** (Base'den bağımsız üret):
4. `warblade_SE.png` aç (128×128)
5. **Edit Image PRO:**

| Alan | Değer |
|------|-------|
| Init image | YES |
| Strength | 220 |
| Action | same dark armored warrior, collapsed face-down on ground completely flat, arms spread at sides, greatsword fallen beside body, death pose motionless, isometric southeast, vibrant Hades art style, pixel art |

6. Export: `warblade_SE_dead.png`

**ADIM 3 — Interpolate:**
7. Frame'leri 160×160'a genişlet
8. **Interpolate Pro:**

| | Seg 1 — Stagger | Seg 2 — Fall |
|-|-----------------|--------------|
| Start | warblade_SE.png | warblade_SE_stagger.png |
| End | warblade_SE_stagger.png | warblade_SE_dead.png |
| Action | warrior staggers from fatal blow | warrior collapses completely death |
| Frames | **4** | **8** |

9. Birleştir = 12f. Export: `warblade_death_SE_f00-11.png`

---

## AŞAMA 4 — SW ANİMASYONLARI

SE animasyonları tamam → SW için batch flipX:
1. Her SE animasyon dosyasını Aseprite'ta aç
2. Tüm frame'leri seç → `Sprite → Flip Horizontal`
3. `warblade_*_SW_*.png` olarak export

---

## DOSYA YAPISI (hedef)

```
Assets/Sprites/Characters/Warblade/
  base/
    warblade_S.png        ← mevcut ✅
    warblade_SE.png       ← Aşama 1 çıktısı
    warblade_SW.png       ← flipX
  keyframes/              ← ara dosyalar, Unity'e gitmez
    warblade_SE_windup.png
    warblade_SE_peak.png
    warblade_SE_dashpeak.png
    warblade_SE_dashlanding.png
    warblade_SE_stagger.png
    warblade_SE_dead.png
    warblade_SE_midstride.png
  anims/
    idle/    warblade_idle_SE_f00-07.png   (8f)
    walk/    warblade_walk_SE_f00-09.png   (10f)
    attack/  warblade_attack_SE_f00-11.png (12f)
    dash/    warblade_dash_SE_f00-07.png   (8f)
    death/   warblade_death_SE_f00-11.png  (12f)
```

---

## KONTROL LİSTESİ

- [ ] warblade_SE.png üretildi
- [ ] warblade_SW.png (flipX) tamamlandı
- [ ] idle 8f tamamlandı
- [ ] walk 10f tamamlandı (2 seg)
- [ ] attack 12f tamamlandı (3 seg)
- [ ] dash 8f tamamlandı (2 seg)
- [ ] death 12f tamamlandı (2 seg)
- [ ] SW animasyonları flipX tamamlandı
