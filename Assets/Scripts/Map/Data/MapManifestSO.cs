using UnityEngine;

namespace RIMA.Map
{
    [CreateAssetMenu(fileName = "MapManifest", menuName = "RIMA/Map/Map Manifest")]
    public class MapManifestSO : ScriptableObject
    {
        public string manifestId;
        public string displayName;
        public int actOrder;
        public RoomManifestSO startingRoom;
        public RoomManifestSO endingRoom;
        public RoomManifestSO[] rooms;
        public RoomManifestSO[] checkpointRooms;
    }
}
