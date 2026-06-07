# RIMA — LIVE Unity-MCP Playtest, RAW-DATA Collection (ax Gemini 3.5 Flash High)

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself. DO NOT edit code/scenes/prefabs and DO NOT commit — this is INVESTIGATION ONLY (cx owns fixes). Read-only inspection + screenshots.

# ⚠️ HARD RULE — RAW DATA ONLY, NO VISUAL INTERPRETATION
You are a DATA COLLECTOR, not a critic. The orchestrator (Opus) interprets the visuals — you do NOT. This project has been burned twice by Flash fabricating visual reports, so:
- Report ONLY: factual MCP reads (GameObject hierarchy paths, component field VALUES, counts, asset NAMES, transform scales, light intensity numbers, collider isTrigger booleans) AND saved screenshot FILE PATHS.
- FORBIDDEN: any sentence with "looks/seems/appears/good/bad/nice/ugly/too big/too dark/fine" — those are interpretations. Just give the NUMBER or the screenshot path and let the orchestrator judge.
- If you cannot read a value, write "COULD NOT VERIFY: <why>". NEVER invent or assume a value.
- Save a screenshot for EVERY issue and write its absolute path. The orchestrator will eyeball them.

# Amaç
User playtested and reported bugs. Connect to the OPEN Unity instance via MCP, walk the demo flow, and COLLECT raw evidence per issue below. Every line = a measured value or a screenshot path.

# Setup
- Unity is already open. Use MCP. List instances / set active if needed.
- Determine the flow: Build Settings scene 0, and the path MainMenu → Chamber (character select) → enter rift → run room(s). Report the actual scene names + load order.
- Caveats (known): MCP `CaptureScreenshot` includes overlay UI but headless WASD input may NOT move the player. If you can't drive movement, inspect runtime state via hierarchy + component reads, and teleport the player transform ONLY for observation if necessary (note that you did). Capture Game-view screenshots at each step; save paths and reference them.

# Verify each reported issue WITH EVIDENCE
1. **Reward size** (cx may have just scaled it down): find the reward pickup at runtime, report its sprite world-bounds vs the player's, and a screenshot. Is it now reasonable (~player height) or still oversized?
2. **Camera zoom** (cx may have just zoomed out): report the live camera orthographic size / PixelPerfect ref-res, and whether the room+player are comfortably framed. Screenshot.
3. **Pedestal ring in a combat room + "3 Warblade":** In the room shown in the user's screenshots (pedestal ring + HP bars + reward), enumerate at runtime: which scene is this? Is the combat HUD (HP/stamina bars + hotbar) active in it? Is the 10-disc pedestal ring present? How many character STATUES/sprites are on pedestals, and which class sprite does each use (list per pedestal)? Confirm or refute "3 Warblade, others missing." Give the GameObject hierarchy + sprite asset names. THIS IS THE TOP PRIORITY — it contradicts a static finding that 10/10 class sprites exist on disk, so the truth is a runtime/scene-bleed bug.
4. **Door interaction model:** locate the exit door/gate GameObject(s) in the run room. Is it a walk-through trigger (Collider isTrigger → teleport/advance) or a [G]-interact? Report the component + method. Is the door easy to LOCATE when it unlocks (any beacon/arrow/glow)? Screenshot.
5. **Entry room door render (user image 3):** the first room on Play — does the door/portal render as a black silhouette or not? Report what's actually there + screenshot.
6. **"Nothing visible" dark room (user image 4):** load/observe the room that appears nearly all-dark. Report: is it an empty room template (no props/mobs), a global-light/lighting issue, or camera framing empty space? Give the global light intensity, the room template name, mob/prop counts, and a screenshot.

# Output
Write a detailed markdown report to: `STAGING/_process/2026-06/_done_axopus_live_playtest_2026-06-08.md`
Structure: per-issue section with VERDICT (confirmed / refuted / could-not-verify) + evidence (screenshot path, hierarchy path, field values). End with a prioritized fix-list (most player-facing first) and which fixes are code vs asset vs scene-config. Also state plainly which user complaints are REAL vs which are already-OK-post-cx-fix.
