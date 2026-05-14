using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    [Serializable]
    public struct RoomConnection
    {
        public RoomRecipe from;
        public RoomRecipe to;
        public string connectionType;
    }

    [CreateAssetMenu(fileName = "DungeonRecipe", menuName = "RIMA/Map/Dungeon Recipe")]
    public class DungeonRecipe : ScriptableObject
    {
        public List<RoomRecipe> rooms = new List<RoomRecipe>();
        public List<RoomConnection> connections = new List<RoomConnection>();
    }
}
