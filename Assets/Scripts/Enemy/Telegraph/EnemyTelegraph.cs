using RIMA.Combat;
using UnityEngine;

namespace RIMA.Enemy.Telegraph
{
    [DisallowMultipleComponent]
    public class EnemyTelegraph : MonoBehaviour
    {
        [Header("Channels")]
        [SerializeField] private EnemyOutlinePulse outlinePulse;
        [SerializeField] private TelegraphGroundMarker groundMarker;
        [SerializeField] private bool channelScreenShake = true;

        [Header("Screen Shake")]
        [SerializeField] private float screenShakeMagnitude = 0.05f;
        [SerializeField] private float screenShakeDuration = 0.3f;

        private ScreenShakeDriver _shakeDriver;

        private void Awake()
        {
            ResolveChannels();
            _shakeDriver = FindFirstObjectByType<ScreenShakeDriver>();
        }

        public void StartTelegraph(float duration, float aoeRadius)
        {
            ResolveChannels();

            if (outlinePulse != null)
            {
                outlinePulse.TelegraphStart(duration);
            }

            if (groundMarker != null)
            {
                groundMarker.TelegraphStart(duration, aoeRadius);
            }

            if (channelScreenShake)
            {
                TriggerScreenShake(aoeRadius);
            }
        }

        public void CancelTelegraph()
        {
            if (outlinePulse != null)
            {
                outlinePulse.TelegraphEnd();
            }

            if (groundMarker != null)
            {
                groundMarker.TelegraphEnd();
            }
        }

        private void ResolveChannels()
        {
            if (outlinePulse == null)
            {
                outlinePulse = GetComponent<EnemyOutlinePulse>();
            }

            if (groundMarker == null)
            {
                groundMarker = GetComponent<TelegraphGroundMarker>();
            }
        }

        private void TriggerScreenShake(float aoeRadius)
        {
            if (_shakeDriver == null)
            {
                return;
            }

            CombatEventBus.PublishTelegraph(new TelegraphEvent
            {
                worldPos = transform.position,
                source = gameObject,
                magnitude = Mathf.Max(0f, screenShakeMagnitude),
                duration = Mathf.Max(0.01f, screenShakeDuration),
                radius = Mathf.Max(0.01f, aoeRadius)
            });
        }
    }
}
