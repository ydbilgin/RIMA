#if UNITY_EDITOR
using UnityEngine;
using RIMA.Environment;

namespace RIMA.MapDesigner.VisualEditor
{
    public static class LiveAutotiler
    {
        public static void TriggerLiveAutotile()
        {
            // Find CliffAutoPlacer component in the scene and call Regenerate
            CliffAutoPlacer placer = Object.FindAnyObjectByType<CliffAutoPlacer>();
            if (placer != null && placer.IsReady)
            {
                placer.Regenerate();
                // Mark dirty so changes persist in the editor
                if (placer.cliffTilemap != null)
                {
                    UnityEditor.EditorUtility.SetDirty(placer.cliffTilemap.gameObject);
                }
            }
        }
    }
}
#endif
