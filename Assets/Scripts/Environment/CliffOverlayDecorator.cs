using System.Collections.Generic;
using RIMA.RoomPainter;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.Environment
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public sealed class CliffOverlayDecorator : MonoBehaviour
    {
        private const string ContainerName = "CliffOverlays";

        [Header("Input")]
        [SerializeField] private CliffMeshGenerator meshGenerator;
        [SerializeField] private Sprite cliffS;
        [SerializeField] private Sprite cliffSE;
        [SerializeField] private Sprite cliffSW;
        [SerializeField] private Sprite cliffE;
        [SerializeField] private Sprite cliffW;
        [SerializeField] private Sprite cliffN;
        [SerializeField] private Sprite cliffNE;
        [SerializeField] private Sprite cliffNW;
        [SerializeField] private Sprite cliffCyanGlow;

        [Header("Sampling")]
        [SerializeField] private float spacing = 0.7f;
        [SerializeField, Range(0f, 1f)] private float clusterChance = 0.42f;
        [SerializeField, Range(0f, 1f)] private float cyanChance = 0.08f;
        [SerializeField] private float scaleMin = 0.9f;
        [SerializeField] private float scaleMax = 1.35f;
        [SerializeField] private float jitter = 0.14f;
        [SerializeField] private int seed = 31417;

        [Header("Sorting")]
        [SerializeField] private string sortingLayerName = RoomDepthStack.LayerEntities;
        [SerializeField] private int baseSortingOrder = -6;

        [Header("Metadata")]
        [SerializeField] private int lastSpawnedCount;

        public int LastSpawnedCount => lastSpawnedCount;
        public float Spacing => spacing;
        public float ClusterChance => clusterChance;
        public float CyanChance => cyanChance;
        public float ScaleMin => scaleMin;
        public float ScaleMax => scaleMax;
        public float Jitter => jitter;
        public int Seed => seed;

        private void Reset()
        {
            meshGenerator = GetComponent<CliffMeshGenerator>();
        }

        [ContextMenu("Regenerate Overlays")]
        public void RegenerateOverlays()
        {
            if (meshGenerator == null)
                meshGenerator = GetComponent<CliffMeshGenerator>();

            ClearOverlays();
            lastSpawnedCount = 0;

            if (meshGenerator == null)
            {
                Debug.LogWarning("[CliffOverlayDecorator] CliffMeshGenerator is not assigned.", this);
                return;
            }

            List<CliffMeshGenerator.CliffBoundarySegment> segments = meshGenerator.BuildBoundarySegmentsForOverlay();
            if (segments.Count == 0)
            {
                Debug.LogWarning("[CliffOverlayDecorator] No boundary segments available.", this);
                return;
            }

            Transform container = EnsureContainer();
            int currentLoop = -1;
            float loopDistance = 0f;
            float nextSampleDistance = 0f;
            int sampleIndex = 0;
            int lastCyanSample = -9999;

            for (int i = 0; i < segments.Count; i++)
            {
                CliffMeshGenerator.CliffBoundarySegment segment = segments[i];
                if (segment.loopIndex != currentLoop)
                {
                    currentLoop = segment.loopIndex;
                    loopDistance = 0f;
                    nextSampleDistance = SampleSpacing(sampleIndex) * 0.5f;
                }

                float length = Vector3.Distance(segment.startWorld, segment.endWorld);
                if (length < 0.001f)
                    continue;

                float segmentStartDistance = loopDistance;
                float segmentEndDistance = loopDistance + length;
                Vector3 tangent = (segment.endWorld - segment.startWorld).normalized;

                while (nextSampleDistance <= segmentEndDistance)
                {
                    float localDistance = nextSampleDistance - segmentStartDistance;
                    float t = Mathf.Clamp01(localDistance / length);
                    TrySpawnSample(container, segment, tangent, t, sampleIndex, ref lastCyanSample);
                    sampleIndex++;
                    nextSampleDistance += SampleSpacing(sampleIndex);
                }

                loopDistance = segmentEndDistance;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
#endif

            Debug.Log("[CliffOverlayDecorator] Spawned " + lastSpawnedCount + " overlay sprites from " +
                      segments.Count + " boundary segments.", this);
        }

        private void TrySpawnSample(Transform container, CliffMeshGenerator.CliffBoundarySegment segment,
            Vector3 tangent, float t, int sampleIndex, ref int lastCyanSample)
        {
            float density = FacingDensity(segment.direction);
            if (Hash01(seed + sampleIndex * 7717 + 13) > density)
                return;

            Sprite sprite = SpriteForDirection(segment.direction);
            if (sprite == null)
                return;

            Vector3 edgePoint = Vector3.Lerp(segment.startWorld, segment.endWorld, t);
            int clusterCount = 1;
            if (Hash01(seed + sampleIndex * 7919 + 29) < clusterChance)
                clusterCount = 2 + (Hash01(seed + sampleIndex * 2971 + 43) > 0.62f ? 1 : 0);

            for (int clusterIndex = 0; clusterIndex < clusterCount; clusterIndex++)
            {
                int h = seed + sampleIndex * 1009 + clusterIndex * 9176;
                float tangentOffset = (HashCentered(h + 1) * jitter) + (clusterIndex - (clusterCount - 1) * 0.5f) * jitter * 0.85f;
                float normalOffset = HashCentered(h + 2) * jitter * 0.35f;
                Vector3 position = edgePoint + tangent * tangentOffset + segment.outwardNormal * normalOffset;
                float scale = Mathf.Lerp(scaleMin, scaleMax, Hash01(h + 3));
                float rotation = HashCentered(h + 4) * 4f;
                SpawnSprite(container, sprite, "CliffOverlay_" + segment.direction + "_" + sampleIndex + "_" + clusterIndex,
                    position, scale, rotation, sampleIndex);
            }

            bool canCyan = cliffCyanGlow != null && sampleIndex - lastCyanSample > 1;
            if (canCyan && Hash01(seed + sampleIndex * 5861 + 71) < cyanChance)
            {
                int h = seed + sampleIndex * 1327 + 89;
                Vector3 position = edgePoint + tangent * (HashCentered(h + 1) * jitter * 0.6f) +
                                   segment.outwardNormal * (HashCentered(h + 2) * jitter * 0.25f);
                float scale = Mathf.Lerp(0.8f, 1.05f, Hash01(h + 3));
                float rotation = HashCentered(h + 4) * 3f;
                SpawnSprite(container, cliffCyanGlow, "CliffOverlay_Cyan_" + sampleIndex, position, scale, rotation, sampleIndex + 1);
                lastCyanSample = sampleIndex;
            }
        }

        private void SpawnSprite(Transform container, Sprite sprite, string objectName, Vector3 position,
            float scale, float rotation, int depthIndex)
        {
            GameObject overlay = new GameObject(objectName);
            overlay.transform.SetParent(container, false);
            overlay.transform.position = position;
            overlay.transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
            overlay.transform.localScale = new Vector3(scale, scale, 1f);

            SpriteRenderer renderer = overlay.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = baseSortingOrder + Mathf.Clamp(depthIndex % 4, 0, 3);
            lastSpawnedCount++;
        }

        private Transform EnsureContainer()
        {
            Transform container = transform.Find(ContainerName);
            if (container != null)
                return container;

            GameObject containerObject = new GameObject(ContainerName);
            containerObject.transform.SetParent(transform, false);
            return containerObject.transform;
        }

        private void ClearOverlays()
        {
            Transform container = transform.Find(ContainerName);
            if (container == null)
                return;

            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Transform child = container.GetChild(i);
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }
        }

        private Sprite SpriteForDirection(CliffMeshGenerator.CliffBoundaryDirection direction)
        {
            switch (direction)
            {
                case CliffMeshGenerator.CliffBoundaryDirection.S: return cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.SE: return cliffSE != null ? cliffSE : cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.SW: return cliffSW != null ? cliffSW : cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.E: return cliffE != null ? cliffE : cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.W: return cliffW != null ? cliffW : cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.N: return cliffN != null ? cliffN : cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.NE: return cliffNE != null ? cliffNE : cliffS;
                case CliffMeshGenerator.CliffBoundaryDirection.NW: return cliffNW != null ? cliffNW : cliffS;
                default: return cliffS;
            }
        }

        private float FacingDensity(CliffMeshGenerator.CliffBoundaryDirection direction)
        {
            switch (direction)
            {
                case CliffMeshGenerator.CliffBoundaryDirection.S: return 1f;
                case CliffMeshGenerator.CliffBoundaryDirection.SE:
                case CliffMeshGenerator.CliffBoundaryDirection.SW:
                    return 0.85f;
                case CliffMeshGenerator.CliffBoundaryDirection.E:
                case CliffMeshGenerator.CliffBoundaryDirection.W:
                    return 0.55f;
                case CliffMeshGenerator.CliffBoundaryDirection.NE:
                case CliffMeshGenerator.CliffBoundaryDirection.NW:
                    return 0.22f;
                case CliffMeshGenerator.CliffBoundaryDirection.N:
                    return 0.12f;
                default:
                    return 0.5f;
            }
        }

        private float SampleSpacing(int index)
        {
            float baseSpacing = Mathf.Max(0.1f, spacing);
            return baseSpacing * Mathf.Lerp(0.78f, 1.22f, Hash01(seed + index * 1931 + 5));
        }

        private static float Hash01(int value)
        {
            unchecked
            {
                uint x = (uint)value;
                x ^= 2747636419u;
                x *= 2654435769u;
                x ^= x >> 16;
                x *= 2654435769u;
                x ^= x >> 16;
                return (x & 0x00FFFFFFu) / 16777215f;
            }
        }

        private static float HashCentered(int value)
        {
            return Hash01(value) * 2f - 1f;
        }
    }
}
