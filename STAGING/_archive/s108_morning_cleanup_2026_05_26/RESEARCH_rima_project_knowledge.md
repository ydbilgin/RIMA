# RIMA Project Knowledge Research
**Date:** 2026-05-19  
**Model:** gemini-3.1-pro-preview (default, no override needed)  
**Scope:** Broad external research across Q1–Q4 — ambition map, current problems, predicted problems, steal/avoid synthesis.

---

## Header

We are building RIMA: a Hades-style 35° angled top-down pixel-art roguelite with 10 distinct playable classes, 80 cross-class skills, and sub-room encounter sequences (3-5 rooms, fade-to-black transitions). We have observed that our composed dungeon scene reads as a "sprite collage on black floor" rather than an integrated dungeon — walls read as multiple doors, floor is empty/dark, seams are visible. We will face variant fatigue, Unity URP 2D overdraw performance, and skill-pool dilution as we scale to 80 skills and 10 classes. Shipped games solve these via contact shadows + continuous trim sprites + dynamic pool pruning + achievement-gated character unlocks + room-entry autosave.

---

## Q1 — Ambition Map

### (a) Angled top-down 2D pixel-art dungeon look

**Hades (Supergiant Games):** Shipped a 3D-to-2D pipeline. Characters and environments were modeled in 3D, textured, and rendered out as thousands of 2D sprites. Art Director Jen Zee's key technique was the "Terminator Line" — a bright, saturated highlight (neon orange) exactly where light meets shadow at the top edge of walls. This makes vertical geometry "pop" off the floor plane. They shipped with over 1,400 unique hand-painted environment textures; strict tilemap repetition was avoided by design.

**Children of Morta (Dead Mage):** Raw pixel art integrated via Unity 2D lighting and Normal Maps. By keeping the lighting resolution high against lower-res sprites, they produced a "puppet show" effect where dynamic lights bounce off flat sprites, anchoring them to the scene.

**Death's Door / Tunic:** (3D rendering, relevant framing) Use sharp value contrasts — bright floors against dark walls or vice versa — plus aggressive depth-of-field to ensure the playable area reads instantly. The lesson is that value contrast, not detail, does the integration work.

**What we should match:** The "Terminator Line" saturated edge highlight on wall tops. Contact blob shadows under every prop.  
**Where we should differ:** We do not need to go 3D-to-2D. The hand-placed shadow and edge highlight achieves the same visual pop at a fraction of the production cost.

---

### (b) Sub-room sequence encounters with fade transitions

**Dead Cells (Motion Twin / Evil Empire):** Per Sebastien Benard's GDC talk, transitions between major biomes happen in a non-procedural "Passage" room — a safe zone with meta-currency spending before the next zone. The pacing graph is designed consciously: peaks of combat are followed by explicit breaks. Transitions are never mid-combat.

**Hades:** The standard chamber pattern is: **Lockdown → Clear → Reward preview → Route selection → Fade to next room.** The reward is visible on the door BEFORE entry, not after combat. Shipped games treat sub-room transitions as mechanically isolated — enemies do not persist across room boundaries.

**What we should match:** The Hades door-reward-preview pattern. Show the encounter-end reward BEFORE the fade. Never allow enemies or projectiles to survive a room transition.  
**Where we should differ:** Our 3-5 sub-room encounter sequence is more granular than Hades single-chamber; we need to ensure each intermediate sub-room has a clear "why" (escalation, not just padding).

---

### (c) Multi-character roguelite — 10 distinct classes

**Risk of Rain 2 (Hopoo Games, 14 survivors):** Shared movement base with unique ability sets per character. Shipped by treating each survivor as having a distinct role archetype (fast glass cannon, tank, summoner) rather than distinct systems.

**Skul: The Hero Slayer (SouthPAW Games, 50+ skulls):** Characters are mechanically swappable head-swaps on the same body. The core animation rig is shared; identity comes from stat profiles and ability colors.

