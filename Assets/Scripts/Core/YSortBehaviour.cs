using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Core
{
    public class YSortBehaviour : MonoBehaviour
    {
        [SerializeField] private string sortingLayerName = "Entities";
        [SerializeField] private int baseOrder = 0;
        [SerializeField] private float yMultiplier = 100f;

        private SpriteRenderer sr;
        private TilemapRenderer tr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                tr = GetComponent<TilemapRenderer>();
            }
        }

        private void LateUpdate()
        {
            int order = baseOrder - Mathf.RoundToInt(transform.position.y * yMultiplier);

            if (sr != null)
            {
                sr.sortingLayerName = sortingLayerName;
                sr.sortingOrder = order;
            }
            else if (tr != null)
            {
                tr.sortingLayerName = sortingLayerName;
                tr.sortingOrder = order;
            }
        }
    }
}
