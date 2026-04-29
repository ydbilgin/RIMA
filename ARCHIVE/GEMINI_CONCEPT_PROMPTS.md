# RIMA — Gemini Concept Prompt Kütüphanesi
> **Amaç:** Keşif modu — Gemini ile tarz kararı al, PixelLab'da üret.
> **Kullanım:** Her promptu Gemini'ye GÖRSEL ile birlikte gönder. Hangi görselleri ekleyeceğin her bölümde belirtildi.
> **Dil:** Promptlar İngilizce (Gemini için). Notlar Türkçe (biz için).

---

## WORKFLOW KARARI — KEŞİF MOD

```
PHASE 0 → CONCEPT (Gemini)
  Bu dosyadaki promptları Gemini'ye gönder
  Çıktıya bak → "bu tarz" onayı ver
  Onaylanmayan tarza girme — yeni prompt yaz

PHASE 1 → EXPLORE (PixelLab Standard)
  Standard mode → hızlı iterasyon
  Her karakter için 3-5 varyant
  Her çıktının SEED'ini not et
  Claude QC: siluet ✓ rift energy ✓ palette ✓

PHASE 2 → SELECT
  Beğenilen + seed → "confirmed" listesi
  Animate etme kararı burada verilir

PHASE 3 → PRODUCE (PixelLab Pro)
  Pro mode + aynı seed + params → final sprite

PHASE 4 → ANIMATE (Aseprite)
  USER_IDLE_WALK_DEATH / USER_CHAR_ATTACKS workflow
```

---

## BÖLÜM 0 — STİL ANCHOR (Her promptun başına yapıştır)

> **Her promptu göndermeden önce şu görselleri Gemini'ye ekle:**
> - `TASARIM/CLASS_CONCEPTS/warblade.png`
> - `TASARIM/CLASS_CONCEPTS/shadowblade.png`
> - `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/rima_style_anchor.png`

```
STYLE REFERENCE: The attached images define the exact visual direction.
Match this style precisely — DO NOT drift toward:
- Generic medieval skull/spike fantasy armor
- Bright colorful high fantasy (no gold plate, no rainbow magic)
- Anime or chibi proportions
- Painterly or illustrative style (this must be PIXEL ART)
- Modern fantasy (no sci-fi elements)

REQUIRED STYLE RULES:
- Pixel art, chunky visible pixels, no anti-aliasing
- Black outlines, 1-pixel thick
- Limited palette: dark base (dark iron, cracked leather, worn cloth) + ONE energy accent per character
- Energy accents ONLY: cold blue (rift energy) OR void purple (corruption) — NOT fire red, NOT generic green, NOT gold
- Battle-worn: torn cloaks, dented armor, rust, scars — everything has seen combat
- Heroic proportions: muscular but not exaggerated, no giant pauldrons
- Transparent background (alpha)
- Game sprite quality — Hades art direction (Fractured Epic: vivid dramatic contrast, NOT grimdark desaturation)
```

---

## BÖLÜM 1 — CLASS ROSTER SHEET (10 class, tek görsel)

> **Ekle:** warblade.png + shadowblade.png + elementalist.png (CLASS_CONCEPTS klasöründen, pixel art olanlar)
> **Amaç:** Tüm class'ların bir arada görsel uyumunu kontrol et.

