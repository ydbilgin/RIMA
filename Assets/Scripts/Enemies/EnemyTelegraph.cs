using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Runtime 2D attack warning helper for enemy skills.
    /// Uses LineRenderer only, so enemies can show readable warnings without prefab setup.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class EnemyTelegraph : MonoBehaviour
    {
        public enum Shape
        {
            Circle,
            Line,
            Cone
        }

        [Header("Visual")]
        [SerializeField] private Color color = new Color(1f, 0.18f, 0.08f, 0.85f);
        [SerializeField] private float strokeWidth = 0.055f;
        [SerializeField] private Material lineMaterial;

        [Header("Lifetime")]
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private bool destroyOnComplete = true;

        private LineRenderer line;
        private float elapsed;
        private bool running;
        private static Material sharedDefaultMaterial;

        public bool IsRunning => running;
        public float Duration => duration;

        private void Awake()
        {
            EnsureRenderer();
        }

        private void Update()
        {
            if (!running) return;

            elapsed += Time.deltaTime;
            float t = duration <= 0f ? 1f : Mathf.Clamp01(elapsed / duration);
            Color faded = color;
            faded.a = Mathf.Lerp(color.a, 0f, t);
            line.startColor = faded;
            line.endColor = faded;

            if (elapsed < duration) return;

            running = false;
            if (destroyOnComplete)
                Destroy(gameObject);
            else
                line.enabled = false;
        }

        public void ShowCircle(Vector2 center, float radius, float showDuration)
        {
            duration = Mathf.Max(0.01f, showDuration);
            transform.position = center;
            Draw(BuildCirclePoints(Vector3.zero, radius, 40), true);
        }

        public void ShowLine(Vector2 start, Vector2 direction, float length, float width, float showDuration)
        {
            duration = Mathf.Max(0.01f, showDuration);
            transform.position = start;
            Draw(BuildLinePoints(Vector3.zero, direction, length, width), false);
        }

        public void ShowCone(Vector2 origin, Vector2 direction, float radius, float angle, float showDuration)
        {
            duration = Mathf.Max(0.01f, showDuration);
            transform.position = origin;
            Draw(BuildConePoints(Vector3.zero, direction, radius, angle, 24), false);
        }

        public static EnemyTelegraph SpawnCircle(Vector2 center, float radius, float duration)
        {
            EnemyTelegraph telegraph = Create("EnemyTelegraph_Circle");
            telegraph.ShowCircle(center, radius, duration);
            return telegraph;
        }

        public static EnemyTelegraph SpawnLine(Vector2 start, Vector2 direction, float length, float width, float duration)
        {
            EnemyTelegraph telegraph = Create("EnemyTelegraph_Line");
            telegraph.ShowLine(start, direction, length, width, duration);
            return telegraph;
        }

        public static EnemyTelegraph SpawnCone(Vector2 origin, Vector2 direction, float radius, float angle, float duration)
        {
            EnemyTelegraph telegraph = Create("EnemyTelegraph_Cone");
            telegraph.ShowCone(origin, direction, radius, angle, duration);
            return telegraph;
        }

        public static Vector3[] BuildCirclePoints(Vector3 center, float radius, int segments)
        {
            int safeSegments = Mathf.Max(8, segments);
            var points = new Vector3[safeSegments + 1];
            float safeRadius = Mathf.Max(0.01f, radius);

            for (int i = 0; i <= safeSegments; i++)
            {
                float angle = i * Mathf.PI * 2f / safeSegments;
                points[i] = center + new Vector3(Mathf.Cos(angle) * safeRadius, Mathf.Sin(angle) * safeRadius, 0f);
            }

            return points;
        }

        public static Vector3[] BuildLinePoints(Vector3 start, Vector2 direction, float length, float width)
        {
            Vector2 dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            Vector2 normal = new Vector2(-dir.y, dir.x) * Mathf.Max(0.01f, width) * 0.5f;
            Vector2 end = (Vector2)start + dir * Mathf.Max(0.01f, length);

            return new[]
            {
                (Vector3)((Vector2)start + normal),
                (Vector3)(end + normal),
                (Vector3)(end - normal),
                (Vector3)((Vector2)start - normal),
                (Vector3)((Vector2)start + normal)
            };
        }

        public static Vector3[] BuildConePoints(Vector3 origin, Vector2 direction, float radius, float angle, int segments)
        {
            int safeSegments = Mathf.Max(4, segments);
            var points = new Vector3[safeSegments + 3];
            Vector2 dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            float centerAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float halfAngle = Mathf.Clamp(angle, 1f, 360f) * 0.5f;
            float safeRadius = Mathf.Max(0.01f, radius);

            points[0] = origin;
            for (int i = 0; i <= safeSegments; i++)
            {
                float t = (float)i / safeSegments;
                float a = Mathf.Lerp(centerAngle - halfAngle, centerAngle + halfAngle, t) * Mathf.Deg2Rad;
                points[i + 1] = origin + new Vector3(Mathf.Cos(a) * safeRadius, Mathf.Sin(a) * safeRadius, 0f);
            }
            points[points.Length - 1] = origin;

            return points;
        }

        private static EnemyTelegraph Create(string objectName)
        {
            var go = new GameObject(objectName);
            return go.AddComponent<EnemyTelegraph>();
        }

        private void Draw(Vector3[] points, bool loop)
        {
            EnsureRenderer();
            elapsed = 0f;
            running = true;

            line.loop = loop;
            line.positionCount = points.Length;
            line.SetPositions(points);
            line.enabled = true;
        }

        private void EnsureRenderer()
        {
            if (line != null) return;

            line = GetComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.widthMultiplier = strokeWidth;
            line.numCapVertices = 2;
            line.numCornerVertices = 2;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.receiveShadows = false;
            line.sortingLayerName = "VFX";
            line.sortingOrder = 20;
            line.startColor = color;
            line.endColor = color;

            if (lineMaterial != null)
            {
                line.material = lineMaterial;
                return;
            }

            if (sharedDefaultMaterial == null)
            {
                Shader shader = Shader.Find("Sprites/Default");
                if (shader != null)
                    sharedDefaultMaterial = new Material(shader);
            }

            if (sharedDefaultMaterial != null)
                line.material = sharedDefaultMaterial;
        }
    }
}
