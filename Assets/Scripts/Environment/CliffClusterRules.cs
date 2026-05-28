using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>F1: Adaptive cluster filter rules for CliffAutoPlacer. Orphan (small/isolated) floor
    /// clusters below threshold are excluded from cliff placement, producing cleaner floating-island feel.</summary>
    [CreateAssetMenu(fileName = "CliffClusterRules", menuName = "RIMA/Environment/Cliff Cluster Rules")]
    public sealed class CliffClusterRules : ScriptableObject
    {
        [Tooltip("Minimum cluster size to keep (absolute cell count). Clusters smaller than this AND below coverageRatioFallback are skipped.")]
        public int minClusterSize = 4;

        [Tooltip("Coverage ratio fallback (cluster.Count / floorTotal). If ratio >= this value the cluster is kept regardless of minClusterSize.")]
        [Range(0f, 0.1f)]
        public float coverageRatioFallback = 0.005f;

        [Tooltip("Use iso 8-connectivity BFS (true) or 4-connectivity (false, safe default fallback).")]
        public bool use8Connectivity = true;
    }
}
