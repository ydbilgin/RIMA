#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System.Collections.Generic;
using RIMA.RoomPainter;
using UnityEngine;

namespace RIMA.DevTools
{
    public static class WallRunBuilder
    {
        private const int MaxInstantiationsPerCall = 64;
        private const string SortingLayerName = "Entities";

        public static List<GameObject> BuildRun(
            Grid grid,
            Vector3Int fromCell,
            Vector3Int toCell,
            WallPiece piece,
            Transform parent,
            HashSet<Vector3Int> occupied)
        {
            List<GameObject> created = new List<GameObject>();
            if (grid == null) return created;

            occupied ??= new HashSet<Vector3Int>();

            List<Vector3Int> line = RoomDataMutator.GridLine(fromCell, toCell);
            if (line.Count == 0) return created;

            bool xDominant = Mathf.Abs(toCell.x - fromCell.x) >= Mathf.Abs(toCell.y - fromCell.y);
            int step = Mathf.Max(1, piece.footprint.x);
            int lastPlacedIndex = -1;
            List<Vector3Int> placements = new List<Vector3Int>();
            HashSet<Vector3Int> previewOccupied = new HashSet<Vector3Int>(occupied);

            for (int i = 0; i < line.Count && placements.Count < MaxInstantiationsPerCall; i++)
            {
                if (lastPlacedIndex >= 0)
                {
                    int distance = xDominant
                        ? Mathf.Abs(line[i].x - line[lastPlacedIndex].x)
                        : Mathf.Abs(line[i].y - line[lastPlacedIndex].y);
                    if (distance < step) continue;
                }

                Vector3Int cell = line[i];
                if (occupied.Contains(cell)) continue;

                placements.Add(cell);
                previewOccupied.Add(cell);
                lastPlacedIndex = i;
            }

            for (int i = 0; i < placements.Count; i++)
            {
                Vector3Int cell = placements[i];
                WangResult result = WangResolver.Resolve4(cell, previewOccupied.Contains);
                GameObject go = PlaceOne(grid, cell, piece, parent, result.shape, result.rotationDegrees);
                if (go == null) continue;

                occupied.Add(cell);
                created.Add(go);
            }

            return created;
        }

        public static GameObject PlaceOne(Grid grid, Vector3Int cell, WallPiece piece, Transform parent)
        {
            return PlaceOne(grid, cell, piece, parent, WangShape.Single, 0f);
        }

        public static GameObject PlaceOne(
            Grid grid,
            Vector3Int cell,
            WallPiece piece,
            Transform parent,
            WangShape shape,
            float rotationDegrees)
        {
            return PlaceOne(grid, cell, piece, parent, SpriteForShape(piece, shape), rotationDegrees);
        }

        public static Sprite SpriteForShape(WallPiece piece, WangShape shape)
        {
            Sprite fallback = piece.straightSprite != null ? piece.straightSprite : piece.sprite;
            switch (shape)
            {
                case WangShape.Single:
                    return piece.singleSprite != null ? piece.singleSprite : fallback;
                case WangShape.End:
                    return piece.endSprite != null ? piece.endSprite : fallback;
                case WangShape.Straight:
                    return fallback;
                case WangShape.Corner:
                    return piece.cornerSprite != null ? piece.cornerSprite : fallback;
                case WangShape.T:
                    return piece.tSprite != null ? piece.tSprite : fallback;
                case WangShape.Cross:
                    return piece.crossSprite != null ? piece.crossSprite : fallback;
                default:
                    return fallback;
            }
        }

        private static GameObject PlaceOne(
            Grid grid,
            Vector3Int cell,
            WallPiece piece,
            Transform parent,
            Sprite spriteOverride,
            float rotationDegrees)
        {
            if (grid == null) return null;

            GameObject go;
            if (piece.prefab != null)
            {
                go = Object.Instantiate(piece.prefab, parent);
            }
            else
            {
                Sprite sprite = spriteOverride != null ? spriteOverride : piece.sprite;
                if (sprite == null) return null;

                string name = string.IsNullOrEmpty(piece.displayName) ? sprite.name : piece.displayName;
                go = new GameObject(name);
                if (parent != null) go.transform.SetParent(parent, false);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
            }

            go.transform.position = FootPosition(grid, cell);
            go.transform.rotation = Quaternion.Euler(0f, 0f, rotationDegrees);
            ApplySpriteRules(go, spriteOverride);
            return go;
        }

        private static Vector3 FootPosition(Grid grid, Vector3Int cell)
        {
            Vector3 position = grid.GetCellCenterWorld(cell);
            Vector3 cellSize = grid.cellSize;
            position.y -= cellSize.y * 0.5f;
            return position;
        }

        private static void ApplySpriteRules(GameObject go, Sprite spriteOverride)
        {
            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer sr in renderers)
            {
                if (spriteOverride != null) sr.sprite = spriteOverride;
                sr.sortingLayerName = SortingLayerName;
                sr.sortingOrder = 0;
                sr.spriteSortPoint = SpriteSortPoint.Pivot;
            }
        }

    }
}

#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
