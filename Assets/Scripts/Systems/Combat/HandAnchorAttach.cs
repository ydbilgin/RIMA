using UnityEngine;
using RIMA.Data;

namespace RIMA
{
    /// <summary>
    /// Level 1 OrbitAttach: static HandAnchor. Weapon parented here at spawn.
    /// Karar #123 Yol A Level 1 fallback. Level 2 reads per-sprite hand anchors.
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

        private GameObject _weaponInstance;
        private WeaponDatabaseSO.WeaponEntry _currentEntry;

        private void Start()
        {
            AttachWeapon("Base");
        }

        private void LateUpdate()
        {
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
