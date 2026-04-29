using UnityEngine;

namespace RIMA
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 8f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

        [Header("Room Bounds (world units)")]
        [SerializeField] private bool useBounds = true;
        [SerializeField] private Vector2 roomMin = new Vector2(0f, 0f);
        [SerializeField] private Vector2 roomMax = new Vector2(20f, 15f);

        private Camera cam;

        public void SetTarget(Transform t) => target = t;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void Start()
        {
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + offset;

            if (useBounds && cam != null)
            {
                float halfH = cam.orthographicSize;
                float halfW = halfH * cam.aspect;

                float minX = roomMin.x + halfW;
                float maxX = roomMax.x - halfW;
                desired.x = (maxX >= minX)
                    ? Mathf.Clamp(desired.x, minX, maxX)
                    : (roomMin.x + roomMax.x) * 0.5f;

                float minY = roomMin.y + halfH;
                float maxY = roomMax.y - halfH;
                desired.y = (maxY >= minY)
                    ? Mathf.Clamp(desired.y, minY, maxY)
                    : (roomMin.y + roomMax.y) * 0.5f;
            }

            Vector3 shakeOffset = CameraShake.Instance != null ? CameraShake.Instance.CurrentOffset : Vector3.zero;
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime) + shakeOffset;
        }
    }
}
