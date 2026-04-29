using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Cross-class skill aktive edilince Player üzerinde beliren hayalet sprite.
    /// Prefab'a bu script + SpriteRenderer eklenir.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CrossClassGhostEffect : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 0.6f;
        [SerializeField] private float startAlpha = 0.55f;
        [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.3f, 0);

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void Play(Color classColor)
        {
            transform.position += spawnOffset;

            // Renk + additive blend
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.SetFloat("_Mode", 1); // Additive-ish
            sr.material = mat;

            classColor.a = startAlpha;
            sr.color = classColor;

            // Player'ın sprite'ını kopyala
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                var playerSR = playerGO.GetComponent<SpriteRenderer>();
                if (playerSR != null) sr.sprite = playerSR.sprite;
            }

            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            float elapsed = 0f;
            Color startColor = sr.color;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                // Yukarı doğru hafif yüksel + soluk
                transform.position += Vector3.up * Time.deltaTime * 0.4f;
                startColor.a = Mathf.Lerp(startAlpha, 0f, t);
                sr.color = startColor;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
