using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Narrow passage. Only passable while dashing. Skill shots pass through.
    /// Indestructible.
    /// </summary>
    public sealed class NarrowPassage : ObstacleBase
    {
        private void Awake()
        {
            obstacleType = ObstacleType.NarrowPassage;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }

        /// <summary>Returns true only when the entity is actively dashing.</summary>
        public override bool CanWalkThrough(bool isDashing) => isDashing;
        public override bool CanSkillPassThrough() => true;
    }
}
