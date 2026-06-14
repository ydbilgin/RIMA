# TASK (ax / Gemini) — Cleanup plan + Map Designer diagnosis (analysis & judgment, report inline)

NLM ACCESS: Query the RIMA design knowledge base to ground your KEEP/DELETE and Map Designer decisions:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Use it to confirm what is canonical (floors, perspective, Map Designer intent) before judging anything redundant.

RESPOND INLINE (not to a file you create) — the dispatcher captures your stdout. Be a decisive analyst, not a doer: you produce a PLAN the orchestrator relays to the user for approval. Do NOT edit/delete/move files. Do NOT write code.

## Amaç (Goal)
The user says RIMA is "extremely cluttered even though we have almost nothing," Map Designer "doesn't work," and there are too many floors / leftover nonsense. A Codex detection pass already produced the FACTS. Your job: turn those facts + NLM canon into a clear, conservative, decision-ready cleanup + fix plan that the orchestrator (Claude) will summarize to the user. The user approves → a later pass applies it.

## Your INPUT (read these first)
- `CODEX_DONE_laurethayday.md` — the Codex detection report (§1 re-pulled 451 tile, §2 floor inventory, §3 characters/cliffs, §4 orphan kits, §5 scene census, §6 Map Designer technical state, §7 STAGING/RIMA clutter census). This is your ground truth for what EXISTS and what is REFERENCED.
- `CURRENT_STATUS.md` — top block only (current state).
- NLM (above) for canonical design intent.

## Deliverables (4 sections, inline, tight)

### A) ASSET cleanup verdict
For floors/characters/cliffs/orphan-kits from the Codex inventory, give a per-item table: **KEEP / ARCHIVE / DELETE** + one-line reason + evidence (referenced or orphan).
- KEEP: the 451 tile (`PixelLabFloor451`, the user's chosen floor), characters, cliffs.
- Floors: the user said "too many floors" and wants ONLY 451. Recommend archiving the redundant floor sets (pl_floor, PixelLabFloorFlat/flat, iso_floor, etc.) — but FLAG any that are still wired into the live scene/registry so we don't break a working reference (note the migration needed).
- Orphan kits (IsoMockKit/KitC_BG/Phase0_ScaleTest/etc.): verdict + reason.
- CONSERVATIVE: prefer ARCHIVE over DELETE. Only recommend DELETE for true zero-reference duplicates, and say why it's safe.

### B) SCENE cleanup
From the Codex scene census: which scene is the live one, and what junk/duplicate root objects should be removed? List concrete object names to cut and what to keep. Note this needs Unity open to apply.

### C) MAP DESIGNER — why it doesn't work + fix plan
From Codex §6 + CURRENT_STATUS + NLM: state the ROOT CAUSE(s) in plain terms and a concrete, ordered fix plan (files + what to change). Known leads to confirm/expand: (a) `RoomDataJson`↔`LiveRoomReloader` schema mismatch (saved rooms don't appear at runtime/F5), (b) paint/variant/render path, (c) GUI not verifiable from MCP. Separate "what's actually broken" from "works but unverified." Mark which fixes are code (→Codex) vs config/data.

### D) FOLDER simplification plan (STAGING + RIMA root)
From Codex §7: propose a concrete target structure. Which STAGING buckets → `_archive/` (with rough file counts), what stays live. Same for the 30 root .md files. Give the user a 1-paragraph "after" picture: how many files remain visible vs archived. Do NOT propose touching `agy_snapshots/` (LIVE config) or canonical root files (CLAUDE/RULES/AGENTS/CURRENT_STATUS).

## Output discipline
- Prioritize: lead with the highest-impact, lowest-risk actions.
- Be specific (names, counts, files) — the orchestrator relays this to the user verbatim-ish, so it must read as a clear proposal.
- Flag anything where you're UNSURE or where NLM/Codex facts conflict — don't guess.
- End with a one-line `RECOMMENDATION:` (the single most important thing to do first).