```
[PASTE SECTION 0 STYLE ANCHOR HERE]

Draw a pixel art CHARACTER ROSTER SHEET showing all 10 playable classes of a Fractured Epic action RPG.
Arrange them in a single horizontal line, all at the same baseline.
Each character: approximately 128x160 pixels, isolated sprite with transparent background.
Show all 10 in one wide image — this is a reference lineup, not separate images.

THE 10 CLASSES (left to right order):

ENERGY COLOR RULE — CRITICAL:
NOT every class uses void purple. Purple = void corruption (only Shadowblade, Brawler, Hexer).
Other classes use their own specific accent — see below. DO NOT add purple glow to hands/weapons unless specified.

1. WARBLADE — Heavy warrior. Two-handed greatsword with COLD BLUE (#7BA7BC) rift energy cracks running through the blade — like frozen lightning, not purple. Dark cracked iron armor, black battle cloak torn at edges. COLD BLUE light seeps from every armor crack and joint. No purple anywhere.

2. ELEMENTALIST — Female practical mage. Sleeveless dark teal tunic with leather straps and pouches. One bare arm with elemental tattoos — FIRE ORANGE sigil, ICE BLUE sigil, LIGHTNING YELLOW sigil, visible simultaneously. Holds a multi-colored orb cycling between elements. No void energy at all.

3. SHADOWBLADE — Assassin. All-black tactical leather armor, silver buckles. Deep hood, face in shadow. Dual curved blades. VOID PURPLE smoke from weapons and feet — this is the ONLY class where purple smoke comes from hands/weapons. Glowing void purple eyes.

4. RANGER — Female tactical rift hunter. DARK CHARCOAL/SLATE GREY leather armor — NO green anywhere. Low tactical cowl (not a large hood). Lower face wrapped in dark scarf. Longbow with COLD BLUE RIFT CRACK ETCHINGS along the wood. Arrow tips cold blue shimmer. Crouching ready stance — always alert. Multiple arrow bundles at belt. No fur. Dungeon hunter, not forest archer.

5. RAVAGER — Male berserker. Shirtless, MASSIVE build — the largest human in the roster. Tribal scarification glows BLOOD RED (#8B1A1A) — rage energy, not void. Large notched axe with dried blood stains. Minimal dark iron bracers only. War paint on face, wild dangerous expression. Faint blood-red heat distortion aura around body. No purple, no blue. [IMPORTANT: Previous output missed this character — Ravager must appear between Ranger and Brawler]

6. BRAWLER — Male fighter. Bare-chested athletic build. Dark pants, heavy boots. VOID PURPLE energy crackling around both fists — raw void channeled directly. Tribal tattoos pulse purple. Combat stance, fists raised. (Purple is correct here — he channels void directly.)

7. RONIN — Male wanderer-samurai. Torn grey-green battle robe, partial iron armor. One drawn katana with COLD SILVER-BLUE shimmer along the blade edge — NOT a flame, a sharp crystalline gleam. Subtle, not dramatic. The cursed quality is in the blade's unnatural stillness, not fire.

8. GUNSLINGER — Male. Long worn dark brown duster coat, wide brim hat. Dual revolvers. NO purple glow on hands — his power is in the guns, not his body. Gun barrels have barely-visible COLD SILVER rift etchings (rune engravings). When firing: cold silver-white muzzle flash. Hands are normal. He looks like a gunfighter, not a mage.

9. HEXER — Female curse mage. Floor-length dark crimson tattered robes. Iron lantern with CURSED GREEN-PURPLE flame inside (both colors mixed). Dark wooden staff. Pale sunken-eyed face. Green-black void corruption tendrils at feet. (Two accents are correct for her — decay green + void purple is her identity.)

10. SUMMONER — Female commander of the sacrificed. Layered dark grey-black robes, BONE-WHITE trim on every layer. Iron staff topped with COLD BLUE fracture crystal. Cold blue summoning circle at feet. ONE ARM EXTENDED FORWARD in commanding gesture — pointing and directing, not raised to sky. 3-4 small COLD BLUE SPIRIT WISPS orbit around her body (her minion army always present). Gaunt but sharp commanding face. NO green energy — COLD BLUE only. She looks like a field general, not a passive necromancer.

LAYOUT: Single wide horizontal image. All characters same baseline height. Small label beneath each character with their class name. Dark stone dungeon floor as subtle ground shadow (no full background, just ground contact shadow).
```

---

## BÖLÜM 2 — ACT 1 MOB REFERENCE SHEET

> **Ekle:** rima_style_anchor.png + rima_mob_reference.png (PixelLab_Refs_128 klasöründen)
> **Amaç:** Act 1 düşman tarzını onaylamak. Boyut farkını görmek.

