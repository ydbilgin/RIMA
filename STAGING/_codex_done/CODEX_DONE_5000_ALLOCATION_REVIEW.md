# Codex Allocation Review

## Hybrid LOCK verdict
KEEP.

Keeping Codex imagegen for tiles is still the right call. The 5000 PixelLab refresh changes capacity, not provider fit. PixelLab's strongest value is coherent character/object identity across views and animation states; that is exactly where RIMA has unresolved production risk: 10 class anchors, state variants, mobs, props, and VFX provider testing. Tiles are currently a composition/readability problem, not a generation-credit problem. The Boona reviews agree v15d needs negative space, floor hierarchy, path protection, and cluster discipline before more tile supply.

I would REVISIT, not SWITCH, under these conditions:

- PixelLab proves materially better at seamless top-down tilesets at RIMA's required camera scale, including edge cases like corners, transitions, walls, decals, and alternate floor weights.
- Codex tile output keeps failing canvas/size/repeatability after the process_tiles workflow is applied.
- Unity import time for Codex tiles becomes higher than PixelLab's total generation plus cleanup time.
- PixelLab can produce consistent biome families cheaply enough that the opportunity cost does not threaten character, mob, and prop completion.
- A locked visual direction demands one provider's style across every asset family and the current hybrid output visibly clashes in-engine.

Until those are true, switching tiles to PixelLab spends the best budget on the lowest-risk category and risks delaying the categories that block game feel.

## Allocation counter-proposal

| Category | Opus gen | Codex gen | Delta | Reasoning |
|---|---:|---:|---:|---|
| Characters: 10 anchors + direction/state iteration | 500 | 900 | +400 | 50 per class is tight if anchors need outfit readability, weapon silhouette, facing, idle/run/attack hooks, and regen for style misses. Characters are identity-critical and still TBD. |
| Mobs: 8 roster + elite/readability variants | 400 | 650 | +250 | Combat needs enemy silhouettes as much as player anchors. Reserve enough for Imp-to-Hulk plus 1-2 readability passes on size/role. |
| Hero props / cluster templates | 300 | 450 | +150 | v15d composition needs authored cluster anchors, not just more scatter props. This is the bridge between map readability and art identity. |
| VFX provider A/B and skill-hit basics | 100 | 250 | +150 | 100 is enough for a token pilot, not enough to test dash trail, hitspark, one class skill, and import/readability failures. |
| Item icons / UI-HUD combat icons | 0 | 300 | +300 | Missing from Opus. Skill offers, pickups, class identity, and HUD readability need a small but real asset budget. |
| Environmental hazards / interactables | 0 | 250 | +250 | With 5000 credits, hazards are now worth exploring because they affect rooms and combat loops, not only visuals. |
| Boss prep / large enemy silhouette exploration | 0 | 350 | +350 | Do not produce full bosses yet, but buy silhouette and direction discovery now to prevent late art-direction shock. |
| Reserve / regen / failed generations | 3700 | 1850 | -1850 | 74% reserve is too conservative after the critical categories are still undefined. Keep a large reserve, but allocate enough to force decisions. |
| Total | 5000 | 5000 | 0 | Keeps strategic buffer while funding missing production categories. |

The main disagreement is reserve size. A reserve should absorb failures after direction-setting work starts; it should not postpone direction-setting work. I would lock about 1850 as reserve and require a mid-point review before spending beyond the first 3150.

## Phase order verdict
REORDER.

Map-fix remains Week 1 priority for Codex/Unity because v15d composition is already diagnosed and code/tooling work can proceed without PixelLab. But PixelLab spending should start with character anchors immediately in parallel, not after map support. The bottleneck is split by workstream:

- Codex workstream: v15d composition first. This fixes the stage where all future assets will be judged.
- PixelLab workstream: class anchors first. Combat, skill fantasy, UI identity, and animation testing all depend on knowing what the player classes look like.
- VFX workstream: start small only after 2-3 character anchors exist, so trails/hitsparks can be judged against real silhouettes.
- Mob workstream: begin after the first character batch locks style, but do not wait until Week 3 if combat prototyping needs readable enemies sooner.

My proposed order:

1. Week 1: v15d composition implementation and metrics, while PixelLab produces 3 priority class anchors.
2. Week 1-2: finish 10 class anchors enough for silhouette lock; do not over-animate before anchors pass in-engine review.
3. Week 2: produce first 3 mobs covering small, ranged, and heavy reads; start hero prop clusters for v15d rooms.
4. Week 2-3: VFX A/B with PixelLab/autosprite/Codex candidates using real character and mob sprites.
5. Week 3+: complete mob roster, item/UI icon pass, hazard prototypes, and boss silhouette prep.

## Missing categories

- Item icons / pickups: 200-300 gen. Needed for rewards, inventory-like choices, class upgrades, and skill offer readability.
- HUD / skill icons: 150-250 gen. The UI state blueprint needs visual language, especially if 10 classes have distinct skill identities.
- Skill VFX beyond dual-pilot: 250-500 gen. Dash trail and hitspark are provider tests, not enough for class skill identity.
- Environmental hazards / interactables: 200-350 gen. Valuable now because hazards can shape combat rooms and Act 1 encounter variety.
- Boss silhouette prep: 250-500 gen. Not full production, but early large-sprite style tests reduce late risk.
- Class portrait / selection bust crops: 150-300 gen. If anchors are locked, generate portrait-ready variants while the style is hot.
- Prop variants by biome/cluster role: 200-400 gen. Cluster templates need focal, support, blocker, and dressing variants.
- Import/test failure reserve by category: 15-25% inside each category, not only one global reserve.

## 5000 stretch suggestions

- Multi-style character bakeoff for the first 3 class anchors: run controlled variants before committing all 10 classes.
- Directional consistency test for one class and one mob: validate front/back/side/SW-facing needs before scaling animation work.
- Large enemy and boss silhouette exploration: not a full boss roster, but enough to lock scale language.
- Environmental hazard kit: spikes, rift cracks, cursed pools, explosive crystals, slow fields, and destructible blockers.
- Item and skill icon language: establish icon shape, border, rarity, class color, and readability at HUD scale.
- Room identity props: produce cluster-defining hero props that let v15d rooms read as authored zones.
- VFX provider ladder: PixelLab/autosprite/Codex comparison for hitspark, dash trail, ground telegraph, projectile, and status aura.

Do not use the refresh to brute-force every deferred system. Use it to derisk visual identity where late changes are expensive.

## Confidence

Hybrid LOCK verdict: HIGH. Current evidence says map quality is blocked by composition rules and Unity placement behavior, not tile provider capacity. PixelLab should be spent where its coherence advantage matters most.

Allocation counter-proposal: MEDIUM-HIGH. The exact numbers will move after first batch failure rates, but Opus' 3700 reserve is too high while anchors, mobs, UI/icons, hazards, and boss prep are unallocated or underfunded.

Phase order verdict: HIGH. v15d should be Week 1 for Codex, but PixelLab should not wait on it. Parallel character-anchor work is the fastest way to reduce combat and identity uncertainty.

Missing categories verdict: MEDIUM. UI/icons, skill VFX, hazards, and boss prep are definitely missing; the rough budgets need validation against actual PixelLab cost per usable output.

5000 stretch verdict: MEDIUM. The refresh enables broader discovery, but only if spend is gated by in-engine review and not scattered across unrelated wish-list assets.
