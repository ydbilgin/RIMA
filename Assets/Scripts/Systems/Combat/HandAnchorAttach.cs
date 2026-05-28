using UnityEngine;
using RIMA.Core;
using RIMA.Combat;
using RIMA.Data;

namespace RIMA
{
    /// <summary>
    /// Level 1 OrbitAttach: static HandAnchor. Weapon parented here at spawn.
    /// Karar #123 Yol A Level 1 fallback. Level 2 reads per-sprite hand anchors.
    ///
    /// A2 Mount Bridge: drives OrientationSync per-frame from PlayerController.FacingDirection.
    /// Also handles per-direction weapon sort order (weapon behind body for N/NE/NW).
    /// </summary>
    public class HandAnchorAttach : MonoBehaviour
    {
        private enum AttachMode
        {
            Level1Static,
            Level2SpriteHandData
        }

        [SerializeField] private WeaponDatabaseSO weaponDatabase;
        [SerializeField] private string classId = "Warblade";
        [SerializeField] private Transform handAnchor; // assign in Inspector
        [SerializeField] private AttachMode attachMode = AttachMode.Level1Static;
        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private SpriteHandData[] spriteHandData;
        [SerializeField] private bool preferRightHand;

        [Header("Orientation Bridge (A2)")]
        [SerializeField] private OrientationSync orientationSync;
        [SerializeField] private SpriteRenderer weaponRenderer; // set after spawn; auto-found if null

        private PlayerController _playerController;
        private PlayerAttack _playerAttack;
        private GameObject _weaponInstance;
        private WeaponDatabaseSO.WeaponEntry _currentEntry;
        private FacingDir8 _lastDir = (FacingDir8)(-1); // force first sync

        /// <summary>The runtime-spawned weapon instance (set after Start).</summary>
        public GameObject WeaponInstance => _weaponInstance;

        private void Awake()
        {
            // PlayerController/PlayerAttack are not in the prefab asset; acquire at runtime.
            _playerController = GetComponent<PlayerController>();
            _playerAttack = GetComponent<PlayerAttack>();
        }

        private void OnEnable()
        {
            // Trigger the procedural weapon swing on the attack input frame.
            if (_playerAttack != null)
                _playerAttack.OnComboStep += HandleComboStep;
        }

        private void OnDisable()
        {
            if (_playerAttack != null)
                _playerAttack.OnComboStep -= HandleComboStep;
        }

        // Fired by PlayerAttack at attack start (input frame). Sweeps the weapon
        // through an arc relative to the current facing-dir base rotation.
        private void HandleComboStep(int step)
        {
            if (orientationSync == null || _playerController == null) return;
            FacingDir8 dir = VectorToDir8(_playerController.FacingDirection);
            // Keep flip/offset/sort current for the swing direction before it starts.
            orientationSync.Sync(dir);
            UpdateWeaponSortOrder(dir);
            _lastDir = dir;
            // Align the swing's strike frame to the mechanical hit (attackStartup).
            orientationSync.BeginSwing(dir, _playerAttack.CurrentSwingWindow, _playerAttack.CurrentStrikeFraction);
        }

        private void Start()
        {
            AttachWeapon("Base");

            // Wire OrientationSync.weaponTransform to the spawned weapon instance.
            if (orientationSync != null && _weaponInstance != null)
            {
                orientationSync.SetWeaponTransform(_weaponInstance.transform);

                // Grab weapon renderer for sort order updates.
                if (weaponRenderer == null)
                    weaponRenderer = _weaponInstance.GetComponentInChildren<SpriteRenderer>();
            }

            // Force initial sync.
            _lastDir = (FacingDir8)(-1);
        }

