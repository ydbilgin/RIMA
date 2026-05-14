ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_yasinderyabilgin.md AS THE VERY LAST STEP.

# Codex Task: Game UI Screens — Main Menu + Character Select (Polished)

## Context

RIMA is a 2D top-down Unity roguelite. URP 2D Renderer, PPU=64, dark gritty palette (charcoal #2C2A2A, cyan rift accents #00FFCC, cold blue #7BA7BC). 10 playable classes: Warblade, Ranger, Shadowblade, Elementalist, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer. School deadline ~25 days.

Existing: `Assets/Scripts/UI/CharacterSelectScreen.cs` exists but is basic. Scene to load after character select: "RoomPipelineTest".

## Files to Create/Modify

### 1. `Assets/Scenes/UI/MainMenu.unity` (new scene)

Create via Editor script `Assets/Editor/CreateUIScenes.cs` with `[MenuItem("RIMA/Tools/Create UI Scenes")]`.

**MainMenu scene contents:**
- Camera: PixelPerfectCamera PPU=64, 480×270 ref
- Canvas: Screen Space Overlay, CanvasScaler reference 480×270
- Background: solid dark `#1A1818` (darkest value of our palette) + a subtle animated rift crack (simple sine-wave position offset on a 64px VFX sprite)
- Title: "RIMA" — large pixel font (use `LegacyRuntime.ttf` if no custom font, size 48, color #C8C0B0 warm white)
- Subtitle: "THE RIFT HUNTERS" — small, size 14, color #7BA7BC cold blue, letter spacing
- Menu buttons (vertical stack, center): 
  - [BAŞLA] → loads CharacterSelect scene
  - [AYARLAR] → stub (shows "Yakında" tooltip)
  - [ÇIKIŞ] → Application.Quit()
- Button style: dark `#2A2828` background, 1px cold blue border, text in warm white, hover = border turns cyan, press = scale 0.95
- Animated: title slowly pulses alpha 0.85↔1.0 (sine, 3s period)

### 2. `Assets/Scenes/UI/CharacterSelect.unity` (new scene)

**CharacterSelect scene contents:**
- Camera + Canvas same as MainMenu
- Background: same dark `#1A1818`
- Left panel (40% width): Class info display
  - Class name (large)
  - Class role badge (e.g., "Melee DPS", "Ranged", "Support")
  - Flavor text (1-2 lines, small)
  - Placeholder portrait: 64×64 gray square (will be replaced with actual sprites)
- Right panel (60% width): Class grid
  - 2×5 grid of class buttons (10 classes)
  - Each button: class name text + small icon area
  - Selected = highlighted cyan border
  - Hover = light highlight
- Bottom bar:
  - [GERİ] → loads MainMenu
  - [SEÇT] → loads game scene (SceneManager.LoadScene("RoomPipelineTest"))
  - Selected class name display

**Class data (hardcoded array in script):**
```
Warblade    — Yakın Dövüş DPS     — "Gücün sesi kılıcın sesidir."
Ranger      — Uzak Dövüş          — "Her ok bir son, her nefes bir hedef."
Shadowblade — Suikastçı           — "Gölge, en iyi kalkan."
Elementalist— Büyücü              — "Rift enerjisi, irade ile şekillenir."
Ravager     — Tank/DPS            — "Acı, beni güçlendirir."
Ronin       — Kontr/DPS           — "Kılıcın doğru yolu, tek yoldur."
Gunslinger  — Uzak DPS            — "Her kurşun bir karar."
Brawler     — Dövüşçü             — "Yumruk, söylemden güçlüdür."
Summoner    — Destek/Çağırıcı     — "Ruhlar, emrime amadedir."
Hexer       — Kontrolcü/Debuff    — "Düşmanın zayıflığı, benim silahım."
```

### 3. `Assets/Scripts/UI/MainMenuController.cs`

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RIMA.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string characterSelectScene = "CharacterSelect";
        
        public void OnStartClicked()  => SceneManager.LoadScene(characterSelectScene);
        public void OnQuitClicked()   => Application.Quit();
        public void OnSettingsClicked() => Debug.Log("[MainMenu] Ayarlar yakında.");
    }
}
```

### 4. `Assets/Scripts/UI/CharacterSelectController.cs` (replaces/extends existing)

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
            public string classId; // for future weapon/stat lookup
        }

        [Header("UI References")]
        [SerializeField] private TMP_Text selectedClassName;
        [SerializeField] private TMP_Text selectedClassRole;
        [SerializeField] private TMP_Text selectedClassFlavor;
        [SerializeField] private Button confirmButton;
        [SerializeField] private string gameSceneName = "RoomPipelineTest";

        [Header("Class Grid")]
        [SerializeField] private Transform classButtonContainer;
        [SerializeField] private GameObject classButtonPrefab;

        [Header("Class Data")]
        [SerializeField] private ClassData[] classes;

        private int selectedIndex = 0;
        private Button[] classButtons;

        private void Start()
        {
            if (classes == null || classes.Length == 0) classes = GetDefaultClasses();
            BuildClassGrid();
            SelectClass(0);
        }

        private void BuildClassGrid()
        {
            classButtons = new Button[classes.Length];
            for (int i = 0; i < classes.Length; i++)
            {
                int idx = i;
                var go = Instantiate(classButtonPrefab, classButtonContainer);
                var btn = go.GetComponent<Button>();
                var label = go.GetComponentInChildren<TMP_Text>();
                if (label) label.text = classes[i].className;
                btn.onClick.AddListener(() => SelectClass(idx));
                classButtons[i] = btn;
            }
        }

        public void SelectClass(int index)
        {
            selectedIndex = index;
            var c = classes[index];
            if (selectedClassName)  selectedClassName.text  = c.className;
            if (selectedClassRole)  selectedClassRole.text  = c.role;
            if (selectedClassFlavor) selectedClassFlavor.text = c.flavorText;
            // Visual highlight: reset all, highlight selected
            for (int i = 0; i < classButtons.Length; i++)
            {
                var colors = classButtons[i].colors;
                colors.normalColor = (i == selectedIndex) ? new Color(0f, 1f, 0.8f, 0.3f) : new Color(0.1f, 0.1f, 0.1f, 1f);
                classButtons[i].colors = colors;
            }
        }

        public void OnConfirmClicked() => SceneManager.LoadScene(gameSceneName);
        public void OnBackClicked()    => SceneManager.LoadScene("MainMenu");

        private ClassData[] GetDefaultClasses() => new ClassData[]
        {
            new ClassData { className="Warblade",     role="Yakın Dövüş DPS",    flavorText="Gücün sesi kılıcın sesidir.",        classId="warblade" },
            new ClassData { className="Ranger",       role="Uzak Dövüş",         flavorText="Her ok bir son, her nefes bir hedef.", classId="ranger" },
            new ClassData { className="Shadowblade",  role="Suikastçı",          flavorText="Gölge, en iyi kalkan.",              classId="shadowblade" },
            new ClassData { className="Elementalist", role="Büyücü",             flavorText="Rift enerjisi, irade ile şekillenir.", classId="elementalist" },
            new ClassData { className="Ravager",      role="Tank/DPS",           flavorText="Acı, beni güçlendirir.",             classId="ravager" },
            new ClassData { className="Ronin",        role="Kontr/DPS",          flavorText="Kılıcın doğru yolu, tek yoldur.",    classId="ronin" },
            new ClassData { className="Gunslinger",   role="Uzak DPS",           flavorText="Her kurşun bir karar.",              classId="gunslinger" },
            new ClassData { className="Brawler",      role="Dövüşçü",            flavorText="Yumruk, söylemden güçlüdür.",        classId="brawler" },
            new ClassData { className="Summoner",     role="Destek/Çağırıcı",    flavorText="Ruhlar, emrime amadedir.",           classId="summoner" },
            new ClassData { className="Hexer",        role="Kontrolcü/Debuff",   flavorText="Düşmanın zayıflığı, benim silahım.", classId="hexer" }
        };
    }
}
```

