# REVIEW TASK: UI-JSON editor (commit c97ddfa1) — STATIC, READ-ONLY

Rules: evidence file:line · do NOT modify files · do NOT enter play mode or run tests (cite the commit's own evidence) · another agent is editing runtime files — review the COMMIT DIFF only (`git show c97ddfa1`).

Spec: `STAGING/R4_DECISION_2026-06-07.md` §1 + `STAGING/TASK_uijson_editor_2026-06-07.md`. Repo CWD = RIMA root.

Focus (verdict PASS / PASS-WITH-NOTES / FAIL, findings table severity/file:line/fix):
1. Y-flip correctness BOTH directions (exporter `jsonY=(h-1)-gridY`, importer inverse) — spawn/slots/props/walkable consistent? Any single-direction flip bug a round-trip test might mask (e.g., both sides flipped twice = identity)?
2. Diff-check write: BOM handling, line-endings (CRLF vs LF) — could it rewrite identical payloads every save (git noise) or never rewrite changed ones?
3. Debounce loop: EditorApplication.update handler — removed on window close? Leak/multiple-subscribe risk when window reopened?
4. Undo correctness: every mutation path (paint drag, set entry, set slots) records BEFORE mutation? Drag-paint = one Undo group or per-cell spam?
5. Importer props-preserve fix: v1 JSON without props → props untouched; v2 WITH props → replaced. Edge: v2 with EMPTY props array — wipe or preserve (which is spec-correct?).
6. Set-slot auto-enforcement (direction=North/isExit/socketId): can it create DUPLICATE sockets if one already exists at another cell? Does it move-or-create correctly?
7. Schematic mouse→cell inverse mapping: zoom/offset/scroll handled? Off-by-one at cell borders?

Write full review to `STAGING/_review_uijson_axflash.md`. End with verdict.
