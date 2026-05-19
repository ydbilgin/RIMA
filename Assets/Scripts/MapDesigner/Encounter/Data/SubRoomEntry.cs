using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Room.Data;

namespace RIMA.MapDesigner.Encounter
{
    [Serializable]
    public class SubRoomEntry
    {
        public string subRoomKey;
        public RoomTemplateSO room;
        public List<SubRoomLink> links = new List<SubRoomLink>();
        public bool isEntry;
        public bool isFinal;
        public string subRoomTag;
    }
}
