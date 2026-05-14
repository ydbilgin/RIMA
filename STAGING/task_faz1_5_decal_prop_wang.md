# TASK: Room Designer Faz 1.5 — DecalPainter + PropPlacer + Wang Transition Resolver

## EXECUTE EVERY STEP — DO NOT JUST PLAN. WRITE CODE, BUILD, COMMIT.

---

## Scope (8-10h estimate)

- **DecalPainter** — Hybrid Perlin + Poisson-disk scatter, sub-seed `SeedPipeline.DeriveSubSeed(seed, "decal")`, `DecalsTilemap` katmanını doldurur (collider OFF). `RimaRoomBaselineTemplate`'a `decalSprites` alanı eklenir. `btn-paint-decals` toolbar butonuna bağlanır.
- **PropPlacer** — Anchor zone'lardan prop dağıtımı. `RimaArchetypeGenerators` concrete impller: combat (4 mob spawner + 1 loot anchor). `RimaRoomBaselineTemplate.propSpecs` (PropSpec array, REF_NUGGETS §2 alanları dahil). Prefab instantiate + Light2D bağlantı doğrulaması.
- **Wang transition resolver** — WallAutoConnect mevcut 4-bit NSEW zaten çalışıyor. Faz 1.5 eki: biome edge floor→floor transition variant seçimi + designer manual override brush (override lock mekanizması mevcut `overrideVariantIndex` ile entegre).
- **Determinism testi** — 5-seed bit-identical RoomBlueprint kuralı (Faz 1.0 devam). Decal scatter + prop placement aynı seed → aynı sonuç.
- **EditMode testleri** — ≥3 yeni test.
- **Tek commit** `[faz1.5] Room Designer polish: DecalPainter + PropPlacer + Wang transition resolver`

---

## Karar Bağlamı (inline)

**Karar #115 — AI-Assisted Map Builder (LOCKED 2026-05-13)**
LLM/PixelLab API çağrısı YASAK. Tüm AI baseline = pure C# deterministic. `System.Random` kullanılır, `UnityEngine.Random` YASAK. Aynı `GenerationInput` → bit-identical `RoomBlueprint`; 5 farklı seed üretimi RoomLoader'da runtime hatasız yükler.

**Karar #116 — Tile Transition Quality Standard (LOCKED 2026-05-13)**
Floor-wall transition Raggedness >=40% (Wang autotile), 3+ variant zorunlu. Baked light/glow tile içinde YASAK — runtime URP 2D Light. Wang resolver bu kararla ölçülür.

**Karar #117 — Room Designer Portable Core (LOCKED 2026-05-13)**
`Assets/RoomDesigner.Core/` RIMA referansı içermez. `SeedPipeline.DeriveSubSeed` Core katmanındadır — DecalPainter bunu doğrudan çağırır. Yeni Game Layer dosyaları `Assets/Scripts/Systems/Map/Rima*/` altına gider.

**Karar #118 — Hybrid Tile Composition System (LOCKED 2026-05-13)**
Tilemap stack 4 katman: (1) Base/Floor, (2) Decal (transparent, collider OFF), (3) Wall, (4) Prop. DecalsTilemap Faz 1.0'da `RoomDesignerCanvas` içinde zaten oluşturuldu (sortingOrder=20). PropPlacer prefab instantiate eder — Prop katmanı discrete sprite'tır, Tilemap değil.

---

## Mevcut Durum — Kod Okuma Gereksinimleri

**Önce bu dosyaları oku, sonra yaz:**

```
Assets/RoomDesigner.Core/Runtime/SeedPipeline.cs
Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs
Assets/Editor/RoomDesigner/Canvas/RoomDesignerCanvas.cs
Assets/Editor/RoomDesigner/FloorVariantPainter.cs
Assets/Editor/RoomDesigner/WallAutoConnect.cs
Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs
Assets/Scripts/Systems/Map/RimaRoomBaselineTemplate.cs
Assets/Scripts/Runtime/Rooms/RoomBlueprint.cs
Assets/Tests/EditMode/Editor/RoomDesignerIntegrationTests.cs
```

---

## Dosya Konumları

### YENİ dosyalar — yaz:

```
Assets/Editor/RoomDesigner/DecalPainter.cs
    -- namespace: RIMA.Editor.RoomDesigner
    -- static class: DecalPainter
    -- PaintDecals(Tilemap decalTilemap, RoomBlueprint bp, Sprite[] decalSprites, int masterSeed, float density)
    -- Sub-seed: SeedPipeline.DeriveSubSeed(masterSeed, "decal")
    -- Algoritma:
         Adım 1 — Poisson disk nokta üret (System.Random(subSeed), min distance = 2.5 tile, saf C# — Physics2D YASAK)
         Adım 2 — Her nokta için Perlin gürültüsü (seed-offset ile) density * noise > threshold ise yaz
         Adım 3 — Sprite seç: (uint)((x*73856093)^(y*19349663)^subSeed) % decalSprites.Length
         Adım 4 — decalTilemap.SetTile(cell, selected)
    -- density param range: 0.0-1.0 (default 0.35)
    -- decalSprites null veya empty → LogWarning + return false
    -- decalTilemap collider: TilemapCollider2D olmamalı (kontrol et, varsa LogWarning)
    -- Determinizm kuralı: aynı masterSeed + aynı room bounds → identical decal pattern
    -- bp.decalVariantIndex[] güncelle (byte[], roomWidth*roomHeight boyutunda; 0 = decal yok, >0 = variant index+1)

Assets/Scripts/Systems/Map/PropSpec.cs
    -- namespace: RIMA.Systems.Map
    -- [System.Serializable] struct PropSpec
    -- Alanlar (REF_NUGGETS §2 uyumlu):
         GameObject prefab
         bool emitsLight
         PropLightKind lightSourceKind   // enum: None, Torch, RiftCrystal, Candle
         bool requiresVisibleSource      // true → Light2D var ama prop sprite görünür olmalı
         int depthBandMin, depthBandMax  // F1=0, F2=1, F3=2 (REF_NUGGETS §2: depth bands)
         string anchorTag                // "MobSpawner" / "Loot" / "WallLight_N" vb.

Assets/Scripts/Systems/Map/PropLightKind.cs
    -- enum PropLightKind { None, Torch, RiftCrystal, Candle }

Assets/Scripts/Systems/Map/RimaArchetypeGenerators.cs
    -- namespace: RIMA.Systems.Map
    -- static class RimaArchetypeGenerators
    -- GetDefaultAnchorZones(string archetypeId, int seed, int roomWidth, int roomHeight) -> AnchorZone[]
         "combat":  4 adet AnchorZone{tag="MobSpawner", radius=1.5f} + 1 adet {tag="Loot", radius=1.0f}
         "elite":   2 MobSpawner + 1 EliteSpawner + 1 Loot
         "boss":    1 BossSpawner (center) + 2 Loot (edges)
         "loot":    3 Loot + 0 MobSpawner
         "vista":   0 MobSpawner + 2 Scenery
         unknown:   1 MobSpawner (fallback)
    -- Pozisyonlar: System.Random(SeedPipeline.DeriveSubSeed(seed, "anchor")) ile room bounds içine rassal dağıt (corner exclusion: %10 margin)

Assets/Editor/RoomDesigner/PropPlacer.cs
    -- namespace: RIMA.Editor.RoomDesigner
    -- static class PropPlacer
    -- PlaceProps(GameObject stageRoot, AnchorZone[] anchors, PropSpec[] propSpecs, RoomBlueprint bp, int masterSeed)
    -- Sub-seed: SeedPipeline.DeriveSubSeed(masterSeed, "prop")
    -- Her anchor için:
         propSpecs içinden anchorTag eşleşenleri filtrele
         System.Random ile bir PropSpec seç
         GameObject.Instantiate(propSpec.prefab, worldPos, Quaternion.identity, stageRoot.transform)
         propSpec.requiresVisibleSource=true ise → Light2D + SpriteRenderer kontrolü:
             prop.GetComponentInChildren<Light2D>() != null AND prop.GetComponentInChildren<SpriteRenderer>() != null → OK
             yoksa → Debug.LogWarning("PropPlacer: prop '{name}' requiresVisibleSource=true ama Light2D veya SpriteRenderer eksik")
    -- Dönüş: List<GameObject> instantiate edilenler (RoomSaver entegrasyonu için)
    -- UnityEngine.Random YASAK

Assets/Tests/EditMode/Editor/RoomDesignerFaz15Tests.cs
    -- namespace: RIMA.Tests.Editor
    -- Test 1: DecalPainter_SameSeed_ProducesBitIdenticalPattern
    -- Test 2: PropPlacer_CombatArchetype_Places4MobAnd1Loot
    -- Test 3: WangTransitionResolver_BiomeEdge_SelectsEdgeVariant
    -- [TearDown] DestroyImmediate cleanup
```

### DÜZENLE — mevcut dosyalar:

