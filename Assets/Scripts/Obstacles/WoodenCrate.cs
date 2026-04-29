using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Wooden crate obstacle. Impassable, blocks skill shots, but can be
    /// destroyed by dealing damage to it.
    /// </summary>
    public sealed class WoodenCrate : DestructibleObstacle
    {
        [Header("Destruction FX")]
        [SerializeField] private GameObject destructionVFXPrefab;
        [SerializeField] private AudioClip destructionSound;

        protected override void Awake()
        {
            base.Awake();
            obstacleType = ObstacleType.WoodenCrate;
        }

        public override bool CanWalkThrough(bool isDashing) => false;
        public override bool CanSkillPassThrough() => false;

        protected override void Die()
        {
            if (destructionVFXPrefab != null)
                Instantiate(destructionVFXPrefab, transform.position, Quaternion.identity);

            if (destructionSound != null)
                AudioSource.PlayClipAtPoint(destructionSound, transform.position);

            base.Die();
        }
    }
}
