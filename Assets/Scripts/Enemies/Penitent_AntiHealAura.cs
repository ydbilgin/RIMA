using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Penitent'in anti-heal aurası.
    /// MobAttack_PenitentCombo + BaseMobBehavior ile aynı GameObject'e eklenir.
    ///
    /// Etki: yakın oyuncunun Health.healMultiplier'ını yarıya indirir.
    /// Ölünce aura <deathAuraDuration> saniye daha devam eder.
    /// Lifesteal / sustain build'leri aurada işe yaramaz.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class Penitent_AntiHealAura : MonoBehaviour
    {
        [Header("Anti-Heal Aura")]
        [SerializeField] private float auraRadius      = 4f;
        [SerializeField] private float healMultiplier  = 0.5f;   // oyuncunun heal'i bu ile çarpılır
        [SerializeField] private float deathLingerTime = 3f;

        private BaseMobBehavior mob;
        private Health          playerHealth;
        private bool            auraApplied;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            mob.OnDeathTriggered += OnDeath;
        }

        private void OnDestroy()
        {
            if (mob != null) mob.OnDeathTriggered -= OnDeath;
            ForceRemoveAura();
        }

        private void Start()
        {
            var player = mob.Player;
            if (player != null) playerHealth = player.GetComponent<Health>();
        }

        private void Update()
        {
            if (mob.CurrentState == BaseMobBehavior.MobState.Dead) return;
            if (playerHealth == null) return;

            bool inRange = Vector2.Distance(transform.position, mob.Player.position) <= auraRadius;

            if (inRange && !auraApplied)
            {
                auraApplied = true;
                playerHealth.healMultiplier *= healMultiplier;
            }
            else if (!inRange && auraApplied)
            {
                auraApplied = false;
                playerHealth.healMultiplier /= healMultiplier;
            }
        }

        private void OnDeath()
        {
            if (auraApplied)
                StartCoroutine(LingerRoutine());
        }

        private IEnumerator LingerRoutine()
        {
            yield return new WaitForSeconds(deathLingerTime);
            ForceRemoveAura();
        }

        private void ForceRemoveAura()
        {
            if (!auraApplied || playerHealth == null) return;
            playerHealth.healMultiplier /= healMultiplier;
            auraApplied = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, auraRadius);
        }
    }
}
