# Create Image Pro — Batch 16 v8 (no class names → no text trigger)
**Karar refs:** #71 weapons in hand spirit / #80 Silhouette Bible / #98 rift palette / #100 chibi 64x64 / #123 weapon decouple
**Reference image:** `C:\Users\ydbil\Downloads\ChatGPT Image 15 May 2026 00_04_26.png`
**Canvas:** 64×64
**Credit:** ~6

## V7 → V8 değişim (text triggerına direkt cevap)

| V7 sorunu | V8 fix |
|---|---|
| Class isimleri prompt'ta açık ("Warblade —", "Ranger —") → AI sprite'a label çiziyor | **Class isimleri TAMAMEN KALDIRILDI.** Sadece tarif. Identity Karar #80 silhouette bible tarifinden anchor. |
| Negative text guards baskın değildi | NEGATIVE'e ek: "any letters, any words, any alphabet characters, any glyphs, any typography". Prompt **başına** "NO TEXT" agresif vurgu. |
| Reference description'da text engellemesi açık değildi | Reference description'a "the generated output must contain ZERO text or labels" eklendi. |

Class identity prompt **tarifinden** anlaşılır — class diversity tarif anchor'larıyla zaten farklı.

---

## Reference image description (yapıştır)

```
Use this reference for: chibi proportions, weapon-ready idle pose philosophy, dark gritty Salt-and-Sanctuary mood, and palette. The generated output must contain ZERO text, ZERO words, ZERO labels, ZERO character names, ZERO captions. Sixteen visually distinct characters, no two share identity. Do not copy any single character from the reference verbatim.
```

---

## YAPIŞTIRMA — Main prompt V8

```
ABSOLUTE RULE: the generated image must contain NO text, NO words, NO letters, NO labels, NO character names, NO captions, NO titles, NO numbers. The image is pure pixel art with zero typography of any kind.

Pixel art chibi character batch — 16 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Chibi proportions: oversized head ~40% of total height, broad shoulders, short stubby legs ~25% height. Sprite fills 85-90% of canvas vertically. Camera: high top-down 30-35 degrees, all sprites face south (direct front view), both eyes and full face visible, shoulders horizontally even.

WEAPON-READY IDLE RULE: the first ten characters are playable classes. Each one stands in a weapon-ready idle stance with hands positioned naturally as if about to grip or use their weapon — but the weapon itself is NOT drawn in this sprite. Hands look natural and relaxed in the ready position, fingers loosely curled where a grip would be, no ghostly or transparent weapon outline. The eighth character is an exception: bare fists ARE the weapon, fully clenched. The sixth character retains an empty black sheath silhouette at the left hip.

MOB RULE: the last six characters are non-humanoid hostile entities. Fused weapons, claws, orbs, hazards are permitted.

Palette: muted desaturated earth and metal tones for non-rift entities. Rift cyan #00FFCC and violet #5A2A8A appear only on rift-affiliated entities (the third character, the ninth, the tenth, and all six mobs).

The sixteen characters in order:

— Broad-shouldered stocky bearded male, dark brown leather plate armor with brass belt buckle. Both hands held apart at chest level as if gripping an invisible two-handed greatsword pointed downward in low guard.

— Slim athletic female, ash-blonde long braid, green hooded scout cloak over leather tunic, empty quiver on back. Left hand raised at shoulder height as if gripping an invisible bow vertically, right hand drawn back near the right ear as if having just released an arrow.

— Narrow whip-thin male, short dark messy hair falling to eyebrows, clean-shaven face with no scar and no tattoo, near-black deep purple cropped armor with void purple shoulder accents and neon teal sash. Both hands held at hip level in tight reverse-grip stance as if holding twin invisible daggers blade-down, no glow on the invisible blades, no embedded crystal effect.

— Slim upright female, honey-blonde chin-length bob, deep indigo cropped robe with golden cuffs. Right palm raised open and upward at chest level as if cradling an invisible rune disc, left hand relaxed at side.

— Bulkier and taller than the first male, massive hunched build, wild dark-red shoulder-length hair, scarred bare arms, dark blood-red rough hide armor with crimson torn cloth wraps. Both fists clenched at hip level in low predatory crouch as if gripping invisible dual hatchets, shoulders rolled forward.

— Slim narrow upright male, long black hair tied in a high topknot, dark navy near-black kimono with pale gold collar trim, a plain black empty sheath silhouette clearly visible at left hip. Right hand resting on the empty sheath grip as if about to draw, body angled slightly three-quarter.

— Short slim athletic female, deep auburn-red shoulder-length hair, dark grey-purple trench coat with fire orange collar accents, dark leather thigh holsters visible at both hips. Both hands relaxed at sides hovering just above the holsters as if ready to quick-draw twin pistols.

— Heavily muscular wide male, shaved head, no shirt, bare muscular torso, dark brown leather hand-wrappings tightly bound on both fists, fire orange waistband, bare feet. Both fists clenched solid and raised to chin level in tight boxing guard, knees softly bent in low fight stance.

— Tall lean female, long platinum-white hair flowing over shoulders, very dark green-black robe with neon green trim, hood down. Left hand at chest level palm-up as if holding an invisible soul lantern, right arm raised diagonally upward palm-open in conducting gesture.

— Short slim female, long dark purple hair partially covering one eye, dark purple-black hooded robe with hood up and dark red trim. Left arm bent across chest cradling an invisible book at heart level, right hand at side as if gripping an invisible vertical staff.

— Non-humanoid floating support entity, torso suspended in mid-air with torn cloth trailing downward where lower body would be, both arms outstretched horizontally wide, thin cyan halo ring glowing behind the head, violet warmth suffuses the inner cloth folds, sorrowful hollow-eyed face.

— Non-humanoid skeletal crystalline creature, jagged teal-cyan crystal shards jutting from shoulders spine and forearms, muted dark grey-blue body between shards, hunched four-limbed predatory stance with long clawed crystal hands.

— Non-humanoid heavy humanoid creature with self-flagellant aesthetic, chain or spike wrappings on both arms, dark crimson-grey armor plating, glowing red aura ring at feet as a flat disc on the ground, clawed gauntlets, hooded face hidden in shadow, forward-leaning tank stance.

— Non-humanoid hooded oracle in dark teal-grey robes, glowing rift cyan pattern markings clearly visible on face cheeks and both open palms, both arms raised slightly forward in ominous warning gesture, hood drawn but face markings glow through.

— Non-humanoid heavy melee golem, large wide blocky creature filling the canvas, cracked stone or corroded metal body with dark void seams emitting faint violet inner glow through the cracks, hollow pitch-black eye sockets, massive fists lowered at sides like clubs, slow lumbering hunched stance.

— Non-humanoid hooded cultist with hood drawn low completely hiding the face in total shadow, both hands cradling a swirling violet rift fracture orb at chest height, the orb emits faint cyan tendrils drifting outward, tattered dark vestments, hunched casting posture.

Negative prompt: text, words, letters, alphabet, glyphs, typography, captions, labels, character names, sprite names, sprite labels, title cards, watermark, signatures, numbers, digits, grid headers, name tags, written language of any kind, 3d render, soft shading, blur, smooth gradient, isometric projection, anti-aliasing, soft edges, realistic proportions, painterly style, three-quarter shoulder turn, drawn weapon visible in hand, sword in hand, axe in hand, dagger in hand, pistol in hand, bow in hand, staff in hand, lantern in hand, ghostly transparent weapon outline, weapon glow in empty hands, identical armored male warrior repeated.
```

