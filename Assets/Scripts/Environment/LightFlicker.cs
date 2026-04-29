using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RIMA.Environment
{
    /// <summary>
    /// Adds realistic flickering to Light2D components (torches, braziers, etc.)
    /// Modulates intensity and optionally color for atmospheric effect.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class LightFlicker : MonoBehaviour
    {
        [Header("Flicker Settings")]
        [SerializeField] private float baseIntensity = 1.5f;
        [SerializeField] private float flickerAmount = 0.3f;
        [SerializeField] private float flickerSpeed = 8f;
        
        [Header("Radius Pulse")]
        [SerializeField] private bool pulseRadius = true;
        [SerializeField] private float baseOuterRadius = 5f;
        [SerializeField] private float radiusPulseAmount = 0.5f;
        
        private Light2D light2D;
        private float noiseOffset;
        
        private void Awake()
        {
            light2D = GetComponent<Light2D>();
            noiseOffset = Random.Range(0f, 100f); // Each light flickers differently
            
            if (light2D != null)
            {
                baseIntensity = light2D.intensity;
                baseOuterRadius = light2D.pointLightOuterRadius;
            }
        }
        
        private void Update()
        {
            if (light2D == null) return;
            
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed + noiseOffset, 0f);
            float flicker = (noise - 0.5f) * 2f * flickerAmount; // -flickerAmount to +flickerAmount
            
            light2D.intensity = baseIntensity + flicker;
            
            if (pulseRadius)
            {
                float radiusNoise = Mathf.PerlinNoise(Time.time * flickerSpeed * 0.7f + noiseOffset + 50f, 0f);
                float radiusPulse = (radiusNoise - 0.5f) * 2f * radiusPulseAmount;
                light2D.pointLightOuterRadius = baseOuterRadius + radiusPulse;
            }
        }
    }
}
