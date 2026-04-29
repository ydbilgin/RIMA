using UnityEngine;

namespace RIMA
{
    public class StopDustVFX : MonoBehaviour
    {
        private Rigidbody2D rb;
        private ParticleSystem dust;
        private bool wasMoving;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            dust = GetComponentInChildren<ParticleSystem>();
        }

        void FixedUpdate()
        {
            bool isMoving = rb.linearVelocity.magnitude > 0.15f;
            if (wasMoving && !isMoving && dust != null)
                dust.Play();
            wasMoving = isMoving;
        }
    }
}