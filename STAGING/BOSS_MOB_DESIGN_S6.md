# RIMA — BOSS & MOB DESIGN LOCK (S6 demo)

> Status: **LOCKED (Opus, S6)** · Scope: 5-room wishlist vertical slice (~10 min) · One boss, three demo mobs.
> Built on: `DESIGN_LOCK_DEMO_S6.md`, `RIMA_DIRECTION_LOCK_S6.md`, `DECISIONS_S6.md`, and the **live code**
> `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs` (2-phase, HP 800) + `BaseMobBehavior.cs` / `ShardWalker.cs`.
> Design only. Every number is a serialized-field target. **VFX-first / graybox-first**: telegraphs are color+shape; no art required to ship.

---

## 0. CANON RECONCILIATION (the one conflict, resolved)

| Source | Says |
|---|---|
| `RIMA_DIRECTION_LOCK_S6.md` §2 | Demo boss = "50%-HP single placeholder kill phase only (full 3-phase deferred)" |
| `DESIGN_LOCK_DEMO_S6.md` §1.3 + user brief | 3-phase, chains break at 50%, true-form beat near 33% |
| Live code `PenitentSovereign.cs` | 2-phase: transition at 50%, HP 800, Phase 2 = +40% speed + new roster |

**DECISION (Opus): "2+1"** — keep the live 50% chains-break as the real phase gate (P1→P2), add a **lightweight
Phase-3 "Unleashed" overlay at 33%** = a *modifier layer on the existing P2 roster*, NOT a third full attack set.
Satisfies canon's 33% chains-shatter true-form beat + the user's 3-phase brief, honors "don't build a raid",
maps onto the code already written. **Supersedes** the literal §2 "single placeholder" wording (later DESIGN_LOCK + user win).

---

## 1. THE PENITENT SOVEREIGN — 3-phase design

**Identity (canon):** tragic self-punishing guardian; chains = self-discipline holding the Rift corruption IN.
"Bowed but unbroken." Emotional arc = his discipline visibly losing across three phases.

**Global stats (keep live):** `bossMaxHP = 800`, `telegraphDuration = 0.75` (P1 base), `meleeStopRange = 1.6`,
`detectionRange = 14`, `cooldownMin/Max = 1.2/2.0`. Arena = R5 14×14, 4 chain anchors.

**Cyan law (readability spine):** cyan `#00FFCC` on the boss = seal-energy bleeding through as discipline fails
(boss-only exception to the cyan reservation). Telegraph SHAPES stay warm(P1)/purple(P2) so "danger windup" ≠ "seal glow".
- P1: cyan contained (thin chain seams), body slate — "sealed, holding."
- P2 (50%): cyan cracks open, telegraph→purple `#7A00FF` — "the wound is open."
- P3 (33%): cyan floods (body veined), telegraph gains cyan leading edge — "seal loose, only rage."

### PHASE 1 — "The Vigil" (800→400) — teaching phase, fully telegraphed, speed 2.5, telegraph 0.75
Rotation = live `{ChainWhip, ShackleThrow, Surge, HolyLash}`:
| Attack | Telegraph | Active | Recovery | Dmg | Punish |
|---|---|---|---|---|---|
| Chain Whip | 0.75s line tell | 6m×1.2m line, 1 tick | ~0.6s | 20 | sidestep line |
| Shackle Throw (ranged) | 0.60s | projectile spd 7, 2s slow | ~0.5s | 12 | dodgeable |
| Penitent Surge | 0.85s + trauma 0.5 | 4m radial + 10 knock | ~0.7s | 15 | outrange >4m |
| Holy Lash | 0.75s | 180° arc r2.5 + 7 knock | ~0.6s | 18 | roll through |

**50% beat — CHAINS BREAK:** live `phaseTransitionDone` (≤0.5·Max), ~1.5s freeze/invuln → P2 (speed×1.4, new roster).
Readable: white-flash→purple-burst (live) + 2/4 chain anchors snap (cyan spit), trauma 0.7, monolog *"Discipline breaks before the chain does."* + 0.1s hitstop+zoom via existing HitPauseDriver/CameraPunchController.

### PHASE 2 — "The Open Wound" (400→264) — speed 3.5, shorter tells
Rotation = live `{FractureStrike, FractureCharge, ChainExplosion, Wrath}`:
| Attack | Telegraph | Active | Recovery | Dmg | Counterplay |
|---|---|---|---|---|---|
| Fracture Strike | 0.30s + 3× flash | 3 melee ticks, final knock 8 | ~0.5s | 16×(≤3) | dash out after tick 1 |
| Fracture Charge | 0.65s | arena dash spd18, line + 12 knock | ~0.5s | 22 | dodge perpendicular |
| Chain Explosion | 0.50s | 3 markers, 3s delay 1.2m blasts + 9 knock | — | 25 | move; markers show gaps |
| Sovereign's Wrath | **1.45s** + trauma 0.8 | full ring, **2.5m center safe** | ~0.6s | 30 | RUN TO HIM (safe under boss) |

