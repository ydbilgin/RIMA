#if UNITY_EDITOR
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class StampExecutor : IBrushExecutor
    {
        private readonly WallStampExecutor wallStampExecutor = new WallStampExecutor();

        public PaintMode SupportedMode
        {
            get { return PaintMode.Stamp; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            if (op != null && op.targetLayer == TargetLayer.L3)
            {
                return wallStampExecutor.Apply(stroke, op);
            }

            return DecorativeExecutorUtility.PlaceSingle(stroke, op, "Brush Decal Stamp", 0);
        }
    }
}
#endif
