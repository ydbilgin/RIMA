# S Direction Prompt Pack — S41
> 2026-04-25 | Her class için yalnızca SOUTH / S yönü üretim promptları

## Kullanım Soruları

### Referans vermem gerekir mi?
Evet, ama referansın görevini sınırlı söyle. ChatGPT'ye `C:\Users\ydbil\Downloads\warblade_pro\warbladeNEW7.png` dosyasını kamera/perspektif/game-sprite scale anchor olarak ver. Bu görsel S yönü değildir; yan profil/pose/kıyafet/silah/renk kopyalanmayacak. Eğer `F:\CodexGame\RIMA\rima_style_anchor.png` verirsen sadece mood/lighting/world feel için kullan; environment üretmemesi gerektiğini söyle.

### Pixel-art durumunu vurguluyor mu?
Evet. Her prompt `PIXEL-ART CONVERSION FRIENDLY`, `downsampling to 128x128`, `large readable shapes`, `limited color palette`, `no micro-detail`, `black background only` gibi ifadeler içeriyor. Bu aşama doğrudan final pixel art değil; ChatGPT concept'in PixelLab/Aseprite 128px dönüşümüne uygun olması hedefleniyor.

### Doğru açıda mı?
Evet, promptlar high top-down ARPG gameplay camera için yazıldı ve `isometric character` demiyor. S yönünde body/head/hips/shoulders frontal kalmalı. Üretimden sonra şu 5 kriterden biri fail ise regenerate et: top of head visible, top surfaces of shoulders visible, face foreshortened, feet smaller/lower than head, no eye-level heroic poster look.

## Referans Cümlesi
Her promptun başında geçen referans cümlesi şu anlamda kullanılmalı:

```text
Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference.
Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation.
The new character must face SOUTH directly and follow this prompt.
```

---# WARBLADE — Disciplined Heavy Warrior

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A disciplined heavy warrior, male, mid-to-late 30s. Mature weathered tan skin, short dark hair, short well-kept brown beard with a few grey strands. Broad-shouldered but not grotesquely oversized — 8 heads tall, field-worn not gym-sculpted. Partial steel plate over brown leather and cloth: dark steel chest plate with riveted edges (no ornamental engravings), matching steel pauldrons strapped over leather underlayer, brown leather arm bracers, a worn brown leather skirt panel over cloth undershirt, a wide cloth-wrapped belt. One faded scar across the left cheek. Expression calm and controlled — not aggressive, not smiling. This is a veteran at rest between battles.

WEAPON: Two-handed greatsword. Long straight steel blade, approximately the height of the character's torso in length. Leather-wrapped grip at the base, simple flat cross guard, no ornamental engravings on the blade. Blade is unsheathed. The sword is carried in a resting position: held by the right hand at the grip, resting up against the right shoulder with the blade pointing diagonally upward behind and above the right shoulder — like a soldier casually resting a rifle on the shoulder, but with a greatsword.

POSE: Asymmetric action-ready resting stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Right hand grips the greatsword grip at shoulder height, blade resting diagonally upward over the right shoulder (blade tip pointing up-right behind the head). Left arm hangs loosely at the left side, left hand in a relaxed fist (not tense, not open — just a calm resting fist). Right foot is a half-step forward, both feet planted, both toes pointing toward the camera. Hips square to camera, both shoulders square to camera plane — the asymmetry comes ONLY from arm and weapon placement, NOT from torso rotation or body twist. Slight lean to the left (body weight shifting to left leg, casual). Head facing DIRECTLY at the camera, eyes looking at the viewer with calm assessment — NOT looking toward the sword, NOT looking sideways. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF THE HEAD — the part in the dark hair, the top of the skull; (2) we see the TOP SURFACES of the pauldrons and shoulders, not just the front faces — as if looking down from a short ladder; (3) the face is FORESHORTENED — chin appears closer to camera than forehead, nose tip closer than eye level, beard chin area appears closer; (4) the feet are SMALLER and lower in the frame than the head; (5) we see the BACK OF THE LEFT HAND hanging at the side; (6) we see the TOPS of the boots from above; (7) his eyes are positioned in the UPPER THIRD of his face area, NOT at face center.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. If you cannot clearly see the top of his head and the tops of the pauldrons, the camera angle is WRONG — regenerate from a higher vantage point.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. That exact high-down angle. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View him STRAIGHT FROM THE FRONT (south). He faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see his face from above-front (top of head visible, face foreshortened, eyes in upper third of face).

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual thread/stitching, NO individual hair strands, NO photographic skin texture. Think Hades 2 character splash art simplified — bold clean shapes, readable at small size. The pauldrons should be ONE or TWO big readable shapes, not a cluster of rivets. The belt wrapping — readable as a shape, NOT detailed weave. Less is more.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from boots to top of head with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall).

