# Karakter Üretim — LIVE v11 (Image #14 drift fix + Gunslinger user override)

> **LIVE version: v11** — v10 + 8 spesifik drift fix (Image #14 analiz sonrası) + Gunslinger hair user override (kızıl → dark)
> **Karar #74/#100/#144/#145 LIVE LOCK** korunur
> **v10 archive:** `_archive/character_production_prompts_v10.md`

---

## v11 Drift Fix Özeti (v10'dan değişiklikler)

| # | Sınıf | v10 problemi (Image #14'te görülen drift) | v11 fix |
|---|---|---|---|
| 1 | Warblade | (1,1) teal/black coat dominant, brown leather zayıf | "DOMINANT dark brown leather, NOT teal NOT navy NOT black; brass buckle clearly at center chest" eklendi |
| 2 | Elementalist | (3,4) red hair drift Image #12'den devam etti | "honey-blonde MANDATORY, NOT red, NOT auburn, NOT dark; low bun visibly behind head" reinforced |
| 3 | Ronin | (1,3) topknot/samurai cue zayıf | "BLACK HAIR TIED IN SAMURAI TOPKNOT visibly behind head, knot shape clear from above" reinforced |
| 4 | Ravager | (1,4) CANONICAL ✅ | değişiklik yok |
| 5 | Hexer | (2,1) generic hooded caster riski, curse-rune accent zayıf | "DARK RED HEX-RUNE ACCENT clearly on collar and cuffs" reinforced |
| 6 | Brawler | (2,2) hand wraps + guard pose belirsiz | "DARK LEATHER WRAPPINGS clearly covering hands and forearms, boxing guard hand position" reinforced |
| 7 | Shadowblade | (2,3) generic dark coat, purple accent zayıf | "DOMINANT BRIGHT VOID PURPLE ACCENT GLOW on shoulders/joints, narrow vertical profile, elbows tucked" reinforced |
| 8 | Ranger | (2,4) CANONICAL ✅ | değişiklik yok |
| 9 | Summoner | (3,1) staff gesture yok, generic robe figure | "one hand raised in SUMMONING GESTURE, palm facing down conducting, faint cyan glow at fingertips" eklendi |
| 10 | Gunslinger | User override: kızıl → koyu saç (Image #14 (4,4) anchor) | "deep auburn red hair" → "**dark short hair**", "NOT red hair, NOT auburn" eklendi |

---

## Settings (PixelLab UI) — v10 ile aynı

- Mode: **PixelLab → Create Image Pro**
- Style ref panel: **BOŞ** (kullanıcı tercihi)
- Camera (UI dropdown): **High top-down**
- Output: **64×64** veya 4×4 contact sheet
- Describe reference kutusu: **BOŞ**

---

## REFERENCE DESCRIPTION

**Style ref kullanılmıyor — bu kutu BOŞ bırakılır.** Tüm sinyal MAIN PROMPT içinden.

---

## MAIN PROMPT v11 (tek blok — PixelLab main prompt kutusuna yapıştır)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, EXACTLY 30 to 35 degrees downward tilt from horizontal, ARPG-style overhead bird's-eye perspective like Hades and Hyper Light Drifter and Hammerwatch. All sprites face south (body oriented toward the bottom of the screen, camera high above looking downward). Each character is viewed clearly from above at a diagonal — the top plane of the head and hair crown is visible as a rounded shape, shoulders are visibly angled diagonally toward the camera as seen from above, head is slightly compressed vertically due to the downward camera tilt, torso height shortened by foreshortening. Feet small and low in the sprite. Soft oval ground shadow beneath each character's feet. NOT a flat front view, NOT a side profile, NOT a three-quarter southeast view, NOT a character-select portrait, NOT an isometric projection, NOT a pure 90-degree bird's-eye top-down.

ABSOLUTE PROPORTION RULE — CHIBI PIXEL ART, BIG HEAD MANDATORY: extreme chibi proportions like Hammerwatch / Hyper Light Drifter / Tunic / Eastward. Total body height = exactly 3 to 4 head heights, NEVER 5 or 6. Oversized cartoon head dominates the upper half of the sprite — the head must read as approximately 40 to 45 percent of total sprite height, large enough that the face features (eyes, mouth, hair shape) are clearly readable at 64x64 pixel resolution. Broad shoulders directly below the big head, no neck or only 1-pixel neck. Short stubby legs about 20 to 25 percent of total height. Hands and feet small and chunky. The face is the largest single readable element of the sprite — eyes, eyebrows, nose, mouth all visible as distinct pixel clusters. Sprite fills 85 to 90 percent of canvas vertically, full body visible from top of head to feet. The body type is compact and cute-tough, NOT tall, NOT slim, NOT realistic, NOT mature adult, NOT super-deformed 2-head.

ABSOLUTE TEXT RULE: the generated image must contain NO text, NO words, NO letters, NO labels, NO character names, NO captions, NO numbers, NO typography of any kind.

Every character is completely weapon-free.

Pixel art chibi character batch — 10 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color.

WEAPON-FREE RULE: each character carries no weapons of any kind. The body is completely free of swords, axes, bows, daggers, pistols, staves, orbs, lanterns, grimoires, shields, sheaths, scabbards, holsters, quivers, weapon belts, weapon harnesses, weapon straps. Both hands hold nothing, palms open and clearly empty at the sides. The back, belt, shoulders, hips, and thighs are completely empty of weapons or weapon attachments.

POSE: all characters stand in a calm balanced idle pose, both feet on the ground at shoulder width, arms relaxed and open at the sides, head looking forward, face calm and neutral, body symmetric.

The ten characters in order, all adult, all weapon-free:

— Adult male, light tan skin, dark short hair, dark masculine beard clearly visible on the jaw, weary grounded patient face. Broad-shouldered heavy-build man. He wears DOMINANT dark brown leather armor across his torso, NOT teal NOT navy NOT black, with clearly visible brass buckle accents at the center chest and at the waist, a weary veteran look not a shiny knight. Palette: dark brown leather body tones dominant and unambiguous, brass accents on buckles and trim, faint cold blue highlight details on edges only.

— Adult female, pale skin, warm honey-blonde hair MANDATORY (NOT red, NOT auburn, NOT dark, NOT black) in a neat low bun visibly behind the head with loose temple strands, curious ember-bright face. Slim refined woman with slim upper body. She wears a cropped sleeveless dusty indigo top showing a small bare midriff strip, a flowing high-waisted deep teal skirt, dark fitted tights, high boots, and a cream sash tied at the waist. Palette: dusty indigo top dominant, deep teal skirt accent, cream sash and trim, warm gold accent highlights, honey-blonde hair only.

— Adult male, Asian features and skin tone, BLACK HAIR TIED IN A SAMURAI TOPKNOT visibly behind the head with the knot shape clear from the above angle, composed lethal-calm meditative face. Slim narrow-profile man with sideways body angle. He wears a dark navy near-black kimono and hakama style outfit, both layered together with overlapping panels, distinctively samurai-coded silhouette. Palette: dark navy near-black robes dominant, pale gold and brass accent details on the belt and trim, black hair.

— Adult male, weathered tan skin, wild dark matted shoulder-length hair, furious restrained masculine face. Heavily muscular bulky man with massive shoulders. He wears rough crude armor with a leather harness and iron studs across the chest, dyed in a dark blood-red tone, brutal and unpolished aesthetic. Palette: dark red-brown leather dominant, vivid blood red accents on straps and trim, weathered tan skin.

— Adult female, gray-pale sickly skin, dark hair hidden under a drawn-up hood, brooding ancient mid-incantation face mostly shadowed under the hood. Slim upright slightly forward-leaning woman. She wears a very dark purple-black high-collared robe with the hood drawn up over her head, the high collar visible at the neck. DARK RED HEX-RUNE ACCENT TONES clearly visible on the collar trim and cuffs, distinguishing from generic hooded caster. Palette: dark purple-black robe dominant, DARK RED HEX-RUNE accent unambiguous on collar and cuffs, gray-pale skin, black hood.

— Adult male, dark skin tone, very short hair, hungry restless gleeful masculine face. Heavily muscular shirtless man with bare muscular chest visible. DARK LEATHER WRAPPINGS clearly visible covering both hands and forearms in symmetric pattern, hands held in a slight boxing guard position, paired with a leather belt at the waist. Palette: dark leather brown wrappings dominant, vibrant orange charge accent glow at the knuckles and forearms, deep skin tone.

— Adult male, olive skin, short dark hair, coiled predatory silent face. Very slim upright narrow-vertical-profile man with elbows tucked close to the torso, distinctly narrow body width. He wears near-black deep purple form-fitting armor across the torso and arms, no hood concealing the face. DOMINANT BRIGHT VOID PURPLE ACCENT GLOW on the shoulder pads, elbow joints, and chest seams, distinguishing from generic dark coat. Palette: near-black dark purple armor dominant, BRIGHT VOID PURPLE accent details on the trim and joints unambiguous, olive skin.

— Adult female, tan skin, off-white bleached-ivory hair tied in a low loose ponytail with escaping temple strands, a faint scar on one cheek, alert predator-still face. Slim-torso broad-shouldered tactical hunter woman. She wears battle-worn dark forest green asymmetric armor with a heavier right pauldron on the right shoulder and a left forearm leather wrap on the opposite arm. Cold blue accent strips on the hood (down on the back) and the straps. Palette: dark forest green armor dominant, cold blue accent strips, warm tan leather details, bleached-ivory hair.

— Adult female, pale skin, long straight dark hair flowing loose, reverent melancholic face. Tall upright conductor-build woman. She wears a dark indigo green-black high-collared robe that falls to her ankles, with cyan trim along the collar and cuffs. ONE HAND RAISED IN A SUMMONING GESTURE with palm facing down as if conducting, faint cyan ambient glow at the fingertips signaling minion command intent (no weapon, no staff, no orb in hand). Palette: dark indigo green-black robe dominant, neon minion green and cyan trim accents on collar and cuffs, long straight dark hair, pale skin.

— Adult female, brown skin, DARK SHORT HAIR (NOT red, NOT auburn, NOT ginger), cocky ready hair-trigger face. Agile kinetic-build woman in a slight micro-crouch posture. She wears a long dark grey-purple trench coat with the flaps falling evenly, a rift-marked bandana tied around her neck, and bone and feather accessories visible on the collar and belt. Distinctly not generic western cowboy aesthetic, no goggles. Palette: dark grey-purple trench coat dominant, fire orange accent highlights at the bandana and trim, dark hair, leather tan accessories.
```

---

## NEGATIVE PROMPT v11 (v10 + 7 yeni drift reject)

```
flat front view, eye-level shot, straight-on view, side view, side profile, three-quarter shoulder turn, isometric projection, low angle, looking up at character, no overhead tilt, tall adult proportions, mature adult proportions, 5 head tall body, 6 head tall body, 7 head tall body, 8 head tall body, realistic body proportions, slim adult body, tall lean body, Diablo 2 sprite proportions, Hero Siege proportions, Last Epoch proportions, small head, tiny head, normal head size, realistic head size, adult head, long legs, slim legs, athletic proportions, foreshortened tall body, blurry tiny face, unreadable face at 64 pixels, text, words, letters, alphabet, glyphs, typography, captions, labels, character names, sprite names, title cards, watermark, signatures, numbers, digits, weapons of any kind, sword, swords, longsword, greatsword, katana, dagger, daggers, axe, axes, hatchet, hammer, mace, spear, polearm, halberd, bow, longbow, crossbow, quiver, arrows, pistol, pistols, gun, rifle, staff, wand, orb, scepter, shield, buckler, sheath, scabbard, holster, holsters, lantern, grimoire, book, weapon belt, weapon harness with weapons, weapon strap, hilt, blade, blade tip, hilt sticking out, weapon on back, weapon on hip, weapon on belt, weapon on shoulder, X-shaped crossed weapons on back, drawn weapon, weapon visible in hand, ghostly weapon outline, weapon glow, 3d render, soft shading, blur, blurred face, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, identical characters repeated, child, teenager, young face, goggles, generic western cowboy, shiny knight armor, teal coat on Warblade, navy coat on Warblade, black coat on Warblade, red hair on Elementalist, auburn hair on Elementalist, dark hair on Elementalist, generic hooded caster without rune accent, generic dark coat on Shadowblade without purple glow, generic robe figure on Summoner without summoning gesture, samurai without visible topknot, red hair on Gunslinger, auburn hair on Gunslinger, ginger hair on Gunslinger, deep auburn red hair on female brown skin character
```

---

## v11 Üretim Hedefi

Aynı 10-karakter ürün — bu sefer drift fix'li. Beklenen sonuçlar:
1. (1) Warblade — brown leather + brass buckle clear ✅
2. (2) Elementalist — honey-blonde low bun, **red hair drift kapanır**
3. (3) Ronin — samurai topknot clear visible
4. (4) Ravager — canonical (zaten geçmişti)
5. (5) Hexer — dark red hex-rune accent clear
6. (6) Brawler — bandage wraps + boxing guard visible
7. (7) Shadowblade — void purple glow on shoulders + narrow profile
8. (8) Ranger — canonical (zaten geçmişti)
9. (9) Summoner — summoning gesture clear (no staff, weapon-free rule korunur)
10. (10) Gunslinger — **dark short hair**, brown skin, grey-purple trench (user fix)

## Sıradaki adım

User Create Image Pro V3'e yapıştır + 16 contact sheet (10 canonical + 6 variant slot) üret. Sonuç Image #15 olarak gelir. Image #15 analiz sonrası:
- 4+ canonical accept hedef (artı önceki Ravager/Brawler/Ranger üzerine Warblade + Ronin + Hexer + Shadowblade + Summoner + Elementalist + Gunslinger fix'leri doğrulanır)
- Drift kalanlar Sprint 14 boyunca single-prompt regen ile temizlenir
