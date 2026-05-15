using UnityEngine;
using RIMA.Combat;
using RIMA.Combat.Juice;

namespace RIMA
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class VignetteFlashController : MonoBehaviour
    {
        public static VignetteFlashController Instance { get; private set; }

        [SerializeField] private Color hitColor = new Color(0.4f, 0.7f, 1f, 0.6f);
        [SerializeField] private Color critColor = new Color(1f, 0.85f, 0.3f, 0.7f);
        [SerializeField] private Color killColor = new Color(0.55f, 0.2f, 0.9f, 0.75f);
        [SerializeField] private float decayPerSecond = 5f;
        [SerializeField] private float maxAlpha = 0.85f;

        private SpriteRenderer spriteRenderer;
        private Color currentColor;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentColor = new Color(0f, 0f, 0f, 0f);
            ApplyCurrent();
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
            currentColor.a = 0f;
            ApplyCurrent();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            if (currentColor.a <= 0f)
            {
                return;
            }

            currentColor.a = Mathf.MoveTowards(currentColor.a, 0f, decayPerSecond * Time.unscaledDeltaTime);
            ApplyCurrent();
        }

        private void HandleHit(HitEvent e)
        {
            if (!FeelToggleSettings.VignetteEnabled || !ProcLimiter.TryProc("vignette_hit"))
            {
                return;
            }

            Pulse(e.isCrit ? critColor : hitColor);
        }

        private void HandleKill(KillEvent e)
        {
            if (!FeelToggleSettings.VignetteEnabled || !ProcLimiter.TryProc("vignette_kill"))
            {
                return;
            }

            Pulse(killColor);
        }

        public void Pulse(Color color)
        {
            currentColor = new Color(color.r, color.g, color.b, Mathf.Min(color.a, maxAlpha));
            ApplyCurrent();
        }

        private void ApplyCurrent()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = currentColor;
            }
        }
    }
}
