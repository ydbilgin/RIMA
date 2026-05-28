# RIMA Phase 0 Vertical Slice — Progress + Direction Check for ChatGPT

I followed your previous correction memo and ran Phase 0 (scale test vertical slice). Below is what I did, what worked, what I learned, and the direction I want to take next. **Please review and tell me if I'm on the right track before Phase 1.**

---

## 1. Phase 0 dispatched assets via PixelLab MCP

Per your spec (PPU=32 base, 32×32 terrain native, Wang16 = JSON/corner-driven):

| Asset | Tool | Native size | Result |
|---|---|---|---|
| Floor tile variants (16-frame pack via `create_tiles_pro`) | `square_topdown`, segmentation outline | 32×32 | ⚠️ Got 16 variants of ONE prompt with style drift (some clean stone, some with runes/blood/orange — uncontrolled diversity) |
| Wang16 stone→moss tileset (`create_topdown_tileset`) | high top-down, transition_size=0.5 | 32×32, 16 tiles + 7 transitions in 128×128 sheet | ✅ Works. JSON metadata with `corners: {NE/NW/SE/SW: lower/upper}`, `bounding_box`, `original_position`, `pattern_4x4` |
| Wooden crate prop (`create_object` n_frames=16) | high top-down | 64×64, transparent BG | ✅ 16 review candidates (crate/barrel/chest/trunk variants), pick best |
| Moss decal (`create_object` n_frames=16) | high top-down | 64×64, transparent BG | ✅ 16 organic moss patches (oval blobs, dense/sparse, vines, paths) |

Output saved to `Assets/Sprites/Environment/Phase0_ScaleTest/`. All downloads required `Invoke-WebRequest` with `MaximumRedirection 5` (curl fails on Cloudflare CDN handoff to backblaze CDN).

## 2. Wang16 import — your CORNER-BASED + JSON spec verified

I followed your spec exactly:
- Slice texture per JSON `bounding_box` (NOT by visual row/column order)
- Use proper JSON scan (not regex — regex match got the wrong bbox initially)
- Encode mask as `NW<<3 | NE<<2 | SW<<1 | SE<<0`
- Y-flip: **CONFIRMED needed** — PixelLab `bounding_box` is top-origin, Unity Sprite rect is bottom-origin. Formula: `unityY = sheetHeight - bboxY - tileHeight`

After fix, `wang_0` sprite pixel-samples correctly as all-lower terrain. Resolver works.

## 3. Visual Phase 0 result — key learning

Phase 0 scene: 12×8 tile floor + Wang16 transition patch (5×4 with moss appearing in center) + crate + moss decal + character.

**What worked:**
- PPU=32 lock = clean alignment, no half-cell offsets
- 64×64 character at PPU=32 = exactly 2×2 cells (natural ARPG scale)
- 64×64 crate also 2×2 cells (matches character scale)
- Wang16 transitions render correctly per corner mask

**Issue:**
- Wang16 stone→moss transition LOOKS RIGID and grid-fragmented even though corner resolver works perfectly. **The boundary doesn't read as "natural" — it reads as "tiled boundary"**.
- This visual issue is what Alabaster Dawn / Hades / CrossCode AVOID by NOT using Wang16 for same-elevation terrain blending.

## 4. Pivot — Wang16 use case re-scoped

After empirical test, I'm aligning my plan with your Karar #143-B/C guidance and your "same-family terrain pair WARNING":

**Wang16 RESERVED for elevation/height boundaries only:**
- Cliff edges (terrain elevation change)
- Water borders
- Raised platform edges
- Hazard zone borders
- NOT same-elevation terrain blending

**Same-elevation terrain blending (e.g., moss spreading on stone):**
- **Single base terrain tile** (e.g., pure dark slate stone)
- **Organic oval decal painting** on top — large irregular patches scattered with random rotation, scale, flipX/Y
- Decal "blob" shapes (64×64 or larger) span multiple cells, naturally cross grid boundaries
- This is Aseprite-brush-style painting — paint moss/dirt/grime patches ORGANICALLY where they would naturally appear

I tested this empirically:
- Replaced Wang16 transition patch with 12 organic moss decals (random position, random rotation 0/90/180/270, varied scale 0.7-1.3, slight alpha for blend)
- Result: **fluid moss spreading look** without grid-fragmented boundaries
- Decal placement followed PatchAtlasSO pattern (density, min-distance, edge-bias, allowFlipX/Y) per your Karar #143-M..R

## 5. Confirmed alignment with your spec

