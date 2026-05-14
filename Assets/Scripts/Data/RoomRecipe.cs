using System.Collections.Generic;
using RIMA.MapDesigner;
using RIMA.Systems.Map;
using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(fileName = "RoomRecipe", menuName = "RIMA/Map/Room Recipe")]
    public class RoomRecipe : ScriptableObject
    {
        public Vector2Int size = new Vector2Int(16, 12);
        public List<TerrainDefinition> allowedTerrains = new List<TerrainDefinition>();
        public List<EncounterSlot> encounters = new List<EncounterSlot>();
        public RimaBiomePreset biome;
        public PatchAtlasSO patchAtlas;
        public ScatterBrushSO scatterBrush;
        public int seed = 12345;
    }
}
