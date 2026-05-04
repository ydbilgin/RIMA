using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 5 — Frozen Orb
    /// Yavaş hareket eden küre, yolundakileri 5s chill.
    /// Orb üzerinden Blink → Orb patlar, Frozen 2s.
    /// </summary>
    public class FrozenOrb : SkillBase
    {
        [Header("Frozen Orb")]
        [SerializeField] private GameObject orbPrefab;
        [SerializeField] private float orbSpeed    = 2.5f;
        [SerializeField] private float orbLifetime = 5f;
        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Frozen Orb";
            cooldown = 9f;
            resourceCost = 35;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go = orbPrefab != null
                ? Instantiate(orbPrefab, transform.position, Quaternion.identity)
                : CreateRuntimeOrb();
            var orb = go.GetComponent<FrozenOrbObject>();
            if (orb != null) orb.Init(dir * orbSpeed, orbLifetime);
            ctrl?.RegisterElementCast(ElementalistElement.Frost, 1);
        }

        private GameObject CreateRuntimeOrb()
        {
            var go = new GameObject("FrozenOrb_Runtime");
            go.transform.position = transform.position;
            go.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.45f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = new Color(0.55f, 0.88f, 1f, 0.82f);
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            go.AddComponent<FrozenOrbObject>();
            return go;
        }
    }

    /// <summary>
    /// Frozen Orb nesnesinin davranışı — orbPrefab'a eklenir.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class FrozenOrbObject : MonoBehaviour
    {
        private Rigidbody2D rb;
        private bool exploded;

        public void Init(Vector2 velocity, float lifetime)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = velocity;
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (exploded) return;
            if (other.CompareTag("Player")) return;
            var hp = other.GetComponent<Health>();
            if (hp == null) return;

            var status = other.GetComponent<StatusEffectSystem>();
            status?.ApplyEffect(StatusEffectType.Chill, 5f);
        }

        /// <summary> Blink tarafından çağrılır. </summary>
        public void Explode()
        {
            if (exploded) return;
            exploded = true;
            var hits = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                var status = h.GetComponent<StatusEffectSystem>();
                status?.ApplyEffect(StatusEffectType.Frozen, 2f);
            }
            Destroy(gameObject);
        }
    }
}
