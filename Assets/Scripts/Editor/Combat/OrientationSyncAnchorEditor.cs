using UnityEditor;
using UnityEngine;
using RIMA.Combat;
using RIMA.Core;

namespace RIMA.Combat
{
    [CustomEditor(typeof(OrientationSync))]
    public class OrientationSyncAnchorEditor : global::UnityEditor.Editor
    {
        private static readonly string[] DirectionNames = new string[8]
        {
            "S", "SE", "E", "NE", "N", "NW", "W", "SW"
        };

        private int _selectedDirectionIndex = 0;
        private bool _showAllOffsets = false;
        private bool _showAllRotations = false;
        private bool _largeStep = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw all properties except handOffsets
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (prop.name == "handOffsets")
                    {
                        DrawCustomOffsetsSection(prop);
                    }
                    else if (prop.name == "weaponRotations")
                    {
                        // Drawn in the tuning tool together with the matching offset.
                    }
                    else if (prop.name != "m_Script")
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                    else
                    {
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(prop, true);
                        GUI.enabled = true;
                    }
                } while (prop.NextVisible(false));
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(10);
            GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
            if (GUILayout.Button("Offsets + Rotations -> SOURCE PREFAB KAYDET", GUILayout.Height(28)))
            {
                SaveOffsetsToPrefab();
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.HelpBox(
                "Play modda yaptigin ayarlar cikinca kaybolur -> bu buton live instance'in source prefab'ina yazar (kalici). " +
                "NOT: Scene'de SILAHI surukleme (Sync her kare geri alir); sari Active-yon isaretcisini surukle ya da Offset/Weapon Rotations sayilarini gir + Enter.",
                MessageType.Info);
        }

        private void SaveOffsetsToPrefab()
        {
            var sync = (OrientationSync)target;
            if (!TryResolveSaveTarget(sync, out GameObject prefabGo, out OrientationSync prefabSync, out string path))
            {
                EditorUtility.DisplayDialog("Kayit basarisiz", "Source prefab / OrientationSync bulunamadi.", "Tamam");
                return;
            }

            var src = new SerializedObject(sync);
            var dst = new SerializedObject(prefabSync);
            CopyArrayProp(src, dst, "handOffsets");
            CopyArrayProp(src, dst, "weaponRotations");
            dst.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(prefabSync);
            PrefabUtility.SavePrefabAsset(prefabGo);
            AssetDatabase.SaveAssets();
            Debug.Log($"[OrientationSync] handOffsets + weaponRotations -> {path} kaydedildi.");
        }

        private static bool TryResolveSaveTarget(OrientationSync source, out GameObject prefabGo, out OrientationSync prefabSync, out string path)
        {
            prefabGo = null;
            prefabSync = null;
            path = null;

            OrientationSync sourcePrefabSync = null;
            if (source != null && !EditorUtility.IsPersistent(source))
                sourcePrefabSync = PrefabUtility.GetCorrespondingObjectFromSource(source) as OrientationSync;

            if (sourcePrefabSync != null)
            {
                path = AssetDatabase.GetAssetPath(sourcePrefabSync);
                prefabGo = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                prefabSync = prefabGo != null ? prefabGo.GetComponentInChildren<OrientationSync>(true) : null;
                return prefabGo != null && prefabSync != null;
            }

            if (source != null && EditorUtility.IsPersistent(source))
            {
                path = AssetDatabase.GetAssetPath(source);
                prefabGo = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                prefabSync = source;
                return prefabGo != null;
            }

            return false;
        }

        private static void CopyArrayProp(SerializedObject src, SerializedObject dst, string propName)
        {
            var s = src.FindProperty(propName);
            var d = dst.FindProperty(propName);
            if (s == null || d == null || !s.isArray) return;
            d.arraySize = s.arraySize;
            for (int i = 0; i < s.arraySize; i++)
            {
                var se = s.GetArrayElementAtIndex(i);
                var de = d.GetArrayElementAtIndex(i);
                if (se.propertyType == SerializedPropertyType.Vector2) de.vector2Value = se.vector2Value;
                else if (se.propertyType == SerializedPropertyType.Float) de.floatValue = se.floatValue;
            }
        }

