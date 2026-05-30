using UnityEngine;

namespace RIMA.Combat.Juice
{
    public sealed class CameraPunchController : MonoBehaviour
    {
        public static CameraPunchController Instance { get; private set; }

        /// <summary>Decaying punch offset. Read+added by CameraFollow; this controller does
        /// NOT write the camera transform itself (that would fight the follow and pin the camera).</summary>
        public Vector3 CurrentOffset { get; private set; }

        [SerializeField] private float impulseHit = 0.08f;
        [SerializeField] private float impulseCrit = 0.16f;
        [SerializeField] private float impulseKill = 0.22f;
        [SerializeField] private float impulseDash = 0.10f;
        [SerializeField] private float decayPerSecond = 6f;
        [SerializeField] private float maxOffset = 0.5f;

        private Vector3 currentOffset;

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
            CombatEventBus.OnDash += HandleDash;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
            CombatEventBus.OnKill -= HandleKill;
            CombatEventBus.OnDash -= HandleDash;
            ResetCamera();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void LateUpdate()
        {
            // Do NOT write the camera transform — that fights CameraFollow and pins the camera
            // to a stale captured origin. Decay the offset and expose it via CurrentOffset;
            // CameraFollow adds it on top of the follow position (same pattern as CameraShake).
            if (currentOffset != Vector3.zero)
            {
                currentOffset = Vector3.MoveTowards(currentOffset, Vector3.zero, decayPerSecond * Time.unscaledDeltaTime);
            }

            CurrentOffset = currentOffset;
        }

        private void HandleHit(HitEvent e)
        {
            if (!FeelToggleSettings.CameraPunchEnabled || !ProcLimiter.TryProc("punch_hit"))
            {
                return;
            }

            Apply(-e.hitDirection, e.isCrit ? impulseCrit : impulseHit);
        }

        private void HandleKill(KillEvent e)
        {
            if (!FeelToggleSettings.CameraPunchEnabled || !ProcLimiter.TryProc("punch_kill"))
            {
                return;
            }

            Apply(Vector2.zero, impulseKill);
        }

        private void HandleDash(DashEvent e)
        {
            if (!FeelToggleSettings.CameraPunchEnabled || !ProcLimiter.TryProc("punch_dash"))
            {
                return;
            }

            Vector3 dir = e.endPos - e.startPos;
            if (dir.sqrMagnitude < 0.0001f)
            {
                return;
            }
            dir.Normalize();
            Apply(new Vector2(dir.x, dir.y), impulseDash);
        }

        public void Apply(Vector2 direction, float magnitude)
        {
            Vector3 impulse = direction.sqrMagnitude > 0.0001f
                ? new Vector3(direction.x, direction.y, 0f).normalized * magnitude
                : Random.insideUnitSphere * magnitude;
            impulse.z = 0f;
            currentOffset = Vector3.ClampMagnitude(currentOffset + impulse, maxOffset);
        }

        private void ResetCamera()
        {
            currentOffset = Vector3.zero;
            CurrentOffset = Vector3.zero;
        }
    }
}
