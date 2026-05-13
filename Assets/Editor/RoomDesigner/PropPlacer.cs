using System.Collections.Generic;
using RIMA.RoomDesigner.Core;
using RIMA.Runtime.Rooms;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RIMA.Editor.RoomDesigner
{
    public static class PropPlacer
    {
        public static List<GameObject> PlaceProps(GameObject stageRoot, AnchorZone[] anchors, PropSpec[] propSpecs, RoomBlueprint bp, int masterSeed)
        {
            var placed = new List<GameObject>();
            if (stageRoot == null || anchors == null || propSpecs == null || bp == null)
            {
                Debug.LogWarning("PropPlacer: stage root, anchors, propSpecs, or blueprint is missing.");
                return placed;
            }

            if (propSpecs.Length == 0)
            {
                Debug.LogWarning("PropPlacer: propSpecs is empty.");
                return placed;
            }

            int subSeed = SeedPipeline.DeriveSubSeed(masterSeed, "prop");
            var random = new System.Random(subSeed);
            Grid grid = stageRoot.GetComponent<Grid>();

            for (int i = 0; i < anchors.Length; i++)
            {
                AnchorZone anchor = anchors[i];
                List<PropSpec> candidates = CollectMatchingSpecs(propSpecs, anchor.tag);
                if (candidates.Count == 0)
                {
                    continue;
                }

                PropSpec spec = candidates[random.Next(0, candidates.Count)];
                if (spec.prefab == null)
                {
                    Debug.LogWarning($"PropPlacer: propSpec for anchor '{anchor.tag}' has no prefab.");
                    continue;
                }

                Vector3 worldPos = GetWorldPosition(stageRoot, grid, bp, anchor);
                GameObject prop = Object.Instantiate(spec.prefab, worldPos, Quaternion.identity, stageRoot.transform);
                prop.name = $"{spec.prefab.name}_{anchor.tag}";
                placed.Add(prop);

                if (spec.requiresVisibleSource)
                {
                    Light2D light = prop.GetComponentInChildren<Light2D>();
                    SpriteRenderer spriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();
                    if (light == null || spriteRenderer == null)
                    {
                        Debug.LogWarning($"PropPlacer: prop '{prop.name}' requiresVisibleSource=true ama Light2D veya SpriteRenderer eksik");
                    }
                }
            }

            return placed;
        }

        private static List<PropSpec> CollectMatchingSpecs(PropSpec[] propSpecs, string anchorTag)
        {
            var matches = new List<PropSpec>();
            for (int i = 0; i < propSpecs.Length; i++)
            {
                if (string.Equals(propSpecs[i].anchorTag, anchorTag, System.StringComparison.Ordinal))
                {
                    matches.Add(propSpecs[i]);
                }
            }

            return matches;
        }

        private static Vector3 GetWorldPosition(GameObject stageRoot, Grid grid, RoomBlueprint bp, AnchorZone anchor)
        {
            var cell = new Vector3Int(
                bp.roomOrigin.x + Mathf.RoundToInt(anchor.position.x),
                bp.roomOrigin.y + Mathf.RoundToInt(anchor.position.y),
                0);

            if (grid != null)
            {
                return grid.GetCellCenterWorld(cell);
            }

            return stageRoot.transform.TransformPoint(new Vector3(cell.x, cell.y, 0f));
        }
    }
}
