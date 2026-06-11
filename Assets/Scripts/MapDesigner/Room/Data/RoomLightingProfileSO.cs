using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [CreateAssetMenu(menuName = "RIMA/Room/Room Lighting Profile", fileName = "RoomLightingProfile_New", order = 201)]
    public sealed class RoomLightingProfileSO : ScriptableObject
    {
        public Color globalColor = Color.white;
        [Min(0f)] public float globalIntensity = 1f;
        public List<PointLightSpec> pointLights = new List<PointLightSpec>
        {
            new PointLightSpec { normalizedRoomPosition = new Vector2(0.35f, 0.55f) },
            new PointLightSpec { normalizedRoomPosition = new Vector2(0.65f, 0.55f) },
        };

        [System.Serializable]
        public sealed class PointLightSpec
        {
            public Vector2 normalizedRoomPosition = new Vector2(0.5f, 0.5f);
            public Color color = Color.white;
            [Min(0f)] public float intensity = 1f;
            [Min(0f)] public float innerRadius = 0.5f;
            [Min(0.01f)] public float outerRadius = 3f;
        }
    }
}
