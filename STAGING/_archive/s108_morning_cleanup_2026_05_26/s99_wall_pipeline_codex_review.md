# Codex Wall Pipeline Review

## A) Strateji verdict

- Question 1 verdict: **Pivot dogru, cope atma yanlis.**
- Mevcut 16-piece modular kit'in asil sorunu "piece count" degil, **grammar** sorunu.
- Modular kit tek tek guzel parcalar uretiyor ama room frame olarak bakinca tas bloklari Lego gibi yan yana diziliyor.
- ChatGPT'nin "continuous source art" pivotu bu problemi dogru teshis ediyor.
- Ancak mevcut 16-piece kit cope gitmemeli.
- En pratik karar: **hybrid pipeline**.
- Continuous source art, base perimeter wall silhouette ve connection grammar icin primary olmali.
- Mevcut modular kit, rubble, broken plugs, internal ruins, collision blockers, one-off filler ve proxy library olarak kalmali.
- V2 IMAGEGEN 5 sprite, style/detail anchor olarak mevcut modular kit'ten daha iyi.
- V2 setin problemi 5 piece ile perimeter tamamlayamamasi.
- Continuous source art bu 5 iyi sprite'i "room frame language"e cevirebilir.
- Modular kit'in icinden M09/M10 doorway, M11/M12 broken, M15 short wall ve M16 ruined corner salvage edilebilir.
- M01/M02 straight repetition current formda ana perimeter icin zayif.
- M13/M14 junction pieces vertical slice perimeter icin gereksiz.
- Sonuc: **modular atlas -> continuous source art pivot dogru; existing kit -> secondary overlay/repair/filler source**.

- Question 2 verdict: **Continuous strips first, closed room frame second.**
- Closed room frame first estetik hedefi hizli gosterir.
- Ama production'a daha az riskli baslangic continuous strips.
- Sebep: strips daha kolay crop edilir, daha kolay Unity'de align edilir, daha kolay retry edilir.
- Closed room frame tek image'ta N/S/W/E + 4 corner'i ayni anda cozmek zorunda kalir.
- Tek frame fail ederse tum call bosa gider.
- Strip fail ederse sadece o direction tekrar edilir.
- Closed frame'i reference/target compose olarak kullanmak iyi.
- Closed frame'i first shippable source yapmak riskli.
- Pratik sira:
- Phase 1: target closed-room concept / reference frame.
- Phase 2: N/S/W/E continuous strips, each with visible 64px crop markers or deterministic canvas.
- Phase 3: room test compose.
- Bu, ChatGPT planinin ruhunu korur ama failure blast radius'u dusurur.

- Question 3 verdict: **32 chunk + 36 overlay full-scope; vertical slice icin fazla.**
- 32 chunk mantigi 10-tile by 6-tile arbitrary room frame'i tile-chunk seviyesinde parcaliyorsa anlasilir.
- Ancak Act 1 first room icin bu fazla.
- Vertical slice icin minimum:
- 8-10 wall chunks.
- 6-8 overlays.
- 1 test room frame.
- 1 import/placement pass.
- Gereken minimum chunks:
- N continuous back strip.
- S foreground low strip.
- W side strip.
- E side strip.
- NE corner closure.
- NW corner closure.
- SE corner closure.
- SW corner closure.
- 1 archway or doorway insert.
- 1 collapsed/broken interruption insert.
- Gereken minimum overlays:
- 2 rubble piles.
- 1 cyan rift crack.
- 1 torch or sconce.
- 1 banner or cloth break.
- 1 floor-to-wall shadow/debris strip.
- Optional +2 small chips/moss/edge decals.
- Bu 16-18 asset ile "Lego wall" problemi test edilebilir.
- 68+ asset, first successful room'dan sonra expansion scope olmali.

## B) Technical risks

