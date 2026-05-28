# RIMA Progression Plan v2 FINAL

**Status:** v2 written, NLM-canonical-reconciled, LOCK candidate.
**Basis:** `STAGING/PROGRESSION_PLAN_v1_CODEX.md` plus inline NLM canonical block in `CODEX_TASK_laurethgame.md`.
**Local canonical lock:** `MEMORY/_temp_canonical_lock.md` was absent. Inline NLM canonical block used as authoritative source.
**Rule:** Karar #60/#61/#62/#63 override v1 wherever they conflict.

## v1 Contradictions Resolved

| # | v1 claim | v2 canonical correction |
|---:|---|---|
| 1 | Rune system, 3 MVP slots | Deleted. Skill Draft replaces rune because Karar #60 is NO RUNE. |
| 2 | Skill Rune reward/drop | Deleted from reward catalog and Sheet 3 use. |
| 3 | Boss Key as separate economy | Deleted. Boss gate is 8 fragments because Karar #63. |
| 4 | Echo Essence Orb as run currency | Renamed/reclassified to Shattered Echoes meta-currency. |
| 5 | Health Orb immediate sustain drop | Deleted. HP comes from Elite 12%, Shop, Boss 50%; no floor drop. |
| 6 | Curse Stone pickup/drop | Reclassified as Curse Gate burden/gift UI element, not a drop. |
| 7 | Memory Shard meta currency | Reassigned visually to Component: Rift Stone. |
| 8 | 8-drop canonical set | Replaced with canonical reward catalog: Map Fragment, Gold, Shards, 7 Components, 9 Combined Items, Relics, Skill Draft, Shattered Echoes meta. |
| 9 | 9 room variants including Corridor | Corridor deleted. Act 1 uses 8 node types from Karar #62. |
| 10 | Chest room type | Deleted as Act 1 node type. Rewards come from canonical nodes/events. |
| 11 | Forge room type | Deleted as Act 1 node type. No rune forge loop in v2. |
| 12 | Merchant naming | Renamed to Shop because Karar #62. |
| 13 | Event naming | Renamed to Mystery where it is the Act 1 branch node. |
| 14 | 5 fragments reveal next route band | Replaced by Karar #63 reveal odds: 1 node 65%, 2 nodes 30%, 3 nodes 5%, open node +1 hop. |
| 15 | Fragment can come from Chest/Curse/Event | Replaced by Karar #63: Combat guaranteed, Elite guaranteed, Mystery chance, Rest/Shop/Curse Gate none. |
| 16 | Boss clear/key progression | Replaced by 8-fragment boss gate: 6 Combat + 2 Elite mandatory; branch fragments are bonus and do not count. |
| 17 | Echo Imprint Cascade as locked death behavior | Renamed to Death Imprint proposal status; spec gate pending. |
| 18 | Missing Kirik Tas Tablet map UI | Added TAB MapPanel, 128x128 minimap, rusty gold frame, cyan rift cracks, four-act evolution. |

## 1. Current Visual Inventory And Canonical Roles

| Compact sheet | Existing role | v2 canonical role |
|---|---|---|
| `01_threshold_lineup.png` | Threshold lineup concepts. v1 favored A2 floor scar/rift and C1 compass overlay. | Source for one canonical rift threshold family. Use for Entry/Combat/Elite/Rest/Shop/Curse Gate/Mystery/Boss overlays, not as proof of extra room types. |
| `02_hades_style_reward_doors.png` | Reward door concepts. | Reference for Karar #61 discrete room readability only. Do not create a reward-door economy. |
| `03_reward_drops_gallery.png` | 8 reward visuals: Echo Orb, Memory Shard, Gold Pile, Skill Rune, Health Orb, Map Fragment, Curse Stone, Boss Key. | Remap only the canonical-compatible visuals. Discard Skill Rune, Health Orb, Boss Key. |
| `04_map_progression_marks.png` | Map/progression marks. | Source for Karar #63 Map Fragment pickup, Kirik Tas Tablet map marks, cyan rift cracks, and reveal feedback. |

