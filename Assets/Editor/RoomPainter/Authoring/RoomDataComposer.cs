using System.Collections.Generic;
using RIMA.DevTools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RoomDataComposer
    {
        public const string PreviewRootName = "[RoomPreview_Generated]";
        private const string PlaytestRootName = "[RoomPlaytest_Generated]";

        public Transform PreviewRoot { get; private set; }
        public Grid PreviewGrid { get; private set; }

        public Transform Compose(RoomData room)
        {
            return ComposeInto(room, null, PreviewRootName);
        }

        public Transform ComposeIntoPlaytestRoot(RoomData room)
        {
            return ComposeInto(room, null, PlaytestRootName);
        }

        public void ClearPreview()
        {
            if (PreviewRoot != null)
            {
                ClearChildren(PreviewRoot);
            }
        }

        public Transform ComposeInto(RoomData room, Transform parent, string rootName)
        {
            if (room == null)
            {
                return null;
            }

            room.EnsureDefaults();
            Transform root = ResolveRoot(parent, rootName);
            Grid grid = root.GetComponent<Grid>();
            if (grid == null)
            {
                grid = root.gameObject.AddComponent<Grid>();
            }
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            // Iso cellSize must match the floor tile's diamond W:H ratio or rows gap vertically.
            // RIMA 64px PixelLab iso tiles measure ~62x38px diamond -> (0.96, 0.585). Using 0.94 for
            // both axes (the old value) left large vertical gaps because the diamond is wider than tall.
            grid.cellSize = new Vector3(0.96f, 0.585f, 1f);

            PreviewRoot = rootName == PreviewRootName ? root : PreviewRoot;
            PreviewGrid = rootName == PreviewRootName ? grid : PreviewGrid;

            ClearChildren(root);

            Transform floorParent = CreateGroup(root, "Floor");
            Transform cliffParent = CreateGroup(root, "Cliff");
            Transform wallParent = CreateGroup(root, "Walls");
            Transform propParent = CreateGroup(root, "Props");

            ComposeTileCells(room.floorCells, grid, floorParent, RoomLayer.Floor);
            ComposeTileCells(room.cliffCells, grid, cliffParent, RoomLayer.Cliff);
            RoomDataMutator.MigrateSegmentsToCells(room);
            if (room.wallCells != null && room.wallCells.Count > 0)
            {
                ComposeWallCells(room.wallCells, grid, wallParent);
            }
            else
            {
                ComposeWallSegments(room.wallSegments, grid, wallParent);
            }
            ComposeProps(room.propPlacements, grid, propParent);

            EditorSceneManager.MarkSceneDirty(root.gameObject.scene);
            return root;
        }

        public void FocusTopDown()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null || PreviewRoot == null)
            {
                return;
            }

            sceneView.in2DMode = true;
            sceneView.orthographic = true;
            sceneView.LookAt(GetRootCenter(PreviewRoot), Quaternion.identity, 10f);
            sceneView.Repaint();
        }

        private static Transform ResolveRoot(Transform parent, string rootName)
        {
            Transform root = parent != null ? parent.Find(rootName) : null;
            if (root == null)
            {
                GameObject existing = parent == null ? GameObject.Find(rootName) : null;
                if (existing != null)
                {
                    root = existing.transform;
                }
            }

            if (root != null)
            {
                return root;
            }

            GameObject created = new GameObject(rootName);
            if (parent != null)
            {
                created.transform.SetParent(parent, false);
            }

            return created.transform;
        }

        private static Transform CreateGroup(Transform root, string name)
        {
            GameObject group = new GameObject(name);
            group.transform.SetParent(root, false);
            return group.transform;
        }

        private static void ClearChildren(Transform root)
        {
            if (root == null)
            {
                return;
            }

            for (int i = root.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(root.GetChild(i).gameObject);
            }
        }

        private static void ComposeTileCells(
            List<RoomData.TileCellRecord> cells,
            Grid grid,
            Transform parent,
            RoomLayer layer)
        {
            if (cells == null)
            {
                return;
            }

            Tilemap tilemap = null;
            for (int i = 0; i < cells.Count; i++)
            {
                RoomData.TileCellRecord cell = cells[i];
                string path = ResolveAssetPath(cell.assetGuidOrName);
                if (IsTilemapLayer(layer) && TryComposeTileBaseCell(path, parent, layer, cell, ref tilemap))
                {
                    continue;
                }

                Vector3 position = cell.worldPos == Vector3.zero ? grid.GetCellCenterWorld(cell.cell) : cell.worldPos;
                Vector2 scale = cell.scale == Vector2.zero ? Vector2.one : cell.scale;
                CreateVisual(path, cell.assetGuidOrName, parent, position, cell.rotation, scale, layer);
            }
        }

        private static void ComposeWallSegments(List<WallSegment> segments, Grid grid, Transform parent)
        {
            if (segments == null)
            {
                return;
            }

            HashSet<Vector3Int> occupied = new HashSet<Vector3Int>();
            for (int i = 0; i < segments.Count; i++)
            {
                WallSegment segment = segments[i];
                switch (segment.kind)
                {
                    case SegmentKind.SolidWall:
                        WallRunBuilder.BuildRun(grid, segment.fromCell, segment.toCell, segment.piece, parent, occupied);
                        break;
                    case SegmentKind.Anchor:
                    case SegmentKind.Entrance:
                        WallRunBuilder.PlaceOne(grid, segment.fromCell, segment.piece, parent);
                        break;
                }
            }
        }

        private static void ComposeWallCells(List<WallCell> wallCells, Grid grid, Transform parent)
        {
            if (wallCells == null)
            {
                return;
            }

            for (int i = 0; i < wallCells.Count; i++)
            {
                WallCell wallCell = wallCells[i];
                WallPiece piece = ResolveWallPiece(wallCell.pieceId);
                WallRunBuilder.PlaceOne(grid, wallCell.cell, piece, parent, wallCell.shape, wallCell.rotation);
            }
        }

        private static void ComposeProps(List<RoomData.PropPlacement> placements, Grid grid, Transform parent)
        {
            if (placements == null)
            {
                return;
            }

            for (int i = 0; i < placements.Count; i++)
            {
                RoomData.PropPlacement placement = placements[i];
                Vector3 position = placement.position == Vector3.zero
                    ? grid.GetCellCenterWorld(placement.cell)
                    : placement.position;
                Vector2 scale = placement.scale == Vector2.zero ? Vector2.one : placement.scale;
                CreateVisual(placement.assetGuidOrName, parent, position, placement.rotation, scale, placement.layer);
            }
        }

        private static GameObject CreateVisual(
            string assetGuidOrName,
            Transform parent,
            Vector3 position,
            float rotation,
            Vector2 scale,
            RoomLayer layer)
        {
            string path = ResolveAssetPath(assetGuidOrName);
            return CreateVisual(path, assetGuidOrName, parent, position, rotation, scale, layer);
        }

        private static GameObject CreateVisual(
            string path,
            string assetGuidOrName,
            Transform parent,
            Vector3 position,
            float rotation,
            Vector2 scale,
            RoomLayer layer)
        {
            GameObject go = InstantiateAsset(path, parent);
            if (go == null)
            {
                return null;
            }

            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            go.transform.localScale = new Vector3(scale.x, scale.y, go.transform.localScale.z);

            string guid = string.IsNullOrEmpty(path) ? assetGuidOrName : AssetDatabase.AssetPathToGUID(path);
            ApplyMetadata(go, path, guid, layer);
            return go;
        }

        private static bool IsTilemapLayer(RoomLayer layer)
        {
            return layer == RoomLayer.Floor || layer == RoomLayer.Cliff;
        }

        private static bool TryComposeTileBaseCell(
            string path,
            Transform parent,
            RoomLayer layer,
            RoomData.TileCellRecord cell,
            ref Tilemap tilemap)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile == null)
            {
                return false;
            }

            if (tilemap == null)
            {
                tilemap = EnsureLayerTilemap(parent, layer);
            }

            tilemap.SetTile(cell.cell, tile);
            tilemap.SetTileFlags(cell.cell, TileFlags.None);
            Vector2 scale = cell.scale == Vector2.zero ? Vector2.one : cell.scale;
            tilemap.SetTransformMatrix(
                cell.cell,
                Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, cell.rotation), new Vector3(scale.x, scale.y, 1f)));
            return true;
        }

        private static Tilemap EnsureLayerTilemap(Transform parent, RoomLayer layer)
        {
            Tilemap tilemap = parent.GetComponent<Tilemap>();
            if (tilemap == null)
            {
                tilemap = parent.gameObject.AddComponent<Tilemap>();
            }

            tilemap.ClearAllTiles();

            TilemapRenderer renderer = parent.GetComponent<TilemapRenderer>();
            if (renderer == null)
            {
                renderer = parent.gameObject.AddComponent<TilemapRenderer>();
            }

            RoomDepthStack.DepthSlot slot = RoomDepthStack.SlotFor(layer);
            renderer.sortingLayerName = slot.sortingLayer;
            renderer.sortingOrder = slot.sortingOrder;
            return tilemap;
        }

        private static GameObject InstantiateAsset(string path, Transform parent)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
                if (instance != null)
                {
                    return instance;
                }
            }

            Sprite sprite = LoadSprite(path);
            if (sprite == null)
            {
                return null;
            }

            GameObject go = new GameObject(sprite.name);
            go.transform.SetParent(parent, false);
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;
            return go;
        }

        private static void ApplyMetadata(GameObject go, string path, string guid, RoomLayer layer)
        {
            if (go == null)
            {
                return;
            }

            RoomPainterAsset metadata = string.IsNullOrEmpty(path)
                ? null
                : RoomPainterAssetPostprocessor.LoadMetadataForAssetPath(path);

            string sortingLayer = metadata != null && !string.IsNullOrEmpty(metadata.defaultSortingLayer)
                ? metadata.defaultSortingLayer
                : DefaultSortingLayer(layer);
            int sortingOrder = metadata != null ? metadata.defaultOrder : DefaultSortingOrder(layer);

            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sortingLayerName = sortingLayer;
                renderers[i].sortingOrder = sortingOrder;
                if (metadata != null)
                {
                    renderers[i].color = metadata.tint;
                }
            }

            RoomPainterAssetBinding binding = go.GetComponent<RoomPainterAssetBinding>();
            if (binding == null)
            {
                binding = go.AddComponent<RoomPainterAssetBinding>();
            }

            binding.assetGuid = guid;
        }

        private static string ResolveAssetPath(string assetGuidOrName)
        {
            if (string.IsNullOrEmpty(assetGuidOrName))
            {
                return string.Empty;
            }

            string path = AssetDatabase.GUIDToAssetPath(assetGuidOrName);
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }

            if (assetGuidOrName.StartsWith("Assets/"))
            {
                return assetGuidOrName;
            }

            string[] guids = AssetDatabase.FindAssets(assetGuidOrName);
            for (int i = 0; i < guids.Length; i++)
            {
                path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (AssetDatabase.LoadAssetAtPath<TileBase>(path) != null ||
                    AssetDatabase.LoadAssetAtPath<GameObject>(path) != null ||
                    LoadSprite(path) != null)
                {
                    return path;
                }
            }

            return string.Empty;
        }

        private static WallPiece ResolveWallPiece(string pieceId)
        {
            string path = ResolveAssetPath(pieceId);
            GameObject prefab = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Sprite sprite = string.IsNullOrEmpty(path) ? null : LoadSprite(path);
            string displayName = prefab != null
                ? prefab.name
                : sprite != null
                    ? sprite.name
                    : pieceId;

            return new WallPiece
            {
                prefab = prefab,
                sprite = sprite,
                straightSprite = FindVariantSprite(path, displayName, "straight") ?? sprite,
                cornerSprite = FindVariantSprite(path, displayName, "corner"),
                tSprite = FindVariantSprite(path, displayName, "t"),
                crossSprite = FindVariantSprite(path, displayName, "cross"),
                endSprite = FindVariantSprite(path, displayName, "end"),
                singleSprite = FindVariantSprite(path, displayName, "single"),
                footprint = FootprintFromSpriteSize(sprite),
                displayName = displayName,
                pieceId = pieceId
            };
        }

        private static Sprite FindVariantSprite(string basePath, string baseName, string suffix)
        {
            if (string.IsNullOrEmpty(basePath) || string.IsNullOrEmpty(suffix))
            {
                return null;
            }

            string folder = basePath;
            int slash = folder.LastIndexOf('/');
            folder = slash >= 0 ? folder.Substring(0, slash) : "Assets";
            string[] guids = AssetDatabase.FindAssets("t:Sprite " + suffix, new[] { folder });
            string normalizedBase = NormalizeVariantName(baseName);
            string normalizedSuffix = NormalizeVariantName(suffix);
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                Sprite sprite = LoadSprite(path);
                if (sprite == null)
                {
                    continue;
                }

                string normalizedName = NormalizeVariantName(sprite.name);
                if (normalizedName.Contains(normalizedSuffix) &&
                    (string.IsNullOrEmpty(normalizedBase) || normalizedName.Contains(normalizedBase) || normalizedBase.Contains(normalizedName)))
                {
                    return sprite;
                }
            }

            return null;
        }

        private static string NormalizeVariantName(string value)
        {
            return string.IsNullOrEmpty(value)
                ? string.Empty
                : value.Replace("_", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).ToLowerInvariant();
        }

        private static Vector2Int FootprintFromSpriteSize(Sprite sprite)
        {
            if (sprite == null)
            {
                return Vector2Int.one;
            }

            return new Vector2Int(
                Mathf.Max(1, Mathf.CeilToInt(sprite.rect.width / 64f)),
                Mathf.Max(1, Mathf.CeilToInt(sprite.rect.height / 64f)));
        }

        private static Sprite LoadSprite(string path)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                return sprite;
            }

            Object[] representations = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
            for (int i = 0; i < representations.Length; i++)
            {
                Sprite representation = representations[i] as Sprite;
                if (representation != null)
                {
                    return representation;
                }
            }

            return null;
        }

        // Depth now flows from the single source of truth (RoomDepthStack) so the editor
        // composer, the runtime overlay, and CliffGenerateAction can never disagree on the
        // floating-island stack (L1 floor / L2 cliff / preview / L3 backdrop). cx review fix.
        private static string DefaultSortingLayer(RoomLayer layer)
        {
            return RoomDepthStack.SlotFor(layer).sortingLayer;
        }

        private static int DefaultSortingOrder(RoomLayer layer)
        {
            return RoomDepthStack.SlotFor(layer).sortingOrder;
        }

        private static Vector3 GetRootCenter(Transform root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return root.position;
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds.center;
        }
    }
}
