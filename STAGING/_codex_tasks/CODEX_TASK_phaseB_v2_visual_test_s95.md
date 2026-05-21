# Codex Task — Faz B v2 Visual Test (alignment fixed, layer set canonical)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_phaseB_v2_visual_test_s95.md`
> **Geri dönülebilir:** Test instance create+delete, scene save ETME.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## Önkoşul Doğrulama (atomic cleanup sonrası)

Önce bu dört şartı doğrula, biri yanlışsa BLOCKED:
1. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01` — sprite.pivot pixel = (64, 4)
2. Sahne `PathC_BaseTest.unity` Floor_Tilemap.sortingLayerName = "Ground"
3. Sorting layer set: ["Default", "Ground", "Walls", "Entities", "VFX"] (başka layer yok)
4. Console clean

Geçerse devam.

## Görev

Mevcut wall sprite (`act1_wall_straight_horizontal_v01`, alignment fixed pivot) PathC_BaseTest sahnesinde **5 variant** test, screenshot, Opus verdict confirm.

## Test Setup

1. **Sahne:** PathC_BaseTest.unity (NOT SAVED)
2. **Test parent:** `WallSeatingTest_S95v2` (scene root, test cleanup için)
3. **Base cell:** Grid (4, 4, 0) → `Grid.CellToWorld` ile world pos al
4. **Asset:** `act1_wall_straight_horizontal_v01.png` (pivot artık 0.5, 0.0313 Custom)

## 5 Variant

Önceki Faz B'den aynı plan, sadece **layer adları güncel** (Floor → Ground):

| # | Position offset | SortingLayer | SortingOrder | Beklenen |
|---|---|---|---|---|
| 1 | (0, 0, 0)      | **Entities** | round(-y*100) | **OPUS VERDICT** — foot diamond alt kenarına oturur, karakter doğru sırada |
| 2 | (2, +0.25, 0)  | Entities | round(-y*100) | Wall yukarıda, yanlış |
| 3 | (4, -0.25, 0)  | Entities | round(-y*100) | Wall aşağıda, yanlış |
| 4 | (6, 0, 0)      | **Ground** | -100 | Floor layer'da, karakter occluder yanlış |
| 5 | (8, 0, 0)      | Entities | **0** (sabit) | Y-sort'suz, z-fight |

## Placeholder Character Cube

Her variant'ın 2 cell güneyinde (Y'de -1.0 dünya):
- Unity primitive Cube veya Quad (1×1 unit)
- SpriteRenderer veya MeshRenderer
- sortingLayer "Entities", order = `round(-y*100)`
- **Doğrulama:** Variant 1'de cube wall'un önünde gözükmeli (Y'si daha küçük → daha önde)

## Screenshot

5 screenshot:
- `STAGING/phaseB_v2_wall_seating_variant_{1..5}.png`
- Camera ortographic size 5-7, test instance'ları + cube'lar tam görsün
- Screenshot sonrası camera transform restore

## Output Format

```markdown
# Faz B v2 Visual Test — Codex Report

## Önkoşul Verify
- Wall pivot pixel: (64, 4) PASS
- Floor_Tilemap layer: Ground PASS
- Layer set: [Default, Ground, Walls, Entities, VFX] PASS
- Console: clean PASS

## Variant Results

### Variant 1 — OPUS VERDICT (Entities, Y-sort)
- Screenshot: STAGING/phaseB_v2_wall_seating_variant_1.png
- Wall foot: diamond lower edge YES/NO
- Cube vs wall: cube önde YES/NO (Y-sort doğru)
- Verdict: PASS / FAIL

### Variant 2-5 (diagnostic)
...

## Summary
- Best fit: Variant {N}
- Opus verdict confirmed: YES / NO (eğer NO, sebep)
- Açık tartışma: {varsa}

## Cleanup
- WallSeatingTest_S95v2 deleted: YES
- Camera restored: YES
- Scene dirty: NO
```

## Hard Constraints

- **Scene save YASAK** — test instance create+delete, kalıcı değil
- **Asset .meta YASAK** — pivot zaten doğru
- **Sahnede başka şeye dokunma** — sadece WallSeatingTest_S95v2 hiyerarşisi
- **BLOCKED if unclear:** Önkoşul fail varsa durdur, atomic cleanup'ı tekrarla denemez (orchestrator karar verir)
