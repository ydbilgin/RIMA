# Create Image Pro — Batch 16 v6 (identity-first, no-reference-bias, pose-based weaponless)
**Karar refs:** #71 positive-only / #80 Class Silhouette Bible / #98 rift cyan #00FFCC + violet #5A2A8A / #99 no hand mention / #100 chibi 64x64 / #123 weapon decouple
**Canvas:** 64×64 for ALL 16 variations
**Credit:** ~6 (single generation)

## V5 → V6 değişim (V5 sorunlarına direkt cevap)

| V5 sorunu | V6 fix |
|---|---|
| Reference image (warblade.png) tüm 16 sprite'a "armored male" identity drift'i taşıdı | **Reference image kaldırıldı / opsiyonel.** Angle prompt'tan explicit verilir. |
| "Hollow cradle empty space" abstract → AI silah hayaleti çiziyor | **Pose semantic-based weaponless**: hands-on-belt / hands-behind-back / fists-at-chin. Görünür silah yok ama poz absurd "hollow grip" değil. |
| Class identity prompt sonunda → güç kaybetti | **Identity prompt'un BAŞINA**: her variation `[Class] — [silhouette hook]: [build] + [hair/face] + [outfit]. Pose: [...]` formatında. |
| Build differentiation zayıf (Warblade ≈ Ravager ≈ Ronin) | **Build words explicit** her sınıf için (slim narrow / wide broad / bulky muscular / tall lean), karşılaştırma cümleleri ("bulkier than Warblade"). |
| Chibi proportions tutmadı (head ~30% görünüyor) | **Reference yok → prompt baskın.** "Oversized head 40% of total height" + "stubby legs ~25% of total height" explicit. |

---

## Reference image (V6 OPTIONAL — önerilmez, kullanılırsa keskin ignore)

V6 önerisi: **reference image kullanma.** Pro UI'de Reference Image field BOŞ.

Eğer warblade.png yine de eklenecekse (eski iş akışı), Reference Image description field'a şu metni yapıştır:

```
CRITICAL: This reference is provided ONLY for camera angle (35° high top-down) and facing direction (south, direct front). IGNORE every other aspect — outfit, armor, weapon, hair, beard, build, body proportions, pose, color palette, identity, race, gender. Each variation has its own identity from the main prompt; the reference must NOT bleed body or identity features into any variation. Treat the reference as a faceless camera-only guide.
```

---

## YAPIŞTIRMA — Main prompt V6 (kopyala-yapıştır)

