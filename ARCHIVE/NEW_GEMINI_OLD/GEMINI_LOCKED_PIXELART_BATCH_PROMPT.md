# GEMINI LOCKED BATCH PROMPT (RIMA)

> **QC NOT:** Gemini çıktıları 1024px, RGB, opak arka plan üretir — bu BEKLENEN ve DOĞRU.
> Format gate (128×128, RGBA, transparent) yalnızca PixelLab OUTPUT için geçerlidir.
> Gemini referans dosyalarına format QC uygulanmaz.

Use this prompt **as-is** in Gemini.  
Goal: generate 10 class reference sprites with a **locked camera style** compatible with PixelLab.

## Master Instruction (paste first)
You are generating character reference sprites for a 2D roguelite.

Hard constraints:
- Native pixel art output (not illustration, not painterly)
- 128x128 canvas
- Transparent background
- Single character, full body visible
- Camera: high top-down ARPG view (approximately 35-degree overhead), top of head visible, torso readable from above
- Not isometric grid
- Not side-view
- Not front portrait framing
- Mature realistic proportions: long legs, normal head size, no chibi, no dwarf, no toy-like proportions
- Functional worn gear (not parade armor, not shiny pristine armor)
- Minimal effects only (no giant aura, no heavy particles)
- Single black outline, detailed shading, readable silhouette

Negative constraints (always apply):
- no chibi
- no dwarf-like squat body
- no side profile portrait
- no isometric tile/grid perspective
- no huge glow/aura/fx circles
- no splash-art composition
- no text/watermark/logo/UI panels

Output requirement:
- For each class below, generate 4 variants, pick the best one by readability and camera accuracy, then export only the selected final image.
- Save outputs to:
`F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\CLASS_CONCEPTS\PixelLab_Refs_128\new\new_gemini`

File names (exact):
- warblade_south_lock2.png
- elementalist_south_lock2.png
- shadowblade_south_lock2.png
- ranger_south_lock2.png
- gunslinger_south_lock2.png
- hexer_south_lock2.png
- summoner_south_lock2.png
- brawler_south_lock2.png
- ronin_south_lock2.png
- ravager_south_lock2.png

## Class Prompts (run one by one)

### 1) Warblade (Male)
battle-hardened male warblade, last survivor of a broken martial order, bare head no helmet, worn functional scavenged dark plate mixed with cloth, dark crimson battle wrap, two-handed greatsword held low, subtle cold blue hairline cracks only on blade metal, mature mid-age face, high top-down 35-degree overhead, top of head visible, full body readable, realistic long-leg proportions, pixel art 128x128 transparent background

### 2) Elementalist (Female)
female rift storm elementalist, one raised lightning orb only, no staff no wand, practical blue-purple robe with modest slightly open neckline and flowing cloak, cyan-blue orb accent, high top-down 35-degree overhead, top of head visible, full body readable, mature realistic proportions, pixel art 128x128 transparent background

### 3) Shadowblade (Male)
male shadowblade void assassin, lower-face shadow wrap (not full mask), dual void blades, subtle void-purple blade wisps, black-purple suit with gray armor plates, low stealth-ready stance but still full body readable from above, high top-down 35-degree overhead, pixel art 128x128 transparent background

### 4) Ranger (Female)
female tactical rift hunter (not forest archer), charcoal-slate practical gear, longbow with one nocked arrow, trap canisters and tether spool visible, cold blue accent only on arrow tip, high top-down 35-degree overhead, top of head visible, full body readable, mature realistic proportions, pixel art 128x128 transparent background

### 5) Gunslinger (Female)
female rift-tech run-and-gun duelist, dual pistols, long copper-orange hair clearly visible, practical fitted combat coat (not cowboy), subtle rift marks on barrel only, high top-down 35-degree overhead, top of head visible, full body readable, mature realistic proportions, pixel art 128x128 transparent background

### 6) Hexer (Female)
female hexer curse-and-decay caster, dark staff plus iron lantern, ragged dark-purple practical robes, cursed green and void-purple accent details, hunched but readable silhouette, high top-down 35-degree overhead, full body readable, mature realistic proportions, pixel art 128x128 transparent background

### 7) Summoner (Female)
female summoner battlefield commander, scepter in one hand and open palm in the other, no giant runic circle, ornate but practical purple-gold robes, cold blue crystal accent, high top-down 35-degree overhead, top of head visible, full body readable, mature realistic proportions, pixel art 128x128 transparent background

### 8) Brawler (Male)
male brawler close-pressure fighter, unarmed fists, muscular build, torn green vest with bare arms, reinforced gauntlets, subtle void-purple tattoo accents, grounded stance, high top-down 35-degree overhead, full body readable, long-leg mature proportions, pixel art 128x128 transparent background

### 9) Ronin (Male)
male ronin exile swordsman, layered blue-green robes, dual katana one drawn one sheathed, clean readable silhouette, minimal cold silver blade-edge accent, topknot visible, high top-down 35-degree overhead, full body readable, mature realistic proportions, pixel art 128x128 transparent background

### 10) Ravager (Male)
male ravager blood berserker with the broadest roster silhouette, dual large notched axes, heavy worn practical armor, blood-red scarification accents, no helmet, high top-down 35-degree overhead, top of head visible, full body readable, mature realistic proportions, pixel art 128x128 transparent background

## Acceptance Check (apply before saving each file)
- Camera feels ARPG overhead (not side, not isometric)
- Character is full-body readable at 128px
- Long-leg adult proportions are preserved
- Class identity is clear by weapon + outfit + accent color
- Effects remain minimal and non-flashy
