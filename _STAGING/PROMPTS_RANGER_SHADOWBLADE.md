# ChatGPT Image 2 — Ranger + Shadowblade (REDESIGN) 8 Yön Prompts
**Kullanım:** PIPELINE_8DIR_CHATGPT.md workflow'una uyarak tek conversation'da ilk S üret, sonraki 7 yön için S referans olarak ekle.

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

## Prompt 2 — SOUTH-EAST
```
Same character as the previous image. IDENTICAL outfit, IDENTICAL bone-recurve bow, IDENTICAL hair, IDENTICAL war-paint, IDENTICAL proportions, IDENTICAL color palette. Same asymmetric pose: bow in left hand vertical lower tip near ground, right hand at hip quiver touching arrow.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now view the character in 3/4 FRONT VIEW rotated 45 degrees clockwise. Her body is angled toward the lower-right of the frame. Her right shoulder leads forward toward the camera-right. Viewer sees the right side of her body prominently, the left side is partially occluded behind the torso. Her face is visible at a 3/4 angle, we see her right cheek and nose ridge, left side of face slightly hidden. The bone-recurve bow (in her left hand) is now partially behind her body — blade of bow must remain visible past her right hip silhouette, do NOT fully hide it behind the torso. Right hand still at hip quiver on the foreground side.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same grounded fantasy style. Identity must stay 100% consistent with previous image.
```

## Prompt 3 — EAST
```
Same character as previous. IDENTICAL outfit, bow, hair, war-paint, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE RIGHT-SIDE PROFILE. Character fully facing to the right of the frame. Full side silhouette. Both legs visible in profile. The bone-recurve bow in her left hand is on the FAR side of the body (behind torso from viewer's perspective) but the bow must still read — bow grip visible past her chest silhouette, lower tip of bow visible past her legs, upper tip visible past her head/shoulder. Her right hand (foreground) still at the hip quiver, fingers on arrow fletching, clearly visible. Face in pure profile, we see her right cheek, nose, eye.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle (not pure flat side view — still slightly tilted down to see top of shoulders). Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 4 — NORTH-EAST
```
Same character. IDENTICAL outfit, bow, hair, war-paint, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees clockwise from full back view. Back of her right shoulder leads the view. Viewer sees the back-right of her torso, the top of her head (braid + half-shaved sides visible from above-behind), partial right side of her face (right cheek, right ear, right eye may barely show from the back-3/4 angle). The bone shoulder guard on her right shoulder is prominently visible from behind. The bow in her left hand is on the far (left) side of the body from the viewer's back-3/4 angle — bow must read past her left hip silhouette. Quiver on right hip is on the foreground side, clearly visible.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 5 — NORTH
```
Same character. IDENTICAL outfit, bow, hair, war-paint, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now FULL BACK VIEW. Character facing directly away from the camera. Viewer sees the back of her head (braid + half-shaved sides visible from behind, top-down slightly), the full back silhouette of her body including the animal-bone shoulder guard on her right shoulder (our left from this angle), the embroidered cloth wrap across her left shoulder (our right from this angle), back of the leather chest laces. The bone-recurve bow in her left hand is visible on the character's left side (our right from the back view), grip at mid-body, bow tip near ground on her left side. Quiver on right hip visible from the back (our left from this angle).

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 6 — NORTH-WEST
```
Same character. IDENTICAL outfit, bow, hair, war-paint, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees counter-clockwise from full back. MIRROR of north-east. Back of her left shoulder leads the view. Viewer sees the back-left of her torso, top of her head, partial left side of her face (left cheek). The embroidered cloth wrap on her left shoulder is prominently visible. The bone-recurve bow in her left hand is on the FOREGROUND side of the body now — bow grip + tip clearly visible on the viewer's right side (her left side). Right hand at hip quiver is on the far side of the body, arrow fletching must still be visible past her right hip silhouette.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 7 — WEST
```
Same character. IDENTICAL outfit, bow, hair, war-paint, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE LEFT-SIDE PROFILE. Mirror of east. Character fully facing to the left of the frame. Full side silhouette. The bone-recurve bow in her left hand is now on the FOREGROUND side of the body (between her body and the viewer) — fully visible, grip at her mid-body, lower tip near ground in front of her legs, upper tip near her shoulder height. Her right hand at the hip quiver is on the far side of body (behind torso from viewer) — arrow fletching must still read past her back silhouette. Face in pure left profile, we see her left cheek, nose, left eye, war paint on left side of face.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 8 — SOUTH-WEST
```
Same character. IDENTICAL outfit, bow, hair, war-paint, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 FRONT VIEW rotated 45 degrees counter-clockwise. MIRROR of south-east. Body angled toward the lower-left of the frame. Her left shoulder leads forward toward the camera-left. Viewer sees the left side of her body prominently, the right side is partially occluded behind the torso. Her face is visible at a 3/4 angle — we see her left cheek, the vertical war-paint stripes on her left cheek clearly, left eye. The bone-recurve bow in her left hand is on the foreground side — bow fully visible, grip at her mid-body, tip near ground, upper tip near her shoulder. Her right hand at hip quiver is on the far side, partially behind body — arrow fletching must still read past her right hip silhouette.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
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

