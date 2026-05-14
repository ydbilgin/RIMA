using RIMA;

namespace RIMA.Systems.Map
{
    [System.Serializable]
    public class TilesetPairing
    {
        public int lowerTerrainId;
        public int upperTerrainId;
        public CornerWangTileSetSO tileSet;
    }
}