Canonical role summary:
- Threshold language exists to support Karar #61: Hades-style discrete room flow plus StS macro graph hybrid.
- Map UI must read as Kirik Tas Tablet: abstract grid, rusty gold frame, cyan rift cracks.
- TAB opens the StS-style full-screen MapPanel.
- Top-left MiniMap is Hades-style 128x128.
- Act visual evolution is mandatory: Act 1 castle carvings, Act 2 veined flesh, Act 3 floating pieces, Act 4 mirror.

## 2. Act 1 15-Node Mapping

Karar #62 locks Act 1 as 15 nodes: 13 main + 2 branch, fixed topology, random content. Karar #29 8-9 room references are stale.

| Node id | Type | Count role | Threshold visual | Drop/reward rule |
|---|---|---:|---|---|
| N00 | Entry | 1 | Dormant cyan rift threshold, tutorial-safe marker. | No threat, no drop. |
| N01 | Combat | 1/6 | Cyan rift threshold, low combat pulse. | Guaranteed Map Fragment after macro encounter clear. Skill Draft after fragment pickup. |
| N02 | Combat | 2/6 | Cyan rift threshold, low combat pulse. | Guaranteed Map Fragment after macro encounter clear. Skill Draft after fragment pickup. |
| N03 | Elite | 1/2 | Cyan rift threshold plus heavy elite sigil. | Guaranteed Map Fragment after macro encounter clear. Elite HP chance 12%, no Health Orb drop. |
| N04 | Rest | 1/2 | Quiet rift threshold, transit seal. | No fragment. F1 to F2 transit. |
| N05 | Combat | 3/6 | Cyan rift threshold, low combat pulse. | Guaranteed Map Fragment after macro encounter clear. Skill Draft after fragment pickup. |
| N06 | Shop | 1 | Warm safe NPC/shop sign overlay. | No fragment. Gold spend, HP/shop service allowed. |
| N07 | Combat | 4/6 | Cyan rift threshold, low combat pulse. | Guaranteed Map Fragment after macro encounter clear. Skill Draft after fragment pickup. |
| N08 | Elite | 2/2 | Cyan rift threshold plus heavy elite sigil. | Guaranteed Map Fragment after macro encounter clear. Elite HP chance 12%, no Health Orb drop. |
| N09 | Rest | 2/2 | Quiet rift threshold, transit seal. | No fragment. F2 to F3 transit. |
| N10 | Combat | 5/6 | Cyan rift threshold, low combat pulse. | Guaranteed Map Fragment after macro encounter clear. Skill Draft after fragment pickup. |
| N11 | Combat | 6/6 | Cyan rift threshold, low combat pulse. | Guaranteed Map Fragment after macro encounter clear. Skill Draft after fragment pickup. |
| N12 | Boss | 1 | Sealed boss rift, 8-fragment gate UI. | Opens only after 8 mandatory fragments: 6 Combat + 2 Elite. Boss reward: Relic plus Boss HP chance 50%. |
| B01 | Curse Gate | 1 branch | Red-black burden/gift rift overlay. | No fragment. Risk/reward branch only. Curse Stone visual can be UI, not pickup drop. |
| B02 | Mystery | 1 branch | Asymmetric event rift overlay. | Chance-based Map Fragment. Branch fragment is bonus and does not count toward boss gate quota. Rare Event can award Relic. |

Threat budgets:
- Entry: tutorial, no threat.
- Combat: 8-12 budget.
- Elite: 14-18 budget, 1+ Elite mob.
- Rest/Shop/Curse Gate/Mystery/Boss follow Karar #62 node identity.

## 3. Map Fragment Progression

Karar #63 exact rules:

| Node type | Fragment rule | Boss quota? |
|---|---|---|
| Combat | Guaranteed. | Yes, 6 mandatory Combat fragments. |
| Elite | Guaranteed. | Yes, 2 mandatory Elite fragments. |
| Mystery | Chance when node is open. | No, branch bonus only. |
| Rest | None. | No. |
| Shop | None. | No. |
| Curse Gate | None. | No. |
| Boss | Gate consumes/checks 8 mandatory fragments as progression state. | N/A. |

Pickup flow:
1. Macro encounter clear.
2. Map Fragment spawns only if the node type allows it.
3. Player must press `G` to pick it up.
4. Fragment uses cyan glow and bobbing.
5. Pickup triggers reveal roll.
6. Room clear then fragment pickup then 3-choice Skill Draft.

Reveal:
- 1 node reveal: 65%.
- 2 node reveal: 30%.
- 3 node reveal: 5%.
- Open node bonus: +1 hop.

Map UI:
- Name: Kirik Tas Tablet.
- Layout: abstract grid, rusty gold frame, cyan rift cracks.
- TAB opens full-screen MapPanel in StS style.
- Top-left MiniMap is Hades-style 128x128.
- Act evolution: Act 1 castle carvings, Act 2 veined flesh, Act 3 floating pieces, Act 4 mirror.

## 4. Reward Catalog

Canonical reward catalog only:

| Reward | Status | Source/use |
|---|---|---|
| Map Fragment | Locked. | Combat guaranteed, Elite guaranteed, Mystery chance. Boss gate requires 8 mandatory fragments. |
| Gold | Locked, 5-75. | Economy reward and Shop spend. |
| Shards | Phase 2+, 1-5/enemy. | Enemy-linked future reward. Do not treat as Memory Shard. |
| Iron Shard | Locked Component 1/7. | Component reward icon required. |
| Void Fragment | Locked Component 2/7. | Component reward icon required. |
| Chain Links | Locked Component 3/7. | Component reward icon required. |
| Shadow Veil | Locked Component 4/7. | Component reward icon required. |
| Blood Gem | Locked Component 5/7. | Component reward icon required. |
| Rift Stone | Locked Component 6/7. | Use repurposed Memory Shard visual from Sheet 3. |
| Soul Ember | Locked Component 7/7. | Component reward icon required. |
| Combined Items C01-C09 | Locked category, 9 recipe items. | Generate 9 icons. Names/recipes are not enumerated in this task block; reserve 9 canonical slots without inventing names. |
| Relics | Locked. | Boss + rare Event/Mystery only. |
| Skill Draft | Locked. | 3-choice room reward flow after fragment pickup. |
| Shattered Echoes | Locked meta-currency name. | Meta-currency. Reuses Echo Orb visual after rename. Not "Echo Essence". |

Explicit deletions:
- No Rune reward.
- No Skill Rune.
- No Health Orb drop.
- No Boss Key item.
- No Echo Essence name.

## 5. Skill Draft And Echo Imprint Integration

Karar #60 locks Skill Draft and rejects rune.

Flow:
1. Room clear.
2. Fragment pickup if the node produces one.
3. 3-choice Skill Draft appears.
4. Choices are: new skill, tier upgrade, Echo Imprint.

Skill tiers:
- Common to Rare to Epic to Legendary.
- Max 3 upgrade slots per skill.
- Resource-free.
- Replace on full.

Echo Imprint:
- Separate parallel progression lane.
- 4 per run max, 3 standard.
- Covers Strike Forms, Outlet Forms, and Surge Forms.
- It is a draft choice, not a rune socket, rune item, or modifier drop.

Implementation implication:
- Any future code spec should model Skill Draft as the room reward surface.
- Do not implement rune slots, rune definitions, rune drops, or rune UI.

## 6. Death Imprint

Death Imprint is a proposal, not a locked system.

Canonical status:
- Old name "Echo Imprint Cascade" becomes Death Imprint.
- Top candidate.
- Not locked.
- Spec gate pending.

Required record fields if approved:
- `encounterId`
- `subRoomIndex`
- `subRoomTag`
- mob composition
- environment context

Cadence:
- Per macro encounter.

