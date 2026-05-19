# FINAL VERDICT
APPROVE_WITH_REVISIONS. The 4-5 connected sub-room combat encounter is a strong direction for RIMA, but only if it is defined as one macro Combat/Elite encounter containing internal sub-room beats. The authoring/data side mostly supports this now. The runtime does not yet support the semantics, because current room clear means reward + graph exit. This is an incremental runtime addition, not a generator rewrite, but it needs an EncounterTemplateSO and a small sub-room sequence controller before it becomes a locked Karar.

# 1. Architectural feasibility
Supported now:
- RoomTemplateSO is already a usable sub-room atom: bounds, doorSockets, playerSpawn, enemySpawnSockets, cameraBounds, prefabRef, backgroundLayers, props, walkableGrid, encounterTags, difficultyTags, blockerTags.
- RoomBankSO can pick templates by RoomType and validate all rooms. RoomBankRuntimeTester proves template pick, prefab instantiation, background layer spawn, player/enemy socket usage, props, and exit socket validation.
- DoorSocket and GateSocket already express directional connection points. GateSocket is better suited for authored internal links because it does not directly call graph navigation.
- RoomTransitionFX already implements screen-space fade-to-black with a callback while black. MVP does not need a new camera fade shader.
- BlueprintCanvas, RoomBlueprintSO, RoomSaveLoadService, and AutoPopulator support authored small-room composition, layered decoration, save/load, adjacency dressing, composition budgets, and smaller grid sizes.
- DungeonGraph can remain macro-level. A Combat node can own an encounter sequence instead of one arena.

Missing:
- EncounterTemplateSO: ordered 3-5 RoomTemplateSO refs, entry index, final reward index, internal link metadata, encounter-level seed, threat budget, and per-sub-room allocation rules.
- SubRoomSequenceController/EncounterController: load sub-room N, perform internal fade, move to N+1 without DungeonGraph.Navigate, track currentSubRoomIndex, and emit reward only after final clear.
- Internal door trigger semantics. Current DoorTrigger calls LegacyRuntimeRoomManager.OnPlayerEnteredDoor, which navigates DungeonGraph and starts the next macro room. Reusing it directly would be wrong.
- Reward gate refactor. LegacyRuntimeRoomManager.RoomClearedSequence currently spawns reward/map fragment for every Combat/Elite clear. Sub-room clears must suppress that until encounter final clear.
- Camera bounds refresh. CameraFollow reads floor bounds on Start only; sub-room swaps need a public SetBounds/RefreshBounds path.
- Encounter-level validation: all internal links resolve, entry/final sub-room valid, no orphan exit, final reward placement valid, enemy sockets clear of props, and every sub-room has safe floor/camera bounds.

# 2. Combat design integration
NotebookLM canonical says RoomTemplate should expose sockets/tags only, while enemy identity belongs to a separate EncounterBank-style system. It also confirms Act 1 Combat uses 8-12 threat, Elite 14-18, wave 1 consumes about 40 percent of the budget, wave 2 triggers when wave 1 is 50 percent dead, same enemy type max 4, and reward sequence happens after combat clear. It also corrects the prompt: Karar #80 is Class Silhouette Bible, not room mechanics.

The sub-room idea is compatible if it distributes one encounter budget spatially. It should not replace the wave/threat model with fully independent mini-rooms. Current runtime is behind canon: LegacyRuntimeRoomManager scales raw enemy count by room index and picks random prefabs, not threat-budget compositions. Sub-room work should introduce a thin EncounterBudget allocator, not hardcoded enemy identities in RoomTemplateSO.

Reward placement is compatible only if reward is encounter-final. It conflicts with current implementation unless RoomClearedSequence is guarded by "is final sub-room". RewardPickup currently opens doors after draft; in this feature it must open macro exits only after the final sub-room reward.

# 3. Procedural map impact
DungeonGraph is currently node/room-type based. For this feature, treat its Combat/Elite nodes as encounter-level nodes. CurrentNode, route reveal, map fragment reveal, and Navigate stay macro-level. A graph node resolves to EncounterTemplateSO, then the encounter controller resolves sub-room templates.

The invasive area is LegacyRuntimeRoomManager, not DungeonGraph. StartRoom currently increments currentRoomIndex, paints one room, spawns enemies by room type, and clear opens graph exits. Sub-room sequence should keep currentRoomIndex and DungeonGraph.CurrentNode stable while internally swapping templates. Phase 1 generation does not need a major rewrite.

# 4. Mob/reward design
Different mob pressure per sub-room is good if it is curated from one macro budget: warmup, flank pressure, elite pocket, final mix. It becomes balance hell if each sub-room independently rolls a full encounter.

Final-only reward is correct. Fast-clear abuse is controlled by locking the onward internal gate until local required enemies/objective are cleared. Do not give sub-room map fragments, skill drafts, Echo Imprints, or progression-critical side rewards.

