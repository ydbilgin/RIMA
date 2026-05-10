# RIMA PixelLab Animation Prompts -- 10 Class
Version: v0.1 (2026-05-06)
Canvas: 252x252 | Tool: animate-with-text-v3 | Frames: 8 max per call (interpolate for more)
Workflow: 3-Segment (PEAK first) for attacks | Direct Animate for run

---

## Uretim Sirasi
1. Run South (silahsiz) -> animate-with-text-v3, 6 frame
2. LMB PEAK frame -> Create Image Pro, single frame
3. START->PEAK animate (4 frame) -> PEAK->END animate (4 frame)
4. Silah ekle (Edit Image Pro, ayri adim)
5. Diger yonler: East / North / West ayni workflow

## Yonlendirme Notu
- south.png (full front) -> Unity EAST slot
- east.png (sag profil) -> Unity NORTH slot
- north.png (full arka) -> Unity WEST slot
- west.png (sol profil) -> Unity SOUTH slot

## Genel Stil Referansi (tum classlarda gecerli)
- "Fractured Epic" ton: dramatik, canli, yuksek kontrast -- grimdark degil
- Hades vizuel vibe: parlak silüetler, void karanligi karsisinda class accent rengi
- Arka plan: koyu soğuk, muted gri-mavi / siyah-mor
- Her karakterde bir yerde mavi-mor crack light (Rift energy)
- Silüetler guçlu ve okunabilir
- MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally.
- muted desaturated palette, weathered field-worn

---