```
[PASTE SECTION 0 STYLE ANCHOR HERE]

Draw a pixel art ENEMY REFERENCE SHEET for Act 1 of a Fractured Epic dungeon action RPG.
All enemies shown together in one image, arranged in a SIZE COMPARISON layout.
Background: dark stone dungeon floor, minimal — just enough to show ground contact.
Each enemy has a small name label beneath it.
Show them left-to-right from SMALLEST to LARGEST.

SIZE PHILOSOPHY: Enemies should feel threatening. Most enemies are the SAME HEIGHT or TALLER than the player.
Only swarm creatures (Fracture Imp) and fragile supports (Relic Caster) are smaller.
Show this scale difference dramatically — the player would look small next to most of these enemies.

ENEMY LIST (left to right, smallest to largest):

1. FRACTURE IMP [48px — swarm] — Small imp creature. All sharp angles — pointy ears, pointy claws, bony tail. Mottled void-purple and dark grey skin. Clearly the smallest thing in the room — feral crouching pose, looks chaotic and disposable.

2. RELIC CASTER [80px — fragile support] — Thin cursed wizard. Torn ceremonial robes in dark grey-purple. Holds a broken relic/orb pulsing cold blue light. Skeletal, stooped, clearly fragile. This is the one that looks easy to kill — by design.

3. SEAM CRAWLER [96px — flanker] — Wide, flat, centipede-like rift creature. LOW TO THE GROUND but very WIDE — takes up significant horizontal space. Only claws and spine ridge visible from above. Cold blue glow along body seams. Not tall but has a threatening horizontal spread.

4. SHARD WALKER [112px — grunt] — Humanoid made of fractured crystal and dark stone, nearly player height. Body has visible gaps where cold blue light bleeds through from within. Arms disproportionately long with crystal shard fingers. Imposing despite the fragmented appearance.

5. CHAIN WARDEN [128px — controller] — Heavy armored guard, SAME HEIGHT as player. Dark rusted iron full plate armor. Multiple thick chains hanging from arms — chains drag on ground. Wide stance, combat ready. This should feel like facing another soldier.

6. PENITENT [128px — bruiser] — Heavy rounded bruiser, SAME HEIGHT as player. Shoulders hunched inward. Dark iron armor with self-flagellation implements embedded (chains through armor, spikes at joints). DARK RED (#8B1A1A) decay aura as a visible halo — NOT purple, dark blood-red. Wide, heavy silhouette.

7. VOID THRALL [128px — splitter] — Tall corrupted humanoid at player height. Dark silhouette with dissolved edges — body partially made of void. Purple void energy tendrils from shoulders and back. Glowing purple eyes. Slightly translucent torso. Unsettlingly thin despite full height.

SCALE NOTE: The Imp (48px) should be less than HALF the height of the Void Thrall / Chain Warden / Penitent (128px). The player character (if shown for reference) would be the same height as the three largest enemies.
```

---

## BÖLÜM 3 — ACT 1 ELİTE + BOSS SHEET

> **Ekle:** rima_style_anchor.png + rima_mob_reference.png

```
[PASTE SECTION 0 STYLE ANCHOR HERE]

Draw a pixel art ELITE & BOSS REFERENCE SHEET for Act 1 of a Fractured Epic dungeon RPG.
Single wide image. Dark dungeon atmosphere. Name labels beneath each.
Show size comparison — these should feel dramatically larger/more threatening than normal enemies.

ELITES:

SIZE PHILOSOPHY FOR ELITES AND BOSS:
Elites are SIGNIFICANTLY larger than the player. Boss fills the screen.
Every enemy here should make the player feel small.

1. THE TWICE-BORN [128px × 2] — A pair of imposing humanoids shown side by side, EACH the same size as the player.
   Twin warriors in matching dark iron armor — one with cold BLUE energy, the other void PURPLE.
   Connected by a faint energy tether between chests.
   They mirror each other's pose — one lunging left, one lunging right.
   Together they take up twice the visual space — overwhelming presence.

2. IRON WARDEN [192px — ELITE] — Massive armored juggernaut, CLEARLY TALLER than player.
   Enormous dark iron plate armor, body completely covered. 
   Pauldrons wider than a player character is tall.
   Thick legs planted wide, chains dragging on ground from wrists.
   Cold blue rift energy crackling from every armor joint.
   This should feel IMMOVABLE — like a siege weapon with legs.

3. FRACTURE KNIGHT [160px — ELITE] — Fast elite skirmisher, noticeably larger than player despite being a skirmisher.
   Fractured dark armor with cold blue cracks — looks like a cracked mirror.
   Mid-dash pose — one foot off ground, sword at full swing arc.
   The contrast of BIG but FAST is the key visual — this shouldn't look sluggish.

BOSS:

4. THE PENITENT SOVEREIGN [256px — BOSS] — Act 1 final boss. Show BOTH phases side by side.
   At 256px this character is TWICE the height of the player — should dominate the frame.

   PHASE 1 "Chained": Massive warrior kneeling under the weight of restraint.
   Dark iron and bronze cracked plate armor. Four enormous rusted chains bind arms and torso — chains are as thick as a person's arm.
   Cold blue rift energy imprisoned in every crack. Glowing eyes behind a cracked battle helm.
   Even kneeling, this character is larger than a standing player.
   The contained power is the horror — imagine when the chains break.

   PHASE 2 "Unbound": Chains SHATTERED — fragments still flying.
   Now standing fully upright — full 256px height on display.
   Cold blue rift energy explosion — corona fills the air around the figure.
   Massive chained warhammer raised (chains now become weapons).
   This should feel like a natural disaster just stood up.
   Label clearly: "PHASE 1 — Chained" / "PHASE 2 — Unbound"
```

---

