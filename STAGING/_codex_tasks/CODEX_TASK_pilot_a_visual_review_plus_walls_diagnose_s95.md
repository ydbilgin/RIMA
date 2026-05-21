# Codex Task — Pilot A Visual Review + Walls 55-vs-52 Diagnose (S95)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_pilot_a_visual_review_plus_walls_diagnose_s95.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## İki Bölüm — Tek Atomik Dispatch

### Bölüm 1 — Walls 55 vs 52 Mismatch Diagnose

Önceki Codex Phase A raporu: `Walls` layer 55 renderer (Antigravity 52 demişti). +3 fark sebebi:

**Adım:**
1. PathC_BaseTest.unity sahnesinde `Walls` sortingLayerName ile tüm SpriteRenderer'ları enumerate et
2. Liste raporla: GameObject name, transform.position, parent path, prefab GUID
3. Antigravity'nin tahmini 52 GameObject ile cross-check: hangi 3 extra var?
4. Olası açıklamalar:
   - Antigravity sayımı yanlış (basit sayım hatası)
   - Antigravity tarafından runtime'da spawn olan 3 instance (script-generated)
   - Hidden veya disabled GameObject'ler dahil edilmiş
   - SpriteRenderer + TilemapRenderer karışımı (Antigravity sadece sprite saydı)

Raporla: 3 ekstra renderer kim, açıklama ne.

### Bölüm 2 — Pilot A Visual Review

**Pilot A object_id:** `54c88cfe-bf99-4d22-9964-65eb236380e6`
**Status:** review (4 candidates)

User Pilot A görsel kalitesini Unity sahnesinde yerleştirip değerlendirmek istiyor:
> "corner outer olarak m ıyapmak lazım şu an tutarlı şekilde geldi mi sence incele resimleri indir yerleştir bakalım"

**Adım 1 — PNG Download:**

4 PNG'yi indir (curl, wget veya UnityWebRequest):
- Frame 0: `https://backblaze.pixellab.ai/file/pixellab-characters/objects/f587b47a-7c0e-4f37-a6c9-7d311a2c935f/54c88cfe-bf99-4d22-9964-65eb236380e6/rotations/frame_0.png`
- Frame 1: `https://backblaze.pixellab.ai/file/pixellab-characters/objects/f587b47a-7c0e-4f37-a6c9-7d311a2c935f/54c88cfe-bf99-4d22-9964-65eb236380e6/rotations/frame_1.png`
- Frame 2: `https://backblaze.pixellab.ai/file/pixellab-characters/objects/f587b47a-7c0e-4f37-a6c9-7d311a2c935f/54c88cfe-bf99-4d22-9964-65eb236380e6/rotations/frame_2.png`
- Frame 3: `https://backblaze.pixellab.ai/file/pixellab-characters/objects/f587b47a-7c0e-4f37-a6c9-7d311a2c935f/54c88cfe-bf99-4d22-9964-65eb236380e6/rotations/frame_3.png`

İndirme hedefi: `STAGING/pilot_a_candidates/` klasörü
- pilot_a_frame_0_face_NS.png
- pilot_a_frame_1_face_EW.png
- pilot_a_frame_2_corner_outer.png
- pilot_a_frame_3_arch_opening.png

**Adım 2 — Unity Import:**

`Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/` klasörü oluştur, 4 PNG'yi kopyala.

Import setting (her dosya):
- spriteMode = Single (1)
- spritePixelsPerUnit = 64
- filterMode = Point (0)
- alphaIsTransparency = true
- wrapMode = Clamp
- spriteAlignment = Custom (9)
- spritePivot = (0.5, 0.03125) — bottom-center 4px padding
- spriteMeshType = FullRect (0)

AssetDatabase.ImportAsset her dosya.

**Adım 3 — Sahneye Yerleştir:**

PathC_BaseTest.unity sahnesinde yeni test parent: `PilotATest_S95v2`. 4 instance yerleştir:

Grid cell base = (5, 5, 0). Cell offset:
- Frame 0 (face_NS): cell (5, 5) → `Grid.CellToWorld`
- Frame 1 (face_EW): cell (7, 5) → +2 X
- Frame 2 (corner_outer): cell (9, 5) → +4 X
- Frame 3 (arch_opening): cell (11, 5) → +6 X

Her instance:
- SpriteRenderer.sortingLayerName = "Entities" (Opus verdict L2b için)
- sortingOrder = round(-y * 100)
- Pivot Custom (.meta'dan)
- Transform.position = `Grid.CellToWorld(cell)` direkt

**Adım 4 — Scene View Screenshot:**

Camera framing: 4 wall + ground tiles görünür şekilde
- Screenshot: `STAGING/pilot_a_visual_review.png` (1920×1080)
- Sahne SAVE ETME (test instance'lar persist olmasın, ama screenshot kalsın)

**Adım 5 — Cleanup:**

PilotATest_S95v2 hierarchy delete (sahne dirty kalmasın).

**NOT:** `Assets/Art/AssetPacks/.../pilot_a_test/` PNG'leri ve .meta'ları kalır (asset olarak), sadece sahne instance'ları silinir.

## Output Format

```markdown
# Pilot A Visual Review + Walls Diagnose — Codex Report

## Bölüm 1: Walls 55 vs 52 Mismatch
### Enumerate Result
- Total Walls renderer: 55
- GameObject list:
  - 1. wall_face_X (Grid/Walls/...): pos=(...)
  - ...
  - 55. ...
### Antigravity Cross-Check
- Antigravity raporu 52 — possible causes:
  - Cause X (most likely): ...
- Verdict: harmless / needs cleanup

## Bölüm 2: Pilot A Visual Review
### PNG Download
- 4 frame downloaded: PASS / FAIL
- Sizes: 128×128 each
- STAGING/pilot_a_candidates/ klasörü oluşturuldu

### Unity Import
- 4 dosya Assets/Art/.../pilot_a_test/ altında
- Import setting verify: pivot (64, 4) all PASS

### Scene Placement
- 4 instance PilotATest_S95v2 altında, Grid cell (5,5)-(11,5) layout
- sortingLayer Entities, Y-sort doğru

### Screenshot
- STAGING/pilot_a_visual_review.png yazıldı
- Camera framing: 4 wall + ground

### Cleanup
- PilotATest_S95v2 deleted YES
- PNG asset'ler korundu (asset folder'da)
- Scene dirty: NO

## Açık Sorular
- ...
```

## Hard Constraints

- **Auto-commit YOK.**
- **Sahne SAVE ETME** (instance test, cleanup zorunlu, PNG asset kalır).
- **PNG download** sadece STAGING/'a + asset folder'a. Başka yere YASAK.
- **BLOCKED if unclear:** PNG indirme erişimi yok veya Grid.CellToWorld erişimi yoksa durdur.