```
Assets/Scripts/Systems/Map/RimaRoomBaselineTemplate.cs
    -- Ekle:
         public Sprite[] decalSprites;
         [Range(0f,1f)] public float decalDensity = 0.35f;
         public PropSpec[] propSpecs;

Assets/Scripts/Runtime/Rooms/RoomBlueprint.cs
    -- [Header("Baked Variant Data")] bloğuna ekle:
         public byte[] decalVariantIndex;         // DecalPainter sonucu, roomWidth*roomHeight

Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs
    -- BindToolbar() içine:
         var decalBtn = rootVisualElement.Q<Button>("btn-paint-decals");
         if (decalBtn != null) decalBtn.clicked += PaintDecals;
         var propBtn = rootVisualElement.Q<Button>("btn-place-props");
         if (propBtn != null) propBtn.clicked += PlacePropsAction;
         var transBtn = rootVisualElement.Q<Button>("btn-apply-transitions");
         if (transBtn != null) transBtn.clicked += ApplyTransitionsAction;
    -- PaintDecals() / PlacePropsAction() / ApplyTransitionsAction() private metodlar — her biri guard'lar + ilgili static class çağrısı
    -- GenerateRoom() OTOMATIK decal/prop/transition TETIKLEMEZ (sadece base+wall). Decal/Prop/Transition ayrı butonlar.

Assets/Editor/RoomDesigner/WallAutoConnect.cs
    -- Yeni metod ekle (mevcut RefreshCell() dokunma):
         public static int GetTransitionVariantIndex(BiomeType cellBiome, BiomeType neighborBiome, int connectionMask)
         -- İki biome aynı → 0 döndür (transition yok)
         -- Farklı biome → connectionMask'a göre 1-3 arası edge variant index döndür
             mask & 4 (N) → 1, mask & 8 (S) → 2, mask & 3 (E/W) → 3 (örnek mapping)
    -- BiomeTransitionPainter static helper:
         public static void ApplyBiomeTransitions(Tilemap floorTilemap, RoomBlueprint bp, TileBase[] transitionVariants)
         -- floorTilemap.cellBounds iterate, komşu biome kontrolü, GetTransitionVariantIndex → variant
         -- overrideVariantIndex[idx]=true ise skip (lock mekanizması korunur)
         -- transitionVariants null/empty → return false

Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uxml
    -- toolbar içine btn-generate yanına:
         <ui:Button name="btn-paint-decals" text="Paint Decals" class="rd-toolbar-btn" />
         <ui:Button name="btn-place-props" text="Place Props" class="rd-toolbar-btn" />
         <ui:Button name="btn-apply-transitions" text="Apply Transitions" class="rd-toolbar-btn" />

Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs
    -- Save() içinde stage root child'larını prefab'a alan akışı doğrula
    -- PropPlacer instantiate ettikleri stage root altındaysa otomatik save edilir, ek iş yok
    -- decalTilemap içeriği zaten Tilemap component olarak save'e dahil
```

---

## Implementation Steps (Codex execute)

**ADIM 0 — Baseline**
- `read_console` → compile error varsa düzelt.
- Mevcut EditMode testleri çalıştır → PASS doğrula.