---

## QC her sprite için

### Critical
- [ ] **NO TEXT** — hiçbir sprite altında/üstünde/yanında isim/etiket/yazı yok (V7 sorunu)
- [ ] **Weapon-ready pose** — eller doğal silah tutar, hayalet silah yok
- [ ] **Identity diversity** — 10 class gözle ayırt edilebilir

### Karakterler (1-10 sırayla)
- [ ] 6. Ronin: empty sheath silüeti hip'te
- [ ] 8. Brawler: shirtless muscular + boxing fists at chin
- [ ] Build sırası: 5 (Ravager) > 1 (Warblade) > 8 (Brawler) > 3 (Shadowblade) ≈ 6 (Ronin)
- [ ] Gender: 1/3/5/6/8 erkek, 2/4/7/9/10 kadın

### Moblar (11-16)
- [ ] Non-humanoid silüet
- [ ] Karar #98 palette zorlu

### Tüm 16
- [ ] Chibi proportions head ~40%
- [ ] 64×64 canvas
- [ ] 85-90% sprite fill
- [ ] 1px black outline
- [ ] Transparent background
- [ ] Front-facing 35° top-down

---

## Workflow V8

1. Pro UI Reference Image: ChatGPT görselini ekle
2. Reference description: yukarıdaki kısa metni yapıştır (zero text vurgusu var)
3. Main prompt: V8 blok'unu yapıştır (class isimleri YOK, başında ABSOLUTE RULE var)
4. Canvas: **64×64**
5. Generate → 16 variation
6. QC kritik check: text var mı?
7. Drift varsa hedefli re-roll

---

## Eğer V8 de text üretirse — Plan B

Class isim referansı kalıp sızıyor olabilir (tarif anchor'larından). Plan B:
- Reference image'ı KALDIR (grid format sinyalini kes)
- "ABSOLUTE RULE" maddesini 2x tekrarla (prompt başında + sonunda)
- Single sprite mode'a düş — 16 ayrı Pro UI run, her biri tek karakter (~96 credit, ama text bias kesin sıfır)

Bunu V8 test'i çalışmazsa devreye al.
