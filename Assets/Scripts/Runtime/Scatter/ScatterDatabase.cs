using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Runtime.Scatter
{
    [CreateAssetMenu(menuName = "RIMA/Scatter/Scatter Database")]
    public sealed class ScatterDatabase : ScriptableObject
    {
        public List<ScatterItem> items = new List<ScatterItem>();

        public ScatterItem GetItem(string category)
        {
            if (items == null)
                return null;

            for (int i = 0; i < items.Count; i++)
            {
                ScatterItem item = items[i];
                if (item != null && item.category == category)
                    return item;
            }

            return null;
        }
    }
}
