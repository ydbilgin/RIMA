using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RIMA.Systems.Map;
using RIMA.Environment;

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

        /// <summary>
        /// Secondary class seçildiğinde ShowDraftDelayed coroutine başlatılır ama
        /// IsDraftActive henüz false'dur (2 sn gecikme var). Bu flag o pencereyi kapatır:
        /// coroutine başlayınca true, ShowDraft() çalışmaya başlayınca false.
        /// RoomClearSequence hem IsDraftPending hem IsDraftActive'i bekler.
        /// </summary>
        public bool IsDraftPending { get; private set; }

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
        private const float RoomClearDraftDelay = 2f;

        // ── Cross-class Echo (B5) acquisition cadence (NLM canon: ~1 per 3 combat rooms, capped) ──
        private const int EchoOfferEveryRooms = 3;  // offer an Echo card on rooms 3, 6, 9…
        private const int EchoMaxPerRun       = 4;  // canon cap (one slot per Act)
        private int echoOffersTaken;                // how many Echoes the player has bound this run

        private Warblade_SkillController skillController;
        private Elementalist_SkillController elemSlotController;
        private GameObject               player;
        private RoomConfig               currentRoomConfig;

        // F5 (2026-06-10): run-start class-kit draft pools. The opening draft offers 3 cards from
        // this EXACT list (names must match SkillDatabase.skillName exactly).
        // K1.2 (DEMO_DESIGN_PLAN_2026-06-10): SunderMark excluded (detonation synergy unreadable
        // alone); Blink excluded (no-damage skill feels wrong as first pick).
        private static readonly Dictionary<ClassType, string[]> ClassKits = new Dictionary<ClassType, string[]>
        {
            { ClassType.Warblade,     new[] { "Iron Charge", "Gravity Cleave", "Earthsplitter" } },
            { ClassType.Elementalist, new[] { "Fireball", "Glacial Spike", "Chain Lightning" } },
        };

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
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        private void OnEnable()
        {
            RoomLoader.OnRoomLoaded += HandleRoomLoaded;
            RoomLoader.OnRoomCleared += HandleRoomCleared;
        }

        private void OnDisable()
        {
            RoomLoader.OnRoomLoaded -= HandleRoomLoaded;
            RoomLoader.OnRoomCleared -= HandleRoomCleared;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                skillController = player.GetComponent<Warblade_SkillController>();

            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnSecondaryClassSelected += _ =>
                {
                    // Auto-created instances (DraftManager_Auto) have no serialized refs yet —
                    // resolve them here, not just in ShowDraft(), or this lambda NREs and the
                    // unlock draft never opens.
                    EnsureDependencies();
                    if (offerGenerator != null)
                        offerGenerator.nextDraftIsUnlock = true;
                    IsDraftPending = true;   // flag raised before the 2 s delay so WaitWhile sees it immediately
                    StartCoroutine(ShowDraftDelayed(2f));
                };
        }

        private IEnumerator ShowDraftDelayed(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ShowDraft();
        }

        private void HandleRoomLoaded(RoomConfig config, GameObject _)
        {
            currentRoomConfig = config;
        }

        private void HandleRoomCleared()
        {
            if (IsDraftActive) return;
            if (RoomLoader.DraftDrivenByRoomLoader) return;

            // Phase 1: gate-locked rooms suppress the auto-timer; draft fires from gate (portal) entry only.
#if UNITY_2023_1_OR_NEWER
            var anchor = Object.FindFirstObjectByType<FragmentDropAnchor>();
#else
            var anchor = Object.FindObjectOfType<FragmentDropAnchor>();
#endif
            if (anchor != null && anchor.usePortalGatedDraft)
            {
                Debug.Log("[DraftManager] Gate-locked room — auto-timer suppressed.");
                return;
            }

            int room = RuntimeRoomManager.Instance?.CurrentRoom ?? 1;
            if (room == ForgeRoom1 || room == ForgeRoom2) return;
            if (IsNonDraftRoom(currentRoomConfig)) return;

            StartCoroutine(ShowDraftDelayed(RoomClearDraftDelay));
        }

        /// <summary>
        /// Phase 1 gate (portal) entry point — bypasses room-cleared timer suppression and opens the draft now.
        /// </summary>
        public void TriggerDraftFromFragment(Portal source)
        {
            if (IsDraftActive) return;
            Debug.Log($"[DraftManager] TriggerDraftFromFragment — destination={(source != null ? source.destination.ToString() : "<null>")}");
            ShowDraft();
        }

        private static bool IsNonDraftRoom(RoomConfig config)
        {
            if (config == null) return false;

            string roomType = config.roomType.ToString();
            return roomType == "Corridor" || roomType == "Rest";
        }

        // ── Ana çağrılar ─────────────────────────────────────────

        /// <summary> Oda temizlendikten veya boss yenildikten sonra çağrılır. </summary>
        public void ShowDraft()
        {
            IsDraftPending = false;  // delay penceresi kapandı; aktif durum IsDraftActive'e devredildi
            EnsureDependencies();
            if (offerGenerator == null || offerUI == null)
            {
                Debug.LogWarning("[DraftManager] offerGenerator veya offerUI bulunamadı!");
                IsDraftActive = false;
                return;
            }

            IsDraftActive = true;

            int room = RuntimeRoomManager.Instance?.CurrentRoom ?? 1;

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

            // B5: on the Echo cadence, swap one offer for a curated "Echo of {Class}" card.
            MaybeInjectEchoOffer(offers, primary, room);

            offerUI.Show(offers, OnOfferSelected, room);
        }

        public void HideDraft()
        {
            offerUI?.Hide();
            IsDraftActive = false;
        }

        /// <summary>
        /// F5 (2026-06-10) run-start draft: 3 cards drawn ONLY from the primary class KIT
        /// (Warblade/Elementalist demo kits). The pick lands in the first empty primary slot
        /// (slot 0 = Q). Classes without a kit definition fall back to the normal draft.
        /// </summary>
        public void ShowOpeningKitDraft()
        {
            if (IsDraftActive || IsDraftPending) return;
            EnsureDependencies();
            if (offerUI == null)
            {
                Debug.LogWarning("[DraftManager] ShowOpeningKitDraft: offerUI bulunamadı!");
                return;
            }

            var primary = PlayerClassManager.Instance?.PrimaryClass ?? ClassType.Warblade;
            if (!ClassKits.TryGetValue(primary, out string[] kit))
            {
                Debug.LogWarning($"[DraftManager] No kit defined for {primary}; opening normal draft instead.");
                ShowDraft();
                return;
            }

            SkillDatabase.Instance?.EnsureBuilt();
            var candidates = new List<SkillData>(kit.Length);
            foreach (string name in kit)
            {
                var sd = SkillDatabase.Instance?.FindByName(name);
                if (sd == null)
                {
                    Debug.LogWarning($"[DraftManager] Kit skill '{name}' not found in SkillDatabase.");
                    continue;
                }
                if (!currentActiveSkills.Contains(sd)) candidates.Add(sd);
            }

            if (candidates.Count == 0)
            {
                Debug.LogWarning($"[DraftManager] Opening kit draft: no kit skills resolvable for {primary}; opening normal draft instead.");
                ShowDraft();
                return;
            }

            var offers = new List<RewardOffer>(3);
            while (offers.Count < 3 && candidates.Count > 0)
            {
                int i = Random.Range(0, candidates.Count);
                offers.Add(RewardOffer.FromSkill(candidates[i]));
                candidates.RemoveAt(i);
            }

            IsDraftActive = true;
            offerUI.Show(offers, OnOfferSelected, 1);
            Debug.Log($"[DraftManager] Opening kit draft shown for {primary}: {offers.Count} cards.");
        }

        // ── Cross-class Echo offer (B5) ──────────────────────────

        /// <summary>
        /// On the Echo cadence (every <see cref="EchoOfferEveryRooms"/> rooms, under the per-run cap, and
        /// while a curated guest is still available) replace the LAST offer with an "Echo of {Class}" card.
        /// Reuses the existing 3-card draft flow rather than restructuring the layout for a 4th card —
        /// the Echo is one option among the standard offers.
        /// </summary>
        private void MaybeInjectEchoOffer(List<RewardOffer> offers, ClassType primary, int room)
        {
            if (offers == null || offers.Count == 0) return;
            if (echoOffersTaken >= EchoMaxPerRun) return;
            if (room < EchoOfferEveryRooms || room % EchoOfferEveryRooms != 0) return;

            // Don't re-offer a guest the player already has bound (single active Echo for the demo).
            string boundGuest = ResolveEchoBinding(create: false)?.Binding?.guestSkillName;
            var echo = CrossClassCatalog.PickOffer(primary, boundGuest);
            if (echo == null) return;

            // Replace the last (typically lowest-priority) offer so the count stays at 3.
            offers[offers.Count - 1] = RewardOffer.FromEcho(echo);
            Debug.Log($"[Draft] Echo offer injected: {echo.skillName} (guest skill '{echo.guestSkillName}').");
        }

        /// <summary>Resolve (and optionally create) the player's single <see cref="PlayerCrossClassBinding"/>.
        /// Mirrors the on-demand AddComponent pattern used for skills so no scene wiring is required.</summary>
        private PlayerCrossClassBinding ResolveEchoBinding(bool create)
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return null;
            var b = player.GetComponent<PlayerCrossClassBinding>();
            if (b == null && create) b = player.AddComponent<PlayerCrossClassBinding>();
            return b;
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

            // Cross-class Echo reward (B5) — bind the curated guest to the Echo key (C).
            if (chosen.type == RewardType.CrossClassEcho)
            {
                if (chosen.crossClass != null)
                {
                    var binding = ResolveEchoBinding(create: true);
                    if (binding != null)
                    {
                        binding.Bind(chosen.crossClass);
                        echoOffersTaken++;
                        RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.DraftSelect);
                        Debug.Log($"[Draft] Echo bound: {chosen.crossClass.skillName} → key C.");
                    }
                    else Debug.LogWarning("[Draft] Echo picked but no Player found to bind onto.");
                }
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
                // Codex #1 fix: attach the passive component on demand if not already present.
                if (comp == null)
                    comp = player.AddComponent(passive.skillType) as PassiveBase;
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
            // Codex #1 blocker fix: SkillDatabase is not guaranteed in the playable scene.
            // Without it, SkillOfferGenerator falls back to an unwired serialized list and the draft is empty.
            if (SkillDatabase.Instance == null && FindAnyObjectByType<SkillDatabase>() == null)
            {
                var dbGo = new GameObject("SkillDatabase_Auto");
                dbGo.AddComponent<SkillDatabase>();
                Debug.Log("[DraftManager] SkillDatabase otomatik oluşturuldu.");
            }
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

            // Codex #1 fix: picks can't equip without a Warblade_SkillController on the player.
            // Start() caches it once (null if the prefab/scene lacks one) — re-fetch lazily and create on demand.
            if (skillController == null)
            {
                if (player == null) player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    skillController = player.GetComponent<Warblade_SkillController>()
                                   ?? player.AddComponent<Warblade_SkillController>();
            }
        }

        // ── Yardımcılar ──────────────────────────────────────────

        /// <summary>
        /// F5 (2026-06-10): when the PRIMARY class is Elementalist, primary slots (0-3) live on
        /// Elementalist_SkillController — its Q/E/R/F bindings are the enabled ones and SkillBarUI
        /// reads its slots. The Warblade controller remains the host for secondary slots (4-5).
        /// </summary>
        private bool UseElementalistPrimary()
        {
            if ((PlayerClassManager.Instance?.PrimaryClass ?? ClassType.Warblade) != ClassType.Elementalist)
                return false;

            if (elemSlotController == null)
            {
                if (player == null) player = GameObject.FindGameObjectWithTag("Player");
                elemSlotController = player != null ? player.GetComponent<Elementalist_SkillController>() : null;
            }

            return elemSlotController != null;
        }

        private void AssignActive(SkillData skill, int slot)
        {
            bool elemPrimarySlot = slot < 4 && UseElementalistPrimary();
            Component host = elemPrimarySlot ? (Component)elemSlotController : skillController;

            if (host != null && skill.skillType != null)
            {
                var comp = host.GetComponent(skill.skillType) as SkillBase
                        ?? host.GetComponentInChildren(skill.skillType) as SkillBase;
                // Codex #1 fix: attach the picked skill component on demand.
                if (comp == null)
                    comp = host.gameObject.AddComponent(skill.skillType) as SkillBase;
                if (comp != null)
                {
                    if (elemPrimarySlot) elemSlotController.SetSlot(slot, comp);
                    else                 skillController.SetSlot(slot, comp);
                }
                else
                    Debug.LogWarning($"[Draft] '{skill.skillName}' bileşeni eklenemedi (skillType={skill.skillType}).");
            }
            if (!currentActiveSkills.Contains(skill))
                currentActiveSkills.Add(skill);
        }

        private void FinishPick(SkillData skill)
        {
            offerUI.Hide();
            IsDraftActive = false;
            RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.DraftSelect);
            OnSkillPicked.Invoke(skill);
        }

        private bool IsSecondarySkill(SkillData s)
        {
            var primary = PlayerClassManager.Instance?.PrimaryClass ?? ClassType.Warblade;
            return s.classType != ClassType.None && s.classType != primary;
        }

        private int FindNextPrimarySlot()
        {
            if (UseElementalistPrimary())
            {
                for (int i = 0; i < 4; i++)
                    if (elemSlotController.GetSlot(i) == null) return i;
                return 0;
            }

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
            if (sd.skillType == null) return -1;

            // F5: Elementalist primary keeps its primary slots on its own controller.
            if (UseElementalistPrimary())
            {
                var elemComp = elemSlotController.GetComponent(sd.skillType)
                            ?? elemSlotController.GetComponentInChildren(sd.skillType);
                if (elemComp != null)
                {
                    var elemSlots = elemSlotController.GetAllSlots();
                    for (int i = 0; i < elemSlots.Length; i++)
                        if (elemSlots[i] != null && elemSlots[i] == elemComp) return i;
                }
            }

            if (skillController == null) return -1;
            var comp = skillController.GetComponent(sd.skillType)
                    ?? skillController.GetComponentInChildren(sd.skillType);
            if (comp == null) return -1;
            var slots = skillController.GetAllSlots();
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i] == comp) return i;
            return -1;
        }

        public bool HasSkill(SkillData s) => currentActiveSkills.Contains(s);

        /// <summary>A5 chain-UI: skillNames of the currently-owned active skills, so the draft card
        /// builder can detect Sundered-Beat interlocks (via ChainWindowTracker) against what the
        /// player already runs. Read-only snapshot — never the live mutable list.</summary>
        public IReadOnlyList<string> OwnedActiveSkillNames
        {
            get
            {
                var names = new List<string>(currentActiveSkills.Count);
                for (int i = 0; i < currentActiveSkills.Count; i++)
                {
                    var n = currentActiveSkills[i]?.skillName;
                    if (!string.IsNullOrEmpty(n)) names.Add(n);
                }
                return names;
            }
        }
    }
}
