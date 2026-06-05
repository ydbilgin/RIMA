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
        }

        private void DrawCustomOffsetsSection(SerializedProperty handOffsetsProp)
        {
            OrientationSync sync = (OrientationSync)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Hand Anchor Offsets Tuning Tool", EditorStyles.boldLabel);
            
            EditorGUILayout.HelpBox("Select a direction below or click the markers in SceneView to edit the hand anchor offsets.", MessageType.Info);

            EditorGUI.BeginChangeCheck();
            _selectedDirectionIndex = EditorGUILayout.Popup("Active Direction", _selectedDirectionIndex, DirectionNames);
            _selectedDirectionIndex = GUILayout.SelectionGrid(_selectedDirectionIndex, DirectionNames, 4);
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }

            EditorGUILayout.Space(5);

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
                    SceneView.RepaintAll();
                }

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