## BÖLÜM 4 — ACT 2 MOB PREVIEW SHEET

> **Ekle:** rima_style_anchor.png + Bölüm 2 çıktın (Act 1 mob sheetini referans olarak ekle)
> **Amaç:** Act 2'nin görsel tonu Act 1'den ne kadar farklı?

```
[PASTE SECTION 0 STYLE ANCHOR HERE]

Draw a pixel art ENEMY PREVIEW SHEET for Act 2 "Bleeding Wastes" of a Fractured Epic dungeon RPG.
Act 2 visual tone: swamp/decay theme. Rot, corruption, bleeding wounds, festering magic.
Contrast with Act 1 (fractured crystal/cold stone) — Act 2 is organic decay.
Color shift: dark green-brown-burgundy corruption REPLACES the cold blue rift energy from Act 1.
Same pixel art style, same dark base palette — but accent shifts to decay green and blood burgundy.

ENEMY LIST (smallest to largest):

1. FRACTURE IMP VARIANT [32px] — Same shape as Act 1 Imp but covered in festering sores.
   Boils and pustules on skin that glow faint toxic green. Decay imp.

2. ROT PRIEST [64px] — Stooped corrupted healer.
   Rotting ceremonial cloth in dark green-brown. Festering wounds visible on exposed skin.
   Holds a staff topped with a diseased icon. Green decay energy radiating from hands.
   Clearly fragile but radiates an aura of contamination.

3. MIRE STALKER [64px] — Tall thin swamp creature.
   Long limbs, elongated knees that bend backward. Dark green translucent body.
   Bog water and mud dripping from every surface.
   Leaves a swamp puddle wherever it steps (show this as a trail).

4. THORN BRUTE [96px] — Heavy armor covered in bone spikes and thorns.
   Massive build, dark rusted armor with organic bone growths embedded in it.
   Blood burgundy color accents on bone spurs. Slow, intimidating silhouette.

5. DECAY HERALD [112px] — Tall corrupted cultist champion.
   Long tattered robes, face covered in ritual decay markings.
   Decay green energy swirling around both hands. 
   Staff with a pulsing festering crystal on top. This is the "Act 2 caster boss."

BACKGROUND NOTE: Show subtle swamp/wasteland ground — cracked dry earth with dark puddles, bone fragments. Contrast with Act 1's stone floor.
```

---

## BÖLÜM 5 — BOSS LINEUP (Tüm Bosslar, Boyut Karşılaştırması)

> **Ekle:** rima_style_anchor.png + Bölüm 3 çıktın (Act 1 boss referans olarak)

```
[PASTE SECTION 0 STYLE ANCHOR HERE]

Draw a pixel art BOSS LINEUP showing all 4 major bosses of a Fractured Epic dungeon RPG.
Single wide image. Dramatic lighting — deep shadow with each boss slightly glowing.
Size comparison is CRITICAL — show escalating scale left to right.
Name labels beneath each. Dark atmospheric background (no full scene, just atmospheric fog/shadow).

BOSS LINEUP (left to right, smallest to largest):

SCALE NOTE: These bosses escalate dramatically. Sovereign is twice player height. Final Boss is nearly three times.
Show them left to right with dramatic size escalation — each one noticeably larger than the last.

1. THE PENITENT SOVEREIGN [256px — Act 1 Boss]
   Phase 2 pose: chains broken, standing at FULL HEIGHT — twice a player character.
   Dark cracked iron armor, cold blue rift energy corona exploding outward.
   Chained warhammer raised. This is the smallest boss — and already overwhelming.

2. THE ECHO TWIN [256px — Act 2 Boss]
   A single figure that appears to be TEARING IN HALF — two silhouettes partially separated.
   Left half: cold blue warrior energy (melee aspect).
   Right half: void purple caster energy (magic aspect).
   The two halves face different directions — one looking left, one right.
   Mid-transformation pose. Same height as Sovereign but more dramatic visual.

3. THE FRACTURE SOVEREIGN [320px — Act 3 Boss]
   Not a humanoid — this is a WOUND IN REALITY given form.
   Massive floating entity. A rift/fracture at its center like an open eye.
   Body made of fractured void crystal shards orbiting a core of cold blue/void purple light.
   Stone arena fragments caught in its gravitational pull, circling it.
   Taller than Act 1 Boss — the scale difference should be visible.
   This does not look like a person. It looks like the world breaking.

4. FINAL BOSS [320px+ — Endgame]
   The player's own shadow — dark mirror of the player character.
   Silhouette recognizable as a warrior, but consumed by void corruption.
   Taller than the player by 2.5x — towering, warped.
   All rift energy turned black-void. Fractured reality tears floating around the figure.
   Player's weapons visible but shattered and corrupted.
   Cold eyes in the darkness. This is "what happens if you lose yourself to the fracture."

ATMOSPHERE: Deep black background, each boss has a unique colored halo/glow:
Sovereign = cold blue | Echo Twin = half-blue half-purple | Fracture Sovereign = white-blue | Final Boss = black-purple
```