## Prompt 2 — SOUTH-EAST
```
Same character as previous. IDENTICAL outfit, veil mask, messy black hair, twin curved void-daggers, fragmented cloth at waist, proportions, magenta accent color. Same asymmetric low predator crouch pose: right hand reverse-grip dagger at right hip, left hand forward-grip dagger extended toward viewer, left foot forward right foot back.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 FRONT VIEW rotated 45 degrees clockwise. Body angled toward the lower-right of the frame. Right shoulder forward toward camera-right. Viewer sees the right side of the body prominently, left side partially occluded. Face visible at 3/4 — we see right cheek above the veil mask, right eye, right side of forehead, right side of messy black hair. The forward-extended left arm with dagger now reaches toward the camera-right diagonal (since body rotated). The right hand reverse-grip dagger is on the foreground (right) hip side, clearly visible. Fragmented cloth at waist: right side fully visible, the "phase out" particle edge effect readable on the right hip.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 3 — EAST
```
Same character. IDENTICAL outfit, mask, daggers, pose, palette.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE RIGHT-SIDE PROFILE. Character fully facing to the right of the frame. Full side silhouette. The low predator crouch reads perfectly from the side — clear forward lean, bent knees, asymmetric foot placement (left foot in front now reading as far-side foot from profile view). LEFT ARM (now on the far side) with forward-grip dagger extends TOWARD the camera-right (the direction the body faces). The dagger blade points horizontally to the right, magenta crackle visible along the blade edge. RIGHT ARM (foreground, closest to viewer) with reverse-grip dagger — blade pointing down along the back of the forearm, wrist near right hip — fully visible in profile. Face in right profile: veil mask covers lower face, messy hair falls forward over forehead, right eye visible with faint magenta glow. Fragmented cloth at waist visible as side silhouette with particle edges trailing backward.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 4 — NORTH-EAST
```
Same character. IDENTICAL outfit, mask, daggers, pose, palette.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees clockwise from full back view. Back of right shoulder leads. Viewer sees back-right of the torso, top of the head (messy black hair from above-behind, nape of neck), partial right cheek and ear. The reverse-grip dagger (right hand at right hip) is on the foreground (right) side, clearly visible from the back-3/4 angle — we see the back of the forearm with the blade running downward behind the wrist. The forward-grip dagger (left hand extended) is on the far (left) side — dagger blade must still read past the left hip silhouette, tip pointing away from the viewer at diagonal. Fragmented cloth at waist: particle edges visible trailing from the back-right.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 5 — NORTH
```
Same character. IDENTICAL outfit, mask, daggers, pose, palette.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now FULL BACK VIEW. Character facing directly away from camera. Viewer sees the back of messy black hair, nape of neck, full back silhouette of the void-armor, back of shoulder pads. Low predator crouch reads from behind: slightly bent knees visible below, forward lean visible as shoulders positioned ahead of hips. RIGHT ARM with reverse-grip dagger — we see the BACK of the right forearm at right hip, the dagger blade pointing downward past the hip. LEFT ARM extended forward (away from viewer) — we see the back of the shoulder/upper arm stretching forward, the dagger barely visible past the left side of the body (tip reading past left shoulder silhouette). Fragmented cloth at waist: all edges readable as back silhouette, particle effect trailing toward the viewer (the direction the character moves away from).

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 6 — NORTH-WEST
```
Same character. IDENTICAL outfit, mask, daggers, pose, palette.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees counter-clockwise from full back. MIRROR of north-east. Back of left shoulder leads. Viewer sees back-left of torso, top of head, partial left cheek and ear. The forward-grip dagger (left hand extended) is on the FOREGROUND side now — we see the extended left arm stretching forward and to the left of the frame, dagger blade clearly visible with magenta crackle. The reverse-grip dagger (right hand at right hip) is on the FAR side — dagger blade must still read past the right hip silhouette from the back-3/4 angle. Fragmented cloth at waist: left-side particle edges prominent.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 7 — WEST
```
Same character. IDENTICAL outfit, mask, daggers, pose, palette.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE LEFT-SIDE PROFILE. MIRROR of east. Character fully facing to the left of the frame. Full side silhouette. LEFT ARM (now foreground, closest to viewer) with forward-grip dagger extends TOWARD the camera-left (direction body faces). Dagger blade points horizontally to the left, magenta crackle along blade. RIGHT ARM (now far side, behind torso from viewer) with reverse-grip dagger — blade visible past the back silhouette pointing downward. Face in left profile: veil mask, messy hair over forehead, left eye visible. Fragmented cloth at waist: side silhouette with particle edges trailing backward (to the camera-right).

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 8 — SOUTH-WEST
```
Same character. IDENTICAL outfit, mask, daggers, pose, palette.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 FRONT VIEW rotated 45 degrees counter-clockwise. MIRROR of south-east. Body angled toward the lower-left of the frame. Left shoulder forward toward camera-left. Viewer sees left side of body prominently, right side partially occluded. Face visible at 3/4 — we see left cheek above veil mask, left eye, left side of forehead, messy hair visible from front-left. The forward-grip dagger (left hand extended) is now on the FOREGROUND side, reaching toward the camera-left diagonal. The reverse-grip dagger (right hand at right hip) is on the far (right) side — blade must read past the right hip silhouette pointing downward. Fragmented cloth at waist: left side fully visible, particle edges on left hip.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

---

# Üretim Kontrol Listesi

Her yön üretildikten sonra kontrol:
- [ ] Kimlik (saç, yüz, giyim, silah) önceki yönlerle 100% tutarlı mı?
- [ ] Renk paleti aynı mı? (Ranger: bone-white/brown/rift-purple; Shadowblade: void-black/hot-magenta)
- [ ] Pose asimetrisi korundu mu? (silahın hangi elde olduğu yön değişse bile sabit)
- [ ] Yön okunması belirgin mi? (S ≠ SE ≠ SW görsel farkı net)
- [ ] Silah occlusion kuralı: far-side'da bile blade tip görünür mü?
- [ ] Yüz 3/4 ve profil yönlerinde doğru taraf görünüyor mu?
- [ ] Kamera ~35° korundu mu? (pure side view veya pure overhead değil)

8 yönün tamamı PASS → PixelLab Edit Image ile 128px pixel art downsample → Aseprite final pass → Unity import.