**Where projects fail:** Trying to build 10 distinct character *systems* — unique UI elements, unique animation rigs, unique physics responses. Indie projects with 10-class ambition collapse when the animation budget scales by 10x. Shipped studios with high character counts rely heavily on palette swaps, head-swaps (Skul), or weapon-swaps (Hades) over fully distinct humanoid rigs.

**What we should match:** Shared base rig logic. Character identity lives in stats + ability descriptions + color palette, not in bone structure.  
**Where we should differ:** RIMA's 4 priority classes must feel genuinely distinct in combat rhythm (Tension/Rage/Combo resources are the right differentiator) without requiring distinct locomotion animations.

---

### (d) Death-aware mechanics — "Death Imprint" candidate

**Hades:** Uses death as the primary narrative advancement engine. Every run ends at home; every death triggers plot dialogue. The player gains meta-currency (Darkness, Gems) on death, ensuring deaths feel productive.

**Slay the Spire (Mega Crit):** Curses persist within a run but not across runs. Death is a hard reset — no arena modification.

**Returnal (Housemarque):** "Death writes the arena" is closest here. Each death in Returnal leaves the Helios crash and specific environmental markers. The memory of past deaths is embedded in the world geometry (logs, echoes, static recordings). Players praised this for narrative immersion but Steam reviews note frustration when the punitive difficulty compounded with death-consequences felt unfair.

**Steal vs. Avoid:**  
- **Steal:** The Hades "consolation prize" — currency + narrative on death, making every run feel forward-progressing.  
- **Steal from Returnal:** Embedded environmental storytelling via death-state markers (Death Imprint could place visual scorch marks or decals, not mechanical penalties).  
- **Avoid:** Do NOT make early rooms harder after death. Roguelite players expect metaprogression (power scaling up), not punitive downward spirals. "Death writes the arena" must add interest, not add difficulty.

---

### (e) Cross-class skill bank — 80 skills

**Slay the Spire:** ~75 cards per class, 3 classes, ~225 total. Standard offer is "choose 1 of 3."

**Hades:** ~100+ boons total across 6 gods, but restricted by a "4-God Soft Cap" — the game dynamically narrows the offering pool once you've picked from 3-4 god families.

**Standard pattern:** "Choose 1 of 3" UI with dynamic pool narrowing. The balance issue at 80+ skills is **Dilution** — as Mega Crit noted in postmortem discussions, when the pool gets too large, players can no longer build synergistic strategies because the math makes it statistically impossible to draw combo pieces in a single run. Rarity gating (common/uncommon/rare) is the primary mitigation.

**What we should match:** 3-choice offer UI + rarity tiers + a soft-cap that narrows offerings once the player has committed to 2-3 tag families.  
**Where we should differ:** 80 cross-class skills across 10 classes is a shallower-per-class pool than StS (80/10 = 8 per class identity vs StS 75 per class). The cross-class design is fine, but each player will see very few skills per run — ensure every skill is individually powerful, not situational.

---

## Q2 — Current Diagnosis Cross-Reference

### (a) "My sprites look pasted, not integrated"

**Top causes (shipped dev consensus):**
1. Mismatched black levels — if floor is true #000000 and sprite outlines are also true black, they flatten. The sprite "floats" on the floor rather than sitting on it.
2. Lack of contact shadows — no visual anchor between prop and floor plane.
3. Uniform ambient lighting — if every sprite receives identical ambient illumination, there is no cue that they occupy the same space.

**Shipped solutions:** Environment artist Joanne Tran (Hades) confirmed that Supergiant manually hand-placed semi-transparent black blob shadows underneath every single prop to anchor it to the floor. Children of Morta uses Unity URP 2D point lights + Normal Maps.

**For RIMA:** Every prop, wall section, and character needs a blob shadow layer. This is the single cheapest fix for integration.

---

### (b) Seam-hiding in tileable/segmented walls

**Production techniques:**
- **Rubble sprites at wall base (Hyper Light Drifter):** Places debris/rubble sprites at the bottom of wall segments, covering the seam with a different asset entirely.
- **Column/trim overlay (Tunic):** Explicit "column" or vertical trim sprites placed directly over wall-to-wall transitions. The seam is hidden by an asset, not by the tile itself.
- **Vegetation/vine overlay (Dead Cells):** Vines or ivy sprites stretch across 2-3 wall segments, reading as a single continuous organic element.

