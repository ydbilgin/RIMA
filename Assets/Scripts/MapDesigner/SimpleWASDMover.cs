using UnityEngine;

namespace RIMA.MapDesigner
{
    /// <summary>
    /// Simple WASD movement for testing terrain blend visually.
    /// Attach to Player in ShaderBlend_Test scene.
    /// </summary>
    public class SimpleWASDMover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 8f;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.gravityScale = 0f;
                _rb.freezeRotation = true;
            }
        }

        private void Update()
        {
            float h = 0f, v = 0f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v = 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v = -1f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h = -1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h = 1f;

            Vector2 move = new Vector2(h, v).normalized * moveSpeed;

            if (_rb != null)
            {
                _rb.linearVelocity = move;
            }
            else
            {
                transform.Translate(move * Time.deltaTime, Space.World);
            }
        }
    }
}
