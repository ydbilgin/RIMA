using UnityEditor;
using UnityEngine;
using RIMA.Core;

namespace RIMA.Combat
{
    public sealed class WarbladeMountTunerWindow : EditorWindow
    {
        private static readonly string[] DirectionNames =
        {
            "S", "SE", "E", "NE", "N", "NW", "W", "SW"
        };

        private static readonly string[] FallbackPrefabPaths =
        {
            "Assets/Prefabs/Characters/Warblade.prefab",
            "Assets/Resources/Prefabs/Warblade.prefab",
            "Assets/Prefabs/Player.prefab"
        };
        private const float FacingLockSeconds = 600f;

        private OrientationSync sync;
        private PlayerController player;
        private int selectedDirection;
        private bool largeStep;
        private bool autoApply = true;

        [MenuItem("RIMA/Warblade Mount Tuner")]
        private static void Open()
        {
            GetWindow<WarbladeMountTunerWindow>("Warblade Mount");
        }

        private void OnEnable()
        {
            AutoFindTargets();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Warblade Mount Tuner", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Play modda yon secince karakter o yone kilitlenir. Offset/Rotation ile kilici ayarla, sonra SOURCE PREFAB KAYDET.",
                MessageType.Info);
            DrawModeStatus();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Find Player", GUILayout.Height(24)))
            {
                AutoFindTargets();
                ApplySelectedDirection();
            }
            if (GUILayout.Button("Select Target", GUILayout.Height(24)) && sync != null)
                Selection.activeObject = sync.gameObject;
            EditorGUILayout.EndHorizontal();

            sync = (OrientationSync)EditorGUILayout.ObjectField("OrientationSync", sync, typeof(OrientationSync), true);
            player = (PlayerController)EditorGUILayout.ObjectField("PlayerController", player, typeof(PlayerController), true);
            EditorGUILayout.LabelField("Save Target", ResolveSavePath(sync) ?? "not found");

            autoApply = EditorGUILayout.ToggleLeft("Auto apply selected direction", autoApply);
            largeStep = EditorGUILayout.ToggleLeft("Large step (offset 0.05 / rotation 15)", largeStep);

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Direction", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            selectedDirection = GUILayout.SelectionGrid(selectedDirection, DirectionNames, 4);
            if (EditorGUI.EndChangeCheck() && autoApply)
                ApplySelectedDirection();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply Direction Now", GUILayout.Height(28)))
                ApplySelectedDirection();
            if (GUILayout.Button("Release Facing Lock", GUILayout.Height(28)))
                ReleaseFacingLock();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(8);
            if (sync == null)
            {
                EditorGUILayout.HelpBox("Player / OrientationSync bulunamadi. Play'e gir veya Player prefabini sec.", MessageType.Warning);
                return;
            }

            DrawOffsetControls();
            DrawRotationControls();
            DrawSortRuleHint();

            EditorGUILayout.Space(10);
            GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
            if (GUILayout.Button("SOURCE PREFAB KAYDET", GUILayout.Height(32)))
                SaveOffsetsToPrefab();
            GUI.backgroundColor = Color.white;
        }

