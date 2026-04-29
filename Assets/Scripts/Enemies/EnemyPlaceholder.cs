using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Animator Controller'siz veya sprite'siz düşmanlara renkli placeholder atar.
    /// Sadece debug / prototip amaçlı — gerçek sprite gelince bu script silinecek.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyPlaceholder : MonoBehaviour
    {
        [SerializeField] private Color placeholderColor = Color.red;

        private void Awake()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr.sprite != null) return; // zaten sprite var, müdahale etme

            // 48x48 düz renk texture yarat (2x2 world unit — karakter boyutuna yakın)
            const int size = 48;
            var tex = new Texture2D(size, size);
            var pixels = new Color[size * size];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = placeholderColor;
            tex.SetPixels(pixels);
            tex.Apply();

            sr.sprite = Sprite.Create(tex,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f), 24f);  // 24 PPU → 2 world units
            sr.color = Color.white; // renk texture'da zaten var
        }
    }
}
