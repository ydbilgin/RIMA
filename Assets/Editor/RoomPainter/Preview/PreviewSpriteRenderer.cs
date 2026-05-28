using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal struct PreviewSpriteDrawInfo
    {
        public bool IsValid;
        public Rect DrawRect;
        public Rect TextureCoords;
        public Vector2 PivotGui;
        public Vector2 PixelSize;
        public Texture Texture;
        public Sprite Sprite;
        public string Status;
        public bool PrefabFallback;
    }

    internal static class PreviewSpriteRenderer
    {
        public static PreviewSpriteDrawInfo Prepare(
            Rect canvas,
            AssetEntry selectedAsset,
            int zoom,
            Vector2 pan,
            RoomLayer targetLayer)
        {
            RoomPainterAsset metadata = selectedAsset.metadata;
            Sprite sprite = metadata != null && metadata.sprite != null ? metadata.sprite : selectedAsset.sprite;
            Texture texture = null;
            Rect textureCoords = new Rect(0f, 0f, 1f, 1f);
            Vector2 pixelSize = new Vector2(128f, 128f);
            bool prefabFallback = false;

            if (sprite != null && sprite.texture != null)
            {
                texture = sprite.texture;
                Rect textureRect = sprite.textureRect;
                textureCoords = new Rect(
                    textureRect.x / texture.width,
                    textureRect.y / texture.height,
                    textureRect.width / texture.width,
                    textureRect.height / texture.height);
                pixelSize = sprite.rect.size;
            }
            else if (selectedAsset.prefab != null)
            {
                prefabFallback = PrefabUtility.GetPrefabAssetType(selectedAsset.prefab) != PrefabAssetType.NotAPrefab;
                texture = AssetPreview.GetAssetPreview(selectedAsset.prefab);
                if (texture == null)
                {
                    texture = AssetPreview.GetMiniThumbnail(selectedAsset.prefab);
                }

                if (texture != null)
                {
                    pixelSize = new Vector2(texture.width, texture.height);
                }
            }
            else if (selectedAsset.AssetObject != null)
            {
                texture = AssetPreview.GetAssetPreview(selectedAsset.AssetObject);
                if (texture == null)
                {
                    texture = AssetPreview.GetMiniThumbnail(selectedAsset.AssetObject);
                }

                if (texture != null)
                {
                    pixelSize = new Vector2(texture.width, texture.height);
                }
            }

            if (texture == null)
            {
                return new PreviewSpriteDrawInfo
                {
                    IsValid = false,
                    Status = "No sprite preview available"
                };
            }

            float atmosphericScale = targetLayer == RoomLayer.Parallax ? 0.95f : 1f;
            float effectiveScale = zoom <= 0
                ? ComputeFitScale(canvas, pixelSize)
                : zoom;
            Vector2 drawSize = pixelSize * effectiveScale * atmosphericScale;
            Vector2 center = canvas.center + pan;
            Rect drawRect = new Rect(center.x - drawSize.x * 0.5f, center.y - drawSize.y * 0.5f, drawSize.x, drawSize.y);
            Vector2 normalizedPivot = GetNormalizedPivot(metadata, sprite);
            Vector2 pivotGui = new Vector2(
                drawRect.xMin + normalizedPivot.x * drawRect.width,
                drawRect.yMin + (1f - normalizedPivot.y) * drawRect.height);

            string status = sprite != null
                ? sprite.name + " " + Mathf.RoundToInt(pixelSize.x) + "x" + Mathf.RoundToInt(pixelSize.y)
                : "Prefab thumbnail" + (prefabFallback ? " (asset)" : string.Empty);

            return new PreviewSpriteDrawInfo
            {
                IsValid = true,
                DrawRect = drawRect,
                TextureCoords = textureCoords,
                PivotGui = pivotGui,
                PixelSize = pixelSize,
                Texture = texture,
                Sprite = sprite,
                Status = status,
                PrefabFallback = prefabFallback
            };
        }

        public static void Draw(PreviewSpriteDrawInfo info, float rotationDegrees)
        {
            if (!info.IsValid || info.Texture == null)
            {
                return;
            }

            Matrix4x4 previousMatrix = GUI.matrix;
            if (!Mathf.Approximately(rotationDegrees, 0f))
            {
                GUIUtility.RotateAroundPivot(rotationDegrees, info.DrawRect.center);
            }

            GUI.DrawTextureWithTexCoords(info.DrawRect, info.Texture, info.TextureCoords, true);
            GUI.matrix = previousMatrix;
        }

        private static float ComputeFitScale(Rect canvas, Vector2 pixelSize)
        {
            if (pixelSize.x <= 0f || pixelSize.y <= 0f)
            {
                return 1f;
            }

            float marginedWidth = Mathf.Max(8f, canvas.width * 0.88f);
            float marginedHeight = Mathf.Max(8f, canvas.height * 0.82f);
            float fit = Mathf.Min(marginedWidth / pixelSize.x, marginedHeight / pixelSize.y);
            return Mathf.Clamp(fit, 0.1f, 16f);
        }

        private static Vector2 GetNormalizedPivot(RoomPainterAsset metadata, Sprite sprite)
        {
            if (metadata != null)
            {
                return new Vector2(
                    Mathf.Clamp01(metadata.pivotAnchor.x),
                    Mathf.Clamp01(metadata.pivotAnchor.y));
            }

            if (sprite != null && sprite.rect.width > 0f && sprite.rect.height > 0f)
            {
                return new Vector2(
                    Mathf.Clamp01(sprite.pivot.x / sprite.rect.width),
                    Mathf.Clamp01(sprite.pivot.y / sprite.rect.height));
            }

            return new Vector2(0.5f, 0f);
        }
    }
}
