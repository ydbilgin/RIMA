using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// C overlay — oyun durmaz, sol taraftan açılır.
    /// </summary>
    public class CharacterSheetUI : MonoBehaviour
    {
        public static CharacterSheetUI Instance { get; private set; }

        private GameObject panel;
        private bool visible;

        // Content refs
        private TextMeshProUGUI statsText;
        private TextMeshProUGUI activeText;
        private TextMeshProUGUI passiveText;
        private TextMeshProUGUI ekolText;
        private TextMeshProUGUI traitText;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start() => BuildPanel();

        private void Update()
        {
            if (Keyboard.current != null &&
                Keyboard.current.cKey.wasPressedThisFrame)
                Toggle();

            // Real-time CD update
            if (visible && activeText != null)
                RefreshActives();
        }

        public void Toggle()
        {
            visible = !visible;
            if (panel != null) panel.SetActive(visible);
            if (visible) Refresh();
        }

        // ── Build ────────────────────────────────────────────────

        private void BuildPanel()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            panel = MakeAnchored("CharSheet", canvas.transform,
                new Vector2(0.015f, 0.08f), new Vector2(0.39f, 0.92f));
            var panelImage = panel.AddComponent<Image>();
            panelImage.sprite = RimaUITheme.SmallPanelFrame;
            panelImage.color = new Color(1f, 1f, 1f, 0.95f);

            // Header
            var hdr = MakeAnchored("Header", panel.transform, new Vector2(0f, 0.94f), Vector2.one);
            hdr.AddComponent<Image>().color = new Color(0.08f, 0.08f, 0.14f, 1f);
            var hTmp = MakeAnchored("T", hdr.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            hTmp.text = "BUILD CODEX  [C]"; hTmp.fontSize = 14; hTmp.fontStyle = FontStyles.Bold;
            hTmp.color = RimaUITheme.TextPrimary; hTmp.alignment = TextAlignmentOptions.Center;
            hTmp.raycastTarget = false;

            // Layout: 2 columns, left narrower for stats+ekol, right wider for skills+passives+traits
            // Left column (30%): Stats (top 60%) + Ekol (bottom 38%)
            statsText = MakeColumn("STATE", new Vector2(0.035f, 0.68f), new Vector2(0.965f, 0.925f));
            ekolText  = MakeColumn("PRIMARY / SECONDARY", new Vector2(0.035f, 0.49f), new Vector2(0.965f, 0.665f));

            activeText  = MakeColumn("ACTIVE SKILLS", new Vector2(0.035f, 0.25f), new Vector2(0.965f, 0.475f));
            passiveText = MakeColumn("PASSIVES / ECHOES", new Vector2(0.035f, 0.105f), new Vector2(0.965f, 0.235f));
            traitText   = MakeColumn("ROUTE CONTEXT", new Vector2(0.035f, 0.02f), new Vector2(0.965f, 0.09f));

            panel.SetActive(false);
        }

        private TextMeshProUGUI MakeColumn(string label, Vector2 min, Vector2 max)
        {
            var col = MakeAnchored(label, panel.transform, min, max);
            var colImage = col.AddComponent<Image>();
            colImage.sprite = RimaUITheme.PromptFrame;
            colImage.color = new Color(1f, 1f, 1f, 0.72f);

            // Column header
            var hdr = MakeAnchored("Hdr", col.transform, new Vector2(0f, 0.90f), Vector2.one);
            hdr.AddComponent<Image>().color = new Color(0.10f, 0.10f, 0.18f, 1f);
            var ht = MakeAnchored("T", hdr.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            ht.text = label; ht.fontSize = 9; ht.fontStyle = FontStyles.Bold;
            ht.color = RimaUITheme.Cyan; ht.alignment = TextAlignmentOptions.Center;
            ht.raycastTarget = false;

            // Content text
            var ct = MakeAnchored("Content", col.transform, new Vector2(0.04f, 0.02f), new Vector2(0.96f, 0.89f));
            var tmp = ct.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 10; tmp.color = RimaUITheme.TextPrimary;
            tmp.alignment = TextAlignmentOptions.TopLeft; tmp.enableWordWrapping = true;
            tmp.raycastTarget = false;
            return tmp;
        }

        // ── Refresh ──────────────────────────────────────────────

        private void Refresh()
        {
            RefreshStats();
            RefreshActives();
            RefreshPassives();
            RefreshEkol();
            RefreshTraits();
        }

        private void RefreshStats()
        {
            if (statsText == null) return;
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) { statsText.text = "—"; return; }

            var hp   = player.GetComponent<Health>();
            var rage = player.GetComponent<PlayerResourceBase>();
            var pc   = player.GetComponent<PlayerController>();
            var pa   = player.GetComponent<PlayerAttack>();

            var sb = new System.Text.StringBuilder();

            // HP & Rage
            if (hp != null)
                sb.AppendLine($"<color=#FF6B6B>HP:</color> {hp.CurrentHP} / {hp.MaxHP}");
            if (rage != null)
                sb.AppendLine($"<color=#FF9F43>Resource:</color> {rage.Current} / {rage.Max}");

            sb.AppendLine();

            // Bonuslar
            if (pa != null)
            {
                float dmgBonus = (pa.outgoingDamageMultiplier - 1f) * 100f;
                sb.AppendLine($"<color=#FFA07A>Hasar:</color> +{dmgBonus:F0}%");
            }

            sb.AppendLine($"<color=#87CEEB>CD Azalma:</color> 0%");
            sb.AppendLine($"<color=#98D8C8>CC Direnci:</color> 0%");

            if (pc != null)
                sb.AppendLine($"<color=#F7DC6F>Hız:</color> {pc.MoveSpeed:F1}");

            sb.AppendLine();
            ClassType primary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            ClassType secondary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.SecondaryClass : ClassType.None;
            sb.AppendLine($"<color=#E74C3C>Class:</color> {primary}");
            sb.AppendLine($"<color=#95A5A6>Secondary:</color> {secondary}");

            statsText.text = sb.ToString();
        }

        private void RefreshActives()
        {
            if (activeText == null) return;
            var player = GameObject.FindGameObjectWithTag("Player");
            string[] keys = { "Q", "E", "R", "F", "Z", "X" };
            var sb = new System.Text.StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                var skill = GetPlayerSlot(player, i, out int slotCount);

                if (i >= slotCount)
                {
                    sb.AppendLine($"<color=#555>[{keys[i]}]</color> <color=#777>KILITLI</color>");
                }
                else if (skill != null)
                {
                    string name = string.IsNullOrWhiteSpace(skill.skillName) ? skill.GetType().Name : skill.skillName;
                    float cd = skill.RemainingCooldown;
                    string cdText = cd > 0.1f ? $"<color=#FF6B6B>{cd:F1}s</color>" : "<color=#5FD35F>HAZIR</color>";
                    sb.AppendLine($"<color=#FFF>[{keys[i]}]</color> {name}  {cdText}");
                }
                else
                {
                    sb.AppendLine($"<color=#888>[{keys[i]}]</color> <color=#666>(boş)</color>");
                }
            }

            activeText.text = sb.ToString();
        }

        private void RefreshPassives()
        {
            if (passiveText == null) return;
            if (DraftManager.Instance == null) { passiveText.text = "—"; return; }

            var db = SkillDatabase.Instance;
            if (db == null) { passiveText.text = "—"; return; }

            var sb = new System.Text.StringBuilder();
            bool hasAny = false;

            foreach (var s in db.GetAll())
            {
                if (!s.isPassive) continue;
                int lvl = DraftManager.GetPassiveLevel(s.skillName);
                if (lvl <= 0) continue;

                hasAny = true;

                // Seviye göstergesi: ● dolu, ○ boş
                string dots = "";
                for (int i = 1; i <= 3; i++)
                    dots += i <= lvl ? "<color=#FFA07A>●</color> " : "<color=#444>○</color> ";

                string maxTag = lvl == 3 ? " <color=#5FD35F>MAX</color>" : $" Lv{lvl}";
                sb.AppendLine($"{s.skillName}  {dots}{maxTag}");
            }

            passiveText.text = hasAny ? sb.ToString() : "<color=#666>(pasif yok)</color>";
        }

        private void RefreshEkol()
        {
            if (ekolText == null) return;

            ClassType primary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"<color=#E74C3C>LMB:</color> {GetPrimaryVerb(primary)}");
            sb.AppendLine($"<color=#3498DB>RMB:</color> {GetSecondaryVerb(primary)}");

            ekolText.text = sb.ToString();
        }

        private void RefreshTraits()
        {
            if (traitText == null) return;

            int room = RuntimeRoomManager.Instance != null ? RuntimeRoomManager.Instance.CurrentRoom : 0;
            traitText.text = room > 0 ? $"ROOM {room} | near route only" : "route unknown";
        }

        private static SkillBase GetPlayerSlot(GameObject player, int index, out int slotCount)
        {
            slotCount = 4;
            if (player == null) return null;
            ClassType primary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            if (primary == ClassType.Elementalist)
            {
                var c = player.GetComponent<Elementalist_SkillController>();
                slotCount = c != null ? c.SlotCount : 4;
                return c != null ? c.GetSlot(index) : null;
            }
            if (primary == ClassType.Ranger)
            {
                var c = player.GetComponent<Ranger_SkillController>();
                slotCount = c != null ? c.SlotCount : 4;
                return c != null ? c.GetSlot(index) : null;
            }
            if (primary == ClassType.Shadowblade)
            {
                var c = player.GetComponent<Shadowblade_SkillController>();
                slotCount = c != null ? c.SlotCount : 4;
                return c != null ? c.GetSlot(index) : null;
            }

            var w = player.GetComponent<Warblade_SkillController>();
            slotCount = w != null ? w.SlotCount : 4;
            return w != null ? w.GetSlot(index) : null;
        }

        private static string GetPrimaryVerb(ClassType type) => type switch
        {
            ClassType.Elementalist => "Rift Bolt / elemental builder",
            ClassType.Ranger => "Rift Arrow / precision line",
            ClassType.Shadowblade => "Veil Strike / close burst",
            _ => "Iron Combo / melee pressure",
        };

        private static string GetSecondaryVerb(ClassType type) => type switch
        {
            ClassType.Elementalist => "Switch element / Lightbreak setup",
            ClassType.Ranger => "Roll / mark timing",
            ClassType.Shadowblade => "Veil Flicker / reposition",
            _ => "Rage outlet / guard break",
        };

        // ── Utility ──────────────────────────────────────────────

        private static GameObject MakeAnchored(string name, Transform parent, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = min; rt.anchorMax = max;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }
    }
}
