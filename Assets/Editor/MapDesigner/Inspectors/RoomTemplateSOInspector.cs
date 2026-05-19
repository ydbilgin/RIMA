using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using RIMA.MapDesigner.Room.Data;

namespace RIMA.MapDesigner.Room.Editor
{
    [CustomEditor(typeof(RoomTemplateSO))]
    public class RoomTemplateSOInspector : UnityEditor.Editor
    {
        private ReorderableList _layerList;
        private SerializedProperty _layersProp;
        private RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO _zoneMapCached;

        private static readonly (string semanticName, string fileGlob, int sortingOrder, float offsetX, float offsetY, float scaleX, float scaleY)[] HadesPreset =
        {
            ("FloorBase_PaintedGranite", "layer_00_floor_*", -150, 0f, 0f, 1f, 1f),
            ("FloorVariation_Moss", "layer_10_floor_variation_*", -140, -3f, -2f, 1f, 1f),
            ("Decal_RiftCrack", "*decal_rift*", -130, 0f, 0f, 1f, 1f),
            ("Scatter_Rubble", "*decal_rubble*", -120, -6f, -4f, 1f, 1f),
            ("Wall_Edge_Stone", "*wall_edge*", -110, 0f, -5.5f, 1f, 1f),
            ("WallDecoration_Vines", "*wall_decoration*", -100, 5f, -5f, 1f, 1f),
            ("Prop_StatueSilhouette", "*statue*", -80, 6f, 4f, 1f, 1f),
            ("Ambient_GlowMote", "*glow*", -60, 3.5f, 2.8f, 1f, 1f),
        };

        private void OnEnable()
        {
            _layersProp = serializedObject.FindProperty("backgroundLayers");
            if (_layersProp == null) return;

            _layerList = new ReorderableList(serializedObject, _layersProp, true, true, true, true);
            _layerList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Painted Background Layers (drag to reorder, sortingOrder = z)");
            };
            _layerList.elementHeightCallback = index => GetElementHeight(index);
            _layerList.drawElementCallback = (rect, index, active, focused) => DrawElement(rect, index);
            _layerList.onAddCallback = list =>
            {
                _layersProp.arraySize++;
                var newElem = _layersProp.GetArrayElementAtIndex(_layersProp.arraySize - 1);
                ResetLayerToDefaults(newElem);
            };
        }

        private static void ResetLayerToDefaults(SerializedProperty layer)
        {
            layer.FindPropertyRelative("layerName").stringValue = $"Layer";
            layer.FindPropertyRelative("sprite").objectReferenceValue = null;
            layer.FindPropertyRelative("sortingOrder").intValue = -100;
            layer.FindPropertyRelative("offset").vector2Value = Vector2.zero;
            layer.FindPropertyRelative("scale").vector2Value = Vector2.one;
            layer.FindPropertyRelative("tint").colorValue = Color.white;
            layer.FindPropertyRelative("visible").boolValue = true;
        }

        private float GetElementHeight(int index)
        {
            return EditorGUIUtility.singleLineHeight * 5f + 12f;
        }

