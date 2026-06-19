using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls
{
    public static class WallChainBuilder
    {
        public enum EdgeType
        {
            NE,
            NW,
            SE,
            SW,
            N,
            S,
            E,
            W
        }

        public static GameObject Build(RIMA.Rooms.RoomFootprintPolygon footprint, WallChunkLibrary library, Transform parent)
        {
            var root = new GameObject("WallChainRoot");
            root.transform.SetParent(parent, false);

            if (footprint == null || library == null || footprint.vertices == null || footprint.vertices.Count < 2)
            {
                return root;
            }

            for (int i = 0; i < footprint.vertices.Count; i++)
            {
                Vector2Int v1 = footprint.vertices[i];
                Vector2Int v2 = footprint.vertices[(i + 1) % footprint.vertices.Count];
                EdgeType currentEdge = ClassifyEdge(v1, v2);
                EdgeType prevEdge = GetPreviousEdgeType(footprint, i);

                bool isDoorEdge = ContainsIndex(footprint.doorEdgeIndices, i);
                bool isLowEdge = ContainsIndex(footprint.lowEdgeIndices, i);
                bool isOpenEdge = ContainsIndex(footprint.openEdgeIndices, i);

                if (isDoorEdge)
                {
                    PlaceDoorArch(library, v1, v2, currentEdge, root.transform);
                    continue;
                }

                if (isLowEdge)
                {
                    PlaceLowWallSpan(library, v1, v2, currentEdge, root.transform);
                    continue;
                }

                if (isOpenEdge)
                {
                    continue;
                }

                WallType connectorType = ClassifyConnector(prevEdge, currentEdge, false);
                PlaceChunk(library, library.GetConnectorFor(connectorType), v1, $"Connector_{connectorType}_{v1.x}_{v1.y}", root.transform);
                FillWallSpan(library, v1, v2, currentEdge, root.transform);
                PlaceSeamOverlay(library, v1, prevEdge, currentEdge, root.transform);
            }

            return root;
        }

        public static WallType ClassifyConnector(EdgeType prev, EdgeType next, bool isDoorStart)
        {
            if (isDoorStart)
            {
                return WallType.Connector_DoorLeft;
            }

            return GetAngleClass(prev, next) switch
            {
                0 => WallType.Connector_Straight,
                90 => WallType.Connector_OuterCorner,
                -90 => WallType.Connector_InnerCorner,
                180 => WallType.Connector_End,
                _ => WallType.Connector_Straight
            };
        }

        public static List<WallType> PackSpans(int length)
        {
            var result = new List<WallType>();
            int remaining = Mathf.Max(0, length);

            while (remaining > 0)
            {
                if (remaining >= 3)
                {
                    result.Add(WallType.WallSpan_Long);
                    remaining -= 3;
                }
                else if (remaining >= 2)
                {
                    result.Add(WallType.WallSpan_Medium);
                    remaining -= 2;
                }
                else
                {
                    result.Add(WallType.WallSpan_Short);
                    remaining -= 1;
                }
            }

            return result;
        }

        public static int GetAngleClass(EdgeType prev, EdgeType next)
        {
            float signedAngle = Vector2.SignedAngle(DirectionFor(prev), DirectionFor(next));
            int rounded = Mathf.RoundToInt(signedAngle / 45f) * 45;

            if (rounded == -180)
            {
                return 180;
            }

            return rounded;
        }

        private static EdgeType ClassifyEdge(Vector2Int v1, Vector2Int v2)
        {
            Vector2Int delta = v2 - v1;
            if (delta.x > 0 && delta.y == 0) return EdgeType.E;
            if (delta.x < 0 && delta.y == 0) return EdgeType.W;
            if (delta.y > 0 && delta.x == 0) return EdgeType.N;
            if (delta.y < 0 && delta.x == 0) return EdgeType.S;
            if (delta.x > 0 && delta.y > 0) return EdgeType.NE;
            if (delta.x > 0 && delta.y < 0) return EdgeType.SE;
            if (delta.x < 0 && delta.y > 0) return EdgeType.NW;
            return EdgeType.SW;
        }

        private static EdgeType GetPreviousEdgeType(RIMA.Rooms.RoomFootprintPolygon footprint, int currentIdx)
        {
            int previousIdx = (currentIdx - 1 + footprint.vertices.Count) % footprint.vertices.Count;
            Vector2Int v1 = footprint.vertices[previousIdx];
            Vector2Int v2 = footprint.vertices[currentIdx];
            return ClassifyEdge(v1, v2);
        }

        private static void FillWallSpan(WallChunkLibrary library, Vector2Int v1, Vector2Int v2, EdgeType edge, Transform parent)
        {
            int length = GetEdgeLengthCells(v1, v2);
            List<WallType> spans = PackSpans(length);
            int consumed = 0;

            foreach (WallType spanType in spans)
            {
                int spanLength = GetSpanLength(spanType);
                Vector2 gridPos = SampleAlongEdge(v1, v2, consumed + spanLength * 0.5f);
                WallChunkData data = library.GetSpanFor(edge, spanType);
                PlaceChunk(library, data, gridPos, $"Span_{spanType}_{edge}_{Mathf.RoundToInt(gridPos.x)}_{Mathf.RoundToInt(gridPos.y)}", parent);
                consumed += spanLength;
            }
        }

        private static void PlaceLowWallSpan(WallChunkLibrary library, Vector2Int v1, Vector2Int v2, EdgeType edge, Transform parent)
        {
            int length = GetEdgeLengthCells(v1, v2);
            int consumed = 0;

            PlaceChunk(library, library.GetConnectorFor(WallType.Seam_FrontCornerL), v1, $"Seam_FrontCornerL_{v1.x}_{v1.y}", parent);

            while (consumed < length)
            {
                int pieceLength = length - consumed >= 2 ? 2 : 1;
                Vector2 gridPos = SampleAlongEdge(v1, v2, consumed + pieceLength * 0.5f);
                WallChunkData data = library.GetLowWallFor(pieceLength);
                PlaceChunk(library, data, gridPos, $"LowWall_{pieceLength}x_{edge}_{Mathf.RoundToInt(gridPos.x)}_{Mathf.RoundToInt(gridPos.y)}", parent);
                consumed += pieceLength;
            }

            PlaceChunk(library, library.GetConnectorFor(WallType.Seam_FrontCornerR), v2, $"Seam_FrontCornerR_{v2.x}_{v2.y}", parent);
        }

        private static void PlaceDoorArch(WallChunkLibrary library, Vector2Int v1, Vector2Int v2, EdgeType edge, Transform parent)
        {
            int length = GetEdgeLengthCells(v1, v2);
            Vector2 archPos = SampleAlongEdge(v1, v2, Mathf.Max(1f, length * 0.5f));

            PlaceChunk(library, library.GetConnectorFor(WallType.Connector_DoorLeft), v1, $"Connector_DoorLeft_{edge}_{v1.x}_{v1.y}", parent);
            PlaceChunk(library, library.GetConnectorFor(WallType.DoorArch_2w), archPos, $"DoorArch_2w_{edge}_{Mathf.RoundToInt(archPos.x)}_{Mathf.RoundToInt(archPos.y)}", parent);
            PlaceChunk(library, library.GetConnectorFor(WallType.Connector_DoorRight), v2, $"Connector_DoorRight_{edge}_{v2.x}_{v2.y}", parent);
        }

        private static void PlaceSeamOverlay(
            WallChunkLibrary library,
            Vector2Int pos,
            EdgeType prevEdge,
            EdgeType nextEdge,
            Transform parent)
        {
            WallChunkData data = library.GetSeamFor(prevEdge, nextEdge);
            PlaceChunk(library, data, pos, $"Seam_{prevEdge}_{nextEdge}_{pos.x}_{pos.y}", parent);
        }

        private static GameObject PlaceChunk(WallChunkLibrary library, WallChunkData data, Vector2Int gridPos, string name, Transform parent)
        {
            return PlaceChunk(library, data, new Vector2(gridPos.x, gridPos.y), name, parent);
        }

        private static GameObject PlaceChunk(WallChunkLibrary library, WallChunkData data, Vector2 gridPos, string name, Transform parent)
        {
            if (data == null)
            {
                return null;
            }

            Vector3 worldPos = GridToWorld(gridPos);
            GameObject prefab = library.GetPrefab(data);
            GameObject instance;

            if (prefab != null)
            {
                instance = Object.Instantiate(prefab, worldPos, Quaternion.identity, parent);
            }
            else
            {
                instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Object.DestroyImmediate(instance.GetComponent<BoxCollider>());
                instance.transform.SetParent(parent, false);
                instance.transform.position = worldPos;
                instance.transform.localScale = new Vector3(data.colliderSize.x, data.colliderSize.y, 0.1f);
                var collider = instance.AddComponent<BoxCollider2D>();
                collider.size = Vector2.one;
                collider.offset = data.colliderOffset;
            }

            instance.name = name;
            return instance;
        }

        private static int GetEdgeLengthCells(Vector2Int v1, Vector2Int v2)
        {
            Vector2Int delta = v2 - v1;
            return Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        }

        private static int GetSpanLength(WallType type)
        {
            return type switch
            {
                WallType.WallSpan_Long => 3,
                WallType.WallSpan_Medium => 2,
                _ => 1
            };
        }

        private static Vector2 SampleAlongEdge(Vector2Int v1, Vector2Int v2, float distanceCells)
        {
            int length = Mathf.Max(1, GetEdgeLengthCells(v1, v2));
            float t = Mathf.Clamp01(distanceCells / length);
            return Vector2.Lerp(v1, v2, t);
        }

        private static bool ContainsIndex(List<int> indices, int value)
        {
            return indices != null && indices.Contains(value);
        }

        private static Vector2 DirectionFor(EdgeType edge)
        {
            return edge switch
            {
                EdgeType.NE => new Vector2(1f, 1f).normalized,
                EdgeType.NW => new Vector2(-1f, 1f).normalized,
                EdgeType.SE => new Vector2(1f, -1f).normalized,
                EdgeType.SW => new Vector2(-1f, -1f).normalized,
                EdgeType.N => Vector2.up,
                EdgeType.S => Vector2.down,
                EdgeType.E => Vector2.right,
                EdgeType.W => Vector2.left,
                _ => Vector2.right
            };
        }

        // Rectangular projection with Y-compression for HIGH TOP-DOWN foreshortening.
        // Karar #149 (Iso Tilemap) REVOKED 2026-05-24 - debugging Iter 1 verdict.
        // Y-compression value targets PURE TOP-DOWN (~85-90deg camera, chatgpt_ref match).
        // Tune: 0.5=iso, 0.6-0.7=3/4 Hades, 0.85=near top-down, 1.0=pure top-down.
        public static Vector3 GridToWorld(Vector2Int cell)
        {
            return GridToWorld(new Vector2(cell.x, cell.y));
        }

        private static Vector3 GridToWorld(Vector2 cell)
        {
            // Wall + floor sprite = 128 px wide at PPU=64 -> 2 Unity units.
            // Step X = 2 (sprite width) -> edge-to-edge tile.
            // Step Y = 1 -> pure top-down, chatgpt_ref Image 1 match (~85-90deg).
            // Wall sprite 384 tall = 6 cells visual stack (off-grid rendering).
            float worldX = cell.x * 2f;
            float worldY = cell.y * 1f;
            return new Vector3(worldX, worldY, 0f);
        }
    }

#if UNITY_EDITOR
    internal static class WallChainBuilderEditorMenu
    {
        private const string TestScenePath = "Assets/Scenes/Demo/DiamondRoom_v1.unity";

        // [MenuItem removed — duplicate of RIMAWallChainBuilderMenu, archived]
        private static void BuildTestDiamondRoom()
        {
            if (System.IO.File.Exists(TestScenePath))
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(TestScenePath);
            }
            else
            {
                System.IO.Directory.CreateDirectory("Assets/Scenes/Demo");
                var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
                    UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects,
                    UnityEditor.SceneManagement.NewSceneMode.Single);
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, TestScenePath);
            }

            GameObject existing = GameObject.Find("WallChainRoot");
            if (existing != null)
            {
                Object.DestroyImmediate(existing);
            }

            var footprint = ScriptableObject.CreateInstance<RIMA.Rooms.RoomFootprintPolygon>();
            footprint.vertices = new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(4, 1),
                new Vector2Int(5, 3),
                new Vector2Int(4, 5),
                new Vector2Int(0, 5),
                new Vector2Int(-1, 3)
            };
            footprint.openEdgeIndices = new List<int> { 0 };
            footprint.lowEdgeIndices = new List<int> { 0 };
            footprint.doorEdgeIndices = new List<int> { 3 };

            var library = ScriptableObject.CreateInstance<WallChunkLibrary>();
            WallChainBuilder.Build(footprint, library, null);

            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            Object.DestroyImmediate(footprint);
            Object.DestroyImmediate(library);
        }
    }
#endif
}