        private void DrawCustomOffsetsSection(SerializedProperty handOffsetsProp)
        {
            OrientationSync sync = (OrientationSync)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Hand Anchor Offsets Tuning Tool", EditorStyles.boldLabel);
            
            EditorGUILayout.HelpBox(
                "1) Select direction. 2) Face that direction in Play. 3) Use Nudge + Rotate until it looks right. 4) Press Player.prefab KAYDET.",
                MessageType.Info);

            EditorGUI.BeginChangeCheck();
            _selectedDirectionIndex = EditorGUILayout.Popup("Active Direction", _selectedDirectionIndex, DirectionNames);
            _selectedDirectionIndex = GUILayout.SelectionGrid(_selectedDirectionIndex, DirectionNames, 4);
            if (EditorGUI.EndChangeCheck())
            {
                ApplySelectedDirectionPreview(sync);
                SceneView.RepaintAll();
            }

            EditorGUILayout.Space(5);
            _largeStep = EditorGUILayout.ToggleLeft("Large step (offset 0.05 / rotation 15)", _largeStep);

            if (handOffsetsProp.arraySize >= 8)
            {
                SerializedProperty activeOffsetProp = handOffsetsProp.GetArrayElementAtIndex(_selectedDirectionIndex);

                EditorGUI.BeginChangeCheck();
                Vector2 newOffset = EditorGUILayout.Vector2Field($"Offset ({DirectionNames[_selectedDirectionIndex]})", activeOffsetProp.vector2Value);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(sync, "Modify Hand Offset");
                    activeOffsetProp.vector2Value = newOffset;
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(sync);
                    ApplySelectedDirectionPreview(sync);
                    SceneView.RepaintAll();
                }

                DrawOffsetNudgeControls(sync, activeOffsetProp);
                DrawRotationControls(sync);
                DrawSortRuleHint();

                EditorGUILayout.Space(5);

                _showAllOffsets = EditorGUILayout.Foldout(_showAllOffsets, "Show All 8 Offsets List", true);
                if (_showAllOffsets)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 8; i++)
                    {
                        SerializedProperty elementProp = handOffsetsProp.GetArrayElementAtIndex(i);
                        EditorGUI.BeginChangeCheck();

                        GUIStyle labelStyle = (i == _selectedDirectionIndex) ? EditorStyles.boldLabel : EditorStyles.label;
                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(DirectionNames[i], labelStyle, GUILayout.Width(50));
                        Vector2 val = EditorGUILayout.Vector2Field("", elementProp.vector2Value);
                        EditorGUILayout.EndHorizontal();

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(sync, "Modify Hand Offset");
                            elementProp.vector2Value = val;
                            serializedObject.ApplyModifiedProperties();
                            EditorUtility.SetDirty(sync);
                            if (i == _selectedDirectionIndex)
                                ApplySelectedDirectionPreview(sync);
                            SceneView.RepaintAll();
                        }
                    }
                    EditorGUI.indentLevel--;
                }

                SerializedProperty rotationsProp = serializedObject.FindProperty("weaponRotations");
                _showAllRotations = EditorGUILayout.Foldout(_showAllRotations, "Show All 8 Rotations List", true);
                if (_showAllRotations && rotationsProp != null && rotationsProp.arraySize >= 8)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 8; i++)
                    {
                        SerializedProperty rotProp = rotationsProp.GetArrayElementAtIndex(i);
                        EditorGUI.BeginChangeCheck();

                        GUIStyle labelStyle = (i == _selectedDirectionIndex) ? EditorStyles.boldLabel : EditorStyles.label;

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(DirectionNames[i], labelStyle, GUILayout.Width(50));
                        float val = EditorGUILayout.FloatField(rotProp.floatValue);
                        EditorGUILayout.EndHorizontal();

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(sync, "Modify Weapon Rotation");
                            rotProp.floatValue = val;
                            serializedObject.ApplyModifiedProperties();
                            EditorUtility.SetDirty(sync);
                            if (i == _selectedDirectionIndex)
                                ApplySelectedDirectionPreview(sync);
                            SceneView.RepaintAll();
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("handOffsets array is not initialized with 8 elements.", MessageType.Error);
            }
        }

        private void DrawOffsetNudgeControls(OrientationSync sync, SerializedProperty activeOffsetProp)
        {
            float step = _largeStep ? 0.05f : 0.01f;

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField($"Nudge Offset ({step:0.00})", EditorStyles.miniBoldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Left")) NudgeOffset(sync, activeOffsetProp, new Vector2(-step, 0f));
            if (GUILayout.Button("Right")) NudgeOffset(sync, activeOffsetProp, new Vector2(step, 0f));
            if (GUILayout.Button("Up")) NudgeOffset(sync, activeOffsetProp, new Vector2(0f, step));
            if (GUILayout.Button("Down")) NudgeOffset(sync, activeOffsetProp, new Vector2(0f, -step));
            EditorGUILayout.EndHorizontal();
        }

        private void NudgeOffset(OrientationSync sync, SerializedProperty activeOffsetProp, Vector2 delta)
        {
            Undo.RecordObject(sync, "Nudge Hand Offset");
            activeOffsetProp.vector2Value += delta;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(sync);
            ApplySelectedDirectionPreview(sync);
            SceneView.RepaintAll();
        }

        private void DrawRotationControls(OrientationSync sync)
        {
            SerializedProperty rotationsProp = serializedObject.FindProperty("weaponRotations");
            if (rotationsProp == null || rotationsProp.arraySize < 8)
            {
                EditorGUILayout.HelpBox("weaponRotations array is not initialized with 8 elements.", MessageType.Error);
                return;
            }

            SerializedProperty activeRotationProp = rotationsProp.GetArrayElementAtIndex(_selectedDirectionIndex);

            EditorGUILayout.Space(4);
            EditorGUI.BeginChangeCheck();
            float newRotation = EditorGUILayout.FloatField($"Rotation ({DirectionNames[_selectedDirectionIndex]})", activeRotationProp.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(sync, "Modify Weapon Rotation");
                activeRotationProp.floatValue = newRotation;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(sync);
                ApplySelectedDirectionPreview(sync);
                SceneView.RepaintAll();
            }

            float small = 5f;
            float large = 15f;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(_largeStep ? "-15" : "-5")) NudgeRotation(sync, activeRotationProp, _largeStep ? -large : -small);
            if (GUILayout.Button(_largeStep ? "+15" : "+5")) NudgeRotation(sync, activeRotationProp, _largeStep ? large : small);
            if (GUILayout.Button("Apply Selected Now")) ApplySelectedDirectionPreview(sync);
            EditorGUILayout.EndHorizontal();
        }

        private void NudgeRotation(OrientationSync sync, SerializedProperty activeRotationProp, float delta)
        {
            Undo.RecordObject(sync, "Nudge Weapon Rotation");
            activeRotationProp.floatValue += delta;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(sync);
            ApplySelectedDirectionPreview(sync);
            SceneView.RepaintAll();
        }

        private void DrawSortRuleHint()
        {
            bool behindBody = _selectedDirectionIndex == (int)FacingDir8.N
                || _selectedDirectionIndex == (int)FacingDir8.NE
                || _selectedDirectionIndex == (int)FacingDir8.NW
                || _selectedDirectionIndex == (int)FacingDir8.W;

            EditorGUILayout.HelpBox(
                $"{DirectionNames[_selectedDirectionIndex]} render: weapon {(behindBody ? "BEHIND body" : "IN FRONT of body")} (right-hand rule).",
                MessageType.None);
        }

        private void ApplySelectedDirectionPreview(OrientationSync sync)
        {
            if (sync == null) return;
            sync.Sync((FacingDir8)_selectedDirectionIndex);
            EditorUtility.SetDirty(sync);
            SceneView.RepaintAll();
        }

        private void OnSceneGUI()
        {
            OrientationSync sync = (OrientationSync)target;
            if (sync == null) return;

            serializedObject.Update();
            SerializedProperty handOffsetsProp = serializedObject.FindProperty("handOffsets");
            if (handOffsetsProp == null || handOffsetsProp.arraySize < 8) return;

            Color activeColor = Color.yellow;
            Color inactiveColor = new Color(0.7f, 0.7f, 0.7f, 0.8f);

            for (int i = 0; i < 8; i++)
            {
                SerializedProperty elementProp = handOffsetsProp.GetArrayElementAtIndex(i);
                Vector2 offsetValue = elementProp.vector2Value;
                Vector3 worldPos = LocalToWorld(sync, offsetValue);

                if (i == _selectedDirectionIndex)
                {
                    // Active handle
                    Handles.color = activeColor;
                    
                    GUIStyle activeLabelStyle = new GUIStyle();
                    activeLabelStyle.normal.textColor = activeColor;
                    activeLabelStyle.fontStyle = FontStyle.Bold;
                    activeLabelStyle.fontSize = 11;
                    activeLabelStyle.alignment = TextAnchor.MiddleCenter;
                    
                    Handles.Label(worldPos + new Vector3(0f, 0.08f, 0f), $"{DirectionNames[i]} (Active)", activeLabelStyle);

                    EditorGUI.BeginChangeCheck();
                    // Draw a 2D position handle. Z axis is ignored/zeroed out
                    Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(sync, "Modify Hand Offset");
                        Vector2 localPos = WorldToLocal(sync, newWorldPos);
                        elementProp.vector2Value = localPos;
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(sync);
                        ApplySelectedDirectionPreview(sync);
                        Repaint();
                    }
                }
                else
                {
                    // Inactive handles
                    Handles.color = inactiveColor;
                    
                    float size = HandleUtility.GetHandleSize(worldPos) * 0.15f;
                    float capSize = Mathf.Min(size, 0.04f);

                    // Clicking on an inactive point selects it
                    if (Handles.Button(worldPos, Quaternion.identity, capSize, capSize, Handles.DotHandleCap))
                    {
                        _selectedDirectionIndex = i;
                        Repaint();
                    }

                    GUIStyle inactiveLabelStyle = new GUIStyle();
                    inactiveLabelStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.9f);
                    inactiveLabelStyle.fontSize = 9;
                    inactiveLabelStyle.alignment = TextAnchor.MiddleCenter;
                    
                    Handles.Label(worldPos + new Vector3(0f, 0.08f, 0f), DirectionNames[i], inactiveLabelStyle);
                }
            }
        }

        private Vector3 LocalToWorld(OrientationSync sync, Vector2 localOffset)
        {
            Transform parent = null;
            var handAnchorProp = serializedObject.FindProperty("handAnchor");
            Transform handAnchor = handAnchorProp != null ? handAnchorProp.objectReferenceValue as Transform : null;

            if (handAnchor != null)
            {
                parent = handAnchor.parent;
            }

            if (parent == null)
            {
                parent = sync.transform;
            }

            return parent.TransformPoint(localOffset);
        }

        private Vector2 WorldToLocal(OrientationSync sync, Vector3 worldPos)
        {
            Transform parent = null;
            var handAnchorProp = serializedObject.FindProperty("handAnchor");
            Transform handAnchor = handAnchorProp != null ? handAnchorProp.objectReferenceValue as Transform : null;

            if (handAnchor != null)
            {
                parent = handAnchor.parent;
            }

            if (parent == null)
            {
                parent = sync.transform;
            }

            return parent.InverseTransformPoint(worldPos);
        }
    }
}
