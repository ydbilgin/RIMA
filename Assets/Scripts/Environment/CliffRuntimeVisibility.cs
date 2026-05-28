using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    /// <summary>
    /// F6: Runtime culling stub for cliff tilemaps.
    /// Attaches to a CliffTilemap GameObject and enables Unity's built-in
    /// TilemapRenderer chunk culling so off-screen cliff chunks are not rendered.
    /// No custom frustum math — Unity handles it via detectChunkCullingBounds.
    /// </summary>
    [RequireComponent(typeof(TilemapRenderer))]
    [AddComponentMenu("RIMA/Environment/Cliff Runtime Visibility")]
    public sealed class CliffRuntimeVisibility : MonoBehaviour
    {
        [Header("Culling")]
        [Tooltip("Enable Unity's built-in chunk culling bounds detection. Default: on.")]
        [SerializeField] private bool enableCulling = true;

        [Header("Culling Extensions (optional)")]
        [Tooltip("World-space padding added to each chunk's culling bounds. Increase if tiles pop in at edges.")]
        [SerializeField] private Vector3 cullingExtensions = new Vector3(2f, 2f, 0f);

        private TilemapRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<TilemapRenderer>();
            Apply();
        }

        // Allow toggling at runtime from Inspector without re-entering play mode.
        private void OnValidate() => Apply();

        private void Apply()
        {
            if (_renderer == null)
                _renderer = GetComponent<TilemapRenderer>();
            if (_renderer == null) return;

            _renderer.detectChunkCullingBounds = enableCulling
                ? TilemapRenderer.DetectChunkCullingBounds.Auto
                : TilemapRenderer.DetectChunkCullingBounds.Manual;

            _renderer.chunkCullingBounds = cullingExtensions;
        }
    }
}
