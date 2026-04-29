using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Hasar alınca hit spark prefabı spawn eder.
    /// Health.OnDamageTaken olayına bağlanır — prefab Inspector'dan atanır.
    /// </summary>
    public class HitImpact : MonoBehaviour
    {
        [SerializeField] private GameObject hitSparkPrefab;
        [SerializeField] private float spawnOffsetY = 0.3f;  // düşman merkezi yerine biraz yukarı

        private Health health;

        private void Awake()
        {
            health = GetComponentInParent<Health>() ?? GetComponent<Health>();
        }

        private void OnEnable()
        {
            if (health != null) health.OnDamageTaken.AddListener(OnHit);
        }

        private void OnDisable()
        {
            if (health != null) health.OnDamageTaken.RemoveListener(OnHit);
        }

        private void OnHit(int dmg)
        {
            if (hitSparkPrefab == null) return;
            Vector3 pos = transform.position + Vector3.up * spawnOffsetY;
            Instantiate(hitSparkPrefab, pos, Quaternion.identity);
        }
    }
}
