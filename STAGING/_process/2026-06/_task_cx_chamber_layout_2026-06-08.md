ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself.

# Amaç
Redesign the Attunement Chamber (character-select) layout + selection feel per the user's live-playtest direction. INVESTIGATE what already exists first (do NOT rebuild working features — locked-black + unlock-cost prompt likely already exist). Implement the gaps. Reference sketches: `STAGING/_inbox/character_room_sketches_2026-06-08/RIMA_CHARACTER_ROOM_DESIGN_SKETCHES/01_character_select_ideal.png` (5+5 two arcs, center, back rift, selected-class strip) and `03_chamber_layout.png`.

The chamber is built at runtime by `ChamberSelectBootstrap` from `Chamber_CharSelect.asset` (RoomTemplateSO, currently a cramped 10-pedestal crescent). Pedestal/echo-station spawn is around `ChamberSelectBootstrap.cs:438-477`; sprite binding `:945-954`.

## User's clarified spec (build exactly this)
1. **ENLARGE the room + space the 10 pedestals into a 5+5 layout** (two arcs / sides toward the corners, like sketch 01). CRITICAL: the current center cluster BLOCKS player movement — there MUST be WALKABLE gaps between pedestals so the player can walk between them. No overlap, comfortable spacing. Make the central area (altar/props) walkable or move blockers out of the path.
2. **Selection stays WALK-based:** player walks (WASD) to a pedestal and attunes with [G]. Keep this. Just ensure nothing blocks pathing.
3. **Player walks; the DUMMY does NOT.** The grey creature ("fare"/KUKLA dummy) must be STATIC — freeze its movement (if it has wander/combat-AI, disable locomotion). Keep its current sprite as a placeholder (a proper dummy asset comes later). Do NOT delete it.
4. **On attune, the controlled PLAYER becomes the selected class** — the character you play turns into the selected one, at the player's normal position. VERIFY this already works (the "bürünme" flow); if it doesn't actually swap the player's class/sprite, fix it. Report what you found.
5. **Locked classes = solid BLACK silhouette** (still a recognizable character shape, not an empty disc). On approach, the interaction prompt shows the **unlock cost in currency** — the loc key `chamber_select.prompt.unlock` = "[G] Kilidi Aç — {0} SHATTERED ECHO" already exists; ensure it renders with the real cost. VERIFY both already exist (status says "kilitli=siyah"); fix only the gaps. Do NOT rebuild if working.
6. **Investigate + clean STRAY content bleeding into the chamber:** the user saw confusing elements. Identify the grey creature (is it the intended dummy, a leaked combat mob, or a FractureImp fallback?) and whether any extra door/portal objects are present that shouldn't be. Report findings; remove genuine strays (do NOT remove the intended single rift exit or the intended dummy).

## INVESTIGATE FIRST (report before/with the fix)
- Where pedestal positions come from (RoomTemplateSO cells vs bootstrap math) — this decides how you do the 5+5 enlarge.
- Whether #4 (player-becomes-class) and #5 (locked black + cost prompt) already work — quote file:line.
- What the grey creature actually is.

## VERIFY
- `refresh_unity` + `read_console` → 0 compile errors.
- Run the chamber/relevant smoke + EditMode tests; paste counts. If you can, do a play-probe of the chamber build (pedestal count/positions, walkable gaps, locked-silhouette color) and report VALUES (not "looks good").
- Capture 1-2 Game-view screenshots of the rebuilt chamber and write their absolute paths so the orchestrator can eyeball.

## COMMIT (after verified)
`feat(chamber): 5+5 spaced layout + walkable gaps + static dummy + locked-silhouette/cost verify`
Per-item status (DONE / ALREADY-WORKED / BLOCKED / FOLLOW-UP) + commit hash + screenshot paths in CODEX_DONE.md.
