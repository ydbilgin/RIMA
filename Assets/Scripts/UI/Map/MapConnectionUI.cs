using UnityEngine;
using UnityEngine.UI;

namespace RIMA.UI.Map
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class MapConnectionUI : MonoBehaviour
    {
        [SerializeField] private Image lineImage;
        [SerializeField] private float thickness = 4f;

        private RectTransform rectTransform;

        private void Awake()
        {
            EnsureReferences();
        }

        public void SetEndpoints(Vector2 from, Vector2 to, Color color, float lineThickness)
        {
            EnsureReferences();

            Vector2 delta = to - from;
            rectTransform.anchoredPosition = (from + to) * 0.5f;
            rectTransform.sizeDelta = new Vector2(delta.magnitude, Mathf.Max(1f, lineThickness));
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);

            thickness = lineThickness;
            lineImage.color = color;
            lineImage.raycastTarget = false;
        }

        public void SetColor(Color color)
        {
            EnsureReferences();
            lineImage.color = color;
        }

        private void EnsureReferences()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            if (lineImage == null)
                lineImage = GetComponent<Image>();

            if (lineImage != null)
            {
                lineImage.color = new Color(0.2f, 0.95f, 0.82f, 0.55f);
                lineImage.raycastTarget = false;
            }

            if (rectTransform != null && rectTransform.sizeDelta.y <= 0f)
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, thickness);
        }
    }
}
