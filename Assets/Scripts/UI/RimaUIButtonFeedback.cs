using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RIMA.UI
{
    public sealed class RimaUIButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Outline border;
        [SerializeField] private Color normalBorder = new Color(0.48f, 0.65f, 0.74f, 1f);
        [SerializeField] private Color hoverBorder = new Color(0f, 1f, 0.8f, 1f);
        [SerializeField] private float pressedScale = 0.95f;

        private RectTransform rectTransform;
        private Vector3 baseScale;
        private bool selected;
        private bool hovering;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
            baseScale = rectTransform != null ? rectTransform.localScale : Vector3.one;
            if (border == null) border = GetComponent<Outline>();
            ApplyBorder();
        }

        public void SetSelected(bool value)
        {
            selected = value;
            ApplyBorder();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            ApplyBorder();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            if (rectTransform != null) rectTransform.localScale = baseScale;
            ApplyBorder();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (rectTransform != null) rectTransform.localScale = baseScale * pressedScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (rectTransform != null) rectTransform.localScale = baseScale;
        }

        private void ApplyBorder()
        {
            if (border == null) return;
            border.effectColor = (hovering || selected) ? hoverBorder : normalBorder;
            border.effectDistance = Vector2.one;
        }
    }
}