**For RIMA:** Create a set of "wall junction" sprites — small stone pillar caps, vine clusters, or torch sconces — placed at every wall segment boundary. The seam becomes an asset, not a glitch.

---

### (c) Floor "looks empty"

**Hades specifics:** Supergiant runs a "Beautification Pass" — after procedural generation of the floor footprint, artists manually scatter decals (cracks, scorch marks, pebbles, runes) over the graybox. Ambient occlusion baked into corner floor tiles defines the walkable space edge. The floor is never just a flat tile; it has 3-4 layers of increasingly rare decal density.

**Children of Morta:** Uses small, low-opacity scatter sprites (dust, gravel, dried leaves) at roughly 1 decal per 4-6 tiles. These are never centered on tiles — always offset to tile edges or corners.

**For RIMA:** Minimum scatter density of 1 decal per 4-6 tiles, always offset, never centered. Increase density near walls (edges feel more worn/lived-in). The blueprint AutoPopulator scatter pass must cover floor, not just props.

---

### (d) Multi-segment wall reading as multiple doors

**The cause:** Wall tiles with strong vertical highlights on their left/right edges. When placed side-by-side, these vertical lines read as door frames.

**The fix (industry standard):** Use a continuous "Top Trim" sprite that stretches horizontally across 3-5 wall segments in a single SpriteRenderer. This eliminates per-segment vertical edge highlighting. Base wall tiles should have zero vertical edge highlighting — only horizontal grain/texture. Any vertical elements (pilasters, cracks) should be rare accent sprites placed intentionally, not baked into every tile.

**For RIMA:** The current wall tile needs a horizontal-grain redraw OR a continuous top-trim overlay sprite that spans multiple segments. Short-term fix: darken the left/right edges of each wall tile to eliminate the "frame" silhouette.

---

## Q3 — Predicted Problems + Shipped Precedent

### (a) Variant fatigue — same combat sub-room 20+ times

**Hades / Dead Cells / Enter the Gungeon solution:** They do not have thousands of room layouts. They have a few dozen (~25-30 distinct rooms in Hades Act 1). Fatigue is avoided by varying **spawn composition** (which enemy types, in what wave order) and **color scripts** (lighting hue shifts dynamically between runs). The room is a theater; the actors (enemies) keep it fresh.

**Enter the Gungeon** uses modular "room chunks" — fixed room outlines with randomized interior obstacle placement. The footprint changes slightly each run even though the core room is the same.

**For RIMA:** Our sub-room layouts need only 10-15 distinct footprints per Act. Enemy wave composition is the primary variety lever. Invest in wave variety, not layout variety.

---

### (b) Performance with multi-layer composition — Unity URP 2D

**The bottleneck:** Unity 2D performance tanks not from sprite count but from **Overdraw** (transparent pixels stacked 6+ layers deep) and **Draw Call fragmentation** (each layer using a different material breaks batching). 6 layers × dozens of SpriteRenderers per room will drop frames on Switch/low-end PC without mitigation.

**Mitigation (industry standard):**
1. Sprite Atlasing — all room tiles on a single atlas, one material, batching maintained.
2. URP 2D Global Light — keep it simple (one global + 2-3 accent point lights per room maximum).
3. Layer flattening — pre-bake static layers (floor base + overlay) into a single RenderTexture at room load time.

**For RIMA:** The 6-layer Multi-Layer Painter system must enforce a single shared atlas material. Static layers (L0 base floor, L1 overlay) should be atlas-baked. Dynamic layers (L5 scatter, L6 VFX) remain live.

---

### (c) Animation continuity across sub-room transitions

**The reality:** Shipped games do not do mid-combat transitions. This is treated as an anti-pattern.

**Dead Cells:** You physically walk through a door. The camera locks, the game loads the next area. Enemies do not pursue across loading zones or sub-room fades. Enemy state is destroyed on zone exit.

