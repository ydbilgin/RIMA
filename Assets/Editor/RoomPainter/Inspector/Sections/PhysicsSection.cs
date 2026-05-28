using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class PhysicsSection
    {
        private const float HitboxPreviewSize = 110f;
        private static string[] _unityLayerNames;

        static PhysicsSection()
        {
            EditorApplication.projectChanged += ClearUnityLayerCache;
            EditorApplication.hierarchyChanged += ClearUnityLayerCache;
        }

        public static void Draw(RoomPainterAsset asset, GameObject sceneInstance)
        {
            asset.isBlock = EditorGUILayout.Toggle("Block", asset.isBlock);
            asset.isTrigger = EditorGUILayout.Toggle("Trigger", asset.isTrigger);
            asset.respectPrefabColliders = EditorGUILayout.Toggle("Respect Prefab Colliders", asset.respectPrefabColliders);

            using (new EditorGUI.DisabledScope(!asset.isBlock && !asset.isTrigger))
            {
                asset.bodyType = (RigidbodyType2D)EditorGUILayout.EnumPopup("Body Type", asset.bodyType);
                asset.colliderShape = (ColliderShape)EditorGUILayout.EnumPopup("Collider Shape", asset.colliderShape);
                asset.colliderSize = EditorGUILayout.Vector2Field("Collider Size", asset.colliderSize);
                asset.physicsLayerName = LayerPopup("Physics Layer", asset.physicsLayerName);

                DrawHitboxPreview(asset);
                DrawColliderQuickActions(asset);
            }

            DrawPrefabColliderSection(asset);
            DrawSceneInstanceControls(asset, sceneInstance);
        }

        // ── D4: Prefab Collider Authoring ────────────────────────────────────────

        private static void DrawPrefabColliderSection(RoomPainterAsset asset)
        {
            EditorGUILayout.Space(4f);
            EditorGUILayout.LabelField("Prefab Collider (D4)", EditorStyles.miniBoldLabel);

            // Shape selector dropdown — swaps collider type on the prefab root
            if (asset.prefab != null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Collider Shape:", GUILayout.Width(100f));
                    ColliderShape currentShape = ResolveShapeFromPrefab(asset.prefab);
                    EditorGUI.BeginChangeCheck();
                    ColliderShape nextShape = (ColliderShape)EditorGUILayout.EnumPopup(currentShape, GUILayout.Width(90f));
                    if (EditorGUI.EndChangeCheck() && nextShape != currentShape)
                    {
                        // Polygon deferred — guard
                        if (nextShape == ColliderShape.Polygon)
                        {
                            Debug.LogWarning("Room Painter D4: Polygon collider is deferred to Phase 2. Shape not changed.");
                        }
                        else
                        {
                            ColliderShapeSwapper.SwapShape(asset.prefab, nextShape);
                            AssetDatabase.SaveAssetIfDirty(asset.prefab);
                        }
                    }
                }

                // Live size/offset/trigger display (read-only)
                Collider2D prefabCollider = asset.prefab.GetComponent<Collider2D>();
                if (prefabCollider != null)
                {
                    string sizeLabel = GetColliderSizeLabel(prefabCollider);
                    string offsetLabel = "Offset: (" + prefabCollider.offset.x.ToString("0.00") + ", " + prefabCollider.offset.y.ToString("0.00") + ")";
                    string triggerLabel = "Trigger: " + (prefabCollider.isTrigger ? "Yes" : "No");
                    EditorGUILayout.LabelField(sizeLabel + "  |  " + offsetLabel + "  |  " + triggerLabel, EditorStyles.miniLabel);
                }
                else
                {
                    EditorGUILayout.LabelField("No Collider2D on prefab root.", EditorStyles.miniLabel);
                }

                // Edit in Prefab Mode button
                EditorGUILayout.Space(2f);
                if (GUILayout.Button("Edit in Prefab Mode"))
                {
                    string path = AssetDatabase.GetAssetPath(asset.prefab);
                    if (!string.IsNullOrEmpty(path))
                    {
                        AssetDatabase.OpenAsset(asset.prefab);
                    }
                    else
                    {
                        Debug.LogWarning("Room Painter D4: Cannot open Prefab Mode — prefab has no asset path.");
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No prefab assigned — select an asset with a prefab to enable collider authoring.", MessageType.None);
            }
        }

        private static ColliderShape ResolveShapeFromPrefab(GameObject prefab)
        {
            if (prefab == null)
            {
                return ColliderShape.Box;
            }

            Collider2D c = prefab.GetComponent<Collider2D>();
            if (c is CircleCollider2D)
            {
                return ColliderShape.Circle;
            }

            if (c is CapsuleCollider2D)
            {
                return ColliderShape.Capsule;
            }

            return ColliderShape.Box;
        }

        private static string GetColliderSizeLabel(Collider2D collider)
        {
            if (collider is BoxCollider2D box)
            {
                return "Size: " + box.size.x.ToString("0.00") + " x " + box.size.y.ToString("0.00");
            }

            if (collider is CircleCollider2D circle)
            {
                return "Radius: " + circle.radius.ToString("0.00");
            }

            if (collider is CapsuleCollider2D capsule)
            {
                return "Size: " + capsule.size.x.ToString("0.00") + " x " + capsule.size.y.ToString("0.00");
            }

            return "Size: —";
        }

        // ── end D4 ───────────────────────────────────────────────────────────────

        private static void DrawSceneInstanceControls(RoomPainterAsset asset, GameObject sceneInstance)
        {
            EditorGUILayout.Space(4f);
            EditorGUILayout.LabelField("Scene Instance", EditorStyles.miniBoldLabel);

            bool editEnabled = RoomPainterColliderEditor.Enabled;
            bool nextEnabled = EditorGUILayout.ToggleLeft("Edit collider handles in SceneView", editEnabled);
            if (nextEnabled != editEnabled)
            {
                RoomPainterColliderEditor.Enabled = nextEnabled;
            }

            using (new EditorGUI.DisabledScope(sceneInstance == null))
            {
                Collider2D existing = sceneInstance != null ? sceneInstance.GetComponent<Collider2D>() : null;
                if (sceneInstance != null && existing == null)
                {
                    EditorGUILayout.HelpBox("Selected instance has no Collider2D.", MessageType.Info);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Add Box Collider2D"))
                        {
                            AddCollider(sceneInstance, ColliderShape.Box);
                        }

                        if (GUILayout.Button("Add Circle"))
                        {
                            AddCollider(sceneInstance, ColliderShape.Circle);
                        }

                        if (GUILayout.Button("Add Capsule"))
                        {
                            AddCollider(sceneInstance, ColliderShape.Capsule);
                        }
                    }
                }
                else if (sceneInstance != null)
                {
                    GUILayout.Label("Active collider: " + existing.GetType().Name, EditorStyles.miniLabel);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Apply Asset → Instance"))
                    {
                        RoomPainterPhysicsApplier.Apply(sceneInstance, asset);
                    }

                    if (GUILayout.Button("Pull Instance → Asset"))
                    {
                        PullFromInstance(asset, sceneInstance);
                    }
                }
            }
        }

        private static void AddCollider(GameObject go, ColliderShape shape)
        {
            if (go == null)
            {
                return;
            }

            Undo.RegisterCompleteObjectUndo(go, "Add Room Painter Collider");
            switch (shape)
            {
                case ColliderShape.Circle:
                    Undo.AddComponent<CircleCollider2D>(go);
                    break;
                case ColliderShape.Capsule:
                    Undo.AddComponent<CapsuleCollider2D>(go);
                    break;
                default:
                    Undo.AddComponent<BoxCollider2D>(go);
                    break;
            }
        }

        private static void PullFromInstance(RoomPainterAsset asset, GameObject sceneInstance)
        {
            if (asset == null || sceneInstance == null)
            {
                return;
            }

            Collider2D collider = sceneInstance.GetComponent<Collider2D>();
            if (collider == null)
            {
                Debug.LogWarning("Room Painter: instance has no Collider2D to pull from.");
                return;
            }

            Undo.RecordObject(asset, "Pull Collider To Asset");

            if (collider is BoxCollider2D box)
            {
                asset.colliderShape = ColliderShape.Box;
                asset.colliderSize = box.size;
            }
            else if (collider is CircleCollider2D circle)
            {
                asset.colliderShape = ColliderShape.Circle;
                asset.colliderSize = new Vector2(circle.radius * 2f, circle.radius * 2f);
            }
            else if (collider is CapsuleCollider2D capsule)
            {
                asset.colliderShape = ColliderShape.Capsule;
                asset.colliderSize = capsule.size;
            }

            asset.isTrigger = collider.isTrigger;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
        }

        private static void DrawHitboxPreview(RoomPainterAsset asset)
        {
            Vector2 spriteSize = ResolveSpriteSize(asset);
            if (spriteSize.x <= 0f || spriteSize.y <= 0f)
            {
                EditorGUILayout.HelpBox("Hitbox preview unavailable (no sprite).", MessageType.None);
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                Rect previewArea = GUILayoutUtility.GetRect(HitboxPreviewSize, HitboxPreviewSize, GUILayout.Width(HitboxPreviewSize), GUILayout.Height(HitboxPreviewSize));
                EditorGUI.DrawRect(previewArea, new Color(0.09f, 0.09f, 0.10f, 1f));

                float scale = Mathf.Min((previewArea.width - 8f) / spriteSize.x, (previewArea.height - 8f) / spriteSize.y);
                Vector2 spriteDraw = spriteSize * scale;
                Rect spriteRect = new Rect(
                    previewArea.center.x - spriteDraw.x * 0.5f,
                    previewArea.center.y - spriteDraw.y * 0.5f,
                    spriteDraw.x,
                    spriteDraw.y);
                EditorGUI.DrawRect(spriteRect, new Color(0.18f, 0.20f, 0.24f, 1f));
                DrawRectOutline(spriteRect, new Color(0.45f, 0.45f, 0.50f, 1f));

                Vector2 colliderDraw = new Vector2(
                    Mathf.Abs(asset.colliderSize.x) * scale * 32f,
                    Mathf.Abs(asset.colliderSize.y) * scale * 32f);
                colliderDraw.x = Mathf.Clamp(colliderDraw.x, 2f, spriteDraw.x);
                colliderDraw.y = Mathf.Clamp(colliderDraw.y, 2f, spriteDraw.y);
                Rect colliderRect = new Rect(
                    spriteRect.center.x - colliderDraw.x * 0.5f,
                    spriteRect.center.y - colliderDraw.y * 0.5f,
                    colliderDraw.x,
                    colliderDraw.y);

                Color colliderTint = asset.isTrigger ? new Color(1f, 0.85f, 0.25f, 0.25f) : new Color(0.30f, 0.95f, 0.60f, 0.30f);
                Color outlineTint = asset.isTrigger ? new Color(1f, 0.85f, 0.25f, 1f) : new Color(0.30f, 0.95f, 0.60f, 1f);

                if (asset.colliderShape == ColliderShape.Circle)
                {
                    Handles.BeginGUI();
                    float radius = Mathf.Min(colliderDraw.x, colliderDraw.y) * 0.5f;
                    Handles.color = colliderTint;
                    Handles.DrawSolidDisc(spriteRect.center, Vector3.forward, radius);
                    Handles.color = outlineTint;
                    Handles.DrawWireDisc(spriteRect.center, Vector3.forward, radius);
                    Handles.EndGUI();
                }
                else
                {
                    EditorGUI.DrawRect(colliderRect, colliderTint);
                    DrawRectOutline(colliderRect, outlineTint);
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Label("Hitbox preview", EditorStyles.miniBoldLabel);
                    GUILayout.Label("Sprite: " + Mathf.RoundToInt(spriteSize.x) + " x " + Mathf.RoundToInt(spriteSize.y) + " px", EditorStyles.miniLabel);
                    GUILayout.Label("Collider: " + asset.colliderSize.x.ToString("0.00") + " x " + asset.colliderSize.y.ToString("0.00") + " u", EditorStyles.miniLabel);
                    GUILayout.Label("Shape: " + asset.colliderShape, EditorStyles.miniLabel);
                    GUILayout.Label(asset.isTrigger ? "Mode: Trigger (yellow)" : "Mode: Solid (green)", EditorStyles.miniLabel);
                }
            }
        }

        private static void DrawColliderQuickActions(RoomPainterAsset asset)
        {
            Vector2 spriteSize = ResolveSpriteSize(asset);
            if (spriteSize.x <= 0f || spriteSize.y <= 0f)
            {
                return;
            }

            float pixelsPerUnit = ResolvePixelsPerUnit(asset);
            if (pixelsPerUnit <= 0f)
            {
                pixelsPerUnit = 32f;
            }

            Vector2 fullUnitSize = spriteSize / pixelsPerUnit;

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Fit (100%)"))
                {
                    asset.colliderSize = fullUnitSize;
                }

                if (GUILayout.Button("Tight (85%)"))
                {
                    asset.colliderSize = fullUnitSize * 0.85f;
                }

                if (GUILayout.Button("Half"))
                {
                    asset.colliderSize = fullUnitSize * 0.5f;
                }

                if (GUILayout.Button("Square"))
                {
                    float min = Mathf.Min(fullUnitSize.x, fullUnitSize.y) * 0.85f;
                    asset.colliderSize = new Vector2(min, min);
                }
            }
        }

        private static Vector2 ResolveSpriteSize(RoomPainterAsset asset)
        {
            if (asset.sprite != null && asset.sprite.rect.width > 0f && asset.sprite.rect.height > 0f)
            {
                return asset.sprite.rect.size;
            }

            if (asset.prefab != null)
            {
                SpriteRenderer renderer = asset.prefab.GetComponentInChildren<SpriteRenderer>();
                if (renderer != null && renderer.sprite != null)
                {
                    return renderer.sprite.rect.size;
                }
            }

            return Vector2.zero;
        }

        private static float ResolvePixelsPerUnit(RoomPainterAsset asset)
        {
            if (asset.sprite != null)
            {
                return asset.sprite.pixelsPerUnit;
            }

            if (asset.prefab != null)
            {
                SpriteRenderer renderer = asset.prefab.GetComponentInChildren<SpriteRenderer>();
                if (renderer != null && renderer.sprite != null)
                {
                    return renderer.sprite.pixelsPerUnit;
                }
            }

            return 32f;
        }

        private static void DrawRectOutline(Rect rect, Color color)
        {
            float t = 1f;
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, t), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - t, rect.width, t), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, t, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - t, rect.y, t, rect.height), color);
        }

        private static string LayerPopup(string label, string current)
        {
            string[] layers = GetUnityLayerNames();
            if (layers == null || layers.Length == 0)
            {
                return EditorGUILayout.TextField(label, current);
            }

            int index = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] == current)
                {
                    index = i;
                    break;
                }
            }

            index = EditorGUILayout.Popup(label, index, layers);
            return layers[Mathf.Clamp(index, 0, layers.Length - 1)];
        }

        private static string[] GetUnityLayerNames()
        {
            if (_unityLayerNames == null)
            {
                _unityLayerNames = InternalEditorUtility.layers;
            }

            return _unityLayerNames;
        }

        private static void ClearUnityLayerCache()
        {
            _unityLayerNames = null;
        }
    }
}
