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
    [System.Obsolete("Retired (S6 BLOCK C4) — UIManager owns ESC + timeScale and drives UI/SettingsMenuUI, " +
        "which now carries the Aim/Dash toggles. This was a duplicate ESC/timeScale owner (Bug-2).")]
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
            // Retired (BLOCK C4): disable self so this legacy menu can't register a second ESC handler
            // or fight timeScale if it still lingers in a scene. UIManager + UI/SettingsMenuUI own this now.
            enabled = false;
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
            if (escAction == null) return; // retired — see Awake
            escAction.Enable();
            escAction.performed += ToggleMenu;
        }

        private void OnDisable()
        {
            if (escAction == null) return;
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
