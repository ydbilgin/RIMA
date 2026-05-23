using UnityEngine;
using RIMA.Core;

namespace RIMA.Combat
{
    public sealed class OrientationSync : MonoBehaviour
    {
        [SerializeField] private Transform handAnchor;
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private Vector2[] handOffsets = new Vector2[8]
        {
            new Vector2(0.00f, -0.08f),
            new Vector2(0.08f, -0.04f),
            new Vector2(0.10f, 0.00f),
            new Vector2(0.07f, 0.05f),
            new Vector2(0.00f, 0.08f),
            new Vector2(-0.07f, 0.05f),
            new Vector2(-0.10f, 0.00f),
            new Vector2(-0.08f, -0.04f)
        };
        [SerializeField] private float[] weaponRotations = new float[8]
        {
            -90f,
            -45f,
            0f,
            45f,
            90f,
            135f,
            180f,
            -135f
        };

        public void Sync(FacingDir8 dir)
        {
            int index = (int)dir;
            if (index < 0 || index >= 8) return;

            if (handAnchor != null && handOffsets != null && index < handOffsets.Length)
            {
                handAnchor.localPosition = handOffsets[index];
            }

            if (weaponTransform != null && weaponRotations != null && index < weaponRotations.Length)
            {
                weaponTransform.localRotation = Quaternion.Euler(0f, 0f, weaponRotations[index]);
            }
        }

        private void OnValidate()
        {
            EnsureLength(ref handOffsets, 8);
            EnsureLength(ref weaponRotations, 8);
        }

        private static void EnsureLength<T>(ref T[] values, int length)
        {
            if (values == null)
            {
                values = new T[length];
                return;
            }

            if (values.Length == length) return;

            T[] resized = new T[length];
            for (int i = 0; i < values.Length && i < resized.Length; i++)
            {
                resized[i] = values[i];
            }

            values = resized;
        }
    }
}

