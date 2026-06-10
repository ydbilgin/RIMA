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

        // Optional room-bounds clamp (BUG-2 2026-06-10): RoomRunDirector feeds the built room's
        // floor bounds so the follow camera never frames void beyond the room edge. Disabled by
        // default — existing scenes that use plain follow are unaffected.
        private bool _useBounds;
        private Vector2 _boundsMin;
        private Vector2 _boundsMax;
        private Camera _cam;

        public void SetBounds(Bounds worldBounds)
        {
            _boundsMin = worldBounds.min;
            _boundsMax = worldBounds.max;
            _useBounds = true;
            if (_cam == null) _cam = GetComponent<Camera>();
        }

        public void ClearBounds() => _useBounds = false;

        /// <summary>Jump straight to the target (room transitions) instead of panning across the map.</summary>
        public void SnapToTarget()
        {
            if (target == null) return;
            _basePos = ClampToBounds(target.position + worldOffset);
            currentVelocity = Vector3.zero;
            _baseInit = true;
            transform.position = _basePos;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                var p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) target = p.transform;
                else return;
            }

            if (!_baseInit) { _basePos = transform.position; _baseInit = true; }

            Vector3 desired = ClampToBounds(target.position + worldOffset);
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

        private Vector3 ClampToBounds(Vector3 desired)
        {
            if (!_useBounds) return desired;
            if (_cam == null) _cam = GetComponent<Camera>();
            if (_cam == null || !_cam.orthographic) return desired;

            float halfH = _cam.orthographicSize;
            float halfW = halfH * _cam.aspect;

            float minX = _boundsMin.x + halfW;
            float maxX = _boundsMax.x - halfW;
            desired.x = maxX >= minX
                ? Mathf.Clamp(desired.x, minX, maxX)
                : (_boundsMin.x + _boundsMax.x) * 0.5f;   // room narrower than view → center

            float minY = _boundsMin.y + halfH;
            float maxY = _boundsMax.y - halfH;
            desired.y = maxY >= minY
                ? Mathf.Clamp(desired.y, minY, maxY)
                : (_boundsMin.y + _boundsMax.y) * 0.5f;   // room shorter than view → center

            return desired;
        }

        private void OnValidate()
        {
            if (worldOffset.z == 0) worldOffset = new Vector3(worldOffset.x, worldOffset.y, -10f);
        }
    }
}
