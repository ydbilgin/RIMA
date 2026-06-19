# Codex FINAL-GATE Review — before commit (READ-ONLY)

ACTIVE RULES: (1) think (2) flag only real issues (3) scope = the files below (4) BLOCKED if unclear.

**READ-ONLY / NO GIT:** Do NOT modify any file. Do NOT `git add` / `git commit` / `git stage` ANYTHING. Review + verdict only — the orchestrator commits if you APPROVE.

You are the independent FINAL approval gate before a local commit. Verify three things: (A) the academic report is sound + demo-ready, (B) the playtest/demo-readiness CLAIMS hold up against the actual code, (C) the session's code changes are sound. You MAY use UnityMCP for a spot-check ONLY if needed (you are the sole Unity user; no other agent is running) — but static review is sufficient for most of this.

## A) REPORT
`STAGING/report/RIMA_Senior_Design_Report.md` — verify the dual-council QC fixes actually landed and are correct:
- §11.9 (~line 706): must now say **HitPauseDriver is the single timeScale write owner** and **HitStop is [Obsolete] / removed from MarkPulseBehavior** (the PREVIOUS text was inverted). Cross-check against the actual code (`HitPauseDriver.cs`, `HitStop.cs`, `MarkPulseBehavior.cs`) — is the corrected report text TRUE?
- §8: concrete AI tool names (Codex/Opus/ax/Claude Code) abstracted to generic roles + a footnote ¹.
- Özet/Abstract/Anahtar Kelimeler present before §1.
- §4.4 cross-ref "(§11.4)", boss named in §4.1, §11.11 in the Sorun/Teşhis/Çözüm/Çıkarım template.
- EK A: the 7 new figures (Şekil 13-19) — captions sane, "text-before-figure" convention.
Verdict: is the report internally consistent + demo-ready, or are there remaining blockers?

## B) PLAYTEST / DEMO-READINESS — cross-check CLAIMS vs CODE
Read `STAGING/DEMO_SUNUM_HAZIRLIK_2026-06-19.md` (10 GREEN / 2 YELLOW matrix) + evidence logs in `STAGING/_process/2026-06/` (`_demo_readiness_play_*`, `_demo_readiness_editor_*`, `_done_ses_f2_verify_*`, `_done_elementalist_blaze_vfx_*`, `_done_status_tint_*`, `_done_hitpause_fix_*`, `_root_cause_card0_freeze_*`, `_review_cx_session_vfx_*`). For each GREEN claim, confirm the CODE actually supports it. Specifically scrutinize:
- **SES wiring:** do `Assets/Prefabs/Enemies/FractureImp.prefab`, `Penitent.prefab`, `HalfThrall.prefab` now contain a `StatusEffectSystem` component? Does the Fire/Frost LMB → Burning/Chill + tint path actually work (CastRhythmBehavior SetOnHit → status.ApplyEffect; StatusEffectTint reads ActiveEffects)?
- **LMB VFX:** SkillVfx.ProjectileBlaze trail widthMultiplier-after-widthCurve fix; ImpactExplosion; orb scale 0.28/0.32 in CastRhythmBehavior; cooldown 0.42 in BasicAttackProfile_Elementalist.
- **T9/T7 fix soundness** (RunStats/RoomRunDirector/CheckpointManager/DraftManager/BuildTileBrushController/HitPauseDriver) — already auditor+cx reviewed; confirm nothing regressed.
- **Brush shortcut:** BrushHotkeyHandler.cs Erase is now `KeyCode.E, ShortcutModifiers.Shift` (no Unity Rotate conflict).
- Flag any GREEN claim NOT backed by code, and confirm the 2 YELLOWs (SES-on-enemy now fixed→should be GREEN; Boss code-confirmed-not-live) are honestly stated.

## C) CODE SOUNDNESS
The session's changed code files (see git working tree). Any real bug/regression beyond the already-fixed null-guard (CastRhythmBehavior:103)? The new `StatusEffectTint.cs` + the StatusEffectSystem auto-attach.

## VERDICT
Write findings to `STAGING/_process/2026-06/_review_cx_finalgate_2026-06-19.md`. Return a <=14-line summary ending with a clear line: **`VERDICT: APPROVE-ALL`** (safe to commit) OR **`VERDICT: ISSUES`** with each blocker as `file:line — severity — what`. If ISSUES, the orchestrator fixes before committing.