---

## BÖLÜM 6 — CLASS DETAIL SHEETS (FAZ 1-2 Class'ları)

> Her class için ayrı bir sheet. Aşağıdaki template'i kullan, sadece karakter bilgilerini değiştir.
> **Ekle:** İlgili class'ın CLASS_CONCEPTS görseli (pixel art versiyonu) + warblade.png (stil anchor)

### 6A — WARBLADE DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/warblade.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for WARBLADE, the heavy warrior class of a Fractured Epic action RPG.
Single image showing 4 POSES arranged in a 2x2 grid. Each pose ~128x160px. Name of pose labeled below.
Consistent design across all 4 poses — same character, same armor.

CHARACTER DESIGN:
- Heavy dark cracked iron armor, black battle cloak (torn at edges)
- Two-handed greatsword — blade is dark iron with cold blue rift energy cracks running through it
- Cold blue energy seeps from armor joint cracks  
- Battle-worn: dented armor, scarred exposed skin at neck/hands
- Heroic build, not exaggerated

POSE 1 — "IDLE STANCE": Weight on back foot. Greatsword tip resting on ground, both hands on hilt. Watching. Ready. Cold blue glow subtle.

POSE 2 — "IRON CHARGE": Full sprint dash — one foot off ground, body leaning forward. Sword held low behind, about to swing. Energy trail behind blade.

POSE 3 — "IRON COMBO (3rd hit)": Overhead cleave at peak — sword above head, arms fully extended upward, about to come down. Maximum energy release, cold blue corona.

POSE 4 — "BLADESTORM [V BURST]": Spinning attack — body mid-rotation, sword extended in wide arc. COLD BLUE energy explosion (NOT purple) around entire figure. Armor cracks blazing cold blue. This is the ultimate ability — maximum visual impact.
```

### 6B — SHADOWBLADE DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/shadowblade.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for SHADOWBLADE, the assassin class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- All-black tactical leather armor, silver buckles, padded joints
- Deep hood, face always in shadow — only void PURPLE glowing eyes visible
- Dual curved short blades, void purple smoke trailing from blades
- Slim but not fragile — athletic assassin build
- Void purple shadow tendrils at feet (always)

POSE 1 — "SHADOW IDLE": Crouching slightly, weight forward. Both blades drawn at sides. Purple smoke around feet. Watching.

POSE 2 — "VOID STEP DASH": Mid-teleport — body dissolving into purple void smoke on one side, reforming on the other. Ghost trail of void smoke.

POSE 3 — "SEVER [SIGNATURE ATTACK]": Single devastating slash — blade extended after a clean horizontal cut. Purple after-image of the blade visible. Clean, surgical, lethal.

POSE 4 — "RIFT SCAR [V BURST]": Both blades crossed in front, then exploding outward — void purple rift cracks opening in the ground and air around the character. Maximum shadow energy release.
```

### 6C — ELEMENTALIST DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/elementalist.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for ELEMENTALIST, the battlemage class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Dark teal practical tunic, leather straps and equipment pouches
- One bare arm covered in elemental tattoos (fire sigil, frost sigil, lightning sigil — small, readable)
- Rift energy orb she holds changes color based on active element
- Red-brown hair tied back, practical combat appearance
- Not robes — this is a fighter who also uses magic

POSE 1 — "IDLE (FROST STATE)": Orb glowing cold blue-white. One hand extended, small frost crystal forming.

POSE 2 — "RIFT BOLT CAST (FIRE STATE)": Arm extended forward, launching a rift bolt. Orb and tattoos glowing orange-red. Body slightly recoiling from the cast.

POSE 3 — "LIGHTBREAK TRIGGER": Both arms raised, orb at chest. The moment fire+frost collide — white-blue explosion at center of chest/orb. Elemental convergence visual — all tattoos lit simultaneously.

