using System.Collections.Generic;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Auto;
using RIMA.MapDesigner.Props.Runtime;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Runtime
{
    public static class RoomDecorationPass
    {
        private const float TargetDensity = 0.65f;
        private const int SeedTries = 8;
        private const int PlayerSpawnGuardRadius = 1;
        private const int RewardGuardRadius = 1;

        public static int Apply(
            RoomTemplateSO template,
            HashSet<Vector3Int> floorCells,
            Grid grid,
            Transform decorContainer,
            PropRegistrySO registry,
            int runSeed,
            float tileSize = 1f)
        {
            if (template == null || floorCells == null || decorContainer == null || registry == null)
            {
                return 0;
            }

            IReadOnlyList<PropDefinitionSO> pool = CollectEligibleProps(registry);
            if (pool.Count == 0)
            {
                return 0;
            }

            CompositionRoleMap roleMap = CompositionRoleMapGenerator.GenerateFromRoom(template);
            MarkSpawnGuards(roleMap, template);
            MarkRewardGuard(roleMap, template);

            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            int mixedSeed = MixSeed(template, runSeed);
            List<BridsonPoissonAutoPlacer.PlacementCandidate> candidates = null;
            for (int attempt = 0; attempt < SeedTries; attempt++)
            {
                List<BridsonPoissonAutoPlacer.PlacementCandidate> tryResult = placer.Generate(
                    template,
                    roleMap,
                    pool,
                    mixedSeed + attempt * 13,
                    TargetDensity);
                if (tryResult.Count > 0)
                {
                    candidates = tryResult;
                    break;
                }
            }

            if (candidates == null || candidates.Count == 0)
            {
                return 0;
            }

            List<PropPlacementData> occupied = new List<PropPlacementData>();
            if (template.props != null)
            {
                occupied.AddRange(template.props);
            }

            int placed = 0;
            for (int i = 0; i < candidates.Count; i++)
            {
                BridsonPoissonAutoPlacer.PlacementCandidate candidate = candidates[i];
                if (candidate.prop == null)
                {
                    continue;
                }

                if (!FootprintOnFloor(candidate.prop, candidate.tilePos, candidate.rotationSteps, floorCells))
                {
                    continue;
                }

                PropFootprintValidator.ValidationResult validation = PropFootprintValidator.Validate(
                    candidate.prop,
                    candidate.tilePos,
                    candidate.rotationSteps,
                    template,
                    roleMap,
                    occupied,
                    out _);
                if (validation != PropFootprintValidator.ValidationResult.Valid)
                {
                    continue;
                }

                PropPlacementData placement = new PropPlacementData(ResolvePropIdentity(candidate.prop), candidate.tilePos)
                {
                    rotationSteps = candidate.rotationSteps,
                    flipX = candidate.flipX,
                    variantIndex = candidate.variantIndex
                };

                GameObject instance = SpawnDecoration(candidate.prop, placement, grid, decorContainer, tileSize);
                if (instance == null)
                {
                    continue;
                }

                occupied.Add(placement);
                placed++;
            }

            return placed;
        }

        private static IReadOnlyList<PropDefinitionSO> CollectEligibleProps(PropRegistrySO registry)
        {
            List<PropDefinitionSO> props = new List<PropDefinitionSO>();
            if (registry == null || registry.AllProps == null)
            {
                return props;
            }

            IReadOnlyList<PropDefinitionSO> allProps = registry.AllProps;
            for (int i = 0; i < allProps.Count; i++)
            {
                PropDefinitionSO prop = allProps[i];
                if (prop == null)
                {
                    continue;
                }

                if (prop.worldSprite == null && (prop.variantSprites == null || prop.variantSprites.Length == 0))
                {
                    continue;
                }

                props.Add(prop);
            }

            return props;
        }

        private static void MarkSpawnGuards(CompositionRoleMap roleMap, RoomTemplateSO template)
        {
            if (roleMap == null || template == null)
            {
                return;
            }

            if (template.playerSpawn != null)
            {
                MarkManhattan(roleMap, template.playerSpawn.position, PlayerSpawnGuardRadius);
            }

            if (template.enemySpawnSockets == null)
            {
                return;
            }

            for (int i = 0; i < template.enemySpawnSockets.Count; i++)
            {
                EnemySpawnSocket socket = template.enemySpawnSockets[i];
                if (socket == null)
                {
                    continue;
                }

                int radius = Mathf.Max(1, Mathf.CeilToInt(socket.avoidRadius));
                MarkManhattan(roleMap, socket.position, radius);
            }
        }

        private static void MarkRewardGuard(CompositionRoleMap roleMap, RoomTemplateSO template)
        {
            if (roleMap == null || template == null)
            {
                return;
            }

            Vector2Int centerCell = new Vector2Int(
                Mathf.RoundToInt(template.bounds.center.x),
                Mathf.RoundToInt(template.bounds.center.y));

            if (!TryFindNearestClearanceCell(template, centerCell, out Vector2Int rewardCell))
            {
                rewardCell = centerCell;
            }

            MarkSquare(roleMap, rewardCell, RewardGuardRadius);
        }

        private static void MarkManhattan(CompositionRoleMap roleMap, Vector2Int center, int radius)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) > radius)
                    {
                        continue;
                    }

                    roleMap.SetRoleAt(new Vector2Int(center.x + dx, center.y + dy), CompositionRole.DoorSafety);
                }
            }
        }

        private static void MarkSquare(CompositionRoleMap roleMap, Vector2Int center, int radius)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    roleMap.SetRoleAt(new Vector2Int(center.x + dx, center.y + dy), CompositionRole.DoorSafety);
                }
            }
        }

        private static bool TryFindNearestClearanceCell(RoomTemplateSO template, Vector2Int origin, out Vector2Int cell)
        {
            cell = default;
            if (template == null)
            {
                return false;
            }

            bool found = false;
            int bestSqr = int.MaxValue;
            RectInt bounds = template.bounds;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!template.IsWalkable(candidate) || !HasWalkableClearance(template, candidate))
                    {
                        continue;
                    }

                    int dx = x - origin.x;
                    int dy = y - origin.y;
                    int sqr = dx * dx + dy * dy;
                    if (sqr < bestSqr)
                    {
                        bestSqr = sqr;
                        cell = candidate;
                        found = true;
                    }
                }
            }

            return found;
        }

        private static bool HasWalkableClearance(RoomTemplateSO template, Vector2Int cell)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (!template.IsWalkable(new Vector2Int(cell.x + dx, cell.y + dy)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool FootprintOnFloor(
            PropDefinitionSO prop,
            Vector2Int tilePosition,
            int rotationSteps,
            HashSet<Vector3Int> floorCells)
        {
            Vector2Int size = PropFootprintValidator.GetRotatedFootprint(prop, rotationSteps);
            if (size.x <= 0 || size.y <= 0)
            {
                return false;
            }

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (!floorCells.Contains(new Vector3Int(tilePosition.x + x, tilePosition.y + y, 0)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static GameObject SpawnDecoration(
            PropDefinitionSO def,
            PropPlacementData placement,
            Grid grid,
            Transform parent,
            float tileSize)
        {
            if (def == null || placement == null || parent == null)
            {
                return null;
            }

            GameObject go = new GameObject($"decor_{def.propId}_{placement.tilePosition.x}_{placement.tilePosition.y}");
            go.transform.SetParent(parent, false);

            Vector3 worldPos;
            if (grid != null)
            {
                Vector3 center = grid.GetCellCenterWorld(new Vector3Int(placement.tilePosition.x, placement.tilePosition.y, 0));
                worldPos = new Vector3(center.x, center.y, 0f);
                go.transform.position = worldPos;
            }
            else
            {
                worldPos = new Vector3(placement.tilePosition.x * tileSize, placement.tilePosition.y * tileSize, 0f);
                go.transform.localPosition = worldPos;
            }

            int normalizedRotation = ((placement.rotationSteps % 4) + 4) % 4;
            go.transform.localRotation = Quaternion.Euler(0f, 0f, -90f * normalizedRotation);

            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = PickSprite(def, placement);
            spriteRenderer.flipX = placement.flipX;

            PropSorterRuntime sorter = go.AddComponent<PropSorterRuntime>();
            sorter.PropDef = def;

            return go;
        }

        private static Sprite PickSprite(PropDefinitionSO def, PropPlacementData placement)
        {
            if (def == null)
            {
                return null;
            }

            if (def.variantSprites == null || def.variantSprites.Length == 0)
            {
                return def.worldSprite;
            }

            int idx = placement.variantIndex;
            if (idx < 0 || idx >= def.variantSprites.Length)
            {
                idx = def.PickVariantIndexForTile(placement.tilePosition);
            }

            Sprite sprite = idx >= 0 && idx < def.variantSprites.Length ? def.variantSprites[idx] : null;
            return sprite != null ? sprite : def.worldSprite;
        }

        private static string ResolvePropIdentity(PropDefinitionSO prop)
        {
            return prop != null ? prop.propId : string.Empty;
        }

        private static int MixSeed(RoomTemplateSO template, int runSeed)
        {
            unchecked
            {
                int hash = runSeed == 0 ? 17 : runSeed;
                string roomId = template != null ? template.roomId : null;
                if (!string.IsNullOrEmpty(roomId))
                {
                    for (int i = 0; i < roomId.Length; i++)
                    {
                        hash = (hash * 31) ^ roomId[i];
                    }
                }

                return hash;
            }
        }
    }
}
