using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Makes a floating "seal-shard" crystal gently bob up and down in the air.
    /// Used on hovering light sources in the wall-less floating-island arenas
    /// (the in-world replacement for wall-mounted braziers). The bob is a pure
    /// local-position offset on the Y axis; the phase is derived deterministically
    /// per-instance so multiple shards oscillate out of sync without any randomness.
    /// </summary>
    public class FloatingSealShard : MonoBehaviour
    {
        [SerializeField] private float amplitude = 0.15f;
        [SerializeField] private float speed = 1.5f;
        [SerializeField] private float phaseOffset = 0f;

        private Vector3 _startLocalPos;

        private void Awake()
        {
            _startLocalPos = transform.localPosition;

            // Derive a stable per-instance phase so shards bob out of sync.
            // Deterministic (no Random) — based on the instance ID.
            if (phaseOffset == 0f)
            {
                phaseOffset = (GetInstanceID() & 0x7FFF) * 0.001f;
            }
        }

        private void Update()
        {
            var p = _startLocalPos;
            p.y = _startLocalPos.y + Mathf.Sin((Time.time + phaseOffset) * speed) * amplitude;
            transform.localPosition = p;
        }
    }
}
