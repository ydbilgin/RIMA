# D2: Cliff Fix 0 + 6-Layer Architecture LOCK

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md, .claude/PROJECT_RULES.md, kod, STAGING, memory files.

## Amaç
6-layer asset architecture'ı LOCK et ve cliff_mounting görünmüyor sorununu Fix 0 (1-click) ile anında çöz.

## Bağlam (locked by user 2026-05-27 gece)
- Architecture: 6 layer (L1 Floor / L2 Cliff base / L3 Cliff face decor / L4 Walkable decor / L5 Wall blocker / L6 Gameplay obj)
- Sorting orders: 0 / -1 / 50 / 80 / 100 / 150
- Mounting pivot: top-center (cliff'ten asılı sarkık görünüm)
- Spec ref: `STAGING/RIMA_LIVE_TOOL_DECISION.md` Section 2.4 (6-layer table)
- Önceki Opus verdict: `STAGING/UNIFIED_PAINTER_PLAN.md`

## İş kalemleri (sırayla)

### 1. Cliff Fix 0 — 1-click slot reassignment
- `PlayableArena_Test01.unity` sahnesinde `CliffAutoPlacer` component'ini bul
- `cliffTile` field'ını `CliffTile_Hades.asset`'ten **`DirectionalCliffTile_Hades.asset`**'e değiştir
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` (DOKUNMA — sadece serialized field reference değişir)
- Scene save → Regenerate → cliff_S_new1..new4 variant'ları + 8-yön deterministic rotation LIVE olmalı
- **Verify**: 311 cliff tile mevcut konumda kalır ama her cell hash-based variant gösterir

### 2. `Enums.cs` extend
- File: `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs`
- `AssetCategory` enum'una 3 yeni entry ekle (line 23-41):
  - `CliffFaceDecor` (L3 — mounting_*.prefab gibi cliff'e asılı dekor)
  - `WallBlocker` (L5 — statue, pillar, broken column, küçük duvar)
  - `GameplayObject` (L6 — chest, fragment, gate, trigger)
- `TargetLayer { L1, L2, L3, L4, L5, L6 }` enum (line 17) zaten LIVE — doğrula

### 3. `RoomPainterPhysicsRules.cs` extend
- File: `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs`
- Mevcut 32 keyword + 6 yeni: `mounting`, `statue`, `pedestal`, `plinth`, `rune_circle`, `bone_cluster`, `vine`, `chain`
- Her keyword → PhysicsConfig'e:
  - `mounting` → L3, no collider (cliff face decor)
  - `vine`, `chain` → L3, no collider
  - `statue`, `pedestal`, `plinth` → L5, BoxCollider2D solid
  - `rune_circle`, `bone_cluster` → L4, no collider (walkable decor)
- **YENİ**: `PhysicsConfig` struct'ına `public RoomLayer layer;` field ekle (struct definition'ı bul)
- Keyword order **kritik**: spesifik önce, generik sonra. Örn: `cliff_mount` keyword'ünü `cliff`'ten ÖNCE yerleştir (yoksa cliff yakalar)

### 4. Sorting Layer registration
- File: `ProjectSettings/TagManager.asset`
- 2 yeni sorting layer ekle:
  - `Decor_Cliff` (order 50, mevcut layer'lar sonrasına)
  - `Decor_Floor` (order 80)
- YAML format. Mevcut layer'ların ardına ekle, hash uniqueId üret
- Verify: Unity reopen, ProjectSettings > Tags & Layers > Sorting Layers — 2 yeni LIVE

### 5. Phase 1 critical prefab metadata backfill (~30 prefab)
- Phase 1 critical asset listesi:
  - `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_*.prefab` (×15) → AssetCategory=CliffFaceDecor, sortingLayerName=Decor_Cliff, sortingOrder=50, **pivot top-center**
  - `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_*.prefab` (×14) → AssetCategory=WallBlocker, sortingLayerName=Default, sortingOrder=100, BoxCollider2D solid (Y-sort)
  - `Assets/Prefabs/Obstacles/Chasm.prefab`, `NarrowPassage.prefab`, `StoneColumn.prefab` (×3) → AssetCategory=WallBlocker
  - `Assets/Prefabs/Chest.prefab` → AssetCategory=GameplayObject, sortingLayerName=Default, sortingOrder=150, BoxCollider2D **trigger**
  - `Assets/Prefabs/Environment/MapFragment.prefab` veya equivalent (eski script yolu) → AssetCategory=GameplayObject
  - `Assets/Prefabs/RewardPickup.prefab` → AssetCategory=GameplayObject
  - `Assets/Prefabs/Characters/FractureImp_Playtest.prefab` → AssetCategory=Enemy (mevcut, dokunma)
- **mounting pivot top-center fix**: 15 prefab × SpriteRenderer.sprite.pivot ayarı. Pivot Y=1.0 (top), X=0.5 (center). Programmatic via SpriteImporter.spritePivot veya prefab modify.

## Dosyalar (modify scope)
- `Assets/Scenes/Test/PlayableArena_Test01.unity` (sadece CliffAutoPlacer slot)
- `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs` (+3 enum entry)
- `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs` (+6 keyword + struct field)
- `ProjectSettings/TagManager.asset` (+2 sorting layer)
- ~30 prefab metadata (SpriteRenderer.sortingLayerName/Order, Collider2D, AssetCategory ScriptableObject ref if any)

## YASAK
- Yeni dosya yaratma (D3+ scope)
- Editor window UI değişiklik (D3 scope)
- DirectionalCliffTile.cs / CliffAutoPlacer.cs kod değişiklik (sadece slot)
- Scene baş GameObject ekleme/silme
- mounting pivot top-center harici (bottom-center YASAK kullanıcı verbatim)
- PixelLab gen (gece halt rule)

## Verify
- UnityMCP: `mcp__UnityMCP__refresh_unity scope=all mode=force`
- `mcp__UnityMCP__read_console` → 0 error / 0 warning
- Play mode PlayableArena_Test01 → cliff_S/N/E/W variant rotation görsel test (her cell farklı görünmesi gerek)
- mounting_*.prefab Inspector → sortingOrder=50, AssetCategory=CliffFaceDecor
- statue_*.prefab Inspector → BoxCollider2D solid (not trigger), AssetCategory=WallBlocker
- Chest.prefab Inspector → BoxCollider2D trigger, AssetCategory=GameplayObject

## Output
- `STAGING/D2_LAYER_ARCH_DONE.md` — değiştirilen dosya envanteri + verify sonuçları + compile durum
- LOC est ~250-300

## Süre
~30-45 dk Sonnet bg. Background dispatch (run_in_background:true).

BLOCKED durumu: Eğer (a) `DirectionalCliffTile_Hades.asset` sprite array boş ise (sadece tek sprite per direction var) — sadece slot reassign yap, sprite populate D5 scope. (b) TagManager.asset YAML format bilinmiyor — Unity programmatic API kullan (`SortingLayer.NewLayer` veya AssetDatabase ile asset modify). (c) Prefab metadata struct (`AssetCategory` per-prefab nasıl saklı) — Codex review için ayrı task.
