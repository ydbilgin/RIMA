# Codex Task — Room Designer F1 SKELETON

**Status:** READY FOR DISPATCH
**Branch:** master
**Estimated:** ~800-1200 LOC
**Allowed paths:**
- `Assets/Editor/RoomDesigner/**`
- `Assets/Scripts/Runtime/Rooms/**` (sadece RoomBlueprint, RoomPrefabLink yeni dosyalar; mevcut GateSocket/MobSpawnPoint/RoomMetadata DOKUNMA)

## Hedef

`RimaRoomDesignerWindow` adında Custom EditorWindow oluştur. UI Toolkit shell + IMGUIContainer canvas + plane raycast mouse-to-grid + Save Prefab pipeline. Bu task F1'in **temel iskeleti** — Palette ve Brush task'ları bunun üzerine inşa edilecek.

## Mimari Karar (Opus + Codex review LOCKED)

| Bileşen | Karar |
|---|---|
| Window | Tek `EditorWindow` (`RimaRoomDesignerWindow`) |
| UI shell | UI Toolkit (UXML + USS) — toolbar + sol/sağ panel slot |
| Canvas | `IMGUIContainer` + `Handles.DrawCamera` + `HandleUtility.GUIPointToWorldRay` |
| Grid | Unity native `Grid` (IsometricZAsY) — RIMA scene grid ile uyumlu |
| Tilemaps | 3 ayrı `Tilemap` (Floor/Walls/Decals) — Y-sort doğru |
| Save | Prefab + RoomBlueprint SO pair, manual rollback (try/catch + DeleteAsset on fail) |

## Yapılacak Dosyalar

### 1. `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` (~250 LOC)
- `[MenuItem("RIMA/Room Designer")]` → window aç
- `CreateGUI()` UXML yükle, sol panel slot (`#left-panel`), sağ panel slot (`#right-panel`), merkez canvas slot (`#canvas-host`)
- `IRoomDesignerContext` field expose et (aşağıda)
- `OnEnable/OnDisable`: preview camera lifecycle, scene + tilemap setup
- File watcher polling pattern (F3 hazırlığı): `EditorApplication.update += PollMcp` placeholder, response klasörünü tara — F3'te dolacak

### 2. `Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uxml`
```xml
<UXML xmlns:ui="UnityEngine.UIElements">
  <ui:VisualElement name="root" class="rd-root">
    <ui:VisualElement name="toolbar" class="rd-toolbar">
      <ui:Button name="btn-save" text="Save Room"/>
      <ui:Button name="btn-new"  text="New Room"/>
      <ui:DropdownField name="active-layer" label="Layer"/>
    </ui:VisualElement>
    <ui:VisualElement name="body" class="rd-body">
      <ui:VisualElement name="left-panel"  class="rd-panel rd-left"/>   <!-- Palette buraya kendini Add edecek -->
      <ui:VisualElement name="canvas-host" class="rd-canvas"/>          <!-- IMGUIContainer buraya -->
      <ui:VisualElement name="right-panel" class="rd-panel rd-right"/>  <!-- Inspector / Brush options -->
    </ui:VisualElement>
  </ui:VisualElement>
</UXML>
```

### 3. `Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss`
- USS variable'lar: `--rd-bg`, `--rd-panel-bg`, `--rd-accent` (biome theming F4'te kullanılacak)
- flexbox layout: toolbar üstte, body altta horizontal (left-panel sabit 280px, right-panel sabit 320px, canvas flex-grow:1)

