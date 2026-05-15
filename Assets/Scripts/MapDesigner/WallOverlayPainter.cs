using System.Collections.Generic;
using RIMA.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public sealed class WallOverlayPainter : MonoBehaviour
    {
        private const string RootName = "WallOverlay";

        [SerializeField] private string sortingLayerName = "Wall";
        [SerializeField] private int sortingOrder = 10;

        public void PaintWalls(RoomData room, WallBrushSetSO brushSet, Tilemap baseTilemap, int seed)
        {
            Transform root = EnsureRoot();
            ClearChildren(root);

            if (brushSet == null || room.wallEdges == null)
            {
                return;
            }

            int created = 0;
            for (int i = 0; i < room.wallEdges.Count; i++)
            {
                WallSegment segment = room.wallEdges[i];
                if (segment.isDoorway)
                {
                    continue;
                }

                Sprite sprite = PickSprite(brushSet, segment, seed, i);
                if (sprite == null)
                {
                    continue;
                }

                PlaceWallSprite(segment, sprite, root, baseTilemap, created++);
            }
        }

        public static Vector2 GetOutwardAnchor(WallSegment segment)
        {
            Vector2 midpoint = new Vector2(
                (segment.start.x + segment.end.x) * 0.5f,
                (segment.start.y + segment.end.y) * 0.5f);
            switch (segment.direction)
            {
                case WallDirection.North:
                    return midpoint + Vector2.up * 0.5f;
                case WallDirection.South:
                    return midpoint + Vector2.down * 0.5f;
                case WallDirection.East:
                    return midpoint + Vector2.right * 0.5f;
                case WallDirection.West:
                    return midpoint + Vector2.left * 0.5f;
                default:
                    return midpoint;
            }
        }

        public GameObject PlaceWallSprite(WallSegment segment, Sprite sprite, Transform parent, Tilemap tilemap = null, int index = 0)
        {
            if (sprite == null || parent == null)
            {
                return null;
            }

            GameObject wall = new GameObject("Wall_" + index.ToString("0000"));
            wall.transform.SetParent(parent, false);
            wall.transform.position = CellToWorld(tilemap, GetOutwardAnchor(segment));

            SpriteRenderer renderer = wall.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = sortingOrder;
            return wall;
        }

        private static Sprite PickSprite(WallBrushSetSO brushSet, WallSegment segment, int seed, int index)
        {
            if (segment.isDoorway && brushSet.doorwayGap != null)
            {
                return brushSet.doorwayGap;
            }

            List<Sprite> sprites = segment.isCorner ? brushSet.corner : IsHorizontal(segment.direction) ? brushSet.horizontal : brushSet.vertical;
            if (sprites == null || sprites.Count == 0)
            {
                return null;
            }

            int start = PositiveModulo(Mix(seed, index, (int)segment.direction), sprites.Count);
            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite candidate = sprites[(start + i) % sprites.Count];
                if (candidate != null)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static bool IsHorizontal(WallDirection direction)
        {
            return direction == WallDirection.North || direction == WallDirection.South;
        }

        private Transform EnsureRoot()
        {
            Transform existing = transform.Find(RootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(RootName);
            root.transform.SetParent(transform, false);
            return root.transform;
        }

        private static Vector3 CellToWorld(Tilemap tilemap, Vector2 cell)
        {
            if (tilemap == null)
            {
                return new Vector3(cell.x, cell.y, 0f);
            }

            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 unitX = tilemap.CellToWorld(Vector3Int.right) - origin;
            Vector3 unitY = tilemap.CellToWorld(Vector3Int.up) - origin;
            return origin + unitX * cell.x + unitY * cell.y;
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject child = root.GetChild(i).gameObject;
                if (Application.isPlaying)
                {
                    Destroy(child);
                }
                else
                {
                    DestroyImmediate(child);
                }
            }
        }

        private static int Mix(int seed, int a, int b)
        {
            unchecked
            {
                int hash = seed;
                hash = (hash * 397) ^ a;
                hash = (hash * 397) ^ b;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }
    }
}
