using System;
using RIMA.Data;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Systems.Map;


namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public sealed class MapLayerOrchestrator : MonoBehaviour
    {
        [Serializable]
        public struct LayerToggles
        {
            public bool floorBase;
            public bool floorVariation;
            public bool wallOverlay;
            public bool transitionBrush;
            public bool detailDecal;
            public bool accent;
        }

        [SerializeField]
        private LayerToggles layers = new LayerToggles
        {
            floorBase = true,
            floorVariation = true,
            wallOverlay = true,
            transitionBrush = false,
            detailDecal = false,
            accent = false
        };

        [Header("Shader Blend")]
        [SerializeField] private bool useShaderBlend = false;
        [SerializeField] private TerrainBlendConfig blendConfig;

        public LayerToggles Layers
        {
            get => layers;
            set => layers = value;
        }

        public bool ValidateWalkableMask(RoomData room)
        {
            return room.walkable != null &&
                room.walkable.GetLength(0) == room.size.x &&
                room.walkable.GetLength(1) == room.size.y;
        }

public void Paint(Tilemap floorTilemap, RimaBiomePreset biome, RoomData room, int seed)
        {
            if (room.vertexGrid != null && (layers.floorBase || layers.floorVariation))
            {
                if (useShaderBlend && blendConfig != null && blendConfig.blendMaterial != null)
                {
                    var blendRenderer = EnsureComponent<RIMA.Systems.Map.TerrainBlendRenderer>(transform, "TerrainBlendRenderer");
                    blendRenderer.Render(room, biome, blendConfig);
                }
                else if (floorTilemap != null && biome != null)
                {
                    CornerWangPainter.Paint(floorTilemap, biome, room.vertexGrid, room.size.x, room.size.y, default, seed, false);
                }
            }

            if (!ValidateWalkableMask(room))
            {
                Debug.LogWarning("[MapLayerOrchestrator] Room walkable mask is missing or wrong size.");
                return;
            }

            Transform host = floorTilemap != null && floorTilemap.transform.parent != null ? floorTilemap.transform.parent : transform;
            if (NaturalFeatureGraph.HasFeatureData(room.naturalFeatures))
            {
                EnsureComponent<FeatureEdgeSmoothingPass>(host, "FeatureEdgeSmoothingPass").Paint(floorTilemap, biome, room, room.featureEdgeSmoothingProfile, seed);
            }

            if (layers.wallOverlay && room.wallBrushSet != null)
            {
                EnsureComponent<WallOverlayPainter>(host, "WallOverlayPainter").PaintWalls(room, room.wallBrushSet, floorTilemap, seed);
            }

            if (layers.transitionBrush && room.transitionAtlas != null)
            {
                EnsureComponent<TransitionBrushPainter>(host, "TransitionBrushPainter").PaintTransitions(floorTilemap, room, room.transitionAtlas, seed);
            }

            if (layers.detailDecal && room.decalAtlas != null)
            {
                EnsureComponent<DetailDecalPainter>(host, "DetailDecalPainter").PaintDetails(floorTilemap, room, room.decalAtlas, seed);
            }

            if (layers.accent && room.accentAtlas != null)
            {
                EnsureComponent<AccentPainter>(host, "AccentPainter").PaintAccents(floorTilemap, room, room.accentAtlas, seed);
            }
        }

        private static T EnsureComponent<T>(Transform parent, string objectName) where T : Component
        {
            T existing = parent.GetComponentInChildren<T>(true);
            if (existing != null)
            {
                return existing;
            }

            GameObject child = new GameObject(objectName);
            child.transform.SetParent(parent, false);
            return child.AddComponent<T>();
        }
    }
}
