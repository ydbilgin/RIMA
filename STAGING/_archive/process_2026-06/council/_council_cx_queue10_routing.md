ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
10-task autonomous work queue: per-task feasibility/reuse inventory (file:line evidence) + implementation approach + agent-routing recommendation, so the orchestrator can dispatch each task to the right agent with cross-review. ANALYSIS ONLY — no code changes.

# Context
Autonomous session. User is away. Existing decision docs govern HOW (read them):
- STAGING/CODEANIM_DECISION_2026-06-05.md (knockdown = code-only)
- STAGING/PLAYABLE_ROADMAP_DECISION_2026-06-05.md (session A items)
- STAGING/SODAMAN_LEARNINGS_DECISION_2026-06-04.md (hover/codex backlog)
- STAGING/MODULAR_PROPS_DECISION_2026-06-05.md (checker + auto-placer)
- STAGING/PIXELART_SCALING_REPORT_2026-06-04.md (icon import fix)

# The 10 tasks
1. [M] Knockdown package, code-only: parabola + ~35° tilt + Y-squash 0.6 + bounce + ground shadow (GroundBlobShadow exists) + get-up i-frame; merge the 2 knockback impls + HitImpulse + 3 archetype KnockdownProfile SOs. Trigger: heavy hit + Broken/Sundered state (SkillStateTracker event).
2. [S] Mob death decal/ghost: code squash/fade + ground decal on mob death.
3. [S] Anchor-tuning editor tool (weapon hand-anchor adjustment; prerequisite for weapon production session).
4. [S] Tier-1 hover: wire existing TooltipSystem.cs:84-218 (currently 0 callers) into draft cards via SkillOfferUI/CardJuiceHandler + equipped-synergy pulse (ChainWindowTracker + DraftManager.OwnedActiveSkillNames). No opaque box — ink-wash + cyan hairline.
5. [M] ESC SkillCodexUI MVP: fullscreen, blur+desat bg, UIManager pause-layer (do NOT write timeScale itself), reuse CharacterSelectScreen skill-row + SkillDatabase.GetAll(). MVP = registered classes only.
6. [S] Register remaining 6 classes into SkillDatabase (currently only Warblade/Elementalist/Shadowblade/Ranger).
7. [S] Skill-icon import fix: Assets/Sprites/UI/Icons/*.png Bilinear+PPU100+compressed → Point/PPU64/uncompressed (+ gameplay PPC upscaleRT=0→true?).
8. [M] Checker ground (x+y)%2 + run existing BridsonPoissonAutoPlacer+CompositionRoleMap over 15 prop-poor generated rooms.
9. Three small mechanics: #14 Dynamic-Wave (EncounterController.nextWaveKillFraction=0.5 exists), #26 Card-Weight, #17 Echo-Mote-Heal.
10. Clutter cleanup: remove _FazMVP_Demo_s99 from build settings (referansız=safe), dead code sweep. PlayableArena_Test01 is REFERENCED in active code — DO NOT TOUCH.

# Sub-questions (answer each)
A. Per task: what ALREADY exists (file:line evidence)? Reuse-vs-build verdict. Estimated true size (S/M/L).
B. Per task: concrete implementation insertion points (files to touch) consistent with the decision docs above.
C. Routing: which agent per task — cx (Codex gpt-5.5, code channel) / ax-Opus-4.6 (NEW channel: Claude Opus 4.6 via Antigravity, code-capable + UnityMCP-connected) / ax-Gemini-3.5-Flash (fast, S-isolated) / Sonnet-MCP subagent (mechanical Unity ops)? And who cross-reviews (writer ≠ reviewer)?
D. Order + parallelism: which tasks conflict (same files) and must serialize; which can run in parallel (2 cx profiles parallel OK; ax = one at a time)?
E. Risks/blockers per task (e.g. SkillDatabase 4-class gap order dependency 6→5, scene-open modal risk for icon reimport, knockdown vs death-decal both touch mob death path?).

Write your full answer to CODEX_DONE.md. Do NOT reproduce any prior audit verbatim — fresh file:line checks where cheap, otherwise cite the decision doc.
