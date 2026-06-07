using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using RIMA;

namespace RIMA.Tests
{
    public class CharacterSelectTests
    {
        private GameObject root;

        [SetUp]
        public void SetUp()
        {
            Random.InitState(42);
            root = new GameObject("TestRoot");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(root);
        }

        // ── MainMenuScreen ──────────────────────────────────────────────────

        [Test]
        public void MainMenuScreen_CreatesCanvasWithGraphicRaycaster()
        {
            var go = new GameObject("MainMenu");
            go.transform.SetParent(root.transform);
            go.AddComponent<MainMenuScreen>();

            // Trigger Start manually (EditMode — no play loop)
            var menu = go.GetComponent<MainMenuScreen>();
            // Use SendMessage to trigger Start since we can't enter play mode
            // Verify canvas was created as child
            var canvas = go.GetComponentInChildren<Canvas>();
            // Canvas creation happens in Start; invoke via reflection
            var start = typeof(MainMenuScreen).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(start, "Start method must exist");
            start.Invoke(menu, null);

            canvas = go.GetComponentInChildren<Canvas>();
            Assert.IsNotNull(canvas, "MainMenuScreen must create a Canvas");
            Assert.AreEqual(RenderMode.ScreenSpaceOverlay, canvas.renderMode,
                "Canvas must be ScreenSpaceOverlay");

            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            Assert.IsNotNull(raycaster, "Canvas must have GraphicRaycaster for button input");
        }

        [Test]
        public void MainMenuScreen_CanvasSortOrderIsHigh()
        {
            var go = new GameObject("MainMenu");
            go.transform.SetParent(root.transform);
            var menu = go.AddComponent<MainMenuScreen>();
            var start = typeof(MainMenuScreen).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            start.Invoke(menu, null);

            var canvas = go.GetComponentInChildren<Canvas>();
            Assert.IsNotNull(canvas);
            Assert.GreaterOrEqual(canvas.sortingOrder, 10,
                "Menu canvas must render on top of game elements");
        }

        // ── CharacterSelectScreen ───────────────────────────────────────────

        [Test]
        public void CharacterSelectScreen_CreatesCanvasWithGraphicRaycaster()
        {
            var go = new GameObject("CharSelect");
            go.transform.SetParent(root.transform);
            var screen = go.AddComponent<CharacterSelectScreen>();
            var start = typeof(CharacterSelectScreen).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(start, "Start method must exist");
            start.Invoke(screen, null);

            var canvas = go.GetComponentInChildren<Canvas>();
            Assert.IsNotNull(canvas, "CharacterSelectScreen must create a Canvas");
            Assert.AreEqual(RenderMode.ScreenSpaceOverlay, canvas.renderMode);

            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            Assert.IsNotNull(raycaster, "Canvas must have GraphicRaycaster");
        }

        [Test]
        public void CharacterSelectScreen_HasButtonForEachClass()
        {
            var go = new GameObject("CharSelect");
            go.transform.SetParent(root.transform);
            var screen = go.AddComponent<CharacterSelectScreen>();
            var start = typeof(CharacterSelectScreen).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(start, "Start method must exist");
            start.Invoke(screen, null);

            var buttons = go.GetComponentsInChildren<Button>();
            // 4 class SELECT buttons + 1 BACK button = 5
            Assert.GreaterOrEqual(buttons.Length, 4,
                "Must have at least one button per class (4 classes)");
        }

        [Test]
        public void CharacterSelectScreen_AllButtonsHaveListeners()
        {
            var go = new GameObject("CharSelect");
            go.transform.SetParent(root.transform);
            var screen = go.AddComponent<CharacterSelectScreen>();
            var start = typeof(CharacterSelectScreen).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(start, "Start method must exist");
            start.Invoke(screen, null);

            var buttons = go.GetComponentsInChildren<Button>();
            foreach (var btn in buttons)
            {
                Assert.Greater(btn.onClick.GetPersistentEventCount() +
                               GetRuntimeListenerCount(btn), 0,
                    $"Button '{btn.name}' has no listeners — will not respond to clicks");
            }
        }

