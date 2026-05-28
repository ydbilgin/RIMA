# Codex Task: Zone-to-Layer Brush Auto-Bind System

## Context

User direktifi (S92): "bunları brush mantığında otomatik set edecek tarzda ayarlamalısın" — Map Designer Brush V1 mantığıyla **zone tıklanınca layer otomatik backgroundLayers'a ekleniyor**.

Mevcut:
- `Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs` Custom Inspector ZAten "Apply 8-Layer Hades Preset" button içeriyor.
- `Assets/Scripts/MapDesigner/Room/Data/BackgroundLayerData.cs` veri modeli LIVE.
- 8 asset Spawn_01 için üretildi: floor, floor_variation_moss, decal_rift_crack, decal_rubble, wall_edge_stone, wall_decoration_vines, prop_statue_silhouette, accent_glow_mote.
- 3 ek floor patch (path, soil, grass) in-flight üretiliyor.

## Görev

İki bileşen:

### Bileşen 1: ZoneToLayerMappingSO (Runtime SO)

Path: `Assets/Scripts/MapDesigner/Room/Data/ZoneToLayerMappingSO.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [CreateAssetMenu(menuName = "RIMA/Room/ZoneToLayerMapping", fileName = "ZoneToLayerMapping_New", order = 210)]
    public class ZoneToLayerMappingSO : ScriptableObject
    {
        [System.Serializable]
        public class ZoneLayerEntry
        {
            public string zoneId = "stone";
            public string displayName = "Stone Floor";
            public Sprite sprite;
            public int defaultSortingOrder = -100;
            public Vector2 defaultOffset = Vector2.zero;
            public Vector2 defaultScale = Vector2.one;
            public Color defaultTint = Color.white;
        }

        public List<ZoneLayerEntry> zoneMap = new List<ZoneLayerEntry>();

        public ZoneLayerEntry GetForZone(string zoneId)
        {
            foreach (var entry in zoneMap)
            {
                if (entry != null && string.Equals(entry.zoneId, zoneId, System.StringComparison.OrdinalIgnoreCase))
                    return entry;
            }
            return null;
        }
    }
}
```

### Bileşen 2: Default Mapping Asset

Path: `Assets/Data/Blueprint/ZoneLayerMap_Default.asset` (ScriptableObject Spawn_01 için 11 zone mapping):

Codex bunu execute_code veya Unity AssetDatabase üzerinden oluşturabilir, ya da bir editor script ile generate edebilir.

11 zone mapping (sprite path → filename glob ile bul):
- "stone" → "layer_00_floor_painted_granite" → -150
- "path" → "layer_20_floor_patch_stone_path" → -140 (henüz yok, oluşur)
- "grass" → "layer_22_floor_patch_grass" → -135 (henüz yok)
- "soil" → "layer_21_floor_patch_warm_soil" → -135 (henüz yok)
- "moss" → "layer_10_floor_variation_moss" → -135
- "rift_crack" → "layer_01_decal_rift_crack" → -130
- "rubble" → "layer_02_decal_rubble" → -120
- "wall" → "layer_11_wall_edge_stone" → -110
- "wall_vines" → "layer_12_wall_decoration_vines" → -100
- "statue" → "layer_03_prop_statue_silhouette" → -80
- "glow" → "layer_04_accent_glow_mote" → -60

Sprite path = `Assets/Art/Rooms/Backgrounds/Spawn_01/<filename>.png` (henüz yok olanlar in-flight üretiliyor, asset yoksa null kalsın — sprite generation bitince user manuel link eder veya re-run preset).

### Bileşen 3: RoomTemplateSOInspector ekleme

`Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs` "Apply 8-Layer Hades Preset" button'unun ALTINA yeni section:

