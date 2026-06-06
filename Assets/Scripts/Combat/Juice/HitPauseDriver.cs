using System.Collections;
using RIMA.Combat.Juice;
using UnityEngine;

namespace RIMA.Combat
{
    public class HitPauseDriver : MonoBehaviour
    {
        public static HitPauseDriver Instance { get; private set; }

        // T2 juice tiers: light=.03 / heavy(crit)=.06 / kill=.12 / finisher=.18 / execute=.10 / boss=.20
        [SerializeField] private float pauseDurationHit = 0.03f;      // light hit
        [SerializeField] private float pauseDurationCrit = 0.06f;     // heavy / crit hit
        [SerializeField] private float pauseDurationKill = 0.12f;
        [SerializeField] private float pauseDurationFinisher = 0.18f; // Beat3 commit finisher
        [SerializeField] private float pauseDurationBossDeath = 0.20f;
        [SerializeField] private float pauseDurationExecute = 0.10f;  // DeathBlow execute freeze
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
            CombatEventBus.OnCommitBeat += HandleCommitBeat;
            CombatEventBus.OnKill += HandleKill;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
            CombatEventBus.OnCommitBeat -= HandleCommitBeat;
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

        private void HandleCommitBeat(CommitBeatEvent e)
        {
            if (e.beatIndex != 3 || !FeelToggleSettings.HitstopEnabled || !ProcLimiter.TryProc("hitpause_finisher", minIcdSeconds, 1))
            {
                return;
            }

            TriggerPause(pauseDurationFinisher);
        }

        private void HandleKill(KillEvent e)
        {
            if (!FeelToggleSettings.HitstopEnabled || !ProcLimiter.TryProc("hitpause_kill", minIcdSeconds, 1))
            {
                return;
            }

            TriggerPause(pauseDurationKill);
        }

        /// <summary>Called by ExecutePromptDriver when DeathBlow fires — dedicated execute freeze tier.
        /// Overrides any in-progress hit pause so total freeze ≈ pauseDurationExecute (not stacked).</summary>
        public void TriggerExecutePause()
        {
            if (!FeelToggleSettings.HitstopEnabled) return;
            // Clear any running pause so the execute duration is not stacked on top of it.
            if (pauseCoroutine != null)
            {
                StopCoroutine(pauseCoroutine);
                pendingExtraSeconds = 0f;
                Time.timeScale = previousTimeScale;
                pauseCoroutine = null;
            }
            TriggerPause(pauseDurationExecute);
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
