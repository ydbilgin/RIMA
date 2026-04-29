# RIMA - Gemini Pixel Art Concept Prompts (Strict)
> Use this file when Gemini output must look like pixel art (not photoreal concept render).
> Target: class concept sheets that are visually pixel-art-like before PixelLab production.

---

## Core Rule
- Camera is already mostly correct in your latest outputs.
- Main failure was style drift to photoreal.
- This file hard-locks style to pixel art and gameplay readability.

---

## Antigravity Command
Send this to Antigravity:

```text
Read and apply exactly:
GUIDES/GEMINI_PIXELART_CONCEPT_PROMPTS.md

Generate all 10 classes from scratch as pixel-art concept outputs.
Do not produce photoreal renders.
Use one consistent art pass for all classes (same pixel language, same shading logic).
Reject and regenerate any output that fails RED checks.
```

---

## Global Prompt Prefix (paste at start of every class prompt)
`top-down pixel art character concept, 128x128 game sprite style, visible pixel clusters, hard pixel edges, limited color ramps, minimal anti-aliasing, clean silhouette readability for ARPG gameplay, high overhead top-down camera around 75-80 degree downward angle, top of head visible, body foreshortened from above, full body readable, transparent background preferred (fallback plain neutral background), not isometric, not side-view`

## Global Negative (paste at end of every class prompt)
`photoreal, realistic 3D render, cinematic lighting, depth-of-field blur, smooth airbrush shading, painterly brushstrokes, glossy PBR materials, ultra-detailed skin pores, concept art render style, low top-down, 60-65 degree tilt, 3/4 camera wording, helmet, closed visor, face-concealing mask, chibi proportions, paper cutout look, soft blurry upscale look, ai oil-paint style`

---

## Class Prompts (single block per class)

### 1) Warblade (Male)
`male warblade, mid-age battle veteran (30s-40s), bare head, stern face, partial scavenged chest and shoulder armor over visible cloth and chain, worn dark crimson battle wrap, heavy two-handed greatsword, subtle cold-blue hairline fractures on blade only, grounded practical combat identity, not full plate knight, not paladin`

### 2) Brawler (Male)
`male brawler, clearly masculine read, broad shoulders and thicker neck, reinforced gauntlets, practical rugged armor pieces with visible arm mobility, controlled void-purple marks on gauntlets and tattoos only, close-combat pressure fighter identity, no body aura`

### 3) Elementalist (Female)
`female elementalist, clearly female read, blue-purple hooded robe with cyan rune accents, lightning orb raised in one hand, clean caster silhouette, no staff, no wand, no second weapon`

### 4) Gunslinger (Female)
`female rift-tech pistol duelist, clearly female read, dark fitted tactical combat jacket, dual modified pistols with subtle cold-silver rift trim and small heat-vent grip detail, kinetic run-and-gun posture, not western cowboy, no wide-brim cowboy hat`

### 5) Hexer (Female)
`female hexer, severe calm expression, ragged dark-purple robes, slight hunch, staff with cursed green flame and void-purple skull motif, grounded occult identity`

### 6) Ranger (Female)
`female tactical rift hunter from ruins and dungeons, not forest elf archer, charcoal-slate leather layers with minimal green, low tactical cowl and lower-face wrap, asymmetrical utility belt with trap canisters and tether spool, long thin rift-etched bow, cold-blue only on arrow tips, forward-lean stalker posture`

### 7) Ravager (Male)
`male ravager, brutal raider silhouette, bare head, large body mass but mobile read, partial heavy armor fragments only (spiked shoulders, bracers, belt plates) over leather and cloth, huge double-headed axe, controlled blood-red rage accent #8B1A1A, not full plate juggernaut`

### 8) Ronin (Male)
`male ronin, topknot, layered worn blue-green robes with asymmetrical armor pieces, right katana extended forward in draw-cut, left hand at guard hip, cold silver-blue shimmer on blade edges only, precise tension-burst duelist identity`

### 9) Shadowblade (Male)
`male shadowblade, black-purple tactical suit with gray armor plates, standing coiled forward-lean stance, dual blades extended, controlled void-purple smoke only at blade edges, subtle eye glow, no body-obscuring smoke cloud`

### 10) Summoner (Female)
`female summoner field commander, worn ornate purple-gold robes adapted for movement, command scepter in one hand raised forward, other hand extended in directional control gesture, cold-blue arcane circle at control hand only, practical battlefield command identity`

---

## Assembly Template (Gemini input format)
For each class, build prompt as:

`[Global Prompt Prefix], [Class Prompt], [Global Negative]`

---

## RED Checks (reject and regenerate)
- Looks like photoreal/3D render instead of pixel art.
- Soft painterly gradients with no visible pixel clusters.
- Camera drifts from high top-down.
- Gender lock violation.
- Warblade/Ravager become full-plate tank.
- Elementalist has staff/wand/second weapon.
- Gunslinger reads western cowboy.

## PASS Checks
- Pixel-art-like readability with visible pixel structure.
- 75-80 overhead gameplay-friendly angle.
- Class identity readable in one glance.
- Clean silhouette and controlled accents.

---

## One-Message Prompt (copy/paste to Antigravity)
```text
Read and apply exactly:
GUIDES/GEMINI_PIXELART_CONCEPT_PROMPTS.md

Now generate all 10 classes from scratch using this file's strict pixel-art rules.
For each class, build prompt as:
[Global Prompt Prefix], [Class Prompt], [Global Negative]

Hard requirements:
- Must look like pixel art (not photoreal, not painterly concept render).
- Camera must stay high top-down (75-80°), top of head visible, full-body readable.
- Keep one consistent pixel-art language across all 10 classes.
- Enforce gender locks and class identity locks exactly.
- Reject/regenerate anything that fails RED checks.

Output filenames:
warblade_south_lock.png
brawler_south_lock.png
elementalist_south_lock.png
gunslinger_south_lock.png
hexer_south_lock.png
ranger_south_lock.png
ravager_south_lock.png
ronin_south_lock.png
shadowblade_south_lock.png
summoner_south_lock.png
```
