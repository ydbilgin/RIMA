ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Review the just-implemented "combat_weapon" uncommitted changes for correctness bugs, RIMA convention violations, and the weapon-mount design focus below. Output a concise findings list and a final verdict.

# Review Task: combat_weapon weapon-mount changes

## Step 1 — See the changes
Run this exact command to see the uncommitted changes:

```
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" diff -- Assets/Scripts/Combat/OrientationSync.cs Assets/Scripts/Systems/Combat/HandAnchorAttach.cs Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs
```

These are the three files in scope. Do NOT review anything outside these files. If you need surrounding context, you may read the full files (they are small), but findings must concern only the diffed changes.

## Step 2 — Review

Review for:
1. **Correctness bugs** — logic errors, null derefs, off-by-one, wrong axis, state that never resets, etc.
2. **RIMA convention violations** — namespaces, no speculative code / no "just in case" error handling, minimum code, surgical scope (only these files touched), 8-dir convention (5 sprites + 3 mirror via flip), PPU 64, single-sprite-per-weapon (no per-direction bake).

### Design focus (weapon-mount correctness) — verify each explicitly:
- (a) **flipY (NOT flipX) for W/NW/SW** — the weapon mount must use flipY for the west-facing directions (W, NW, SW), not flipX. Confirm the correct axis is flipped.
- (b) **Procedural swing COMPOSES with per-dir base rotation** — the attack swing animation must add on top of the per-direction base rotation set by OrientationSync, not overwrite it. No double-apply, no fighting/race between the swing driver and OrientationSync writing the same transform field.
- (c) **Swing returns cleanly to base** — after the swing completes, rotation/position returns exactly to the per-dir base with no accumulated drift across repeated attacks.
- (d) **Timing syncs with attackStartup strike frame** — the swing's peak/strike moment must align with the attack's startup/strike frame timing (attackStartup), not be arbitrary.
- (e) **Sort order unaffected** — the weapon sprite's sorting order / sorting layer must not be disturbed by the mount or swing logic.
- (f) **Single-sprite-per-weapon assumption preserved** — no per-direction sprite bake; one sprite rotated/flipped procedurally for all 8 directions.
- (g) **Null-safety on weaponRenderer / weaponTransform** — guarded access; but flag SPECULATIVE null handling that violates the min-code rule (guard only what can realistically be null at runtime).
- (h) **RIMA conventions** — namespaces correct, no speculative code.

## Step 3 — Output format

Output a concise findings list. For each finding use one line:
`[SEVERITY] file:line — issue — fix`
where SEVERITY is one of CRITICAL / HIGH / MEDIUM / LOW / NIT.

Then a single final line, exactly one of:
`STATUS: PASS`  (no correctness bugs and design focus a–h all satisfied)
`STATUS: CONCERNS`  (any bug or unmet design point)

Write your full result to CODEX_DONE (the dispatcher's done file). Respond inline as well, NOT only to a file.
