using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// ESC ile açılan basit ayar paneli.
    /// Dash modu: Toggle (checkbox)
    ///   ✓ işaretli   → imlecin olduğu yöne dash (TowardsMouse)
    ///   işaretsiz    → karakterin baktığı yöne dash (FacingDirection)
    /// </summary>
    public class SettingsMenu : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panel;

        [Header("Dash Mode Toggle")]
        [SerializeField] private Toggle dashToMouseToggle;

        private PlayerController player;
        private InputAction escAction;
        private bool isOpen;

        private void Awake()
        {
            escAction = new InputAction("Escape", InputActionType.Button);
            escAction.AddBinding("<Keyboard>/escape");

            if (panel == null) panel = GameObject.Find("SettingsPanel");
            if (dashToMouseToggle == null)
            {
                var go = GameObject.Find("Toggle_DashToMouse");
                if (go != null) dashToMouseToggle = go.GetComponent<Toggle>();
            }
        }

        private void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.GetComponent<PlayerController>();

            if (dashToMouseToggle != null)
            {
                dashToMouseToggle.onValueChanged.AddListener(OnToggleChanged);
                dashToMouseToggle.SetIsOnWithoutNotify(player != null && player.DashMode == DashMode.TowardsMouse);
            }

            if (panel != null) panel.SetActive(false);
        }

        private void OnEnable()
        {
            escAction.Enable();
            escAction.performed += ToggleMenu;
        }

        private void OnDisable()
        {
            escAction.performed -= ToggleMenu;
            escAction.Disable();
        }

        private void ToggleMenu(InputAction.CallbackContext ctx)
        {
            isOpen = !isOpen;
            if (panel != null) panel.SetActive(isOpen);
            Time.timeScale = isOpen ? 0f : 1f;

            // Panel açılınca toggle'ı güncel değerle senkronize et
            if (isOpen && dashToMouseToggle != null && player != null)
                dashToMouseToggle.SetIsOnWithoutNotify(player.DashMode == DashMode.TowardsMouse);
        }

        private void OnToggleChanged(bool isOn)
        {
            if (player != null)
                player.DashMode = isOn ? DashMode.TowardsMouse : DashMode.FacingDirection;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
