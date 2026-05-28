# TASK: Per-Biome SO Assets + Demo Scene (Faz 1.0+1.5 Pipeline Test)

## EXECUTE EVERY STEP — DO NOT JUST PLAN. WRITE CODE, BUILD, COMMIT.

---

## Scope (3-4h estimate)

- **RimaBiomeType enum** — eğer henüz yoksa, üç değer: `ShatteredRuins (F1)`, `BleedingWastes (F2)`, `CoreApproach (F3)`
- **3 RimaRoomBaselineTemplate SO asset** — `Assets/Art/Templates/F1_ShatteredRuins.asset`, `F2_BleedingWastes.asset`, `F3_CoreApproach.asset`. Sprite alanları (floorVariants, decalSprites, propSpecs) BOŞ bırakılacak — PixelLab asset'leri gelince elle doldurulacak. Sadece skeleton + biome enum + archetype string + width/height + generatorVersion.
- **Demo sahne** — `Assets/Scenes/Demo/RoomPipelineTest.unity`. İçinde:
  - Boş Grid + Tilemap layer (Base, Decal, Wall) hierarchy
  - `RoomPipelineTestController.cs` MonoBehaviour: 4 buton (Inspector veya GUI)
    - "Generate" → seçili template ile RoomBaselineGenerator çağrısı
    - "Paint Decals" → DecalPainter çağrısı
    - "Place Props" → PropPlacer çağrısı (varsa)
    - "Save + Load" → RoomSaver.Save() → RoomLoader.Load() round-trip
  - Inspector'dan template seçimi (F1/F2/F3)
  - Main Camera + Directional Light (Light 2D Global) + EventSystem
- **Editor menu shortcut** — `RIMA > Demo > Open Room Pipeline Test` (sahneyi açar)
- **Bir adet "smoke test" EditMode test** — `PipelineSmokeTest.cs`: F1 template ile Generate + paint decal + place prop dummy pipeline (sprite null olsa bile crash etmez, LogWarning verir)

---

## Karar Bağlamı (inline)

**Karar #100 — Cold granite F1 palette LOCKED**
F1 Shattered Ruins palette: cool grey granite #3A3D42, deep shadow #252830, cold blue rim #7BA7BC, silver/ice accents. Warm brown YASAK F1'de.

**Karar #115/117 — deterministic generator**
F1 archetypeId = "combat" varsayılan. F2 archetypeId = "combat". F3 archetypeId = "boss". generatorVersion = 1.

**Karar #118 — Multi-layer composition**
Base + Decal + Wall + Prop layer. Demo sahnede 3 Tilemap (Base, Decal, Wall) + Stage root (Props instantiate hedef).

---

## Mevcut Durum — Kod Okuma Gereksinimleri

```
Assets/Scripts/Systems/Map/RimaRoomBaselineTemplate.cs  -- alan listesi, [CreateAssetMenu] kontrol et
Assets/Scripts/Systems/Map/RimaRoomBaselineGenerator.cs
Assets/Scripts/Systems/Map/RoomConfig.cs
Assets/Scripts/Systems/Map/RoomLoader.cs
Assets/Scripts/Systems/Map/PropSpec.cs
Assets/Scripts/Systems/Map/PropLightKind.cs
Assets/Scripts/Systems/Map/RimaArchetypeGenerators.cs
Assets/Editor/RoomDesigner/DecalPainter.cs
Assets/Editor/RoomDesigner/PropPlacer.cs
Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs
Assets/Scripts/Runtime/Rooms/RoomBlueprint.cs
Assets/RoomDesigner.Core/Runtime/GenerationInput.cs
```

BiomeType enum nerede tanımlı? Önce grep et:
```
grep -r "enum BiomeType\|enum RimaBiomeType" Assets/Scripts
```
Mevcut enum varsa onu kullan; yoksa `Assets/Scripts/Systems/Map/RimaBiomeType.cs` oluştur (namespace: `RIMA.Systems.Map`).

---

## Dosya Konumları

### YENİ dosyalar:

