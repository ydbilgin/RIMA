using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RIMA
{
    public class DemoCompleteOverlay : MonoBehaviour
    {
        private Canvas _canvas;
        private Text _text;
        private Button _restartButton;

        public static void Show()
        {
            if (FindFirstObjectByType<DemoCompleteOverlay>() != null) return;

            GameObject go = new GameObject("DemoCompleteOverlay");
            go.AddComponent<DemoCompleteOverlay>().Build();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void HookDemoComplete()
        {
            RoomLoader.OnDemoComplete -= Show;
            RoomLoader.OnDemoComplete += Show;
        }

        private void Build()
        {
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 200;
            gameObject.AddComponent<GraphicRaycaster>();

            GameObject bg = new GameObject("BG");
            bg.transform.SetParent(transform, false);
            Image bgImg = bg.AddComponent<Image>();
            bgImg.color = new Color(0f, 0f, 0f, 0.85f);
            RectTransform bgRT = bg.GetComponent<RectTransform>();
            bgRT.anchorMin = Vector2.zero;
            bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = Vector2.zero;
            bgRT.offsetMax = Vector2.zero;

            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(transform, false);
            _text = textGO.AddComponent<Text>();
            _text.text = "DEMO COMPLETE\n\n5 oda, ~10 dakika";
            _text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _text.fontSize = 48;
            _text.alignment = TextAnchor.MiddleCenter;
            _text.color = new Color(0f, 1f, 0.8f, 1f);
            RectTransform textRT = textGO.GetComponent<RectTransform>();
            textRT.anchorMin = new Vector2(0.2f, 0.4f);
            textRT.anchorMax = new Vector2(0.8f, 0.7f);
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;

            GameObject btnGO = new GameObject("RestartButton");
            btnGO.transform.SetParent(transform, false);
            Image btnImg = btnGO.AddComponent<Image>();
            btnImg.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            _restartButton = btnGO.AddComponent<Button>();
            _restartButton.onClick.AddListener(Restart);
            RectTransform btnRT = btnGO.GetComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0.4f, 0.25f);
            btnRT.anchorMax = new Vector2(0.6f, 0.32f);
            btnRT.offsetMin = Vector2.zero;
            btnRT.offsetMax = Vector2.zero;

            GameObject btnText = new GameObject("BtnText");
            btnText.transform.SetParent(btnGO.transform, false);
            Text btnTxt = btnText.AddComponent<Text>();
            btnTxt.text = "RESTART (Room 1)";
            btnTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnTxt.fontSize = 18;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.color = Color.white;
            RectTransform btnTextRT = btnText.GetComponent<RectTransform>();
            btnTextRT.anchorMin = Vector2.zero;
            btnTextRT.anchorMax = Vector2.one;
            btnTextRT.offsetMin = Vector2.zero;
            btnTextRT.offsetMax = Vector2.zero;
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
