using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Gentle vertical bob + slow Z-rotation for placeholder GameObjects.
    /// Reusable on any object — shop stands, rune discs, etc.
    /// Replace with real art/animation when assets are ready.
    /// </summary>
    public class PlaceholderFloat : MonoBehaviour
    {
        [SerializeField] private float bobAmplitude  = 0.12f;  // world-units
        [SerializeField] private float bobSpeed      = 1.4f;   // radians/sec
        [SerializeField] private float rotateSpeed   = 45f;    // degrees/sec Z-axis
        [SerializeField] private float phaseOffset   = 0f;     // sync-offset across instances

        private Vector3 _startLocalPos;

        private void Awake()
        {
            _startLocalPos = transform.localPosition;

            // Stable per-instance phase so multiple objects don't bob in perfect sync.
            if (phaseOffset == 0f)
                phaseOffset = (GetInstanceID() & 0x7FFF) * 0.001f;
        }

        private void Update()
        {
            // Bob
            var p = _startLocalPos;
            p.y += Mathf.Sin((Time.time + phaseOffset) * bobSpeed) * bobAmplitude;
            transform.localPosition = p;

            // Slow Z-rotation
            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime, Space.Self);
        }
    }
}
