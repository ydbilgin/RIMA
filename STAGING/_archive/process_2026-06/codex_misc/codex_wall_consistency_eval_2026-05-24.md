# Wall Pack Consistency Strategy - Codex Independent Eval (2026-05-24)

## Q1 - 5-Layer Validation
The strategy reduces drift but does not guarantee consistency. Layer 1 is useful inside one sheet, yet mixed silhouettes still drift in row rhythm and baseline. Layer 2 can preserve broad palette/style, not exact palette, thickness, or brick rows. Layer 3 is the strongest deterministic layer, but bottom-row alpha detection can lock onto shadows, dust, or detached debris unless component-aware. Layer 4 fixes palette only after quantization; it cannot repair geometry. Layer 5 is weak as a lock: a template is prompt guidance, not a hard mask, so PixelLab can bleed past boundaries. Treat AI layers as inputs, not validators.

## Q2 - Baseline Measurement (Mevcut 17 Asset)
Measurement used the committed final top-level PNG set from `fd00ad23`; current extra `wall_nw_mid_plain_sprite_forge.png` was excluded. Floors are shown for the 17-asset set, but brick rows are not applicable to floors.

| Asset | Baseline | Top Cap | Brick Rows | Unique Colors |
|---|---:|---:|---:|---:|
| iso_floor_broken.png | 60 | 2 | n/a | 1451 |
| iso_floor_clean.png | 60 | 5 | n/a | 1313 |
| iso_floor_cracked.png | 61 | 2 | n/a | 1578 |
| iso_floor_debris.png | 62 | 2 | n/a | 1640 |
| iso_floor_edge_light.png | 61 | 3 | n/a | 1902 |
| iso_floor_rift_glow.png | 61 | 2 | n/a | 1887 |
| wall_n_corner.png | 365 | 21 | 5 | 7194 |
| wall_n_landmark.png | 383 | 17 | 6 | 25462 |
| wall_ne_doorway.png | 366 | 10 | 6 | 8585 |
| wall_ne_mid_broken.png | 373 | 8 | 9 | 9325 |
| wall_ne_mid_plain.png | 369 | 9 | 5 | 7381 |
| wall_ne_mid_variant.png | 370 | 8 | 8 | 9410 |
| wall_nw_doorway.png | 367 | 12 | 5 | 8347 |
| wall_nw_mid_broken.png | 369 | 14 | 10 | 8584 |
| wall_nw_mid_plain.png | 361 | 12 | 8 | 6208 |
| wall_nw_mid_variant.png | 376 | 11 | 5 | 8713 |
| wall_pillar_universal.png | 374 | 15 | 7 | 3408 |

Current wall consistency is not production-tight: baseline range is 361-383, top cap range is 8-21, detected brick rows range is 5-10, and visible unique RGB colors range is 3408-25462. Floors are tighter on baseline/top but still have high unique-color counts. The existing set proves size normalization happened; it does not prove content normalization.

## Q3 - pixel_align.py Function Spec
```python
def detect_baseline_row(image: np.ndarray, alpha_threshold: int = 128) -> int
def detect_top_cap_row(image: np.ndarray, alpha_threshold: int = 128) -> int
def align_to_baseline(image: np.ndarray, target_row: int = 380) -> np.ndarray
def detect_brick_rows(image: np.ndarray, palette: List[RGB]) -> int
def enforce_canvas_size(image: np.ndarray, w: int, h: int, pad_color: RGB = (0,0,0,0)) -> np.ndarray
def verify_consistency(pieces: List[np.ndarray], tolerance: int = 2) -> Dict[str, Any]
```

- `detect_baseline_row`: convert RGB to opaque RGBA if needed; use alpha mask `>= threshold`; return bottom row of the largest connected opaque component, not stray shadow/debris. Return `-1` if empty.
- `detect_top_cap_row`: same mask/component rule, returning the first row. Alpha-only images are valid; transparent RGB noise is ignored.
- `align_to_baseline`: compute shift `target_row - baseline`, paste into same-size transparent canvas, crop overflow, no scaling.
- `detect_brick_rows`: map pixels to palette/dark classes, smooth horizontal luminance/edge scores over opaque wall body, count separated dark/light row peaks with variance tolerance.
- `enforce_canvas_size`: no resampling; crop or pad to `w,h`. Type should be `RGBA`, not `RGB`, if transparent padding is allowed.
- `verify_consistency`: return `{summary, pieces, failures}` with baseline/top/brick/palette ranges, per-piece metrics, and tolerance failures.

## Q4 - Effort Estimate
Estimate: 6-8 hours. Core PIL/numpy implementation is about 3 hours. A 5-7 test suite is 1.5-2 hours: empty alpha, RGB no-alpha, shadow/debris baseline, crop/pad, baseline shift, synthetic brick rows, batch verify report. CLI integration into `pixel_cleanup.py` is 1-1.5 hours because the existing argparse flow already supports batch/single modes. Final validation on the 17 assets and README/example updates is 0.5-1.5 hours. Add time if component detection needs scipy-free connected-component code polished for speed.

## Q5 - Alternative if Strategy Weak
Use a pilot-gated hybrid. First generate Sheet A only, then run `pixel_align.py` and reject unless baseline/top/brick/palette pass. Use Sheet A as anchor for B/D/E/F, but split risky long spans into 64x384 or 128x384 modules and assemble/repeat centers programmatically instead of trusting 768x384. Keep seam overlays as visual cover, then do short Aseprite cleanup only on approved anchors. For brick-row consistency, derive a canonical row template from the best wall and snap generated rows toward it. Tile-based generation fits floors/props better than tall walls.

## Verdict
- **GO_WITH_MODIFICATIONS**
- Strongest layer: Layer 3 plus Layer 4, because they are deterministic and testable.
- Weakest layer: Layer 5, followed by Layer 1/2 if treated as guarantees.
- Next dispatch order: implement `pixel_align.py` report-only mode; run it on current 17 assets; generate Sheet A pilot; QC Sheet A; then produce B/D/E/F; split or defer Sheet C long spans until native-size risk is resolved.
