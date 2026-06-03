using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Feature B (B4) — the Sundered-Echo materialize / dissolve puff.
    ///
    /// A cheap, pooled-free procedural burst that reads as "an echo forming out of black smoke with a
    /// cold cyan edge". Replaces the two flat <c>SpawnCircleVisual</c> circles the echo used before.
    ///
    /// Follows the project's 4 pixelated-particle rules so it sits on PPU64 pixel art:
    ///   1. Point-filtered, low-res (16×16) soft-dot texture, no mips/compression.
    ///   2. World pixel-snap (round to 1/64u) every step so motes land on the pixel grid.
    ///   3. Palette-lock: only black smoke + cyan #00FFCC (no smooth gradients); alpha is the only fade.
    ///   4. 12 FPS stepped sim (not per-frame) so motion matches 12 FPS hand-drawn art.
    ///
    /// One GameObject hosts a handful of child SpriteRenderers (motes); the whole burst self-destroys
    /// after its life. No ParticleSystem prefab dependency, no new asmdef. Sorted on "VFX" like the
    /// other runtime visuals (NOT an Entities Y-sort participant — it is short-lived screen garnish).
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EchoPuffBurst : MonoBehaviour
    {
        private const float SimFps = 12f;                 // rule 4: stepped, not 60fps
        private const float PixelsPerUnit = 64f;          // rule 2: PPU64 snap
        private static readonly Color SmokeBlack = new Color(0.04f, 0.04f, 0.06f, 1f);
        private static readonly Color EdgeCyan   = new Color(0f, 1f, 0.8f, 1f); // #00FFCC

        private static Sprite _dot;

        private struct Mote
        {
            public Transform tf;
            public SpriteRenderer sr;
            public Vector2 vel;       // units/sec (outward)
            public float baseScale;
            public float peakAlpha;
            public bool cyan;
        }

        private Mote[] motes;
        private float life;
        private bool dissolve;        // true = puff-OUT (fade in toward center then out); false = puff-IN

        /// <summary>
        /// Spawn a one-shot Echo puff. <paramref name="dissolve"/> = false for the materialize burst
        /// (motes converge / pop in), true for the dissolve burst (motes scatter out). Both ~<paramref name="life"/>s.
        /// </summary>
        public static EchoPuffBurst Spawn(Vector3 position, float life, bool dissolve, int moteCount = 9)
        {
            var go = new GameObject(dissolve ? "EchoPuffOut" : "EchoPuffIn");
            go.transform.position = position;
            var burst = go.AddComponent<EchoPuffBurst>();
            burst.life = Mathf.Max(0.1f, life);
            burst.dissolve = dissolve;
            burst.Build(Mathf.Clamp(moteCount, 3, 24));
            burst.StartCoroutine(burst.Run());
            return burst;
        }

        private void Build(int count)
        {
            var dot = Dot();
            motes = new Mote[count];
            for (int i = 0; i < count; i++)
            {
                var child = new GameObject("mote");
                child.transform.SetParent(transform, false);

                var sr = child.AddComponent<SpriteRenderer>();
                sr.sprite = dot;
                bool cyan = (i % 3 == 0);                 // ~1/3 cyan edge motes, rest black smoke
                sr.color = cyan ? EdgeCyan : SmokeBlack;
                sr.sortingLayerName = "VFX";
                sr.sortingOrder = cyan ? 22 : 21;         // cyan reads slightly above the smoke

                float ang = (i / (float)count) * Mathf.PI * 2f + Random.Range(-0.3f, 0.3f);
                Vector2 dir = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
                float speed = Random.Range(1.4f, 2.6f) * (cyan ? 1.15f : 1f);

                // Puff-OUT scatters outward; puff-IN starts spread and is pulled inward (negative vel).
                float startRadius = dissolve ? 0.05f : Random.Range(0.35f, 0.7f);
                child.transform.localPosition = dir * startRadius;

                motes[i] = new Mote
                {
                    tf = child.transform,
                    sr = sr,
                    vel = dir * speed * (dissolve ? 1f : -1f),
                    baseScale = Random.Range(0.18f, 0.34f) * (cyan ? 0.8f : 1f),
                    peakAlpha = cyan ? Random.Range(0.55f, 0.8f) : Random.Range(0.45f, 0.7f),
                    cyan = cyan
                };
            }
        }

        private IEnumerator Run()
        {
            float t = 0f;
            float step = 1f / SimFps;
            float accum = 0f;
            // Apply the initial frame immediately so there is no blank first step.
            StepSim(0f, 0f);

            while (t < life)
            {
                t += Time.deltaTime;
                accum += Time.deltaTime;
                // Rule 4: advance the sim only on 12fps ticks (stepped motion).
                while (accum >= step)
                {
                    accum -= step;
                    StepSim(step, t);
                }
                yield return null;
            }
            Destroy(gameObject);
        }

        private void StepSim(float dt, float elapsed)
        {
            float k = Mathf.Clamp01((life > 0f) ? (elapsed / life) : 1f);
            // Alpha envelope: quick rise then ease-out fade — single curve, no color gradient (rule 3).
            float env = k < 0.25f ? (k / 0.25f) : (1f - (k - 0.25f) / 0.75f);
            env = Mathf.Clamp01(env);

            if (motes == null) return;
            for (int i = 0; i < motes.Length; i++)
            {
                var m = motes[i];
                if (m.tf == null) continue;

                // Integrate + decelerate, then snap to the pixel grid (rule 2).
                Vector2 lp = m.tf.localPosition;
                lp += m.vel * dt;
                m.vel *= 0.86f;                           // drag so the burst settles
                Vector2 world = (Vector2)transform.position + lp;
                world = new Vector2(
                    Mathf.Round(world.x * PixelsPerUnit) / PixelsPerUnit,
                    Mathf.Round(world.y * PixelsPerUnit) / PixelsPerUnit);
                m.tf.position = world;

                float scale = m.baseScale * (0.7f + 0.3f * env);
                m.tf.localScale = new Vector3(scale, scale, 1f);

                var c = m.sr.color;
                c.a = m.peakAlpha * env;
                m.sr.color = c;

                motes[i] = m;
            }
        }

        /// <summary>16×16 point-filtered soft dot — bright core, falloff to clear edge (rule 1).</summary>
        private static Sprite Dot()
        {
            if (_dot != null) return _dot;
            const int size = 16;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            Vector2 c = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
            float maxD = size * 0.5f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float d = Vector2.Distance(new Vector2(x, y), c) / maxD;
                    // Stepped falloff (3 bands) — keeps a pixel-art read instead of a smooth blur.
                    float a = d > 1f ? 0f : d > 0.7f ? 0.25f : d > 0.4f ? 0.6f : 1f;
                    tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
                }
            }
            tex.Apply();
            _dot = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), PixelsPerUnit);
            _dot.name = "EchoPuff_Dot";
            return _dot;
        }
    }
}
