# REVIEW TASK: Gate-slot implementation (commit f63ac34c) — STATIC REVIEW ONLY

ACTIVE RULES: (1) evidence with file:line (2) READ-ONLY — do NOT modify files, do NOT enter play mode, do NOT run tests (cite the commit's own test evidence) (3) BLOCKED if unclear.

Review commit `f63ac34c` ("feat(rooms): authored NW/N/NE exit slots with deterministic door selection", 28 files) in CWD repo against spec `STAGING/GATESLOT_DECISION_2026-06-07.md`. Another agent is actively editing designer files — touch nothing, static analysis of the COMMIT DIFF only (`git show f63ac34c`).

Focus (verdict PASS / PASS-WITH-NOTES / FAIL + findings table severity/file:line/fix):
1. Selection logic exactness: 1→N, 2→NW+NE (center EMPTY), 3→all; returned-door order = graph child index (RoomRunDirector trigger contract). Off-by-one / reorder risks?
2. Fallback correctness: insufficient slots → legacy north-row; can a half-state occur (some doors on slots, some on row)? Warning dedup?
3. RoomBankSO/RoomRunDirector pool filter: ValidExitSlotCount >= childCount — does it interact safely with depth-based selection (could it empty the pool at some depth → null template path)?
4. Validator new rules (south corridor, separation, reachability): false-positive risk on crescent/zigzag; performance of reachability check (flood-fill per audit run OK, but is it called per-frame anywhere by mistake?).
5. Fix Sockets migration: legacy door_W/E removal — anything still reading those IDs (grep)? Chamber skip intact?
6. UnifiedMapDesigner preview changes: any GUI-layout breakage risk (called inside existing draw loop)?
7. Tests added: do they actually assert center-empty for 2-door case and child-index order?

Write full review to `STAGING/_review_gateslots_axopus.md` (create the file). End with verdict.
