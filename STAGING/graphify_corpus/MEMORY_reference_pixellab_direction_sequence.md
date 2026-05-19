---
name: PixelLab 8-dir Sequence
description: Project constant for CCW output mapping.
type: reference
---

# SEQUENCE (COUNTER-CLOCKWISE)
| Index | Direction | Code |
|-------|-----------|------|
| 1 | South | S |
| 2 | South-West| SW |
| 3 | West | W |
| 4 | North-West| NW |
| 5 | North | N |
| 6 | North-East| NE |
| 7 | East | E |
| 8 | South-East| SE |

# VERIFICATION (S40)
* NEW3: Face Left (West)
* NEW7: Face Right (East)
* NEW1: Front (South)
* NEW5: Back (North)

# RULE
* When receiving classNEW1-8.png: apply this mapping immediately.
* Check NEW3/NEW7 once per batch to ensure UI hasn't flipped sequence.
