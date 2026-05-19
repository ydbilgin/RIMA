using System;

namespace RIMA.MapDesigner.Encounter
{
    [Serializable]
    public class SubRoomLink
    {
        public string fromDoorSocketId;
        public string toSubRoomKey;
        public string toEntryDoorSocketId;
        public bool requiresClear = true;
    }
}