Death Imprint Cascade benefits only if the saved death signature records encounterId, graph node, subRoomIndex, sub-room tags, mob composition tags, and local context. "Died in Combat node 6" is weak. "Died in node 6, sub-room 3, flank_pocket, bruiser_pressure, low_light" is useful.

# 5. 3 open questions - Codex recommendations
Sub-room size: default 16x10 for combat sub-rooms. Use 12x8 only for connectors, low-threat pockets, ambush beats, or non-combat transition rooms. RIMA needs dash lanes, 64px silhouette readability, and edge/flank spawn pockets; 12x8 is too tight as the default.

Transition type: use fast fade-to-black for MVP. It matches the user intent, the code already has RoomTransitionFX, and it avoids camera-pan/collider complexity. Door + camera pan is polish. Seamless transition should be rejected for this feature because it recreates the large connected-map composition problem.

Mob spawn timing: hybrid. Author sockets in each RoomTemplateSO, then spawn actual enemies on sub-room entry from the encounter budget. Do not pre-instantiate enemies in hidden rooms. Do not fully random-spawn without sockets.

# 6. Production cost re-estimate
The orchestrator's one-week MVP is realistic only for a thin vertical slice, not full production hardening.

- Fade transition reuse: 0.5-1 hour; 3-5 hours with input lock polish and tests.
- EncounterTemplateSO + validator: 4-6 hours.
- SubRoomSequenceController: 10-16 hours if kept separate; 18-28 hours if LegacyRuntimeRoomManager is deeply refactored.
- Internal gate trigger path: 3-5 hours.
- Reward/map fragment gating changes: 3-5 hours.
- Camera bounds/player spawn handoff: 3-6 hours.
- Threat budget/sub-room allocator MVP: 6-10 hours because current code does not implement canonical threat waves.
- Five usable sub-room templates: 10-16 hours minimum.
- Playtest: 1 day smoke validation; 2 days for balance confidence.

Realistic MVP: 5-7 focused engineering days plus 1-2 design/art days. If canonical EncounterBank/threat waves are included, estimate 7-10 working days.

# 7. Risks + blockers
- Save/load mid-encounter is the largest blocker. Need encounterId, graph node, subRoomIndex, cleared sub-room states, RNG seed, player spawn target, active enemies/rewards, and macro graph state. If save/load is out of scope, MVP must explicitly restart the encounter or disallow quit-resume inside an encounter.
- Current DoorTrigger advances DungeonGraph too early for internal doors.
- Current reward flow is room-clear semantics, not encounter-final semantics.
- CameraFollow lacks runtime bounds refresh.
- Pixel Perfect Camera should be fine with screen-space overlay fade, but must be checked on target resolutions.
- 2D Light2D can pop between sub-rooms; MVP can swap while black, polish needs lighting presets/fades.
- Branch E 6 degree camera tilt can make doorway alignment and bounds feel off; validate with tilt enabled.
- RoomBankRuntimeTester is not enough QA for encounters because it spawns only the first enemy socket and only checks one exit.
- Karar #143 edge-biased decoration can compete with edge spawn pockets in small rooms. Add spawn avoid masks/role masks.

# 8. Karar #149 draft
Karar #149 - Combat Encounter Sub-Room Sequence: A Combat or Elite macro node may be implemented as one EncounterTemplateSO containing 3-5 connected RoomTemplateSO sub-rooms. Internal sub-room transitions do not advance DungeonGraph. Sub-rooms give no individual skill draft, map fragment, Echo Imprint, or macro-route reward. The macro reward sequence occurs only after the final required sub-room clear. Enemy identity and budget remain in EncounterBank/threat-budget logic; RoomTemplateSO stores sockets, tags, masks, props, walkable data, and visual data. Default combat sub-room size is 16x10; 12x8 is allowed only for connectors or low-threat pockets. MVP transition is fast fade-to-black via RoomTransitionFX.

# 9. Conflict check
- Karar #25 Meta progression: preserved. Sub-room rewards must not add permanent hub/meta progression.
- Karar #27 Echo Imprint: preserved if cadence counts macro Combat encounters only, not sub-rooms.
- Karar #80 Class Silhouette Bible: preserved. Smaller rooms must keep 64px class readability and avoid clutter over combat sprites.
- Karar #143 6-stage map architecture: preserved, but needs stricter spawn-pocket masks and clear center/edge discipline in small rooms.
- Karar #147 Multi-Layer Painter: preserved. BackgroundLayerData is useful for separate visual composition per sub-room.

# 10. Morning action item
Write a one-page technical spec for EncounterTemplateSO + SubRoomSequenceController before implementation. It must define fields, state transitions, who calls DungeonGraph.Navigate, who calls reward/map fragment/draft, how internal gates unlock, and how currentSubRoomIndex is saved or intentionally reset. Then build one vertical slice: one Combat node with 3 sub-rooms, fade internal transitions, no sub-room rewards, final reward only.
