using System;
using System.Collections.Generic;
using System.Globalization;
using LaurethStudio.PainterSuite.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RoomPainterScenePlacer
    {
        private static readonly Color CliffGhostTint = new Color(0.4f, 0.9f, 1f, 0.6f);
        private static readonly Color ParallaxGhostTint = new Color(0.8f, 0.5f, 1f, 0.6f);

        private readonly HashSet<Vector3Int> _paintedCells = new HashSet<Vector3Int>();
        private bool _isPainting;
        private int _undoGroup;
        // S110 Sonnet Day 3 review HIGH #3 fix — R rotate. 90deg increments.
        private float _rotationDegrees;

        public void Reset()
        {
            _isPainting = false;
            _paintedCells.Clear();
        }

        public void OnSceneGUI(
            SceneView sceneView,
            AssetEntry selectedAsset,
            RoomLayer targetLayer,
            string parallaxTierName,
            float parallaxTierValue,
            int parallaxTierIndex,
            string activeFilter,
            IReadOnlyList<AssetEntry> palette,
            Action<AssetEntry> selectAsset)
        {
            Event e = Event.current;
            HandleVariantCycle(e, activeFilter, palette, selectedAsset, selectAsset, sceneView);

            if (!HasSelection(selectedAsset))
            {
                Reset();
                return;
            }

            Grid grid = FindActiveGrid();
            Vector3 mouseWorld = MouseWorld(e);
            Vector3Int cellPos;
            Vector3 snappedWorld;
            bool hasGrid = SnapToCell(grid, mouseWorld, out cellPos, out snappedWorld);

            if (e.type == EventType.Repaint)
            {
                DrawGhost(selectedAsset, targetLayer, parallaxTierName, parallaxTierValue, mouseWorld, snappedWorld, hasGrid);
            }

            if (!hasGrid)
            {
                Reset();
                return;
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            switch (e.type)
            {
                case EventType.KeyDown when e.keyCode == KeyCode.R:
                    _rotationDegrees = (_rotationDegrees + 90f) % 360f;
                    e.Use();
                    sceneView.Repaint();
                    break;

                case EventType.MouseDown when e.button == 0 && !e.alt:
                    BeginUndoGroup("Paint " + targetLayer);
                    PaintCell(grid, cellPos, snappedWorld, selectedAsset, targetLayer, parallaxTierName, parallaxTierValue, parallaxTierIndex);
                    e.Use();
                    sceneView.Repaint();
                    break;

                case EventType.MouseDrag when _isPainting:
                    PaintCell(grid, cellPos, snappedWorld, selectedAsset, targetLayer, parallaxTierName, parallaxTierValue, parallaxTierIndex);
                    e.Use();
                    sceneView.Repaint();
                    break;

                case EventType.MouseUp when _isPainting && e.button == 0:
                    _isPainting = false;
                    Undo.CollapseUndoOperations(_undoGroup);
                    e.Use();
                    sceneView.Repaint();
                    break;
            }
        }

        private void BeginUndoGroup(string undoName)
        {
            _isPainting = true;
            _paintedCells.Clear();
            Undo.IncrementCurrentGroup();
            _undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName(undoName);
        }

        private void PaintCell(
            Grid grid,
            Vector3Int cellPos,
            Vector3 snappedWorld,
            AssetEntry selectedAsset,
            RoomLayer targetLayer,
            string parallaxTierName,
            float parallaxTierValue,
            int parallaxTierIndex)
        {
            if (_paintedCells.Contains(cellPos))
            {
                return;
            }

            GameObject go = CreatePaintedObject(selectedAsset);
            if (go == null)
            {
                return;
            }

            _paintedCells.Add(cellPos);
            string undoName = "Paint " + targetLayer;
            Undo.RegisterCreatedObjectUndo(go, undoName);
            go.transform.SetParent(grid.transform, true);
            go.transform.position = snappedWorld;
            // S110 Sonnet Day 3 review HIGH #3 fix — apply current rotation (R key cycles 90deg).
            go.transform.rotation = Quaternion.Euler(0f, 0f, _rotationDegrees);

            RoomPainterAsset metadata = selectedAsset.metadata;
            RoomLayer effectiveLayer = metadata != null ? metadata.defaultLayer : targetLayer;

            if (targetLayer == RoomLayer.Parallax || effectiveLayer == RoomLayer.Parallax)
            {
                go.name = "Parallax_" + GetAssetName(selectedAsset) + "_" + parallaxTierName;
                string sortingLayer = metadata != null && !string.IsNullOrEmpty(metadata.defaultSortingLayer) ? metadata.defaultSortingLayer : "Default";
                int sortingOrder = metadata != null ? metadata.defaultOrder : -100 - parallaxTierIndex;
                ConfigureSorting(go, sortingLayer, sortingOrder);
                ParallaxLayer parallaxLayer = go.GetComponent<ParallaxLayer>();
                if (parallaxLayer == null)
                {
                    parallaxLayer = go.AddComponent<ParallaxLayer>();
                }

                parallaxLayer.factor = new Vector2(parallaxTierValue, parallaxTierValue);
                EditorUtility.SetDirty(parallaxLayer);
            }
            else
            {
                go.name = "Cliff_" + GetAssetName(selectedAsset);
                string sortingLayer = metadata != null && !string.IsNullOrEmpty(metadata.defaultSortingLayer) ? metadata.defaultSortingLayer : "Floor";
                int sortingOrder = metadata != null ? metadata.defaultOrder : 5;
                ConfigureSorting(go, sortingLayer, sortingOrder);
            }

            ApplyMetadata(go, selectedAsset);
            EditorUtility.SetDirty(go);
            EditorUtility.SetDirty(grid.gameObject);
            EditorSceneManager.MarkSceneDirty(go.scene);
        }

        private static GameObject CreatePaintedObject(AssetEntry selectedAsset)
        {
            if (selectedAsset.prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(selectedAsset.prefab) as GameObject;
                if (instance != null)
                {
                    return instance;
                }

                return UnityEngine.Object.Instantiate(selectedAsset.prefab);
            }

            if (selectedAsset.sprite == null)
            {
                return null;
            }

            GameObject go = new GameObject();
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = selectedAsset.sprite;
            return go;
        }

        private static void ConfigureSorting(GameObject go, string sortingLayer, int sortingOrder)
        {
            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sortingLayerName = sortingLayer;
                renderers[i].sortingOrder = sortingOrder;
                EditorUtility.SetDirty(renderers[i]);
            }
        }

        private static void ApplyMetadata(GameObject go, AssetEntry selectedAsset)
        {
            UnityEngine.Object source = selectedAsset.sprite != null ? selectedAsset.sprite : (UnityEngine.Object)selectedAsset.prefab;
            string assetGuid = source != null ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(source)) : string.Empty;

            RoomPainterAssetBinding binding = go.GetComponent<RoomPainterAssetBinding>();
            if (binding == null)
            {
                binding = Undo.AddComponent<RoomPainterAssetBinding>(go);
            }

            binding.assetGuid = assetGuid;
            EditorUtility.SetDirty(binding);

            RoomPainterAsset metadata = selectedAsset.metadata;
            if (metadata == null)
            {
                return;
            }

            go.transform.localScale = new Vector3(metadata.defaultScale.x, metadata.defaultScale.y, go.transform.localScale.z);
            go.transform.position += (Vector3)metadata.defaultVisualOffset;

            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].color = metadata.tint;
                if (!string.IsNullOrEmpty(metadata.materialOverridePath))
                {
                    Material material = AssetDatabase.LoadAssetAtPath<Material>(metadata.materialOverridePath);
                    if (material != null)
                    {
                        renderers[i].sharedMaterial = material;
                    }
                }

                renderers[i].shadowCastingMode = metadata.castShadow
                    ? UnityEngine.Rendering.ShadowCastingMode.On
                    : UnityEngine.Rendering.ShadowCastingMode.Off;
                renderers[i].receiveShadows = metadata.receiveLight;
                EditorUtility.SetDirty(renderers[i]);
            }

            if (metadata.isBlock || metadata.isTrigger)
            {
                RoomPainterPhysicsApplier.Apply(go, metadata);
            }
        }

        private static void DrawGhost(
            AssetEntry selectedAsset,
            RoomLayer targetLayer,
            string parallaxTierName,
            float parallaxTierValue,
            Vector3 mouseWorld,
            Vector3 snappedWorld,
            bool hasGrid)
        {
            if (!hasGrid)
            {
                Handles.Label(mouseWorld, "Room Painter: no active Grid in scene");
                return;
            }

            Color tint = targetLayer == RoomLayer.Parallax ? ParallaxGhostTint : CliffGhostTint;
            Sprite sprite = GetPreviewSprite(selectedAsset);
            DrawSpriteGhost(sprite, snappedWorld, tint);

            Handles.color = tint;
            Handles.DrawWireCube(snappedWorld, new Vector3(1f, 0.609f, 0f));

            string label = targetLayer == RoomLayer.Parallax
                ? "[Parallax (" + parallaxTierName + " " + parallaxTierValue.ToString("0.00", CultureInfo.InvariantCulture) + ")]"
                : "[Cliff]";
            Handles.Label(snappedWorld + Vector3.up * 0.45f, label);
        }

        private static void DrawSpriteGhost(Sprite sprite, Vector3 worldPos, Color tint)
        {
            if (sprite == null || sprite.texture == null)
            {
                return;
            }

            Vector2 center = HandleUtility.WorldToGUIPoint(worldPos);
            Vector2 top = HandleUtility.WorldToGUIPoint(worldPos + Vector3.up * (sprite.bounds.size.y * 0.5f));
            Vector2 bottom = HandleUtility.WorldToGUIPoint(worldPos - Vector3.up * (sprite.bounds.size.y * 0.5f));
            float height = Mathf.Max(24f, Mathf.Abs(bottom.y - top.y));
            float aspect = sprite.bounds.size.y > Mathf.Epsilon ? sprite.bounds.size.x / sprite.bounds.size.y : 1f;
            float width = Mathf.Max(24f, height * aspect);
            Rect drawRect = new Rect(center.x - width * 0.5f, center.y - height * 0.5f, width, height);
            Rect textureRect = sprite.textureRect;
            Rect texCoords = new Rect(
                textureRect.x / sprite.texture.width,
                textureRect.y / sprite.texture.height,
                textureRect.width / sprite.texture.width,
                textureRect.height / sprite.texture.height);

            Handles.BeginGUI();
            Color previousColor = GUI.color;
            GUI.color = tint;
            GUI.DrawTextureWithTexCoords(drawRect, sprite.texture, texCoords, true);
            GUI.color = previousColor;
            Handles.EndGUI();
        }

        private static void HandleVariantCycle(
            Event e,
            string activeFilter,
            IReadOnlyList<AssetEntry> palette,
            AssetEntry selectedAsset,
            Action<AssetEntry> selectAsset,
            SceneView sceneView)
        {
            if (e.type != EventType.ScrollWheel || activeFilter != RoomLayer.Cliff.ToString() || palette == null || palette.Count == 0)
            {
                return;
            }

            int currentIndex = -1;
            int firstCliffIndex = -1;
            for (int i = 0; i < palette.Count; i++)
            {
                if (palette[i].suggestedLayer != RoomLayer.Cliff)
                {
                    continue;
                }

                if (firstCliffIndex < 0)
                {
                    firstCliffIndex = i;
                }

                if (palette[i].path == selectedAsset.path)
                {
                    currentIndex = i;
                    break;
                }
            }

            if (firstCliffIndex < 0)
            {
                return;
            }

            int nextIndex = firstCliffIndex;
            if (currentIndex >= 0)
            {
                nextIndex = FindNextCliffIndex(palette, currentIndex);
            }

            selectAsset?.Invoke(palette[nextIndex]);
            e.Use();
            sceneView.Repaint();
        }

        private static int FindNextCliffIndex(IReadOnlyList<AssetEntry> palette, int currentIndex)
        {
            for (int offset = 1; offset <= palette.Count; offset++)
            {
                int candidateIndex = (currentIndex + offset) % palette.Count;
                if (palette[candidateIndex].suggestedLayer == RoomLayer.Cliff)
                {
                    return candidateIndex;
                }
            }

            return currentIndex;
        }

        private static bool SnapToCell(Grid grid, Vector3 worldPos, out Vector3Int cellPos, out Vector3 snappedWorld)
        {
            cellPos = default;
            snappedWorld = worldPos;

            if (grid == null)
            {
                return false;
            }

            // Iso snapping relies on Grid.WorldToCell and Grid.GetCellCenterWorld for the active Grid layout.
            cellPos = grid.WorldToCell(worldPos);
            snappedWorld = grid.GetCellCenterWorld(cellPos);
            return true;
        }

        private static Grid FindActiveGrid()
        {
#if UNITY_2022_2_OR_NEWER
            return UnityEngine.Object.FindAnyObjectByType<Grid>();
#else
            return UnityEngine.Object.FindObjectOfType<Grid>();
#endif
        }

        private static Vector3 MouseWorld(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Mathf.Abs(ray.direction.z) > Mathf.Epsilon)
            {
                float t = -ray.origin.z / ray.direction.z;
                return ray.origin + ray.direction * t;
            }

            return ray.origin;
        }

        private static Sprite GetPreviewSprite(AssetEntry selectedAsset)
        {
            if (selectedAsset.sprite != null)
            {
                return selectedAsset.sprite;
            }

            if (selectedAsset.prefab == null)
            {
                return null;
            }

            SpriteRenderer renderer = selectedAsset.prefab.GetComponentInChildren<SpriteRenderer>();
            return renderer != null ? renderer.sprite : null;
        }

        private static string GetAssetName(AssetEntry selectedAsset)
        {
            UnityEngine.Object assetObject = selectedAsset.AssetObject;
            return assetObject != null ? assetObject.name : "Asset";
        }

        private static bool HasSelection(AssetEntry selectedAsset)
        {
            return !string.IsNullOrEmpty(selectedAsset.path) && (selectedAsset.sprite != null || selectedAsset.prefab != null);
        }
    }
}