### 5. `Assets/Editor/CreateUIScenes.cs`

MenuItem `RIMA/Tools/Create UI Scenes`. Creates MainMenu.unity and CharacterSelect.unity with proper Canvas/Camera setup, all UI GameObjects, proper references wired, and adds both to Build Settings.

**Canvas setup for both scenes:**
- Canvas → Screen Space Overlay → CanvasScaler: Scale With Screen Size, reference 480×270, match 0.5
- EventSystem (if not present)
- PixelPerfectCamera: PPU=64, ref 480×270, crop both

**Button prefab approach:** Instead of a separate prefab file, create buttons programmatically in the editor script using `DefaultControls.CreateButton()` and apply custom styling via `ColorBlock`.

**Color constants:**
```csharp
static readonly Color BG_DARK     = new Color(0.10f, 0.09f, 0.09f, 1f);  // #1A1818
static readonly Color BTN_NORMAL  = new Color(0.16f, 0.16f, 0.16f, 1f);  // #2A2828
static readonly Color BTN_HOVER   = new Color(0.22f, 0.22f, 0.22f, 1f);
static readonly Color BTN_PRESS   = new Color(0.10f, 0.10f, 0.10f, 1f);
static readonly Color CYAN_RIFT   = new Color(0.00f, 1.00f, 0.80f, 1f);  // #00FFCC
static readonly Color COLD_BLUE   = new Color(0.48f, 0.65f, 0.74f, 1f);  // #7BA7BC
static readonly Color WARM_WHITE  = new Color(0.78f, 0.75f, 0.69f, 1f);  // #C8C0B0
```

## Steps

1. Create `Assets/Scripts/UI/MainMenuController.cs`
2. Create `Assets/Scripts/UI/CharacterSelectController.cs` (keep any working code from existing CharacterSelectScreen.cs)
3. Create `Assets/Editor/CreateUIScenes.cs` with full scene creation logic
4. Check compile: 0 errors (TMPro must be available — use `using TMPro;`)
5. Run `RIMA > Tools > Create UI Scenes` → MainMenu.unity and CharacterSelect.unity created in `Assets/Scenes/UI/`
6. Add both scenes to Build Settings (index 0=MainMenu, 1=CharacterSelect, 2=RoomPipelineTest or _FazMVP_Demo)
7. Test MainMenu: [BAŞLA] navigates to CharacterSelect, class grid shows 10 classes, [SEÇT] navigates to game scene
8. Commit all

## Success Criteria

- 0 compile errors
- Both UI scenes open cleanly in Unity
- Class selection updates info panel correctly
- Navigation: MainMenu → CharacterSelect → Game works
- Dark palette applied consistently

## Commit message

`[ui] MainMenu + CharacterSelect scenes — dark rift palette, 10-class selector, navigation flow`


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_yasinderyabilgin.md AS THE VERY LAST STEP.