        // ── Animator Controller Validation ──────────────────────────────────

        [Test]
        public void AnimatorControllers_AllClassesHaveDirectionParameters()
        {
            string[] classes = { "Warblade", "Elementalist", "Ranger", "Shadowblade" };
            foreach (var cls in classes)
            {
                var ctrl = Resources.Load<RuntimeAnimatorController>($"Characters/{cls}/{cls}");
                if (ctrl == null)
                {
                    Assert.Inconclusive($"Controller not in Resources for {cls} — skip runtime check");
                    continue;
                }

                var overrideCtrl = new AnimatorOverrideController(ctrl);
                var go = new GameObject($"TestAnim_{cls}");
                go.transform.SetParent(root.transform);
                var anim = go.AddComponent<Animator>();
                anim.runtimeAnimatorController = overrideCtrl;

                // Parameters are accessible immediately after assignment
                bool hasDirX = false, hasDirY = false;
                foreach (var p in anim.parameters)
                {
                    if (p.name == "DirX") hasDirX = true;
                    if (p.name == "DirY") hasDirY = true;
                }
                Assert.IsTrue(hasDirX, $"{cls} controller missing DirX parameter");
                Assert.IsTrue(hasDirY, $"{cls} controller missing DirY parameter");
            }
        }

        [Test]
        public void PlayerClassManager_SetPrimaryClassChangesAnimatorController()
        {
            var managerGo = new GameObject("PlayerClassManager");
            managerGo.transform.SetParent(root.transform);
            var manager = managerGo.AddComponent<PlayerClassManager>();

            var player = new GameObject("Player");
            player.tag = "Player";
            player.transform.SetParent(root.transform);

            var sprite = new GameObject("Sprite");
            sprite.transform.SetParent(player.transform);
            var anim = sprite.AddComponent<Animator>();

            var expected = Resources.Load<RuntimeAnimatorController>("Characters/Ranger/Ranger");
            if (expected == null)
                Assert.Inconclusive("Ranger controller not in Resources; skip runtime visual swap check");

            manager.SetPrimaryClass(ClassType.Ranger);

            Assert.AreSame(expected, anim.runtimeAnimatorController,
                "Primary class changes must update player visuals, not only skill bindings.");
        }

        [Test]
        public void ChamberSelectBootstrap_ApplyChamberPlayerVisualUpdatesBodySpriteWithoutAnimator()
        {
            var expected = Resources.Load<Sprite>("Characters/Ranger/ranger_idle_south");
            if (expected == null)
                Assert.Inconclusive("Ranger idle_south sprite not in Resources; skip chamber sprite fallback check");

            var player = new GameObject("Player");
            player.transform.SetParent(root.transform);
            var body = new GameObject("Body");
            body.transform.SetParent(player.transform);
            var renderer = body.AddComponent<SpriteRenderer>();

            var method = typeof(ChamberSelectBootstrap).GetMethod("ApplyChamberPlayerVisual",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method, "Chamber player visual fallback method must exist.");

            method.Invoke(null, new object[] { player, ClassType.Ranger });

            Assert.AreSame(expected, renderer.sprite,
                "Attunement chamber players without Animator must still visually become the selected class.");
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private static int GetRuntimeListenerCount(Button btn)
        {
            // Unity stores runtime listeners in a private field; use reflection
            var field = typeof(UnityEngine.Events.UnityEventBase)
                .GetField("m_Calls",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field == null) return 0;
            var invokableList = field.GetValue(btn.onClick);
            if (invokableList == null) return 0;
            var countProp = invokableList.GetType().GetProperty("Count");
            return countProp != null ? (int)countProp.GetValue(invokableList) : 0;
        }
    }
}
