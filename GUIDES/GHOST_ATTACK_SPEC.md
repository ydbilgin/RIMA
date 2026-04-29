# RIMA — Ghost Attack Animasyon Spec
*Karar tarihi: 2026-04-17 | Tüm kararlar kilitlendi*

---

## KARAR: OPSİYON C — KİLİTLENDİ

Ghost attack **iki trigger noktasında** aktive olur:
1. **Cross-class skill havuzu** (80 skill, 2 slot/run) → skill aktive edilince / pasif proc edilince
2. **Z/X secondary skilleri** → kullanıldığında

Her ikisi de "farklı classtan güç ödünç alma" anı. Ghost görünmesi bunu görsel olarak kilitleyen şey.

**Pasif cross-class skillerde:** Her hit değil, PROC anında (CrossClassSkillManager zaten bu şekilde çalışıyor).

---

## GHOST ATTACK ANİMASYON SPEC (TÜM CLASSLARA UYGULANIR)

| Parametre | Değer |
|-----------|-------|
| Frame sayısı | **12f** |
| Segment sayısı | **2** (6f windup→peak + 6f peak→settle) |
| Yön | **4 yön** (S / N / W / E) |
| Canvas | 128×128 (player ile aynı) |
| Araç | Edit PRO (key frames) → Interpolate Pro site (2 pasaj) |
| Unity tint | CrossClassGhostEffect.cs — class rengi MaterialPropertyBlock ile (sprite nötr üretilir) |
| Süre (oyunda) | 0.6s (12fps = tam animasyon) |
| Blend | Additive, alpha 0.5→0 (fade out) |

**Gen plan:**
- Her class: 2 key frame gen × 4 yön = 8 gens base + ~16 interp gens = **~24g/class**
- 10 class × 24g = **~240g total** (5000g bütçeden %4.8)

**Not:** Ghost spritelarda tint YOK — nötr/renkli karakter üretilir. Unity runtime'da `class rengi` MaterialPropertyBlock ile uygulanır. Bu 1 sprite seti = tüm tint renkleri demek.

---

## KEY POSE PLANI (2 Segment)

```
Segment 1 (6f): base pose → peak (strike anı)
  Start: base idle pose
  End:   [class signature strike pose]
  Action: anticipation + strike

Segment 2 (6f): peak → settle
  Start:  [class signature strike pose]
  End:    base pose (veya slight settle)
  Action: recovery + settle
```

---

## CLASS BAŞINA GHOST ATTACK SPEC

### 1. WARBLADE (Renk: #66AAFF soğuk mavi)
**Kimlik:** Geniş yatay kılıç sweepı — önünü açar, ezip geçer

**Strike keyframe prompt (Edit PRO):**
```
battle-worn warrior, powerful one-arm horizontal sword sweep,
greatsword at full extension pointing left, blade at waist height,
body weight fully transferred to front foot, shoulder driving through swing,
armor momentum following through, decisive melee strike,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"warrior winds up for horizontal sword sweep, sword pulled back right, then sweeps blade in wide arc to left side, full weight transfer"`
- Segment 2: `"warrior recovers from sword sweep, blade returning to guard position, weight settling back"`

---

### 2. ELEMENTALİST (Renk: #FF6600 ateş turuncusu)
**Kimlik:** Çift el thrust — her iki avuç açılır, enerji patlar

**Strike keyframe prompt:**
```
elementalist mage, both arms thrust forward simultaneously, palms open outward,
arms fully extended at shoulder height, upper body leaning into cast,
explosive release pose, concentrated discharge gesture,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"mage draws both hands to chest center, gathering energy, then thrusts both arms fully forward in explosive cast"`
- Segment 2: `"mage retracts arms from extended cast, upper body straightens, returns to ready stance"`

---

### 3. SHADOWBLADE (Renk: #9933CC void mor)
**Kimlik:** Çapraz X-kesiş — her iki bıçak içe doğru çizgi çizer

**Strike keyframe prompt:**
```
shadowblade rogue, arms fully crossed at chest center in X-slash position,
both blades pointing outward-down after crossing, body low crouch,
weight evenly distributed, dual-blade cross-cut completion pose,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"rogue opens both arms wide outward, then rapidly crosses them inward in X-slash motion, blades cutting toward center"`
- Segment 2: `"rogue uncrosses arms from X position, rises slightly from crouch, settles back to ready stance"`

---

### 4. RANGER (Renk: #44CC44 doğa yeşili)
**Kimlik:** Hızlı yay çekip bırakma — serbest bırakma anı

**Strike keyframe prompt:**
```
ranger archer, bow arm fully extended forward, string arm snapped back to jaw release,
arrow released moment, upper body rotation into shot, forward lean,
weight on front foot at release, focused aim pose,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"ranger raises bow, draws string back to cheek, full draw position with shoulder rotation into aim"`
- Segment 2: `"ranger releases bowstring, string arm snaps forward, bow arm steady, recoil absorbed, returns to low guard"`

---

