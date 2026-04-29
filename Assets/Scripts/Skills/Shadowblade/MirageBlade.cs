using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 9 — Mirage Blade (Advanced)
    /// 3s: geçilen konumlara gölge bırakır, dokunan düşman %100 hasar + 1 CP.
    /// Shadowstep sonrası → tüm gölgeler hedefe atılır, %250 hasar + 1s stun.
    /// </summary>
    public class MirageBlade : SkillBase
    {
        [Header("Mirage Blade")]
        [SerializeField] private int   shadowDamage  = 35;
        [SerializeField] private float shadowLifetime = 3f;
        [SerializeField] private float shadowInterval = 0.4f;

        private ComboPointSystem combo;
        private List<Vector2> shadowPositions = new List<Vector2>();
        private bool mirrageActive;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Mirage Blade";
            cooldown = 12f;
            resourceCost = 30;
            combo = GetComponentInParent<ComboPointSystem>();
        }

        protected override void Execute() => StartCoroutine(MirageRoutine());

        private IEnumerator MirageRoutine()
        {
            mirrageActive = true;
            shadowPositions.Clear();
            float elapsed = 0f;

            while (elapsed < shadowLifetime)
            {
                shadowPositions.Add(transform.position);
                SpawnShadow(transform.position);
                yield return new WaitForSeconds(shadowInterval);
                elapsed += shadowInterval;
            }
            mirrageActive = false;
        }

        private void SpawnShadow(Vector2 pos)
        {
            // Shadow sphere that damages on contact
            var go = new GameObject("MirageShadow");
            go.transform.position = pos;
            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.5f;
            go.AddComponent<MirageShadow>().Init(shadowDamage, combo);
            Destroy(go, shadowLifetime);
        }

        /// <summary> Shadowstep tetikler — tüm gölgeler hedefe atılır. </summary>
        public void LaunchAllShadows(Transform target)
        {
            StartCoroutine(LaunchRoutine(target));
        }

        private IEnumerator LaunchRoutine(Transform target)
        {
            foreach (var pos in shadowPositions)
            {
                var hits = Physics2D.OverlapCircleAll(target.position, 0.6f);
                foreach (var h in hits)
                {
                    if (h.CompareTag("Player")) continue;
                    h.GetComponent<Health>()?.TakeDamage(Mathf.RoundToInt(shadowDamage * 2.5f));
                    h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 1f);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public class MirageShadow : MonoBehaviour
    {
        private int damage;
        private ComboPointSystem combo;

        public void Init(int dmg, ComboPointSystem cp) { damage = dmg; combo = cp; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;
            var hp = other.GetComponent<Health>();
            if (hp == null || hp.IsDead) return;
            hp.TakeDamage(damage);
            combo?.Add(1);
            Destroy(gameObject);
        }
    }
}