```
Assets/Scripts/Systems/Map/RimaBiomeType.cs (sadece enum yoksa)
    -- enum RimaBiomeType { ShatteredRuins, BleedingWastes, CoreApproach }
    -- namespace: RIMA.Systems.Map

Assets/Art/Templates/F1_ShatteredRuins.asset
    -- RimaRoomBaselineTemplate SO instance
    -- biome = ShatteredRuins, archetypeId = "combat"
    -- minWidth=12, maxWidth=18, minHeight=10, maxHeight=14
    -- floorVariants = [] (boş), wallVariants = [] (boş), decalSprites = [] (boş), propSpecs = [] (boş)
    -- decalDensity = 0.35f, generatorVersion = 1

Assets/Art/Templates/F2_BleedingWastes.asset
    -- biome = BleedingWastes, archetypeId = "combat"
    -- minWidth=14, maxWidth=20, minHeight=12, maxHeight=16
    -- diğer alanlar F1 ile aynı (boş arrays, density 0.40f)

Assets/Art/Templates/F3_CoreApproach.asset
    -- biome = CoreApproach, archetypeId = "boss"
    -- minWidth=20, maxWidth=28, minHeight=16, maxHeight=22
    -- decalDensity = 0.25f (void temalı, daha sparse)

Assets/Scenes/Demo/RoomPipelineTest.unity
    -- Hierarchy:
        Grid (Grid component, Cell Size 1x0.5x1 isometric or 1x1x1 square)
          BaseTilemap (Tilemap + TilemapRenderer, sortingOrder=0)
          DecalsTilemap (Tilemap + TilemapRenderer, sortingOrder=10) - NO TilemapCollider2D
          WallsTilemap (Tilemap + TilemapRenderer + TilemapCollider2D, sortingOrder=20)
        StageRoot (empty GameObject — props instantiate hedef)
        Main Camera (Orthographic, size 8, position 0,0,-10)
        Global Light 2D (Light2D, Global, intensity 0.6)
        Pipeline Controller (RoomPipelineTestController script attached)
        EventSystem
    -- RoomPipelineTestController inspector exposed:
        [SerializeField] RimaRoomBaselineTemplate template;  // F1 default
        [SerializeField] Tilemap baseTilemap, decalsTilemap, wallsTilemap;
        [SerializeField] GameObject stageRoot;
        [SerializeField] int seed = 42;

Assets/Scripts/Demo/RoomPipelineTestController.cs
    -- namespace: RIMA.Demo
    -- MonoBehaviour
    -- 4 button (UI veya [ContextMenu] / OnGUI):
        [ContextMenu("1. Generate")] GenerateRoom()
            input = new GenerationInput { seed, biomeId=template.biome.ToString(),
                archetypeId=template.archetypeId, width=15, height=12, generatorVersion=1 }
            gen = new RimaRoomBaselineGenerator();
            CoreRoomBaselineRunner.Run(gen, input, baseTilemap, wallsTilemap);
            FloorVariantPainter.BakeVariants(baseTilemap, bp, template.floorVariants);
            WallAutoConnect.RefreshNeighborhood(wallsTilemap, allWallCells, template.wallVariants, bp);
        [ContextMenu("2. Paint Decals")] PaintDecals()
            DecalPainter.PaintDecals(decalsTilemap, bp, template.decalSprites, seed, template.decalDensity)
        [ContextMenu("3. Place Props")] PlaceProps()
            anchors = RimaArchetypeGenerators.GetDefaultAnchorZones(template.archetypeId, seed, 15, 12)
            PropPlacer.PlaceProps(stageRoot, anchors, template.propSpecs, bp, seed)
        [ContextMenu("4. Save + Load Round-Trip")] SaveAndLoad()
            RoomSaver.Save(...) → prefab + RoomConfig
            RoomLoader.Load(...) → spawn karşılaştır
    -- bp (RoomBlueprint) controller içinde tutulur, adımlar arası paylaşılır
    -- Sprite/prop boş ise → LogWarning, ama crash YOK (Faz 1.5'te bu guard'lar yazıldı)

Assets/Editor/RoomDesigner/DemoMenu.cs
    -- [MenuItem("RIMA/Demo/Open Room Pipeline Test")]
    -- EditorSceneManager.OpenScene("Assets/Scenes/Demo/RoomPipelineTest.unity")

Assets/Tests/EditMode/Editor/PipelineSmokeTest.cs
    -- namespace: RIMA.Tests.Editor
    -- Test 1: F1Template_FullPipeline_NoExceptions
        F1 template load → GenerationInput oluştur → CoreRoomBaselineRunner.Run
        → DecalPainter.PaintDecals (boş sprite, LogWarning bekle)
        → PropPlacer.PlaceProps (boş propSpecs, LogWarning bekle)
        → Assert: hiçbir exception throw etmedi
    -- [TearDown] DestroyImmediate stage + tilemaps
```

