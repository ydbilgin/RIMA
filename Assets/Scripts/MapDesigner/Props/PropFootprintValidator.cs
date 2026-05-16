using System.Collections.Generic;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Props
{
    public static class PropFootprintValidator
    {
        public enum ValidationResult
        {
            Valid,
            OutOfBounds,
            OverlapsExistingProp,
            ViolatesWalkableConstraint,
            ViolatesRoleConstraint,
            TooCloseToOtherProp,
            InvalidArgument
        }

        public static ValidationResult Validate(
            PropDefinitionSO propDef,
            Vector2Int tilePosition,
            RoomTemplateSO template,
            CompositionRoleMap roleMap,
            IReadOnlyList<PropPlacementData> existingProps,
            out string failureDetail)
        {
            failureDetail = string.Empty;

            if (propDef == null)
            {
                failureDetail = "Prop definition is null.";
                return ValidationResult.InvalidArgument;
            }

            if (template == null)
            {
                failureDetail = "Room template is null.";
                return ValidationResult.InvalidArgument;
            }

            Vector2Int footprintSize = GetSafeFootprint(propDef);
            if (footprintSize.x <= 0 || footprintSize.y <= 0)
            {
                failureDetail = "Prop footprint must be at least 1x1.";
                return ValidationResult.InvalidArgument;
            }

            RectInt footprint = new RectInt(tilePosition, footprintSize);
            if (!FootprintInside(template.bounds, footprint))
            {
                failureDetail = $"Footprint {footprint} is outside room bounds {template.bounds}.";
                return ValidationResult.OutOfBounds;
            }

            if (propDef.requiresWalkableTile)
            {
                foreach (Vector2Int tile in EnumerateFootprint(footprint))
                {
                    if (!IsWalkableTile(template, tile))
                    {
                        failureDetail = $"Footprint tile {tile} is not walkable.";
                        return ValidationResult.ViolatesWalkableConstraint;
                    }
                }
            }

            string preferredWarning = string.Empty;
            if (roleMap != null)
            {
                CompositionRole[] forbiddenRoles = propDef.forbiddenRoles;
                foreach (Vector2Int tile in EnumerateFootprint(footprint))
                {
                    CompositionRole role = roleMap.GetRoleAt(tile);
                    if (ContainsRole(forbiddenRoles, role))
                    {
                        failureDetail = $"Footprint tile {tile} has forbidden role {role}.";
                        return ValidationResult.ViolatesRoleConstraint;
                    }
                }

                CompositionRole[] preferredRoles = propDef.preferredRoles;
                if (preferredRoles != null && preferredRoles.Length > 0 && !AnyFootprintTileMatchesRole(footprint, roleMap, preferredRoles))
                {
                    preferredWarning = "Placement does not touch any preferred role.";
                }
            }

            if (existingProps != null)
            {
                foreach (PropPlacementData existing in existingProps)
                {
                    if (existing == null) continue;
                    RectInt existingFootprint = new RectInt(
                        existing.tilePosition,
                        GetExistingFootprintSize(existing, propDef));
                    if (RectsIntersect(footprint, existingFootprint))
                    {
                        failureDetail = $"Footprint overlaps existing prop at {existing.tilePosition}.";
                        return ValidationResult.OverlapsExistingProp;
                    }
                }
            }

            if (existingProps != null && propDef.distanceFromOtherProps > 0f)
            {
                foreach (PropPlacementData existing in existingProps)
                {
                    if (existing == null) continue;
                    float distance = Vector2.Distance(tilePosition, existing.tilePosition);
                    if (distance < propDef.distanceFromOtherProps)
                    {
                        failureDetail = $"Nearest prop at {existing.tilePosition} is {distance:F2} tiles away.";
                        return ValidationResult.TooCloseToOtherProp;
                    }
                }
            }

            failureDetail = preferredWarning;
            return ValidationResult.Valid;
        }

        private static Vector2Int GetSafeFootprint(PropDefinitionSO propDef)
        {
            if (propDef == null) return Vector2Int.zero;
            return new Vector2Int(propDef.footprintSize.x, propDef.footprintSize.y);
        }

        private static bool FootprintInside(RectInt bounds, RectInt footprint)
        {
            return footprint.xMin >= bounds.xMin &&
                footprint.yMin >= bounds.yMin &&
                footprint.xMax <= bounds.xMax &&
                footprint.yMax <= bounds.yMax;
        }

        private static IEnumerable<Vector2Int> EnumerateFootprint(RectInt footprint)
        {
            for (int y = footprint.yMin; y < footprint.yMax; y++)
            {
                for (int x = footprint.xMin; x < footprint.xMax; x++)
                {
                    yield return new Vector2Int(x, y);
                }
            }
        }

        private static bool IsWalkableTile(RoomTemplateSO template, Vector2Int tile)
        {
            RectInt walkableRect = template.cameraBounds.tileRect;
            if (walkableRect.width <= 0 || walkableRect.height <= 0)
            {
                walkableRect = template.bounds;
            }

            return tile.x >= walkableRect.xMin && tile.x < walkableRect.xMax &&
                tile.y >= walkableRect.yMin && tile.y < walkableRect.yMax;
        }

        private static bool ContainsRole(CompositionRole[] roles, CompositionRole role)
        {
            if (roles == null) return false;
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == role) return true;
            }
            return false;
        }

        private static bool AnyFootprintTileMatchesRole(RectInt footprint, CompositionRoleMap roleMap, CompositionRole[] roles)
        {
            foreach (Vector2Int tile in EnumerateFootprint(footprint))
            {
                if (ContainsRole(roles, roleMap.GetRoleAt(tile))) return true;
            }
            return false;
        }

        private static bool RectsIntersect(RectInt a, RectInt b)
        {
            return a.xMin < b.xMax && a.xMax > b.xMin &&
                a.yMin < b.yMax && a.yMax > b.yMin;
        }

        private static Vector2Int GetExistingFootprintSize(PropPlacementData existing, PropDefinitionSO currentProp)
        {
            if (existing != null && currentProp != null && existing.propDefinitionGuid == GetPropIdentity(currentProp))
            {
                return GetSafeFootprint(currentProp);
            }

#if UNITY_EDITOR
            if (existing != null && !string.IsNullOrEmpty(existing.propDefinitionGuid))
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(existing.propDefinitionGuid);
                PropDefinitionSO loaded = string.IsNullOrEmpty(assetPath)
                    ? null
                    : UnityEditor.AssetDatabase.LoadAssetAtPath<PropDefinitionSO>(assetPath);
                if (loaded != null)
                {
                    return GetSafeFootprint(loaded);
                }
            }
#endif

            return Vector2Int.one;
        }

        private static string GetPropIdentity(PropDefinitionSO propDef)
        {
#if UNITY_EDITOR
            if (propDef != null)
            {
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(propDef);
                string guid = string.IsNullOrEmpty(assetPath) ? string.Empty : UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);
                if (!string.IsNullOrEmpty(guid)) return guid;
            }
#endif
            return propDef != null ? propDef.propId : string.Empty;
        }
    }
}
