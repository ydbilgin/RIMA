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
        private Vector3 _basePos;
        private bool _baseInit;

        private void LateUpdate()
        {
            if (target == null)
            {
                var p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) target = p.transform;
                else return;
            }

            if (!_baseInit) { _basePos = transform.position; _baseInit = true; }

            Vector3 desired = target.position + worldOffset;
            _basePos = Vector3.SmoothDamp(_basePos, desired, ref currentVelocity, smoothTime);

            // Juice offsets are ADDED on top. CameraShake + CameraPunchController expose decaying
            // offsets and must NOT write the camera transform themselves (that pins the camera and
            // fights this follow — the root cause of "camera not following").
            Vector3 fx = Vector3.zero;
            if (RIMA.CameraShake.Instance != null) fx += RIMA.CameraShake.Instance.CurrentOffset;
            if (RIMA.Combat.Juice.CameraPunchController.Instance != null) fx += RIMA.Combat.Juice.CameraPunchController.Instance.CurrentOffset;
            if (RIMA.Combat.ScreenShakeDriver.Instance != null) fx += RIMA.Combat.ScreenShakeDriver.Instance.CurrentOffset;

            transform.position = _basePos + fx;
        }

        private void OnValidate()
        {
            if (worldOffset.z == 0) worldOffset = new Vector3(worldOffset.x, worldOffset.y, -10f);
        }
    }
}
