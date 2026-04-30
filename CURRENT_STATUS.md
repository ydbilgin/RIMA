# CURRENT STATUS
**2026-04-30 · S43 · Phase 1**

## ACTIVE BLOCK — Skill Sheet Visual + Sprite Pipeline

### Skill Design Review Cycle — COMPLETE (R1/R2/R3/R4 + Codex R4 ACCEPT_WITH_CHANGES)
Full review cycle done: ChatGPT R1 -> Codex R1 -> Claude R2 -> ChatGPT R2 + Codex R2 -> Claude R3 -> R4 decisions -> Codex R4 review -> Claude final lock.
All decisions locked. 3 design docs written.

### 3 Design Documents — DONE (2026-04-30)
- `TASARIM/CLASS_STATE_CONTRACT.md` v0.1 — 14 public + ~11 internal states, ownership lock (Sundered=Warblade, Shattered=Brawler), boss/abuse caps universal patterns
- `TASARIM/SUMMONER_ECONOMY_RULES.md` v0.1 — minion cap 8, sacrifice CD 1.5s, charge fill rules, Phase tags, Bone Tide/Soul Tax deferred Later
- `TASARIM/GLOBAL_REPEAT_RULES.md` v0.1 — counter ayrimi (absorb/pre-draw/whiff), Movement Option C, execute gates, zone stacking, **Ulti Toggle: Perfect Condition (rename) + relock-after-cast + room-start-reset**

### R4 Final Lock Highlights (applied to docs)
- "Resource MAX = ulti" -> **"Perfect Condition triggers empowered cast"** (Heat ZERO, Hex Stack 10 fit)
- Ulti relocks after cast; all locks reset ON at room start
- Brawler upgraded state = **Shattered** (NOT Sundered) -- Warblade identity preserved
- Wall-Slammed Phase 2/Later, fallback Ground-Slammed mandatory
- ~50% R4 extra skills converted passive/upgrade (Wound Echo, Pain Reservoir, Scar Echo, Hawk Eye merge, Reload Roll upgrade, Beacon Pull = Command Beacon upgrade, Whisper Mark passive)
- "Just damage" FLAGs resolved: Empty Mag Burst -> Suppressed, Backfire Shot -> Exposed Line + self Backfire, Shockwave Fist -> Off-Balance, Crimson Pact -> Blood Debt, Wind Read -> Opened
- Trinity Storm = Elementalist 2nd ulti (Lightbreak demoted to system trigger)

### Next Block
- ChatGPT skill sheet visual (all skills locked -> image prompt)
- Sprites: CFR v3 pipeline (all 10 classes, PixelLab UI)
- Playtest: re-run PlaytestScenarios.cs

### Pending Design Decisions (user input needed)
- Shadowblade Scar mechanic: integration still shallow (only RMB + Dash create Scars)
- Hexer: no phase-skip — DoT build linear (addressed partially in R3 behavior changes)

### PlaytestScenarios Status
`Assets/Tests/PlayMode/PlaytestScenarios.cs` — 8 scenarios, 0 diagnostics, 3 QC fixes applied.
Fresh MCP run completed 2026-04-30: PlayMode `RIMA.Tests.PlayMode` passed 24/24; failed 0; skipped 0.

---

## LOCKED DECISIONS

### Production / Technical
| Item | Detail |
|---|---|
| Ranger Accent | Cold Blue `#7BA7BC` via RiftGlowVFX runtime |
| Elementalist | Canonical = Tunic anchor, worn/crack via VFX |
| Rift Crack | LINE baked (PixelLab), GLOW runtime (RiftGlowVFX.cs) |
| Brawler | v3 Arena Veteran -- warm tan, bald, torn vest, amber crack |
| RageSystem | CANONICAL = CODE: 1/hit-dealt, 5/hit-taken, 3/kill, decay 10/s |
| Camera/View | CANONICAL = 35 deg ARPG (CoplayDev High Top-Down). 80 deg abandoned |
| PixelLab CFR v3 | Upload anchor (south) -> Low Top-Down -> description -> Generate v3 Rotation -> char_id -> MCP anim |
| PixelLab Direction | S43 anchors SW-facing. Raw labels != canonical game dirs. Remap at Unity import, never rename source files |
| Playtest agent | Codex writes + runs PlayMode tests |
| Encoding rule | Internal .md files: ASCII-only. No Turkish diacritics |
| Agent Briefing | Claude = team captain. Always leave briefing in CODEX.md / CODEX_TASKS.md when decisions affect other agents |
| Ravager Undying Tenacity | death-cheat: fatal hit -> HP 1, 4s immunity + heal disabled, CD 45s |

