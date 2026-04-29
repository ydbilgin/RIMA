using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Test: T → seçim ekranını aç | Shift+T → secondary sıfırla (tekrar test için)
    /// Boss AI entegrasyonu: GetComponent&lt;ClassSelectionTrigger&gt;().Trigger()
    /// </summary>
    public class ClassSelectionTrigger : MonoBehaviour
    {
        [SerializeField] private bool testMode = true;

        private void Update()
        {
            if (!testMode || Keyboard.current == null) return;

            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                bool shiftHeld = Keyboard.current.leftShiftKey.isPressed ||
                                 Keyboard.current.rightShiftKey.isPressed;

                if (shiftHeld)
                    ResetForTesting();
                else
                    Trigger();
            }
        }

        public void Trigger()
        {
            if (PlayerClassManager.Instance == null)
            {
                Debug.LogWarning("[ClassSelectionTrigger] PlayerClassManager yok!");
                return;
            }
            PlayerClassManager.Instance.TriggerClassSelection();
        }

        private void ResetForTesting()
        {
            if (PlayerClassManager.Instance == null) return;
            PlayerClassManager.Instance.ResetSecondaryForTesting();
            Debug.Log("[Test] Secondary class sıfırlandı — T ile tekrar aç.");
        }
    }
}
