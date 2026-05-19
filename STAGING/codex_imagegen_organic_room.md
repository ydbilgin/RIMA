# Codex Task — Image Gen: RIMA Sample Room (Alabaster Dawn Quality)

## Görev
**Codex imagegen skill** kullanarak RIMA sample room'unun **gerçek oyun ekranı tarzında concept art screenshot'unu** üret. User'a PIL diyagram tarzı **yetmiyor** — "Alabaster Dawn" gibi gerçekten oyun içindeymiş gibi görünmeli.

## Hedef Görsel — "RIMA Sample Combat Room"

### Genel betimleme
A pixel-art top-down 2D action roguelite game screenshot in the style of Hades, Hyper Light Drifter, Salt-and-Sanctuary, and especially **Alabaster Dawn**. Wide composition showing a complete combat room from a high top-down 30-35 degree angle camera. The room feels alive, atmospheric, organic — no visible grid lines, no flat tile borders, everything blending naturally through painterly pixel-art technique with rich lighting and texture variation.

### Detayli olarak gostermesi gerekenler

**Floor:** Dark slate gray and brown weathered stone floor with subtle organic variation per area. No visible tile grid. Some pixel-level moss tufts, dirt patches, scattered pebbles. The floor has lived-in stains and faint dripping marks suggesting old rituals.

**Walls:** Stone block walls visible at the top and bottom edges of the screen (top-down perspective showing the upper face of the walls at a 30-35 degree angle). Walls have subtle moss creeping up from the floor, hairline cracks, weathered texture. Wall corners blend smoothly without visible Wang tile seams.

**Lighting:** A burning iron brazier in the right side of the room with a warm orange ember fire and a soft warm glow halo casting light on the surrounding floor. Two tall wax candles near the walls also cast smaller warm glow halos. Dark cool ambient blue-teal tone overall (deep blue-teal #1A2438 ambient).

**Props (placed organically, not grid-aligned):**
- A weathered wooden crate with iron banding (left-center area, slightly tilted)
- A broken ancient stone urn with hairline cracks (top-middle area)
- A burning iron brazier (right side)
- Two tall wax candles (one near left wall, one near right wall)
- Pixel debris and a few bone fragments scattered around the floor
- A dark crimson ritual stain in the center-right floor area (irregular oval, like a rift scar accent)

**Character (THE PLAYER):**
A chibi pixel art character — 3 to 4 head tall, big-head readable face — standing in the center of the room. The character is the RIMA Ranger class:
- Adult female with tan skin
- Off-white bleached-ivory long hair tied in a low loose ponytail
- Battle-worn dark forest green asymmetric armor
- Heavier right pauldron, left forearm wrap
- Cold blue accent strips on the back hood and straps
- Calm balanced idle pose, both feet on ground, arms relaxed and open at sides, weapon-free
- Body weight slightly forward, predator-still alert face

Approximate character height in the scene: should look about 1/8 to 1/10 of the room height (chibi proportions).

**Camera and perspective:** High top-down 30 to 35 degree downward tilt, ARPG style like Hades and Hyper Light Drifter. The character is viewed clearly from above at a diagonal — head and hair crown visible from above, shoulders angled diagonally toward camera, feet small and low. NOT a flat front view, NOT side profile, NOT pure 90-degree top-down.

**Mood and palette — "Vivid Vulnerability + Ritual Catastrophe":**
- Dark gritty atmospheric — NOT sterile, NOT clean, NOT cartoon-cute
- Faded earth tones with subtle accent colors
- Dominant: dark slate gray, deep brown, deep teal shadows
- Accent: warm orange brazier glow, faint dark red ritual stain, deep moss green moss patches, cold blue rim highlights on edges
- Mood: post-battle, ancient, hollow, watchful — like Alabaster Dawn or Salt-and-Sanctuary
- NOT bright colorful — muted, weathered, lived-in

**Style:**
- Pixel art aesthetic but at concept-art resolution (not 64x64 tiny — bigger composition that LOOKS like the in-game view but with concept art polish)
- Hard pixel-ish edges where appropriate (props, character) but soft organic blending where appropriate (light halos, moss patches, rift stain)
- Painterly pixel art — visible pixel grain but organic shapes
- NO visible square tile grid on the floor — tiles BLEND through moss, stains, lighting
- NO UI elements, NO text, NO HUD, NO health bars, NO inventory
- 16:9 ratio composition

### Negative direktifler
- NO visible grid tiles, NO obvious square floor borders
- NO 3D render, NO photorealistic
- NO modern UI elements
- NO text or letters anywhere
- NO multiple identical props side-by-side
- NO weapons in character hands (weapon-free body)
- NO bright cartoon colors, NO anime style
- NO empty bare arena — must be richly textured

## Reference Asset
RIMA Ranger anchor sprite source: `F:/Antigravity Projeler/2d roguelite/RIMA/ANCHORS/characters/04_ranger.png` (64x64 chibi reference — show character matching THIS identity but scaled into the scene properly)

## Codex Aksiyonu
1. **Codex imagegen skill** kullan
2. Yukaridaki promptu image gen tooluna gonder
3. **Hedef output:** 1920x1080 (16:9) veya benzer wide composition, concept art quality
4. **Yatay komposit** (wide screen game view)
5. Output kaydet: `STAGING/concept_art_rima_sample_room.png`

## Iterasyon (Codex'e)
Eger ilk output yetmezse Codex 2-3 iterasyon yapar:
- v1: ilk pass
- v2: feedback uygula (lighting daha mood'lu, palette daha muted, character daha buyuk vs.)
- v3: final concept

## Beklenen Sonuc
Bu screenshot'u user'in raporuna koyacagi quality'de — "Alabaster Dawn quality concept art".

Bu tek bir sample combat room — character + props + lighting + atmosphere ile dolu. Grid VISIBLE OLMAMALI.

## Cikti dosyasi
`STAGING/concept_art_rima_sample_room.png` (concept art)
`STAGING/codex_imagegen_organic_room_DONE.md` (transcript + Codex degerlendirme)
