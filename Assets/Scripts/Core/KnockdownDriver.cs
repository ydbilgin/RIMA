using System.Collections;
using RIMA.Combat.Juice;
using RIMA.Environment;
using UnityEngine;

namespace RIMA
{
    [DisallowMultipleComponent]
    public class KnockdownDriver : MonoBehaviour
    {
        private const string ProxyName = "KnockdownVisualProxy";

        private Rigidbody2D rb;
        private Health health;
        private EnemyAI enemyAI;
        private GroundBlobShadow shadow;
        private Coroutine sequence;

        private Transform visualTarget;
        private SpriteRenderer rootRenderer;
        private SpriteRenderer proxyRenderer;
        private bool usingProxy;
        private bool rootRendererWasEnabled;
        private bool enemyAIWasEnabled;
        private bool healthWasImmune;
        private Vector3 visualBaseLocalPosition;
        private Quaternion visualBaseLocalRotation;
        private Vector3 visualBaseLocalScale;

        public bool IsDownOrGettingUp { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            enemyAI = GetComponent<EnemyAI>();
        }

        public static KnockdownDriver Ensure(GameObject host)
        {
            if (host == null) return null;
            if (!host.TryGetComponent(out KnockdownDriver driver))
                driver = host.AddComponent<KnockdownDriver>();
            return driver;
        }

        public bool TryStart(HitImpulse impulse, KnockdownProfile fallbackProfile)
        {
            if (!FeelToggleSettings.KnockdownEnabled) return false;
            if (IsDownOrGettingUp || health != null && health.IsDead) return false;

            var profile = impulse.ResolveProfile(fallbackProfile);
            if (profile == null) return false;

            if (sequence != null) StopCoroutine(sequence);
            sequence = StartCoroutine(DoKnockdown(impulse, profile));
            return true;
        }

        public void Cancel()
        {
            if (sequence != null)
            {
                StopCoroutine(sequence);
                sequence = null;
            }

            RestoreVisual();
            RestoreControlAndImmunity();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            IsDownOrGettingUp = false;
        }

        private void OnDisable()
        {
            // Knockdown ortasında destroy/deactivate olursa immunity/AI/shadow leak'ini önle (axopus review MAJOR #1)
            if (IsDownOrGettingUp) Cancel();
        }

        private IEnumerator DoKnockdown(HitImpulse impulse, KnockdownProfile profile)
        {
            IsDownOrGettingUp = true;
            CaptureRuntimeState(profile);

            Vector2 direction = impulse.direction.sqrMagnitude > 0.001f ? impulse.direction.normalized : Vector2.right;
            float tiltSign = direction.x < -0.01f ? 1f : -1f;

            if (health != null)
            {
                healthWasImmune = health.IsImmune;
                health.SetImmune(true);
            }

            if (enemyAI != null)
            {
                enemyAIWasEnabled = enemyAI.enabled;
                enemyAI.enabled = false;
            }

            GetComponent<StatusEffectSystem>()?.ApplyEffect(
                StatusEffectType.Stunned,
                profile.LaunchDuration + profile.DownTime + profile.GetUpDuration + profile.GetUpIFrame);

            // T2: knockdown launch SFX + shake (land phase — M-tier)
            RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.KnockdownThud, 0.75f);
            RIMA.Combat.ScreenShakeDriver.Instance?.TriggerKnockdownShake();

            yield return MoveArc(profile, direction, impulse.force, tiltSign);
            yield return Squash(profile, tiltSign);
            yield return Bounce(profile, tiltSign);
            yield return HoldDown(profile, tiltSign);
            yield return GetUp(profile, tiltSign);

            if (profile.GetUpIFrame > 0f)
                yield return new WaitForSeconds(profile.GetUpIFrame);

            RestoreControlAndImmunity();
            RestoreVisual();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            IsDownOrGettingUp = false;
            sequence = null;
        }

        private void CaptureRuntimeState(KnockdownProfile profile)
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (health == null) health = GetComponent<Health>();
            if (enemyAI == null) enemyAI = GetComponent<EnemyAI>();

            shadow = GroundBlobShadow.Ensure(transform, profile.shadowSize, profile.shadowAlpha);
            if (shadow != null) shadow.gameObject.SetActive(true);