POSE 4 — "INFERNO [V BURST]": Arms wide, orb shattered into flames above. Fire raining down around the character from above. This is the arena fire storm. Maximum elemental release.
```

### 6D — RANGER DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/ranger.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for RANGER, the precision archer class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Dark brown forest leather armor, fur-trimmed pauldrons
- Forest green hood (torn at edges), green-brown travelling cloak
- Longbow — dark wood, simple, practical. No glowing etchings. The magic is in the arrows, not the bow.
- Female, athletic, grounded stance — always looks like she knows exactly where she is
- Quiver of arrows with COLD BLUE rift crystal tips — subtle shimmer, not blazing glow
- NO purple on this character. Her rift connection is cold blue, practical, not corrupted.

POSE 1 — "TRACKING IDLE": Bow in hand, arrow nocked but not drawn. Head slightly turned, eyes scanning. Cold blue arrow tip barely glowing. Calm, professional. Looks like a hunter.

POSE 2 — "RIFT SHOT": Full draw — bow pulled back to cheek, eye focused. COLD BLUE rift energy condensing at arrowhead — sharp, crystalline, not a flame. Tension maximum.

POSE 3 — "DASH-SHOT": Mid-roll to the side, firing at an angle while in motion. Arrow already flying with cold blue trail behind it. Body in motion blur. Practical, not flashy.

POSE 4 — "VOID BARRAGE [V BURST]": Rapid-fire pose — multiple cold blue rift arrows in the air simultaneously (5-6 arrows). Quiver glowing cold blue. The quantity of arrows is the visual impact, not glow intensity.
```

### 6E — RAVAGER DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/ravager.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for RAVAGER, the berserker class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Shirtless, massive muscular build — biggest of all classes
- Tribal scarification across chest and arms, scars glow faint blood-red when enraged
- Large brutal single-headed axe, blade notched and blood-stained
- Minimal dark iron arm bracers and shin guards — mostly exposed skin
- Blood-red rage energy as heat shimmer/aura around body at high tension
- War paint on face, wild expression

POSE 1 — "LOW RAGE IDLE": Axe on shoulder, hunched forward, breathing heavy. Barely contained. Eyes unfocused, dangerous.

POSE 2 — "AXE OVERHEAD SMASH": Axe raised two-handed above head, about to come down with full force. Rage aura at maximum — red heat corona around entire figure. Ground crack where he'll land.

POSE 3 — "BLOOD FRENZY": Mid-spin swing — axe blurring in a horizontal arc. Multiple hit lines visible. Body fully committed, no defense.

POSE 4 — "V BURST — CARNAGE": Both arms spread wide, axe raised, screaming. Blood-red void energy explodes outward from the body. This is pure uncontrolled rage — maximum visual chaos.
```

### 6F — BRAWLER DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/brawler.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for BRAWLER, the void-fist fighter class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Bare-chested, athletic build — not as massive as Ravager, fast fighter
- Dark tactical pants, heavy combat boots
- No weapons — void purple energy crackling around both fists like energy gauntlets
- Tribal tattoos across torso and arms that pulse with void purple when energy charges
- Low, grounded fighting stance — weight always balanced, ready to move

POSE 1 — "FIGHT STANCE IDLE": Classic boxing guard. Both fists raised, chin down, eyes forward. Void energy crackling at knuckles. Calm focus.

POSE 2 — "VOID JAB COMBO (3rd hit)": Extended straight punch at peak — arm fully extended, void energy explosion at point of impact. Body rotated through the punch.

POSE 3 — "VOID STEP DASH": Mid-movement between positions — one fist trailing void purple smoke, body in motion. Speed blur.

POSE 4 — "V BURST — VOID ERUPTION": Both fists slammed together above head, then a massive void purple explosion expands outward in a sphere. All tattoos glowing at full intensity.
```

### 6G — RONIN DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/ronin.png as primary style reference (if available, else use NEW_CLASS_CONCEPTS/ronin.png).

Draw a pixel art CHARACTER DETAIL SHEET for RONIN, the wandering swordsman class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Torn dark grey-green battle robe (kimono style), torn at hem and sleeves
- Partial iron armor on shoulders and lower legs only — deliberately exposed
- Two katanas: one sheathed on back, one at hip — primary drawn in combat
- Drawn blade has a COLD SILVER-BLUE shimmer along the cutting edge — not a flame, not glow — a crystalline rift quality. Like the blade has been touched by something cold and unnatural. Subtle.
- Worn cloth wrapped around hands and forearms
- Lean, precise build — economy of movement, nothing wasted
- NO purple. His rift connection is cold and quiet, not volatile.

POSE 1 — "RONIN IDLE": One hand resting on sheathed katana. Weight on one hip, slightly turned. Watchful, detached expression. Barely any energy visible. Stillness is the threat.

POSE 2 — "IAIJUTSU DRAW": Mid-draw — blade leaving sheath at high speed. Cold silver-blue rift shimmer ignites along the blade edge in the instant of drawing. One fluid movement, no wasted motion.

