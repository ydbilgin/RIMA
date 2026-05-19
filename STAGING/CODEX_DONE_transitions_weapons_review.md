# Transitions Pipeline + Weapons Spec Review

## Question 1: Transitions Pipeline

Verdict: C - Split.

Use PixelLab for organic transition language: moss creep, sand fade, dirt blend, grass erosion, small biome-overlap decals. PixelLab's `create_topdown_tileset` is valuable when a biome series is naturally connected and benefits from consistent corner-based autotiling, especially foundation-to-foundation blends.

Keep Codex gpt-image-1 for hard Wang16 boundaries and authored adjacency decals where composition control matters: cliff lips, water borders, stone cuts, ruins, sharp elevation/readability edges. Hard boundaries are gameplay-facing readability assets, not just pretty blends; Codex remains better for deliberate silhouettes, explicit tile semantics, and the current v15d lock's visual direction.

`hybrid_asset_pipeline_lock` implication: extend the lock from "PixelLab = characters/mobs/props; Codex = tiles/walls/decals" to "PixelLab = characters/mobs/props + organic foundation transitions; Codex = hard Wang16 boundaries, walls, decals, and authored transition edges." Do not move all transitions to PixelLab until a real Wang16 hard-boundary proof beats Codex in readability.

## Question 2: Weapons Production Spec

Verdict: weapons should be separate PixelLab `create_object` assets first, not baked into character sprites. Use transparent background, 48x48 default, upscale only for long weapons.

Sprite size by weapon:
Warblade sword 64x64; Ronin katana 64x64; Gunslinger pistol 32x32; Elementalist staff/orb 64x64; Shadowblade dagger 32x32; Ranger bow 64x64; Brawler fist gloves 48x48 pair or two 32x32 hands; Ravager axe 64x64; Hexer wand 48x48; Summoner focus 48x48.

Tool: PixelLab `create_object` for the production pass because it supports transparent isolated objects at the needed sizes and keeps style controllable. Use `create_image_pro` only for a later polish pass on hero weapons. Avoid Codex imagegen unless PixelLab cannot produce a clean silhouette.

Direction count: produce 5 directions + 3 mirrored for sword, katana, staff/orb, bow, axe, wand, and focus. Use 1-dir with Unity SR rotation only for pistol, dagger, and fist gloves if tests show the rotation reads cleanly; otherwise promote them to 5+3.

Anchor point: blade weapons at handle base/grip; pistol at hand grip; staff/wand at lower hand grip; bow at grip center; gloves at wrist/knuckle attachment; axe at handle base; focus at palm/contact point.

Production order: 1 Warblade sword, 2 Ronin katana, 3 Gunslinger pistol, 4 Ranger bow, 5 Elementalist staff/orb, 6 Shadowblade dagger, 7 Ravager axe, 8 Hexer wand, 9 Brawler gloves, 10 Summoner focus.
