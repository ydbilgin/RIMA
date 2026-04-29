using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Base for any obstacle that can be destroyed by dealing damage.
    /// Concrete usage: WoodenCrate.
    /// </summary>
    public abstract class DestructibleObstacle : ObstacleBase
    {
        [Header("Health")]
        [SerializeField, Min(1f)] private float maxHealth = 30f;
        private float currentHealth;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
            gameObject.layer = LayerMask.NameToLayer("ObstacleDestructible");
        }

        /// <summary>
        /// Applies damage. Destroys the object when health reaches zero.
        /// </summary>
        public override void OnDamage(float amount)
        {
            if (amount <= 0f) return;
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                Die();
            }
        }

        /// <summary>
        /// Called when HP hits zero. Override to add VFX/loot before destroying.
        /// Default destroys the GameObject.
        /// </summary>
        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}
