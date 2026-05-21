using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        private void FixedUpdate()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            float x = 0f;
            float y = 0f;

            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;

            Vector2 input = new Vector2(x, y);
            if (input.sqrMagnitude > 1f) input.Normalize();

            rb.position += input * (moveSpeed * Time.fixedDeltaTime);
        }
    }
}
