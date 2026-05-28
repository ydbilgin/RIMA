# Core Wall System Eval (CHATGPT spec v2, Codex 2026-05-24)

## Q1

Partial compatible. WallChainBuilder can place connector-span chains on `GridToWorld(x*2,y*1)`, but current art is 128x384 walls; new spec is 64-256 wide and 32-128 tall. Do not drop in at native scale. Use new sheets as clean core source, then import at 2x runtime scale or regenerate runtime sprites at doubled dimensions. Keep 17 assets as visual height/reference fallback. Keep asset_pack_v2 as variation/reference, not base grammar.

## Q2

Common prefix for all sheets: "RIMA Act1 Shattered Keep pixel art, style anchored to chatgpt_ref_wall_anchor and master_room_pilot, clean undecorated dark slate masonry, no torches, banners, books, moss, cracks, or props baked in, high top-down 3/4 fake-iso, edge-to-edge modular joins, solid #FF00FF chroma-key background."

Sheet 1, 512x512, 4x2 grid: 1 straight connector, 2 outer corner, 3 inner corner, 4 end column, 5 door column left, 6 door column right, 7 alcove connector, 8 protrusion connector.

Sheet 2, 512x512, 2x4 grid: 1 plain middle wall A, 2 plain middle wall B, 3 plain middle wall C, 4 short span 1x, 5 medium span 2x, 6 long span 3x, 7 low wall 1x, 8 low wall 2x.

Sheet 3, 512x512, 2x4 grid: 1 outer corner L, 2 outer corner R, 3 inner corner L, 4 inner corner R, 5 door arch 2w, 6 door arch 3w, 7 open gap, 8 short stop.

Sheet 4, 512x512, 4x4 grid: straight seam, outer corner seam, inner corner seam, door jamb L, door jamb R, base trim, front edge L, front edge R, front corner L, front corner R, pillar-wall seam, shadow patch, cleanup cap, gap filler, plain filler, micro occluder.

## Q3

| # | Piece | Final px |
|---|---|---|
| 1 | Straight Connector | 64x128 |
| 2 | Outer Corner Connector | 64x128 |
| 3 | Inner Corner Connector | 64x128 |
| 4 | End Column | 64x128 |
| 5 | Door Column L | 64x128 |
| 6 | Door Column R | 64x128 |
| 7 | Alcove Connector | 64x128 |
| 8 | Protrusion Connector | 64x128 |
| 9 | Plain Middle Wall A | 128x96 |
| 10 | Plain Middle Wall B | 128x96 |
| 11 | Plain Middle Wall C | 128x96 |
| 12 | Short Span 1x | 128x64 |
| 13 | Medium Span 2x | 192x64 |
| 14 | Long Span 3x | 256x64 |
| 15 | Low Wall 1x | 128x40 |
| 16 | Low Wall 2x | 256x40 |
| 17 | Outer Corner L | 128x96 |
| 18 | Outer Corner R | 128x96 |
| 19 | Inner Corner L | 128x96 |
| 20 | Inner Corner R | 128x96 |
| 21 | Door Arch 2w | 192x128 |
| 22 | Door Arch 3w | 256x128 |
| 23 | Open Gap | 128x64 |
| 24 | Short Stop | 64x96 |
| 25 | Straight Seam | 32x64 |
| 26 | Outer Corner Seam | 64x64 |
| 27 | Inner Corner Seam | 64x64 |
| 28 | Door Jamb L | 32x64 |
| 29 | Door Jamb R | 32x64 |
| 30 | Base Trim | 64x32 |
| 31 | Front Edge L | 64x48 |
| 32 | Front Edge R | 64x48 |
| 33 | Front Corner L | 64x64 |
| 34 | Front Corner R | 64x64 |
| 35 | Pillar-Wall Seam | 32x64 |
| 36 | Shadow Patch | 64x32 |
| 37 | Cleanup Cap | 64x64 |
| 38 | Gap Filler | 64x32 |
| 39 | Plain Filler | 64x64 |
| 40 | Micro Occluder | 32x32 |

## Q4

