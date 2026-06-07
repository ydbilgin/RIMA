ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" if needed. Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# Amaç (Purpose)
GENERATE 3 on-brand game-asset images via the **$imagegen skill** (built-in image_gen / generate2dsprite pipeline — NOT procedural code) for the room-exit PORTAL/door, the REWARD relic, and the ECHO currency. Then identify WHERE each goes (a placement manifest). DO NOT TOUCH UNITY AT ALL (the orchestrator is editing Unity scenes in parallel; the user will place these images later). Pure image generation + local file write only.

# HARD CONSTRAINT — ZERO UNITY
- Do NOT call any Unity/MCP tool. Do NOT open/save scenes. Do NOT use AssetDatabase. Do NOT write into `Assets/` (that would trigger a Unity import). 
- Write ALL outputs to `STAGING/imagegen/assets/` (this folder is OUTSIDE Assets/, so Unity ignores it). Create the folder if missing.

# Generate (use $imagegen / generate2dsprite: image_gen with a SOLID MAGENTA #FF00FF background, then chroma-key the magenta to transparent, crop to content, resize to target; run pixel_cleanup if the key leaves fringe). Output transparent PNGs:
1. `STAGING/imagegen/assets/portal_arch_gen.png` — a standalone GRANITE STONE ARCHWAY (two slate pillars + lintel) framing a tall vertical glowing CYAN rift inside it; nothing else (no surrounding island). Tall. Target ~**128x256** (keep it crisp; we downscale on import). 
2. `STAGING/imagegen/assets/reward_relic_gen.png` — a single iron-framed cyan gem charm icon (the relic from the concept). Target ~**128x128**.
3. `STAGING/imagegen/assets/echo_mote_gen.png` — a single bright cyan energy mote/spark with soft glow. Target ~**64x64**.

# Style anchors (MATCH)
- Portal: `STAGING/imagegen/concept_portal_single_*.png` (granite arch + cyan rift).
- Relic + Echo: `STAGING/imagegen/concept_reward_pickup_currency_*.png`.
- Palette: slate greys (#2E333B/#3C434D/#59626E) + iron (#1B1E24/#717C8A) + ONLY-saturated cyan seal-energy (#BFFFF2->#00FFCC->#00A88C). NO text, NO photoreal, transparent bg.

# Placement manifest (write `STAGING/imagegen/assets/PLACEMENT_MANIFEST.md`)
For EACH image, state exactly where it should later be wired (so the orchestrator/user can place it):
- portal_arch_gen -> DoorNorth GameObject's GateBehavior.spriteUnlockedBase AND spriteRoomCombat, in scenes _IsoGame / _IsoGame_Map02 / _IsoGame_Map03. Import as Sprite(Single), PPU 64, Point filter, alphaIsTransparency, pivot **bottom-center (0.5,0)**. Recommended in-game size note.
- reward_relic_gen -> RoomClearVictoryTrigger.rewardSprite, same 3 scenes. Sprite/PPU64/Point/alpha, pivot center.
- echo_mote_gen -> (future) Echo currency pickup; no wiring yet. Sprite/PPU64/Point/alpha, pivot center.
Also note that the OLD procedural placeholders (Assets/Sprites/Environment/Portal/portal_arch_cyan.png, Assets/Sprites/Reward/reward_relic_cyan.png) are what these replace.

# Verify (NO Unity)
Confirm the 3 PNGs exist in STAGING/imagegen/assets/, are transparent (not magenta), and roughly the target sizes. Confirm the manifest written. Report each path + dimensions. If image_gen is unavailable in your environment, STOP and report BLOCKED (do NOT fall back to procedural and do NOT touch Unity).

# Notes
profile = yekta. NO commit. NO Unity. NO new .cs.

# Output
3 image paths + dimensions + transparency confirm; manifest path; BLOCKED if image_gen unavailable.
