using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade + Elementalist cross-class pasif — "Burning Rage"
    /// • Rage > 50 iken ateş hasarı +25%
    /// • Ateş isabet edince Rage +5
    /// </summary>
    public class CrossClassPassive_WB_Elem : MonoBehaviour
    {
        public const float FireDamageBonus = 0.25f; // Ateş hasarı çarpanı
        public const int   RageOnFireHit  = 5;

        private RageSystem rage;

        private void Start()
        {
            rage = GetComponent<RageSystem>();
            Debug.Log("[CrossClass] Burning Rage aktif: Rage>50 → Ateş +25% | Ateş isabet → Rage+5");
        }

        /// <summary> Fireball, ChainLightning vb. ateş skill'leri hasar uygularken çağırır. </summary>
        public float GetFireDamageMultiplier()
            => (rage != null && rage.CurrentRage > 50) ? 1f + FireDamageBonus : 1f;

        /// <summary> Ateş skill'i hedefe çarptığında çağırır. </summary>
        public void OnFireHit()
            => rage?.AddRage(RageOnFireHit);
    }
}
