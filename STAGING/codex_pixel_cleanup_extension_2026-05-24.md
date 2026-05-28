# Codex Task — pixel_cleanup Extension: GUI + Smart Merge + Region Fix (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: pixel_cleanup base CLI tool'una (bmiykczzi tarafından `Tools/pixel_cleanup/pixel_cleanup.py` olarak yazılıyor) 3 extension ekle: (1) Smart color merging k-means, (2) Tkinter GUI, (3) Region-fix interactive ("burayı düzelt"). Tüm extensions FREE local Python — AI call YOK.

**BAGIMLILIK:** bmiykczzi (base CLI) bitmemiş olabilir. Eğer base yoksa, BLOCKED yaz ve bekle. Base mevcutsa üzerine ekle.

## File Structure (extension)

`Tools/pixel_cleanup/` mevcut yapıya ek:
```
Tools/pixel_cleanup/
├── pixel_cleanup.py           # (mevcut, bmiykczzi'den)
├── smart_merge.py             # NEW — k-means cluster + outlier snap
├── pixel_cleanup_gui.py       # NEW — Tkinter GUI app
├── region_fix.py              # NEW — local region analysis + suggestion engine
└── tests/
    ├── test_smart_merge.py    # NEW
    ├── test_region_fix.py     # NEW
    └── (mevcut test'ler)
```

`~/.claude/skills/pixel-cleanup/SKILL.md` update — GUI mode + smart merge usage örnekleri ekle.

## Extension 1 — smart_merge.py (k-means perceptual)

Spec:
- Input: image array (RGBA), palette (optional)
- Algorithm:
  1. Extract opaque pixels (alpha > threshold)
  2. K-means clustering (sklearn yoksa pure numpy implementation) — k = palette_size veya auto-detect
  3. **Dominant cluster protection**: cluster size > 5% total pixels = dominant, KORUNUR
  4. **Outlier snap**: cluster size < 0.5% = outlier, en yakın dominant cluster'a snap
  5. **Borderline mark**: cluster size 0.5-5% = review needed, raporda işaretle
- Output:
  - Snapped image array
  - Cluster report (centroid, size, classification)
  - Confidence skoru her pixel için

Functions:
```python
def cluster_palette(rgb_array: np.ndarray, k: int = 32, perceptual: bool = True) -> ClusterResult
def classify_clusters(clusters: ClusterResult, total_pixels: int) -> Dict[int, str]
def snap_outliers_to_dominant(rgb_array: np.ndarray, clusters: ClusterResult, classifications: Dict) -> np.ndarray
def compute_pixel_confidence(rgb_array: np.ndarray, clusters: ClusterResult) -> np.ndarray
```

**Perceptual:** Eğer mümkünse RGB → LAB color space (numpy ile manuel CIE), distance perceptual. Eğer fazla karmaşıksa Euclidean RGB OK (note olarak yaz).

