# RIMA — Create Image Pro Idle Pose Batch (10 Class + 6 Mob)
# S69 REORGANIZE + MOB SECTION ADDED
# Source: batch_silahsiz_body_prompts.md (class) + NLM/COMBAT_ROSTER (mob)
# Pipeline: PixelLab Web App Create Image Pro

---

## UNIVERSAL IDLE RULES (ALL CHARACTERS)

- Base rotation = NEUTRAL idle pose (south = camera-facing reference). Class-specific stance idle overrides happen in animation clips (Karar #109 ambient idle).
- Yol A weapon decouple: body silahsiz, weapon ayrı sprite. Exceptions: Ronin sheath body sprite'da kalır, Brawler bare fists (silah yok), Elementalist disc Unity VFX (body unarmed).
- 8-direction support: south frame = reference pose; Custom V3 ile diğer 7 yön.
- Canvas: 64x64 chibi (class), 128x128 mob (Karar #74 elite size), PPU=64.
- Camera: ~35 degree high top-down (Hades match, Karar #113). PixelLab UI = "High top-down view".
- Karar #99: promptta hand mention YASAK (drift riski). Sprite hangi elde silah gösteriyorsa o doğru.
- Karar #71: silah hep elde (Ronin sheath istisna). Bu batch silahsız body — weapon prompt'a yazılmaz.
- Karar #108 narrative format: 4-6 cümle, "pixel/frame/loop" kelime YASAK, magnitude kelimeleri kullan (subtle/minimal/gentle/grounded).
- Embedded glow YASAK (Karar #59). VFX YASAK promptta — particle/glow/light Unity'de.
- Mob siluet okunabilirliği: combat clarity için net silhouette shape.
- Rift mob renk kuralı (Karar #98): Rift yaratıkları (Rift Hound, Plate Widow, Hollow Arbiter, Fracture Imp, Seam Crawler, Relic Caster) = cyan + violet palet zorunlu.

---

## YASAK (drift sebep)

- "dark fantasy" -> use "muted desaturated palette, weathered field-worn"
- "3/4 view" or any game name
- "no eyes" / "eyeless"
- "80 degree" / "extreme top-down bird's eye"
- Hand mention ("right hand grips", "left hand on hilt") — Karar #99
- Weapon name in unarmed body prompt (greatsword, bow, staff, etc.)
- Embedded glow / VFX / particle in prompt
- "dark fantasy" phrase
- Universal ready stance (Karar #80 ihlali — her class kendi pose'unda)

---

## SECTION A — 10 CLASS SILAHSIZ BODY PROMPTS

**Workflow:** PixelLab Web App Create Image Pro -> 8-direction character -> animations
**Style Reference:** Characters/anchors/reference.png or silah-temiz Warblade frame
**View:** High top-down ~35 degree, chibi 64x64, PPU 64
**Karar canon:** Karar #80 (Silhouette Bible) + #99 (weapon visibility) + #59 (VFX vs sprite) + #109 (ambient idle) + #42 (no walk, only run)

### Universal Neutral Pose (applies to all unless overridden per class)
Athletic standing, knees softly bent, body slight forward lean (top-down ~35 degree), arms relaxed at sides with hands open (no grip, no weapon, no fist). Class identity = silhouette/clothing/hair/accent color only.

---

### CLASS 01 — Warblade
```
64x64 chibi top-down male heavy warrior, low guard stance — broad-shouldered V-shape posture, body leaned forward, head slightly down. Two-hand grip position at chest level, both hands curled as if gripping a horizontal greatsword shaft but no weapon visible — empty hand grip. Dark leather armor #4F3A2C with brass buckle accents #C09455, cold blue strap details. View 35 degree high top-down ARPG angle, pixel art PPU 64, 8 directions.
```

### CLASS 02 — Ranger
```
64x64 chibi top-down female tactical rift hunter, asymmetric battle-worn armor, off-white bleached-ivory hair in low loose ponytail with escaping temple strands, faint cheek scar. Tactical hunter stance — left hand at thigh-level in bow-rest grip position (curled fingers gripping empty space where bow would be), right hand free at side. Eyes forward, alert. Dark forest green #2A3520 armor with heavier right pauldron and left forearm leather wrap, cold blue #7BA7BC hood/strap accents. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 03 — Shadowblade
```
64x64 chibi top-down male shadowblade, slim upright vertical posture, near-black deep purple armor #1A1025 with void purple accents #5A2A8A. Both hands curled in reverse-grip position close to body (elbows back, hands held close to torso as if holding twin blades reversed — but no blades visible), empty reverse-grip. Hooded, lean silhouette. NO embedded glow anywhere. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 04 — Elementalist (SILAHSIZ — disc decouple)
```
64x64 chibi top-down female elementalist mage, open palm casting stance — left palm faced upward at chest level (empty palm, no disc visible in this base), right hand at side with relaxed casting gesture. Dark indigo-purple cropped robe with bare midriff, honey-blonde warm hair in soft waves, academic controlled posture. NO weapon, NO disc, NO glow in this base sprite. Just the body in casting-ready pose. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 05 — Ravager
```
64x64 chibi top-down male berserker, forward-leaning aggressive posture, weight on front foot, ready to charge. Both hands curled at hip level in dual-hatchet grip position (fingers curled as if holding short axe handles, but no axes visible). Muscular build, dark rough armor with crimson red #D43F3F accents, bare arms with scars. NO weapons. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 06 — Ronin
```
64x64 chibi top-down male ronin samurai, iaido draw stance — body sideways angled, right hand at right hip in sword-draw position (curled fingers ready to grip a sword handle, but no katana visible), left hand on the empty scabbard/sheath at left hip (the scabbard sheath sprite IS visible, but no blade inside). Dark navy/black kimono, pale gold #C8A87A accents, focused calm expression. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 07 — Gunslinger
```
64x64 chibi top-down female gunslinger duellist, kinetic dueller stance — slightly crouched, weight shifted to one leg, both hands at hip level in pistol-grip position (fingers curled as if holding twin pistols, but no guns visible — empty dual-pistol grip). Long dark grey-purple trenchcoat #4A3A4A, deep auburn red hair tied back, alert eyes. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 08 — Brawler (SILAH ZATEN YOK)
```
64x64 chibi top-down male brawler, boxing/guard stance — fists clenched at chin level, knees bent, body slightly angled, ready to strike. Bare muscular torso with dark leather hand wraps and forearm bandages, orange #FF8C00 accent on belt and wraps. NO weapon anywhere — bare fists. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 09 — Summoner
```
64x64 chibi top-down female necromancer-summoner, orchestra conductor pose — left hand at chest level in lantern-grip position (fingers curled around an empty lantern handle that is not visible), right hand raised in directing/orchestrating gesture, palm open and fingers spread. Dark indigo robes with cyan trim accents, violet detail patterns, hooded with strands of pale hair visible. Calm controlling expression. NO lantern, NO glow, NO summoned minions in this base. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

### CLASS 10 — Hexer
```
64x64 chibi top-down female witch-hexer, curse-bearer stance — left arm holds an empty grimoire-holding position at chest (arm bent across chest as if cradling a thick book, but no book visible), right hand at side curled in staff-grip position (fingers around an empty vertical pole). Very dark purple-black robes with dark red #8B0000 accent trim, long unkempt dark hair, sickly pale skin, patient malicious expression. NO weapon, NO grimoire, NO flame in this base — empty hands in carry position. View 35 degree high top-down, pixel art PPU 64, 8 directions.
```

---

## SECTION B — 6 MOB IDLE POSE PROMPTS

**Biome:** F1 Shattered Keep / Shattered Ruins
**Canvas:** 128x128 (standard mob), PPU=64
**View:** ~35 degree high top-down ARPG camera (Hades match)
**Rift mob palette (Karar #98):** cyan + violet — applies to ALL 6 mobs
**Style:** chibi pixel art, muted desaturated palette, weathered field-worn, Salt and Sanctuary chibi tone
**Source:** COMBAT_ROSTER.md mob specs + mob_boss_skill_expansion_2026-05-13.md + Karar #98

**Mob idle rules:**
- Neutral standing idle — combat role readable from silhouette alone
- Rift energy = cyan glow accent only (NO embedded full glow — Karar #59 applies to mob too)
- Silhouette must be distinct from other mobs in the roster at 128px thumbnail
- South direction reference frame (camera-facing). 4 cardinal directions (N/S/E/W) or 8 if asymmetric.
- Animation target: breathing idle, subtle postural rock — no extreme movement in base pose

---

### MOB 01 — Fracture Imp
**Role:** Swarm Skirmisher / Rift-Born (M01, 48px threat display but 128px canvas)
**Combat role:** AoE bait, swarm pressure, swarming melee rush. Sivri uzun kollar, küçük üçgen gövde, bas silüetin %40'i.
**T1 skills:** Rift Lunge (leap attack), Death Splatter (rift goo on death)
```
128x128 pixel art chibi top-down rift creature, small impish swarm unit — tiny triangular torso, oversized head dominating 40 percent of silhouette, long spindly arms ending in sharp claws, low-slung crouching stance ready to lunge. Body riddled with hairline fractures leaking faint cyan light. Skin muted dark violet-grey, rift crack glow cyan #00FFCC at fracture lines only, violet #5A2A8A deep shadow. Coiled spring posture, weight forward on all limbs, head tilted with hungry wide eyes. NO full body glow — only crack line light leakage. View 35 degree high top-down ARPG, pixel art PPU 64, south facing, muted desaturated palette.
```

---

### MOB 02 — Seam Crawler
**Role:** Skirmisher / Rift-Born (M03, 96x96 wide horizontal)
**Combat role:** Zemin cataklarinda yasayan, hit-and-run flanker. Yassil zemine yapışık, 6 pençe çıkıntısı. Horizontal wide silhouette.
**T1 skills:** Submerge (50 percent hidden underground), Burst Strike (emerge burst 28 dmg + knockback)
```
128x128 pixel art chibi top-down rift creature, low-slung hexapedal crawler — wide flat body pressed close to cracked stone floor, six claw-legs splayed outward for lateral reach, carapace segmented like chitinous plates with seam lines glowing faint cyan. Body is wider than tall — yatay silhouette dominates. Upper shell muted dark violet-brown, seam crack glow cyan #00FFCC tracing along carapace joints, belly shadow violet #5A2A8A. Head is broad and flat with forward-facing mandibles. Idle stance low and still, coiled for emergence, body barely above ground. NO upright posture — purely horizontal threat. View 35 degree high top-down ARPG, pixel art PPU 64, south facing, muted desaturated palette.
```

---

### MOB 03 — Relic Caster
**Role:** Support / Fractured (M06, 80x80 small fragile — intentional small silhouette = execution priority cue)
**Combat role:** Summoner/support — calls Shardlings, gives Aegis Mark shields to allies. En kucuk figur, ince uzun govde, dik kristal kirik elinde.
**T1 skills:** Summon Shardling (1.5s channel), Aegis Mark (ally shield 3s)
**Visual note:** Intentionally smallest mob in room — fragile support, easy to target-read at 128px.
```
128x128 pixel art chibi top-down fractured mage, small slender support caster — tall narrow torso with thin limbs, deliberately fragile silhouette making it the smallest figure in any encounter. Holds a broken relic shard upright before chest (jagged crystal fragment, dark with inlaid cyan veins, not glowing fully — just cracked light). Worn ceremonial robes heavily tattered, muted violet-grey cloth, cyan #00FFCC accent trim at collar and cuffs, dark violet #5A2A8A robe shadow. Posture upright but slightly hunched with focus, relic cradled with both hands at chest level. Head small, eyes intent and glowing faint cyan. NO full glow — relic crack light only. View 35 degree high top-down ARPG, pixel art PPU 64, south facing, muted desaturated palette.
```

---

### MOB 04 — Rift Hound
**Role:** Fast skirmisher / Rift-Born (T1 grunt — Karar #74 small/orta mob, 96x96)
**Combat role:** Hızlı saldıran rift yaratığı. Hayvan formu, quadruped, rift enerjisiyle örülmüş vücut. Speed punisher archetype.
**Design basis:** Karar #98 cyan+violet rift creature. Karar #74: grunt tier = 96x96 (Seam Crawler eşdeğeri).
**Canvas:** 96x96 px, PPU=64 (NOT 128x128 — Hound grunt değil elite)
```
96x96 pixel art chibi top-down rift beast, quadruped canine-like creature with elongated front legs and raised spinal ridge along back, low coiled hunting stance with weight forward on front paws, head held low and forward elongated, dark matted fur-stone hybrid hide #2A2530 with warmer rust-leather patches #3D2D26, hairline cyan #00E5C8 rift cracks tracing along spine ridge and seeping through hide seams, eye slits glowing faint cyan, tendril void-tail trailing wisps behind (silhouette only, not full glow), muted desaturated Salt and Sanctuary tone, view 35 degree high top-down, south facing, PPU 64.
```
<!-- Source: rima-design 3_mob_visual_brief.md (2026-05-14) -->

---

### MOB 05 — Plate Widow
**Role:** Armored elite / Rift-Born (T2 elite — Karar #74 elite class, 128x128)
**Combat role:** Armored female form, defensive elite. F1 plate variant. High threat priority due to armored silhouette.
**Design basis:** Karar #98 cyan+violet rift creature. Disciplined human-knight aesthetic — fragmented samurai plate, NOT insect. Trapezoidal silhouette: wide shoulders + narrow armored waist + planted leg base.
**Canvas:** 128x128 px, PPU=64
```
128x128 pixel art chibi top-down armored rift widow, tall female figure in fragmented dark mourning-violet plate armor #3A2D3A with cracked seam lines glowing faint cyan #00E5C8, helm fully closed with narrow horizontal visor slit pair, wide-stanced defensive pose with left forearm raised in plate-guard across torso and right arm coiled back at hip, layered shoulder plates angled outward in trapezoidal silhouette, narrow armored waist, planted leg base, hairline cyan crack veins along plate joints only — no full body glow, muted desaturated Salt and Sanctuary tone, view 35 degree high top-down, south facing, PPU 64.
```
<!-- Source: rima-design 3_mob_visual_brief.md (2026-05-14) -->

---

### MOB 06 — Hollow Arbiter
**Role:** Controller / Rift-Born (T2 elite — Karar #74 elite class, 128x128)
**Combat role:** Hollow void authority figure, zone debuffer. Tall slender form, chest cavity open with depth glow. CRITICAL: no chains anywhere — distinct from Penitent Sovereign.
**Design basis:** Karar #98 cyan+violet rift creature. "Hollow" = open chest cavity depth glow (NOT surface glow). "Arbiter" = ritual-upright authority, tall slender (not towering — distinct from Hollow Hulk 160px).
**Canvas:** 128x128 px, PPU=64
```
128x128 pixel art chibi top-down hollow arbiter judge, tall slender bipedal authority figure draped in dim stone-grey wrapped cloth #2D2A35 with decayed wisp-violet drapery accents #5B4A6E, hooded head with two suspended cyan #00E5C8 orb eyes, chest cavity visibly open as a narrow vertical slit through the wrapped cloth with cyan depth glow seeping from within (cavity opening only — not surface body glow), rigid upright posture with both arms raised at shoulder height palms turned outward in arbiter judgment stance, narrow waist with raised bony shoulder spikes beneath cloth, fabric trailing at base, no chains anywhere, muted desaturated Salt and Sanctuary tone, view 35 degree high top-down, south facing, PPU 64.
```
<!-- Source: rima-design 3_mob_visual_brief.md (2026-05-14) -->

---

## Decouple Sprite List (class section — ayrı gen)

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

---

## Animation Pipeline Reference (Karar #42 + Karar #108)

**Idle:** Template-first (`breathing-idle` 8f) — ambient özelleştirme polish phase
**Run:** 6 frame, 8 directions separately, no flip (Karar #46)
**Attack (heavy/multi-frame):** Karar #14 3-Segment Workflow

## Cost Estimate

**Per class (body + idle):**
- Body silahsız regen (8-dir): ~3-5 gen
- Idle (template): 1 gen
- Run (2 extreme x 8 dir + interpolate): ~6-8 gen
- Weapon sprite: ~1 gen
- **Total per class: ~12-15 gen**

**Per mob (idle pose only — 4 directions):**
- Create Image Pro mob idle: ~2-3 gen
- **Total per mob: ~2-3 gen**

**Full roster:**
- 10 class: ~120-150 gen
- 6 mob: ~12-18 gen
- **Grand total estimate: ~132-168 gen**

---

## Dispatch Order (recommended)

1. Reference image select — silahsiz Warblade base (existing anchor or fresh gen)
2. Warblade silahsiz regen (1 class, pilot pass)
3. PASS olursa diğer 9 class paralel dispatch
4. 9 decouple sprite ayrı gen (object workflow)
5. Idle template + Run extreme pose sirayla
6. Mob section: Fracture Imp pilot first (simplest silhouette), then remaining 5 mobs
