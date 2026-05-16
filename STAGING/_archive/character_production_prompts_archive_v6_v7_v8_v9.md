# Karakter Üretim — v6 (V8 mantığı + chibi force via Style Reference)

> Mode: PixelLab Web UI → Create from Style Reference (eski V8 sprite chibi'yi style ref olarak kullan)
> Reference sprite: eski V8 batch'ten 1 chibi karakter (önerilen: Warblade sol üst sprite)
> Output: 64×64, style strength 0.50–0.65 (yüksek — chibi style transferini güçlü ister)
> Workflow: 1 chibi style ref + 1 reference description + 1 main prompt + 1 negative → Generate

---

## STYLE REFERENCE IMAGE (PixelLab "create from style reference" panel)

Drag drop edilecek sprite: eski V8 batch output'undan 1 chibi karakter.
Önerilen: Warblade (sol üst, plate armor + crimson palette, south facing, chibi 3-4 head tall).
Alternatif: Elementalist (4. sprite, feminine chibi), Necromancer (9. sprite, neutral palms).

## REFERENCE IMAGE DESCRIPTION (PixelLab "describe reference" kutusu)

```
Use this reference ONLY for chibi proportions (3 to 4 heads tall, oversized head about 40 percent of total height, short stubby legs about 25 percent), the high top-down camera angle (30 to 35 degrees downward tilt, character facing south toward camera), and the pixel art style (1 pixel solid outline, hard pixel edges, no anti-aliasing, flat cell shading max 2 tones per color).

Do not transfer: identity, clothing, armor, hair, color palette, weapons, equipment, sheaths, holsters, held objects, or any visible detail of the reference character. The new characters are completely different people described in the main prompt.

Ten visually distinct adult characters, all weapon-free, no two share identity.
```

---

## MAIN PROMPT (PixelLab main prompt kutusu — tek blok, tüm 10 karakter)

```
ABSOLUTE RULE: the generated image must contain NO text, NO words, NO letters, NO labels, NO character names, NO captions, NO titles, NO numbers. The image is pure pixel art with zero typography of any kind. Every character is completely weapon-free.

Pixel art chibi character batch — 10 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color.

Chibi proportions: oversized head about 40 percent of total height, broad shoulders, short stubby legs about 25 percent of total height. Sprite fills 85-90 percent of canvas vertically.

Camera: high top-down 30-35 degrees downward tilt, ARPG-style overhead perspective like Hades. All sprites face south, direct front view, both eyes and full face visible, shoulders horizontally even.

WEAPON-FREE RULE: each character carries no weapons of any kind. The body is completely free of swords, axes, bows, daggers, pistols, staves, orbs, shields, sheaths, scabbards, holsters, quivers, weapon belts, weapon harnesses, weapon straps. Both hands hold nothing, palms open and clearly empty at the sides. The back, belt, shoulders, hips, and thighs are completely empty of weapons or weapon attachments. No ghostly or transparent weapon outline anywhere on the body.

POSE: all characters stand in a calm balanced idle pose, both feet on the ground at shoulder width, arms relaxed and open at the sides, head looking forward, face calm and neutral, body symmetric left side matching right side.

The ten characters in order, all adult, all weapon-free:

— Adult male. Broad-shouldered muscular man in a heavy metal chestplate with a circular engraved emblem at the center, plain leather under-shirt visible at neck and arms, plain cloth tabard over the legs. Short cropped dark messy hair. Masculine weathered face with a faint scar on one cheek, stern expression. Palette: deep crimson red armor, dark gunmetal steel plates, bronze trim accents, leather-brown straps.

— Adult female. Slim refined woman in layered flowing robes with a high collar. A runic circlet rests across her forehead with small carved symbols. Warm honey-blonde hair pulled back in a low bun with a few face-framing strands. Feminine face, calm controlled expression. Palette: ice-blue robes, silver trim, warm honey hair, cream undertones.

— Adult male. Lean disciplined man in a weathered haori coat, open at the front with both flaps falling at equal length. Dark hair tied in a topknot. A scarred eye-band runs across both eyes, symmetric. Masculine face with defined jaw, calm focused expression. The hip is plain, no scabbard or sheath of any kind. Palette: indigo blue haori, slate grey under-robe, black hair, warm cream sash, bronze accents.

— Adult male. Heavily-built bulky man with broad thick arms, strong masculine build. Wears a leather and iron-studded chest harness with studs distributed evenly on both sides. Wild matted shoulder-length dark hair, symmetric volume. Savage weathered face, jaw heavy and masculine. The back and belt are completely empty, no weapon straps. Palette: rust-brown leather, bone-white accents, dark iron studs, weathered tan skin.

— Adult female. Slim sickly woman in a dark hooded mantle drawn up symmetrically, draping evenly on both sides. Hollow feminine features, gaunt cheeks, shadowed eyes, cold expression. A single faint crimson curse-mark glows on the center of her forehead. Long dark hair partially visible under the hood. Palette: void-purple mantle, sickly muted green inner cloth, black hood, pale skin, faint dark crimson curse-mark.

— Adult male. Stocky compact man with thick muscular frame, masculine build. Sleeveless tunic with bare muscular arms exposed. Both hands wrapped in cloth bandages, wrapping symmetric on each hand. Buzz-cut short or shaved head. Masculine face with strong jaw, determined expression. Palette: ochre yellow tunic, earth brown belt, cream bandages, warm skin.

— Adult male. Slim agile lean man with masculine build. Wears a symmetric hooded cloak with the hood up, falling evenly on both sides. Arms and torso wrapped in form-fitting dark cloth, symmetric. A neutral half-mask covers his lower face. Masculine eyes visible above the mask. The back, belt, and thighs are plain cloth, no weapon straps or sheaths. Palette: deep void-purple cloak, matte black wrappings and mask, cold steel-grey accents, faint cyan glow only in the eyes.

— Adult female. Athletic medium-build feminine woman. Wears a weathered leather hood, symmetric on both sides. A light cloak with symmetric side pouches on each hip. Brown hair tied in a practical braid or ponytail. Utility belt with two empty side pouches mirrored. Alert feminine expression. The pouches are flat and empty, no arrows or weapons inside, no quiver on the back. Palette: forest green cloak, warm tan leather, bronze buckles, dark olive cloth.

— Adult female. Medium-build feminine woman in a layered ceremonial robe with symmetric folds and an even hem on both sides. A bone-circlet hair piece centered on her forehead. Dark brown hair pulled back in a formal style. Feminine face, solemn expression, gaze gently downward but forward. Palette: deep teal outer robe, bone-white ornaments, dusty cream inner robes, bronze sigil accents, dark brown hair.

— Adult female. Tall lean woman with sharp confident bearing, feminine face. Long open coat with symmetric flaps falling evenly. Wide-brim hat tilted slightly forward, symmetric. Deep auburn red hair under the hat. Utility belt with two empty symmetric side pouches. Focused expression with hint of smirk. The holsters are flat and empty, no pistols or guns inside. Palette: dusty bronze coat trim, matte black coat body, deep auburn hair, leather tan hat and belt.
```

---

## NEGATIVE PROMPT (PixelLab negative prompt kutusu)

```
text, words, letters, alphabet, glyphs, typography, captions, labels, character names, sprite names, sprite labels, title cards, watermark, signatures, numbers, digits, grid headers, name tags, written language of any kind, weapons of any kind, sword, swords, longsword, greatsword, katana, dagger, daggers, axe, axes, hatchet, hammer, mace, spear, polearm, halberd, bow, longbow, crossbow, quiver, arrows, pistol, pistols, gun, rifle, staff, wand, orb, scepter, shield, buckler, sheath, scabbard, holster, holsters, weapon belt, weapon harness, weapon strap, hilt, blade, blade tip, hilt sticking out, weapon on back, weapon on hip, weapon on belt, weapon on shoulder, X-shaped crossed weapons on back, twin blades on back, drawn weapon, weapon visible in hand, ghostly weapon outline, weapon glow, 3d render, soft shading, blur, smooth gradient, isometric projection, anti-aliasing, soft edges, realistic proportions, painterly style, anime style, three-quarter shoulder turn, identical characters repeated, child, teenager, young face
```

---

## ÜRETİM (v6 — Style Reference yolu)

1. PixelLab Web UI → Create from Style Reference (mode)
2. Style reference panel'e: eski V8 batch'ten 1 chibi sprite drop (önerilen Warblade sol üst, image #3 dosyasından)
3. Describe reference kutusuna REFERENCE bloğunu yapıştır (style + proportions + camera transfer, identity transfer YOK)
4. Negative prompt kutusuna NEGATIVE PROMPT bloğunu yapıştır
5. Main prompt kutusuna MAIN PROMPT bloğunu yapıştır (tek blok, 10 karakter dahil)
6. Setting: style strength 0.50–0.65 (chibi transferi için yüksek) / Camera High top-down / Output 64×64
7. Generate → 10+ variation çıkar
8. Sırayla 10 karakteri (Warblade → Elementalist → Ronin → Ravager → Hexer → Brawler → Shadowblade → Ranger → Summoner → Gunslinger) tanı + en uygun olanı seç + kaydet
9. Drift olan sınıflar için sadece o sınıfı tek prompt'la regenerate (aynı style ref + identity prompt + negative)

Eğer style transferi aşırı olursa (sprite'lar reference Warblade'e çok benzer renkte/silüette):
- Style strength'i 0.40-0.50'ye düşür
- Reference description'da "Do not transfer" listesini güçlendir
- Birden fazla chibi sprite'ı ref olarak ver (eğer PixelLab multi-ref destekliyorsa)

Karakter tanıma (output'tan hangisi hangi sınıf):
1. Crimson chestplate + chest emblem → Warblade
2. Ice-blue robes + honey-blonde bun + circlet → Elementalist
3. Indigo haori + topknot + eye-band → Ronin
4. Rust-brown studded harness + matted hair → Ravager
5. Void-purple hood + curse-mark + gaunt → Hexer
6. Ochre sleeveless + hand bandages + shaved head → Brawler
7. Void-purple cloak + half-mask + cyan eyes → Shadowblade
8. Forest green cloak + leather hood + braid → Ranger
9. Deep teal robe + bone circlet → Summoner
10. Dusty bronze coat + wide-brim hat + auburn hair → Gunslinger

---

## QC

- 10 farklı karakter, identity çakışması yok
- Hiçbir karakterde silah (eller / sırt / bel / omuz / thigh / hat — hepsi boş)
- Chibi 3-4 head tall, head ~40% height
- Tüm sprite face south (kameraya bakan), high top-down 30-35°
- Cinsiyet doğru (5 male / 5 female, sırayla 1-male, 2-female, 3-male, 4-male, 5-female, 6-male, 7-male, 8-female, 9-female, 10-female)
- Palette her sınıf için description'a uygun
- 64x64 canvas, transparent background, 1px outline
- Pixel art (anime/realistic değil), no anti-aliasing

FAIL durumları:
- Silah varsa: negative prompt yapışmamış olabilir → kutuya tekrar yapıştır + regenerate
- Açı yan/ortogonal çıkıyorsa: "face south + high top-down 30-35° + face camera" cümlelerini tekrar kontrol et
- Realistic proportions: chibi cümlesi token olarak güçlü değil → BAŞA çek veya 2× tekrarla
- Çocuk/teen yüz: NEGATIVE "child, teenager, young face" var ama yetmediyse "adult" vurgu artır

---

## KAYIT

Her sınıfı tek tek kaydet:
```
Assets/Sprites/Characters/{ClassName}/base/{classname}_base_v3.png
```

End of v6 production sheet.

---

## SINGLE WARBLADE TEST (v7 — tek karakter, açı agresif)

> Mode: PixelLab → Create Image S-XL New (tek karakter chibi 64×64)
> UI setting: Camera = High top-down (dropdown), Reference image = YOK
> Amaç: 1 Warblade üret → açı doğru çıkarsa onu v6 style reference olarak kullan

### MAIN PROMPT (tek Warblade)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, 30 to 35 degrees downward tilt, ARPG-style overhead bird's-eye perspective like Hades. The character is viewed clearly from above at a diagonal, not a flat front view. The head appears slightly compressed vertically due to the downward camera tilt. Shoulders are visibly angled diagonally as if seen from above. Both feet visible on the ground in front of the character's silhouette. No straight-on flat front view, no side profile, no isometric projection.

Pixel art chibi character sprite, 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Chibi proportions: oversized head about 40 percent of total height, broad shoulders, short stubby legs about 25 percent of total height. Sprite fills 85 to 90 percent of canvas vertically.

Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood.

WEAPON-FREE RULE: the character carries no weapons of any kind. The body is completely free of swords, axes, bows, daggers, pistols, staves, orbs, shields, sheaths, scabbards, holsters, quivers, weapon belts, weapon harnesses, weapon straps. Both hands hold nothing, palms open and clearly empty at the sides. The back, belt, shoulders, hips, and thighs are completely empty of weapons or weapon attachments.

POSE: standing in a calm balanced idle pose, both feet on the ground at shoulder width, arms relaxed and open at the sides, head looking forward toward the camera, face calm and neutral, body symmetric left side matching right side.

CHARACTER: Adult male. Broad-shouldered muscular adult man in a heavy metal chestplate with a circular engraved emblem at the center, plain leather under-shirt visible at neck and arms, plain cloth tabard over the legs. Short cropped dark messy hair. Masculine weathered face with a faint scar on one cheek, stern expression.

PALETTE: deep crimson red armor, dark gunmetal steel plates, bronze trim accents, leather-brown straps.
```

### NEGATIVE PROMPT

```
flat front view, eye-level shot, straight-on view, side view, side profile, three-quarter shoulder turn, isometric projection, low angle, looking up at character, no overhead tilt, weapons of any kind, sword, swords, longsword, greatsword, katana, dagger, daggers, axe, axes, hatchet, hammer, mace, spear, polearm, halberd, bow, longbow, crossbow, quiver, arrows, pistol, pistols, gun, rifle, staff, wand, orb, scepter, shield, buckler, sheath, scabbard, holster, holsters, weapon belt, weapon harness, weapon strap, hilt, blade, hilt sticking out, weapon on back, weapon on hip, weapon on belt, weapon on shoulder, X-shaped crossed weapons on back, twin blades on back, drawn weapon, weapon visible in hand, ghostly weapon outline, weapon glow, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, realistic proportions, painterly style, anime style, child, teenager, young face, text, words, letters, watermark, signature
```

### Settings (PixelLab UI)

- Mode: Create Image S-XL New
- Camera: High top-down (dropdown)
- Output: 64×64 square
- Reference image: YOK (boş bırak, bias istemiyoruz)

### QC kriterleri

- Chibi 3-4 head tall (head ~40% height)
- Face camera (south facing, yüz görünür)
- High top-down tilt görünür → shoulder hattı diagonal, kafa hafif compressed
- Silah hiç yok (eller / sırt / bel / omuz / thigh hepsi boş)
- Plate chestplate + circular emblem net
- Crimson red + gunmetal steel + bronze palette dominant
- Pixel art, 1px outline, transparent background

PASS → bu sprite v6 workflow'da style reference panel'e drop, 10 sınıf batch üretilir
FAIL → hangi kriter bozuk belirle (açı/chibi/silah/palette) → ilgili cümleyi prompt'ta güçlendir + regenerate

End of single Warblade test (v7).

---

## 10-CHARACTER BATCH WITH WARBLADE STYLE REF (v8 — kullanım için)

> Mode: PixelLab → Create from Reference
> Style ref panel: senin chibi Warblade sprite'ın (image #7 — başarılı tek Warblade output)
> Style strength: 0.55–0.70 (yüksek — chibi + açı transferini güçlü ister)
> UI camera setting: High top-down (dropdown'dan tekrar seç, prompt + UI iki katmanlı sinyal)
> Output: 64×64

### REFERENCE DESCRIPTION (describe reference kutusu)

```
Use this reference for chibi proportions (compact 3 to 4 heads tall, oversized head about 40 percent of total height, short stubby legs about 25 percent), the high top-down camera angle (30 to 35 degrees downward tilt, ARPG-style overhead bird's-eye perspective, shoulders visibly angled diagonally, head slightly compressed by camera tilt), the pixel art style (1 pixel solid outline, hard pixel edges, no anti-aliasing, flat cell shading max 2 tones per color), and the dark gritty pixel art mood.

Do not transfer: identity, face, beard, hair style, exact armor design, chestplate shape, color palette, clothing details, or any specific design element of the reference character. The new characters are completely different people described in the main prompt, with their own unique outfits and palettes. Each new character has a fresh identity.

Ten visually distinct adult characters total, all weapon-free, no two share identity.
```

### MAIN PROMPT (10 karakter tek blok)

```
ABSOLUTE RULE: the generated image must contain NO text, NO words, NO letters, NO labels, NO character names, NO captions, NO titles, NO numbers. Every character is completely weapon-free.

Pixel art chibi character batch — 10 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color.

Chibi proportions: compact 3 to 4 heads tall, oversized head about 40 percent of total height, broad shoulders, short stubby legs about 25 percent of total height. Sprite fills 85 to 90 percent of canvas vertically.

ABSOLUTE CAMERA RULE: high top-down camera angle, 30 to 35 degrees downward tilt, ARPG-style overhead bird's-eye perspective like Hades. All sprites face south, viewed from above at a clear diagonal, shoulders visibly angled diagonally toward the camera, head slightly compressed vertically due to downward tilt. Both eyes and full face visible. No flat front view, no side profile, no isometric projection.

WEAPON-FREE RULE: each character carries no weapons of any kind. The body is completely free of swords, axes, bows, daggers, pistols, staves, orbs, shields, sheaths, scabbards, holsters, quivers, weapon belts, weapon harnesses, weapon straps. Both hands hold nothing, palms open and clearly empty at the sides. The back, belt, shoulders, hips, and thighs are completely empty of weapons or weapon attachments. No ghostly or transparent weapon outline anywhere.

POSE: all characters stand in a calm balanced idle pose, both feet on the ground at shoulder width, arms relaxed and open at the sides, head looking forward, face calm and neutral, body symmetric left side matching right side.

The ten characters in order, all adult, all weapon-free:

— Adult male. Broad-shouldered muscular man in a heavy metal chestplate with a circular engraved emblem at the center, plain leather under-shirt visible at neck and arms, plain cloth tabard over the legs. Short cropped dark messy hair. Masculine weathered face with a faint scar on one cheek, stern expression. Palette: deep crimson red armor, dark gunmetal steel plates, bronze trim accents, leather-brown straps.

— Adult female. Slim refined woman in layered flowing robes with a high collar. A runic circlet rests across her forehead with small carved symbols. Warm honey-blonde hair pulled back in a low bun with a few face-framing strands. Feminine face, calm controlled expression. Palette: ice-blue robes, silver trim, warm honey hair, cream undertones.

— Adult male. Lean disciplined man in a weathered haori coat, open at the front with both flaps falling at equal length. Dark hair tied in a topknot. A scarred eye-band runs across both eyes, symmetric. Masculine face with defined jaw, calm focused expression. The hip is plain, no scabbard or sheath of any kind. Palette: indigo blue haori, slate grey under-robe, black hair, warm cream sash, bronze accents.

— Adult male. Heavily-built bulky man with broad thick arms, strong masculine build. Wears a leather and iron-studded chest harness with studs distributed evenly on both sides. Wild matted shoulder-length dark hair, symmetric volume. Savage weathered face, jaw heavy and masculine. The back and belt are completely empty, no weapon straps. Palette: rust-brown leather, bone-white accents, dark iron studs, weathered tan skin.

— Adult female. Slim sickly woman in a dark hooded mantle drawn up symmetrically, draping evenly on both sides. Hollow feminine features, gaunt cheeks, shadowed eyes, cold expression. A single faint crimson curse-mark glows on the center of her forehead. Long dark hair partially visible under the hood. Palette: void-purple mantle, sickly muted green inner cloth, black hood, pale skin, faint dark crimson curse-mark.

— Adult male. Stocky compact man with thick muscular frame, masculine build. Sleeveless tunic with bare muscular arms exposed. Both hands wrapped in cloth bandages, wrapping symmetric on each hand. Buzz-cut short or shaved head. Masculine face with strong jaw, determined expression. Palette: ochre yellow tunic, earth brown belt, cream bandages, warm skin.

— Adult male. Slim agile lean man with masculine build. Wears a symmetric hooded cloak with the hood up, falling evenly on both sides. Arms and torso wrapped in form-fitting dark cloth, symmetric. A neutral half-mask covers his lower face. Masculine eyes visible above the mask. The back, belt, and thighs are plain cloth, no weapon straps or sheaths. Palette: deep void-purple cloak, matte black wrappings and mask, cold steel-grey accents, faint cyan glow only in the eyes.

— Adult female. Athletic medium-build feminine woman. Wears a weathered leather hood, symmetric on both sides. A light cloak with symmetric side pouches on each hip. Brown hair tied in a practical braid or ponytail. Utility belt with two empty side pouches mirrored. Alert feminine expression. The pouches are flat and empty, no arrows or weapons inside, no quiver on the back. Palette: forest green cloak, warm tan leather, bronze buckles, dark olive cloth.

— Adult female. Medium-build feminine woman in a layered ceremonial robe with symmetric folds and an even hem on both sides. A bone-circlet hair piece centered on her forehead. Dark brown hair pulled back in a formal style. Feminine face, solemn expression, gaze gently downward but forward. Palette: deep teal outer robe, bone-white ornaments, dusty cream inner robes, bronze sigil accents, dark brown hair.

— Adult female. Tall lean woman with sharp confident bearing, feminine face. Long open coat with symmetric flaps falling evenly. Wide-brim hat tilted slightly forward, symmetric. Deep auburn red hair under the hat. Utility belt with two empty symmetric side pouches. Focused expression with hint of smirk. The holsters are flat and empty, no pistols or guns inside. Palette: dusty bronze coat trim, matte black coat body, deep auburn hair, leather tan hat and belt.
```

### NEGATIVE PROMPT

```
flat front view, eye-level shot, straight-on view, side view, side profile, three-quarter shoulder turn, isometric projection, low angle, looking up at character, no overhead tilt, text, words, letters, alphabet, glyphs, typography, captions, labels, character names, sprite names, title cards, watermark, signatures, numbers, digits, weapons of any kind, sword, swords, longsword, greatsword, katana, dagger, daggers, axe, axes, hatchet, hammer, mace, spear, polearm, halberd, bow, longbow, crossbow, quiver, arrows, pistol, pistols, gun, rifle, staff, wand, orb, scepter, shield, buckler, sheath, scabbard, holster, holsters, weapon belt, weapon harness, weapon strap, hilt, blade, blade tip, hilt sticking out, weapon on back, weapon on hip, weapon on belt, weapon on shoulder, X-shaped crossed weapons on back, twin blades on back, drawn weapon, weapon visible in hand, ghostly weapon outline, weapon glow, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, realistic proportions, painterly style, anime style, three-quarter shoulder turn, identical characters repeated, child, teenager, young face
```

### Üretim adımları

1. PixelLab Web UI → Create from Reference (mode)
2. Style ref panel'e: senin chibi Warblade sprite'ını drag-drop (image #7'deki başarılı output)
3. Describe reference kutusuna REFERENCE DESCRIPTION bloğunu yapıştır
4. Main prompt kutusuna MAIN PROMPT bloğunu yapıştır
5. Negative prompt kutusuna NEGATIVE PROMPT bloğunu yapıştır
6. Setting: style strength 0.55-0.70, Camera High top-down (UI dropdown), Output 64×64
7. Generate → 10+ variation çıkar
8. Karakter tanıma listesinden sırayla 10 sınıfı belirle + en uygun olanı seç + kaydet

### Karakter tanıma (output'tan)

1. Crimson chestplate + chest emblem + dark messy hair → Warblade
2. Ice-blue robes + honey-blonde bun + circlet → Elementalist
3. Indigo haori + topknot + eye-band → Ronin
4. Rust-brown studded harness + matted hair → Ravager
5. Void-purple hood + curse-mark + gaunt → Hexer
6. Ochre sleeveless + hand bandages + shaved head → Brawler
7. Void-purple cloak + half-mask + cyan eyes → Shadowblade
8. Forest green cloak + leather hood + braid → Ranger
9. Deep teal robe + bone circlet → Summoner
10. Dusty bronze coat + wide-brim hat + auburn hair → Gunslinger

### QC

- 10 farklı karakter, identity çakışması yok
- Hepsi chibi 3-4 head tall (Warblade ref kalite seviyesinde)
- Açı high top-down (shoulder diagonal, head slightly compressed)
- Silahsız (eller / sırt / bel / omuz / thigh hepsi boş)
- Cinsiyet doğru (5 male / 5 female sırayla: 1m, 2f, 3m, 4m, 5f, 6m, 7m, 8f, 9f, 10f)
- Palette her sınıf için description'a uygun
- Identity reference Warblade'e bulaşmamış (yeni karakterlerin kendi kıyafet/palette var)

FAIL durumları:
- Identity Warblade'e bulaşmış (tüm karakterler plate armor / crimson palette) → reference description "do not transfer" güçlendir + style strength düşür (0.40-0.55)
- Açı yine yan/düz → main prompt camera cümleleri başına çek + UI dropdown High top-down doğru mu kontrol
- Silah var → negative prompt yapışmamış olabilir, kutuya tekrar yapıştır + regen
- Chibi değil → reference'tan chibi transferi yetmemiş → style strength yükselt (0.70'e)

End of v8 batch with Warblade style ref.

---

## 10-CHARACTER BATCH WITH CANONICAL IDENTITY (v9 — DEPRECATED, used stale local memory)

⚠️ v9 local memory'den yazıldı, NLM canonical farklı çıktı. **v10 kullan (en altta).**

## (v9 archived below for diff comparison — DO NOT USE)

> Canonical kaynak: project_character_visual_identity.md + project_class_colors.md + project_class_genders.md + project_class_identity_pivots_s43.md (RIMA memory)
> Mode: PixelLab → Create from Reference
> Style ref panel: chibi Warblade sprite (image #7)
> Settings: style strength 0.55-0.70, Camera High top-down, 64×64

### REFERENCE DESCRIPTION

```
Use this reference for chibi proportions (3 to 4 heads tall, oversized head about 40 percent of total height, short stubby legs about 25 percent), the high top-down camera angle (30 to 35 degrees downward tilt, ARPG-style overhead, shoulders visibly angled diagonally, head slightly compressed by camera tilt), the pixel art style (1 pixel solid outline, hard pixel edges, no anti-aliasing, flat cell shading max 2 tones per color), and the dark gritty pixel art mood.

Do not transfer: identity, face, beard, hair style, exact armor design, chestplate shape, color palette, clothing details, or any specific design element of the reference character. The new characters are completely different people with their own unique outfits, skin tones, hair, and palettes described in the main prompt.

Ten visually distinct adult characters total, all weapon-free, no two share identity.
```

### MAIN PROMPT (canonical identity, 10 karakter tek blok)

```
ABSOLUTE RULE: the generated image must contain NO text, NO words, NO letters, NO labels, NO character names, NO captions, NO numbers. Every character is completely weapon-free.

Pixel art chibi character batch — 10 distinct game sprites for a top-down 2D roguelite. Style: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Each sprite is 64x64 pixels, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color.

Chibi proportions: compact 3 to 4 heads tall, oversized head about 40 percent of total height, broad shoulders, short stubby legs about 25 percent of total height. Sprite fills 85 to 90 percent of canvas vertically.

ABSOLUTE CAMERA RULE: high top-down camera angle, 30 to 35 degrees downward tilt, ARPG-style overhead bird's-eye perspective like Hades. All sprites face south, viewed from above at a clear diagonal, shoulders visibly angled diagonally toward camera, head slightly compressed vertically due to downward tilt. No flat front view, no side profile, no isometric.

WEAPON-FREE RULE: each character carries no weapons of any kind. The body is completely free of swords, axes, bows, daggers, pistols, staves, orbs, lanterns, shields, sheaths, scabbards, holsters, quivers, weapon belts, weapon harnesses, weapon straps. Both hands hold nothing, palms open and clearly empty at the sides. The back, belt, shoulders, hips, and thighs are completely empty of weapons or weapon attachments.

POSE: all characters stand in a calm balanced idle pose, both feet on the ground at shoulder width, arms relaxed and open at the sides, head looking forward, face calm and neutral, body symmetric.

The ten characters in order, all adult, all weapon-free:

— Adult male, light tan skin, dark short messy hair, dark masculine beard. Broad-shouldered muscular man. He wears full steel plate armor across his torso with an ember-orange tabard hanging in symmetric folds over his legs. Masculine weathered face, stern expression. Palette: dark gray and black steel plate dominant, ember orange-red tabard accent, bronze trim details, light tan skin.

— Adult female, pale skin, warm honey-blonde hair pulled back in a tight bun with a few face-framing strands. Slim refined woman. She wears a cropped indigo top with cream undertones, midriff visible, paired with flowing indigo lower garments. Pale feminine face, calm controlled expression. Palette: deep indigo cropped top dominant, cream undertones, warm honey blonde hair, faint warm amber glow accent.

— Adult male, Asian features and skin tone, black hair tied in a low knot at the back of the head. Lean disciplined man. He wears a sage green over-robe layered over indigo hakama trousers. Asian masculine face, calm focused expression. Palette: sage green over-robe dominant, indigo hakama accent, black hair, cold silver-blue accent details.

— Adult male, sunburned weathered tan skin, wild blond-red shoulder-length matted hair with symmetric volume, blond-red masculine beard. Heavily-built bulky man, completely shirtless with a visible diagonal chest scar. He wears only rough brown fur garments around the waist. Savage masculine face. Palette: brown fur waist garment, sunburned tan skin dominant, blond-red hair and beard accent, bone-white accent details.

— Adult female, gray-pale sickly skin, long dark hair partially visible under a drawn-up hood. Slim sickly woman with hollow features. She wears an asymmetric dark crimson tunic with a black hood drawn up symmetrically. Gaunt feminine cheeks, shadowed eyes, cold expression. Palette: dark crimson tunic dominant, black hood, gray-pale skin, faint green-purple glow accent.

— Adult male, deep ebony skin, very short tight fade hair. Stocky compact muscular man, completely shirtless showing bare muscular chest. Amber-colored tribal tattoos visible on his chest and arms in a symmetric pattern. Strong masculine jaw, determined expression. He wears burnt orange and rust-colored garments around the waist. Palette: burnt orange and rust waist garment dominant, deep ebony skin, amber tribal tattoo accent, faint void-purple highlights at the wrists.

— Adult male, olive skin, black short hair. Slim agile lean man. He wears leather chest armor in midnight blue with a waist cloth wrap of the same tone. A neutral mask covers the lower half of his face. Masculine eyes visible above the mask. Palette: midnight blue leather and waist cloth dominant, black mask, olive skin.

— Adult female, tan skin with visible freckles across the nose and cheeks, auburn red-brown hair tied in a practical braid down the back. Athletic medium-build woman. She wears a sleeveless forest-green leather vest with a carved bone shoulder guard on the right shoulder. Tan freckled feminine face, alert expression. Palette: forest green and olive vest dominant, warm tan leather, bronze buckles, bone-white shoulder guard accent, auburn hair.

— Adult female, white-pale skin, long silver-white hair flowing loose. Medium-build feminine woman. She wears a long black mantle that drapes symmetrically down to the ankles, with bone-white trim along the edges. Solemn feminine face, gaze gently downward but forward. Palette: deep black mantle dominant, bone-white trim accent, silver-white hair, faint cold blue glow accent at the chest level.

— Adult female, brown skin, deep dark red hair tied in a braid under a wide-brim hat. Tall lean woman with sharp confident bearing. She wears an asymmetric burgundy long coat where one side hangs longer than the other in deliberate asymmetric flaps. Wide-brim hat tilted slightly forward. Brown feminine face, focused expression with a hint of smirk. No goggles. Palette: burgundy and dark red coat dominant, dark auburn hair, leather tan hat and belt accent.
```

### NEGATIVE PROMPT

```
flat front view, eye-level shot, straight-on view, side view, side profile, three-quarter shoulder turn, isometric projection, low angle, looking up at character, no overhead tilt, text, words, letters, alphabet, glyphs, typography, captions, labels, character names, sprite names, title cards, watermark, signatures, numbers, digits, weapons of any kind, sword, swords, longsword, greatsword, katana, dagger, daggers, axe, axes, hatchet, hammer, mace, spear, polearm, halberd, bow, longbow, crossbow, quiver, arrows, pistol, pistols, gun, rifle, staff, wand, orb, scepter, shield, buckler, sheath, scabbard, holster, holsters, lantern, weapon belt, weapon harness, weapon strap, hilt, blade, blade tip, hilt sticking out, weapon on back, weapon on hip, weapon on belt, weapon on shoulder, X-shaped crossed weapons on back, drawn weapon, weapon visible in hand, ghostly weapon outline, weapon glow, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, realistic proportions, painterly style, anime style, identical characters repeated, child, teenager, young face, goggles
```

### Karakter tanıma (output → sınıf)

1. Light tan + plate armor + ember tabard + beard → Warblade
2. Pale + indigo crop top + blonde bun → Elementalist
3. Asian + sage green robe + indigo hakama + low knot → Ronin
4. Sunburned + shirtless + chest scar + blond-red beard → Ravager
5. Gray-pale + dark crimson tunic + black hood → Hexer
6. Deep ebony + shirtless + amber tattoos + tight fade → Brawler
7. Olive + midnight blue leather + mask → Shadowblade
8. Tan freckles + auburn braid + forest green vest + bone guard → Ranger
9. White-pale + silver-white hair + black mantle → Summoner
10. Brown + asymmetric burgundy coat + wide-brim hat + dark red braid → Gunslinger

### QC

- 10 farklı canonical karakter, identity çakışması yok
- Cinsiyet: 5M (1,3,4,6,7), 5F (2,5,8,9,10)
- Skin tonu canonical: light tan / pale / Asian / sunburned / gray-pale / deep ebony / olive / tan-freckles / white-pale / brown
- Saç canonical: dark messy / blonde bun / black low knot / blond-red matted / dark under hood / tight fade / black short / auburn braid / silver-white loose / dark red braid
- Palette canonical (dominant color her sınıf için doğru olmalı)
- Hepsi chibi 3-4 head tall
- Hepsi silahsız
- Açı high top-down (shoulder diagonal)

End of v9 (DEPRECATED).
