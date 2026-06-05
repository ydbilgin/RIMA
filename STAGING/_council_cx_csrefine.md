ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v2 layout refinement (framed boxes + character spread + skills placement) FEASIBILITY/REUSE lensi. ANALİZ ONLY. Sonucu profil-DONE'a yaz.

# READ
STAGING/CHARSELECT_REFINE_BRIEF_2026-06-04.md + Assets/Scripts/UI/CharacterSelectScreen.cs (current BuildRosterRoom, BuildRoomCharacter, BuildSkillDetailPanel, placement table).

# Answer (concrete, cite methods/lines)
1. Skills placement feasibility: bottom-bar right zone vs vertical right side-panel — which is least-code given current BuildSkillDetailPanel + RoomLayer? Does a vertical right panel overlap RoomLayer character anchors (X up to .80/.92)? Confirm the layering/raycast implications.
2. Framed boxes: how to wrap current bottom HUD zones in 9-slice frames using existing Pack consts (panel_frame_9slice/card_frame_9slice/button_9slice) — Image.type=Sliced, which method to edit, minimal change.
3. Character spread: where is the placement table in code? Easiest way to widen it (just change normalized coords + maybe per-row scale)? Any overlap/raycast concern when spreading (front-over-back sibling order already handled?).
4. Least-code change plan for all 3 (which methods to touch, keep procedural, keep all data wiring intact).
Terse, cite paths/methods.
