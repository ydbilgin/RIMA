using RIMA.RoomPainter;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA.RoomPainter.Editor
{
    internal sealed class RoomPainterAssetBindingPostBuildStripper : IPreprocessBuildWithReport, IProcessSceneWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
        }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                RoomPainterAssetBinding[] bindings = roots[i].GetComponentsInChildren<RoomPainterAssetBinding>(true);
                for (int j = 0; j < bindings.Length; j++)
                {
                    Object.DestroyImmediate(bindings[j]);
                }
            }
        }
    }
}
