using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Simple dot-based mini-map.
    /// Shows room border + player (white) + enemies (red) as colored dots.
    /// Attach to the MiniMap root UI object; assign mapBackground RectTransform.
    ///
    /// TODO (tasarım güncellenince):
    ///   - Düz renkli dotlar → ikon/sprite (player silüeti, skull)
    ///   - Tek oda → multi-room harita (geçilen odaları göster)
    ///   - Kapı ikonları N/S/E/W kenarlarda
    ///   - Minimap toggle (M tuşu ile aç/kapat)
    /// </summary>
    public class MiniMap : MonoBehaviour
    {
        [Header("Room Bounds (override if RuntimeRoomManager absent)")]
        [SerializeField] private float roomWidth  = 20f;
        [SerializeField] private float roomHeight = 15f;

        [Header("Dot Sizes")]
        [SerializeField] private float playerDotSize = 8f;
        [SerializeField] private float enemyDotSize  = 5f;
        [SerializeField] private float fragmentDotSize = 6f;

        [Header("Colors")]
        [SerializeField] private Color playerColor = Color.white;
        [SerializeField] private Color enemyColor  = new Color(0.9f, 0.25f, 0.2f, 1f);
        [SerializeField] private Color fragmentColor = new Color(0.35f, 1f, 0.55f, 1f);

        [Header("Update Rate")]
        [SerializeField] private float updateInterval = 0.1f;

        // Runtime refs
        private RectTransform mapRect;
        private Transform playerTransform;
        private RectTransform playerDot;

        private readonly List<(Transform enemy, RectTransform dot)> enemyDots
            = new List<(Transform, RectTransform)>();
        private readonly List<(MapFragment fragment, RectTransform dot)> fragmentDots
            = new List<(MapFragment, RectTransform)>();

        private float nextUpdate;

        private void Awake()
        {
            mapRect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;

            if (RuntimeRoomManager.Instance != null)
            {
                roomWidth  = RuntimeRoomManager.Instance.RoomWidth;
                roomHeight = RuntimeRoomManager.Instance.RoomHeight;
            }

            playerDot = CreateDot("PlayerDot", playerColor, playerDotSize);
        }

        private void Update()
        {
            if (Time.time < nextUpdate) return;
            nextUpdate = Time.time + updateInterval;

            UpdatePlayerDot();
            SyncEnemyDots();
            SyncFragmentDots();
        }

        // ─── Dot updates ────────────────────────────────────────

        private void UpdatePlayerDot()
        {
            if (playerTransform == null || playerDot == null) return;
            playerDot.anchoredPosition = WorldToMap(playerTransform.position);
        }

        private void SyncEnemyDots()
        {
            // Remove dots for destroyed enemies
            for (int i = enemyDots.Count - 1; i >= 0; i--)
            {
                if (enemyDots[i].enemy == null)
                {
                    if (enemyDots[i].dot != null) Destroy(enemyDots[i].dot.gameObject);
                    enemyDots.RemoveAt(i);
                }
            }

            // Gather currently tracked transforms for fast lookup
            var tracked = new HashSet<Transform>();
            foreach (var pair in enemyDots) tracked.Add(pair.enemy);

            // Add dots for newly spawned enemies
            foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (!tracked.Contains(go.transform))
                    enemyDots.Add((go.transform, CreateDot("EnemyDot", enemyColor, enemyDotSize)));
            }

            // Update positions
            foreach (var (t, dot) in enemyDots)
            {
                if (t != null && dot != null)
                    dot.anchoredPosition = WorldToMap(t.position);
            }
        }

        private void SyncFragmentDots()
        {
            for (int i = fragmentDots.Count - 1; i >= 0; i--)
            {
                if (fragmentDots[i].fragment == null)
                {
                    if (fragmentDots[i].dot != null) Destroy(fragmentDots[i].dot.gameObject);
                    fragmentDots.RemoveAt(i);
                }
            }

            var tracked = new HashSet<MapFragment>();
            foreach (var pair in fragmentDots) tracked.Add(pair.fragment);

            foreach (var fragment in Object.FindObjectsByType<MapFragment>(FindObjectsSortMode.None))
            {
                if (!tracked.Contains(fragment))
                    fragmentDots.Add((fragment, CreateDot("MapFragmentDot", fragmentColor, fragmentDotSize)));
            }

            foreach (var (fragment, dot) in fragmentDots)
            {
                if (fragment != null && dot != null)
                    dot.anchoredPosition = WorldToMap(fragment.transform.position);
            }
        }

        // ─── Coordinate helpers ──────────────────────────────────

        private Vector2 WorldToMap(Vector3 world)
        {
            float w = mapRect.rect.width;
            float h = mapRect.rect.height;

            // world origin (0,0) → map bottom-left corner;  pivot is center (0.5,0.5)
            float x = (world.x / roomWidth)  * w - w * 0.5f;
            float y = (world.y / roomHeight) * h - h * 0.5f;
            return new Vector2(x, y);
        }

        // ─── Dot factory ────────────────────────────────────────

        private RectTransform CreateDot(string dotName, Color color, float size)
        {
            var go  = new GameObject(dotName, typeof(RectTransform));
            go.transform.SetParent(mapRect, false);
            var rt  = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(size, size);
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot     = new Vector2(0.5f, 0.5f);
            var img = go.AddComponent<Image>();
            img.color = color;
            return rt;
        }
    }
}
