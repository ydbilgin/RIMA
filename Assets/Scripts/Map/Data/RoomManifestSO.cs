using UnityEngine;

namespace RIMA.Map
{
    [CreateAssetMenu(fileName = "RoomManifest", menuName = "RIMA/Map/Room Manifest")]
    public class RoomManifestSO : ScriptableObject
    {
        public string roomId;
        public TextAsset jsonLayout;
        public Vector2Int defaultCameraBounds;
        public RoomManifestSO[] connectedRooms;
    }
}
