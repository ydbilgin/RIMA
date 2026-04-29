using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Sprite atanmamış SpriteRenderer'a çalışma zamanında renkli şekil oluşturur.
    /// Gerçek sprite gelince bu component'i sil.
    /// SR.color'dan renk alır — beyaz ise kendi serialized color'unu kullanır.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlaceholderSprite : MonoBehaviour
    {
        public enum Shape { Square, Circle, Diamond }

        [SerializeField] private Color color = Color.white;
        [SerializeField] private int pixelSize = 48;
        [SerializeField] private Shape shape = Shape.Square;

        private Texture2D _tex; // prevent GC

        private void Awake()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr.sprite != null) return; // Gerçek sprite varsa dokunma

            // Renk: SR.color beyaz değilse onu kullan, yoksa serialized color
            Color useColor = (sr.color != Color.white) ? sr.color : color;

            _tex = new Texture2D(pixelSize, pixelSize);
            _tex.filterMode = FilterMode.Point;
            var pixels = new Color[pixelSize * pixelSize];
            var clear  = new Color(0, 0, 0, 0);
            int half   = pixelSize / 2;

            for (int y = 0; y < pixelSize; y++)
            {
                for (int x = 0; x < pixelSize; x++)
                {
                    bool fill = false;
                    switch (shape)
                    {
                        case Shape.Square:
                            fill = true;
                            break;
                        case Shape.Circle:
                            float dx = x - half + 0.5f;
                            float dy = y - half + 0.5f;
                            fill = (dx * dx + dy * dy) <= (half * half);
                            break;
                        case Shape.Diamond:
                            fill = (Mathf.Abs(x - half) + Mathf.Abs(y - half)) <= half;
                            break;
                    }
                    pixels[y * pixelSize + x] = fill ? useColor : clear;
                }
            }

            _tex.SetPixels(pixels);
            _tex.Apply();

            sr.sprite = Sprite.Create(
                _tex,
                new Rect(0, 0, pixelSize, pixelSize),
                new Vector2(0.5f, 0.5f),
                48f
            );
            sr.color = Color.white; // renk zaten texture'da

            // Unlit material kullan — URP 2D lit material sorun çıkarıyor
            var defaultMat = new Material(Shader.Find("Sprites/Default"));
            if (defaultMat != null)
                sr.sharedMaterial = defaultMat;
        }
    }
}
