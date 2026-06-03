# RIMA — Reward System + Portal Decision (2026-06-02)

> Inputs: NLM canon + ax Gemini 3.1 Pro (full design) + ax Gemini 3.5 Flash (adversarial second-lens) + Opus synthesis. T2 + T3 of `QUEUE_CLIFF_REWARD_PORTAL_2026-06-02.md`. **Docs only** — code wiring follows separately (portal = cx; reward MVP = cx after this locks).

## Canon (NLM, authoritative)
- Reward primitives = **Skill Draft** (3-choice skill offer; `SkillOfferGenerator` exists) + **Map Fragment**.
- Meta currencies = **Echo** (Cartographer → map upgrades) · **Boss Fragment** (Vrel → craft/augment).
- Hub NPCs: Ferryman (lore), Vrel (craft), Sister Mourne (HP/heal), Cartographer (map).
- Door→Portal locked. Phased: P1 single portal→Skill Draft · P2 3-portal fan (FanLayoutSolver, per-portal room-type) · P3 portal+preview islands+cyan-orb travel.

## Where the two AIs DISAGREED (and my call)
| Question | 3.1 Pro | 3.5 Flash | **OPUS DECISION** |
|---|---|---|---|
| Skill Draft cadence | every room | elites/bosses only (fatigue + creep) | **Every COMBAT room, rarity-scaled by room type.** Skill Draft IS the canon primitive → keep it central; fatigue/creep = a TUNING problem (note as balance risk), not a reason to gut the loop. Normal=common-weighted, Elite=rare, Boss=epic. |
| Reward timing | on portal-enter | at room CENTER on clear (safe test space) | **At room center on clear** (3.5 wins — lets player test the new skill before committing to a portal; better feel). |
| MVP scope | full P1 (relic+interact+portal+popup) | leanest (cut relic/fragments/NPC) | **Lean MVP** (3.5 wins — see MVP below). |
| "Relics" | new room reward | not in canon | **Fold relics INTO the Skill Draft pool** (passive-skill entries). No separate relic system. |
| NPC anchoring | Vrel/Cartographer only | add Mourne's Tear (HP) + Ferryman's Ledger (lore) | **Adopt** — treasure room heal = "Mourne's Tear" (HP upgrade); rare "Ferryman's Ledger" lore drop. Anchors all 4 NPCs. |

## LOCKED REWARD DESIGN

### What the player carries OUT of a run (→ hub)
- **Echo** — soft currency. Drops from enemies + breakables. Pickup = small magnet radius + short vacuum delay (NOT instant auto-collect — keep it tactile, 3.5's point). → Cartographer: meta-map upgrades (new path types, elite nodes, start HP, +reroll).
- **Map Fragment** — from Elite + Treasure rooms. → Cartographer: unlock new Acts/biomes + run modifiers.
- **Boss Fragment** — Boss only. → Vrel: permanent class craft/augment (base stats / starting gear).

### In-run rewards (cadence)
| Room | Echo | Skill Draft | Other |
|---|---|---|---|
| Normal (combat) | yes | yes (common-weighted) | — |
| Elite | 2× | yes (rare-weighted) | Map Fragment |
| Treasure (no combat) | burst | — | destructible crystal → **Mourne's Tear** (HP) or Echo burst; rare **Ferryman's Ledger** (lore) |
| Boss | big | yes (epic/max-tier) | Boss Fragment |

### Skill Draft mechanics
- 3 choices via `SkillOfferGenerator`, presented at **room center on clear**.
- Rarity weight scales by room type (base 70 common / 25 rare / 5 epic; elite/boss shift toward rare/epic).
- **No banking** (must pick → keeps momentum). **1 reroll/run** (Cartographer-upgradeable). Relics live inside this pool as passive entries.
- ⚠️ BALANCE RISK (3.5): every-room drafts can power-creep by room ~4 → tune skill power + offer minor/major split; revisit after first playtest.

### MVP — wire THIS week (leanest loop that still feels like a reward)
`clear last enemy → Skill Draft 3-choice popup at room center → pick → portal (single, vertical) becomes active at island edge → enter → load next room.`
DEFER for MVP: Echo currency + magnet, Map/Boss Fragments, hub NPC data, treasure/elite room types, reroll, rarity weights. (Existing relic-pickup may stay as a visual placeholder only.)
Build order after MVP: Echo + pickup feel → room types (elite/treasure) + Map Fragment → 3-portal fan (P2) → hub NPC wiring → P3 preview/orb.

## LOCKED PORTAL DECISION

### **Portal = VERTICAL** (both AIs agree; locked)
Upright cyan rift at the island edge, plane ~facing camera, player walks INTO it. Reasons: flat horizontal discs foreshorten to thin ellipses + hide under the player sprite at this 3/4 iso angle; a vertical tear reads as a clean silhouette against the purple void, sells the "rift torn in the void" fantasy, and supports the "portals rise/grow upward from the edge" room-clear reveal + the 3-portal fan-along-the-edge layout.

### Art / Unity spec (for the later together-session PixelLab gen)
- Canvas ~**64×128** (tall billboard). Single front-facing sprite — **no 8-dir art** needed.
- **Pivot bottom-center (0.5, 0)** → grows upward rooted to the ground edge during spawn anim.
- **Sorting:** Y-sort by bottom pivot (player sorts in front as they approach, clips in on enter).
- **Idle:** 6–8 frame cyan shimmer loop.
- **Room-type icon (P2 fan):** separate child SpriteRenderer floating centered in the rift, slow bob.
- ⚠️ PixelLab generation = gated (together-with-user session). cx production NOW uses placeholder/existing portal art + the existing `Portal.cs` / `PortalSpawnController.cs` / `FanLayoutSolver.cs` / `RoomTypeData.cs` code.

## Next actions
- T3 cx: replace door/DoorTrigger flow with the existing vertical-Portal system wired into the 3 live scenes (MVP single portal at island edge → on enter, MapFlowManager next map). Inspect existing portal scripts first → precise cx task.
- T2 cx (after T3 or parallel): wire MVP reward loop (Skill Draft popup at room center on clear).
