# Karakter Üretim — LIVE (v10 chibi NLM canonical, silahsız body)

> **LIVE version: v10** — NLM canonical identity + chibi 3-4 head + 30-35° + silahsız body
> **Image #9 analiz sonucu:** Image #9 visual style = chibi/demi-chibi aile (4-5 head, 35-40° angle). Prompt'taki "mature 5-6 head 60°" etiketi PixelLab output'una uymadı; gerçek PixelLab output chibi pixel art bias'ı yüzünden chibi aile çıkıyor.
> **Karar #74** (chibi 64×64) + **Karar #100** (chibi RESTORE + 35°) + **Karar #144** (silahsız body) **LIVE LOCK korunur**
> **v11 mature/60° denemesi:** archive'a alındı (`_archive/character_production_prompts_explore_v11_mature.md`); bu yol yanlış strategi olarak değerlendirildi
> **Eski versiyonlar:** `_archive/character_production_prompts_archive_v6_v7_v8_v9.md`

---

## Settings (PixelLab UI)

- Mode: **PixelLab → Create Image Pro** (direkt sıfırdan üretim, style ref YOK)
- **Style ref panel: BOŞ** — kullanıcı tercihi: style ref kullanılmaz, tüm sinyal prompt + UI setting'den
- Camera (UI dropdown): **High top-down** (zorunlu, prompt + UI iki katman sinyal)
- Output: **64×64** (per-character) veya 4×4 contact sheet (16-variation batch)
- Describe reference kutusu: **BOŞ** (style ref panel boş olduğu için bu kutu da kullanılmaz)

---

## REFERENCE DESCRIPTION

**Style ref kullanılmıyor — bu kutu BOŞ bırakılır.** Tüm chibi proportions + açı + style sinyali aşağıdaki MAIN PROMPT içinden gelir.

---

## MAIN PROMPT (PixelLab main prompt kutusu — tek blok, NLM canonical 10 karakter)

> **Önemli:** Style ref boş olduğu için açı + chibi proportions vurgusu prompt'un EN BAŞINDA, agresif 5-katmanlı sinyal olarak yazılır. PixelLab başlangıç token'lara daha çok ağırlık verir.

