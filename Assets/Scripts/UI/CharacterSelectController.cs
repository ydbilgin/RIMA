using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RIMA.UI
{
    public class CharacterSelectController : MonoBehaviour
    {
        [System.Serializable]
        public class ClassData
        {
            public string className;
            public string role;
            public string flavorText;
            public string classId;
        }

        [Header("UI References")]
        [SerializeField] private TMP_Text selectedClassName;
        [SerializeField] private TMP_Text selectedClassRole;
        [SerializeField] private TMP_Text selectedClassFlavor;
        [SerializeField] private TMP_Text selectedClassFooter;
        [SerializeField] private Button confirmButton;
        [SerializeField] private string gameSceneName = "RoomPipelineTest";

        [Header("Class Grid")]
        [SerializeField] private Transform classButtonContainer;
        [SerializeField] private GameObject classButtonPrefab;

        [Header("Class Data")]
        [SerializeField] private ClassData[] classes;

        private int selectedIndex;
        private Button[] classButtons;

        private void Start()
        {
            if (classes == null || classes.Length == 0) classes = GetDefaultClasses();
            BuildClassGrid();
            SelectClass(0);
            if (confirmButton != null) confirmButton.onClick.AddListener(OnConfirmClicked);
        }

        private void BuildClassGrid()
        {
            if (classButtonContainer == null || classButtonPrefab == null)
            {
                Debug.LogWarning("[CharacterSelectController] Class grid references are missing.");
                return;
            }

            for (int i = classButtonContainer.childCount - 1; i >= 0; i--)
            {
                var child = classButtonContainer.GetChild(i);
                if (child.gameObject == classButtonPrefab) continue;
                Destroy(child.gameObject);
            }

            classButtons = new Button[classes.Length];
            for (int i = 0; i < classes.Length; i++)
            {
                int idx = i;
                var go = Instantiate(classButtonPrefab, classButtonContainer);
                go.name = "ClassButton_" + classes[i].className;
                go.SetActive(true);

                var btn = go.GetComponent<Button>();
                var label = go.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = classes[i].className;

                if (btn != null)
                {
                    btn.onClick.AddListener(() => SelectClass(idx));
                    classButtons[i] = btn;
                }
            }
        }

        public void SelectClass(int index)
        {
            if (classes == null || index < 0 || index >= classes.Length) return;

            selectedIndex = index;
            var c = classes[index];
            if (selectedClassName != null) selectedClassName.text = c.className;
            if (selectedClassRole != null) selectedClassRole.text = c.role;
            if (selectedClassFlavor != null) selectedClassFlavor.text = c.flavorText;
            if (selectedClassFooter != null) selectedClassFooter.text = c.className;

            if (classButtons == null) return;

            for (int i = 0; i < classButtons.Length; i++)
            {
                if (classButtons[i] == null) continue;

                var colors = classButtons[i].colors;
                colors.normalColor = i == selectedIndex
                    ? new Color(0f, 1f, 0.8f, 0.30f)
                    : new Color(0.16f, 0.16f, 0.16f, 1f);
                colors.highlightedColor = new Color(0.22f, 0.22f, 0.22f, 1f);
                colors.pressedColor = new Color(0.10f, 0.10f, 0.10f, 1f);
                classButtons[i].colors = colors;

                var feedback = classButtons[i].GetComponent<RimaUIButtonFeedback>();
                if (feedback != null) feedback.SetSelected(i == selectedIndex);
            }
        }

        public void OnConfirmClicked()
        {
            ApplySelectedClass();
            SceneManager.LoadScene(gameSceneName);
        }

        public void OnBackClicked() => SceneManager.LoadScene("MainMenu");

        private void ApplySelectedClass()
        {
            if (classes == null || selectedIndex < 0 || selectedIndex >= classes.Length) return;
            if (PlayerClassManager.Instance == null) return;

            string className = classes[selectedIndex].className;
            if (System.Enum.TryParse(className, out ClassType classType))
            {
                PlayerClassManager.Instance.SetPrimaryClass(classType);
            }
        }

        private ClassData[] GetDefaultClasses() => new ClassData[]
        {
            new ClassData { className = "Warblade", role = "Yakın Dövüş DPS", flavorText = "Gücün sesi kılıcın sesidir.", classId = "warblade" },
            new ClassData { className = "Ranger", role = "Uzak Dövüş", flavorText = "Her ok bir son, her nefes bir hedef.", classId = "ranger" },
            new ClassData { className = "Shadowblade", role = "Suikastçı", flavorText = "Gölge, en iyi kalkan.", classId = "shadowblade" },
            new ClassData { className = "Elementalist", role = "Büyücü", flavorText = "Rift enerjisi, irade ile şekillenir.", classId = "elementalist" },
            new ClassData { className = "Ravager", role = "Tank/DPS", flavorText = "Acı, beni güçlendirir.", classId = "ravager" },
            new ClassData { className = "Ronin", role = "Kontr/DPS", flavorText = "Kılıcın doğru yolu, tek yoldur.", classId = "ronin" },
            new ClassData { className = "Gunslinger", role = "Uzak DPS", flavorText = "Her kurşun bir karar.", classId = "gunslinger" },
            new ClassData { className = "Brawler", role = "Dövüşçü", flavorText = "Yumruk, söylemden güçlüdür.", classId = "brawler" },
            new ClassData { className = "Summoner", role = "Destek/Çağırıcı", flavorText = "Ruhlar, emrime amadedir.", classId = "summoner" },
            new ClassData { className = "Hexer", role = "Kontrolcü/Debuff", flavorText = "Düşmanın zayıflığı, benim silahım.", classId = "hexer" }
        };
    }
}
