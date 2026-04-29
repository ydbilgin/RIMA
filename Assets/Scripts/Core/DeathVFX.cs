using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ölüm anında bir burst VFX prefabı spawn eder.
    /// Health.OnDeath olayına bağlanır — prefab Inspector'dan atanır.
    /// </summary>
    public class DeathVFX : MonoBehaviour
    {
        [SerializeField] private GameObject deathBurstPrefab;

        private Health health;

        private void Awake()
        {
            health = GetComponentInParent<Health>() ?? GetComponent<Health>();
        }

        private void OnEnable()
        {
            if (health != null) health.OnDeath.AddListener(OnDeath);
        }

        private void OnDisable()
        {
            if (health != null) health.OnDeath.RemoveListener(OnDeath);
        }

        private void OnDeath()
        {
            if (deathBurstPrefab == null) return;
            Instantiate(deathBurstPrefab, transform.position, Quaternion.identity);
        }
    }
}
