using System;
using UnityEngine;
using RIMA.Combat;
using RIMA.Environment;

namespace RIMA
{
    /// <summary>
    /// Tüm düşmanların ortak tabanı.
    /// Hareket, state machine, ölüm yönetimi burada.
    /// Sprite flip işlemi EnemyAnimator tarafından yapılır (4-yön+flip sistemi).
    /// Saldırı davranışı MobAttack_* componentleri üstlenir.
    /// Elite affix'ler MobAffix_* olarak ayrı component eklenir.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class BaseMobBehavior : MonoBehaviour
    {
        // ─── Config ──────────────────────────────────────────────────────────

        [Header("Detection")]
        [SerializeField] public float detectionRange = 8f;
        [SerializeField] public float attackRange    = 1.5f;

        [Header("Movement")]
        [SerializeField] public float chaseSpeed = 3f;

        [Header("Attack Timing")]
        [Tooltip("Attack component'i bu süreyi ayarlar. Inspector'da override.")]
        [SerializeField] public float attackCooldown = 1.5f;

        // ─── State ───────────────────────────────────────────────────────────

        public enum MobState { Idle, Chase, Attack, Dead }
        public MobState CurrentState { get; private set; } = MobState.Idle;

        // ─── Events ──────────────────────────────────────────────────────────

        /// <summary>Saldırı anı geldi — yön: oyuncuya doğru. Attack componentler bunu dinler.</summary>
        public event Action<Vector2> OnAttackReady;

        /// <summary>Ölüm tetiklendi — Affix ve special componentler bunu dinler.</summary>
        public event Action OnDeathTriggered;

        /// <summary>Hasar alındı — Affix'ler refleks davranışı için dinleyebilir.</summary>
        public event Action<int> OnDamageTaken;

        // ─── References ──────────────────────────────────────────────────────

        public Transform Player     { get; private set; }
        public Health    Health     { get; private set; }
        public Rigidbody2D Rb       { get; private set; }
        public StatusEffectSystem StatusFx { get; private set; }
        public SpriteRenderer SR    { get; private set; }

        // ─── Internal ────────────────────────────────────────────────────────

        private float attackTimer;
        private Animator anim;
        private AttackTokenType attackTokenType;

        // ─── Init ────────────────────────────────────────────────────────────

        protected virtual void Awake()
        {
            Rb       = GetComponent<Rigidbody2D>();
            Health   = GetComponent<Health>();
            StatusFx = GetComponent<StatusEffectSystem>();
            SR       = GetComponentInChildren<SpriteRenderer>();
            anim     = GetComponentInChildren<Animator>();
            attackTokenType = HasRangedAttackComponent() ? AttackTokenType.Ranged : AttackTokenType.Melee;

            Rb.gravityScale  = 0f;
            Rb.freezeRotation = true;
            Rb.bodyType = RigidbodyType2D.Kinematic;
            Rb.useFullKinematicContacts = true;

            // Enemy physics layer + ignore Player<->Enemy body collision. Kinematic enemies
            // chasing the dynamic player were transferring their chase velocity (e.g. -3 Y)
            // into the player via contact → player drifted into the void. Damage is handled by
            // combat overlap/triggers, not this body collision, so ignoring it is safe.
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer >= 0) gameObject.layer = enemyLayer;
            int playerLayer = LayerMask.NameToLayer("Player");
            if (enemyLayer >= 0 && playerLayer >= 0)
                Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

            GroundBlobShadow.Ensure(transform, new Vector2(0.9f, 0.30f), 0.28f);

            // Ölüm squash/fade + decal residue — yoksa otomatik ekle
            if (!TryGetComponent<MobDeathResidue>(out _))
                gameObject.AddComponent<MobDeathResidue>();

            // Spawn anında renk sıfırla — önceki ölüm fade'i prefab'a yazılmışsa temizle
            if (SR != null) SR.color = Color.white;

            Health.OnDeath.AddListener(HandleDeath);
            Health.OnDamageTaken.AddListener(d => OnDamageTaken?.Invoke(d));
        }