COLOR PALETTE: dark steel gray for chest plate and pauldrons, warm brown leather (not tan, not orange — worn field brown) for underlayer, skirt panel, bracers and belt, ember orange-red as accent color (faint on blade edge catch-light, warm in skin, a thread detail on belt wrap). NO blue accents anywhere. Skin is weathered tan. Hair and beard dark brown with grey hints. Limited to 5-7 main colors total.
```

---

# ELEMENTALIST — Scholar of Prime Forces

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A female scholarly mage, mid-30s, mature and grounded — NOT a glamorous noble, NOT a young fantasy girl. Honey-blonde hair pulled into a low practical bun at the nape of the neck, with a few loose strands around the temples. Weathered light skin with faint sun damage, small age lines at the eyes — she has spent time in the field, not in a palace. Lean but not frail build, 8 heads tall. Her robes are practical scholar's traveling clothes — dusty dark indigo (nearly navy blue but muted and worn, NOT vibrant) layered over a cream underlayer. The outer robe falls to mid-calf and is travel-stained at the hem. A wide leather belt cinches the robe at the waist, with small leather pouches attached — each pouch slightly bulging (element rune reagents, herbs, field notes). Fingerless gloves in dark leather, leaving the knuckles and fingertips bare. Worn leather knee-high boots. No eyewear, no spectacles, no goggles — clear forehead, hair framing only. Expression is focused and calm — mid-thought, not aggressive.

WEAPON / IMPLEMENT: Tall wooden staff, approximately as tall as the character. Straight dark wood grain, slightly worn smooth by long use, wrapped in leather cord at the grip zone near the middle. The metal cap at the top holds a small crystal orb in a simple cradle mount — the orb emits a subtle warm glow (amber-orange fire color for this illustration). The orb is approximately fist-sized. Staff is held in the LEFT hand, lower tip resting lightly on the ground beside the left foot (ground contact, zemin desteği), the staff leaning slightly inward toward the character's centerline.

POSE: Asymmetric mid-cast resting stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Left hand holds the staff at roughly hip height, lower tip touching the ground to the left of her left foot — the staff is her physical anchor. Right hand is raised to chest height, palm open and facing slightly upward, fingers spread in a gentle casting gesture — a small element rune (a faint glowing amber-orange geometric symbol) is visible floating between her fingertips, the focus of her attention. Shoulders are square to the camera (NOT rotated, NOT angled). Hips are square to camera. Left foot is a half-step forward, both feet pointing toward the camera. Head facing DIRECTLY at the camera, eyes looking at the viewer with calm scholarly focus — NOT looking at her casting hand, NOT looking sideways. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF HER HEAD — the honey-blonde bun from above, the top of the skull; (2) we see the TOP SURFACES of her shoulders, not just the front faces — like looking down at her from a short ladder; (3) her face is FORESHORTENED — chin appears closer to camera than forehead, nose tip closer than eyes; (4) her feet are SMALLER and lower in the frame than her head; (5) we see the BACK OF HER RIGHT HAND in the casting gesture (looking down onto hand top); (6) we see the TOPS of her boots from above; (7) her eyes are positioned in the UPPER THIRD of her face area, NOT at face center.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show her face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. DO NOT give her aristocratic glamour, ornate gold jewelry, or a regal silhouette — she is a field scholar. If you cannot clearly see the top of her head and the tops of her shoulders, the camera angle is WRONG — regenerate from a higher vantage point.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. That exact high-down angle. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View her STRAIGHT FROM THE FRONT (south). She faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see her face from above-front (top of head visible, face foreshortened, eyes in upper third of face).

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual thread/stitching, NO individual hair strands. The staff should be ONE clean vertical line with the orb as a distinct glowing shape at the top — not a cluster of fine wood-grain details. The belt pouches: 2-3 readable blob shapes, not a detailed leather texture. The rune on her palm: a simple geometric symbol, not intricate calligraphy.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from boots to top of head with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall). She should look like a credible scholar who does field work, not a fantasy pinup.

COLOR PALETTE: dusty indigo-navy for the outer robe (muted, NOT vivid blue), cream-off-white for the underlayer, worn dark leather for belt, pouches, gloves and boots, amber-orange as the primary accent (orb glow, rune on palm, subtle warmth in skin). Honey-blonde hair. Weathered light skin. NO heavy gold ornaments. NO vibrant royal blue. Limited to 5-7 main colors total.
```

---

# RANGER (REDESIGN) — Rift Stalker

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A female wildlands rift-stalker, NOT a generic Tolkien archer. Lean athletic build, tribal hunter aesthetic, mid-20s. Short tight braid with half-shaved sides of the head. Vertical war-paint stripes painted under both eyes in muted rift-purple color. NO hood. NO cape. Bone-and-horn ornamented light armor: an animal-bone shoulder guard sits on the right shoulder (carved bone plates), worn brown leather chest piece laced across the torso, an embroidered cloth wrap draped across the left shoulder in bone-white with subtle sigil patterns. Side-hip leather quiver attached to the right hip (NOT a back quiver), several arrows with dark fletching visible. She wears worn leather trousers tucked into knee-high soft leather boots, a wide leather belt with small bone trinkets and a hunting knife. Skin is weathered tanned fair with small scars across arms and one across the jaw. Eyes are intense green. Expression is calm and alert, not hostile.

WEAPON: Bone-recurve bow held in her left hand. The bow is carved from a single piece of large animal bone (pale ivory-white), with slight rift-purple crackling veins running along the inside curve of the bow. The bowstring is twisted sinew. The bow is held vertically, lower tip near the ground but not touching, gripped firmly in her left hand mid-bow. Her right hand hovers at the side-hip quiver, fingers resting on the fletching of an arrow, as if about to draw.

