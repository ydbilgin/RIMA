using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade dash başlangıcında spawn edilen siluet.
    /// recallWindow saniye içinde soluklaşır ve silinir.
    /// ShadowRecall bu bileşeni kontrol eder.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShadowSilhouette : MonoBehaviour
    {
        private SpriteRenderer sr;
        private float lifetime;
        private float elapsed;
        private bool recalled;

        public void Init(Sprite sprite, float window)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite  = sprite;
            sr.color   = new Color(0.3f, 0.1f, 0.7f, 0.65f);
            sr.sortingLayerName = "Default";
            sr.sortingOrder     = -1;
            lifetime = window;
        }

        private void Update()
        {
            if (recalled) return;
            elapsed += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / lifetime);
            Color c = sr.color;
            c.a = 0.65f * t;
            sr.color = c;
            if (elapsed >= lifetime)
                Destroy(gameObject);
        }

        public void OnRecall()
        {
            recalled = true;
            Destroy(gameObject);
        }
    }
}
