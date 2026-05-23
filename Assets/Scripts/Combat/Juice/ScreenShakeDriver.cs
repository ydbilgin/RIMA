using System.Collections;
using RIMA.Combat.Juice;
using UnityEngine;

namespace RIMA.Combat
{
    public class ScreenShakeDriver : MonoBehaviour
    {
        [SerializeField] private float hitMagnitude = 0.05f;
        [SerializeField] private float hitDuration = 0.1f;
        [SerializeField] private float critMagnitude = 0.12f;
        [SerializeField] private float critDuration = 0.18f;
        [SerializeField] private float commitBeat3Magnitude = 0.15f;
        [SerializeField] private float commitBeat3Duration = 0.2f;
        [SerializeField] private float killMagnitude = 0.1f;
        [SerializeField] private float killDuration = 0.15f;
        [SerializeField] private float dashMagnitude = 0.04f;
        [SerializeField] private float dashDuration = 0.08f;
        [SerializeField] private float minIcdSeconds = 0.04f;

        private Camera targetCamera;
        private Coroutine shakeCoroutine;
        private Vector3 baseLocalPosition;

        private void OnEnable()
        {
            CombatEventBus.OnHit += HandleHit;
            CombatEventBus.OnCommitBeat += HandleCommitBeat;
            CombatEventBus.OnKill += HandleKill;
            CombatEventBus.OnDash += HandleDash;
            CombatEventBus.OnTelegraph += HandleTelegraph;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
            CombatEventBus.OnCommitBeat -= HandleCommitBeat;
            CombatEventBus.OnKill -= HandleKill;
            CombatEventBus.OnDash -= HandleDash;
            CombatEventBus.OnTelegraph -= HandleTelegraph;
            ResetCameraPosition();
        }

        private void HandleHit(HitEvent e)
        {
            if (!FeelToggleSettings.ShakeEnabled || !ProcLimiter.TryProc("shake_hit", minIcdSeconds))
            {
                return;
            }

            if (e.isCrit)
            {
                Shake(critMagnitude, critDuration);
            }
            else
            {
                Shake(hitMagnitude, hitDuration);
            }
        }

        private void HandleCommitBeat(CommitBeatEvent e)
        {
            if (e.beatIndex == 3 && FeelToggleSettings.ShakeEnabled && ProcLimiter.TryProc("shake_commit3", minIcdSeconds))
            {
                Shake(commitBeat3Magnitude, commitBeat3Duration);
            }
        }

        private void HandleKill(KillEvent e)
        {
            if (!FeelToggleSettings.ShakeEnabled || !ProcLimiter.TryProc("shake_kill", minIcdSeconds))
            {
                return;
            }

            Shake(killMagnitude, killDuration);
        }

        private void HandleDash(DashEvent e)
        {
            if (!FeelToggleSettings.ShakeEnabled || !ProcLimiter.TryProc("shake_dash", minIcdSeconds))
            {
                return;
            }

            Shake(dashMagnitude, dashDuration);
        }

        private void HandleTelegraph(TelegraphEvent e)
        {
            if (!FeelToggleSettings.ShakeEnabled || !ProcLimiter.TryProc("shake_telegraph", minIcdSeconds))
            {
                return;
            }

            Shake(e.magnitude, e.duration);
        }

        public void Shake(float magnitude, float duration)
        {
            Camera cam = ResolveCamera();
            if (cam == null)
            {
                return;
            }

            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                ResetCameraPosition();
            }

            baseLocalPosition = cam.transform.localPosition;
            shakeCoroutine = StartCoroutine(ShakeRoutine(cam.transform, Mathf.Max(0f, magnitude), Mathf.Max(0.01f, duration)));
        }

        private IEnumerator ShakeRoutine(Transform cameraTransform, float magnitude, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && cameraTransform != null)
            {
                float t = elapsed / duration;
                float falloff = 1f - t;
                Vector2 offset = UnityEngine.Random.insideUnitCircle * magnitude * falloff;
                cameraTransform.localPosition = baseLocalPosition + new Vector3(offset.x, offset.y, 0f);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            ResetCameraPosition();
            shakeCoroutine = null;
        }

        private Camera ResolveCamera()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            return targetCamera;
        }

        private void ResetCameraPosition()
        {
            Camera cam = ResolveCamera();
            if (cam != null)
            {
                cam.transform.localPosition = baseLocalPosition;
            }
        }
    }
}
