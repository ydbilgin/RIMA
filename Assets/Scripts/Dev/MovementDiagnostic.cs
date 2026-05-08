using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace RIMA.Dev
{
    /// <summary>
    /// Runtime movement/input diagnostic tool.
    /// Logs PlayerController state, timeScale, moveAction enabled/phase,
    /// and UIManager overlay states every second.
    /// Only compiled in the Unity Editor (Conditional("UNITY_EDITOR")).
    /// </summary>
    [AddComponentMenu("RIMA/Dev/Movement Diagnostic")]
    public class MovementDiagnostic : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private float logInterval = 1f;

        private float _timer;

        private void Awake()
        {
            if (player == null)
                player = FindFirstObjectByType<PlayerController>();
        }

        private void Update()
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer >= logInterval)
            {
                _timer = 0f;
                Log();
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void Log()
        {
            // ── timeScale ────────────────────────────────────────────────
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"[MovementDiag] timeScale={Time.timeScale:F3}");

            // ── PlayerController state ───────────────────────────────────
            if (player != null)
            {
                var rb = player.GetComponent<Rigidbody2D>();
                var vel = rb != null ? rb.linearVelocity : Vector2.zero;

                sb.AppendLine($"[MovementDiag]   IsMoving={player.IsMoving}  FacingDirection={player.FacingDirection}");
                sb.AppendLine($"[MovementDiag]   Rigidbody2D.linearVelocity={vel}");

                // ── moveAction via reflection ────────────────────────────
                var fi = typeof(PlayerController).GetField(
                    "moveAction",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (fi != null)
                {
                    var action = fi.GetValue(player);
                    if (action != null)
                    {
                        // UnityEngine.InputSystem.InputAction has .enabled and .phase
                        var actionType = action.GetType();
                        var enabledProp = actionType.GetProperty("enabled");
                        var phaseProp   = actionType.GetProperty("phase");
                        var enabledVal = enabledProp?.GetValue(action);
                        var phaseVal   = phaseProp?.GetValue(action);
                        sb.AppendLine($"[MovementDiag]   moveAction.enabled={enabledVal}  .phase={phaseVal}");
                    }
                    else
                    {
                        sb.AppendLine("[MovementDiag]   moveAction=null");
                    }
                }
                else
                {
                    sb.AppendLine("[MovementDiag]   moveAction field not found via reflection");
                }
            }
            else
            {
                sb.AppendLine("[MovementDiag]   PlayerController not found in scene");
            }

            // ── UIManager State ──────────────────────────────────────────
            if (UIManager.Instance != null)
            {
                sb.AppendLine($"[MovementDiag]   UIManager.IsTabOpen={UIManager.Instance.IsTabOpen}");
                sb.AppendLine($"[MovementDiag]   UIManager.IsSettingsOpen={UIManager.Instance.IsSettingsOpen}");
                sb.AppendLine($"[MovementDiag]   UIManager.IsSkillOfferOpen={UIManager.Instance.IsSkillOfferOpen}");
            }
            else
            {
                sb.AppendLine("[MovementDiag]   UIManager.Instance=null");
            }

            UnityEngine.Debug.Log(sb.ToString());
        }
    }
}
