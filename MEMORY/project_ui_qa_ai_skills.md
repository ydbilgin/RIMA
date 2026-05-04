---
topic: project_ui_qa_ai_skills
updated: 2026-05-04
---

# Project UI, QA, and AI Skills

Use when: UI concept, character menu, encounter UI, QA tester flow, Unity tests, shared
Claude/Codex skills, token savings.

Primary design input:
- `TASARIM/UI_QA_AND_AI_SKILL_RECOMMENDATIONS_2026-05-04.md`
- `TASARIM/UI_CONCEPTS/rima_ui_template_concept_2026-05-04.png`

Key points:
- `character_menu_concept.png` has useful mood and layout, but its generic RPG equipment/stat
  content should be replaced by RIMA run/build systems.
- Current code supports Hades-like active/passive draft rewards, but rich encounter room content is
  only partial.
- UI should focus on class identity, active kit, passive/echo build, route context, room objective,
  and reward state.
- UI images are templates/references. Runtime fills frames/cards/slots from real player, route,
  room, and offer data.
- Unity EditMode tests are useful for deterministic regressions. They are not enough for feel,
  visual framing, combat readability, or UI obstruction.
- A QA-tester workflow should combine tests, Play Mode/manual scenarios, screenshots, console logs,
  severity, and repro steps.
- Memory is the default place for facts and decisions.
- Skills are only useful for repeated workflows: what to open, what to ignore, what checks to run,
  and what output format to produce.
- A skill does not create permanent memory by itself. Its short description triggers loading, then
  `SKILL.md` teaches the behavior for that task.
- Good RIMA skills should include trigger, goal, allowed files, forbidden mistakes, workflow,
  output format, and verification.
- Do not create skills for one-off knowledge. Shared AI skills should be small workflow helpers,
  not large always-loaded manuals.
