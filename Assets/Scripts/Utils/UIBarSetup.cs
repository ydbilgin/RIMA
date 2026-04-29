using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// HUD bar Image'larına sprite atanmamışsa beyaz texture oluşturur.
    /// Her Image child'ına otomatik uygulanır.
    /// </summary>
    [ExecuteAlways]
    public class UIBarSetup : MonoBehaviour
    {
        private static Sprite _whiteSpr;

        private static Sprite WhiteSprite
        {
            get
            {
                if (_whiteSpr == null)
                {
                    var tex = new Texture2D(2, 2);
                    tex.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white });
                    tex.Apply();
                    tex.filterMode = FilterMode.Bilinear;
                    _whiteSpr = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 1f);
                }
                return _whiteSpr;
            }
        }

        private void Awake() => Apply();

        private void Apply()
        {
            foreach (var img in GetComponentsInChildren<Image>(true))
                if (img.sprite == null)
                    img.sprite = WhiteSprite;
        }
    }
}
