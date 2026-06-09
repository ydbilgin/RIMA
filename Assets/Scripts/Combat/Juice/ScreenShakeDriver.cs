using System.Collections;
using RIMA.Combat.Juice;
using UnityEngine;

namespace RIMA.Combat
{
    public class ScreenShakeDriver : MonoBehaviour
    {
        public static ScreenShakeDriver Instance { get; private set; }

        public Vector3 CurrentOffset { get; private set; }

        // T2 shake tiers: S=small (light hit), M=medium (heavy/knockdown), L=large (execute/finisher)
        [SerializeField] private float hitMagnitude = 0.04f;           // S — light hit
        [SerializeField] private float hitDuration = 0.10f;
        [SerializeField] private float critMagnitude = 0.10f;          // M — heavy/crit hit
        [SerializeField] private float critDuration = 0.18f;
        [SerializeField] private float commitBeat3Magnitude = 0.18f;   // L — finisher
        [SerializeField] private float commitBeat3Duration = 0.22f;
        [SerializeField] private float killMagnitude = 0.10f;          // M — kill
        [SerializeField] private float killDuration = 0.15f;
        [SerializeField] private float dashMagnitude = 0.04f;
        [SerializeField] private float dashDuration = 0.08f;
        [SerializeField] private float knockdownMagnitude = 0.13f;     // M — knockdown land
        [SerializeField] private float knockdownDuration = 0.22f;
        [SerializeField] private float executeMagnitude = 0.18f;       // L — execute payoff
        [SerializeField] private float executeDuration = 0.28f;
        [SerializeField] private float minIcdSeconds = 0.04f;

        private Coroutine shakeCoroutine;

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
            ResetOffset();
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
            if (!FeelToggleSettings.ShakeEnabled || !ProcLimiter.TryProc("shake_hit", minIcdSeconds))
            {
                return;
            }

            if (e.isCrit)
            {
                Shake(critMagnitude, critDuration, e.hitDirection);
            }
            else
            {
                Shake(hitMagnitude, hitDuration, e.hitDirection);
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

        /// <summary>Called by ExecutePromptDriver on DeathBlow fire — L-tier execute shake.</summary>
        public void TriggerExecuteShake()
        {
            if (!FeelToggleSettings.ShakeEnabled) return;
            Shake(executeMagnitude, executeDuration);
        }

        /// <summary>Called by knockdown land — M-tier knockdown shake.</summary>
        public void TriggerKnockdownShake()
        {
            if (!FeelToggleSettings.ShakeEnabled) return;
            Shake(knockdownMagnitude, knockdownDuration);
        }

        public void Shake(float magnitude, float duration)
        {
            Shake(magnitude, duration, Vector2.zero);
        }

        public void Shake(float magnitude, float duration, Vector2 hitDirection)
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                ResetOffset();
            }

            shakeCoroutine = StartCoroutine(ShakeRoutine(Mathf.Max(0f, magnitude), Mathf.Max(0.01f, duration), hitDirection));
        }

        private IEnumerator ShakeRoutine(float magnitude, float duration, Vector2 hitDirection)
        {
            Vector2 axis = hitDirection.sqrMagnitude > 0.0001f ? hitDirection.normalized : Vector2.zero;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float falloff = 1f - t;
                Vector2 random = UnityEngine.Random.insideUnitCircle;
                Vector2 directional = axis != Vector2.zero ? axis * UnityEngine.Random.Range(-1f, 1f) : random;
                Vector2 offset = Vector2.ClampMagnitude(directional * 0.6f + random * 0.4f, 1f) * magnitude * falloff;
                CurrentOffset = new Vector3(offset.x, offset.y, 0f);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            ResetOffset();
            shakeCoroutine = null;
        }

        private void ResetOffset()
        {
            CurrentOffset = Vector3.zero;
        }

        /// <summary>
        /// Runtime bootstrap: if no ScreenShakeDriver exists in the loaded scene
        /// (e.g. _Arena which has no juice prefab wired), create one on a hidden
        /// GameObject so boss shake calls are never silently null.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void EnsureInstance()
        {
            if (Instance != null) return;
            var go = new GameObject("[ScreenShakeDriver-Auto]");
            DontDestroyOnLoad(go);
            go.AddComponent<ScreenShakeDriver>();
        }
    }
}
