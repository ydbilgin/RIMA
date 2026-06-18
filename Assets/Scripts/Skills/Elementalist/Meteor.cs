using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 7 — Meteor
    /// 1.5s kanal → büyük AoE knockdown.
    /// Frozen/slowed hedef → knockdown 3s + hasar +%50.
    /// </summary>
    public class Meteor : SkillBase
    {
        [Header("Meteor")]
        [SerializeField] private float channelTime  = 1.5f;
        [SerializeField] private float impactRadius = 3f;
        [SerializeField] private int   damage       = 90;
        [SerializeField] private float knockdownTime = 1.5f;
        private Elementalist_SkillController ctrl;

        // PixelLab falling meteor visual
        private static Sprite s_MeteorSprite;
        private const float FallVisualDuration = 0.3f; // seconds before impact

        protected override void Awake()
        {
            base.Awake();
            skillName = "Meteor";
            cooldown = 14f;
            resourceCost = 60;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute() => StartCoroutine(MeteorRoutine());

        private IEnumerator MeteorRoutine()
        {
            Vector2 target = player != null
                ? (Vector2)player.transform.position + player.FacingDirection * 4f
                : (Vector2)transform.position;

            SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Fire);

            // Channel — spawn falling visual 0.3s before impact
            float waitBeforeFall = Mathf.Max(0f, channelTime - FallVisualDuration);
            yield return new WaitForSeconds(waitBeforeFall);
            SpawnFallVisual(target);
            yield return new WaitForSeconds(channelTime - waitBeforeFall);

            ctrl?.RegisterElementCast(ElementalistElement.Fire, 1);
            // Fire impact burst at the meteor crater — mirrors Fireball's impact spark layering.
            SkillVfx.ImpactBurst((Vector3)target, VfxElement.Fire);

            var hits = Physics2D.OverlapCircleAll(target, impactRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player")) continue;
                var hp = hit.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                int dmg = damage;
                float kd = knockdownTime;

                var status = hit.GetComponent<StatusEffectSystem>();
                if (status != null &&
                    (status.GetStacks(StatusEffectType.Frozen) > 0 || status.GetStacks(StatusEffectType.Chill) > 0))
                {
                    dmg = Mathf.RoundToInt(damage * 1.5f);
                    kd = 3f;
                }

                SkillRuntime.DealDamage(hp, dmg, this);
                status?.ApplyEffect(StatusEffectType.Stunned, kd);

                var rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = (hit.transform.position - (Vector3)target).normalized;
                    rb.AddForce(dir * 5f, ForceMode2D.Impulse);
                }
            }
        }

        /// <summary>
        /// Spawns a brief falling rock sprite above the impact point, drops it down over FallVisualDuration.
        /// </summary>
        private void SpawnFallVisual(Vector2 impactPoint)
        {
            if (s_MeteorSprite == null)
                s_MeteorSprite = Resources.Load<Sprite>("VFX/Skills/meteor_main");

            var go = new GameObject("Meteor_FallVisual");
            Vector3 startPos = (Vector3)impactPoint + new Vector3(0f, 2.5f, 0f);
            go.transform.position = startPos;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "VFX";
            sr.sortingOrder = 25;
            if (s_MeteorSprite != null)
            {
                sr.sprite = s_MeteorSprite;
                sr.color  = Color.white;
            }
            else
            {
                sr.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
                sr.color  = new Color(1f, 0.5f, 0.1f, 0.9f);
            }
            go.transform.localScale = new Vector3(0.6f, 0.6f, 1f);

            StartCoroutine(FallDown(go, startPos, (Vector3)impactPoint, FallVisualDuration));
        }

        private static IEnumerator FallDown(GameObject go, Vector3 from, Vector3 to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && go != null)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                go.transform.position = Vector3.Lerp(from, to, t);
                yield return null;
            }
            if (go != null) Destroy(go);
        }
    }
}
