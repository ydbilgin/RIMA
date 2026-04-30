# CURRENT STATUS
**2026-05-01 · S43 · Phase 1**

## Active Block
Skill audit v2 LOCKED + canonical rewrite committed (2026-04-30 evening). Production gate HOLD pending Visual Contract.

**2026-05-01 session:**
- S43 dirty-pass renames applied: 15 cosmetic skill names for Brawler/Ravager/Hexer/Gunslinger (Bully, Crackjaw, Cheap Shot, Curbstomp, Kidney Hook, Wild Hack, Gnash, Choke Throw, Foul Wave, Spitback, Bleed Tax, Hot Lead, etc.)
- 7 active docs updated (canonical + CLASS_STATE_CONTRACT, GLOBAL_REPEAT_RULES, FAZ5, ITEM_BUILD_MATRIX, MAP_ITEM_SYSTEM, audit note)
- Warblade/Elementalist/Ronin/Ranger/Shadowblade/Summoner names untouched (tone kimliği temiz)
- Memory: feedback_orchestra_discipline.md added (multi-file rename → rima-codex, not Opus inline)

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
| 2 | Visual Contract template written | TODO (Claude P1) |
| 3 | Top-4 class contracts filled (Brawler, Shadowblade, Ravager, Ranger) | TODO (Claude P1) |
| 4 | Unity state overlay spec | TODO (Claude P1) |
| 5 | Brawler char_id idle/walk/dash anchor | In progress (S43 active) |

After gate clears -> V3 keyframe REST workflow, peak frames only, 15 gen/dir.

## Next Priorities
1. **Visual Contract template** -> `TASARIM/SKILL_VISUAL_CONTRACT.md` (per-skill schema)
2. **Fill contracts** for top-4 priority classes (~64 contracts)
3. **Unity state overlay spec** (Sundered, Scar, Mark, Heat, Charge, Hex pips)
4. **Brawler anchor** -> when contracts done, V3 keyframe workflow

## Pending User Decisions
- (none — 3 open audit decisions LOCKED 2026-04-30 evening: Phantom Step CUT, Wireline Trap PROMOTE, Point Blank Execute Heat-zero gate)

## Critical Numbers
- PixelLab gen budget: **2586/5000 used**, ~2414 left, deadline 2026-05-18
- Tests: **24/24 PASS** (PlayMode, 2026-04-30)

## Locked Design (summary)
- States: 14 public (see SINIF_VE_SKILL_KARAR_BELGESI.md)
- Ulti Toggle: Shift+skill, default Lock ON, Perfect Condition = empowered cast (#54)
- Counter archetypes (#57): Warblade=absorb/break, Ronin=pre-draw timing, Brawler=whiff/evade body — no overlap
- State ownership (#55): Sundered = Warblade only; Brawler = Cracked -> Shattered
- Execute gates (#56): HP<%30 BANNED in all classes; state gates only
- Movement (#58): Space = short dash neutral; max 1 skill movement per build
- Encoding: ASCII-only in all .md files
- Sprite direction: 4 cardinal (S/E/N/W). Symmetric classes: W = E flip. Asymmetric: W separate.
- Brawler upgraded state: Shattered (Brawler Sundered = forbidden per #55)

## Skill Audit Identity Anchors (LOCKED 2026-04-30)
| Class | Owns | Avoids |
|---|---|---|
| Warblade | Sundered/Broken, absorb-counter | armor language by other classes |
| Elementalist | spell shapes, element reactions, Lightbreak | physical traps |
| Shadowblade | Scar placement/collapse, phase-through | generic teleport-slash |
| Ranger | trap lines, marks, kill zones | run-and-gun |
| Ravager | HP trade, frenzy chain | armor break |
| Ronin | sheathe timing, Opened state | generic mobility |
| Gunslinger | Heat rhythm, reload, slide+shoot | mark/trap planning |
| Brawler | Cracked/Shattered, launch, body counter | armor break, pre-draw counter |
| Summoner | minion bodies, sacrifice economy | enemy stacks |
| Hexer | enemy stacks, spread, curse phases | minion bodies |

## Refs
- Skills: `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` (v2 canonical, commit 22ed58c)
- Audit decision: `TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md` (LOCKED)
- Architecture: `SYSTEM_MAP.md`
- Decisions: `MASTER_KARAR_BELGESI.md`
- Scene: `Assets/Scenes/_IsoGame.unity`
- MCP: CoplayDev v9.6.8
- Anchors: `Characters/anchors/<class>_anchor.png`
