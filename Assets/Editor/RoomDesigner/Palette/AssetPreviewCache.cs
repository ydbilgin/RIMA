namespace RIMA.Editor.RoomDesigner.Palette
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public sealed class AssetPreviewCache : IDisposable
    {
        private static readonly HashSet<AssetPreviewCache> LiveCaches = new HashSet<AssetPreviewCache>();

        private readonly Dictionary<int, Texture2D> cache = new Dictionary<int, Texture2D>();
        private readonly Action onAnyLoaded;
        private bool disposed;

        public AssetPreviewCache(Action repaintCallback)
        {
            onAnyLoaded = repaintCallback;
            LiveCaches.Add(this);
            EditorApplication.update += Tick;
        }

        public Texture2D Get(Object asset)
        {
            if (asset == null)
            {
                return null;
            }

            int id = asset.GetInstanceID();
            if (cache.TryGetValue(id, out Texture2D tex) && tex != null)
            {
                return tex;
            }

            tex = AssetPreview.GetAssetPreview(asset) ?? AssetPreview.GetMiniThumbnail(asset);
            if (tex != null)
            {
                cache[id] = tex;
            }

            return tex;
        }

        public void Invalidate()
        {
            cache.Clear();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            EditorApplication.update -= Tick;
            LiveCaches.Remove(this);
            cache.Clear();
        }

        internal static void InvalidateAll()
        {
            foreach (AssetPreviewCache liveCache in LiveCaches)
            {
                liveCache.Invalidate();
            }
        }

        private void Tick()
        {
            if (AssetPreview.IsLoadingAssetPreviews())
            {
                onAnyLoaded?.Invoke();
            }
        }
    }

    internal sealed class AssetPreviewCachePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            AssetPreviewCache.InvalidateAll();
        }
    }
}
