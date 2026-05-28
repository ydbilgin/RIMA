using System.Collections.Generic;
using System.IO;
using RIMA.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace RIMA.EditorTools
{
    public static class CreateUIScenes
    {
        private const string UiSceneFolder = "Assets/Scenes/UI";
        private const string RiftSpritePath = "Assets/Sprites/UI/rift_crack_64.png";
        private const string MainMenuPath = UiSceneFolder + "/MainMenu.unity";
        private const string CharacterSelectPath = UiSceneFolder + "/CharacterSelect.unity";
        private const string RoomPipelinePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";
        private const string DemoPath = "Assets/Scenes/Demo/_FazMVP_Demo.unity";

        private static readonly Color BG_DARK = new Color(0.10f, 0.09f, 0.09f, 1f);
        private static readonly Color BTN_NORMAL = new Color(0.16f, 0.16f, 0.16f, 1f);
        private static readonly Color BTN_HOVER = new Color(0.22f, 0.22f, 0.22f, 1f);
        private static readonly Color BTN_PRESS = new Color(0.10f, 0.10f, 0.10f, 1f);
        private static readonly Color CYAN_RIFT = new Color(0.00f, 1.00f, 0.80f, 1f);
        private static readonly Color COLD_BLUE = new Color(0.48f, 0.65f, 0.74f, 1f);
        private static readonly Color WARM_WHITE = new Color(0.78f, 0.75f, 0.69f, 1f);

        [MenuItem("RIMA/Tools/Create UI Scenes")]
        public static void CreateAll()
        {
            Directory.CreateDirectory(UiSceneFolder);
            EnsureRiftSpriteAsset();
            CreateMainMenuScene();
            CreateCharacterSelectScene();
            ConfigureBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[CreateUIScenes] MainMenu and CharacterSelect scenes created.");
        }

        private static void CreateMainMenuScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddCamera();
            var canvas = AddCanvas("MainMenuCanvas");
            AddEventSystem();

            var root = AddPanel("Root", canvas.transform, BG_DARK);
            Stretch(root, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            var rift = AddImage("RiftCrack", root, AssetDatabase.LoadAssetAtPath<Sprite>(RiftSpritePath), CYAN_RIFT);
            rift.sizeDelta = new Vector2(64f, 64f);
            rift.anchorMin = rift.anchorMax = new Vector2(0.5f, 0.5f);
            rift.anchoredPosition = new Vector2(112f, 38f);
            rift.localRotation = Quaternion.Euler(0f, 0f, -18f);
            var riftGroup = rift.gameObject.AddComponent<CanvasGroup>();
            riftGroup.alpha = 0.45f;
            rift.gameObject.AddComponent<RimaRiftCrackAnimator>();

            var title = AddText("Title", root, "RIMA", 48f, FontStyles.Bold, WARM_WHITE, TextAlignmentOptions.Center);
            title.rectTransform.anchorMin = title.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            title.rectTransform.sizeDelta = new Vector2(240f, 62f);
            title.rectTransform.anchoredPosition = new Vector2(0f, 76f);
            var titleGroup = title.gameObject.AddComponent<CanvasGroup>();
            var pulse = title.gameObject.AddComponent<RimaAlphaPulse>();
            SetObject(pulse, "canvasGroup", titleGroup);

            var subtitle = AddText("Subtitle", root, "THE RIFT HUNTERS", 14f, FontStyles.Bold, COLD_BLUE, TextAlignmentOptions.Center);
            subtitle.characterSpacing = 6f;
            subtitle.rectTransform.anchorMin = subtitle.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            subtitle.rectTransform.sizeDelta = new Vector2(260f, 24f);
            subtitle.rectTransform.anchoredPosition = new Vector2(0f, 33f);

            var buttonRoot = AddPanel("MenuButtons", root, Color.clear);
            buttonRoot.anchorMin = buttonRoot.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRoot.sizeDelta = new Vector2(140f, 106f);
            buttonRoot.anchoredPosition = new Vector2(0f, -42f);
            Object.DestroyImmediate(buttonRoot.GetComponent<Image>());
            var stack = buttonRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            stack.spacing = 8f;
            stack.childControlWidth = true;
            stack.childControlHeight = true;
            stack.childForceExpandWidth = true;
            stack.childForceExpandHeight = false;

            var controller = canvas.gameObject.AddComponent<MainMenuController>();
            var start = AddButton(buttonRoot, "BaslaButton", "BAŞLA", 140f, 30f);
            var settings = AddButton(buttonRoot, "SettingsButton", "AYARLAR", 140f, 30f);
            var quit = AddButton(buttonRoot, "QuitButton", "ÇIKIŞ", 140f, 30f);
            UnityEventTools.AddPersistentListener(start.onClick, controller.OnStartClicked);
            UnityEventTools.AddPersistentListener(settings.onClick, controller.OnSettingsClicked);
            UnityEventTools.AddPersistentListener(quit.onClick, controller.OnQuitClicked);

            var tooltip = AddText("SettingsTooltip", root, "Yakında", 10f, FontStyles.Normal, CYAN_RIFT, TextAlignmentOptions.Center);
            tooltip.rectTransform.anchorMin = tooltip.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            tooltip.rectTransform.sizeDelta = new Vector2(120f, 20f);
            tooltip.rectTransform.anchoredPosition = new Vector2(0f, -126f);
            tooltip.gameObject.SetActive(false);
            SetObject(controller, "settingsTooltip", tooltip);

            EditorSceneManager.SaveScene(scene, MainMenuPath);
        }

        private static void CreateCharacterSelectScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddCamera();
            var canvas = AddCanvas("CharacterSelectCanvas");
            AddEventSystem();

            var root = AddPanel("Root", canvas.transform, BG_DARK);
            Stretch(root, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            var title = AddText("Header", root, "CLASS SELECT", 18f, FontStyles.Bold, WARM_WHITE, TextAlignmentOptions.Center);
            title.rectTransform.anchorMin = new Vector2(0f, 1f);
            title.rectTransform.anchorMax = new Vector2(1f, 1f);
            title.rectTransform.pivot = new Vector2(0.5f, 1f);
            title.rectTransform.sizeDelta = new Vector2(0f, 28f);
            title.rectTransform.anchoredPosition = new Vector2(0f, -8f);

            var left = AddPanel("InfoPanel", root, new Color(0.13f, 0.12f, 0.12f, 0.92f));
            Stretch(left, new Vector2(0.04f, 0.18f), new Vector2(0.39f, 0.86f), Vector2.zero, Vector2.zero);
            AddOutline(left.gameObject, COLD_BLUE);

            var portrait = AddPanel("PortraitPlaceholder", left, new Color(0.32f, 0.32f, 0.32f, 1f));
            portrait.anchorMin = portrait.anchorMax = new Vector2(0.5f, 1f);
            portrait.pivot = new Vector2(0.5f, 1f);
            portrait.sizeDelta = new Vector2(64f, 64f);
            portrait.anchoredPosition = new Vector2(0f, -18f);

            var name = AddText("ClassName", left, "Warblade", 20f, FontStyles.Bold, WARM_WHITE, TextAlignmentOptions.Center);
            AnchorRect(name.rectTransform, new Vector2(0.08f, 0.56f), new Vector2(0.92f, 0.76f));

            var role = AddText("RoleBadge", left, "Yakın Dövüş DPS", 11f, FontStyles.Bold, CYAN_RIFT, TextAlignmentOptions.Center);
            AnchorRect(role.rectTransform, new Vector2(0.18f, 0.45f), new Vector2(0.82f, 0.55f));

            var flavor = AddText("FlavorText", left, "Gücün sesi kılıcın sesidir.", 10f, FontStyles.Normal, COLD_BLUE, TextAlignmentOptions.Center);
            flavor.enableWordWrapping = true;
            AnchorRect(flavor.rectTransform, new Vector2(0.08f, 0.12f), new Vector2(0.92f, 0.42f));

            var right = AddPanel("ClassGridPanel", root, Color.clear);
            Stretch(right, new Vector2(0.43f, 0.18f), new Vector2(0.96f, 0.86f), Vector2.zero, Vector2.zero);
            Object.DestroyImmediate(right.GetComponent<Image>());

            var grid = AddPanel("ClassButtonContainer", right, Color.clear);
            Stretch(grid, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            Object.DestroyImmediate(grid.GetComponent<Image>());
            var gridLayout = grid.gameObject.AddComponent<GridLayoutGroup>();
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 2;
            gridLayout.cellSize = new Vector2(118f, 34f);
            gridLayout.spacing = new Vector2(10f, 8f);
            gridLayout.childAlignment = TextAnchor.MiddleCenter;

            var template = CreateClassButtonTemplate(grid);

            var bottom = AddPanel("BottomBar", root, new Color(0.08f, 0.075f, 0.075f, 0.96f));
            Stretch(bottom, new Vector2(0f, 0f), new Vector2(1f, 0.14f), Vector2.zero, Vector2.zero);

            var back = AddButton(bottom, "BackButton", "GERİ", 82f, 28f);
            SetAnchored(back.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(52f, 0f));
            var confirm = AddButton(bottom, "ConfirmButton", "SEÇT", 82f, 28f);
            SetAnchored(confirm.GetComponent<RectTransform>(), new Vector2(1f, 0.5f), new Vector2(-52f, 0f));

            var selectedFooter = AddText("SelectedClassFooter", bottom, "Warblade", 12f, FontStyles.Bold, WARM_WHITE, TextAlignmentOptions.Center);
            selectedFooter.rectTransform.anchorMin = selectedFooter.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            selectedFooter.rectTransform.sizeDelta = new Vector2(160f, 24f);
            selectedFooter.rectTransform.anchoredPosition = Vector2.zero;

            var controller = canvas.gameObject.AddComponent<CharacterSelectController>();
            SetObject(controller, "selectedClassName", name);
            SetObject(controller, "selectedClassRole", role);
            SetObject(controller, "selectedClassFlavor", flavor);
            SetObject(controller, "selectedClassFooter", selectedFooter);
            SetObject(controller, "confirmButton", confirm);
            SetObject(controller, "classButtonContainer", grid);
            SetObject(controller, "classButtonPrefab", template);
            SetString(controller, "gameSceneName", "RoomPipelineTest");
            SetClasses(controller);

            UnityEventTools.AddPersistentListener(back.onClick, controller.OnBackClicked);

            EditorSceneManager.SaveScene(scene, CharacterSelectPath);
        }

        private static void AddCamera()
        {
            var cameraGo = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(PixelPerfectCamera));
            cameraGo.tag = "MainCamera";
            var camera = cameraGo.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BG_DARK;
            camera.orthographic = true;

            var pixelPerfect = cameraGo.GetComponent<PixelPerfectCamera>();
            pixelPerfect.assetsPPU = 64;
            pixelPerfect.refResolutionX = 480;
            pixelPerfect.refResolutionY = 270;
            pixelPerfect.cropFrameX = true;
            pixelPerfect.cropFrameY = true;
        }

        private static Canvas AddCanvas(string name)
        {
            var canvasGo = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(480f, 270f);
            scaler.matchWidthOrHeight = 0.5f;
            return canvas;
        }

        private static void AddEventSystem()
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = null;
        }

        private static RectTransform AddPanel(string name, Transform parent, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return go.GetComponent<RectTransform>();
        }

        private static RectTransform AddImage(string name, Transform parent, Sprite sprite, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;
            image.preserveAspect = true;
            image.raycastTarget = false;
            return go.GetComponent<RectTransform>();
        }

        private static TextMeshProUGUI AddText(string name, Transform parent, string text, float size, FontStyles style, Color color, TextAlignmentOptions alignment)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            var tmp = go.GetComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.fontStyle = style;
            tmp.color = color;
            tmp.alignment = alignment;
            tmp.raycastTarget = false;
            tmp.enableWordWrapping = false;
            return tmp;
        }

        private static Button AddButton(Transform parent, string name, string label, float width, float height)
        {
            var resources = new DefaultControls.Resources();
            var go = DefaultControls.CreateButton(resources);
            go.name = name;
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(width, height);

            var image = go.GetComponent<Image>();
            image.color = BTN_NORMAL;
            image.raycastTarget = true;
            AddOutline(go, COLD_BLUE);

            var button = go.GetComponent<Button>();
            var colors = button.colors;
            colors.normalColor = BTN_NORMAL;
            colors.highlightedColor = BTN_HOVER;
            colors.pressedColor = BTN_PRESS;
            colors.selectedColor = BTN_HOVER;
            colors.colorMultiplier = 1f;
            button.colors = colors;

            var text = go.GetComponentInChildren<Text>();
            if (text != null) Object.DestroyImmediate(text.gameObject);

            var tmp = AddText("Label", go.transform, label, 12f, FontStyles.Bold, WARM_WHITE, TextAlignmentOptions.Center);
            Stretch(tmp.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            go.AddComponent<RimaUIButtonFeedback>();
            return button;
        }

        private static GameObject CreateClassButtonTemplate(Transform parent)
        {
            var button = AddButton(parent, "ClassButtonTemplate", "Warblade", 118f, 34f);
            var rect = button.GetComponent<RectTransform>();

            var icon = AddPanel("Icon", rect, new Color(0.28f, 0.28f, 0.28f, 1f));
            icon.anchorMin = icon.anchorMax = new Vector2(0f, 0.5f);
            icon.pivot = new Vector2(0f, 0.5f);
            icon.sizeDelta = new Vector2(18f, 18f);
            icon.anchoredPosition = new Vector2(8f, 0f);

            var label = button.GetComponentInChildren<TextMeshProUGUI>();
            label.alignment = TextAlignmentOptions.Left;
            label.rectTransform.offsetMin = new Vector2(32f, 0f);
            label.rectTransform.offsetMax = new Vector2(-6f, 0f);

            button.gameObject.SetActive(false);
            return button.gameObject;
        }

        private static void ConfigureBuildSettings()
        {
            var scenes = new List<EditorBuildSettingsScene>
            {
                new EditorBuildSettingsScene(MainMenuPath, true),
                new EditorBuildSettingsScene(CharacterSelectPath, true)
            };

            if (File.Exists(RoomPipelinePath))
            {
                scenes.Add(new EditorBuildSettingsScene(RoomPipelinePath, true));
            }
            else if (File.Exists(DemoPath))
            {
                scenes.Add(new EditorBuildSettingsScene(DemoPath, true));
            }

            if (File.Exists(DemoPath) && (scenes.Count < 3 || scenes[2].path != DemoPath))
            {
                scenes.Add(new EditorBuildSettingsScene(DemoPath, true));
            }

            foreach (var existing in EditorBuildSettings.scenes)
            {
                if (!File.Exists(existing.path)) continue;
                if (scenes.Exists(s => s.path == existing.path)) continue;
                scenes.Add(existing);
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void EnsureRiftSpriteAsset()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(RiftSpritePath));
            if (!File.Exists(RiftSpritePath))
            {
                var tex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
                var transparent = new Color32(0, 0, 0, 0);
                var cyan = new Color32(0, 255, 204, 255);
                var dim = new Color32(0, 255, 204, 90);
                for (int y = 0; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        tex.SetPixel(x, y, transparent);
                    }
                }

                for (int y = 4; y < 60; y++)
                {
                    int center = 31 + Mathf.RoundToInt(Mathf.Sin(y * 0.32f) * 7f);
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int x = Mathf.Clamp(center + dx, 0, 63);
                        tex.SetPixel(x, y, dx == 0 ? cyan : dim);
                    }

                    if (y % 11 == 0)
                    {
                        int branchLength = 8;
                        int direction = y % 22 == 0 ? -1 : 1;
                        for (int i = 0; i < branchLength; i++)
                        {
                            int bx = Mathf.Clamp(center + direction * i, 0, 63);
                            int by = Mathf.Clamp(y + i / 2, 0, 63);
                            tex.SetPixel(bx, by, dim);
                        }
                    }
                }

                tex.Apply();
                File.WriteAllBytes(RiftSpritePath, tex.EncodeToPNG());
                Object.DestroyImmediate(tex);
            }

            AssetDatabase.ImportAsset(RiftSpritePath, ImportAssetOptions.ForceUpdate);
            var importer = (TextureImporter)AssetImporter.GetAtPath(RiftSpritePath);
            if (importer == null) return;
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 64f;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        private static void AddOutline(GameObject go, Color color)
        {
            var outline = go.GetComponent<Outline>() ?? go.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = Vector2.one;
            outline.useGraphicAlpha = false;
        }

        private static void Stretch(RectTransform rt, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }

        private static void AnchorRect(RectTransform rt, Vector2 min, Vector2 max)
        {
            Stretch(rt, min, max, Vector2.zero, Vector2.zero);
        }

        private static void SetAnchored(RectTransform rt, Vector2 anchor, Vector2 position)
        {
            rt.anchorMin = rt.anchorMax = anchor;
            rt.pivot = anchor;
            rt.anchoredPosition = position;
        }

        private static void SetObject(Object target, string fieldName, Object value)
        {
            var serialized = new SerializedObject(target);
            serialized.FindProperty(fieldName).objectReferenceValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetString(Object target, string fieldName, string value)
        {
            var serialized = new SerializedObject(target);
            serialized.FindProperty(fieldName).stringValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetClasses(Object target)
        {
            var data = new[]
            {
                ("Warblade", "Yakın Dövüş DPS", "Gücün sesi kılıcın sesidir.", "warblade"),
                ("Ranger", "Uzak Dövüş", "Her ok bir son, her nefes bir hedef.", "ranger"),
                ("Shadowblade", "Suikastçı", "Gölge, en iyi kalkan.", "shadowblade"),
                ("Elementalist", "Büyücü", "Rift enerjisi, irade ile şekillenir.", "elementalist"),
                ("Ravager", "Tank/DPS", "Acı, beni güçlendirir.", "ravager"),
                ("Ronin", "Kontr/DPS", "Kılıcın doğru yolu, tek yoldur.", "ronin"),
                ("Gunslinger", "Uzak DPS", "Her kurşun bir karar.", "gunslinger"),
                ("Brawler", "Dövüşçü", "Yumruk, söylemden güçlüdür.", "brawler"),
                ("Summoner", "Destek/Çağırıcı", "Ruhlar, emrime amadedir.", "summoner"),
                ("Hexer", "Kontrolcü/Debuff", "Düşmanın zayıflığı, benim silahım.", "hexer")
            };

            var serialized = new SerializedObject(target);
            var classes = serialized.FindProperty("classes");
            classes.arraySize = data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                var item = classes.GetArrayElementAtIndex(i);
                item.FindPropertyRelative("className").stringValue = data[i].Item1;
                item.FindPropertyRelative("role").stringValue = data[i].Item2;
                item.FindPropertyRelative("flavorText").stringValue = data[i].Item3;
                item.FindPropertyRelative("classId").stringValue = data[i].Item4;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
