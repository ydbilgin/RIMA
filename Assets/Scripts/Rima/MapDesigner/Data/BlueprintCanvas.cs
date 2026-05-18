using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    [Serializable]
    public sealed class BlueprintCanvas
    {
        [SerializeField] private Vector2Int gridSize = new Vector2Int(36, 22);
        [SerializeField] private List<IntentMapEntry> serializedIntentMap = new List<IntentMapEntry>();
        private readonly Dictionary<Vector2Int, string> intentMap = new Dictionary<Vector2Int, string>();

        public BlueprintCanvas()
        {
        }

        public BlueprintCanvas(Vector2Int gridSize)
        {
            this.gridSize = gridSize;
        }

        public Vector2Int GridSize => gridSize;
        public Dictionary<Vector2Int, string> IntentMap => intentMap;
        public int Count => intentMap.Count;

        public void SetGridSize(Vector2Int nextGridSize)
        {
            gridSize = new Vector2Int(Mathf.Max(1, nextGridSize.x), Mathf.Max(1, nextGridSize.y));
            TrimOutOfBounds();
        }

        public void Paint(Vector2Int cell, string zoneId, int brushSize)
        {
            if (string.IsNullOrEmpty(zoneId))
            {
                return;
            }

            ForEachBrushCell(cell, brushSize, pos => intentMap[pos] = zoneId);
        }

        public void Erase(Vector2Int cell, int brushSize)
        {
            ForEachBrushCell(cell, brushSize, pos => intentMap.Remove(pos));
        }

        public void FloodFill(Vector2Int seed, string zoneId)
        {
            if (!InBounds(seed) || string.IsNullOrEmpty(zoneId))
            {
                return;
            }

            string targetZone = GetZoneAt(seed);
            if (string.Equals(targetZone, zoneId, StringComparison.Ordinal))
            {
                return;
            }

            var queue = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            queue.Enqueue(seed);
            visited.Add(seed);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                intentMap[current] = zoneId;

                EnqueueIfMatching(new Vector2Int(current.x + 1, current.y), targetZone, queue, visited);
                EnqueueIfMatching(new Vector2Int(current.x - 1, current.y), targetZone, queue, visited);
                EnqueueIfMatching(new Vector2Int(current.x, current.y + 1), targetZone, queue, visited);
                EnqueueIfMatching(new Vector2Int(current.x, current.y - 1), targetZone, queue, visited);
            }
        }

        public void Clear()
        {
            intentMap.Clear();
            serializedIntentMap.Clear();
        }

        public IEnumerable<Vector2Int> CellsForZone(string zoneId)
        {
            foreach (KeyValuePair<Vector2Int, string> pair in intentMap)
            {
                if (string.Equals(pair.Value, zoneId, StringComparison.Ordinal))
                {
                    yield return pair.Key;
                }
            }
        }

        public IEnumerable<(Vector2Int a, Vector2Int b)> BoundaryEdges()
        {
            var cells = new List<Vector2Int>(intentMap.Keys);
            cells.Sort(CompareCells);

            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int cell = cells[i];
                string zone = intentMap[cell];
                Vector2Int right = new Vector2Int(cell.x + 1, cell.y);
                Vector2Int up = new Vector2Int(cell.x, cell.y + 1);

                if (intentMap.TryGetValue(right, out string rightZone) && !string.Equals(zone, rightZone, StringComparison.Ordinal))
                {
                    yield return (cell, right);
                }

                if (intentMap.TryGetValue(up, out string upZone) && !string.Equals(zone, upZone, StringComparison.Ordinal))
                {
                    yield return (cell, up);
                }
            }
        }

        public string GetZoneAt(Vector2Int cell)
        {
            return intentMap.TryGetValue(cell, out string zoneId) ? zoneId : null;
        }

        public string ToJson()
        {
            SyncSerializedFromMap();
            return JsonUtility.ToJson(new SerializedCanvas { gridSize = gridSize, entries = serializedIntentMap }, true);
        }

        public void FromJson(string json)
        {
            intentMap.Clear();
            serializedIntentMap.Clear();
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            SerializedCanvas data = JsonUtility.FromJson<SerializedCanvas>(json);
            gridSize = data.gridSize.x > 0 && data.gridSize.y > 0 ? data.gridSize : new Vector2Int(36, 22);
            if (data.entries == null)
            {
                return;
            }

            for (int i = 0; i < data.entries.Count; i++)
            {
                IntentMapEntry entry = data.entries[i];
                if (InBounds(entry.pos) && !string.IsNullOrEmpty(entry.zoneId))
                {
                    intentMap[entry.pos] = entry.zoneId;
                }
            }
        }

        private void EnqueueIfMatching(Vector2Int cell, string targetZone, Queue<Vector2Int> queue, HashSet<Vector2Int> visited)
        {
            if (!InBounds(cell) || visited.Contains(cell))
            {
                return;
            }

            if (!string.Equals(GetZoneAt(cell), targetZone, StringComparison.Ordinal))
            {
                return;
            }

            visited.Add(cell);
            queue.Enqueue(cell);
        }

        private void ForEachBrushCell(Vector2Int center, int brushSize, Action<Vector2Int> action)
        {
            int clampedSize = Mathf.Clamp(brushSize, 1, 5);
            int radiusBefore = clampedSize / 2;
            int radiusAfter = clampedSize - radiusBefore - 1;

            for (int y = center.y - radiusBefore; y <= center.y + radiusAfter; y++)
            {
                for (int x = center.x - radiusBefore; x <= center.x + radiusAfter; x++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (InBounds(pos))
                    {
                        action(pos);
                    }
                }
            }
        }

        private bool InBounds(Vector2Int cell)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < gridSize.x && cell.y < gridSize.y;
        }

        private void TrimOutOfBounds()
        {
            var remove = new List<Vector2Int>();
            foreach (Vector2Int cell in intentMap.Keys)
            {
                if (!InBounds(cell))
                {
                    remove.Add(cell);
                }
            }

            for (int i = 0; i < remove.Count; i++)
            {
                intentMap.Remove(remove[i]);
            }
        }

        private void SyncSerializedFromMap()
        {
            serializedIntentMap.Clear();
            foreach (KeyValuePair<Vector2Int, string> pair in intentMap)
            {
                serializedIntentMap.Add(new IntentMapEntry(pair.Key, pair.Value));
            }

            serializedIntentMap.Sort((a, b) => CompareCells(a.pos, b.pos));
        }

        private static int CompareCells(Vector2Int a, Vector2Int b)
        {
            int y = a.y.CompareTo(b.y);
            return y != 0 ? y : a.x.CompareTo(b.x);
        }

        [Serializable]
        private sealed class SerializedCanvas
        {
            public Vector2Int gridSize;
            public List<IntentMapEntry> entries;
        }
    }
}
