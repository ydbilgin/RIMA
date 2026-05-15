# S82 Twitter Research - SENTEZ

## RIMA Lockable Kararlar (oneri)

1. Karar #14X - Organic water/river masks. River, shore, swamp, and wet-bank areas should be generated as organic masks over the room skeleton, with nearby cliff/path rules adjusted so water never looks pasted on.
2. Karar #14X - Generated asset index. Every asset batch that enters a prototype must include an asset index with path, frame size, frame count, offsets, intended animation state, and integration notes.
3. Karar #14X - Candidate-first visual pipeline. For important backgrounds, rooms, hero props, or tilesets, generate 4 candidates, preview them in-engine, then lock one. Raw image quality alone is not enough.
4. Karar #14X - Debug overlays are production tools. Hitboxes, interact radii, walk bounds, wave state, health bars, audio toggles, and restart cleanup state should be visible in prototype/debug panels.
5. Karar #14X - AI asset QA gate. AI-generated tiles/props must pass seam, alpha, scale, silhouette, and palette checks in a pixel editor before they can become production assets.
6. Karar #14X - Biome hero props. Each RIMA biome should have 3-5 large anchor props or edge pieces beyond the repeatable tileset, so procedural rooms keep scenic identity.
7. Karar #14X - Combat readability before spectacle. Slash arcs, elite telegraphs, damage flashes, and short freeze/shake should be readable at 64px chibi scale and should not clutter the 35-degree field.

## Oyun Fikirleri (studio)

1. Gossip Farm Roguelite. A cozy farm game where crops grow from secrets: plant rumors, harvest favors, manage trust, and survive festivals where lies become monsters or social penalties.
2. Spell-Drawing Duel. A small PvP arena game where players draw 3-5 stroke sigils to cast variants, with simple arenas and deep mastery from gesture timing and spell grammar.
3. Bridgewright Villages. A repair/crafting sim where every bridge reconnects trade, monster routes, and local stories; the map changes politically and economically as crossings reopen.
4. Island Fishing Micro-RPG. A one-screen fishing game where tides, catches, and fish cards mutate the island layout, shop stock, and risk/reward spots over short runs.
5. Desert Caravan Generator. A procedural travel game where Voronoi rivers, cliffs, and oasis masks define routes, ambushes, settlement placement, and resource pressure.

## Pipeline Tavsiyeleri

- Link 7 is the strongest process reference: use starter templates, skill packs, generated candidates, an asset index, browser/in-engine test loops, debug panels, restart cleanup checks, then audio/polish.
- Link 3 should feed RIMA map tooling: create macro graphs first, then layer organic masks for rivers, banks, vegetation, and terrain transitions.
- Link 11 is the caution: Gemini can give usable rough object/tile ideas, especially large objects, but final RIMA tiles need PixelLab or manual pixel cleanup.
- PixelLab create_tiles_pro should be preferred for controlled RIMA tile packs; use 64px targets when matching RIMA scale, with style refs and seam review.
- Use PixelLab/map object generation for hero props and large organic objects, then clean them in Aseprite/EDGE3/Krita before integration.
- Codex should own mechanical integration: asset indexes, validators, Unity import settings, debug overlays, tile seam tests, and prototype mechanics.
- Gemini is best for variant prompts, design rationale, rough concept comparisons, and batch doc summaries; do not treat it as final asset authority.

## Pixellab Interpolate Notu

No clear interpolate-styled animation pipeline was directly shown in the downloaded media. The closest related idea is the link 7 candidate/sub-agent workflow and link 11 AI variant comparison. For RIMA brainstorm: try interpolation only for idle/VFX loops or large object state transitions, then manually select frames and clean silhouettes.

## INCOMPLETE

None. All 11 links downloaded with gallery-dl, all have frames/contact sheets, all have notes.md, and link 7 has full video, 17 frames, audio extraction, and Whisper transcript.