Patch WallChunk without breaking old prefabs: add `seamSocketLeft`, `seamSocketRight`, and `optionalPropSocket`; keep legacy `seamSocket` as fallback. Extend `ApplySockets()` names: `seam_left`, `seam_right`, `prop`, plus existing `torch` and `banner`. Retrofit template and prefabs with new child transforms. Later consolidate Torch/Banner into OptionalPropSocket categories, but do not delete old fields yet.

## Q5

Current builder supports only perimeter basics: classify edge, place one pillar connector at each vertex, fill every interior cell with short span, skip open edges. It does not support door arches, low front edges, open gaps, inner/outer corner semantics, alcove/protrusion grammar, span length packing, or seam overlays (`GetSeamFor` returns null). Add an assembly-plan layer over WallChainBuilder rather than replacing it.

## Q6

MIN counts assume longest valid span pieces and one open/front edge. Small 4x3: 4 connectors, 3 spans, 4 seams, 2 low walls = 13. Medium 6x4: 4 connectors, 4 spans, 4 seams, 2 low walls = 14. Large 8x5: 4 connectors, 4 spans, 4 seams, 2 low walls = 14. If forced to 1x spans only: 14, 20, 26.

## Q7

Workflow: 1. Lock clean base sprite and metadata ID. 2. Send base sprite to PixelLab Edit Image. 3. Prompt only the variant material: cracked, broken, mossy, library, prison, flooded. 4. Use AI Freedom medium. 5. Preserve silhouette, pivot, sockets, cell footprint, and magenta/transparent background. 6. Export as same piece ID plus variant suffix. 7. QC edge match against clean base. 8. Add variant to library as alternate skin, not new grammar.

## Q8

MVP playable closed room needs 11 pieces: straight connector, outer corner connector, end column, plain middle wall A, short span 1x, medium span 2x, low wall 1x, low wall 2x, door column L, door column R, door arch 2w. Add straight seam and outer corner seam only if visible joins fail; otherwise defer seam sheet until after builder proof.

## Q9

Clean first is the right engineering order. A modular system must prove pivots, sockets, colliders, span packing, door gaps, and edge joins before decoration multiplies the test matrix. Variants after that become skins on stable pieces; variants before that hide structural bugs and make every art pass harder to diagnose.

## Q10

Maps to new spec: v2 straight_column, outer/inner_corner_connector, end_column, door_column_left/right, alcove_connector, protrusion_connector, wall_span_short/medium/long, low_parapet, straight/corner/door/base/pillar seams. Superseded as core: cracked_wall_span, broken_wall_span, cracked_alt, broken_alt, prison_bar_wall, library_bookcase_wall, ritual_wall, flooded_low_wall. Keep as later variation references: all superseded specialty/variant walls, props, torch, banner, brazier, bookshelf insert. Discard: none delete; quarantine floors unless floor lock is re-opened.

## Verdict

- **PARTIAL**
- Reconciliation required: yes.
- Revoke/update the stale "pure top-down 85-90deg" comments in WallChainBuilder; they conflict with the live HIGH TOP-DOWN 3/4 lock.
- Do not revive true iso diamond floor from this wall spec. Floor/iso tilemap requires separate Opus-level decision.
- Revise asset_pack_v2 role: reference/variation backup, not production core.
- Resolve scale by either importing new sprites at 2x runtime scale or regenerating the clean spec at 128/256 runtime dimensions for Warblade 120px.

Dispatch plan:
1. PixelLab Sheet 1 connectors.
2. PixelLab Sheet 2 spans.
3. Import MVP pieces, patch prefab sockets, and test one room.
4. PixelLab Sheet 4 seams/front edge after visible joins are known.
5. PixelLab Sheet 3 doors/shapes.
6. Only then run Edit Image variants from clean base sprites.

## Risk

- Native 64x128 connectors are too small against Warblade 120px unless scaled or regenerated.
- Current WallChainBuilder cannot express doors, front edges, span packing, or seam selection without a semantic plan layer.
- Mixing v2 decorated pieces into the clean core will reintroduce visual noise and make connection bugs harder to isolate.
