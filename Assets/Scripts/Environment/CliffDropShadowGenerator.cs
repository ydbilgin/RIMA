using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// F2: Procedural alpha-gradient drop shadow sprite for cliff edges.
    /// 32x16 px texture: top row = alpha 0.6, bottom row = alpha 0 (linear fade).
    /// Singleton cache — DontSave, recreated on domain reload.
    /// Pattern: GroundBlobShadow.cs texture/sprite generation approach.
    /// </summary>
    public static class CliffDropShadowGenerator
    {
        private const int TexW = 32;
        private const int TexH = 16;
        private const float TopAlpha = 0.6f;

        private static Sprite _cachedSprite;

        /// <summary>Returns the shared shadow sprite, creating it on first call.</summary>
        public static Sprite GetSprite()
        {
            if (_cachedSprite != null) return _cachedSprite;

            var tex = new Texture2D(TexW, TexH, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode   = TextureWrapMode.Clamp,
                name       = "RIMA_CliffDropShadow_Tex"
            };

            // Row 0 = bottom (y=0), row TexH-1 = top.
            // Unity Texture2D pixel origin is bottom-left.
            // We want top of texture (high y) = opaque, bottom (y=0) = transparent.
            for (int y = 0; y < TexH; y++)
            {
                // t: 0 at bottom, 1 at top
                float t     = y / (float)(TexH - 1);
                float alpha = Mathf.Lerp(0f, TopAlpha, t);

                for (int x = 0; x < TexW; x++)
                    tex.SetPixel(x, y, new Color(0f, 0f, 0f, alpha));
            }

            tex.Apply();
            // F2 fix: removed tex.hideFlags = HideFlags.DontSave so scene serialises the asset
            // and shadow tiles survive cold scene reopen without waiting for OnEnable→Regenerate.

            // Pivot at top-centre so the shadow hangs DOWN from the cliff cell position.
            _cachedSprite = Sprite.Create(
                tex,
                new Rect(0, 0, TexW, TexH),
                new Vector2(0.5f, 1f),   // pivot: top-centre
                TexW                      // PPU = texture width → 1 unit wide
            );
            _cachedSprite.name     = "RIMA_CliffDropShadow";
            // F2 fix: removed _cachedSprite.hideFlags = HideFlags.DontSave — same reason as tex above.

            return _cachedSprite;
        }

        /// <summary>Call on domain reload / play-mode transition to force recreation.</summary>
        public static void InvalidateCache()
        {
            _cachedSprite = null;
        }
    }
}
