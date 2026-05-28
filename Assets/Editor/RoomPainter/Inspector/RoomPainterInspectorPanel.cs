using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RoomPainterInspectorPanel
    {
        private const string FoldoutPrefix = "RIMA.RoomPainter.Foldout.";
        private const float HeroPreviewSize = 88f;
        private const float SectionBandHeight = 22f;

        private static readonly SectionDescriptor[] Sections =
        {
            new SectionDescriptor("Identity", new Color(0.18f, 0.58f, 0.68f, 1f), SectionKey.Identity),
            new SectionDescriptor("Placement", new Color(0.30f, 0.62f, 0.32f, 1f), SectionKey.Placement),
            new SectionDescriptor("Physics", new Color(0.78f, 0.42f, 0.18f, 1f), SectionKey.Physics),
            new SectionDescriptor("Parallax", new Color(0.55f, 0.40f, 0.78f, 1f), SectionKey.Parallax),
            new SectionDescriptor("Visual", new Color(0.82f, 0.42f, 0.62f, 1f), SectionKey.Visual),
            new SectionDescriptor("Metadata", new Color(0.42f, 0.42f, 0.46f, 1f), SectionKey.Metadata),
        };

        private static readonly GUIContent EditingSoContent = new GUIContent("Editing SO");
        private static readonly GUIContent EditingInstanceContent = new GUIContent("Editing instance source");
        private static readonly GUIContent SceneSelectionContent = new GUIContent("Scene selection available");
        private static readonly GUIContent FuturePlacementsContent = new GUIContent("Affects future placements");

        private readonly Dictionary<string, bool> _foldouts = new Dictionary<string, bool>();
        private readonly Dictionary<RoomPainterAsset, Vector2> _scrollPositions = new Dictionary<RoomPainterAsset, Vector2>();
        private GUIStyle _soBannerStyle;
        private GUIStyle _instanceBannerStyle;
        private GUIStyle _sectionHeaderStyle;
        private GUIStyle _layerBadgeStyle;
        private GUIStyle _heroNameStyle;

        public RoomPainterInspectorPanel()
        {
            LoadFoldoutState();
        }

        public void Draw(RoomPainterAsset asset, GameObject sceneInstance)
        {
            Draw(asset, sceneInstance, RoomPainterMode.Tile);
        }

        // D3: overload accepting current mode for mode-specific section
        public void Draw(RoomPainterAsset asset, GameObject sceneInstance, RoomPainterMode mode)
        {
            RoomPainterAsset activeAsset = asset != null ? asset : ResolveAssetFromSceneInstance(sceneInstance);

            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
            {
                if (activeAsset == null)
                {
                    EditorGUILayout.LabelField("Inspector", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox("Select an asset in the palette or a painted object in the scene to author.", MessageType.Info);
                    GUILayout.FlexibleSpace();
                    return;
                }

                DrawModeBanner(asset, sceneInstance, activeAsset);
                DrawHeroCard(activeAsset);
                RoomPainterInstanceList.Draw(activeAsset);

                bool assetChanged = false;
                Vector2 scroll = GetScrollPosition(activeAsset);
                scroll = EditorGUILayout.BeginScrollView(scroll, false, true);

                // D3: Mode-specific section at the top of the inspector scroll area
                DrawModeSpecificSection(mode, activeAsset, sceneInstance);

                for (int i = 0; i < Sections.Length; i++)
                {
                    SectionDescriptor descriptor = Sections[i];
                    int captureIndex = i;
                    DrawSection(descriptor, () =>
                    {
                        assetChanged |= DrawSectionChanged(() => DrawSectionBody(Sections[captureIndex].Key, activeAsset, sceneInstance));
                    });
                }

                EditorGUILayout.EndScrollView();
                _scrollPositions[activeAsset] = scroll;

                if (assetChanged)
                {
                    Undo.RecordObject(activeAsset, "Edit Room Painter Asset");
                    EditorUtility.SetDirty(activeAsset);
                    AssetDatabase.SaveAssetIfDirty(activeAsset);
                    SceneView.RepaintAll();
                }
            }
        }

        private void LoadFoldoutState()
        {
            for (int i = 0; i < Sections.Length; i++)
            {
                _foldouts[Sections[i].Title] = EditorPrefs.GetBool(FoldoutPrefix + Sections[i].Title, true);
            }
        }

        // D3: Mode-specific section shown at the top of the inspector scroll area
        private void DrawModeSpecificSection(RoomPainterMode mode, RoomPainterAsset asset, GameObject sceneInstance)
        {
            string title;
            Color accentColor;
            switch (mode)
            {
                case RoomPainterMode.Tile:
                    title = "Tile Mode";
                    accentColor = new Color(0.30f, 0.55f, 1.00f, 1f);
                    break;
                case RoomPainterMode.Cliff:
                    title = "Cliff Mode";
                    accentColor = new Color(0.65f, 0.65f, 0.65f, 1f);
                    break;
                case RoomPainterMode.Decor:
                    title = "Decor Mode";
                    accentColor = new Color(0.35f, 0.80f, 0.45f, 1f);
                    break;
                case RoomPainterMode.Object:
                    title = "Object Mode";
                    accentColor = new Color(0.95f, 0.78f, 0.20f, 1f);
                    break;
                default:
                    return;
            }

            bool expanded = GetFoldout(title);
            const float bandHeight = 22f;
            Rect headerRect = GUILayoutUtility.GetRect(0f, bandHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(headerRect, accentColor * new Color(0.7f, 0.7f, 0.7f, 1f));
            Rect accentBar = new Rect(headerRect.x, headerRect.y, 4f, headerRect.height);
            EditorGUI.DrawRect(accentBar, accentColor);
            Rect arrowRect = new Rect(headerRect.x + 10f, headerRect.y + 4f, 14f, headerRect.height - 6f);
            Rect labelRect = new Rect(arrowRect.xMax + 2f, headerRect.y + 2f, headerRect.width - arrowRect.xMax - 6f, headerRect.height);
            GUI.Label(arrowRect, expanded ? "▼" : "►", GetSectionHeaderStyle());
            GUI.Label(labelRect, title, GetSectionHeaderStyle());

            Event evt = Event.current;
            if (evt.type == EventType.MouseDown && evt.button == 0 && headerRect.Contains(evt.mousePosition))
            {
                bool next = !expanded;
                _foldouts[title] = next;
                EditorPrefs.SetBool(FoldoutPrefix + title, next);
                evt.Use();
                expanded = next;
            }

            if (!expanded)
            {
                GUILayout.Space(2f);
                return;
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                switch (mode)
                {
                    case RoomPainterMode.Tile:
                        DrawTileModeBody(asset);
                        break;
                    case RoomPainterMode.Cliff:
                        DrawCliffModeBody(asset);
                        break;
                    case RoomPainterMode.Decor:
                        DrawDecorModeBody(asset);
                        break;
                    case RoomPainterMode.Object:
                        DrawObjectModeBody(asset, sceneInstance);
                        break;
                }
            }

            GUILayout.Space(4f);
        }

        private static void DrawTileModeBody(RoomPainterAsset asset)
        {
            EditorGUILayout.LabelField("Floor / Tile", EditorStyles.boldLabel);
            if (asset == null)
            {
                EditorGUILayout.HelpBox("Select a Floor tile from the palette.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField("Layer", asset.defaultLayer.ToString());
        }

        // D5: Cliff inspector sub-section — variant dropdown + manual cell counts + buttons
        private static int _cliffVariantIndex;
        private static readonly string[] CliffVariantLabels =
        {
            "Variant 0 (cliff_S)", "Variant 1 (new1)", "Variant 2 (new2)",
            "Variant 3 (new3)", "Variant 4 (new4)"
        };

        private static void DrawCliffModeBody(RoomPainterAsset asset)
        {
            EditorGUILayout.LabelField("Cliff / Wall Blocks", EditorStyles.boldLabel);
            if (asset == null)
            {
                EditorGUILayout.HelpBox("Select a Cliff asset from the palette.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.LabelField("Layer", asset.defaultLayer.ToString());
            }

            EditorGUILayout.Space(4f);

            // --- Active Cliff Variant dropdown ---
            // Loads spritesS thumbnails from DirectionalCliffTile_Hades asset
            RIMA.Environment.DirectionalCliffTile hadesTile =
                UnityEditor.AssetDatabase.LoadAssetAtPath<RIMA.Environment.DirectionalCliffTile>(
                    "Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset");

            int variantCount = hadesTile != null && hadesTile.spritesS != null ? hadesTile.spritesS.Length : 0;
            if (variantCount > 0)
            {
                EditorGUILayout.LabelField("Active Cliff Variant (South)", EditorStyles.boldLabel);

                // Build label array from actual sprite names (fallback to index label)
                string[] labels = new string[variantCount];
                for (int i = 0; i < variantCount; i++)
                {
                    labels[i] = hadesTile.spritesS[i] != null ? i + ": " + hadesTile.spritesS[i].name : i + ": (null)";
                }

                _cliffVariantIndex = Mathf.Clamp(_cliffVariantIndex, 0, variantCount - 1);
                _cliffVariantIndex = EditorGUILayout.Popup(_cliffVariantIndex, labels);

                // Preview thumbnail
                if (hadesTile.spritesS[_cliffVariantIndex] != null)
                {
                    Texture2D thumb = UnityEditor.AssetPreview.GetAssetPreview(hadesTile.spritesS[_cliffVariantIndex]);
                    if (thumb != null)
                    {
                        Rect thumbRect = GUILayoutUtility.GetRect(48f, 48f, GUILayout.Width(48f), GUILayout.Height(48f));
                        EditorGUI.DrawRect(thumbRect, new Color(0.10f, 0.10f, 0.12f, 1f));
                        GUI.DrawTexture(thumbRect, thumb, ScaleMode.ScaleToFit);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("DirectionalCliffTile_Hades not found or spritesS[] empty.", MessageType.Warning);
            }

            EditorGUILayout.Space(4f);

            // --- Manual cell stats ---
            RIMA.Environment.CliffAutoPlacer placer =
                UnityEngine.Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();

            if (placer != null)
            {
                EditorGUILayout.LabelField("Manual painted: " + placer.ManualPaintedCells.Count, EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Manual erased: " + placer.ManualOverrideCells.Count, EditorStyles.miniLabel);

                EditorGUILayout.Space(4f);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Clear Manual Painted", EditorStyles.miniButton))
                    {
                        if (UnityEditor.EditorUtility.DisplayDialog(
                            "Clear Manual Painted",
                            "Remove all manually painted cliff cells from the whitelist? This cannot be undone.",
                            "Clear", "Cancel"))
                        {
                            placer.ClearManualPainted();
                        }
                    }

                    if (GUILayout.Button("Clear Manual Override", EditorStyles.miniButton))
                    {
                        if (UnityEditor.EditorUtility.DisplayDialog(
                            "Clear Manual Override",
                            "Remove all manually erased (blacklisted) cliff cells? This cannot be undone.",
                            "Clear", "Cancel"))
                        {
                            placer.ClearManualOverrides();
                        }
                    }
                }

                if (GUILayout.Button("Regenerate (C)", EditorStyles.miniButton))
                {
                    placer.Regenerate();
                    UnityEngine.Debug.Log("[RoomPainter] Cliff regenerated: " + placer.LastGeneratedCount + " tiles.");
                }

                // D5.5 disclaimer: Regenerate only affects algorithmic cliff
                EditorGUILayout.HelpBox("Regenerate / Clear buttons affect Algorithmic cliff only. Shift+Click in SceneView paints Decor cliff.", MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox("No CliffAutoPlacer found in scene.", MessageType.Info);
            }

            // D5.5: Decor Cliff sub-section
            DrawDecorCliffSection();
        }

        // D5.5: Decor Cliff free-form painter controls
        private static void DrawDecorCliffSection()
        {
            EditorGUILayout.Space(6f);

            Color prevBg = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.12f, 0.55f, 0.75f, 1f);
            EditorGUILayout.LabelField("Decor Cliff  (Free-form)", EditorStyles.boldLabel);
            GUI.backgroundColor = prevBg;

            // Instructions
            EditorGUILayout.HelpBox("Shift+Click in SceneView → paint free-form cliff (no floor check).\nShift+Alt+Click → erase from DecorCliffTilemap.", MessageType.Info);

            // Tile count
            var decorGo = UnityEngine.GameObject.Find("DecorCliffTilemap");
            UnityEngine.Tilemaps.Tilemap decorTilemap = decorGo != null ? decorGo.GetComponent<UnityEngine.Tilemaps.Tilemap>() : null;

            if (decorTilemap != null)
            {
                int decorCount = decorTilemap.GetUsedTilesCount();
                EditorGUILayout.LabelField("Decor cliff tile count: " + decorCount, EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Session painted: " + DecorCliffPainter.DecorPaintedThisSession, EditorStyles.miniLabel);

                if (GUILayout.Button("Clear Decor Cliff", EditorStyles.miniButton))
                {
                    if (UnityEditor.EditorUtility.DisplayDialog(
                        "Clear Decor Cliff",
                        "Clear all tiles from DecorCliffTilemap? This cannot be undone.",
                        "Clear", "Cancel"))
                    {
                        decorTilemap.ClearAllTiles();
                        EditorUtility.SetDirty(decorTilemap.gameObject);
                        DecorCliffPainter.ResetSessionCounter();
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("DecorCliffTilemap not found in scene.", MessageType.Warning);

                // One-click creation helper
                if (GUILayout.Button("Create DecorCliffTilemap", EditorStyles.miniButton))
                {
                    CreateDecorCliffTilemap();
                }
            }
        }

        // D5.5: Create DecorCliffTilemap under the scene's Grid root
        private static void CreateDecorCliffTilemap()
        {
            // Find Grid root (Floor GameObject contains the Grid component)
            var floorGo = UnityEngine.GameObject.Find("Floor");
            UnityEngine.Transform parent = floorGo != null ? floorGo.transform : null;

            var decorGo = new UnityEngine.GameObject("DecorCliffTilemap");
            if (parent != null)
                decorGo.transform.SetParent(parent, false);

            decorGo.AddComponent<UnityEngine.Tilemaps.Tilemap>();
            var rend = decorGo.AddComponent<UnityEngine.Tilemaps.TilemapRenderer>();
            rend.sortingLayerName = "Decor_Cliff";
            rend.sortingOrder = 50;

            // Copy material from existing CliffTilemap if available
            var cliffGo = UnityEngine.GameObject.Find("CliffTilemap");
            if (cliffGo != null)
            {
                var srcRend = cliffGo.GetComponent<UnityEngine.Tilemaps.TilemapRenderer>();
                if (srcRend != null && srcRend.sharedMaterial != null)
                    rend.sharedMaterial = srcRend.sharedMaterial;
            }

            UnityEditor.EditorUtility.SetDirty(decorGo);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(decorGo.scene);
            UnityEngine.Debug.Log("[RoomPainter] DecorCliffTilemap created under " + (parent != null ? parent.name : "scene root") + ".");
        }

        private static void DrawDecorModeBody(RoomPainterAsset asset)
        {
            EditorGUILayout.LabelField("Decor (Walkable / Wall)", EditorStyles.boldLabel);
            if (asset == null)
            {
                EditorGUILayout.HelpBox("Select a Decor asset from the palette.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField("Layer", asset.defaultLayer.ToString());
        }

        private static void DrawObjectModeBody(RoomPainterAsset asset, GameObject sceneInstance)
        {
            EditorGUILayout.LabelField("Gameplay Object", EditorStyles.boldLabel);
            if (asset == null)
            {
                EditorGUILayout.HelpBox("Select a gameplay object (chest, fragment, gate…) from the palette.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField("Layer", asset.defaultLayer.ToString());

            if (sceneInstance != null)
            {
                Collider2D col = sceneInstance.GetComponentInChildren<Collider2D>();
                if (col != null)
                {
                    EditorGUILayout.LabelField("Trigger", col.isTrigger ? "Yes" : "No");
                }
            }
        }

        private void DrawModeBanner(RoomPainterAsset paletteAsset, GameObject sceneInstance, RoomPainterAsset activeAsset)
        {
            bool hasSceneBinding = sceneInstance != null && sceneInstance.GetComponent<RoomPainterAssetBinding>() != null;
            bool editingSo = paletteAsset != null;
            GUIStyle style = editingSo ? GetSoBannerStyle() : GetInstanceBannerStyle();

            using (new EditorGUILayout.VerticalScope(style))
            {
                EditorGUILayout.LabelField(editingSo ? EditingSoContent : EditingInstanceContent, EditorStyles.boldLabel);

                if (editingSo && hasSceneBinding)
                {
                    EditorGUILayout.LabelField(SceneSelectionContent, EditorStyles.miniLabel);
                }
                else if (editingSo)
                {
                    EditorGUILayout.LabelField(FuturePlacementsContent, EditorStyles.miniLabel);
                }
            }
        }

        private void DrawHeroCard(RoomPainterAsset asset)
        {
            Object source = asset.sprite != null ? (Object)asset.sprite : asset.prefab;
            Texture2D preview = source != null ? AssetPreview.GetAssetPreview(source) : null;
            if (preview == null && source != null)
            {
                preview = AssetPreview.GetMiniThumbnail(source) as Texture2D;
            }

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                Rect previewRect = GUILayoutUtility.GetRect(HeroPreviewSize, HeroPreviewSize, GUILayout.Width(HeroPreviewSize), GUILayout.Height(HeroPreviewSize));
                EditorGUI.DrawRect(previewRect, new Color(0.10f, 0.10f, 0.12f, 1f));
                if (preview != null)
                {
                    GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
                }

                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
                {
                    GUILayout.Space(2f);
                    GUI.Label(GUILayoutUtility.GetRect(0f, 22f, GUILayout.ExpandWidth(true)), DisplayName(asset), GetHeroNameStyle());

                    DrawLayerBadge(asset.defaultLayer);

                    string dims = ResolveDimensions(asset);
                    GUILayout.Label(dims, EditorStyles.miniLabel);

                    string path = asset.sprite != null
                        ? AssetDatabase.GetAssetPath(asset.sprite)
                        : (asset.prefab != null ? AssetDatabase.GetAssetPath(asset.prefab) : string.Empty);
                    if (!string.IsNullOrEmpty(path))
                    {
                        GUILayout.Label(System.IO.Path.GetFileName(path), EditorStyles.miniLabel);
                    }
                }
            }

            GUILayout.Space(4f);
        }

        private void DrawLayerBadge(RoomLayer layer)
        {
            Color badge = ColorForLayer(layer);
            GUIStyle style = GetLayerBadgeStyle();

            Rect rect = GUILayoutUtility.GetRect(new GUIContent(layer.ToString()), style, GUILayout.Width(86f), GUILayout.Height(18f));
            Color prevBg = GUI.backgroundColor;
            GUI.backgroundColor = badge;
            GUI.Label(rect, layer.ToString(), style);
            GUI.backgroundColor = prevBg;
        }

        private void DrawSection(SectionDescriptor descriptor, System.Action drawBody)
        {
            bool expanded = GetFoldout(descriptor.Title);

            Rect headerRect = GUILayoutUtility.GetRect(0f, SectionBandHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(headerRect, descriptor.AccentColor * new Color(0.7f, 0.7f, 0.7f, 1f));

            Rect accentBar = new Rect(headerRect.x, headerRect.y, 4f, headerRect.height);
            EditorGUI.DrawRect(accentBar, descriptor.AccentColor);

            Rect arrowRect = new Rect(headerRect.x + 10f, headerRect.y + 4f, 14f, headerRect.height - 6f);
            Rect labelRect = new Rect(arrowRect.xMax + 2f, headerRect.y + 2f, headerRect.width - arrowRect.xMax - 6f, headerRect.height);

            GUIStyle headerStyle = GetSectionHeaderStyle();
            GUI.Label(arrowRect, expanded ? "▼" : "►", headerStyle);
            GUI.Label(labelRect, descriptor.Title, headerStyle);

            Event evt = Event.current;
            if (evt.type == EventType.MouseDown && evt.button == 0 && headerRect.Contains(evt.mousePosition))
            {
                bool next = !expanded;
                _foldouts[descriptor.Title] = next;
                EditorPrefs.SetBool(FoldoutPrefix + descriptor.Title, next);
                evt.Use();
                expanded = next;
            }

            if (!expanded)
            {
                GUILayout.Space(2f);
                return;
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                drawBody();
            }

            GUILayout.Space(2f);
        }

        private void DrawSectionBody(SectionKey key, RoomPainterAsset asset, GameObject sceneInstance)
        {
            switch (key)
            {
                case SectionKey.Identity:
                    IdentitySection.Draw(asset);
                    break;
                case SectionKey.Placement:
                    PlacementSection.Draw(asset);
                    break;
                case SectionKey.Physics:
                    PhysicsSection.Draw(asset, sceneInstance);
                    break;
                case SectionKey.Parallax:
                    ParallaxSection.Draw(asset);
                    break;
                case SectionKey.Visual:
                    VisualSection.Draw(asset, sceneInstance);
                    break;
                case SectionKey.Metadata:
                    MetadataSection.Draw(asset);
                    break;
            }
        }

        private bool DrawSectionChanged(System.Action draw)
        {
            EditorGUI.BeginChangeCheck();
            draw();
            return EditorGUI.EndChangeCheck();
        }

        private bool GetFoldout(string label)
        {
            if (_foldouts.TryGetValue(label, out bool expanded))
            {
                return expanded;
            }

            expanded = EditorPrefs.GetBool(FoldoutPrefix + label, true);
            _foldouts[label] = expanded;
            return expanded;
        }

        private Vector2 GetScrollPosition(RoomPainterAsset activeAsset)
        {
            if (activeAsset != null && _scrollPositions.TryGetValue(activeAsset, out Vector2 scrollPosition))
            {
                return scrollPosition;
            }

            return Vector2.zero;
        }

        private GUIStyle GetSoBannerStyle()
        {
            if (_soBannerStyle == null)
            {
                _soBannerStyle = CreateBannerStyle(new Color(0.05f, 0.20f, 0.24f, 1f));
            }

            return _soBannerStyle;
        }

        private GUIStyle GetInstanceBannerStyle()
        {
            if (_instanceBannerStyle == null)
            {
                _instanceBannerStyle = CreateBannerStyle(new Color(0.28f, 0.16f, 0.04f, 1f));
            }

            return _instanceBannerStyle;
        }

        private GUIStyle GetSectionHeaderStyle()
        {
            if (_sectionHeaderStyle == null)
            {
                _sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    normal = { textColor = Color.white }
                };
            }

            return _sectionHeaderStyle;
        }

        private GUIStyle GetLayerBadgeStyle()
        {
            if (_layerBadgeStyle == null)
            {
                _layerBadgeStyle = new GUIStyle(EditorStyles.miniButton)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 10,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white }
                };
            }

            return _layerBadgeStyle;
        }

        private GUIStyle GetHeroNameStyle()
        {
            if (_heroNameStyle == null)
            {
                _heroNameStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 13,
                    alignment = TextAnchor.UpperLeft
                };
            }

            return _heroNameStyle;
        }

        private static GUIStyle CreateBannerStyle(Color backgroundColor)
        {
            Texture2D texture = new Texture2D(1, 1)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            texture.SetPixel(0, 0, backgroundColor);
            texture.Apply();

            return new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(8, 8, 6, 6),
                normal =
                {
                    background = texture,
                    textColor = Color.white
                }
            };
        }

        private static RoomPainterAsset ResolveAssetFromSceneInstance(GameObject sceneInstance)
        {
            if (sceneInstance == null)
            {
                return null;
            }

            RoomPainterAssetBinding binding = sceneInstance.GetComponent<RoomPainterAssetBinding>();
            if (binding == null || string.IsNullOrEmpty(binding.assetGuid))
            {
                return null;
            }

            string metadataPath = RoomPainterAssetPostprocessor.MetadataRoot + "/" + binding.assetGuid + ".asset";
            return AssetDatabase.LoadAssetAtPath<RoomPainterAsset>(metadataPath);
        }

        private static string DisplayName(RoomPainterAsset asset)
        {
            if (asset == null)
            {
                return "None";
            }

            if (!string.IsNullOrEmpty(asset.displayName))
            {
                return asset.displayName;
            }

            if (asset.sprite != null)
            {
                return asset.sprite.name;
            }

            return asset.prefab != null ? asset.prefab.name : asset.name;
        }

        private static string ResolveDimensions(RoomPainterAsset asset)
        {
            if (asset.sprite != null)
            {
                Rect rect = asset.sprite.rect;
                return Mathf.RoundToInt(rect.width) + " x " + Mathf.RoundToInt(rect.height) + " px";
            }

            if (asset.prefab != null)
            {
                return "Prefab";
            }

            return "—";
        }

        private static Color ColorForLayer(RoomLayer layer)
        {
            switch (layer)
            {
                case RoomLayer.Floor: return new Color(0.30f, 0.62f, 0.32f, 1f);
                case RoomLayer.Cliff: return new Color(0.18f, 0.58f, 0.68f, 1f);
                case RoomLayer.Wall: return new Color(0.40f, 0.40f, 0.44f, 1f);
                case RoomLayer.Edge: return new Color(0.55f, 0.55f, 0.40f, 1f);
                case RoomLayer.Decals: return new Color(0.60f, 0.46f, 0.30f, 1f);
                case RoomLayer.Props: return new Color(0.78f, 0.42f, 0.18f, 1f);
                case RoomLayer.Lighting: return new Color(0.92f, 0.78f, 0.20f, 1f);
                case RoomLayer.Parallax: return new Color(0.55f, 0.40f, 0.78f, 1f);
                case RoomLayer.Occlusion: return new Color(0.40f, 0.40f, 0.55f, 1f);
                case RoomLayer.Collision: return new Color(0.82f, 0.30f, 0.30f, 1f);
                default: return new Color(0.55f, 0.55f, 0.60f, 1f);
            }
        }

        private enum SectionKey
        {
            Identity,
            Placement,
            Physics,
            Parallax,
            Visual,
            Metadata
        }

        private readonly struct SectionDescriptor
        {
            public readonly string Title;
            public readonly Color AccentColor;
            public readonly SectionKey Key;

            public SectionDescriptor(string title, Color accentColor, SectionKey key)
            {
                Title = title;
                AccentColor = accentColor;
                Key = key;
            }
        }
    }
}
