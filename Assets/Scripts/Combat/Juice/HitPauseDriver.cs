using System.Collections;
using RIMA.Combat.Juice;
using UnityEngine;

namespace RIMA.Combat
{
    public class HitPauseDriver : MonoBehaviour
    {
        public static HitPauseDriver Instance { get; private set; }

        [SerializeField] private float pauseDurationHit = 0.04f;
        [SerializeField] private float pauseDurationCrit = 0.08f;
        [SerializeField] private float pauseDurationKill = 0.14f;
        [SerializeField] private float pauseTimeScale = 0f;
        [SerializeField] private float minIcdSeconds = 0.05f;

        private Coroutine pauseCoroutine;
        private float previousTimeScale = 1f;
        private float pendingExtraSeconds;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            CombatEventBus.OnHit += HandleHit;
            CombatEventBus.OnKill += HandleKill;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
            CombatEventBus.OnKill -= HandleKill;

            if (pauseCoroutine != null)
            {
                StopCoroutine(pauseCoroutine);
                Time.timeScale = previousTimeScale;
                pauseCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void HandleHit(HitEvent e)
        {
            if (!FeelToggleSettings.HitstopEnabled || !ProcLimiter.TryProc("hitpause_hit", minIcdSeconds, 1))
            {
                return;
            }

            TriggerPause(e.isCrit ? pauseDurationCrit : pauseDurationHit);
        }

        private void HandleKill(KillEvent e)
        {
            if (!FeelToggleSettings.HitstopEnabled || !ProcLimiter.TryProc("hitpause_kill", minIcdSeconds, 1))
            {
                return;
            }

            TriggerPause(pauseDurationKill);
        }

        public void TriggerPause(float duration)
        {
            if (pauseCoroutine != null)
            {
                pendingExtraSeconds = Mathf.Max(pendingExtraSeconds, duration);
                return;
            }

            pauseCoroutine = StartCoroutine(PauseRoutine(Mathf.Max(0f, duration)));
        }

        private IEnumerator PauseRoutine(float duration)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = Mathf.Clamp01(pauseTimeScale);
            pendingExtraSeconds = 0f;

            yield return new WaitForSecondsRealtime(duration);

            while (pendingExtraSeconds > 0f)
            {
                float extra = pendingExtraSeconds;
                pendingExtraSeconds = 0f;
                yield return new WaitForSecondsRealtime(extra);
            }

            Time.timeScale = previousTimeScale;
            pauseCoroutine = null;
        }
    }
}
