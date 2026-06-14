# Research Task — Mina the Hollower (systems / architecture angle)

ACTIVE RULES: (1) think before answering (2) min output, no filler (3) research only — write NO project files, NO code edits (4) BLOCKED if you cannot find sources.

NLM ACCESS: not needed for this task (external game research).

## Goal
Research **Mina the Hollower** (Yacht Club Games, 2025 — Game Boy Color-style top-down action-adventure, whip combat, burrow/hollow mechanic). Focus on **transferable SYSTEMS & ARCHITECTURE patterns**, not lore.

We have two consumer projects:
- **RIMA** — 2D top-down action-roguelite ARPG (Unity, URP 2D). Weapon + skill draft, cyan "seal/rift" aesthetic, floating-island arena demo, boss with phases. Run-based.
- **LaurethStudio** — sister project: game-dev TOOLING / pipeline (map/room editors, sprite & tilemap pipeline, palette-lock tools).

## What to deliver (concise, bulleted, evidence-where-possible)
1. **Combat/equipment system model** — how Mina's charm/equipment + secondary-item ("Tonic"/sidekick) systems are structured. What's the data model implication for a build-variety system? Map to RIMA's weapon+skill-draft roguelite.
2. **Burrow/Hollow mechanic** — mechanically what it does (i-frame dodge? traversal? combat reposition?). Is it a viable "signature traversal verb" RIMA could adapt (we have dash + rift-break)?
3. **Room/dungeon authoring** — Zelda-likes use hand-authored screen-rooms with gated secrets. What level-design/authoring patterns matter, and what would a level/room editor (LaurethStudio) need to support them (screen-locking camera, room transitions, secret reveal, one-way gates)?
4. **Boss structure** — telegraph language, phase transitions, attack-pattern vocabulary. 2-3 concrete takeaways for RIMA's "Penitent Sovereign" boss.
5. **Top 3 "steal this" items for RIMA** and **top 3 for LaurethStudio (tooling/pipeline)**, each one line + why.

Keep total under ~600 words. Cite specifics (mechanic names, system names) where known; if uncertain, say so rather than inventing.
