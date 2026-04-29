using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Low barricade. Blocks all movement but skill shots fly over it.
    /// Indestructible.
    /// </summary>
    public sealed class LowBarricade : ObstacleBase
    {
        private void Awake()
        {
            obstacleType = ObstacleType.LowBarricade;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }

        public override bool CanWalkThrough(bool isDashing) => false;
        public override bool CanSkillPassThrough() => true;
    }
}
