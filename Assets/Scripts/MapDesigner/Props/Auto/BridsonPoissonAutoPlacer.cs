#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Props.Auto
{
    public sealed class BridsonPoissonAutoPlacer
    {
        public struct PlacementCandidate
        {
            public Vector2Int tilePos;
            public PropDefinitionSO prop;
            public int rotationSteps;
            public bool flipX;
            public int variantIndex;
        }

        private const int CandidatesPerActive = 30;
        private const float TwoPi = 6.28318530717959f;

        public List<PlacementCandidate> Generate(
            RoomTemplateSO template,
            CompositionRoleMap roleMap,
            IReadOnlyList<PropDefinitionSO> propPool,
            int seed,
            float targetDensity)
        {
            List<PlacementCandidate> result = new List<PlacementCandidate>();
            if (template == null || propPool == null || propPool.Count == 0) return result;
            if (targetDensity <= 0f) return result;

            RectInt bounds = template.bounds;
            if (bounds.width <= 0 || bounds.height <= 0) return result;

            CompositionRoleMap effectiveRoleMap = roleMap ?? CompositionRoleMapGenerator.GenerateFromRoom(template);
            float minRadius = ResolveMinRadius(propPool);
            if (minRadius <= 0f) minRadius = 1f;

            System.Random rng = new System.Random(seed);
            List<Vector2> samples = new List<Vector2>();
            List<int> active = new List<int>();
            List<PropPlacementData> simulated = new List<PropPlacementData>();

            Vector2 firstPoint = PickFirstSeed(rng, bounds);
            samples.Add(firstPoint);
            active.Add(0);

            int safety = bounds.width * bounds.height * 4;
            while (active.Count > 0 && safety > 0)
            {
                safety--;
                int activeIdx = rng.Next(active.Count);
                Vector2 origin = samples[active[activeIdx]];

                bool placedFromOrigin = false;
                for (int attempt = 0; attempt < CandidatesPerActive; attempt++)
                {
                    Vector2 candidate = RandomAnnulus(rng, origin, minRadius, minRadius * 2f);
                    Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(candidate.x), Mathf.FloorToInt(candidate.y));
                    if (!IsInsideBounds(bounds, tilePos)) continue;
                    if (!TileDensityRoll(rng, effectiveRoleMap, tilePos, targetDensity)) continue;
                    if (HasSampleWithin(samples, candidate, minRadius)) continue;

                    PropDefinitionSO prop = PickProp(rng, propPool, effectiveRoleMap, tilePos);
                    if (prop == null) continue;

                    int rotation = rng.Next(0, 4);
                    int variant = (prop.variantSprites != null && prop.variantSprites.Length > 0)
                        ? rng.Next(0, prop.variantSprites.Length)
                        : -1;

                    PropFootprintValidator.ValidationResult validation = PropFootprintValidator.Validate(
                        prop,
                        tilePos,
                        rotation,
                        template,
                        effectiveRoleMap,
                        simulated,
                        out _);
                    if (validation != PropFootprintValidator.ValidationResult.Valid) continue;

                    bool flipX = IsMirrorEligible(prop) && rng.Next(0, 2) == 0;
                    samples.Add(candidate);
                    active.Add(samples.Count - 1);
                    simulated.Add(new PropPlacementData(prop.propId, tilePos)
                    {
                        rotationSteps = rotation,
                        flipX = flipX,
                        variantIndex = variant
                    });
                    result.Add(new PlacementCandidate
                    {
                        tilePos = tilePos,
                        prop = prop,
                        rotationSteps = rotation,
                        flipX = flipX,
                        variantIndex = variant
                    });

                    placedFromOrigin = true;
                    break;
                }

                if (!placedFromOrigin)
                {
                    active.RemoveAt(activeIdx);
                }
            }

            return result;
        }

        public static float DensityForRole(CompositionRole role)
        {
            switch (role)
            {
                case CompositionRole.CleanCenter: return 0.1f;
                case CompositionRole.DecoratedEdge: return 1.0f;
                case CompositionRole.FocalCluster: return 2.0f;
                case CompositionRole.WallBand: return 0f;
                case CompositionRole.DoorSafety: return 0f;
                case CompositionRole.Empty: return 0f;
                default: return 0f;
            }
        }

        private static float ResolveMinRadius(IReadOnlyList<PropDefinitionSO> pool)
        {
            float minRadius = float.MaxValue;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i] == null) continue;
                float dist = pool[i].distanceFromOtherProps;
                if (dist > 0f && dist < minRadius) minRadius = dist;
            }
            return minRadius == float.MaxValue ? 1f : minRadius;
        }

        private static Vector2 PickFirstSeed(System.Random rng, RectInt bounds)
        {
            float fx = bounds.xMin + (float)rng.NextDouble() * bounds.width;
            float fy = bounds.yMin + (float)rng.NextDouble() * bounds.height;
            return new Vector2(fx, fy);
        }

        private static Vector2 RandomAnnulus(System.Random rng, Vector2 origin, float minR, float maxR)
        {
            float angle = (float)(rng.NextDouble() * TwoPi);
            float r = Mathf.Sqrt(minR * minR + (float)rng.NextDouble() * (maxR * maxR - minR * minR));
            return new Vector2(origin.x + Mathf.Cos(angle) * r, origin.y + Mathf.Sin(angle) * r);
        }

        private static bool IsInsideBounds(RectInt bounds, Vector2Int tile)
        {
            return tile.x >= bounds.xMin && tile.x < bounds.xMax &&
                tile.y >= bounds.yMin && tile.y < bounds.yMax;
        }

        private static bool TileDensityRoll(System.Random rng, CompositionRoleMap roleMap, Vector2Int tile, float targetDensity)
        {
            CompositionRole role = roleMap != null ? roleMap.GetRoleAt(tile) : CompositionRole.CleanCenter;
            float roleMultiplier = DensityForRole(role);
            if (roleMultiplier <= 0f) return false;

            float probability = Mathf.Clamp01(targetDensity * roleMultiplier);
            return rng.NextDouble() <= probability;
        }

        private static bool HasSampleWithin(List<Vector2> samples, Vector2 candidate, float radius)
        {
            float r2 = radius * radius;
            for (int i = 0; i < samples.Count; i++)
            {
                Vector2 d = samples[i] - candidate;
                if (d.sqrMagnitude < r2) return true;
            }
            return false;
        }

        private static PropDefinitionSO PickProp(System.Random rng, IReadOnlyList<PropDefinitionSO> pool, CompositionRoleMap roleMap, Vector2Int tilePos)
        {
            CompositionRole role = roleMap != null ? roleMap.GetRoleAt(tilePos) : CompositionRole.CleanCenter;
            List<PropDefinitionSO> eligible = new List<PropDefinitionSO>();
            for (int i = 0; i < pool.Count; i++)
            {
                PropDefinitionSO prop = pool[i];
                if (prop == null) continue;
                if (ContainsRole(prop.forbiddenRoles, role)) continue;
                eligible.Add(prop);
            }
            if (eligible.Count == 0) return null;
            return eligible[rng.Next(eligible.Count)];
        }

        private static bool IsMirrorEligible(PropDefinitionSO prop)
        {
            if (prop == null) return false;
            return prop.footprintSize.x == prop.footprintSize.y;
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
    }
}
#endif
