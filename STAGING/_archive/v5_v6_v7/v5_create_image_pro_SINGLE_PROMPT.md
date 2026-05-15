# Create Image Pro — Batch 16 v5 (RIMA-canon, reference-driven angle)
**Karar refs:** #71 positive-only / #80 Class Silhouette Bible / #98 rift cyan #00FFCC + violet #5A2A8A / #99 no hand mention / #100 chibi 64x64 / #123 weapon decouple
**Reference image:** Attach `Characters/anchors/warblade.png` to Create Image Pro
**Canvas:** 64×64 for ALL 16 variations
**Credit:** ~6 (single generation)

## V4 → V5 değişim
- ✗ Per-variation "True south, direct front view." closer kaldırıldı (reference image taşıyor)
- ✗ Per-variation "south-facing" / "high top-down ~30-35°" kaldırıldı (reference image taşıyor)
- ✓ Her karakter EXPLICIT "is unarmed / weaponless body" positive ile yazıldı (Karar #71 positive-only enforcement)
- ✓ NEGATIVE PROMPT sadeleşti — sadece style guards (3d/AA/gradient/painterly), weapon ve direction negatives positive ile karşılandı

---

## Reference image description (yapıştırılacak text — angle+direction only)

Bu metni Create Image Pro UI'da reference image'ın description / instruction field'ına yapıştır:

```
Use this reference image only as an angle and direction guide. Match: camera angle and facing direction. Do not match: outfit, armor, colors, build, weapon, hairstyle, pose, accessories, or any identity feature.
```

---

## YAPIŞTIRMA — Main prompt (kopyala-yapıştır)

```
# Batch 16 — RIMA v5 Final Prompt (reference-driven angle, explicit unarmed)
# Karar #134 procedural+polish, Karar #80 Class Silhouette Bible, Karar #98 rift palette
# Karar #99 no hand mention, Karar #123 weapon decouple

GLOBAL STYLE BLOCK (apply to ALL 16 variations):
Canvas size: 64x64 pixels for ALL 16 variations (characters AND mobs alike — single batch canvas, no size mixing). Pixel art chibi style, locked to the attached reference image for camera angle and facing direction only (do not copy any other identity feature from the reference character). Visual reference family: Hades / Hyper Light Drifter / Into Samomor (Sang Hendrix RPG Maker MZ) + Salt and Sanctuary dark gritty mood.
Chibi proportions: oversized head (~40% of total height), short legs, broad shoulders.
Sprite must occupy 85-90% of the canvas height. Transparent background.
Outline: Thick 1px black pixel outline, no anti-aliasing, no gradients, no painterly shading. Hard pixel edges.
Palette: Muted desaturated Salt-and-Sanctuary Shattered Keep base tones, dark gritty mood. Karar #98 rift accents (cyan #00FFCC + violet #5A2A8A) used sparingly for rift-affiliated entities only.
Symmetric direct-facing pose: shoulders visibly even, face and both eyes clearly visible from the camera.
Each variation is a standalone character sprite.

CHARACTERS 1-10 are PLAYABLE CLASSES with weapon decouple (Karar #123): the body sprite is fully WEAPONLESS — hands carry only empty grip-shaped negative space where the absent weapon will attach later as a separate runtime sprite. EXCEPTIONS: Brawler (intentionally bare-fisted, fists are the weapon, no decouple), Ronin (the body retains a plain empty sheath silhouette on the hip; the sword itself is absent).

MOBS 11-16 are NON-HUMANOID HOSTILE ENTITIES with fused/integrated weapons and hazards permitted (Karar #123 mob clause).

Silhouette must be readable at 64x64 and at 16px thumbnail.

VARIATIONS:

1) Warblade — male fighter, body is completely unarmed in this sprite (weapon attaches later), both hands held at chest level shaped as a hollow two-hand cradle grip with empty horizontal space between them as if an invisible greatsword were resting there. Body angled forward slightly in low guard stance, broad heavy stocky build, dark brown leather armor #4F3A2C with brass buckle accents #C09455. Silhouette reads: wide shoulders + two-hand empty cradle at chest + fully weaponless.

2) Ranger — female hunter, body is completely unarmed in this sprite, one hand held at the thigh shaped as a hollow vertical grip with empty space inside the fist as if an invisible bow handle were there, the other arm hangs loose at the side. Tactical scout stance, slim upright build, head tilted as if listening, dark forest armor #2A3520 with cold blue trim #7BA7BC. Silhouette reads: tall slim + thigh-grip empty hand asymmetric + fully weaponless.

3) Shadowblade — male assassin, body is completely unarmed in this sprite, both hands held near the torso with palms turned inward and elbows tucked back, fists shaped as hollow reverse-grip cradles with empty space inside as if invisible twin daggers were there. Slim compact build, near-black deep purple armor #1A1025 with void purple accent #5A2A8A. Silhouette reads: narrow + reverse-grip twin hands tight to torso + elbows back + fully weaponless.

4) Elementalist — female robed mage, body holds no item in this sprite, one palm faced upward at chest level cradling empty air as if calling power into an invisible disc, the other arm loose at the side. Open casting stance, long dark robe #2A1F35 wide at hem narrow at shoulder, warm honey-blonde hair, golden cuff accent #FFD700. Silhouette reads: wide robe hem + upward empty palm at chest + distinct honey-blonde hair.

5) Ravager — male berserker, body is completely unarmed in this sprite, both hands held at hip level shaped as hollow short-handle grips with empty space inside the fists as if invisible dual axes were gripped there. Aggressive forward hunched crouch, shoulders rolled forward menacingly, massive bulky build wider than Warblade, dark blood-red rough leather armor #3A1A0A with blood red accent #D43F3F. Silhouette reads: bulkier than Warblade + dual hip-curled empty hands + forward menace + fully weaponless.

6) Ronin — male duelist, the body sprite retains a plain empty sheath silhouette at the hip (the sword itself is absent, will attach later), one hand held at the hip shaped as a hollow draw-ready grip with empty space, the other resting on the empty sheath. Iaido draw posture body angled three-quarter sideways, slim narrow profile, dark navy near-black kimono-style garment #1A1A2E with pale gold accent #C8A87A at collar. Silhouette reads: slim + empty draw-grip one side + plain empty sheath silhouette opposite hip.

7) Gunslinger — female duellist, body is completely unarmed in this sprite, both hands held at hip level shaped as hollow vertical handle grips with empty space inside the fists as if invisible pistols were drawn there. Gentle micro-crouch with weight shifted onto one leg, dark grey-purple trench coat #1A1520 with fire orange accent #FF4400 at collar and cuffs, deep auburn-red hair visible above collar. Silhouette reads: trench coat wide + two hip-curled empty hands flanking torso + auburn hair.

8) Brawler — male bare-fisted fighter, intentionally weaponless (Karar #123 exception: fists ARE the weapon, no decouple), both fists clenched solid and raised to chin level in boxing guard, knees softly bent in fight stance. Heavy muscular build, dark leather hand-wrappings #2A1A10 with orange accent #FF8C00. Silhouette reads: wide muscle mass + raised clenched solid fists at chin + low crouch.

9) Summoner — female conjurer, body is completely unarmed in this sprite, one hand held at chest level shaped as a hollow vertical handle grip with empty space inside as if an invisible lantern were held there, the other arm raised palm-open in gentle directing gesture. Orchestra-conductor stance, very dark green-black robe #0A1A0A, neon green trim accent #00FF88. Silhouette reads: tall robe + empty chest-grip hand + other arm raised palm-open directing.

10) Hexer — female curse-bearer, body holds no item in this sprite, one arm bent across the chest shaped as if cradling an invisible book at the heart with the book-shape implied only by hollow arm geometry, the other hand at the side shaped as a hollow vertical pole grip with empty space as if an invisible staff were held there. Very dark purple-black hooded robe #1A0A1A with dark red accent #8B0000 at hood edge. Silhouette reads: hood up + book-cradle empty arm posture at chest + side empty pole-grip hand.

11) Spire Choirling mob — non-humanoid hostile support, torso floating suspended in mid-air with arms outstretched horizontal wide in eerie welcoming posture, torn cloth trailing downward where the lower body would be, thin cyan halo-ring (#00FFCC) glowing behind the head, violet warmth (#5A2A8A) suffusing the inner folds of the trailing cloth, former-singer-turned-wrong. Silhouette reads: floating torso + arms wide outstretched + halo ring behind head + cloth trail below.

12) Shard Walker mob — non-humanoid hostile fast melee, skeletal crystalline humanoid creature, jagged teal-cyan crystal shards (#00FFCC) jutting from shoulders and spine, muted dark grey-blue body #2A3545, four-limbed upright posture with clawed hands, hunched predatory gait. Silhouette reads: jagged crystal protrusions on shoulders + hunched gait + clawed hands.

13) Penitent Bruiser mob — non-humanoid hostile tank, heavy humanoid creature with self-flagellant aesthetic (chain or spike wrappings on arms), dark crimson-grey armor #3A1A1A, glowing red aura ring at feet (heal-debuff indicator #D43F3F), stoic forward-leaning tank posture, clawed gauntlets. Silhouette reads: wide heavy torso + red aura ring at feet level + chain-wrapped arms.

14) Riftbound Augur mob — non-humanoid hostile ranged oracle, corrupted oracle humanoid in dark teal-grey robes #1A2B2B with rift cyan markings (#00FFCC) on face and hands visible as glowing pattern lines, both arms slightly raised in ominous warning gesture, hood drawn but face markings visible. Silhouette reads: wide gesture arms raised + glowing face and hand markings + hooded robe.

15) Hollow Hulk mob — non-humanoid hostile heavy melee, large golem-like creature wide and blocky filling most of canvas, cracked stone or corroded metal body #2A2A2A with dark void seams (#5A2A8A faint inner glow), no visible eyes (hollow black sockets), massive fists lowered at sides, slow lumbering stance. Silhouette reads: square-wide blocky torso + massive fists at sides + hollow eye sockets.

16) Rift Acolyte mob — non-humanoid hostile area-denial caster, robed cultist humanoid with both hands cradling a swirling violet rift fracture orb (#5A2A8A) at chest height, the orb emitting faint cyan tendrils (#00FFCC) outward, hood drawn low over hidden face, tattered dark vestments #1E1E2A, slightly hunched casting posture. Silhouette reads: hooded robed figure + orb cradle front chest + cyan tendrils outward.

NEGATIVE PROMPT (style guards only — weapon/direction handled by positive instructions):
3d render, soft shading, blur, smooth gradient, isometric projection, anti-aliasing, soft edges, realistic proportions, painterly style, three-quarter shoulder turn.
```

---

## QC her sprite için (üretim sonrası)

### Karakterler (1-10)
- [ ] Body silahsız (visible weapon yok) — explicit positive instruction
- [ ] Hand geometry weapon shape'i koruyor (empty cradle/grip)
- [ ] 6 Ronin: plain empty sheath silhouette at hip retained
- [ ] 8 Brawler: bare fists, no decouple
- [ ] 10 Hexer: book-cradle body posture, grimoire passive
- [ ] Reference image angle ve facing direction yansımış (35°+south)

### Moblar (11-16)
- [ ] Non-humanoid silhouette (humanoid'den ayrılabilir)
- [ ] Fused weapons/hazards OK
- [ ] Karar #98 palette: cyan #00FFCC + violet #5A2A8A açık görünüyor

### Tüm 16
- [ ] 64×64 canvas
- [ ] 85-90% sprite fill
- [ ] 1px black outline, no AA
- [ ] Transparent background
- [ ] Symmetric direct front view (reference'tan inherit)

---

## Workflow

1. Yukarıdaki **Main prompt YAPIŞTIRMA blok**unu kopyala (code-block içindeki tüm metin)
2. Create Image Pro'ya yapıştır description field'a
3. `Characters/anchors/warblade.png` reference image olarak attach et
4. Reference image description field'ına yukarıdaki **Reference image description** metnini yapıştır
5. Canvas: **64×64** (Create Image Pro UI'da set et)
6. Generate → 16 variation çıkacak (10 character + 6 mob)
7. QC checklist uygula
8. Bana sonuçları göster
