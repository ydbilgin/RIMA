# S82 — Map Designer Validation + Gallery

**Date:** 2026-05-15 (S82 gece, kullanici yatti)
**Status:** Map Designer 5-layer pipeline DOĞRULANDI. 6 procedural map üretildi.

---

## Sistem Validation Sonucu

**Map Designer pipeline ÇALIŞIYOR.** S78 D1-D7 commit'leri LIVE, Karar #135 Phase 1 hibrit workflow gerçekten organik map üretiyor.

| Katman | Durum | Detay |
|---|---|---|
| L1 Corner Wang (BaseTilemap) | ✓ LIVE | `CornerWangPainter.Paint` — biome.tilesetPairings ile transition |
| L2 Multi-variant Wang | ✓ LIVE | variantSeed via MixHash, per-cell variant pick |
| L3 Patch overlay | ✓ LIVE | `PatchOverlayPainter` — moss/rift atlases, 60-200 sprite/map |
| L4 Scatter brush | ✓ LIVE | `ScatterBrushPainter` — stones/moss_tufts, 3-10 instance/map |
| L5 URP 2D Lights | ⚠ TEST FAILED | Sprite material `Sprite-Lit-Default` değil → sahne tek-renk oluyor. Faz 1.5'te shader migration gerekir |

---

## 6-Map Gallery (Assets/Screenshots/)

| # | Recipe | Seed | Atlas | Scatter | Patch # | Scatter # | Dosya |
|---|---|---|---|---|---|---|---|
| 1 | Combat 16x12 | 78101 | Moss | Stones | 187 | 4 | s82_map_iter3_lights_removed.png |
| 2 | Combat 16x12 | 12345 | Moss | Stones | 163 | 10 | map02_combat_seed12345_moss_stones.png |
| 3 | Combat 16x12 | 99887 | **Rift** | Stones | 74 | 4 | map03_combat_seed99887_rift_stones.png |
| 4 | Combat 16x12 | 42424 | Moss | **Moss Tufts** | 165 | 6 | map04_combat_seed42424_moss_tufts.png |
| 5 | **Corridor 24x8** | 11111 | Moss | Moss Tufts | 189 | 3 | map05_corridor_seed11111_moss_tufts.png |
| 6 | Corridor 24x8 | 55555 | **Rift** | Stones | 61 | 7 | map06_corridor_seed55555_rift_stones.png |

**Varyasyon kaynaklari (doğruluk):**
- Seed → ProcGen layout (path carve, secondary terrain perlin scatter) + Wang variant pick
- Recipe → grid boyutu + tile palet
- Atlas (Moss/Rift) → patch decals + biome karakteri (yeşil moss vs. cyan/violet rift fracture)
- Scatter (Stones/Moss Tufts) → 3D mass detail

**Pembe lekeler hakkında (gözleminize cevap):**
- Map 3/6 (Rift atlas): pembe/cyan glow rift_fracture decal'leri — **Karar #98 palette doğru** (#5A2A8A violet + #00FFCC cyan)
- Map 1/2/4 (Moss atlas): birkaç pembe blob → SceneView icon (Camera/Light prefab gizmoları)
- Map 5: pembe yok (en temiz)

---

## NLM-Driven Bulk Map Üretimi — HAZIR

Doğallık eşiği geçti. Sabah kararı:
- Kullanici NLM canon biome spec çekecek (Karar #80 + #135 + biome paletleri)
- Codex ortak karar dispatch: 10-20 map varyasyonu (Shattered Keep + Alabaster Dawn karışık)
- ProceduralRoomGenerator + CornerWangPainter loop, seed array'i ile

Code path zaten hazır:
```csharp
var room = ProceduralRoomGenerator.Generate(recipe, seed);
CornerWangPainter.Paint(baseTilemap, biome, room.vertexGrid, w, h, origin, seed);
patchPainter.PaintPatches(baseTilemap, patchAtlas, seed);
scatterPainter.PaintScatter(baseTilemap, scatterBrush, seed);
```

---

## Çözülmesi Gereken Konular (sabah)

1. **L5 URP 2D Lights** — SpriteRenderer'lar default Unlit material kullanıyor. 2D Light ekleyince tüm sahne tek-renk olur. Çözüm seçenekleri:
   - **A:** Tüm map sprite renderer'larını `Sprite-Lit-Default` material'a migrate et
   - **B:** Light2D'yi sadece player + VFX layer'da kullan, map sprite'larini Unlit bırak
   - **C:** Yumuşak post-process bloom + sahne ambient'ı (Light2D'siz)
   - **Öneri:** B veya C — A çok dosya etkiler

2. **Wall walkable=true** — biome.terrains[id=1 Wall] hâlâ walkable=true. CollisionType=Wall set edilmeli, NavMesh/physics için.

3. **V8 sprite slice errors** — console'da 12+ "rect outside texture" warning. Idle sprite import import settings sorunu, char prefab'larini etkiler. Sprite Editor manuel açıp slice rect düzeltmek gerek.

4. **Pembe SceneView gizmoları** — gerçek game view'da görünmez (sadece Scene panel). Game build için irrelevant.

---

## Sabah Önerilen Sıra (S83)

1. (10 dk) Pembe blob root cause final teyit — Game window screenshot al, gizmolar yoksa false alarm
2. (15 dk) Wall walkable=false + collisionType=Wall fix (BiomePreset edit)
3. (30 dk) `/nlm` ile biome canon çek, Codex dispatch: 10 map bulk generation
4. (45 dk) Faz 1 MVP yan görevleri:
   - Karar #137 4-katman VFX scaffold devam (CombatEventBus zaten var, VFXRouter prefab entries oluştur)
   - Karar #140 dash anim → PixelLab vs VFX karar
5. (asset import) 5 yeni weapon + 2 tile sprite Unity'ye indir

---

## Ek: Bu Gece Üretilenler (kayıt)

**Codex VFX Scaffold (commit `433631e`):**
- CombatEventBus.cs + 7 event struct
- VFXRouter.cs (tag-based routing, pool)
- ProcLimiter.cs (per-tag ICD + frame cap)
- HitPauseDriver.cs, ScreenShakeDriver.cs, HitFlashDriver.cs, DamageNumberDriver.cs
- VFXBusDemo.cs (inspector test buttons)
- dotnet build PASS 0 hata

**Weapon QC seçimleri (PixelLab object_id):**
- `31ee0f73` — Warblade T2 Rift greatsword (frame 12, cyan rift fracture)
- `a032d9b5` — Ronin katana (frame 13, clean hamon)
- `9312ea86` — Shadowblade reverse dagger (frame 7)
- `894bba4a` — Gunslinger flintlock pistol (frame 0)
- `4bde2642` — Hexer curse staff (frame 0, green-cyan flame)
- `886684b6` — Cliff drop tile base (frame 11, dark strata)
- `a5dbe36c` — Rift pool tile base (frame 12, violet+cyan vortex)

Mirror L/R variants (Shadowblade R, Gunslinger R, Ravager R) Unity `flipX` ile yapılır — yeni sprite gerek YOK.

Karar #124 Faz 1 weapon batch: 8/8 unique sprite hazır (Warblade Base S80 + bu 7).
