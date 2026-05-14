# S75-C — Object Layer (Faz 1.5 Stub Impl)

**Effort:** medium
**Prereq:** S75-B merged

---

## GOAL

PixelLab Map Tool'un `objects/manifest.json` katmanına karşılık gelen NPC/prop/spawn placement sistemi. Map Designer'da [Objects] toolbar butonu şu an placeholder; gerçek panel + save/load + apply-to-scene.

---

## DATA MODEL

### MapObjectPlacement
**Yeni dosya:** `Assets/Scripts/Systems/Map/MapObjectPlacement.cs`

```csharp
using System;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [Serializable]
    public class MapObjectPlacement
    {
        public string id = Guid.NewGuid().ToString();
        public string prefabPath;     // "Assets/Prefabs/Mobs/Knight.prefab"
        public Vector2 positionPx;    // canvas pixel coords (relative to bounding box)
        public int layer = 0;         // sort order
        public bool visible = true;
        public string displayName;    // e.g., "Knight Patrol A"
    }
}
```

### MapSaveData extend
**File:** `Assets/Editor/RimaMapDesignerWindow.cs`

```csharp
[Serializable]
public class MapSaveData
{
    public int width;
    public int height;
    public int[] terrainGrid;
    public string biomePresetGuid;
    public LayerSaveData[] layers;
    public int[] vertexData;
    public string[] layerNames;
    public RIMA.Systems.Map.MapObjectPlacement[] objects; // NEW
}
```

Save/Load roundtrip objects array.

---

## UI: ObjectsPanel (slide-out right panel)

**Yeni dosya:** `Assets/Editor/ObjectsPanelDrawer.cs`

```csharp
namespace RIMA.Editor {
    public class ObjectsPanelDrawer {
        public bool isOpen = false;
        public string activePrefabFolder = "Assets/Prefabs/Mobs";
        public GameObject selectedPrefab;
        public bool placeMode = false;
        
        public void Draw(Rect panelRect, List<MapObjectPlacement> objects, Action<MapObjectPlacement> onAdd, Action<MapObjectPlacement> onRemove) {
            // Header
            // Folder selector: Mobs / Props / SpawnPoints
            // Prefab list (scrollable, with preview thumbnail via AssetPreview)
            // [Place Mode] toggle
            // List of placed objects (id, prefab name, position) with [Remove] button
        }
    }
}
```

**Map Designer integration:**
- Toolbar [Objects] button (currently placeholder) → toggle `objectsPanel.isOpen`
- Right panel'in width'ini ObjectsPanel açıkken 200 → 360'a genişlet
- ObjectsPanelDrawer.Draw(rightPanelRect, ...) çağır

**Place mode behavior:**
- `placeMode && objectsPanel.isOpen && hover canvas && selectedPrefab != null` ise:
  - Cursor altında prefab AssetPreview yarı şeffaf overlay
  - Mouse click → new MapObjectPlacement { prefabPath, positionPx, displayName } add
- Esc / [Place Mode] toggle off → cancel

**Render placed objects on canvas:**
- DrawGridCanvas içinde objects loop:
  - Her object için AssetPreview.GetAssetPreview(prefab) sprite
  - Position px / cellSize → canvas coords
  - Draw with GUI.DrawTexture
  - Click on object (when NOT in place mode) → select, show object id + [Remove] in panel

---

## ApplyToScene with objects

`ApplyToScene()` mevcut tilemap paint. **EKLE:**
- After tilemap paint, instantiate objects:
  ```csharp
  foreach (var obj in mapObjects) {
      var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(obj.prefabPath);
      if (prefab == null) continue;
      var instance = PrefabUtility.InstantiatePrefab(prefab, output.tilemap.transform.parent) as GameObject;
      Vector3 worldPos = output.tilemap.CellToWorld(new Vector3Int(...)) + offset based on positionPx;
      instance.transform.position = worldPos;
      instance.name = obj.displayName ?? prefab.name;
      Undo.RegisterCreatedObjectUndo(instance, "Place RIMA Object");
  }
  ```

---

## EXAMPLES FOLDERS

Eğer henüz yoksa oluştur:
- `Assets/Prefabs/Mobs/` (varsa kullan)
- `Assets/Prefabs/Props/` 
- `Assets/Prefabs/SpawnPoints/`

İçinde **placeholder prefab** koy (TR named):
- `PlayerSpawnPoint.prefab` (empty GameObject + Gizmos.DrawWireSphere)
- `MobSpawnPoint.prefab` 

---

## VALIDATION

1. `dotnet build` PASS
2. Map Designer aç → [Objects] toolbar → ObjectsPanel açılır
3. Folder dropdown → Mobs seç → prefab list yüklenir
4. Bir prefab tıkla → selected
5. [Place Mode] toggle → canvas üstünde cursor preview
6. Canvas tıkla → MapObjectPlacement add edilir, canvas'ta visible
7. Save map → JSON objects array contains entry
8. Load map → object re-rendered
9. Apply to Scene → instance Hierarchy'de görünür
10. Console error 0

**Screenshot:** STAGING/s75c_objects_panel.png

---

## COMMIT MESAJI

```
[S75-C] Object Layer (Faz 1.5 stub impl)

- MapObjectPlacement class (id, prefabPath, position, layer, displayName)
- MapSaveData.objects[] (JSON roundtrip)
- ObjectsPanelDrawer slide-out right panel: folder + prefab list + place mode
- Map Designer canvas renders placed objects with AssetPreview
- ApplyToScene instantiates prefabs at world positions
- Placeholder prefabs: PlayerSpawnPoint, MobSpawnPoint
```
