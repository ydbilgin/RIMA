using UnityEngine;

namespace RIMA.UI
{
    public sealed class RimaRiftCrackAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private Vector2 amplitude = new Vector2(3f, 1.5f);
        [SerializeField] private float period = 2.4f;

        private Vector2 basePosition;

        private void Awake()
        {
            if (target == null) target = transform as RectTransform;
            if (target != null) basePosition = target.anchoredPosition;
        }

        private void Update()
        {
            if (target == null || period <= 0f) return;
            float t = Time.unscaledTime * Mathf.PI * 2f / period;
            target.anchoredPosition = basePosition + new Vector2(Mathf.Sin(t) * amplitude.x, Mathf.Sin(t * 0.7f) * amplitude.y);
        }
    }
}
