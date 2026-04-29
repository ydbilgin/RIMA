# 18 NİSAN — PixelLab Üretim Talimatı
*Faz 1 kapsamı: Warblade + Crimson Crypt Tileset + VFX*
*Her adım adım adım. Revize gerekmeyecek şekilde yazıldı.*

---

## ⚠️ KAYDETME KURALI — Her Üretimde Zorunlu

**SAĞ TIKLA KAYDETME.** Sağ tık → "Resmi farklı kaydet" alpha kanalı olmayan versiyonu verir → siyah arka plan.

**Doğru yöntem — her generate sonrası:**
1. Generate tamamlanınca görselin **sağ üst köşesindeki ↓ (indirme) ikonu**na tıkla
2. Transparent PNG indirilir → hedef klasöre taşı
3. Yanlışlıkla siyah arka planla kaydettiysen: Aseprite → Magic Wand (W) → Tolerance 15 → siyaha tıkla → Delete (en temizi yeniden üret)

---

## HAZIRLIK — Başlamadan Önce

### Class Anchor Dosyaları (KİLİTLENDİ — 2026-04-19)

**Klasör:** `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/`

| Class | Anchor Dosya | Silah |
|-------|-------------|-------|
| Warblade | `warblade1.png` | Greatsword (iki el) |
| Brawler | `brawler.png` | Silahsız |
| Elementalist | `elementalist.png` | Orb (element rengi Unity'de overlay — Frozen/Fire/Light) |
| Gunslinger | `gunslinger.png` | Tabanca |
| Hexer | `hexer.png` | Staff/kitap |
| Ranger | `ranger.png` | Yay |
| Ravager | `ravager.png` | Büyük silah |
| Ronin | `ronin.png` | Katana |
| Shadowblade | `shadowblade.png` | Hançer |
| Summoner | `summoner.png` | Staff/el |
| Tileset | `RIMA_Concept_CombatScene.png` | — |
| VFX | Referans yok | — |

> **Bu dosyalar init_image ANCHOR'dır.** Pixel art, şeffaf bg, 128px — seed aramaya, concept art'a gerek yok.
> Her class üretimi kendi anchor'ından başlar. Gemini concept art artık kullanılmaz.

### Sabit Parametreler (Her üretimde aynı)
```
View:      high top-down (ca 35 degrees)
Isometric: ON
Outline:   single color black outline
Shading:   detailed shading
Detail:    highly detailed
```

### Canvas Hazırlığı (Interpolate Pro öncesi — Aseprite)

```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Ne zaman:** Sadece Interpolate Pro'ya gönderilecek frame'ler için. Edit Image PRO'ya 128×128 direkt verilir.
**Neden:** Interpolate Pro edge'leri kırpar — padding olmadan uzuv kenar dışına taşar.
**Yön kuralı:** 8 yön — S → SW → W → NW → N → NE → E → SE. Her yön ayrı üretilir, flipX kullanılmaz.
**Animasyon init:** Her yön için o yönün `warblade_neutral_base_X.png`'si kullanılır.

---

## BÖLÜM 1 — WARBLADE BASE SPRİTE

### Araç Karar Logu (test edildi — kilitlendı)

| Araç | Sonuç | Karar |
|------|-------|-------|
| Create Character (8 yön) | Cüce proporsiyon, silah her yönde kayıyor | ❌ Terk |
| 8-directional sprite feature | Isometric açı yanlış (RPG Maker view) | ❌ Terk |
| Create Image M/XL, tek tek | Anchor init + zincirle tutarlı | ✅ ANA YÖNTEM |
| Seed hunting | Gereksiz — pixel art anchor zaten kilitleniyor | ❌ Terk |
| Gemini concept art init | Stil dönüşümü → variance → terk | ❌ Terk |

> **Mob için:** 8-directional sprite denenebilir — silahsız, basit silüet.

---

### Pipeline (KİLİTLENDİ — 2026-04-19)

```
AŞAMA 1: combat_idle 8 yön  → anchor init, strength 220, direction zincir
AŞAMA 2: neutral_base_S     → anchor init, strength 80, grip+neutral prompt
AŞAMA 3: neutral_base 8 yön → neutral_base_S init, strength 220, direction zincir
AŞAMA 4: silah ayrı         → izole üretim
AŞAMA 5: Aseprite composite → body + silah birleştir, bake
```

**Kurallar:**
- Anchor = pixel art, şeffaf, 128px → seed arama yok, concept art yok
- Prompt kısa: UI direction + isometric + view prompta yazılmaz, UI halleder
- PixelLab yeniden tasarım yapmaz — stil ve yön uygular

---

### Prompt Formatı (kilitlendı)

```
same character, [poz/el pozisyonu], transparent background
```

Daha fazlası yazılmaz. Referans init_image detayı taşır.

---

### AŞAMA 1 — combat_idle 8 Yön

`pixellab.ai → Create Image M veya XL`

**Warblade anchor:** `warblade1.png` — bu zaten combat_idle_S, S yönü HAZIR.

**S yönü için onay kriteri:** warblade1.png'i aç, yeterince iyi mi değerlendir.
- İyiyse: direkt `warblade_combat_idle_S.png` olarak kaydet/kullan
- Değilse: aşağıdaki ayarlarla yeni üret

Ayarlar (S hariç 7 yön için — S'yi anchor olarak kullan):
```
Direction:           [değişir]
Guidance weight:     80
Init image:          warblade_combat_idle_S.png  ← bir önceki yön
Init image strength: 220
Remove background:   ON
Seed:                0
```

**PROMPT (tüm yönler için aynı):**
```
same character, combat idle stance, transparent background
```

| # | Direction | Init | Kaydet |
|---|-----------|------|--------|
| 1 | South ✅ | warblade1.png (anchor) | `warblade_combat_idle_S.png` |
| 2 | South-West | combat_idle_S | `warblade_combat_idle_SW.png` |
| 3 | West | combat_idle_SW | `warblade_combat_idle_W.png` |
| 4 | North-West | combat_idle_W | `warblade_combat_idle_NW.png` |
| 5 | North | combat_idle_NW | `warblade_combat_idle_N.png` |
| 6 | North-East | combat_idle_N | `warblade_combat_idle_NE.png` |
| 7 | East | combat_idle_NE | `warblade_combat_idle_E.png` |
| 8 | South-East | combat_idle_E | `warblade_combat_idle_SE.png` |

**Kayıt:** `Assets/Sprites/Characters/Warblade/base/`

---

### AŞAMA 2 — neutral_base_S

```
Direction:           South (facing camera)
Guidance weight:     80
Init image:          warblade1.png (anchor)
Init image strength: 80  ← düşük: poz değişsin, kimlik kalsın
Remove background:   ON
Seed:                0
```

**PROMPT:**
```
same character, both hands gripped together low in front neutral idle stance, no sword visible, transparent background
```

→ ↓ butonu → `warblade_neutral_base_S.png`

**Onay kriteri:** Eller kılıç tutacak pozisyonda (aşağıda, birbirine yakın), gövde dik simetrik, kılıç görünmüyor.

---

### AŞAMA 3 — neutral_base 7 Ek Yön

```
Guidance weight:     80
Init image strength: 220
Remove background:   ON
Seed:                0
```

**PROMPT (tüm yönler):**
```
same character, both hands gripped together low in front neutral idle stance, no sword visible, transparent background
```

| # | Direction | Init | Kaydet |
|---|-----------|------|--------|
| 1 | South ✅ | — | `warblade_neutral_base_S.png` |
| 2 | South-West | neutral_base_S | `warblade_neutral_base_SW.png` |
| 3 | West | neutral_base_SW | `warblade_neutral_base_W.png` |
| 4 | North-West | neutral_base_W | `warblade_neutral_base_NW.png` |
| 5 | North | neutral_base_NW | `warblade_neutral_base_N.png` |
| 6 | North-East | neutral_base_N | `warblade_neutral_base_NE.png` |
| 7 | East | neutral_base_NE | `warblade_neutral_base_E.png` |
| 8 | South-East | neutral_base_E | `warblade_neutral_base_SE.png` |

---

### AŞAMA 4 — Silah Sprite (Warblade: Greatsword)

`pixellab.ai → Create Image M veya XL`

```
Isometric: ON  |  Remove BG: ON  |  Seed: 0
```

**PROMPT:**
```
massive two-handed greatsword, entire blade cold blue glowing energy veins, dark iron crossguard, isolated weapon no character, transparent background
```

→ `warblade_greatsword.png` → `Assets/Sprites/Characters/Warblade/base/`

**Onay:** Tüm blade'de mavi energy tutarlı, arka plan yok.

---

### AŞAMA 5 — Aseprite Composite → Final Bake

neutral_base 8 yön için:
1. `warblade_neutral_base_X.png` aç
2. Yeni layer → `warblade_greatsword.png` import
3. Kılıcı grip noktasına hizala (her iki el ortasında, aşağıda)
4. Pelerin overlap düzelt
5. Flatten → Export: `warblade_neutral_base_X.png` (üzerine yaz)

combat_idle 8 yön için aynı adımlar — kılıcı diagonal/yukarı konumla.

```
Assets/Sprites/Characters/Warblade/base/
  warblade_neutral_base_S/SW/W/NW/N/NE/E/SE.png   ← tüm animasyonların master frame'i
  warblade_combat_idle_S/SW/W/NW/N/NE/E/SE.png    ← attack ref + promo
  warblade_greatsword.png                           ← silah sprite (composite için)
```

> neutral_base 8 yön onaylanmadan BÖLÜM 2'ye geçme.

---

## BÖLÜM 2 — WARBLADE ANİMASYONLAR

> Her animasyon bölümünün ilk işi şudur: Aseprite'ta ilgili frame'i aç, `Sprite → Canvas Size → 160×160 → Anchor: center → OK` yap ve üzerine kaydet.
> Interpolate Pro'ya 128×128 frame gönderme. Canvas prep atlanmaz.

### Yön Tekrarı — Dosya Adı Kuralı

Her animasyon bölümünün sonunda "Yön tekrarı: S → SW → ... → SE" yazar. Bu adımları uygularken:
- UI'da **Direction** seçimini değiştir.
- Tüm input/output dosya adlarındaki `_S` suffix'ini `_<DIR>` yap.

| Yön | Suffix | Örnek (Idle breath frame) |
|-----|--------|--------------------------|
| South | `_S` | `warblade_breath_S.png` |
| South-West | `_SW` | `warblade_breath_SW.png` |
| West | `_W` | `warblade_breath_W.png` |
| North-West | `_NW` | `warblade_breath_NW.png` |
| North | `_N` | `warblade_breath_N.png` |
| North-East | `_NE` | `warblade_breath_NE.png` |
| East | `_E` | `warblade_breath_E.png` |
| South-East | `_SE` | `warblade_breath_SE.png` |

**Not:** E ve W için Aseprite'ta flipX kullanma — isometrik'te bozulur, her yön ayrı üretilir.

---

### 2A — IDLE (9 frame, 2 segment)

**Adım 0 — Canvas prep (zorunlu):**
```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
`warblade_neutral_base_S.png` için bunu yap. Idle varyasyonunda kullanılacak base frame mutlaka 160px canvas üstünde olmalı.

**Adım 1 — Breath keyframe üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_neutral_base_S.png  (160px canvas version)
Edit description: "same warrior, very subtle chest rise from a slow breath, armor micro-shift upward, slight head drop, weight settling, nearly imperceptible movement — pixel art idle breath"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_breath_S.png`

**Adım 2 — Breath frame'i canvas expand et:**
```
Aseprite → open warblade_breath_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
Base frame ve breath frame artık ikisi de 160×160 olmalı.

**Adım 3 — Interpolate Pro, pasaj A (base → breath):**

**Araç:** pixellab.ai → **Interpolate Pro** (site — extension basic 64×64 kısıtlı, 160px için site gerekli)

```
Start Image: warblade_neutral_base_S.png  (160px)
End Image:   warblade_breath_S.png  (160px)
Action description: "subtle breath in, armor rising"
Number of frames: 5
```

**Adım 4 — Interpolate Pro, pasaj B (breath → base):**

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Start Image: warblade_breath_S.png  (160px)
End Image:   warblade_neutral_base_S.png  (160px)
Action description: "exhale, armor settling back to rest"
Number of frames: 5
```

**Adım 5 — Aseprite — stitch ve dedup:**
- Pasaj A frame'lerini import et.
- Hemen arkasına pasaj B frame'lerini import et.
- Duplicate boundary frame'i sil: pasaj B'nin ilk frame'i = pasaj A'nın son frame'i (`warblade_breath_S.png`).
- Sadece bir kopya bırak.
- Final sayı: `(5 + 5) - 1 = 9 frame`.
- Böylece loop dikişsiz olur; same-image loop yapılmaz.

Yön tekrarı: S → SW → W → NW → N → NE → E → SE (8 yön, aynı adımlar, sadece Direction değişir)

---

### 2B — RUN LOOP (11 frame, 2 segment)

**Adım 0 — Canvas prep (zorunlu):**
```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
`warblade_neutral_base_S.png` ile başla. Run key pose üretiminde kullanılacak her input/output frame 160×160 canvas'a alınır.

**Adım 1 — run_A keyframe üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_neutral_base_S.png  (160px canvas)
Edit description: "warrior mid-stride, left leg fully forward, right arm swinging forward, heavy armor leaning into movement, weight on front foot, Fractured Epic pixel art, Hades art style"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_runA_S.png`

**Adım 2 — run_A frame'ini canvas expand et:**
```
Aseprite → open warblade_runA_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Adım 3 — run_B keyframe üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_runA_S.png  (160px)
Edit description: "same warrior, opposite stride: right leg now fully forward, left arm swinging forward, mirrored gait from previous pose, same armor, same momentum direction, Fractured Epic pixel art, Hades art style"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_runB_S.png`

**Adım 4 — run_B frame'ini canvas expand et:**
```
Aseprite → open warblade_runB_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Adım 5 — Interpolate Pro, pasaj A (run_A → run_B):**

**Araç:** pixellab.ai → **Interpolate Pro** (site — extension basic 64×64 kısıtlı, 160px için site gerekli)

```
Start Image: warblade_runA_S.png
End Image:   warblade_runB_S.png
Action description: "heavy armored warrior mid-sprint, transitioning stride"
Number of frames: 6
```

**Adım 6 — Interpolate Pro, pasaj B (run_B → run_A):**

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Start Image: warblade_runB_S.png
End Image:   warblade_runA_S.png
Action description: "heavy armored warrior mid-sprint, returning stride"
Number of frames: 6
```

**Adım 7 — Aseprite — stitch ve dedup:**
- Pasaj A frame'lerini import et.
- Ardından pasaj B frame'lerini import et.
- Duplicate boundary frame'i sil: pasaj A sonundaki `run_B` = pasaj B başındaki `run_B`.
- Sadece bir kopya bırak.
- Final sayı: `(6 + 6) - 1 = 11 frame`.
- Run loop böyle kurulur; aynı görseli başa ve sona verip titreşimli sahte loop üretme.

Yön tekrarı: S → SW → W → NW → N → NE → E → SE (8 yön, aynı adımlar, sadece Direction değişir)

---

### 2B.5 — STOP / RUN→IDLE GEÇİŞ (3 frame, 1 segment)

> Hades modeli: hareket durunca kısa decel → idle. Walk animasyonu YOK.

**Adım 0 — Canvas prep (zorunlu):**
```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
`warblade_runA_S.png` ve `warblade_neutral_base_S.png` için bunu teyit et. Stop geçişinde yeni keyframe üretimi yok.

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Start Image: warblade_runA_S.png
End Image:   warblade_neutral_base_S.png
Action description: "warrior decelerates from sprint, weight settling, armor momentum settling, planting feet into ready stance"
Number of frames: 3
```

→ Kısa, snappy geçiş. 3 frame yeterli. Stop için ek keyframe üretme.

Yön tekrarı: S → SW → W → NW → N → NE → E → SE (8 yön, aynı adımlar, sadece Direction değişir)

---

### 2C — DEATH (12 frame, 2 segment)

**Adım 0 — Canvas prep (zorunlu):**
```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
Önce `warblade_neutral_base_S.png` frame'ini 160×160 olarak teyit et. Edit PRO'dan çıkacak `stagger` ve `dead` frame'lerini de interpolate öncesi aynı canvas'a alacaksın.

**Segment 1 — Death keyframe (yerde yatan) üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_neutral_base_S.png
Edit description: "warrior collapses forward, falls flat face-down on ground, greatsword dropped to side, armor dented, lifeless body, isometric perspective, hades game art style, Fractured Epic death pose, 128x128"
Output size: 128×128
Remove background: ON
```
→ En iyi yerde yatan frame'i kaydet: `warblade_dead_S.png`

**Segment 2 — Stagger keyframe üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_neutral_base_S.png
Edit description: "warrior staggers backward from fatal blow, knees buckling, head dropping, greatsword slipping from grip, isometric perspective, hades game art style, Fractured Epic death stagger"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_stagger_S.png`

**Segment 3 — Edit PRO çıktıları için canvas expand:**
```
Aseprite → open warblade_stagger_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
Aseprite → open warblade_dead_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Animasyon üret — 2 pasaj:**

**Araç:** pixellab.ai → **Interpolate Pro** (site — extension basic 64×64 kısıtlı, 160px için site gerekli)

```
Pasaj 1:
Start: warblade_neutral_base_S.png  (ayakta, 160px)
End: warblade_stagger_S.png  (160px)
Frames: 5
Action: "warrior receives fatal blow, staggers backward"

Pasaj 2:
Start: warblade_stagger_S.png  (160px)
End: warblade_dead_S.png  (160px)
Frames: 8
Action: "warrior collapses to ground, final fall"
```

**Aseprite — stitch ve dedup:**
- Pasaj 1'i import et.
- Sonra pasaj 2'yi import et.
- Duplicate boundary frame'i sil: pasaj 1 sonundaki `warblade_stagger_S.png` = pasaj 2 başındaki `warblade_stagger_S.png`.
- Sadece bir kopya bırak.
- Final sayı: `(5 + 8) - 1 = 12 frame`.

Yön tekrarı: S → SW → W → NW → N → NE → E → SE (8 yön, aynı adımlar, sadece Direction değişir)

---

### 2D — ATTACK (12 frame, 3 segment — ZİNCİRLEME)

> ZİNCİRLEME KURAL: Her Edit Image PRO adımı bir önceki adımın output'unu input olarak alır.
> Windup üretildikten sonra impact üretimi onun üstünden yapılır. Bu zincir bozulursa poz tutarlılığı bozulur.

**Adım 0 — Canvas prep (zorunlu):**
```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
Önce `warblade_neutral_base_S.png` frame'ini 160×160 olarak teyit et. Windup ve impact çıktıları da interpolate öncesi aynı canvas'a alınır.

**Adım 1 — Windup keyframe üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_neutral_base_S.png  (160px)
Edit description: "warrior winds up for a powerful overhead greatsword slam, sword raised high above head with both hands, body coiling, weight shifting to back foot, anticipation pose, isometric perspective, hades game art style, Fractured Epic"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_windup_S.png`

**Adım 2 — Windup frame'ini canvas expand et:**
```
Aseprite → open warblade_windup_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Adım 3 — Impact keyframe üret (chained input):**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_windup_S.png  ← önceki adımın çıktısı, zincir bozulmaz
Edit description: "warrior slams greatsword downward at full force, sword at lowest point impact, arms fully extended downward, body bent forward, weight on front foot, explosive impact moment, cold blue rift energy bursting from blade, isometric perspective, hades game art style"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_impact_S.png`

**Adım 4 — Impact frame'ini canvas expand et:**
```
Aseprite → open warblade_impact_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Adım 5 — Interpolate Pro, pasaj A (base → windup):**

**Araç:** pixellab.ai → **Interpolate Pro** (site) — her pasaj ayrı ayrı üretilir

```
Pasaj 1: base → windup
Start: warblade_neutral_base_S.png
End: warblade_windup_S.png
Frames: 4
Action: "warrior raises greatsword overhead, windup"
```

**Adım 6 — Interpolate Pro, pasaj B (windup → impact):**

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Pasaj 2: windup → impact
Start: warblade_windup_S.png
End: warblade_impact_S.png
Frames: 6
Action: "warrior slams greatsword down with full force, explosive strike"
```

**Adım 7 — Interpolate Pro, pasaj C (impact → base):**

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Pasaj 3: impact → base
Start: warblade_impact_S.png
End: warblade_neutral_base_S.png
Frames: 4
Action: "warrior recovers to ready stance after attack"
```

**Adım 8 — Aseprite — stitch ve dedup:**
- Önce pasaj A'yı import et.
- Sonra pasaj B'yi import et.
- Sonra pasaj C'yi import et.
- A/B sınırında duplicate `windup` frame'ini sil.
- B/C sınırında duplicate `impact` frame'ini sil.
- Toplam sınır sayısı 2 olduğu için final sayı: `(4 + 6 + 4) - 2 = 12 frame`.
- Boundary frame'lerinin son kopyasını değil, tek kopyasını bırak.

Yön tekrarı: S → SW → W → NW → N → NE → E → SE (8 yön, aynı adımlar, sadece Direction değişir)

---

### 2E — DASH (8 frame, 3 segment — ZİNCİRLEME)

> ZİNCİRLEME KURAL: dash_launch üretildikten sonra dash_peak onun üzerinden üretilir.
> Base'ten direkt peak'e zıplama yapma; launch pozu dash'in okunurluğunu sağlar.

**Adım 0 — Canvas prep (zorunlu):**
```
Aseprite → dosyayı aç → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```
Önce `warblade_neutral_base_S.png` frame'ini 160×160 olarak teyit et. Launch ve peak frame'leri de interpolate öncesi aynı canvas'a alınır.

**Adım 1 — Dash launch keyframe üret:**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_neutral_base_S.png  (160px)
Edit description: "warrior explosively pushing off into a dash, torso dropping forward, rear leg driving off the ground, greatsword beginning to trail behind, launch pose, heavy armor surging into motion, Fractured Epic charge pose, isometric perspective, hades game art style"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_dash_launch_S.png`

**Adım 2 — Dash launch frame'ini canvas expand et:**
```
Aseprite → open warblade_dash_launch_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Adım 3 — Dash peak keyframe üret (chained input):**

**Araç:** pixellab.ai → Edit Image PRO

```
Image to edit: warblade_dash_launch_S.png  ← önceki adımın çıktısı, zincir bozulmaz
Edit description: "warrior in maximum forward dash lean, body almost horizontal, greatsword trailing behind for momentum, legs fully extended, heavy armor surging forward, Fractured Epic charge pose, isometric perspective, hades game art style"
Output size: 128×128
Remove background: ON
```
→ Kaydet: `warblade_dash_peak_S.png`

**Adım 4 — Dash peak frame'ini canvas expand et:**
```
Aseprite → open warblade_dash_peak_S.png → Sprite → Canvas Size → 160×160 → Anchor: center → OK → kaydet üzerine yaz
```

**Adım 5 — Interpolate Pro, pasaj A (base → dash_launch):**

**Araç:** pixellab.ai → **Interpolate Pro** (site) — her pasaj ayrı ayrı üretilir

```
Pasaj 1: base → dash_launch
Start: warblade_neutral_base_S.png
End: warblade_dash_launch_S.png
Frames: 3
Action: "warrior launches into forward dash, explosive acceleration"
```

**Adım 6 — Interpolate Pro, pasaj B (dash_launch → dash_peak):**

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Pasaj 2: dash_launch → dash_peak
Start: warblade_dash_launch_S.png
End: warblade_dash_peak_S.png
Frames: 4
Action: "warrior commits fully into the dash, reaching maximum forward lean"
```

**Adım 7 — Interpolate Pro, pasaj C (dash_peak → base):**

**Araç:** pixellab.ai → **Interpolate Pro** (site)

```
Pasaj 3: dash_peak → base
Start: warblade_dash_peak_S.png
End: warblade_neutral_base_S.png
Frames: 3
Action: "warrior decelerates, plants feet, returns to ready stance"
```

**Adım 8 — Aseprite — stitch ve dedup:**
- Önce pasaj A'yı import et.
- Sonra pasaj B'yi import et.
- Sonra pasaj C'yi import et.
- A/B sınırında duplicate `dash_launch` frame'ini sil.
- B/C sınırında duplicate `dash_peak` frame'ini sil.
- Toplam sınır sayısı 2 olduğu için final sayı: `(3 + 4 + 3) - 2 = 8 frame`.

Yön tekrarı: S → SW → W → NW → N → NE → E → SE (8 yön, aynı adımlar, sadece Direction değişir)

---

### Warblade Animasyon Özeti

> **Not:** Walk animasyonu YOK — Hades modeli. Hareket = Run. Stop = kısa geçiş.
> `warblade_neutral_base_<DIR>.png` = Idle'ın first+last frame'i = tüm animasyon keyframe üretiminin kaynağı. Her yönde o yönün base'i kullanılır.

| Animasyon | Frame | Segment | Araç |
|-----------|-------|---------|------|
| Idle | 9 | 2 | Edit PRO (breath) → Interpolate Pro ×2 → dedup |
| Run | 11 | 2 | Edit PRO ×2 (runA+runB) → Interpolate Pro ×2 → dedup |
| Stop | 3 | 1 | Interpolate Pro (runA→base) |
| Attack | 12 | 3 | Edit PRO ×2 (windup+impact, chained) → Interpolate Pro ×3 → dedup ×2 |
| Dash | 8 | 3 | Edit PRO ×2 (launch+peak, chained) → Interpolate Pro ×3 → dedup ×2 |
| Death | 12 | 2 | Edit PRO ×2 (stagger+dead) → Interpolate Pro ×2 → dedup |

---

## BÖLÜM 3 — CRİMSON CRYPT TİLESET

### Araç: pixellab.ai → Create tiles PRO

**Sabit ayarlar:**
```
Tile type:    Square top-down
Tile view:    high top-down
Isometric:    ON
Tile size:    32 (prop) / 16 (floor — site'de varsa)
Outline mode: Segmentation
Shading:      Detailed shading
Style Image:  RIMA_Concept_CombatScene.png  ← tam yol: F:/Antigravity Projeler/2d roguelite/TASARIM/RIMA_Concept_CombatScene.png
```

---

### 3A — FLOOR TİLELARI (önce bunları yap — seed'i kaydet!)

**KISA:**
```
cold dark stone dungeon floor, hairline blue rift cracks
```

**DETAYLI:**
```
cold dark stone dungeon floor tiles, hairline cold blue rift energy cracks running through stone, worn and ancient, Fractured Epic dungeon, Crimson Crypt biome, muted dark grey-brown tones with cold blue accent, isometric perspective, hades game art style
```

**⚠️ Seed Kaydet:** Üretilince sağ üstteki seed numarasını not al.
Tutarlı floor varyasyonları için aynı seed'i kullan.

**Hedef:** 3-4 varyasyon (farklı çatlak pattern) — tek sıkışık görünüm yerine organik zemin.

---

### 3B — DUVAR TİLELARI (kritik — ön yüz görünmeli)

**KISA:**
```
stone dungeon wall, visible front face, Fractured Epic, torch sconce
```

**DETAYLI:**
```
dungeon stone wall with clearly visible front-facing side surface, 3D depth effect, wall has height — top edge and front face both visible, dark stone with blood stain traces, torch sconce mounted on wall, shadow at base of wall, Fractured Epic Crimson Crypt dungeon, cold blue rift glow from wall cracks, muted dark tones, classic ARPG 3D perspective depth
```

**KRİTİK KURAL:** "Flat top" duvar YASAK. Ön yüz göründüğünde oyunun depth hissi oluşur.
Üretilen tile'da duvarın ön yüzü yoksa → tekrar üret.

---

### 3C — PROP TİLELARI (sırayla üret)

Her prop için:
- **Araç:** pixellab.ai → Create tiles PRO (veya Generate Image Pixflux — eğer tek obje)
- **Remove background: ON**
- **Boyut: 32×32**

**Prop 1 — Kemik Yığını:**
```
KISA: pile of bones, dungeon floor, Fractured Epic
DETAYLI: scattered pile of bones and skulls on dungeon floor, dark stone surface, Crimson Crypt biome, muted pale bone color, Fractured Epic pixel art, top-down view, 32x32, transparent background
```

**Prop 2 — Kırık Sütun:**
```
KISA: broken stone column, dungeon, crumbled top
DETAYLI: crumbled broken stone pillar/column, top shattered, dark stone surface, cold blue rift crack running through stone, Crimson Crypt dungeon, Fractured Epic pixel art, top-down view, 32x32, transparent background
```

**Prop 3 — Kan Lekesi (zemin):**
```
KISA: dried blood stain, stone floor, dark
DETAYLI: dried dark crimson blood stain on cold stone floor, irregular shape, Crimson Crypt biome, Fractured Epic dungeon, pixel art, top-down view, 32x32, transparent background
```

**Prop 4 — Meşale:**
```
KISA: lit wall torch, dungeon, warm orange flame
DETAYLI: mounted dungeon wall torch burning with warm orange-yellow flame, iron bracket, contrast against cold dark stone wall, Crimson Crypt biome, Fractured Epic pixel art, side view, 32x32, transparent background
```

---

### Tileset Dosya Yapısı

```
Assets/Sprites/Tiles/CrimsonCrypt/
  floor_01.png
  floor_02.png
  floor_03.png
  wall_N.png      ← kuzey duvar
  wall_S.png      ← güney duvar (ön yüz bu)
  wall_E.png
  wall_W.png
  prop_bones.png
  prop_column.png
  prop_bloodstain.png
  prop_torch.png
```

---

## BÖLÜM 4 — VFX SPRİTELARI

### Araç: pixellab.ai → Generate Image (Pixflux) — SYNC, anında PNG

**Sabit:**
```
Remove background: ON
Transparent background zorunlu
```

---

### 4A — SLASH ARC (Warblade saldırı)

```
Boyut: 128×128
PROMPT:
pixel art sword slash arc effect, crescent shaped white slash trail with cold blue glowing energy outline, sharp streak, transparent background, Fractured Epic game VFX sprite, no character, no background, isolated effect only, 128x128
```

**Negative prompt:**
```
character, body, background, color fill, noise, blur, gradient sky
```

**Kaç frame:** 1 peak frame yeterli. Unity'de scale + fade animasyonu yapar.
**Kaydet:** `Assets/Sprites/VFX/slash_arc.png`

---

### 4B — HIT SPARK (düşmana vuruş anı)

```
Boyut: 64×64
PROMPT:
pixel art hit impact burst, sharp white and cold blue sparks radiating outward, small impact explosion effect, 5-6 sharp spike particles, transparent background, Fractured Epic melee hit VFX, no character, no background, 64x64
```

**2 frame üret:**
- Frame 1: tam patlama (tüm spark'lar açılmış)
- Frame 2: aynı prompt ile daha soluk/dağılmış varyasyon → dissipate frame

**Kaydet:**
```
Assets/Sprites/VFX/hit_spark_01.png
Assets/Sprites/VFX/hit_spark_02.png
```

---

### 4C — DEATH BURST (düşman ölümü)

```
Boyut: 96×96
PROMPT:
pixel art death explosion burst, dark purple void energy erupting outward, particle fragments dissolving into void, circular burst shape, transparent background, Fractured Epic enemy death VFX, no character, no background, 96x96, deep purple #8C33E6 core
```

**2 frame üret:**
- Frame 1: tam patlama
- Frame 2: dağılma (aynı prompt, biraz daha soluk)

**Kaydet:**
```
Assets/Sprites/VFX/death_burst_01.png
Assets/Sprites/VFX/death_burst_02.png
```

---

## ÜRETIM SIRASI (Önerilen)

```
1. Warblade base sprite (8 yön ayrı üretim) → önce bunlar olmadan anim yok
2. Tileset floor + wall → unity'de çalışmak için
3. Warblade idle + run → en kolay animasyonlar, pipeline'ı test et
4. VFX sprites → hızlı üretim, Pixflux sync
5. Warblade death → orta zorluk
6. Warblade attack → en karmaşık, en sona bırak
7. Warblade dash → attack'tan sonra kolay
8. Tileset props → en sona, zorunlu değil
```

---

## SORUN GİDERME

| Sorun | Çözüm |
|-------|-------|
| Karakter bodur görünüyor | Prompt'a "heroic proportions, tall warrior" ekle |
| Silah bir frame'de kayboluyor | Edit Image PRO ile o frame'i inpaint et |
| Yön tutarsız | Her yönü ayrı üret, Direction seçimini değiştir; flipX kullanma — isometrik'te derinlik bozulur |
| Floor tile'da grid izi var | Farklı seed dene + prompt'a "seamless, no grid lines" ekle |
| Duvar ön yüzsüz geldi | Prompt'a "isometric wall with visible height and front face, not flat" ekle |
| Animasyon drift (karakter kayıyor) | ai_freedom'u 350'ye düşür |
| Enhanced with AI RIMA detaylarını atladı | Manuel olarak DETAYLI prompt'tan eksik kısımları ekle |
