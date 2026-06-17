using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Runtime 2D attack warning helper for enemy skills.
    /// T6.1: decal-sprite ground overlays replace raw LineRenderer primitives.
    ///   · Circle → telegraph_circle_ring.png scaled to match radius, Ground sorting, alpha 0→0.6→0 pulse
    ///   · Line   → telegraph_line_beam.png oriented toward target, scaled to match length/width
    ///   · Cone   → telegraph_cone_fan.png oriented toward target, scaled to match radius
    /// LineRenderer is kept as a thin, low-alpha fallback if the decal sprites are missing.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class EnemyTelegraph : MonoBehaviour
    {
        public enum Shape { Circle, Line, Cone }

        // ── Decal sprite resource paths (under Assets/Resources/) ──────────────
        private const string CircleDecalPath = "Art/Telegraphs/telegraph_circle_ring";
        private const string LineDecalPath   = "Art/Telegraphs/telegraph_line_beam";
        private const string ConeDecalPath   = "Art/Telegraphs/telegraph_cone_fan";

        [Header("Visual")]
        [SerializeField] private Color color      = new Color(1f, 0.22f, 0.06f, 0.70f);
        [SerializeField] private float strokeWidth = 0.025f;   // thin fallback line
        [SerializeField] private Material lineMaterial;

        [Header("Decal alpha")]
        [SerializeField] private float decalPeakAlpha = 0.60f;

        [Header("Lifetime")]
        [SerializeField] private float duration        = 0.35f;
        [SerializeField] private bool  destroyOnComplete = true;

        // Delayed-explosion mode: the marker holds at a steady high alpha for the
        // first `holdFraction` of its life (reads as "this WILL explode here"),
        // then flash-fades in the final stretch. Used by SpawnDelayedRing.
        private bool  holdMode;
        private const float HoldFraction = 0.80f;

        private LineRenderer  line;
        private SpriteRenderer decalSR;
        private Vector3 decalInitialScale;   // MINOR-4: cache non-uniform scale set by SpawnDecal
        private float elapsed;
        private bool  running;
        private static Material sharedDefaultMaterial;

        public bool IsRunning => running;
        public float Duration  => duration;

        private void Awake() { EnsureRenderer(); }

        private void Update()
        {
            if (!running) return;

            elapsed += Time.deltaTime;
            float t   = duration <= 0f ? 1f : Mathf.Clamp01(elapsed / duration);

            float alphaT;
            if (holdMode)
            {
                // Delayed-explosion: steady high alpha (hold), then fast flash-fade.
                if (t < HoldFraction)
                {
                    alphaT = 1f;
                }
                else
                {
                    // Final stretch flickers (sin pulse) so the imminent blast reads.
                    float ft = (t - HoldFraction) / (1f - HoldFraction);
                    alphaT = Mathf.Abs(Mathf.Cos(ft * Mathf.PI * 3f)) * (1f - ft);
                }
            }
            else
            {
                // Pulse: grow 0→peak over first 70%, then fade to 0.
                alphaT = t < 0.7f ? t / 0.7f : 1f - (t - 0.7f) / 0.3f;
            }
            float alpha  = decalPeakAlpha * alphaT;

            // Update decal sprite alpha
            if (decalSR != null)
            {
                Color dc = decalSR.color; dc.a = alpha; decalSR.color = dc;
                // Grow slightly as the telegraph charges (0.9→1.0 scale over duration).
                // MINOR-4 fix: multiply initial non-uniform scale (length×width) rather than
                // overwriting with a uniform Vector3.one * scale, which flattened line/cone decals.
                float scale = Mathf.Lerp(0.88f, 1.0f, t);
                decalSR.transform.localScale = decalInitialScale * scale;
            }

            // Keep fallback line very subtle
            Color lc = color; lc.a = Mathf.Lerp(0.15f, 0f, t);
            line.startColor = lc; line.endColor = lc;

            if (elapsed < duration) return;

            running = false;
            if (destroyOnComplete) Destroy(gameObject);
            else                   line.enabled = false;
        }

        // ── Public Show API ──────────────────────────────────────────────────

        public void ShowCircle(Vector2 center, float radius, float showDuration)
        {
            duration = Mathf.Max(0.01f, showDuration);
            transform.position = center;

            // Decal: sprite is designed as 1-unit circle ring → scale = diameter
            float worldDiameter = radius * 2f;
            SpawnDecal(CircleDecalPath, center, 0f, worldDiameter, worldDiameter);

            // Fallback thin line ring
            Draw(BuildCirclePoints(Vector3.zero, radius, 40), true);
        }

        public void ShowLine(Vector2 start, Vector2 direction, float length, float width, float showDuration)
        {
            duration = Mathf.Max(0.01f, showDuration);
            transform.position = start;

            Vector2 dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            // Offset decal center to the midpoint of the beam
            Vector2 midpoint = start + dir * (length * 0.5f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // Sprite is designed as a horizontal beam → scale X=length, Y=width
            SpawnDecal(LineDecalPath, midpoint, angle, length, Mathf.Max(0.25f, width * 1.4f));

            // Fallback thin rect outline
            Draw(BuildLinePoints(Vector3.zero, direction, length, width * 0.3f), false);
        }

        public void ShowCone(Vector2 origin, Vector2 direction, float radius, float angle, float showDuration)
        {
            duration = Mathf.Max(0.01f, showDuration);
            transform.position = origin;

            Vector2 dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            SpawnDecal(ConeDecalPath, origin, rotAngle, radius, radius);

            Draw(BuildConePoints(Vector3.zero, direction, radius, angle, 24), false);
        }

        /// <summary>
        /// Fixed-position delayed-explosion ring (e.g. ChainExplosion ground markers).
        /// The ring holds at full alpha while the blast charges, then flash-fades.
        /// `delay` MUST equal the caller's real explosion windup so the warning is honest.
        /// </summary>
        public void ShowDelayedRing(Vector2 center, float radius, float delay)
        {
            holdMode = true;
            duration = Mathf.Max(0.01f, delay);
            transform.position = center;

            float worldDiameter = radius * 2f;
            SpawnDecal(CircleDecalPath, center, 0f, worldDiameter, worldDiameter);

            Draw(BuildCirclePoints(Vector3.zero, radius, 40), true);
        }

        // ── Static factory ───────────────────────────────────────────────────

        public static EnemyTelegraph SpawnCircle(Vector2 center, float radius, float duration)
        {
            EnemyTelegraph t = Create("EnemyTelegraph_Circle");
            t.ShowCircle(center, radius, duration);
            return t;
        }

        public static EnemyTelegraph SpawnLine(Vector2 start, Vector2 direction, float length, float width, float duration)
        {
            EnemyTelegraph t = Create("EnemyTelegraph_Line");
            t.ShowLine(start, direction, length, width, duration);
            return t;
        }

        public static EnemyTelegraph SpawnCone(Vector2 origin, Vector2 direction, float radius, float angle, float duration)
        {
            EnemyTelegraph t = Create("EnemyTelegraph_Cone");
            t.ShowCone(origin, direction, radius, angle, duration);
            return t;
        }

        /// <summary>Delayed-explosion ring; `delay` = the real explosion windup (e.g. ChainExplosion 3s).</summary>
        public static EnemyTelegraph SpawnDelayedRing(Vector2 center, float radius, float delay)
        {
            EnemyTelegraph t = Create("EnemyTelegraph_DelayedRing");
            t.ShowDelayedRing(center, radius, delay);
            return t;
        }

        /// <summary>
        /// Snap-to-damage feedback: short bright burst fired the instant a telegraph resolves.
        /// Wraps the existing SkillVfx impact motor (self-destructing, additive); no new VFX engine.
        /// </summary>
        public static void FlashImpact(Vector2 pos, VfxElement element = VfxElement.Physical)
        {
            SkillVfx.ImpactBurst(pos, element);
        }

        // ── Geometry helpers ─────────────────────────────────────────────────

        public static Vector3[] BuildCirclePoints(Vector3 center, float radius, int segments)
        {
            int safeSegs = Mathf.Max(8, segments);
            var points   = new Vector3[safeSegs + 1];
            float r      = Mathf.Max(0.01f, radius);
            for (int i = 0; i <= safeSegs; i++)
            {
                float a = i * Mathf.PI * 2f / safeSegs;
                points[i] = center + new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0f);
            }
            return points;
        }

        public static Vector3[] BuildLinePoints(Vector3 start, Vector2 direction, float length, float width)
        {
            Vector2 dir    = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            Vector2 normal = new Vector2(-dir.y, dir.x) * Mathf.Max(0.01f, width) * 0.5f;
            Vector2 end    = (Vector2)start + dir * Mathf.Max(0.01f, length);
            return new[]
            {
                (Vector3)((Vector2)start + normal), (Vector3)(end + normal),
                (Vector3)(end - normal), (Vector3)((Vector2)start - normal),
                (Vector3)((Vector2)start + normal)
            };
        }

        public static Vector3[] BuildConePoints(Vector3 origin, Vector2 direction, float radius, float angle, int segments)
        {
            int safeSegs  = Mathf.Max(4, segments);
            var points    = new Vector3[safeSegs + 3];
            Vector2 dir   = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            float cAngle  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float halfAng = Mathf.Clamp(angle, 1f, 360f) * 0.5f;
            float r       = Mathf.Max(0.01f, radius);
            points[0] = origin;
            for (int i = 0; i <= safeSegs; i++)
            {
                float t = (float)i / safeSegs;
                float a = Mathf.Lerp(cAngle - halfAng, cAngle + halfAng, t) * Mathf.Deg2Rad;
                points[i + 1] = origin + new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0f);
            }
            points[points.Length - 1] = origin;
            return points;
        }

        // ── Internal helpers ─────────────────────────────────────────────────

        private static EnemyTelegraph Create(string objectName)
        {
            var go = new GameObject(objectName);
            return go.AddComponent<EnemyTelegraph>();
        }

        /// <summary>Spawn a ground-layer SpriteRenderer decal as a child of this telegraph.</summary>
        private void SpawnDecal(string resourcePath, Vector2 worldPos, float zRotation, float scaleX, float scaleY)
        {
            Sprite spr = Resources.Load<Sprite>(resourcePath);
            if (spr == null) return;  // no decal sprite → fallback line handles it

            var go = new GameObject("TelegraphDecal");
            go.transform.SetParent(transform, false);
            go.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
            go.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            go.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            decalInitialScale = go.transform.localScale;   // MINOR-4: cache for per-frame multiply

            decalSR = go.AddComponent<SpriteRenderer>();
            decalSR.sprite           = spr;
            decalSR.sortingLayerName = "Decals";   // above Floor tiles, below Walls/Entities
            decalSR.sortingOrder     = 5;
            decalSR.color            = new Color(color.r, color.g, color.b, 0f); // start transparent, Update() drives alpha
        }

        private void Draw(Vector3[] points, bool loop)
        {
            EnsureRenderer();
            elapsed = 0f;
            running = true;
            line.loop           = loop;
            line.positionCount  = points.Length;
            line.SetPositions(points);
            line.enabled = true;
        }

        private void EnsureRenderer()
        {
            if (line != null) return;

            line = GetComponent<LineRenderer>();
            line.useWorldSpace    = false;
            line.widthMultiplier  = strokeWidth;
            line.numCapVertices   = 2;
            line.numCornerVertices = 2;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.receiveShadows   = false;
            line.sortingLayerName = "VFX";
            line.sortingOrder     = 20;
            line.startColor       = color;
            line.endColor         = color;

            if (lineMaterial != null) { line.material = lineMaterial; return; }

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
