---
topic: project_ui_state_blueprint
updated: 2026-05-04
---

# Project UI State Blueprint

Use when: HUD, skill bar, reward UI, gate choice, partial map, character/build overlay, UI concept.

Primary doc:
- `TASARIM/RIMA_UI_STATE_BLUEPRINT_2026-05-04.md`

References:
- `TASARIM/UI_CONCEPTS/rima_ui_template_concept_2026-05-04.png`
- `TASARIM/UI_CONCEPTS/rima_gate_socket_concept_2026-05-04.png`
- `TASARIM/UI_PRODUCTION_RULES_FROM_OPUS_REVIEW_2026-05-04.md`
- `TASARIM/RIMA_UI_HUD_PRODUCTION_COMPOSITE_2026-05-04.md`
- `TASARIM/UI_CONCEPTS/HUD/rima_hud_composite_mockup_2026-05-04.svg`

Rules:
- Combat HUD is quiet; player/enemy readability beats skill display.
- Skill details live in reward/build overlays, not always-on combat UI.
- UI images are templates; runtime fills frames/cards/slots from real data.
- Use reusable 9-slice frames, runtime text/icons, high-contrast icons, and micro-animations.
- Do not show fake run resources like Rift Tension/Soul/Reroll unless runtime systems exist.
- Gate choices are in-world thresholds with room-type promises, not fixed cardinal placeholder doors.
- Map shows visited/current/near revealed nodes only; no full map by default.
- Current production draft combines combat HUD, reward draft, gate choice, partial route strip,
  and build overlay responsibilities without turning combat HUD into a skill catalogue.