        protected virtual void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) Player = p.transform;

            // Fallback: PlaceholderSprite çalışmadıysa sprite hala null olabilir
            EnsureVisibleSprite();
        }

        /// <summary>
        /// Sprite null ise runtime placeholder oluşturur. Tüm mob'ları korur.
        /// </summary>
        private void EnsureVisibleSprite()
        {
            if (SR == null) return;
            if (SR.sprite != null && SR.sprite.texture != null) return;

            // Renkli placeholder oluştur
            var tex = new Texture2D(48, 48);
            var pixels = new Color[48 * 48];
            var c = new Color(0.85f, 0.15f, 0.15f, 1f); // kırmızı = düşman
            for (int i = 0; i < pixels.Length; i++) pixels[i] = c;
            tex.SetPixels(pixels);
            tex.Apply();
            tex.filterMode = FilterMode.Point;

            SR.sprite = Sprite.Create(tex, new Rect(0, 0, 48, 48), new Vector2(0.5f, 0.5f), 48f);
            SR.color = Color.white;

            // Unlit material — URP 2D lit sorun çıkarıyor
            var shader = Shader.Find("Sprites/Default");
            if (shader != null)
                SR.sharedMaterial = new Material(shader);
        }

        // ─── Update / FixedUpdate ────────────────────────────────────────────

        protected virtual void Update()
        {
            if (CurrentState == MobState.Dead || Player == null) return;

            attackTimer -= Time.deltaTime;

            // StatusEffect: speed multiplier
            float speedMult = StatusFx != null ? StatusFx.moveSpeedMultiplier : 1f;
            if (speedMult <= 0f)
            {
                Rb.linearVelocity = Vector2.zero;
                return;
            }

            float dist = Vector2.Distance(transform.position, Player.position);
            UpdateState(dist);
        }

        private void UpdateState(float dist)
        {
            if (dist <= attackRange)
                CurrentState = MobState.Attack;
            else if (dist <= detectionRange)
                CurrentState = MobState.Chase;
            else
                CurrentState = MobState.Idle;
        }

        protected virtual void FixedUpdate()
        {
            if (CurrentState == MobState.Dead || Player == null)
            {
                Rb.linearVelocity = Vector2.zero;
                return;
            }

            float speedMult = StatusFx != null ? StatusFx.moveSpeedMultiplier : 1f;
            if (speedMult <= 0f) { Rb.linearVelocity = Vector2.zero; return; }

            Vector2 toPlayer = ((Vector2)Player.position - (Vector2)transform.position);
            Vector2 dir      = toPlayer.normalized;

            if (CurrentState == MobState.Chase)
            {
                Vector2 desiredVel = dir * (chaseSpeed * speedMult);
                // Walkability clamp: prevent mobs from crossing void/hole cells.
                // Uses the shared helper (same logic as PlayerController) with O(1) grid lookup.
                // Permissive when no WalkabilityMap in scene (legacy behavior preserved).
                desiredVel = WalkabilityMap.ClampVelocityToWalkable(WalkabilityMap.Instance, transform.position, desiredVel, Time.fixedDeltaTime);
                Rb.linearVelocity = desiredVel;
                // Sprite flip artık EnemyAnimator tarafından yönetiliyor
            }
            else
            {
                Rb.linearVelocity = Vector2.zero;

                if (CurrentState == MobState.Attack && attackTimer <= 0f)
                {
                    if (OnAttackReady == null) return;

                    var tokenManager = AttackTokenManager.Instance;
                    if (tokenManager == null || !tokenManager.TryConsumeToken(gameObject, attackTokenType)) return;

                    attackTimer = attackCooldown;
                    OnAttackReady?.Invoke(dir);
                }
            }
        }

        // ─── Death ───────────────────────────────────────────────────────────

        private void HandleDeath()
        {
            CurrentState = MobState.Dead;
            Rb.linearVelocity = Vector2.zero;

            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            AttackTokenManager.Instance?.OnEnemyDeath(gameObject);

            OnDeathTriggered?.Invoke();

            // Death animasyonu varsa oynat, yoksa fade out
            if (anim != null && anim.runtimeAnimatorController != null)
            {
                anim.SetTrigger("IsDead");
            }
            else
            {
                // Fallback: basit fade out
                if (SR != null) SR.color = new Color(0.25f, 0.25f, 0.25f, 0.4f);
            }

            // 3 saniye sonra destroy
            Destroy(gameObject, 3f);
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        protected virtual void OnDestroy()
        {
            AttackTokenManager.Instance?.ReturnToken(gameObject, attackTokenType);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private bool HasRangedAttackComponent()
        {
            return GetComponent<MobAttack_Throw>() != null || GetComponent<SeamCrawler_Homing>() != null;
        }
    }
}
