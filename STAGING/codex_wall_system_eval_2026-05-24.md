# Wall System Brief - Codex Technical Eval (2026-05-24)

## Q1 - Mapping
Commit fd00ad23 finals: iso_floor_broken/clean/cracked/debris/edge_light/rift_glow; wall_n_corner, wall_n_landmark, wall_ne_doorway/mid_broken/mid_plain/mid_variant, wall_nw_doorway/mid_broken/mid_plain/mid_variant, wall_pillar_universal. Brief set: 10 connectors, 12 spans, 14 seams, 12 props, tall variants. Mapping: floors -> floor palette only; pillar -> connector; NE/NW mids -> 128x384 medium spans; doorways/landmark -> specialty spans; n_corner -> corner proxy. Missing: most connector variants, short/long spans, seam overlays, props, tall variants, front/parapet pieces.

## Q2 - Size schema
Yes for pixel math: at PPU=64, 64 px = 1 world unit, so 128x64 = 2x1, 64x384 = 1x6, 128x384 = 2x6, 192x384 = 3x6. Unity Tilemap plus WallSnapPlacer can be pixel-perfect if sprites keep bottom-center pivots, untrimmed canvases, point filtering, and manifest footprints. Custom Y sort axis (0,1,0) is already configured. Risk: tall transparent canvases need explicit pivot/sortPoint and collider footprints.

## Q3 - PixelLab native compat
Direct sources conflict: the task says 256x768 S-XL was verified on 2026-05-24, but MEMORY lists S-XL/Pro presets up to 512 square and 688x384 non-square, and says not to expect 768. 256x256 is native/safe. 768x384 is not documented and is the highest-risk sheet. Treat A/B/D/E as web-UI custom-size only; split C into supported widths or use 688x384/crop only after a pilot.

## Q4 - Unity integration
Current RoomLoader instantiates explicit room.walls prefab ids at Tilemap cell centers; no automatic WallSpanPlacer/fill exists. The hierarchy is valid, but diamond rooms need a boundary-trace algorithm: derive perimeter edges from walkable mask, compress straight runs into spans, place connectors at direction changes, then overlays/props. Seam overlays are sufficient for 2-4 px mismatch, not structural drift. Y-sort needs bottom pivots plus front/back sorting offsets for columns crossing side/back chains.

## Q5 - Real production time
Per sheet: S-XL gen 5-12m, download 1-2m, alpha cleanup 2-5m, grid slice 1-3m, piece QC 8-20m, Unity import/slice metadata 5-10m. A/B/D/E: 22-45m each; C: 25-50m due size retry risk; F: 18-35m. Total realistic: 2.5-5h. 1-1.5h only works with first-pass art, accepted custom sizes, and superficial QC.

## Verdict
- **PARTIAL**
- Rationale: Architecture fits RIMA's PPU64/Y-sort direction, but the 60-piece scope and undocumented 768-wide sheet make it too risky as one production batch. Ship a smaller connector/span/seam pilot first.

## Eger GO ise - recommended dispatch order
1. F: 256x256 socket prop pilot
2. A: 256x768 connector columns
3. B: 256x768 short/medium spans
4. D: 256x768 seam overlays
5. E: 256x768 specialty spans
6. C: long spans only after supported-size pilot or split
