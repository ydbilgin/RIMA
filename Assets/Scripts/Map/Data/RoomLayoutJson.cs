using System;
using UnityEngine;

namespace RIMA.Map
{
    [Serializable]
    public class RoomLayoutJson
    {
        public string schema_version;
        public string room_id;
        public string display_name;
        public string act;
        public string room_type;
        public int width;
        public int height;
        public RoomShape shape;
        public RoomFloor floor;
        public RoomWall[] walls;
        public RoomProp[] props;
        public RoomMobSpawn[] mob_spawns;
        public RoomDoor[] doors;
        public RoomSpawnPoint[] spawn_points;
        public RoomCameraBounds camera_bounds;
        public RoomLighting lighting;
        public RoomAudio audio;
        public RoomPostProcess post_process;
        public RoomNarrative narrative;
        public RoomRewards rewards;
    }

    [Serializable]
    public class RoomShape
    {
        public string type;
        public RoomAlcove[] alcoves;
        public RoomRect[] rects;
        public RoomRect inner_obstacle_rect;
    }

    [Serializable]
    public class RoomAlcove
    {
        public string wall;
        public int offset;
        public int width;
        public int depth;
    }

    [Serializable]
    public class RoomRect
    {
        public int x;
        public int y;
        public int width;
        public int height;
    }

    [Serializable]
    public class RoomFloor
    {
        public string default_material;
        public RoomZone[] zones;
        public RoomAccent[] accents;
    }

    [Serializable]
    public class RoomZone
    {
        public string material;
        public RoomRect rect;
        public bool blend_edges;
    }

    [Serializable]
    public class RoomAccent
    {
        public string material;
        public int x;
        public int y;
    }

    [Serializable]
    public class RoomWall
    {
        public string prefab;
        public int x;
        public int y;
        public int rotation;
        public int variant_seed;
    }

    [Serializable]
    public class RoomProp
    {
        public string prefab;
        public float x;
        public float y;
        public float rotation;
        public float scale = 1f;
        public bool flip_x;
        public int sorting_order_override;
    }

    [Serializable]
    public class RoomMobSpawn
    {
        public string mob_id;
        public float x;
        public float y;
        public int wave = 1;
        public bool elite;
        public RoomPoint[] patrol_path;
    }

    [Serializable]
    public class RoomDoor
    {
        public string direction;
        public float x;
        public float y;
        public string target_room_id;
        public string target_spawn_point;
        public bool locked_initial;
        public string key_required;
    }

    [Serializable]
    public class RoomSpawnPoint
    {
        public string id;
        public float x;
        public float y;
    }

    [Serializable]
    public class RoomCameraBounds
    {
        public float x_min;
        public float x_max;
        public float y_min;
        public float y_max;
    }

    [Serializable]
    public class RoomLighting
    {
        public string global_color;
        public float global_intensity = 1f;
        public RoomPointLight[] point_lights;
    }

    [Serializable]
    public class RoomPointLight
    {
        public float x;
        public float y;
        public string color;
        public float intensity;
        public float outer_radius = 4f;
        public bool flicker;
    }

    [Serializable]
    public class RoomAudio
    {
        public string music_track_id;
        public string ambient_loop_id;
        public string music_state_on_combat;
    }

    [Serializable]
    public class RoomPostProcess
    {
        public string profile_id;
    }

    [Serializable]
    public class RoomNarrative
    {
        public string lore_note_id;
        public RoomNpcSpawn[] npc_spawn;
    }

    [Serializable]
    public class RoomNpcSpawn
    {
        public string npc_id;
        public float x;
        public float y;
        public string dialog_id;
    }

    [Serializable]
    public class RoomRewards
    {
        public RoomChest[] chests;
        public RoomPickup[] pickups;
    }

    [Serializable]
    public class RoomChest
    {
        public string tier;
        public float x;
        public float y;
        public string key_required;
    }

    [Serializable]
    public class RoomPickup
    {
        public string pickup_id;
        public float x;
        public float y;
    }

    [Serializable]
    public class RoomPoint
    {
        public float x;
        public float y;
    }

    [Serializable]
    public class RoomManifestJson
    {
        public string manifest_schema_version;
        public string manifest_id;
        public string display_name;
        public int act_order;
        public string starting_room;
        public string ending_room;
        public string[] checkpoint_rooms;
        public RoomLayoutJson[] rooms;
    }
}
