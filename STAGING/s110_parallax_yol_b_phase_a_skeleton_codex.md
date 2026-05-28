ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**Output dosyası:** `CODEX_DONE_parallax_phase_a_skeleton.md` (kısa, max 400 kelime)
**Code dosyaları:** doğrudan Assets/ altına yaz (aşağıda listeli)

---

# Amaç

ChatGPT spec (`STAGING/s109_chatgpt_room_painter_spec.md`) Phase A iskeletini Unity Editor'da derlenebilir şekilde implement et. **Compile clean MVP skeleton** — feature yok, sadece struct + window stub.

## Bağlam

- Engine: Unity 2022.3+, URP 2D
- Mevcut: `Packages/com.laureth.painter-suite/` v0.4.0 LIVE (Collider Painter MVP)
- Aktif sahne: `Assets/Scenes/Test/PlayableArena_Test01.unity` (dokunma)
- ParallaxLayer.cs: `Packages/com.laureth.painter-suite/Runtime/ParallaxLayer.cs` (UPM, dokunma)

## Görev — yeni dosyalar yarat

### 1. Asmdef
**Path:** `Assets/Editor/RoomPainter/RIMA.RoomPainter.Editor.asmdef`
```json
{
    "name": "RIMA.RoomPainter.Editor",
    "rootNamespace": "RIMA.RoomPainter",
    "references": [
        "RIMA.Runtime",
        "RIMA.MapDesigner",
        "Unity.2D.Tilemaps"
    ],
    "includePlatforms": ["Editor"],
    "autoReferenced": true
}
```

### 2. RoomLayer enum
**Path:** `Assets/Scripts/RoomPainter/RoomLayer.cs` (Runtime'da — runtime enum, editor SO ref eder)

10 değer:
```csharp
namespace RIMA.RoomPainter
{
    public enum RoomLayer
    {
        Floor = 0,
        Edge = 1,
        Cliff = 2,
        Wall = 3,
        Props = 4,
        Decals = 5,
        Lighting = 6,
        Collision = 7,
        Occlusion = 8,
        Parallax = 9
    }
}
```

Eğer `RIMA.Runtime` asmdef yoksa, RoomLayer.cs için `Assets/Scripts/RoomPainter/` altında **kendi runtime asmdef** yarat: `RIMA.RoomPainter.Runtime.asmdef` (rootNamespace `RIMA.RoomPainter`, no references gerek).

### 3. ScriptableObject — RoomPainterAsset
**Path:** `Assets/Scripts/RoomPainter/RoomPainterAsset.cs`

ChatGPT spec field listesi (Bölüm "RoomPainterAsset" kısmı):
- `string id`
- `string displayName`
- `string category`
- `Sprite sprite` (nullable)
- `GameObject prefab` (nullable)
- `RoomLayer defaultLayer`
- `string defaultSortingLayer`
- `int defaultOrder`
- `Vector2 defaultScale = Vector2.one`
- `Vector2 defaultVisualOffset = Vector2.zero`

`[CreateAssetMenu(fileName = "RoomPainterAsset", menuName = "RIMA/Room Painter/Asset")]`

### 4. ScriptableObject — RoomLayerData
**Path:** `Assets/Scripts/RoomPainter/RoomLayerData.cs`

Field listesi (ChatGPT spec'ten çıkar — eğer eksik field varsa, conservative default ekle):
- `RoomLayer layer`
- `string sortingLayerName`
- `int defaultOrder`
- `float depthValue` (parallax için)
- `bool isStatic`
- `bool isRoomLocked`
- `bool isCameraRelative`
- `bool ySortEnabled`

`[CreateAssetMenu(fileName = "RoomLayerData", menuName = "RIMA/Room Painter/Layer Data")]`

### 5. ScriptableObject — RoomData
**Path:** `Assets/Scripts/RoomPainter/RoomData.cs`

Stub for top-level room asset:
- `string roomId`
- `string displayName`
- `List<RoomLayerData> layers` (10 layer slot)
- `List<PlacementRecord> placements` (PlacementRecord inner serializable struct: `RoomPainterAsset asset`, `Vector3 worldPos`, `RoomLayer layer`, `int orderOverride`, `Vector2 scaleOverride`)

`[CreateAssetMenu(fileName = "RoomData", menuName = "RIMA/Room Painter/Room Data")]`

### 6. EditorWindow stub
**Path:** `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs`

Çok minimal stub:
- Menu: `[MenuItem("RIMA/Room Painter")]` → `RimaRoomPainterWindow.ShowWindow()`
- OnGUI: 3 alan — sol palette panel placeholder (boş scrollview), orta scene info, sağ inspector placeholder
- Üst toolbar: 10 layer toggle (her layer için button, hiçbiri henüz işlevsel)
- Bottom status bar: "Phase A skeleton — feature integration pending"
- Hidden iç state field'lar (Sonnet'ın Phase A plan'ında ne diyecekse — yine de minimal başla)

## Yapma

- Asset palette gerçek folder scan YOK
- SceneView placement YOK
- Save/load YOK
- VisualEditorScenePainter ile karışma YOK
- Mevcut Painter Suite package'a dokunma YOK
- Aktif sahnedeki bir tile bile dokunma YOK

## Verification

1. `grep -rn "RimaRoomPainterWindow" Assets/Editor/RoomPainter/` → 1 hit
2. `grep -rn "RoomLayer\." Assets/Scripts/RoomPainter/` → en az 5 hit (3 SO içinde + enum use)
3. Unity console compile error 0 (orchestrator post-dispatch verify edecek)
4. Menu `RIMA > Room Painter` Unity'de görünür (orchestrator visual test)

## Çıktı

`CODEX_DONE_parallax_phase_a_skeleton.md` — dosyaları say + her birinin LOC + compile durumu + her hangi BLOCKED durum varsa.
