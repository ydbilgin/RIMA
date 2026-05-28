# A1 Wang16 v2 Cell Analysis

| Cell | Beklenen | Mevcut | Verdict | Action |
|---|---|---|---|---|
| 1 (1,1) | ISOLATED (none) | ISOLATED-ish double island, no clean edge pass | PASS | Keep |
| 2 (1,2) | CAP-N (S) | CAP-S / north-edge granite | FAIL | Rebuild cell to CAP-N / S-only |
| 3 (1,3) | CAP-S (N) | CAP-S / N-only | PASS | Keep |
| 4 (1,4) | VERTICAL-PASS (N+S) | VERTICAL-PASS / N+S | PASS | Keep |
| 5 (2,1) | CAP-W (E) | CORNER/SIDE mix, N+E bleed | FAIL | Rebuild cell to CAP-W / E-only |
| 6 (2,2) | CORNER-NW (E+S) | CORNER-NW / E+S | PASS | Keep |
| 7 (2,3) | CORNER-SW (E+N) | CORNER-SE-like / W+S | FAIL | Rebuild cell to CORNER-SW / E+N |
| 8 (2,4) | T-LEFT (E+N+S) | T-LEFT / E+N+S | PASS | Keep |
| 9 (3,1) | CAP-E (W) | CORNER-SE-like / W+S | FAIL | Rebuild cell to CAP-E / W-only |
| 10 (3,2) | CORNER-NE (W+S) | CORNER-NW-like / N+E | FAIL | Rebuild cell to CORNER-NE / W+S |
| 11 (3,3) | CORNER-SE (W+N) | CORNER-SE / W+N | PASS | Keep |
| 12 (3,4) | T-RIGHT (W+N+S) | T-RIGHT with east-edge bleed | FAIL | Rebuild cell to T-RIGHT / W+N+S |
| 13 (4,1) | HORIZONTAL-PASS (E+W) | HORIZONTAL-PASS / E+W | PASS | Keep |
| 14 (4,2) | T-TOP (E+W+S) | T-BOTTOM-like / E+W+N | FAIL | Rebuild cell to T-TOP / E+W+S |
| 15 (4,3) | T-BOTTOM (E+W+N) | T-TOP-like with bottom bleed | FAIL | Rebuild cell to T-BOTTOM / E+W+N |
| 16 (4,4) | FULL (N+E+S+W) | FULL / N+E+S+W | PASS | Keep |
