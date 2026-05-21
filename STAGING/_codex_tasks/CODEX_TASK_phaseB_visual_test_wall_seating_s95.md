# Codex Task — Faz B Visual Test: Wall Seating (5 variant + screenshot)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_phaseB_visual_test_wall_seating_s95.md`
> **Geri dönülebilir:** Test sonrası sahnedeki tüm test instance'ları sil. Scene save ETME (user manual save). Git temiz kalır.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — sadece test instance create+delete (4) BLOCKED if unclear.

## Görev

Mevcut wall sprite'ı (`act1_wall_straight_horizontal_v01`, yeni pivot 0.5,0.0313 ile re-imported) `PathC_BaseTest.unity` sahnesinde **5 farklı konfigürasyonda** yerleştir, her birinden **scene view screenshot** al, hangisi isometric diamond tile alt kenarına tam oturuyor rapor et.

## Hazırlık

1. **Sahneyi aç:** `Assets/Scenes/Demo/PathC_BaseTest.unity`
2. **Test parent:** Scene root altında yeni GameObject `WallSeatingTest_S95` oluştur (test cleanup için kolay reference). Bunun altında 5 child wall instance.
3. **Target cell:** Grid hücresi (4, 4, 0). `Grid.CellToWorld(new Vector3Int(4, 4, 0))` ile world pos al → her variant bu base'den offsetlenecek (X'te 2 unit ara, Y'de offset variant).
4. **Asset:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png`

## 5 Variant

| # | Position | Pivot (TextureImporter sonrası) | SortingLayer | SortingOrder | Beklenen |
|---|---|---|---|---|---|
| 1 | base + (0, 0, 0)     | (0.5, 0.0313) (yeni) | Entities | round(-y*100) | **OPUS VERDICT** — foot tam diamond alt kenarına oturur |
| 2 | base + (2, 0.25, 0)  | (0.5, 0.0313)        | Entities | round(-y*100) | Wall yukarıda kalıyor, foot tile üstüne giriyor (yanlış) |
| 3 | base + (4, -0.25, 0) | (0.5, 0.0313)        | Entities | round(-y*100) | Wall aşağı batık, foot tile altına geçiyor (yanlış) |
| 4 | base + (6, 0, 0)     | (0.5, 0.0313)        | **Floor** | -100 | Y-sort yok, karakter önüne geçebilir veya hep arkada |
| 5 | base + (8, 0, 0)     | (0.5, 0.0313)        | Entities | **0** (sabit) | sortingOrder Y-sort'suz, karakter occluder yanlış |

(Variant 2-5 contrast/diagnostic — beklenen "yanlış", görsel olarak doğrulanacak.)

## Screenshot Stratejisi

- **Yöntem:** UnityMCP `screenshot` veya `get_scene_view_image` (varsa). Yoksa Camera.targetTexture + Texture2D.ReadPixels ile manuel:
  ```csharp
  var cam = Camera.main;
  var rt = new RenderTexture(1920, 1080, 24);
  cam.targetTexture = rt;
  cam.Render();
  RenderTexture.active = rt;
  var tex = new Texture2D(1920, 1080);
  tex.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
  tex.Apply();
  File.WriteAllBytes("STAGING/phaseB_variant_X.png", tex.EncodeToPNG());
  ```
- **5 ayrı screenshot:** `STAGING/phaseB_wall_seating_v01_variant_{1..5}.png`
- **Camera framing:** Test instance'ların tamamını görecek şekilde camera pozisyonunu geçici ayarla. Ortographic size 4-6. Test sonrası **camera değişikliğini geri al** (undo).

## Karakter Mock (sortingOrder doğrulama için)

Variant 4 ve 5'in karakter Y-sort etkisini test etmek için her variant'ın iki cell altına bir **placeholder karakter cube** koy:
- 1×1 unit cube veya sprite (`Assets/Art/character_test_placeholder` varsa kullan, yoksa primitive cube)
- sortingLayer `Entities`, order `round(-y*100)`
- Variant 1: karakter wall'un önünde mi (Y-sort doğru) — beklenen YES
- Variant 4: karakter wall'un arkasında kalır (Floor layer wall'ı arkada) — beklenen YES (yanlış sonuç görünür)
- Variant 5: karakter Y'sine göre değil 0'a göre — yanlış z-fight

## Output Format

```markdown
# Faz B Visual Test — Wall Seating Codex Report

## Test Setup
- Sahne: PathC_BaseTest.unity (NOT SAVED — user manual save)
- Test parent: WallSeatingTest_S95 (silinecek)
- Base cell: (4, 4, 0), Grid.CellToWorld = (Wx, Wy, 0)
- Asset: act1_wall_straight_horizontal_v01, pivot (0.5, 0.0313)

## Variant Results

### Variant 1 — OPUS VERDICT (pivot 0.0313, offset 0, Entities, Y-sort)
- Screenshot: `STAGING/phaseB_wall_seating_v01_variant_1.png`
- Visual: foot pixel diamond'un alt kenarında, karakter wall'un önünde
- Verdict: PASS / FAIL (sebep)

### Variant 2 — +0.25 Y offset
...

### Variant 3 — -0.25 Y offset
...

### Variant 4 — Floor sortingLayer
...

### Variant 5 — fixed sortingOrder 0
...

## Summary
- Best fit: Variant {N}
- Opus verdict confirmed: YES / NO
- Açık tartışma: {varsa}

## Cleanup
- WallSeatingTest_S95 GameObject deleted: YES/NO
- Camera transform restored: YES/NO
- Scene dirty flag: NO (user save etmedi)
```

## Cleanup Adımları (HARD)

1. Test bitince **tüm WallSeatingTest_S95 hiyerarşisini sil** (Undo.DestroyObjectImmediate)
2. Camera transform/orthographic size'ı geri al
3. Scene'i **SAVE ETME** — user manual save eder
4. Screenshot dosyaları `STAGING/` klasörüne kal, silme (user inceleyecek)
5. Git diff: 0 dosya değişikliği (sahne save edilmedi) + 5 yeni PNG (STAGING/ untracked) + 1 .md rapor

## Hard Constraints

- **Sahne save YASAK** — test instance create + delete, save etmeden bitir.
- **Asset .meta değişikliği YASAK** — Wall PNG pivot'u zaten doğru, ekstra import değişikliği yok.
- **Sadece test scope** — başka prefab/asset/scene modifikasyonu yok.
- **BLOCKED if unclear:** Screenshot API erişimi yoksa veya cube primitive yoksa durdur, rapor.
