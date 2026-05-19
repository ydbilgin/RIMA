using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "RoomDecalData", menuName = "RIMA/MapDesigner/RoomDecalData")]
    public class RoomDecalDataSO : ScriptableObject
    {
        public string roomId;
        public List<DecalPlacement> placements = new List<DecalPlacement>();
    }

    [System.Serializable]
    public struct DecalPlacement
    {
        public Vector2 worldPos;
        public int spriteId;
        public byte layer;
        public byte rotationStep;
        public byte flags;
        public short tintPackedRGB;
    }
}
