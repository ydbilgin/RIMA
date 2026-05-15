using RIMA.MapDesigner;
using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(fileName = "FeatureMask", menuName = "RIMA/Map/Feature Mask")]
    public class FeatureMaskSO : ScriptableObject
    {
        public Texture2D alphaMask;
        public AnimationCurve remap = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public bool invert;
        public Vector2 worldOffset = Vector2.zero;
        public Vector2 scale = Vector2.one;

        public float Sample(Vector2Int cell, Vector2Int roomSize)
        {
            if (alphaMask == null || roomSize.x <= 0 || roomSize.y <= 0)
            {
                return 1f;
            }

            Vector2 safeScale = scale == Vector2.zero ? Vector2.one : scale;
            float u = (cell.x + 0.5f) / roomSize.x * safeScale.x + worldOffset.x;
            float v = (cell.y + 0.5f) / roomSize.y * safeScale.y + worldOffset.y;
            float alpha = alphaMask.GetPixelBilinear(u, v).a;
            if (invert)
            {
                alpha = 1f - alpha;
            }

            return remap != null && remap.keys != null && remap.keys.Length > 0 ? remap.Evaluate(alpha) : alpha;
        }
    }
}
