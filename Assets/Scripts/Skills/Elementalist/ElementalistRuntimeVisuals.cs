using UnityEngine;

namespace RIMA
{
    public static class ElementalistRuntimeVisuals
    {
        private static Sprite circleSprite;
        private static Sprite crackSprite;

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

        /// <summary>
        /// Procedural placeholder crack/split decal (white, tintable) for the Broken/Sundered
        /// visual tell. Jagged forked lines on a transparent ground — a 2D "shattered armour"
        /// read. Tinted at the SpriteRenderer (red for Broken, orange-red for Sundered).
        /// TODO: replace with PixelLab crack decal art.
        /// </summary>
        public static Sprite GetCrackSprite()
        {
            if (crackSprite != null) return crackSprite;

            const int size = 32;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            var clear = new Color(1f, 1f, 1f, 0f);
            for (int i = 0; i < size * size; i++) tex.SetPixel(i % size, i / size, clear);

            var white = Color.white;
            // A central forked crack: a near-vertical spine with a couple of diagonal branches.
            DrawLine(tex, 16, 4, 15, 27, white);   // spine
            DrawLine(tex, 16, 16, 6, 24, white);   // left branch
            DrawLine(tex, 16, 16, 26, 23, white);  // right branch
            DrawLine(tex, 15, 9, 22, 6, white);    // upper branch
            DrawLine(tex, 15, 20, 9, 11, white);   // mid-left branch

            tex.Apply();
            crackSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 32f);
            crackSprite.name = "Runtime_Crack_Placeholder";
            return crackSprite;
        }

        // Bresenham-ish line plot with 1px thickness for the placeholder crack.
        private static void DrawLine(Texture2D tex, int x0, int y0, int x1, int y1, Color c)
        {
            int dx = Mathf.Abs(x1 - x0), dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            int guard = 0;
            while (guard++ < 256)
            {
                if (x0 >= 0 && x0 < tex.width && y0 >= 0 && y0 < tex.height)
                    tex.SetPixel(x0, y0, c);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}
