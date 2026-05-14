# Character Idle (Weaponless) + Weapon Prompts — LOCK

**Date:** 2026-05-14 S73
**Source:** rima-design Opus verdict (dispatch a080564b62e90b77b)
**Status:** LOCKED for PixelLab Create Image Pro batch

**Locked rules honored:**
- Karar #71 — silah grip pose-ready (silahsız body bile grip geometry)
- Karar #80 — Class Silhouette Bible (10 class identity canon)
- Karar #99 — silah görünürlüğü (NO hand mention in prompts)
- Karar #108 — Custom V3 hard rules
- Karar #109 — per-class ambient idle personality
- Karar #123 — Yol A weapon decouple (body unarmed)
- Karar #124 — Faz 1: Warblade Base+T2 Rift only
- Karar #98 — F1 mob palette canon

---

## K1-K5 LOCKED Decisions

### K1: Silahsız Idle Pose per Class
Her class için motion-only pose (silah yok, grip geometry korunur). Pose detayları aşağıdaki prompt'larda.

### K2: Weapon Prompt Approach
**Create Image Pro per-weapon** — NOT Style Reference.
- Batch küçük (8 base weapon), Pro precision daha değerli
- Style sheet → drift riski yüksek
- Her silah explicit silhouette pose
- Style anchor: `Characters/anchors/reference.png` (palette/shading borrow)
- Tahmini kredi: 11 × ~6 = ~66 credit