POSE: Asymmetric action-ready stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Bow vertical in her left hand (lower tip close to ground). Right hand at the hip quiver, fingers touching arrow fletching. Both shoulders square to the camera (NOT rotated, NOT angled — the pose's asymmetry comes only from arm placement and weapon, not from torso twist). Both feet planted shoulder-width apart, toes pointing toward the camera (slight stagger okay — one foot can be half an inch forward, but both feet face the camera). Hips square to camera. Head facing DIRECTLY at the camera, eyes looking forward at the viewer. NO body rotation, NO face turn toward arrow or bow. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so the body must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above her head, looking DOWN at her. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF HER HEAD — the part in her hair, the top of her braid; (2) we see the TOP SURFACES of her shoulders, not just the front faces; (3) her face is FORESHORTENED — chin appears closer to camera than forehead, nose tip closer than eyes; (4) her feet are SMALLER and lower in the frame than her head; (5) we see the BACK OF HER HANDS hanging down; (6) we see the TOPS of her boots from above; (7) her eyes are positioned in the UPPER THIRD of her face area, NOT at face center.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show her face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. If you cannot clearly see the top of her head and the tops of her shoulders, the camera angle is WRONG — regenerate from a higher vantage point.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. That exact high-down angle. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View her STRAIGHT FROM THE FRONT (south). She faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see her face from above-front (top of head visible, face foreshortened, eyes in upper third of face).

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual thread/stitching, NO scar pores, NO individual hair strands. Think Hades 2 character splash art simplified — bold clean shapes, readable at small size. Less is more — fewer accessories, simpler armor surfaces, cleaner cloth folds. The bone shoulder guard should be ONE big readable shape, not a cluster of tiny bones. The arrow fletching, dagger trinkets, belt details — keep MINIMAL and readable as single shapes.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from boots to top of head with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall).

COLOR PALETTE: bone-white and ivory for armor/wrap, worn brown leather, muted rift-purple accents on war paint and bow crackle, tanned skin, intense green eyes, dark brown hair. NO forest green anywhere. Limited to 5-7 main colors total.
```

---

# SHADOWBLADE (REDESIGN) — Veil Walker

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A lean assassin, lithe muscular build, mid-20s, androgynous but slightly masculine. Messy black hair falling over the forehead, slightly long at the back. A VEIL MASK covers the lower half of the face from just below the nose down to the neck — dark cloth wrap tied at the back, edges slightly torn. Eyes and forehead visible: dark eyes with a faint hot-magenta glow at the pupils. Weathered fair skin, a thin scar crossing the left eyebrow. Body-fit dark void-armor: close-fitting matte black chest armor with subtle sigil etchings on the chestplate, form-fitting dark sleeves, dark leather shoulder pads. Waist has fragmented cloth wraps — dark cloth panels hanging from the belt, the edges look pixel-shattered / unraveling into void particles (suggesting a "phasing out" visual effect at cloth edges). Belt has 4-5 small throwing daggers sheathed along it, visible. Dark leather gloves with knuckle reinforcement. Dark tight trousers into soft leather boots.

WEAPON: TWIN CURVED VOID-DAGGERS, one in each hand. Each dagger has a matte black blade with a slight inward curve (like a karambit but longer, ~30cm). A hot-magenta void-crackle runs along the inner cutting edge of each blade — not glowing brightly, more like cracks leaking a faint magenta light. Dark wrapped grips with small ring pommel.

POSE: LOW PREDATOR CROUCH, BODY AND HEAD FULLY FRONTAL TO CAMERA. Knees slightly bent (about 20-30 degrees of bend), body lowered but TORSO STAYS SQUARE TO THE CAMERA — shoulders parallel to the camera plane, hips parallel to the camera plane (NO twist, NO rotation). RIGHT HAND holds a dagger in REVERSE GRIP (blade pointing downward along the forearm, wrist near right hip — dagger blade extends down the back of the right thigh). LEFT HAND holds a dagger in NORMAL FORWARD GRIP, arm extended STRAIGHT TOWARD THE CAMERA at chest height (the dagger points directly at the viewer — its tip is the closest point of the character to the camera). Both feet planted shoulder-width apart, both toes pointing toward the camera (slight stagger okay — left foot can be a few centimeters more forward, but both feet face camera). HEAD FACING DIRECTLY AT THE CAMERA, eyes looking straight at the viewer with focused cold expression — NOT looking up from under hair, NOT angled away. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF THE HEAD — the messy black hair from above, the crown of the skull; (2) we see the TOP SURFACES of the rolled-forward shoulders (because the character is in low crouch with shoulders forward, the top of the shoulders is even more visible from this high angle); (3) the face is FORESHORTENED — chin closer to camera than forehead, the veil mask seen from slightly above; (4) feet are SMALLER and lower in the frame than the head; (5) we see the BACK of the right hand at right hip (reverse-grip dagger) from above; (6) we see the TOP of the forward-extended left arm + the foreshortened dagger pointing toward the camera (the dagger appears as a short stub because it points directly at the viewer); (7) we see the TOPS of the boots from above.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. If you cannot clearly see the top of the head and the tops of the shoulders, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera. That exact high-down angle. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View STRAIGHT FROM THE FRONT (south). Character faces camera directly with no rotation of body or head.

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual thread/stitching, NO individual hair strands. Think Hades 2 character splash art simplified. Belt daggers should be 4-5 readable simple shapes, NOT a cluster of tiny details. Fragmented cloth at waist: the "phase out" particle effect must be readable as a few large pixel-shaped chunks at cloth edges, NOT thousands of tiny particles.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated look with ONE vivid magenta accent color, Hades-style vivid contrast — NOT grimdark, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall).

COLOR PALETTE: pure void-black armor and cloth, hot magenta accent (NOT cold purple — warmer, almost neon magenta) on dagger cracks, eye glow, and cloth-edge particles, worn dark brown leather, weathered fair skin. Overall dark with one vivid accent color. NO blue, NO cold purple, NO gold.
```

---

