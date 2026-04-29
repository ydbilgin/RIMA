using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Mouse imleci sprite üzerine gelince renkli outline gösterir.
    /// Enemy → kırmızı outline, NPC → rift mavisi outline.
    /// Child olarak ikinci bir SpriteRenderer kullanır (hafif, shader gerektirmez).
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class HoverOutline : MonoBehaviour
    {
        public enum OutlineType { Enemy, NPC, Neutral }

        [Header("Outline Tipi")]
        [SerializeField] public OutlineType outlineType = OutlineType.Enemy;

        [Header("Renkler (RIMA Paleti)")]
        [SerializeField] private Color enemyColor   = new Color(0.90f, 0.18f, 0.18f, 0.85f); // rift kırmızısı
        [SerializeField] private Color npcColor     = new Color(0.28f, 0.72f, 0.95f, 0.85f); // rift mavisi
        [SerializeField] private Color neutralColor = new Color(0.85f, 0.85f, 0.85f, 0.70f); // nötr gri

        [Header("Ayarlar")]
        [SerializeField] private float outlineScale = 1.18f;   // outline büyüklüğü

        private SpriteRenderer mainSr;
        private SpriteRenderer outlineSr;
        private GameObject outlineGo;

        private void Start()
        {
            mainSr = GetComponent<SpriteRenderer>();
            CreateOutlineChild();
        }

        private void CreateOutlineChild()
        {
            outlineGo = new GameObject("_Outline");
            outlineGo.transform.SetParent(transform, false);
            outlineGo.transform.localScale = Vector3.one * outlineScale;

            outlineSr = outlineGo.AddComponent<SpriteRenderer>();
            outlineSr.sortingLayerID  = mainSr.sortingLayerID;
            outlineSr.sortingOrder    = mainSr.sortingOrder - 1; // sprite'ın arkasında
            outlineSr.sprite          = mainSr.sprite;
            outlineSr.color           = GetOutlineColor();
            outlineSr.enabled         = false;
        }

        private void OnValidate()
        {
            // Sprite değişirse outline da güncelle
            if (outlineSr != null && mainSr != null)
                outlineSr.sprite = mainSr.sprite;
        }

        private void OnMouseEnter()
        {
            if (outlineSr == null) return;
            outlineSr.sprite  = mainSr.sprite;
            outlineSr.color   = GetOutlineColor();
            outlineSr.enabled = true;
        }

        private void OnMouseExit()
        {
            if (outlineSr != null)
                outlineSr.enabled = false;
        }

        private Color GetOutlineColor()
        {
            if (outlineType == OutlineType.Enemy)   return enemyColor;
            if (outlineType == OutlineType.NPC)     return npcColor;
            return neutralColor;
        }

        // Runtime'da başka bir sprite atandığında outline'ı güncel tut
        public void RefreshSprite()
        {
            if (outlineSr != null && mainSr != null)
                outlineSr.sprite = mainSr.sprite;
        }
    }
}
