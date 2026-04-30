---
name: PixelLab Direction Offset
type: feedback
trigger: direction, blend tree, SW-facing, import, sprite mapping
description: S43 anchors are SW-facing (1-step CW offset); canonical game dirs != PixelLab source labels
---

## Rule
- S43 anchors were generated SW-facing instead of exact South-facing (1-step CW offset).
- Raw PixelLab direction labels are NOT canonical RIMA game directions.
- Remap at Unity import time -- never rename source files, never ask PixelLab to "fix South".

## Mapping (canonical game -> PixelLab source file)
S=south-east, SE=east, E=north-east, NE=north, N=north-west, NW=west, W=south-west, SW=south

## In Every PixelLab Task
Add: "keep current character direction setup, do not reinterpret facing"

## In Every Unity Import Task
Add: "apply direction offset mapping before naming clips and wiring blend tree"
