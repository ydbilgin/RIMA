ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Knockdown package, CODE-ONLY (no new animations): heavy hits on Broken/Sundered targets knock mobs down with a readable arc+squash+get-up sequence, unified through one impulse pipeline.

# Canonical spec
READ FIRST: `STAGING/CODEANIM_DECISION_2026-06-05.md` (locked decision) + your own prior audit `CODEX_DONE_yasinderyabilgin.md` §1 (file:line inventory — reuse it, it is fresh).
Routing decision (orchestrator): `STAGING/QUEUE10_ROUTING_DECISION_2026-06-05.md` §"Çözülen anlaşmazlıklar" item 1.

# Build
1. `HitImpulse` struct/class + `KnockdownProfile` ScriptableObject (3 assets: Light/Heavy/Boss archetypes) + `KnockdownDriver` (visual child: parabola arc + ~35° tilt + Y-squash 0.6 on landing + single simple bounce + reuse `GroundBlobShadow` while airborne/down + get-up i-frame via `Health.SetImmune`).
2. `KnockbackReceiver` becomes the unified impulse entry point (`ApplyImpulse`).
3. `KnockbackComponent` (legacy boss path): COMPAT-FORWARDING ADAPTER into KnockbackReceiver — do NOT invasively rewrite PenitentSovereign call sites.
4. Trigger: heavy hit + target has Broken/Sundered (`SkillStateTracker` events, already exist).
5. Direct `Rigidbody2D.AddForce` skills (Cleave/IronCharge/WarStomp/BladeRush) may bypass the package in v1 — leave them working as-is, note them in your DONE report.

# Hard constraints (combat-critical)
- Get-up i-frame is MANDATORY — juggle-lock (re-knockdown while down) must be impossible. While down: invulnerable; on get-up: brief i-frame then normal damage + AI resume.
- Do not break the existing playable loop (_Arena run + Chamber dummy). Chamber combat dummy is your test bench.
- Code-only: no scene file edits, no new art. EditMode tests for impulse/profile math where cheap.
- SMOKE TEST before done (UnityMCP, Unity is open): enter play, heavy-hit a dummy/enemy → arc→land→squash→down(invuln)→get-up→takes damage again, AI resumes. Console clean.
- Commit when verified (English message, ydbilgin identity, NO Co-Authored-By trailer).

Write result summary to CODEX_DONE.md (what built, file list, smoke-test evidence, known gaps).