- Question 4 verdict: **Magenta #FF00FF background insight dogru.**
- Mevcut `scripts/process_imagegen_sprite.py` siyah background icin luma threshold kullanıyor:
- `alpha = luma > 30 ? 255 : 0`.
- Bu, pure black BG'yi temizliyor.
- Ama real near-black wall shadow degerleri de luma <= 30 ise transparan olur.
- V2 IMAGEGEN final PNG stats:
- `wall_n_v2.png`: 96x96, visible near-black opaque = 0%.
- `corner_SE_v2.png`: 96x96, visible near-black opaque = 0%.
- `archway_v2.png`: 128x128, visible near-black opaque = 0%.
- `collapsed_stub_v2.png`: 96x80, visible near-black opaque = 0%.
- Bu temiz halo verir ama en koyu occlusion piksellerini de kesmis olabilir.
- Modular kit final PNG stats:
- M01-M16 icinde pure black opaque pikseller var.
- M01: 206 pure-black visible pixels.
- M03: 103 pure-black visible pixels.
- M16: 158 pure-black visible pixels.
- Bu, wall art icinde gercek siyah/near-black kullanildigini kanitliyor.
- ChatGPT ref images ise cok daha koyu:
- Several refs have 30-73% near-black visible area.
- Siyah BG ile automatic alpha extraction bu tur ref ambiance'i wall shadow ile karistirir.
- Magenta #FF00FF, yesil degil, iyi secim.
- Sebep: RIMA palette'inde cyan rift var; green/magenta ikisi de genelde assette yok ama magenta daha kolay exact-key edilir.
- En iyi tercih:
- Source generation: magenta matte or true alpha.
- Script: exact magenta-to-alpha + optional 1px fringe cleanup.
- Avoid: luma threshold for final production walls.
- Current black-BG V2 pieces usable, ama production standard olmamali.

- Question 5 verdict: **688x384 PixelLab Create Image Pro local docs'a gore destekli; live MCP/API endpoint bu task'ta dogrulanmadi.**
- `MEMORY/PIXELLAB_TOOL_GUIDE.md` says `create_image_pro` output sizes include:
- 32x32, 64x64, 128x128, 256x256, 344x192, 341x341, 384x216, 512x512, 512x288, 632x424, 424x632, **688x384**, custom size beta.
- `STAGING/_research/PIXELLAB_WORKFLOW_INSIGHTS.md` also says large/non-square canvases include 688x384.
- Constraint says **NO PixelLab MCP call**, so I did not live-call PixelLab.
- NLM query attempted, but auth is expired; no canonical NLM confirmation available this run.
- Practical verdict:
- Treat 688x384 as available in PixelLab Web UI / Pro memory.
- Do not assume MCP exposes every Web UI preset until a read-only tool schema or prior successful task confirms it.
- Since user screenshot reportedly shows 688x384, risk is low for Web UI and medium for MCP automation.

- Question 6 verdict: **Inpaint/edit reliability: useful for variants, not safe as primary topology generator.**
- Local docs say `edit_image` exists for simple edits and `edit_image_pro` for nuanced edits.
- Docs recommend explicit "keep unchanged" constraints.
- Existing local research recommends Edit Image Pro for sheet re-theme and damage states.
- But current S99 state-pipeline evidence is negative:
- Batch 2 via state pipeline drifted.
- Generic "Transform into RIMA" was stopped.
- Only one archway-like output was good, other states weak.
- Therefore:
- Doorway/broken variants via edit can work if source geometry is already clean.
- It is not reliable enough to cut clean openings into arbitrary closed room frame without per-output QC.
- Best use:
- Inpaint doorway into an already sliced strip with strict mask.
- Keep base wall, perspective, palette, height, pivot unchanged.
- Repair one variant at a time.
- Do not batch 5 state variants for core wall geometry.

- Question 7 verdict: **640x384 direct is cleaner than 688x384 crop if 64px slicing matters.**
- 688 / 64 = 10.75.
- 384 / 64 = 6.
- ChatGPT's crop-to-640x384 fix is valid.
- But cleaner alternatives exist:
- Generate 640x384 directly if tool custom size beta or init image lock accepts it.
- Generate 512x384 for 8x6 if room width can be smaller.
- Generate per-strip:
- N/S: 640x96 or 640x128.
- W/E: 96x384 or 128x384.
- Generate closed concept at 688x384 only as non-sliced reference.
- Production-safe rule:
- Any slice source must have width and height divisible by 64.
- If tool only offers 688x384 reliably, crop a centered 640x384 with a fixed script before slicing.
- Keep crop margin outside planned room bounds; never crop through art.

