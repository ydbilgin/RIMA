using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Level 1 OrbitAttach: static HandAnchor. Weapon parented here at spawn.
    /// Karar #123 Yol A Level 1. Level 2 (per-frame AnimationCurve) = Faz 2.
    /// </summary>
    public class HandAnchorAttach : MonoBehaviour
    {
        [SerializeField] private WeaponDatabaseSO weaponDatabase;
        [SerializeField] private string classId = "Warblade";
        [SerializeField] private Transform handAnchor; // assign in Inspector

        private GameObject _weaponInstance;

        private void Start()
        {
            AttachWeapon("Base");
        }

        public void AttachWeapon(string formId)
        {
            if (_weaponInstance != null) Destroy(_weaponInstance);
            var entry = weaponDatabase?.GetWeapon(classId, formId);
            if (entry?.weaponPrefab == null) return;

            _weaponInstance = Instantiate(entry.weaponPrefab, handAnchor);
            _weaponInstance.transform.localPosition = entry.anchorOffset;
            _weaponInstance.transform.localRotation = Quaternion.identity;
        }
    }
}
