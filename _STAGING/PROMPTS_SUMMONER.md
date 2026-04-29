# ChatGPT Image 2 — Summoner 8 Yön Prompts
**Kullanım:** PIPELINE_8DIR_CHATGPT.md workflow'una uyarak tek conversation'da ilk S üret, sonraki 7 yön için S referans olarak ekle.

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

## Prompt 2 — SOUTH-EAST
```
Same character as the previous image. IDENTICAL skull-mask helm with glowing cyan eye sockets, IDENTICAL dark void-black robe with tattered hem, IDENTICAL soul lantern (left hand, shoulder height, cyan flame inside iron cage), IDENTICAL bone fetishes on belt, IDENTICAL proportions, IDENTICAL color palette. Same asymmetric pose: left hand holding lantern at shoulder height, right hand lowered with palm-down command gesture, left foot forward.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now view the character in 3/4 FRONT VIEW rotated 45 degrees clockwise. Body angled toward the lower-right of the frame. Right shoulder leads forward toward the camera-right. Viewer sees the right side of the body prominently, left side partially occluded. Skull helm visible at 3/4 — the right eye socket glowing cyan, the skull profile from the right-front angle. The soul lantern (left hand at shoulder height) is on the FAR (left) side — the lantern cage and cyan soul-flame must still read past the left shoulder silhouette at shoulder height — do NOT hide the lantern behind the torso. The right hand (palm-down command gesture) is on the FOREGROUND (right) side, clearly visible.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity must stay 100% consistent.
```

## Prompt 3 — EAST
```
Same character. IDENTICAL skull helm, robe, soul lantern, belt fetishes, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE RIGHT-SIDE PROFILE. Character fully facing to the right of the frame. Full side silhouette. The skull helm is a distinctive silhouette from the side — the skull profile (forehead dome, hollow eye socket visible from the side, jaw/chin shape). The soul lantern in the left hand is on the FAR (left, back) side — the lantern cage and the cyan flame glow must still read past the back/shoulder silhouette at shoulder height. The lantern's cyan glow creates a subtle backscatter visible even from the right profile — the glow catches on the back-left robe. The right hand (palm-down) is on the FOREGROUND (right) side — the back of the right hand and the palm-facing-ground gesture visible in profile. Robe silhouette: the full floor-length robe visible in side profile with tattered hem edges.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 4 — NORTH-EAST
```
Same character. IDENTICAL skull helm, robe, soul lantern, belt fetishes, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees clockwise from full back view. Back of the RIGHT shoulder leads the view. Viewer sees the back-right of the torso (the back of the dark robe, back of the right shoulder), the TOP of the skull helm from above-behind (the skull crown top visible — the top dome of the skull armor), partial right side of the skull helm (right cheek area of the skull, right eye socket glow dimly visible from the back-right). The soul lantern (left hand at shoulder height) is on the FAR (left) side — the lantern must still read past the left shoulder silhouette at shoulder height — the cyan glow catches on the left shoulder area of the robe. The right hand palm-down is on the FOREGROUND (right) side, visible from behind-right.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 5 — NORTH
```
Same character. IDENTICAL skull helm, robe, soul lantern, belt fetishes, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now FULL BACK VIEW. Character facing directly away from the camera. Viewer sees the back: the TOP of the skull helm from directly above-behind (the skull crown, top dome of the helm — the most distinctive element from the back, must be clearly a skull shape even from behind/above), the full back of the dark robe, the tattered hem, the bone fetishes on the belt visible from the back (hanging at the back-waist area). The soul lantern in the left hand: from the back, the lantern is on our RIGHT (character's left side) at shoulder height — the iron cage and cyan soul-flame must read past the left shoulder silhouette. The right hand palm-down: on our LEFT (character's right side), the back of the lowered right hand visible past the right hip area.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 6 — NORTH-WEST
```
Same character. IDENTICAL skull helm, robe, soul lantern, belt fetishes, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 BACK VIEW rotated 45 degrees counter-clockwise from full back. MIRROR of north-east. Back of the LEFT shoulder leads the view. Viewer sees the back-left of the torso, top of the skull helm, partial left side of the skull helm (left eye socket with cyan glow dimly visible). The soul lantern in the left hand is now on the FOREGROUND (left) side — the lantern is clearly visible from the back-left angle at shoulder height: the iron cage, the cyan soul-flame inside, the ring handle in the left hand. The right hand palm-down is on the FAR (right) side — the back of the right hand at hip level must read past the right torso/shoulder silhouette.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 7 — WEST
```
Same character. IDENTICAL skull helm, robe, soul lantern, belt fetishes, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now PURE LEFT-SIDE PROFILE. MIRROR of east. Character fully facing to the left of the frame. Full side silhouette. The skull helm left profile — left eye socket with cyan glow visible from the side, the skull jaw/chin profile, the skull dome above. The soul lantern in the left hand is on the FOREGROUND (left, near viewer) side — the lantern is fully visible in left profile at shoulder height: the iron cage readable as a simple cage shape, the cyan soul-flame glowing within, the ring handle in the left hand. The right hand palm-down is on the FAR (right) side — the back of the right hand lowered at hip height must still read past the back/robe silhouette. The bone fetishes on the belt visible from the left profile hanging at the waist.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

## Prompt 8 — SOUTH-WEST
```
Same character. IDENTICAL skull helm, robe, soul lantern, belt fetishes, proportions, palette, pose.

ONLY CHANGE: viewing direction / character rotation. Keep the same high top-down ARPG camera height and perspective.

Now 3/4 FRONT VIEW rotated 45 degrees counter-clockwise. MIRROR of south-east. Body angled toward the lower-left of the frame. Left shoulder leads forward toward the camera-left. Viewer sees the left side of the body prominently, the right side partially occluded. Skull helm at left-front 3/4 — left eye socket glowing cyan, skull profile from the left-front angle. The soul lantern (left hand at shoulder height) is on the FOREGROUND (left) side — the lantern is prominently visible in the foreground: the iron cage, the cyan glow, the hand holding the ring handle, clearly readable. The right hand palm-down is on the FAR (right) side — the back of the right hand and the palm-down command gesture must still read past the right torso/shoulder silhouette — do NOT let the commanding gesture disappear entirely.

TOP-DOWN ARPG CAMERA at approximately 35 degrees high angle. Full body visible, black background, 1024x1024, same style. Identity 100% consistent.
```

---

# Üretim Kontrol Listesi — Summoner

Her yön üretildikten sonra kontrol:
- [ ] Kimlik (skull-mask helm with cyan eye glow, void-black robe with tattered hem, soul lantern with cyan flame, bone fetishes on belt) önceki yönlerle 100% tutarlı mı?
- [ ] Renk paleti aynı mı? (void black + bone white skull helm + CYAN accent — NO green, NO orange flame)
- [ ] Pose asimetrisi korundu mu? (lantern daima sol elde omuz hizasında; sağ el command gesture — yön değişse bile sabit)
- [ ] Skull helm: tüm yönlerde skull kimliği okunuyor mu?
- [ ] Yön okunması belirgin mi? (S ≠ SE ≠ SW görsel farkı net)
- [ ] Lantern occlusion kuralı: E ve NE yönlerinde bile lantern cage + cyan glow siluetten görünür mü?
- [ ] Yüz (skull helm) 3/4 ve profil yönlerinde doğru taraf görünüyor mu?
- [ ] Kamera ~35° korundu mu? (pure side view veya pure overhead değil)

8 yönün tamamı PASS → PixelLab Edit Image ile 128px pixel art downsample → Aseprite final pass → Unity import.
