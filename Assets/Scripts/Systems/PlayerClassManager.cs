using System;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Primary ve secondary class'ı yönetir.
    /// Boss öldükten sonra TriggerClassSelection() çağrılır → ClassSelectionUI açılır.
    /// </summary>
    public class PlayerClassManager : MonoBehaviour
    {
        public static PlayerClassManager Instance { get; private set; }

        public ClassType PrimaryClass   { get; private set; } = ClassType.Warblade;
        public ClassType SecondaryClass { get; private set; } = ClassType.None;

        public event Action OnClassSelectionRequested;
        public event Action<ClassType> OnSecondaryClassSelected;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        /// <summary> Boss AI veya test butonu çağırır — class seçim ekranını açar. </summary>
        public void TriggerClassSelection()
        {
            if (SecondaryClass != ClassType.None) return;
            OnClassSelectionRequested?.Invoke();
        }

        /// <summary> ClassSelectionUI çağırır. </summary>
        public void SelectSecondaryClass(ClassType type)
        {
            if (SecondaryClass != ClassType.None || type == ClassType.None) return;

            SecondaryClass = type;

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) { Debug.LogWarning("[PlayerClassManager] Player bulunamadı!"); return; }

            AddSecondaryController(player, type);
            AddCrossClassPassive(player, type);

            var wbCtrl = player.GetComponent<Warblade_SkillController>();
            wbCtrl?.UnlockSecondarySlots();

            OnSecondaryClassSelected?.Invoke(type);
            Debug.Log($"[PlayerClassManager] Secondary class: {type}");
        }

        private void AddSecondaryController(GameObject player, ClassType type)
        {
            switch (type)
            {
                case ClassType.Elementalist:
                    AddIfMissing<Elementalist_SkillController>(player, secondary: true);
                    AddIfMissing<ManaSystem>(player);
                    break;
                case ClassType.Shadowblade:
                    AddIfMissing<Shadowblade_SkillController>(player, secondary: true);
                    AddIfMissing<EnergySystem>(player);
                    break;
                case ClassType.Ranger:
                    AddIfMissing<Ranger_SkillController>(player, secondary: true);
                    AddIfMissing<FocusSystem>(player);
                    break;
            }
        }

        private void AddCrossClassPassive(GameObject player, ClassType type)
        {
            switch (type)
            {
                case ClassType.Elementalist:
                    AddIfMissing<CrossClassPassive_WB_Elem>(player);
                    break;
                case ClassType.Shadowblade:
                    AddIfMissing<CrossClassPassive_WB_Shadow>(player);
                    break;
                case ClassType.Ranger:
                    AddIfMissing<CrossClassPassive_WB_Ranger>(player);
                    break;
            }
        }

        /// <summary> Sadece test modunda kullanılır — Shift+T ile secondary sıfırlanır. </summary>
        public void ResetSecondaryForTesting()
        {
            SecondaryClass = ClassType.None;

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            // Secondary controller ve pasif'leri kaldır
            TryRemove<Elementalist_SkillController>(player);
            TryRemove<Shadowblade_SkillController>(player);
            TryRemove<Ranger_SkillController>(player);
            TryRemove<CrossClassPassive_WB_Elem>(player);
            TryRemove<CrossClassPassive_WB_Shadow>(player);
            TryRemove<CrossClassPassive_WB_Ranger>(player);

            // Warblade slotlarını kilitle
            player.GetComponent<Warblade_SkillController>()?.LockSecondarySlots();
        }

        private static void TryRemove<T>(GameObject go) where T : Component
        {
            var c = go.GetComponent<T>();
            if (c != null) Destroy(c);
        }

        private static void AddIfMissing<T>(GameObject go, bool secondary = false) where T : Component
        {
            if (go.GetComponent<T>() != null) return;
            go.AddComponent<T>();
        }
    }
}
