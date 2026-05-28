using UnityEngine;

namespace RIMA.Walls.V2
{
    [CreateAssetMenu(fileName = "WallPieceData", menuName = "RIMA/Walls V2/Wall Piece Data")]
    public class WallPieceData : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public WallPieceType type;
        public WallDirection direction = WallDirection.Any;

        [Header("Footprint (cells)")]
        public Vector2Int footprintSize = new Vector2Int(1, 1);
        public int lengthInCells = 1;
        public WallHeight heightType = WallHeight.Normal;

        [Header("Connectivity")]
        public bool connectLeft = true;
        public bool connectRight = true;

        [Header("Pivot")]
        public Vector2 anchorOffset = Vector2.zero;

        [Header("Sockets (local pos relative to root)")]
        public Vector2 leftSocketLocal = new Vector2(-0.5f, 0f);
        public Vector2 rightSocketLocal = new Vector2(0.5f, 0f);
        public Vector2 seamSocketLeftLocal = new Vector2(-0.5f, 0f);
        public Vector2 seamSocketRightLocal = new Vector2(0.5f, 0f);

        [Header("Collider")]
        public Vector2 colliderSize = Vector2.one;
        public Vector2 colliderOffset = Vector2.zero;

        [Header("Visual")]
        public Color placeholderColor = Color.white;
        public Sprite spriteRef;

        [Header("Prefab")]
        public GameObject prefab;
    }
}
