# RIMA — PORTAL + DIEGETIC PREVIEW + ORB-TRAVEL SYSTEM SPEC (S6)

**Status:** DESIGN-LOCKED (build pending user sign-off on 2 open forks below).
**Sources:** 2-round converged consult — cx (Codex, technical/code hooks) + ax (Gemini, design/reference) + Opus synthesis. User co-designed.
**Replaces:** abstract overhead StS node-map (concept_map_overhead) — user rejected "see everything at once".

---

## 0. THE CORE LOOP (locked)
Room cleared → portals ignite on island edge (count = branch count) → upcoming room(s) appear as REAL iso room-islands floating DARK/STATIC/MOB-LESS in the void → hover a portal = its matching island lights up → enter portal = avatar morphs into a color-matched ORB, zips across void on a curved path, crash-lands on the chosen island which illuminates → camera leads the trip. Total ≈ 800ms. (= ax "Void Bridge Select".)

## 1. LOCKED DECISIONS
- **Doors → PORTALS.** Radially symmetric: 1 rift sprite + center VFX + **floating rune-icon** (color alone is NOT enough — colorblind/visual-bleed). Icon per type: combat=crossed-swords, elite=skull, treasure=chalice, rest, boss. Common accent cyan #00FFCC, center/glow tinted by type. Portal count = `DungeonGraph.CurrentNode.exits`.
- **Travel = morph→orb→teleport.** Separate orb GO (not sprite-swap). <0.8s, camera sweeps AHEAD of the orb. Morph is PROCEDURAL (engine scale/dissolve), never hand-drawn per-orientation.
- **Previews = REAL room-islands in the void, NOT an abstract map, NOT a flat RenderTexture screen.** Dark / static / unlit / **mob-less**; illuminate + scale up only on hover/selection. (User's "mobsuz" instinct = industry standard: Bastion/Hades.)
- **Map-fragment = REPURPOSED, not removed.** Basic 1-step preview ALWAYS FREE (no blind choices). Fragment = "scout/reveal-further" charge → `DungeonGraph.RevealAhead(steps)`. Reveal depth = fragment level (1 frag → 1 node ahead, 2 → 2 nodes of connected branches).
- **Rooms are iso diamonds** (not square). Previews are scaled iso islands (~0.45–0.65).

## 2. BUILD RECIPE (converged cx+ax)

### 2A. Portal (Effort M) — extend `Assets/Scripts/Environment/Portal.cs`
- Add: `DoorDirection graphDirection`, `int targetNodeId`, `RoomType roomType`, `Color portalColor`, child `SpriteRenderer centerSwirl` + rune icon, `Configure(ExitChoice)`.
- Change `PortalSpawnController.SpawnPortals` to source `DungeonGraph.Instance.CurrentNode.exits` (NOT `RoomTypeData.PickPortalCount` random — that makes portals LIE).
- VFX: static rift sprite + child **animated sprite-sheet** swirl + tiny pixel ParticleSystem sparks. **No Shader Graph for demo** (setup cost + fights pixel readability). Rune = panning/rotating + gentle bob.

### 2B. Orb travel (Effort M) — new `PortalTravelDirector` (on Systems/Map obj)
- Owns travel state (not Portal/Gate/RoomLoader). Subscribes `Portal.OnEntered`, double-trigger guard.
- Orb GO: `SpriteRenderer` glow disk + rune, `Light2D`, **`TrailRenderer`** (does most of the visual read), spark `ParticleSystem`, child `CrashFlash`.
- New helper `PlayerTravelVisualState`: disable `PlayerController` **AND `PlayerAttack`** (own InputActions!) + class skill input; cache+restore `Rigidbody2D.simulated`/velocity/colliders; hide renderers under explicit `visualRoot` (NOT `SetActive(false)` on whole player — breaks Health/inventory/state).
- Camera: `CameraFollow.target` already public → retarget to orb (or a lead-target 0.5–1.0u ahead), restore to player after spawn.
- Tween: **coroutine + AnimationCurve, Bezier path** (start=portal, mid=above/forward in void, end=landing). **No DOTween in project** — don't add it. Duration 0.55–0.70s.
- **Don't use the 0.25s black fade as the signature.** Keep `RoomTransitionFX` only as a brief safety mask; signature = 0.08–0.12s cyan/white impact flash at crash.
- ⚠️ Biggest risk = state restoration (player visuals/weapon/VFX spread across children; PlayerAttack has own input). Too-broad disable breaks state; too-narrow leaks input/weapon during orb.

### 2C. Preview islands (Effort M demo / L final) — `RoomPreviewIsland` prefab + `PortalPreviewController`
- **CHOSEN TECH = Option A: low-detail STATIC visual-only prefab** placed beside the island (NOT live RenderTexture — RT reads as a flat TV screen and fails the "real island" fantasy; NOT thumbnail-quad — reads as UI/painting). This directly delivers the user's "mobless real room" goal.
- Prefab = floor/cliff/wall/prop renderers ONLY. NO enemies, NO colliders, NO AI, NO spawners, NO Gate. `PreviewIslandView`: `SetDark/FadeIn/Illuminate/Bind`.
- Mob-less is automatic: preview prefab simply never contains spawn logic (don't instantiate gameplay prefabs).
- Binding: shared `ExitChoice {index, DoorDirection, targetNodeId, RoomType}` assigned to BOTH Portal i and Preview i → cannot drift.
- Lighting: unlit/dark-tinted material + preview `Light2D` OFF until selected. Hover = fade in color-matched rim-light on the matching island.
- Readability during combat: previews <25% brightness + low-opacity fog, placed across a WIDE void gap (no overlap with combat/projectiles); all background void VFX on a LOWER render layer than island geometry.
- Perf OK: 2–3 islands @ ~30–80 renderers each, no scripts/lights.
- **Demo:** handmade preview prefab variants. **Final:** data-driven from `RoomData` via a RUNTIME composer (current `RoomDataComposer` is Editor-only/`AssetDatabase`). `RoomThumbnailBaker` + `RoomData.thumbnailPath` already exist if a thumbnail/minimap fallback is ever needed.

### 2D. Map-fragment repurpose (Effort S–M)
- Keep `Systems/Map/RoomLoader.SpawnFragmentThenDraftUnlock` (draft+gate unlock stays). On pickup ALSO call `DungeonGraph.RevealAhead(revealSteps)` then refresh preview spawn.
- ⚠️ TWO MapFragment classes exist: `RIMA.Environment.MapFragment` (draft/unlock) vs `RIMA.MapFragment` Core (calls RevealAhead). Merge/rename before polish.
- Fragment escalation: reveal more previews → reveal +1 depth → upgrade preview detail (type first, then contents). Maps onto `RoomNode.revealed` / `GetRevealedStepsAhead()`.

## 3. TIMINGS (ax, total ≈800ms)
| Beat | Time | What |
|---|---|---|
| Room-clear reveal | 0–300ms | last enemy dies → camera slight zoom-out, portals rise/grow from edge |
| Portal hover | 150ms | matching preview island illuminates + scales +5% |
| Morph-in | 100ms | player squash (scaleY→0.1, scaleX→2.0) + 100ms alpha fade → orb |
| Void travel | 350ms | orb arcs Bezier; TrailRenderer+Light2D carry the read; camera leads to target |
| Arrival/illuminate | 200ms | crash burst + camera shake; player scales back 100ms; island fog lifts 200ms |

## 4. ANIMATED vs STATIC (juice-per-effort order)
1. Comet/ribbon trail on orb (HIGH juice / LOW effort, procedural)
2. Crash-land impact burst + camera shake (HIGH / LOW)
3. Portal swirl + floating rune (MED / LOW)
4. Hover-glow island rim-light (MED / MED)
5. Orb morph squash-stretch (MED / MED)
6. Dark preview islands — STATIC (perf save)
7. Far parallax void — STATIC

## 5. CRITICAL TECH BLOCKER (do first)
The live `Systems/Map/RoomLoader` only does `LoadNext/JumpToRoom` over a LINEAR `_sequence`. Portals are real branch choices ONLY if the loader becomes **graph-aware** (node-id → room-data mapping). `RuntimeRoomManager.OnPlayerEnteredDoor` already does `DungeonGraph.Navigate(dir)` — reuse that pattern. **Without this, portal choice is cosmetic and still advances linearly.**

## 6. FORKS
- **F1 — Preview detail: LOCKED = (C) real layout visible, enemies hidden.** Preview island shows genuine room geometry (floor/walls/traps/shape) but NO enemy positions — the natural result of the mob-less real-island approach. No extra enemy-preview data needed.
- **F2 — Further-ahead reveal (spending fragment to scout 2+ nodes): DEFERRED** (user: secondary; lock core loop first). When decided: camera-pan across void to distant islands, vs distant islands appear smaller/farther in place. NOT a HUD overlay (user rejected abstract map).

## 7. DEMO BUILD ORDER (recommended)
1. Graph-aware RoomLoader entry (§5 blocker). 2. Portal.cs graph-bind + count (§2A). 3. PortalTravelDirector + orb (§2B). 4. RoomPreviewIsland prefab + bind (§2C, 1 handmade preview first). 5. Map-fragment RevealAhead hook (§2D). 6. Polish: trail/crash/hover-glow + timings (§3).
