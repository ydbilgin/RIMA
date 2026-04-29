using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Impassable stone column. Blocks all movement and skill shots.
    /// Indestructible.
    /// </summary>
    public sealed class StoneColumn : ObstacleBase
    {
        private void Awake()
        {
            obstacleType = ObstacleType.StoneColumn;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }

        public override bool CanWalkThrough(bool isDashing) => false;
        public override bool CanSkillPassThrough() => false;
    }
}
