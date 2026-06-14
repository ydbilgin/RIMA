using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Düşman Animator sürücüsü — BaseMobBehavior state machine ile senkronize.
    /// Animator Controller'da Speed, DirX, DirY, IsAttacking (bool), IsDead parametreleri olmalı.
    /// 
    /// 4-yön + flip: sağa bakan yönlerde (DirX > 0) sprite flip edilir,
    /// animator'a negatif DirX gönderilir → sol clip kullanılır.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody2D rb;
        private Health health;
        private BaseMobBehavior mob;
        private SpriteRenderer sr;

        private static readonly int SpeedHash      = Animator.StringToHash("Speed");
        private static readonly int DirXHash       = Animator.StringToHash("DirX");
        private static readonly int DirYHash       = Animator.StringToHash("DirY");
        private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
        private static readonly int IsDeadHash     = Animator.StringToHash("IsDead");

        private Vector2 lastDir = Vector2.down;
        private bool _isDead = false;
        private Sprite _fallbackSprite;

        private void Awake()
        {
            anim   = GetComponent<Animator>();
            rb     = GetComponentInParent<Rigidbody2D>();
            health = GetComponentInParent<Health>();
            mob    = GetComponentInParent<BaseMobBehavior>();
            sr     = GetComponentInParent<SpriteRenderer>();
            if (sr == null) sr = GetComponent<SpriteRenderer>();

            // Cache the authored prefab sprite as a fallback. Some enemies' idle clips reference
            // sprite frames that were archived out of Assets/ (e.g. FractureImp, Penitent), so the
            // animator drives the SpriteRenderer to null each frame → invisible body. LateUpdate
            // runs AFTER the Animator's per-frame sprite write (player loop order:
            // DirectorUpdateAnimationEnd → ScriptRunBehaviourLateUpdate), so restoring here wins
            // and persists. (Full idle animation needs the archived frames re-imported — post-demo;
            // this keeps the enemy VISIBLE meanwhile.)
            if (sr != null) _fallbackSprite = sr.sprite;

            if (health != null)
                health.OnDeath.AddListener(OnDeath);
        }

        // Runs after the Animator's per-frame sprite write; restores the cached sprite when a
        // broken clip drove SpriteRenderer.sprite to null.
        private void LateUpdate()
        {
            if (_isDead || sr == null) return;
            if (sr.sprite == null && _fallbackSprite != null)
                sr.sprite = _fallbackSprite;
        }

        private void Update()
        {
            if (_isDead) return;
            if (anim == null || rb == null || anim.runtimeAnimatorController == null) return;

            Vector2 vel = rb.linearVelocity;
            float speed = vel.magnitude;

            if (speed > 0.1f)
                lastDir = vel.normalized;

            // ── 4-yön + flip sistemi ──
            bool facingRight = lastDir.x > 0.05f;
            Vector2 animDir = lastDir;

            if (facingRight)
                animDir.x = -animDir.x;

            if (sr != null)
                sr.flipX = facingRight;

            // walk-as-idle: speed asla 0 gönderme, blend tree'de kalmasın
            anim.SetFloat(SpeedHash, Mathf.Max(speed, 0.01f));
            anim.SetFloat(DirXHash, animDir.x);
            anim.SetFloat(DirYHash, animDir.y);

            // Attack animasyonu — BaseMobBehavior state'ine bağlı
            if (mob != null)
            {
                bool attacking = mob.CurrentState == BaseMobBehavior.MobState.Attack;
                anim.SetBool(IsAttackingHash, attacking);
            }
        }

        private void OnDeath()
        {
            _isDead = true;
            
            // Hemen sprite'ı gizle — mor kare görünmesin
            if (sr != null) sr.enabled = false;
            
            // Animator varsa death trigger
            if (anim != null && anim.runtimeAnimatorController != null)
                anim.SetTrigger(IsDeadHash);
            
            // Kısa süre sonra tamamen yok et
            StartCoroutine(DestroyAfterDelay());
        }

        private System.Collections.IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(0.3f);
            Destroy(transform.root.gameObject);
        }
    }
}
