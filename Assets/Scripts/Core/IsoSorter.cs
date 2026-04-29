using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Isometric depth sorting — her sprite'ı Y pozisyonuna göre sıralar.
    /// Daha aşağıdaki obje = daha önde (yüksek sortingOrder).
    /// Tüm dinamik objelere (player, enemy, interactable) ekle.
    /// Static tile'lar Unity'nin kendi isometric sorting'ini kullanır.
    /// </summary>
    [ExecuteAlways]
    public class IsoSorter : MonoBehaviour
    {
        [Tooltip("Base sorting order offset. Player: 0, Enemy: 0, Item: -1")]
        [SerializeField] private int baseOrder = 0;

        [Tooltip("Y offset for pivot correction (sprite'ın ayak noktası)")]
        [SerializeField] private float pivotOffsetY = 0f;

        private SpriteRenderer sr;
        private static readonly int SORT_SCALE = 100;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            if (sr == null) return;
            // Aşağıdaki obje daha önde: pozitif Y → daha arkada → düşük order
            float y = transform.position.y + pivotOffsetY;
            sr.sortingOrder = baseOrder - Mathf.RoundToInt(y * SORT_SCALE);
        }
    }
}