# RAVAGER — Brutal Melee Berserker

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A massive male barbarian, late 30s, heavily scarred. Broad massive build — thick neck, wide shoulders, heavy limbs — but 8 heads tall proportions maintained (not chibi, not caricature). Bronze-brown skin covered in tribal scarification patterns across the chest and upper arms (geometric slash patterns, NOT tattoos — these are raised scar tissue). Full unkempt dark beard, roughly tied long dark hair pulled back into a crude topknot with a leather cord, several strands hanging loose around the face. Bare chest — the scarification patterns are prominently visible on the chest and upper arms. Worn leather kilt (mid-thigh length), leather leg wraps at the shins, crude leather sandals strapped to the feet. A FUR MANTLE draped over the LEFT shoulder only — large animal pelt, hanging down the left arm and back, roughly fastened at the left shoulder with a bone pin. This fur mantle is a key Ravager identifier (do NOT put it on both shoulders). One thick braided leather bracelet on the right wrist. Expression is barely contained ferocity — jaw clenched, eyes hard and focused, not mid-roar.

WEAPON: Massive two-handed great axe. The axe head is enormous — a wide single-curved steel blade with a chipped edge (battle-worn, NOT pristine), attached to a thick wooden haft approximately chest-height in length. The haft is wrapped in leather cord at grip zones with bone fragments lashed into the wrapping. The top of the haft has a blunt steel butt cap.

POSE: Asymmetric action-ready resting stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. The great axe rests on the RIGHT shoulder — right hand grips the haft firmly just below the axe head at the top (upper grip), the axe head resting against and above the right shoulder, blade edge facing outward-right. Left hand grips the haft lower, at roughly waist height (stabilizing/balancing grip, the haft angling diagonally from right shoulder to left hip area). Left shoulder is slightly forward (the fur mantle hangs from the left shoulder, angling the silhouette). Left foot is a half-step forward, wide stance, toes pointing toward the camera. Hips square to camera, shoulders square to camera — the asymmetry comes ONLY from the diagonal axe position and the fur mantle on the left. Body has a slight aggressive forward lean from the hips. Head facing DIRECTLY at the camera, eyes looking at the viewer with hard contained intensity. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF HIS HEAD — the crude topknot, the crown of the skull; (2) we see the TOP SURFACES of his massive shoulders, the top of the fur mantle on the left shoulder — like looking down from a short ladder; (3) the face is FORESHORTENED — chin appears closer to camera than forehead, nose closer than eyes; (4) the feet are SMALLER and lower in the frame than the head; (5) we see the TOPS of his hands on the axe haft; (6) we see the TOPS of his sandaled feet from above; (7) his eyes are positioned in the UPPER THIRD of his face area, NOT at face center.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. DO NOT put fur on both shoulders — only the LEFT shoulder. If you cannot clearly see the top of his head and tops of his shoulders, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View him STRAIGHT FROM THE FRONT (south). He faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see his face from above-front (top of head visible, face foreshortened, eyes in upper third of face).

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual hair strands. The great axe head should be ONE BIG readable angular shape — not detailed metalwork. The fur mantle: readable as a large chunky organic shape, NOT individual fur strands. The scarification: 3-4 bold simple lines per area, NOT intricate fine patterns. Less is more at 128px.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from sandals to top of head with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall).

COLOR PALETTE: dirty dark bronze for skin with highlights, dark brown leather for kilt, leg wraps, and bracelet, dark grey-brown fur for the left shoulder mantle, battle-worn dark steel for the axe head (chipped and darkened, NOT shiny), rough wood-brown for the axe haft, crimson red-blood accent (faint dried blood along the axe blade edge, crimson thread in the haft lashing, a subtle red tone in the scarification). NO bright colors. Muted and dark with one crimson accent. Limited to 5-7 main colors total.
```

---

# RONIN — Disciplined Draw-Master

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A wandering samurai, male, mid-30s. Slim wiry build — lean and precise, not bulky. Clean-shaven, sharp facial features with weathered light-tan skin and subtle frown lines. Topknot hairstyle (dark hair pulled up into a traditional topknot, clean and tight). No beard, no facial hair. Calm, downward-focused eyes — zen alertness, internal focus, not looking for a fight. Expression is still and contained. One thin scar from the corner of the left eye down the left cheek.

CLOTHING: Wide dark hakama pants (traditional Japanese wide-leg trousers, muted dark charcoal-indigo color, slightly worn and dusty at the hem). Short kimono top in muted indigo (same color family as hakama but slightly lighter), hem tucked into a wide fabric sash belt. A single shoulder armor piece on the RIGHT shoulder — a simple segmented pauldron in dark lacquered steel, strapped across the right shoulder. Cloth-wrapped sandals with visible tabi socks. A wide fabric sash-belt in dark grey-brown cinches the kimono. Companion wakizashi is visible at the belt on the right side — short sword in a matte black lacquered scabbard, handle showing.

WEAPON: Katana in a matte black lacquered scabbard, worn at the LEFT hip with the scabbard tucked through the sash belt on the left side (edge-up, traditional katana carry position). The katana is SHEATHED — blade NOT drawn. Left hand holds the scabbard at the throat (the collar where blade meets scabbard), steadying it. Right hand rests on the katana's handle/grip — fingers curled around the wrapped grip in a light iaido draw-ready position (NOT gripping tightly, NOT drawing — just resting, ready).

POSE: Asymmetric draw-ready stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Left hand holds the scabbard throat at the left hip, scabbard angling slightly (edge-up, tip toward the rear). Right hand rests on the katana grip at the left hip. This creates an asymmetric arm position: both hands converge toward the left hip where the katana is holstered. Left foot is a half-step forward. Body has a slight turn toward the left (the draw vector) — but SHOULDERS and HIPS remain SQUARE TO THE CAMERA PLANE (the turn is very subtle, NOT a visible body rotation — just a slight weight shift). Head facing DIRECTLY at the camera, eyes looking downward at a calm angle — NOT looking at the viewer directly, NOT looking sideways. Eyes are downcast in a relaxed but alert gaze (zen alertness). This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF HIS HEAD — the topknot from above, the top of the skull; (2) we see the TOP SURFACES of his shoulders, including the segmented pauldron top surface on the right shoulder; (3) the face is FORESHORTENED — chin appears closer to camera than forehead, nose tip closer than eyes; (4) his feet are SMALLER and lower in the frame than his head; (5) we see the TOPS of both hands at the left hip; (6) we see the TOPS of his sandaled feet from above; (7) his eyes are positioned in the UPPER THIRD of his face area, NOT at face center.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. DO NOT draw the katana — it stays sheathed at all times. If you cannot clearly see the top of his head and the tops of his shoulders, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View him STRAIGHT FROM THE FRONT (south). He faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see his face from above-front (top of head visible, face foreshortened, eyes in upper third of face).

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual cloth thread weave, NO individual hair strands. The hakama's wide legs should be TWO big readable shapes, not detailed fabric folds. The katana scabbard: ONE clean line shape with the wrapped grip clearly distinct. The shoulder pauldron: 2-3 segmented plates readable as distinct shapes, NOT riveted metalwork detail.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from sandals to top of head with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall). This is a wandering samurai in a dark fantasy world, NOT an anime hero.

COLOR PALETTE: muted dark charcoal-indigo for hakama (very dark, desaturated blue-black), slightly lighter muted indigo for kimono top, matte black for katana scabbard and wakizashi scabbard, dark lacquered steel with silver-edge highlight for the right shoulder pauldron, natural skin (weathered light-tan), dark hair. Silver edge accents on blade handles and pauldron edges. NO warm colors, NO red, NO brown leather — this palette is cool and minimal: indigo + black + silver. Limited to 5-7 main colors total.
```