        private void DrawModeStatus()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    player != null
                        ? "READY: yon butonlari karakteri o yone kilitler."
                        : "PLAY MODE: once Find Player bas.",
                    MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "EDIT MODE: sadece prefab/preview degerleri degisir; karakter otomatik donmez. Canli ayar icin Play'e gir.",
                    MessageType.Warning);
            }
        }

        private void AutoFindTargets()
        {
            sync = ResolveSelectedSync();
            player = sync != null ? sync.GetComponentInParent<PlayerController>() : null;

            if (player == null)
                player = FindFirst<PlayerController>();

            sync = player != null
                ? player.GetComponentInChildren<OrientationSync>(true)
                : sync != null ? sync : FindFirst<OrientationSync>();

            if (sync == null)
            {
                GameObject prefab = LoadFallbackPrefab();
                if (prefab != null)
                    sync = prefab.GetComponentInChildren<OrientationSync>(true);
            }
        }

        private static OrientationSync ResolveSelectedSync()
        {
            if (Selection.activeGameObject == null) return null;
            return Selection.activeGameObject.GetComponentInParent<OrientationSync>()
                ?? Selection.activeGameObject.GetComponentInChildren<OrientationSync>(true);
        }

        private static T FindFirst<T>() where T : Object
        {
            T[] objects = Resources.FindObjectsOfTypeAll<T>();
            T fallback = null;
            for (int i = 0; i < objects.Length; i++)
            {
                T obj = objects[i];
                if (obj == null) continue;
                if (EditorUtility.IsPersistent(obj)) continue;

                if (obj is Component component)
                {
                    if (component.gameObject.hideFlags != HideFlags.None) continue;
                    if (component.gameObject.activeInHierarchy) return obj;
                    fallback ??= obj;
                }
                else
                {
                    fallback ??= obj;
                }
            }

            return fallback;
        }

        private void DrawOffsetControls()
        {
            SerializedObject so = new SerializedObject(sync);
            SerializedProperty offsets = so.FindProperty("handOffsets");
            if (offsets == null || !offsets.isArray || offsets.arraySize < 8)
            {
                EditorGUILayout.HelpBox("handOffsets eksik veya 8 eleman degil.", MessageType.Error);
                return;
            }

            SerializedProperty active = offsets.GetArrayElementAtIndex(selectedDirection);

            EditorGUILayout.LabelField($"Offset ({DirectionNames[selectedDirection]})", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            Vector2 value = EditorGUILayout.Vector2Field("Value", active.vector2Value);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(sync, "Modify Warblade Mount Offset");
                active.vector2Value = value;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(sync);
                ApplySelectedDirection();
            }

            float step = largeStep ? 0.05f : 0.01f;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Left")) NudgeOffset(new Vector2(-step, 0f));
            if (GUILayout.Button("Right")) NudgeOffset(new Vector2(step, 0f));
            if (GUILayout.Button("Up")) NudgeOffset(new Vector2(0f, step));
            if (GUILayout.Button("Down")) NudgeOffset(new Vector2(0f, -step));
            EditorGUILayout.EndHorizontal();
        }

        private void NudgeOffset(Vector2 delta)
        {
            SerializedObject so = new SerializedObject(sync);
            SerializedProperty offsets = so.FindProperty("handOffsets");
            if (offsets == null || offsets.arraySize <= selectedDirection) return;

            Undo.RecordObject(sync, "Nudge Warblade Mount Offset");
            SerializedProperty active = offsets.GetArrayElementAtIndex(selectedDirection);
            active.vector2Value += delta;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(sync);
            ApplySelectedDirection();
        }

        private void DrawRotationControls()
        {
            SerializedObject so = new SerializedObject(sync);
            SerializedProperty rotations = so.FindProperty("weaponRotations");
            if (rotations == null || !rotations.isArray || rotations.arraySize < 8)
            {
                EditorGUILayout.HelpBox("weaponRotations eksik veya 8 eleman degil.", MessageType.Error);
                return;
            }

            SerializedProperty active = rotations.GetArrayElementAtIndex(selectedDirection);

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField($"Rotation ({DirectionNames[selectedDirection]})", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            float value = EditorGUILayout.FloatField("Degrees", active.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(sync, "Modify Warblade Mount Rotation");
                active.floatValue = value;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(sync);
                ApplySelectedDirection();
            }

            float step = largeStep ? 15f : 5f;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"-{step:0}")) NudgeRotation(-step);
            if (GUILayout.Button($"+{step:0}")) NudgeRotation(step);
            EditorGUILayout.EndHorizontal();
        }

        private void NudgeRotation(float delta)
        {
            SerializedObject so = new SerializedObject(sync);
            SerializedProperty rotations = so.FindProperty("weaponRotations");
            if (rotations == null || rotations.arraySize <= selectedDirection) return;

            Undo.RecordObject(sync, "Nudge Warblade Mount Rotation");
            SerializedProperty active = rotations.GetArrayElementAtIndex(selectedDirection);
            active.floatValue += delta;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(sync);
            ApplySelectedDirection();
        }

        private void DrawSortRuleHint()
        {
            bool behindBody = selectedDirection == (int)FacingDir8.N
                || selectedDirection == (int)FacingDir8.NE
                || selectedDirection == (int)FacingDir8.NW
                || selectedDirection == (int)FacingDir8.W;

            EditorGUILayout.HelpBox(
                $"{DirectionNames[selectedDirection]}: weapon {(behindBody ? "BEHIND body" : "IN FRONT of body")}.",
                MessageType.None);
        }

        private void ApplySelectedDirection()
        {
            if (sync == null) AutoFindTargets();
            if (sync == null) return;

            FacingDir8 dir = (FacingDir8)selectedDirection;
            if (Application.isPlaying && player != null)
                player.FaceCombatDirection(ToVector(dir), FacingLockSeconds);

            sync.Sync(dir);
            EditorUtility.SetDirty(sync);
            SceneView.RepaintAll();
            Repaint();
        }

        private void ReleaseFacingLock()
        {
            if (Application.isPlaying && player != null)
                player.FaceCombatDirection(ToVector((FacingDir8)selectedDirection), 0f);
        }

        private void SaveOffsetsToPrefab()
        {
            if (sync == null)
            {
                EditorUtility.DisplayDialog("Kayit basarisiz", "OrientationSync bulunamadi.", "Tamam");
                return;
            }

            if (!TryResolveSaveTarget(sync, out GameObject prefabGo, out OrientationSync prefabSync, out string path))
            {
                EditorUtility.DisplayDialog("Kayit basarisiz", "Source prefab / OrientationSync bulunamadi.", "Tamam");
                return;
            }

            if (prefabSync == sync && !EditorUtility.IsPersistent(prefabSync))
                return;

            SerializedObject src = new SerializedObject(sync);
            SerializedObject dst = new SerializedObject(prefabSync);
            CopyArray(src, dst, "handOffsets");
            CopyArray(src, dst, "weaponRotations");
            dst.ApplyModifiedPropertiesWithoutUndo();

            EditorUtility.SetDirty(prefabSync);
            PrefabUtility.SavePrefabAsset(prefabGo);
            AssetDatabase.SaveAssets();
            Debug.Log($"[WarbladeMountTuner] handOffsets + weaponRotations -> {path} kaydedildi.");
        }

        private static string ResolveSavePath(OrientationSync source)
        {
            if (source == null) return null;
            if (TryResolveSaveTarget(source, out _, out _, out string path)) return path;
            return null;
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
                prefabSync = prefabGo != null ? prefabGo.GetComponentInChildren<OrientationSync>(true) : sourcePrefabSync;
                return prefabSync != null && prefabGo != null;
            }

            if (source != null && EditorUtility.IsPersistent(source))
            {
                path = AssetDatabase.GetAssetPath(source);
                prefabGo = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                prefabSync = source;
                return prefabGo != null;
            }

            prefabGo = LoadFallbackPrefab();
            if (prefabGo == null) return false;
            path = AssetDatabase.GetAssetPath(prefabGo);
            prefabSync = prefabGo.GetComponentInChildren<OrientationSync>(true);
            return prefabSync != null;
        }

        private static GameObject LoadFallbackPrefab()
        {
            for (int i = 0; i < FallbackPrefabPaths.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FallbackPrefabPaths[i]);
                if (prefab != null && prefab.GetComponentInChildren<OrientationSync>(true) != null)
                    return prefab;
            }

            return null;
        }

        private static void CopyArray(SerializedObject src, SerializedObject dst, string propName)
        {
            SerializedProperty s = src.FindProperty(propName);
            SerializedProperty d = dst.FindProperty(propName);
            if (s == null || d == null || !s.isArray) return;

            d.arraySize = s.arraySize;
            for (int i = 0; i < s.arraySize; i++)
            {
                SerializedProperty se = s.GetArrayElementAtIndex(i);
                SerializedProperty de = d.GetArrayElementAtIndex(i);
                if (se.propertyType == SerializedPropertyType.Vector2)
                    de.vector2Value = se.vector2Value;
                else if (se.propertyType == SerializedPropertyType.Float)
                    de.floatValue = se.floatValue;
            }
        }

        private static Vector2 ToVector(FacingDir8 dir)
        {
            switch (dir)
            {
                case FacingDir8.S: return Vector2.down;
                case FacingDir8.SE: return new Vector2(1f, -1f).normalized;
                case FacingDir8.E: return Vector2.right;
                case FacingDir8.NE: return new Vector2(1f, 1f).normalized;
                case FacingDir8.N: return Vector2.up;
                case FacingDir8.NW: return new Vector2(-1f, 1f).normalized;
                case FacingDir8.W: return Vector2.left;
                case FacingDir8.SW: return new Vector2(-1f, -1f).normalized;
                default: return Vector2.down;
            }
        }
    }
}