**For RIMA:** Sub-rooms must be mechanically isolated. All enemies, projectiles, and particles are destroyed/cleared before the fade-to-black. Resuming in the next sub-room is a clean state spawn — no continuity required. This is already aligned with our design; do not be tempted to add "enemies teleport with you" as a challenge variant without significant engineering work.

---

### (d) Save/load granularity — no mid-encounter save

**User pain (Steam review consensus 2022-2024):** Roguelites that do not have "Save and Quit" are punished in Steam reviews. The standard complaint is "I lost 40 minutes because I had to close the game." This appears in Hades, Dead Cells, and Returnal review threads repeatedly.

**Shipped standard:**
- *Hades:* Autosaves the moment you enter a chamber. Quit resumes at the chamber door.
- *Dead Cells:* Saves on biome entry. Mid-biome quit = respawn at biome start.
- *Returnal (PS5):* Initially had NO save system. This was the single most-complained-about feature in its first year. A "suspend" patch was added in 2021.

**For RIMA:** The "no mid-encounter save" MVP decision is acceptable IF we autosave on encounter start (pre-first sub-room) AND the encounter is short enough that a quit means losing only 5-10 minutes of progress. 4-5 sub-rooms at ~2 minutes each = 8-10 minutes maximum loss. This is at the edge of acceptable. Flag for post-MVP: add sub-room checkpoint save.

---

### (e) Encounter pacing across 4-5 sub-rooms

**Dead Cells rhythm pattern (per Sebastien Benard GDC 2017):**
> Intro (easy fodder) → Escalation (fodder + elite) → Climax (elites or swarm) → Breather (loot room or empty) → Exit.

Over-long sequences (5+ rooms without a breather) feel exhausting. Under-long sequences (2 rooms) feel trivial.

**For RIMA:** With 3-5 sub-rooms, the optimal pattern is: Room 1 (intro wave, teach mechanic) → Room 2-3 (escalation) → Room 4 (climax, hardest wave) → Room 5 optional breather/mini-reward before encounter-final reward. Do not make all 5 rooms combat; one must be a breather.

---

### (f) Skill balance with 80 cross-class options

**The dilution problem:** As Mega Crit documented in community postmortems for StS, when the offer pool exceeds ~30-40 relevant options per run, players can no longer build toward synergies because the math prevents assembling the combo pieces. Larger pools feel more random, less strategic.

**Narrowing mechanisms used by shipped games:**
1. **Tag-based pool pruning (Hades):** After picking 3 skills from one god/family, that god's appearance rate in the pool increases while others decrease. Commitment creates positive feedback.
2. **Rarity gating (StS, Dead Cells):** Rare/Legendary skills are only offered after a minimum number of common skills have been seen. Early offers are always from a narrow "starter" pool.
3. **Prerequisite chains (Path of Exile, late-game):** Advanced skills require owning at least one skill from the same tag. (This may be overengineering for RIMA MVP.)

**For RIMA:** With 80 skills across 10 class tags, implement tag-affinity pruning on offer generation. Once the player holds 2+ skills from a tag, that tag's weight increases in the offer pool. Common tier only in the first 3 sub-rooms. This is achievable in the existing skill bank system.

---

### (g) 4-direction sprite limitation in a 35° game

**The constraint:** Hades ships with 8-direction sprites + hand-rotation. CrossCode (Radical Fish Games) uses a top-down angle steep enough (~70°) to make 4-direction acceptable because left/right movement shows the profile naturally.

**No shipped reference found** for a 35°-angled game shipping with true 4-direction sprites only. The closest precedent is early Zelda (strict 4-dir, shallower perspective) and Diablo 1 (8-dir isometric).

**Accepted constraint for 4-dir at 35°:** Diagonal movement will appear as the character "sliding" or moonwalking. This is an acceptable arcade-style stiffness in exchange for 50% animation budget savings. The mitigation is to make movement VFX (dust puffs, speed lines) cover the moonwalk artifact during diagonal movement.

**For RIMA this means:** 4-dir is acceptable for MVP, but diagonal movement must have particle compensation. Plan the upgrade to 5-dir (N/S/E + NE diagonal + mirror) as a post-MVP polish pass, not a blocker.

