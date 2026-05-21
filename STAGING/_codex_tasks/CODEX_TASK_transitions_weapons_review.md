# Codex Task — Transitions Pipeline + Weapons Spec Review (small chunks)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Scope
**NO CODE — design review only, 2 questions ≤ 300 words each.**

## Background
RIMA hybrid asset pipeline LOCK (S87 Karar #157): PixelLab = characters/mobs/props, Codex gpt-image-1 = tiles/walls/decals. User is reconsidering whether transitions should also move to PixelLab.

PixelLab MCP available tools:
- `create_tiles_pro` — 16 variations from numbered descriptions, segmentation mode for lineless smooth tiles, 32×32 size
- `create_topdown_tileset` — Wang tileset for top-down maps with corner-based autotiling (16 or 23 tiles), supports `transition_size` 0.0-1.0, can chain via base_tile_id for connected biome series (ocean→beach→grass→stone)
- `create_object` — 32-256px, 1-direction or 8-direction
- Codex gpt-image-1 — current tile producer for v15d

Memory references: `hybrid_asset_pipeline_lock`, `feedback_blueprint_first_map_design`, `weaponless_animation_v1` (silahsız body + WeaponSR child SR), `8dir_mirror_production_strategy` (5 produce + 3 mirror).

## Question 1: Transitions pipeline (≤ 300 words)

User asks: foundation (floor) PixelLab; should TRANSITIONS (biome-to-biome edges, Wang16 hard boundaries, adjacency decals) also be PixelLab — OR keep Codex for transitions?

Options:
- A) ALL PixelLab — `create_tiles_pro` for foundation + `create_topdown_tileset` Wang for transitions
- B) Hybrid keep — PixelLab foundation + Codex transitions (current LOCK extended)
- C) Split — PixelLab organic transitions (moss creep, sand fade) + Codex hard Wang16 (cliff, water borders) 

Verdict: A / B / C + 2-sentence reasoning. Update hybrid_asset_pipeline_lock implication.

## Question 2: Weapons production spec (≤ 300 words)

10 classes need weapons (Warblade sword, Ronin katana, Gunslinger pistol, Elementalist staff/orb, Shadowblade dagger, Ranger bow, Brawler fist gloves, Ravager axe, Hexer wand, Summoner focus).

Per `weaponless_animation_v1` LOCK: silahsız body + WeaponSR child SR — weapons are SEPARATE sprite children attached to character, NOT baked in.

Per `8dir_mirror_production_strategy`: 5 directions produced + 3 mirrored.

Constraints:
- Character size: 120-128px (chibi anchors)
- Weapon scale relative to chibi: ~30-50% character size
- 8-direction handling: weapon orientation tracks character facing
- Transparent background mandatory

Provide:
- **Sprite size**: per-weapon recommendation (32×32, 48×48, 64×64, etc.)
- **Tool**: PixelLab create_object (32-256px) vs PixelLab create_image_pro (web UI, more detailed) vs Codex imagegen
- **Direction count**: 1-dir (Unity rotate at runtime via SR rotation) vs 8-dir (full sprite per facing) vs 5+3 mirror
- **Anchor point**: where on weapon Unity SR pivot goes (e.g. handle base for swords)
- **Production order**: priority list (1-10)

## Output
Write `STAGING/CODEX_DONE_transitions_weapons_review.md` with both verdicts. Total ≤ 600 words. Be opinionated.

## What NOT to do
- No code edits
- No image gen
- No new task files
- No long preamble
