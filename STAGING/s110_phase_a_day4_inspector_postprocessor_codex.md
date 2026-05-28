ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**Output dosyası:** `CODEX_DONE_room_painter_day4.md` (max 600 kelime)
**Code dosyaları:** `Assets/Editor/RoomPainter/` + `Assets/Scripts/RoomPainter/` altına yaz/düzenle

---

# Amaç

Phase A Day 4 — All-in-one Inspector panel + Auto-import AssetPostprocessor + Physics integration.

User direktifi (verbatim): "Rigidbody2D'i Unity üzerinden ayarlamak yerine kendi içindeki bi mantıkla ben ayarlasam, block koyulacak objelerde de onu Unity'e import edince kendi halinde koysa."

## Bağlam — mevcut state

- Window: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (360 LOC, sekmeli palet + asset palette LIVE)
- Placer: `Assets/Editor/RoomPainter/RoomPainterScenePlacer.cs` (~330 LOC, SceneView click/drag + iso snap + R-rotate + cyan/purple ghost LIVE)
- Helper: `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs` (InferLayer 40+ keyword LIVE)
- SO: `Assets/Scripts/RoomPainter/` — RoomLayer, RoomPainterAsset, RoomLayerData, RoomData
- 0 compile error

## Source spec

`F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md` (Sonnet yazdı, 5200 kelime, 9 bölüm + 3 appendix).

**ÖNCELİKLE OKU:** Bölüm 3 (Inspector all-in-one), Bölüm 4 (AssetPostprocessor pipeline), Bölüm 5 (Block/physics inference 30 keyword table).

## Görev — Day 4 handoff (Sonnet spec'ten implementable form)

### 7 yeni dosya yarat

