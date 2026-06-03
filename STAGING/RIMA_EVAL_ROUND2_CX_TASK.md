# RIMA Skill/Mechanic Eval — ROUND 2 (canon-informed reconcile) — CX

ACTIVE RULES: (1) think before answering (2) min output, no filler (3) ANALYSIS ONLY — no code, no file edits (4) BLOCKED if you can't read needed files.

NLM ACCESS (already confirmed working in round-1): uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>". Re-query if you need a canon detail.

## READ FIRST
`STAGING/RIMA_CANON_BRIEF_FROM_NLM.md` — the FULL consolidated picture: NLM canon (§1-5), code↔canon drifts (§6), and ALL THREE agents' round-1 findings incl. your own (§7 agy, §8 cx=you, §9 Opus). The ROUND-2 instruction is at the bottom of that file.

## Your job (systems / data-model lens)
You converged with agy + Opus in round-1. Now, reacting to the consolidated picture:
1. **Challenge or confirm** the peers' load-bearing claims. Specifically validate/refute: Opus's "commit-beat carries no gameplay payload → that's THE problem"; the "9-family tag system + Rift-proc unbuilt" claim; "two skill-definition tracks" severity; whether the 6 resource systems should be UNIFIED into ClassResourceDefinition for the demo or left as-is.
2. **Resolve contradictions** with a concrete call: (a) which is the single live skill-definition track — migrate to `RIMA.SkillData` or to `ActiveSkillData`? (b) cut the 0.6s combo log AND keep commit-beat — confirm. (c) `roleTags[]` NEW field vs overloading existing `SkillTag` — which, and why.
3. **Lock the demo-scope reorg** — exact ORDERED minimal change-set (Warblade-only) to make "I built something" + the BREAK signature verb live. Each item: file(s) touched, demo-blocker vs Phase-2, rough effort. Include the `IClassSkillController` interface + `ChainWindowTracker` if you agree they're needed.
4. Give the Phase-2-ready data-model spine (SkillDefinitionSO/ClassDefinitionSO/OfferRule) but mark what is demo vs deferred.

Be decisive — this feeds the FINAL lock. ~600-900 words. CONFIRMED (read) vs INFERRED.
