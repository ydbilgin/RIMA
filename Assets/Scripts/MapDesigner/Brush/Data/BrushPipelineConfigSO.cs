using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "BrushPipelineConfig", menuName = "RIMA/MapDesigner/BrushPipelineConfig")]
    public class BrushPipelineConfigSO : ScriptableObject
    {
        public bool useDataFirstDecals = false;
        public bool useDataFirstScatter = false;
    }
}
