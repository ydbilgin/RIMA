using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    /// <summary>
    /// Draft akışını yönetir.
    /// - Pasifler: slot almaz, max 3 seviye, aynı pasif tekrar = upgrade.
    /// - Aktifler: 4 primary + 2 secondary slot; slot dolunca replace modu.
    /// - Değiştirme ücretsiz.
    /// </summary>
    public class DraftManager : MonoBehaviour
    {
        public static DraftManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private SkillOfferGenerator offerGenerator;
        [SerializeField] private SkillOfferUI         offerUI;

        // ── Durum ────────────────────────────────────────────────
        private readonly List<SkillData>       currentActiveSkills = new List<SkillData>();
        private readonly Dictionary<string,int> passiveLevels       = new Dictionary<string,int>();

        private SkillData pendingSkill;
        private int       pendingSlot;

        /// <summary>RoomClearedSequence'in WaitUntil için polling yapar.</summary>
        public bool IsDraftActive { get; private set; }

        public UnityEvent<SkillData> OnSkillPicked = new UnityEvent<SkillData>();

        /// <summary>
        /// Forge milestone tetiklenince ateşlenir.
        /// arg: forge numarası (1 = Oda 4 ekol seç, 2 = Oda 8 yükselt).
        /// AttackForgeUI bu event'e abone olur.
        /// </summary>
        public UnityEvent<int> OnForgeRoom = new UnityEvent<int>();

        // Milestone tanımları
        private const int ForgeRoom1 = 4;
        private const int ForgeRoom2 = 8;

        private Warblade_SkillController skillController;
        private GameObject               player;

        // ── Static query (SkillOfferGenerator erişim için) ──────
        public static int GetPassiveLevel(string skillName)
        {
            if (Instance == null) return 0;
            Instance.passiveLevels.TryGetValue(skillName, out int lvl);
            return lvl;
        }

        // ── Yaşam döngüsü ────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                skillController = player.GetComponent<Warblade_SkillController>();

            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnSecondaryClassSelected += _ =>
                {
                    offerGenerator.nextDraftIsUnlock = true;
                    StartCoroutine(ShowDraftDelayed(2f));
                };
        }

        private IEnumerator ShowDraftDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            ShowDraft();
        }

        // ── Ana çağrılar ─────────────────────────────────────────

        /// <summary> Oda temizlendikten veya boss yenildikten sonra çağrılır. </summary>
        public void ShowDraft()
        {
            EnsureDependencies();
            if (offerGenerator == null || offerUI == null)
            {
                Debug.LogWarning("[DraftManager] offerGenerator veya offerUI bulunamadı!");
                IsDraftActive = false;
                return;
            }

            IsDraftActive = true;

            int room = LegacyRuntimeRoomManager.Instance?.CurrentRoom ?? 1;

            // ── Forge milestone: Forge UI kendi akışını yönetir ───
            if (room == ForgeRoom1)
            {
                Debug.Log("[DraftManager] Oda 4 — Attack Forge #1");
                OnForgeRoom.Invoke(1);
                IsDraftActive = false; // Forge kendi UI'ını yönetir
                return;
            }
            if (room == ForgeRoom2)
            {
                Debug.Log("[DraftManager] Oda 8 — Attack Forge #2");
                OnForgeRoom.Invoke(2);
                IsDraftActive = false;
                return;
            }

            // ── Normal draft ──────────────────────────────────────
            var primary   = PlayerClassManager.Instance?.PrimaryClass   ?? ClassType.Warblade;
            var secondary = PlayerClassManager.Instance?.SecondaryClass ?? ClassType.None;
            int maxSlots  = skillController?.SlotCount ?? 4;

            var offers = offerGenerator.GenerateOffers(
                currentActiveSkills, primary, secondary, maxSlots, room);

            if (offers == null || offers.Count == 0)
            {
                // Skill havuzu boş — fallback olarak altın/iyileşme sun
                offers = new System.Collections.Generic.List<RewardOffer>
                {
                    RewardOffer.FromGold(40 + UnityEngine.Random.Range(0, 30)),
                    RewardOffer.FromHeal(20),
                    RewardOffer.FromGold(60 + UnityEngine.Random.Range(0, 20))
                };
            }

            offerUI.Show(offers, OnOfferSelected, room);
        }

        public void HideDraft()
        {
            offerUI?.Hide();
            IsDraftActive = false;
        }

        /// <summary> Sandıktan gelen tek skill için doğrudan draft aç. </summary>
        public void ShowDraftWithSkill(SkillData skill)
        {
            if (skill == null || offerUI == null) return;
            var offers = new System.Collections.Generic.List<RewardOffer> { RewardOffer.FromSkill(skill) };
            offerUI.Show(offers, OnOfferSelected);
        }

        // ── Seçim akışı ──────────────────────────────────────────

        private void OnOfferSelected(RewardOffer chosen, int _ignored)
        {
            if (chosen == null) return;

            // Gold reward
            if (chosen.type == RewardType.Gold)
            {
                PlayerEconomy.Instance?.AddGold(chosen.goldAmount);
                offerUI.Hide();
                IsDraftActive = false;
                return;
            }

            // Heal reward
            if (chosen.type == RewardType.Heal)
            {
                var hp = player?.GetComponent<Health>();
                if (hp != null) hp.Heal(Mathf.RoundToInt(hp.MaxHP * chosen.healPercent / 100f));
                offerUI.Hide();
                IsDraftActive = false;
                return;
            }

            // Skill reward
            if (chosen.skill == null) return;
            if (chosen.skill.isPassive) { HandlePassivePick(chosen.skill); return; }
            HandleActivePick(chosen.skill);
        }

        // ── Pasif ────────────────────────────────────────────────

        private void HandlePassivePick(SkillData passive)
        {
            int lvl = GetPassiveLevel(passive.skillName);
            if (lvl >= PassiveBase.MaxLevel) return; // garanti filtreli gelir ama çift kontrol

            passiveLevels[passive.skillName] = lvl + 1;

            // MonoBehaviour pasif varsa LevelUp çağır
            if (passive.skillType != null && player != null)
            {
                var comp = player.GetComponent(passive.skillType) as PassiveBase
                        ?? player.GetComponentInChildren(passive.skillType) as PassiveBase;
                comp?.LevelUp();
            }

            Debug.Log($"[Draft] Pasif '{passive.skillName}' → Lv {lvl + 1}");
            FinishPick(passive);
        }

        // ── Aktif ────────────────────────────────────────────────

        private void HandleActivePick(SkillData skill)
        {
            int maxSlots = skillController?.SlotCount ?? 4;

            if (currentActiveSkills.Count >= maxSlots)
            {
                // Slot dolu → replace modu
                pendingSkill = skill;
                offerUI.ShowReplaceMode(currentActiveSkills, skill, OnReplaceChosen, OnSkipChosen);
                return;
            }

            int slot = IsSecondarySkill(skill) ? FindNextSecondarySlot() : FindNextPrimarySlot();
            AssignActive(skill, slot);
            FinishPick(skill);
        }

        private void OnReplaceChosen(SkillData toReplace)
        {
            if (pendingSkill == null || toReplace == null) return;

            // Eski skillin slot indeksini bul
            int replaceSlot = FindSlotOf(toReplace);
            if (replaceSlot < 0) replaceSlot = FindNextPrimarySlot();

            currentActiveSkills.Remove(toReplace);
            AssignActive(pendingSkill, replaceSlot);

            var picked = pendingSkill;
            pendingSkill = null;
            FinishPick(picked);
        }

        private void OnSkipChosen()
        {
            pendingSkill = null;
            offerUI.Hide();
            IsDraftActive = false;
        }

        private void EnsureDependencies()
        {
            if (offerUI == null)
            {
                offerUI = FindAnyObjectByType<SkillOfferUI>();
                if (offerUI == null)
                {
                    var go = new GameObject("SkillOfferUI_Auto");
                    offerUI = go.AddComponent<SkillOfferUI>();
                    Debug.Log("[DraftManager] SkillOfferUI otomatik oluşturuldu.");
                }
            }
            if (offerGenerator == null)
            {
                offerGenerator = FindAnyObjectByType<SkillOfferGenerator>();
                if (offerGenerator == null)
                {
                    var go = new GameObject("SkillOfferGenerator_Auto");
                    offerGenerator = go.AddComponent<SkillOfferGenerator>();
                    Debug.Log("[DraftManager] SkillOfferGenerator otomatik oluşturuldu.");
                }
            }
        }

        // ── Yardımcılar ──────────────────────────────────────────

        private void AssignActive(SkillData skill, int slot)
        {
            if (skillController != null && skill.skillType != null)
            {
                var comp = skillController.GetComponent(skill.skillType) as SkillBase
                        ?? skillController.GetComponentInChildren(skill.skillType) as SkillBase;
                if (comp != null)
                    skillController.SetSlot(slot, comp);
                else
                    Debug.LogWarning($"[Draft] '{skill.skillName}' bileşeni Player'da bulunamadı.");
            }
            if (!currentActiveSkills.Contains(skill))
                currentActiveSkills.Add(skill);
        }

        private void FinishPick(SkillData skill)
        {
            offerUI.Hide();
            IsDraftActive = false;
            OnSkillPicked.Invoke(skill);
        }

        private bool IsSecondarySkill(SkillData s)
        {
            var primary = PlayerClassManager.Instance?.PrimaryClass ?? ClassType.Warblade;
            return s.classType != ClassType.None && s.classType != primary;
        }

        private int FindNextPrimarySlot()
        {
            if (skillController == null) return 0;
            for (int i = 0; i < 4; i++)
                if (skillController.GetSlot(i) == null) return i;
            return 0;
        }

        private int FindNextSecondarySlot()
        {
            if (skillController == null) return 4;
            for (int i = 4; i < 6; i++)
                if (skillController.GetSlot(i) == null) return i;
            return 4;
        }

        private int FindSlotOf(SkillData sd)
        {
            if (skillController == null || sd.skillType == null) return -1;
            var comp = skillController.GetComponent(sd.skillType)
                    ?? skillController.GetComponentInChildren(sd.skillType);
            if (comp == null) return -1;
            var slots = skillController.GetAllSlots();
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i] == comp) return i;
            return -1;
        }

        public bool HasSkill(SkillData s) => currentActiveSkills.Contains(s);
    }
}