            visualTarget = ResolveVisualTarget();
            visualBaseLocalPosition = visualTarget.localPosition;
            visualBaseLocalRotation = visualTarget.localRotation;
            visualBaseLocalScale = visualTarget.localScale;
        }

        private Transform ResolveVisualTarget()
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>(true);
            SpriteRenderer selected = null;
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] == null) continue;
                if (renderers[i].GetComponent<GroundBlobShadow>() != null) continue;
                if (renderers[i].name == ProxyName) continue;
                selected = renderers[i];
                break;
            }

            if (selected == null || selected.transform != transform)
                return selected != null ? selected.transform : transform;

            rootRenderer = selected;
            rootRendererWasEnabled = rootRenderer.enabled;
            if (proxyRenderer == null)
            {
                var proxy = new GameObject(ProxyName);
                proxy.transform.SetParent(transform, false);
                proxyRenderer = proxy.AddComponent<SpriteRenderer>();
            }

            CopyRenderer(rootRenderer, proxyRenderer);
            proxyRenderer.enabled = true;
            rootRenderer.enabled = false;
            usingProxy = true;
            return proxyRenderer.transform;
        }

        private static void CopyRenderer(SpriteRenderer source, SpriteRenderer target)
        {
            target.sprite = source.sprite;
            target.color = source.color;
            target.flipX = source.flipX;
            target.flipY = source.flipY;
            target.drawMode = source.drawMode;
            target.size = source.size;
            target.tileMode = source.tileMode;
            target.maskInteraction = source.maskInteraction;
            target.sortingLayerID = source.sortingLayerID;
            target.sortingOrder = source.sortingOrder;
            target.sharedMaterial = source.sharedMaterial;
        }

        private IEnumerator MoveArc(KnockdownProfile profile, Vector2 direction, float force, float tiltSign)
        {
            float elapsed = 0f;
            float duration = profile.LaunchDuration;
            Vector2 velocity = direction * Mathf.Max(0f, force);
            WalkabilityMap walkMap = WalkabilityMap.Instance;

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                float height = Mathf.Sin(t * Mathf.PI) * profile.ArcHeight;
                float decay = 1f - t;
                if (rb != null)
                {
                    // Walkability clamp: knockdown arc cannot push actor into void/holes.
                    Vector2 frameVel = WalkabilityMap.ClampVelocityToWalkable(walkMap, transform.position, velocity * decay, Time.deltaTime);
                    rb.linearVelocity = frameVel;
                }
                SetVisual(height, profile.tiltAngle * tiltSign * t, Vector3.one);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (rb != null) rb.linearVelocity = Vector2.zero;
            SetVisual(0f, profile.tiltAngle * tiltSign, Vector3.one);
        }

        private IEnumerator Squash(KnockdownProfile profile, float tiltSign)
        {
            float elapsed = 0f;
            float duration = profile.LandingSquashDuration;
            Vector3 squash = new Vector3(profile.LandingSquashX, profile.LandingSquashY, 1f);

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                SetVisual(0f, profile.tiltAngle * tiltSign, Vector3.Lerp(Vector3.one, squash, t));
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Bounce(KnockdownProfile profile, float tiltSign)
        {
            for (int i = 0; i < profile.BounceCount; i++)
            {
                float elapsed = 0f;
                float duration = profile.BounceDuration;
                float height = profile.BounceHeight / (i + 1f);
                while (elapsed < duration)
                {
                    float t = Mathf.Clamp01(elapsed / duration);
                    float arc = Mathf.Sin(t * Mathf.PI) * height;
                    float squashY = Mathf.Lerp(profile.LandingSquashY, 0.72f, t);
                    SetVisual(arc, profile.tiltAngle * tiltSign, new Vector3(1.08f, squashY, 1f));
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private IEnumerator HoldDown(KnockdownProfile profile, float tiltSign)
        {
            SetVisual(0f, profile.tiltAngle * tiltSign, new Vector3(1.04f, 0.68f, 1f));
            if (profile.DownTime > 0f)
                yield return new WaitForSeconds(profile.DownTime);
        }

        private IEnumerator GetUp(KnockdownProfile profile, float tiltSign)
        {
            float elapsed = 0f;
            float duration = profile.GetUpDuration;
            Vector3 startScale = new Vector3(1.04f, 0.68f, 1f);

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = t * t * (3f - 2f * t);
                float hop = Mathf.Sin(t * Mathf.PI) * profile.BounceHeight * 0.5f;
                SetVisual(hop, Mathf.Lerp(profile.tiltAngle * tiltSign, 0f, eased), Vector3.Lerp(startScale, Vector3.one, eased));
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void SetVisual(float yOffset, float zAngle, Vector3 scaleMultiplier)
        {
            if (visualTarget == null) return;
            visualTarget.localPosition = visualBaseLocalPosition + new Vector3(0f, yOffset, 0f);
            visualTarget.localRotation = visualBaseLocalRotation * Quaternion.Euler(0f, 0f, zAngle);
            visualTarget.localScale = new Vector3(
                visualBaseLocalScale.x * scaleMultiplier.x,
                visualBaseLocalScale.y * scaleMultiplier.y,
                visualBaseLocalScale.z * scaleMultiplier.z);
        }

        private void RestoreVisual()
        {
            if (visualTarget != null)
            {
                visualTarget.localPosition = visualBaseLocalPosition;
                visualTarget.localRotation = visualBaseLocalRotation;
                visualTarget.localScale = visualBaseLocalScale;
            }

            if (usingProxy)
            {
                if (proxyRenderer != null) proxyRenderer.enabled = false;
                if (rootRenderer != null) rootRenderer.enabled = rootRendererWasEnabled;
                usingProxy = false;
            }
        }

        private void RestoreControlAndImmunity()
        {
            if (health != null) health.SetImmune(healthWasImmune);
            if (enemyAI != null) enemyAI.enabled = enemyAIWasEnabled;
        }
    }
}
