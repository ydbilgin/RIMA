---
name: PixelLab Direction Offset
type: feedback
description: S43 anchors SW-facing offset; canonical game directions must map to PixelLab source labels
---
## Rule
- S43 anchors were generated SW-facing instead of exact South-facing.
- Raw PixelLab labels are not canonical RIMA game directions.
- PixelLab developer knows this; editable direction mapping is on their roadmap.
## Do NOT
- Do not rename source files in `STAGING/anchors/chars/<class>/rotations/`.
- Do not ask PixelLab to "fix South"; this wastes credits and risks inconsistency.
- Do not trust PixelLab panel labels as canonical.
## Mapping
- Canonical game -> PixelLab file: S=south-east, SE=east, E=north-east, NE=north, N=north-west, NW=west, W=south-west, SW=south.
- PixelLab file -> canonical game: south=SW, south-east=S, east=SE, north-east=E, north=NE, north-west=N, west=NW, south-west=W.
## In Every Task
- PixelLab generation: `keep current character direction setup, do not reinterpret facing`.
- Unity import/wiring: `apply direction offset mapping before naming clips and wiring blend tree`.
## Status
All 10 S43 class anchor folders present with 8 rotations and matching metadata paths.
