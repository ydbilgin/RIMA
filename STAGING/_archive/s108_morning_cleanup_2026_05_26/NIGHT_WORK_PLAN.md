# RIMA S93 Night Work Plan (Self-Instruction Doc)

**Purpose:** Continuous work plan for orchestrator (Claude) during user sleep. When re-invoked by agent completion notification, read this doc + CURRENT_STATUS.md + recent agent output, execute next phase.

**User constraints:**
- Zero PixelLab gen tonight (Codex `gpt-image-1` OK, different billing)
- Save status + memory frequently (electricity risk)
- Use agents heavily — Opus for design, Codex for code/review
- CB path: `F:\LaurethStudio\02_GAMES\CircuitBreaker`
- Mechanic bank: `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK`
- Goal: morning briefing ready, decisions queued for user

---

## Phase 1 — Active Dispatches (NOW)

| ID | Agent | Task | Output |
|---|---|---|---|
| `bxzr42sk8` | Codex | Ronin implementation Day 1 | `STAGING/CODEX_DONE_ronin_implementation.md` |
| `a8dd3f49c6552dac5` | Opus rima-design | Tile angle architecture | `STAGING/TILE_ANGLE_ARCHITECTURE_OPUS.md` |
| `abca8238da3126548` | Opus rima-design | CB pivot + epic mechanic | `STAGING/EPIC_MECHANIC_AND_CB_PIVOT_OPUS.md` |
| `btecte55f` | Codex | 4-class skill bank | `STAGING/RIMA_4CLASS_SKILL_DESIGN_BANK.md` |
| `a242c808b1024097b` | general-purpose | DaveX tweet scrape | `STAGING/twitter_research_DaveX/SUMMARY.md` |

---

## Phase 2 — Completion Handlers (do when each returns)

### When **Codex Ronin (`bxzr42sk8`)** completes:
1. Read `STAGING/CODEX_DONE_ronin_implementation.md`
2. Verify code compiles (Unity Console via UnityMCP `read_console`)
3. If errors: dispatch Codex fix dispatch
4. If clean: dispatch Codex SELF-REVIEW of own Ronin code (quality, pattern adherence)
5. Update `CURRENT_STATUS.md` Active Dispatches table
6. Save memory note about Ronin completion if Tension resource design is locked

### When **Opus Tile Angle (`a8dd3f49c6552dac5`)** completes:
1. Read `STAGING/TILE_ANGLE_ARCHITECTURE_OPUS.md`
2. Extract single recommended branch (A-F)
3. Dispatch Codex review: validate branch feasibility, technical risks
4. Save memory: tile angle branch lock + revoke conflicting prior memory
5. Update CURRENT_STATUS

### When **Opus CB Pivot (`abca8238da3126548`)** completes:
1. Read `STAGING/EPIC_MECHANIC_AND_CB_PIVOT_OPUS.md`
2. Extract pivot verdict + top epic mechanic candidate
3. Dispatch Codex review: validate pivot honesty + epic mechanic implementability
4. Save memory: CB pivot decision direction + epic mechanic candidate
5. Update CURRENT_STATUS

### When **Codex 4-Class Skill (`btecte55f`)** completes:
1. Read `STAGING/RIMA_4CLASS_SKILL_DESIGN_BANK.md`
2. Validate count (should be 48 skills total, 4 active + 4 passive + 4 echo per class)
3. Dispatch Opus rima-design review: balance audit, family tag distribution, signature strength
4. Save memory: skill bank reference + implementation priority order
5. Update CURRENT_STATUS

### When **DaveX Tweet (`a242c808b1024097b`)** completes:
1. Read `STAGING/twitter_research_DaveX/SUMMARY.md`
2. If asset pack workflow is real + actionable:
   - Dispatch Codex experiment: ONE gpt-image-1 prompt for "RIMA asset pack" (floor tile + walls + props + scatter as single coherent tilesheet)
   - $0.10 cost, single prompt, no PixelLab usage
3. If not actionable: just note in status
4. Save memory: ChatGPT asset pack workflow reference

---

## Phase 3 — Cross-Cutting Research (do when <2 dispatches running)

Order by ROI:

