using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "Brush_New", menuName = "RIMA/Brush/Brush Preset", order = 101)]
    public class MapDesignerBrushPresetSO : ScriptableObject
    {
        public string brushName;
        public BrushCategory category;
        public PaintMode paintMode;
        public List<BrushLayerOperation> operations = new List<BrushLayerOperation>();
        public Sprite previewIcon;
        public bool showInPalette = true;
        [TextArea(2, 5)] public string description;
        [Range(-1, 9)] public int hotkeyIndex = -1;
        public BrushPipelineConfigSO pipelineConfig;
    }
}
