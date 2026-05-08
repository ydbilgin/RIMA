using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Hades-style reward orb — oda temizlenince ortaya düşer.
    /// Oyuncu yaklaştığında [G] istemi belirir, basınca skill draft açılır.
    /// Draft kapanınca orb yok olur.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class RewardPickup : MonoBehaviour
    {
        [SerializeField] private float interactRadius = 2.5f;
        private const Key InteractKey = Key.G;
        [SerializeField] private float bobAmplitude   = 0.10f;
        [SerializeField] private float bobSpeed       = 2.2f;
        [SerializeField] private float glowSpeed      = 3.0f;

        private Transform  playerTransform;
        private GameObject promptGO;
        private SpriteRenderer sr;
        private bool       interacted;
        private Vector3    startPos;
        private Color      baseColor;
        private bool       playerSearchDone;

        /// <summary>RoomClearedSequence reminder'ı için: oyuncu ödülünü aldı mı?</summary>
        public bool WasCollected => interacted;

        private void Start()
        {
            startPos        = transform.position;
            sr              = GetComponent<SpriteRenderer>();
            baseColor       = sr != null ? sr.color : Color.white;
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            playerSearchDone = playerTransform != null;
            BuildPromptLabel();
        }

        private void Update()
        {
            if (interacted) return;

            // Bob + glow animasyonu (sadece alpha pulse, RGB korunur)
            transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            if (sr != null)
            {
                float alpha = 0.60f + Mathf.Sin(Time.time * glowSpeed) * 0.40f;
                sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            }

            if (playerTransform == null)
            {
                if (playerSearchDone) return; // already tried, give up
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
                playerSearchDone = true;
                if (playerTransform == null) return;
            }

            float dist = Vector2.Distance(transform.position, playerTransform.position);
            bool inRange = dist <= interactRadius && IsRoomClearInteractionAllowed();

            if (promptGO != null) promptGO.SetActive(inRange);

            if (inRange)
            {
                if (HUDController.Instance != null)
                    HUDController.Instance.SetInteractionPrompt("Ödül Seç");
            }
            else
            {
                if (HUDController.Instance != null)
                    HUDController.Instance.HideInteractionPrompt();
            }



            if (inRange && Keyboard.current != null && Keyboard.current[InteractKey].wasPressedThisFrame)
            {
                Debug.Log("[RewardPickup] G pressed! Starting interact...");
                StartCoroutine(DoInteract());
            }
        }

        private static bool IsRoomClearInteractionAllowed()
        {
            return LegacyRuntimeRoomManager.Instance == null || LegacyRuntimeRoomManager.Instance.IsRoomCleared;
        }

        private IEnumerator DoInteract()
        {
            interacted = true;
            if (promptGO != null) promptGO.SetActive(false);
            if (HUDController.Instance != null) HUDController.Instance.HideInteractionPrompt();

            if (DraftManager.Instance == null)
            {
                // Draft yok — kapıları aç ve yok ol
                if (LegacyRuntimeRoomManager.Instance != null)
                    LegacyRuntimeRoomManager.Instance.OpenDoorsAfterReward();
                Destroy(gameObject);
                yield break;
            }

            DraftManager.Instance.ShowDraft();
            yield return new WaitUntil(() =>
                DraftManager.Instance == null || !DraftManager.Instance.IsDraftActive);

            // Draft bitti — kapıları aç
            if (LegacyRuntimeRoomManager.Instance != null)
                LegacyRuntimeRoomManager.Instance.OpenDoorsAfterReward();

            Destroy(gameObject);
        }

        // ── World-space prompt "[ G ] Ödül Seç" ─────────────────────

        private void OnDestroy()
        {
            if (HUDController.Instance != null)
                HUDController.Instance.HideInteractionPrompt();
        }

        private void BuildPromptLabel()
        {
            var cvGO = new GameObject("PromptCanvas");
            cvGO.transform.SetParent(transform, false);
            cvGO.transform.localPosition = new Vector3(0f, 0.75f, 0f);

            var cv = cvGO.AddComponent<Canvas>();
            cv.renderMode = RenderMode.WorldSpace;
            cv.sortingOrder = 20;
            var rt = cvGO.GetComponent<RectTransform>();
            rt.sizeDelta   = new Vector2(160f, 30f);
            rt.localScale  = Vector3.one * 0.012f;

            // Arka plan
            var bgGO  = new GameObject("BG");
            bgGO.transform.SetParent(cvGO.transform, false);
            var bgImg = bgGO.AddComponent<UnityEngine.UI.Image>();
            bgImg.color = new Color(0f, 0f, 0f, 0.75f);
            var bgRT  = bgGO.GetComponent<RectTransform>();
            bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;

            // Metin
            var txtGO  = new GameObject("Label");
            txtGO.transform.SetParent(cvGO.transform, false);
            var tmp    = txtGO.AddComponent<TextMeshProUGUI>();
            tmp.text   = "[ G ]  Ödül Seç";
            tmp.fontSize = 18;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color  = new Color(0.55f, 0.88f, 1f);
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
