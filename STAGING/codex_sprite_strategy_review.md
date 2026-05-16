1. ONERI: Hybrid - use Strategy A for structural L3 wall assets and Strategy B for organic L4/L5/L6 decoration masters, with an automated slice manifest so the user never edits slice rects by hand.

2. PRINCIPAL REASON: L3 walls are gameplay-readable perimeter caps, so each horizontal, vertical, corner, and doorway piece should be explicit, reviewable, and easy to replace as separate PNG variants. L4/L5/L6 are decorative painter-pixel stamps, where one master texture with multiple slices gives lower PixelLab dispatch cost, cleaner Unity asset browsing, shared texture memory, and better batching. The UX promise is not decided by A or B alone; it is decided by whether RIMA stores size bucket, pivot, layer role, weight, minDistance, and atlas-rule metadata in BrushPack/AssetPool instead of exposing it to the user. Hybrid is the safest production path because it separates structural correctness from decorative variety.

3. PIXEL-ART RISKS:
- Strategy A risks: many independent textures can import with inconsistent Pixels Per Unit, filter mode, compression, pivot, alpha padding, and sprite mesh settings unless import presets are enforced. Variant PNGs may drift in palette or outline weight if regenerated individually. File count grows quickly, and separate textures can reduce dynamic batching benefits. Pixel-perfect remains safe if every PNG is authored at native resolution and placed without runtime scale.
- Strategy B risks: slice bleeding occurs if the 512x512 master lacks transparent gutters between sub-sprites. Sprite Editor rects live in .meta, so bad merges can silently move pivots or boundaries. Tight mesh can crop soft painter alpha unless set consistently. Auto slicing is unsafe for organic patches; use deterministic manifest slicing or reviewed manual slicing. Pixel-perfect remains safe if slices are native-size sub-rects, not scaled, and all texture import settings are locked to Point, no compression, no mipmaps.
- Shared risk: do not use non-integer Transform scale for brush size. Even integer scale should be avoided for final art because RIMA needs painter-pixel silhouettes, not magnified stamps. Brush radius must choose a different native sprite size, not scale one sprite.

4. PRODUCTION COST: Estimated PixelLab dispatches should optimize for master sheets, not one image per final stamp.
- L3 wall horizontal: 2 dispatches recommended, one 512x288 or 688x384 master for long segments plus one variant/regeneration if needed. Expected output: 6-10 explicit PNG cuts.
- L3 wall vertical: 2 dispatches recommended, one 384x216 or 512x288 master plus one variant/regeneration if needed. Expected output: 6-10 explicit PNG cuts.
- L3 wall corner: 1-2 dispatches recommended, one 512x512 master containing NE/NW/SE/SW corner families with padding. Expected output: 8-12 explicit PNG cuts.
- L3 wall doorway: 1-2 dispatches recommended, one 512x288 master for broken caps, threshold edges, and gap transitions. Expected output: 5-8 explicit PNG cuts.
- L4 transition: 1 dispatch per biome material pair, 512x512 master. Expected slices: 1 hero 256, 4 medium 128, 12 small 64, optional 16 micro 32.
- L5 detail: 1 dispatch per biome detail family, 341x341 or 512x512 master. Expected slices: mostly 32/64, a few 128, total 24-40 slices.
- L6 accent: 1-2 dispatches per hero rift family, 512x512 or 632x424 master. Expected slices: 1-2 hero, 3-5 medium shards, 8-12 small chips.
- Credit summary: Hybrid should keep a biome starter set around 9-14 PixelLab dispatches instead of dozens of one-off PNG dispatches. The recut/slice work should be automated locally and should cost no PixelLab credits.

5. MASTER BOYUT TABLOSU:

| Layer | Recommended master | Strategy | Slice/output count | Notes |
|---|---:|---|---:|---|
| L3 wall horizontal | 688x384 or 512x288 | A | 6-10 PNGs | Long irregular caps, 32x32 grid-aware length families, explicit pivots. |
| L3 wall vertical | 384x216 or 512x288 | A | 6-10 PNGs | Same silhouette language as horizontal, vertical readability first. |
| L3 wall corner | 512x512 | A | 8-12 PNGs | Put NE/NW/SE/SW in one master only if gutters are planned; export explicit corner assets. |
| L3 wall doorway | 512x288 | A | 5-8 PNGs | Door gap, broken cap, side threshold, optional rubble lip. |
| L4 transition moss/dirt | 512x512 | B | 17-33 slices | 256 hero, 128 medium, 64 small, optional 32 micro; best candidate for master slicing. |
| L5 detail cracks/rubble | 341x341 or 512x512 | B | 24-40 slices | Mostly 32 and 64, sparse 128; high variant count matters more than hero size. |
| L6 accent rift | 512x512 or 632x424 | B with curated hero slices | 12-20 slices | Keep 1-2 hero rifts, shard families, glow chips; sparse placement only. |

