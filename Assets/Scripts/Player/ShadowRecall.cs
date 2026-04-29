using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Shadowblade'e özel dash recall mekaniği.
    ///
    /// Akış:
    ///   1. Dash başlar → dash origin'de siluet spawn edilir.
    ///   2. recallWindow saniye içinde Left Shift'e basılırsa → oyuncu siluet pozisyonuna ışınlanır.
    ///   3. Süre dolarsa siluet solar ve kaybolur.
    ///
    /// Sadece Shadowblade Player prefabında olmalı.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class ShadowRecall : MonoBehaviour
    {
        [Header("Recall")]
        [SerializeField] private float recallWindow = 3f;
        [SerializeField] private GameObject silhouettePrefab;

        private bool             wasDashing;
        private Vector2          silhouettePos;
        private ShadowSilhouette activeSilhouette;
        private float            recallTimer;

        public bool RecallAvailable => activeSilhouette != null && recallTimer > 0f;

        private PlayerController controller;
        private SpriteRenderer   sr;
        private Rigidbody2D      rb;
        private InputAction      recallAction;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            sr         = GetComponentInChildren<SpriteRenderer>();
            rb         = GetComponent<Rigidbody2D>();

            recallAction = new InputAction("ShadowRecall", InputActionType.Button);
            recallAction.AddBinding("<Keyboard>/leftShift");
            recallAction.AddBinding("<Gamepad>/leftTrigger");
        }

        private void OnEnable()
        {
            recallAction.Enable();
            recallAction.performed += HandleRecall;
        }

        private void OnDisable()
        {
            recallAction.performed -= HandleRecall;
            recallAction.Disable();
        }

        private void Update()
        {
            bool isDashing = controller.IsDashing;

            if (isDashing && !wasDashing)
                BeginSilhouette();

            wasDashing = isDashing;

            if (recallTimer > 0f)
            {
                recallTimer -= Time.deltaTime;
                if (recallTimer <= 0f)
                    DestroySilhouette();
            }
        }

        private void BeginSilhouette()
        {
            DestroySilhouette();
            if (silhouettePrefab == null) return;

            silhouettePos = transform.position;
            var go = Instantiate(silhouettePrefab, (Vector3)(Vector2)silhouettePos, Quaternion.identity);
            activeSilhouette = go.GetComponent<ShadowSilhouette>();
            if (activeSilhouette != null)
                activeSilhouette.Init(sr != null ? sr.sprite : null, recallWindow);
            recallTimer = recallWindow;
        }

        private void HandleRecall(InputAction.CallbackContext ctx)
        {
            if (!RecallAvailable) return;
            StartCoroutine(DoRecall());
        }

        private IEnumerator DoRecall()
        {
            if (activeSilhouette != null) activeSilhouette.OnRecall();
            activeSilhouette = null;
            recallTimer      = 0f;

            if (sr != null)
            {
                sr.color = new Color(0.3f, 0.1f, 0.7f, 0.8f);
                yield return new WaitForSecondsRealtime(0.06f);
                sr.color = Color.white;
            }

            if (rb != null) rb.position = silhouettePos;
            else transform.position = (Vector3)(Vector2)silhouettePos;

            CameraShake.Instance?.Shake(0.08f, 0.08f);
        }

        private void DestroySilhouette()
        {
            if (activeSilhouette != null)
            {
                activeSilhouette.OnRecall();
                activeSilhouette = null;
            }
            recallTimer = 0f;
        }
    }
}
