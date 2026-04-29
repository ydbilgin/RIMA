using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// TAB overlay — oyun durmaz, üstten açılır.
    /// Layout: Stats (sol üst) | Aktif (sağ üst) | Pasif (sağ orta) | Ekol (sol alt) | Trait (sağ alt)
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
            if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
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
                new Vector2(0.05f, 0.10f), new Vector2(0.95f, 0.90f));
            panel.AddComponent<Image>().color = new Color(0.03f, 0.03f, 0.08f, 0.96f);

            // Header
            var hdr = MakeAnchored("Header", panel.transform, new Vector2(0f, 0.94f), Vector2.one);
            hdr.AddComponent<Image>().color = new Color(0.08f, 0.08f, 0.14f, 1f);
            var hTmp = MakeAnchored("T", hdr.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            hTmp.text = "KARAKTER ŞEMATİĞİ  [TAB: Kapat]"; hTmp.fontSize = 14; hTmp.fontStyle = FontStyles.Bold;
            hTmp.color = new Color(0.80f, 0.82f, 1f); hTmp.alignment = TextAlignmentOptions.Center;
            hTmp.raycastTarget = false;

            // Layout: 2 columns, left narrower for stats+ekol, right wider for skills+passives+traits
            // Left column (30%): Stats (top 60%) + Ekol (bottom 38%)
            statsText = MakeColumn("STATS", new Vector2(0.01f, 0.54f), new Vector2(0.30f, 0.93f));
            ekolText  = MakeColumn("LMB/RMB EKOL", new Vector2(0.01f, 0.02f), new Vector2(0.30f, 0.52f));

            // Right column (68%): Active (top 48%) + Passive (mid 24%) + Trait (bottom 24%)
            activeText  = MakeColumn("AKTİF SKİLLER", new Vector2(0.32f, 0.54f), new Vector2(0.99f, 0.93f));
            passiveText = MakeColumn("PASİFLER", new Vector2(0.32f, 0.28f), new Vector2(0.99f, 0.52f));
            traitText   = MakeColumn("TRAİTLER", new Vector2(0.32f, 0.02f), new Vector2(0.99f, 0.26f));

            panel.SetActive(false);
        }

        private TextMeshProUGUI MakeColumn(string label, Vector2 min, Vector2 max)
        {
            var col = MakeAnchored(label, panel.transform, min, max);
            col.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.10f, 0.80f);

            // Column header
            var hdr = MakeAnchored("Hdr", col.transform, new Vector2(0f, 0.90f), Vector2.one);
            hdr.AddComponent<Image>().color = new Color(0.10f, 0.10f, 0.18f, 1f);
            var ht = MakeAnchored("T", hdr.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            ht.text = label; ht.fontSize = 9; ht.fontStyle = FontStyles.Bold;
            ht.color = new Color(0.55f, 0.58f, 0.72f); ht.alignment = TextAlignmentOptions.Center;
            ht.raycastTarget = false;

            // Content text
            var ct = MakeAnchored("Content", col.transform, new Vector2(0.04f, 0.02f), new Vector2(0.96f, 0.89f));
            var tmp = ct.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 9; tmp.color = new Color(0.75f, 0.78f, 0.90f);
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
            var rage = player.GetComponent<RageSystem>();
            var pc   = player.GetComponent<PlayerController>();
            var pa   = player.GetComponent<PlayerAttack>();

            var sb = new System.Text.StringBuilder();

            // HP & Rage
            if (hp != null)
                sb.AppendLine($"<color=#FF6B6B>HP:</color> {hp.CurrentHP} / {hp.MaxHP}");
            if (rage != null)
                sb.AppendLine($"<color=#FF9F43>Rage:</color> {rage.CurrentRage} / {rage.MaxRage}");

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
            sb.AppendLine($"<color=#E74C3C>Sınıf:</color> Warblade");
            sb.AppendLine($"<color=#95A5A6>Secondary:</color> Yok");

            statsText.text = sb.ToString();
        }

        private void RefreshActives()
        {
            if (activeText == null) return;
            var player = GameObject.FindGameObjectWithTag("Player");
            var sc = player?.GetComponent<Warblade_SkillController>();
            if (sc == null) { activeText.text = "—"; return; }

            string[] keys = { "Q", "E", "R", "F", "Z", "X" };
            var sb = new System.Text.StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                var skill = i < sc.SlotCount ? sc.GetSlot(i) : null;

                if (i == 4 || i == 5)
                {
                    // Z/X slotları kilitli (secondary class için)
                    sb.AppendLine($"<color=#555>[{keys[i]}]</color> <color=#777>KILITLI</color>");
                }
                else if (skill != null)
                {
                    string name = skill.gameObject.name.Replace("(Clone)", "").Trim();
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

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<color=#E74C3C>LMB EKOL</color>");
            sb.AppendLine("<color=#666>Seçilmedi</color>");
            sb.AppendLine();
            sb.AppendLine("<color=#3498DB>RMB EKOL</color>");
            sb.AppendLine("<color=#666>Seçilmedi</color>");

            ekolText.text = sb.ToString();
        }

        private void RefreshTraits()
        {
            if (traitText == null) return;

            // Trait sistemi henüz yok — placeholder
            traitText.text = "<color=#666>(trait yok)</color>";
        }

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
