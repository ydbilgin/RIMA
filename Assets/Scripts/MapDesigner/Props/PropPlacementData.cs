using System;
using UnityEngine;

namespace RIMA.MapDesigner.Props
{
    [Serializable]
    public sealed class PropPlacementData
    {
        public string propDefinitionGuid;
        public Vector2Int tilePosition;
        public int rotationSteps = 0;
        public bool flipX;
        public string placedByUser;
        public int variantIndex = -1;

        public PropPlacementData() { }

        public PropPlacementData(string guid, Vector2Int pos)
        {
            propDefinitionGuid = guid;
            tilePosition = pos;
            rotationSteps = 0;
            variantIndex = -1;
        }

        public void RotateClockwise()
        {
            rotationSteps = (rotationSteps + 1) & 3;
        }
    }
}
