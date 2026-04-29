using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Obstacle types in the game world.
    /// </summary>
    public enum ObstacleType
    {
        /// <summary>Impassable, blocks skill shots, indestructible.</summary>
        StoneColumn,

        /// <summary>Impassable, blocks skill shots, can be destroyed by damage.</summary>
        WoodenCrate,

        /// <summary>Impassable, skill shots pass over, indestructible.</summary>
        LowBarricade,

        /// <summary>Only dash can pass through, skill shots pass over, indestructible.</summary>
        NarrowPassage,

        /// <summary>Impassable (no ground), skill shots pass over, indestructible.</summary>
        Chasm
    }

    /// <summary>
    /// Abstract base class for all obstacle objects in the scene.
    /// Concrete subtypes implement traversal and skill-shot rules.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class ObstacleBase : MonoBehaviour
    {
        [SerializeField] protected ObstacleType obstacleType;

        /// <summary>Which obstacle category this instance belongs to.</summary>
        public ObstacleType ObstacleType => obstacleType;

        /// <summary>
        /// Returns whether a walking (or dashing) entity can pass through this obstacle.
        /// </summary>
        /// <param name="isDashing">True when the entity is currently dashing.</param>
        public abstract bool CanWalkThrough(bool isDashing);

        /// <summary>
        /// Returns whether a skill-shot projectile can pass through this obstacle.
        /// </summary>
        public abstract bool CanSkillPassThrough();

        /// <summary>
        /// Called when this obstacle receives incoming damage.
        /// Override in destructible subclasses (e.g. WoodenCrate).
        /// Base implementation is intentionally empty.
        /// </summary>
        /// <param name="amount">Raw damage amount received.</param>
        public virtual void OnDamage(float amount) { }
    }
}
