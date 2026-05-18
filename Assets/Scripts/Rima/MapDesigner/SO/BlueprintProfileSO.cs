using System;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "BlueprintProfile", menuName = "RIMA/MapDesigner/Blueprint/Profile")]
    public sealed class BlueprintProfileSO : ScriptableObject
    {
        public string profileId;
        public BlueprintZoneTypeSO[] zones;
        public BlueprintAdjacencyRuleSO[] adjacencyRules;
        public Vector2Int gridSize = new Vector2Int(36, 22);

        public BlueprintZoneTypeSO GetZone(string zoneId)
        {
            if (zones == null || string.IsNullOrEmpty(zoneId))
            {
                return null;
            }

            for (int i = 0; i < zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = zones[i];
                if (zone != null && string.Equals(zone.zoneId, zoneId, StringComparison.Ordinal))
                {
                    return zone;
                }
            }

            return null;
        }

        public BlueprintAdjacencyRuleSO GetRule(string zoneA, string zoneB)
        {
            if (adjacencyRules == null || string.IsNullOrEmpty(zoneA) || string.IsNullOrEmpty(zoneB))
            {
                return null;
            }

            for (int i = 0; i < adjacencyRules.Length; i++)
            {
                BlueprintAdjacencyRuleSO rule = adjacencyRules[i];
                if (rule == null)
                {
                    continue;
                }

                bool direct = string.Equals(rule.zoneIdA, zoneA, StringComparison.Ordinal) &&
                              string.Equals(rule.zoneIdB, zoneB, StringComparison.Ordinal);
                bool reverse = string.Equals(rule.zoneIdA, zoneB, StringComparison.Ordinal) &&
                               string.Equals(rule.zoneIdB, zoneA, StringComparison.Ordinal);
                if (direct || reverse)
                {
                    return rule;
                }
            }

            return null;
        }

        private void OnValidate()
        {
            if (zones == null)
            {
                return;
            }

            for (int i = 0; i < zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = zones[i];
                if (zone == null)
                {
                    continue;
                }

                string id = string.IsNullOrEmpty(zone.zoneId) ? zone.name : zone.zoneId;
                if (zone.macroFillSprites == null || zone.macroFillSprites.Length == 0)
                {
                    Debug.LogWarning($"[Blueprint] Zone '{id}' missing Layer 1 macro fill, will use solid fallback color");
                }

                if (zone.baseFloorSprites == null || zone.baseFloorSprites.Length == 0)
                {
                    Debug.LogWarning($"[Blueprint] Zone '{id}' missing Layer 2 base floor sprites");
                }
            }
        }
    }
}
