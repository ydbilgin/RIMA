# MASTER PLAN — S106 OVERNIGHT (Synthesized)

> **Approved by:** Opus (orchestrator, autonomous mode), 2026-05-25 03:25
> **Inputs:** Codex ideation (`CODEX_DONE_yasinderyabilgin.md`) + Antigravity ideation (`AGY_DONE_ydbilgin.md`) + Antigravity research (`STAGING/s106_overnight/ideation/agy_research_response.md`)
> **Agreement level:** ~90% — both AIs converge on order, bug list, and agent assignments. Synthesis below adopts the consensus + selectively merges unique additions.

---

## ✅ Consensus Decisions

| # | Decision | Rationale (both ideations agreed) |
|---|---|---|
| 1 | **Stream C (P0 Safety) BEFORE Stream D (Painter UX)** | Painter can't claim "world's easiest" if underlying builder has grouping fragmentation + door/open colliders that block player passage. Both AIs called this out as the most critical risk. |
| 2 | **Stream B (Asset Taxonomy) PARALLEL to Stream C** | No file overlap (Codex: code+ScriptableObject; Antigravity: PNG vision). User explicitly said placeholders sufficient → taxonomy is NOT a blocker for builder/painter. |
| 3 | **Single-Writer Rule** (Antigravity) | Codex and Antigravity NEVER edit same source file in same window. Antigravity = vision/research/JSON manifest. Codex = C# + .asset writes. |
| 4 | **Pre-authored Golden Layouts** before Stream E | 5 canonical layout JSONs (Combat/Ritual/Flooded/Library/Boss) written before any rendering — immutable test inputs. |
| 5 | **Dedicated RoomDebugGizmo component** (Antigravity addition) | Separate script attached to room root, not overload of WallChainRoomBuilder.OnDrawGizmos. Survives domain reload via [NonSerialized]. |
| 6 | **AssetDatabase batching with try/finally** | Every batch op wrapped, scene saved before destructive ops, groups of 10-20 assets to avoid reload deadlock. |
| 7 | **Validation independence** | No agent reviews its own work. Codex implements → Antigravity + rima-qc review. Antigravity classifies → Codex + Opus review. |
| 8 | **Placeholder-first proof** | Generate all 5 rooms with EXISTING 14 placeholders first. ONLY THEN do 1 real-asset visual swap (Basic Combat) to prove logic↔visual independence. NO 40-piece real-asset spawn before logic proves. |

---

## 🚫 Bugs to fix (Stream C P0 — already dispatched to Codex at 03:14)

Both ideations independently identified the same 6 line-numbered bugs. Verified file-level by Opus before dispatch:

| # | Bug | File | Lines | Status |
|---|---|---|---|---|
| 1 | Edge sort by single axis → irregular footprint fragmentation | WallChainRoomBuilder.cs | 275, 286, 296 | DISPATCHED |
| 2 | length=2 ignores startIsCorner/endIsCorner | WallChainRoomBuilder.cs | 419-428 | DISPATCHED |
| 3 | wpd_door_arch.asset colliderSize 2,1 BLOCKS player | wpd_door_arch.asset | 28 | DISPATCHED |
| 4 | wpd_open_gap.asset colliderSize 1,1 BLOCKS player | wpd_open_gap.asset | 28 | DISPATCHED |
| 5 | OnDrawGizmos only white ticks (color legend missing) | WallChainRoomBuilder.cs | 858-864 | DISPATCHED |
| 6 | Door save flat array / load nested → silent loss | RoomPainterWindow.cs | 849 + 996-1024 | DISPATCHED |

**Dispatched:** `cx_dispatch.py --task-file STREAM_C_P0_SAFETY_TASK.md --effort xhigh --timeout 2400` (background `b2y5bnbsr`).

---

## 🆕 Additions discovered during validation

These came from Opus's file-level verification + Antigravity's deeper analysis:

| # | Item | Source | Stream |
|---|---|---|---|
| A1 | NicheSpec vs alcovePositions REDUNDANCY in RoomSpec (drift risk) | Antigravity | Stream C round 2 (after P0) |
| A2 | Water/island reservation must modify walkable in ComputeFootprint (not just visual) | Both | Stream C round 2 |
| A3 | Grouped alcove semantics — 2-wide × 2-deep as SINGLE group, not 4 cuts | Antigravity ADIM 4 deep read | Stream D / Stream C round 2 |
| A4 | RoomSocket schema (enemy / prop / portal / altar / VFX / boss / wave / player-entrance) | Both | Stream C round 2 (RoomSpec additions) |
| A5 | NSEW 4-bit bitmasking algorithm for painter wall auto-derivation (Q4 research) | Antigravity research | Stream D |

