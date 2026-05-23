using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Harita parçası — oda temizlenince düşer.
    /// Oyuncu yaklaşınca [G] prompt'u belirir, G'ye basınca toplanır.
    /// Toplama sonrası DungeonGraph.RevealAhead çağrılır.
    ///
    /// Setup:
    ///   - SpriteRenderer ekle (placeholder veya gerçek sprite)
    ///   - CircleCollider2D (isTrigger = true)   ← proximity detect
    ///   - Bu script
    ///   - Prefab olarak kaydet → RuntimeRoomManager.mapFragmentPrefab'a ata
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class MapFragment : MonoBehaviour
    {
        [SerializeField] private float interactRadius = 3.0f;
        private const Key InteractKey = Key.G;
        [SerializeField] private float bobAmplitude   = 0.12f;
        [SerializeField] private float bobSpeed       = 2.5f;
        [SerializeField] private float glowPulseSpeed = 3f;

        /// <summary>RuntimeRoomManager tarafından set edilir: 1 = 1 adım, 2 = 2 adım.</summary>
        [HideInInspector] public int revealSteps = 1;

        public static event System.Action OnFragmentCollected;

        private bool collected;
        private Vector3 startPos;
        private SpriteRenderer sr;
        private Color baseColor;
        private Transform playerTransform;
        private GameObject promptGO;
        private bool playerSearchDone;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
            if (col.radius < 0.3f) col.radius = 0.4f;
        }

        private void Start()
        {
            startPos = transform.position;
            baseColor = sr != null ? sr.color : Color.white;
            AcquirePlayer();
            BuildPromptLabel();
            if (!playerSearchDone) Invoke(nameof(LateAcquirePlayer), 0.5f);
        }

        private void LateAcquirePlayer()
        {
            AcquirePlayer();
            if (!playerSearchDone) Invoke(nameof(LateAcquirePlayer), 0.5f);
        }

        private void AcquirePlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            playerTransform = player.transform;
            playerSearchDone = true;
        }

        private void Update()
        {
            if (collected) return;

            // Bob + glow (sadece alpha pulse, RGB korunur)
            transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            if (sr != null)
            {
                float alpha = 0.6f + Mathf.Sin(Time.time * glowPulseSpeed) * 0.4f;
                sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            }

            if (playerTransform == null)
            {
                return;
            }

            float dist = Vector2.Distance(transform.position, playerTransform.position);
            bool inRange = dist <= interactRadius && IsRoomClearInteractionAllowed();
            if (promptGO != null) promptGO.SetActive(inRange);

            if (inRange)
            {
                if (HUDController.Instance != null)
                    HUDController.Instance.SetInteractionPrompt("Harita");
            }
            else
            {
                if (HUDController.Instance != null)
                    HUDController.Instance.HideInteractionPrompt();
            }



            if (inRange && Keyboard.current != null && Keyboard.current[InteractKey].wasPressedThisFrame)
            {
                Debug.Log("[MapFragment] G pressed! Collecting...");
                Collect();
            }
        }

        private static bool IsRoomClearInteractionAllowed()
        {
            return RuntimeRoomManager.Instance == null || RuntimeRoomManager.Instance.IsRoomCleared;
        }

        private void Collect()
        {
            collected = true;
            if (promptGO != null) promptGO.SetActive(false);
            if (HUDController.Instance != null) HUDController.Instance.HideInteractionPrompt();

            DungeonGraph.Instance?.RevealAhead(revealSteps);
            DungeonMapUI.Instance?.RefreshMap();
            OnFragmentCollected?.Invoke();
            StartCoroutine(CollectSequence());
        }

        private IEnumerator CollectSequence()
        {
            // Brief scale pop then destroy
            float t = 0f;
            Vector3 baseScale = transform.localScale;
            while (t < 0.18f)
            {
                t += Time.deltaTime;
                float s = 1f + Mathf.Sin(t / 0.18f * Mathf.PI) * 0.5f;
                transform.localScale = baseScale * s;
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (HUDController.Instance != null)
                HUDController.Instance.HideInteractionPrompt();
        }

        // ── World-space prompt "[ G ] Harita" ─────────────────────

        private void BuildPromptLabel()
        {
            var cvGO = new GameObject("PromptCanvas");
            cvGO.transform.SetParent(transform, false);
            cvGO.transform.localPosition = new Vector3(0f, 0.65f, 0f);

            var cv = cvGO.AddComponent<Canvas>();
            cv.renderMode = RenderMode.WorldSpace;
            cv.sortingOrder = 20;
            var rt = cvGO.GetComponent<RectTransform>();
            rt.sizeDelta   = new Vector2(120f, 26f);
            rt.localScale  = Vector3.one * 0.012f;

            // Arka plan
            var bgGO  = new GameObject("BG");
            bgGO.transform.SetParent(cvGO.transform, false);
            var bgImg = bgGO.AddComponent<UnityEngine.UI.Image>();
            bgImg.color = new Color(0f, 0f, 0f, 0.70f);
            var bgRT  = bgGO.GetComponent<RectTransform>();
            bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;

            // Metin
            var txtGO  = new GameObject("Label");
            txtGO.transform.SetParent(cvGO.transform, false);
            var tmp    = txtGO.AddComponent<TextMeshProUGUI>();
            tmp.text   = "[ G ]  Harita";
            tmp.fontSize = 16;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color  = new Color(0.6f, 1f, 0.7f);
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
            var txtRT  = txtGO.GetComponent<RectTransform>();
            txtRT.anchorMin = Vector2.zero; txtRT.anchorMax = Vector2.one;
            txtRT.offsetMin = txtRT.offsetMax = Vector2.zero;

            promptGO = cvGO;
            promptGO.SetActive(false);
        }
    }
}

