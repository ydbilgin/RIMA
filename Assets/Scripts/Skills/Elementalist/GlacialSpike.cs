using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 2 — Glacial Spike
    /// 6m buz hattı: hattaki düşmanlar %40 slow + %180 hasar. Frost State+2.
    /// Fireball DoT aktif hedefe → Freeze 2s + DoT hasarı tek seferde patlar.
    /// </summary>
    public class GlacialSpike : SkillBase
    {
        [Header("Glacial Spike")]
        [SerializeField] private float lineLength = 6f;
        [SerializeField] private float lineWidth  = 0.6f;
        [SerializeField] private int   damage     = 45;
        [SerializeField] private float slowDuration = 2.5f;

        private Elementalist_SkillController ctrl;

        // PixelLab spike cluster visual
        private static Sprite s_SpikeSprite;
        private const int   SpikeCount = 3;
        private const float SpikeFadeDuration = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Glacial Spike";
            cooldown = 5f;
            resourceCost = 25;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 origin = transform.position;

            SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Frost);

            var hits = Physics2D.BoxCastAll(origin + dir * (lineLength * 0.5f),
                new Vector2(lineWidth, lineLength), Vector2.SignedAngle(Vector2.up, dir), Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player")) continue;
                var hp = hit.collider.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                int dmg = damage;
                var status = hit.collider.GetComponent<StatusEffectSystem>();
                if (status != null)
                {
                    // Fireball DoT aktifse → Freeze + DoT burst
                    if (status.GetStacks(StatusEffectType.Burning) > 0)
                    {
                        status.ApplyEffect(StatusEffectType.Frozen, 2f);
                        dmg = Mathf.RoundToInt(damage * 1.5f);
                    }
                    else
                    {
                        status.ApplyEffect(StatusEffectType.Chill, slowDuration);
                    }
                }
                SkillRuntime.DealDamage(hp, dmg, this);
            }

            ctrl?.RegisterElementCast(ElementalistElement.Frost, 2);
            ctrl?.ConsumeFireState(1);

            SpawnSpikeVisuals(origin, dir);
            // Frost impact burst at the far end of the line — same spark layering as Fireball's impact.
            SkillVfx.ImpactBurst((Vector3)(origin + dir * lineLength), VfxElement.Frost);
        }

        /// <summary>
        /// Spawns SpikeCount cluster sprites evenly distributed along the spike line, fades out in 0.5s.
        /// </summary>
        private void SpawnSpikeVisuals(Vector2 origin, Vector2 dir)
        {
            if (s_SpikeSprite == null)
                s_SpikeSprite = Resources.Load<Sprite>("VFX/Skills/glacial_spike_cluster");

            for (int i = 0; i < SpikeCount; i++)
            {
                float t = (i + 0.5f) / SpikeCount; // 0.17, 0.5, 0.83 — evenly spaced
                Vector3 pos = (Vector3)(origin + dir * (lineLength * t));

                var go = new GameObject("GlacialSpike_Visual");
                go.transform.position = pos;
                go.transform.localScale = new Vector3(0.55f, 0.55f, 1f);

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sortingLayerName = "VFX";
                sr.sortingOrder = 15;
                if (s_SpikeSprite != null)
                {
                    sr.sprite = s_SpikeSprite;
                    sr.color  = new Color(0.7f, 0.92f, 1f, 1f);
                }
                else
                {
                    sr.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
                    sr.color  = new Color(0.55f, 0.88f, 1f, 0.9f);
                }

                StartCoroutine(FadeAndDestroy(go, sr, SpikeFadeDuration));
            }
        }

        private static IEnumerator FadeAndDestroy(GameObject go, SpriteRenderer sr, float duration)
        {
            float elapsed = 0f;
            Color startColor = sr.color;
            while (elapsed < duration && go != null)
            {
                elapsed += Time.deltaTime;
                float a = Mathf.Lerp(startColor.a, 0f, elapsed / duration);
                sr.color = new Color(startColor.r, startColor.g, startColor.b, a);
                yield return null;
            }
            if (go != null) Destroy(go);
        }
    }
}
