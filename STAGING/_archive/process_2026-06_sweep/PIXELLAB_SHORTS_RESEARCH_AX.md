# RESEARCH — PixelLab AI YouTube Shorts: extract anything useful for RIMA's pipeline

ACTIVE RULES: (1) think before answering (2) no speculation — say what you could/couldn't access (3) RIMA-relevance filter (4) BLOCKED if unclear.
Respond INLINE in your final message (NOT a file — dispatcher captures stdout). Use your web/browse tools.

## Task
Go through the **PixelLab AI** YouTube Shorts channel: **https://www.youtube.com/@PixelLab_AI/shorts**
Inspect the shorts (as many as you can enumerate), and report **what's genuinely useful for RIMA**, a Unity 2D
top-down pixel-art ARPG roguelite (PPU 64, 8-direction characters via 5-bake+3-mirror, cyan #00FFCC brand, weapons as
HandAnchor child sprites, isometric/topdown tiles, VFX as sprites/particles).

If the /shorts grid is hard to enumerate, also try: the channel's videos tab, web search
("PixelLab AI youtube short <topic>"), and the PixelLab docs/blog the shorts reference.

## What to extract per useful short (skip fluff/marketing)
- **Title / topic** + 1-line what it demonstrates.
- **The actual technique or feature shown** (e.g. a tool, a setting, a workflow, a prompt pattern, a new feature like
  rotation/animation/tilesets/inpainting/style-transfer/skeleton-rigging).
- **Why it matters for RIMA** — concrete: would it improve our character 8-dir bake, weapon mounts, tile/cliff
  generation, VFX sprites, animation states, style consistency, or batch throughput?
- **Any numbers/limits** mentioned (sizes, frame counts, credit costs, canvas maxes).

## Deliverable (inline, structured)
1. A short table: Short → technique → RIMA use → priority (High/Med/Low).
2. **Top 5 takeaways** we should adopt or test, ranked by ROI for our current demo work.
3. Anything that **contradicts or updates** our current PixelLab knowledge (we believe: create_1_direction_object batch
   for weapons, create_character for 8-dir, 64px char / PPU64, S-XL Pro 16:9=688x384 / square≤512, animate step is
   user-gated). Flag new features (e.g. better animation, skeletons, inpainting, tileset tools) we may be missing.
4. What you could NOT access (be honest about enumeration limits).
