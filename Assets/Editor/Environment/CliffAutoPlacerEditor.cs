using RIMA.Environment;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.Environment
{
    [CustomEditor(typeof(CliffAutoPlacer))]
    public sealed class CliffAutoPlacerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var placer = (CliffAutoPlacer)target;

            EditorGUILayout.Space();
            if (!placer.IsReady)
            {
                EditorGUILayout.HelpBox("Assign a floor Tilemap and CliffPlacementRules asset before regenerating.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.LabelField("Preview Count", placer.CountPreviewPlacements().ToString());
                EditorGUILayout.LabelField("Manual Overrides (erased)", placer.ManualOverrideCells.Count.ToString());
                EditorGUILayout.LabelField("Manual Painted (forced)", placer.ManualPaintedCells.Count.ToString());

                // F1: Adaptive cluster filter status
                EditorGUILayout.Space(4);
                if (placer.ClusterRules != null)
                {
                    EditorGUILayout.HelpBox("Adaptive Filter ENABLED (ClusterRules assigned)", MessageType.Info);
                    int orphanCount = placer.CountOrphanCells();
                    EditorGUILayout.LabelField("Orphan Cell Count", orphanCount.ToString());
                }
                else
                {
                    EditorGUILayout.HelpBox("Adaptive Filter DISABLED — assign ClusterRules to enable.", MessageType.None);
                }
            }

            using (new EditorGUI.DisabledScope(!placer.IsReady))
            {
                if (GUILayout.Button("Regenerate"))
                {
                    placer.Regenerate();
                    EditorUtility.SetDirty(placer.gameObject);
                }
            }
        }
    }
}
