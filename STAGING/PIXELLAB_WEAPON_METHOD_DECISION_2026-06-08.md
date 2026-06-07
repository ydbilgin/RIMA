# PixelLab Weapon Method Decision - 2026-06-08

## ✅ FINAL DECISION (LOCKED — 3-source consensus: cx + ax-3.1-Pro + ChatGPT, 2026-06-08)

**Method = B-weighted HYBRID.**
- **Production = PixelLab Object generation** (`create_1_direction_object` / object endpoint): single isolated transparent object, one angle (runtime mount/orientation solves the 8 directions).
- **Create Image Pro = IDEATION ONLY** (silhouette/style/mood exploration, square focus-object pre-tries). NEVER the final production source — its exact non-square + transparent + target-size behavior is UNVERIFIED.
- **Sizing rule:** target-size native FIRST. If the object endpoint can't emit an exact non-square canvas → generate in a square/near-square canvas and **transparent CROP** to the target bbox. **CROP allowed (grid-safe); SCALE/downscale FORBIDDEN (breaks the pixel grid).** No big-canvas→downscale, ever.
- **Unity import:** PPU64 + Point + alpha transparency + **custom grip pivot set in Unity** (PixelLab returns NO pivot metadata).
- **Prompt:** single weapon object — "no character, no hands, no environment, transparent background, horizontal-right, handle left / tip right" + class-specific negatives (no Elementalist staff/wand · no Gunslinger western/revolver · no Shadowblade glow/aura · no Hexer whip · Brawler = no weapon). Never "weapon held by a character" (model spawns hands/body).
- **Batch by ASPECT FAMILY (≤8/batch, don't mix wildly different aspects):** B1 small linear (Shadowblade dagger + Gunslinger pistol) · B2 focus/square (Elementalist disc + Summoner lantern + Hexer grimoire) · B3 medium/large (Ranger bow + Ravager axe + Ronin katana; Warblade greatsword only if replacing placeholder).
- **Per-weapon (PROVISIONAL — confirm at VERIFY-LIVE):** Rune Disc 48×48 center-pivot (yönsüz, no blade rotation) · Ranger Bow ~64×40-48 bbox, left grip ~(0.32,0.50), leftHandPrimary · Shadowblade Dagger 32-40×24, grip ~(0.20,0.50), single sprite + offhand flipX.

**🔒 GATE — VERIFY LIVE before ANY production (first step of the gated PixelLab session, 1 cheap test):** Does the object endpoint support exact non-square canvas? transparent alpha guaranteed? candidate/review count? `style_images` + size together? sub-32px height (greatsword 64×16)? If non-square unsupported → use square+crop path. Do NOT write these as locked until tested against the live API/UI.

Sources: this doc (cx, grounded) · `_process/2026-06/_research_axpro_pixellab_method_2026-06-08.md` (ax-3.1-Pro) · ChatGPT web review (B-hybrid + target-bbox/crop nuance + aspect-family batches).

---

Status: ANALYSIS ONLY. No generation, no code. (cx's original recommendation below; superseded by the FINAL DECISION above where they differ.)

## Recommendation

Use a hybrid workflow, with Method B as the production path.

- Production: use the object pipeline for the final weapon asset: API `POST /objects`, `directions: 1`, `no_background: true`, target `image_size` or target-sized `style_images`, plus `item_descriptions` and review selection. The local session docs name the MCP wrapper as `mcp__pixellab__create_1_direction_object`; the REST API reference names the endpoint `POST /objects` and says "1-direction mode uses the consistent-style pipeline". VERIFY LIVE: whether the MCP tool is exposed as `create_1_direction_object` or maps to `/objects`.
- Exploration: use Create Image Pro only when broad visual discovery is needed. API name is `POST /generate-image-v2` / client `generate_image_v2`, not `create_tiles_pro`. At 64 px max dimension it should return 16 images, but it does not provide the object review/promote workflow or grip metadata.
- Do not use `create_tiles_pro` for weapons. It is a tile tool with `tile_type`, `tile_size`, and numbered tile prompts.
- Do not use legacy `POST /map-objects` as the primary weapon path. It can create transparent map props with rectangular `image_size`, but the docs call it the legacy object pipeline and it has no `n_frames`, `item_descriptions`, or object review selection flow.

Reason: RIMA needs one clean transparent sprite per weapon, imported PPU 64 / Point, with a manual grip pivot. The object pipeline is built for one-direction static objects with candidate review and selected-frame promotion. Create Image Pro is useful for discovery, not final HandAnchorAttach production.

## API Evidence

### Method A: Create Image Pro

Exact API endpoint:

- `POST /generate-image-v2`
- Summary: "Generate image (Pro)"
- Client example: `client.generate_image_v2(...)`
- Key params: `description`, `image_size.width`, `image_size.height`, `seed`, `no_background`, `reference_images` up to 4, `style_image`, `style_options`.

Findings:

- Variations at 64 px: confirmed for `/generate-image-v2`. Docs say output counts by max dimension: up to 42 px gives 64 images, 43-85 px gives 16 images, 86-170 px gives 4 images, above 170 px gives 1 image. A 64 px image is in the 16-image tier.
- Transparency: available via `no_background: true`. Docs describe automatic background removal.
- Non-square: supported by `/generate-image-v2` through `image_size.width` and `image_size.height`; supported sizes depend on aspect ratio, with examples like 512x512 square and 688x384 16:9. A 64x16 greatsword-like target is within the documented 16 px minimum. VERIFY LIVE for quality at very thin silhouettes.
- Point/pixel-perfect: not an API output metadata concept. PixelLab generates pixel art; Unity import must set Point filter, PPU 64, no mipmaps/compression blur.
- Grip pivot: not supported by the API. Pivot must be set manually in Unity Sprite Editor or importer metadata.
- MCP name: not `create_tiles_pro`. If exposed through MCP, expect something equivalent to `generate_image_v2` / "Generate image (Pro)". VERIFY LIVE against the current MCP tool list.

`POST /generate-with-style-v2` is a related Pro endpoint, but it is style generation, not the object review pipeline. It accepts `style_images` (1-4), `description`, `image_size`, `style_description`, `seed`, `no_background`. Docs say output counts include 33-64 px -> 16 images and 65-128 px -> 4 images, but also say supported sizes are square and non-standard sizes are internally padded. For non-square weapons, treat it as VERIFY LIVE.

### Method B: Item/object generation

Exact API endpoint:

- `POST /objects`
- Summary: "Create object (1-direction consistent-style or 8-direction)"
- Local MCP/session name: `mcp__pixellab__create_1_direction_object`
- Related review endpoints: `GET /objects/{object_id}`, `POST /objects/{object_id}/select-frames`, `POST /objects/{object_id}/dismiss-review`.

Key params:

- `description`: required text prompt.
- `directions: 1`: routes to the consistent-style static object pipeline.
- `image_size.width`, `image_size.height`: object image size. Docs say square or rectangular, 32-256 px.
- `view`: `low top-down`, `high top-down`, or `side`.
- `n_frames`: only for `directions: 1`; must be one of `1`, `4`, `16`, `64`. Natural value depends on `image_size` (max dimension around 42 -> 64, 85 -> 16, 170 -> 4, else 1). `n_frames > 1` returns review status for selected-frame promotion.
- `style_images`: consistent-style references, max 8, each with raw RGBA bytes plus `width` and `height` (16-512 in the reference schema).
- `item_descriptions`: per-frame descriptions for multi-frame packs.
- `no_background`: default true in the LLMS docs.

Findings:

- Non-square target size: supported by `/objects` as rectangular `image_size`, but with documented minimum width and height of 32 px. This means 64x48 bow and 48x48 disc are clean fits; 64x16 greatsword or 32x16 dagger are below the documented height minimum if using `image_size`.
- Thin 16 px-high targets: VERIFY LIVE. The docs allow `style_images[].height` down to 16, and local constraints say style refs can define output size, but `/objects.image_size.height` says min 32. If the live MCP accepts target-sized style refs as output size, it may handle 64x16. Otherwise generate on a 32 px-high transparent canvas and crop empty alpha rows without downscaling.
- Transparency: supported via `no_background: true`, default true in object docs.
- Point/pixel-perfect: Unity import controls Point filtering and PPU. PixelLab does not emit Unity import settings.
- Grip pivot: not supported. The production constraints explicitly say pivot/grip metadata does not come from PixelLab; set it manually in Unity.
- `create_object_state`: not the first-generation path. It edits an existing completed object via `object_id` and `edit_description`; use it only for variants of an accepted object.
- `create_map_object` / `POST /map-objects`: legacy and single-prop oriented. It supports transparent map objects and rectangular sizes up to 400, but lacks the multi-candidate object review flow.

## Target Size Versus Downscale

Use target-size native. Do not generate 128 then downscale to 64.

Evidence:

- The weapon audit locks target-size native and warns against 128->64 grid damage.
- Production constraints state PixelLab native pixel output is the advantage and downscale often breaks the pixel grid.
- The live Warblade asset is 64x16, imported with PPU 64, Point filter, alpha transparency, custom pivot 0.18/0.5.

How each method honors this:

- `/objects`: honors target-size native when `image_size` is legal, or when live MCP confirms target-sized `style_images` define output size. Best production fit.
- `/generate-image-v2`: can request very thin targets like 64x16 because `image_size` min is 16; useful if `/objects` rejects sub-32 heights.
- `/generate-with-style-v2`: can request `image_size`, but docs imply square/padded handling. VERIFY LIVE before using it for thin weapons.
- Downscale workflow: reject as default. Cropping transparent padding is acceptable if no resampling occurs.

## Style References

Yes, use style references, but size them deliberately.

- `/objects` supports `style_images` in consistent-style mode, max 8. Schema fields are `style_images[].base64`, `style_images[].width`, `style_images[].height`.
- `/generate-image-v2` supports one `style_image` and up to four `reference_images`.
- `/generate-with-style-v2` supports `style_images` 1-4.

Warblade style reference:

- The live `Warblade_Greatsword.png` is a valid style reference candidate for metal treatment, outline, and horizontal-right convention.
- Do not pass the raw 64x16 Warblade ref into every object batch if the live tool uses the largest ref to define output size. It may force or bias the output size and candidate tier.
- Preferred: prepare per-target transparent reference canvases: e.g. Warblade style pasted on a 64x48 canvas for bow, 32x32 or 32x16 target for dagger, and omit it for the rune disc if the blade silhouette pollutes the concept. Pair with the class character ref at the same target size.
- Local constraints say `size` and `style_images` cannot be used together and that style ref size controls output. The API schema does not explicitly state this conflict for `/objects`. Treat as VERIFY LIVE against the actual MCP/API before production.

Batch-size effect:

- API `/objects` says `n_frames` legal values are 1, 4, 16, 64 and natural value depends on `image_size`.
- Local production constraints say with style refs: <=85 px gives 8 candidates, <=170 px gives 4, above gives 1; without style refs: <=42 px gives 64, <=85 px gives 16, <=170 px gives 4.
- This is a doc conflict. For RIMA's audit lock, keep production batches <=8. In practice, request 4 candidates per weapon when using the REST `n_frames` enum, or use 8 only if the live MCP wrapper exposes `num_candidates: 8`.

## One-Weapon Workflow

Example: Ranger compound bow.

1. Prepare references at target size.
   - Ranger class sprite downscaled/padded to the bow target canvas.
   - Optional existing bow ref at the same canvas.
   - Optional Warblade metal/style ref pasted into the same canvas, only if it does not override bow silhouette.
2. Submit object generation.
   - Tool: `mcp__pixellab__create_1_direction_object` if available; REST equivalent `POST /objects`.
   - Params: `directions: 1`, `description`, `image_size` if live tool allows it with refs, `style_images`, `item_descriptions`, `n_frames` or MCP `num_candidates`, `no_background: true`, seed optional.
3. Poll object status.
   - REST: `GET /objects/{object_id}`.
4. Pick one frame.
   - REST: `POST /objects/{object_id}/select-frames` with a single selected index.
   - Dismiss rejected review objects with `POST /objects/{object_id}/dismiss-review` when appropriate.
5. Validate PNG.
   - Transparent alpha.
   - Target canvas or transparent-cropped target, no resampling.
   - Horizontal-right for directional weapons: grip left, tip/action direction right.
   - No anti-aliased blur.
6. Import in Unity.
   - PPU 64.
   - Filter Mode Point.
   - No mipmaps, no lossy compression.
   - Sprite pivot set manually at grip point.
7. Runtime hookup.
   - Ranger: left-hand primary, custom flip policy, likely no blade-style flipY.
   - Shadowblade: one dagger sprite, runtime mirrored offhand, no embedded blade glow.
   - Elementalist: hover/palm mount, no blade rotation, pivot center.

## Batch Strategy

Do not mix aspects or sizes in one production batch.

Reason: every generated object shares the same `image_size` and/or style-reference target. Mixing a disc, a bow, and a dagger in one batch creates bad size pressure and review ambiguity.

Recommended grouped jobs for the 3 gated weapons:

| Batch | Weapon | Target | Candidates | Notes |
|---|---|---:|---:|---|
| B1 | Ranger compound bow | 64x48 | 4 or live-MCP 8 | Rectangular, horizontal-right, left-hand grip. |
| B2 | Shadowblade dagger | 32x16 target, or 32x32 canvas with 32x16 visible dagger | 4 or live-MCP 8 | Sub-32 height is VERIFY LIVE for `/objects`; crop transparent padding only if needed. |
| B3 | Elementalist rune disc | 48x48 | 4 or live-MCP 8 | Square disc, center pivot, hover mount. |

Can different aspects share a batch? No for production. They can share a discovery prompt in Create Image Pro only if the goal is mood-board exploration, not import-ready output.

## Three Gated Weapon Size Table

| Weapon | Production target | Aspect | Drawing convention | Grip / pivot | Runtime note |
|---|---:|---:|---|---|---|
| Elementalist rune disc | 48x48 | 1:1 | Top-down circular disc, no handle, no staff/wand | Center, `(0.5, 0.5)` | Hover above right palm; no blade rotation; bob/spin script or mount mode. |
| Ranger compound bow | 64x48 | 4:3 | Horizontal-right authoring, asymmetric mechanical bow | Grip at riser, about `(0.18, 0.50)` | Left-hand primary; no Warblade flipY policy by default. |
| Shadowblade dagger | 32x16 visible target; 32x32 canvas fallback | 2:1 visible | Horizontal-right, reverse-grip single dagger, no blade glow | Handle center, about `(0.20, 0.50)` for 32x16 or `(0.20, 0.50)` on padded canvas | One main sprite; mirrored offhand at runtime. |

These sizes preserve the live Warblade convention: authored horizontal-right, target-size native, PPU 64, manual grip pivot. The visible dagger target is intentionally thinner than the documented `/objects.image_size` minimum, so it is the main VERIFY LIVE item.

## Method Decision Against Pipeline Needs

RIMA's `HandAnchorAttach` path needs one selected transparent sprite with a known manual pivot. Variations are useful only until the final selection exists.

- Pure Method A: not recommended for production. It can produce many options at low resolution, but output selection/import requires more manual curation and it does not map cleanly to the object review workflow.
- Pure Method B: recommended for final production when size constraints are legal.
- Hybrid: recommended overall. Use Method A only for uncertain silhouettes or style discovery, then generate the final selected asset through Method B at native target size. If Method B rejects a thin 16 px-high weapon, use Method A at 64x16/32x16 or use Method B on a 32 px-high transparent canvas and crop without scaling.

## VERIFY LIVE Items

1. Actual MCP tool name: local plans say `mcp__pixellab__create_1_direction_object`; API reference says REST `POST /objects`.
2. Whether `POST /v2/create-1-direction-object` exists in the current live API, since local production constraints mention it but the fetched reference lists `POST /objects`.
3. Whether `/objects` allows `style_images` together with `image_size`. Local constraints say no; API schema does not show a mutual exclusion.
4. Whether target-sized `style_images` can force sub-32 output height, such as 64x16 or 32x16, despite `/objects.image_size.height` min 32.
5. Candidate count with style refs: local constraints say <=85 px gives 8 candidates; API docs expose `n_frames` values of 1, 4, 16, 64.
6. Whether `generate-with-style-v2` handles non-square weapon outputs cleanly or pads them in a way that harms import-ready thin silhouettes.
7. Whether the live UI/API returns already-cropped transparent PNGs for object review frames or a fixed canvas that needs transparent cropping before Unity import.