```
ABSOLUTE CAMERA RULE: high top-down camera angle, EXACTLY 30 to 35 degrees downward tilt from horizontal, ARPG-style overhead bird's-eye perspective like Hades and Hyper Light Drifter and Hammerwatch. All sprites face south (body oriented toward the bottom of the screen, camera high above looking downward). Each character is viewed clearly from above at a diagonal — the top plane of the head and hair crown is visible as a rounded shape, shoulders are visibly angled diagonally toward the camera as seen from above, head is slightly compressed vertically due to the downward camera tilt, torso height shortened by foreshortening. Feet small and low in the sprite. Soft oval ground shadow beneath each character's feet. NOT a flat front view, NOT a side profile, NOT a three-quarter southeast view, NOT a character-select portrait, NOT an isometric projection, NOT a pure 90-degree bird's-eye top-down.

ABSOLUTE PROPORTION RULE — CHIBI PIXEL ART, BIG HEAD MANDATORY: extreme chibi proportions like Hammerwatch / Hyper Light Drifter / Tunic / Eastward. Total body height = exactly 3 to 4 head heights, NEVER 5 or 6. Oversized cartoon head dominates the upper half of the sprite — the head must read as approximately 40 to 45 percent of total sprite height, large enough that the face features (eyes, mouth, hair shape) are clearly readable at 64x64 pixel resolution. Broad shoulders directly below the big head, no neck or only 1-pixel neck. Short stubby legs about 20 to 25 percent of total height. Hands and feet small and chunky. The face is the largest single readable element of the sprite — eyes, eyebrows, nose, mouth all visible as distinct pixel clusters. Sprite fills 85 to 90 percent of canvas vertically, full body visible from top of head to feet. The body type is compact and cute-tough, NOT tall, NOT slim, NOT realistic, NOT mature adult, NOT super-deformed 2-head.

ABSOLUTE TEXT RULE: the generated image must contain NO text, NO words, NO letters, NO labels, NO character names, NO captions, NO numbers, NO typography of any kind.

Every character is completely weapon-free.

Pixel art chibi character batch — 10 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color.

WEAPON-FREE RULE: each character carries no weapons of any kind. The body is completely free of swords, axes, bows, daggers, pistols, staves, orbs, lanterns, grimoires, shields, sheaths, scabbards, holsters, quivers, weapon belts, weapon harnesses, weapon straps. Both hands hold nothing, palms open and clearly empty at the sides. The back, belt, shoulders, hips, and thighs are completely empty of weapons or weapon attachments.

POSE: all characters stand in a calm balanced idle pose, both feet on the ground at shoulder width, arms relaxed and open at the sides, head looking forward, face calm and neutral, body symmetric.

The ten characters in order, all adult, all weapon-free:

— Adult male, light tan skin, dark short hair, dark masculine beard, weary grounded patient face. Broad-shouldered heavy-build man. He wears dark brown leather armor across his torso with brass buckle accents at the chest and waist, a weary veteran look not a shiny knight. Palette: dark brown leather body tones dominant, brass accents on buckles and trim, faint cold blue highlight details on edges.

— Adult female, pale skin, warm honey-blonde hair in a neat low bun with loose temple strands, curious ember-bright face. Slim refined woman with slim upper body. She wears a cropped sleeveless dusty indigo top showing a small bare midriff strip, a flowing high-waisted deep teal skirt, dark fitted tights, high boots, and a cream sash tied at the waist. Palette: dusty indigo top dominant, deep teal skirt accent, cream sash and trim, warm gold accent highlights, honey-blonde hair.

— Adult male, Asian features and skin tone, black hair tied in a low knot at the back of the head, composed lethal-calm meditative face. Slim narrow-profile man with sideways body angle. He wears a dark navy near-black kimono and hakama style outfit, both layered together with overlapping panels. Palette: dark navy near-black robes dominant, pale gold and brass accent details on the belt and trim, black hair.

— Adult male, weathered tan skin, wild dark matted shoulder-length hair, furious restrained masculine face. Heavily muscular bulky man with massive shoulders. He wears rough crude armor with a leather harness and iron studs across the chest, dyed in a dark blood-red tone, brutal and unpolished aesthetic. Palette: dark red-brown leather dominant, vivid blood red accents on straps and trim, weathered tan skin.

— Adult female, gray-pale sickly skin, dark hair hidden under a drawn-up hood, brooding ancient mid-incantation face mostly shadowed under the hood. Slim upright slightly forward-leaning woman. She wears a very dark purple-black high-collared robe with the hood drawn up over her head, the high collar visible at the neck. Palette: dark purple-black robe dominant, dark red hex-rune accent tones, gray-pale skin, black hood.

— Adult male, dark skin tone, very short hair, hungry restless gleeful masculine face. Heavily muscular shirtless man with bare muscular chest visible. Dark leather wrappings cover both hands and forearms in symmetric pattern, paired with a leather belt at the waist. Palette: dark leather brown wrappings dominant, vibrant orange charge accent glow at the knuckles and forearms, deep skin tone.

— Adult male, olive skin, short dark hair, coiled predatory silent face. Very slim upright narrow-vertical-profile man with elbows tucked close to the torso. He wears near-black deep purple form-fitting armor across the torso and arms, no hood concealing the face. Palette: near-black dark purple armor dominant, bright void purple accent details on the trim and joints, olive skin.

— Adult female, tan skin, off-white bleached-ivory hair tied in a low loose ponytail with escaping temple strands, a faint scar on one cheek, alert predator-still face. Slim-torso broad-shouldered tactical hunter woman. She wears battle-worn dark forest green asymmetric armor with a heavier right pauldron on the right shoulder and a left forearm leather wrap on the opposite arm. Cold blue accent strips on the hood (down on the back) and the straps. Palette: dark forest green armor dominant, cold blue accent strips, warm tan leather details, bleached-ivory hair.

— Adult female, pale skin, long straight dark hair flowing loose, reverent melancholic face. Tall upright conductor-build woman. She wears a dark indigo green-black high-collared robe that falls to her ankles, with cyan trim along the collar and cuffs. Palette: dark indigo green-black robe dominant, neon minion green and cyan trim accents on collar and cuffs, long straight dark hair, pale skin.

— Adult female, brown skin, deep auburn red hair, cocky ready hair-trigger face. Agile kinetic-build woman in a slight micro-crouch posture. She wears a long dark grey-purple trench coat with the flaps falling evenly, a rift-marked bandana tied around her neck, and bone and feather accessories visible on the collar and belt. Distinctly not generic western cowboy aesthetic, no goggles. Palette: dark grey-purple trench coat dominant, fire orange accent highlights at the bandana and trim, deep auburn red hair, leather tan accessories.
```

---

## NEGATIVE PROMPT (PixelLab negative prompt kutusu)

```
flat front view, eye-level shot, straight-on view, side view, side profile, three-quarter shoulder turn, isometric projection, low angle, looking up at character, no overhead tilt, tall adult proportions, mature adult proportions, 5 head tall body, 6 head tall body, 7 head tall body, 8 head tall body, realistic body proportions, slim adult body, tall lean body, Diablo 2 sprite proportions, Hero Siege proportions, Last Epoch proportions, small head, tiny head, normal head size, realistic head size, adult head, long legs, slim legs, athletic proportions, foreshortened tall body, blurry tiny face, unreadable face at 64 pixels, text, words, letters, alphabet, glyphs, typography, captions, labels, character names, sprite names, title cards, watermark, signatures, numbers, digits, weapons of any kind, sword, swords, longsword, greatsword, katana, dagger, daggers, axe, axes, hatchet, hammer, mace, spear, polearm, halberd, bow, longbow, crossbow, quiver, arrows, pistol, pistols, gun, rifle, staff, wand, orb, scepter, shield, buckler, sheath, scabbard, holster, holsters, lantern, grimoire, book, weapon belt, weapon harness with weapons, weapon strap, hilt, blade, blade tip, hilt sticking out, weapon on back, weapon on hip, weapon on belt, weapon on shoulder, X-shaped crossed weapons on back, drawn weapon, weapon visible in hand, ghostly weapon outline, weapon glow, 3d render, soft shading, blur, blurred face, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, identical characters repeated, child, teenager, young face, goggles, generic western cowboy, shiny knight armor
```

