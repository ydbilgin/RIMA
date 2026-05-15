using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "BrushPack_New", menuName = "RIMA/Brush/Brush Pack", order = 102)]
    public class BrushPackSO : ScriptableObject
    {
        public string packName;
        public string version = "1.0";
        public List<MapDesignerBrushPresetSO> brushes = new List<MapDesignerBrushPresetSO>();
        public List<AssetPoolSO> referencedPools = new List<AssetPoolSO>();
        public Texture2D coverImage;
    }
}
