using UnityEngine;

namespace RIMA.Rooms
{
    /// <summary>
    /// Stores a baked room template plus authored gameplay and decor anchor data.
    /// </summary>
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "RIMA/Rooms/Room Template")]
    public class RoomTemplate : ScriptableObject
    {
        [Header("Identity")]
        public string templateId;
        public string biomeTag;
        public string lightingVariant;

        [Header("Base Template")]
        public Sprite baseImage;
        public bool mirrorFlipAllowed = true;

        [Header("Collision")]
        public Vector2[] wallPathLocalPoints;

        [Header("Decor")]
        public OverlayAnchor[] anchors;

        [Header("Connections")]
        public Vector2[] doorSocketsLocalPoints;

        [Header("Encounter")]
        public Vector2[] enemySpawnPoints;

        [Header("Camera")]
        public Vector2 cameraBoundsCenter;
        public Vector2 cameraBoundsSize;
    }
}
