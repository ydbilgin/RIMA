# Weapon Sprite Sizes + Prompts -- LOCK S74

**Date:** 2026-05-14 S74
**Source:** rima-design Opus judgment (sub-agent dispatch a22f8def), J3 section
**Status:** LOCKED
**Supersedes:** K3 weapon size table in character_idle_weaponless_prompts_LOCK.md (S73)

---

## Honored Kararlar

- Karar #71 -- positive-only; grip continuity with body sprite
- Karar #98 -- F1 accent palette: cyan #00FFCC / violet #5A2A8A where applicable
- Karar #99 -- silhouette readability; NO hand mentions in prompts
- Karar #123 -- weapon decouple is player-only
- Karar #124 -- Faz 1: Warblade Base + T2 Rift only

---

## Final Canvas Size Table (J3 Opus)

| Class | Weapon | Canvas | Adet | Notes |
|---|---|---|---|---|
| Warblade | Greatsword (Base) | 56x20 | 1 | |
| Warblade T2 Rift | Greatsword (Rift variant) | 56x20 | 1 | Karar #124 |
| Ranger | Compound bow | 48x56 | 1 | tall vertical |
| Shadowblade | Twin daggers | 24x24 | 2 (pair) | L + R |
| Elementalist | Disc | NO_SPRITE | 0 | Unity VFX |
| Ravager | Dual hatchets | 28x28 | 2 (pair) | L + R |
| Ronin | Katana | 56x20 | 1 | sheath stays on body sprite |
| Gunslinger | Dual pistols | 24x20 | 2 (pair) | L + R |
| Brawler | Bare fists | NO_SPRITE | 0 | body sprite carries fists |
| Summoner | Soul lantern | 28x32 | 1 | |
| Hexer | Curse staff | 48x56 | 1 | |
| **Hexer** | **Grimoire** | **CUT** | **0** | see below |
| **Total** | | | **11 sprites** | |

---

## Hexer Dual Weapon Conflict Resolution

**Decision: Single staff only. Grimoire CUT from weapon batch.**

Opus rationale (verbatim from J3): The Hexer body sprite already craddles a passive book-shaped silhouette at the chest -- the grimoire is an identity body accessory, not an equippable active weapon. Issuing it as a separate decoupled weapon sprite would violate Karar #18 (passive body accessory clause) and Karar #123 (decouple applies to active hand-held weapons only). The curse staff is the active weapon and receives the single weapon sprite. Grimoire stays embedded in the body idle pose geometry, never separately attached at runtime.

---

## Universal Weapon Prompt Prefix

Apply at the START of every weapon prompt below:

> Isolated [weapon type] on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame.

---

## 11 Weapon Prompts

---

### 1. Warblade Greatsword (Base) -- 56x20

- **Class:** Warblade
- **Canvas:** 56x20
- **Adet:** 1

**Prompt:**

Isolated greatsword on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. The blade rests with point at the east end and hilt at the west, oriented horizontal across the canvas, dark steel body with a subtle hairline edge highlight along the fuller groove. The leather-wrapped grip sits at the west quarter, a plain wide crossguard spanning the grip join. The steel reads worn from campaign use, surfaces scarred but not rusted, silhouette clearly massive and two-handed. Style: Salt and Sanctuary matte painterly pixel, heavy weapon authority.

---

### 2. Warblade Greatsword T2 Rift Variant -- 56x20 (Karar #124)

- **Class:** Warblade (Tier 2 Rift form)
- **Canvas:** 56x20
- **Adet:** 1

**Prompt:**

