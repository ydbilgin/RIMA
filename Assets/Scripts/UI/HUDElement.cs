using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Bir HUD paneline ekle → HUDEditorManager açıkken drag + resize + scale destekler.
    ///
    /// Özellikler:
    ///   - Grid snap (release'de en yakın grid hücresine oturur)
    ///   - Overlap rejection (başka elemana değiyorsa orijinal pozisyona döner)
    ///   - Scale presets: 75 / 100 / 125 / 150 %
    ///   - 4 köşe resize handle
    ///   - ESC ile edit mode'dayken yapılan değişiklik iptal edilir
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class HUDElement : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("Identity")]
        [SerializeField] private string elementId;
        public string ElementId => elementId;

        [Header("Constraints")]
        [SerializeField] private bool allowResize = true;
        [SerializeField] private Vector2 minSize = new Vector2(40, 20);
        [SerializeField] private Vector2 maxSize = new Vector2(800, 600);

        // ── Rects ──────────────────────────────────────────────────
        private RectTransform rt;
        private Canvas rootCanvas;

        // Varsayılan değerler (Reset için)
        private Vector2 defaultPosition;
        private Vector2 defaultSize;
        private float defaultScale = 1f;

        // Drag state
        private bool isDragging;
        private Vector2 dragOffset;      // pointer ile elemanın pivot arasındaki fark
        private Vector2 posBeforeDrag;   // overlap reject için

        // Resize state
        private bool isResizing;
        private int resizeCorner;        // 0=BL 1=BR 2=TR 3=TL
        private Vector2 resizeStartSize;
        private Vector2 resizeStartPos;
        private Vector2 resizeStartMouse;

        // Edit mode görsel
        private GameObject editBorder;
        private GameObject[] cornerHandles = new GameObject[4];
        private GameObject scalePanel;

        // ── Lifecycle ──────────────────────────────────────────────

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            rootCanvas = GetComponentInParent<Canvas>();

            defaultPosition = rt.anchoredPosition;
            defaultSize = rt.sizeDelta;
        }

        private void Start()
        {
            HUDEditorManager.Instance?.Register(this);
        }

        private void OnDestroy()
        {
            HUDEditorManager.Instance?.Unregister(this);
        }

        // ── Ekran alanı (overlap check için) ──────────────────────

        public Rect ScreenRect
        {
            get
            {
                var corners = new Vector3[4];
                rt.GetWorldCorners(corners);
                return new Rect(corners[0].x, corners[0].y,
                                corners[2].x - corners[0].x,
                                corners[2].y - corners[0].y);
            }
        }

        // ── Save / Load ────────────────────────────────────────────

        public HUDElementEntry Capture() => new HUDElementEntry
        {
            id = elementId,
            anchoredPosition = rt.anchoredPosition,
            sizeDelta = rt.sizeDelta,
            scale = rt.localScale.x
        };

        public void Apply(HUDElementEntry e)
        {
            rt.anchoredPosition = e.anchoredPosition;
            rt.sizeDelta = e.sizeDelta;
            rt.localScale = Vector3.one * e.scale;
        }

        public void ResetToDefault()
        {
            rt.anchoredPosition = defaultPosition;
            rt.sizeDelta = defaultSize;
            rt.localScale = Vector3.one * defaultScale;
        }

        // ── Edit Mode ──────────────────────────────────────────────

        public void EnterEditMode()
        {
            BuildEditVisuals();
        }

        public void ExitEditMode()
        {
            DestroyEditVisuals();
            isDragging = false;
            isResizing = false;
        }

        // ── Drag (IPointerDownHandler vb.) ────────────────────────

        public void OnPointerDown(PointerEventData e)
        {
            if (!HUDEditorManager.Instance.IsEditing) return;

            posBeforeDrag = rt.anchoredPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rt, e.position, e.pressEventCamera, out Vector2 local);
            dragOffset = rt.anchoredPosition - LocalToAnchored(local);

            isDragging = true;
        }

        public void OnDrag(PointerEventData e)
        {
            if (!isDragging || !HUDEditorManager.Instance.IsEditing) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rt.parent as RectTransform, e.position, e.pressEventCamera, out Vector2 parentLocal);

            rt.anchoredPosition = parentLocal + dragOffset;
        }

        public void OnPointerUp(PointerEventData e)
        {
            if (!isDragging) return;
            isDragging = false;

            if (!HUDEditorManager.Instance.IsEditing) return;

            // Grid snap
            var mgr = HUDEditorManager.Instance;
            Vector2 snapped = SnapAnchoredToGrid(rt.anchoredPosition, mgr);
            rt.anchoredPosition = snapped;

            // Overlap check — reddedilirse eski yere dön
            if (mgr.Overlaps(this, ScreenRect))
                rt.anchoredPosition = posBeforeDrag;

            // Ekran sınırları dışına çıkmayı engelle
            ClampToScreen();
        }

        // ── Scale Presets ─────────────────────────────────────────

        public void SetScale(float factor)
        {
            rt.localScale = Vector3.one * factor;

            // Overlap — reddedilirse default scale'e dön
            if (HUDEditorManager.Instance.Overlaps(this, ScreenRect))
                rt.localScale = Vector3.one * defaultScale;
        }

        // ── Resize (köşe handle sürükleme) ───────────────────────

        private void BeginResize(int corner, Vector2 mouseScreen)
        {
            isResizing = true;
            resizeCorner = corner;
            resizeStartSize = rt.sizeDelta;
            resizeStartPos = rt.anchoredPosition;
            resizeStartMouse = mouseScreen;
        }

        private void UpdateResize(Vector2 mouseScreen)
        {
            if (!isResizing) return;

            Vector2 delta = mouseScreen - resizeStartMouse;

            // Her köşe farklı eksen yönü etkiler
            // 0=BL: -x,-y | 1=BR: +x,-y | 2=TR: +x,+y | 3=TL: -x,+y
            float dx = (resizeCorner == 1 || resizeCorner == 2) ? delta.x : -delta.x;
            float dy = (resizeCorner == 2 || resizeCorner == 3) ? delta.y : -delta.y;

            Vector2 newSize = resizeStartSize + new Vector2(dx, dy);
            newSize.x = Mathf.Clamp(newSize.x, minSize.x, maxSize.x);
            newSize.y = Mathf.Clamp(newSize.y, minSize.y, maxSize.y);
            rt.sizeDelta = newSize;

            // Pivot'u korumak için pozisyonu ayarla
            Vector2 sizeDiff = newSize - resizeStartSize;
            float px = (resizeCorner == 0 || resizeCorner == 3) ? -sizeDiff.x * 0.5f : sizeDiff.x * 0.5f;
            float py = (resizeCorner == 0 || resizeCorner == 1) ? -sizeDiff.y * 0.5f : sizeDiff.y * 0.5f;
            rt.anchoredPosition = resizeStartPos + new Vector2(px, py);
        }

        private void EndResize()
        {
            if (!isResizing) return;
            isResizing = false;

            if (HUDEditorManager.Instance.Overlaps(this, ScreenRect))
            {
                rt.sizeDelta = resizeStartSize;
                rt.anchoredPosition = resizeStartPos;
            }
        }

        // ── Edit Visuals ──────────────────────────────────────────

        private void BuildEditVisuals()
        {
            // Kenarlık
            editBorder = MakeChild("EditBorder");
            var img = editBorder.AddComponent<Image>();
            img.color = new Color(0.3f, 0.7f, 1f, 0.4f);
            img.raycastTarget = false;
            var brt = editBorder.GetComponent<RectTransform>();
            brt.anchorMin = Vector2.zero;
            brt.anchorMax = Vector2.one;
            brt.offsetMin = new Vector2(-2, -2);
            brt.offsetMax = new Vector2(2, 2);

            // 4 köşe handle
            if (allowResize)
            {
                Vector2[] corners = { new(0, 0), new(1, 0), new(1, 1), new(0, 1) };
                for (int i = 0; i < 4; i++)
                {
                    int idx = i; // closure için
                    var h = MakeCornerHandle(corners[i]);
                    cornerHandles[i] = h;

                    var trigger = h.AddComponent<ResizeHandle>();
                    trigger.Setup(
                        onBegin: pos => BeginResize(idx, pos),
                        onDrag:  pos => UpdateResize(pos),
                        onEnd:   ()  => EndResize()
                    );
                }
            }

            // Scale preset butonları
            BuildScalePanel();
        }

        private void DestroyEditVisuals()
        {
            if (editBorder != null) Destroy(editBorder);
            foreach (var h in cornerHandles)
                if (h != null) Destroy(h);
            if (scalePanel != null) Destroy(scalePanel);
        }

        private void BuildScalePanel()
        {
            scalePanel = MakeChild("ScalePanel");
            var panelRt = scalePanel.GetComponent<RectTransform>();
            panelRt.anchorMin = new Vector2(0, 1);
            panelRt.anchorMax = new Vector2(0, 1);
            panelRt.pivot = new Vector2(0, 0);
            panelRt.anchoredPosition = new Vector2(0, 4);
            panelRt.sizeDelta = new Vector2(180, 26);

            var bg = scalePanel.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.1f, 0.85f);

            float[] presets = { 0.75f, 1f, 1.25f, 1.5f };
            string[] labels = { "75%", "100%", "125%", "150%" };

            float btnW = 42f;
            for (int i = 0; i < 4; i++)
            {
                float f = presets[i];
                var btn = MakeScaleButton(scalePanel.transform, labels[i], new Vector2(4 + i * (btnW + 2), 3), new Vector2(btnW, 20));
                btn.GetComponent<Button>().onClick.AddListener(() => SetScale(f));
            }
        }

        private GameObject MakeCornerHandle(Vector2 anchor)
        {
            const float s = 10f;
            var go = MakeChild($"Handle_{anchor}");
            var hrt = go.GetComponent<RectTransform>();
            hrt.anchorMin = anchor;
            hrt.anchorMax = anchor;
            hrt.pivot = anchor;
            hrt.anchoredPosition = Vector2.zero;
            hrt.sizeDelta = new Vector2(s, s);

            var img = go.AddComponent<Image>();
            img.color = new Color(0.3f, 0.7f, 1f, 0.9f);
            return go;
        }

        private GameObject MakeScaleButton(Transform parent, string label, Vector2 pos, Vector2 size)
        {
            var go = new GameObject($"Btn_{label}", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var brt = go.GetComponent<RectTransform>();
            brt.anchorMin = brt.anchorMax = new Vector2(0, 0);
            brt.pivot = new Vector2(0, 0);
            brt.anchoredPosition = pos;
            brt.sizeDelta = size;

            var img = go.AddComponent<Image>();
            img.color = new Color(0.15f, 0.2f, 0.3f, 1f);

            go.AddComponent<Button>();

            var txtGo = new GameObject("Txt", typeof(RectTransform));
            txtGo.transform.SetParent(go.transform, false);
            var trt = txtGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = trt.offsetMax = Vector2.zero;
            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.fontSize = 10;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            tmp.raycastTarget = false;

            return go;
        }

        private GameObject MakeChild(string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(rt, false);
            return go;
        }

        // ── Yardımcı ──────────────────────────────────────────────

        private Vector2 LocalToAnchored(Vector2 local)
        {
            // RectTransform local → anchored dönüşüm
            var parentRt = rt.parent as RectTransform;
            if (parentRt == null) return local;
            Vector2 parentSize = parentRt.rect.size;
            Vector2 anchorOffset = new Vector2(
                (rt.anchorMin.x + rt.anchorMax.x) * 0.5f * parentSize.x - parentSize.x * 0.5f,
                (rt.anchorMin.y + rt.anchorMax.y) * 0.5f * parentSize.y - parentSize.y * 0.5f
            );
            return local + anchorOffset;
        }

        private Vector2 SnapAnchoredToGrid(Vector2 anchored, HUDEditorManager mgr)
        {
            // Anchored → screen → snap → anchored geri
            var parentRt = rt.parent as RectTransform;
            if (parentRt == null) return anchored;

            Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(
                rootCanvas.worldCamera, rt.position);
            Vector2 snapped = mgr.SnapToGrid(screenCenter);

            // fark kadar anchored'ı kaydır
            Vector2 diff = snapped - screenCenter;
            return anchored + diff;
        }

        private void ClampToScreen()
        {
            var parentRt = rt.parent as RectTransform;
            if (parentRt == null) return;

            Vector2 parentSize = parentRt.rect.size;
            Vector2 halfSize = rt.sizeDelta * rt.localScale.x * 0.5f;

            float minX = -parentSize.x * 0.5f + halfSize.x;
            float maxX =  parentSize.x * 0.5f - halfSize.x;
            float minY = -parentSize.y * 0.5f + halfSize.y;
            float maxY =  parentSize.y * 0.5f - halfSize.y;

            rt.anchoredPosition = new Vector2(
                Mathf.Clamp(rt.anchoredPosition.x, minX, maxX),
                Mathf.Clamp(rt.anchoredPosition.y, minY, maxY)
            );
        }
    }

    // ── Köşe handle input handler ─────────────────────────────────

    public class ResizeHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private Action<Vector2> _onBegin;
        private Action<Vector2> _onDrag;
        private Action _onEnd;

        public void Setup(Action<Vector2> onBegin, Action<Vector2> onDrag, Action onEnd)
        {
            _onBegin = onBegin;
            _onDrag  = onDrag;
            _onEnd   = onEnd;

            // Raycast target olması lazım
            var img = GetComponent<Image>();
            if (img != null) img.raycastTarget = true;
        }

        public void OnPointerDown(PointerEventData e) => _onBegin?.Invoke(e.position);
        public void OnDrag(PointerEventData e)        => _onDrag?.Invoke(e.position);
        public void OnPointerUp(PointerEventData e)   => _onEnd?.Invoke();
    }
}