        private void DrawElement(Rect rect, int index)
        {
            var layer = _layersProp.GetArrayElementAtIndex(index);
            var nameProp = layer.FindPropertyRelative("layerName");
            var spriteProp = layer.FindPropertyRelative("sprite");
            var orderProp = layer.FindPropertyRelative("sortingOrder");
            var offsetProp = layer.FindPropertyRelative("offset");
            var scaleProp = layer.FindPropertyRelative("scale");
            var tintProp = layer.FindPropertyRelative("tint");
            var visibleProp = layer.FindPropertyRelative("visible");

            float line = EditorGUIUtility.singleLineHeight;
            float pad = 2f;
            float y = rect.y + pad;

            float thumbSize = 40f;
            Rect thumbRect = new Rect(rect.x, y, thumbSize, thumbSize);
            Sprite spriteRef = spriteProp.objectReferenceValue as Sprite;
            if (spriteRef != null)
            {
                Texture2D preview = AssetPreview.GetAssetPreview(spriteRef);
                if (preview != null)
                {
                    GUI.DrawTexture(thumbRect, preview, ScaleMode.ScaleToFit);
                }
                else
                {
                    EditorGUI.LabelField(thumbRect, "...");
                    if (AssetPreview.IsLoadingAssetPreview(spriteRef.GetInstanceID()))
                    {
                        Repaint();
                    }
                }
            }
            else
            {
                EditorGUI.HelpBox(thumbRect, "no sprite", MessageType.None);
            }

            float colX = rect.x + thumbSize + 6f;
            float colW = rect.width - thumbSize - 6f;

            EditorGUI.PropertyField(new Rect(colX, y, colW * 0.5f, line), nameProp, GUIContent.none);
            EditorGUI.PropertyField(new Rect(colX + colW - 60f, y, 60f, line), visibleProp, new GUIContent("vis"));
            y += line + pad;

            EditorGUI.PropertyField(new Rect(colX, y, colW, line), spriteProp, GUIContent.none);
            y += line + pad;

            EditorGUI.PropertyField(new Rect(colX, y, colW * 0.45f, line), orderProp, new GUIContent("order"));
            EditorGUI.PropertyField(new Rect(colX + colW * 0.5f, y, colW * 0.5f, line), offsetProp, new GUIContent("offset"));
            y += line + pad;

            EditorGUI.PropertyField(new Rect(colX, y, colW * 0.45f, line), scaleProp, new GUIContent("scale"));
            EditorGUI.PropertyField(new Rect(colX + colW * 0.5f, y, colW * 0.5f, line), tintProp, new GUIContent("tint"));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(4f);
            EditorGUILayout.LabelField("Quick Setup", EditorStyles.boldLabel);
            if (GUILayout.Button(new GUIContent("Apply 8-Layer Hades Preset", "Auto-populate backgroundLayers with the canonical 8-layer scheme using sprites found in Assets/Art/Rooms/Backgrounds/<roomId>/"), GUILayout.Height(28)))
            {
                ApplyHadesPreset();
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Brush Zone Add (auto-bind from ZoneToLayerMappingSO)", EditorStyles.boldLabel);

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
                        if (GUILayout.Button(new GUIContent(label, disabled ? "Sprite missing - generate or assign first" : $"Add {entry.zoneId} layer to room"), GUILayout.Height(22), GUILayout.MinWidth(80)))
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

            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            while (iterator.NextVisible(false))
            {
                if (iterator.propertyPath == "backgroundLayers") continue;
                EditorGUILayout.PropertyField(iterator, true);
            }

            EditorGUILayout.Space(8f);
            if (_layerList != null)
            {
                _layerList.DoLayoutList();
            }
            else
            {
                EditorGUILayout.HelpBox("backgroundLayers serialized property not found on RoomTemplateSO.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ApplyHadesPreset()
        {
            var template = target as RIMA.MapDesigner.Room.Data.RoomTemplateSO;
            if (template == null) return;

            string roomId = string.IsNullOrEmpty(template.roomId) ? "Spawn_01" : template.roomId;
            string baseDir = $"Assets/Art/Rooms/Backgrounds/{roomId}";

            if (!AssetDatabase.IsValidFolder(baseDir))
            {
                Debug.LogWarning($"[Hades Preset] No asset folder found at: {baseDir}. Generate layers first, then retry.");
                return;
            }

            Undo.RecordObject(template, "Apply Hades Preset");

            serializedObject.Update();
            _layersProp.arraySize = HadesPreset.Length;

            int matched = 0;
            string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { baseDir });
            for (int i = 0; i < HadesPreset.Length; i++)
            {
                var preset = HadesPreset[i];
                var elem = _layersProp.GetArrayElementAtIndex(i);
                elem.FindPropertyRelative("layerName").stringValue = preset.semanticName;
                elem.FindPropertyRelative("sortingOrder").intValue = preset.sortingOrder;
                elem.FindPropertyRelative("offset").vector2Value = new Vector2(preset.offsetX, preset.offsetY);
                elem.FindPropertyRelative("scale").vector2Value = new Vector2(preset.scaleX, preset.scaleY);
                elem.FindPropertyRelative("tint").colorValue = Color.white;
                elem.FindPropertyRelative("visible").boolValue = true;

                Sprite found = null;
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    string filename = System.IO.Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                    string pattern = preset.fileGlob.Replace("*", ".*").ToLowerInvariant();
                    if (System.Text.RegularExpressions.Regex.IsMatch(filename, pattern))
                    {
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

        private void AddZoneLayer(RIMA.MapDesigner.Room.Data.ZoneToLayerMappingSO.ZoneLayerEntry entry)
        {
            var template = target as RIMA.MapDesigner.Room.Data.RoomTemplateSO;
            if (template == null || entry == null || entry.sprite == null || _layersProp == null) return;

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
    }
}
