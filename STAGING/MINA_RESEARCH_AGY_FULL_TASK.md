# Research Task — Mina the Hollower (FULL, ALL ANGLES, extra comprehensive report)

ACTIVE RULES: (1) think before answering (2) dense but no filler (3) research only — write NO project files (4) label uncertain claims, do NOT fabricate quotes/numbers.

NLM ACCESS: not needed (external game research). Use deep web research, multiple sources, cite where possible.

## Goal
You already gave a pipeline-focused pass. Now do a **single comprehensive ALL-ANGLES report** on **Mina the Hollower** (Yacht Club Games, 2025 — Game Boy Color-style top-down action-adventure; chain-whip combat; "Hollow"/burrow signature mechanic; Kickstarter origin; proprietary engine drawing textured 3D triangles in a GBC-constrained look). Cover EVERY angle below, don't restrict to art. Go deeper than a summary — this is the "extra" thorough report.

Two consumer projects to map findings to:
- **RIMA** — 2D top-down Unity action-ROGUELITE ARPG. cursor-aim attacks, dash (i-frames), "rift-break" ability, weapon + skill-DRAFT build system, boss ("Penitent Sovereign") with phase transitions, cyan "seal/rift" VFX, PPU 64, 8-dir sprites (5 drawn + 3 mirrored), URP 2D lights, floating-island arena, ~10-min run-based vertical slice.
- **LaurethStudio** — sister project: game-dev TOOLING / production pipeline (sprite + tilemap pipeline, room/map editor, palette-lock, Wang tiles).

## Cover ALL of these (deep, bulleted, cite where possible)
1. **Core combat loop** — whip range/arc/timing, attack chaining, charge/heavy variants, knockback/stagger, the moment-to-moment rhythm. Critic praise/complaints on combat feel.
2. **Burrow / Hollow mechanic** — exact behavior (i-frame dodge? traversal? combat reposition? cooldown/resource-gated?), how it interacts with combat and puzzles. Is it a viable "signature traversal verb" RIMA could adapt next to its existing dash + rift-break WITHOUT redundancy?
3. **Build & progression variety** — charms, equipment, secondary items ("Tonics"?), sidekicks/familiars, currencies, upgrade economy. How they create build diversity, and the underlying DATA MODEL implications. Map to RIMA's weapon+skill-draft roguelite.
4. **Boss & encounter design** — telegraph language, phase transitions, attack-pattern vocabulary, arena design. 2-3 concrete takeaways for RIMA's phased boss.
5. **Level / dungeon / room authoring** — screen-room structure, gated secrets, one-way gates, camera screen-locking, transitions, backtracking. What a room/map EDITOR (LaurethStudio) must support to author this.
6. **Art direction & palette discipline** — GBC constraints they self-imposed (color count per tile, resolution), how they modernized (lighting, parallax, scanline tricks) without breaking the retro read. Concrete numbers if findable.
7. **Sprite & animation production** — perspective, directional approach, frame counts, tooling (Pro Motion NG?), modular sprite assembly, indexed-color workflow. Reusable for RIMA 8-dir + LaurethStudio pipeline.
8. **Tilemap & environment construction** — tileset approach, autotiling vs handcrafted, lighting/atmosphere layering, readability partitioning (desaturated world vs saturated interactables).
9. **VFX & game juice** — hitstop, screen shake, palette-flash damage, telegraphs, high-contrast overlay VFX, audio-visual impact tricks reviewers praised.
10. **Difficulty & accessibility** — difficulty curve, accessibility options, death/checkpoint/save structure; what's relevant to a roguelite.
11. **Reception & commercial** — review consensus (Metacritic/OpenCritic range if known), what reviewers loved/criticized, sales/Kickstarter context. What lessons a small studio should draw.

## Then deliver two ranked lists
- **TOP 6 "steal this" for RIMA (game)** — ranked, each: one-line mechanic + why it fits RIMA + rough implementation difficulty (low/med/high).
- **TOP 6 "steal this" for LaurethStudio (tooling/pipeline)** — ranked, each: one-line + why + difficulty.

Be thorough (this is the deep extra report; ~1000-1300 words OK). Cite specifics (named mechanics/items/tools) where sources give them; explicitly flag anything uncertain.
