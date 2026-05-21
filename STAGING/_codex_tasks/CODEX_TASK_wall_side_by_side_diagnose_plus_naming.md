# Codex Task — Yan Yana Wall Paint Diagnose + İsim Revize (S95)

> **Profile:** any active cx profile (Unity açık)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_wall_side_by_side_diagnose_plus_naming.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## User Direktifi (S95 LATE NIGHT 2026-05-20)

> "hala hata var yan yana koyamıyorum block kısmı olduğundan üst üste koyabiliyorum ama yan yana olmuyor. ayrıca isimlendirmeler doğru olmalı bu poroblemi de çöz"

## İki Sorun

### Sorun 1 — Yan Yana Wall Paint Engelleniyor

**Tahmin (orchestrator):**
- Wall sprite 128×128 px @ PPU 64 → world size 2×2 unit
- Grid cellSize 0.94 → sprite 2 cell genişliğinde
- `CollisionMode.WallBlock` formula: `BoxCollider2D.size = spriteWidth * scale, 0.8f` → collider 2 cell wide
- Yan yana 2 wall paint → 2 collider çakışıyor → physics push-out veya cell-occupied check engelliyor
- "Üst üste konabiliyor" = aynı cell'e 2. paint → yığılma OK (sprite z-stack)
- "Yan yana olmuyor" = adjacent cell'e paint → engellenmiş veya görsel olarak iç içe geçmiş

**Tanı adımları:**

1. **PathC_BaseTest.unity** sahnesinde aktif Pilot A wall'ları inceleme:
   - 8 mevcut Pilot A wall (Bölüm 2 dispatch sonrası) position listesi
   - Adjacent paint denemesi: Painter'da pilot_a_wall_face_EW seç → cell (5, 5)'e paint → sonra (6, 5)'e paint → ne oluyor?
   - BoxCollider2D.size + offset değerleri her instance için
   - Sprite world width: actual `SpriteRenderer.bounds.size.x`
   - Grid cellSize: actual

2. **PaintPrefab kodunda cell-occupied check var mı?**
   - Line 1453 civarı: aynı cell'de zaten prefab varsa engelleme var mı?
   - `snapToGrid` mantığı: cell-based unique placement mı?

3. **Collider intersect:**
   - Adjacent paint denemesinde 2 BoxCollider2D çakışıyor mu? Physics2D.OverlapBox check
   - Çakışıyorsa: physics auto push wall'ları üst üste yığar mı?

4. **Tanı raporu:** sorunun root cause'u kesin tespit + fix önerisi

**Olası fix yolları (sadece öneri, uygulama):**
- A) `CollisionMode.WallBlock` formula değişikliği: `BoxCollider2D.size = (cellWidth, 0.8f)` — sprite görsel taşar ama collider 1 cell wide (Hades pattern)
- B) Sprite import re-pivot — wall sprite'ı cell-width'e crop / scale
- C) Painter cell-overlap allowance — 2 wall yan yana cell'e konulabilir (collider intersect tolerable)
- D) Auto-Wang sistemi: yan yana paint = face_EW continuous chain (Wang tile sistem zaten yatay-yatay birleşmeli)

### Sorun 2 — İsimlendirme

User: "isimlendirmeler doğru olmalı bu poroblemi de çöz"

**Tanı:**
- Mevcut asset adları: `pilot_a_wall_face_EW.png`, `pilot_a_wall_corner_outer.png`, `pilot_a_wall_arch_opening.png` — "pilot_a_" prefix gereksiz mi?
- Prefab adları: `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_*.prefab` — aynı sorun
- Palette display name: Codex'in `CleanName` (line 280) ne yapıyor? Pilot A prefab adları palette'te ne olarak görünüyor?
- Sahne Hierarchy: paint edilen wall'lar `Walls` parent altında ne isimle görünüyor?

**Olası fix yolları (öneri):**
- A) Pilot A prefab/PNG'leri rename: `wall_face_EW.png` (pilot_a prefix sil) — clean canonical naming
- B) Display name override: palette'te "Duvar - Yan Yüz" gibi Türkçe label (CleanName extension)
- C) Sahne instance suffix: "pilot_a_wall_face_EW (3)" yerine "Duvar_Yan_03" gibi
- D) AssetPostprocessor naming validator: yeni asset import'unda standart adlandırma uygula

**Tanı raporu:** Mevcut naming inconsistency listele + user-friendly öneri.

## Görev (Sadece Tanı + Öneri, UYGULAMA YOK)

1. Sorun 1 tanı raporu (root cause kesin + 3-4 fix yolu trade-off)
2. Sorun 2 naming envanter + user-friendly öneri
3. Test sonuçları (sahnede adjacent paint denemesi + collider size raporu)

**UYGULAMA YOK** — sadece rapor. User karar verecek hangi fix uygulanır.

## Output Format

```markdown
# Yan Yana Wall + İsim Diagnose — Codex Report

## Sorun 1: Yan Yana Wall Paint
### Tanı Verileri
- Cell width: X
- Sprite world width: Y
- BoxCollider2D.size (WallBlock formula): (W, H)
- Mevcut 8 Pilot A wall instance positions: [list]
- Adjacent paint test (5,5) → (6,5): X cm overlap, ne oldu?

### PaintPrefab Cell Check
- Line 1453: aynı cell occupancy check VAR / YOK
- snapToGrid logic: cell-based unique / overlap allow

### Root Cause
- Specific reason: ...

### Fix Önerisi (3-4 yol, trade-off)
- A) ...
- B) ...
- C) ...
- D) ...

## Sorun 2: İsimlendirme
### Mevcut Naming Envanteri
- PNG: [list with adlar]
- Prefab: [list]
- Palette display: ne görünüyor (CleanName output)
- Hierarchy instance: ne görünüyor (suffix sayım)

### Tutarsızlıklar
- ...

### User-Friendly Öneri
- ...
```

## Hard Constraints

- **UYGULAMA YOK** — sadece tanı + öneri rapor.
- **Sahnede deneme paint OK** (test için), ama cleanup zorunlu (paint edilen test wall'lar sil, sahne dirty=NO).
- **Asset/prefab rename YASAK** bu task'ta.
- **Auto-commit YOK.**
- **BLOCKED if unclear:** Painter erişim sorun, sahne kapalı, vb. STOP.
