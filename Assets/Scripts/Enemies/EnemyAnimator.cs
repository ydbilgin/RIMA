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
    // LateUpdate sprite-keeper must run AFTER BaseMobBehavior.LateUpdate (default order 0),
    // which re-applies a runtime red 48x48 placeholder every frame when the animator clip drove
    // the sprite to null. A positive execution order makes this keeper the LAST writer so the real
    // authored sprite wins (otherwise Unity's undefined same-GameObject LateUpdate order let the red
    // placeholder overwrite us).
    [DefaultExecutionOrder(100)]
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

        // Runs after the Animator's AND BaseMobBehavior's per-frame sprite writes (see
        // DefaultExecutionOrder above). FractureImp/Penitent clips drive SpriteRenderer.sprite to
        // null every frame, then BaseMobBehavior.EnsureVisibleSprite fills a runtime red 48x48
        // placeholder (a NON-NULL sprite with an empty name) — so a plain `sprite == null` guard
        // never fires and the body stays a red square. Restore the cached authored sprite whenever
        // the current sprite is missing OR is that nameless runtime placeholder. Asset-backed
        // animation frames (working enemies) carry a real sprite name and are left untouched.
        private void LateUpdate()
        {
            if (_isDead || sr == null || _fallbackSprite == null) return;
            if (sr.sprite == null || string.IsNullOrEmpty(sr.sprite.name))
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
