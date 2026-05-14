using RIMA;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [System.Serializable]
    public class TilesetPairing
    {
        public int lowerTerrainId;
        public int upperTerrainId;
        public CornerWangTileSetSO tileSet;

        [Range(0f, 1f)]
        [Tooltip("PixelLab transition size: 0.0 = zemin<->zemin (no elevation), 0.1 = dar seam, 0.25 = organik wide blend, 0.5+ = cok genis")]
        public float transitionSize = 0.25f;

        [TextArea(2, 4)]
        [Tooltip("PixelLab transition prompt - re-generation icin referans")]
        public string transitionDescription = "";
    }
}
