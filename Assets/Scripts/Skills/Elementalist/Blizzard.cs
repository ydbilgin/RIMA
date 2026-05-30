using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 12 — Blizzard (Master)
    /// 3s kanal → 5m alana 8s slow+tick.
    /// Meteor'dan önce → Meteor knockdown 4s.
    /// </summary>
    public class Blizzard : SkillBase
    {
        [Header("Blizzard")]
        [SerializeField] private GameObject damageZonePrefab;
        [SerializeField] private float channelTime = 3f;
        [SerializeField] private float zoneDuration = 8f;
        [SerializeField] private float zoneRadius   = 5f;
        [SerializeField] private int   tickDamage   = 8;

        public bool IsActive { get; private set; }
        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Blizzard";
            cooldown = 30f;
            resourceCost = 80;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute() => StartCoroutine(BlizzardRoutine());

        private IEnumerator BlizzardRoutine()
        {
            yield return new WaitForSeconds(channelTime);

            IsActive = true;
            ctrl?.RegisterElementCast(ElementalistElement.Frost, 1);
            Vector2 target = player != null
                ? (Vector2)player.transform.position + player.FacingDirection * 3f
                : (Vector2)transform.position;

            if (damageZonePrefab != null)
            {
                var go = Instantiate(damageZonePrefab, target, Quaternion.identity);
                var col = go.GetComponent<CircleCollider2D>();
                if (col != null) col.radius = zoneRadius;
                var zone = go.GetComponent<DamageZone>();
                zone?.Init(zoneDuration, tickDamage, 0.5f, StatusEffectType.Chill, 1.5f);
            }
            else
            {
                // Prefab yoksa kod tabanlı tick
                StartCoroutine(BlizzardTick(target));
            }

            yield return new WaitForSeconds(zoneDuration);
            IsActive = false;
        }

        private IEnumerator BlizzardTick(Vector2 center)
        {
            float elapsed = 0f;
            while (elapsed < zoneDuration)
            {
                var hits = Physics2D.OverlapCircleAll(center, zoneRadius);
                foreach (var h in hits)
                {
                    if (h.CompareTag("Player")) continue;
                    var hp = h.GetComponent<Health>();
                    if (hp != null && !hp.IsDead) SkillRuntime.DealDamage(hp, tickDamage, this);
                    h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Chill, 1.5f);
                }
                yield return new WaitForSeconds(0.5f);
                elapsed += 0.5f;
            }
        }
    }
}
