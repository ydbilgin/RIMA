using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string characterSelectScene = "CharacterSelect";
        [SerializeField] private TMP_Text settingsTooltip;

        private Coroutine tooltipRoutine;

        public void OnStartClicked() => SceneManager.LoadScene(characterSelectScene);

        public void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnSettingsClicked()
        {
            Debug.Log("[MainMenu] Ayarlar yakında.");
            if (settingsTooltip == null) return;
            if (tooltipRoutine != null) StopCoroutine(tooltipRoutine);
            tooltipRoutine = StartCoroutine(ShowSettingsTooltip());
        }

        private IEnumerator ShowSettingsTooltip()
        {
            settingsTooltip.gameObject.SetActive(true);
            settingsTooltip.text = "Yakında";
            yield return new WaitForSecondsRealtime(1.4f);
            if (settingsTooltip != null) settingsTooltip.gameObject.SetActive(false);
            tooltipRoutine = null;
        }
    }
}
