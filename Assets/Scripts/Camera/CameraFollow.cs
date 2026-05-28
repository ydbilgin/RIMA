using UnityEngine;

namespace RIMA.CameraSystem
{
    [DefaultExecutionOrder(100)]
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        [Range(0.05f, 1f)] public float smoothTime = 0.15f;
        public Vector3 worldOffset = new Vector3(0f, 0f, -10f);

        private Vector3 currentVelocity;

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + worldOffset;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref currentVelocity, smoothTime);
        }

        private void OnValidate()
        {
            if (worldOffset.z == 0) worldOffset = new Vector3(worldOffset.x, worldOffset.y, -10f);
        }
    }
}
