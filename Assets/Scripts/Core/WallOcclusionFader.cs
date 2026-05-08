using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    [RequireComponent(typeof(Tilemap))]
    public class WallOcclusionFader : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private string targetTag = "Player";
        [SerializeField, Range(0.1f, 10f)] private float fadeRadius = 2.2f;
        [SerializeField, Range(0.05f, 1f)] private float minimumAlpha = 0.38f;
        [SerializeField, Range(1f, 30f)] private float fadeSpeed = 10f;
        [SerializeField, Range(1, 12)] private int cellSearchRadius = 5;
        [SerializeField, Range(0.1f, 2f)] private float verticalDistanceWeight = 0.75f;

        private readonly Dictionary<Vector3Int, float> currentAlpha = new Dictionary<Vector3Int, float>();
        private readonly Dictionary<Vector3Int, float> desiredAlpha = new Dictionary<Vector3Int, float>();
        private readonly List<Vector3Int> keys = new List<Vector3Int>();
        private Tilemap tilemap;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            tilemap.CompressBounds();
        }

        private void LateUpdate()
        {
            ResolveTarget();
            if (target == null || tilemap == null) return;

            desiredAlpha.Clear();
            Vector3Int center = tilemap.WorldToCell(target.position);

            for (int x = center.x - cellSearchRadius; x <= center.x + cellSearchRadius; x++)
            {
                for (int y = center.y - cellSearchRadius; y <= center.y + cellSearchRadius; y++)
                {
                    var cell = new Vector3Int(x, y, 0);
                    if (!tilemap.HasTile(cell)) continue;

                    Vector3 world = tilemap.GetCellCenterWorld(cell);
                    Vector2 delta = world - target.position;
                    float weightedDistance = Mathf.Sqrt(
                        delta.x * delta.x +
                        delta.y * verticalDistanceWeight * delta.y * verticalDistanceWeight);

                    if (weightedDistance > fadeRadius) continue;

                    float t = Mathf.Clamp01(weightedDistance / fadeRadius);
                    desiredAlpha[cell] = Mathf.Lerp(minimumAlpha, 1f, t);
                }
            }

            keys.Clear();
            foreach (var pair in currentAlpha) keys.Add(pair.Key);
            foreach (var pair in desiredAlpha)
            {
                if (!currentAlpha.ContainsKey(pair.Key)) keys.Add(pair.Key);
            }

            float step = fadeSpeed * Time.deltaTime;
            foreach (Vector3Int cell in keys)
            {
                float current = currentAlpha.TryGetValue(cell, out float alpha) ? alpha : 1f;
                float targetAlpha = desiredAlpha.TryGetValue(cell, out float wanted) ? wanted : 1f;
                float next = Mathf.MoveTowards(current, targetAlpha, step);

                SetCellAlpha(cell, next);

                if (Mathf.Approximately(next, 1f) && !desiredAlpha.ContainsKey(cell))
                {
                    currentAlpha.Remove(cell);
                }
                else
                {
                    currentAlpha[cell] = next;
                }
            }
        }

        private void OnDisable()
        {
            if (tilemap == null) return;

            foreach (var pair in currentAlpha)
            {
                SetCellAlpha(pair.Key, 1f);
            }
            currentAlpha.Clear();
        }

        private bool targetSearchDone;

        private void ResolveTarget()
        {
            if (target != null) return;
            if (targetSearchDone) return; // one-shot — don't retry every frame

            var go = GameObject.FindGameObjectWithTag(targetTag);
            if (go != null) target = go.transform;
            targetSearchDone = true;
        }

        private void SetCellAlpha(Vector3Int cell, float alpha)
        {
            tilemap.SetTileFlags(cell, TileFlags.None);
            tilemap.SetColor(cell, new Color(1f, 1f, 1f, alpha));
        }
    }
}
