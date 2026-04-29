using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger kaynak sistemi — Focus.
    /// 4m+ uzaktayken +10/sn | 2m- yakınken -20/sn.
    /// Focus 75+: hasar +%25. Focus 100: sonraki skill serbest cast.
    /// </summary>
    public class FocusSystem : PlayerResourceBase
    {
        [SerializeField] private int maxFocus = 100;
        [SerializeField] private float farRegenPerSecond = 10f;
        [SerializeField] private float closeDecayPerSecond = 20f;
        [SerializeField] private float farThreshold = 4f;
        [SerializeField] private float closeThreshold = 2f;

        private float current;
        private Transform player;
        private Transform nearestEnemy;

        public override int Current => Mathf.RoundToInt(current);
        public override int Max => maxFocus;
        public float FocusPercent => current / maxFocus;

        public bool IsDamageBoosted => current >= 75f;
        public bool IsFreeCast => current >= maxFocus;

        private bool freeCastPending;
        public bool ConsumeFreeCast()
        {
            if (!freeCastPending) return false;
            freeCastPending = false;
            return true;
        }

        private void Awake()
        {
            current = 50f;
            player = transform;
        }

        private void Update()
        {
            float dist = GetNearestEnemyDistance();
            float delta = 0f;

            if (dist > farThreshold)
                delta = farRegenPerSecond * Time.deltaTime;
            else if (dist < closeThreshold)
                delta = -closeDecayPerSecond * Time.deltaTime;

            float prev = current;
            current = Mathf.Clamp(current + delta, 0f, maxFocus);

            if (!freeCastPending && current >= maxFocus && prev < maxFocus)
                freeCastPending = true;

            if (Mathf.Abs(current - prev) > 0.1f)
                OnResourceChanged?.Invoke(Current, maxFocus);
        }

        private float GetNearestEnemyDistance()
        {
            var enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
            float minDist = float.MaxValue;
            foreach (var e in enemies)
            {
                float d = Vector2.Distance(player.position, e.transform.position);
                if (d < minDist) minDist = d;
            }
            return enemies.Length == 0 ? float.MaxValue : minDist;
        }

        public override bool TrySpend(int amount)
        {
            if (freeCastPending) { freeCastPending = false; return true; }
            if (current < amount) return false;
            current -= amount;
            OnResourceChanged?.Invoke(Current, maxFocus);
            return true;
        }

        public override void Add(int amount)
        {
            current = Mathf.Clamp(current + amount, 0f, maxFocus);
            OnResourceChanged?.Invoke(Current, maxFocus);
        }
    }
}
