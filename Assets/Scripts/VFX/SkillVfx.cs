using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA
{
    public enum VfxElement
    {
        Physical,
        Fire,
        Frost,
        Lightning,
        Void,
        Arcane
    }

    public static class SkillVfx
    {
        private const string SortingLayerName = "VFX";
        private const int SortingOrder = 20;
        private const float FallbackLife = 0.35f;

        private static SkillVfxRunner runner;
        private static Material sharedAdditiveMaterial;

        public static void SpawnTinted(GameObject prefab, Vector3 pos, VfxElement element, Vector2 dir = default)
        {
            SpawnTintedInstance(prefab, pos, element, dir, null);
        }

        public static void PlayBurst(GameObject sprite, Vector3 pos, VfxElement element, float scaleFrom = 0.5f, float scaleTo = 1.3f, float life = 0.25f)
        {
            GameObject instance = SpawnTintedInstance(sprite, pos, element, default, null);
            if (instance == null)
                return;

            ApplyAdditiveCore(instance);
            instance.transform.localScale = Vector3.one * scaleFrom;
            Runner.Run(ScaleFadeAndDestroy(instance, scaleFrom, scaleTo, Mathf.Max(0.01f, life), 0f));
        }

        public static void PlaySweep(GameObject arcSprite, Vector3 pos, Vector2 dir, VfxElement element, float life = 0.2f, bool additiveSprite = false)
        {
            GameObject instance = SpawnTintedInstance(arcSprite, pos, element, dir, null);
            if (instance == null)
                return;

            // Additive blend makes the element tint read as glow over the base sprite. The slash
            // base art is intrinsically teal, so a multiply tint to ember reads muddy (teal x orange
            // = greenish-brown); additive pushes it cleanly toward the element color instead.
            if (additiveSprite)
                ApplyAdditiveSprite(instance, Palette(element));

            instance.transform.localScale = Vector3.one * 0.8f;
            Runner.Run(ScaleFadeAndDestroy(instance, 0.8f, 1.15f, Mathf.Max(0.01f, life), 18f));
        }

        public static void CastFlash(Transform caster, VfxElement element)
        {
            if (caster == null)
                return;

            GameObject prefab = LoadPrefab("Prefabs/VFX/HandGlowVFX", "Assets/Resources/Prefabs/VFX/HandGlowVFX.prefab");
            GameObject instance = SpawnTintedInstance(prefab, caster.position, element, default, caster);
            if (instance == null)
                return;

            instance.transform.localPosition = Vector3.zero;
            Runner.Run(DestroyAfter(instance, 0.22f));
        }

        public static void CastFlash(GameObject caster, VfxElement element)
        {
            if (caster != null)
                CastFlash(caster.transform, element);
        }

        public static void ImpactBurst(Vector3 pos, VfxElement element)
        {
            GameObject prefab = LoadPrefab("Prefabs/VFX/HitSpark", "Assets/Resources/Prefabs/VFX/HitSpark.prefab")
                ?? LoadPrefab("Prefabs/VFX/DeathBurst", "Assets/Resources/Prefabs/VFX/DeathBurst.prefab");

            if (prefab != null)
            {
                PlayBurst(prefab, pos, element);
                return;
            }

            // Fallback template is a scene object, not an asset — destroy it after the instance spawns
            // (PlayBurst already Instantiated from it). Without this it leaks one GameObject per call in builds.
            GameObject template = CreateRuntimeSpritePrefab("ImpactBurst", null);
            PlayBurst(template, pos, element);
            Object.Destroy(template);
        }

        public static void ProjectileTrail(GameObject go, VfxElement element)
        {
            if (go == null)
                return;

            TrailRenderer trail = go.GetComponent<TrailRenderer>();
            if (trail == null)
                trail = go.AddComponent<TrailRenderer>();

            Color color = Palette(element);
            trail.time = 0.22f;
            trail.startWidth = 0.18f;
            trail.endWidth = 0f;
            trail.numCapVertices = 2;
            trail.minVertexDistance = 0.03f;
            trail.autodestruct = false;
            trail.sharedMaterial = SharedAdditiveMaterial;
            trail.startColor = color;
            trail.endColor = WithAlpha(color, 0f);
            ForceSorting(trail, SortingOrder + 2);
        }

        public static void MeleeArc(Vector3 pos, Vector2 dir, VfxElement element)
        {
            // crescent is the more neutral/brighter base (lower chroma, larger near-white mass) so the
            // additive element tint reads cleaner than on the more saturated teal slash_arc_main.
            Sprite sprite = Resources.Load<Sprite>("VFX/Skills/slash_arc_crescent")
                ?? Resources.Load<Sprite>("VFX/Skills/slash_arc_main");

            GameObject arc = CreateRuntimeSpritePrefab("MeleeArc", sprite);
            PlaySweep(arc, pos, dir, element, 0.18f, additiveSprite: true);
            Object.Destroy(arc);

            GameObject hitSpark = LoadPrefab("Prefabs/VFX/HitSpark", "Assets/Resources/Prefabs/VFX/HitSpark.prefab");
            if (hitSpark != null)
                SpawnTinted(hitSpark, pos, element, dir);
        }

        public static void GroundCrack(Vector3 pos, VfxElement element)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/Environment/Decals/floor_riftcrack")
                ?? LoadSprite("Assets/Resources/Sprites/Environment/Decals/floor_riftcrack.png");

            GameObject decal = CreateRuntimeSpritePrefab("GroundCrack", sprite);
            GameObject instance = SpawnTintedInstance(decal, pos, element, default, null);
            Object.Destroy(decal);

            if (instance == null)
                return;

            instance.transform.localScale = Vector3.one * 0.65f;
            Runner.Run(ScaleFadeAndDestroy(instance, 0.65f, 1.1f, 0.45f, 0f));
        }

        public static void ChainBolt(Vector3 from, Vector3 to, VfxElement element)
        {
            GameObject bolt = new GameObject("SkillVfx_ChainBolt");
            LineRenderer line = bolt.AddComponent<LineRenderer>();
            Color color = Palette(element);

            line.useWorldSpace = true;
            line.positionCount = 5;
            line.numCapVertices = 2;
            line.numCornerVertices = 2;
            line.widthMultiplier = 0.08f;
            line.sharedMaterial = SharedAdditiveMaterial;
            line.colorGradient = BuildGradient(color, 1f);
            ForceSorting(line, SortingOrder + 4);

            Vector3 delta = to - from;
            Vector3 normal = delta.sqrMagnitude > 0.001f
                ? Vector3.Cross(delta.normalized, Vector3.forward)
                : Vector3.up;

            line.SetPosition(0, from);
            line.SetPosition(1, Vector3.Lerp(from, to, 0.25f) + normal * 0.12f);
            line.SetPosition(2, Vector3.Lerp(from, to, 0.5f) - normal * 0.08f);
            line.SetPosition(3, Vector3.Lerp(from, to, 0.75f) + normal * 0.1f);
            line.SetPosition(4, to);

            Runner.Run(FadeLineAndDestroy(line, 0.16f));
        }

        public static Color Palette(VfxElement element)
        {
            switch (element)
            {
                case VfxElement.Physical:
                    return FromHex(0xE89020);
                case VfxElement.Fire:
                    return FromHex(0xFF6A1F);
                case VfxElement.Frost:
                    return FromHex(0x7FE0FF);
                case VfxElement.Lightning:
                    return FromHex(0xFFE600);
                case VfxElement.Void:
                    return FromHex(0xB36BFF);
                case VfxElement.Arcane:
                    return FromHex(0x00FFCC);
                default:
                    return Color.white;
            }
        }

        private static SkillVfxRunner Runner
        {
            get
            {
                if (runner != null)
                    return runner;

                GameObject host = new GameObject("SkillVfxRunner");
                Object.DontDestroyOnLoad(host);
                runner = host.AddComponent<SkillVfxRunner>();
                return runner;
            }
        }

        private static Material SharedAdditiveMaterial
        {
            get
            {
                if (sharedAdditiveMaterial != null)
                    return sharedAdditiveMaterial;

                Shader shader = Shader.Find("Particles/Additive")
                    ?? Shader.Find("Legacy Shaders/Particles/Additive")
                    ?? Shader.Find("Sprites/Default");

                sharedAdditiveMaterial = new Material(shader)
                {
                    name = "SkillVfx_SharedAdditive",
                    hideFlags = HideFlags.HideAndDontSave
                };
                return sharedAdditiveMaterial;
            }
        }

        private static GameObject SpawnTintedInstance(GameObject prefab, Vector3 pos, VfxElement element, Vector2 dir, Transform parent)
        {
            if (prefab == null)
                return null;

            Quaternion rotation = DirectionRotation(dir);
            GameObject instance = Object.Instantiate(prefab, pos, rotation, parent);
            instance.name = prefab.name + "_SkillVfx";
            instance.SetActive(true);

            Tint(instance, Palette(element));
            ConfigureParticlesForDestroy(instance);
            Runner.Run(DestroyFallbackIfNeeded(instance));
            return instance;
        }

        private static void Tint(GameObject root, Color color)
        {
            SpriteRenderer[] spriteRenderers = root.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Color current = spriteRenderers[i].color;
                spriteRenderers[i].color = WithAlpha(color, current.a);
                ForceSorting(spriteRenderers[i], SortingOrder);
            }

            ParticleSystem[] particleSystems = root.GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < particleSystems.Length; i++)
            {
                ParticleSystem.MainModule main = particleSystems[i].main;
                main.startColor = color;

                ParticleSystem.ColorOverLifetimeModule overLifetime = particleSystems[i].colorOverLifetime;
                if (overLifetime.enabled)
                    overLifetime.color = BuildGradient(color, color.a);
            }

            ParticleSystemRenderer[] particleRenderers = root.GetComponentsInChildren<ParticleSystemRenderer>(true);
            for (int i = 0; i < particleRenderers.Length; i++)
            {
                // Keep the prefab's own material — forcing additive on every spawn blows out large soft
                // particles (whiteout). Spec: additive is core-only (PlayBurst opts in via ApplyAdditiveCore).
                ForceSorting(particleRenderers[i], SortingOrder + 10);
            }

            TrailRenderer[] trails = root.GetComponentsInChildren<TrailRenderer>(true);
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].startColor = color;
                trails[i].endColor = WithAlpha(color, 0f);
                trails[i].sharedMaterial = SharedAdditiveMaterial;
                ForceSorting(trails[i], SortingOrder + 2);
            }

            LineRenderer[] lines = root.GetComponentsInChildren<LineRenderer>(true);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].colorGradient = BuildGradient(color, color.a);
                lines[i].sharedMaterial = SharedAdditiveMaterial;
                ForceSorting(lines[i], SortingOrder + 4);
            }
        }

        private static void ConfigureParticlesForDestroy(GameObject root)
        {
            ParticleSystem[] particleSystems = root.GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < particleSystems.Length; i++)
            {
                ParticleSystem.MainModule main = particleSystems[i].main;
                main.stopAction = ParticleSystemStopAction.Destroy;

                if (!particleSystems[i].isPlaying)
                    particleSystems[i].Play(true);
            }
        }

        private static IEnumerator DestroyFallbackIfNeeded(GameObject instance)
        {
            if (instance == null)
                yield break;

            ParticleSystem[] particles = instance.GetComponentsInChildren<ParticleSystem>(true);
            if (particles.Length > 0)
                yield break;

            yield return new WaitForSeconds(FallbackLife);

            if (instance != null)
                Object.Destroy(instance);
        }

        private static IEnumerator ScaleFadeAndDestroy(GameObject instance, float scaleFrom, float scaleTo, float life, float rotateDegrees)
        {
            SpriteRenderer[] spriteRenderers = instance.GetComponentsInChildren<SpriteRenderer>(true);
            ParticleSystem[] particleSystems = instance.GetComponentsInChildren<ParticleSystem>(true);
            float elapsed = 0f;

            while (elapsed < life && instance != null)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / life);
                float eased = 1f - Mathf.Pow(1f - t, 2f);
                float alpha = 1f - t;

                instance.transform.localScale = Vector3.one * Mathf.Lerp(scaleFrom, scaleTo, eased);
                if (Mathf.Abs(rotateDegrees) > 0.01f)
                    instance.transform.Rotate(0f, 0f, rotateDegrees * Time.deltaTime / life);

                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    if (spriteRenderers[i] == null)
                        continue;

                    Color c = spriteRenderers[i].color;
                    c.a = alpha;
                    spriteRenderers[i].color = c;
                }

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i] == null)
                        continue;

                    ParticleSystem.MainModule main = particleSystems[i].main;
                    Color c = main.startColor.color;
                    c.a = alpha;
                    main.startColor = c;
                }

                yield return null;
            }

            if (instance != null)
                Object.Destroy(instance);
        }

        private static IEnumerator FadeLineAndDestroy(LineRenderer line, float life)
        {
            if (line == null)
                yield break;

            Color color = line.startColor;
            float elapsed = 0f;

            while (elapsed < life && line != null)
            {
                elapsed += Time.deltaTime;
                float alpha = 1f - Mathf.Clamp01(elapsed / life);
                line.colorGradient = BuildGradient(color, alpha);
                yield return null;
            }

            if (line != null)
                Object.Destroy(line.gameObject);
        }

        private static IEnumerator DestroyAfter(GameObject instance, float life)
        {
            yield return new WaitForSeconds(life);

            if (instance != null)
                Object.Destroy(instance);
        }

        private static void ApplyAdditiveCore(GameObject instance)
        {
            ParticleSystemRenderer[] particleRenderers = instance.GetComponentsInChildren<ParticleSystemRenderer>(true);
            for (int i = 0; i < particleRenderers.Length; i++)
                particleRenderers[i].sharedMaterial = SharedAdditiveMaterial;
        }

        // Switches sprite renderers to additive blend and amplifies the element color so a teal base
        // sprite reads as a clean element glow. Element-agnostic: scales the whole color uniformly so
        // its dominant channel reaches AdditiveTargetPeak (>1) while keeping channel RATIOS intact,
        // so hue is preserved for any element (ember-orange Physical, purple Void, future elements).
        private const float AdditiveTargetPeak = 1.5f;

        private static void ApplyAdditiveSprite(GameObject instance, Color color)
        {
            float peak = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            float factor = peak > 0.0001f ? AdditiveTargetPeak / peak : 1f;
            Color boosted = new Color(color.r * factor, color.g * factor, color.b * factor);

            SpriteRenderer[] spriteRenderers = instance.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] == null)
                    continue;

                spriteRenderers[i].sharedMaterial = SharedAdditiveMaterial;
                spriteRenderers[i].color = new Color(boosted.r, boosted.g, boosted.b, spriteRenderers[i].color.a);
            }
        }

        private static void ForceSorting(Renderer renderer, int order)
        {
            if (renderer == null)
                return;

            renderer.sortingLayerName = SortingLayerName;
            renderer.sortingOrder = Mathf.Max(renderer.sortingOrder, order);
        }

        private static Quaternion DirectionRotation(Vector2 dir)
        {
            if (dir.sqrMagnitude < 0.001f)
                return Quaternion.identity;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, 0f, angle);
        }

        private static Gradient BuildGradient(Color color, float alpha)
        {
            Color start = WithAlpha(color, alpha);
            Color end = WithAlpha(color, 0f);

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(start, 0f), new GradientColorKey(color, 1f) },
                new[] { new GradientAlphaKey(start.a, 0f), new GradientAlphaKey(end.a, 1f) });
            return gradient;
        }

        private static Color FromHex(int rgb)
        {
            float r = ((rgb >> 16) & 0xFF) / 255f;
            float g = ((rgb >> 8) & 0xFF) / 255f;
            float b = (rgb & 0xFF) / 255f;
            return new Color(r, g, b, 1f);
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static GameObject CreateRuntimeSpritePrefab(string name, Sprite sprite)
        {
            GameObject go = new GameObject(name);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingLayerName = SortingLayerName;
            sr.sortingOrder = SortingOrder;
            go.SetActive(false);
            return go;
        }

        private static GameObject LoadPrefab(string resourcesPath, string assetPath)
        {
            GameObject prefab = Resources.Load<GameObject>(resourcesPath);
            if (prefab != null)
                return prefab;

#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
#else
            return null;
#endif
        }

        private static Sprite LoadSprite(string assetPath)
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
#else
            return null;
#endif
        }
    }

    public sealed class SkillVfxRunner : MonoBehaviour
    {
        public void Run(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}
