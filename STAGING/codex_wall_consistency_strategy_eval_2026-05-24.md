# Codex Task — Wall Pack Consistency Strategy Independent Eval (2026-05-24)

ACTIVE RULES: (1) think before reviewing (2) min output, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: 56-60 piece modüler wall pack üretiminde piece'ler arası **kalınlık/baseline/palette/brick row** consistency'sini garantilemek için orchestrator'ın önerdiği 5-katman stratejiyi independent olarak değerlendir. Strateji'nin zayıf noktalarını tespit et. pixel_align.py tool spec'ini netleştir.

---

## Konteks

**Mevcut 17 asset (bdkrtgasb commit fd00ad23) — baseline measurement gerekli:**
- 11 wall asset (128×384, 256×384, 64×384) at `STAGING/concepts/fractured_chamber/iso_assets/`
- 6 floor asset (128×64)
- Already programmatically downscaled via PIL nearest-neighbor (consistent SIZE)
- BUT içerik consistency (brick row count, baseline row, top cap, palette drift) measured DEĞİL

**Brief önerisi (ChatGPT'den):**
- ~60 piece: 10 connector + 12 wall span + 14 seam + 12 prop + tall variants
- Diamond/irregular room footprint
- Connector-column-driven with seam overlays
- Detay: `STAGING/codex_wall_system_brief_eval_2026-05-24.md` Codex eval'i (verdict: PARTIAL — pilot first)

**Sheet üretim planı (PixelLab S-XL batch):**
- Sheet A: 8 connector columns, 256×768 (4×2 grid, her cell 64×384)
- Sheet B: 8 short/medium spans, 256×768
- Sheet C: 4 long spans, 768×384 (boyut native risk — Codex eval'de flagged)
- Sheet D: 16 seam pieces, 256×768
- Sheet E: 4 specialty walls, 256×768
- Sheet F: 16 socket props, 256×256

---

## Orchestrator'ın 5-Layer Consistency Stratejisi

### Layer 1 — Batch Sheet Generation
Her sheet'i tek generation'da üret → AI otomatik tutarlılık (palette, brick texture, lighting). Sonra programmatic slice.

### Layer 2 — Style Reference Lock (Cross-Sheet)
Sheet A (Connector Columns) = ANCHOR. Sheet B-F'in her birinde A'yı PixelLab "Style Reference" slot'a koy → palette + visual continuity.

### Layer 3 — Programmatic Crop & Align (pixel_align.py — yazılacak)
- Force-crop her piece'i 384px tall'a (PIL)
- Baseline detection: en alttaki opaque row → row 380'e align
- Top cap detection: en üstteki opaque row → row 0
- 4px shadow strip altta standart

### Layer 4 — Palette Snap (pixel_cleanup + smart_merge)
32-color RIMA palette'e quantize. Outlier'lar en yakın palette rengine snap.

### Layer 5 — Reference Template Overlay (önerilen)
Her gen'e 384px boş template ver:
- "Wall area = rows 12-372"
- "Top cap = rows 0-12"
- "Baseline = rows 372-384"
AI shape'i bu boundary'lere doldurur.

---

## Görev — Bu 5 Soruyu Cevapla

Her cevap < 150 kelime.

### Q1 — 5-layer strategy validation

Bu 5 katman gerçekten consistency'yi garanti eder mi? AI generation gerçekten:
- (a) batch sheet'te tek style üretir mi (Layer 1)?
- (b) style reference cross-sheet palette korur mu (Layer 2)?
- (c) programmatic baseline detection PixelLab S-XL çıkışlarında çalışır mı (Layer 3)?
- (d) reference template overlay PixelLab'de gerçekten "boundary lock" yapar mı yoksa AI taşar mı (Layer 5)?

İmkansız VEYA zayıf olan katmanları işaretle.

### Q2 — Mevcut 17 asset baseline measurement

Hızlı bir PIL script ile şu metrik'leri ölç:
- Her wall asset için baseline row (en alttaki opaque row)
- Top cap row (en üstteki opaque row)
- Brick row count (kabaca, dark-light alternating pattern)
- Palette renk sayısı (unique colors)

Sonuçları tablo halinde ver. Mevcut 17 asset zaten ne kadar tutarlı?

### Q3 — pixel_align.py function spec

Aşağıdaki fonksiyonların algoritma + signature'ını ver:

```python
def detect_baseline_row(image: np.ndarray, alpha_threshold: int = 128) -> int
def detect_top_cap_row(image: np.ndarray, alpha_threshold: int = 128) -> int
def align_to_baseline(image: np.ndarray, target_row: int = 380) -> np.ndarray
def detect_brick_rows(image: np.ndarray, palette: List[RGB]) -> int
def enforce_canvas_size(image: np.ndarray, w: int, h: int, pad_color: RGB = (0,0,0,0)) -> np.ndarray
def verify_consistency(pieces: List[np.ndarray], tolerance: int = 2) -> Dict[str, Any]
```

Algoritma detayı:
- Baseline detection edge case: alpha-only image, transparent BG karışıklığı
- Brick row detection: horizontal line pattern detection, varyans tolerance
- Verify consistency: rapor formatı

### Q4 — Effort estimate

pixel_align.py implementation: Codex effort tahminin nedir?
- Pure Python (PIL + numpy)
- Test suite gerekiyor (5-7 test)
- pixel_cleanup.py'ye CLI flag entegrasyonu
- Tahminin: kaç saat?

### Q5 — Alternative if strategy weak

Eğer 5-layer strateji yetersizse, alternatif bir consistency strategy öner. Örneğin:
- Tek tek gen + programmatic stretch
- Manuel Aseprite cleanup pipeline
- Tile-based generation (PixelLab create_tiles_pro)
- Yoksa "stratejisi yeterli"

---

## Çıktı

`STAGING/codex_wall_consistency_eval_2026-05-24.md` yaz:

```markdown
# Wall Pack Consistency Strategy — Codex Independent Eval (2026-05-24)

## Q1 — 5-Layer Validation
[answer]

## Q2 — Baseline Measurement (Mevcut 17 Asset)
| Asset | Baseline | Top Cap | Brick Rows | Unique Colors |
|---|---|---|---|---|
| wall_nw_mid_plain.png | ... | ... | ... | ... |
...

## Q3 — pixel_align.py Function Spec
[detailed function signatures + algorithm descriptions]

## Q4 — Effort Estimate
[hours]

## Q5 — Alternative if Strategy Weak
[answer or "strategy sufficient"]

## Verdict
- **GO / GO_WITH_MODIFICATIONS / RECONSIDER**
- Strongest layer: [...]
- Weakest layer: [...]
- Next dispatch order: [...]
```

git commit otomatik yapma — orchestrator review sonrası.
