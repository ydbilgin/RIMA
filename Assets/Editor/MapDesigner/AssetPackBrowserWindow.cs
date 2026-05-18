#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;
using AssetPackCategory = RIMA.MapDesigner.SO.AssetPackCategory;
using AssetPackManifestSO = RIMA.MapDesigner.SO.AssetPackManifestSO;
using PatchAtlasSO = RIMA.MapDesigner.SO.PatchAtlasSO;
using PropDefinitionSO = RIMA.MapDesigner.SO.PropDefinitionSO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RIMA.MapDesigner.Editor
{
    public class AssetPackBrowserWindow : EditorWindow
    {
        private const float LeftPanelWidth = 260f;
        private const float RightPanelWidth = 300f;
        private const float BottomGridHeight = 220f;
        private const string AllCategoryId = "__all__";
        private const float PlacementSnapStep = 1f / 32f;
        private const string DefaultRoomRootPath = "PlayableRoom/Pro_Redesign_v14_CombatRoom";
        private const string GhostPreviewName = "_GhostPreview";

        [SerializeField] private AssetPackManifestSO activePack;
        [SerializeField] private string searchQuery = string.Empty;
        [SerializeField] private string activeCategoryId = AllCategoryId;
        [SerializeField] private AssetPackEntry selectedEntry;
        [SerializeField] private float thumbnailSize = 64f;
        [SerializeField] private Transform activeRoomRoot;
        [SerializeField] private bool placementMode;

        private AssetPackCatalog catalog;
        private SelectedSpriteInspector selectedSpriteInspector;
        private Vector2 categoryScroll;
        private Vector2 gridScroll;
        private Vector2 inspectorScroll;
        private AssetPackEntry hoverEntry;
        private List<AssetPackEntry> visibleEntries = new List<AssetPackEntry>();
        private GameObject ghostPreview;
        private SpriteRenderer ghostRenderer;
        private Vector3 lastGhostPosition;
        private bool lastGhostValid;

        public AssetPackCatalog Catalog => catalog;
        public AssetPackManifestSO ActivePack => activePack;
        public string ActiveCategoryId => activeCategoryId;
        public AssetPackEntry SelectedEntry => selectedEntry;
        public Transform ActiveRoomRoot => activeRoomRoot;
        public bool IsPlacementMode => placementMode;
        public GameObject GhostPreview => ghostPreview;
        public Vector3 LastGhostPosition => lastGhostPosition;
        public IReadOnlyList<AssetPackEntry> VisibleEntries => visibleEntries;
        public float ThumbnailSize
        {
            get => thumbnailSize;
            set => thumbnailSize = ClampThumbnailSize(value);
        }

        [MenuItem("Tools/RIMA/Map Designer/Asset Pack Browser")]
        public static void Open()
        {
            var window = GetWindow<AssetPackBrowserWindow>("Asset Pack Browser");
            window.minSize = new Vector2(1100f, 620f);
            window.Show();
            window.RefreshCatalog();
        }

        private void OnEnable()
        {
            minSize = new Vector2(1100f, 620f);
            EnsureInitialized();
            RefreshCatalog();
            SceneView.duringSceneGui -= OnSceneViewGUI;
            SceneView.duringSceneGui += OnSceneViewGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneViewGUI;
            hoverEntry = null;
            DisablePlacementMode();
        }

        private void OnGUI()
        {
            try
            {
                EnsureInitialized();
                RefreshVisibleEntries();
                DrawTopBar();
                DrawMainLayout();
                DrawBottomSpriteGrid();
            }
            catch (ExitGUIException)
            {
                throw;
            }
            catch (Exception ex)
            {
                EditorGUILayout.HelpBox("Asset Pack Browser failed during repaint. See Console.", MessageType.Error);
                Debug.LogException(ex);
            }
        }

        private void EnsureInitialized()
        {
            if (catalog == null) catalog = new AssetPackCatalog();
            if (selectedSpriteInspector == null) selectedSpriteInspector = new SelectedSpriteInspector();
            thumbnailSize = ClampThumbnailSize(thumbnailSize);
        }

        private void DrawTopBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("RIMA Asset Pack Browser", EditorStyles.boldLabel, GUILayout.Width(220f));
                if (activePack != null)
                {
                    GUILayout.Label($"Sprites: {catalog.CountProductionSprites(activePack)}", GUILayout.Width(110f));
                }
                else
                {
                    GUILayout.Label("No pack selected", GUILayout.Width(140f));
                }

                DrawActiveRoomRootToolbar();

                GUILayout.FlexibleSpace();
                GUILayout.Label("Thumbnail", GUILayout.Width(70f));
                ThumbnailSize = GUILayout.HorizontalSlider(thumbnailSize, 48f, 96f, GUILayout.Width(120f));
                GUILayout.Label($"{thumbnailSize:0}px", GUILayout.Width(48f));
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70f)))
                {
                    RefreshCatalog();
                }
            }
        }

        private void DrawActiveRoomRootToolbar()
        {
            EnsureActiveRoomRoot();
            GUILayout.Label("Active Room Root:", GUILayout.Width(105f));
            EditorGUI.BeginChangeCheck();
            Transform nextRoot = (Transform)EditorGUILayout.ObjectField(activeRoomRoot, typeof(Transform), true, GUILayout.Width(260f));
            if (EditorGUI.EndChangeCheck())
            {
                SetActiveRoomRoot(nextRoot, true);
            }

            if (GUILayout.Button("Use Selection", EditorStyles.toolbarButton, GUILayout.Width(95f)))
            {
                SetActiveRoomRoot(Selection.activeTransform, true);
            }

            if (activeRoomRoot != null && GUILayout.Button("Ping", EditorStyles.toolbarButton, GUILayout.Width(42f)))
            {
                EditorGUIUtility.PingObject(activeRoomRoot.gameObject);
            }
        }

        private void DrawMainLayout()
        {
            Rect layoutRect = GUILayoutUtility.GetRect(position.width, Mathf.Max(220f, position.height - BottomGridHeight - 42f), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            Rect leftRect = new Rect(layoutRect.x, layoutRect.y, LeftPanelWidth, layoutRect.height);
            Rect rightRect = new Rect(layoutRect.xMax - RightPanelWidth, layoutRect.y, RightPanelWidth, layoutRect.height);
            Rect centerRect = new Rect(leftRect.xMax + 6f, layoutRect.y, Mathf.Max(200f, rightRect.x - leftRect.xMax - 12f), layoutRect.height);

            GUILayout.BeginArea(leftRect, EditorStyles.helpBox);
            DrawLeftPanel();
            GUILayout.EndArea();

            GUILayout.BeginArea(centerRect, EditorStyles.helpBox);
            DrawCenterPreview();
            GUILayout.EndArea();

            GUILayout.BeginArea(rightRect, EditorStyles.helpBox);
            inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);
            selectedSpriteInspector.Draw(selectedEntry);
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawLeftPanel()
        {
            GUILayout.Label("Browser", EditorStyles.boldLabel);
            DrawPackDropdown();
            EditorGUILayout.Space(4f);
            GUILayout.Label("Search", EditorStyles.miniBoldLabel);
            string nextSearch = EditorGUILayout.TextField(searchQuery ?? string.Empty, EditorStyles.toolbarSearchField);
            if (!string.Equals(nextSearch, searchQuery, StringComparison.Ordinal))
            {
                searchQuery = nextSearch;
                RefreshVisibleEntries();
            }

            EditorGUILayout.Space(8f);
            GUILayout.Label("Categories", EditorStyles.miniBoldLabel);
            categoryScroll = EditorGUILayout.BeginScrollView(categoryScroll);
            DrawCategoryButton("All", AllCategoryId);
            IReadOnlyList<AssetPackCategory> categories = catalog.GetCategories(activePack);
            for (int i = 0; i < categories.Count; i++)
            {
                string categoryName = categories[i].categoryName;
                if (string.IsNullOrEmpty(categoryName)) continue;
                DrawCategoryButton(categoryName, categoryName);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawPackDropdown()
        {
            IReadOnlyList<AssetPackManifestSO> packs = catalog.Packs;
            if (packs.Count == 0)
            {
                EditorGUILayout.HelpBox("No AssetPackManifestSO assets found.", MessageType.Info);
                return;
            }

            int currentIndex = 0;
            string[] names = new string[packs.Count];
            for (int i = 0; i < packs.Count; i++)
            {
                names[i] = string.IsNullOrEmpty(packs[i].displayName) ? packs[i].name : packs[i].displayName;
                if (packs[i] == activePack) currentIndex = i;
            }

            int nextIndex = EditorGUILayout.Popup("Pack", currentIndex, names);
            if (nextIndex >= 0 && nextIndex < packs.Count && packs[nextIndex] != activePack)
            {
                SelectPack(packs[nextIndex]);
            }
        }

        private void DrawCategoryButton(string label, string categoryId)
        {
            bool selected = string.Equals(activeCategoryId, categoryId, StringComparison.OrdinalIgnoreCase);
            GUIStyle style = selected ? EditorStyles.toolbarButton : EditorStyles.miniButton;
            if (GUILayout.Button(label, style, GUILayout.Height(24f)))
            {
                SelectCategory(categoryId);
            }
        }

        private void DrawCenterPreview()
        {
            AssetPackEntry previewEntry = hoverEntry ?? selectedEntry;
            GUILayout.Label("Preview", EditorStyles.boldLabel);
            Rect previewRect = GUILayoutUtility.GetRect(256f, 256f, GUILayout.ExpandWidth(false));
            previewRect.width = Mathf.Min(256f, previewRect.width);
            DrawChecker(previewRect);

            if (previewEntry != null && previewEntry.sprite != null)
            {
                DrawSprite(previewRect, previewEntry.sprite, ScaleMode.ScaleToFit);
            }
            else
            {
                GUI.Label(previewRect, "No Sprite", CenteredMiniLabel());
            }

            EditorGUILayout.Space(6f);
            if (previewEntry != null)
            {
                DrawMetadata(previewEntry);
            }
            else
            {
                EditorGUILayout.HelpBox("Hover or select a sprite to inspect metadata.", MessageType.Info);
            }

            EditorGUILayout.Space(6f);
            string rootLabel = activeRoomRoot != null ? GetTransformPath(activeRoomRoot) : "No active room root";
            string placementLabel = placementMode ? $"Placement: {rootLabel}" : "Placement: inactive";
            EditorGUILayout.HelpBox(placementLabel, placementMode && IsValidPlacementTarget(activeRoomRoot) ? MessageType.Info : MessageType.Warning);
        }

        private void DrawMetadata(AssetPackEntry entry)
        {
            foreach (string line in SelectedSpriteInspector.BuildMetadataLines(entry))
            {
                EditorGUILayout.LabelField(line, EditorStyles.miniLabel);
            }
        }

        private void DrawBottomSpriteGrid()
        {
            Rect gridRect = GUILayoutUtility.GetRect(position.width, BottomGridHeight, GUILayout.ExpandWidth(true));
            GUILayout.BeginArea(gridRect, EditorStyles.helpBox);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Sprite Grid", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.Label($"{visibleEntries.Count} visible", EditorStyles.miniLabel);
            }

            if (activePack == null)
            {
                EditorGUILayout.HelpBox("Select an asset pack to browse sprites.", MessageType.Info);
                GUILayout.EndArea();
                return;
            }

            if (visibleEntries.Count == 0)
            {
                EditorGUILayout.HelpBox($"No sprites in {GetPackDisplayName(activePack)} for the current filter.", MessageType.Info);
                GUILayout.EndArea();
                return;
            }

            gridScroll = EditorGUILayout.BeginScrollView(gridScroll);
            DrawGridRows();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawGridRows()
        {
            float cellSize = thumbnailSize + 24f;
            int columns = Mathf.Max(1, Mathf.FloorToInt((position.width - 32f) / cellSize));
            int index = 0;
            while (index < visibleEntries.Count)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    for (int col = 0; col < columns && index < visibleEntries.Count; col++, index++)
                    {
                        DrawGridCell(visibleEntries[index], cellSize);
                    }
                }
            }
        }

        private void DrawGridCell(AssetPackEntry entry, float cellSize)
        {
            Rect rect = GUILayoutUtility.GetRect(cellSize, cellSize + 18f, GUILayout.Width(cellSize), GUILayout.Height(cellSize + 18f));
            bool isSelected = selectedEntry != null && string.Equals(selectedEntry.entryId, entry.entryId, StringComparison.Ordinal);
            Color bg = isSelected ? new Color(0.18f, 0.38f, 0.65f, 1f) : new Color(0.18f, 0.18f, 0.18f, 1f);
            EditorGUI.DrawRect(rect, bg);

            Rect thumbRect = new Rect(rect.x + 8f, rect.y + 6f, thumbnailSize, thumbnailSize);
            DrawChecker(thumbRect);
            if (entry.sprite != null)
            {
                DrawSprite(thumbRect, entry.sprite, ScaleMode.ScaleToFit);
            }
            else
            {
                GUI.Label(thumbRect, "Missing", CenteredMiniLabel());
            }

            Rect labelRect = new Rect(rect.x + 4f, rect.yMax - 18f, rect.width - 8f, 16f);
            GUI.Label(labelRect, entry.displayName, CenteredMiniLabel());

            Event evt = Event.current;
            if (rect.Contains(evt.mousePosition))
            {
                hoverEntry = entry;
                if (evt.type == EventType.MouseDown && evt.button == 0)
                {
                    SelectEntry(entry);
                    evt.Use();
                }
                if (evt.type == EventType.Repaint)
                {
                    Repaint();
                }
            }
        }

        private void SelectPack(AssetPackManifestSO pack)
        {
            activePack = pack;
            activeCategoryId = AllCategoryId;
            selectedEntry = null;
            hoverEntry = null;
            DisablePlacementMode();
            RefreshVisibleEntries();
            Repaint();
        }

        private void SelectCategory(string categoryId)
        {
            activeCategoryId = string.IsNullOrEmpty(categoryId) ? AllCategoryId : categoryId;
            RefreshVisibleEntries();
            Repaint();
        }

        private void SelectEntry(AssetPackEntry entry)
        {
            selectedEntry = entry;
            if (selectedEntry != null && selectedEntry.sprite != null)
            {
                EnablePlacementMode();
            }
            else
            {
                DisablePlacementMode();
            }
            Repaint();
        }

        private void RefreshCatalog()
        {
            EnsureInitialized();
            catalog.Refresh();
            if (activePack == null || !catalog.ContainsPack(activePack))
            {
                activePack = catalog.Packs.Count > 0 ? catalog.Packs[0] : null;
                activeCategoryId = AllCategoryId;
                selectedEntry = null;
                hoverEntry = null;
            }
            RefreshVisibleEntries();
            Repaint();
        }

        private void RefreshVisibleEntries()
        {
            if (catalog == null)
            {
                visibleEntries = new List<AssetPackEntry>();
                return;
            }

            visibleEntries = new List<AssetPackEntry>(catalog.Query(activePack, activeCategoryId, searchQuery));
        }

        private void EnablePlacementMode()
        {
            if (selectedEntry == null || selectedEntry.sprite == null)
            {
                DisablePlacementMode();
                return;
            }

            EnsureActiveRoomRoot();
            placementMode = true;
            EnsureGhostPreview();
            ApplyGhostSprite(selectedEntry.sprite);
            SceneView.RepaintAll();
        }

        private void DisablePlacementMode()
        {
            placementMode = false;
            DestroyGhostPreview();
        }

        private void OnSceneViewGUI(SceneView sceneView)
        {
            if (!placementMode || selectedEntry == null || selectedEntry.sprite == null)
            {
                return;
            }

            Event evt = Event.current;
            if (evt.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }

            if (IsEscapePressedThisFrame())
            {
                DisablePlacementMode();
                Repaint();
                sceneView.Repaint();
                return;
            }

            Vector3 worldPosition = SnapWorldPosition(SceneMouseToWorld(evt.mousePosition));
            UpdateGhostPreview(worldPosition);

            if (evt.type == EventType.MouseDown && evt.button == 1)
            {
                DisablePlacementMode();
                evt.Use();
                Repaint();
                sceneView.Repaint();
                return;
            }

            if (evt.type == EventType.MouseDown && evt.button == 0)
            {
                PlaceSelectedAt(worldPosition);
                evt.Use();
                Repaint();
                sceneView.Repaint();
            }
            else if (evt.type == EventType.MouseMove || evt.type == EventType.Repaint)
            {
                sceneView.Repaint();
            }
        }

        private Vector3 SceneMouseToWorld(Vector2 mousePosition)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            if (Mathf.Abs(ray.direction.z) > 0.0001f)
            {
                float distance = -ray.origin.z / ray.direction.z;
                return ray.origin + ray.direction * distance;
            }

            return new Vector3(ray.origin.x, ray.origin.y, 0f);
        }

        private void UpdateGhostPreview(Vector3 worldPosition)
        {
            EnsureGhostPreview();
            ApplyGhostSprite(selectedEntry.sprite);
            lastGhostPosition = worldPosition;
            lastGhostValid = IsValidPlacementTarget(activeRoomRoot);

            if (ghostPreview == null)
            {
                return;
            }

            ghostPreview.transform.position = worldPosition;
            ghostPreview.hideFlags = HideFlags.HideAndDontSave;
            if (ghostRenderer != null)
            {
                ghostRenderer.color = lastGhostValid ? new Color(1f, 1f, 1f, 0.45f) : new Color(1f, 0.2f, 0.2f, 0.45f);
                ghostRenderer.sortingOrder = selectedEntry.defaultSortingOrder;
            }
        }

        private void EnsureGhostPreview()
        {
            if (ghostPreview != null && ghostRenderer != null)
            {
                return;
            }

            DestroyGhostPreview();
            ghostPreview = new GameObject(GhostPreviewName);
            ghostPreview.hideFlags = HideFlags.HideAndDontSave;
            ghostRenderer = ghostPreview.AddComponent<SpriteRenderer>();
            ghostRenderer.color = new Color(1f, 1f, 1f, 0.45f);
        }

        private void ApplyGhostSprite(Sprite sprite)
        {
            if (ghostRenderer != null)
            {
                ghostRenderer.sprite = sprite;
            }
        }

        private void DestroyGhostPreview()
        {
            if (ghostPreview != null)
            {
                DestroyImmediate(ghostPreview);
            }

            ghostPreview = null;
            ghostRenderer = null;
        }

        private GameObject PlaceSelectedAt(Vector3 worldPosition)
        {
            EnsureActiveRoomRoot();
            if (!IsValidPlacementTarget(activeRoomRoot))
            {
                Debug.LogWarning("Asset Pack Browser placement failed: active room root must be a Transform in the active scene.");
                return null;
            }

            return PlaceEntry(selectedEntry, activeRoomRoot, SnapWorldPosition(worldPosition), true);
        }

        public GameObject PlaceEntryForTests(AssetPackEntry entry, Transform parent, Vector3 worldPosition)
        {
            return PlaceEntry(entry, parent, SnapWorldPosition(worldPosition), true);
        }

        public void SetActiveRoomRootForTests(Transform root)
        {
            SetActiveRoomRoot(root, false);
        }

        public void DisablePlacementForTests()
        {
            DisablePlacementMode();
        }

        public void UpdateGhostPositionForTests(Vector3 worldPosition)
        {
            if (!placementMode)
            {
                EnablePlacementMode();
            }

            UpdateGhostPreview(SnapWorldPosition(worldPosition));
        }

        private static GameObject PlaceEntry(AssetPackEntry entry, Transform parent, Vector3 worldPosition, bool selectAfterCreate)
        {
            if (entry == null || entry.sprite == null || parent == null)
            {
                return null;
            }

            var placedObject = new GameObject(string.IsNullOrEmpty(entry.displayName) ? "PlacedSprite" : entry.displayName);
            Undo.RegisterCreatedObjectUndo(placedObject, "Place Asset Pack Sprite");
            placedObject.transform.SetParent(parent, true);
            placedObject.transform.position = worldPosition;

            SpriteRenderer renderer = placedObject.AddComponent<SpriteRenderer>();
            renderer.sprite = entry.sprite;
            renderer.sortingOrder = entry.defaultSortingOrder;

            if (entry.collisionPreset.blocksMovement && entry.collisionPreset.colliderShape != ColliderShape.None)
            {
                AttachAutoCollider(placedObject, entry.sprite, entry.collisionPreset);
            }

            if (selectAfterCreate)
            {
                Selection.activeGameObject = placedObject;
                EditorGUIUtility.PingObject(placedObject);
            }

            return placedObject;
        }

        public static Collider2D AttachAutoCollider(GameObject target, Sprite sprite, CollisionPreset preset)
        {
            if (target == null || sprite == null || !preset.blocksMovement || preset.colliderShape == ColliderShape.None)
            {
                return null;
            }

            Bounds bounds = sprite.bounds;
            float ratio = Mathf.Clamp(preset.colliderFootprintRatio, 0.3f, 1f);
            Vector2 size = new Vector2(bounds.size.x * ratio, bounds.size.y * ratio);
            Vector2 offset = new Vector2(bounds.center.x, bounds.center.y) + preset.colliderOffset;
            Collider2D collider = null;

            switch (preset.colliderShape)
            {
                case ColliderShape.Box:
                    var box = target.AddComponent<BoxCollider2D>();
                    box.size = size;
                    box.offset = offset;
                    collider = box;
                    break;
                case ColliderShape.Circle:
                    var circle = target.AddComponent<CircleCollider2D>();
                    circle.radius = Mathf.Min(bounds.size.x, bounds.size.y) * ratio * 0.5f;
                    circle.offset = offset;
                    collider = circle;
                    break;
                case ColliderShape.Capsule:
                    var capsule = target.AddComponent<CapsuleCollider2D>();
                    capsule.size = size;
                    capsule.offset = offset;
                    capsule.direction = size.y >= size.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal;
                    collider = capsule;
                    break;
                case ColliderShape.PolygonAutoTrace:
                    collider = target.AddComponent<PolygonCollider2D>();
                    break;
            }

            if (collider != null)
            {
                collider.isTrigger = preset.isTrigger;
                int layer = string.IsNullOrEmpty(preset.colliderLayer) ? -1 : LayerMask.NameToLayer(preset.colliderLayer);
                if (layer >= 0)
                {
                    target.layer = layer;
                }
                else
                {
                    Debug.LogWarning($"Asset Pack Browser: layer '{preset.colliderLayer}' does not exist. Keeping '{target.name}' on Default.");
                }
            }

            return collider;
        }

        public static Vector3 SnapWorldPosition(Vector3 worldPosition)
        {
            return new Vector3(
                Mathf.Round(worldPosition.x / PlacementSnapStep) * PlacementSnapStep,
                Mathf.Round(worldPosition.y / PlacementSnapStep) * PlacementSnapStep,
                Mathf.Round(worldPosition.z / PlacementSnapStep) * PlacementSnapStep);
        }

        private void EnsureActiveRoomRoot()
        {
            if (IsValidPlacementTarget(activeRoomRoot))
            {
                return;
            }

            GameObject defaultRoot = GameObject.Find(DefaultRoomRootPath);
            if (defaultRoot != null && IsValidPlacementTarget(defaultRoot.transform))
            {
                activeRoomRoot = defaultRoot.transform;
                return;
            }

            if (IsValidPlacementTarget(Selection.activeTransform))
            {
                activeRoomRoot = Selection.activeTransform;
            }
        }

        private void SetActiveRoomRoot(Transform root, bool ping)
        {
            activeRoomRoot = IsValidPlacementTarget(root) ? root : null;
            if (ping && activeRoomRoot != null)
            {
                Selection.activeTransform = activeRoomRoot;
                EditorGUIUtility.PingObject(activeRoomRoot.gameObject);
            }
        }

        private static bool IsValidPlacementTarget(Transform root)
        {
            if (root == null || EditorUtility.IsPersistent(root))
            {
                return false;
            }

            Scene activeScene = SceneManager.GetActiveScene();
            return activeScene.IsValid() && root.gameObject.scene == activeScene;
        }

        private static bool IsEscapePressedThisFrame()
        {
            Keyboard keyboard = Keyboard.current;
            return keyboard != null && keyboard.escapeKey.wasPressedThisFrame;
        }

        private static string GetTransformPath(Transform transform)
        {
            if (transform == null)
            {
                return "None";
            }

            string path = transform.name;
            Transform current = transform.parent;
            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            return path;
        }

        public void SetCatalogForTests(AssetPackCatalog testCatalog)
        {
            catalog = testCatalog;
            selectedSpriteInspector = selectedSpriteInspector ?? new SelectedSpriteInspector();
            activePack = catalog != null && catalog.Packs.Count > 0 ? catalog.Packs[0] : null;
            activeCategoryId = AllCategoryId;
            selectedEntry = null;
            RefreshVisibleEntries();
        }

        public void SelectPackForTests(AssetPackManifestSO pack) => SelectPack(pack);
        public void SelectCategoryForTests(string categoryId) => SelectCategory(categoryId);
        public void SelectEntryForTests(AssetPackEntry entry) => SelectEntry(entry);
        public void SetSearchForTests(string query)
        {
            searchQuery = query ?? string.Empty;
            RefreshVisibleEntries();
        }

        public static float ClampThumbnailSize(float value) => Mathf.Clamp(value, 48f, 96f);

        private static string GetPackDisplayName(AssetPackManifestSO pack)
        {
            return pack == null ? "None" : (string.IsNullOrEmpty(pack.displayName) ? pack.name : pack.displayName);
        }

        private static void DrawChecker(Rect rect)
        {
            const float tile = 16f;
            for (float y = rect.y; y < rect.yMax; y += tile)
            {
                for (float x = rect.x; x < rect.xMax; x += tile)
                {
                    bool dark = ((int)((x - rect.x) / tile) + (int)((y - rect.y) / tile)) % 2 == 0;
                    EditorGUI.DrawRect(new Rect(x, y, Mathf.Min(tile, rect.xMax - x), Mathf.Min(tile, rect.yMax - y)),
                        dark ? new Color(0.22f, 0.22f, 0.22f, 1f) : new Color(0.30f, 0.30f, 0.30f, 1f));
                }
            }
        }

        private static void DrawSprite(Rect rect, Sprite sprite, ScaleMode scaleMode)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(sprite);
            if (texture == null) texture = AssetPreview.GetMiniThumbnail(sprite) as Texture2D;
            if (texture != null)
            {
                GUI.DrawTexture(rect, texture, scaleMode, true);
            }
        }

        private static GUIStyle CenteredMiniLabel()
        {
            GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
            style.alignment = TextAnchor.MiddleCenter;
            style.wordWrap = true;
            return style;
        }
    }

    public sealed class AssetPackBrowserPanel
    {
        public void Draw(AssetPackCatalog catalog, AssetPackManifestSO activePack, string activeCategoryId, string searchQuery)
        {
            EditorGUILayout.LabelField("Browser panel is owned by AssetPackBrowserWindow in B-1.", EditorStyles.miniLabel);
        }
    }

    public sealed class CategoryTreeView
    {
        public void Draw(IReadOnlyList<AssetPackCategory> categories, string selectedCategoryId)
        {
            if (categories == null) return;
            for (int i = 0; i < categories.Count; i++)
            {
                EditorGUILayout.LabelField(categories[i].categoryName, EditorStyles.miniLabel);
            }
        }
    }

    public sealed class SpriteGridView
    {
        public void Draw(IReadOnlyList<AssetPackEntry> entries, AssetPackEntry selectedEntry, float thumbnailSize)
        {
            EditorGUILayout.LabelField(entries == null ? "0 sprites" : $"{entries.Count} sprites", EditorStyles.miniLabel);
        }
    }

    public sealed class SearchBar
    {
        public string Draw(string currentQuery)
        {
            return EditorGUILayout.TextField(currentQuery ?? string.Empty, EditorStyles.toolbarSearchField);
        }
    }

    public sealed class SelectedSpriteInspector
    {
        public void Draw(AssetPackEntry selectedEntry)
        {
            GUILayout.Label("Selected Sprite", EditorStyles.boldLabel);
            if (selectedEntry == null)
            {
                EditorGUILayout.HelpBox("No sprite selected.", MessageType.Info);
                return;
            }

            foreach (string line in BuildMetadataLines(selectedEntry))
            {
                EditorGUILayout.LabelField(line, EditorStyles.wordWrappedMiniLabel);
            }

            DrawPlacedObjectInspector(selectedEntry);
        }

        public static IReadOnlyList<string> BuildMetadataLines(AssetPackEntry entry)
        {
            var lines = new List<string>();
            if (entry == null)
            {
                return lines;
            }

            lines.Add($"Name: {entry.displayName}");
            lines.Add($"Pack: {entry.sourcePack}");
            lines.Add($"Atlas: {entry.sourceAtlas}");
            lines.Add($"Category: {entry.categoryName}");
            lines.Add($"Source: {entry.sourcePath}");
            lines.Add($"Pixel Size: {entry.pixelSize.x} x {entry.pixelSize.y}");
            lines.Add($"PPU: {entry.pixelsPerUnit}");
            lines.Add($"Sorting Order: {entry.defaultSortingOrder}");
            lines.Add($"Blocks Movement: {entry.defaultBlocksMovement}");
            if (entry.isPreviewOnly) lines.Add("Preview Only: True");
            if (entry.isMissing) lines.Add("Missing Sprite: True");
            return lines;
        }

        private static void DrawPlacedObjectInspector(AssetPackEntry selectedEntry)
        {
            EditorGUILayout.Space(8f);
            GUILayout.Label("Placed Object", EditorStyles.boldLabel);

            GameObject selectedObject = Selection.activeGameObject;
            SpriteRenderer renderer = selectedObject != null ? selectedObject.GetComponent<SpriteRenderer>() : null;
            if (selectedObject == null || renderer == null)
            {
                EditorGUILayout.HelpBox("Select a placed sprite GameObject to edit transform, renderer, and collider settings.", MessageType.Info);
                return;
            }

            Sprite[] variants = GetVariants(selectedEntry);
            if (variants.Length > 0)
            {
                int currentVariant = Mathf.Max(0, Array.IndexOf(variants, renderer.sprite));
                EditorGUI.BeginChangeCheck();
                int nextVariant = EditorGUILayout.IntSlider("Variant", currentVariant, 0, variants.Length - 1);
                if (EditorGUI.EndChangeCheck())
                {
                    ApplyVariantIndex(selectedObject, selectedEntry, nextVariant);
                }
            }

            EditorGUI.BeginChangeCheck();
            float currentScale = selectedObject.transform.localScale.x;
            float nextScale = EditorGUILayout.Slider("Scale", currentScale, 0.3f, 2f);
            if (EditorGUI.EndChangeCheck())
            {
                ApplyScale(selectedObject, nextScale);
            }

            EditorGUI.BeginChangeCheck();
            float nextAlpha = EditorGUILayout.Slider("Alpha", renderer.color.a, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                ApplyAlpha(selectedObject, nextAlpha);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUI.BeginChangeCheck();
                bool nextFlipX = EditorGUILayout.ToggleLeft("Flip X", renderer.flipX, GUILayout.Width(90f));
                bool nextFlipY = EditorGUILayout.ToggleLeft("Flip Y", renderer.flipY, GUILayout.Width(90f));
                if (EditorGUI.EndChangeCheck())
                {
                    ApplyFlip(selectedObject, nextFlipX, nextFlipY);
                }
            }

            EditorGUI.BeginChangeCheck();
            int nextSortingOrder = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);
            if (EditorGUI.EndChangeCheck())
            {
                ApplySortingOrder(selectedObject, nextSortingOrder);
            }

            CollisionPreset preset = selectedEntry != null ? selectedEntry.collisionPreset : default;
            if (preset.blocksMovement || selectedObject.GetComponent<Collider2D>() != null)
            {
                EditorGUILayout.Space(6f);
                GUILayout.Label("Collider", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                preset.colliderShape = (ColliderShape)EditorGUILayout.EnumPopup("Shape", preset.colliderShape);
                preset.colliderFootprintRatio = EditorGUILayout.Slider("Footprint", Mathf.Clamp(preset.colliderFootprintRatio, 0.3f, 1f), 0.3f, 1f);
                preset.colliderOffset = EditorGUILayout.Vector2Field("Offset", preset.colliderOffset);
                preset.isTrigger = EditorGUILayout.Toggle("Is Trigger", preset.isTrigger);
                if (EditorGUI.EndChangeCheck())
                {
                    preset.blocksMovement = preset.colliderShape != ColliderShape.None;
                    ApplyColliderConfig(selectedObject, renderer.sprite, preset);
                }
            }
        }

        public static void ApplyScale(GameObject target, float scale)
        {
            if (target == null)
            {
                return;
            }

            Undo.RecordObject(target.transform, "Set Placed Sprite Scale");
            float clamped = Mathf.Clamp(scale, 0.3f, 2f);
            target.transform.localScale = new Vector3(clamped, clamped, clamped);
            EditorUtility.SetDirty(target.transform);
        }

        public static void ApplyAlpha(GameObject target, float alpha)
        {
            SpriteRenderer renderer = target != null ? target.GetComponent<SpriteRenderer>() : null;
            if (renderer == null)
            {
                return;
            }

            Undo.RecordObject(renderer, "Set Placed Sprite Alpha");
            Color color = renderer.color;
            color.a = Mathf.Clamp01(alpha);
            renderer.color = color;
            EditorUtility.SetDirty(renderer);
        }

        public static void ApplyFlip(GameObject target, bool flipX, bool flipY)
        {
            SpriteRenderer renderer = target != null ? target.GetComponent<SpriteRenderer>() : null;
            if (renderer == null)
            {
                return;
            }

            Undo.RecordObject(renderer, "Set Placed Sprite Flip");
            renderer.flipX = flipX;
            renderer.flipY = flipY;
            EditorUtility.SetDirty(renderer);
        }

        public static void ApplySortingOrder(GameObject target, int sortingOrder)
        {
            SpriteRenderer renderer = target != null ? target.GetComponent<SpriteRenderer>() : null;
            if (renderer == null)
            {
                return;
            }

            Undo.RecordObject(renderer, "Set Placed Sprite Sorting Order");
            renderer.sortingOrder = sortingOrder;
            EditorUtility.SetDirty(renderer);
        }

        public static void ApplyVariantIndex(GameObject target, AssetPackEntry selectedEntry, int variantIndex)
        {
            SpriteRenderer renderer = target != null ? target.GetComponent<SpriteRenderer>() : null;
            Sprite[] variants = GetVariants(selectedEntry);
            if (renderer == null || variants.Length == 0)
            {
                return;
            }

            int index = ((variantIndex % variants.Length) + variants.Length) % variants.Length;
            Undo.RecordObject(renderer, "Set Placed Sprite Variant");
            renderer.sprite = variants[index];
            EditorUtility.SetDirty(renderer);
        }

        public static void ApplyColliderConfig(GameObject target, Sprite sprite, CollisionPreset preset)
        {
            if (target == null)
            {
                return;
            }

            Collider2D[] existingColliders = target.GetComponents<Collider2D>();
            for (int i = existingColliders.Length - 1; i >= 0; i--)
            {
                Undo.DestroyObjectImmediate(existingColliders[i]);
            }

            if (preset.blocksMovement && preset.colliderShape != ColliderShape.None)
            {
                AssetPackBrowserWindow.AttachAutoCollider(target, sprite, preset);
            }
        }

        private static Sprite[] GetVariants(AssetPackEntry selectedEntry)
        {
            if (selectedEntry == null)
            {
                return Array.Empty<Sprite>();
            }

            if (selectedEntry.sourceObject is PatchAtlasSO atlas && atlas.variants != null)
            {
                return atlas.variants;
            }

            if (selectedEntry.sourceObject is PropDefinitionSO prop && prop.visual != null)
            {
                return new[] { prop.visual };
            }

            return selectedEntry.sprite != null ? new[] { selectedEntry.sprite } : Array.Empty<Sprite>();
        }
    }

    public sealed class VariantSlider
    {
        public int Draw(int currentVariant, int variantCount) => currentVariant;
    }

    public sealed class ScaleSlider
    {
        public float Draw(float currentScale) => currentScale;
    }

    public sealed class AlphaSlider
    {
        public float Draw(float currentAlpha) => currentAlpha;
    }

    public sealed class ColliderConfigPanel
    {
        public void Draw(CollisionPreset preset)
        {
            EditorGUILayout.LabelField($"Collider: {preset.colliderShape}", EditorStyles.miniLabel);
        }
    }

    public sealed class ScenePlacementController
    {
        public void EnablePlacement(AssetPackEntry selectedEntry) { }
        public void DisablePlacement() { }
        public void OnSceneGUI(SceneView sceneView) { }
    }

    public sealed class GhostPreviewRenderer
    {
        public void DrawGhost(AssetPackEntry selectedEntry, Vector3 worldPosition, bool isValid) { }
    }

    public sealed class PlacementValidator
    {
        public PlacementValidationResult Validate(AssetPackEntry selectedEntry, Vector3 worldPosition)
        {
            return selectedEntry == null || selectedEntry.sprite == null ? PlacementValidationResult.MissingSprite : PlacementValidationResult.Valid;
        }
    }

    public sealed class ClickToPlaceHandler
    {
        public GameObject Place(AssetPackEntry selectedEntry, Vector3 worldPosition) => null;
    }

    public sealed class UndoRedoManager
    {
        public void BeginPlacementGroup(string label) { }
        public void RegisterCreatedObject(GameObject placedObject, string label) { }
        public void EndPlacementGroup() { }
    }

    public sealed class AssetPackCatalog
    {
        private readonly AssetPackManifestLoader loader;
        private readonly List<AssetPackManifestSO> packs = new List<AssetPackManifestSO>();
        private IReadOnlyList<AssetPackManifestSO> testPacks;

        public IReadOnlyList<AssetPackManifestSO> Packs => packs;

        public AssetPackCatalog()
            : this(new AssetPackManifestLoader())
        {
        }

        public AssetPackCatalog(AssetPackManifestLoader loader)
        {
            this.loader = loader;
            Refresh();
        }

        public AssetPackCatalog(IReadOnlyList<AssetPackManifestSO> packsForTests)
        {
            loader = null;
            testPacks = packsForTests ?? Array.Empty<AssetPackManifestSO>();
            Refresh();
        }

        public void Refresh()
        {
            packs.Clear();
            IReadOnlyList<AssetPackManifestSO> source = testPacks ?? loader.LoadAllManifests();
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] != null)
                {
                    packs.Add(source[i]);
                }
            }
        }

        public bool ContainsPack(AssetPackManifestSO pack)
        {
            return pack != null && packs.Contains(pack);
        }

        public IReadOnlyList<AssetPackCategory> GetCategories(AssetPackManifestSO pack)
        {
            return pack != null && pack.categories != null ? pack.categories : Array.Empty<AssetPackCategory>();
        }

        public IReadOnlyList<AssetPackEntry> Query(AssetPackManifestSO pack, string categoryId, string searchQuery)
        {
            if (pack == null)
            {
                return Array.Empty<AssetPackEntry>();
            }

            var entries = BuildEntries(pack);
            string normalizedSearch = (searchQuery ?? string.Empty).Trim();
            bool filterCategory = !string.IsNullOrEmpty(categoryId) && !string.Equals(categoryId, "__all__", StringComparison.OrdinalIgnoreCase);
            bool filterSearch = !string.IsNullOrEmpty(normalizedSearch);
            if (!filterCategory && !filterSearch)
            {
                return entries;
            }

            var filtered = new List<AssetPackEntry>();
            for (int i = 0; i < entries.Count; i++)
            {
                AssetPackEntry entry = entries[i];
                if (filterCategory && !string.Equals(entry.categoryName, categoryId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (filterSearch && !ContainsIgnoreCase(entry.displayName, normalizedSearch) && !ContainsIgnoreCase(entry.sourceAtlas, normalizedSearch))
                {
                    continue;
                }

                filtered.Add(entry);
            }

            return filtered;
        }

        public int CountProductionSprites(AssetPackManifestSO pack)
        {
            IReadOnlyList<AssetPackEntry> entries = Query(pack, "__all__", string.Empty);
            int count = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                if (!entries[i].isPreviewOnly && entries[i].sprite != null)
                {
                    count++;
                }
            }

            return count;
        }

        public IReadOnlyList<AssetPackEntry> BuildEntries(AssetPackManifestSO pack)
        {
            var entries = new List<AssetPackEntry>();
            if (pack == null)
            {
                return entries;
            }

            var usedAtlases = new HashSet<PatchAtlasSO>();
            IReadOnlyList<AssetPackCategory> categories = GetCategories(pack);
            for (int c = 0; c < categories.Count; c++)
            {
                AssetPackCategory category = categories[c];
                if (string.IsNullOrEmpty(category.categoryName)) continue;
                for (int a = 0; pack.atlases != null && a < pack.atlases.Count; a++)
                {
                    PatchAtlasSO atlas = pack.atlases[a];
                    if (atlas == null || !AtlasMatchesCategory(atlas, category)) continue;
                    AppendAtlasEntries(entries, pack, atlas, category.categoryName);
                    usedAtlases.Add(atlas);
                }
            }

            for (int a = 0; pack.atlases != null && a < pack.atlases.Count; a++)
            {
                PatchAtlasSO atlas = pack.atlases[a];
                if (atlas == null || usedAtlases.Contains(atlas)) continue;
                AppendAtlasEntries(entries, pack, atlas, CategoryFromRole(atlas.role));
            }

            for (int p = 0; pack.props != null && p < pack.props.Count; p++)
            {
                PropDefinitionSO prop = pack.props[p];
                if (prop == null) continue;
                entries.Add(BuildPropEntry(pack, prop));
            }

            return entries;
        }

        private static void AppendAtlasEntries(List<AssetPackEntry> entries, AssetPackManifestSO pack, PatchAtlasSO atlas, string categoryName)
        {
            if (atlas.variants == null)
            {
                return;
            }

            for (int i = 0; i < atlas.variants.Length; i++)
            {
                entries.Add(BuildSpriteEntry(pack, atlas, atlas.variants[i], i, categoryName));
            }
        }

        private static AssetPackEntry BuildSpriteEntry(AssetPackManifestSO pack, PatchAtlasSO atlas, Sprite sprite, int index, string categoryName)
        {
            string packId = string.IsNullOrEmpty(pack.packId) ? pack.name : pack.packId;
            string atlasName = !string.IsNullOrEmpty(atlas.atlasId) ? atlas.atlasId : atlas.name;
            string spriteName = sprite != null ? sprite.name : $"{atlasName}_{index:00}_Missing";
            bool preview = IsPreviewName(spriteName);
            Vector2Int size = sprite != null ? new Vector2Int(Mathf.RoundToInt(sprite.rect.width), Mathf.RoundToInt(sprite.rect.height)) : Vector2Int.zero;
            int ppu = sprite != null ? Mathf.RoundToInt(sprite.pixelsPerUnit) : 0;

            return new AssetPackEntry
            {
                entryId = $"{packId}/{atlasName}/{index}",
                displayName = spriteName,
                categoryName = categoryName,
                sprite = sprite,
                sourceObject = atlas,
                sourcePack = string.IsNullOrEmpty(pack.displayName) ? pack.name : pack.displayName,
                sourceAtlas = atlasName,
                sourcePath = AssetDatabase.GetAssetPath(atlas),
                pixelSize = size,
                pixelsPerUnit = ppu,
                defaultSortingOrder = DefaultSortingOrder(categoryName),
                defaultBlocksMovement = DefaultBlocksMovement(categoryName),
                collisionPreset = CollisionPreset.ForCategory(categoryName, spriteName),
                isPreviewOnly = preview,
                isMissing = sprite == null
            };
        }

        private static AssetPackEntry BuildPropEntry(AssetPackManifestSO pack, PropDefinitionSO prop)
        {
            Sprite sprite = prop.visual;
            string packId = string.IsNullOrEmpty(pack.packId) ? pack.name : pack.packId;
            string propName = string.IsNullOrEmpty(prop.propId) ? prop.name : prop.propId;
            Vector2Int size = sprite != null ? new Vector2Int(Mathf.RoundToInt(sprite.rect.width), Mathf.RoundToInt(sprite.rect.height)) : Vector2Int.zero;
            int ppu = sprite != null ? Mathf.RoundToInt(sprite.pixelsPerUnit) : 0;
            const string categoryName = "VerticalProps";
            CollisionPreset collisionPreset = CollisionPreset.ForProp(prop, categoryName);

            return new AssetPackEntry
            {
                entryId = $"{packId}/prop/{propName}",
                displayName = propName,
                categoryName = categoryName,
                sprite = sprite,
                sourceObject = prop,
                sourcePack = string.IsNullOrEmpty(pack.displayName) ? pack.name : pack.displayName,
                sourceAtlas = "PropDefinition",
                sourcePath = AssetDatabase.GetAssetPath(prop),
                pixelSize = size,
                pixelsPerUnit = ppu,
                defaultSortingOrder = DefaultSortingOrder(categoryName),
                defaultBlocksMovement = collisionPreset.blocksMovement,
                collisionPreset = collisionPreset,
                isPreviewOnly = IsPreviewName(propName),
                isMissing = sprite == null
            };
        }

        private static bool AtlasMatchesCategory(PatchAtlasSO atlas, AssetPackCategory category)
        {
            if (atlas == null) return false;
            string assetPath = AssetDatabase.GetAssetPath(atlas);
            string fileName = string.IsNullOrEmpty(assetPath) ? string.Empty : Path.GetFileNameWithoutExtension(assetPath);
            string[] keys =
            {
                atlas.name,
                atlas.atlasId,
                fileName,
                CategoryFromRole(atlas.role)
            };

            if (category.atlasNames != null)
            {
                for (int i = 0; i < category.atlasNames.Count; i++)
                {
                    string atlasRef = category.atlasNames[i];
                    for (int k = 0; k < keys.Length; k++)
                    {
                        if (EqualsIgnoreCase(atlasRef, keys[k]))
                        {
                            return true;
                        }
                    }
                }
            }

            return EqualsIgnoreCase(category.categoryName, CategoryFromRole(atlas.role));
        }

        private static string CategoryFromRole(PatchRole role)
        {
            switch (role)
            {
                case PatchRole.BaseFloor: return "BaseFloor";
                case PatchRole.MacroPatch: return "Macro";
                case PatchRole.OrganicDecal: return "OrganicDecal";
                case PatchRole.DetailScatter: return "DetailScatter";
                case PatchRole.Accent: return "Accent";
                default: return "Unknown";
            }
        }

        private static int DefaultSortingOrder(string categoryName)
        {
            if (ContainsIgnoreCase(categoryName, "Wall")) return 20;
            if (ContainsIgnoreCase(categoryName, "Vertical")) return 30;
            if (ContainsIgnoreCase(categoryName, "Atmospheric")) return 15;
            if (ContainsIgnoreCase(categoryName, "Accent")) return 12;
            if (ContainsIgnoreCase(categoryName, "Scatter")) return 8;
            if (ContainsIgnoreCase(categoryName, "Decal")) return 6;
            if (ContainsIgnoreCase(categoryName, "Floor")) return 0;
            return 0;
        }

        private static bool DefaultBlocksMovement(string categoryName)
        {
            return CollisionPreset.ForCategory(categoryName).blocksMovement;
        }

        private static bool IsPreviewName(string name)
        {
            return ContainsIgnoreCase(name, "preview") || ContainsIgnoreCase(name, "contact_sheet");
        }

        private static bool ContainsIgnoreCase(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(search) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool EqualsIgnoreCase(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed class AssetPackManifestLoader
    {
        public IReadOnlyList<AssetPackManifestSO> LoadAllManifests()
        {
            var manifests = new List<AssetPackManifestSO>();
            string[] guids = AssetDatabase.FindAssets("t:AssetPackManifestSO");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AssetPackManifestSO manifest = AssetDatabase.LoadAssetAtPath<AssetPackManifestSO>(path);
                if (manifest != null)
                {
                    manifests.Add(manifest);
                }
            }

            manifests.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
            return manifests;
        }
    }

    public sealed class LegacyBrushPackAdapter
    {
        public IReadOnlyList<AssetPackEntry> BuildEntries(UnityEngine.Object brushPackOrPool) => Array.Empty<AssetPackEntry>();
    }

    public sealed class PatchAtlasAdapter
    {
        public IReadOnlyList<AssetPackEntry> BuildEntries(UnityEngine.Object patchAtlas) => Array.Empty<AssetPackEntry>();
    }

    public sealed class PropDefinitionAdapter
    {
        public AssetPackEntry BuildEntry(UnityEngine.Object propDefinition) => null;
    }

    [Serializable]
    public sealed class AssetPackEntry
    {
        public string entryId;
        public string displayName;
        public string categoryName;
        public Sprite sprite;
        public UnityEngine.Object sourceObject;
        public string sourcePack;
        public string sourceAtlas;
        public string sourcePath;
        public Vector2Int pixelSize;
        public int pixelsPerUnit;
        public int defaultSortingOrder;
        public bool defaultBlocksMovement;
        public CollisionPreset collisionPreset;
        public bool isPreviewOnly;
        public bool isMissing;
    }

    [Serializable]
    public struct CollisionPreset
    {
        public bool blocksMovement;
        public ColliderShape colliderShape;
        public float colliderFootprintRatio;
        public Vector2 colliderOffset;
        public bool isTrigger;
        public string colliderLayer;

        public static CollisionPreset ForCategory(string categoryName)
        {
            return ForCategory(categoryName, string.Empty);
        }

        public static CollisionPreset ForCategory(string categoryName, string assetName)
        {
            bool walls = !string.IsNullOrEmpty(categoryName) && categoryName.IndexOf("Wall", StringComparison.OrdinalIgnoreCase) >= 0;
            bool vertical = !string.IsNullOrEmpty(categoryName) && categoryName.IndexOf("VerticalProp", StringComparison.OrdinalIgnoreCase) >= 0;
            bool roundVertical = vertical && (
                ContainsIgnoreCase(assetName, "brazier") ||
                ContainsIgnoreCase(assetName, "barrel") ||
                ContainsIgnoreCase(assetName, "round"));
            return new CollisionPreset
            {
                blocksMovement = walls || vertical,
                colliderShape = walls ? ColliderShape.Box : (roundVertical ? ColliderShape.Circle : (vertical ? ColliderShape.Box : ColliderShape.None)),
                colliderFootprintRatio = walls ? 1f : (vertical ? 0.6f : 0f),
                colliderOffset = Vector2.zero,
                isTrigger = false,
                colliderLayer = walls || vertical ? "Walls" : string.Empty
            };
        }

        public static CollisionPreset ForProp(PropDefinitionSO prop, string categoryName)
        {
            if (prop == null)
            {
                return ForCategory(categoryName);
            }

            bool blocks = prop.blocksMovement || prop.hasCollision;
            ColliderShape shape = FromPropShape(prop.colliderShape);
            if (blocks && shape == ColliderShape.None && prop.hasCollision)
            {
                shape = ColliderShape.Box;
            }

            return new CollisionPreset
            {
                blocksMovement = blocks,
                colliderShape = blocks ? shape : ColliderShape.None,
                colliderFootprintRatio = blocks ? Mathf.Clamp(prop.colliderFootprintRatio, 0.3f, 1f) : 0f,
                colliderOffset = prop.colliderOffset,
                isTrigger = prop.isTrigger,
                colliderLayer = blocks ? prop.colliderLayer : string.Empty
            };
        }

        private static ColliderShape FromPropShape(PropDefinitionSO.ColliderShape shape)
        {
            switch (shape)
            {
                case PropDefinitionSO.ColliderShape.Box: return ColliderShape.Box;
                case PropDefinitionSO.ColliderShape.Circle: return ColliderShape.Circle;
                case PropDefinitionSO.ColliderShape.Capsule: return ColliderShape.Capsule;
                case PropDefinitionSO.ColliderShape.PolygonAutoTrace: return ColliderShape.PolygonAutoTrace;
                default: return ColliderShape.None;
            }
        }

        private static bool ContainsIgnoreCase(string value, string search)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(search) && value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    public enum ColliderShape
    {
        None,
        Box,
        Circle,
        Capsule,
        PolygonAutoTrace
    }

    public enum PlacementValidationResult
    {
        Unknown,
        Valid,
        MissingSprite,
        OverlapsBlockingGeometry,
        MissingLayer,
        InvalidColliderConfig
    }
}
#endif
