using UnityEngine;
using RIMA;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "RIMA/Map/Room Template")]
    public class RoomTemplate : ScriptableObject
    {
        public LargeDungeonMapPainterBase.LayoutKind layout;
        public int widthTiles;
        public int heightTiles;
    }
}