Round 2 of Stream C will tackle A1-A5 ONLY IF Stream C P0 finishes early. Otherwise these defer to morning.

---

## 📋 FINAL TIMELINE (synthesized, both AIs aligned)

| Time (24h) | Stream | Implementer | Reviewer(s) | Output |
|---|---|---|---|---|
| 02:55-03:25 | A: Master+Asset lock | Opus | (self) | MASTER_CONTEXT, IDEATION_TASK, RESEARCH_TASK, MASTER_PLAN |
| 03:00-03:11 | A.5: Antigravity research | Antigravity (yasinderyabilgin) | (single-shot research) | ideation/agy_research_response.md |
| 03:05-03:13 | A.5: Codex ideation | Codex (yasinderyabilgin) | (single-shot ideation) | CODEX_DONE_yasinderyabilgin.md |
| 03:08-03:23 | A.5: Antigravity ideation | Antigravity (ydbilgin) | (single-shot ideation) | AGY_DONE_ydbilgin.md (27KB, despite UnicodeEncodeError on print — file written OK) |
| **03:14-04:30** | **C P0 Safety Pass** | **Codex (cx_dispatch xhigh)** | Opus self-check + rima-qc gate | 6 bugs fixed, compile clean, gizmo screenshot |
| **03:30-05:00** | **B Asset Taxonomy** (PARALLEL) | **Antigravity vision** | Codex (JSON→asset conv) + Opus accept | sheet_2/3/4 classified A-H, manifest JSON |
| 04:30-06:00 | D Painter UX overhaul | Codex xhigh | Antigravity (UX critique) + rima-qc (compile) + Opus (acceptance) | Validation panel, live preview, 5 presets, save/load fix, NSEW auto-wall derivation |
| 05:00-06:30 | C2 Prefab+sockets audit | Codex | rima-qc + Antigravity | 14 placeholder prefabs vs WallPiece_Root standard; sockets added; door/gap collider stripped at prefab level (belt-and-suspenders with bug 3/4 fix) |
| 06:30-07:00 | Golden Layouts JSON | Opus | (self-write 5 layouts) | STAGING/s106_overnight/stream_e_rooms/layouts/{combat,ritual,flooded,library,boss}.json |
| 07:00-08:00 | E 5 Test Rooms | Codex + UnityMCP | rima-qc (screenshots) + Antigravity (vs chatgpt_ref vision) + Opus | 5 rooms × {scene, gizmo, used assets, gap report, verdict} |
| 08:00-08:20 | B2 Real-asset swap proof | Codex | Antigravity vision | 1 room (Combat) with real Sheet 1 sprites swapped in for placeholders, logic unchanged |
| 08:20-08:45 | Morning report | Opus + rima-sonnet polish | (self) | STAGING/s106_morning/OVERNIGHT_DELIVERABLE.md |

**Buffer 08:45-09:00:** flex time for blockers, retries, NLM sync, git status review.

---

## 🔄 Multi-Agent Review Loop (per-task gate)

```
ROUND 1 — IMPLEMENT
  Implementer dispatches with task spec + MASTER_CONTEXT.md reference + ACTIVE RULES + NLM ACCESS
  Writes verifiable artifact (diff/screenshot/JSON/scene)
  Must include: "Files opened" + "Files written" lists (Antigravity addition)

ROUND 2 — PARALLEL MULTI-AI REVIEW
  Reviewer A (different AI) → PASS / FAIL / REWORK + notes
  Reviewer B (rima-qc or Opus) → independent verdict
  Reviewers receive: artifact paths + task spec + comparison criteria

ROUND 3 — RECONCILE (Opus)
  All PASS → DONE, advance
  Disagreement → Opus arbitrates, logs reasoning in SESSION_LOG
  ≥1 FAIL → back to implementer with combined feedback
  Max 3 rounds → BLOCKED, surface to user_morning_review
```

## 🔒 Hard rules (carry from MASTER_CONTEXT)
- No Unity crash (AssetDatabase batch + try/finally + scene save + group 10-20)
- No PixelLab gen
- No Karar #152/#153/#154 progress without user
- No new asset generation suggestions without 3-room test first
- No `& $agy` direct (use agy_dispatch.py)
- Logic-first (algorithm before sprite swap)
- Verifiable proof only (no "yaptım" claims)
- Update CURRENT_STATUS + memory + SESSION_LOG after every step

## 📝 Self-improvements applied tonight (meta)
- agy_dispatch.py: forced UTF-8 stdout on Windows (cp1254 encoding bug bit Antigravity ideation print)
- IDEATION_TASK.md: added "respond inline only, no file-write" instruction for Antigravity
- Memory: [[feedback-agy-inline-response-only]] saved as durable rule for future overnight runs
