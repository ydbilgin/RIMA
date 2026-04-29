using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Tüm MobAffix_* componentlerinin uyguladığı arayüz.
    /// Saldırı componentleri bu interface üzerinden affix'lere haber verir.
    /// </summary>
    public interface IMobAffix
    {
        /// <summary>Fırlatma saldırısı anında — projectile GO'suna efekt eklemek için.</summary>
        void OnProjectileSpawned(GameObject projectile);

        /// <summary>Melee vuruş anında — hedefe efekt eklemek için.</summary>
        void OnMeleeHit(StatusEffectSystem target);
    }
}
