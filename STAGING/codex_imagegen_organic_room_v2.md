# Codex Task — RIMA Sample Room Concept Art (v2 — Our Actual Pipeline)

## Acil Not — Bu Task v2

**Onceki v1 dispatch (`codex_imagegen_organic_room.md`) yanlis cerceveydi.** "Alabaster Dawn" referans olarak alma — onun yerine RIMA'nin **kendi production pipeline'iyla**, **kendi prop prompt'larimizla**, **kendi karakter anchor'larimizla** anlamli sample room concept art ciz.

User feedback: "alabaster dawn'i kopya alma — bizim gercekten uretecegimiz seylerle, prompt'larimizla, elimizdekilerle anlamli sekilde ciz".

## Asil Goal
Codex `imagegen` skill kullanarak RIMA sample combat room'unun **gercek pipeline'imiz ile uretilecek hali**'ni gosteren tek concept art screenshot.

## Bizim Pipeline (gercek elimizdeki)

### Karakter
RIMA 10 canonical chibi karakter anchor'i `F:/Antigravity Projeler/2d roguelite/RIMA/ANCHORS/characters/`:
- 01_warblade.png (M, dark hair + beard + brown leather + brass)
- 02_brawler.png (M, dark-skin bald + leather wraps + orange knuckle glow)
- 03_ravager.png (M, dark messy hair + dark blood-red armor + harness + studs)
- 04_ranger.png (F, bleached-ivory ponytail + forest green asymmetric armor + cold blue accent) ← bu sahnedeki karakter
- 05_shadowblade.png (M, narrow + near-black purple + void purple glow)
- 06_gunslinger.png (F, brown skin + dark short hair + teal/blue cape — sahnede degil bu sample'da)
- 07_ronin.png (M, samurai topknot + dark navy kimono)
- 08_elementalist.png (F, honey-blonde low bun + dusty indigo + cream sash + teal skirt)
- 09_hexer.png (F, dark hood + dark purple-black robe + dark red hex-rune accent)
- 10_summoner.png (F, long dark hair + indigo green-black robe + cyan trim)

Tum karakterler **silahsiz body** (Karar #144) — silahlar runtime WeaponSR child SR olarak Unity'de eklenir.

### Bizim prop production prompt'larimiz (STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md v6)
Bu file'daki STEP 1-12 prop'lar — her biri **64×64 chibi pixel art** olarak PixelLab Create Image Pro V3 ile uretilecek:

**P0 props (bu gece uretilecek):**
- Wooden Crate (small) — weathered dark brown planks, iron banding, brass rivets
- Stone Urn (broken) — dark gray weathered stone, hairline cracks, hollow top, sigil-line carved grooves
- Candle + Iron Holder — tall thin wax candle, soft warm orange flame, cooled wax drips, dark cast iron base
- Debris Pile — mixed stone rubble + bone fragments scattered, irregular asymmetric outline

**P1 props (yarin):**
- Stone Column intact — tall thin temple column, dark slate gray, simple capital plate, moss tufts at base (64x128)
- Stone Column broken — large diagonal break, leaning upper portion, debris at base (64x128)
- Brazier (lit) — iron three-legged base, shallow wide dish, warm orange ember coals, small contained flame
- Hanging Banner (torn) — long dark crimson red cloth, frayed edges, horizontal tear midway (64x128)

**P2 props (sonra):**
- Stone Altar, Treasure Pile, Hanging Chains, Kneeling Statue

### Bizim style discipline (Karar referanslari)
- **Karar #74:** 64×64 chibi tiles + PPU=64
- **Karar #100:** High top-down 30-35° camera angle LIVE LOCK (Hades match, NOT Slormancer 40-50°)
- **Karar #143-E:** Layered pipeline L1 base + L2 atlas + L3 Wang16 walls + L4 organic + L5 detail + L6 accent + Props
- **Karar #144:** Weapon-free body — WeaponSR Unity child SR
- **Karar #145:** PixelLab Character States workflow

### Tone palette: "Vivid Vulnerability + Ritual Catastrophe + Fractured Epic"
- Dominant: dark slate gray, deep brown, dusty blue
- Accent: faint dark red (rift/curse), warm orange (fire/glow), deep moss green, cold blue rim highlight
- NOT bright cartoon, NOT anime, NOT clean sterile
- Mood: weathered, ritual, post-battle aftermath, ancient

### Bizim Brush V1 katmanli oda mimarisi
**Sample combat room** Brush V1 ile su 6 katman + props ile kompoze edilir:
- L1 base tone (dark blue-teal #1A2438 ambient)
- L2 floor atlas (StoneDungeon Wang16-style floor tiles 64×64, multiple variants, randomly selected per cell)
- L3 wall Wang16 (16-tile corner set 64×96, auto-edge selection at room border)
- L4 organic decals (moss soft oval brushes painted in corners, breaking tile grid visually)
- L5 detail (cracks, rubble cluster, scattered pebbles + bones)
- L6 accent (dark crimson rift scar overlay sprawling across multiple tiles)
- Props placed via PropPlacer (R rotation, Bridson Poisson scatter for natural organic positioning)
- Lighting via Unity URP 2D Lights (warm torch glows + ambient deep blue-teal)

## CIZILECEK SAHNE — RIMA Sample Combat Room

Bu sahne **bizim** asset'lerimizle (yukaridaki prompt'larla uretilecek prop'lar + ANCHORS karakterimiz) kompoze olunca **NASIL gorunecek**'i goster.

### Komposit
Top-down 2D pixel art combat room screenshot, 30-35 degree downward camera tilt (ARPG view).
- Room dimensions: about 16x10 tiles visible
- Floor: dark weathered stone with random tile variation, organic per-area variation (Brush V1 random tile pick), no visible grid lines
- Walls: stone block walls visible on top and bottom edges with Wang16-style auto-edge corner blending, subtle moss creeping up
- Lighting: 2-3 warm torch/brazier light cones casting orange glow halos on the floor, otherwise dark blue-teal ambient atmosphere

### Sahnedeki Asset'ler
**1 RIMA Ranger character** (chibi 64×64 3-4 head tall, bleached-ivory ponytail, dark forest green asymmetric armor, calm idle pose, weapon-free hands) — central position, 3x scale in scene

**Props (yukaridaki STEP 1-8 promptlarimizla uretilecekler):**
- 1 Wooden Crate near a wall
- 1 Stone Urn near a wall, slightly broken
- 1 burning Brazier with warm light cone (right side)
- 2 Candles near the walls (smaller warm glow halos)
- 1 Stone Column (intact) in the room
- 1 Hanging Banner (torn dark crimson) on a wall
- Debris pile in a corner

**L4-L6 katmanlari:**
- Multiple moss patches (deep green) sprawling across tile boundaries in floor areas
- Hairline cracks across multiple tiles
- 1 large dark crimson rift scar accent in the center-right floor area (irregular oval, multi-blob organic, spans ~2x2 tiles, with radial crack lines)
- Scattered pebbles and a few bone fragments across the floor

### Mood
Atmospheric, weathered, alive — like a real game screenshot showing a ranger entering an ancient combat room with old battle scars, moss-grown corners, dim torchlight, ready for the next fight. Hollow, watchful, ritual-touched.

### Camera
High top-down 30-35° tilt, ARPG perspective. Character viewed from above at diagonal. Tile floor visible from above-angle perspective. Wall tops visible (showing the upper plane of the wall blocks at the tilt angle).

## Negative Direktifler
- NO visible square tile grid on floor (tiles BLEND through moss + lighting + variation)
- NO Alabaster Dawn style reference — use OUR Vivid Vulnerability palette
- NO Hades artwork copy — use OUR mood, OUR characters
- NO modern UI, NO HUD, NO health bars, NO inventory, NO text
- NO 3D render, NO photorealistic
- NO bright cartoon colors, NO anime style
- NO weapons in character hands (Karar #144 weapon-free)
- NO empty bare arena — must be richly textured with our actual decals + props
- NO duplicate identical props (one of each, organically placed)

## Iterasyon (Codex serbest)
v1 ilk pass → v2 mood/palette/character size adjust → v3 final concept (en az 2 iterasyon).

## Output dosyalari
- `STAGING/concept_art_rima_sample_room_v2.png` (final concept)
- `STAGING/codex_imagegen_organic_room_v2_DONE.md` (transcript + decisions)

## Kaynak Asset Path'leri (Codex'in inspect/load edebilecekleri)
- `F:/Antigravity Projeler/2d roguelite/RIMA/ANCHORS/characters/04_ranger.png` (karakter reference sprite)
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md` (prop production prompts v6)
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/character_production_prompts.md` (character v11 prompt — Ranger description satiri)
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Environment/StoneDungeon/` (mevcut tile spriteleri — referans)

## Onemli — Pipeline Truth
Bu sahne **uretildigi sirada** boyle gorunecek:
1. PixelLab v6 prompt'lariyla 12 prop uretilir (each 64×64 chibi)
2. Anchors klasorundeki 10 karakter PixelLab character ID alir
3. Brush V1 Editor'da bir oda kompoze edilir (StoneDungeon biome + props + moss + accent)
4. Unity scene'de oda spawn olur, lighting setup yapilir, screenshot alinir

**Concept art bu screenshot'un nasil gorunecegini gostermelidir** — gercek pipeline output projection.
