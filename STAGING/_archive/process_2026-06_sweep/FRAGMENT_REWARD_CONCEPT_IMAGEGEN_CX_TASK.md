# CX IMAGEGEN TASK — Map Fragment + Reward pickups concept art (PLACEHOLDER, PixelLab-standard)

ACTIVE RULES: (1) think (2) on-brand not realistic (3) only the assets requested (4) note BLOCKED on tool limit.

## GOAL
Generate ON-BRAND concept art for RIMA's **ground pickups** so we can SEE what the map fragment + rewards will look like. These are PLACEHOLDERS — they will be re-made in PixelLab later, so produce them at **PixelLab-matching standards** (clean pixel-art, transparent background, point-scalable) so the swap is seamless.

## STANDARDS (match PixelLab so replacement drops in cleanly)
- **Transparent PNG-32** (NOT magenta, NOT a scene — isolated item on transparent bg).
- **Pixel-art / pixel-leaning**, crisp edges (generate larger e.g. 512 then it will be downsampled to the pixel target).
- **Target footprint: 64x64 and 128x128** pixel-art items at PPU 64 (RIMA pickup-item scale). Deliver each item centered with small margin.
- **On-brand palette:** charcoal/blackened-iron/blue-slate (#1C1D24-#2E303F) + CYAN #00FFCC for the seal-energy glow (SPARING — glow/runes only). HARD-NO: photoreal, gloss 3D bevel, gold, vector gradient, neon overload.
- Each item should read as a **floating/bobbing ground pickup** with a subtle cyan glow/emissive (the in-game item bobs + pulses).

## DELIVERABLES → `STAGING/fragment_reward_concepts/` (exact names)
1. `map_fragment.png` — the **Map Fragment**: a broken cyan-veined stone tablet (canonical spec: charcoal stone shard with glowing #00FFCC seal-cracks). The progression pickup. 2-3 small rotation/break variants on one transparent sheet OR a single hero item.
2. `reward_skill_rune.png` — a **skill-draft reward**: a floating cyan rune-stone / sigil shard (player picks up → skill draft). Glowing engraved rune on dark stone.
3. `reward_treasure.png` — a generic **loot/treasure** pickup: a small cracked-open stone cache or relic with cyan glow spilling out. On-brand, no gold.
4. `reward_soul_essence.png` — a **soul/essence** pickup (HP or currency): a small drifting cyan wisp/ember over a dark fragment.
5. `pickup_sheet_contact.png` — a contact sheet of all 4 side-by-side at consistent scale for comparison.

## OUTPUT
Short report: the 5 file paths + 1-line note on each. Keep all 4 items in the SAME pixel-art style + palette. Do NOT touch other project files.