---

# GUNSLINGER — Dueling Outlaw

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A female duelist-outlaw, mid-20s. Lean and precise build, 8 heads tall. Deep auburn-red hair in a messy practical braid — the braid falls to one side, slightly unruly with loose strands around the temples and face. Weathered light skin with a scattered cluster of freckles across the nose and cheeks. Sharp, calm eyes with a dangerous quiet intelligence. One thin scar across the right collarbone. Expression is dangerous calm — not smiling, not aggressive, simply completely at ease because she knows she's the fastest in the room.

CLOTHING: NOT puffy sleeves western fantasy, NOT saloon girl, NOT clean heroic western. Worn dark leather long-coat (mid-thigh length), slightly battered at the collar and cuffs, NOT cape-like — it's a fitted coat. Dark trousers tucked into weathered dark leather mid-calf boots. A leather chest piece worn over the coat (strapped chest armor, NOT ornamental — functional, field-worn). Wide leather belt with a large flat brass buckle. A dusty red-brown bandana (dark cloth with a subtle worn crack pattern, NO blue or purple glow) knotted loosely around the neck. A wide-brim hat in worn brown leather, slightly bent and weathered — with a bone or feather accessory tucked into the hat band on one side. Leather wrist bracers on both wrists. Two leather hip-holsters on each outer thigh — each holster with a pistol.

WEAPON: Twin pistols — dual-grip revolvers. Dark steel barrels and frames with brass accents at the cylinder and trigger guard. Leather-wrapped grip panels. The RIGHT PISTOL is held in the right hand, barrel pointing downward toward the ground in a relaxed gun-slinger resting grip (NOT aimed, just held at the side). The LEFT PISTOL is in the left hip holster — partially holstered, the grip visible, barrel inside the holster (in the process of being re-holstered or about to draw — not fully holstered, not drawn).

POSE: Asymmetric duelist resting stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Right hand holds the right pistol with barrel pointing down at the right side, arm slightly away from body (relaxed, confident). Left hand rests on the partially-holstered left pistol grip at the left hip — fingers lightly touching the grip (ready-to-draw, NOT actively gripping). Right shoulder is slightly forward. Right foot is slightly back in a duelist stance, both feet pointed toward the camera. Hips square to camera, shoulders square to camera — the asymmetry comes ONLY from arm and weapon placement. Head facing DIRECTLY at the camera, eyes looking at the viewer with that calm dangerous assessment — like she's clocked exactly how many steps to the door. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF HER HAT — the crown of the wide-brim hat from above (the hat top surface, the dent in the crown if any); (2) we see the TOP SURFACES of her shoulders and the top of the leather coat collar; (3) the face is FORESHORTENED — chin appears closer to camera than forehead, nose tip closer than eyes — the hat brim partially shadows the forehead; (4) her feet are SMALLER and lower in the frame than her head; (5) we see the TOP of the right hand holding the pistol barrel-down; (6) we see the TOPS of her boots from above; (7) her eyes are positioned in the UPPER THIRD of her face area under the hat shadow, NOT at face center.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. DO NOT give her puffy sleeves or western fantasy clichés — this is a rift-world outlaw, NOT a fantasy cowgirl. If you cannot clearly see the top of the hat and the tops of the shoulders, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View her STRAIGHT FROM THE FRONT (south). She faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see her hat brim from above-front and her face foreshortened beneath it.

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual hair strands. The hat should be ONE BIG readable shape — brim as a clean silhouette, crown as a simple dome shape, the bone/feather trim as ONE readable accent. The pistols: dark body + brass accent readable as two simple shapes. The coat: large dark shape with subtle edge detail, NOT detailed stitching.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from boots to top of hat with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall). Rift-world outlaw aesthetic, NOT clean western movie.

