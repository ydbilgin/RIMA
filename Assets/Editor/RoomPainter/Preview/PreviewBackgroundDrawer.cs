using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal static class PreviewBackgroundDrawer
    {
        private const int CheckerSize = 16;
        private static Texture2D _checkerTexture;

        public static void Draw(Rect area, int zoom, Vector2 pan)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            EditorGUI.DrawRect(area, new Color(0.10f, 0.10f, 0.12f, 1f));
            Texture2D checker = GetCheckerTexture();
            float tileSize = CheckerSize * Mathf.Max(1, zoom);
            Rect texCoords = new Rect(
                -pan.x / tileSize,
                -pan.y / tileSize,
                area.width / tileSize,
                area.height / tileSize);

            GUI.DrawTextureWithTexCoords(area, checker, texCoords, true);
        }

        private static Texture2D GetCheckerTexture()
        {
            if (_checkerTexture != null)
            {
                return _checkerTexture;
            }

            _checkerTexture = new Texture2D(CheckerSize, CheckerSize)
            {
                hideFlags = HideFlags.HideAndDontSave,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Repeat
            };

            Color dark = new Color(0.12f, 0.12f, 0.14f, 1f);
            Color light = new Color(0.18f, 0.18f, 0.21f, 1f);
            int half = CheckerSize / 2;

            for (int y = 0; y < CheckerSize; y++)
            {
                for (int x = 0; x < CheckerSize; x++)
                {
                    bool alternate = (x < half && y < half) || (x >= half && y >= half);
                    _checkerTexture.SetPixel(x, y, alternate ? light : dark);
                }
            }

            _checkerTexture.Apply();
            return _checkerTexture;
        }
    }
}
