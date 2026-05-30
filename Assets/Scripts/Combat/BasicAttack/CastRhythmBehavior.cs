using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Cast rhythm behavior for Elementalist.
    /// LMB: Rift Bolt projectile with empowered 3rd shot.
    /// RMB: Tap = element switch, Hold = Lightbreak.
    /// </summary>
    public class CastRhythmBehavior : BasicAttackBehaviorBase
    {
        public override void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;

            if (owner.CommitTimer > 0f)
                owner.BufferedAttack = true;
            else
                ExecuteBolt(owner, profile);
        }

        public override void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (pressed)
                owner.ElementalistSecondaryStartedAt = Time.time;

            if (released && owner.ElementalistSecondaryStartedAt >= 0f)
            {
                bool held = Time.time - owner.ElementalistSecondaryStartedAt >= 0.2f;
                ExecuteSecondary(owner, held);
                owner.ElementalistSecondaryStartedAt = -1f;
            }
        }

        protected override void ExecuteBufferedLMB(PlayerAttack owner, BasicAttackProfile profile)
        {
            ExecuteBolt(owner, profile);
        }

        private void ExecuteBolt(PlayerAttack owner, BasicAttackProfile profile)
        {
            owner.Controller.FaceCombatTarget();

            owner.CommitTimer = profile.projectileCooldown;
            owner.ComboTimer = 0f;
            owner.ComboStep = 0;

            Vector2 dir = owner.Controller.FacingDirection.sqrMagnitude > 0.01f
                ? owner.Controller.FacingDirection.normalized
                : Vector2.right;

            var elementalist = owner.GetComponent<Elementalist_SkillController>();
            bool empowered = elementalist != null && elementalist.RegisterRiftBoltShot();
            int damage = empowered
                ? Mathf.RoundToInt(profile.projectileDamage * 1.6f)
                : profile.projectileDamage;

            var go = new GameObject("RiftBolt_Runtime");
            go.transform.position = owner.transform.position + (Vector3)(dir * 0.35f);

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = empowered ? 0.22f : 0.15f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = GetRiftBoltColor(elementalist);
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            float scale = empowered ? 0.52f : 0.34f;
            go.transform.localScale = new Vector3(scale, scale, 1f);

            var projectile = go.AddComponent<PlayerProjectile>();
            projectile.Init(dir * profile.projectileSpeed, damage, life: 2.2f, piercing: false, attacker: owner.gameObject);
            projectile.SetOnHit(hit =>
            {
                elementalist?.RegisterRiftBoltHit(empowered);
                var status = hit.GetComponent<StatusEffectSystem>();
                if (status == null || elementalist == null) return;
                if (elementalist.ActiveElement == ElementalistElement.Fire)
                    status.ApplyEffect(StatusEffectType.Burning, 3f);
                else if (elementalist.ActiveElement == ElementalistElement.Frost)
                    status.ApplyEffect(StatusEffectType.Chill, 2f);
                else
                    status.ApplyEffect(StatusEffectType.RiftMark, 4f);
            });

            LightPulse.Emit(new Color(0.28f, 0.76f, 1f), 0.8f, 0.06f);
        }

        private static Color GetRiftBoltColor(Elementalist_SkillController elementalist)
        {
            if (elementalist == null) return new Color(0.34f, 0.82f, 1f, 0.92f);
            return elementalist.ActiveElement switch
            {
                ElementalistElement.Fire => new Color(1f, 0.42f, 0.16f, 0.95f),
                ElementalistElement.Frost => new Color(0.34f, 0.82f, 1f, 0.92f),
                ElementalistElement.Light => new Color(1f, 0.9f, 0.36f, 0.95f),
                _ => new Color(0.34f, 0.82f, 1f, 0.92f)
            };
        }

        private void ExecuteSecondary(PlayerAttack owner, bool held)
        {
            var elementalist = owner.GetComponent<Elementalist_SkillController>();
            if (elementalist == null) return;
            if (held && elementalist.TryLightbreak()) return;
            elementalist.SwitchElement();
        }
    }
}
