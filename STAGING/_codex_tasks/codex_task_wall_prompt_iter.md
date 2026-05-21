# CODEX TASK — Wall Sprite Prompt Iteration (Imagegen, PixelLab Pre-Verification)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## Hedef

PixelLab `create_object` ile **wall sprite** üretmeden önce, **gpt-image-1 imagegen** kullanarak optimal prompt formülasyonunu doğrula. PixelLab gen pahalı (20-40 gen/call), imagegen ucuz/hızlı. Prompt confidence kazandıktan sonra PixelLab batch'i ayrı dispatch'te yapılacak.

## Bağlam

RIMA Path C Hybrid pipeline LOCK — wall pieces PixelLab `create_object` ile üretilmişti (S95). Sonuç başarısız: 7-piece pilot A set canvas-fill %52 (içerik kanvasın yarısı dolu, geri kalan transparent). Yan yana paint → görsel gap. Referans hedef = Hades-style Diablo dungeon (continuous brick pattern, mortar lines visible, edge-to-edge content fill).

Hedef tile özellikleri:
- **Modular tile**: yan yana koyulduğunda **görsel olarak kesintisiz duvar**
- **35° isometric side view**, gravity vertical
- **Canvas fill %95+**: content kanvasın 4 kenarına dokunur
- **Mate edges**: sol kenarın brick pattern'i sağ kenarınkinin natural devamı (horizontal tiling)
- **Visible mortar lines**: bloklar arası harç boşluğu görünür
- **Stil**: Hades architectural detail, painterly pixel art, dark granite + cyan rift accent

## Görev — 3 Iteration Imagegen

### Iteration 1 — Base Prompt Test
4 candidate üret, prompt A:

```
A single horizontal stone brick wall section, viewed from a 35-degree
isometric side angle. The wall fills the entire 128 by 128 pixel canvas
edge-to-edge with no transparent borders or empty margins. Wall surface
touches all four canvas edges. Visible mortar lines between blocks form
a continuous brick pattern that would tile seamlessly when placed
adjacent horizontally. The block pattern at the left edge mirrors the
right edge for horizontal tileability. Dark granite gray base with subtle
cyan rift cracks running through mortar joints. Painterly pixel art
style, Hades-style architectural detail, gravity vertical. Pure
transparent background outside the wall.
```

Output: `STAGING/concepts/wall_prompt_iter/iter_1/wall_face_EW_v1_NN.png` (4 PNG, NN=01..04)

Her PNG için **alpha bounds + canvas-fill ratio hesapla** (Python PIL veya başka kütüphane). Sonuç JSON yaz:
```json
{
  "iter": 1,
  "prompt_id": "A",
  "candidates": [
    {"file": "wall_face_EW_v1_01.png", "fill_x": 0.XX, "fill_y": 0.XX, "verdict": "PASS|FAIL"},
    ...
  ]
}
```

PASS kriteri: `fill_x ≥ 0.92 AND fill_y ≥ 0.92`.

### Iteration 2 — Refine Based on Iter 1

Iter 1'de başarısız olan yönlere göre prompt refine et:
- Eğer kanvas dolmuyorsa → "make the wall extend beyond the canvas, then crop", "tile is a section of an infinite wall", "no negative space" gibi eklemeler
- Eğer mortar görünmüyorsa → "explicit dark gaps between each block", "1-pixel-wide mortar lines"
- Eğer brick pattern tile etmiyorsa → "blocks aligned in horizontal rows, vertical seams alternating"

Prompt B'yi yaz, 4 candidate gen. Output `iter_2/`.

### Iteration 3 — Variant Test

Iter 2 PASS olunca, **NS direction** + **corner** test et:
- 4 candidate: 2× face_NS (vertical wall mate edges), 2× corner_outer (mate edges to both face directions)
- Prompt'u NS/corner için adapte et (NS = "vertical wall, mate edges top-bottom"; corner = "L-shape where two wall faces meet at 90°")

Output `iter_3/`. JSON ekle.

## Çıktı

1. `STAGING/concepts/wall_prompt_iter/iter_1/`, `iter_2/`, `iter_3/` — PNG dosyaları
2. `STAGING/concepts/wall_prompt_iter/results.json` — tüm iter'lerin metrics
3. `STAGING/CODEX_DONE_wall_prompt_iter.md` — final report:
   - Hangi iter PASS verdi
   - Final prompt formülü (PixelLab'a yapıştırılacak)
   - Her iter'in lessons learned
   - Tahmini PixelLab başarı oranı

## Kısıtlar

- Pure imagegen (gpt-image-1), PixelLab MCP çağırma
- Output PNG sadece 128×128
- Her iter sonrası kullanıcıya/orchestrator'a output snapshot yaz (CODEX_DONE_partial_*.md mid-progress)
- Toplam 12 candidate PNG, 3 prompt iteration
- BLOCKED if: imagegen API erişimi yok, output write izni yok, alpha analiz kütüphanesi (PIL) bulunamıyor

## Başarı Kriteri

- Iter 3 sonunda en az 2 candidate (face_EW veya face_NS) `fill_x ≥ 0.92 AND fill_y ≥ 0.92`
- Final prompt orchestrator'a hand-off için ready
- PixelLab create_object'e copy-paste edilebilir prompt string