CLI flag: `--smart_merge` veya `--cluster_size 32` (pixel_cleanup.py'ye ekle).

## Extension 2 — pixel_cleanup_gui.py (Tkinter)

Spec:
- Tkinter (built-in Python, ekstra install YOK)
- Window layout:
  ```
  ┌─────────────────────────────────────────────┐
  │ pixel_cleanup GUI - [file]                  │
  ├─────────────────────────────────────────────┤
  │  ┌──ORIGINAL──┐  ┌──CLEANED PREVIEW──┐    │
  │  │            │  │                    │    │
  │  │  clickable │  │  diff overlay     │    │
  │  │            │  │  toggle           │    │
  │  └────────────┘  └────────────────────┘    │
  │                                              │
  │  Tools panel:                                │
  │    [Stray ☑] [Outlier ☑] [Color Noise ☑]    │
  │    [Smart Merge ☑] [Region Fix Mode ☐]      │
  │                                              │
  │  Sliders:                                    │
  │    min_area     [== 4 ==]                   │
  │    palette_tol  [== 24 ==]                  │
  │    noise_thresh [== 40 ==]                  │
  │    confidence   [== 0.7 ==]                 │
  │                                              │
  │  Auto-decision threshold:                    │
  │    [Manual] -[ ]------- [Aggressive Auto]   │
  │                                              │
  │  [Analyze] [Apply Auto] [Save] [Export Rep] │
  └─────────────────────────────────────────────┘
  ```
- Features:
  - Sol panel: Original image, click → highlight region
  - Sağ panel: Cleaned preview, diff toggle (red overlay = removed, green = changed)
  - Toggle her cleanup adımı (stray / outlier / noise / smart merge)
  - Slider thresholds
  - Click region → region_fix.py modal açar
  - "Auto-decision" slider: confidence threshold (>0.9 manual, <0.9 ignore vs >0.5 aggressive auto)
- Image display: PIL ImageTk
- Zoom: scroll wheel + drag
- Save: PNG output + JSON report
- Open: file dialog veya komut satırı `--gui input.png`

## Extension 3 — region_fix.py (Interactive Region Analysis)

Spec:
- Input: image, click coordinates (x, y), region size (default 32×32)
- Algorithm:
  1. Crop region from full image
  2. Run full analysis suite (stray, outlier, noise, cluster)
  3. Generate suggestions list:
     - "3 stray pixels detected (alpha < threshold)"
     - "2 off-palette pixels (closest: #5A4936 at distance 38)"
     - "Edge anti-aliasing on bottom row (8 semi-transparent)"
     - "Color noise cluster of 5 pixels (suggest: merge to #2B2118)"
  4. Confidence per suggestion
- GUI integration: modal popup
  - Suggestion list with confidence
  - "[Apply]" per suggestion veya "[Apply All]"
  - Preview before/after

Functions:
```python
def analyze_region(image: np.ndarray, x: int, y: int, size: int = 32) -> List[Suggestion]
def apply_suggestion(image: np.ndarray, suggestion: Suggestion) -> np.ndarray
def render_region_preview(image: np.ndarray, x: int, y: int, size: int, suggestions: List) -> Image
```

## Auto-Decision Logic (cross-extension)

```python
# Pixel level
confidence > 0.9  → AUTO_FIX (apply without asking)
0.6 <= conf < 0.9 → MARK_FOR_REVIEW (preview overlay)
confidence < 0.6  → IGNORE (might be intentional accent)

# Region level
all suggestions high-conf → "Apply All" available
mixed → user picks per suggestion
all low-conf → "No reliable fixes detected, manual mode required"
```

`--auto_mode aggressive` flag → 0.5 threshold
`--auto_mode safe` (default) → 0.7 threshold
`--auto_mode manual` → 1.0 (no auto)

## Integration with base pixel_cleanup.py

1. `pixel_cleanup.py --gui` → pixel_cleanup_gui.py invoke
2. `pixel_cleanup.py --smart_merge` → smart_merge.py integrate to apply pipeline
3. `pixel_cleanup.py --region x,y,size` → region_fix.py invoke

Modular import — base hatasız çalışmaya devam etmeli.

## Tests

`tests/test_smart_merge.py`:
- Test 1: 3 dominant cluster image → no outliers detected
- Test 2: image with 5 stray pixels → outliers detected + snapped
- Test 3: gradient image → cluster size analysis correct
- Test 4: confidence skor 0-1 range valid

`tests/test_region_fix.py`:
- Test 1: clean region → no suggestions
- Test 2: region with stray + outlier → multiple suggestions returned
- Test 3: apply_suggestion modifies array correctly
- Test 4: preview rendering doesn't modify original

GUI test (manual smoke): `tests/test_gui_smoke.md` document — how to manually verify GUI launches.

## Skill Update

`~/.claude/skills/pixel-cleanup/SKILL.md` ekle:
- GUI mode usage section
- Smart merge usage section
- Region fix usage section
- Auto-decision logic explanation
- New triggers: "pixel cleanup gui", "interactive pixel fix", "smart palette merge", "click to fix pixel"

## Verification

1. `smart_merge.py` py_compile PASS
2. `pixel_cleanup_gui.py` py_compile PASS (Tkinter import test)
3. `region_fix.py` py_compile PASS
4. Test suite PASS (smart_merge + region_fix)
5. CLI integration: `python pixel_cleanup.py --smart_merge --input X.png` çalışır
6. GUI smoke: `python pixel_cleanup.py --gui` window açılır (verify with screenshot via Codex if possible)
7. SKILL.md updated, new triggers documented

## Çıktı Raporu

`STAGING/codex_pixel_cleanup_extension_DONE.md` yaz:
- Created files (paths)
- Test results
- GUI smoke screenshot (if obtainable)
- Issues/blockers
- Recommended next steps (e.g., user test session, refinements)

git commit otomatik yapma — orchestrator review sonrası.