1. **`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs`** — `AssetPostprocessor.OnPostprocessAllAssets` override. Yeni sprite/prefab `Assets/Sprites/` veya `Assets/Prefabs/` altına düştüğünde:
   - InferLayer ile kategori belirle
   - GUID-keyed `RoomPainterAsset` SO oluştur `Assets/RoomPainter/AssetMetadata/<guid>.asset`
   - Block inference (Bölüm 5 keyword table'ı kullan) → Physics defaults set
   - Event publish (event bus üzerinden window refresh)
   - **Anti double-trigger:** `feedback_double_auto_regen_avoid.md` memory'ne göre `isUpdating` check + deferred queue `EditorApplication.delayCall`

2. **`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs`** — static class:
   - 30+ keyword table (Sonnet spec Bölüm 5'ten birebir kopyala): wall/cliff/pillar=Block YES BoxCollider2D Static, floor/decal/moss=NO, enemy/npc=YES Capsule Dynamic, pickup/item=NO Trigger, prop=YES default, vb.
   - Method `static PhysicsConfig Resolve(string assetPath)` → struct: `bool isBlock, BodyType bodyType, ColliderShape colliderShape, bool isTrigger, string physicsLayerName`

3. **`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetEvents.cs`** — basit event bus:
   - `static event Action<RoomPainterAsset> AssetCreatedOrUpdated`
   - `static event Action<string /*guid*/> AssetDeleted`
   - Window OnEnable subscribe, OnDisable unsubscribe — auto refresh palette

4. **`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsApplier.cs`** — Designer inspector'da "Apply Physics" tıklarsa, `RoomPainterAsset.physicsConfig` → seçili GameObject'e:
   - `AddComponent<Rigidbody2D>()` ve `bodyType` set
   - `AddComponent<BoxCollider2D>()` veya `CircleCollider2D` shape'e göre
   - Trigger flag uygula
   - `gameObject.layer = LayerMask.NameToLayer(physicsLayerName)`

5. **`Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs`** — orchestrator class:
   - `Draw(RoomPainterAsset asset, GameObject sceneInstance)` — hangi mode'da? (SO edit / instance edit / dual banner)
   - 6 foldout section çağırır (aşağıdaki 6 dosya)
   - EditorPrefs persist foldout state

6. **6 section dosyası** `Assets/Editor/RoomPainter/Inspector/Sections/`:
   - `IdentitySection.cs` — path, preview 128×128, display name override
   - `PlacementSection.cs` — RoomLayer dropdown, sorting layer + order, Y-sort axis, pivot anchor
   - `PhysicsSection.cs` — Block toggle + bodyType + colliderShape + size + trigger + physicsLayer
   - `ParallaxSection.cs` — sadece Parallax tab aktifken: tier dropdown + override slider 0.01-1.50 + camera relative + pixel snap
   - `VisualSection.cs` — tint color, material override, light interaction
   - `MetadataSection.cs` — tags multi + notes string field

7. **`Assets/Scripts/RoomPainter/RoomPainterAssetBinding.cs`** — runtime MonoBehaviour:
   - `public string assetGuid;` field
   - Painted GO'lara her instantiate sırasında otomatik eklenir (RoomPainterScenePlacer.PaintCell'de hook)
   - Pick tool ve Save/Load için reliable source SO resolve

### 3 dosyayı extend

1. **`Assets/Scripts/RoomPainter/RoomPainterAsset.cs`** — 14 yeni field (Sonnet spec Bölüm 3'ten):
   ```csharp
   // Placement
   public string defaultSortingLayer = "Default";
   public int defaultOrder;
   public bool ySortEnabled = true;
   public Vector2 pivotAnchor = new Vector2(0.5f, 0f);

   // Physics
   public bool isBlock;
   public RigidbodyType2D bodyType = RigidbodyType2D.Static;
   public ColliderShape colliderShape = ColliderShape.Box;
   public Vector2 colliderSize = Vector2.one;
   public bool isTrigger;
   public string physicsLayerName = "Default";

   // Visual
   public Color tint = Color.white;
   public string materialOverridePath; // path string (lazy load)
   public bool castShadow = true;
   public bool receiveLight = true;

   // Metadata
   public List<string> tags = new List<string>();
   public string notes;
   ```
   `ColliderShape` enum: Box, Circle, Capsule, Polygon

2. **`Assets/Scripts/RoomPainter/RoomLayerData.cs`** — `ySortAxis` enum:
   ```csharp
   public enum YSortAxis { None, X, Y, NegY, Custom }
   public YSortAxis ySortAxis = YSortAxis.NegY;
   ```

3. **`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs`** — `DrawInspectorPanel` stub'ı (placeholder) gerçek implementation ile değiştir:
   - `private RoomPainterInspectorPanel _inspectorPanel;` field
   - OnEnable: `_inspectorPanel = new RoomPainterInspectorPanel();`
   - DrawInspectorPanel içeriği: `_inspectorPanel.Draw(_selectedAsset.metadata, currentSceneSelection);`

### Reuse

- **`Packages/com.laureth.painter-suite/Editor/Colliders/ColliderTemplateService.cs`** — Box/Circle/Polygon authoring için
- **`Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs`** — drag-state pattern (manual collider sizing için Day 5+)

### RoomPainterScenePlacer.cs — hook ekle

PaintCell sonrasına:
```csharp
var binding = go.AddComponent<RoomPainterAssetBinding>();
binding.assetGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(selectedAsset.sprite ?? (Object)selectedAsset.prefab));

// Physics auto-apply if block
if (selectedAsset.metadata != null && selectedAsset.metadata.isBlock)
{
    RoomPainterPhysicsApplier.Apply(go, selectedAsset.metadata);
}
```

`AssetEntry` struct'ına `RoomPainterAsset metadata` field eklenecek (Day 4 scope içinde, RoomPainterAssetScanner.cs minor update). Eğer SO yoksa null, fallback inferred values.

## Verification

1. `grep -rn "RoomPainterAssetPostprocessor\|RoomPainterAssetEvents\|RoomPainterPhysicsRules" Assets/Editor/RoomPainter/` → ≥ 5 hits
2. `grep -rn "RoomPainterInspectorPanel\|IdentitySection\|PhysicsSection" Assets/Editor/RoomPainter/Inspector/` → ≥ 7 hits
3. `grep -n "isBlock\|colliderShape\|physicsLayerName" Assets/Scripts/RoomPainter/RoomPainterAsset.cs` → ≥ 3 hits
4. `grep -n "RoomPainterAssetBinding" Assets/Scripts/RoomPainter/` → 1 hit
5. Unity compile 0 error (orchestrator post-dispatch verify edecek)
6. Sprite import test: `Assets/Sprites/Environment/RIMA_AssetParts_v2/` altına yeni dummy sprite koy (ya da existing'i re-import), Assets/RoomPainter/AssetMetadata/<guid>.asset otomatik oluşmalı

## Yapma

- Drag-drop palette → SceneView **YOK** (Day 5)
- Erase/Pick/Box-select **YOK** (Day 5)
- Save/Load RoomData **YOK** (Day 6)
- Mevcut Phase 1-3 (cliff Phase 1+2+3) dokunma
- Cliff sahne state'i (auto LIVE 311 tile) dokunma
- Mevcut PainterSuite v0.4.0 package dosyalarına dokunma

## Çıktı

`CODEX_DONE_room_painter_day4.md`:
- 10 dosya listesi + LOC + her dosyanın 1 satır işlevi
- Verification grep çıktıları
- Compile durumu
- Eğer Sonnet spec'inde belirsizlik veya çakışma fark ettiysen (örn "Bölüm 3 SO field listesi ile Bölüm 4 postprocessor inferred defaults overlap") BLOCKED + detay

**Phase A Day 4 ship hedefi: Sprite import → otomatik kategorize + metadata SO + Inspector'da Rigidbody2D/Collider2D/sorting/parallax/visual/tags tek panelde edit edilir.** Designer Unity Inspector'a hiç gitmesin.
