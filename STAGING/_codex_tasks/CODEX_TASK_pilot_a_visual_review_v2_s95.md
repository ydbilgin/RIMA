# Codex Task — Pilot A Visual Review v2 (Local PNG, No Network)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_pilot_a_visual_review_v2_s95.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## Bağlam

Önceki dispatch (`b29wjlm2r`) backblaze.pixellab.ai sandbox network erişim olmadığı için PNG download FAIL. **Orchestrator local olarak 4 PNG'yi indirdi.** Codex bu local file'ları kullanacak — network erişim YOK.

## Görev

### Adım 1 — Local PNG'leri Unity Asset'e Kopyala

**Kaynak (orchestrator indirdi):**
- `STAGING/pilot_a_candidates/pilot_a_frame_0_face_NS.png` (10912 bytes — flat tile drift)
- `STAGING/pilot_a_candidates/pilot_a_frame_1_face_EW.png` (7618 bytes — side billboard)
- `STAGING/pilot_a_candidates/pilot_a_frame_2_corner_outer.png` (11647 bytes — corner piece)
- `STAGING/pilot_a_candidates/pilot_a_frame_3_arch_opening.png` (13473 bytes — arch opening)

**Hedef:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/`
- `pilot_a_frame_0_face_NS.png`
- `pilot_a_frame_1_face_EW.png`
- `pilot_a_frame_2_corner_outer.png`
- `pilot_a_frame_3_arch_opening.png`

`AssetDatabase.CopyAsset` veya File.Copy + AssetDatabase.ImportAsset.

### Adım 2 — Import Settings Standardize

Her dosya için:
- spriteMode = Single (1)
- spritePixelsPerUnit = 64
- filterMode = Point (0)
- alphaIsTransparency = true
- wrapMode = Clamp
- spriteAlignment = Custom (9)
- spritePivot = (0.5, 0.03125) — bottom-center 4px padding (128px height assumed)
- spriteMeshType = FullRect (0)
- maxTextureSize = 256

`AssetDatabase.ImportAsset` her dosya.

### Adım 3 — Sahneye Yerleştir

PathC_BaseTest.unity sahnesinde yeni test parent: `PilotATest_S95v2` (scene root).

4 instance yerleştir:
- Frame 0 (face_NS): cell (5, 5, 0) → `Grid.CellToWorld`
- Frame 1 (face_EW): cell (7, 5, 0)
- Frame 2 (corner_outer): cell (9, 5, 0)
- Frame 3 (arch_opening): cell (11, 5, 0)

Her instance:
- SpriteRenderer.sortingLayerName = "Entities"
- sortingOrder = round(-transform.position.y * 100)
- Transform.position = `Grid.CellToWorld(cell)` direkt
- Transform.localScale = (1, 1, 1)

### Adım 4 — Scene View Screenshot

Camera framing: 4 wall + ground tiles görünür şekilde. Camera orthographic size 5-7.

Screenshot: `STAGING/pilot_a_visual_review_v2.png` (1920×1080)

**ÖNEMLİ:** Camera değişikliği geri al (Scene save edilmiyor, kalıcı değil).

### Adım 5 — Cleanup

`PilotATest_S95v2` GameObject delete (sahne dirty olmasın).

**ASSET PNG'ler kalır** (asset folder'da, future use için).

**Scene SAVE ETME.**

## Output Format

```markdown
# Pilot A Visual Review v2 — Codex Report

## PNG Copy
- 4 PNG copied to Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/: PASS

## Unity Import
- 4 PNG import settings verified: PASS / FAIL
- spritePivot pixel (W*0.5, ~4) verify per file: PASS

## Scene Placement
- PilotATest_S95v2 created, 4 instances at cells (5,5)-(11,5)
- sortingLayer Entities, Y-sort applied

## Screenshot
- STAGING/pilot_a_visual_review_v2.png: written
- Camera framing: includes 4 walls + surrounding ground

## Cleanup
- PilotATest_S95v2 deleted: YES
- PNG assets kept in Assets/: YES
- Scene dirty: NO

## Açık Sorular
- ...
```

## Hard Constraints

- **Network erişim YOK** — sadece local file ops.
- **Auto-commit YOK.**
- **Scene SAVE ETME** — test instance create+delete, kalıcı değil.
- **PNG asset folder kal** — silmeyin (future use için: Batch 1.1b face_NS yeniden gen veya finalize).
- **BLOCKED if unclear:** Grid component erişim sorunu, sahne kapalı vs durdur.
