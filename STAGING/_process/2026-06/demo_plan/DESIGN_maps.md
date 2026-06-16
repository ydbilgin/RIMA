# DESIGN — Bigger, Better-Composed Cliff Maps (G1)
> Demo 19 Haz. Read-only planning. Canon-safe. **No new system** — author with the EXISTING `RoomTemplateSO` + `IsoRoomBuilder` + `RoomBankSO` pipeline.
> Verified against real code (not package examples): `IsoRoomBuilder.cs`, `RoomRunDirector.cs`, `RoomBankSO.cs`, `RoomTemplateSO.cs`, `Assets/Data/Rooms/{Generated,Library,Special}/*.asset`, `Assets/Data/Rooms/DemoRoomBank.asset`, `_Arena.unity`.

---

## 0. REALITY CHECK — how rooms actually work (live path)

- Rooms are **authored `RoomTemplateSO` .asset files** (NOT runtime-procedural for the demo). Each holds `bounds` (RectInt w×h, tile=32px top-down), a per-tile `walkabilityMap`, `playerSpawn`, `enemySpawnSockets`, `doorSockets`, `props` (PropPlacementData), and `lightingProfile`.
- `RoomRunDirector.BuildCurrentRoom()` → `roomBank.Pick(type, seed, exitSlotCount)` → `IsoRoomBuilder.Build(template)`. The builder paints floor tiles, derives the **floating-island cliff silhouette automatically** (any floor cell whose SW/SE neighbour is void gets a cliff sprite — `BuildCliffs`), builds the boundary collider, spawns markers/props/lighting/exit-doors. **The cliff-island look is emergent from the walkability silhouette** — that is the canon identity lever.
- Live bank = `Assets/Data/Rooms/DemoRoomBank.asset`. Current contents:
  - **combat** (9): bridge_lobes, cross, diamond, lshape, donut, hourglass, teardrop, organic_blob, twin_basins
  - **elite** (2): crescent, trident · **boss** (1): shattered_oval · **merchant** (1): Shop_01
- `RoomType.Chest` has **no list** in the bank → Chest nodes currently fall back to `fallbackTemplate` (gap, see §7).

