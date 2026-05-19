using UnityEngine;
using RIMA.Environment;

namespace RIMA
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float detectionRange = 8f;
        [SerializeField] private float attackRange = 1.2f;

        [Header("Movement")]
        [SerializeField] private float chaseSpeed = 3f;

        [Header("Combat")]
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float attackCooldown = 1.2f;

        private enum State { Idle, Chase, Attack }
        private State state = State.Idle;

        private Rigidbody2D rb;
        private Health health;
        private Transform player;
        private float attackTimer;
        private float attackWindupTimer;
        private SpriteRenderer sr;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
            GroundBlobShadow.Ensure(transform, new Vector2(0.9f, 0.30f), 0.28f);
            health = GetComponent<Health>();
            sr = GetComponentInChildren<SpriteRenderer>();

            if (health != null)
                health.OnDeath.AddListener(OnDeath);
        }

        private void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        private void Update()
        {
            if (health != null && health.IsDead) return;
            if (player == null) return;

            attackTimer -= Time.deltaTime;
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange)
                state = State.Attack;
            else if (dist <= detectionRange)
                state = State.Chase;
            else
                state = State.Idle;
        }

        private void FixedUpdate()
        {
            if (health != null && health.IsDead)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            if (player == null) return;

            if (state == State.Chase)
            {
                Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
                rb.linearVelocity = dir * chaseSpeed;
                if (sr != null && Mathf.Abs(dir.x) > 0.1f)
                    sr.flipX = dir.x < 0f;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                if (attackWindupTimer > 0f)
                {
                    attackWindupTimer -= Time.fixedDeltaTime;
                    if (attackWindupTimer <= 0f)
                    {
                        var ph = player.GetComponent<Health>();
                        if (ph != null) ph.TakeDamage(attackDamage);
                    }
                }
                else if (state == State.Attack && attackTimer <= 0f)
                {
                    attackTimer = attackCooldown;
                    attackWindupTimer = 0.35f;
                    EnemyTelegraph.SpawnCircle(transform.position, attackRange, 0.35f);
                }
            }
        }

        private void OnDeath()
        {
            rb.linearVelocity = Vector2.zero;
            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            if (sr != null) sr.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            this.enabled = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
