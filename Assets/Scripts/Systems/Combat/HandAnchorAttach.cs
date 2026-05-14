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

            Vector3 target = hasLeft && hasRight
                ? Vector3.Lerp(SpritePixelToWorld(bodyRenderer, data.sprite, leftPx), SpritePixelToWorld(bodyRenderer, data.sprite, rightPx), 0.5f)
                : SpritePixelToWorld(bodyRenderer, data.sprite, preferRightHand && hasRight ? rightPx : leftPx);

            _weaponInstance.transform.position = target + (_currentEntry != null ? _currentEntry.anchorOffset : Vector3.zero);
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