---

## ÜRETİM (style ref YOK — direkt Create Image Pro)

1. PixelLab Web UI → **Create Image Pro** (mode)
2. **Style ref panel: BOŞ bırak** (drag-drop yapma)
3. **Describe reference kutusu: BOŞ bırak** (style ref yok)
4. **Main prompt kutusuna MAIN PROMPT bloğunu yapıştır** (3 ABSOLUTE RULE başlangıçta + chibi NLM canonical 10 karakter)
5. **Negative prompt kutusuna NEGATIVE PROMPT bloğunu yapıştır**
6. **Setting:** Camera **High top-down** (UI dropdown — zorunlu), Output **64×64**
7. **Generate** → 10+ variation çıkar (style ref olmadığı için tutarlılık daha çok prompt'ta yazılan açı/chibi cümlelerine bağlı)
8. Karakter tanıma listesinden sırayla 10 sınıfı belirle + en uygun olanı seç + kaydet
9. Drift olan sınıflar için sadece o sınıfı tek prompt'la regenerate

**Style ref olmadığı için risk:** Açı ve chibi proportions PixelLab'in interpretasyonuna kalır. ABSOLUTE CAMERA RULE + ABSOLUTE PROPORTION RULE prompt başına yazıldı (5-katmanlı sinyal). Eğer output yine flat front view veya mature adult çıkarsa style ref yolu denenebilir.

---

## Karakter tanıma (output → sınıf)

| # | Belirleyici özellik | Sınıf |
|---|---|---|
| 1 | Light tan + dark brown leather + brass buckle + beard | Warblade |
| 2 | Pale + dusty indigo crop top + cream sash + teal skirt + blonde bun | Elementalist |
| 3 | Asian + dark navy kimono/hakama + low knot | Ronin |
| 4 | Weathered tan + dark blood-red leather harness + matted hair | Ravager |
| 5 | Gray-pale + dark purple-black high-collar robe + hood | Hexer |
| 6 | Dark skin + shirtless + leather hand/forearm wrappings + orange glow | Brawler |
| 7 | Olive + near-black deep purple armor + no hood | Shadowblade |
| 8 | Tan + bleached-ivory ponytail + dark forest green asymmetric armor + cheek scar | Ranger |
| 9 | Pale + long dark hair + dark indigo high-collar robe + cyan trim | Summoner |
| 10 | Brown + dark grey-purple trench + rift bandana + auburn hair | Gunslinger |

---

## QC

- 10 farklı NLM-canonical karakter
- Cinsiyet: 5M (1,3,4,6,7), 5F (2,5,8,9,10)
- Skin: light tan / pale / Asian / weathered tan / gray-pale / dark / olive / tan / pale / brown
- Saç: dark+beard / honey-blonde bun / black low knot / dark matted / dark under hood / very short / dark short / bleached-ivory ponytail / long straight dark / deep auburn red
- Palette dominant: dark brown leather / dusty indigo / dark navy / dark blood-red / dark purple-black / dark leather brown / near-black purple / dark forest green / dark indigo green-black / dark grey-purple
- Hepsi chibi 3-4 head tall (Image #9 ailesi)
- Hepsi silahsız (eller / sırt / bel / omuz / thigh boş)
- Açı high top-down 30-35° (Hades / HLD tier)

FAIL durumları:
- Açı yan/düz çıkıyor → ABSOLUTE CAMERA RULE prompt'un en başında olmalı, UI dropdown High top-down doğru mu kontrol, "30 to 35 degrees" + "head slightly compressed" + "shoulders angled diagonally" cümleleri 2× tekrarla
- Mature 5-6 head adult çıkıyor (chibi değil) → ABSOLUTE PROPORTION RULE prompt'un en başında olmalı, "3 to 4 heads tall, head 40 percent" 2× tekrarla
- Silah var → NEGATIVE PROMPT yapışmamış olabilir → kutuya tekrar yapıştır + regen
- Identity drift (sınıf canonical'a uymadı) → ilgili sınıfı tek-prompt regen, identity description'ı güçlendir
- Style ref yokluğu sorunu büyük olursa → Image #10 style ref kullanma seçeneğine dön (eski Settings notunda alternatif)

---

## KAYIT

```
Assets/Sprites/Characters/{ClassName}/base/{classname}_base_v3.png
```

End of LIVE production sheet (v10 chibi NLM canonical, silahsız body).
