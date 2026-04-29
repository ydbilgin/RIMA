using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Chasm / pit. No ground — nothing can walk over it.
    /// Skill shots fly over it. Blink can cross it (handled by PlayerController).
    /// Uses a trigger collider so the PlayerController detects entry and blocks movement.
    /// </summary>
    public sealed class Chasm : ObstacleBase
    {
        private void Awake()
        {
            obstacleType = ObstacleType.Chasm;
            // Trigger so detection works without blocking physics movement directly.
            // PlayerController reads CanWalkThrough() to block or allow entry.
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
            var col = GetComponent<Collider2D>();
            if (col != null) col.isTrigger = true;
        }

        public override bool CanWalkThrough(bool isDashing) => false;
        public override bool CanSkillPassThrough() => true;
    }
}