### Why rooms "feel too small" (root cause — it is NOT the footprints)
The big templates already exist (24×18 → 36×28). The smallness is **camera + density**, two cheap levers:
1. `RoomRunDirector.useFixedDemoCamera = true`, `fixedOrthographicSize = 5.0` → the view is hard-zoomed; even a 36×28 boss room only shows ~10×7 tiles at PPU64. The follow-cam tracks the player so the player never *sees* the island scale.
2. Combat/Elite rooms run with `builder.spawnProps = false` (F1 soft-lock fix) → arenas read as flat empty discs, no depth/landmarks → "small + bare".
3. Old `Library/Combat_*` templates are genuinely tiny (12×8, 16×10) — must NOT be in the demo bank (they aren't, good).

---

## 1. TARGET ROOM FOOTPRINTS (tiles, 32px top-down grid — canon)

Keep widescreen-ish ratios (~4:3 → 1.4:1) so cliff fronts (SW/SE edges) read. Inspirations: Hades chamber *variety* (silhouette per room), Dead Cells *density* (props as landmarks), Children of Morta *readable open combat core*.

| Role | Current | TARGET footprint | Combat core (clear) | Notes |
|---|---|---|---|---|
| Combat (standard) | 24–26 × 18 | **28 × 20** | ≥ 18×12 open | bump small ones to 28×20 band; keep 2 dash-lanes (8+ tile) |
| Combat (large/showcase) | 30–36 × 18–24 | **34 × 22** | ≥ 24×14 | bridge_lobes / organic_blob — the "wow scale" rooms |
| Elite | 30 × 22 | **32 × 22** | ≥ 22×16, CENTER EMPTY | elite silhouette reads at range |
| Chest / Reward | 22 × 16 | **22 × 16** (keep) | small, calm | side-pocket feel; donut_vault is on-canon |
| Merchant | 16 × 12 (Shop_01) | **20 × 14** | n/a | too cramped for 3 stands + browsing; widen |
| Boss | 36 × 28 | **40 × 28** | ≥ 30×20, CENTER EMPTY | the scale payoff; back-platform elevation feel |

> **Composition rule of three (canon-safe, from D5 ADAPT):** door-front clearance ≥ 3 tiles · column/obstacle pairs ≥ 4 tiles apart (dash passes) · 1 flat tile of floor at every chasm/cliff rim · rhythm = open core → pinch → open. **No center obstacle in elite/boss** (readability). These are *composition principles* lifted from package ROOM_PHIL; the iso-cellSize 1×0.5 math is REJECTED (RIMA = 32×32 top-down).

---

## 2. COMPOSITION + PROP DENSITY (per role)

Cliff identity comes from the **silhouette** (walkability shape), not interior walls. Vary the silhouette per room — that is the Hades-variety lever and it is FREE (just the walkability mask).

- **Combat (standard):** organic blob / teardrop / hourglass silhouette. Interior **mostly open** (combat needs the dash lanes). Density via **non-blocking decals + 2–4 edge landmarks** (broken pillar stub, rune cluster, brazier) hugging the cliff rim — NOT in the combat core. Re-enable a *curated* prop set (see §3, the F1 fix only needs spawn-area clearance, not zero props).
- **Combat (large showcase):** bridge_lobes / organic_blob → 2 connected lobes with a pinch bridge (chasm on both sides = floating-island drama). Combat happens in the bigger lobe; the bridge is the "open→pinch→open" beat.
- **Elite:** crescent / trident → **center empty**, elevation-feel platforms at the wing tips (2×2), 2 symmetric edge pillars near the entrance only. Elite silhouette must be visible at range (zoom-out, §4).
- **Chest/Reward:** donut/diamond vault → small, calm, 1 centerpiece (the chest spawns at room center via `ResolveRewardSpawnPosition`), low light, ember accents. Few props, high polish.
- **Merchant:** widen Shop_01 to 20×14, 3 stand anchor points across the back third (`ShopRoomController.Setup(center)` places stands — keep center clear), warm light, decorative-only props at edges.
- **Boss:** shattered_oval at 40×28 → vast empty arena, back-third subtle elevation read (platform), entrance columns as the only obstacles, chasm rim all around (the floating-island peak). Center 30×20 must stay clear for Gravity Cleave / Iron Charge.

**Density target:** combat = 4–8 props (edge-only) · elite = 2–4 (entrance + wings) · chest/merchant = 3–5 decorative · boss = 2 entrance columns only. All blocking props MUST respect 3-tile door clearance + 3×3 spawn clearance (`HasWalkableClearance` already enforces spawn nudge).

---

## 3. CAMERA FRAMING IMPLICATIONS (the actual "feels small" fix)

This is the single highest-leverage change and it is **one field + a per-room hint**, no new system:

- **Raise the zoom-out.** Per-room target: standard `fixedOrthographicSize ≈ 6.5`, large/elite `≈ 7.5`, boss `≈ 9.0`. Two clean options (pick at impl time):
  - (A) keep `useFixedDemoCamera=true` but raise `fixedOrthographicSize` to ~7.0 globally (cheapest, uniform), OR
  - (B) set `useFixedDemoCamera=false` → `FitCameraToRoom()` already computes ortho from `TryGetLastFloorWorldBounds` + `cameraPadding`; this auto-frames each silhouette and *shows the island scale*. Follow-cam still tracks in big rooms via `ConfigureFollowCamera`. **Recommended for "see the cliff" demo wow** — but it changes per-room zoom, so verify spawn-readability live.
- `ConfigurePixelPerfectCamera` already rescales refResolution from ortho size at PPU=64 — bigger ortho stays pixel-correct. No PPU change (PPU=64 LOCKED).
- Re-enabling curated props (§2) gives the camera *landmarks* to parallax against → the room reads as a real place, not a disc.

---

## 4. HOW TO AUTHOR — using the EXISTING tooling (no new system)

The named-silhouette assets in `Assets/Data/Rooms/Generated/` ARE the authoring output. To get bigger/better rooms, **edit/add `RoomTemplateSO` assets and (re)reference them in `DemoRoomBank.asset`** — that is the entire pipeline.

Per room, three surgical authoring moves:
1. **Footprint:** in the `.asset`, set `bounds.width/height` to §1 target, and regenerate the `walkabilityMap` (length = w×h, index `y*w+x`) for the new silhouette. The painter/QC tools exist: `RIMA/Rooms/QC/*` menus (`RoomQCFixMenu.cs`) validate + reseed props; `RoomTemplateValidator` checks exit slots; `IsoRoomBuilderTester` / `RoomBankRuntimeTester` build a template in-editor to preview the cliff silhouette. Authoring the mask = the existing `RoomPainterWindow` / template-loader-saver (`RoomTemplateLoader/Saver`, `RoomTemplateJsonExporter`) — no code.
2. **Sockets:** keep `playerSpawn` on an interior cell with 3×3 clearance (director will nudge anyway). Ensure `doorSockets` give ≥3 valid **exit** slots so 3-door branch nodes pick a wide room (`Pick(type, seed, requiredExitSlots)` filters by `ValidExitSlotCount`). Door-front 3-tile clearance.
3. **Props:** add curated edge props via the existing `props` list (`PropPlacementData` + `PropRegistrySO` GUIDs). For combat/elite the director sets `spawnProps=false` today — to show density, either (a) author props only outside the spawn pocket and relax the combat `spawnProps` guard to `true` for the *new* curated templates, or (b) use `enableAutoDecoration` (visual-only `RoomDecorationPass`, never blocks) for risk-free density. **Option (b) is the safest demo path** — decoration is non-blocking by construction, so the F1 soft-lock can't regress.

Then: drop the new/edited templates into `DemoRoomBank.asset` lists (combat/elite/boss/merchant) and **add a Chest list** (§7). Run `RIMA/Rooms/QC/Smoke Test All Templates` to verify every template builds.

---

## 5. ASSET GAPS

- **Chest room bank list empty** → `RoomType.Chest` falls to fallbackTemplate. Need ≥1 chest template wired (the `chest_large_donut_vault_01` / `chest_large_reliquary_diamond_01` assets EXIST, just not in the bank).
- **Merchant only 1 template (Shop_01, 16×12)** → cramped; widen to 20×14 (edit existing) or author a 2nd.
- **Cliff/portal sprites:** `IsoRoomBuilder` exits early if cliff or portal sprites are missing (`HasAnyPortalSprite`). Confirm the cliff S/SE/SW sprites + portal sprites are assigned on the `_Arena` IsoRoomBuilder instance (they were in prior verified runs — low risk).
- **Edge-landmark props** for cliff rims (broken pillar stub, brazier, rune cluster) — if not already in PropRegistry, these are the only *new art* needed; otherwise reuse existing CombatBiome props. Decals already exist (`Assets/Data/Blueprint/GeneratedProps/CombatBiome_v15d`).
- **No new boss/elite art** needed — silhouette + zoom does the work.

---

## 6. DEMO FEASIBILITY (19 Haz, 3 gün)

- **Camera zoom-out (§3):** 1 field change (or toggle to FitCameraToRoom) → **highest wow, ~30 min + live verify**. DO FIRST.
- **Re-add curated density via `enableAutoDecoration` (§4b):** data-only, non-blocking, can't regress F1 → **safe, ~half day**.
- **Footprint bumps (§1):** edit ~3-4 hero templates (standard combat 28×20, boss 40×28, widen merchant) + reseed walkability via painter → **~1 day, needs live smoke-test per room**.
- **Chest bank wiring (§5):** trivial, **15 min**.
- **Risk:** every walkability edit must pass `Smoke Test All Templates` + a live spawn-clearance check (the F1/F2 soft-lock class). Don't touch the `spawnProps=false` combat guard unless using decoration path.

---

## 7. RECOMMENDED DEMO CUT (priority order)
1. Camera zoom-out (toggle `FitCameraToRoom` OR raise fixedOrthographicSize ~7.0) — *the* "rooms feel bigger" fix.
2. Wire Chest templates into `DemoRoomBank` (close the fallback gap).
3. `enableAutoDecoration` curated density on 3-4 hero combat/elite templates (non-blocking → safe).
4. Widen Merchant 16×12 → 20×14.
5. (If time) bump boss → 40×28 + 1-2 large combat showcase silhouettes (bridge_lobes/organic_blob already 30-36 wide).
Each step is data/inspector-only on EXISTING systems; verify each with `Smoke Test All Templates` + live spawn check.
