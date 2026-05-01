# CURRENT STATUS
**2026-05-01 · S43 · Phase 1**

## Active Block
Skill audit v2 LOCKED + canonical rewrite committed (2026-04-30 evening). Production gate HOLD pending Visual Contract.

**2026-05-01 session:**
- S43 dirty-pass renames applied: 15 cosmetic skill names for Brawler/Ravager/Hexer/Gunslinger
- Visual Contract template written + Codex review (10 fixes) + 4 design lead decisions locked
- UNITY_STATE_OVERLAY_SPEC.md written
- 40 skill contracts written: Brawler (13), Ravager (12), Ranger (10), Shadowblade (12)
  - Path: `STAGING/PROMPTS_S43/CONTRACTS/<CLASS>/`
  - Scarbinding + Scar Collapse batch dependency enforced (Q1 decision)
  - All Section G checkboxes pending design lead sign-off

**2026-04-30 session (audit):**
- 3-round skill audit completed (Claude generic + Codex generic + Codex identity-fit)
- 192 skill rows scored across 10 classes; both audits independently flagged same priority classes
- Codex caught MASTER #56 violations Claude missed (4 HP-execute clauses now removed)
- Codex CT-DOC-CANONICAL applied all v2 changes to `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` (commit 22ed58c)

**v2 audit numbers (LOCKED):**
- 22 REDESIGN (5 mechanical #56/#58 + 1 state-ownership #55 + 16 visual contract)
- 14 MERGE (Off-Balance reverted to KEEP)
- 1 PROMOTE (Wireline Trap R4 -> Ranger #11)
- 2 CUT (Hawk Eye R4 + Ronin Phantom Step #7)
- 38 TIGHTEN (Unity engine state work)
- ~115 KEEP

**Identity Anchors LOCKED** for all 10 classes (OWNS / AVOIDS table). Cross-class differentiation rules in `TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md`.

## Production Gate — Status (HOLD)

| # | Gate item | Status |
|---|---|---|
| 1 | Canonical doc rewritten per v2 audit | DONE (22ed58c) |
| 2 | Visual Contract template written | DONE (2026-05-01) |
| 3 | Top-4 class contracts filled (Brawler, Shadowblade, Ravager, Ranger) | DONE (2026-05-01) -- 40 contracts total; design lead sign-off pending |
| 4 | Unity state overlay spec | DONE (2026-05-01, TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| 5 | Brawler char_id idle/walk/dash anchor | In progress (S43 active) |

After gate clears -> V3 keyframe REST workflow, peak frames only, 15 gen/dir.

## Next Priorities
1. **Design lead sign-off** on all 40 contracts in `STAGING/PROMPTS_S43/CONTRACTS/` (gate #3 unblocks gen)
2. **Brawler anchor** sprite -> V3 keyframe workflow
3. **Gate #5 clears** -> PixelLab batch dispatch (V3 REST, peak frames only, 15 gen/dir)

## Critical Numbers
- PixelLab gen budget: **2586/5000 used**, ~2414 left, deadline 2026-05-18
- Tests: **24/24 PASS** (PlayMode, 2026-04-30)

## Locked Design
See `MASTER_KARAR_BELGESI.md` (rules #54-#58) and `TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md` (identity anchors, LOCKED 2026-04-30).

## Refs
- Skills: `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` (v2 canonical, commit 22ed58c)
- Audit decision: `TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md` (LOCKED)
- Architecture: `SYSTEM_MAP.md`
- Decisions: `MASTER_KARAR_BELGESI.md`
- Scene: `Assets/Scenes/_IsoGame.unity`
- MCP: CoplayDev v9.6.8
- Anchors: `Characters/anchors/<class>_anchor.png`
