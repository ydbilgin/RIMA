# Codex Task — pixel_cleanup.py Local Python Tool (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: AI-generated pixel art PNG cleanup tool yaz. PixelLab/AI çıktısındaki stray pixels, palette outliers, semi-transparent edges, color noise tespit + opsiyonel temizleme. **PURE Python, AI call YOK, credit harcamaz, batch sınırsız.**

User strateji: ChatGPT/PixelLab sub aktifken AI gen kullan; sub bittiğinde bu tool standalone yeterli olmalı.

## File Structure

```
F:/Antigravity Projeler/2d roguelite/RIMA/Tools/pixel_cleanup/
├── pixel_cleanup.py          # Main tool
├── palette_examples/
│   ├── rima_shattered_keep.json   # 32-color palette
│   └── default_dungeon.json
├── tests/
│   └── test_pixel_cleanup.py
└── README.md                  # Usage + examples
```

Ek olarak `~/.claude/skills/pixel-cleanup/` klasörüne SKILL.md kopyala (trigger keywords: "pixel cleanup", "clean pixel art", "artifact removal").

## Spec (ChatGPT'nin orijinal brief'i, aynen uygulanacak)

### Komut Satırı

```bash
python pixel_cleanup.py --input input.png --output cleaned.png --palette palette.json --report report.json --preview preview.png

# Batch mode
python pixel_cleanup.py --input_dir assets/raw --output_dir assets/cleaned --palette palette.json
```

### Bağımlılıklar
- Python 3.11+
- Pillow (PIL)
- numpy
- (opsiyonel) scipy.ndimage VEYA scikit-image — connected components için. Mümkünse pure Pillow+numpy ile yap, gerekirse scipy.

### 1. Alpha Cleanup
- `alpha == 0` → background, korunur
- `0 < alpha < 255` → semi-transparent, tespit edilir
- `--alpha_threshold N` (default 128): alpha < N → 0 (transparent), alpha >= N → 255 (opaque)
- `--keep_alpha` flag → VFX assetleri için, alpha cleanup'ı bypass

### 2. Stray Pixel Detection
- Alpha mask üzerinde 8-connected components analizi
- En büyük component = ana asset
- Alanı `--min_component_area N` (default 4) altında olan ve ana asset'ten kopuk component'ler = stray
- `--remove_stray` flag → stray'ları silmek için (default: report only)
- `mask_stray.png` export (kırmızı = stray pixels)

### 3. Palette Outlier Detection
- `palette.json` format:
  ```json
  {
    "name": "rima_shattered_keep",
    "colors": [
      [16, 12, 18],
      [38, 30, 35],
      ...
    ]
  }
  ```
- Her opaque pixel için en yakın palette rengi bul (RGB Euclidean distance)
- Distance > `--palette_tolerance N` (default 24) → outlier
- `mask_palette_outliers.png` export (magenta = outliers)
- `--snap_to_palette` flag → outlier'ları en yakın palette rengine çevir

### 4. Local Color Noise Detection
- Opaque bölgelerde 3×3 neighborhood
- Eğer bir pixel komşularının çoğunluğundan (>=5/8) belirgin farklı (RGB distance > `--noise_threshold` default 40) → suspicious
- Küçük (<= 3 pixel) izole color islands = noise
- `mask_color_noise.png` export (cyan = noise)
- `--fix_color_noise` flag → median filter ile temizle

### 5. Preview Output
`preview.png` üret:
- Orijinal görsel base
- Kırmızı overlay = stray pixels
- Magenta overlay = palette outliers
- Sarı overlay = semi-transparent edges
- Cyan overlay = color noise
- 4 mask'i tek görselde gösterir

### 6. Report JSON

`report.json`:
```json
{
  "image_path": "input.png",
  "image_size": [128, 384],
  "total_opaque_pixels": 12450,
  "semi_transparent_pixel_count": 340,
  "stray_component_count": 5,
  "removed_pixel_count": 0,
  "palette_outlier_count": 23,
  "color_noise_count": 8,
  "bounding_box": [4, 12, 124, 380],
  "recommended_actions": [
    "Run with --remove_stray to clean 5 stray components (12 total pixels)",
    "Palette outliers detected (23). Consider --snap_to_palette.",
    "Edge anti-aliasing detected (340 semi-transparent). Use --alpha_threshold 128."
  ]
}
```

### 7. Güvenlik
- Default: **report only**, hiçbir değişiklik yapmaz
- `--apply_cleanup` flag → değişiklikleri output PNG'ye yazar
- Orijinal dosya **asla** overwrite edilmez
- Output path mevcut dosyaysa hata ver (overwrite koruması) veya `--force` ister

### 8. Ekstra Features
- `--make_palette N` (default 32) → görselden en yaygın N rengi extract, `suggested_palette.json` üret (palette.json yokken)
- RGB distance function ayrı fonksiyon olarak (perceptual color distance opsiyonel — CIEDE2000 future)

