using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(menuName = "RIMA/Sprite Hand Data", fileName = "SpriteHandData_New")]
    public class SpriteHandData : ScriptableObject
    {
        public Sprite sprite;
        public Vector2 handLeftPx;
        public Vector2 handRightPx;
        public bool hasLeftHand = true;
        public bool hasRightHand = true;

        public bool Matches(Sprite candidate)
        {
            return sprite != null && candidate == sprite;
        }

        public bool TryGetLeft(out Vector2 pixel)
        {
            pixel = handLeftPx;
            return hasLeftHand && sprite != null;
        }

        public bool TryGetRight(out Vector2 pixel)
        {
            pixel = handRightPx;
            return hasRightHand && sprite != null;
        }
    }
}
