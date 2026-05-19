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
        public event Action<ClassType> OnPrimaryClassSet;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        /// <summary> CharacterSelectScreen calls this at game start to set the primary class. </summary>
        public void SetPrimaryClass(ClassType type)
        {
            if (type == ClassType.None) return;
            PrimaryClass = type;
            ApplyPrimaryClassToPlayer(type);
            OnPrimaryClassSet?.Invoke(type);
            Debug.Log($"[PlayerClassManager] Primary class set: {type}");
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
                    AddIfMissing<ManaSystem>(player);
                    AddIfMissing<Elementalist_SkillController>(player, secondary: true);
                    break;
                case ClassType.Shadowblade:
                    AddIfMissing<EnergySystem>(player);
                    AddIfMissing<Shadowblade_SkillController>(player, secondary: true);
                    break;
                case ClassType.Ranger:
                    AddIfMissing<FocusSystem>(player);
                    AddIfMissing<Ranger_SkillController>(player, secondary: true);
                    break;
                case ClassType.Ronin:
                    AddIfMissing<TensionSystem>(player);
                    AddIfMissing<RoninController>(player, secondary: true);
                    break;
            }
        }

        private void ApplyPrimaryClassToPlayer(ClassType type)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            var warblade = player.GetComponent<Warblade_SkillController>();
            var elementalist = player.GetComponent<Elementalist_SkillController>();
            var shadowblade = player.GetComponent<Shadowblade_SkillController>();
            var ranger = player.GetComponent<Ranger_SkillController>();
            var ronin = player.GetComponent<RoninController>();

            if (warblade != null) warblade.enabled = type == ClassType.Warblade;
            if (elementalist != null) elementalist.enabled = type == ClassType.Elementalist;
            if (shadowblade != null) shadowblade.enabled = type == ClassType.Shadowblade;
            if (ranger != null) ranger.enabled = type == ClassType.Ranger;
            if (ronin != null) ronin.enabled = type == ClassType.Ronin;

            switch (type)
            {
                case ClassType.Warblade:
                    AddIfMissing<RageSystem>(player);
                    AddIfMissing<Warblade_SkillController>(player).enabled = true;
                    break;
                case ClassType.Elementalist:
                    AddIfMissing<ManaSystem>(player);
                    AddIfMissing<Elementalist_SkillController>(player).enabled = true;
                    break;
                case ClassType.Shadowblade:
                    AddIfMissing<EnergySystem>(player);
                    AddIfMissing<Shadowblade_SkillController>(player).enabled = true;
                    break;
                case ClassType.Ranger:
                    AddIfMissing<FocusSystem>(player);
                    AddIfMissing<Ranger_SkillController>(player).enabled = true;
                    break;
                case ClassType.Ronin:
                    AddIfMissing<TensionSystem>(player);
                    AddIfMissing<RoninController>(player).enabled = true;
                    break;
            }

            ApplyBasicAttackProfile(player, type);
            ApplyPrimaryClassVisual(player, type);
        }

        public void SwitchClass(ClassType type) => SetPrimaryClass(type);

        private static void ApplyPrimaryClassVisual(GameObject player, ClassType type)
        {
            var anim = player.GetComponentInChildren<Animator>();
            if (anim == null) return;

            var ctrl = Resources.Load<RuntimeAnimatorController>($"Characters/{type}/{type}");
            if (ctrl == null)
            {
                Debug.LogWarning($"[PlayerClassManager] Animator controller not found for {type}");
                return;
            }

            anim.runtimeAnimatorController = ctrl;
            anim.Rebind();
            anim.Update(0f);
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
            TryRemove<RoninController>(player);
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

        private static T AddIfMissing<T>(GameObject go, bool secondary = false) where T : Component
        {
            var existing = go.GetComponent<T>();
            if (existing != null) return existing;
            return go.AddComponent<T>();
        }

        private static void ApplyBasicAttackProfile(GameObject player, ClassType type)
        {
            var attack = player.GetComponent<PlayerAttack>();
            if (attack == null) return;

            BasicAttackProfile profile = Resources.Load<BasicAttackProfile>($"Combat/BasicAttack/BasicAttackProfile_{type}");

#if UNITY_EDITOR
            if (profile == null && type == ClassType.Ronin)
                profile = UnityEditor.AssetDatabase.LoadAssetAtPath<BasicAttackProfile>(
                    "Assets/Data/Combat/Profiles/Ronin_BasicAttackProfile.asset");
#endif

            if (profile == null)
            {
                Debug.LogWarning($"[PlayerClassManager] BasicAttackProfile not found for {type}");
                return;
            }

            attack.SetBasicAttackProfile(profile);
        }
    }
}