**ADIM 1 — Data layer (PropSpec, PropLightKind, RimaArchetypeGenerators)**
- `PropLightKind.cs` enum yaz.
- `PropSpec.cs` struct yaz (tüm alanlar).
- `RimaArchetypeGenerators.cs` static class yaz — `GetDefaultAnchorZones(archetypeId, seed, w, h)`.
- AnchorZone struct mevcut mu kontrol et (Core'da). Yoksa `Assets/RoomDesigner.Core/Runtime/AnchorZone.cs` ekle.
- `read_console` → PASS.

**ADIM 2 — Template + Blueprint genişletme**
- `RimaRoomBaselineTemplate.cs`: `decalSprites[]`, `decalDensity`, `propSpecs[]` ekle.
- `RoomBlueprint.cs`: `decalVariantIndex byte[]` ekle.
- `read_console` → PASS.

**ADIM 3 — DecalPainter**
- `Assets/Editor/RoomDesigner/DecalPainter.cs` yaz.
- Poisson disk + Perlin + deterministic sprite seçimi + `decalVariantIndex` bake.
- TilemapCollider2D null guard + LogWarning.
- `read_console` → PASS.

**ADIM 4 — PropPlacer**
- `Assets/Editor/RoomDesigner/PropPlacer.cs` yaz.
- Anchor tag matching → PropSpec filter → deterministic seçim → Instantiate → Light2D doğrulama.
- UnityEngine.Random YASAK — System.Random(subSeed) zorunlu.
- `read_console` → PASS.

**ADIM 5 — Wang Transition Resolver**
- `WallAutoConnect.cs` içine `GetTransitionVariantIndex()` ekle.
- `BiomeTransitionPainter` static helper (aynı dosyada veya ayrı): `ApplyBiomeTransitions()`.
- `overrideVariantIndex` lock'a saygı göster.
- `read_console` → PASS.

**ADIM 6 — Window entegrasyonu**
- `BindToolbar()` 3 buton register et (decal, prop, transition).
- `PaintDecals()`, `PlacePropsAction()`, `ApplyTransitionsAction()` private metodlar.
- `RoomDesignerWindow.uxml` 3 buton ekle.
- **GenerateRoom() ek değişiklik yapma** — Generate sadece base+wall, decal/prop/transition AYRI BUTON.
- `read_console` → PASS.

**ADIM 7 — EditMode testleri**
- `RoomDesignerFaz15Tests.cs` yaz (3 test).
- `read_console` PASS + tüm yeni testler GREEN.

**ADIM 8 — 5-seed determinism manuel doğrulama**
- Window aç, SO ata (decalSprites ≥1, propSpecs ≥1 combat).
- 5 seed (42, 137, 2501, 9999, 31415): Generate → Paint Decals → Place Props → Apply Transitions.
- Her seed iki kere → `decalVariantIndex` byte-for-byte identical olmalı.

**ADIM 9 — Final read_console + tüm testler**
- Sıfır compile error, mevcut + yeni testler PASS.

**ADIM 10 — Commit**
- `git add -A`
- `git commit -m "[faz1.5] Room Designer polish: DecalPainter + PropPlacer + Wang transition resolver"`

---

## Acceptance Criteria

- [ ] `read_console` compile PASS (sıfır error)
- [ ] `btn-paint-decals` toolbar'da, tıklandığında `DecalsTilemap` dolar
- [ ] `btn-place-props` toolbar'da, combat archetype için 4 MobSpawner + 1 Loot prop instantiate eder
- [ ] `btn-apply-transitions` toolbar'da, biome edge floor hücrelerini transition variant ile değiştirir
- [ ] `DecalsTilemap` üzerinde `TilemapCollider2D` yok (varsa LogWarning)
- [ ] `requiresVisibleSource=true` prop'larda Light2D + SpriteRenderer birlikte var; eksikse LogWarning
- [ ] `WallAutoConnect.GetTransitionVariantIndex(biomeA, biomeB, mask)` farklı biome için >0 döndürür
- [ ] `overrideVariantIndex=true` hücreleri ApplyBiomeTransitions tarafından atlanır
- [ ] Seed 42 → Decal → tekrar seed 42 → `bp.decalVariantIndex` byte-for-byte identical
- [ ] 5-seed determinism PASS (42, 137, 2501, 9999, 31415) — decal + prop dahil
- [ ] EditMode 3 yeni test GREEN
- [ ] Mevcut Faz 1.0 testleri regresyon yok

---

## Git

Tek commit:
```
[faz1.5] Room Designer polish: DecalPainter + PropPlacer + Wang transition resolver
```

---

## Out of Scope (Faz 1.6+)

- Inpaint Region brush mode (kilitsiz hücre re-seed)
- Force re-seed komutu
- Anchor Zone painter (manual)
- RenderTexture cache + repaint debounce
- Preview kamera ~35° konverjans kalibrasyonu (Karar #113)
- Boss/elite archetype prop detayları (Faz 2)
- PixelLab API entegrasyonu (Karar #115 YASAK)
- `floorOverrideVariantIndex` (Faz 1.6)
- Drop shadow (child SpriteRenderer multiply oval — Karar #116(g), Faz 1.6)
- Multi-room linking, corridor anchor join
- Prop layer Y-sort polishing

---

## Risk Notları (Codex okusun)

1. **UnityEngine.Random YASAK** — Sadece `System.Random(subSeed)`. Determinism testi bunu doğrular.
2. **Poisson disk headless** — Unity Physics2D YASAK (EditMode testte çalışmaz). Saf C# mesafe kontrolü, `Math.Sqrt`.
3. **DecalsTilemap collider** — `RoomDesignerCanvas.CreateTilemap("Decals",...)` collider eklemiyor; manual eklenmişse warning.
4. **AnchorZone struct konumu** — Faz 1.0'da mevcut olabilir (Core/Runtime). Önce kontrol et, varsa extend; yoksa yeni yaz.
5. **PropPlacer + RoomSaver** — `RoomSaver.Save()` stage root child'larını prefab'a alıyor mu kontrol et. Alıyorsa ek iş yok. Almıyorsa PropPlacer dönüş listesini Saver'a ilet.
6. **BiomeType enum** — `RIMA.Runtime.Rooms` namespace, `WallAutoConnect.cs` zaten erişiyor.
7. **EditMode test headless Tilemap** — `new GameObject().AddComponent<Tilemap>()` + `[TearDown] DestroyImmediate` cleanup.
8. **decalVariantIndex boyutu** — `roomWidth * roomHeight`. 0 = decal yok, >0 = variant index+1.
