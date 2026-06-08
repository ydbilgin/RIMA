using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls.V2
{
    public class RoomDebugGizmo : MonoBehaviour
    {
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private List<Vector2Int> walkableCells = new List<Vector2Int>();
        [SerializeField] private List<RectInt> waterRects = new List<RectInt>();
        [SerializeField] private List<RectInt> islandRects = new List<RectInt>();
        [SerializeField] private List<RoomSocket> sockets = new List<RoomSocket>();

        public void Capture(RoomSpec spec, IEnumerable<Vector2Int> footprint, float worldCellSize)
        {
            cellSize = Mathf.Max(0.01f, worldCellSize);
            walkableCells = footprint != null ? new List<Vector2Int>(footprint) : new List<Vector2Int>();
            waterRects = spec != null && spec.waterPoolRects != null ? new List<RectInt>(spec.waterPoolRects) : new List<RectInt>();
            islandRects = spec != null && spec.interiorIslandRects != null ? new List<RectInt>(spec.interiorIslandRects) : new List<RectInt>();
            sockets = spec != null && spec.sockets != null ? new List<RoomSocket>(spec.sockets) : new List<RoomSocket>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                return;
            }

            DrawCells();
            DrawRects(waterRects, Color.cyan);
            DrawRects(islandRects, new Color(1f, 0.55f, 0.15f, 1f));
            DrawSockets();
        }
#endif

        private void DrawCells()
        {
            Gizmos.color = Color.green;
            Vector3 size = new Vector3(0.9f * cellSize, 0.9f * cellSize, 0.01f);
            foreach (var cell in walkableCells)
                Gizmos.DrawWireCube(transform.TransformPoint(CellCenter(cell)), size);
        }

        private void DrawRects(List<RectInt> rects, Color color)
        {
            Gizmos.color = color;
            foreach (var rect in rects)
            {
                Vector3 center = new Vector3((rect.x + rect.width * 0.5f) * cellSize, (rect.y + rect.height * 0.5f) * cellSize, -0.02f);
                Vector3 size = new Vector3(rect.width * cellSize, rect.height * cellSize, 0.01f);
                Gizmos.DrawWireCube(transform.TransformPoint(center), size);
            }
        }

        private void DrawSockets()
        {
            foreach (var socket in sockets)
            {
                Gizmos.color = SocketColor(socket.type);
                Gizmos.DrawSphere(transform.TransformPoint(CellCenter(socket.cell)), 0.12f * cellSize);
            }
        }

        private Vector3 CellCenter(Vector2Int cell)
        {
            return new Vector3((cell.x + 0.5f) * cellSize, (cell.y + 0.5f) * cellSize, 0f);
        }

        private static Color SocketColor(SocketType type)
        {
            switch (type)
            {
                case SocketType.EnemyMelee:
                case SocketType.EnemyRanged:
                case SocketType.EnemyElite:
                case SocketType.EnemyBoss:
                case SocketType.EnemyWave:
                    return Color.red;
                case SocketType.ObjectiveDoor:
                case SocketType.ObjectiveExit:
                case SocketType.ObjectiveChest:
                case SocketType.ObjectiveTrigger:
                case SocketType.ObjectiveRitual:
                case SocketType.ObjectivePortal:
                    return Color.magenta;
                default:
                    return Color.yellow;
            }
        }
    }
}
