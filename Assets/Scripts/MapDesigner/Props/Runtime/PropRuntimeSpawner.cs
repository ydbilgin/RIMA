using System.Collections.Generic;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Props.Runtime
{
    public sealed class PropRuntimeSpawner
    {
        public sealed class SpawnResult
        {
            public int requested;
            public int spawned;
            public int unresolved;
            public List<GameObject> instances = new List<GameObject>();
        }

        public SpawnResult Spawn(RoomTemplateSO template, PropRegistrySO registry, Transform parent, float tileSize = 1f)
        {
            SpawnResult result = new SpawnResult();
            if (template == null || template.props == null) return result;
            if (registry == null) return result;

            result.requested = template.props.Count;
            for (int i = 0; i < template.props.Count; i++)
            {
                PropPlacementData placement = template.props[i];
                if (placement == null) continue;

                PropDefinitionSO def = registry.ResolveGuid(placement.propDefinitionGuid);
                if (def == null)
                {
                    result.unresolved++;
                    continue;
                }

                GameObject go = SpawnSingle(def, placement, parent, tileSize);
                if (go != null)
                {
                    result.spawned++;
                    result.instances.Add(go);
                }
            }

            return result;
        }

        private static GameObject SpawnSingle(PropDefinitionSO def, PropPlacementData placement, Transform parent, float tileSize)
        {
            if (def == null || placement == null) return null;

            GameObject go = new GameObject($"prop_{def.propId}_{placement.tilePosition.x}_{placement.tilePosition.y}");
            if (parent != null) go.transform.SetParent(parent, false);

            Vector3 worldPos = new Vector3(placement.tilePosition.x * tileSize, placement.tilePosition.y * tileSize, 0f);
            go.transform.localPosition = worldPos;

            int normalizedRotation = ((placement.rotationSteps % 4) + 4) % 4;
            go.transform.localRotation = Quaternion.Euler(0f, 0f, -90f * normalizedRotation);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = PickVariantSprite(def, placement);

            PropSorterRuntime sorter = go.AddComponent<PropSorterRuntime>();
            sorter.PropDef = def;

            if (def.blocksWalkable)
            {
                PropColliderAutoBuilder builder = go.AddComponent<PropColliderAutoBuilder>();
                builder.PropDef = def;
                builder.RotationSteps = normalizedRotation;
                builder.EnsureCollider();
            }

            return go;
        }

        private static Sprite PickVariantSprite(PropDefinitionSO def, PropPlacementData placement)
        {
            if (def == null) return null;
            if (def.variantSprites == null || def.variantSprites.Length == 0) return def.worldSprite;
            int idx = placement.variantIndex;
            if (idx < 0 || idx >= def.variantSprites.Length)
            {
                int seed = unchecked((placement.tilePosition.x * 73856093) ^ (placement.tilePosition.y * 19349663));
                idx = unchecked((seed * 1103515245 + 12345) & int.MaxValue) % def.variantSprites.Length;
            }
            return def.variantSprites[idx] != null ? def.variantSprites[idx] : def.worldSprite;
        }
    }
}
