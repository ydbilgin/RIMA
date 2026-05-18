using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Data;
using RIMA.MapDesigner.Editor.Blueprint;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "RoomBlueprint", menuName = "RIMA/MapDesigner/Blueprint/Room")]
    public sealed class RoomBlueprintSO : ScriptableObject
    {
        public string roomId;
        public string displayName;
        public BlueprintProfileSO profile;
        public Vector2Int gridSize = new Vector2Int(36, 22);
        public int defaultSeed;
        public int currentSeed;
        public List<IntentMapEntry> intentMap = new List<IntentMapEntry>();

        public BlueprintCanvas ToCanvas()
        {
            var canvas = new BlueprintCanvas(gridSize);
            if (intentMap == null)
            {
                return canvas;
            }

            for (int i = 0; i < intentMap.Count; i++)
            {
                IntentMapEntry entry = intentMap[i];
                if (!string.IsNullOrEmpty(entry.zoneId))
                {
                    canvas.Paint(entry.pos, entry.zoneId, 1);
                }
            }

            return canvas;
        }

        public void FromCanvas(BlueprintCanvas canvas)
        {
            intentMap ??= new List<IntentMapEntry>();
            intentMap.Clear();

            if (canvas == null)
            {
                gridSize = new Vector2Int(36, 22);
                return;
            }

            gridSize = canvas.GridSize;
            foreach (KeyValuePair<Vector2Int, string> pair in canvas.IntentMap)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    intentMap.Add(new IntentMapEntry(pair.Key, pair.Value));
                }
            }

            intentMap.Sort((a, b) =>
            {
                int y = a.pos.y.CompareTo(b.pos.y);
                return y != 0 ? y : a.pos.x.CompareTo(b.pos.x);
            });
        }
    }
}