6. BRUSH RADIUS MAPPING: Use manual preset buckets, not linear scaling. Radius 1-2 picks micro/small 32 with high minDistance and low density. Radius 3-4 picks small 64. Radius 5-6 picks medium 96/128 where available, otherwise 64 plus lower density. Radius 7-8 picks large 128/192. Radius 9 picks hero 256 only for layers that declare heroAllowed=true. Radius 10 picks hero 256/320 only for L6 or explicit L4 hero strokes. Mapping should be table-driven per BrushPack:
- radius 1: bucket XS, max sprite 32
- radius 2: bucket XS/S, max sprite 32
- radius 3: bucket S, max sprite 64
- radius 4: bucket S, max sprite 64
- radius 5: bucket M, max sprite 128
- radius 6: bucket M, max sprite 128
- radius 7: bucket L, max sprite 192
- radius 8: bucket L, max sprite 192
- radius 9: bucket XL/Hero, max sprite 256, heroAllowed required
- radius 10: bucket Hero, max sprite 256-320, layer whitelist required

7. KARAR #143 UYUM: Strategy metadata should live above both PNG cuts and sliced sprites, so atlas rules work the same for A and B. encounterAvoidRadius filters candidate placement before instantiation, especially for L6 and large L4 stamps. edgeBiased should increase probability near tilemap floor/wall boundaries for L4 transitions and near wall caps for L5 rubble, but should not override walkable filtering. minDistance must be per bucket, not global: 32px details can repeat closer than 256px hero rifts. wallProximityFactor should push L3/L4/L5 assets toward perimeter context while leaving L6 sparse and deliberately composed. Strategy B integrates slightly better for L4/L5 because many variants share one texture and one BrushPack family, but the rule engine should not care whether a sprite is a separate PNG or a sub-sprite.

8. V1 IMPLEMENT IMPACT: Brush V1 does not need a user-facing UX change, but it needs backend metadata normalization.
- Sprint 6 default brush pack: add SizeBucket metadata, nativePixelSize, layerRole, weight, minDistance, heroAllowed, edgeBiasWeight, allowedRadiusRange, and optional flip policy for each Sprite entry.
- Sprint 6 composite executor: choose sprite by radius bucket first, then atlas filters, then weighted random, then minDistance rejection, then flip.
- Sprint 7 Auto-Dress: generate placements from the same metadata so hand strokes and auto dress produce consistent density.
- Sprint 7 Regenerate: preserve stroke seed, radius bucket, layer role, and rule fields while swapping variants.
- Sprint 7 Smart Fill: use bucket presets and wallProximityFactor instead of raw brush radius as the only signal.
- Asset import/spec: add locked import preset for Point filter, no compression, no mipmaps, Pixels Per Unit, pivot mode, gutters, and Sprite Multiple manifests for B.
- No change should require the user to open Unity Sprite Editor during normal production.

9. V2 FORWARD-COMPAT: Hybrid is more marketplace-friendly than pure A or pure B. Marketplace packs can ship structural L3 as explicit named assets with namespace prefixes like biome_crypt_l3_wall_h_01, while decorative L4/L5/L6 can ship as sheet plus manifest under biome_crypt_l4_transition.sheet.json. Conflict resolution becomes deterministic if every entry has a stable GUID-like local id, namespace, layerRole, bucket, and sourceSheet id. Strategy B lowers asset-browser noise for large biome packs, while Strategy A keeps critical wall pieces inspectable. V2 should treat both separate PNGs and sliced sprites as the same logical BrushAsset record.

10. SHIPPED EXAMPLES: Hades uses authored painterly ground breakup and hero-readable accent shapes; it does not feel like one texture scaled up and down, so RIMA should pick authored stamp sizes. Dead Cells relies on strong tile readability plus decal variety; structural pieces stay controlled while dressing varies aggressively. Hyper Light Drifter proves that sparse, high-contrast pixel accents need deliberate placement and should not be sprayed like noise, which maps to L6 heroAllowed gating. Death's Door and Tunic both show the value of readable silhouettes and consistent top-down perspective; wall and doorway assets should be explicit and curated. Polybrush is the closest Unity mental model for the backend: the artist paints intent, while the tool samples variants, density, slope/proximity rules, and scatter constraints.

11. UX PROMISE GUARANTEE: Hybrid best satisfies "Paint-like brush, automatic sensible set" with zero extra user work, but only if slicing and metadata are generated by pipeline scripts. Pure A is easier to reason about but pushes too much variant management into AssetPool and creates file noise. Pure B is efficient but fails the zero-extra-work promise if the user must draw rects or repair .meta slices. The guarantee should be: user dispatches a small number of PixelLab masters, drops them into an intake folder, runs/imports a generated manifest, then paints with B/E/[/]/1-9. The brush system handles bucket selection, atlas rules, minDistance, weighted variation, flip, layer coordination, and native-size sprite placement without runtime scale.
