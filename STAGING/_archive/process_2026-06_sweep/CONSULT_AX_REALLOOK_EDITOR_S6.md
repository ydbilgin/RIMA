# AX (Gemini) CONSULT — "real game look?" honest gap + REAL map-editor tool UX

Game-design/art-direction/UX researcher. Two questions. Concise, ranked, practical markdown. NO code. Be HONEST (the user suspects the current build looks placeholder, not shippable).

## CONTEXT
RIMA = 2D top-down 3/4 action-roguelite (Unity URP 2D, PPU64; camera flat-ortho, NO tilt; 80 char sprites at ~70-80 deg top-down 3/4, cannot redraw). We built an enclosed "Ruined Keep" room: walls = flat-front imagegen PLACEHOLDER masonry sprites (low detail, slightly stretched), a near-featureless dark floor, ~12 basic 2D point lights (amber torches + cyan altar/gate), a simple radial vignette overlay. Structure = enclosed 4-sided room (N wall + arch, E/W runs, S low parapet w/ entrance gap), 13-wide. The intended FINAL art pipeline is **PixelLab** (these imagegen sprites are explicitly placeholders to be replaced).
User's verbatim doubt: "is there even a REAL GAME look to this, in your opinion?"

## Q1 — HONEST "real game look" gap (priority)
Be candid: what concretely separates this placeholder top-down room from a COMMERCIAL "real game look"? Compare to Hades, Children of Morta, Moonlighter, Tunic, CrossCode, Death's Door. List the gaps RANKED by visual impact:
- wall/prop art detail & lighting (hand-painted vs flat imagegen; baked shading, rim light, normal maps?)
- floor: texture variation, tile seams, cracks, decals, foreshortening (currently featureless dark)
- lighting: dynamic shadows, light shape/falloff quality, color grading, bloom
- props/clutter density & variety (debris, banners, chains, rubble, vegetation)
- particles/ambiance (dust, embers, fog, god-rays)
- cohesion / art-direction consistency
For each: is it worth POLISHING the current placeholders, or go straight to PixelLab finals? Give the highest-ROI ordered path to "looks like a real game." Be honest about whether placeholder-tweaking is a dead end.

## Q2 — REAL in-engine MAP EDITOR tool UX
The user wants a PROPER tool (not a minimal overlay): "for edit mode a real tool opens, my maps are LISTED, I see the top-down view, and when I place things my map is created." So: (a) a MAP LIST / browser (open existing rooms/maps, new map), (b) top-down editing view, (c) place objects/tiles → builds + SAVES the map.
Survey reference editors for this "browse → top-down place → save" loop: RPG Maker, Tiled, Super Mario Maker 2, Core/Crayta, Townscaper, RimWorld/Factorio blueprints, Hammer/level editors, any roguelite room editor. What makes the loop good: map-list panel layout, new/duplicate/delete, thumbnail previews of maps, the place→autosave vs explicit-save, top-down pan/zoom, palette docking. Give a concise dos/don'ts + a recommended window layout (panels) for RIMA's tool.

Keep it tight + decision-useful. Opus makes the final call.
