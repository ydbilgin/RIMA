using System.Collections.Generic;
using RIMA.RoomPainter;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.Environment
{
    public sealed class CliffMeshGenerator : MonoBehaviour
    {
        private static readonly Vector3Int NorthCell = new Vector3Int(1, 1, 0);
        private static readonly Vector3Int SouthCell = new Vector3Int(-1, -1, 0);
        private static readonly Vector3Int EastCell = new Vector3Int(1, -1, 0);
        private static readonly Vector3Int WestCell = new Vector3Int(-1, 1, 0);

        private static readonly Vector3Int NorthEastCell = new Vector3Int(1, 0, 0);
        private static readonly Vector3Int NorthWestCell = new Vector3Int(0, 1, 0);
        private static readonly Vector3Int SouthEastCell = new Vector3Int(0, -1, 0);
        private static readonly Vector3Int SouthWestCell = new Vector3Int(-1, 0, 0);

        private const string GeneratedPrefix = "CliffMeshLoop_";

        [Header("Input")]
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Material cliffMaterial;
        [SerializeField] private bool generateOnStart = true;

        [Header("Shape")]
        [SerializeField] private Vector2 cellSize = new Vector2(0.96f, 0.585f);
        [SerializeField] private float cliffHeightWorld = 2.35f;
        [SerializeField] private float taperAmount = 0.25f;
        [SerializeField] private float bottomJitter = 0.38f;
        [SerializeField] private float teethClustering = 0.5f;
        [SerializeField] private float tileWorldLength = 3.84f;

        [Header("Metadata")]
        [SerializeField] private List<Vector3Int> cliffCells = new List<Vector3Int>();
        [SerializeField] private int lastLoopCount;
        [SerializeField] private int lastOuterLoopCount;
        [SerializeField] private int lastHoleLoopCount;
        [SerializeField] private int lastVertexCount;

        private struct Segment
        {
            public Vector2Int start;
            public Vector2Int end;
            public Vector3Int cell;
            public Vector3Int voidNeighbor;
        }

        public enum CliffBoundaryDirection
        {
            S,
            SE,
            SW,
            E,
            W,
            N,
            NE,
            NW
        }

        public struct CliffBoundarySegment
        {
            public Vector3 startWorld;
            public Vector3 endWorld;
            public Vector3 outwardNormal;
            public Vector3Int cell;
            public CliffBoundaryDirection direction;
            public int loopIndex;
            public bool isHole;
        }

        private struct Side
        {
            public Vector3Int neighbor;
            public Vector2Int startOffset;
            public Vector2Int endOffset;

            public Side(Vector3Int neighbor, Vector2Int startOffset, Vector2Int endOffset)
            {
                this.neighbor = neighbor;
                this.startOffset = startOffset;
                this.endOffset = endOffset;
            }
        }

        private sealed class BoundaryLoop
        {
            public readonly List<Vector2Int> points = new List<Vector2Int>();
            public readonly List<int> segmentIndices = new List<int>();
            public float signedArea;
            public bool isHole;
        }

        public Tilemap GroundTilemap
        {
            get => groundTilemap;
            set => groundTilemap = value;
        }

        public Material CliffMaterial
        {
            get => cliffMaterial;
            set => cliffMaterial = value;
        }

        public IReadOnlyList<Vector3Int> CliffCells => cliffCells;
        public int LastLoopCount => lastLoopCount;
        public int LastOuterLoopCount => lastOuterLoopCount;
        public int LastHoleLoopCount => lastHoleLoopCount;
        public int LastVertexCount => lastVertexCount;
        public float CliffHeightWorld => cliffHeightWorld;
        public float TaperAmount => taperAmount;
        public float BottomJitter => bottomJitter;
        public float TeethClustering => teethClustering;
        public float TileWorldLength => tileWorldLength;
        public Vector2 CellSize => cellSize;

        private static readonly Side[] BoundarySides =
        {
            new Side(SouthEastCell, new Vector2Int(0, 0), new Vector2Int(1, 0)),
            new Side(NorthEastCell, new Vector2Int(1, 0), new Vector2Int(1, 1)),
            new Side(NorthWestCell, new Vector2Int(1, 1), new Vector2Int(0, 1)),
            new Side(SouthWestCell, new Vector2Int(0, 1), new Vector2Int(0, 0)),
        };

        private void Start()
        {
            if (generateOnStart)
                Regenerate();
        }

        [ContextMenu("Regenerate")]
        public void Regenerate()
        {
            if (groundTilemap == null)
            {
                Debug.LogWarning("[CliffMeshGenerator] Ground Tilemap is not assigned.", this);
                return;
            }

            HashSet<Vector3Int> floorCells = CollectFloorCells();
            if (floorCells.Count == 0)
            {
                Debug.LogWarning("[CliffMeshGenerator] Ground Tilemap has no floor tiles.", this);
                return;
            }

            List<Segment> segments = ExtractBoundarySegments(floorCells);
            List<BoundaryLoop> loops = StitchLoops(segments);

            ClearGeneratedChildren();
            ApplyDepthStack();

            lastLoopCount = loops.Count;
            lastOuterLoopCount = 0;
            lastHoleLoopCount = 0;
            lastVertexCount = 0;

            for (int i = 0; i < loops.Count; i++)
            {
                BoundaryLoop loop = loops[i];
                if (loop.isHole) lastHoleLoopCount++;
                else lastOuterLoopCount++;
                BuildLoopMesh(loop, i);
            }

            CliffOverlayDecorator overlayDecorator = GetComponent<CliffOverlayDecorator>();
            if (overlayDecorator != null && overlayDecorator.enabled)
                overlayDecorator.RegenerateOverlays();

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
#endif

            Debug.Log("[CliffMeshGenerator] Generated " + lastLoopCount + " loops (outer " + lastOuterLoopCount +
                      ", holes " + lastHoleLoopCount + "), " + lastVertexCount + " vertices, " + cliffCells.Count +
                      " boundary cells.", this);
        }

        public string GetLastSummary()
        {
            return "loops=" + lastLoopCount + ", outer=" + lastOuterLoopCount + ", holes=" + lastHoleLoopCount +
                   ", vertices=" + lastVertexCount + ", cliffCells=" + cliffCells.Count;
        }

        public List<CliffBoundarySegment> BuildBoundarySegmentsForOverlay()
        {
            var result = new List<CliffBoundarySegment>();
            if (groundTilemap == null)
                return result;

            HashSet<Vector3Int> floorCells = CollectFloorCells();
            if (floorCells.Count == 0)
                return result;

            List<Segment> segments = ExtractBoundarySegments(floorCells);
            List<BoundaryLoop> loops = StitchLoops(segments);
            Vector3 zero = groundTilemap.CellToWorld(Vector3Int.zero);

            for (int loopIndex = 0; loopIndex < loops.Count; loopIndex++)
            {
                BoundaryLoop loop = loops[loopIndex];
                for (int i = 0; i < loop.segmentIndices.Count; i++)
                {
                    Segment segment = segments[loop.segmentIndices[i]];
                    Vector3 normal = groundTilemap.CellToWorld(segment.voidNeighbor) - zero;
                    if (normal.sqrMagnitude < 0.0001f)
                        normal = CornerToWorld(segment.end) - CornerToWorld(segment.start);

                    result.Add(new CliffBoundarySegment
                    {
                        startWorld = CornerToWorld(segment.start),
                        endWorld = CornerToWorld(segment.end),
                        outwardNormal = normal.normalized,
                        cell = segment.cell,
                        direction = QuantizeDirection(normal),
                        loopIndex = loopIndex,
                        isHole = loop.isHole
                    });
                }
            }

            return result;
        }

        private HashSet<Vector3Int> CollectFloorCells()
        {
            var floorCells = new HashSet<Vector3Int>();
            foreach (Vector3Int cell in groundTilemap.cellBounds.allPositionsWithin)
            {
                if (groundTilemap.HasTile(cell))
                    floorCells.Add(cell);
            }
            return floorCells;
        }

        private List<Segment> ExtractBoundarySegments(HashSet<Vector3Int> floorCells)
        {
            var segments = new List<Segment>();
            var boundaryCells = new HashSet<Vector3Int>();

            foreach (Vector3Int cell in floorCells)
            {
                bool isBoundaryCell = false;
                foreach (Side side in BoundarySides)
                {
                    if (floorCells.Contains(cell + side.neighbor))
                        continue;

                    isBoundaryCell = true;
                    segments.Add(new Segment
                    {
                        start = new Vector2Int(cell.x + side.startOffset.x, cell.y + side.startOffset.y),
                        end = new Vector2Int(cell.x + side.endOffset.x, cell.y + side.endOffset.y),
                        cell = cell,
                        voidNeighbor = side.neighbor
                    });
                }

                if (isBoundaryCell)
                    boundaryCells.Add(cell);
            }

            cliffCells.Clear();
            cliffCells.AddRange(boundaryCells);
            cliffCells.Sort(CompareCells);
            return segments;
        }

        private List<BoundaryLoop> StitchLoops(List<Segment> segments)
        {
            segments.Sort(CompareSegments);

            var byStart = new Dictionary<Vector2Int, List<int>>();
            for (int i = 0; i < segments.Count; i++)
            {
                if (!byStart.TryGetValue(segments[i].start, out List<int> list))
                {
                    list = new List<int>();
                    byStart.Add(segments[i].start, list);
                }
                list.Add(i);
            }

            var used = new bool[segments.Count];
            var loops = new List<BoundaryLoop>();

            for (int i = 0; i < segments.Count; i++)
            {
                if (used[i])
                    continue;

                var loop = new BoundaryLoop();
                int currentIndex = i;
                Vector2Int start = segments[i].start;
                Vector2Int cursor = start;
                int guard = 0;

                while (guard++ < segments.Count + 1)
                {
                    Segment segment = segments[currentIndex];
                    used[currentIndex] = true;
                    loop.points.Add(segment.start);
                    loop.segmentIndices.Add(currentIndex);
                    cursor = segment.end;

                    if (cursor == start)
                        break;

                    currentIndex = FindNextSegment(cursor, byStart, used, segments);
                    if (currentIndex < 0)
                    {
                        Debug.LogWarning("[CliffMeshGenerator] Open boundary loop at " + cursor + ".", this);
                        break;
                    }
                }

                if (loop.points.Count >= 3 && cursor == start)
                {
                    loop.signedArea = ComputeSignedArea(loop.points);
                    loop.isHole = loop.signedArea < 0f;
                    loops.Add(loop);
                }
            }

            return loops;
        }

        private int FindNextSegment(Vector2Int start, Dictionary<Vector2Int, List<int>> byStart, bool[] used, List<Segment> segments)
        {
            if (!byStart.TryGetValue(start, out List<int> candidates))
                return -1;

            int best = -1;
            for (int i = 0; i < candidates.Count; i++)
            {
                int candidate = candidates[i];
                if (used[candidate])
                    continue;
                if (best < 0 || CompareSegments(segments[candidate], segments[best]) < 0)
                    best = candidate;
            }

            return best;
        }

        private float ComputeSignedArea(List<Vector2Int> points)
        {
            double area = 0.0;
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 a = CornerToWorld(points[i]);
                Vector3 b = CornerToWorld(points[(i + 1) % points.Count]);
                area += (double)a.x * b.y - (double)b.x * a.y;
            }
            return (float)(area * 0.5);
        }

        private void BuildLoopMesh(BoundaryLoop loop, int loopIndex)
        {
            int count = loop.points.Count;
            var top = new Vector3[count];
            var bottom = new Vector3[count];
            var u = new float[count];
            float perimeter = 0f;

            for (int i = 0; i < count; i++)
            {
                top[i] = CornerToWorld(loop.points[i]);
                if (i > 0)
                    perimeter += Vector3.Distance(top[i - 1], top[i]);
                u[i] = perimeter / Mathf.Max(tileWorldLength, 0.0001f);
            }

            for (int i = 0; i < count; i++)
            {
                int prev = (i - 1 + count) % count;
                int next = (i + 1) % count;
                Vector2 tangent = ((Vector2)(top[next] - top[prev])).normalized;
                Vector2 inward = new Vector2(-tangent.y, tangent.x);
                float height = Mathf.Max(0.01f, cliffHeightWorld);
                float jitter = Mathf.Clamp01(bottomJitter);
                float cluster = ValueNoise1D(i * Mathf.Lerp(0.9f, 0.22f, Mathf.Clamp01(teethClustering)) + loopIndex * 13.37f);
                float local = HashCentered(i + loopIndex * 92821);
                float depthMultiplier = Mathf.Clamp(1f + jitter * (Mathf.Lerp(-0.55f, 0.95f, cluster) + local * 0.35f), 0.45f, 1.55f);
                float lateral = height * jitter * Mathf.Lerp(0.035f, 0.14f, cluster) * HashCentered(i * 31 + loopIndex * 719);

                bottom[i] = top[i] + Vector3.down * (height * depthMultiplier) +
                            new Vector3(inward.x, inward.y, 0f) * Mathf.Max(0f, taperAmount) +
                            new Vector3(tangent.x, tangent.y, 0f) * lateral;
            }

            var vertices = new Vector3[count * 2];
            var uv = new Vector2[count * 2];
            var colors = new Color[count * 2];
            for (int i = 0; i < count; i++)
            {
                vertices[i] = transform.InverseTransformPoint(top[i]);
                vertices[i + count] = transform.InverseTransformPoint(bottom[i]);
                uv[i] = new Vector2(u[i], 1f);
                uv[i + count] = new Vector2(u[i], 0f);
                colors[i] = new Color(0.72f, 0.68f, 0.62f, 1f);
                colors[i + count] = new Color(0.25f, 0.20f, 0.32f, 1f);
            }

            var triangles = new int[count * 6];
            int ti = 0;
            for (int i = 0; i < count; i++)
            {
                int next = (i + 1) % count;
                triangles[ti++] = i;
                triangles[ti++] = next;
                triangles[ti++] = i + count;
                triangles[ti++] = next;
                triangles[ti++] = next + count;
                triangles[ti++] = i + count;
            }

            var mesh = new Mesh
            {
                name = GeneratedPrefix + loopIndex + (loop.isHole ? "_Hole" : "_Outer")
            };
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.colors = colors;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            GameObject child = new GameObject(mesh.name);
            child.transform.SetParent(transform, false);

            var filter = child.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;

            var renderer = child.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = ResolveMaterial();
            ApplyDepthStack(renderer);

            lastVertexCount += vertices.Length;
        }

        private Vector3 CornerToWorld(Vector2Int corner)
        {
            return groundTilemap.CellToWorld(new Vector3Int(corner.x, corner.y, 0));
        }

        private Material ResolveMaterial()
        {
            if (cliffMaterial != null)
                return cliffMaterial;

            Shader shader = Shader.Find("RIMA/2D/CliffVoidFade");
            return shader != null ? new Material(shader) : null;
        }

        private void ApplyDepthStack()
        {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>(true))
                ApplyDepthStack(renderer);
        }

        private static void ApplyDepthStack(MeshRenderer renderer)
        {
            RoomDepthStack.DepthSlot slot = RoomDepthStack.SlotFor(RoomLayer.Cliff);
            renderer.sortingLayerName = slot.sortingLayer;
            renderer.sortingOrder = slot.sortingOrder;
        }

        private void ClearGeneratedChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (!child.name.StartsWith(GeneratedPrefix, System.StringComparison.Ordinal))
                    continue;

                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }
        }

        private static int CompareCells(Vector3Int a, Vector3Int b)
        {
            int y = a.y.CompareTo(b.y);
            return y != 0 ? y : a.x.CompareTo(b.x);
        }

        private static int CompareSegments(Segment a, Segment b)
        {
            int sy = a.start.y.CompareTo(b.start.y);
            if (sy != 0) return sy;
            int sx = a.start.x.CompareTo(b.start.x);
            if (sx != 0) return sx;
            int ey = a.end.y.CompareTo(b.end.y);
            if (ey != 0) return ey;
            return a.end.x.CompareTo(b.end.x);
        }

        private CliffBoundaryDirection QuantizeDirection(Vector3 vector)
        {
            Vector3 zero = groundTilemap != null ? groundTilemap.CellToWorld(Vector3Int.zero) : Vector3.zero;
            CliffBoundaryDirection bestDirection = CliffBoundaryDirection.S;
            float bestDot = float.NegativeInfinity;

            TestDirection(CliffBoundaryDirection.S, SouthCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.SE, SouthEastCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.SW, SouthWestCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.E, EastCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.W, WestCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.N, NorthCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.NE, NorthEastCell, vector, zero, ref bestDirection, ref bestDot);
            TestDirection(CliffBoundaryDirection.NW, NorthWestCell, vector, zero, ref bestDirection, ref bestDot);

            return bestDirection;
        }

        private void TestDirection(CliffBoundaryDirection direction, Vector3Int cellDirection, Vector3 vector, Vector3 zero,
            ref CliffBoundaryDirection bestDirection, ref float bestDot)
        {
            if (groundTilemap == null)
                return;

            Vector3 candidate = groundTilemap.CellToWorld(cellDirection) - zero;
            if (candidate.sqrMagnitude < 0.0001f)
                return;

            float dot = Vector3.Dot(vector.normalized, candidate.normalized);
            if (dot <= bestDot)
                return;

            bestDot = dot;
            bestDirection = direction;
        }

        private static float Hash01(int seed)
        {
            unchecked
            {
                uint value = (uint)seed;
                value ^= 2747636419u;
                value *= 2654435769u;
                value ^= value >> 16;
                value *= 2654435769u;
                value ^= value >> 16;
                return (value & 0x00FFFFFFu) / 16777215f;
            }
        }

        private static float HashCentered(int seed)
        {
            return Hash01(seed) * 2f - 1f;
        }

        private static float ValueNoise1D(float x)
        {
            int ix = Mathf.FloorToInt(x);
            float t = Mathf.SmoothStep(0f, 1f, x - ix);
            return Mathf.Lerp(Hash01(ix), Hash01(ix + 1), t);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
        private static void KeepCanonicalScreenVectorsDocumented()
        {
            _ = NorthCell;
            _ = SouthCell;
            _ = EastCell;
            _ = WestCell;
            _ = NorthEastCell;
            _ = NorthWestCell;
            _ = SouthEastCell;
            _ = SouthWestCell;
        }
    }
}
