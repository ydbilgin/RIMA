# Codex Task: "Apply Hades Preset" Button — RoomTemplateSOInspector

## Context

Multi-Layer Painter LOCK (Karar #147) yapısında **smart auto-bind** eksik. User Inspector'da Spawn_01 açtığında **manuel** olarak her layer'a sprite + offset + sortingOrder set ediyor. Buna "akıllı" demek için tek tıkla preset uygulayan button gerekiyor.

Var olan asset library (Spawn_01 için):
```
Assets/Art/Rooms/Backgrounds/Spawn_01/
├── layer_00_floor_painted_granite.png    (632×424, FloorBase)
├── layer_10_floor_variation_moss.png     (256×256, FloorVariation — Layer 1 in 8-scheme)
├── layer_01_decal_rift_crack.png         (256×256, RiftCrack — Layer 2)
├── layer_02_decal_rubble.png             (256×256, Rubble — Layer 3)
├── layer_11_wall_edge_stone.png          (384×384, WallEdge — Layer 4)  ← in production
├── layer_12_wall_decoration_vines.png    (128×128, WallDecoration — Layer 5)  ← in production
├── layer_03_prop_statue_silhouette.png   (256×256, Statue — Layer 6)
└── layer_04_accent_glow_mote.png         (128×128, GlowMote — Layer 7)
```

## Görev

`Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs` dosyasına **cerrahi ekleme**:

1. **OnInspectorGUI başına** yeni bir button bölümü:
```csharp
EditorGUILayout.Space(4f);
EditorGUILayout.LabelField("Quick Setup", EditorStyles.boldLabel);
if (GUILayout.Button(new GUIContent("Apply 8-Layer Hades Preset", "Auto-populate backgroundLayers with the canonical 8-layer scheme using sprites found in Assets/Art/Rooms/Backgrounds/<roomId>/"), GUILayout.Height(28))) {
    ApplyHadesPreset();
}
EditorGUILayout.Space(8f);
```

Bu button mevcut iterator-based default field draw'dan ÖNCE çağrılır (üstte görünsün).

2. **Yeni method `ApplyHadesPreset()`**:

```csharp
private static readonly (string semanticName, string fileGlob, int sortingOrder, float offsetX, float offsetY, float scaleX, float scaleY)[] HadesPreset = {
    ("FloorBase_PaintedGranite", "layer_00_floor_*",       -150,  0f,    0f,    1f,   1f),
    ("FloorVariation_Moss",       "layer_10_floor_variation_*", -140, -3f, -2f,  1f, 1f),
    ("Decal_RiftCrack",           "*decal_rift*",          -130,  0f,    0f,    1f,   1f),
    ("Scatter_Rubble",            "*decal_rubble*",        -120, -6f,  -4f,   1f,   1f),
    ("Wall_Edge_Stone",           "*wall_edge*",           -110,  0f,   -5.5f, 1f,   1f),
    ("WallDecoration_Vines",      "*wall_decoration*",     -100,  5f,   -5f,   1f,   1f),
    ("Prop_StatueSilhouette",     "*statue*",              -80,   6f,    4f,   1f,   1f),
    ("Ambient_GlowMote",          "*glow*",                -60,   3.5f,  2.8f, 1f,   1f),
};

private void ApplyHadesPreset()
{
    var template = target as RIMA.MapDesigner.Room.Data.RoomTemplateSO;
    if (template == null) return;
    
    string roomId = string.IsNullOrEmpty(template.roomId) ? "Spawn_01" : template.roomId;
    string baseDir = $"Assets/Art/Rooms/Backgrounds/{roomId}";
    
    if (!AssetDatabase.IsValidFolder(baseDir)) {
        EditorUtility.DisplayDialog("Asset Folder Missing", $"No asset folder found at: {baseDir}\n\nGenerate layers first, then retry.", "OK");
        return;
    }
    
    Undo.RecordObject(template, "Apply Hades Preset");
    
    serializedObject.Update();
    _layersProp.arraySize = HadesPreset.Length;
    
    int matched = 0;
    for (int i = 0; i < HadesPreset.Length; i++) {
        var preset = HadesPreset[i];
        var elem = _layersProp.GetArrayElementAtIndex(i);
        elem.FindPropertyRelative("layerName").stringValue = preset.semanticName;
        elem.FindPropertyRelative("sortingOrder").intValue = preset.sortingOrder;
        elem.FindPropertyRelative("offset").vector2Value = new Vector2(preset.offsetX, preset.offsetY);
        elem.FindPropertyRelative("scale").vector2Value = new Vector2(preset.scaleX, preset.scaleY);
        elem.FindPropertyRelative("tint").colorValue = Color.white;
        elem.FindPropertyRelative("visible").boolValue = true;
        
        // Find sprite by file glob in roomId folder
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { baseDir });
        Sprite found = null;
        foreach (var guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string filename = System.IO.Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
            if (System.Text.RegularExpressions.Regex.IsMatch(filename, preset.fileGlob.Replace("*", ".*").ToLowerInvariant())) {
                found = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                break;
            }
        }
        elem.FindPropertyRelative("sprite").objectReferenceValue = found;
        if (found != null) matched++;
    }
    
    serializedObject.ApplyModifiedProperties();
    EditorUtility.SetDirty(template);
    AssetDatabase.SaveAssets();
    
    Debug.Log($"[Hades Preset] Applied to '{template.roomId}'. Matched {matched}/{HadesPreset.Length} sprites from {baseDir}");
}
```

3. Eğer mevcut `using` satırlarında System.IO veya System.Text.RegularExpressions yoksa, fully-qualified isimler kullan (yukarıda zaten kullanılmış). Asmdef referansı SystemAssemblies'ı kapsar, problem yok.

## Hard limits

- KOD YAZMA başka dosyaya — sadece bu Inspector script'ine ekle
- `_layersProp` already declared (existing field)
- `serializedObject` accessor (Editor base class) zaten kullanılıyor
- EditorUtility.DisplayDialog BANNED memory `feedback_no_dialog.md` der — onun yerine `Debug.LogWarning` kullan. Yani folder missing case'i:
```csharp
if (!AssetDatabase.IsValidFolder(baseDir)) {
    Debug.LogWarning($"[Hades Preset] No asset folder found at: {baseDir}. Generate layers first, then retry.");
    return;
}
```

## Output

`CODEX_DONE_hades_preset_button.md`:
- Status: SUCCESS / FAILED
- File modified: `Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs`
- Lines added (count)
- Compile: PASS / FAIL (Unity compile after, check console)
- Test plan for user: "Open Spawn_01.asset → Inspector → click 'Apply 8-Layer Hades Preset' button at top → backgroundLayers fills with 8 entries"

Tek dosya değişiklik. Surgical. Compile after, console clean check.