```
# Batch 16 — RIMA v6 Final Prompt (identity-first, pose-based weaponless)
# Karar #80 Silhouette Bible, Karar #100 chibi, Karar #123 weapon decouple, Karar #98 rift palette

GLOBAL STYLE BLOCK (apply to ALL 16 variations):

Canvas: 64×64 pixels for every variation (characters AND mobs). Single batch, no canvas-size mixing.

Pixel art chibi style. Visual reference family: Hades + Hyper Light Drifter + Into Samomor (Sang Hendrix RPG Maker MZ) + Salt and Sanctuary dark gritty mood.

Chibi proportions, strictly enforced:
- Head occupies the top ~40% of the total sprite height (oversized).
- Torso occupies the middle ~35%.
- Legs occupy the bottom ~25% (stubby, short).
- Shoulders broad relative to waist.
- Sprite fills 85-90% of the 64×64 canvas vertically; transparent background.

Camera angle: high top-down ~30-35° tilt. Facing: south, direct front. Both eyes and full face visible. Shoulders horizontally even; no three-quarter turn.

Outline & shading: thick 1px solid black pixel outline. Hard pixel edges. No anti-aliasing, no gradients, no painterly blending, no soft shadows. Flat cell shading only — at most 2 tonal steps per color region.

Palette family: muted desaturated Salt-and-Sanctuary Shattered Keep base tones, dark gritty atmosphere. Rift accents (cyan #00FFCC + violet #5A2A8A) appear ONLY on rift-affiliated entities (Shadowblade void purple, Summoner cyan trim, Hexer dark red, and all six mobs). Non-rift classes stay in earth/metal/leather palette.

WEAPONLESS BODY RULE (Karar #123 Yol A):
All 10 playable classes (variations 1-10) must have body sprites with NO visible weapon. Weapons attach at runtime as separate sprites in Unity. Achieve weaponlessness through naturally weapon-free poses (hands resting on belt, crossed arms, fists at chin, hands behind back, palm-up casting, etc.) — NOT through "invisible weapon hollow grip" hand shapes. Hands look natural and relaxed where no weapon is held. Two exceptions: Brawler (bare fists ARE the weapon — Karar #123 exception), Ronin (body retains an empty sheath silhouette at the left hip but the katana itself is absent).

MOB RULE: variations 11-16 are non-humanoid hostile entities. Fused/integrated weapons, claws, hazards, and orbs are permitted (Karar #123 mob clause). Each mob has a distinctly non-humanoid silhouette readable at 16px thumbnail.

SILHOUETTE READABILITY: every sprite must be identifiable by silhouette alone at 16px. No two variations may share the same silhouette family.

VARIATIONS (identity-first format — [Class] — [hook]: [build + hair + outfit]. Pose: [...]):

1) Warblade — heavy frontline swordsman: WIDE broad-shouldered stocky build, short brown messy hair and a thick chestnut beard, dark brown plated leather armor #4F3A2C with brass buckle accents #C09455 across the chest, cold blue cloth wrap #7BA7BC at the waist. Pose: both hands rest at waist level on the belt buckle, arms relaxed, weight evenly planted, low guard stance. Silhouette hook: wide square shoulders + bearded face + hands-on-belt.

2) Ranger — agile rift hunter, FEMALE: SLIM tall athletic build, ash-blonde long braid swept over one shoulder, dark forest-green hooded scout cloak #2A3520 over leather tunic, cold blue trim #7BA7BC at collar, amber accent #FFB000 on quiver strap (empty quiver visible on back). Pose: arms loosely crossed in front of the chest in alert listening stance, head slightly tilted as if hearing distant sound. Silhouette hook: hooded female + long blonde braid + crossed arms.

3) Shadowblade — phase-through assassin, MALE: NARROW whip-thin compact build (slimmer than Ranger), shaved head with a vertical black void scar tattoo running over the right eye, near-black deep purple cropped armor #1A1025 with void-purple shoulder accents #5A2A8A, neon teal sash #00FFCC at the waist. Pose: arms crossed tight at the chest with elbows tucked back, palms hidden in opposite armpits, predatory still stance. Silhouette hook: narrow shaved-head + face scar + tight crossed arms.

4) Elementalist — academic mage, FEMALE: SLIM upright build, warm honey-blonde hair in a chin-length bob, deep indigo cropped robe #2A1F35 with golden cuff accent #FFD700 at the sleeves, knee-high boots. Pose: right palm faced upward at chest level open and empty (no item, no glow in this sprite — Unity VFX will add the rune disc at runtime), left hand relaxed at side. Silhouette hook: cropped robe female + honey-blonde bob + upward-open palm at chest.

5) Ravager — berserker, MALE: BULKIER and TALLER than Warblade, massive hunched broad-shouldered build, wild dark-red shoulder-length hair, no beard, scarred bare arms, dark blood-red rough hide armor #3A1A0A with crimson red accent #D43F3F on torn cloth wraps. Pose: shoulders rolled forward in aggressive predator crouch, both fists clenched solid at hip level (knuckles bare and visible — these are not weapon grips, just clenched anger), feral lean. Silhouette hook: bigger than Warblade + wild red hair + hunched aggressive crouch.

6) Ronin — iaido duelist, MALE: SLIM narrow upright build, long black hair tied in a high topknot, clean-shaven, dark navy near-black kimono-style garment #1A1A2E with pale gold collar trim #C8A87A, a plain black empty sheath silhouette clearly visible on the left hip (the katana itself is absent — will attach later). Pose: hands clasped formally behind the back, body angled slightly three-quarter to the camera but still facing south, calm dueling readiness. Silhouette hook: topknot + plain empty sheath on hip + hands behind back.

7) Gunslinger — kinetic dueller, FEMALE: SHORT slim athletic build, deep auburn-red shoulder-length hair, dark grey-purple trench coat #1A1520 cinched at the waist, fire orange accent #FF4400 at collar lapels and cuffs, dark leather thigh holsters visible at both hips (holsters are empty — pistols attach later). Pose: arms relaxed at sides with both palms open and visible, slight weight-shifted micro-crouch onto the right leg, gunslinger ready stance. Silhouette hook: auburn hair female + trench coat with empty thigh holsters + open palms.

8) Brawler — bare-fisted boxer, MALE: HEAVILY muscular wide compact build (Karar #123 exception — fists ARE the weapon, intentionally weaponless without decouple), shaved head, no beard, bare muscular torso (no shirt), dark brown leather hand-wrappings #2A1A10 tightly wound on both fists from knuckles to mid-forearm, fire orange cloth waistband #FF8C00, bare feet. Pose: both fists clenched and raised to chin level in tight boxing guard, knees softly bent, low fight stance. Silhouette hook: shirtless muscular + hand-wrapped fists raised at chin + boxing crouch.

9) Summoner — necromancer conjurer, FEMALE: TALL lean build, long platinum-white hair flowing over the shoulders, very dark green-black hooded robe with hood DOWN #0A1A0A, neon green trim #00FF88 at the hem and cuffs, deep indigo inner lining visible. Pose: left hand at chest level palm-up holding nothing (the soul lantern attaches at runtime), right arm raised diagonally upward with palm open in conducting gesture as if directing an unseen choir. Silhouette hook: hood-down platinum hair + chest palm-up + raised conducting arm.

10) Hexer — patient witch, FEMALE: SHORT slim build, long dark purple hair partially covering one eye, dark purple-black hooded robe with hood UP #1A0A1A (hood casts shadow over upper face but eyes still glow faintly visible), dark red trim #8B0000 at the hood edge and sleeves, no other accent. Pose: left arm crossed over the chest at heart level cradling absolutely nothing (grimoire attaches later), right arm hangs straight down at the side with the hand relaxed and empty. Silhouette hook: hood-up + dark purple visible hair + arm-cradle-at-chest posture.

11) Spire Choirling — non-humanoid hostile support: torso-only floating figure suspended in mid-air with torn cloth trailing downward where lower body would be, both arms outstretched horizontally in eerie welcoming posture, thin cyan halo-ring #00FFCC glowing behind the head, violet warmth #5A2A8A suffuses the inner folds of the trailing cloth, hollow black eye sockets, sorrowful former-singer face. Silhouette hook: floating torso + halo ring + outstretched arms + cloth trail tail.

12) Shard Walker — non-humanoid hostile fast melee: skeletal crystalline humanoid creature, jagged teal-cyan crystal shards #00FFCC jutting from shoulders, spine, and forearms, muted dark grey-blue body #2A3545 between the shards, four-limbed upright hunched posture, long clawed crystalline hands, predatory forward lean. Silhouette hook: jagged crystal shoulder protrusions + hunched gait + clawed crystal hands.

13) Penitent Bruiser — non-humanoid hostile tank: heavy humanoid creature with self-flagellant aesthetic, chain or spike wrappings on both arms, dark crimson-grey armor plating #3A1A1A, glowing red aura ring #D43F3F at feet (heal-debuff indicator visible as a flat disc on the ground), stoic forward-leaning tank posture, clawed gauntlets, hooded face hidden in shadow. Silhouette hook: wide heavy torso + red aura disc at feet + chain-wrapped arms.

14) Riftbound Augur — non-humanoid hostile ranged oracle: corrupted oracle humanoid in dark teal-grey hooded robes #1A2B2B, glowing rift cyan #00FFCC pattern markings clearly visible on face cheeks and both palms, both arms slightly raised in ominous warning gesture with palms forward, hood drawn but face markings glow through. Silhouette hook: raised warning arms + glowing cyan face and palm markings + hooded robe.

15) Hollow Hulk — non-humanoid hostile heavy melee: large golem-like creature wide and blocky filling most of the canvas, cracked stone or corroded metal body #2A2A2A with dark void seams emitting faint violet #5A2A8A inner glow through the cracks, no visible eyes (hollow pitch-black sockets), massive fists lowered at sides like clubs, slow lumbering stance, slightly hunched. Silhouette hook: square-wide blocky torso + violet crack glow + hollow eye sockets.

16) Rift Acolyte — non-humanoid hostile area-denial caster: robed cultist humanoid figure with hood drawn LOW over hidden face (no facial features visible — total shadow), both hands cradle a swirling violet rift fracture orb #5A2A8A at chest height, the orb emits faint cyan tendrils #00FFCC drifting outward, tattered dark vestments #1E1E2A, slightly hunched casting posture. Silhouette hook: hooded hidden face + cradled violet orb at chest + cyan tendrils.

NEGATIVE PROMPT (style + identity drift guards):
3d render, soft shading, blur, smooth gradient, isometric projection, anti-aliasing, soft edges, realistic proportions, painterly style, three-quarter shoulder turn, identical armored male warrior, generic male fighter body for every variation, repeated identity, repeated outfit across variations, visible weapons on classes 1-10, hollow ghostly weapon outline, transparent weapon, weapon glow in empty hands.
```

