using UnityEngine;

namespace RIMA.Runtime.Rooms
{
    [CreateAssetMenu(menuName = "RIMA/Room Blueprint")]
    public class RoomBlueprint : ScriptableObject
    {
        public string roomId;
        public string biome;
        public GameObject prefab;
        public string roomType;
        public int gateCount;
    }
}
