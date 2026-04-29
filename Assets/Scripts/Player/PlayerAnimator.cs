using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Tüm animator parametrelerini tek yerden yönetir.
    ///
    /// 8-yön tam clip sistemi (east-flip kaldırıldı — gerçek east sprite'lar mevcut).
    /// SW attack flip korundu: SW attack sprite'ları SE gibi üretildiği için
    /// SW saldırısında SE clip + flipX kullanılıyor (TODO: sprite düzeltilince kaldır).
    ///
    /// Animator'da gereken parametreler:
    ///   float  Speed       — 0=idle, 1=run
    ///   float  DirX        — hareket yönü X (-1..1)
    ///   float  DirY        — hareket yönü Y (-1..1)
    ///   bool   IsDashing   — dash animasyonu
    ///   trigger IsDead     — ölüm (geri dönmez)
    ///   int    ComboStep   — 0/1/2 = normal · 3/4/5 = skill sonrası chained
    ///   trigger Attack     — her vuruşta tetiklenir
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerController controller;
        private PlayerAttack     attack;
        private Animator         anim;
        private SpriteRenderer   sr;

        // ─── Parameter hashes ────────────────────────────────────────────────

        private static readonly int SpeedHash     = Animator.StringToHash("Speed");
        private static readonly int DirXHash      = Animator.StringToHash("DirX");
        private static readonly int DirYHash      = Animator.StringToHash("DirY");
        private static readonly int IsDeadHash    = Animator.StringToHash("IsDead");
        private static readonly int IsDashHash    = Animator.StringToHash("IsDashing");
        private static readonly int ComboStepHash = Animator.StringToHash("ComboStep");
        private static readonly int AttackHash    = Animator.StringToHash("Attack");

        [Header("Movement Feel")]
        [SerializeField] private float moveAnimSpeed = 1.5f;

        private Vector2 lastDir = Vector2.down;

        // SW attack: Kiro SW sprite'larını SE gibi ürettiği için
        // SW saldırısında SE clip + flipX kullanıyoruz.
        // TODO: Kiro doğru SW sprite üretince bu flag'i kaldır.
        private bool   _swAttackFlip    = false;
        private float  _swAttackEndTime = 0f;
        private const float SwAttackDuration = 0.8f;

        // ─── Init ────────────────────────────────────────────────────────────

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            attack     = GetComponent<PlayerAttack>();
            anim       = GetComponentInChildren<Animator>();
            // Animator ile aynı GameObject'teki SR'ı al (Sprite child).
            // GetComponentInChildren root'u da arar — Player root'ta ayrı SR var,
            // yanlış olanı döndürürdü.
            sr = anim != null ? anim.GetComponent<SpriteRenderer>() : GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (attack != null) attack.OnComboStep += HandleComboStep;
        }

        private void OnDisable()
        {
            if (attack != null) attack.OnComboStep -= HandleComboStep;
        }

        // ─── Update ──────────────────────────────────────────────────────────

        private void Update()
        {
            if (anim == null || anim.runtimeAnimatorController == null) return;

            Vector2 dir = controller.FacingDirection;
            if (dir.sqrMagnitude > 0.01f)
                lastDir = dir.normalized;

            // SW attack flip süresi doldu mu?
            if (Time.time > _swAttackEndTime) _swAttackFlip = false;

            bool facingSW = lastDir.x < -0.1f && lastDir.y < -0.1f;
            bool movingDiagDown = controller.IsMoving && lastDir.y < -0.1f && Mathf.Abs(lastDir.x) > 0.1f;

            // Temporary run-direction fix:
            // Only E/S/SE run clips exist right now. For down-left movement (SW),
            // sample SE motion and mirror it. Down-right (SE) uses SE directly.
            float animDirX = lastDir.x;
            bool shouldFlip = false;
            if (movingDiagDown)
            {
                animDirX = Mathf.Abs(lastDir.x); // always sample SE slot for diagonal-down run
                shouldFlip = lastDir.x < 0f;     // down-left input (SW) mirrors SE->SW
            }

            // Attack override: SW attack still uses SE+flip fallback behavior.
            if (facingSW && _swAttackFlip)
            {
                animDirX = Mathf.Abs(lastDir.x);
                shouldFlip = true;
            }

            if (sr != null) sr.flipX = shouldFlip;

            // Hareket halinde animasyon hızını artır — koşma hissi
            anim.speed = controller.IsMoving ? moveAnimSpeed : 1f;

            anim.SetFloat(SpeedHash,  controller.IsMoving ? 1f : 0f);
            anim.SetFloat(DirXHash,   animDirX);
            anim.SetFloat(DirYHash,   lastDir.y);
            anim.SetBool(IsDashHash,  controller.IsDashing);
        }

        // ─── Combo ───────────────────────────────────────────────────────────

        private void HandleComboStep(int step)
        {
            if (anim == null || anim.runtimeAnimatorController == null) return;
            anim.SetInteger(ComboStepHash, step % 3);
            anim.SetTrigger(AttackHash);

            // SW saldırısında flip başlat
            if (lastDir.x < -0.1f && lastDir.y < -0.1f)
            {
                _swAttackFlip    = true;
                _swAttackEndTime = Time.time + SwAttackDuration;
            }
        }

        // ─── Death ───────────────────────────────────────────────────────────

        /// <summary>Health.OnDeath → Inspector'dan bağla.</summary>
        public void TriggerDeath()
        {
            if (anim != null) anim.SetTrigger(IsDeadHash);
        }
    }
}