### Skill System (R3/R4 Locked)
| Item | Detail |
|---|---|
| Cross-Class System | State response families (not pair matrix). Each class: 1 primary public state + optional internal |
| Execute Gates | No HP<30% execute. Each class uses class-specific state gate (see below) |
| Counter Separation | Warblade: absorb/break input. Ronin: pre-draw timing. Brawler: whiff/evade body movement |
| Movement System | Option C: Space = short/no state/no damage/resource-neutral. Skill movement = state-interaction required |
| Ulti Toggle | Shift+skill key per-skill toggle. Default Lock ON. Class resource MAX = ulti. No separate currency |
| Brawler Core | weave/combo/break. Launched REMOVED. Wall-Slammed (tricks, no custom mob anim). Pinned added |
| Pixel Art Constraint | Skills: caster anim + overlay VFX + env VFX + code-driven mob slide. NO custom mob animations |
| Wall-Slammed Method | Caster push anim + code slide + wall impact VFX + hit-react + camera shake + hit-stop. No new mob anim |
| Skill Test | 6-line: Verb / State / Slot Reason / Overlap / Abuse / Encounter Question |
| State List | 14 public states (see below). ~25 total with internal |
| Lightbreak | Convergence Meter (single bar, Fire fills left, Frost fills right). Comprehension test gate |
| Smoke Veil Faz 1 | Minimum spec: projectile tracking break + aim cone reset + boss no-invis abuse |
| Class State Contract | Iterative: v0.1 -> class pass -> v0.2 lock. Mandatory fields: Producer/Consumers/Public-Internal/Readability/Boss Rule/Abuse Cap |
| Iron Crush | Redesign: 6s Crush stack, Broken target 2x, 3 stacks -> Sundered, cross-class window, boss = micro-stagger only |
| Death Blow Gate | Broken / Sundered / Staggered / Rift-marked target (not HP<30%) |
| Summoner Priority | Economy Rules before class redesign. Separate doc required |
| Max Movement Skills | 1 per build (default). No Space+skill movement i-frame stacking. No CD reset loops |

### Execute Gates (per class)
| Class | Execute Skill | Gate |
|---|---|---|
| Warblade | Death Blow | Broken / Sundered / Staggered target |
| Ranger | Final Strike | Marked + trap-triggered target |
| Gunslinger | Deadshot | Last bullet / perfect reload / line aim |
| Ronin | Flash Draw | Tension full + draw window |
| Shadowblade | Severance | Scar collapse afterward |

### State List (Final)
| Class | Public | Internal/Hybrid |
|---|---|---|
| Warblade | Broken | Sundered |
| Elementalist | Burning, Frozen | Converged/Lightbroken |
| Shadowblade | Rift Scar | Collapsing, Phased Through |
| Ranger | Marked, Trapped | Snared |
| Ravager | Wounded | Frenzied (self), Blood Debt (self) |
| Ronin | Opened | Draw Window, Afterimage |
| Gunslinger | Suppressed | Exposed Line |
| Brawler | Cracked, Wall-Slammed | Pinned, Off-Balance |
| Summoner | Corpse Field | Sacrifice Mark, Commanded |
| Hexer | Hexed, Overloaded | Cursed Link |

Public count: 14. Target: 12-16. PASS.

### Ulti-Capable Skills (per class)
| Class | Skill 1 | Skill 2 |
|---|---|---|
| Warblade | Death Blow | Iron Charge |
| Ravager | Bloodied Roar | Carnage Spin |
| Ronin | Flash Draw | Iaido Strike |
| Shadowblade | Severance | Veil Flicker |
| Ranger | Final Strike | Quiver Pulse |
| Gunslinger | Deadshot | Empty Mag Burst |
| Elementalist | Lightbreak | Trinity Storm |
| Summoner | Mass Sacrifice | -- |
| Hexer | Hexblast | Hex Cascade |
| Brawler | Pulverize finisher | Glass Strike |

### Skill Additions (R4 -- pending Codex R4 review lock)
See CODEX_TASKS.md for full list. ~30 extra skills across 10 classes.
Will go into SINIF_VE_SKILL_KARAR_BELGESI.md after Codex R4 lock.

---

## Anchor Status
| Class | Status | char_id | Rot | Anim |
|---|---|---|---|---|
| Warblade | LOCKED | pending | pending | pending |
| Ravager | LOCKED | pending | pending | pending |
| Ronin | LOCKED | pending | pending | pending |
| Gunslinger | LOCKED | pending | pending | pending |
| Ranger | LOCKED | pending | pending | pending |
| Shadowblade | LOCKED | pending | pending | pending |
| Elementalist | LOCKED | pending | pending | pending |
| Summoner | LOCKED | pending | pending | pending |
| Hexer | LOCKED | pending | pending | pending |
| Brawler | LOCKED | pending | pending | pending |

