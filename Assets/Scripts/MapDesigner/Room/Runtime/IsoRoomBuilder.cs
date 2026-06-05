using System.Collections.Generic;
using RIMA;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner.Room.Runtime
{
    public sealed class IsoRoomBuilder : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap collisionTilemap;
        [SerializeField] private Transform cliffContainer;
        [SerializeField] private Transform markerContainer;
        [SerializeField] private TileBase floorTile;
        [SerializeField] private TileBase collisionTile;
        [SerializeField] private Sprite cliffSouth;
        [SerializeField] private Sprite cliffSouthEast;
        [SerializeField] private Sprite cliffSouthWest;
        [SerializeField] private Vector2 tuckSouth = new Vector2(0f, 0.29f);
        [SerializeField] private Vector2 tuckSouthEast = new Vector2(-0.48f, 0.29f);
        [SerializeField] private Vector2 tuckSouthWest = new Vector2(0.48f, 0.29f);
        [SerializeField] private string cliffSortingLayer = "Floor";
        [SerializeField] private int cliffSortOrderBase = -30;
        [SerializeField] private float cliffSortYSpan = 20f;

        [Header("Props (obstacles + decor)")]
        [SerializeField] private PropRegistrySO propRegistry;
        [SerializeField] private Transform propsContainer;

        [Header("Door Gates")]
        [SerializeField] private Transform gatesContainer;
        [SerializeField] private Sprite gateNorthSprite;
        [SerializeField] private Sprite gateWestSprite; // East reuses West with flipX
        [SerializeField] private Sprite runeCombatSprite;
        [SerializeField] private Sprite runeEliteSprite;
        [SerializeField] private int gateSortOrderBase = 40;
        [SerializeField] private Vector2 runeLocalOffset = new Vector2(0f, 1.4f);
        [SerializeField] private Vector2 gateTuck = new Vector2(0f, -0.25f);
        [SerializeField] private float gateRowSpacing = 2.4f;

        private static readonly Vector3Int SouthWestNeighbor = new Vector3Int(-1, 0, 0);
        private static readonly Vector3Int SouthEastNeighbor = new Vector3Int(0, -1, 0);

        private static readonly Vector3Int[] IsoEightNeighbors =
        {
            new Vector3Int(-1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 1, 0),
        };

        public HashSet<Vector3Int> LastFloorCells { get; private set; }

        // Set during BuildMarkers; lets a run director teleport the player onto the new room.
        public Transform PlayerSpawnMarker { get; private set; }
        public IReadOnlyList<Transform> EnemySpawnMarkers => enemySpawnMarkers;

        private readonly List<Transform> enemySpawnMarkers = new List<Transform>();

        public void Build(RoomTemplateSO template)
        {
            if (template == null)
            {
                Debug.LogWarning("[IsoRoomBuilder] Cannot build null room template.");
                return;
            }

            if (template.bounds.width <= 0 || template.bounds.height <= 0)
            {
                Debug.LogWarning($"[IsoRoomBuilder] Cannot build {template.roomId}: empty bounds.");
                return;
            }

            if (!ResolveRequiredReferences())
            {
                return;
            }

            EnsureContainers();
            ClearPrevious();

            HashSet<Vector3Int> floorCells = BuildFloor(template);
            LastFloorCells = new HashSet<Vector3Int>(floorCells);

            int cliffCellCount = BuildCliffs(floorCells);
            BuildBoundary(template, floorCells);
            BuildMarkers(template);
            int propCount = BuildProps(template);

            Debug.Log($"[IsoRoomBuilder] Built {template.roomId}: {floorCells.Count} floor, {cliffCellCount} cliff, {propCount} props.");
        }

        private bool ResolveRequiredReferences()
        {
            if (grid == null)
            {
                grid = GetComponentInParent<Grid>();
            }

            if (grid == null)
            {
                grid = FindObjectOfType<Grid>();
            }

            if (groundTilemap == null)
            {
                groundTilemap = FindNamedTilemap("GroundTilemap", "Ground");
            }

            if (collisionTilemap == null)
            {
                collisionTilemap = FindNamedTilemap("CollisionTilemap", "Collision");
            }

            if (grid == null)
            {
                Debug.LogError("[IsoRoomBuilder] Missing Grid reference.");
                return false;
            }

            if (groundTilemap == null)
            {
                Debug.LogError("[IsoRoomBuilder] Missing ground Tilemap reference.");
                return false;
            }

            if (collisionTilemap == null)
            {
                Debug.LogError("[IsoRoomBuilder] Missing collision Tilemap reference.");
                return false;
            }

            if (floorTile == null)
            {
                Debug.LogError("[IsoRoomBuilder] Missing floorTile reference.");
                return false;
            }

            if (collisionTile == null)
            {
                Debug.LogError("[IsoRoomBuilder] Missing collisionTile reference.");
                return false;
            }

            return true;
        }

        private Tilemap FindNamedTilemap(params string[] names)
        {
            Tilemap namedTilemap = FindNamedTilemapIn(GetComponentsInChildren<Tilemap>(true), names);
            if (namedTilemap != null || grid == null)
            {
                return namedTilemap;
            }

            return FindNamedTilemapIn(grid.GetComponentsInChildren<Tilemap>(true), names);
        }

        private Tilemap FindNamedTilemapIn(Tilemap[] tilemaps, string[] names)
        {
            for (int i = 0; i < tilemaps.Length; i++)
            {
                for (int n = 0; n < names.Length; n++)
                {
                    if (tilemaps[i].name == names[n])
                    {
                        return tilemaps[i];
                    }
                }
            }

            return null;
        }

        private void EnsureContainers()
        {
            if (cliffContainer == null)
            {
                cliffContainer = CreateContainer("CliffSprites");
            }

            if (markerContainer == null)
            {
                markerContainer = CreateContainer("RoomMarkers");
            }

            if (propsContainer == null)
            {
                propsContainer = CreateContainer("Props");
            }

            if (gatesContainer == null)
            {
                gatesContainer = CreateContainer("Gates");
            }
        }

        private Transform CreateContainer(string containerName)
        {
            GameObject container = new GameObject(containerName);
            Transform containerTransform = container.transform;
            containerTransform.SetParent(transform, false);
            return containerTransform;
        }

        private void ClearPrevious()
        {
            groundTilemap.ClearAllTiles();
            collisionTilemap.ClearAllTiles();
            DestroyChildren(cliffContainer);
            DestroyChildren(markerContainer);
            DestroyChildren(propsContainer);
            DestroyChildren(gatesContainer);
        }

        private void DestroyChildren(Transform container)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                GameObject child = container.GetChild(i).gameObject;
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

        private HashSet<Vector3Int> BuildFloor(RoomTemplateSO template)
        {
            HashSet<Vector3Int> floorCells = new HashSet<Vector3Int>();
            RectInt bounds = template.bounds;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    if (template.IsWalkable(new Vector2Int(x, y)))
                    {
                        floorCells.Add(new Vector3Int(x, y, 0));
                    }
                }
            }

            // A blocking prop turns its cell non-walkable, but the prop still sits ON solid
            // ground. Union prop footprints into the floor mask so those interior cells get a
            // floor tile instead of reading as a void pit.
            AddPropFloorCells(template, floorCells);

            foreach (Vector3Int cell in floorCells)
            {
                groundTilemap.SetTile(cell, floorTile);
            }

            return floorCells;
        }

        private void AddPropFloorCells(RoomTemplateSO template, HashSet<Vector3Int> floorCells)
        {
            if (template.props == null || propRegistry == null)
            {
                return;
            }

            for (int i = 0; i < template.props.Count; i++)
            {
                PropPlacementData placement = template.props[i];
                if (placement == null)
                {
                    continue;
                }

                PropDefinitionSO def = propRegistry.ResolveGuid(placement.propDefinitionGuid);
                // Only blocking props turn their cell non-walkable, so only they need generated
                // floor support. Decor sits on already-walkable floor, so it never adds cells.
                if (def == null || !def.blocksWalkable)
                {
                    continue;
                }

                int rotation = ((placement.rotationSteps % 4) + 4) % 4;
                bool swap = rotation == 1 || rotation == 3;
                int width = Mathf.Max(1, swap ? def.footprintSize.y : def.footprintSize.x);
                int height = Mathf.Max(1, swap ? def.footprintSize.x : def.footprintSize.y);

                for (int dx = 0; dx < width; dx++)
                {
                    for (int dy = 0; dy < height; dy++)
                    {
                        Vector2Int cell = new Vector2Int(placement.tilePosition.x + dx, placement.tilePosition.y + dy);
                        // Clip footprints that spill outside the room so we never paint a stray
                        // floor bump beyond the authored silhouette.
                        if (template.bounds.Contains(cell))
                        {
                            floorCells.Add(new Vector3Int(cell.x, cell.y, 0));
                        }
                    }
                }
            }
        }

        private int BuildCliffs(HashSet<Vector3Int> floorCells)
        {
            bool warnedMissingSprite = false;
            int placedCount = 0;

            // Per-cell front-edge rule: any floor cell whose down-left (SW) or down-right (SE)
            // neighbour is void gets a cliff. This covers wings, inner notches and hole rims —
            // every void-facing front edge — while leaving back/side edges clean (camera angle).
            foreach (Vector3Int cell in floorCells)
            {
                bool swVoid = !floorCells.Contains(cell + SouthWestNeighbor);
                bool seVoid = !floorCells.Contains(cell + SouthEastNeighbor);
                if (!swVoid && !seVoid)
                {
                    continue;
                }

                Sprite sprite = GetCliffSprite(cell, floorCells, out Vector2 tuck);
                if (sprite == null)
                {
                    if (!warnedMissingSprite)
                    {
                        Debug.LogWarning("[IsoRoomBuilder] Skipping cliff cells because one or more cliff sprites are missing.");
                        warnedMissingSprite = true;
                    }

                    continue;
                }

                GameObject cliffObject = new GameObject("Cliff");
                cliffObject.transform.SetParent(cliffContainer, false);

                SpriteRenderer spriteRenderer = cliffObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.sortingLayerName = cliffSortingLayer;

                Vector3 cellCenter = grid.GetCellCenterWorld(cell);
                Vector3 position = new Vector3(cellCenter.x + tuck.x, cellCenter.y + tuck.y, 0f);
                cliffObject.transform.position = position;
                spriteRenderer.sortingOrder = cliffSortOrderBase + Mathf.RoundToInt(cliffSortYSpan - position.y);
                placedCount++;
            }

            return placedCount;
        }

        private Sprite GetCliffSprite(Vector3Int cell, HashSet<Vector3Int> floorCells, out Vector2 tuck)
        {
            bool swVoid = !floorCells.Contains(cell + SouthWestNeighbor);
            bool seVoid = !floorCells.Contains(cell + SouthEastNeighbor);

            if (swVoid && seVoid)
            {
                tuck = tuckSouth;
                return cliffSouth;
            }

            if (swVoid)
            {
                tuck = tuckSouthWest;
                return cliffSouthWest;
            }

            if (seVoid)
            {
                tuck = tuckSouthEast;
                return cliffSouthEast;
            }

            tuck = tuckSouth;
            return cliffSouth;
        }

        private void BuildBoundary(RoomTemplateSO template, HashSet<Vector3Int> floorCells)
        {
            RectInt bounds = template.bounds;

            for (int y = bounds.yMin - 1; y < bounds.yMax + 1; y++)
            {
                for (int x = bounds.xMin - 1; x < bounds.xMax + 1; x++)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);
                    // Use the effective floor mask (walkable + prop support), NOT raw walkable,
                    // so an interior prop-blocked cell never gets a void-ring boundary wall.
                    if (floorCells.Contains(cell) || !HasWalkableNeighbor(floorCells, x, y))
                    {
                        continue;
                    }

                    collisionTilemap.SetTile(cell, collisionTile);
                }
            }

            EnsureBoundaryCollider();
        }

        private bool HasWalkableNeighbor(HashSet<Vector3Int> floorCells, int x, int y)
        {
            Vector3Int cell = new Vector3Int(x, y, 0);
            for (int i = 0; i < IsoEightNeighbors.Length; i++)
            {
                Vector3Int neighbor = cell + IsoEightNeighbors[i];
                if (floorCells.Contains(neighbor))
                {
                    return true;
                }
            }

            return false;
        }

        private void EnsureBoundaryCollider()
        {
            TilemapCollider2D tilemapCollider = collisionTilemap.GetComponent<TilemapCollider2D>();
            if (tilemapCollider == null)
            {
                tilemapCollider = collisionTilemap.gameObject.AddComponent<TilemapCollider2D>();
            }

            Rigidbody2D rigidbody2D = collisionTilemap.GetComponent<Rigidbody2D>();
            if (rigidbody2D == null)
            {
                rigidbody2D = collisionTilemap.gameObject.AddComponent<Rigidbody2D>();
            }

            CompositeCollider2D compositeCollider = collisionTilemap.GetComponent<CompositeCollider2D>();
            if (compositeCollider == null)
            {
                compositeCollider = collisionTilemap.gameObject.AddComponent<CompositeCollider2D>();
            }

            tilemapCollider.usedByComposite = true;
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
            compositeCollider.isTrigger = false;

            int defaultLayer = LayerMask.NameToLayer("Default");
            if (defaultLayer >= 0)
            {
                collisionTilemap.gameObject.layer = defaultLayer;
            }

            TilemapRenderer tilemapRenderer = collisionTilemap.GetComponent<TilemapRenderer>();
            if (tilemapRenderer != null)
            {
                tilemapRenderer.enabled = false;
            }
        }

        private void BuildMarkers(RoomTemplateSO template)
        {
            PlayerSpawnMarker = null;
            enemySpawnMarkers.Clear();
            if (template.playerSpawn != null)
            {
                PlayerSpawnMarker = CreateMarker("PlayerSpawn", template.playerSpawn.position).transform;
            }

            if (template.enemySpawnSockets != null)
            {
                for (int i = 0; i < template.enemySpawnSockets.Count; i++)
                {
                    EnemySpawnSocket socket = template.enemySpawnSockets[i];
                    if (socket == null)
                    {
                        continue;
                    }

                    string socketId = string.IsNullOrEmpty(socket.socketId) ? i.ToString() : socket.socketId;
                    enemySpawnMarkers.Add(CreateMarker($"EnemySpawn_{socketId}", socket.position).transform);
                }
            }

            if (template.doorSockets != null)
            {
                for (int i = 0; i < template.doorSockets.Count; i++)
                {
                    DoorSocket socket = template.doorSockets[i];
                    if (socket == null)
                    {
                        continue;
                    }

                    string socketId = string.IsNullOrEmpty(socket.socketId) ? i.ToString() : socket.socketId;
                    CreateMarker($"Door_{socket.direction}_{socketId}_W{socket.widthInTiles}_Exit{socket.isExit}", socket.position);
                }
            }
        }

        private GameObject CreateMarker(string markerName, Vector2Int tilePosition)
        {
            GameObject marker = new GameObject(markerName);
            marker.transform.SetParent(markerContainer, false);
            Vector3 markerCenter = grid.GetCellCenterWorld(new Vector3Int(tilePosition.x, tilePosition.y, 0));
            marker.transform.position = new Vector3(markerCenter.x, markerCenter.y, 0f);
            return marker;
        }

        public bool TryGetLastFloorWorldBounds(out Bounds bounds)
        {
            bounds = default;
            if (LastFloorCells == null || LastFloorCells.Count == 0 || grid == null)
            {
                return false;
            }

            bool initialized = false;
            foreach (Vector3Int cell in LastFloorCells)
            {
                Vector3 world = grid.GetCellCenterWorld(cell);
                if (!initialized)
                {
                    bounds = new Bounds(world, Vector3.zero);
                    initialized = true;
                }
                else
                {
                    bounds.Encapsulate(world);
                }
            }

            Vector3 cellSize = grid.cellSize;
            bounds.Expand(new Vector3(Mathf.Max(0.5f, cellSize.x), Mathf.Max(0.5f, cellSize.y), 0f));
            return true;
        }

        // Spawns obstacle/decor props authored in template.props at iso cell centers.
        // Reuses the existing prop data (PropDefinitionSO/PropRegistrySO) and runtime
        // components (PropSorterRuntime for Y-sort, PropColliderAutoBuilder for blockers).
        private int BuildProps(RoomTemplateSO template)
        {
            if (template.props == null || template.props.Count == 0)
            {
                return 0;
            }

            if (propRegistry == null)
            {
                Debug.LogWarning("[IsoRoomBuilder] No PropRegistry assigned; skipping props.");
                return 0;
            }

            int placed = 0;
            for (int i = 0; i < template.props.Count; i++)
            {
                PropPlacementData placement = template.props[i];
                if (placement == null)
                {
                    continue;
                }

                PropDefinitionSO def = propRegistry.ResolveGuid(placement.propDefinitionGuid);
                if (def == null)
                {
                    continue;
                }

                Sprite sprite = def.PickVariant(PropDefinitionSO.StableTileSeed(placement.tilePosition));
                if (sprite == null)
                {
                    sprite = def.worldSprite;
                }

                GameObject propObject = new GameObject($"prop_{def.propId}_{placement.tilePosition.x}_{placement.tilePosition.y}");
                propObject.transform.SetParent(propsContainer, false);

                Vector3 center = grid.GetCellCenterWorld(new Vector3Int(placement.tilePosition.x, placement.tilePosition.y, 0));
                propObject.transform.position = new Vector3(center.x, center.y, 0f);

                SpriteRenderer spriteRenderer = propObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.flipX = placement.flipX;

                PropSorterRuntime sorter = propObject.AddComponent<PropSorterRuntime>();
                sorter.PropDef = def;

                if (def.blocksWalkable && def.colliderShape != PropDefinitionSO.ColliderShape.None)
                {
                    PropColliderAutoBuilder colliderBuilder = propObject.AddComponent<PropColliderAutoBuilder>();
                    colliderBuilder.PropDef = def;
                    colliderBuilder.RotationSteps = placement.rotationSteps;
                    colliderBuilder.EnsureCollider();
                }

                placed++;
            }

            return placed;
        }

        // Places a door-gate visual (slate threshold + cyan rift) plus a floating room-type
        // rune at each door socket. Gameplay lock/unlock wiring stays with GateBehavior (P5);
        // this is the iso visual only. South has no gate (no south exit in canon).
        // Spawns the run's exit doors as a side-by-side row (Hades-style) at the back of the
        // current room. One door per entry in doorTypes; each shows its destination room-type
        // rune. The run director calls this with the current graph node's choices and uses the
        // returned door objects to attach triggers / control their lit state.
        public System.Collections.Generic.List<GameObject> BuildExitDoors(System.Collections.Generic.IReadOnlyList<RoomType> doorTypes)
        {
            var doors = new System.Collections.Generic.List<GameObject>();
            if (doorTypes == null || doorTypes.Count == 0 || gateNorthSprite == null)
            {
                return doors;
            }

            if (LastFloorCells == null || LastFloorCells.Count == 0 || grid == null)
            {
                return doors;
            }

            if (gatesContainer == null)
            {
                gatesContainer = CreateContainer("Gates");
            }
            DestroyChildren(gatesContainer);

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, 0f);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, 0f);
            foreach (Vector3Int cell in LastFloorCells)
            {
                Vector3 world = grid.GetCellCenterWorld(cell);
                min = Vector3.Min(min, world);
                max = Vector3.Max(max, world);
            }

            float rowY = Mathf.Lerp(min.y, max.y, 0.72f);
            float rowXMin = float.MaxValue;
            float rowXMax = float.MinValue;
            foreach (Vector3Int cell in LastFloorCells)
            {
                Vector3 world = grid.GetCellCenterWorld(cell);
                if (Mathf.Abs(world.y - rowY) < 0.4f)
                {
                    rowXMin = Mathf.Min(rowXMin, world.x);
                    rowXMax = Mathf.Max(rowXMax, world.x);
                }
            }
            if (rowXMin > rowXMax)
            {
                rowXMin = min.x;
                rowXMax = max.x;
            }
            float centerX = (rowXMin + rowXMax) * 0.5f;
            float usableWidth = Mathf.Max(0f, (rowXMax - rowXMin) - 1.8f);
            int count = doorTypes.Count;
            float spacing = (count > 1 && (count - 1) * gateRowSpacing > usableWidth)
                ? usableWidth / (count - 1)
                : gateRowSpacing;

            for (int i = 0; i < count; i++)
            {
                RoomType doorType = doorTypes[i];
                GameObject gateObject = new GameObject($"ExitDoor_{i}_{doorType}");
                gateObject.transform.SetParent(gatesContainer, false);

                float x = centerX + (i - (count - 1) * 0.5f) * spacing;
                Vector3 position = new Vector3(x + gateTuck.x, rowY + gateTuck.y, 0f);
                gateObject.transform.position = position;

                SpriteRenderer gateRenderer = gateObject.AddComponent<SpriteRenderer>();
                gateRenderer.sprite = gateNorthSprite;
                gateRenderer.sortingLayerName = cliffSortingLayer;
                gateRenderer.sortingOrder = gateSortOrderBase + Mathf.RoundToInt(cliffSortYSpan - position.y);

                Sprite runeSprite = (doorType == RoomType.Elite || doorType == RoomType.Boss)
                    ? runeEliteSprite
                    : runeCombatSprite;
                if (runeSprite != null)
                {
                    GameObject runeObject = new GameObject("Rune");
                    runeObject.transform.SetParent(gateObject.transform, false);
                    runeObject.transform.localPosition = new Vector3(runeLocalOffset.x, runeLocalOffset.y, 0f);

                    SpriteRenderer runeRenderer = runeObject.AddComponent<SpriteRenderer>();
                    runeRenderer.sprite = runeSprite;
                    runeRenderer.sortingLayerName = cliffSortingLayer;
                    runeRenderer.sortingOrder = gateRenderer.sortingOrder + 1;
                }

                doors.Add(gateObject);
            }

            return doors;
        }
    }
}