## C) Unity architecture

- Question 8 verdict: **WallChunkDefinition SO + RoomWallFramePlacer is over-engineered for first pass.**
- Existing `WallPrefabRegistry_Act1.asset` already stores wallId -> prefab mappings.
- Current registry has both v2 core entries and all M01-M16 modular entries.
- Current runtime class is tiny:
- `WallEntry { wallId, baseSpriteId, flipX, prefab }`.
- `GetPrefab(string wallId)`.
- That is enough for vertical slice.
- Adding `WallChunkDefinition` now creates a second registry model before the wall art grammar is proven.
- Better path:
- Extend current `WallPrefabRegistry` only when a concrete need appears.
- Possible minimal additions later:
- `Vector2Int footprintTiles`.
- `WallDirection direction`.
- `bool isContinuousStrip`.
- `string variantGroup`.
- No new placer component until placement rules are stable.
- Existing JSON/room loader can request prefab IDs.
- Manual room test can use fixed transforms.
- Recommendation:
- Keep `WallPrefabRegistry_Act1.asset`.
- Add new strip prefab IDs later: `strip_n_10`, `strip_s_10`, `strip_w_6`, `strip_e_6`, `corner_ne_cont`, etc.
- Defer `RoomWallFramePlacer` until there are at least 3 room sizes to generate.

- Question 9 verdict: **3D sprite-card placement is a bad pivot for RIMA right now.**
- RIMA is locked to URP 2D Renderer.
- Current project has Transparency Sort Axis `(0,1,0)`.
- Sprites use bottom-center pivot and sortPoint=Pivot.
- 3D-card pipeline would affect camera, lighting, sorting, collision assumptions, authoring tools, and import rules.
- The advantage is only depth-parallax/card tilt.
- The current wall problem is not lack of 3D.
- It is source art continuity and crop/placement grammar.
- 2D sprites can solve it:
- longer strips;
- shared source art;
- consistent pivots;
- proper sorting layer;
- overlay decals.
- Verdict: stay 2D.
- If 3D is ever tested, keep it as separate prototype scene, not production pivot.

- Question 10 verdict: **Sorting approach works conceptually, but v2 prefab layer mismatch must be fixed before production.**
- Project setting:
- `ProjectSettings/GraphicsSettings.asset` has transparency sort mode custom axis and axis `(0,1,0)`.
- Sorting layers exist:
- Default, Ground, Floor, Decals, Walls, Entities, VFX, UI, Player.
- Older wall prefabs and modular prefabs are serialized on `Walls` sorting layer:
- `m_SortingLayerID: 593505845`.
- `m_SpriteSortPoint: 1`.
- V2 IMAGEGEN prefabs are serialized on `Default` sorting layer:
- `m_SortingLayerID: 0`.
- `m_SpriteSortPoint: 1`.
- This is a real production risk.
- Pivot sorting can only work correctly if layers are also correct.
- Continuous chunk pipeline should set:
- Sorting Layer: Walls.
- SortPoint: Pivot.
- Pivot: bottom-center.
- PPU: 64.
- Collider from footprint, not full visual bounds for arch/opening.
- If continuous wall strips overlap floor and entities, layer + pivot sorting is enough for 2D.
- But prefab normalization pass is required.

## D) Pipeline practicality

- Question 11 verdict: **Closed room frame at 688x384 is good for concept, marginal for slice-quality production.**
- 688x384 is about 10.75 x 6 tiles at 64 PPU.
- If all N/S/W/E + corners live in one image, each wall face receives limited pixel area.
- It can show ambiance well.
- It is less ideal for clean per-piece extraction.
- North wall detail may be acceptable.
- West/east strips become narrow.
- Corners and doorway openings fight for space.
- If target is a single fixed room background, 688x384 can work.
- If target is modular/chunked Unity walls, strips are safer.
- Use closed frame as:
- art direction reference;
- final compose smoke test;
- not the only source for every production sprite.

