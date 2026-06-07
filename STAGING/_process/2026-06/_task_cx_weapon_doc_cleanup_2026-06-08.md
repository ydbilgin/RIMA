ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself. DO NOT use Unity MCP (another agent owns Unity).

# Amaç
Execute the doc cleanup from section 6 of YOUR audit `STAGING/WEAPON_PIPELINE_AUDIT_2026-06-08.md` (you wrote it; re-read §6 + §2 finding 15). DOC EDITS ONLY — no code, no Unity.

## RULES
- SURGICAL edits only (add banners / correct specific drift lines / prepend status headers). Do NOT rewrite whole files.
- PRESERVE existing Turkish characters in the files you edit (do NOT ASCII-mangle existing TR text). New banner text may be Turkish or English but must not corrupt existing content.
- After edits, the canon-violation TERMS must be neutralized (see verify step).

## EDITS
1. **`STAGING/WEAPON_BATCH_PLAN.md`** — add a `> ⚠️ SUPERSEDED / CANON-CORRECTION` banner at the very top: canonical sources = `STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md` (canon) + `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md` (immediate session). Then correct the 4 canon-drift lines INLINE (audit §2 finding 15): Elementalist staff/orb → floating golden rune disc (staff/wand FORBIDDEN); Gunslinger flintlock/western → rift-tech pistol (western FORBIDDEN); Hexer curse-staff/whip → grimoire/totem/scepter (whip FORBIDDEN); Brawler gauntlet/fist-as-weapon → NO weapon (body cosmetic only).

2. **`STAGING/ANIMATION_PROMPT_CATALOG.md`** — prepend a status header right under the title: `DEMO ACTIVE: none — new char/mob animation = 0 per CODEANIM_DECISION_2026-06-05. POST-DEMO: SPLIT body animation reference only. DO NOT GENERATE BEFORE TIMING FREEZE.`

3. **`STAGING/WEAPON_ANIM_VFX_PRODUCTION_LOCK.md`** — add a correction note where it claims "OrientationSync dead" / 4-diagonal bridge: mark STALE — OrientationSync IS live-wired in the canonical `Assets/Prefabs/Player.prefab` and called by `HandAnchorAttach` (ref the audit). Keep the rest (PPU64/order lock stays canonical).

4. **`STAGING/A1_WEAPONDB_CLARIFY.md`** — update its dead-code status line(s): OrientationSync is NO LONGER dead in the canonical runtime player. Keep its canonical DB/prefab inventory sections.

5. **`STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md`** — it already has a SUPERSEDED note for PPU/order; ensure the top banner clearly states it is reference-only for prompt/mount history (PPU/order superseded by the LOCK doc). Minimal touch.

6. **`STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md`** — resolve the open "ask user 64px vs roster" ambiguity: change it to RESOLVED per the audit — COMBAT_ROSTER S43 wins. Demo 4 archetypes + sizes: Shard Walker 112 / Void Thrall 128 / Chain Warden 128 / Relic Caster 80; Penitent Bruiser + Fracture Imp = post-demo. Keep a one-line "user may override at session start" note. Do NOT delete the prompt blocks.

## VERIFY
- Grep the edited files: the forbidden terms as WEAPON for the wrong class must be gone or explicitly marked forbidden — confirm no remaining line presents "Elementalist staff/orb", "Gunslinger flintlock/western", "Hexer whip", or "Brawler gauntlet" as the intended weapon.
- Confirm no existing Turkish text was ASCII-corrupted (spot-check).
- Report each file's edit summary.

## COMMIT
`docs(weapon): canon-drift cleanup + stale-status fixes per audit (2026-06-08)`
List per-file edits + commit hash in CODEX_DONE.md.
