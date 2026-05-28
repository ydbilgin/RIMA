using System;
using UnityEngine;

namespace RIMA.Environment
{
    [CreateAssetMenu(fileName = "CliffPlacementRules", menuName = "RIMA/Environment/Cliff Placement Rules")]
    public sealed class CliffPlacementRules : ScriptableObject
    {
        public enum CliffDirection
        {
            South,
            North,
            East,
            West,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest
        }

        [Serializable]
        public struct DirectionOffset
        {
            public CliffDirection direction;
            public Vector2 offset;
        }

        [Header("Cliff sprite")]
        public Sprite cliffBase;
        public Sprite[] cliffVariants = Array.Empty<Sprite>();

        [Header("Placement")]
        public Vector2 spriteScale = Vector2.one;
        public Vector2 worldOffset;
        public DirectionOffset[] directionOffsets = Array.Empty<DirectionOffset>();

        [Header("Rendering")]
        public int sortingOrder = -50;
        public string sortingLayer = "Ground";
        public int pixelsPerUnit = 64;
        public Vector2 spritePivot = new Vector2(0.5f, 1f);

        public Sprite GetVariant(int seed)
        {
            if (cliffVariants == null || cliffVariants.Length == 0) return cliffBase;
            int idx = (seed & 0x7fffffff) % cliffVariants.Length;
            return cliffVariants[idx];
        }

        public Vector2 GetOffset(CliffDirection direction)
        {
            for (int i = 0; i < directionOffsets.Length; i++)
            {
                if (directionOffsets[i].direction == direction)
                {
                    return worldOffset + directionOffsets[i].offset;
                }
            }

            return worldOffset;
        }
    }
}
