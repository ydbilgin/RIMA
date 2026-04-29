using System.Collections;
using TMPro;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Hasar sayısı popup'ı. Health.OnDamageTaken → buradan spawn edilir.
    /// Prefab: DamagePopup.prefab — TextMeshPro + bu script.
    /// Kullanım: DamagePopup.Show(position, amount, color)
    /// </summary>
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private float riseSpeed  = 1.8f;
        [SerializeField] private float lifetime   = 0.7f;
        [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 1.4f, 1f, 0f);

        private static GameObject prefab;

        // ─── Static spawn helper ─────────────────────────────────────────

        public static void Show(Vector3 worldPos, int damage, Color? color = null)
        {
            if (prefab == null)
            {
                prefab = Resources.Load<GameObject>("DamagePopup");
                if (prefab == null) { Debug.LogWarning("[RIMA] DamagePopup prefab bulunamadı: Resources/DamagePopup"); return; }
            }

            var go = Instantiate(prefab, worldPos + Vector3.up * 0.3f, Quaternion.identity);
            var popup = go.GetComponent<DamagePopup>();
            if (popup == null) return;

            popup.tmp.text  = damage.ToString();
            popup.tmp.color = color ?? Color.white;
            popup.tmp.fontSize = damage >= 40 ? 5f : 3.5f;  // finisher daha büyük
        }

        // ─── Instance lifecycle ──────────────────────────────────────────

        private void Awake()
        {
            if (tmp == null) tmp = GetComponent<TextMeshPro>();
        }

        private void OnEnable() => StartCoroutine(Animate());

        private IEnumerator Animate()
        {
            float elapsed = 0f;
            Vector3 startPos = transform.position;

            while (elapsed < lifetime)
            {
                float t = elapsed / lifetime;
                transform.position = startPos + Vector3.up * (riseSpeed * elapsed);
                float s = scaleCurve.Evaluate(t);
                transform.localScale = Vector3.one * s;

                // Fade out son %40
                if (tmp != null && t > 0.6f)
                {
                    var c = tmp.color;
                    c.a = Mathf.Lerp(1f, 0f, (t - 0.6f) / 0.4f);
                    tmp.color = c;
                }

                elapsed += Time.unscaledDeltaTime;  // HitStop sırasında da görünsün
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