### 4. `Assets/Editor/RoomDesigner/Canvas/RoomDesignerCanvas.cs` (~250 LOC)
- `IMGUIContainer` türetilmiş veya wrapper class
- `Camera previewCam` (ortografik, layer mask = RoomDesigner only)
- Scene oluştur: GameObject `_RoomDesignerStage` (HideFlags.DontSave) + Grid + 3 Tilemap (Floor/Walls/Decals) + camera
- Çizim:
```csharp
void DrawCanvas()
{
    var r = GUILayoutUtility.GetRect(0, 100000, 0, 100000, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
    Event e = Event.current;
    Handles.DrawCamera(r, previewCam);
    if (r.Contains(e.mousePosition))
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        var plane = new Plane(Vector3.forward, Vector3.zero);
        if (plane.Raycast(ray, out float hit))
        {
            Vector3 world = ray.GetPoint(hit);
            Vector3Int cell = grid.WorldToCell(world);
            ctx.HoveredCell = cell;
            if (e.type is EventType.MouseDown or EventType.MouseDrag)
            {
                ctx.InvokeBrush(e.button, cell);
                e.Use();
            }
        }
    }
    if (e.type == EventType.ScrollWheel) { Zoom(e.delta.y); e.Use(); }
}
```
- HiDPI not: `EditorGUIUtility.pixelsPerPoint` kontrol et — `Handles.DrawCamera` ile uyum
- Repaint **sadece** drag/zoom/pan veya `ctx.IsDirty` true iken
- 60fps gate için `System.Diagnostics.Stopwatch` ile son 60 frame ortalaması debug log'a düş (sadece `RoomDesignerCanvas.DEBUG_PERF` define varsa)

### 5. `Assets/Editor/RoomDesigner/IRoomDesignerContext.cs` (~80 LOC)
**KRITIK:** Palette ve Brush task'ları bu interface'e karşı çalışacak. Imza değişmemeli.
```csharp
public interface IRoomDesignerContext
{
    // Tilemap ler
    Tilemap FloorTilemap { get; }
    Tilemap WallsTilemap { get; }
    Tilemap DecalsTilemap { get; }
    Tilemap GetActiveTilemap();
    RoomLayer ActiveLayer { get; set; }   // Floor / Walls / Decals

    // Palette → Brush iletişim
    TileBase ActiveTile { get; set; }
    BrushMode ActiveBrush { get; set; }   // Stamp / Eraser / Picker / Bucket

    // Mouse hover state
    Vector3Int HoveredCell { get; set; }
    bool IsCanvasHovered { get; }

    // Brush dispatcher — Canvas çağırır, BrushController dinler
    void InvokeBrush(int mouseButton, Vector3Int cell);

    // UI hooks
    VisualElement LeftPanel { get; }
    VisualElement RightPanel { get; }

    // Repaint signal
    void MarkDirty();
}

public enum RoomLayer { Floor, Walls, Decals }
public enum BrushMode { Stamp, Eraser, Picker, Bucket }
```
Concrete impl `RimaRoomDesignerWindow` içinde (window itself implements interface).

### 6. `Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs` (~200 LOC)
**Atomic save pattern (Codex review Soru 3 cevabı — manual rollback ZORUNLU):**
```csharp
public static class RoomSaver
{
    public static (string prefabPath, string blueprintPath) Save(GameObject roomRoot, string roomId, string biome)
    {
        string dir = $"Assets/_Generated/Rooms/{biome}";
        Directory.CreateDirectory(dir);
        string soPath     = $"{dir}/{roomId}.asset";
        string prefabPath = $"{dir}/{roomId}.prefab";
        var made = new List<string>();
        try
        {
            var bp = ScriptableObject.CreateInstance<RoomBlueprint>();
            AssetDatabase.CreateAsset(bp, soPath); made.Add(soPath);

            PrefabUtility.SaveAsPrefabAsset(roomRoot, prefabPath, out bool ok);
            if (!ok) throw new InvalidOperationException("Prefab save failed");
            made.Add(prefabPath);

            AssetDatabase.ImportAsset(soPath);
            AssetDatabase.ImportAsset(prefabPath);

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            bp.prefab = prefab;
            bp.roomId = roomId;
            bp.biome  = biome;

            var link = prefab.GetComponent<RoomPrefabLink>() ?? prefab.AddComponent<RoomPrefabLink>();
            link.blueprint = bp;

            EditorUtility.SetDirty(bp);
            EditorUtility.SetDirty(prefab);
            AssetDatabase.SaveAssets();
            return (prefabPath, soPath);
        }
        catch
        {
            foreach (var p in made) AssetDatabase.DeleteAsset(p);
            throw;
        }
    }
}
```