### PHASE 3 — "Unleashed" (264→0) — the OVERLAY (no new coroutines)
**33% beat:** new one-shot gate `≤ Ceil(Max·0.33)`, ~1.0s shorter transition, then apply modifier layer:
- speed ×1.15 (3.5→~4.0); `cooldownMin/Max` ×0.8 (→~1.0/1.6); all telegraphs ×0.85 **floored at 0.22s** (reaction floor — never cross);
- drop Shackle Throw/slow entirely; bias rotation toward Fracture Charge + Fracture Strike (de-weight the long tells).
Readable: remaining 2 anchors shatter; body full cyan-veined; telegraph cyan leading edge; monolog *"There is nothing left to hold."*
Game-feel: same 0.1s freeze+zoom as 50%; boss-death = 0.20s hitstop tier + trauma 1.0 (live).
**Why enough:** three distinct beats (heavy/sealed → fast/cracked → flurry/unleashed) + two chain-shatter moments + rising cyan flood = the *feel* of 3 phases without a third roster's cost.

**Death routing (locked):** `suppressClassSelectOnDeath = true` STAYS true; death fires `RaiseDemoComplete`→Victory, NOT ClassSelectionTrigger. R5 gate-less.
**HP tuning knob:** 800 ≈ 90–120s fight. If it drags, drop `bossMaxHP` to 650 before touching damage.

---

## 2. DEMO MOB ROSTER + ENCOUNTER PACING

**Value-contrast rule:** muted slate body, each mob clears a value-contrast floor vs the slate floor; cyan reserved (mob telegraphs WARM, never cyan).

| Mob | Live HP | Role | Behavior | Window | Threat | Value contrast |
|---|---|---|---|---|---|---|
| FractureImp | 100→**60** | Swarm | chase (3/detect 8), melee 1.5m; death=ShardScatter | melee ~0.3–0.4s → tick, cd 1.5s | low solo, swarm-dangerous | **lightest** (most numerous); amber core fleck, NOT cyan |
| ShardWalker_GB | 55 | Skirmisher/zoner | kites 2–5m, fires shards | **0.35s tell → projectile** dmg8 spd7 cd1.8; death=6 radial | medium; punishes standing still | mid; **FIX: telegraph cyan→amber** |
| HollowHulk_GB | 280 | Tank/anvil | slow, high-poise, big committed melee | ~0.6–0.7s windup → heavy + knock, long recovery | high per-hit, low mobility | **darkest/heaviest**; amber seam in body crack |

### 2.2 Per-room (5 rooms)
| Room | Type | Mobs | Teaches | Diff |
|---|---|---|---|---|
| R1 Broken Entry | Combat (tutorial) | **3 FractureImp**, single wave | combo/dash, "cyan=boundary"; one type = clean read | ★☆☆☆☆ winnable first try |
| R2 Guard Hall | Combat | W1 **4 Imp** → W2 **2 Imp + 1 ShardWalker** | ranged threat in isolation; close the gap | ★★☆☆☆ |
| R3 Ambush Cloister | Combat (peak) | W1 **2 ShardWalker + 2 Imp** → W2 **1 Hulk + 3 Imp** | tank + mixed-threat; prioritize targets | ★★★☆☆ hardest non-boss |
| R4 Vestibule | Reward (no mobs) | none | breather + draft; pacing valley | — |
| R5 Containment | Boss | Penitent Sovereign | climax; everything learned | ★★★★★ |

**Curve:** monotype → +ranged → +tank/mix → rest → boss. Each threat introduced ALONE one room before combined.
**Guardrail:** on-screen ≤~5 (R1-2), ≤~6 (R3); AttackTokenManager gates concurrent attackers. If R3 chaotic, cut W1 ShardWalkers to 1 before touching HP.

---

## 3. OPEN QUESTIONS (flagged)
0. **NLM auth expired** at authoring → couldn't re-read boss phase-3 source `0263dc65`. Spec is canon-grounded via ratified locks. If `0263dc65` names a third-form attack, fold into the P3 overlay. **User: `! nlm login` before boss-art.** Not a blocker.
1. **2-phase vs 3-phase (DECIDED "2+1")** supersedes `RIMA_DIRECTION_LOCK_S6.md` §2 literal wording. Confirm §2 updates to "2+1".
2. **Boss-death misroute** (known): R5 death must fire RaiseDemoComplete→Victory (suppressClassSelectOnDeath stays true). Owned by Victory work.
3. **FractureImp HP (DECIDED 60)** — revert to 100 only if R1 should be slower. Playtest call.
4. **ShardWalker cyan telegraph → amber** — tiny code touch (cyan reservation).
