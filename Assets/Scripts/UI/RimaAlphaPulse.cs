using UnityEngine;

namespace RIMA.UI
{
    public sealed class RimaAlphaPulse : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float minAlpha = 0.85f;
        [SerializeField] private float maxAlpha = 1f;
        [SerializeField] private float period = 3f;

        private void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (canvasGroup == null || period <= 0f) return;
            float t = (Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f / period) + 1f) * 0.5f;
            canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
        }
    }
}