```csharp
EditorGUILayout.Space(8f);
EditorGUILayout.LabelField("Brush Zone Add (auto-bind from ZoneToLayerMappingSO)", EditorStyles.boldLabel);

// Auto-find default mapping
if (_zoneMapCached == null)
{
    string[] guids = AssetDatabase.FindAssets("t:ZoneToLayerMappingSO");
    if (guids.Length > 0)
    {
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        _zoneMapCached = AssetDatabase.LoadAssetAtPath<RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO>(path);
    }
}

_zoneMapCached = (RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO)EditorGUILayout.ObjectField("Zone Map", _zoneMapCached, typeof(RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO), false);

if (_zoneMapCached != null && _zoneMapCached.zoneMap != null && _zoneMapCached.zoneMap.Count > 0)
{
    EditorGUILayout.HelpBox("Click a zone button to add a layer at room center (offset can be edited after).", MessageType.None);
    
    // Render zone buttons in 4-column grid
    int perRow = 4;
    int rowCount = (_zoneMapCached.zoneMap.Count + perRow - 1) / perRow;
    for (int row = 0; row < rowCount; row++)
    {
        EditorGUILayout.BeginHorizontal();
        for (int col = 0; col < perRow; col++)
        {
            int idx = row * perRow + col;
            if (idx >= _zoneMapCached.zoneMap.Count) break;
            var entry = _zoneMapCached.zoneMap[idx];
            if (entry == null) continue;
            string label = !string.IsNullOrEmpty(entry.displayName) ? entry.displayName : entry.zoneId;
            bool disabled = entry.sprite == null;
            EditorGUI.BeginDisabledGroup(disabled);
            if (GUILayout.Button(new GUIContent(label, disabled ? "Sprite missing — generate or assign first" : $"Add {entry.zoneId} layer to room"), GUILayout.Height(22), GUILayout.MinWidth(80)))
            {
                AddZoneLayer(entry);
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();
    }
}
else
{
    EditorGUILayout.HelpBox("No ZoneToLayerMappingSO found. Create one via Assets > Create > RIMA > Room > ZoneToLayerMapping.", MessageType.Warning);
}
EditorGUILayout.Space(8f);
```

`AddZoneLayer` method:

```csharp
private void AddZoneLayer(RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO.ZoneLayerEntry entry)
{
    var template = target as RIMA.MapDesigner.Room.Data.RoomTemplateSO;
    if (template == null || entry == null || entry.sprite == null) return;
    
    Undo.RecordObject(template, $"Add Zone Layer: {entry.zoneId}");
    serializedObject.Update();
    
    int newIndex = _layersProp.arraySize;
    _layersProp.arraySize++;
    var elem = _layersProp.GetArrayElementAtIndex(newIndex);
    elem.FindPropertyRelative("layerName").stringValue = !string.IsNullOrEmpty(entry.displayName) ? entry.displayName : entry.zoneId;
    elem.FindPropertyRelative("sprite").objectReferenceValue = entry.sprite;
    elem.FindPropertyRelative("sortingOrder").intValue = entry.defaultSortingOrder;
    elem.FindPropertyRelative("offset").vector2Value = entry.defaultOffset;
    elem.FindPropertyRelative("scale").vector2Value = entry.defaultScale;
    elem.FindPropertyRelative("tint").colorValue = entry.defaultTint;
    elem.FindPropertyRelative("visible").boolValue = true;
    
    serializedObject.ApplyModifiedProperties();
    EditorUtility.SetDirty(template);
    AssetDatabase.SaveAssets();
    
    Debug.Log($"[Zone Add] Added '{entry.zoneId}' layer to '{template.roomId}' (index {newIndex}). Sprite: {entry.sprite.name}, sortingOrder: {entry.defaultSortingOrder}");
}
```

Cache field eklenecek: `private RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO _zoneMapCached;`

### Bileşen 4: Generate default mapping asset

Asset oluşturma için Unity editor script veya inline execute_code önerebilirsin. Codex execute_code üzerinden:

```csharp
var so = ScriptableObject.CreateInstance<RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO>();
so.zoneMap = new System.Collections.Generic.List<...>();
// Populate 11 entries with sprite lookup by filename glob
UnityEditor.AssetDatabase.CreateAsset(so, "Assets/Data/Blueprint/ZoneLayerMap_Default.asset");
```

Eğer sprite henüz Assets'te yoksa (path/grass/soil in-flight), entry sprite=null bırakılır, user sonradan link eder.

## Hard limits

- Sadece 1 NEW SO class (.cs) + 1 NEW asset file + Inspector mevcut dosyaya ekleme
- Memory `feedback_no_dialog.md` ban — `EditorUtility.DisplayDialog` YOK, `Debug.Log/LogWarning` kullan
- Compile clean, mevcut tests 419/419 PASS kalsın
- Asmdef değişikliği gerek yok (mevcut RIMA.Runtime + RIMA.MapDesigner.Editor coverage)

## Output

`CODEX_DONE_brush_zone_layer_binding.md`:
- Status: SUCCESS / FAILED
- Files: 1 new SO class + 1 new asset + 1 Inspector edit
- Compile: PASS/FAIL
- Test plan: User opens Spawn_01.asset → Inspector → "Brush Zone Add" section → click "Stone Floor" / "Path" / "Grass" → backgroundLayers'a entry eklenir, scene re-spawn'da görünür.

Final goal: Brush V1 paint stroke hook PHASE 2'de eklenir (şu an dropdown/button olur, brush gerçek stroke entegrasyonu sonraki sprint).