---

## Priority Queue
1. **SINIF_VE_SKILL_KARAR_BELGESI.md**: integrate R4 extra skills + active/passive split + Phase tags
2. **Skill Sheet**: ChatGPT generates visual (all skills locked, ready for image prompt)
3. **Sprites**: All 10 classes CFR v3 (user generates PixelLab UI) -> char_id -> MCP anim
4. **Playtest**: Re-run PlaytestScenarios.cs after MCP/Test Runner is unstuck
5. **Docs**: Update ARA_RAPOR_RIMA_v2.docx -- re-embed anchor PNGs (manual)
6. **Archive**: STAGING review files (R1/R2/R3/R4) -> ARCHIVE/ (3 docs replaced them)

---

## Script Status
| Script | Status | Note |
|---|---|---|
| `BossAI_PenitentSovereign.cs` | Done | |
| `HollowMite.cs` | Done | |
| `TheWound.cs` | Done | |
| `HandGlowVFX.cs` | Done | |
| `RiftGlowVFX.cs` | Done | |
| `SkillFlowTracker.cs` | Done | |
| `DeathScreenManager.cs` | Done | |
| `PrefabWiringSetup.cs` | Done | |
| EditMode Tests (x6) | Done | |
| PlayMode Tests (x16) | Done | 16/16 PASS -- Codex MCP |
| PlaytestScenarios.cs | PASS | 8 scenarios; 0 diagnostics; MCP PlayMode run passed 24/24 on 2026-04-30 |
| Prefab Wiring | Setup Ready | Run Tools > RIMA > Setup All Prefabs & UI |
| `RIMA.Editor.asmdef` | Fixed | |

---

## Doc Debt Status
All F01-F16 items Done. Skill name drift fully resolved (CT-SKILL-01). R3/R4 design review cycle complete.
3 new design docs DONE (CLASS_STATE_CONTRACT, SUMMONER_ECONOMY_RULES, GLOBAL_REPEAT_RULES).
Pending: SINIF_VE_SKILL_KARAR_BELGESI.md integration of R4 extra skills + active/passive classification.

---

## Review Files (ARCHIVED 2026-04-30)
| File | Status |
|---|---|
| ARCHIVE/CODEX_TAMAMLANDI/CHATGPT_YORUMU_2026-04-29.md | ARCHIVED |
| ARCHIVE/CODEX_TAMAMLANDI/CODEX_DEGERLENDIRME_CHATGPT_YORUMU_2026-04-29.md | ARCHIVED |
| ARCHIVE/CODEX_TAMAMLANDI/REVIEW_R2_CLAUDE_DECISIONS_2026-04-30.md | ARCHIVED |
| ARCHIVE/CODEX_TAMAMLANDI/CHATGPT_R2_2026-04-30.md | ARCHIVED |
| ARCHIVE/CODEX_TAMAMLANDI/CODEX_R2_2026-04-30.md | ARCHIVED |
| STAGING/CLAUDE_R3_FINAL_DECISIONS_2026-04-30.md | Skipped (decisions in STATUS) |

---

## Infrastructure
- Git repo (code/docs only, PNGs excluded)
- .gitattributes added -- LF normalization, binary exclusions
- QC Master: `TASARIM/_ANCHOR_QC_MASTER_S43.md`
- ARA_RAPOR_RIMA_v2.docx -- anchor PNGs updated, needs re-embed (manual)
- MCP: CoplayDev v9.6.8 via uvx/mcpforunityserver
- PixelLab MCP: added 2026-04-29 to Claude (.claude.json) + Codex (config.toml)
  - Tools: animate_character, get_character, list_characters. create_character FORBIDDEN.
  - Keyframe/first-last-frame NOT in MCP API -- UI only.
  - Gen budget: 2553/5000 used, ~2447 left, deadline 2026-05-18.
- Statusline: `C:/Users/ydbil/.claude/usage_statusline.py` via `.bat` wrapper

---

## Refs
- **Scene:** `Assets/Scenes/_IsoGame.unity`
- **Architecture:** `SYSTEM_MAP.md`
- **Decisions:** `MASTER_KARAR_BELGESI.md`
- **Skills:** `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
- **Review Cycle:** `STAGING/CODEX_TASKS.md` (R4 review active)
