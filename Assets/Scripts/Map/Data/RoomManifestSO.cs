using UnityEngine;

namespace RIMA.Map
{
    [CreateAssetMenu(fileName = "RoomManifest", menuName = "RIMA/Map/Room Manifest")]
    public class RoomManifestSO : ScriptableObject
    {
        private const string CurrentSchemaVersion = "1.0";

        public string roomId;
        public TextAsset jsonLayout;
        public Vector2Int defaultCameraBounds;
        public RoomManifestSO[] connectedRooms;

        [SerializeField] private string schemaVersion = CurrentSchemaVersion;

        public string SchemaVersion => schemaVersion;

        public bool IsCompatibleSchema(string incomingVersion)
        {
            return incomingVersion == schemaVersion;
        }

        public bool TryMigrateSchema(string incomingVersion, out string migratedVersion)
        {
            migratedVersion = incomingVersion;
            return IsCompatibleSchema(incomingVersion);
        }
    }
}
