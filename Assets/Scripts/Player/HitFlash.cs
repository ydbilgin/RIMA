using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Hasar alınca SpriteRenderer'ı kısa süre beyaz yapar.
    /// Health.OnHealthChanged eventine otomatik bağlanır.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class HitFlash : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private Color flashColor = Color.white;

        private SpriteRenderer sr;
        private Color originalColor;
        private Coroutine flashRoutine;
        private Health health;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            originalColor = sr.color;
            health = GetComponentInParent<Health>();
        }

        private void OnEnable()
        {
            if (health != null)
                health.OnHealthChanged.AddListener(OnHit);
        }

        private void OnDisable()
        {
            if (health != null)
                health.OnHealthChanged.RemoveListener(OnHit);
        }

        private void OnHit(int current, int max)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            sr.color = flashColor;
            yield return new WaitForSecondsRealtime(flashDuration);  // hitstop sırasında da çalışır
            sr.color = originalColor;
            flashRoutine = null;
        }
    }
}