## WARBLADE -- Erkek -- Cold blue #7BA7BC

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Prompt:**
> A heavily armored male warrior running fast toward the camera, alternating arms and legs as they lift their knees high, full front view, no weapon, heavy plate armor with cold blue #7BA7BC cracked light on chest plate and pauldrons, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (3. vuruş -- Omuz itisi, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak impact moment)
**Not:** En kritik an -- sol omuz tam vitese girmiş, yumruk-itme forward extension en uzak noktada
**Prompt:**
> A heavily armored male warrior at the peak impact moment of a powerful shoulder-charge thrust, body fully extended forward with left shoulder driving into the strike, arms locked in final push position, heavy two-handed great sword lowered after the blow (weapon held in trailing hand), full front view, cold blue #7BA7BC glowing cracks on chest armor and sword edge blazing at peak energy, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** idle/ready pose -- warrior standing upright, two-handed greatsword raised to right shoulder, weight back
**Target:** PEAK frame (shoulder thrust, full forward extension)
**Prompt:**
> A heavily armored male warrior transitioning from an upright ready stance with a great sword raised at right shoulder, shifting weight forward into a powerful shoulder-charge thrust, body driving forward, sword arcing downward as shoulder leads the strike, full front view, cold blue #7BA7BC armor crack light intensifying through the motion, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (full forward extension shoulder thrust)
**Target:** recovery pose -- warrior pulling back to neutral guard, weight re-centered
**Prompt:**
> A heavily armored male warrior recovering from a fully extended shoulder-charge thrust, pulling body weight back to center, sword rising back to guard position, momentum dissipating, stance resettling into a balanced guard, full front view, cold blue #7BA7BC armor crack light fading back to resting glow, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## ELEMENTALIST -- Kadin -- Aktif elemente gore (Fire / Frost / Lightning)

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz versiyon -- eller bos, sonradan staff eklenecek
**Prompt:**
> A slender female mage running fast toward the camera, alternating arms and legs as they lift their knees, full front view, no weapon, flowing robes with arcane rune trim, muted desaturated palette, weathered field-worn, subtle elemental energy flickering at fingertips (no large VFX), dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (3. bolt -- Commit-Beat, element bolt fully released)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak release moment)
**Not:** Eller tam uzatilmis, bolt firlatma aninin en ileri noktasi -- VFX baked olmayacak, sadece elin pozisyonu ve vucudun acilmasi
**Prompt:**
> A slender female mage at the peak moment of releasing an elemental bolt, both arms fully extended forward palms open, body leaning into the cast, weight on front foot, robes flowing back from the force, fingertips with a concentrated point of energy (small tight glow, no large explosion), full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent on robe trim, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- mage upright, hands raised to chest level in casting position
**Target:** PEAK frame (arms fully extended, bolt release)
**Prompt:**
> A slender female mage transitioning from an upright casting stance with hands raised to chest level, arms extending forward as she channels and releases an elemental bolt, body weight shifting onto front foot, robes beginning to blow back, hands pushing forward to full extension, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (arms fully extended, bolt released)
**Target:** recovery pose -- mage pulling arms back to ready casting stance
**Prompt:**
> A slender female mage recovering from a fully extended elemental bolt release, arms pulling back from extended position toward chest, body weight re-centering, robes settling, returning to a neutral casting ready stance, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## SHADOWBLADE -- Erkek -- Void purple

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- cerceve yoksa hancer icin ayri Edit Image Pro adimi
**Prompt:**
> A lean male assassin running fast toward the camera, alternating arms and legs as they lift their knees, low crouched aggressive sprint posture, full front view, no weapons, dark leather armor with void purple #6A0DAD accent trim and faint smoke wisps trailing from wrists, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (3. vuruş -- Cift hancer capraz kesim, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak impact -- cross-slash fully landed)
**Not:** Iki hancer birbirinden uzak, capraz kesim tamamlanmis, vurucu pozisyon
**Prompt:**
> A lean male assassin at the peak impact moment of a dual reverse-grip dagger cross-slash, both arms fully spread in an X pattern after cutting across, daggers pointing outward, body low and aggressive in a deep forward lunge, full front view, void purple #6A0DAD glowing smoke trailing from dagger blades and eyes, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- assassin crouched low, both daggers held reverse-grip close to body
**Target:** PEAK frame (arms spread X pattern after cross-slash)
**Prompt:**
> A lean male assassin transitioning from a low crouched ready stance with reverse-grip daggers held close to body, explosively cross-slashing both arms outward in an X pattern, body surging forward into a deep lunge, void purple smoke intensifying as blades accelerate, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (arms spread X after cross-slash, deep lunge)
**Target:** recovery pose -- assassin pulling back to low crouched guard
**Prompt:**
> A lean male assassin recovering from a spread-arm cross-slash position, pulling both reverse-grip daggers back toward body, lunge retracting, settling back into a low crouched guard stance, void purple smoke wisps fading, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## RANGER -- Kadin -- Cold blue #7BA7BC (runtime only, karakterde minimal)

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- yay ayri Edit Image Pro ile eklenecek
**Prompt:**
> A nimble female ranger running fast toward the camera, alternating arms and legs as they lift their knees, upright agile sprint posture, full front view, no weapon, light leather-and-cloth ranger armor with minimal trim, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (Sarjli ok cekimi, Commit-Beat -- tam geri cekis aninda)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak draw -- bow at full draw, moment before release)
**Not:** Yay tam geri cekilmis, vucut gerilmis, nisan alinmis -- VFX baked yok
**Prompt:**
> A nimble female ranger at the peak draw moment of a charged arrow shot, bow arm fully extended forward, draw arm pulled back to cheek in full draw position, body twisted and tense, full front view, no weapon visible yet (weapon added separately), intense focus posture, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light on bow arm, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- ranger upright, arms relaxed at sides
**Target:** PEAK frame (full draw position, bow arm extended, draw arm to cheek)
**Prompt:**
> A nimble female ranger transitioning from a relaxed upright stance, raising both arms into a bow-draw position, bow arm extending forward while draw arm pulls back toward cheek, body twisting into shooting stance, building tension through the draw, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (full draw position)
**Target:** recovery pose -- ranger lowering arms back to relaxed ready stance
**Prompt:**
> A nimble female ranger recovering after releasing an arrow at full draw, bow arm recoiling slightly from the release, draw arm snapping forward, body tension releasing, arms lowering back toward a relaxed ready stance, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## RAVAGER -- Erkek -- Blood red #8B1A1A

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- balta ayri Edit Image Pro ile eklenecek
**Prompt:**
> A massive brutish male berserker running fast toward the camera, alternating arms and legs as they lift their knees with heavy thundering strides, full front view, no weapon, heavy scarred berserk armor with blood red #8B1A1A rage aura wisps at shoulders and tattoo markings glowing faintly, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (3. vuruş -- Baltayi yere carptirma, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak impact -- axe slammed into ground)
**Not:** Beden tamamen one egik, omuzlar one cikik, yere vurma aninin en agir noktasi
**Prompt:**
> A massive brutish male berserker at the peak impact moment of slamming a great axe into the ground, body bent fully forward, both arms driving downward, shoulders hunched forward, deep forward lunge with weight fully committed into the strike, full front view, blood red #8B1A1A rage aura blazing at shoulders, tattoo markings fully illuminated, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- berserker upright, axe raised overhead in two hands
**Target:** PEAK frame (body bent forward, axe driven into ground)
**Prompt:**
> A massive brutish male berserker transitioning from an upright stance with great axe raised overhead in both hands, explosively driving the axe downward in a massive overhead slam, body bending fully forward as weight commits into the strike, rage aura intensifying through the motion, full front view, blood red #8B1A1A aura flaring, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (body bent forward, axe at ground level)
**Target:** recovery pose -- berserker slowly straightening back up, axe lifting
**Prompt:**
> A massive brutish male berserker slowly recovering from a full-force axe ground slam, body straightening back up from the deep forward bend, arms lifting the axe back up from the ground, momentum dissipating into a heavy recovery, full front view, blood red #8B1A1A rage aura settling back to a resting glow, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## RONIN -- Erkek -- Cold silver-blue

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- katana ayri Edit Image Pro ile eklenecek
**Prompt:**
> A lean disciplined male ronin running fast toward the camera, alternating arms and legs as they lift their knees with controlled precise strides, full front view, no weapon, worn dark kimono-style light armor with cold silver-blue shimmer on hem and collar trim, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (Iaido kesiş -- katana tam cekilis/kesim ani, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak -- draw-and-cut fully extended)
**Not:** Katana tam cekilis ile yatay kesim, kol tam uzatilmis, beden one rotate
**Prompt:**
> A lean disciplined male ronin at the peak moment of an iaido draw-cut, body pivoted forward, sword arm fully extended in a horizontal slash, katana blade at full extension with cold silver-blue shimmer along the edge, the other hand releasing the scabbard, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- ronin upright, hand on scabbard hilt, blade sheathed
**Target:** PEAK frame (body pivoted, arm extended, draw-cut fully extended)
**Prompt:**
> A lean disciplined male ronin transitioning from an upright stance with hand resting on sheathed katana hilt, explosively drawing and cutting in a single iaido motion, body pivoting forward as the blade leaves the scabbard and extends into a full horizontal slash, cold silver-blue shimmer appearing on blade edge as it accelerates, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (body pivoted, arm extended after draw-cut)
**Target:** recovery pose -- ronin returning to upright, blade held ready or returning to sheath
**Prompt:**
> A lean disciplined male ronin recovering from a fully extended iaido draw-cut, body pivoting back to center, sword arm pulling the blade back to a held-ready guard position, stance re-centering with controlled discipline, cold silver-blue shimmer settling on blade edge, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## GUNSLINGER -- Kadin -- Cold silver (minimal)

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- tabancalar ayri Edit Image Pro ile eklenecek
**Prompt:**
> A quick agile female gunslinger running fast toward the camera, alternating arms and legs as they lift their knees with a low-forward sprint lean, full front view, no weapons, rift-tech coat and light combat gear with minimal cold silver trim on holsters and collar, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (Slide->ates, cift tabanca atis ani, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak -- both guns at full aim extension after slide)
**Not:** Slide sonrasi her iki el uzatilmis, ates noktasi -- enerji VFX yok, sadece kol ve el pozisyonu
**Prompt:**
> A quick agile female gunslinger at the peak firing moment after a combat slide, body low and forward-leaning after the slide, both arms fully extended forward aiming dual pistols, hands steady at the moment of trigger pull, rift-tech engraving glowing faintly cold silver at gun barrels (no muzzle flash VFX), full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- gunslinger upright, hands near holsters
**Target:** PEAK frame (body low after slide, both arms extended aiming)
**Prompt:**
> A quick agile female gunslinger transitioning from an upright ready stance with hands near holsters, drawing both pistols and dropping into a combat slide, body lowering and surging forward as both arms extend into a forward aim, rift-tech engravings activating along barrels, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (body low after slide, both arms extended)
**Target:** recovery pose -- gunslinger rising back to upright guard, arms lowering
**Prompt:**
> A quick agile female gunslinger recovering from a low extended dual-pistol aim after a combat slide, body rising back to an upright mobile stance, arms pulling back from extended aim toward a mid-guard position, rift-tech glow settling, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## BRAWLER -- Erkek -- Amber #FF8800

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- Brawler zaten silahsiz, ama knuckle-wrap detaylari onemli
**Prompt:**
> A stocky powerful male brawler running fast toward the camera, alternating arms and legs as they lift their knees with short explosive strides, fists pumping, full front view, bare-knuckle fighter with wrapped hands and minimal chest armor, amber #FF8800 crack glow on left wrist knuckle wrap and faint aura at fists, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (Hizli combo son vurus -- Uppercut veya hayalet yumruk, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak -- right hook or uppercut at full extension)
**Not:** Vurma kolu tam uzatilmis, vucudun donmesi tamamlanmis, amber knuckle tam parlak
**Prompt:**
> A stocky powerful male brawler at the peak impact moment of a right hook, punching arm fully extended, body pivoted fully into the blow, weight transferred onto front foot, knuckles at the end of their arc, full front view, amber #FF8800 crack glow blazing on left wrist and knuckles at peak impact, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- brawler in low boxing guard, fists raised
**Target:** PEAK frame (punching arm fully extended, body pivoted)
**Prompt:**
> A stocky powerful male brawler transitioning from a low boxing guard stance with both fists raised, loading a right hook by rotating hips back and pulling right arm back, then explosively pivoting hips forward and extending the punch to full reach, amber crack glow brightening at knuckles through the motion, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (punching arm fully extended, body pivoted)
**Target:** recovery pose -- brawler pulling arm back to guard, resetting stance
**Prompt:**
> A stocky powerful male brawler recovering from a fully extended right hook, pulling punching arm back from extension toward guard position, body pivoting back to neutral balanced boxing stance, amber crack glow fading back to resting amber pulse, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## SUMMONER -- Kadin -- Cold blue (kristal, cagirma daireleri)

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- kristal staff ayri Edit Image Pro ile eklenecek
**Prompt:**
> A graceful female summoner running fast toward the camera, alternating arms and legs as they lift their knees, upright flowing run, full front view, no weapon, layered arcane robes with cold blue crystal fragments embedded in shoulder armor, faint summoning circle runes glowing cold blue at hem, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (Minyon cagirma komut jesti, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak -- command gesture at full extension)
**Not:** Her iki el one uzatilmis, parmaklar acik, cagirma jestinin en ileri noktasi -- VFX yok
**Prompt:**
> A graceful female summoner at the peak moment of a summoning command gesture, both arms fully extended forward with fingers spread in a commanding open-palm gesture, body leaning forward with authority, cold blue crystal fragments on armor blazing, faint summoning circle rune light at fingertips (small point of light, no large VFX), full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- summoner upright, hands at chest level in gathering pose
**Target:** PEAK frame (both arms fully extended forward in command gesture)
**Prompt:**
> A graceful female summoner transitioning from an upright stance with hands gathered at chest level, extending both arms forward in a commanding summoning gesture, cold blue crystal light intensifying on armor as both arms push out to full extension, body leaning into the command, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (both arms fully extended command gesture)
**Target:** recovery pose -- summoner pulling arms back to neutral gathering stance
**Prompt:**
> A graceful female summoner recovering from a fully extended summoning command gesture, arms pulling back from extension to a neutral gathered stance at chest level, cold blue crystal glow settling, body re-centering, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## HEXER -- Kadin -- Cursed green #3A8A4A + void purple #6A0DAD

