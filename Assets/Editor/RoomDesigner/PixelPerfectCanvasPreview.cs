namespace RIMA.Editor.RoomDesigner
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering.Universal;
    using UnityEngine.UIElements;
    using Button = UnityEngine.UIElements.Button;
    using Object = UnityEngine.Object;

    internal sealed class PixelPerfectCanvasPreview
    {
        private const int TilePixels = 32;
        private const int RefWidth = 320;
        private const int RefHeight = 180;
        private const int PPU = 64;

        private readonly IRoomDesignerContext ctx;
        private bool isActive;
        private int zoomLevel = 2;
        private Button labelRef;

        public PixelPerfectCanvasPreview(IRoomDesignerContext ctx)
        {
            this.ctx = ctx;
        }

        public void SetLabelRef(Button btn)
        {
            labelRef = btn;
            UpdateLabel();
        }

        public void Toggle()
        {
            isActive = !isActive;
            UpdateLabel();
        }

        public void DrawOverlay(Rect canvasRect)
        {
            if (!isActive || canvasRect.width <= 0f || canvasRect.height <= 0f)
            {
                return;
            }

            int cellSize = TilePixels * zoomLevel;

            Handles.BeginGUI();
            Color previousColor = GUI.color;
            GUI.color = new Color(0x1A / 255f, 0x1C / 255f, 0x20 / 255f, 0.6f);

            for (float x = canvasRect.xMin; x <= canvasRect.xMax; x += cellSize)
            {
                GUI.DrawTexture(new Rect(x, canvasRect.yMin, 1f, canvasRect.height), Texture2D.whiteTexture);
            }

            for (float y = canvasRect.yMin; y <= canvasRect.yMax; y += cellSize)
            {
                GUI.DrawTexture(new Rect(canvasRect.xMin, y, canvasRect.width, 1f), Texture2D.whiteTexture);
            }

            GUI.color = previousColor;

            Rect labelRect = new Rect(canvasRect.xMin + 8f, canvasRect.yMin + 8f, 190f, 22f);
            GUI.Label(labelRect, "RefCam: 320x180 @ PPU=64", EditorStyles.whiteLabel);

            const float buttonWidth = 32f;
            const float buttonHeight = 22f;
            const float buttonGap = 4f;
            float groupWidth = buttonWidth * 4f + buttonGap * 3f;
            float startX = canvasRect.xMax - groupWidth - 8f;
            float yPos = canvasRect.yMin + 8f;

            for (int i = 1; i <= 4; i++)
            {
                Rect buttonRect = new Rect(startX + (i - 1) * (buttonWidth + buttonGap), yPos, buttonWidth, buttonHeight);
                if (GUI.Button(buttonRect, i + "x"))
                {
                    zoomLevel = i;
                    ctx.MarkDirty();
                }
            }

            Handles.EndGUI();
        }

        public void RefreshPixelPerfectCamera()
        {
            var cameras = Object.FindObjectsByType<PixelPerfectCamera>(FindObjectsSortMode.None);
            foreach (var camera in cameras)
            {
                camera.refResolutionX = RefWidth;
                camera.refResolutionY = RefHeight;
                camera.assetsPPU = PPU;
                camera.cropFrameX = true;
                camera.cropFrameY = true;
                camera.stretchFill = false;
            }

            Debug.Log("PixelPerfectCamera refreshed");
        }

        public void Dispose()
        {
        }

        private void UpdateLabel()
        {
            if (labelRef != null)
            {
                labelRef.text = isActive ? "[Pixel-Perfect Preview] ON" : "[Pixel-Perfect Preview] OFF";
            }
        }
    }
}
