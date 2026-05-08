using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Death screen + restart loop.
    /// Listens to Player's Health.OnDeath → shows death panel → restart options.
    ///
    /// TODO (tasarım güncellenince):
    ///   - "YOU DIED" text → RIMA'ya özgü Türkçe/fantezi mesaj veya class-specific quip
    ///   - Sade dark overlay → full ekran ölüm sahnesi (karakter yere düşme, sis efekti)
    ///   - Sadece Room/Kill sayısı → detaylı run stats (en yüksek combo, toplam hasar vs.)
    ///   - "TRY AGAIN" → class seçim ekranına dönüş (Faz 2+)
    ///
    /// Setup:
    ///   1. Create UI Canvas → Panel "DeathScreen" (full screen, dark overlay)
    ///   2. Child Text "DeathTitle" (TMP) → "YOU DIED"
    ///   3. Child Text "DeathStats" (TMP) → room count, kills
    ///   4. Child Button "RestartButton" → "Try Again"
    ///   5. Attach this script to the Canvas or a GameManager object
    ///   6. Assign references in Inspector OR let auto-find by name
    /// </summary>
    public class DeathScreenManager : MonoBehaviour
    {
        [Header("UI References (auto-found if null)")]
        [SerializeField] private GameObject deathPanel;
        [SerializeField] private TextMeshProUGUI deathTitle;
        [SerializeField] private TextMeshProUGUI deathStats;
        [SerializeField] private Button restartButton;

        [Header("Settings")]
        [SerializeField] private float fadeInDuration = 0.8f;
        [SerializeField] private float slowMoScale = 0.15f;
        [SerializeField] private float slowMoDuration = 1.5f;

        private Health playerHealth;
        private CanvasGroup canvasGroup;
        private int killCount;
        private bool isDead;

        private void Awake()
        {
            // Auto-find UI elements
            if (deathPanel == null)
            {
                var go = GameObject.Find("DeathScreen");
                if (go != null) deathPanel = go;
            }

            if (deathPanel != null)
            {
                canvasGroup = deathPanel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = deathPanel.AddComponent<CanvasGroup>();

                deathPanel.SetActive(false);
            }

            if (deathTitle == null)
            {
                var go = GameObject.Find("DeathTitle");
                if (go != null) deathTitle = go.GetComponent<TextMeshProUGUI>();
            }

            if (deathStats == null)
            {
                var go = GameObject.Find("DeathStats");
                if (go != null) deathStats = go.GetComponent<TextMeshProUGUI>();
            }

            if (restartButton == null)
            {
                var go = GameObject.Find("RestartButton");
                if (go != null) restartButton = go.GetComponent<Button>();
            }

            if (restartButton != null)
                restartButton.onClick.AddListener(RestartRun);
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                    playerHealth.OnDeath.AddListener(OnPlayerDied);
            }
        }

        private void Update()
        {
            // Quick restart with R key while dead (works even without UI panel)
            if (isDead)
            {
                if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                    RestartRun();
            }
        }

        /// <summary>Track kills for death stats.</summary>
        public void RegisterKill()
        {
            killCount++;
        }

        private void OnPlayerDied()
        {
            if (isDead) return; // prevent double-fire
            isDead = true;
            Debug.Log("[DeathScreenManager] Player died. Press R to restart.");
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            // 1. Slowmo
            Time.timeScale = slowMoScale;

            yield return new WaitForSecondsRealtime(slowMoDuration);

            // 2. Freeze
            Time.timeScale = 0f;

            // 3. Show death panel with fade
            if (deathPanel != null)
            {
                deathPanel.SetActive(true);

                // Stats
                int roomNum = LegacyRuntimeRoomManager.Instance != null
                    ? LegacyRuntimeRoomManager.Instance.CurrentRoom
                    : 0;

                if (deathTitle != null)
                    deathTitle.text = "YOU DIED";

                if (deathStats != null)
                    deathStats.text = $"Room: {roomNum}\nKills: {killCount}";

                // Fade in
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f;
                    float elapsed = 0f;
                    while (elapsed < fadeInDuration)
                    {
                        elapsed += Time.unscaledDeltaTime;
                        canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
                        yield return null;
                    }
                    canvasGroup.alpha = 1f;
                }
            }

            // Disable player input
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var ctrl = player.GetComponent<PlayerController>();
                if (ctrl != null) ctrl.enabled = false;
            }
        }

        public void RestartRun()
        {
            Time.timeScale = 1f;
            if (Application.isPlaying)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
