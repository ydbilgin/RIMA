# Boona Tweet Analysis Package — for 3-Agent Review
**Source:** https://x.com/boona11/status/2056135521064067475 (2026-05-17)
**Author:** Ibrahim Boona (@boona11) — solo dev, "Isle Builder" 2D top-down island builder
**Engagement:** 168 likes, 10 reposts, 15 comments, ~110sn video
**Tech stack reveal (from replies):** plain HTML/JS, no engine

---

## RIMA Context (why we care)

RIMA = 2D top-down roguelite (Hades/Diablo aesthetic, 30-35° angled top-down, 64px chibi).

Current map (v15c 8-layer painted, `PlayableRoom_combat_v15c_8layer.png`, 375 painted cells):
- **Problem:** Purple crystals, blue runes, skulls, walls scattered everywhere with no focal point. Player silhouette (bandit knight bottom-center) drowns in floor noise. Floor is `stone-brick + grass + dirt + crystal-shard` with ~equal weight → no readable hierarchy. 8 layers all competing. Zero negative space.
- Already in memory: `feedback_blueprint_first_map_design` (3-step zone→prop→decal LOCK), `feedback_layered_terrain_mandatory` (3-layer fill rule), `project_room_composer_paint_intent_lock` (semantic brush 3-mode).

The user (yasinderyabilgin) asked: "sade ama doğal görünür mapler istiyorum" + "tweet altındaki cevapları da incele, en son kararı Opus versin".

---

## OP Tweet (verbatim)

> Built this 2D island builder in 2 days.
> And honestly, 2D game dev turned out to be way harder than I expected, maybe even harder than 3D in some ways.
> Along the way I built a crowd system, shader-based terrain, water effects, reefs, ships, and marine life.
> Still not finished, but it's getting there.
> #indiegame #gamedev

---

## Video Analysis (4 keyframes, full 110sn)

Browser app at localhost:8003. UI: BUILD TOOLS sidebar (left) + Terrain/Decor/Props/World/Map/Help panels (right). Editing/playing in real-time.

**Composition principles observed:**

| Principle | Boona | RIMA v15c | Gap |
|---|---|---|---|
| **Negative space ratio** | ~60% water (focal isolation) | ~0% (every cell filled) | CRITICAL |
| **Floor zone count** | 3 tiers (water → sand ring → grass interior) | 4-5 competing tiers (stone+grass+dirt+crystal) | CRITICAL |
| **Color palette size** | ~6-8 distinct colors | 20+ (purples+blues+greens+greys all saturated) | HIGH |
| **Prop clustering** | Trees in groups of 3-5, sparse else | Uniform scatter (every 2-3 cells) | HIGH |
| **Path as composition** | Sand path through grass + wooden bridges | No paths defined | MEDIUM |
| **Value contrast** | Each biome has distinct value (water dark, sand bright) | All biomes muddy mid-value | HIGH |
| **Prop size hierarchy** | Trees big, NPCs medium, flowers small | All props mid-size | MEDIUM |
| **Edge transitions** | Shader-blended smooth + autotile cliff/sand edges | Discrete cell-based decal (Wang 16 partial) | MEDIUM |

---

## Reply Thread — Key Technical Reveals

### Crowd system (asked by @dviolite)
> A bit of both. **Pathfinding (BFS over walkable tiles**, with **decoration blockers like trees/houses respected**) handles the actual movement. On top of that there's **POI-based goal picking** (beaches, paths, clearings, crossings) **with capacities**, **day/night population shifts**, and small [truncated]

### Tile assets (asked by @maybe_im_sam)
> 2D looks simple from the outside, but getting the assets, edges, transitions, and layering to feel right is a whole thing. For this one I used a mix of **hand-cleaned tile assets**, **autotiling rules**, **shader-based terrain blending**, and **a lot of iteration** [truncated]

### Tech stack (asked by @picoito)
> It's a 2D tile-based island builder with **autotiling**, **shader-based terrain blending**, water/wind effects, crowds, and marine life, **all in plain HTML/JS**.

### Other replies
- @maybe_im_sam: "Would love to see more about how you created tile assets — 2D is definitely harder than 3D imo because creating decent graphics is not as straight forward"
- @aanondev: asks if it's "pokemon style view? Is there 45 degree tilt or not?" (Boona didn't answer in fetched batch — angle ambiguous)
- Praise replies (~10): "beautiful", "looks great", "cute and pretty", "speechless so beautiful", "amazing concept hard to pull off in 2 days"

---

## Hypotheses for RIMA — for agents to evaluate

**H1 — Negative space mandate.** RIMA rooms need 15-25% intentionally empty floor cells (no props, no decals — just dominant base tile). This creates breathing room and lets player+enemies pop. Currently 0%.

**H2 — Floor hierarchy lock.** Per-zone floor budget enforced in `BlueprintZoneTypeSO`: 1 dominant tile (~70% cells), 1 secondary (~20%), 1 accent (~10%). Currently 4 floor types at ~25% each = no signal.

**H3 — Prop clustering rule.** Replace uniform-density scatter with cluster-based placement: each "prop cluster" = 3-5 props in a tight group with empty cells around. Total cluster count per room: 3-5 (currently 15+).

**H4 — Palette diet.** Reduce active palette to 8-10 colors per room. Currently each prop comes with its own palette (purple crystal, blue rune, grey wall, green grass, brown dirt) competing. Suggestion: 1 dominant biome palette + 1 accent only.

**H5 — Shader-based biome blending.** Adopt Boona's approach: instead of discrete Wang-16 transition tiles, use a shader that blends two biome textures based on distance-to-zone-edge field. Smoother, fewer assets, harder edges still possible.

**H6 — Path as primary composition.** Every room needs a "main path" zone painted first; props/clusters arrange around path. Acts as visual flow + dash lane.

**H7 — POI capacities for prop placement.** Borrow Boona's POI+capacity model: each "interest point" in a room has a capacity (max N props within radius R). Auto-populator respects capacities → natural clustering.

**H8 — Skip path: pivot to Boona's "2 days HTML/JS" approach.** Reject — RIMA combat + scope much bigger, Unity LOCK stays. But take the composition principles, not the engine.

---

## Decisions needed (Opus to finalize)

1. Which hypotheses (H1-H7) to LOCK now as immediate v15d composition fixes (Codex dispatch tonight)?
2. Which to defer to S90+ for proper architecture work?
3. Which to reject?
4. Specific numbers — e.g. negative space %, hero prop cap, palette cap?
5. Separately: autosprite VFX pilot scope (dash trail + hitspark proposed earlier).

Agents (Codex + Gemini) — give your independent take on H1-H8, then propose concrete v15d action items with numbers. Opus will synthesize.
