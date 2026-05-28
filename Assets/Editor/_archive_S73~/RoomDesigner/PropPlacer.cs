using System.Collections.Generic;
using RIMA.RoomDesigner.Core;
using RIMA.Runtime.Rooms;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RIMA.Editor.RoomDesigner
{
    public static class PropPlacer
    {
        private const string DropShadowSpritePath = "Assets/Art/VFX/DropShadow_Oval.png";
        private static readonly Vector3 ShadowOffset = new Vector3(0f, -0.2f, 0.01f);

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
                AddDropShadow(prop);
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

        private static void AddDropShadow(GameObject prop)
        {
            Sprite shadowSprite = AssetDatabase.LoadAssetAtPath<Sprite>(DropShadowSpritePath);
            if (shadowSprite == null)
            {
                Debug.LogWarning($"PropPlacer: drop shadow sprite missing at {DropShadowSpritePath}");
                return;
            }

            var shadow = new GameObject("Shadow");
            shadow.transform.SetParent(prop.transform, false);
            shadow.transform.localPosition = ShadowOffset;
            shadow.transform.SetAsFirstSibling();

            var shadowRenderer = shadow.AddComponent<SpriteRenderer>();
            shadowRenderer.sprite = shadowSprite;
            shadowRenderer.color = new Color(0f, 0f, 0f, 0.4f);
            shadowRenderer.sortingOrder = -1;
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
