# Create Image Pro — Batch 16 v7 (weapon-ready poses, simplified, no text bias)
**Karar refs:** #71 weapons in hand (pose spirit) / #80 Class Silhouette Bible / #98 rift cyan+violet / #100 chibi 64x64 / #123 weapon decouple body (silahsız sprite ama weapon-ready POZ)
**Reference image:** `C:\Users\ydbil\Downloads\ChatGPT Image 15 May 2026 00_04_26.png` (kullanıcının ChatGPT'ye çizdirdiği 16-sprite style guide)
**Canvas:** 64×64 for ALL 16 variations
**Credit:** ~6

## V6 → V7 değişim (kullanıcı geri bildirimine direkt cevap)

| V6 sorunu | V7 fix |
|---|---|
| Sprite üstünde "1) Warblade" gibi text yazıları çıktı | Numbering KALDIRILDI (bullet point yok, "Variation A:" yok, sadece isim+tarif). NEGATIVE'e text guards eklendi. |
| Idle pozlar "hands on belt / behind back / crossed arms" → weapon attach edince anatomi bozuk | TÜM pozlar **weapon-ready idle stance**: eller silah tutar pozisyonda durur, silah kendisi sprite'ta YOK. Unity attach anatomik oturur. |
| Class identity 5-7 satır narrative → drift | Her class **max 2 cümle**, tek nefes: `[Build + hair + outfit]. [Weapon-ready pose].` |
| Reference image yokluğu/yanlış kullanımı | ChatGPT görseli reference olarak feed edilir → style + chibi + weapon-ready poses inherit (kullanıcı zaten o estetiği istiyor). |

---

## Reference image — ChatGPT görseli kullanım

Pro UI'da Reference Image field'a yapıştır: `C:\Users\ydbil\Downloads\ChatGPT Image 15 May 2026 00_04_26.png`

Reference Image description field'a şu kısa metni yapıştır:

```
Match the chibi proportions, weapon-ready idle poses, dark gritty Salt-and-Sanctuary mood, and color palette shown in this reference. Each of the 16 variations is distinct — do not copy any single character verbatim, but maintain the overall style, lighting, and stance philosophy.
```

---

## YAPIŞTIRMA — Main prompt V7 (kopyala-yapıştır)

```
Pixel art chibi character batch — 16 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Chibi proportions: oversized head ~40% of total height, broad shoulders, short stubby legs ~25% height. Sprite fills 85-90% of canvas vertically. Camera: high top-down 30-35 degrees, all sprites face south (direct front view), both eyes and full face visible, shoulders horizontally even.

WEAPON-READY IDLE RULE: characters 1-10 are playable classes. Each one stands in a weapon-ready idle stance with hands positioned naturally as if about to grip or use their weapon — but the weapon itself is NOT drawn in this sprite (weapons attach later as separate sprites in Unity). Hands look natural and relaxed in the ready position, fingers loosely curled where a grip would be, no ghostly or transparent weapon outline. Brawler is an exception: bare fists ARE the weapon, fully clenched. Ronin retains an empty black sheath silhouette at the left hip.

MOB RULE: characters 11-16 are non-humanoid hostile entities. Fused weapons, claws, orbs, hazards are permitted (mob clause).

Palette: muted desaturated earth and metal tones for non-rift classes. Rift cyan #00FFCC and violet #5A2A8A appear only on rift-affiliated entities (Shadowblade, Summoner accents, Hexer accents, and all 6 mobs).

Sixteen characters in this batch:

Warblade — broad-shouldered stocky bearded male, dark brown leather plate armor with brass belt buckle. Both hands held apart at chest level as if gripping an invisible two-handed greatsword pointed downward in low guard.

Ranger — slim athletic female, ash-blonde long braid, green hooded scout cloak over leather tunic, empty quiver on back. Left hand raised at shoulder height as if gripping an invisible bow vertically, right hand drawn back near the right ear as if having just released an arrow.

Shadowblade — narrow whip-thin male, shaved head with a black scar tattoo over right eye, near-black deep purple cropped armor with void purple shoulder accents and neon teal sash. Both hands held at hip level in tight reverse-grip stance as if holding twin invisible daggers blade-down.

Elementalist — slim upright female, honey-blonde chin-length bob, deep indigo cropped robe with golden cuffs. Right palm raised open and upward at chest level as if cradling an invisible rune disc, left hand relaxed at side.

Ravager — bulkier and taller than Warblade, massive hunched build, wild dark-red shoulder-length hair, scarred bare arms, dark blood-red rough hide armor with crimson torn cloth wraps. Both fists clenched at hip level in low predatory crouch as if gripping invisible dual hatchets, shoulders rolled forward.

Ronin — slim narrow upright male, long black hair tied in a high topknot, dark navy near-black kimono with pale gold collar trim, a plain black empty sheath silhouette clearly visible at left hip (katana absent). Right hand resting on the empty sheath grip as if about to draw, body angled slightly three-quarter.

Gunslinger — short slim athletic female, deep auburn-red shoulder-length hair, dark grey-purple trench coat with fire orange collar accents, dark leather thigh holsters visible at both hips (empty). Both hands relaxed at sides hovering just above the holsters as if ready to quick-draw twin pistols.

Brawler — heavily muscular wide male, shaved head, no shirt (bare muscular torso), dark brown leather hand-wrappings tightly bound on both fists, fire orange waistband, bare feet. Both fists clenched solid and raised to chin level in tight boxing guard, knees softly bent in low fight stance.

Summoner — tall lean female, long platinum-white hair flowing over shoulders, very dark green-black robe with neon green trim, hood down. Left hand at chest level palm-up as if holding an invisible soul lantern, right arm raised diagonally upward palm-open in conducting gesture.

Hexer — short slim female, long dark purple hair partially covering one eye, dark purple-black hooded robe with hood up and dark red trim. Left arm bent across chest cradling an invisible grimoire at heart level, right hand at side as if gripping an invisible vertical staff.

Spire Choirling — non-humanoid floating support entity, torso suspended in mid-air with torn cloth trailing downward where lower body would be, both arms outstretched horizontally wide, thin cyan halo ring glowing behind the head, violet warmth suffuses the inner cloth folds, sorrowful hollow-eyed face.

Shard Walker — non-humanoid skeletal crystalline creature, jagged teal-cyan crystal shards jutting from shoulders spine and forearms, muted dark grey-blue body between shards, hunched four-limbed predatory stance with long clawed crystal hands.

Penitent Bruiser — non-humanoid tank, heavy humanoid creature with self-flagellant aesthetic, chain or spike wrappings on both arms, dark crimson-grey armor plating, glowing red aura ring at feet as a flat disc on the ground, clawed gauntlets, hooded face hidden in shadow, forward-leaning tank stance.

Riftbound Augur — non-humanoid hooded oracle in dark teal-grey robes, glowing rift cyan pattern markings clearly visible on face cheeks and both open palms, both arms raised slightly forward in ominous warning gesture, hood drawn but face markings glow through.

Hollow Hulk — non-humanoid heavy melee golem, large wide blocky creature filling the canvas, cracked stone or corroded metal body with dark void seams emitting faint violet inner glow through the cracks, hollow pitch-black eye sockets, massive fists lowered at sides like clubs, slow lumbering hunched stance.

Rift Acolyte — non-humanoid hooded cultist with hood drawn low completely hiding the face in total shadow, both hands cradling a swirling violet rift fracture orb at chest height, the orb emits faint cyan tendrils drifting outward, tattered dark vestments, hunched casting posture.

Negative prompt: text, words, labels, numbers, captions, character names, watermark, sprite labels, title cards, grid headers, 3d render, soft shading, blur, smooth gradient, isometric projection, anti-aliasing, soft edges, realistic proportions, painterly style, three-quarter shoulder turn, drawn weapon visible in hand, sword in hand, axe in hand, dagger in hand, pistol in hand, bow in hand, staff in hand, lantern in hand, ghostly transparent weapon outline, weapon glow in empty hands, identical armored male warrior repeated, generic male fighter body for every variation.
```

---

## QC her sprite için

### Karakterler (1-10)
- [ ] **Text yok**: sprite üstünde isim/numara/etiket yazısı görünmüyor
- [ ] **Weapon-ready pose**: eller silah tutar pozisyonda doğal, "hayalet silah" outline yok
- [ ] **Identity diversity**: 10 sprite gözle ayırt edilebilir (build + hair + outfit)
- [ ] **6 Ronin**: empty sheath silüeti hip'te görünür
- [ ] **8 Brawler**: shirtless muscular + boxing fists at chin
- [ ] **Build sırası**: Ravager > Warblade > Brawler > Shadowblade ≈ Ronin
- [ ] **Gender**: 1/3/5/6/8 erkek, 2/4/7/9/10 kadın

### Moblar (11-16)
- [ ] Non-humanoid silüet
- [ ] Karar #98 palette: cyan #00FFCC + violet #5A2A8A açık

### Tüm 16
- [ ] **Chibi proportions**: head ~40% (V6'da düşüktü)
- [ ] 64×64 canvas
- [ ] 85-90% sprite fill
- [ ] 1px black outline, no AA
- [ ] Transparent background
- [ ] Front-facing 35° top-down

---

## Workflow V7

1. Pro UI Reference Image field'a ChatGPT görselini ekle (`ChatGPT Image 15 May 2026 00_04_26.png`)
2. Reference description'a yukarıdaki kısa metni yapıştır
3. Main prompt blok'unu Pro UI prompt field'a yapıştır
4. Canvas: **64×64**
5. Generate → 16 variation
6. QC checklist uygula
7. Drift'li sprite varsa "regenerate just this one" hedefli re-roll
8. 16 onaylanınca → her birini MCP `create_character` reference'a ver → 8-direction sprite sheet

---

## V6 arşivlenir mi?

Evet — V7 başarılı çıkarsa V5 ve V6 `_archive/v5_v6/` altına taşınır. V5 = warblade.png reference identity drift kaynağı, V6 = wrong pose + text bias. V7 = canonical Pro batch prompt.
