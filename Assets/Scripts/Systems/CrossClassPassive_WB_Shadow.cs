using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade + Shadowblade cross-class pasif — "Blood Poison"
    /// • Warblade kanama tick'lerinden Energy +3
    /// • Kill'de Energy +15
    /// </summary>
    public class CrossClassPassive_WB_Shadow : MonoBehaviour
    {
        public const int EnergyOnBleedTick = 3;
        public const int EnergyOnKill      = 15;

        private EnergySystem energy;

        private void Start()
        {
            energy = GetComponent<EnergySystem>();
            Debug.Log("[CrossClass] Blood Poison aktif: Bleed tick → Energy+3 | Kill → Energy+15");
        }

        /// <summary> DeepWound veya bleed DoT her tick'te çağırır. </summary>
        public void OnBleedTick()
            => energy?.Add(EnergyOnBleedTick);

        /// <summary> Düşman öldüğünde çağırır (EnemyAI.OnDeath → Health.OnDeath event zinciri). </summary>
        public void OnKill()
            => energy?.Add(EnergyOnKill);
    }
}
