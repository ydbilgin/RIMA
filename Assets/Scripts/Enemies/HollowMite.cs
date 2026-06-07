using System.Collections;
using UnityEngine;
using RIMA.Environment;

namespace RIMA
{
    /// <summary>
    /// Hollow Mite — Swarm tier (48px). Hızlı zigzag, sürü taktik.
    /// Sprite gelince EnemyAnimator eklenir. Şu an placeholder SpriteRenderer ile çalışır.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class HollowMite : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float chaseSpeed   = 5.5f;
        [SerializeField] private float zigzagAmplitude = 1.2f;
        [SerializeField] private float zigzagFrequency = 3.5f;

        [Header("Combat")]
        [SerializeField] private int   attackDamage   = 6;
        [SerializeField] private float attackRange    = 0.6f;
        [SerializeField] private float attackCooldown = 0.8f;
        [SerializeField] private float detectionRange = 9f;

        private Rigidbody2D rb;
        private Health health;
        private Transform player;
        private float attackTimer;
        private float zigzagTimer;
        private bool isDead;

        private void Awake()
        {
            rb     = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();

            rb.gravityScale  = 0f;
            rb.freezeRotation = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
            GroundBlobShadow.Ensure(transform, new Vector2(0.58f, 0.20f), 0.24f);

            health.OnDeath.AddListener(OnDeath);

            // T6.1 FIX: replace oversized purple placeholder box with a small readable diamond.
            // Remove this block once real PixelLab sprite is assigned in the Inspector.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = MakeSmallDiamond();
                sr.color  = new Color(0.45f, 0.28f, 0.72f, 0.90f); // dim violet — swarm-tier colour
                // 48px intended canvas; PPU=64 → ~0.75u, keep natural scale
                transform.localScale = Vector3.one;
            }
        }

        /// <summary>8×8 diamond sprite — compact placeholder so the mob doesn't render as a huge colour block.</summary>
        private static Sprite MakeSmallDiamond()
        {
            const int S = 8;
            var tex = new Texture2D(S, S, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;
            tex.wrapMode   = TextureWrapMode.Clamp;
            int half = S / 2;
            for (int y = 0; y < S; y++)
                for (int x = 0; x < S; x++)
                {
                    // Diamond shape: |dx| + |dy| <= half
                    bool inside = Mathf.Abs(x - half + 0.5f) + Mathf.Abs(y - half + 0.5f) <= half;
                    tex.SetPixel(x, y, inside ? Color.white : Color.clear);
                }
            tex.Apply(false, true);
            return Sprite.Create(tex, new Rect(0, 0, S, S), new Vector2(0.5f, 0.5f), S);
        }

        private void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;

            // Stagger attacks in swarms
            attackTimer = Random.Range(0f, attackCooldown);
        }

        private void Update()
        {
            if (isDead || player == null) return;

            attackTimer  -= Time.deltaTime;
            zigzagTimer  += Time.deltaTime;

            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange)
            {
                rb.linearVelocity = Vector2.zero;
                if (attackTimer <= 0f)
                {
                    attackTimer = attackCooldown;
                    player.GetComponent<Health>()?.TakeDamage(attackDamage);
                }
            }
            else if (dist <= detectionRange)
            {
                Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;

                // Zigzag perpendicular to chase direction
                Vector2 perp    = new Vector2(-toPlayer.y, toPlayer.x);
                float   zigzag  = Mathf.Sin(zigzagTimer * zigzagFrequency) * zigzagAmplitude;

                rb.linearVelocity = (toPlayer + perp * zigzag).normalized * chaseSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * 4f);
            }
        }

        private void OnDeath()
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;
            StartCoroutine(DeathDelay());
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);
        }
    }
}
