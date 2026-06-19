# RIMA Demo-Readiness Report — 2026-06-19
> Live re-verification (workflow `wf_6b63fb6e-f5c`): 2 serial Unity passes (in-play + editor) + synthesis. Supersedes the earlier draft. Evidence logs: `STAGING/_process/2026-06/_demo_readiness_play_2026-06-19.md` + `_demo_readiness_editor_2026-06-19.md`. Screenshots: `Assets/Screenshots/Playtest_2026-06-19/`.

## (1) Demo-Item Status Matrix

| # | Demo Item | Status | Evidence |
|---|-----------|--------|----------|
| 1 | Full flow (MainMenu→CharSelect→_Arena, both classes) | 🟢 GREEN | Both runs clean; Player live, RRD.builder=IsoRoomBuilder WIRED, enemies=2, timeScale=1, 0 console errors |
| 2 | Opening draft (entry skill pick) | 🟢 GREEN | IsDraftActive=True + 3 cards; PickCard(0)→False, timeScale=1, skill equipped |
| 3 | Elementalist combat — small fireball LMB + impact + status tint | 🟢 GREEN | RiftBolt scale 0.32 (small, NOT blob) + trail, Fire color, dmg 60→38; **SES now wired onto FractureImp/Penitent/HalfThrall prefabs** → Fire LMB → Burning DoT (HP 60→48) + RED tint; Frost → Chill (moveSpeed 0.80) + BLUE tint — verified live on the default demo enemy |
| 4 | Warblade combat (LMB Cleave + RMB War Stomp) | 🟢 GREEN | Cleave 30→2; War Stomp Rage 100→70, AoE kill 32→0 |
| 5 | T9 death→restart | 🟢 GREEN | RestartRun()→draft re-appeared (timeScale=0)→pick→timeScale=1 |
| 6 | T7 reward→door | 🟢 GREEN | Combat clear→RewardPickup→Collect→draft→pick→DoorOpen, 7 doors |
| 7 | Build Mode (place props, persist) | 🟢 GREEN | Enter(timeScale=0)→2 props (0→2)→Exit(timeScale=1, persist 2/2, HP=100) |
| 8 | Director Mode (spawn/stat/telemetry) | 🟢 GREEN | spawn 0→1, telemetry=4, SetStat maxHP 250=True; reset before exit |
| 9 | HUD (skill bar + cooldown + HP) | 🟢 GREEN | 6 slots + CD timer nodes; HPFill=1.00; HUDController on HUD_Canvas |
| 10 | Boss (Penitent Sovereign telegraph + phases) | 🟡 YELLOW | Code-confirmed (Telegraph, WindupSeconds, Phase1/2Turn, DoPhase2/3Transition, 8 attacks), NOT live-reached this pass |
| 11 | Map Designer — cliff auto-generate (USER-LEVEL) | 🟢 GREEN | UnifiedMapDesigner, 26 templates; CliffAutoPlacer Regenerate 0→22 deterministic, floor=128 |
| 12 | Map Designer — hand-paint + reconcile | 🟢 GREEN | AddManualPainted→Regenerate folded it 22→23, painted survived; Clear→22 |
| — | Console health | 🟢 GREEN | 0 errors both passes; 1 benign scene-reload-cleanup warning on RestartRun |

**Tally: 11 GREEN · 1 YELLOW (Boss — code-confirmed, optional live stretch) · 0 RED. No demo-killers.**

## (2) Final Presentation Order
> Thesis: *"RIMA is not just a game — it's an environment with reusable tooling. ~20% game / ~60% architecture-tooling / ~20% graphify audit."* Lead tooling-first.

**ACT A — Environment is Authored, Not Hardcoded (~60% tooling)**
1. **Map Designer — cliff auto-generate.** Open "RIMA/Map Designer," Regenerate → 22 tiles deterministically. *"Levels aren't hand-placed prefabs — a tool generates cliff-island geometry from rules."*
2. **Hand-paint + reconcile.** Paint a cell, Regenerate → 22→23, painted survives, non-adjacent auto-rejected. *"Designer overrides and procedural geometry reconcile, not fight."*
3. **Build Mode (in-play editor).** Enter, place props, Exit → persist, gameplay resumes. *"Same authoring power inside the running game."*
4. **Director Mode.** Spawn enemy, bump stat, show telemetry. *"Live tuning — spawn, stat-route, telemetry, no recompile."*

**ACT B — It Plays Like a Game (~20%)**
5. **Full flow + opening draft.** Class select → run → entry skill card. *"Roguelite loop."*
6. **Elementalist combat — the NEW juice.** Small fireball LMB + trail + impact, then **status tint: chill=blue, burn=red pulse** (fires naturally on the default demo enemies — SES now wired). *"Readable small projectiles + elemental status recolors the enemy."*
7. **Warblade combat.** LMB Cleave + RMB War Stomp AoE. *"Second class, melee/Rage identity."*
8. **T7 reward → door.** Clear → reward → pick → door. *"Room-clear rewards a choice + gates progression."*
9. **T9 death → restart.** Die → restart → draft re-appears. *"Clean death loop, no stale state."*
10. **HUD beat.** Skill bar cooldown + HP. *"Live cooldown + HP feedback."*

**ACT C — Audit Layer (~20%)**
11. **Graphify figures.** God-nodes, tooling clusters, dependency audit. *"Codebase is queryable — modular tooling, not spaghetti."*
> Boss = optional code/telemetry-confirmed mention or stretch live attempt; NOT a required beat.

## (3) Demo-Day Checklist + Pre-Demo Fixes
**Hygiene (before presenting):**
- [ ] **MCP OFF** — close UnityMCP/embedded-python bridges. No agent touching Unity during demo.
- [ ] **Run dev-direct from `_Arena`** — not standalone/GameEntryScenes path.
- [ ] `_Arena` clean state (NOT unsaved verification state); reload scene fresh.
- [ ] Director stats default, timeScale=1.
- [ ] Backup screenshots staged: `Assets/Screenshots/Playtest_2026-06-19/`.
- [ ] Verify no freeze on enter/exit Build Mode (timeScale dual-owner guards in).

**YELLOW — present-as-is, know the script:**
- **#3 Status tint on default enemy: FIXED** — StatusEffectSystem now wired onto FractureImp/Penitent/HalfThrall prefabs (verified live: Fire→Burning+RED, Frost→Chill+BLUE). The tint/burn/chill beat fires naturally on the default demo enemy; no special enemy or pre-demo content fix needed.
- **#10 Boss:** not live-reached. Code-confirmed mention or optional stretch; don't script a required live boss kill.

**RED: none.**

---
*Lead tooling-first (Acts A→B→C) to land the "environment + reusable tooling" thesis. SES is now wired onto the demo enemies, so the new burn/chill tint fires naturally — no pre-demo content fix required. Only YELLOW left is the boss (code-confirmed, optional live stretch).*

## (4) PixelLab/VFX inventory (no new generation needed)
Explosion VFX exist (`rima_vfx_explosion` ×2 → `Resources/VFX/Skills/vfx_explosion_a/b.png`, Fireball+LMB impact wired) + shatter + 8-dir Fireball + meteor/frozen_orb. PixelLab Tier 2 active (5000 gen available).
