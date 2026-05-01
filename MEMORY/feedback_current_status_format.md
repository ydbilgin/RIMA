---
name: CLAUDE.md and CURRENT_STATUS.md leanness rule
description: Both CLAUDE.md and CURRENT_STATUS.md must stay minimal — they load every message. No redundant content.
type: feedback
---

CLAUDE.md and CURRENT_STATUS.md must stay as lean as possible. Both load automatically every message; bloat compounds across the session.

**Why:** At session start, these two files together with gitStatus and system prompt fill 13%+ of context before any real work begins.

**How to apply:**

### CLAUDE.md
- Target: under ~55 lines.
- NO: role descriptions, model routing tables, token saving bullet lists — all live in AGENTS.md.
- NO: asset pipeline workflow steps — live in PRODUCTION_GUIDE_S43.md.
- YES: pointers to the canonical doc (one line max per topic).
- Rule: if the content already exists in another doc, replace with a pointer. Never duplicate.

### CURRENT_STATUS.md
- Target: under ~60 lines.
- NO: locked decision summaries (live in MASTER_KARAR_BELGESI.md / canonical skill doc).
- NO: identity anchor tables (live in SKILL_AUDIT_DECISION_YYYY-MM-DD.md).
- NO: "Pending User Decisions: (none)" — omit when empty.
- YES: active block (1-2 lines), next 3 priorities, critical numbers, key refs.
- When a section grows, move detail to the canonical doc and leave a single-line pointer here.

## Date Rule (all .md files)
Only update a date field when the content of that section actually changes.
Do NOT update dates as a routine "sync" step.
**Why:** Date-only edits add noise in git history and cost tokens for no value.
