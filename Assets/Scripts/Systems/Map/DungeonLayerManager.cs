using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    public class DungeonLayerManager : MonoBehaviour
    {
        public static DungeonLayerManager Instance { get; private set; }

        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private Tilemap detailTilemap;
        [SerializeField] private Tilemap aoTilemap;

        [Header("Tile Pools")]
        [SerializeField] public TileBase[] f1FloorTiles;

        private Transform _propParent;

        private void Awake()
        {
            Instance = this;

            // Auto-find tilemaps from parent Grid if not assigned
            if (floorTilemap == null)
            {
                var t = transform.parent?.Find("Ground");
                if (t != null) floorTilemap = t.GetComponent<Tilemap>();
            }
            if (detailTilemap == null)
            {
                var t = transform.parent?.Find("Detail");
                if (t != null) detailTilemap = t.GetComponent<Tilemap>();
            }
            if (aoTilemap == null)
            {
                var t = transform.parent?.Find("AO");
                if (t != null) aoTilemap = t.GetComponent<Tilemap>();
            }

            if (_propParent == null)
            {
                _propParent = new GameObject("Props").transform;
                _propParent.SetParent(transform);
            }
        }

        /// <summary>
        /// Places random detail tiles within the supplied room bounds.
        /// </summary>
        public void PlaceDetailTiles(BoundsInt roomBounds, TileBase[] detailTilePool)
        {
            if (detailTilePool == null || detailTilePool.Length == 0)
            {
                return;
            }

            foreach (Vector3Int position in roomBounds.allPositionsWithin)
            {
                if (Random.value >= 0.3f)
                {
                    continue;
                }

                TileBase tile = detailTilePool[Random.Range(0, detailTilePool.Length)];
                detailTilemap.SetTile(position, tile);
            }
        }

        /// <summary>
        /// Places ambient occlusion shadow tiles on floor cells adjacent to north or west wall tiles.
        /// </summary>
        public void PlaceAOShadows(BoundsInt roomBounds, Tilemap wallTilemap, TileBase aoShadowTile)
        {
            foreach (Vector3Int position in roomBounds.allPositionsWithin)
            {
                if (floorTilemap.GetTile(position) == null)
                {
                    continue;
                }

                bool hasNorthWall = wallTilemap.GetTile(position + Vector3Int.up) != null;
                bool hasWestWall = wallTilemap.GetTile(position + Vector3Int.left) != null;

                if (hasNorthWall || hasWestWall)
                {
                    aoTilemap.SetTile(position, aoShadowTile);
                }
            }
        }

        /// <summary>
        /// Scatters prop prefabs on valid floor cells within the supplied room bounds.
        /// </summary>
        public void ScatterProps(BoundsInt roomBounds, GameObject[] propPrefabs, int count)
        {
            if (propPrefabs == null || propPrefabs.Length == 0 || count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                for (int attempt = 0; attempt < 100; attempt++)
                {
                    Vector3Int cellPosition = new Vector3Int(
                        Random.Range(roomBounds.xMin, roomBounds.xMax),
                        Random.Range(roomBounds.yMin, roomBounds.yMax),
                        Random.Range(roomBounds.zMin, roomBounds.zMax));

                    if (floorTilemap.GetTile(cellPosition) == null)
                    {
                        continue;
                    }

                    GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
                    Vector3 worldPosition = floorTilemap.CellToWorld(cellPosition) + floorTilemap.cellSize * 0.5f;
                    Instantiate(prefab, worldPosition, Quaternion.identity, _propParent);
                    break;
                }
            }
        }

        /// <summary>
        /// Clears generated detail tiles, ambient occlusion tiles, and scattered props.
        /// </summary>
        public void Clear()
        {
            detailTilemap.ClearAllTiles();
            aoTilemap.ClearAllTiles();

            if (_propParent == null)
            {
                return;
            }

            for (int i = _propParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_propParent.GetChild(i).gameObject);
            }
        }
    }
}
