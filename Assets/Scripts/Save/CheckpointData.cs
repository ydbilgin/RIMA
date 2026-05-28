using System;

namespace RIMA.Save
{
    [Serializable]
    public class CheckpointData
    {
        public int playerHealth;
        public int playerMaxHealth;
        public string currentRoomId;
        public string currentActId;
        public string[] inventory = Array.Empty<string>();
        public string[] equipped = Array.Empty<string>();
        public long timestamp;
    }
}
