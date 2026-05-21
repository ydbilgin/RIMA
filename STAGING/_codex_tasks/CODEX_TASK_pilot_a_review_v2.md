# Codex Task — Pilot A Visual Review v2 (Local PNG, No Network)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_pilot_a_visual_review_v2.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## Bağlam

Önceki dispatch backblaze.pixellab.ai network erişim yoktu. **Orchestrator 4 PNG'yi local'e indirdi.** Codex bu local file'lardan asset folder'a kopyalayacak. Network erişim YOK gerek.

## 4 Local PNG (orchestrator indirdi)

- `STAGING/pilot_a_candidates/pilot_a_frame_0_face_NS.png` (10912 bytes — flat tile drift)
- `STAGING/pilot_a_candidates/pilot_a_frame_1_face_EW.png` (7618 bytes — side billboard)
- `STAGING/pilot_a_candidates/pilot_a_frame_2_corner_outer.png` (11647 bytes — corner)
- `STAGING/pilot_a_candidates/pilot_a_frame_3_arch_opening.png` (13473 bytes — arch)

## Görev — 5 Adım

### 1. Kopyala

`Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/` klasörü oluştur. 4 PNG'yi kopyala (File.Copy + AssetDatabase.ImportAsset).

### 2. Import Settings

Her dosya için:
```
spriteMode = Single (1)
spritePixelsPerUnit = 64
filterMode = Point (0)
alphaIsTransparency = true
wrapMode = Clamp
spriteAlignment = Custom (9)
spritePivot = (0.5, 0.03125)
spriteMeshType = FullRect (0)
maxTextureSize = 256
```

### 3. Sahneye Yerleştir

PathC_BaseTest.unity'de yeni `PilotATest_S95v2` parent (scene root). 4 instance:
- Frame 0 (face_NS): cell (5, 5, 0)
- Frame 1 (face_EW): cell (7, 5, 0)
- Frame 2 (corner_outer): cell (9, 5, 0)
- Frame 3 (arch_opening): cell (11, 5, 0)

Her instance:
- `transform.position = Grid.CellToWorld(cell)` direkt
- `SpriteRenderer.sortingLayerName = "Entities"`
- `sortingOrder = round(-y * 100)`

### 4. Screenshot

Scene view camera framing: 4 wall + ground tiles göster. Orthographic size 5-7.

Output: `STAGING/pilot_a_visual_review_v2.png` (1920×1080)

Camera transform geri al (kalıcı değişiklik yok).

### 5. Cleanup

`PilotATest_S95v2` delete. Sahne SAVE ETME (test instance kalıcı değil).

**PNG asset'ler kalır** (asset folder'da, Batch 1.1b veya future use için).

## Output Format

```
# Pilot A Visual Review v2 — Codex Report

## Copy + Import
- 4 PNG → Assets/.../pilot_a_test/ : PASS
- Pivot pixel (64, 4) per file: PASS

## Scene Placement
- 4 instance Entities layer, cells (5,5)-(11,5)

## Screenshot
- STAGING/pilot_a_visual_review_v2.png written

## Cleanup
- PilotATest_S95v2 deleted: YES
- PNG assets preserved: YES
- Scene dirty: NO
```

## Hard Constraints

- Network erişim YOK — sadece local file
- Auto-commit YOK
- Scene SAVE ETME
- PNG asset'leri silmeyin (kalır)