- Question 12 verdict: **Strips are faster to production; frame is faster to subjective approval.**
- Closed frame:
- Pros: one image quickly answers "does it feel like ChatGPT ref?"
- Cons: hard to slice, hard to repair, all-or-nothing failure.
- Strips:
- Pros: deterministic dimensions, easier crop, one-direction retries, Unity-friendly.
- Cons: one extra compose step to evaluate room feel.
- For this project:
- ChatGPT ref ambiance missing is the top complaint.
- Therefore start with one reference frame only if user/Opus needs visual approval.
- Then produce strips as real assets.
- Do not spend 7 phases before first strip test.

- Question 13 verdict: **Folder structure proposal is directionally useful but too verbose for vertical slice.**
- ChatGPT suggested:
- `Source/Cropped/Sliced/Variants/Overlays/UnityReady/`.
- That is reasonable for a mature pipeline.
- It is too much for first room.
- Simple practical structure:
- `Assets/Art/Walls/Act1_ShatteredKeep/source/`
- `Assets/Art/Walls/Act1_ShatteredKeep/processed/`
- `Assets/Art/Walls/Act1_ShatteredKeep/overlays/`
- `Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/continuous_v1/`
- `STAGING/wall_pipeline/` for temp source/crop logs.
- Keep source art outside Unity import if it is large/temporary unless needed.
- Promote only processed PNGs and prefabs to `Assets`.
- Use manifests rather than many folders:
- `processed/_manifest.json`.
- This is enough for traceability without folder sprawl.

- Question 14 verdict: **One script first; split later only if repetition appears.**
- Crop, slice, magenta-to-alpha, and validate can be one script for first pass.
- Current project already has:
- `scripts/process_imagegen_sprite.py`.
- `scripts/slice_modular_wall_kit.py`.
- These are small, direct, and task-specific.
- For continuous wall source:
- Create one `scripts/process_wall_source.py` later if implementation starts.
- It should:
- Load source.
- Convert magenta exact key to alpha.
- Crop to 640x384 if needed.
- Slice by a manifest.
- Validate divisibility, alpha edges, output sizes.
- Emit JSON report.
- Four separate scripts now would add coordination burden.
- Split only after there are multiple source shapes or repeated commands.

## E) Scope realism

- Vertical slice minimum: **10 chunks + 8 overlays**.
- Chunks:
- 1 N continuous back strip.
- 1 S foreground low strip.
- 1 W continuous side strip.
- 1 E continuous side strip.
- 4 corner closures.
- 1 archway/door insert.
- 1 broken/collapsed interruption insert.
- Overlays:
- 2 rubble/debris piles.
- 1 cyan rift crack.
- 1 torch/sconce or flame anchor.
- 1 banner/cloth break.
- 1 floor-to-wall shadow/debris seam.
- 2 small chips/moss/crack decals.
- This is enough to prove:
- walls connect;
- room has ChatGPT-ref ambience;
- Unity pivots/sorting work;
- overlay density can break repetition.

- 32 chunks + 36 overlays is realistic only after the grammar passes.
- It is not first production scope.
- Treat 68+ asset as Act 1 wall library v1.
- Treat 18 asset as vertical-slice room proof.
- The existing registry already has 26 serialized entries after current work, so data capacity is not the bottleneck.
- The bottleneck is art grammar and source slicing.

- Phase priority: **Phase 1, Phase 2, Phase 3 yeterli production start icin**.
- Phase 1: Source grammar pilot.
- Produce or select one closed-room target and one 640x384/divisible source plan.
- Decide magenta/alpha convention.
- Decide exact chunk manifest.
- Phase 2: Strip generation and processing.
- Generate N/S/W/E strips or one crop-safe source.
- Run one local process script.
- Import prefabs under existing registry.
- Phase 3: Unity room test.
- Build a fixed room scene.
- Use existing `WallPrefabRegistry_Act1.asset`.
- Fix sorting layer/PPU/pivot.
- Screenshot QC against ChatGPT refs.

