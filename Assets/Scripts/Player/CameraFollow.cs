using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    [DefaultExecutionOrder(100)]
    [System.Obsolete("Not the live spine - see WORK_ORDER_24_48H_S6. Live camera = Camera/CameraFollow.cs (RIMA.CameraSystem).")]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 8f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

        [Header("Combat Framing")]
        [SerializeField] private bool enforceCombatOrthographicSize = true;
        [SerializeField, Range(4.2f, 7.5f)] private float combatOrthographicSize = 5.15f;

        [Header("Room Bounds (world units)")]
        [SerializeField] private bool useBounds = true;
        [SerializeField] private Vector2 roomMin = new Vector2(0f, 0f);
        [SerializeField] private Vector2 roomMax = new Vector2(20f, 15f);
        [SerializeField] private bool autoBoundsFromFloorTilemap = true;
        [SerializeField] private string floorTilemapPath = "IsoGrid/Ground";
        [SerializeField] private Vector2 boundsPadding = new Vector2(0.25f, 0.25f);

        [Header("Startup")]
        [SerializeField] private bool snapToTargetOnStart = true;

        private Camera cam;

        public void SetTarget(Transform t) => target = t;

        public void SetBounds(Vector2 min, Vector2 max)
        {
            roomMin = min;
            roomMax = max;
            useBounds = true;
        }

        public void SetBounds(Bounds worldBounds)
        {
            roomMin = new Vector2(worldBounds.min.x + boundsPadding.x, worldBounds.min.y + boundsPadding.y);
            roomMax = new Vector2(worldBounds.max.x - boundsPadding.x, worldBounds.max.y - boundsPadding.y);
            useBounds = true;
        }

        private void Awake()
        {
            cam = GetComponent<Camera>();
            ApplyCombatFraming();
        }

        private void Start()
        {
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }

            if (useBounds && autoBoundsFromFloorTilemap)
            {
                TryReadRoomBoundsFromFloor();
            }

            ApplyCombatFraming();

            if (snapToTargetOnStart && target != null)
            {
                transform.position = ClampToRoom(target.position + offset);
            }
        }

        private void ApplyCombatFraming()
        {
            if (!enforceCombatOrthographicSize || cam == null || !cam.orthographic) return;
            cam.orthographicSize = combatOrthographicSize;
        }

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = ClampToRoom(target.position + offset);

            // Juice offsets added on top (both must expose offsets, NOT write the transform — see CameraPunchController).
            Vector3 fxOffset = (CameraShake.Instance != null ? CameraShake.Instance.CurrentOffset : Vector3.zero)
                             + (RIMA.Combat.Juice.CameraPunchController.Instance != null ? RIMA.Combat.Juice.CameraPunchController.Instance.CurrentOffset : Vector3.zero);
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime) + fxOffset;
        }

        private Vector3 ClampToRoom(Vector3 desired)
        {
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

            return desired;
        }

        private void TryReadRoomBoundsFromFloor()
        {
            Tilemap floor = null;
            var floorGO = GameObject.Find(floorTilemapPath) ?? GameObject.Find("Room/Floor");
            if (floorGO != null) floor = floorGO.GetComponent<Tilemap>();
            if (floor == null) 
            {
                useBounds = false; // Disable bounds if we can't find the floor
                return;
            }

            var rendererBounds = floor.GetComponent<Renderer>()?.bounds;
            if (rendererBounds == null || rendererBounds.Value.size.sqrMagnitude <= 0f)
            {
                floor.CompressBounds();
                rendererBounds = floor.localBounds;
            }

            Bounds bounds = rendererBounds.Value;
            roomMin = new Vector2(bounds.min.x + boundsPadding.x, bounds.min.y + boundsPadding.y);
            roomMax = new Vector2(bounds.max.x - boundsPadding.x, bounds.max.y - boundsPadding.y);
        }
    }
}
