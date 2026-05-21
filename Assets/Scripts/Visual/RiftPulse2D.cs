using UnityEngine;

public sealed class RiftPulse2D : MonoBehaviour
{
    [SerializeField] private float frequency = 0.8f;
    [SerializeField] private float amplitude = 0.03f;
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
        transform.localScale = new Vector3(baseScale.x * pulse, baseScale.y, baseScale.z);
    }
}
