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
    /// Attack/skill aim modu: Toggle (checkbox)
    ///   ✓ işaretli   → imlecin olduğu yöne saldırı/skill
    ///   işaretsiz    → karakterin son baktığı yöne saldırı/skill
    /// </summary>
    public class SettingsMenu : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panel;

        [Header("Dash Mode Toggle")]
        [SerializeField] private Toggle dashToMouseToggle;

        [Header("Attack Aim Toggle")]
        [SerializeField] private Toggle attackAimToMouseToggle;

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

            if (attackAimToMouseToggle == null)
            {
                var go = GameObject.Find("Toggle_AttackAimToMouse");
                if (go != null) attackAimToMouseToggle = go.GetComponent<Toggle>();
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

            if (attackAimToMouseToggle != null)
            {
                attackAimToMouseToggle.onValueChanged.AddListener(OnAttackAimToggleChanged);
                attackAimToMouseToggle.SetIsOnWithoutNotify(player != null && player.AttackAimMode == CombatAimMode.TowardsMouse);
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
            if (isOpen && attackAimToMouseToggle != null && player != null)
                attackAimToMouseToggle.SetIsOnWithoutNotify(player.AttackAimMode == CombatAimMode.TowardsMouse);
        }

        private void OnToggleChanged(bool isOn)
        {
            if (player != null)
            {
                player.DashMode = isOn ? DashMode.TowardsMouse : DashMode.FacingDirection;
                PlayerPrefs.Save();
            }
        }

        private void OnAttackAimToggleChanged(bool isOn)
        {
            if (player != null)
            {
                player.AttackAimMode = isOn ? CombatAimMode.TowardsMouse : CombatAimMode.CharacterFacing;
                PlayerPrefs.Save();
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
