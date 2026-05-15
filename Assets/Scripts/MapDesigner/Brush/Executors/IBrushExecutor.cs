using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors
{
    public interface IBrushExecutor
    {
        PaintMode SupportedMode { get; }
        BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op);
    }

    [Serializable]
    public struct BrushExecutorResult
    {
        public bool success;
        public int spawnedCount;
        public List<GameObject> spawnedObjects;
        public List<UnityEngine.Object> modifiedAssets;
        public string errorMessage;
    }
}
