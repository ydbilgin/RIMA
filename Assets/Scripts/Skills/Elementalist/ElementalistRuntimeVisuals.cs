using UnityEngine;

namespace RIMA
{
    public static class ElementalistRuntimeVisuals
    {
        private static Sprite circleSprite;

        public static Sprite GetCircleSprite()
        {
            if (circleSprite != null) return circleSprite;

            const int size = 32;
            const float radius = 14f;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            var clear = new Color(1f, 1f, 1f, 0f);
            var fill = Color.white;
            Vector2 center = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float d = Vector2.Distance(new Vector2(x, y), center);
                    tex.SetPixel(x, y, d <= radius ? fill : clear);
                }
            }

            tex.Apply();
            circleSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 32f);
            circleSprite.name = "Elementalist_Runtime_Circle";
            return circleSprite;
        }
    }
}