| Your spec | My execution | Verdict |
|---|---|---|
| PPU=32 base, 32×32 terrain, 64×64 char/prop as 2×2 cells | Done | ✅ |
| Wang16 = JSON-driven, corner-based, Y-flipped import | Done after regex bug fix | ✅ |
| Wang16 only for elevation/feature edges | Reserved — not used for same-elevation blending | ✅ |
| PatchAtlas for organic blending (Karar #143-M..R) | Implemented in Phase 0 final scene | ✅ |
| Same-family terrain pair → use overlay, not Wang | Validated empirically | ✅ |
| TerrainDefinitionSO decouple visual + gameplay | Pending (Phase 1) | ⏳ |
| WallKitSO modular wall library | Pending (Phase 2+) | ⏳ |
| Layered model L0-L11 | Phase 0 used L2 + L4 + props + char only | ⏳ partial |
| Vertical slice phase 0 first | Done | ✅ |

## 6. Questions for you before Phase 1

### Q1 — Floor variant strategy
PixelLab's `create_tiles_pro` returns "16 variants of ONE prompt" (variant pool of single concept) rather than "1 instance of 16 different prompts" (numbered specification). To get 6-8 specific floor variants (clean / mossy / cracked / worn / stained / rift-touched / dirt / blood), I have two options:

- **A)** 1 `create_tiles_pro` call → 16 variants → pick best 6-8 from natural drift
- **B)** 6-8 separate `create_object` calls (n_frames=16 each) → pick 1 best from each → guaranteed coverage but more credit

Which is industry-standard for floor variant production? Should I always use option B for controlled variety?

### Q2 — Decal density for "natural arena" feel
For a Hades-arena-style room (12×8 tiles = 96 cells), how many organic moss/dirt/grime decals achieve a "natural" feel without crowding? My Phase 0 used 12 decals (1 per 8 cells). Visually decent but more would help. What's your recommended density per cell for organic patches?

### Q3 — Y-flip standing rule
PixelLab JSON `bounding_box` is top-origin. My import correctly applies `unityY = textureHeight - bboxY - tileHeight`. **Should I assume this Y-flip rule for ALL PixelLab JSON-driven imports going forward, or are some PixelLab tools already bottom-origin compatible?**

### Q4 — PatchAtlasSO defaults per Karar #143-M..R
You proposed specific defaults:
- L4 Transition: density 0.08, edgeBiased, wallProximityFactor 1.5, minDistance 6, allowFlipX, centerReduction 0.05
- L5 Detail: density 0.18, edgeBiased, wallProximityFactor 1.2, minDistance 2, allowFlipX+Y, centerReduction 0.08
- L6 Accent: density 0.03, edgeBiased=false, encounterAvoidRadius 4, minDistance 12

For RIMA's Type B Hades arena (large open floor, distant walls), should these defaults change? Specifically, should L4 density go UP (0.15+) to compensate for open arena lacking edge density?

### Q5 — Phase 1 plan validation

Phase 1 = Minimal Type A enclosed dungeon (one biome, one room type, all layers wired):

**Asset dispatch:**
1. **L2 floor**: 1 base floor variant + 1 macro variant (pure dark slate stone, no decoration baked in) → use `create_object` n=16, pick 1-2 cleanest from "weathered dark slate gray stone, NO runes NO blood NO moss"
2. **L3 Wang16**: NOT for this phase (no elevation)
3. **L4 decals**: 6 types × `create_object` n=16 each → pick 4-5 per type = ~25 organic patches
4. **L5 detail scatter**: 4 types × n=16 → pick 4-5 per type = ~18 scatter
5. **L6 accent**: 2 types (rift scar + ritual circle) × n=16 → pick 2-3 per type
6. **WallKitSO**: skipping for Phase 1 (using painted background piece from previous Codex output as temporary room boundary)
7. **Props**: 6 critical (crate, urn, candle, brazier, banner, statue) — `create_object` n=16 each, pick 1-2

**Approx 22 PixelLab dispatches, ~50-70 credit consumption.**

Then Unity Brush V1 wire-up + Phase 1 sample room compose + sırıtma test with character.

**Is this Phase 1 plan correct, or am I missing something?**

### Q6 — Visual style drift mitigation
PixelLab generation has style drift (some tiles came back with runes/blood/orange that I didn't request). How do you control this?

- Negative prompts (you advised against negative-prompt bombardment)
- More specific positive prompts ("pure stone, no decoration, no symbols")
- Re-roll seed if drift detected
- Use `style_images` parameter to feed approved reference

Best practice for keeping output on-spec?

---

## 7. What I'm NOT planning yet (deferred)

- TerrainDefinitionSO + TerrainTransitionGraphSO + WallKitSO C# code — coming after Phase 1 visual validation
- LoRA training — research branch only, PixelLab vendor primary per your guidance
- 4×8 ragged transition tilesets — defer to Phase 2 (Edge Naturalization)
- DungeonRecipeSO + RoomRecipeSO — Phase 2 after asset library proven
- Runtime vs Editor generation modes — Phase 3

---

## Summary

I'm on the path of:
- **Phase 0 done** (PPU=32 + Wang16 corner-based + organic decal painting verified architecturally)
- **Phase 1 next** (Type A enclosed dungeon, single biome, all layers L2+L4+L5+L6+props+char wired, no Wang16 boundary, organic decal-driven blending)
- **Phase 2 later** (WallKit, multiple biomes, Wang16 for elevation, 4×8 transition patches)
- **Phase 3 last** (Brush V1 procgen wire-up, RoomRecipe/DungeonRecipe, runtime mode separation)

**Am I aligned with your architecture vision, or is there a wrong turn I'm making?**

Specifically curious about Q1 (floor variant strategy), Q2 (decal density), Q5 (Phase 1 plan completeness), Q6 (style drift mitigation).

Thank you.