v2 rule:
- Do not ship Death Imprint as locked progression behavior.
- Do not attach fragment loss to death.
- Treat Death Imprint as a separate future spec gate after the orchestrator approves the proposal.

## 7. Sheet 3 Reward Visual Repurpose Map

| Sheet 3 Visual | Canonical Reward Mapping | Action |
|---|---|---|
| Echo Orb | Shattered Echoes meta-currency | RENAME + reuse. |
| Memory Shard | Component: Rift Stone | REASSIGN. It reads as cyan crystal and fits Rift Stone. |
| Gold Pile | Gold | KEEP. |
| Skill Rune | None | DISCARD. No rune system. |
| Health Orb | None | DISCARD. No Health Orb drop. |
| Map Fragment | Map Fragment | KEEP. |
| Curse Stone | Curse Gate burden/gift UI | REPURPOSE as UI element, not a drop. |
| Boss Key | None | DISCARD. Boss gate uses 8-fragment rule. |

Missing generation:
- 7 Components icon set: 6 missing because Rift Stone is covered by Memory Shard repurpose. Generate Iron Shard, Void Fragment, Chain Links, Shadow Veil, Blood Gem, Soul Ember.
- 9 Combined Item icons.
- Relic icon.
- Skill Draft 3-choice UI screen.

## 8. Production Cost

| Work item | Gen budget | Codex implementation | Shader/UI work |
|---|---:|---|---|
| Threshold base and overlays | 0-1 batch | None now. Later config table for 8 node types. | Universal rift threshold material states: dormant, active, elite, safe, curse, mystery, boss sealed. |
| Sheet 3 repurpose | 0 batch | Rename/remap data only. | Icon extraction and atlas cleanup. |
| Components | 1 batch | Reward enum/data entries later. | 6 new icons, plus repurposed Rift Stone. |
| Combined Items | 1 batch | Recipe data pending canonical names/recipes. | 9 new icons. |
| Relic icon | 0-1 batch | Relic reward source rules later. | Boss/rare Mystery reward presentation. |
| Skill Draft UI | 1 UI pass | Draft controller/data integration later. | 3-choice full reward panel. |
| Kirik Tas Tablet MapPanel | 1 UI pass | Fragment reveal logic and graph state later. | TAB full-screen map, 128x128 minimap, act evolution skins. |
| Death Imprint | 0 now | Blocked by spec gate. | No production until proposal lock. |

Minimum production total:
- 2 icon batches: Components + Combined Items.
- 2 UI passes: Skill Draft + Kirik Tas Tablet.
- Optional 1 threshold/relic batch only if extraction fails or relic needs bespoke visual.

## 9. Next-Step Dispatch

1. **Design lock review:** Orchestrator confirms v2 as LOCK candidate. Dependency: none.
2. **Asset extraction/remap:** Extract Sheet 3 kept/reassigned visuals and discard forbidden visuals. Dependency: v2 lock review.
3. **Asset generation:** Generate missing 6 Component icons, 9 Combined Item icons, Relic icon, Skill Draft UI screen. Dependency: extraction/remap list accepted.
4. **Map/UI spec:** Write Kirik Tas Tablet UI implementation spec with TAB MapPanel, 128x128 MiniMap, reveal odds, +1 hop open-node bonus, and four-act visual evolution. Dependency: v2 lock review.
5. **Code implementation spec:** Define reward flow data: room clear -> fragment spawn -> `G` pickup -> reveal -> Skill Draft; enforce boss gate 8 mandatory fragments; exclude rune, boss key, health orb, and Echo Essence. Dependency: map/UI spec and asset remap accepted.

## Final Decision

Progression Plan v2 is reconciled to NLM canonical. Act 1 uses Karar #61 discrete room plus macro graph structure, Karar #62 15-node topology, Karar #63 Map Fragment rules, and Karar #60 Skill Draft with Echo Imprint. Rune, Boss Key, Health Orb drop, Echo Essence naming, Corridor, Chest, and Forge are removed from the locked plan.
