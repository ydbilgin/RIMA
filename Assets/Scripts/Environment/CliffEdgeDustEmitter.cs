using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    /// <summary>
    /// F4: Spawns dust ParticleSystems along cliff edge cells (floor cells with S/SE/SW void).
    /// Reads edge inventory from CliffAutoPlacer — does NOT modify it.
    /// Performance guard: scene-wide 200 particle cap + camera distance LOD.
    /// </summary>
    [AddComponentMenu("RIMA/Environment/Cliff Edge Dust Emitter")]
    public sealed class CliffEdgeDustEmitter : MonoBehaviour
    {
        [Header("Dependencies")]
        public CliffAutoPlacer cliffPlacer;
        public CliffDustSettings settings;

        [Header("Sorting")]
        [Tooltip("Should match cliff base sorting order (-1 relative to cliff sprites).")]
        public int sortingOrder = -1;
        public string sortingLayerName = "Ground";

        // Iso neighbor vectors (same as CliffAutoPlacer)
        private static readonly Vector3Int SouthCell   = new Vector3Int(-1, -1, 0);
        private static readonly Vector3Int SouthEast   = new Vector3Int( 0, -1, 0);
        private static readonly Vector3Int SouthWest   = new Vector3Int(-1,  0, 0);

        private readonly List<ParticleSystem> _emitters = new List<ParticleSystem>();
        private Transform _emitterRoot;
        private Camera _cam;

        private void Start()
        {
            _cam = Camera.main;
            BuildEmitters();
        }

        /// <summary>Destroys existing emitter children and rebuilds from current cliff edge cells.</summary>
        [ContextMenu("Rebuild Dust Emitters")]
        public void BuildEmitters()
        {
            if (cliffPlacer == null || settings == null)
            {
                Debug.LogWarning("[CliffEdgeDustEmitter] cliffPlacer or settings not assigned.", this);
                return;
            }

            // Tear down old root
            if (_emitterRoot != null)
                Destroy(_emitterRoot.gameObject);

            _emitters.Clear();
            _emitterRoot = new GameObject("_DustEmitters").transform;
            _emitterRoot.SetParent(transform, false);

            Tilemap floorMap = cliffPlacer.floorTilemap;
            if (floorMap == null) return;

            // Collect edge cells: floor cells where S, SE, or SW neighbor is void
            var edgeCells = new List<Vector3Int>();
            foreach (Vector3Int cell in floorMap.cellBounds.allPositionsWithin)
            {
                if (!floorMap.HasTile(cell)) continue;
                bool sEmpty  = !floorMap.HasTile(cell + SouthCell);
                bool seEmpty = !floorMap.HasTile(cell + SouthEast);
                bool swEmpty = !floorMap.HasTile(cell + SouthWest);
                if (sEmpty || seEmpty || swEmpty)
                    edgeCells.Add(cell);
            }

            foreach (Vector3Int cell in edgeCells)
            {
                Vector3 world = floorMap.GetCellCenterWorld(cell);
                _emitters.Add(CreateEmitter(world));
            }
        }

        private ParticleSystem CreateEmitter(Vector3 worldPos)
        {
            var go = new GameObject("DustPS");
            go.transform.SetParent(_emitterRoot, false);
            go.transform.position = worldPos;

            var ps = go.AddComponent<ParticleSystem>();

            // --- Main module ---
            var main = ps.main;
            main.loop = true;
            main.playOnAwake = true;
            main.startLifetime  = new ParticleSystem.MinMaxCurve(settings.lifetimeMin, settings.lifetimeMax);
            main.startSpeed     = new ParticleSystem.MinMaxCurve(settings.fallSpeed * 0.5f, settings.fallSpeed);
            main.startSize      = new ParticleSystem.MinMaxCurve(settings.startSizeMin, settings.startSizeMax);
            main.startColor     = settings.colorTint;
            main.gravityModifier = settings.gravityModifier;
            // Per-emitter hard cap: divide total budget across all emitters (+1 for current not yet added)
            int emitterCount = _emitters.Count;
            main.maxParticles = Mathf.Max(1, settings.maxTotalParticles / Mathf.Max(1, emitterCount + 1));
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            // --- Emission ---
            var emission = ps.emission;
            emission.enabled    = true;
            emission.rateOverTime = settings.emissionRate;

            // --- Shape: small disc offset so particles drift from edge ---
            var shape = ps.shape;
            shape.enabled    = true;
            shape.shapeType  = ParticleSystemShapeType.Circle;
            shape.radius     = 0.15f;
            shape.rotation   = new Vector3(90f, 0f, 0f); // face down

            // --- Velocity over lifetime: downward + tiny random lateral ---
            var vol = ps.velocityOverLifetime;
            vol.enabled = true;
            vol.space   = ParticleSystemSimulationSpace.World;
            vol.y = new ParticleSystem.MinMaxCurve(-settings.fallSpeed, -settings.fallSpeed * 0.5f);
            vol.x = new ParticleSystem.MinMaxCurve(-settings.lateralSpread, settings.lateralSpread);

            // --- Color over lifetime: alpha fade out ---
            var col = ps.colorOverLifetime;
            col.enabled = true;
            var grad = new Gradient();
            grad.SetKeys(
                new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
            );
            col.color = new ParticleSystem.MinMaxGradient(grad);

            // --- Renderer ---
            var rend = go.GetComponent<ParticleSystemRenderer>();
            rend.sortingLayerName = sortingLayerName;
            rend.sortingOrder     = sortingOrder;
            rend.renderMode = ParticleSystemRenderMode.Billboard;
            // CRITICAL: assign URP-compatible material to avoid magenta in URP 2D
            if (rend.sharedMaterial == null)
            {
                var defaultMat = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
                if (defaultMat != null) rend.sharedMaterial = defaultMat;
            }

            ps.Play();
            return ps;
        }

        private void Update()
        {
            if (_cam == null) _cam = Camera.main; // lazy re-fetch (scene reload safe)
            if (_cam == null || settings == null || _emitters.Count == 0) return;

            // LOD: count total active particles scene-wide from our emitters
            int totalActive = 0;
            foreach (var ps in _emitters)
                if (ps != null) totalActive += ps.particleCount;

            float cullSqr = settings.lodCullDistance * settings.lodCullDistance;
            Vector3 camPos = _cam.transform.position;

            foreach (var ps in _emitters)
            {
                if (ps == null) continue;

                bool tooFar = (ps.transform.position - camPos).sqrMagnitude > cullSqr;
                bool overBudget = totalActive >= settings.maxTotalParticles;

                var emission = ps.emission;
                bool shouldEmit = !tooFar && !overBudget;

                if (shouldEmit && !emission.enabled)
                    emission.enabled = true;
                else if (!shouldEmit && emission.enabled)
                    emission.enabled = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Rebuild in edit mode when settings change
            if (!Application.isPlaying && cliffPlacer != null && settings != null)
                BuildEmitters();
        }
#endif
    }
}
