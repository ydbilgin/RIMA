using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// VoidThrall'ın ölüm split mekaniği.
    /// BaseMobBehavior + MobAttack_Melee ile aynı GameObject'e eklenir.
    ///
    /// Ölünce iki HalfThrall spawn eder:
    ///   - Her biri VoidThrall'ın maxHP'sinin %35'i kadar HP
    ///   - Daha hızlı (chaseSpeed × speedMultiplier)
    ///   - Ayrı hareket eder, ayrı öldürülür
    ///
    /// Strateji notu: burst hasarla önlemezsen iki sorun olur.
    /// HalfThrall prefabı: BaseMobBehavior + MobAttack_Melee
    ///   → Inspector'da chaseSpeed yüksek ayarla (3.5–4f)
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    [RequireComponent(typeof(Health))]
    public class VoidThrall_DeathSplit : MonoBehaviour
    {
        [Header("Split")]
        [SerializeField] private GameObject halfThrallPrefab;
        [Tooltip("Her half'ın max HP'si: VoidThrall maxHP × bu oran")]
        [SerializeField] private float hpFraction   = 0.35f;
        [SerializeField] private Vector2 spawnOffset = new Vector2(0.6f, 0f);

        private BaseMobBehavior mob;
        private Health          health;

        private void Awake()
        {
            mob    = GetComponent<BaseMobBehavior>();
            health = GetComponent<Health>();
            mob.OnDeathTriggered += Split;
        }

        private void OnDestroy()
        {
            if (mob != null) mob.OnDeathTriggered -= Split;
        }

        private void Split()
        {
            if (halfThrallPrefab == null) return;

            int halfHP = Mathf.RoundToInt(health.MaxHP * hpFraction);

            SpawnHalf( spawnOffset, halfHP);
            SpawnHalf(-spawnOffset, halfHP);

            // Ana GameObject bir sonraki frame'de BaseMobBehavior tarafından zaten devre dışı bırakılıyor
        }

        private void SpawnHalf(Vector2 offset, int hp)
        {
            Vector2 pos = (Vector2)transform.position + offset;
            var go = Instantiate(halfThrallPrefab, pos, Quaternion.identity);

            var h = go.GetComponent<Health>();
            if (h != null) h.SetMaxHP(hp);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.6f, 0f, 0.8f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)spawnOffset, 0.3f);
            Gizmos.DrawWireSphere(transform.position - (Vector3)spawnOffset, 0.3f);
        }
    }
}