Isolated greatsword on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. The blade rests horizontal with point east and hilt west, dark steel base form identical in silhouette to the base greatsword, but thin cold cyan rift fractures (#00FFCC) trace hairline cracks running along the blade from crossguard toward tip, glowing at gentle intensity. The crossguard carries a faint violet rune etching (#5A2A8A) that sits quietly against the iron. The rift cracks read as magical infection grown into the metal, not damage. Palette shifts toward cool tones overall.

---

### 3. Ranger Compound Bow -- 48x56

- **Class:** Ranger
- **Canvas:** 48x56
- **Adet:** 1

**Prompt:**

Isolated compound bow on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. The bow stands vertical at canvas center, dark wood limbs curving outward symmetrically at the top and bottom cam ends, the bowstring in slack rest spanning the full vertical length, a leather-wrapped riser at the center. The cams at each limb tip read as compound mechanics, angular and functional. No arrow present. Silhouette is tall and slim, tool-pragmatic with no ornament. Mood is a hunter's working instrument, worn grip, reliable.

---

### 4a. Shadowblade Reverse Dagger LEFT -- 24x24

- **Class:** Shadowblade
- **Canvas:** 24x24
- **Adet:** 1 (left of pair)

**Prompt:**

Isolated reverse-grip dagger on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A short single-edged dagger sits at mid-canvas with the blade pointing southeast in a reverse-grip angle, blackened steel blade absorbing light along the flat, a subtle edge highlight at the cutting side. The plain hilt sits at the northwest corner of the canvas, no guard, grip wrapped in dark cloth. Compact silhouette reads as a fast assassination tool. No decoration.

---

### 4b. Shadowblade Reverse Dagger RIGHT -- 24x24

- **Class:** Shadowblade
- **Canvas:** 24x24
- **Adet:** 1 (right of pair)

**Prompt:**

Isolated reverse-grip dagger on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A short single-edged dagger sits at mid-canvas with the blade pointing southwest in a reverse-grip angle (mirror of the left dagger), blackened steel blade, subtle cutting-edge highlight, plain dark-cloth hilt at the northeast corner. Silhouette and details match the left dagger exactly but mirrored. No decoration.

---

### 5a. Ravager Hatchet LEFT -- 28x28

- **Class:** Ravager
- **Canvas:** 28x28
- **Adet:** 1 (left of pair)

**Prompt:**

Isolated hatchet on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A single-handed hatchet rests at canvas center, the heavy iron axe-head at the east side with a chipped and grooved cutting edge, the dark stained wooden haft running west with a leather-wrapped grip at the end. Head-heavy weight distribution is visible in the silhouette mass. Surface is brutally worn, chipped from impact, no polish. Silhouette reads as a primitive weapon carried and used hard.

---

### 5b. Ravager Hatchet RIGHT -- 28x28

- **Class:** Ravager
- **Canvas:** 28x28
- **Adet:** 1 (right of pair)

**Prompt:**

Isolated hatchet on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A single-handed hatchet rests at canvas center, axe-head at the west side (mirror of the left hatchet), wooden haft running east, leather-wrapped grip at the east end. Details and silhouette mass match the left hatchet exactly, orientation mirrored.

---

### 6. Ronin Katana -- 56x20

- **Class:** Ronin
- **Canvas:** 56x20
- **Adet:** 1
- **Note:** This sprite is the drawn blade only. Scabbard remains integrated in the body sprite (Karar #123 Ronin exception).

**Prompt:**

Isolated katana blade on fully transparent background, no character body, no hand, no shadow, no scabbard. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. The blade rests horizontal with tip at east and tsuka grip at west, polished pale steel with a subtle blue tint along the hamon line near the edge, grain-textured tsuka wrap at the grip end, a simple round tsuba at the blade join. Single elegant edge, no embellishment beyond the traditional form. Silhouette reads as refined single-edge authority.

---

### 7a. Gunslinger Pistol LEFT -- 24x20

- **Class:** Gunslinger
- **Canvas:** 24x20
- **Adet:** 1 (left of pair)

**Prompt:**

Isolated pistol on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A short flintlock-styled dueling pistol rests with barrel pointing east, blackened steel barrel, dark wood grip curving south at the west end, restrained metalwork on the lock plate. The hammer sits in a cocked-ready position. Compact readable silhouette: barrel east, grip south-west, cock visible at the top of the lock. Worn but functional, a tool carried by someone who draws first.

---

### 7b. Gunslinger Pistol RIGHT -- 24x20

- **Class:** Gunslinger
- **Canvas:** 24x20
- **Adet:** 1 (right of pair)

**Prompt:**

Isolated pistol on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A short flintlock-styled dueling pistol rests with barrel pointing west (mirror of the left pistol), dark wood grip curving south at the east end, hammer cocked. Details and silhouette match the left pistol exactly, orientation mirrored.

---

### 8. Summoner Soul Lantern -- 28x32

- **Class:** Summoner
- **Canvas:** 28x32
- **Adet:** 1

**Prompt:**

Isolated soul lantern on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. A handheld lantern hangs at canvas center, curved iron frame forming an oval cage around a soft cyan-violet flame inside the glass panels (#00FFCC light, #5A2A8A inner warmth), the hanging chain looped at the top of the canvas. The flame is gentle and contained, soul-light trapped rather than burning. Frame is aged iron, surface worn. Chain links read as hand-wrought. Silhouette reads as a mystical tethered light source.

---

### 9. Hexer Curse Staff -- 48x56

- **Class:** Hexer
- **Canvas:** 48x56
- **Adet:** 1

**Prompt:**

Isolated curse staff on fully transparent background, no character body, no hand, no shadow. Pixel art matching the RIMA chibi style anchor provided (muted desaturated palette, weathered field-worn, hairline detail, painterly shading). View angle: 35-degree high top-down to match player sprite render. Single static resting-pose frame. The staff stands vertical at canvas center, shaft running the full height in twisted dark wood with bark stripped in irregular bands, a curse-focus head at the apex where bone or root tendrils are bound around the top third with thin cord. A soft green-cyan flame sits at the very tip of the staff head, glowing gently upward and occupying the upper fifth of the canvas. The flame is restrained, like something old that no longer needs to burn bright. Silhouette reads: tall column, slight organic twist, dangerous focus at the crown.

---

## NO_SPRITE Entries

### Elementalist Disc

**Status: NO_SPRITE**
The Elementalist disc is a Unity VFX particle system at runtime (Karar #59 explicit). The body sprite is fully unarmed. No weapon sprite is generated. The disc visual is handled by the game engine, not a spritesheet.

---

### Brawler Bare Fists

**Status: NO_SPRITE**
The Brawler class carries no weapon. Fists are clenched and visible as part of the body sprite itself (boxing guard idle, clenched geometry baked into the character animation). Weapon decouple does not apply. No separate weapon sprite is generated.

---

## Faz 1 Weapon Batch Summary

| # | Weapon | Canvas | Gen |
|---|---|---|---|
| 1 | Warblade Greatsword Base | 56x20 | 1 |
| 2 | Warblade Greatsword T2 Rift | 56x20 | 1 |
| 3 | Ranger Compound Bow | 48x56 | 1 |
| 4a | Shadowblade Dagger L | 24x24 | 1 |
| 4b | Shadowblade Dagger R | 24x24 | 1 |
| 5a | Ravager Hatchet L | 28x28 | 1 |
| 5b | Ravager Hatchet R | 28x28 | 1 |
| 6 | Ronin Katana | 56x20 | 1 |
| 7a | Gunslinger Pistol L | 24x20 | 1 |
| 7b | Gunslinger Pistol R | 24x20 | 1 |
| 8 | Summoner Soul Lantern | 28x32 | 1 |
| 9 | Hexer Curse Staff | 48x56 | 1 |
| -- | Elementalist Disc | NO_SPRITE | 0 |
| -- | Brawler Fists | NO_SPRITE | 0 |
| **Total** | | | **11 gen** |

**Estimated credit:** 11 x ~6 = ~66 credit (Create Image Pro)

**Style anchor for all weapon gens:** Characters/anchors/reference.png
