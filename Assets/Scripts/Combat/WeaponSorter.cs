using UnityEngine;
using RIMA.Core;

namespace RIMA.Combat
{
    public sealed class WeaponSorter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer weaponRenderer;
        [SerializeField] private SpriteRenderer bodyRenderer;

        public void UpdateSort(FacingDir8 dir)
        {
            if (weaponRenderer == null || bodyRenderer == null) return;

            weaponRenderer.sortingOrder = bodyRenderer.sortingOrder + (IsBehindBody(dir) ? -1 : 1);
        }

        private static bool IsBehindBody(FacingDir8 dir)
        {
            return dir == FacingDir8.N || dir == FacingDir8.NE || dir == FacingDir8.NW;
        }
    }
}