- Phase 4-7 defer.
- Door/broken variants via inpaint defer until base strips pass.
- Full overlay library defer until base strips pass.
- New SO architecture defer until at least 3 room sizes exist.
- 68+ asset expansion defer until first room looks good.

## F) Risk + recovery

| Phase | Risk | Probability | Recovery |
|---|---|---:|---|
| Source reference | Closed frame looks good but cannot be sliced cleanly | MED | Use as art target only; generate strips separately with same style ref |
| Source reference | AI returns pure top-down instead of Hades-iso 70-75 | MED | Add "high top-down, visible front faces, bottom-center sprite footprint" and rerun one pilot |
| Source reference | Frame too dark for alpha extraction | HIGH if black BG | Use magenta #FF00FF or true alpha; avoid luma threshold |
| Source reference | 688x384 creates 10.75-tile width | HIGH | Generate 640x384 directly or crop fixed center 640x384 before slice |
| Strip generation | N/S/W/E strips drift in style | MED | Use accepted reference image(s); generate pairs in same call if possible; normalize palette after |
| Strip generation | Corners do not connect to strips | MED | Put straight + corner in same source sheet or generate corner from strip endpoints |
| Strip generation | Side strips read as isolated pillars | MED | Include floor contact shadow and continuation stones; QC in composed room |
| Inpaint variants | Doorway destroys wall grammar | MED-HIGH | Inpaint only after base strip approved; mask small area; fallback to separate archway overlay |
| Inpaint variants | Broken wall variants become new style | MED | Use same source and strict "keep unchanged" prompt; repair one variant at a time |
| Alpha processing | Magenta fringe remains on edge | LOW-MED | Exact-key plus 1px edge color cleanup; validate visible magenta count = 0 |
| Alpha processing | Near-black shadows removed | HIGH with black BG | Use magenta/alpha, not luma threshold |
| Slicing | 64px grid cuts through art | MED | Use manifest with gutters; crop only from divisible canvas; validate bbox and edge occupancy |
| Unity import | Wrong PPU/pivot | LOW | Reuse existing import normalization; validate meta `spritePixelsToUnits: 64`, pivot `{0.5,0}` |
| Unity import | Wrong sorting layer | HIGH for current v2 prefabs | Batch set SpriteRenderer sortingLayerName = Walls; verify YAML or Unity console |
| Unity placement | Continuous strip collider blocks archway | MED | Collider from gameplay footprint; split visual and collision child if doorway |
| Unity placement | Entity/wall overlap sorting fails | LOW-MED | Keep custom sort axis `(0,1,0)`, sortPoint Pivot, bottom-center pivots |
| Scope | 68+ assets consumes session before proof | HIGH | Cap first pass to 10 chunks + 8 overlays; expand only after screenshot pass |
| Tool | PixelLab MCP lacks same 688x384 preset | MED | Use Web UI manual or custom/init size; fallback to 512x384/640 custom |
| Tool | PixelLab edit state pipeline drifts | HIGH for state batching | Avoid state pipeline; use Edit Pro/Same Style Pro one targeted variant at a time |

## Codex final recommendation

- Approach: **C - hybrid selective continuous**.
- This means:
- continuous source art for base perimeter;
- existing modular kit retained as secondary/filler;
- V2 IMAGEGEN walls retained as style/detail anchors;
- no 3D pivot;
- no new architecture until visual grammar passes.

- Scope: **10 chunks first pass + 8 overlays max**.
- Do not approve 32+36 until first room screenshot passes.
- The first win condition is one Act 1 room that does not look like disconnected blocks.