### 5. RAVAGER (Renk: #FF3322 kan kırmızısı)
**Kimlik:** Yukarıdan aşağı balta slam — tüm vücut aşağı iner

**Strike keyframe prompt:**
```
ravager berserker, massive two-handed axe buried at ground level, both arms fully down,
knees bent deep at impact, torso bent forward, axe head at lowest point of slam,
explosive downward strike completion, full body weight committed downward,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"berserker lifts massive axe overhead with both hands, full extension above head, gathering momentum for downswing"`
- Segment 2: `"berserker drives axe down with full force into ground, knees absorbing impact, pulls axe back up through recovery"`

---

### 6. RONİN (Renk: #FFFFFF saf beyaz)
**Kimlik:** Iaido kın çekişi — tek yatay kesik, anlık

**Strike keyframe prompt:**
```
ronin samurai, iaido draw-cut completed, sword fully extended horizontal at waist level,
sword just cleared the scabbard, body in deep side stance, weight on front foot,
blade edge forward, cut completed in single motion, focused precision pose,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"ronin hand grips sword hilt, begins lightning-fast iaido draw, body rotating into side stance as blade clears scabbard"`
- Segment 2: `"ronin holds extended cut position briefly, then re-sheathes sword in smooth controlled motion, returning to neutral"`

---

### 7. GUNSLİNGER (Renk: #FFB800 altın sarısı)
**Kimlik:** Hızlı draw ve atış — kol omuz hizasında uzanır, tetikte

**Strike keyframe prompt:**
```
gunslinger, arm fully extended forward at shoulder height, pistol aimed and fired,
slight backward lean from recoil, non-gun hand at side, one-eye aim pose,
confident quick-draw shooter stance, gun smoke implied at barrel,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"gunslinger reaches for holstered pistol, quick-draws upward, raises arm to shoulder level in aim position"`
- Segment 2: `"gunslinger fires, absorbs recoil lean, lowers arm smoothly, returns to walking stance"`

---

### 8. BRAWLER (Renk: #FF8800 yanık turuncu)
**Kimlik:** Yükselen uppercut — yerin gücünü yumruğa taşır

**Strike keyframe prompt:**
```
brawler fighter, rising uppercut at peak, fist at full overhead extension,
legs pushing up from ground, shoulder rotating upward into punch,
body fully committed upward and forward into strike,
other arm pulled back as counterweight, explosive upward power,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"brawler bends knees low, coils body, winds up fist downward then explodes upward in rising uppercut"`
- Segment 2: `"brawler reaches peak of uppercut, weight overextended upward, drops back to fighting stance with guard up"`

---

### 9. SUMMONER (Renk: #22FF88 ruh yeşili)
**Kimlik:** Aşağı-yukarı çağırma jesti — yerden ruhu çeker

**Strike keyframe prompt:**
```
summoner necromancer, right arm at full overhead extension, palm facing upward,
left hand at side, summoning gesture completed, upper body slightly arched back,
energy trailing upward from raised palm, summoning pull gesture apex,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"summoner sweeps arm downward to ground level, palm facing down in gathering gesture, then pulls upward in summoning arc"`
- Segment 2: `"summoner holds overhead summoning position, arm slowly lowers back to chest, energy fades, returns to neutral"`

---

### 10. HEXER (Renk: #CCFF00 lanet sarısı)
**Kimlik:** Parmak lanet jesti — işaret parmağı lanet fırlatır

**Strike keyframe prompt:**
```
hexer warlock, right arm fully extended forward at eye level, index finger pointing outward,
other hand drawn back to hip, body leaning slightly forward into curse,
malevolent pointing pose, head slightly tilted with focused malice,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
128x128, transparent background, dark fantasy, hades game art style
```

**Animasyon action:**
- Segment 1: `"hexer raises pointing hand from hip to eye level, gathering crackling curse energy at fingertip, aims deliberately"`
- Segment 2: `"hexer fires curse from fingertip, arm extends to full point, slight forward lurch, then pulls arm back to rest"`

---

## GEN HESABI ÖZETİ

| Faz | Classlar | Ghost gen |
|-----|---------|-----------|
| Faz 1 | Warblade | ~24g |
| Faz 2 | +Elementalist, Shadowblade, Ranger | +72g |
| Faz 3 | +Ravager, Ronin, Gunslinger, Brawler | +96g |
| Faz 4 | +Summoner, Hexer | +48g |
| **TOPLAM** | 10 class | **~240g** |

5000g bütçede: **%4.8** — tüm 10 class için ghost attacks.

---

## ÜRETİM SIRASI (PER CLASS)

```
1. Warblade idle SE frame → CrossClassGhost.prefab referans sprite güncelle
2. Her class için:
   a. Edit PRO ile strike keyframe üret (1g × 4 yön = 4g)
   b. Interpolate Pro (site): base→peak 6f (4 yön × 1 call = 4g)
   c. Interpolate Pro (site): peak→settle 6f (4 yön × 1 call = 4g)
   d. Aseprite'ta birleştir → 12f animation
   e. Unity'de Animator Controller'a "GhostAttack" state ekle
```
