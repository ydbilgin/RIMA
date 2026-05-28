using UnityEngine;

namespace RIMA.RoomPainter
{
    public enum YSortAxis
    {
        UseLayerDefault,
        None,
        X,
        Y,
        NegY,
        Custom
    }

    [CreateAssetMenu(fileName = "RoomLayerData", menuName = "RIMA/Room Painter/Layer Data")]
    public sealed class RoomLayerData : ScriptableObject
    {
        public RoomLayer layer = RoomLayer.Floor;
        public string sortingLayerName = "Default";
        public int defaultOrder;
        public float depthValue;
        public bool isStatic = true;
        public bool isRoomLocked = true;
        public bool isCameraRelative;
        public bool ySortEnabled;
        public YSortAxis ySortAxis = YSortAxis.NegY;
    }
}
