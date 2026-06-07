ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed scenes only (4) BLOCKED if unclear.
NLM ACCESS: query NLM via uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" if needed. Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# Amaç (Purpose)
REPLACE the crude procedural placeholders with REAL GENERATED art for the room-exit PORTAL/door, the REWARD relic, and the ECHO currency, using the **$imagegen skill** (built-in image_gen / generate2dsprite pipeline — NOT procedural Texture2D code). On-brand, transparent, correctly sized, imported, and re-wired into the 3 live scenes. (The previous task wrongly used procedural code drawing; this redoes it properly with image_gen.)

# Style anchors (MATCH these — they are the approved look)
- Portal/door: `STAGING/imagegen/concept_portal_single_*.png` = a GRANITE STONE ARCH framing a glowing vertical CYAN rift, on a slate floating island. Make the sprite version of just the arch+rift (no surrounding island/ruins).
- Reward relic: the iron-framed cyan gem charm in `STAGING/imagegen/concept_reward_pickup_currency_*.png`.
- Echo currency: the small glowing cyan motes/sparks in the same image.
- RIMA palette: slate/iron greys (#2E333B/#3C434D/#59626E + iron #1B1E24/#717C8A) + the ONLY saturated color cyan seal-energy (core #BFFFF2 -> #00FFCC -> glow #00A88C). NO text, NO photoreal.

# How to generate (use $imagegen / generate2dsprite skill)
For each asset, call image_gen with an on-brand prompt + a SOLID MAGENTA (#FF00FF) background (for chroma-keying), then use the local processor / pixel pipeline to: chroma-key the magenta to transparent, crop to content, and resize to the target px. (This is the generate2dsprite workflow.) If a clean key needs it, run pixel_cleanup. Output transparent PNGs:
1. **Portal/door** -> `Assets/Sprites/Environment/Portal/portal_arch_gen.png`. A standalone stone archway (two pillars + lintel, slate granite) with a tall vertical cyan rift glowing inside it. Final import size target ~**64x128** (tall), pivot **bottom-center (0.5,0)**.
2. **Reward relic** -> `Assets/Sprites/Reward/reward_relic_gen.png`. An iron-framed cyan gem charm, single icon, transparent. ~**64x64**, pivot center.
3. **Echo currency** -> `Assets/Sprites/Reward/echo_mote_gen.png`. A single bright cyan energy mote/spark with soft glow. ~**24x24**, pivot center.
Import each: Sprite (Single), PPU 64, Point filter, alphaIsTransparency=true, pivots as above.

# Re-wire (3 scenes: _IsoGame, _IsoGame_Map02, _IsoGame_Map03)
- DoorNorth GateBehavior.spriteUnlockedBase + spriteRoomCombat = `portal_arch_gen` (replace the procedural portal_arch_cyan refs).
- RoomClearVictoryTrigger.rewardSprite = `reward_relic_gen` (replace reward_relic_cyan).
- Keep DoorTrigger.autoEnterOnOverlap=true. Save each scene.
- Move the OLD procedural placeholders (portal_arch_cyan.png, reward_relic_cyan.png, echo_mote.png + .meta) to `Assets/Sprites/_archive~/` (do NOT delete; keep GUIDs out of the way).

# Verify
- Console clean (read_console). Confirm 3 generated PNGs exist + are transparent (not magenta) + import settings/pivots. Confirm per-scene wiring + saved. Do NOT enter play mode (D3D12 risk). Screenshot the portal in _IsoGame edit-mode: `Assets/Screenshots/portal_gen_v1.png` (temporarily enable DoorNorth SpriteRenderer if needed).

# Notes
- USE $imagegen (image_gen), NOT procedural Texture2D drawing. NO new .cs. NO commit. Do NOT touch cliffs. profile laurethayday (orchestrator re-dispatches yekta if quota-limited). If image_gen is unavailable in your environment, STOP and report BLOCKED (do not fall back to procedural).

# Output
3 generated sprite paths + transparency/import confirm; per-scene re-wire confirm; archive confirm; portal screenshot path.