### Run Animasyonu
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 6 | **Yon:** South (front-facing, full front view)
**Not:** Silahsiz -- fener ve rune-etched eldivenler karakterde, ayri silah ekleme yok
**Prompt:**
> A wiry sinister female hexer running fast toward the camera, alternating arms and legs as they lift their knees with an unsettling fluid gait, full front view, no held weapon, dark tattered robes with cursed green #3A8A4A rune stitching and void purple #6A0DAD shadow wisps trailing from rune-etched bare hands, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light accent, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera, the steep overhead angle hides the eyes naturally

### LMB Attack -- PEAK Frame (Lanet yayma, tek hedef curse, Commit-Beat)
**Tool:** Create Image Pro
**Canvas:** 252x252 | **Frame:** 1 (peak -- cursing hand thrust at full extension)
**Not:** Tek el ileri uzatilmis parmaklar kenetlenmis, lanet jesti -- rune parlamasi parmak uclarinda
**Prompt:**
> A wiry sinister female hexer at the peak moment of casting a curse, one arm fully extended forward with fingers clutched into a claw-grip hex gesture, the other hand pulling back near the chest amplifying the curse, body leaning into the cast, rune-etched hand blazing with cursed green #3A8A4A and void purple #6A0DAD at fingertips (tight contained glow, no large explosion), full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, strong readable silhouette, rift energy blue-purple crack light, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- START->PEAK
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** ready stance -- hexer upright, arms loosely at sides, rune hands faintly glowing
**Target:** PEAK frame (one arm extended claw-hex gesture, other arm pulled back)
**Prompt:**
> A wiry sinister female hexer transitioning from a loose upright stance with faintly glowing rune hands at sides, raising one arm forward into an extending claw-grip hex gesture while pulling the other arm back toward chest, cursed green and void purple rune glow intensifying through the motion, body leaning into the cast, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

