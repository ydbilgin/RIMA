# Research: Hades & Hades-likes -> RIMA adaptation ideas — CX (feasibility / map-structure lens)

ACTIVE RULES: (1) think before answering (2) concrete + codeable — tie each idea to what's buildable on RIMA's existing spine (3) no speculation about systems that don't exist (4) flag conflicts with LOCKED direction.

NLM ACCESS: not required, but you may query:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

## Your lens (CX = implementation feasibility + map/room structure)
For each adaptation idea, judge HOW it would be built on RIMA's actual code spine and rank by ROI (impact / effort). Focus especially on **room/map structure, run flow, and the draft/reward loop** — the things RIMA's RoomLoader + DraftManager + Gate already model. Mine: Hades 1/2 (room-exit boon choice, doors telegraph reward, Chaos gates, mini-bosses), Cinderia, Dead Cells (biome branching, scrolls/cells, cursed chests), Curse of the Dead Gods (corruption/greed risk, room blessing), Enter the Gungeon (curse/reward), Risk of Rain 2 (time pressure scaling), Wizard of Legend (arcana shop), Returnal (room modifiers).

## RIMA spine you're targeting (real classes)
- `RIMA.Systems.Map.RoomLoader` — loads 5 rooms + boss arena sequentially, `OnRoomLoaded` event, room-clear -> next.
- `DraftManager` — between-room skill/passive draft (pick 1 of N).
- `Gate` + `MapFragment` — fragment drops on clear -> unlocks the gate to advance.
- Boss `PenitentSovereign` — 50% chain-break, 33% Phase-3.
- Combat: cursor-aim, 4 skills Q/E/R/F + ult, per-class resource, dash i-frames. Signature verb "Sundered Beat" (BREAK guard -> EXECUTE).
- LOCKED: cursor-aim, floating-island, PixelLab sprites, cyan-sparing, painterly slash VFX. Do not propose breaking these.

## Output format — RANKED by ROI (impact/effort), best first
Each: `[Source game] -> [Mechanic] -> [RIMA mapping on which existing class/system] -> [Impl cost: S/M/L + what code changes] -> [theme fit] -> [conflict flags]`.
12-18 ideas. End with "TOP 5 by ROI for the 10-min demo".

## Save your answer
Write the full ranked answer as your final summary (it lands in CODEX_DONE_<profile>.md). Also write it to `STAGING/RESEARCH_HADESLIKE_CX.md`.