### K3: Weapon Sprite Sizes (canvas)
| Class | Weapon | Canvas | Adet |
|---|---|---|---|
| Warblade | Greatsword | 64×32 | 1 |
| Ranger | Compound bow | 64×64 | 1 |
| Shadowblade | Twin reverse daggers | 32×32 | 2 (pair) |
| Elementalist | Disc | **NO SPRITE** (Unity VFX, Karar #59) | 0 |
| Ravager | Dual hatchets | 32×32 | 2 (pair) |
| Ronin | Katana blade | 64×32 | 1 (sheath stays on body) |
| Gunslinger | Dual pistols | 32×32 | 2 (pair) |
| Brawler | Bare fists | **NO SPRITE** | 0 |
| Summoner | Soul lantern | 32×32 | 1 |
| Hexer | Grimoire + curse staff | 32×32 + 64×64 | 2 |
| **Total** | | | **11 weapon sprites** |

Faz 1 scope: tüm yukarıdakiler Base form only. Warblade T2 Rift variant Karar #124 gereği +1 ek sprite.

### K4: Universal Anchor Reference Instruction
Her class prompt'ın **BAŞINDA** bu cümle var:

> *Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference.*

### K5: "16 Varyasyon" — Doğru Yorum
**16 = 10 class + 6 mob = full base roster south-pass.** 16 seed reroll DEĞİL.
- Each gen = 1 character base (south frame)
- Base sprite QC geçince → Create Character (8-dir derivation, ayrı step)
- Fail olursa tek o sprite regen, batch regen YOK

**Toplam Faz 1 gen sayısı:**
| Stage | Count | Notes |
|---|---|---|
| Character base (silahsız south) | 16 | Create Image Pro |
| 8-dir derivation | 16 | Create Character (ayrı step) |
| Weapon sprites Faz 1 | 11 | Create Image Pro |
| Warblade T2 Rift variant | 1 | Karar #124 |
| **Toplam Create Image Pro** | **~28** | ~168 credit @ 6/gen |

---

## 10 Class Weaponless Idle Prompts

> Her prompt **K4 prefix + pose statement + motion verbs + locked parts + mood** formatında.
> Anchor file: `Characters/anchors/{class_name}.png`
> Output: 64×64 chibi, south-facing single frame

### 1. Warblade
- **Anchor:** `Characters/anchors/warblade.png`
- **Pose:** Low guard stance, both hands curled at chest level (two-hand grip geometry, no weapon)
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The fighter holds a low guard stance, body angled forward slightly, both hands curled at chest level holding empty horizontal space as if cradling a long object. Chest rises and falls in a slow breathing rhythm while shoulders shift subtly with the inhale. Head, hips, and feet remain fixed; only the torso breathing and shoulder roll animate. Mood is grounded, patient, dangerous.

### 2. Ranger
- **Anchor:** `Characters/anchors/ranger.png`
- **Pose:** Tactical hunter stance, one hand curled at thigh (vertical grip geometry), other loose at side
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The hunter stands in a tactical scout pose, one hand curled at the thigh holding empty vertical space, the other hand loose at the side. Torso breathes shallowly, head tilts a degree as if listening to distant motion. Hips, feet, and arms below the wrist remain fixed; only chest, shoulders, and head animate. Mood is alert, patient, predator-still.

### 3. Shadowblade
- **Anchor:** `Characters/anchors/shadowblade.png`
- **Pose:** Slim upright, both hands curled near torso palms inverted (reverse-grip geometry), elbows tucked back
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The assassin stands slim and upright, both hands curled near the torso with palms turned inward and elbows tucked back. Body weight rocks almost imperceptibly forward and back, head dips a fraction with each weight shift. Hips, feet, and lower arms remain fixed; only the body sway and head dip animate. Mood is coiled, predatory, silent.

### 4. Elementalist
- **Anchor:** `Characters/anchors/elementalist.png`
- **Pose:** Open casting stance, one palm upward at chest level (empty palm, disc absent), other loose
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The mage stands in an open casting stance, one palm faced upward at chest level holding empty cradled space, the other hand loose at the side. Fingers flex faintly as if calling power, chest rises slowly with a deep breath. Hips, feet, and the lowered arm remain fixed; only the upper palm fingers, chest, and a slight head lift animate. Mood is curious, focused, ember-bright.

### 5. Ravager
- **Anchor:** `Characters/anchors/ravager.png`
- **Pose:** Aggressive forward crouch, both hands curled at hip level (dual short-handle geometry), shoulders rolled forward
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The berserker hunches in an aggressive forward crouch, both hands curled at hip level holding empty short-handle space, shoulders rolled forward in barely contained menace. Shoulders rise and clench with each heavy breath while head tilts low and forward. Hips, feet, and curled hands remain fixed; only the shoulders, neck, and head animate. Mood is furious, restrained, about-to-snap.

### 6. Ronin
- **Anchor:** `Characters/anchors/ronin.png`
- **Pose:** Iaido draw stance, body sideways, one hand curled at right hip (draw-ready), other on empty scabbard at left hip — **scabbard sprite stays on body** (Karar #123 exception)
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon detail from the reference, but DO retain a plain sheath silhouette at the left hip. The duelist stands in an iaido draw posture, body angled three-quarter sideways, one hand curled at the right hip in draw-ready geometry, the other resting on the empty sheath at the left hip. Chest breathes with quiet discipline, head holds a meditative tilt. Hips, feet, and both hand positions remain fixed; only chest, shoulders, and a faint head sway animate. Mood is composed, lethal-calm, mid-meditation.

### 7. Gunslinger
- **Anchor:** `Characters/anchors/gunslinger.png`
- **Pose:** Duellist micro-crouch, both hands curled at hip level (dual vertical handle geometry), weight on one leg
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The duellist stands in a slight micro-crouch with weight shifted onto one leg, both hands curled at hip level holding empty vertical handle space. Shoulders roll lightly with breathing, head turns a few degrees as if scanning for a draw moment. Hips, feet, and curled hand geometry remain fixed; only the shoulders, head turn, and chest animate. Mood is cocky, ready, hair-trigger.

### 8. Brawler
- **Anchor:** `Characters/anchors/brawler.png`
- **Pose:** Boxing guard, both fists clenched at chin level (this class IS bare-fisted — no decouple needed)
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW body in the stance described below; do not copy outfit or accessory detail from the reference, but DO retain the bare-fisted nature — no weapon, just clenched fists. The fighter holds a boxing guard, both fists clenched and raised to chin level, knees softly bent in a fight stance. Fists bob in a tiny rhythmic feint, shoulders roll with the bounce, head bobs in counter-rhythm. Hips and feet remain planted; only fists, shoulders, and head animate. Mood is hungry, restless, gleeful.

### 9. Summoner
- **Anchor:** `Characters/anchors/summoner.png`
- **Pose:** Orchestra conductor pose, one hand curled at chest (vertical handle geometry — lantern absent), other raised palm-open in directing gesture
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The summoner stands in an orchestra-conductor pose, one hand curled at chest level holding empty vertical handle space, the other raised palm-open in a gentle directing gesture. The conducting palm fingers flex faintly as if calling unseen voices, chest rises and falls deeply. Hips, feet, and the chest-held hand remain fixed; only the raised palm fingers, chest, and shoulder lift animate. Mood is reverent, melancholic, communing-with-spirits.

### 10. Hexer
- **Anchor:** `Characters/anchors/hexer.png`
- **Pose:** Curse-bearer stance, one arm bent across chest cradling empty book-shaped space (grimoire geometry), other hand curled at side (vertical pole geometry — staff absent)
- **Prompt:**
> Reference image provided shows the 35-degree high top-down camera angle, the chibi 64×64 body proportions, and the south-facing direction. Generate a NEW unarmed body in the stance described below; do not copy any weapon, accessory, or outfit detail from the reference. The hexer stands in a curse-bearer pose, one arm bent across the chest cradling empty book-shaped space, the other hand curled at the side holding empty vertical pole space. Breathing is slow and heavy, head tilts down toward the cradled space as if reading something invisible. Hips, feet, and both hand geometries remain fixed; only chest, head tilt, and a faint shoulder shrug animate. Mood is brooding, ancient, mid-incantation.

---

## 6 Mob Weaponless Idle Prompts

> **NOTE:** Mobs are NOT decoupled per Karar #123 (player-only). Mob prompts retain weapons where canon requires (Hollow Arbiter sword+crown).
> Source: `STAGING/idle_batch_class_mob_create_image_pro.md` Section B (S70 batch — already Karar #98 compliant)
> Anchor folder: `Characters/anchors/mobs/{name}.png`

**Mob inventory (already locked in S70 batch, copy-paste from there for PixelLab):**
1. **Seam Crawler** — `mobs/seam_crawler.png` — F1 fracture imp 64×64
2. **Plate Widow** — `mobs/plate_widow.png` — armored matriarch 80×80 (rima-design 3-mob brief)
3. **Relic Caster** — `mobs/relic_caster.png` — 64×64
4. **Rift Hound** — `mobs/rift_gound.png` (filename typo — canonical: rift_hound) — 96×96
5. **Hollow Arbiter** — `mobs/hollow_arbitter.png` (filename typo — canonical: hollow_arbiter) — 96×96 (sword + crown, NOT decoupled)
6. **Fracture Imp** — `mobs/fracture_imp.png` — 64×64

→ S70 mob prompt'larını birebir kopyala. rima-design audit'i bunlara dokunmamış, Karar #98 cyan+violet compliant.

---

## Weapon Sprite Prompts (11 sprite, Create Image Pro)

> **Style anchor:** `Characters/anchors/reference.png` (her gen'de attach et — palette/shading borrow)
> **Tool:** Create Image Pro per-weapon (K2 verdict)
> **Output:** Transparent background, isolated weapon, no character

### Universal weapon prompt prefix (her silah için bunu başa koy)
> *Isolated [weapon type] on fully transparent background, no character body, no hand, no shadow. Pixel art at 32 pixels per unit, matching the RIMA chibi style anchor provided (dark gritty palette, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static frame, no animation.*

---

### 1. Warblade Greatsword (Base) — 64×32
> [Universal prefix] A two-handed greatsword laid horizontal across the canvas, blade pointing east, dark steel with hairline edge highlights, leather-wrapped grip on the west end, plain crossguard. Worn from use but not rusted. Silhouette readable as massive heavy weapon. Style: Salt and Sanctuary mat painterly pixel.

### 1b. Warblade Greatsword T2 Rift Variant — 64×32 (Karar #124)
> [Universal prefix] A two-handed greatsword laid horizontal, blade pointing east, dark steel base with thin cold-blue cyan rift fractures glowing along the blade edge (not bright, hairline trace), leather-wrapped grip, crossguard with faint violet rune etching. The rift cracks read as magical infection, not damage. Silhouette identical to base form, palette shifted toward cool tones with cyan accent.

### 2. Ranger Compound Bow — 64×64
> [Universal prefix] A compound recurve bow standing vertical at canvas center, dark wood limbs curving outward at top and bottom, taut bowstring running the full vertical length, leather-wrapped grip at center. No arrow. Worn hunter equipment, no ornament. Silhouette readable as tall slim hunter tool.

### 3a. Shadowblade Reverse Dagger (LEFT) — 32×32
> [Universal prefix] A short reverse-grip dagger held mid-canvas, blade pointing down-east, dark blackened steel blade, plain hilt, no guard. Compact and lethal, no decoration. Silhouette readable as quick assassination weapon.

### 3b. Shadowblade Reverse Dagger (RIGHT) — 32×32
> [Universal prefix] A short reverse-grip dagger held mid-canvas, blade pointing down-west (mirror of left), dark blackened steel blade, plain hilt, no guard. Identical to the left dagger but mirrored.

### 4a. Ravager Hatchet (LEFT) — 32×32
> [Universal prefix] A short single-handed hatchet, axe head pointing east, dark stained wooden handle, heavy iron head with chipped cutting edge, leather-wrapped grip. Brutal and worn. Silhouette readable as primitive weapon of rage.

### 4b. Ravager Hatchet (RIGHT) — 32×32
> [Universal prefix] A short single-handed hatchet, axe head pointing west (mirror of left). Identical otherwise to the left hatchet.

### 5. Ronin Katana Blade — 64×32
> [Universal prefix] A katana drawn from its sheath, blade laid horizontal pointing east, polished pale steel with subtle blue tint near the edge, leather-wrapped tsuka grip on the west end, simple round tsuba guard. Silhouette readable as elegant single-edge sword. NO scabbard in this sprite (scabbard stays on body).

### 6a. Gunslinger Pistol (LEFT) — 32×32
> [Universal prefix] A short flintlock-style dueling pistol, barrel pointing east, dark wood grip, blackened steel barrel, ornate but restrained metalwork on the lock. Single weapon, compact silhouette.

### 6b. Gunslinger Pistol (RIGHT) — 32×32
> [Universal prefix] A short flintlock-style dueling pistol, barrel pointing west (mirror of left). Identical otherwise to the left pistol.

### 7. Summoner Soul Lantern — 32×32
> [Universal prefix] A handheld lantern with a curved iron frame, hanging chain at the top, glowing pale-green ghost-flame inside the glass — flame is soft, not bright, like trapped soul-light. Worn but still functioning. Silhouette readable as mystical light source.

### 8a. Hexer Grimoire (Book) — 32×32
> [Universal prefix] A closed leather-bound grimoire, dark cover with faint embossed sigil glowing thin cyan-green at the center, brass corner reinforcements, ribbon marker dangling. Single static closed book viewed from above-front. Silhouette readable as a cursed tome.

### 8b. Hexer Curse Staff — 64×64
> [Universal prefix] A tall wooden staff standing vertical, twisted dark wood with bark stripped in places, topped with a small green flame at the apex (soft glow, not bright), bone or root tangles wrapped around the top third. Silhouette readable as a witch's curse focus. The flame at the top occupies the upper quarter of the canvas.

### 9. Brawler — **NO SPRITE**
Brawler kullanır = clenched fists (body sprite'ın parçası). Weapon decouple uygulanmaz, weapon sprite gerekmez.

### 10. Elementalist — **NO SPRITE**
Elementalist disc'i Unity VFX (Karar #59 explicit). Body silahsız zaten, disc runtime particle. Weapon sprite gerekmez.

---

## Generation Sırası Önerisi

1. **İlk batch: 10 class base** — `Characters/anchors/{name}.png` anchor + class prompt
2. **İkinci batch: 6 mob base** — `Characters/anchors/mobs/{name}.png` anchor + S70 mob prompt
3. QC: 16 base sprite incele, fail olanları regen (tek tek)
4. **Üçüncü batch: 11 weapon sprite** — `Characters/anchors/reference.png` style anchor + weapon prompt
5. **Faz 1 Karar #124: +1 Warblade T2 Rift variant**
6. (Sonraki step) Create Character ile 8-yön derivation (ayrı tool, ayrı kredi tier)

**Total Faz 1: ~28 Create Image Pro gen, ~168 credit.**

---

## QC Checklist (user gen başlamadan önce kontrol)

- [ ] Hiçbir class prompt'unda "weapon/sword/blade/bow/staff/grimoire/lantern/pistol/hatchet/katana" yok
- [ ] Hiçbir class prompt'unda "left hand / right hand / his hand / her hand" yok (Karar #99)
- [ ] Her class prompt K4 prefix ile başlıyor
- [ ] 10 class + 6 mob = 16 base prompt
- [ ] 11 weapon prompt (pairs ×2 doğru sayıldı)
- [ ] Brawler + Elementalist için NO SPRITE notu açık
- [ ] Anchor file path'leri canonical (`mobs/seam_crawler.png` vs `seam_crawler.png` — mob anchor'lar `Characters/anchors/mobs/` altında)
- [ ] Ronin için "scabbard stays on body" açık not (Karar #123 istisna)
- [ ] Warblade T2 Rift variant prompt ayrı listede (Karar #124)

---

## Notes

- **NLM auth expired** session sırasında — rima-design memory dosyalarını kullandı (canon güvende, NLM verification next session öncesi yapılmalı)
- **S70 idle batch** bu LOCK ile **DEPRECATED** — class prompt'ları K4 prefix + motion-only formatla yeniden yazılmış. Mob section S70'den birebir kullanılabilir.
- Frame anim (Create Character 8-dir) ayrı pipeline — bu LOCK sadece SOUTH BASE FRAME için