POSE 3 — "RIFT SLASH FOLLOW-THROUGH": After a horizontal cut — blade extended, cold silver-blue after-image of the cut arc visible in the air. The energy is in the cut itself, not around the body. Clean, lethal, precise.

POSE 4 — "V BURST — RIFT BLADE STORM": Multiple cold silver-blue blade after-images surrounding the character — as if he cut in 8 directions simultaneously. Cold blue-white rift tears at each cut point. Calm expression in the center. The cold color palette makes this feel controlled, not chaotic.
```

### 6H — GUNSLINGER DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/gunslinger.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for GUNSLINGER, the rift-pistol duelist class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Long worn dark brown duster coat, dust and road-worn
- Wide-brimmed hat, slightly tilted, practical not stylish
- Dual revolvers — dark iron, heavy, mechanical. Barely-visible COLD SILVER rift rune etchings on the barrels (very subtle — you have to look). The enhancement is in the gun, not the shooter.
- Dark vest, practical clothing — no decoration, no ornamentation
- Lean build, sideways duelist stance — half profile to the target
- NO glowing hands. NO purple energy at palms. He is a gunfighter, not a mage. The supernatural is in the ammunition, invisible until fired.
- When guns fire: COLD SILVER-WHITE muzzle flash (slightly unusual color for gunpowder — the only hint of rift enhancement)

POSE 1 — "GUNFIGHTER IDLE": Both guns holstered. Arms loose at sides, hands near holsters. Eyes flat and calculating. He looks completely normal — that's the point. A gunslinger doesn't need to look magical.

POSE 2 — "SINGLE SHOT": One gun drawn, arm extended, firing. Cold silver-white muzzle flash at the barrel. Minimal recoil — practiced. Body barely moves. The shot is already gone.

POSE 3 — "DOUBLE DRAW": Both guns drawn simultaneously — both firing. Two cold silver muzzle flashes. Coat pushed back slightly from stance. Spent brass casings in the air. No trails, no glow — just the flash.

POSE 4 — "V BURST — RAPID FIRE": Rapid firing both guns — multiple shots, multiple muzzle flashes layered. The chaos is in the speed and volume of fire, NOT magical trails. Coat swept out from movement. This is a human weapon pushed to its limit, not a magic spell.
```

### 6I — HEXER DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/hexer.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for HEXER, the curse mage class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Floor-length dark crimson robes, tattered hem dragging on ground
- Iron lantern in one hand — cursed flame inside is green-purple, not natural fire
- Dark wooden staff with an iron tip, held in other hand
- Pale skin, sunken dark-circled eyes — not undead, just deeply wrong
- Green-black void corruption tendrils rising from ground at feet (always present)
- Slow, deliberate movement — she never rushes

POSE 1 — "HEXER IDLE": Staff planted, lantern raised slightly. Corruption tendrils swirling at feet. Expression blank, distant. Something is wrong with the air around her.

POSE 2 — "CURSE CAST": Lantern raised high, other hand extended forward — a hex projectile (dark green-purple spiral) launching from fingers. Body slightly leaning into the cast.

POSE 3 — "HEXBLAST (7+ stacks)": Both hands raised, lantern swinging — massive curse explosion around her. Green-black void energy expanding outward in a ring. Robes billowing in the blast.

POSE 4 — "V BURST — DAMNATION FIELD": Arms spread wide, lantern at full blaze. Massive field of corruption covering the ground — green-black void energy rising like smoke from a large area. She stands at the center, calm.
```

### 6J — SUMMONER DETAIL

```
[PASTE SECTION 0 STYLE ANCHOR HERE]
ATTACH: TASARIM/CLASS_CONCEPTS/summoner.png as primary style reference.

Draw a pixel art CHARACTER DETAIL SHEET for SUMMONER, the commander-summoner class of a Fractured Epic action RPG.
Single image showing 4 POSES in a 2x2 grid. Each ~128x160px.

CHARACTER DESIGN:
- Rich purple robes with gold trim and armored shoulderpiece — commander, not necromancer
- Ornate golden scepter with purple crystal orb head — this is her core instrument (NOT a staff, NOT cold blue)
- Brown hair, purple gemstone tiara crown — regal identity
- Calm authoritative expression — she commands, does not plead
- Lean, tall silhouette — presence through posture, not bulk
- Purple and gold are her colors — NOT cold blue (that's Elementalist), NOT grey-black (that's old design)
- NOTE: No large runic circles at hands — summoning effects are Unity VFX, not in the reference sprite

