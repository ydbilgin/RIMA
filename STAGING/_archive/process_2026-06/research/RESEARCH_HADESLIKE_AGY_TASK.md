# Research: Hades & Hades-likes -> RIMA adaptation ideas — AGY (web-research / breadth lens)

ACTIVE RULES: (1) think before answering (2) concrete, specific, sourced — no vague platitudes (3) ground every idea in RIMA's actual systems below (4) flag anything that conflicts with RIMA's LOCKED direction.

NLM ACCESS: If you need RIMA design canon, query:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

## Your lens (AGY = broad web research)
Mine MANY games for specific, named mechanics — INCLUDING lesser-known ones. Bring breadth + specifics (how the mechanic actually works in that game), then map each to RIMA. Pull from at least: **Hades 1, Hades 2, Cinderia**, Dead Cells, Hyper Light Drifter, Children of Morta, Bastion/Transistor, Curse of the Dead Gods, Returnal, Enter the Gungeon, Wizard of Legend, Risk of Rain 2, Skul: The Hero Slayer, Ravenswatch, Tiny Rogues, Brotato. Cover: room/map structure & run flow, moment-to-moment combat feel, reward/draft economy, narrative delivery during runs, biome/encounter pacing, boss design, meta-progression.

## RIMA — what you're adapting TO (do not contradict)
- Top-down 2D ARPG **roguelite**, ~10-min vertical-slice demo.
- World "**Sundered Brink**": a floating shattered **seal-keep** fragment in the void. **Cyan #00FFCC = seal energy / reality-cracks**; dark slate/iron palette.
- Boss "**Penitent Sovereign**": chained tragic guardian — breaks chains at 50% HP, "Unleashed" Phase-3 at 33%.
- Structure: **5 rooms + boss arena** (RoomLoader spine). **Skill-draft between rooms** (pick actives/passives). **Map Fragment** drop -> **Gate** unlock.
- Combat: **cursor-aim**; 10 classes (Warblade primary); 4 skills Q/E/R/F + ultimate; per-class resource (Rage etc.); dash w/ i-frames; hit-stop + screenshake + damage-numbers. Signature verb = **"Sundered Beat" (BREAK -> EXECUTE)** — break enemy guard/armor, then execute.
- LOCKED (do NOT propose breaking these): cursor-aim stays; floating-island setting stays; character art = PixelLab pixel sprites; cyan used sparingly; slash VFX = painterly flipbook.

## Output format — write a RANKED list (best ideas first)
For each idea: `[Source game] -> [Mechanic, 1 line how it works] -> [RIMA mapping, concrete] -> [Impl cost for the demo: S/M/L] -> [Why it fits the seal-keep / Sundered Beat identity] -> [Conflict flags, if any]`.
Aim for 12-20 ideas. End with a short "TOP 5 I'd build first" call.

## Save your answer to
`STAGING/RESEARCH_HADESLIKE_AGY.md` (and your normal AGY_DONE file). 