---

## QC her sprite için (üretim sonrası)

### Karakterler (1-10)
- [ ] **Identity ayrı**: 10 sprite gözle ayırt edilebilir mi (build + hair + outfit kombinasyonu)
- [ ] **Body silahsız + natural pose**: ellerde ne silah ne de "hollow grip" hayaleti yok, eller doğal pozisyonda
- [ ] **6 Ronin**: empty sheath silüeti hip'te görünür mü
- [ ] **8 Brawler**: shirtless muscular + hand-wrapped raised fists
- [ ] **10 Hexer**: hood up + arm-cradle posture (grimoire YOK)
- [ ] **Build differentiation**: Ravager > Warblade > Brawler > Shadowblade ≈ Ronin order'ı görsel
- [ ] **Gender visible**: 1/3/5/6/8 erkek vs 2/4/7/9/10 kadın net

### Moblar (11-16)
- [ ] Non-humanoid silhouette (humanoid'den ayrılabilir)
- [ ] Karar #98 palette: cyan #00FFCC + violet #5A2A8A açık görünüyor
- [ ] 15 Hollow Hulk crack glow violet
- [ ] 11 Spire Choirling halo + cloth trail

### Tüm 16
- [ ] **Chibi proportions**: head ~40% height (V5'in zayıflığı)
- [ ] 64×64 canvas
- [ ] 85-90% sprite fill
- [ ] 1px black outline, no AA
- [ ] Transparent background
- [ ] Front-facing 35° top-down

---

## Workflow V6

1. Pro UI'da Reference Image field'ı **BOŞ BIRAK** (V6 önerisi)
2. Yukarıdaki **Main prompt YAPIŞTIRMA blok**unu kopyala
3. Pro UI prompt field'a yapıştır
4. Canvas: **64×64**
5. Generate → 16 variation
6. QC checklist uygula
7. Sorunlu sprite varsa: Pro UI'da o variation'ı "regenerate just this one" ile re-roll et (genelde 2-3 sprite drift'i kalır, hedefli re-roll ucuz)
8. 16 sprite onaylandığında her birini `create_character` MCP'ye reference olarak ver → 8-direction sprite sheet otomatik

---

## V5 dosyası

`STAGING/create_image_pro_SINGLE_PROMPT.md` — V5 archived olarak duruyor, V6 üretimi başarılı olursa V5 dosyası `_archive/v5/` altına taşınır.
