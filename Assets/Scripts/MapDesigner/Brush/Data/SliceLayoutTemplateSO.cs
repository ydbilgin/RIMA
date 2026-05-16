using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "SliceLayoutTemplate_New", menuName = "RIMA/Brush/Slice Layout Template", order = 110)]
    public class SliceLayoutTemplateSO : ScriptableObject
    {
        public string templateName;
        public Vector2Int masterSize;
        public List<SliceCell> cells = new List<SliceCell>();
        public int gutterSize = 16;
        public Vector2 defaultPivot = new Vector2(0.5f, 0.5f);
        public bool wangAware;
    }

    [Serializable]
    public class SliceCell
    {
        public string cellName;
        public RectInt rect;
        public SizeBucket bucket;
        public string[] tags;
        public Vector2 pivotOverride = new Vector2(0.5f, 0.5f);
        public bool usePivotOverride;
        public bool heroAllowed;
    }
}
