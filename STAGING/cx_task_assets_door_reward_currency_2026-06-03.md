ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files/scenes only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
Create on-brand PLACEHOLDER sprite assets for the room-exit PORTAL/door, the REWARD pickup, and the ECHO currency, at the right sizes, and wire the portal + reward sprites into the game. (Real PixelLab pixel-art is a later together-session; these are code-generated placeholders that look decent and are correctly sized/positioned so the game looks intentional for the playable build.) Unity `execute_code` to generate PNGs + assign; NO new .cs scripts (avoid recompile). Reference art direction: concept images in `STAGING/imagegen/concept_portal_single_*.png` (granite stone ARCH with a glowing cyan rift inside), `concept_reward_*.png` (cyan runic relic + map-shard), `concept_reward_pickup_currency_*.png` (small cyan Echo motes).

# RIMA palette (use these)
- Stone/granite: dark slate greys #2E333B / #3C434D / #59626E (with subtle noise/edge highlight).
- Iron trim: #1B1E24 dark + #717C8A light edge.
- Cyan seal-energy (the ONLY saturated color): core #BFFFF2 -> #00FFCC -> glow #00A88C, transparent falloff.
- Backgrounds transparent (alphaIsTransparency).

# Assets to create (Texture2D + EncodeToPNG, import Sprite/Single, PPU64, Point filter, alphaIsTransparency=true)
1. **PORTAL / door** -> `Assets/Sprites/Environment/Portal/portal_arch_cyan.png`, **128x192 px (= 2x3 world units... NO)**. CORRECTION: make it **64x128 px** (= 1.0 x 2.0 world units), **pivot BOTTOM-CENTER (0.5, 0)**. Draw a granite stone ARCH (two vertical pillars + a top lintel, stone-grey with darker mortar lines + a light top edge) framing a tall VERTICAL cyan rift in the middle: bright near-white core down the center, fading to #00FFCC then #00A88C, with a few thin crackle branches; transparent outside the arch. This is the concept_portal_single look, simplified. It must read as "stone doorway with a cyan rift inside", standing upright.
2. **REWARD relic** -> `Assets/Sprites/Reward/reward_relic_cyan.png`, **48x48 px**, pivot center. An iron-framed diamond/gem charm (dark slate gem in an iron filigree frame) with a soft cyan inner glow — like the relic in concept_reward_pickup. Transparent bg.
3. **ECHO currency mote** -> `Assets/Sprites/Reward/echo_mote.png`, **16x16 px**, pivot center. A small bright cyan spark/diamond mote with soft glow falloff. Transparent bg.

# Wiring
- **Portal (3 scenes: _IsoGame, _IsoGame_Map02, _IsoGame_Map03):** on DoorNorth, replace the current `portal_vertical_placeholder` reference on the GateBehavior with the new `portal_arch_cyan` sprite (assign to GateBehavior.spriteUnlockedBase AND spriteRoomCombat via SerializedObject). Keep DoorTrigger.autoEnterOnOverlap=true, Entities sorting, collider as-is. Save each scene.
- **Reward (3 scenes):** find `RoomClearVictoryTrigger` and set its `rewardSprite` field to the new `reward_relic_cyan` sprite (SerializedObject). Save.
- **Echo mote:** just create the sprite asset (no drop logic — that is the reward-MVP step, separate). Leave it imported and ready.

# Verify
- Compile/console clean (read_console). 
- Confirm the 3 PNGs exist + import settings (Sprite, PPU64, pivots as specified).
- Confirm per scene: DoorNorth GateBehavior sprite refs = portal_arch_cyan; RoomClearVictoryTrigger.rewardSprite = reward_relic_cyan; saved.
- Screenshot the portal arch in the scene to `Assets/Screenshots/portal_arch_v1.png` (edit-mode is fine; you can temporarily enable the DoorNorth SpriteRenderer to see it). Do NOT enter play mode (D3D12 crash risk).

# Notes
- NO new .cs. NO commit. Do NOT touch cliffs. If a procedural draw looks too crude, still deliver it (placeholder) and note it. profile: laurethayday (if quota-limited, the orchestrator will re-dispatch with yekta).

# Output
3 sprite paths + import confirm; per-scene wiring confirm; console status; portal screenshot path.
