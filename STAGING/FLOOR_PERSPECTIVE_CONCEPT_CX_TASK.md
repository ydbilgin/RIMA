# CX IMAGEGEN TASK — Floor perspective comparison + wall-less improvement (CONCEPT ART)

ACTIVE RULES: (1) think before drawing (2) on-brand, not realistic (3) surgical — only the 3 images requested (4) BLOCKED note if a tool limit blocks you.

NLM ACCESS (optional): uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"  (auth may be expired; if so, use the briefs below — do NOT block).

## GOAL
The user wants to SEE the difference between **top-down 3/4** and **isometric** for RIMA, grounded in the project's own character + floor tile, and get IDEAS for improving the WALL-LESS floating-island look. Produce **3 concept illustrations** via image_gen. These are THINKING MOCKUPS, not shippable game assets — generate freely (this is the sanctioned exception to "characters = PixelLab only"; here the character is just illustrative).

## REFERENCE IMAGES (use as style reference / init image if your tool supports it; else describe-match)
- Character (hero): `Assets/Resources/Characters/Warblade/warblade_idle_south.png` — chibi-proportioned warrior, blackened-iron/charcoal plate armor, dark hair, top-down 3/4 pixel sprite. Give him a glowing CYAN greatsword.
- Floor tile (ISO version): `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/tile_3.png` — dark cracked stone-masonry slab drawn as a 35-degree isometric diamond.
- Floor tile (SQUARE/top-down version): `Assets/Sprites/AssetPackV3/floor/tile_3.png` — dark stone-brick slab drawn flat top-down.

## ON-BRAND PALETTE (HARD)
charcoal / blue-slate / blackened-iron `#1C1D24` -> `#2E303F`, with CYAN `#00FFCC` used SPARINGLY (seal-cracks, rift energy, weapon glow only). Floating severed seal-keep fragment over a deep purple-to-black void. Painterly, pixel-leaning.
HARD-NO: photoreal, glossy 3D bevels, vector gradients, gold, parchment, neon overload, baked UI text frames/borders.

## DELIVERABLES — 3 PNGs to `STAGING/floor_perspective_concepts/`
Name them exactly: `01_isometric.png`, `02_topdown_3q.png`, `03_wallless_improved.png`. Aim ~1024-1536px each.

### 01_isometric.png  (the WRONG-for-us look, for comparison)
The hero standing on a small patch (~5x5 tiles) of dark cracked stone-masonry floor, rendered in **CLASSIC 2:1 ISOMETRIC projection**: floor tiles are clear DIAMOND/RHOMBUS shapes, camera looks down the diagonal at a shallow ~30-35deg angle, the hero is posed at the matching iso angle. Floating keep fragment over purple void, faint cyan seal-cracks in the stone. The diagonal diamond grid should be obvious. Burn a small caption in a corner: "ISOMETRIC".

### 02_topdown_3q.png  (the TARGET look — Hades / Children of Morta)
Same hero, same dark stone floor, but **HIGH TOP-DOWN 3/4 projection**: camera looks down steeply ~70-80deg from horizon. Floor tiles are SQUARE, grid lines aligned to the screen's horizontal & vertical axes (NOT diamonds), with only a SUBTLE vertical foreshortening (tiles slightly wider than tall) and faint low-contrast seams. Hero drawn top-down 3/4 (top of head + shoulders visible, facing viewer). Same floating fragment, void, cyan seal-cracks, same palette. Caption: "TOP-DOWN 3/4".

### 03_wallless_improved.png  (IDEAS — how to make a wall-less floating arena read with depth)
An IMPROVED top-down 3/4 concept for a **WALL-LESS** floating-island combat arena. Show how to give it depth + a clear floating edge WITHOUT any walls:
- bright CYAN rim-light tracing the broken stone edge,
- floor plane subtly foreshortened, low-contrast tile seams,
- floating rock shards + hanging iron chains around the rim (height cues instead of walls),
- soft pools of cyan light on the floor + scattered debris/decals that break the tile grid,
- drifting edge-fog/mist where the island meets the void,
- the hero standing center on solid-reading ground that clearly FLOATS.
Caption: "WALL-LESS — improved".

## NOTES
- If image_gen cannot ingest the reference PNGs, match them from the descriptions above.
- Keep all 3 in the SAME art style + palette so the comparison is fair.
- Report the 3 output paths at the end. Do NOT touch any other project files.
