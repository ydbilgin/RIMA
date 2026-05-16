using System;
using UnityEngine;

namespace RIMA.MapDesigner.Composition
{
    [Serializable]
    public class CompositionRoleMap
    {
        public RectInt bounds;
        public CompositionRole[] flatRoleGrid;

        public CompositionRoleMap() { }

        public CompositionRoleMap(RectInt bounds)
        {
            this.bounds = bounds;
            int count = Mathf.Max(0, bounds.width * bounds.height);
            flatRoleGrid = new CompositionRole[count];
        }

        public CompositionRole GetRoleAt(Vector2Int tilePos)
        {
            if (!IsInside(tilePos)) return CompositionRole.Empty;
            int idx = IndexFor(tilePos);
            if (idx < 0 || flatRoleGrid == null || idx >= flatRoleGrid.Length) return CompositionRole.Empty;
            return flatRoleGrid[idx];
        }

        public void SetRoleAt(Vector2Int tilePos, CompositionRole role)
        {
            if (!IsInside(tilePos)) return;
            int idx = IndexFor(tilePos);
            if (idx < 0 || flatRoleGrid == null || idx >= flatRoleGrid.Length) return;
            flatRoleGrid[idx] = role;
        }

        public int CountOfRole(CompositionRole role)
        {
            if (flatRoleGrid == null) return 0;
            int n = 0;
            for (int i = 0; i < flatRoleGrid.Length; i++)
            {
                if (flatRoleGrid[i] == role) n++;
            }
            return n;
        }

        private bool IsInside(Vector2Int tilePos)
        {
            return tilePos.x >= bounds.xMin && tilePos.x < bounds.xMax &&
                   tilePos.y >= bounds.yMin && tilePos.y < bounds.yMax;
        }

        private int IndexFor(Vector2Int tilePos)
        {
            int lx = tilePos.x - bounds.xMin;
            int ly = tilePos.y - bounds.yMin;
            return ly * bounds.width + lx;
        }
    }
}