POSE 1 — "SUMMONER IDLE": Scepter held at side, other hand relaxed open. Calm, surveying. Commander at rest.

POSE 2 — "SUMMON CALL": Scepter raised, other arm extended forward in commanding gesture — open palm directing. No magical circle visible.

POSE 3 — "COMMAND GESTURE": Both arms directing her summons — one pointing forward like a general, scepter angled. She is the conductor.

POSE 4 — "V BURST — RIFT LEGION": Arms spread wide, scepter blazing purple. Authoritative stance — maximum presence. The power is in her posture, not particle effects.
```

---

## BÖLÜM 7 — ENVIRONMENT PROP SHEET

> **Ekle:** rima_style_anchor.png + rima_mob_reference.png (sahne referansı için)
> **Amaç:** Oda elementlerinin görsel tonu.

```
[PASTE SECTION 0 STYLE ANCHOR HERE]

Draw a pixel art ENVIRONMENT PROP REFERENCE SHEET for a Fractured Epic dungeon.
Isometric perspective — everything viewed from a 45-degree angle from above-right.
Show all props isolated with transparent background, arranged in a grid layout.
Consistent style with the isometric dungeon in the reference image.

ACT 1 PROPS (label each):

FLOOR TILES (show 3x3 tile arrangement):
- Dark cold stone dungeon floor, hairline cold blue rift cracks running through some tiles

WALL TILES (show a L-shaped corner section):
- Two visible faces (isometric: front face + side face)
- Dark grey-brown stone, mortar crumbling, torch sconce on wall
- Cold blue rift crack running down one wall

PROPS (each isolated):
- RIFT CRYSTAL CLUSTER: Jagged cold blue crystals growing from floor, 32x32 isometric
- BONE PILE: Scattered bones, some still with dark iron armor fragments, 32x32
- RUSTED TORCH: Wall mount with cold blue (not orange) rift flame, 16x32
- BROKEN PILLAR: Half-height, dark stone, crumbled top, 32x48
- SEALED DOOR: Heavy iron door with rift crystal lock mechanism, 48x48
- CHEST: Battle-worn iron chest, rift crystal lock, 32x24
- RIFT CRACK: Floor crack that glows cold blue from below, ground hazard marker
- SOUL ORB: Collectible — small floating cold blue orb, 16x16

COLOR ANCHOR: All Act 1 props use dark grey-brown stone + cold blue rift accents. No warm colors. Cold, dead dungeon.
```

---

## BÖLÜM 8 — ANTI-DRIFT PROMPT (Gemini sonucu yanlış gelince)

> Gemini çıktısı tarzdan kaydıysa, bu düzeltme ekini kullan. Önceki promptun üstüne yapıştır.

```
STYLE CORRECTION — the previous output drifted from the reference. Please redo with these constraints:

WHAT WENT WRONG (check what applies):
[ ] Too bright — the palette must be DARK. Base colors: near-black, dark iron grey, dark leather brown, dark forest green. NO bright colors except the energy accents (cold blue / void purple).
[ ] Too ornate — remove all decorative flourishes. No skull motifs, no elaborate scrollwork, no baroque armor. These are warriors who fight, not parade in armor.
[ ] Energy color wrong — the ONLY allowed energy/magic colors are: cold blue (#7BA7BC range) and void purple (#6B3D9E range). Remove fire-red, bright green, gold, orange.
[ ] Not pixel art — add "chunky visible pixels, black 1px outline, no anti-aliasing, limited palette sprite art" and redo.
[ ] Wrong proportions — heroic build but realistic scale. Remove any exaggeration (giant hands, tiny waist, enormous shoulders). Reference the attached warblade.png proportions exactly.
[ ] Background visible — transparent background ONLY. Remove all backgrounds, environment, scenery. Sprite only.

Redo the image with these corrections applied. Keep everything else from the previous prompt.
```

---

## NOTLAR

**Gemini'de en iyi sonuç için:**
1. Gemini 2.0 Flash veya Gemini 1.5 Pro kullan (Imagen 3 değil — pixel art anlayışı daha zayıf)
2. Her promptta mutlaka 2-3 referans görsel ekle (text tek başına yetmez)
3. Çıktı kabul edilirse: "Save this style, I'll use it as reference for the next prompt" yaz → tutarlılık artar
4. Kabul edilmezse: BÖLÜM 8'deki anti-drift promptu uygula

**PixelLab'a geçiş kararı:**
Gemini'den onaylanan her karakter → STYLE_BIBLE.md'deki template ile PixelLab Standard'a gir.
Seed al → Pro'ya geç → Animate.
