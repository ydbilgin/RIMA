# CURRENT STATUS
**2026-04-29 · S43 · Phase 1**

## ACTIVE BLOCK — Sprite Rotation Production (PixelLab CFR v3 → All 10 Classes)
CFR v3 prompts ready for all 10 classes. Workflow: PixelLab UI → upload anchor → Low Top-Down → paste description → Generate v3 Rotation → save char_id → MCP animation production.
Prompts: `STAGING/PROMPTS_S43/PIXELLAB_CFR_V3_PROMPTS.md`

### PlaytestScenarios Status
`Assets/Tests/PlayMode/PlaytestScenarios.cs` created with 8 expanded PlayMode scenarios. Codex `validate_script` returned 0 diagnostics, but MCP PlayMode run did not complete because the first run stuck on `DeathScreen_PlayerDies_ShowsDeathScreen`.

Claude follow-up applied test-only QC fixes:
- `WaitForRoomCleared` uses `Time.unscaledDeltaTime`.
- `RewardPickup` wait loop uses `Time.unscaledDeltaTime`.
- `RageSystem_HitTaken_AddsRage` explicitly wires `Health.OnDamageTaken -> RageSystem.OnTakeDamage` inside the test.
- Death screen wait already uses `WaitForSecondsRealtime`.

Current state: test file is compile-clean by validator and needs a fresh Unity Test Runner / MCP re-run after the stuck MCP job is cleared. Memory sync reminder for Claude: update project memory with the PlaytestScenarios stuck-run pattern, realtime/unscaled wait rule, and explicit RageSystem event wiring note. Codex should not update memory directly for this handoff.

### Locked Decisions
| Item | Detail |
|---|---|
| Ranger Accent | Cold Blue `#7BA7BC` via RiftGlowVFX runtime |
| Elementalist | Canonical = Tunic anchor, worn/crack via VFX |
| Rift Crack | LINE baked (PixelLab), GLOW runtime (RiftGlowVFX.cs) |
| Brawler | v3 Arena Veteran — warm tan, bald, torn vest, amber crack |
| RageSystem | **CANONICAL = CODE**: 1/hit-dealt, 5/hit-taken, 3/kill, decay 10/s |
| Camera/View | **CANONICAL = 35 deg ARPG** (CoplayDev High Top-Down). 80 deg concept abandoned. |
| PixelLab CFR v3 | Upload anchor (south) → Low Top-Down → description → Generate v3 Rotation → char_id → MCP anim |
| PixelLab Direction | S43 anchors are SW-facing (exact South not achievable). Raw PixelLab labels != canonical game dirs. Do NOT rename source files. Remap applied at Unity import. |
| Playtest agent | Codex writes + runs PlayMode tests. Gemini CLI = web research only. |
| Encoding rule | Internal .md files: ASCII-only. No Turkish diacritics. |

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

## Priority Queue
1. **Sprites:** All 10 classes — PixelLab CFR v3 rotations (user generates UI) → char_id per class → MCP anim
2. **Playtest:** Re-run `PlaytestScenarios.cs` after MCP/Test Runner is unstuck; current file has Claude QC fixes but no completed PASS/FAIL run yet
3. **Skill Review:** Full skill audit all classes — Claude + user approval required
4. **Docs:** Update ARA_RAPOR_RIMA_v2.docx — re-embed anchor PNGs (manual)

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
| PlayMode Tests (x16) | Done | 16/16 PASS — Codex MCP |
| PlaytestScenarios.cs | Ready | 8 scenarios; 0 diagnostics; all 3 Claude QC fixes confirmed in file (unscaledDeltaTime x2, explicit rage wire); fresh MCP/Test Runner run pending |
| Prefab Wiring | Setup Ready | Run Tools > RIMA > Setup All Prefabs & UI |
| `RIMA.Editor.asmdef` | Fixed | |

## Doc Debt Status
| Task | Status |
|---|---|
| F01: PRODUCTION_GUIDE ref cleaned | Done |
| F02: RageSystem values → SYSTEM_MAP | Done |
| F03: SYSTEM_MAP stale scripts | Done |
| F07: CROSS_CLASS_SKILL_MATRIX archived | Done — Codex |
| F08: STYLE_BIBLE camera sync | Done — Claude |
| F09: STYLE_BIBLE color table sync | Done — Claude |
| F10: Stale anim guides archived | Done — Codex |
| F11: SINIF name drift cleaned | Done — Codex |
| F12: Elementalist MASTER-SINIF sync | Done — Codex |
| F13: FAZ3 Crusader/Lancer/Rift Parry removed | Done — Codex |
| F14: FAZ5 stale class/skill updated | Done — Codex |
| F15: GDD stale header | Done |
| F16: BOSS_DESIGN, COMBAT_ROSTER S42 refs | Done |
| SINIF QC (Claude) | PASS — Soul Dart written |
| Encoding fix: STYLE_BIBLE.md rewritten ASCII | Done — Claude |
| Agent routing update: AGENTS.md | Done — Claude |

## Infrastructure
- Git repo (code/docs only, PNGs excluded)
- .gitattributes added — LF normalization, binary exclusions
- QC Master: `STAGING/anchors/_ANCHOR_QC_MASTER_S43.md`
- ARA_RAPOR_RIMA_v2.docx — anchor PNGs updated, needs re-embed (manual)
- MCP: CoplayDev v9.6.8 via uvx/mcpforunityserver

## Refs
- **Scene:** `Assets/Scenes/_IsoGame.unity`
- **Architecture:** `SYSTEM_MAP.md`
- **Decisions:** `MASTER_KARAR_BELGESI.md`
