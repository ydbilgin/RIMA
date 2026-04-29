# ChatGPT Image 2 — Ronin 8 Yön Prompts
**Kullanım:** PIPELINE_8DIR_CHATGPT.md workflow'una uyarak tek conversation'da ilk S üret, sonraki 7 yön için S referans olarak ekle.

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

## Prompt 2 — SOUTH-EAST
```
Same character as the previous image. IDENTICAL outfit, IDENTICAL sheathed katana at left hip, IDENTICAL topknot, IDENTICAL right shoulder pauldron, IDENTICAL wakizashi at right belt, IDENTICAL proportions, IDENTICAL color palette. Same asymmetric pose: left hand on scabbard throat at left hip, right hand resting on katana grip at left hip, both hands converging at left hip, left foot half-step forward.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now view the character in 3/4 FRONT VIEW rotated 45 degrees clockwise. His body is angled toward the lower-right of the frame. His right shoulder leads forward toward the camera-right. The right shoulder pauldron is now the prominent foreground shoulder element — clearly visible from the front-right 3/4 angle. Viewer sees the right side of his body, left side partially occluded. Face at 3/4 — right cheek, nose ridge, right eye visible. The katana scabbard at the left hip is on the FAR (left) side — the scabbard tip must still read past the left hip silhouette. Both hands at the left hip (foreground converging point) must be readable even at this rotated angle.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity must stay 100% consistent.
```

## Prompt 3 — EAST
```
Same character. IDENTICAL outfit, sheathed katana, topknot, right shoulder pauldron, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE RIGHT-SIDE PROFILE. Character fully facing to the right of the frame. Full side silhouette. The right shoulder pauldron is on the FOREGROUND (right, near viewer) side — the segmented plates clearly visible from the side profile. The katana at the left hip is on the FAR (left, back) side — the scabbard tip and the wrapped grip handle must both read past the back/left silhouette so the weapon identity is clear. Both hands at the left hip: right hand on the grip (far side), left hand on the scabbard throat (far side) — at least the back of the right wrist and the outline of the left hand should read past the far-side silhouette. Face in pure right profile — right cheek, sharp nose, right eye, downward gaze, the thin scar down the left cheek is on the far side.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 4 — NORTH-EAST
```
Same character. IDENTICAL outfit, sheathed katana, topknot, right shoulder pauldron, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees clockwise from full back view. Back of his RIGHT shoulder leads the view. Viewer sees the back-right of his torso, the top of his head (topknot clearly visible from above-behind), partial right side of his face (right cheek, right ear). The right shoulder pauldron is on the FOREGROUND (right) side — visible from behind-right, the segmented plates showing their top surfaces. The katana at the left hip is on the FAR (left) side — the scabbard tip must still read past the left hip/side silhouette. The wakizashi at the right belt is on the foreground side — its scabbard visible at the right hip from behind-right.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 5 — NORTH
```
Same character. IDENTICAL outfit, sheathed katana, topknot, right shoulder pauldron, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now FULL BACK VIEW. Character facing directly away from the camera. Viewer sees the back of his head (topknot visible from behind-top, the knot clearly defined), the full back silhouette — back of the indigo kimono, wide hakama pants from behind, the back of the sash belt. The right shoulder pauldron is on our LEFT (his right shoulder) — the back of the pauldron segments visible, top surfaces lit from the high-down camera. The katana at the left hip: from the back, the scabbard is on our RIGHT (his left) — the scabbard tip visible below the left hand, the grip visible at hip height. The wakizashi at the right belt: visible on our left (his right side) at hip height.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 6 — NORTH-WEST
```
Same character. IDENTICAL outfit, sheathed katana, topknot, right shoulder pauldron, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees counter-clockwise from full back. MIRROR of north-east. Back of his LEFT shoulder leads the view. Viewer sees the back-left of his torso, top of his head, partial left side of his face (left cheek, the scar from left eye visible). The katana at the left hip is now on the FOREGROUND (left) side — the scabbard and both hands at the left hip are clearly visible from the back-left angle. The right shoulder pauldron is on the FAR (right) side — the pauldron segments must still read past the right shoulder silhouette. The wakizashi is on the FAR (right) side.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 7 — WEST
```
Same character. IDENTICAL outfit, sheathed katana, topknot, right shoulder pauldron, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE LEFT-SIDE PROFILE. MIRROR of east. Character fully facing to the left of the frame. Full side silhouette. The katana at the left hip is on the FOREGROUND (left, near viewer) side — the scabbard is clearly visible in the left profile: the black lacquered scabbard angling at the hip, the wrapped grip at hip height, the scabbard tip reading past the front of the body. Both hands at the left hip: left hand on scabbard throat and right hand on grip are both on the near (foreground) side, readable. The right shoulder pauldron is on the FAR (right) side — the pauldron segments must still read past the back/right shoulder silhouette. Face in pure left profile — left cheek, the thin scar visible from jaw to below left eye, left eye in downward gaze.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 8 — SOUTH-WEST
```
Same character. IDENTICAL outfit, sheathed katana, topknot, right shoulder pauldron, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 FRONT VIEW rotated 45 degrees counter-clockwise. MIRROR of south-east. Body angled toward the lower-left of the frame. His left shoulder leads forward toward the camera-left. Viewer sees the left side of his body prominently, the right side partially occluded. Face at 3/4 — left cheek with the scar visible, left eye in downward gaze. The katana at the left hip is on the FOREGROUND (left) side — the scabbard, the left hand on scabbard throat, and the right hand on the grip are all on the foreground side, the full draw-ready hand position clearly readable. The right shoulder pauldron is on the FAR (right) side — the pauldron top and segments must still read past the right shoulder silhouette — do NOT let it disappear entirely behind the body.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

---

# Üretim Kontrol Listesi — Ronin

Her yön üretildikten sonra kontrol:
- [ ] Kimlik (topknot, clean-shaven, left cheek scar, muted indigo, right shoulder pauldron, sheathed katana at left hip) önceki yönlerle 100% tutarlı mı?
- [ ] Renk paleti aynı mı? (muted indigo + matte black + silver edge — NO warm colors, NO brown leather)
- [ ] Pose asimetrisi korundu mu? (katana daima sol kalçada kınında, her iki el sol kalçada — yön değişse bile sabit)
- [ ] Katana kınında kaldı mı? (asla çekilmemiş)
- [ ] Yön okunması belirgin mi? (S ≠ SE ≠ SW görsel farkı net)
- [ ] Silah occlusion kuralı: E ve NE yönlerinde bile scabbard tip ve grip görünür mü?
- [ ] Yüz 3/4 ve profil yönlerinde doğru taraf görünüyor mu?
- [ ] Kamera ~35° korundu mu? (pure side view veya pure overhead değil)

8 yönün tamamı PASS → PixelLab Edit Image ile 128px pixel art downsample → Aseprite final pass → Unity import.