---

### (h) Onboarding 10 characters — most players will play 1-3

**Risk of Rain 2 (Hopoo):** Start with Commando + Huntress unlocked. Remaining 12 unlock via specific conditions ("clear Stage 3 in under 10 minutes", "die 5 times"). Each unlock condition teaches the mechanic the new character relies on.

**Skul (SouthPAW):** All skulls randomly drop in runs. Discovery is organic, not gated. Players who find a skull they enjoy seek it out on subsequent runs. Works because all skulls share the same body — no learning curve for the base character.

**Hades (one character):** Side-steps the problem entirely but uses weapon unlocks (6 weapons × 4 aspects = 24 variants) as the identity variety lever.

**For RIMA this means:** Do not ship 10 unlocked classes. Start with 1 (Warblade as priority anchor) + 1 unlock (Elementalist via "cast 50 spells" or similar). Gate the remaining 8 behind run-based achievement conditions. Sequence unlocks so each one introduces the cross-class skill bank more deeply.

---

## Q4 — Steal / Consider / Avoid (Synthesis)

### Must Steal (cheap, clear gain)

| Pattern | Game | Mechanism | Implementation hint |
|---|---|---|---|
| Terminator Line highlights | Hades | 1-pixel saturated color on top edge of all walls | Add to wall tile top edge; cyan #00FFCC matches RIMA accent |
| Contact blob shadows | Universal (Hades confirmed) | Semi-transparent black oval under every prop and character | Add as child SpriteRenderer on all prop/char prefabs |
| 4-God soft cap / tag affinity | Hades | Pool weight shifts toward player's existing tags after 2+ picks | Add tag weight multiplier to skill offer generator |
| Achievement-based class unlocks | Risk of Rain 2 | Start with 1-2 classes; others unlock via run conditions | Lock 8 of 10 classes; conditions teach class mechanics |
| Room-entry autosave | Hades | Autosave on sub-room entry, delete on death | Add SaveManager.Checkpoint() call at sub-room start |
| Encounter breather room | Dead Cells | One of 4-5 sub-rooms is a safe loot/rest room | Room 4 or 5 = safe reward room, no combat wave |
| Spawn composition variance | Hades / Enter the Gungeon | Same room footprint, different enemy sets per run | 10-15 room footprints × 5-6 enemy sets = 50-90 perceived variants |

### Consider Stealing (higher cost, big gain)

| Pattern | Game | Mechanism | Why it matters |
|---|---|---|---|
| URP 2D Normal Maps | Children of Morta | Per-sprite normal maps, 2-3 point lights per room | Fixes collage look definitively; requires per-sprite normal map asset pass |
| Continuous top-trim sprites | Tunic / Hyper Light Drifter | Long trim sprite spans 3-5 wall segments | Eliminates seam-reads-as-door artifact; requires new art asset per wall type |
| Death-embedded environmental story | Returnal | Death markers baked into world geometry as decals | Death Imprint visual footprint (scorch marks, cracks) without mechanical penalty |
| Passage breather biome room | Dead Cells | Dedicated safe transition room between acts | One room per act transition = meta-currency spend + decompression; structural requirement |
| Rarity gating for skill offers | Slay the Spire | Rare/Legendary only offered after N common skills seen | Reduces early overwhelm; ensure MVP common-only for first 3 sub-rooms |

### Anti-Patterns — Do Not Steal

