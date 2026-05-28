using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RoomPainterPreviewPane
    {
        private const string PreviewPaneZoomKey = "RIMA.RoomPainter.PreviewPane.Zoom";
        private const string PreviewPaneShow3DKey = "RIMA.RoomPainter.PreviewPane.Show3D";
        private const string PreviewPaneRotationKey = "RIMA.RoomPainter.PreviewPane.Rotation";
        private const float HeaderHeight = 24f;
        private const float FooterHeight = 48f;
        private const int FitZoom = 0;
        private const int MaxZoom = 8;
        private static readonly string[] ZoomLabels = { "Fit", "1x", "2x", "3x", "4x", "5x", "6x", "8x" };
        private static readonly int[] ZoomValues = { 0, 1, 2, 3, 4, 5, 6, 8 };

        private readonly PreviewInputHandler _inputHandler = new PreviewInputHandler();
        private int _zoom;
        private Vector2 _pan;
        private float _rotationOverride;
        private bool _show3DMock;

        public RoomPainterPreviewPane()
        {
            _zoom = ClampZoom(EditorPrefs.GetInt(PreviewPaneZoomKey, FitZoom));
            _rotationOverride = EditorPrefs.GetFloat(PreviewPaneRotationKey, 0f);
            _show3DMock = EditorPrefs.GetBool(PreviewPaneShow3DKey, true);
        }

        public void Draw(Rect area, AssetEntry selectedAsset, RoomLayer targetLayer, float currentRotation)
        {
            if (area.width <= 1f || area.height <= HeaderHeight + FooterHeight)
            {
                return;
            }

            EditorGUI.DrawRect(area, new Color(0.08f, 0.08f, 0.09f, 1f));

            Rect headerRect = new Rect(area.x, area.y, area.width, HeaderHeight);
            Rect footerRect = new Rect(area.x, area.yMax - FooterHeight, area.width, FooterHeight);
            Rect canvasRect = new Rect(area.x, headerRect.yMax, area.width, area.height - HeaderHeight - FooterHeight);
            bool hasSelection = !string.IsNullOrEmpty(selectedAsset.path);
            RoomPainterAsset metadata = selectedAsset.metadata;

            DrawHeader(headerRect, selectedAsset, hasSelection);

            if (_inputHandler.Handle(canvasRect, ref _zoom, ref _pan, ref _rotationOverride))
            {
                _zoom = ClampZoom(_zoom);
                PersistTransientState();
                GUI.changed = true;
                EditorWindow.focusedWindow?.Repaint();
            }

            GUI.BeginGroup(canvasRect);
            Rect localCanvas = new Rect(0f, 0f, canvasRect.width, canvasRect.height);
            PreviewBackgroundDrawer.Draw(localCanvas, _zoom, _pan);

            if (hasSelection)
            {
                PreviewSpriteDrawInfo spriteInfo = PreviewSpriteRenderer.Prepare(localCanvas, selectedAsset, _zoom, _pan, targetLayer);
                if (spriteInfo.IsValid)
                {
                    PreviewOverlayRenderer.DrawDepthUnderlay(localCanvas, spriteInfo, metadata, targetLayer, _show3DMock);
                    PreviewSpriteRenderer.Draw(spriteInfo, currentRotation + _rotationOverride);
                    PreviewOverlayRenderer.DrawDepthOverlay(localCanvas, spriteInfo, metadata, targetLayer, _show3DMock);
                    PreviewOverlayRenderer.DrawTopOverlays(localCanvas, spriteInfo, metadata);
                }
                else
                {
                    DrawCenteredHelp(localCanvas, spriteInfo.Status);
                }
            }
            else
            {
                DrawCenteredHelp(localCanvas, "Select an asset in the palette to see preview");
            }

            GUI.EndGroup();
            DrawFooter(footerRect, selectedAsset, hasSelection, targetLayer);
        }

        private void DrawHeader(Rect rect, AssetEntry selectedAsset, bool hasSelection)
        {
            Rect titleRect = new Rect(rect.x + 8f, rect.y + 3f, rect.width * 0.45f, rect.height - 4f);
            GUI.Label(titleRect, "Live Preview", EditorStyles.boldLabel);

            if (!hasSelection)
            {
                return;
            }

            string name = selectedAsset.AssetObject != null ? selectedAsset.AssetObject.name : selectedAsset.path;
            Rect detailRect = new Rect(rect.x + rect.width * 0.45f, rect.y + 4f, rect.width * 0.55f - 8f, rect.height - 4f);
            GUI.Label(detailRect, name, EditorStyles.miniLabel);
        }

        private void DrawFooter(Rect rect, AssetEntry selectedAsset, bool hasSelection, RoomLayer targetLayer)
        {
            GUI.Box(rect, GUIContent.none, EditorStyles.toolbar);

            float zoomWidth = Mathf.Clamp(rect.width - 360f, 200f, 320f);
            Rect zoomLabelRect = new Rect(rect.x + 6f, rect.y + 4f, 34f, 18f);
            Rect zoomRect = new Rect(zoomLabelRect.xMax + 4f, rect.y + 3f, zoomWidth, 20f);
            Rect toggleRect = new Rect(zoomRect.xMax + 8f, rect.y + 3f, 104f, 20f);
            Rect hintRect = new Rect(rect.xMax - 176f, rect.y + 5f, 170f, 18f);
            Rect statusRect = new Rect(rect.x + 6f, rect.y + 26f, rect.width - 12f, 18f);

            GUI.Label(zoomLabelRect, "Zoom", EditorStyles.miniLabel);
            EditorGUI.BeginChangeCheck();
            int selectedZoomIndex = GUI.Toolbar(zoomRect, IndexFromZoom(_zoom), ZoomLabels, EditorStyles.toolbarButton);
            if (EditorGUI.EndChangeCheck())
            {
                _zoom = ZoomFromIndex(selectedZoomIndex);
                PersistTransientState();
            }

            EditorGUI.BeginChangeCheck();
            _show3DMock = GUI.Toggle(toggleRect, _show3DMock, "Show 3D mock", EditorStyles.toolbarButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(PreviewPaneShow3DKey, _show3DMock);
            }

            GUI.Label(hintRect, "Wheel zoom | MMB pan | R rotate | 0 fit", EditorStyles.miniLabel);

            string status = hasSelection
                ? "Layer: " + (selectedAsset.metadata != null ? selectedAsset.metadata.defaultLayer : targetLayer) + "  |  " + DescribeZoom(_zoom)
                : "No selection";
            GUI.Label(statusRect, status, EditorStyles.miniLabel);
        }

        private void DrawCenteredHelp(Rect canvas, string message)
        {
            Rect boxRect = new Rect(canvas.center.x - 150f, canvas.center.y - 24f, 300f, 48f);
            GUI.Label(boxRect, message, EditorStyles.helpBox);
        }

        private void PersistTransientState()
        {
            EditorPrefs.SetInt(PreviewPaneZoomKey, _zoom);
            EditorPrefs.SetFloat(PreviewPaneRotationKey, _rotationOverride);
        }

        private static int ClampZoom(int zoom)
        {
            return Mathf.Clamp(zoom, FitZoom, MaxZoom);
        }

        private static int IndexFromZoom(int zoom)
        {
            for (int i = 0; i < ZoomValues.Length; i++)
            {
                if (ZoomValues[i] == zoom)
                {
                    return i;
                }
            }

            if (zoom == 7)
            {
                return 6;
            }

            return zoom <= FitZoom ? 0 : ZoomValues.Length - 1;
        }

        private static int ZoomFromIndex(int index)
        {
            if (index < 0 || index >= ZoomValues.Length)
            {
                return FitZoom;
            }

            return ZoomValues[index];
        }

        private static string DescribeZoom(int zoom)
        {
            return zoom <= FitZoom ? "Zoom: Fit" : "Zoom: " + zoom + "x";
        }
    }
}
