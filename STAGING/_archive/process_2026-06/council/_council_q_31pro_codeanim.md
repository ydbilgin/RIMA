# Council Question — Code-only animation strategy: knockdown & the no-produced-anims limit (DEEP TECHNIQUE lens)

You are ONE advisor in a RIMA council (others: Gemini 3.5 Flash lean, Opus design+code-audit). YOUR LENS = DEEP PROCEDURAL-ANIMATION TECHNIQUE (do web research; cite specific games/techniques).

READ FIRST: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_council_brief_codeanim.md` — full context (RIMA already bets on code-anim: separate weapon-hand swung by code, SlashArcVFX, Knock+HitFlash live on 13 mobs; chars have ONLY produced idle+walk 8-dir; camera high top-down 3/4; 64px chars).

ANSWER the brief's K1-K4. Emphasis:
- K1 tier table: for EACH motion (knockback/KNOCKDOWN/stagger/death/spawn/dash/melee/cast/lunge/elite-intro) → code-only feasible? → 1-3 line recipe → a shipped pixel-art game doing it that way. KNOCKDOWN in depth: parabolic arc + sprite rotation to horizontal + bounces + DETACHED shadow ellipse + dust + get-up; how top-down 3/4 games handle the "lying sprite reads wrong" problem (rotation vs runtime-rotated single frame vs corpse-style); pixel-grid rotation concerns and mitigations (stepped rotation, RotSprite, outline shader).
- K2: what STILL deserves produced frames (boss stagger? 1-frame cast pose?) — the "when is it worth it" line.
- K3: tunable architecture on top of existing Knock/HitFlash/JuiceManager: JuiceProfile SO vs fields; knockdown mechanics (poise/threshold, ground state, get-up i-frames) at DEMO scale — call out over-engineering.
- K4: risks (64px readability, many-mobs-down chaos, "everything code" cheapness ceiling) + mitigations.

OUTPUT to STDOUT: K1 table + K2/K3/K4 bullets + TL;DR position. English OK, concrete, cite games.