COLOR PALETTE: deep auburn-red hair (NOT orange, NOT bright red — deep dark auburn with warm highlights), dark leather for the long coat (very dark brown, nearly black-brown), worn brown leather for the hat and holsters, dark steel for pistol barrels, brass for pistol accents and belt buckle, dusty red-brown as accent on the bandana and coat trim. Weathered light freckled skin. The wide-brim hat: worn brown with one weathered shade lighter than the coat. Limited to 5-7 main colors total.
```

---

# BRAWLER — Unrelenting Boxer

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A tall lean-muscular male fighter, mid-20s. Athletic lean build — visible muscle definition but NOT a bodybuilder, NOT grotesquely oversized. 8 heads tall proportions. Bare chest — tribal arcane tattoos in purple ink covering the upper chest, both shoulders, and upper arms (geometric angular sigil patterns, like rune-marks drawn in bold lines — they glow very slightly with a faint purple-arcane light, suggesting magical augmentation). Short black hair, very short almost buzzed on the sides, slightly longer on top. Clean-shaven, sharp angular face with focused intensity. Bronze-warm skin tone. Small scars visible across the knuckles and one on the chin. Expression is controlled aggression — chin slightly tucked behind the left fist, eyes forward and alert.

CLOTHING: Bare chest (tattoos visible). Loose dark trousers (dark charcoal-brown, slightly baggy — fighter's training pants, NOT tactical military, NOT fantasy pants). Leather ankle wraps visible above bare feet OR low flat training shoes — footwear is minimal and flat. Leather wrist wraps extend from the wrist up the forearm on both arms (protective wrapping, several layers visible). BONE AND METAL GAUNTLETS on both hands — the gauntlets cover the knuckles and the back of the hand (NOT full gloves — the palm and fingers from the first knuckle down are bare/wrapped but the back-of-hand armor and knuckle reinforcement are clearly the dominant visual element). The gauntlet knuckle plate has small bone spurs or metal studs readable as a distinct silhouette element. These gauntlets are the character's primary visual identity — make them distinctive and readable.

POSE: Boxing guard stance, BODY AND HEAD STAY FRONTAL TO CAMERA. LEFT ARM: left shoulder rolled slightly forward, left fist raised HIGH — at jaw/chin level, protecting the face from a straight-on view (the left fist is at the same height as the chin, the arm bent, elbow pointing down-forward). This is a tight boxing guard on the left. RIGHT ARM: right fist chambered at chest height, slightly back from the left — the right elbow is pulled back, right fist at sternum/chest height (ready to fire a cross or hook). This creates the classic boxing southpaw/peek-a-boo guard. Both fists are closed tightly, the gauntlet knuckle plates prominent on both fists. Left foot is forward, right foot back — standard boxing stance. Body has a slight forward lean, but NO visible torso rotation; only the guard arm placement creates asymmetry. HIPS and SHOULDERS remain square to the camera plane — NOT a 3/4 body turn. Head facing DIRECTLY at the camera, chin slightly tucked behind the raised left fist, eyes looking at the viewer over the left fist in an intense focused glare. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF HIS HEAD — the short hair, crown of the skull; (2) we see the TOP SURFACES of his shoulders and the tops of the gauntlets on both raised fists; (3) the face is FORESHORTENED — chin behind the left fist appears closer, forehead further back; (4) his feet are SMALLER and lower in the frame; (5) we see the TOPS of his fists and the back-of-hand armor of both gauntlets from the high-down angle; (6) we see the TOPS of his feet/shoes from above; (7) his eyes are in the UPPER THIRD of his face area, peering over the raised left fist.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT center the eyes vertically in the face. DO NOT give him bulky fantasy plate armor — he fights with gauntlets and bare chest only. If you cannot clearly see the top of his head, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View him STRAIGHT FROM THE FRONT (south). He faces the camera directly with no rotation of body or head (body and head frontal to camera — the guard pose's asymmetry comes only from arm placement). Combined with the high-down camera tilt, we see his face from above-front, eyes peering over the left fist.

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual hair strands. The gauntlets should be TWO big bold readable shapes — each gauntlet as a distinct dark steel + bone shape, NOT cluttered with tiny rivets. The tattoos: 4-6 bold angular line patterns per area, NOT intricate detailed calligraphy. The tattoo glow: a subtle purple rim-light effect, NOT a bright neon blast.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from feet to top of head with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated weathered look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall). He should look like a disciplined fighter with arcane augmentation, NOT a fantasy bruiser.

COLOR PALETTE: bronze-warm skin tone, dark charcoal-brown for the trousers, dark steel with bone-white for the gauntlet knuckle plates (the gauntlets are the lightest/most visible element against the dark trousers), leather-brown for the wrist wraps and ankle wraps, arcane purple as the accent (tattoo glow on the chest/shoulders, subtle rim-light on the gauntlets). Short black hair. Limited to 5-7 main colors total.
```

---

