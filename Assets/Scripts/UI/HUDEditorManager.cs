using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// BDO-style HUD editor.
    /// - ESC+H (veya menu butonu) ile edit mode aç/kapat
    /// - Grid snap, overlap rejection, scale presets (75/100/125/150%)
    /// - Layout JSON olarak persistentDataPath'e kaydedilir
    /// </summary>
    public class HUDEditorManager : MonoBehaviour
    {
        public static HUDEditorManager Instance { get; private set; }

        [Header("Grid")]
        [SerializeField] private int gridSize = 10;          // px — snap resolution
        [SerializeField] private Color gridColor = new Color(1f, 1f, 1f, 0.08f);

        [Header("Edit Mode UI")]
        [SerializeField] private GameObject editModeOverlay; // isteğe bağlı — "HUD EDIT" label
        [SerializeField] private Canvas rootCanvas;

        // ── State ──────────────────────────────────────────────────
        public bool IsEditing { get; private set; }
        private readonly List<HUDElement> elements = new();
        private GameObject gridOverlay;

        private const string SaveFileName = "hud_layout.json";
        private static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        // ── Lifecycle ──────────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            if (rootCanvas == null)
                rootCanvas = GetComponentInParent<Canvas>();

            LoadLayout();
        }

        private void Update()
        {
            // ESC + H toggle
            if (Input.GetKey(KeyCode.Escape) && Input.GetKeyDown(KeyCode.H))
                SetEditing(!IsEditing);
        }

        // ── Public API ─────────────────────────────────────────────

        public void Register(HUDElement el)
        {
            if (!elements.Contains(el))
                elements.Add(el);
        }

        public void Unregister(HUDElement el) => elements.Remove(el);

        public void SetEditing(bool on)
        {
            IsEditing = on;

            if (on)
            {
                BuildGridOverlay();
                foreach (var el in elements) el.EnterEditMode();
            }
            else
            {
                DestroyGridOverlay();
                foreach (var el in elements) el.ExitEditMode();
                SaveLayout();
            }

            if (editModeOverlay != null)
                editModeOverlay.SetActive(on);
        }

        /// <summary>Çakışma kontrolü — başka bir element'le overlap var mı?</summary>
        public bool Overlaps(HUDElement mover, Rect proposedScreen)
        {
            foreach (var el in elements)
            {
                if (el == mover) continue;
                if (proposedScreen.Overlaps(el.ScreenRect))
                    return true;
            }
            return false;
        }

        /// <summary>Grid'e snap — ekran koordinatında çalışır.</summary>
        public Vector2 SnapToGrid(Vector2 screenPos)
        {
            return new Vector2(
                Mathf.Round(screenPos.x / gridSize) * gridSize,
                Mathf.Round(screenPos.y / gridSize) * gridSize
            );
        }

        public void ResetAll()
        {
            foreach (var el in elements) el.ResetToDefault();
            if (File.Exists(SavePath)) File.Delete(SavePath);
        }

        // ── Save / Load ────────────────────────────────────────────

        public void SaveLayout()
        {
            var data = new HUDLayoutData();
            foreach (var el in elements)
                data.elements.Add(el.Capture());

            File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
        }

        public void LoadLayout()
        {
            if (!File.Exists(SavePath)) return;

            HUDLayoutData data;
            try { data = JsonUtility.FromJson<HUDLayoutData>(File.ReadAllText(SavePath)); }
            catch { return; }

            foreach (var entry in data.elements)
            {
                var el = elements.Find(e => e.ElementId == entry.id);
                el?.Apply(entry);
            }
        }

        // ── Grid overlay ───────────────────────────────────────────

        private void BuildGridOverlay()
        {
            if (gridOverlay != null) return;

            gridOverlay = new GameObject("HUD_GridOverlay", typeof(RectTransform), typeof(Canvas), typeof(CanvasRenderer));
            gridOverlay.transform.SetParent(rootCanvas.transform, false);
            var rt = gridOverlay.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            gridOverlay.AddComponent<GridOverlayRenderer>().Init(gridSize, gridColor);
            gridOverlay.transform.SetAsFirstSibling(); // elemanların altında kalsın
        }

        private void DestroyGridOverlay()
        {
            if (gridOverlay != null) Destroy(gridOverlay);
            gridOverlay = null;
        }
    }

    // ── Serializable veri sınıfları ─────────────────────────────────

    [Serializable]
    public class HUDLayoutData
    {
        public List<HUDElementEntry> elements = new();
    }

    [Serializable]
    public class HUDElementEntry
    {
        public string id;
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
        public float scale = 1f;
    }

    // ── Grid çizici ─────────────────────────────────────────────────

    [RequireComponent(typeof(CanvasRenderer))]
    public class GridOverlayRenderer : Graphic
    {
        private int _grid;
        private Color _col;

        public void Init(int grid, Color col)
        {
            _grid = grid;
            _col = col;
            color = col;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (_grid <= 0) return;

            var r = rectTransform.rect;
            float lineW = 1f;
            int vi = 0;

            // Dikey çizgiler
            for (float x = r.xMin; x <= r.xMax; x += _grid)
            {
                AddLine(vh, ref vi, new Vector2(x, r.yMin), new Vector2(x, r.yMax), lineW);
            }
            // Yatay çizgiler
            for (float y = r.yMin; y <= r.yMax; y += _grid)
            {
                AddLine(vh, ref vi, new Vector2(r.xMin, y), new Vector2(r.xMax, y), lineW);
            }
        }

        private void AddLine(VertexHelper vh, ref int vi, Vector2 a, Vector2 b, float w)
        {
            Vector2 dir = (b - a).normalized;
            Vector2 perp = new Vector2(-dir.y, dir.x) * (w * 0.5f);

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            vert.position = a - perp; vh.AddVert(vert);
            vert.position = a + perp; vh.AddVert(vert);
            vert.position = b + perp; vh.AddVert(vert);
            vert.position = b - perp; vh.AddVert(vert);

            vh.AddTriangle(vi, vi + 1, vi + 2);
            vh.AddTriangle(vi, vi + 2, vi + 3);
            vi += 4;
        }
    }
}