### LMB Attack -- PEAK->END
**Tool:** Animate with Text NEW (v3)
**Canvas:** 252x252 | **Frames:** 4
**Input:** PEAK frame (one arm extended claw hex, other arm pulled back)
**Target:** recovery pose -- hexer letting both arms lower back to loose resting position
**Prompt:**
> A wiry sinister female hexer recovering from a claw-grip hex gesture, extended arm lowering and curling back toward the body, pulled-back arm releasing from amplify position, both arms settling loosely at sides, cursed green and void purple rune glow dimming back to a faint resting pulse, body returning to a loose unsettling neutral stance, full front view, muted desaturated palette, weathered field-worn, dark cold grey-blue background, Fractured Epic pixel art style, 252x252 canvas, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, the character has normal eyes facing forward not looking up at the camera

---

## Kullanim Notlari

### Silah Ekleme Adimi (Run ve PEAK sonrasi)
- Onceden animate-with-text-v3 ile silahsiz karakter kare setleri uret
- Her kare icin "Edit Image Pro" ile silah eklenir: prompt ornegi:
  > Add a [WEAPON_DESCRIPTION] held in [hand position] matching the body pose, cold blue #7BA7BC edge glow, no VFX, same pixel art style, same canvas 252x252

### Diger Yonler
- South (full front) tamamlaninca: East (sag profil), North (full arka), West (sol profil) icin ayni workflow tekrarlanir
- Her yon icin ayri animate cagrilari -- flip YASAK

### Interpolation ile Frame Ciftleme
- 6 frame run animasyonu sonrasi: "Interpolate NEW" (interpolation-v2) ile 12 frame'e cikar
- Attack segment'leri 4+4 = 8 frame, interpolation ile 16'ya cikarilabilir

### VFX Hatirlati (Unity Side)
- Projectile trail -> ParticleSystem
- Impact flash -> PointLight2D burst
- State decal (curse, rage aura) -> Shader overlay
- Yalnizca karakter pose/silhouette/minimal glow PixelLab'da uretilir
