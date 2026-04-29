using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Blazing Elite Affix — tüm saldırılara Burning efekti ekler.
    /// Görsel: turuncu aura (SpriteRenderer tint veya particle sistemi).
    /// Act 1-2 düşmanlarına eklenir.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAffix_Blazing : MonoBehaviour, IMobAffix
    {
        [SerializeField] private float burnDuration = 3f;
        [SerializeField] private Color eliteColor   = new Color(1f, 0.5f, 0.1f, 1f);

        private void Start()
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = eliteColor;

            // HP buff (elite %50 daha dayanıklı)
            var hp = GetComponent<Health>();
            if (hp != null) hp.SetMaxHP(Mathf.RoundToInt(hp.MaxHP * 1.5f));
        }

        public void OnProjectileSpawned(GameObject projectile)
        {
            // Projecile'a Burning uygulayacak bileşen ekle
            projectile.AddComponent<BlazeOnHit>().burnDuration = burnDuration;
        }

        public void OnMeleeHit(StatusEffectSystem target)
        {
            target?.ApplyEffect(StatusEffectType.Burning, burnDuration);
        }
    }

    internal class BlazeOnHit : MonoBehaviour
    {
        internal float burnDuration;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            other.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Burning, burnDuration);
        }
    }
}
