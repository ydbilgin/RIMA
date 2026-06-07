# REVIEW TASK: T2 combat juice + SFX + execute prompt + dash buffer (commit 549e185b) — STATIC, READ-ONLY

Rules: evidence file:line · do NOT modify files · do NOT play/run tests (cite commit's own evidence) · review COMMIT DIFF only (`git show 549e185b`). Repo CWD = RIMA root.
Spec: `STAGING/TASK_T2_juice_sfx_2026-06-07.md` + MASTER_PLAN T2 + R5 M1 bullet.

Focus (verdict PASS/PASS-WITH-NOTES/FAIL + findings severity/file:line/fix):
1. ExecutePromptDriver: nearest-target scan cost (per-frame FindObjects? cached list?), prompt cleanup on enemy death/scene change (leak?), "warm gold not cyan" — check the actual color value respects palette (no cyan).
2. DeathBlow wiring: OnExecuteFired AFTER damage — double-fire risk on multi-hit? Order vs existing kill-pause (HitPauseDriver kill tier) — stacking freezes?
3. SFX wiring: Health.TakeDamage playing HitImpact for EVERY damage tick (DoT spam risk?); ChamberAmbient — looped where, stopped on scene change? Draft hover SFX rate-limit?
4. InputBufferService window 0.18→0.08: was 0.18 used by anything else (attack buffering?) — did shrinking it regress another input feel?
5. New enum values appended (not inserted) — serialized Sfx references stable?
6. Juice values vs FeelToggleSettings: still globally disableable?
Write full review to `STAGING/_review_T2_axflash.md`. End with verdict.
