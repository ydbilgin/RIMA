using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    public static class RimaGeneratedSpriteCache
    {
        private static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        public static Sprite Load(string resourcePath, float pixelsPerUnit = 100f)
        {
            if (string.IsNullOrWhiteSpace(resourcePath)) return null;
            if (Cache.TryGetValue(resourcePath, out Sprite cached)) return cached;

            Texture2D texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                Debug.LogWarning($"[RimaGeneratedSpriteCache] Missing Resources texture: {resourcePath}");
                return null;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                pixelsPerUnit,
                0,
                SpriteMeshType.FullRect);

            Cache[resourcePath] = sprite;
            return sprite;
        }
    }
}
