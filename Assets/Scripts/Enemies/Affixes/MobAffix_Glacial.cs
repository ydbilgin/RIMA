using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Glacial Elite Affix — saldırılar Chill uygular; 3 stack → Frozen.
    /// Görsel: mavi kristal aura.
    /// ShardWalker Glacial: shard'lar freeze zone bırakır (OnProjectileSpawned).
    /// Act 1-2 düşmanlarına eklenir.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAffix_Glacial : MonoBehaviour, IMobAffix
    {
        [SerializeField] private float chillDuration = 2.5f;
        [SerializeField] private float zoneRadius    = 1.5f;
        [SerializeField] private float zoneDuration  = 3f;
        [SerializeField] private Color eliteColor    = new Color(0.4f, 0.8f, 1f, 1f);

        private void Start()
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = eliteColor;

            var hp = GetComponent<Health>();
            if (hp != null) hp.SetMaxHP(Mathf.RoundToInt(hp.MaxHP * 1.5f));
        }

        public void OnProjectileSpawned(GameObject projectile)
        {
            // ShardWalker: mermi yere düşünce/patlarsa freeze zone bırakır
            var zone = projectile.AddComponent<GlacialZoneOnImpact>();
            zone.Init(chillDuration, zoneRadius, zoneDuration);
        }

        public void OnMeleeHit(StatusEffectSystem target)
        {
            target?.ApplyEffect(StatusEffectType.Chill, chillDuration);
        }
    }

    internal class GlacialZoneOnImpact : MonoBehaviour
    {
        private float chillDuration, radius, duration;
        private bool  triggered;

        public void Init(float chill, float r, float dur)
        {
            chillDuration = chill; radius = r; duration = dur;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered) return;
            if (!other.CompareTag("Player") && other.GetComponent<Health>() == null) return;
            triggered = true;
            SpawnZone();
        }

        private void SpawnZone()
        {
            var zone = new GameObject("GlacialZone");
            zone.transform.position = transform.position;

            var col = zone.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = radius;

            zone.AddComponent<GlacialZoneTick>().Init(chillDuration);
            Destroy(zone, duration);
        }
    }

    internal class GlacialZoneTick : MonoBehaviour
    {
        private float chillDuration;
        public void Init(float dur) => chillDuration = dur;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            other.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Chill, chillDuration);
        }
    }
}