---

## Implementation Steps

**ADIM 0 — Baseline**
- `read_console` compile temiz mi?

**ADIM 1 — RimaBiomeType enum**
- Mevcut enum kontrol et, yoksa yaz.

**ADIM 2 — 3 SO asset oluştur**
- AssetDatabase.CreateAsset ile programatik oluştur (editor script veya manuel inspector).
- Tercih: `[MenuItem("RIMA/Demo/Create F1+F2+F3 Templates")]` editor script bir kez çalıştırılır → 3 asset üretir.

**ADIM 3 — Demo sahne oluştur**
- New Scene → 3 Tilemap + Grid + StageRoot + Camera + Light + Controller.
- Tilemap sortingOrder ayarla.

**ADIM 4 — RoomPipelineTestController.cs yaz**
- 4 method, ContextMenu attribute, inspector-driven template seçimi.

**ADIM 5 — DemoMenu.cs editor menu**
- `RIMA > Demo > Open Room Pipeline Test` shortcut.

**ADIM 6 — PipelineSmokeTest EditMode test**
- Tek test: full pipeline exception-free assert.

**ADIM 7 — read_console + test çalıştır**
- Compile PASS + smoke test GREEN.

**ADIM 8 — Manuel smoke (Editor)**
- Sahne aç → ContextMenu 1,2,3,4 sırayla → Console hata yok.

**ADIM 9 — Commit**
```
[demo] Per-biome templates + RoomPipelineTest demo scene
```

---

## Acceptance Criteria

- [ ] `RimaBiomeType` enum 3 değerle tanımlı (`ShatteredRuins`, `BleedingWastes`, `CoreApproach`)
- [ ] `Assets/Art/Templates/F1_ShatteredRuins.asset`, `F2_BleedingWastes.asset`, `F3_CoreApproach.asset` mevcut
- [ ] `Assets/Scenes/Demo/RoomPipelineTest.unity` açılır, hierarchy doğru (3 Tilemap + StageRoot + Camera + Light + Controller)
- [ ] `RIMA > Demo > Open Room Pipeline Test` menu çalışır
- [ ] Controller'da 4 ContextMenu method test edilebilir (sprite boş olsa bile crash yok)
- [ ] `PipelineSmokeTest_F1Template_FullPipeline_NoExceptions` test GREEN
- [ ] `read_console` compile PASS
- [ ] Mevcut testler regresyon yok (175/175 PASS kalır)

---

## Git

```
[demo] Per-biome templates + RoomPipelineTest demo scene
```

---

## Out of Scope

- Asset population (PixelLab batch sonrası elle yapılacak)
- F2/F3 archetype detayları (Faz 2)
- Boss arena özel layout (Faz 2)
- Multi-room linking demo (Faz 1.6)

---

## Risk Notları

1. **BiomeType enum çakışması** — mevcut tanım varsa onu kullan, ikincisi yazma.
2. **SO CreateAssetMenu attribute** — RimaRoomBaselineTemplate'te `[CreateAssetMenu]` yoksa inspector'dan üretemezsin; editor script ile `ScriptableObject.CreateInstance<>` + `AssetDatabase.CreateAsset()` kullan.
3. **Tilemap sortingOrder uniqueliği** — Base=0, Decal=10, Wall=20 (Faz 1.0 ile uyumlu).
4. **Decal tilemap collider** — DecalPainter LogWarning verir; TilemapCollider2D EKLEME.
5. **Camera position** — orthographic size 8, room ~15x12 cell, hepsi görünür olmalı.
