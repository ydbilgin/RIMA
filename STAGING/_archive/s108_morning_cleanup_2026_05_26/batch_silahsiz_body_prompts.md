# 10 Class Weaponless Body — Batch Prompt Pack (S69, Yol A — NLM CANON)

**Workflow:** PixelLab Web App Create Image Pro (Karar #108) → 8-direction character → animations
**Style Reference:** `Characters/anchors/reference.png` veya silah-temiz Warblade frame
**View:** High top-down ~35°, chibi 64×64, PPU 64
**Karar canon:** Karar #80 (Silhouette Bible) + #99 (weapon visibility) + #59 (VFX vs sprite) + #109 (ambient idle) + #42 (no walk, only run)

## Universal Rules (S69 REVIZE — Neutral Idle LOCKED)

- **Base rotation = NEUTRAL idle pose** — sadece yön referansı; class-specific stance idle ANİMASYON clip'lerinde override edilir (Karar #109 ambient idle)
- **Reason:** Yol A'da weapon-attach per-frame anchor üzerinden gider; base pose'un grip-shape olması gerekmez, hand position her anim clip'inde tanımlanır
- **Universal neutral pose:** athletic standing, knees softly bent, body slight forward lean (top-down ~35°), arms relaxed at sides with hands open (no grip, no weapon, no fist)
- **Embedded glow YASAK** (Karar #59) — sadece body + accent color
- **VFX YASAK promptta** — particle/glow/light Unity'de
- **Brawler ve Elementalist promptlarında bile** silah/disc YOK — hepsi neutral
- **Class identity** = sadece silhouette/clothing/saç/accent renk (silah yok)

## 10 Class — Silahsız Body Prompts (PixelLab Create Image Pro)

### 1) Warblade
```
64x64 chibi top-down male heavy warrior, low guard stance — broad-shouldered V-shape posture, body leaned forward, head slightly down. Two-hand grip position at chest level, both hands curled as if gripping a horizontal greatsword shaft but no weapon visible — empty hand grip. Dark leather armor #4F3A2C with brass buckle accents #C09455, cold blue strap details. View 35 degree high top-down ARPG angle, pixel art PPU 64, 8 directions.
```

### 2) Ranger
```
64x64 chibi top-down female tactical rift hunter, asymmetric battle-worn armor, off-white bleached-ivory hair in low loose ponytail with escaping temple strands, faint cheek scar. Tactical hunter stance — left hand at thigh-level in bow-rest grip position (curled fingers gripping empty space where bow would be), right hand free at side. Eyes forward, alert. Dark forest green #2A3520 armor with heavier right pauldron and left forearm leather wrap, cold blue #7BA7BC hood/strap accents. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 3) Shadowblade
```
64x64 chibi top-down male shadowblade, slim upright vertical posture, near-black deep purple armor #1A1025 with void purple accents #5A2A8A. Both hands curled in reverse-grip position close to body (elbows back, hands held close to torso as if holding twin blades reversed — but no blades visible), empty reverse-grip. Hooded, lean silhouette. NO embedded glow anywhere. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 4) Elementalist (SILAHSIZ — disc decouple)
```
64x64 chibi top-down female elementalist mage, open palm casting stance — left palm faced upward at chest level (empty palm, no disc visible in this base), right hand at side with relaxed casting gesture. Dark indigo-purple cropped robe with bare midriff, honey-blonde warm hair in soft waves, academic controlled posture. NO weapon, NO disc, NO glow in this base sprite. Just the body in casting-ready pose. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 5) Ravager
```
64x64 chibi top-down male berserker, forward-leaning aggressive posture, weight on front foot, ready to charge. Both hands curled at hip level in dual-hatchet grip position (fingers curled as if holding short axe handles, but no axes visible). Muscular build, dark rough armor with crimson red #D43F3F accents, bare arms with scars. NO weapons. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 6) Ronin
```
64x64 chibi top-down male ronin samurai, iaido draw stance — body sideways angled, right hand at right hip in sword-draw position (curled fingers ready to grip a sword handle, but no katana visible), left hand on the empty scabbard/sheath at left hip (the scabbard sheath sprite IS visible, but no blade inside). Dark navy/black kimono, pale gold #C8A87A accents, focused calm expression. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 7) Gunslinger
```
64x64 chibi top-down female gunslinger duellist, kinetic dueller stance — slightly crouched, weight shifted to one leg, both hands at hip level in pistol-grip position (fingers curled as if holding twin pistols, but no guns visible — empty dual-pistol grip). Long dark grey-purple trenchcoat #4A3A4A, deep auburn red hair tied back, alert eyes. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 8) Brawler (SILAH ZATEN YOK)
```
64x64 chibi top-down male brawler, boxing/guard stance — fists clenched at chin level, knees bent, body slightly angled, ready to strike. Bare muscular torso with dark leather hand wraps and forearm bandages, orange #FF8C00 accent on belt and wraps. NO weapon anywhere — bare fists. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 9) Summoner
```
64x64 chibi top-down female necromancer-summoner, orchestra conductor pose — left hand at chest level in lantern-grip position (fingers curled around an empty lantern handle that is not visible), right hand raised in directing/orchestrating gesture, palm open and fingers spread. Dark indigo robes with cyan trim accents, violet detail patterns, hooded with strands of pale hair visible. Calm controlling expression. NO lantern, NO glow, NO summoned minions in this base. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### 10) Hexer
```
64x64 chibi top-down female witch-hexer, curse-bearer stance — left arm holds an empty grimoire-holding position at chest (arm bent across chest as if cradling a thick book, but no book visible), right hand at side curled in staff-grip position (fingers around an empty vertical pole). Very dark purple-black robes with dark red #8B0000 accent trim, long unkempt dark hair, sickly pale skin, patient malicious expression. NO weapon, NO grimoire, NO flame in this base — empty hands in carry position. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

## Decouple Sprite List (ayrı gen)

| Sprite | Owner class | Notlar |
|---|---|---|
| Greatsword | Warblade | Two-hand horizontal carry |
| Compound bow | Ranger | Left-hand at-rest grip |
| Twin blades (pair) | Shadowblade | Reverse-grip her iki el |
| Dual hatchets (pair) | Ravager | Her iki el ayrı sprite |
| Katana (blade only) | Ronin | Sheath body sprite içinde |
| Dual pistols (pair) | Gunslinger | Her iki el ayrı sprite |
| Grimoire (book) | Hexer | Göğüste tutulan |
| Curse staff | Hexer | Sağ el (yeşil flame VFX'le) |
| Soul lantern | Summoner | Sol el |
| Rune disc | Elementalist | Sol palm üstünde (VFX glow ayrı) |

**9 ayrı decouple sprite** (bazı class'lar çoklu).

## Animation Pipeline (Karar #42 + Karar #14)

**Idle:** Template-first (`breathing-idle` 8f) — ambient özelleştirme polish phase

**Run:** Brian's Extreme Pose workflow
1. 2 ekstrem poz üret per direction (right-step apex + left-step apex)
2. PixelLab Interpolate → 6 in-between frames
3. Output: 8 frame run cycle per direction

**Attack (heavy/multi-frame):** Karar #14 3-Segment Workflow
- Windup (4-6 frame)
- Peak frame (1 frame, paylaşılan)
- Follow-through (4-6 frame)
- Birleştirilince 10-13 frame

## Cost Estimate (per class)

- Body silahsız regen (8-dir): ~3-5 gen
- Idle (template): 1 gen
- Run (2 extreme × 8 dir + interpolate): ~6-8 gen
- Weapon sprite: ~1 gen
- **Total per class: ~12-15 gen**

**Full roster (10 class):** ~120-150 gen
**MVP (Warblade only):** ~12-15 gen

## YASAK (drift sebep)

- ❌ Universal ready stance (Karar #80 ihlali — her class kendi pose'unda)
- ❌ Silah ismini geçirme prompt'ta (greatsword, bow, staff vs yok — sadece "empty grip position")
- ❌ Embedded glow (Karar #59)
- ❌ VFX/particle/light prompt'ta (Unity tarafı iş)
- ❌ Walk animasyonu (Karar #42 — sadece run cycle)
- ❌ Enhance with AI (Web UI Custom V3)

## Dispatch Sırası (önerilen)

1. **Reference image** seç — silahsız warblade base (var mı kontrol, yoksa Pixoroma'da Warblade'i style-only export)
2. **Warblade silahsız regen** (1 class, pilot)
3. **PASS** olursa diğer 9 class paralel dispatch
4. **9 decouple sprite** ayrı gen (object workflow)
5. **Idle template + Run extreme pose** sırayla
