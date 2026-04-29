using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade + Ranger cross-class pasif — "Predator's Mark"
    /// • CC'li (stunned/slowed) düşmanlara Ranger hasarı +30%
    /// • CC bittikten 3s süre "Exposed" debuff kalır
    /// </summary>
    public class CrossClassPassive_WB_Ranger : MonoBehaviour
    {
        public const float RangerDamageBonus    = 0.30f;
        public const float ExposedLingerDuration = 3f;

        private void Start()
        {
            Debug.Log("[CrossClass] Predator's Mark aktif: CC'li hedef → Ranger +30% | CC biter → 3s Exposed");
        }

        /// <summary>
        /// Ranger skill'i hasar hesaplarken çağırır.
        /// target üzerinde StatusEffect Stun/Slow veya Exposed varsa bonus uygulanır.
        /// </summary>
        public float GetRangerDamageMultiplier(GameObject target)
        {
            if (target == null) return 1f;
            var status = target.GetComponent<StatusEffectSystem>();
            if (status == null) return 1f;

            bool hasCC      = status.HasEffect(StatusEffectType.Stunned) || status.HasEffect(StatusEffectType.Chill);
            bool hasExposed = status.HasEffect(StatusEffectType.Exposed);

            return (hasCC || hasExposed) ? 1f + RangerDamageBonus : 1f;
        }

        /// <summary>
        /// CC uygulandığında çağırılır — bitiş sonrası Exposed linger ekler.
        /// (IronCharge stun, WarStomp knockup, GravityCleave slow vb.)
        /// </summary>
        public void OnCCApplied(GameObject target, float ccDuration)
        {
            if (target == null) return;
            var status = target.GetComponent<StatusEffectSystem>();
            status?.ApplyEffect(StatusEffectType.Exposed, ccDuration + ExposedLingerDuration);
        }
    }
}