### 7. `Assets/Scripts/Runtime/Rooms/RoomBlueprint.cs` (yeni — eğer yoksa, varsa DOKUNMA)
```csharp
[CreateAssetMenu(menuName="RIMA/Room Blueprint")]
public class RoomBlueprint : ScriptableObject
{
    public string roomId;
    public string biome;
    public GameObject prefab;
    public string roomType;   // combat / elite / shop / rest / boss
    public int gateCount;
    // F2'de mob spawn manifest, metadata genişler
}
```

### 8. `Assets/Scripts/Runtime/Rooms/RoomPrefabLink.cs`
```csharp
public class RoomPrefabLink : MonoBehaviour
{
    public RoomBlueprint blueprint;
}
```

## Pre-existing Sözleşme — DOKUNMA

`Assets/Scripts/Runtime/Rooms/GateSocket.cs`, `MobSpawnPoint.cs`, `RoomMetadata.cs` — bu task'ta dokunma. F2'de inspector entegrasyonu yapılacak.

## Acceptance Criteria

- [ ] `RIMA → Room Designer` menü item açılıyor
- [ ] Window içinde toolbar + sol/orta/sağ alan görünüyor
- [ ] Canvas IMGUI render'lıyor (boş Tilemap), 60fps+ idle
- [ ] Mouse canvas üstündeyken hover cell hesaplanıyor (debug toolbar'da `cell: (x,y)` göster)
- [ ] `Save Room` butonu basınca atomic save çalışıyor → `Assets/_Generated/Rooms/_TEST/test_room.prefab` + `.asset` pair üretiyor
- [ ] Save fail durumu (örn: dosya yazılamaz) → orphan asset bırakmıyor (rollback delete çalışıyor) — bunu test için bir try/catch deneyi ekle, beklenmedik path testi gerekmez
- [ ] Compile error yok, console temiz
- [ ] `IRoomDesignerContext` interface dolu ve `RimaRoomDesignerWindow` onu implement ediyor (Palette ve Brush bu kontrata karşı geliştirilecek)

## CODEX_DISPATCH Global Kurallar

- Model: **gpt-5.5** (o4-mini YASAK)
- Yorum yazma — WHY açık değilse istisna
- Hata yönetimi sadece sistem sınırlarında (file IO, AssetDatabase çağrıları)
- Test yoksa önce test yaz: bu task için EditMode test `Assets/Tests/EditMode/Editor/RoomDesignerSkeletonTests.cs`:
  - Window açılabiliyor mu (`EditorWindow.GetWindow<RimaRoomDesignerWindow>()`)
  - `IRoomDesignerContext` default state geçerli mi
  - `RoomSaver.Save` happy path geçiyor mu (geçici klasörde, sonra cleanup)

## Güven Döngüsü (CODEX_DISPATCH zorunlu)

1. Implementasyon → "%100 güven var mı?"
2. Açıklar → düzelt → tekrar değerlendir
3. %100 güven → commit

## Commit Message

```
feat(room-designer): F1 skeleton — EditorWindow + UI Toolkit shell + IMGUIContainer canvas + atomic save

- RimaRoomDesignerWindow + UXML/USS layout (toolbar + 3 panel slot)
- RoomDesignerCanvas (IMGUIContainer + Handles.DrawCamera + plane raycast)
- IRoomDesignerContext contract (Palette/Brush wire'lanacak)
- RoomSaver atomic save pattern (Prefab + RoomBlueprint pair, manual rollback)
- EditMode tests skeleton (window open + save happy path)
```

## Kaynak

- `STAGING/roomdesigner_codex_review.md` — 6 teknik soru cevabı (özellikle Soru 1, 3)
- `MEMORY/project_room_designer_plan.md` — mimari karar tablosu