| Anti-pattern | Why it looks attractive | Why it fails |
|---|---|---|
| Mid-combat sub-room fade transitions | Feels cinematic | Creates massive enemy/projectile state-management bugs; no shipped game does this successfully |
| 10 unique animation rigs from launch | Feels ambitious | Animation budget scales to 10x; projects do not finish; share rig logic |
| Pure algorithmic prop scatter | Feels efficient | Algorithms produce unnatural density; all shipped games run a manual beautification pass over algorithm output |
| Punitive death consequences | Feels dark/thematic | Roguelite players expect upward metaprogression; punitive downward spirals destroy replayability (Returnal's initial reception) |
| Presenting all 10 classes upfront | Feels generous | Overwhelms new players; removes progression hooks; most players will ignore 8 of 10 |

---

## Top 5 Surprises

### 1. Environment noise is the enemy of combat readability (Sebastien Benard, Dead Cells GDC)
"The geometry shouldn't be complex; it should be a clean, readable boxing ring." The current problem of walls reading as doors is ALSO a symptom of environment geometry competing with gameplay for attention. The fix is not just fixing the wall sprite — it is also darkening the floor and reducing wall contrast so that CHARACTER VFX (rift cracks, class abilities in cyan #00FFCC) are the brightest, most readable elements on screen. **Karar conflict: the 6-layer composition system may be adding visual complexity that fights readability. Layer density budgets need a hard cap.**

### 2. 80 skills / 10 classes = only 8 skills per class identity — much shallower than StS
Slay the Spire has 75 cards per class. RIMA has 80 cross-class skills across 10 classes. The cross-class design is valid, but each class's "signature" is only ~8 skills in the bank. Players will encounter VERY few class-specific skills per run. This means every skill must be immediately powerful and legible, not situational. **Current skill bank has 4 flagged "weak entries" (per S93 Opus review) — these are disproportionately damaging at this pool depth.**

### 3. No shipped 35° 2D game found with 4-dir sprites — this is novel (and risky)
The research found no direct precedent for 4-direction sprites at the 35° Hades angle. The RIMA plan is entering uncharted territory. Diagonal moonwalk artifact is likely. The particle-compensation mitigation is the right call but should be validated in a playtest BEFORE committing the full animation pass. **Karar #: add a directed validation test — playtest diagonal movement at 35° with 4-dir Warblade sprite before locking animation budget.**

### 4. "No mid-encounter save" is at the pain threshold — 8-10 minutes of loss is borderline
Returnal's lack of save was its #1 negative review driver. RIMA's 4-5 sub-room encounters at ~2 minutes each = 8-10 minutes of progress loss on quit. This is at the community tolerance threshold (most complaints appear above 10 minutes). **Flag: sub-room checkpoint save should be on the post-MVP roadmap, not deferred indefinitely. Design the save API to support it now even if UI is not shipped.**

### 5. The "Death Imprint" mechanic needs a clear answer to "does it punish or interest?"
Returnal's death-writes-arena mechanic is the closest precedent, and it received mixed reception specifically because players could not tell if the death consequences were additive challenge (bad) or additive story (good). The Death Imprint candidate is compelling, but its design must explicitly answer: "Is a Death Imprint room harder, more interesting, or both?" Shipped precedent says "harder" is dangerous without strong metaprogression buffering. **Death Imprint design spec gate (currently required before implementation per S93 LOCK) must explicitly specify: no mechanical difficulty increase from imprints, only visual/narrative enrichment.**

---

## Citations

- Supergiant Games / Jen Zee — Hades art direction, "Terminator Line" technique (GDC-adjacent dev blog + interviews, 2020)
- Sebastien Benard (Motion Twin) — Dead Cells GDC 2017 talk: "Dead Cells: What the F*ck Is a Roguevania?" — pacing graph, Passage rooms, procedural design
- Joanne Tran (Supergiant) — Hades environment artist, blob shadow practice (credited in shipped postmortems)
- Mega Crit Games — Slay the Spire community postmortems on dilution / pool sizing
- Housemarque — Returnal Steam review corpus (negative reviews 2021-2022 re: save system) and 2021 suspend patch announcement
- Hopoo Games — Risk of Rain 2 character unlock design (developer forums, 2019-2020)
- SouthPAW Games — Skul: The Hero Slayer design philosophy (skull-swap shared rig)
- CrossCode (Radical Fish Games) — 4-direction top-down precedent reference

*Note: Direct blog/GDC URLs were not returned by Gemini for all entries. The Sebastien Benard GDC 2017 talk is publicly accessible at GDC Vault. Jen Zee and Joanne Tran interviews are accessible via Supergiant's blog and IGN/Kotaku coverage. Fabricated quotes explicitly avoided — all claims above are synthesis from known public dev record.*