        private void LateUpdate()
        {
            // --- A2: Orientation bridge (Level1Static) ---
            if (attachMode == AttachMode.Level1Static && _playerController != null && orientationSync != null)
            {
                FacingDir8 dir = VectorToDir8(_playerController.FacingDirection);
                // Don't advance _lastDir while a swing owns the rotation: Sync() skips the
                // rotation write mid-swing, so we must let the swing finish (IsSwinging false)
                // and then resync to the current dir, otherwise the weapon stays stuck at the
                // old swing-dir base rotation until facing changes again.
                if (dir != _lastDir && !orientationSync.IsSwinging)
                {
                    _lastDir = dir;
                    orientationSync.Sync(dir);
                    UpdateWeaponSortOrder(dir);
                }
            }

            // --- Level2SpriteHandData (unchanged) ---
            if (attachMode != AttachMode.Level2SpriteHandData) return;
            if (_weaponInstance == null || bodyRenderer == null || bodyRenderer.sprite == null) return;
            if (!TryGetCurrentHandData(out SpriteHandData data)) return;

            bool hasLeft = data.TryGetLeft(out Vector2 leftPx);
            bool hasRight = data.TryGetRight(out Vector2 rightPx);
            if (!hasLeft && !hasRight) return;

            Vector3 leftWorld = hasLeft ? SpritePixelToWorld(bodyRenderer, data.sprite, leftPx) : Vector3.zero;
            Vector3 rightWorld = hasRight ? SpritePixelToWorld(bodyRenderer, data.sprite, rightPx) : Vector3.zero;
            bool useTwoHand = _currentEntry != null && _currentEntry.twoHanded && hasLeft && hasRight;
            Vector3 target = useTwoHand
                ? Vector3.Lerp(leftWorld, rightWorld, 0.5f)
                : preferRightHand && hasRight ? rightWorld : leftWorld;

            Vector3 gripOffset = _currentEntry != null ? _currentEntry.gripOffset : Vector3.zero;
            _weaponInstance.transform.position = target + gripOffset;

            if (useTwoHand && _currentEntry.orientBetweenHands)
            {
                Vector3 direction = rightWorld - leftWorld;
                if (direction.sqrMagnitude > 0.0001f)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _currentEntry.orientationOffsetDegrees;
                    _weaponInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
        }

        public void AttachWeapon(string formId)
        {
            if (_weaponInstance != null) Destroy(_weaponInstance);
            var entry = weaponDatabase?.GetWeapon(classId, formId);
            if (entry?.weaponPrefab == null) return;

            _currentEntry = entry;
            _weaponInstance = Instantiate(entry.weaponPrefab, handAnchor);
            _weaponInstance.transform.localPosition = entry.anchorOffset;
            _weaponInstance.transform.localRotation = Quaternion.identity;
        }

        // ─── A2 helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Converts a 2D screen-space direction to the nearest FacingDir8 octant.
        /// Convention: +X=E, -X=W, +Y=N, -Y=S (matches PlayerAnimator SnapToFourDiagonal).
        /// FacingDir8 order: S(0), SE(1), E(2), NE(3), N(4), NW(5), W(6), SW(7).
        /// </summary>
        public static FacingDir8 VectorToDir8(Vector2 dir)
        {
            if (dir.sqrMagnitude < 0.0001f) return FacingDir8.S;

            // atan2 returns angle in (-π, π], 0=east, CCW positive.
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // Shift so that 0° = South in FacingDir8 (south = -Y = atan2 -90°).
            // Map: angle → closest 45° step, starting from S.
            // S=-90, SE=-45, E=0, NE=45, N=90, NW=135, W=180/-180, SW=-135.
            // Quantize to 8 steps of 45°, index 0=S.
            // Remap angle so that S(-90°) → index 0:
            float remapped = angle + 90f; // S→0, SE→45, E→90, NE→135, N→180, NW→225, W→270/-90, SW→-45/315
            // Normalize to [0, 360)
            remapped = ((remapped % 360f) + 360f) % 360f;
            int index = Mathf.RoundToInt(remapped / 45f) % 8;
            return (FacingDir8)index;
        }

        private void UpdateWeaponSortOrder(FacingDir8 dir)
        {
            if (weaponRenderer == null || bodyRenderer == null) return;
            // Weapon behind body when facing N/NE/NW (player back is visible, weapon arm goes behind).
            bool behindBody = dir == FacingDir8.N || dir == FacingDir8.NE || dir == FacingDir8.NW;
            weaponRenderer.sortingOrder = bodyRenderer.sortingOrder + (behindBody ? -1 : 1);
        }

        private bool TryGetCurrentHandData(out SpriteHandData data)
        {
            data = null;
            if (spriteHandData == null) return false;

            Sprite current = bodyRenderer.sprite;
            for (int i = 0; i < spriteHandData.Length; i++)
            {
                SpriteHandData candidate = spriteHandData[i];
                if (candidate != null && candidate.Matches(current))
                {
                    data = candidate;
                    return true;
                }
            }

            return false;
        }

        private static Vector3 SpritePixelToWorld(SpriteRenderer renderer, Sprite sprite, Vector2 pixel)
        {
            Vector2 local = (pixel - sprite.pivot) / sprite.pixelsPerUnit;
            return renderer.transform.TransformPoint(local);
        }

        private void OnDrawGizmosSelected()
        {
            if (bodyRenderer == null || bodyRenderer.sprite == null) return;
            if (!TryGetCurrentHandData(out SpriteHandData data)) return;

            if (data.TryGetLeft(out Vector2 leftPx))
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(SpritePixelToWorld(bodyRenderer, data.sprite, leftPx), 0.04f);
            }

            if (data.TryGetRight(out Vector2 rightPx))
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(SpritePixelToWorld(bodyRenderer, data.sprite, rightPx), 0.04f);
            }
        }
    }
}
