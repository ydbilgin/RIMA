using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using RIMA.Systems.Map;

namespace RIMA.UI.Map
{
    /// <summary>
    /// Wires the (previously orphan) MapPanelUI to room progression.
    /// Builds a linear "fractured map" path graph for the demo room sequence, highlights the
    /// current room, reveals the next room's type (the RIMA map-fragment reveal beat), and
    /// flashes the map on each room transition. Toggle full view with M.
    /// Self-bootstraps its Canvas + MapPanelUI so no manual scene wiring is required
    /// (matches the auto-create pattern used by DraftManager).
    /// </summary>
    public class MapProgressController : MonoBehaviour
    {
        [Header("Refs (auto-created if null)")]
        [SerializeField] private MapPanelUI panel;

        [Header("Demo path — room type per index (3 combat -> reward -> boss)")]
        [SerializeField] private MapNodeType[] roomTypes =
        {
            MapNodeType.Combat, MapNodeType.Combat, MapNodeType.Combat, MapNodeType.Rest, MapNodeType.Boss
        };

        [Header("Behaviour")]
        [SerializeField] private float revealFlashSeconds = 0.8f; // agy: 2.2s lock too long for action tempo
        [SerializeField] private float nodeSpacing = 110f;

        private MapGraphData graph;
        private int currentIndex;
        private Coroutine flashRoutine;

        private void Awake()
        {
            EnsurePanel();
            BuildGraph();
            currentIndex = 0;
            ApplyProgress();
            if (panel != null) panel.Hide();
        }

        private void OnEnable()  { RoomLoader.OnRoomChanged += HandleRoomChanged; }
        private void OnDisable() { RoomLoader.OnRoomChanged -= HandleRoomChanged; }

        private void Update()
        {
            var kb = Keyboard.current;
            if (kb != null && kb.mKey.wasPressedThisFrame) Toggle();
        }

        private void HandleRoomChanged(int index)
        {
            if (graph == null || graph.nodes.Count == 0) return;
            currentIndex = Mathf.Clamp(index, 0, graph.nodes.Count - 1);
            ApplyProgress();
            FlashReveal();
        }

        private void BuildGraph()
        {
            graph = new MapGraphData();
            int n = roomTypes.Length;
            float startY = -((n - 1) * nodeSpacing) * 0.5f;
            for (int i = 0; i < n; i++)
            {
                float x = (i % 2 == 0) ? -70f : 70f; // gentle zig-zag down the tablet
                var node = new MapNodeData(i, roomTypes[i].ToString(), roomTypes[i],
                    new Vector2(x, startY + i * nodeSpacing));
                if (i < n - 1) node.connections.Add(i + 1);
                node.threatTier = roomTypes[i] == MapNodeType.Boss ? 3 : 1;
                graph.nodes.Add(node);
            }
        }

        private void ApplyProgress()
        {
            if (graph == null) return;
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                MapNodeData node = graph.nodes[i];
                node.isVisited     = i < currentIndex;
                node.isCurrentRoom = i == currentIndex;
                // Fragment reveal: only the path up to "one room ahead" is known.
                node.isRevealed    = i <= currentIndex + 1;
            }
            if (panel != null && panel.isActiveAndEnabled) panel.Refresh();
        }

        private void Toggle()
        {
            if (panel == null) return;
            if (panel.gameObject.activeSelf) panel.Hide();
            else { panel.Show(graph); panel.Refresh(); }
        }

        private void FlashReveal()
        {
            if (!isActiveAndEnabled || panel == null) return;
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            panel.Show(graph);
            panel.Refresh();
            yield return new WaitForSeconds(revealFlashSeconds);
            panel.Hide();
            flashRoutine = null;
        }

        private void EnsurePanel()
        {
            if (panel != null) return;
            panel = FindFirstObjectByType<MapPanelUI>(FindObjectsInactive.Include);
            if (panel != null) return;

            var canvasGo = new GameObject("MapCanvas_Auto");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 900;
            canvasGo.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasGo.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            var panelGo = new GameObject("MapPanelUI_Auto");
            panelGo.transform.SetParent(canvasGo.transform, false);
            panel = panelGo.AddComponent<MapPanelUI>();
        }
    }
}