1. **Hades art pipeline deep dive** (web search) — independent of Opus, validates verdict
   - Search: "Hades 2D 3D pixel art floor tile pipeline interview"
   - Search: "Supergiant Games art process Hades"
   - Save findings to `STAGING/RESEARCH_hades_art_pipeline.md`
2. **Children of Morta + Death's Door tile angle** — comparable angled top-down references
   - Search per-game art tile examples
   - Save to `STAGING/RESEARCH_angled_topdown_games.md`
3. **Mechanic bank direct study** — read `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\` contents directly
   - List all mechanic categories + counts
   - Save summary to `STAGING/RESEARCH_mechanic_bank_summary.md`
4. **Codex Ronin code spot-check** — when Ronin completes, read actual .cs files committed
   - Verify Tension resource logic against Warblade Rage pattern
   - Save to `STAGING/CODEX_REVIEW_ronin_code_quality.md`
5. **Codex review dispatch of EACH completed agent** (parallel chain)

---

## Phase 4 — Synthesis Layer (when all Phase 2 done)

1. Dispatch ONE mega-Codex synthesis: `STAGING/codex_task_morning_briefing_synthesis.md`
   - Inputs: all 5 Phase 1 outputs + all Phase 2 reviews + Phase 3 research
   - Output: `STAGING/MORNING_BRIEFING.md` — single decision-tree for user
2. Update CURRENT_STATUS.md TLDR for morning pickup
3. Memory index cleanup
4. Final task list status

---

## Phase 5 — Optional Tile Angle Experiment

ONLY IF:
- DaveX tweet shows ChatGPT asset pack workflow viable AND
- Opus tile angle verdict mentions perspective-baked tile option AND
- User constraint "no PixelLab gen" doesn't apply to gpt-image-1 (verify — gpt-image-1 is Codex subscription, different from PixelLab)

Then:
- Single gpt-image-1 dispatch via Codex for "RIMA tilesheet at 35° low top-down angle, 4 floor tiles + 4 wall tiles + scatter props, painterly chunky pixel art, no border, seamless edges"
- $0.10 cost
- Output evaluated for tile angle match

---

## Status Save Cadence

- **Every agent completion:** brief status line + memory note if discovery
- **Every 3 completions:** full CURRENT_STATUS update
- **Phase transition:** full status + memory update
- **Before Phase 4 synthesis dispatch:** full status snapshot
- **Final (Phase 4 done):** MORNING_BRIEFING.md complete + STATUS.md TLDR rewritten

---

## End-State Morning Target

By the time user wakes up:

- ✅ `STAGING/MORNING_BRIEFING.md` — synthesis of all night work, ready for 5-min read
- ✅ `CURRENT_STATUS.md` TLDR clear about night decisions + open questions
- ✅ All 5 Phase 1 outputs locked + each reviewed by Codex
- ✅ Hades + comparable game research saved
- ✅ Mechanic bank summary
- ✅ Codex Ronin code committed (or BLOCKED report)
- ✅ Tile angle verdict locked (single branch)
- ✅ CB pivot honest verdict (continue/pivot/hybrid)
- ✅ Top 1 epic mechanic candidate
- ✅ 48 skill designs documented
- ✅ DaveX workflow understood (actionable yes/no)
- ✅ Memory index clean

---

## Hard Rules (do NOT violate)

- ❌ NO PixelLab gen tonight (`mcp__pixellab__*` create/animate/etc. forbidden)
- ✅ Codex `gpt-image-1` OK (different billing, Codex subscription)
- ❌ NO duplicating ongoing agent work
- ❌ NO decisions for user — only dispatch + synthesize + present options
- ❌ NO more than 6 parallel agents (context fragmentation risk)
- ✅ Status save EVERY checkpoint
- ✅ Read this doc on re-invocation

---

## Re-Invocation Checklist (for future me)

When notified an agent completes:
1. Read `STAGING/NIGHT_WORK_PLAN.md` (this doc)
2. Read tail of `CURRENT_STATUS.md` for last state
3. Identify completed agent from notification
4. Execute completion handler from Phase 2
5. Save status update
6. If <2 dispatches running, pick next from Phase 3 or dispatch follow-up review
7. If all Phase 1 + Phase 2 done, start Phase 4 synthesis
8. Repeat until all phases complete or user wakes