- Tools: **hybrid**.
- Use PixelLab Create Image Pro or Web UI Pro for controlled source/strip generation if available.
- Use gpt-image-1 / ChatGPT image only if it is producing stronger mood refs, then process locally.
- Use PixelLab Edit Image Pro/Same Style Pro for targeted variants after source approval.
- Avoid `create_object_state` for core wall variants.
- Avoid PixelLab MCP object generation for non-square/tall wall chunks until non-square behavior is proven.

- Background: **magenta or true alpha; not black**.
- Black worked for the five V2 sprites only because the luma threshold produced clean hard alpha.
- It is risky for production because RIMA wall shadows and ChatGPT refs contain real near-black pixels.
- Magenta exact-key is the pragmatic standard.

- Unity: **2D existing pipeline**.
- Keep URP 2D Renderer.
- Keep Transparency Sort Axis `(0,1,0)`.
- Keep bottom-center pivot and SpriteRenderer sortPoint=Pivot.
- Fix current v2 prefab sorting layer to Walls during the next implementation task.
- Extend `WallPrefabRegistry_Act1.asset` rather than replacing it.

- Folder structure: **simple flat with manifest**.
- Recommended first pass:
- `Assets/Art/Walls/Act1_ShatteredKeep/source/`
- `Assets/Art/Walls/Act1_ShatteredKeep/processed/`
- `Assets/Art/Walls/Act1_ShatteredKeep/overlays/`
- `Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/continuous_v1/`
- `STAGING/wall_pipeline/`
- Avoid `Source/Cropped/Sliced/Variants/Overlays/UnityReady/` until the pipeline repeats successfully.

- First 3 production calls:

```text
CALL 1 - Act 1 continuous wall source pilot
Create a pixel-art source image for a Hades-like high top-down dungeon room wall frame.
Canvas: 640x384, exact 10x6 tile grid at 64 px per tile.
Background: solid magenta #FF00FF only outside the wall art.
Style: dark Shattered Keep granite, cold blue-grey stone, visible top caps and front faces, 70-75 degree high top-down view, painterly pixel art, bottom-center sprite footprints, no characters, no UI, no text.
Composition: one closed rectangular room perimeter with continuous connected walls, not separate blocks. Include north back wall, south low foreground wall, west/east side walls, four continuous corners. Leave central floor area open.
Lighting: cool dark shadows, subtle cyan rift accents only in cracks, no glowing fog baked into transparent area.
Hard constraints: all important wall edges must fit inside the 640x384 canvas; no cropping; no black outside background; no duplicated rooms; no labels.
```

```text
CALL 2 - Directional strips from approved source style
Create four isolated continuous wall strips for the same Shattered Keep room style.
Canvas: 640x384 or four separate strip canvases if supported.
Background: solid magenta #FF00FF outside art.
Output pieces: north back wall strip 640x96, south foreground low wall strip 640x64 or 640x96, west side wall strip 96x384, east side wall strip 96x384.
Each strip must be continuous masonry, with matching stone size, matching cyan crack density, and clear bottom-center footprint.
Style must match the approved room source: dark blue-grey granite, Hades-like high top-down, visible top caps, readable front faces, pixel art, no characters, no props, no UI, no text.
Hard constraints: do not create separate Lego blocks; do not add black background; leave clean magenta around silhouettes for alpha keying.
```

```text
CALL 3 - Minimal overlay/decal pass
Create eight isolated overlay sprites for Act 1 Shattered Keep wall dressing.
Canvas: 512x512 sheet with 8 clearly separated slots, magenta #FF00FF outside sprites.
Sprites: two rubble piles, one cyan rift crack decal, two small stone chip decals, one torch/sconce without large baked glow, one torn banner/cloth, one floor-to-wall dust/shadow debris strip.
Style: match the approved continuous wall strips, dark granite dungeon, restrained cyan accent, Hades-like high top-down pixel art.
Hard constraints: each overlay is separate and fully visible, no characters, no weapons, no UI, no text, no black background, no connected full wall pieces.
```

- Final decision:
- Approve ChatGPT's strategic pivot.
- Cut ChatGPT's scope.
- Keep existing Unity architecture.
- Standardize magenta/alpha now.
- Produce one room proof before any 68-asset library.
