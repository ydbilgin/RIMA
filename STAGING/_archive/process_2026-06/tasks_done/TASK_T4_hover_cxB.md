ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Tier-1 hover: wire the existing-but-orphaned `TooltipSystem` into skill draft cards + equipped-synergy pulse. (SODAMAN backlog item 1.)

# Spec
READ FIRST: `STAGING/SODAMAN_LEARNINGS_DECISION_2026-06-04.md` §6 item 1 + the inventory in `CODEX_DONE_yasinderyabilgin.md` §4 (fresh file:line evidence).
- `SkillOfferUI.CardJuiceHandler.OnPointerEnter/Exit` → `TooltipSystem.Instance.Show/Hide` with the card's skill data (name/tier/desc/CD — TooltipSystem.BuildTooltip already does this).
- Styling: NO opaque box — translucent ink-wash + cyan hairline (UI canon: "UI yoktur sadece bilgi vardır").
- Synergy pulse: on hover, if hovered skill chains with an OWNED equipped skill (`ChainWindowTracker` relation + `DraftManager.OwnedActiveSkillNames`), pulse the matching `SkillBarUI` slot (add a narrow public pulse method if none exists).
- Hover-SFX: SKIP in v1 (no clip available).
- Scope guard: do NOT build the Tier-2 tether line; draft-card scope only.

# Constraints
- Code-only, no scene edits. Don't touch `SkillOfferGenerator` weighting (separate deferred task).
- Verify (UnityMCP, Unity open): play mode → trigger a draft → hover cards → tooltip shows correct content, no jitter, Seç click still works, equipped slot pulses on synergy match. Console clean.
- Commit when verified (English, ydbilgin identity, no Claude trailer).

Write result to CODEX_DONE.md.
