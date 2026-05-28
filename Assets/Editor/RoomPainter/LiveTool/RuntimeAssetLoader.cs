using UnityEditor;
using UnityEngine;
using RIMA.Live;

namespace RIMA.Editor.RoomPainter.LiveTool
{
    /// <summary>
    /// C9 — RuntimeAssetLoader (F3).
    /// Editor-side registry consumer: loads the baked RuntimeAssetRegistry from its
    /// Resources path and caches it for the palette window.
    /// Isolated in its own class so Tool.exe (runtime) can swap in a Resources.Load
    /// implementation without touching palette logic.
    /// </summary>
    public static class RuntimeAssetLoader
    {
        private const string RegistryAssetPath = "Assets/Resources/Live/RuntimeAssetRegistry.asset";

        private static RuntimeAssetRegistry _cached;
        private static bool _attempted;

        /// <summary>
        /// Load and return the baked RuntimeAssetRegistry.
        /// Returns null (never throws) if the asset does not exist yet.
        /// Call Reload() to force a fresh load after a bake.
        /// </summary>
        public static RuntimeAssetRegistry Load()
        {
            if (_attempted && _cached != null) return _cached;

            _attempted = true;
            _cached = AssetDatabase.LoadAssetAtPath<RuntimeAssetRegistry>(RegistryAssetPath);

            if (_cached == null)
                Debug.LogWarning("[RuntimeAssetLoader] Registry not found at " + RegistryAssetPath +
                                 ". Run RIMA → Live Tool → Bake Asset Registry first.");

            return _cached;
        }

        /// <summary>
        /// Evict the cached instance so the next Load() call re-reads from disk.
        /// Call this after baking (the baker triggers AssetDatabase.Refresh automatically).
        /// </summary>
        public static void Reload()
        {
            _cached = null;
            _attempted = false;
        }
    }
}