## Code Quality Requirements

- Pure Python, type hints (3.11+ uyumlu)
- Modular: cleanup.py'da ayrı fonksiyonlar:
  - `load_image(path) -> np.ndarray`
  - `detect_alpha_issues(arr, threshold) -> mask`
  - `detect_stray_components(alpha_mask, min_area) -> List[Component]`
  - `detect_palette_outliers(rgb_arr, palette, tolerance) -> mask`
  - `detect_color_noise(rgb_arr, threshold) -> mask`
  - `generate_preview(orig, masks) -> Image`
  - `apply_cleanup(arr, masks, flags) -> np.ndarray`
- Yorum satırları: WHY-only, WHAT obvious'lar yok
- Error handling: dosya yok, palette format hatası, vb.
- CLI: argparse, help mesajları açık

## Sample Palette (RIMA Shattered Keep, 32 renk)

`palette_examples/rima_shattered_keep.json` üret. Renkler:
- Stone wall base: #1A1612, #2B2118, #3A2E22, #45382B
- Stone wall highlight: #5A4936, #6B5A47, #7C6B58
- Mortar dark: #0F0C0A, #15110D
- Cyan rift: #00DDFF, #44E5FF, #7FF0FF
- Cyan rift dim: #006B85, #003D4D
- Torch warm: #FF8000, #FFA040, #FFD080
- Skull/bone: #E0D5B0, #B8A878
- Blood/red: #8B0000, #C82020
- Ritual gold: #FFD700, #B8941F
- Pure black: #000000
- Pure white: #FFFFFF
- Mid grey: #6E6963

(32 renk için ek tonlar — RIMA palette mantığında ayarla)

## Tests

`tests/test_pixel_cleanup.py`:
- Pillow + numpy ile test PNG'leri yarat (no external dependencies)
- Test 1: clean image → 0 stray, 0 outlier
- Test 2: image with stray dots → detection sayısı doğru
- Test 3: image with semi-transparent edges → detect + alpha_threshold fix
- Test 4: image with off-palette pixels → outlier sayısı doğru
- Test 5: batch mode → 5 input → 5 output + 5 report
- Test 6: `--apply_cleanup` flag → output file written
- Test 7: report_only → no output file written

Run: `python -m pytest tests/test_pixel_cleanup.py -v`

## README.md

Sample usage docs:
- Quick start (1 PNG)
- Batch usage
- Common flags
- Palette format
- Example output preview
- RIMA integration (Codex auto-cleanup in wall production pipeline)

## Claude Skill Entry

`~/.claude/skills/pixel-cleanup/SKILL.md`:

```markdown
---
name: pixel-cleanup
description: Clean AI-generated pixel art PNG artifacts (stray pixels, palette outliers, semi-transparent edges, color noise). Pure Python, no AI cost. Use on PixelLab/AI gen output before Unity import. Triggers on 'pixel cleanup', 'clean pixel art', 'artifact removal', 'palette snap', 'stray pixel detection'.
---

# pixel-cleanup — AI Pixel Art Artifact Cleanup

## Use cases
- AI-generated pixel art has stray dots, semi-transparent edges, off-palette colors
- Need to verify pixel-perfectness before Unity import
- Batch process 60+ pieces in seconds

## Tool location
`F:/Antigravity Projeler/2d roguelite/RIMA/Tools/pixel_cleanup/pixel_cleanup.py`

## Workflow
1. Run with `--input X.png` (report only by default)
2. Review `report.json` recommended_actions
3. Re-run with `--apply_cleanup` flags

## Common flags
- `--remove_stray` — drop isolated pixel islands
- `--snap_to_palette` — quantize to allowed palette
- `--fix_color_noise` — median filter local noise
- `--alpha_threshold 128` — binarize alpha
- `--apply_cleanup` — write output (default: report only)

## Examples
```bash
# Inspect
python pixel_cleanup.py --input wall_nw_mid_plain.png --palette rima_shattered_keep.json --report report.json --preview preview.png

# Apply full cleanup
python pixel_cleanup.py --input wall_nw_mid_plain.png --output wall_nw_mid_plain_clean.png --palette rima_shattered_keep.json --remove_stray --snap_to_palette --apply_cleanup
```
```

## Verification

1. `Tools/pixel_cleanup/pixel_cleanup.py` syntax-valid
2. `python pixel_cleanup.py --help` çalışıyor
3. Test suite PASS (7/7)
4. Sample palette JSON valid format
5. `~/.claude/skills/pixel-cleanup/SKILL.md` kopyalandı
6. README.md mevcut

## Çıktı Raporu

`STAGING/codex_pixel_cleanup_DONE.md` yaz:
- Created files list (paths)
- Test result (7/7 PASS bekleniyor)
- Pilot run on 1 wall asset (wall_nw_mid_plain.png) — report.json değerleri raporla
- Issues / blockers

git commit otomatik yapma — orchestrator review sonrası.