# SUMMONER — Death Commander

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A tall slim necromancer, gender and age deliberately ambiguous — gaunt angular features, neither clearly young nor old, neither clearly male nor female. Height emphasizes the tall slim proportions (lean but not skeletal in the body). Gaunt face with high cheekbones and hollow cheeks. Most distinctively: a SKELETON-MASK HELM — a fitted face guard shaped like a bleached skull, covering the face from forehead to jaw (like a stylized skull visor on a helm, NOT a halloween mask — it's armored, fitted, slightly stylized). The eye sockets of the skull mask glow with cyan light from within. The skull mask is the single most important identity feature — it must be clear, bold, and readable. The hood of the robe is pushed PARTIALLY BACK — the hood is down around the neck/upper back area, revealing the skull helm above it. Dark hair not visible (hidden by the skull helm). Gaunt bare neck above the robe collar.

CLOTHING: Dark hooded robe — the hood is pushed back/down, so the robe body is the main garment. The robe is floor-length (or nearly so), void black in color. The hem and cuff edges are tattered/ragged with fraying, suggesting age and use. Around the waist, a leather belt with small bone fetishes hanging from it — several small carved bones, a knuckle-bone, a small skull charm — these are the decorative details, hanging on short leather cords. Beneath the robe, dark clothing is barely visible. Tattered dark boots peek out at the hem.

IMPLEMENT: SOUL LANTERN held in the LEFT HAND — a small iron cage lantern approximately palm-sized. Inside the iron cage, a soul flame burns — a cyan/blue-white flame (NOT yellow, NOT orange fire — cool cyan ghost-flame). The lantern is held at SHOULDER HEIGHT (not chest, not waist — shoulder height) in the left hand, the lantern swinging slightly as if being carried while walking. The cage has a simple ring handle that the left hand holds.

POSE: Asymmetric commanding stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Left hand holds the soul lantern at shoulder height, the lantern slightly extended away from the body (as if holding a torch out to illuminate the way or as an authority symbol over the undead). Right hand is at the right side, lowered, palm facing downward and slightly open — the palm faces the ground in a command gesture (like gesturing for an unseen minion to move forward, pointing or indicating direction). Left foot is a half-step forward. Body has a slight lean toward the left (the lantern side). Hips square to camera, shoulders square to camera — the asymmetry comes ONLY from arm placement (one high/extended, one low/commanding). Head (skull helm) facing DIRECTLY at the camera, the glowing cyan eye sockets looking at the viewer — an unnerving unblinking gaze. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF THE SKULL HELM — the top of the stylized skull crown, the top of the dome of the head armor; (2) we see the TOP SURFACES of the shoulders and the tops of the bony shoulder area of the robe; (3) the skull helm face is FORESHORTENED from above — the skull's chin appears closer, the skull's forehead/crown recedes slightly; (4) the feet (barely visible under the robe hem) are lower in the frame; (5) we see the TOP of the soul lantern being held at shoulder height — looking down onto the cage top; (6) we see the TOP SURFACE of the right hand in the palm-down command gesture; (7) the skull mask eye sockets are positioned in the UPPER THIRD of the visible face area.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the skull helm straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT make the hood fully up — it is pushed back, revealing the skull helm. DO NOT use yellow/orange fire in the lantern — cyan soul-flame only. If you cannot clearly see the top of the skull helm, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View STRAIGHT FROM THE FRONT (south). The character faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see the skull helm from above-front.

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail. The skull helm must be the dominant readable element — bold clean skull shape, NOT detailed etching. The soul lantern: a simple iron-cage rectangle with cyan glow inside (2-3 bars visible as a cage, not intricate metalwork). The bone fetishes on the belt: 3-4 simple bone shapes, NOT a detailed cluster. The robe: large dark mass shape with minimal fold lines.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from hem to top of skull helm with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated look, Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature proportions (~8 heads tall, emphasis on tall/slim/gaunt).

COLOR PALETTE: void black for the robe (pure deep black, slightly matte), bleached bone-white for the skull helm (pale ivory-white, the dominant light element against the dark robe), cyan for the soul-flame in the lantern and the eye socket glow (cool bright cyan — the PRIMARY accent color, the most vivid color in the palette), dark iron grey for the lantern cage, worn leather brown for the belt. Skin: gaunt and pale (barely visible — neck area only). Limited to 5-7 main colors total. NO green (that is the Hexer's color). NO purple as primary accent (that is Brawler's). Cyan distinguishes the Summoner.
```

---

# HEXER — Curse-Binder

## Prompt 1 — SOUTH (ilk, full detail)

**ATTACH REFERENCE IMAGE first:** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile sprite — our style and camera angle anchor).

```
REFERENCE USE — CRITICAL: Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference. Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation. The new character must follow the direction specified in this prompt while keeping the same high top-down ARPG gameplay camera feel.

Generate a character concept illustration. Single character, centered, full body visible, black background only.

CHARACTER: A female curse-binder, slim build, late 20s. Dark loose hair visible beneath the hood (dark brown or black, slightly wavy, falling past the shoulders). Pale skin with a faint sickly undertone — not corpse-pale, but not healthy — like someone who spends all their time in cursed shadow. Sharp eyes with a faint green-violet dual glow (the pupils catch both colors — this is her most distinctive facial feature). High cheekbones, thin lips slightly pursed in mid-concentration. Small geometric curse-mark tattoos barely visible on the neck and the back of the right hand (dark ink, simple rune shapes). Expression is focused and slightly predatory — she's in the middle of weaving something dangerous.

CRITICALLY IMPORTANT — NOT A SKELETON MASK: This is the Hexer, NOT the Summoner. The Hexer's face is FULLY VISIBLE — no skull mask, no skull helm, no face covering. The hood is UP but the face is FULLY VISIBLE within the hood opening. Dark hair visible falling from beneath the hood. This must be clearly a living human face, NOT any kind of skull or mask.

CLOTHING: Dark hooded robe with the hood UP — the hood frames the face, face fully visible within it. The robe is deep void-black as a base but has SIGIL-EMBROIDERY along the hem edges, sleeve cuffs, and hood border — the embroidery is in deep violet and cursed green, geometric sigil patterns (angular rune shapes, simple and bold). The embroidery glows very slightly (the curse-energy activating the thread). The robe falls to floor length. A wide dark sash belt cinches the robe at the waist. Small floating soul-glow shapes — 2-3 tiny green-violet light fragments — orbit around her shoulders at close range (like moths attracted to a flame, very small, NOT a dramatic magical aura — subtle).

IMPLEMENT: CURSE STAFF held in the RIGHT HAND — a tall twisted wood staff (gnarled, irregular — NOT straight, NOT clean — the wood is twisted and knotted as if grown or corrupted). The staff is approximately the character's height. At the TOP of the staff: a floating SIGIL ORB — a small floating magical sphere, a few inches in diameter, made of condensed curse-energy: deep green and violet swirling together (NOT a simple glowing orb — it looks like two curse-colors braided together). The staff is held with the lower tip just above the ground or lightly touching it, in the RIGHT HAND, staff vertical.

LEFT HAND: Raised to CHEST HEIGHT, palm facing slightly outward with fingers spread in a curse-weave gesture. Between the fingers, small void runes float (tiny geometric shapes, glowing green-violet, smaller than a fingernail — readable as a gesture, NOT a dramatic explosion of magic). This is the hand weaving the curse.

POSE: Asymmetric mid-incantation stance, but BODY AND HEAD STAY FRONTAL TO CAMERA. Right hand holds the curse staff vertically, lower tip near/touching the ground beside the right foot. Left hand is at chest height, palm outward, fingers spread in curse-weave gesture with tiny floating runes between the fingers. Right foot is a half-step forward. Body has a slight lean toward the left (toward the weaving hand). Hips square to camera, shoulders square to camera — the asymmetry comes ONLY from the two different arm positions and the staff. Head facing DIRECTLY at the camera, the green-violet glowing eyes looking at the viewer with unsettling focused intensity. This is the BASELINE FRONT VIEW — every other direction will rotate from this exact frontal pose, so body and head must be unambiguously frontal here.

CAMERA ANGLE — READ CAREFULLY, CRITICAL: Match the EXACT camera angle of the attached reference image. The camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking DOWN at them. Visible consequences (must all be true): (1) we CLEARLY SEE THE TOP OF THE HOOD — the hood fabric from above, the crown of the head, dark hair peeking from the hood; (2) we see the TOP SURFACES of the shoulders and the sigil-embroidery on the shoulder cuffs; (3) the face WITHIN the hood is FORESHORTENED — chin closer to camera, forehead recedes toward the hood shadow; (4) the feet (barely visible under the robe hem) are lower and smaller in the frame; (5) we see the TOP of the sigil orb at the top of the staff — looking down onto the orb from above; (6) we see the TOP SURFACE of the left hand in the curse-weave gesture; (7) her glowing eyes are in the UPPER THIRD of her visible face area within the hood.

YASAKLAR: DO NOT use eye-level perspective. DO NOT use heroic poster pose. DO NOT show the face straight-on at viewer eye height. DO NOT make head and feet the same apparent size. DO NOT give her a skull mask or skeleton helm — that is the Summoner. DO NOT put the hood down — it stays UP with face visible. DO NOT use cyan as the accent — cyan is the Summoner's color. Use green + violet only. If you cannot clearly see the top of the hood and the tops of the shoulders, the camera angle is WRONG — regenerate.

REFERENCE: HADES 2 in-game character view, DIABLO 4 character creation top-down camera, PATH OF EXILE 2 character close-up. Match the attached reference image's perspective EXACTLY.

CAMERA DIRECTION: View her STRAIGHT FROM THE FRONT (south). She faces the camera directly with no rotation of body or head. Combined with the high-down camera tilt, we see her face from above-front within the hood, foreshortened, green-violet eyes in the upper third of the face.

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors). Soft painterly shading, NO photographic micro-detail, NO individual hair strands, NO detailed embroidery stitch-work. The curse staff: ONE twisted organic shape with a distinct glowing orb at the top — the twisted wood texture is suggested by 2-3 curves, NOT detailed bark. The sigil orb: a simple glowing sphere with two-color swirl — readable as one distinct element. The embroidery on the robe: 3-4 bold sigil shapes along the edges, NOT a dense pattern. The floating void runes at the left hand: 2-3 tiny geometric shapes, readable as a gesture accent.

OUTPUT: 1024x1024 PNG, black background only, no environment, no ground shadow. Character centered, full body visible from hem to top of hood with ~10% margin.

GENERAL STYLE: Grounded fantasy aesthetic, muted desaturated look with vivid dual-accent (green + violet), Hades-style vivid contrast — NOT grimdark, NOT dark fantasy genre, NOT anime, NOT chibi. Mature realistic proportions (~8 heads tall).

COLOR PALETTE: void black for the robe (deep matte black), deep violet and cursed green as the DUAL ACCENT colors (the sigil embroidery, the orb, the eye glow, the floating runes — both colors together, NOT just one), dark wood for the staff, pale-sickly skin. Dark hair under the hood. Limited to 5-7 main colors total. NO cyan (that is the Summoner). NO blue. The violet is warm-leaning (lean toward magenta-violet, NOT cold blue-violet). The green is sickly (lean toward yellow-green-acid, NOT emerald). Together they feel like poison and shadow.
```
