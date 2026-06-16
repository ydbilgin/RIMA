# B2 aim→cursor fix — ADVERSARIAL COUNCIL (writer=builder-opus; you are a DIFFERENT reviewer)

ACTIVE RULES: think · min · surgical · BLOCKED if unclear. HOSTILE reviewer — do NOT auto-approve; find a flaw. Report ≤10 lines: VERDICT (PASS/FAIL/RISK) + concrete findings. Code review only, NO Unity (already compiles 0-error).

## Context (RIMA demo; 8-dir LOCKED — reject any 4-cardinal)
User bug: basic attack (LMB) aimed at movement-facing, not the mouse cursor; also "hits too far". 
builder-opus root cause: cursor-aim IS fully implemented (`PlayerController.FaceCombatTarget()` → `GetMouseDirectionOrFallback()` cursor-dir, every basic attack + skills call it). The defect = a persisted PlayerPref `AttackAimMode=0` (CharacterFacing) routed it to the movement branch; the one-shot migration key was already consumed so it couldn't re-correct. "Too far" = SAME root (wrong direction → off-target hitCenter), not a range bug (hitRange 1.2-1.5 untouched).

## Fix applied (PlayerController.cs, +4/-1)
Bumped `AttackAimModeCursorDefaultMigrationKey` `"..._20260503"` → `"..._20260616"` so `EnsureCombatAimDefault()` re-asserts the documented TowardsMouse default ONCE per machine. Live demo pref also set to 1 (TowardsMouse). builder-opus data-proved idempotency (a deliberate post-migration CharacterFacing toggle is NOT clobbered).

## Attack it (verify in real code)
1. Does `EnsureCombatAimDefault()` actually RUN at startup (Awake/Start/static init)? If it doesn't run, the migration bump does nothing. Confirm the call site + that it executes before first attack.
2. Idempotency: after the one-shot migration sets TowardsMouse, if the user toggles to CharacterFacing in Settings, does the migration leave it alone on subsequent runs (key already set)? Confirm no clobber loop.
3. Any OTHER reader/writer of `AttackAimMode` pref that could conflict or re-route to CharacterFacing?
4. `GetMouseDirectionOrFallback`: when mouse present, does it correctly give (mouseWorld - player) dir for the attack? Edge: mouse over UI / no camera → sane fallback?
5. Scope: is +4/-1 truly enough, or is there another path (e.g. a skill that hardcodes FacingDirection ignoring combat override) still aiming wrong?

Report VERDICT + findings.
