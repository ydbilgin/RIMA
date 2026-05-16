#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Props.Editor
{
    public sealed class PropsTab
    {
        private const string PropsRoot = "Assets/Data/Brush/Props";
        private readonly EditorWindow owner;
        private readonly PropPlacer placer = new PropPlacer();
        private readonly List<PropDefinitionSO> props = new List<PropDefinitionSO>();
        private Vector2 leftScroll;
        private Vector2 rightScroll;
        private double lastRefreshTime;

        public PropsTab(EditorWindow owner)
        {
            this.owner = owner;
        }

        public bool IsActive
        {
            get => placer.IsActive;
            set => placer.IsActive = value;
        }

        public PropDefinitionSO SelectedProp { get; private set; }
        public RoomTemplateSO TargetTemplate { get; private set; }
        public PropFootprintValidator.ValidationResult LastValidationResult => placer.LastValidationResult;
        public string LastFailureDetail => placer.LastFailureDetail;

        public void Draw()
        {
            RefreshPropListIfNeeded();

            TargetTemplate = (RoomTemplateSO)EditorGUILayout.ObjectField("Target Room Template", TargetTemplate, typeof(RoomTemplateSO), false);

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawLeftPanel();
                DrawCenterPanel();
                DrawRightPanel();
            }

            DrawBottomPanel();
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (!IsActive) return;
            if (SelectedProp == null || TargetTemplate == null) return;

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            Event e = Event.current;
            Vector2Int tile = WorldToTile(e);
            CompositionRoleMap roleMap = CompositionRoleMapGenerator.GenerateFromRoom(TargetTemplate);
            placer.OnHover(tile, SelectedProp, TargetTemplate, roleMap);
            placer.DrawPreview(
                tile,
                SelectedProp,
                placer.LastValidationResult == PropFootprintValidator.ValidationResult.Valid,
                new Color(0.2f, 0.9f, 0.35f, 1f),
                new Color(1f, 0.2f, 0.2f, 1f));

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                placer.OnSceneClick(tile, SelectedProp, TargetTemplate, roleMap);
                owner?.Repaint();
                e.Use();
            }

            if (e.type == EventType.Repaint)
            {
                SceneView.RepaintAll();
            }
        }

        private void DrawLeftPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(200f)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Props", EditorStyles.boldLabel);
                    if (GUILayout.Button("Refresh", GUILayout.Width(70f)))
                    {
                        RefreshPropList(force: true);
                    }
                }

                leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
                foreach (PropDefinitionSO prop in props)
                {
                    if (prop == null) continue;
                    GUIStyle style = SelectedProp == prop ? EditorStyles.helpBox : GUIStyle.none;
                    using (new EditorGUILayout.HorizontalScope(style, GUILayout.MinHeight(40f)))
                    {
                        Texture icon = prop.icon != null ? prop.icon.texture : Texture2D.grayTexture;
                        GUILayout.Label(icon, GUILayout.Width(32f), GUILayout.Height(32f));
                        using (new EditorGUILayout.VerticalScope())
                        {
                            string title = string.IsNullOrEmpty(prop.displayName) ? prop.name : prop.displayName;
                            GUILayout.Label(title, EditorStyles.boldLabel);
                            GUILayout.Label(prop.propId ?? string.Empty, EditorStyles.miniLabel);
                        }
                        if (GUILayout.Button("Select", GUILayout.Width(52f)))
                        {
                            SelectedProp = prop;
                            owner?.Repaint();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawCenterPanel()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
                SelectedProp = (PropDefinitionSO)EditorGUILayout.ObjectField("Selected", SelectedProp, typeof(PropDefinitionSO), false);

                if (SelectedProp == null)
                {
                    EditorGUILayout.HelpBox("Select a prop definition.", MessageType.Info);
                    return;
                }

                Rect previewRect = GUILayoutUtility.GetRect(160f, 160f, GUILayout.ExpandWidth(true));
                GUI.Box(previewRect, GUIContent.none);
                if (SelectedProp.worldSprite != null)
                {
                    Texture texture = SelectedProp.worldSprite.texture;
                    GUI.DrawTexture(previewRect, texture, ScaleMode.ScaleToFit, true);
                }
                else
                {
                    GUI.Label(previewRect, "No world sprite", EditorStyles.centeredGreyMiniLabel);
                }

                EditorGUILayout.LabelField("Footprint", $"{SelectedProp.footprintSize.x} x {SelectedProp.footprintSize.y}");
                EditorGUILayout.LabelField("Sprite Anchor", SelectedProp.spriteAnchor.ToString());
                EditorGUILayout.LabelField("Sorting", SelectedProp.sortingMode.ToString());
            }
        }

        private void DrawRightPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(180f)))
            {
                EditorGUILayout.LabelField("Placement Rules", EditorStyles.boldLabel);
                rightScroll = EditorGUILayout.BeginScrollView(rightScroll);
                if (SelectedProp != null)
                {
                    EditorGUILayout.Toggle("Blocks Walkable", SelectedProp.blocksWalkable);
                    EditorGUILayout.Toggle("Requires Walkable", SelectedProp.requiresWalkableTile);
                    EditorGUILayout.Toggle("Respects Mask", SelectedProp.respectsWalkableMask);
                    EditorGUILayout.FloatField("Min Distance", SelectedProp.distanceFromOtherProps);
                    EditorGUILayout.LabelField("Preferred", RolesToString(SelectedProp.preferredRoles), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("Forbidden", RolesToString(SelectedProp.forbiddenRoles), EditorStyles.wordWrappedLabel);
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawBottomPanel()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
                if (SelectedProp == null || TargetTemplate == null)
                {
                    EditorGUILayout.HelpBox("Assign a target template and selected prop before placing.", MessageType.Info);
                    return;
                }

                MessageType messageType = placer.LastValidationResult == PropFootprintValidator.ValidationResult.Valid
                    ? MessageType.Info
                    : MessageType.Warning;
                string detail = string.IsNullOrEmpty(placer.LastFailureDetail) ? "Ready." : placer.LastFailureDetail;
                EditorGUILayout.HelpBox($"{placer.LastValidationResult}: {detail}", messageType);
            }
        }

        private void RefreshPropListIfNeeded()
        {
            if (EditorApplication.timeSinceStartup - lastRefreshTime > 3.0)
            {
                RefreshPropList(force: false);
            }
        }

        private void RefreshPropList(bool force)
        {
            lastRefreshTime = EditorApplication.timeSinceStartup;
            props.Clear();

            if (!AssetDatabase.IsValidFolder(PropsRoot))
            {
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:PropDefinitionSO", new[] { PropsRoot });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                PropDefinitionSO prop = AssetDatabase.LoadAssetAtPath<PropDefinitionSO>(path);
                if (prop != null && (force || !props.Contains(prop)))
                {
                    props.Add(prop);
                }
            }
        }

        private static Vector2Int WorldToTile(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            return new Vector2Int(Mathf.FloorToInt(ray.origin.x), Mathf.FloorToInt(ray.origin.y));
        }

        private static string RolesToString(CompositionRole[] roles)
        {
            if (roles == null || roles.Length == 0) return "(none)";
            return string.Join(", ", roles);
        }
    }
}
#endif
