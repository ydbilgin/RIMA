using System.Collections;
using NUnit.Framework;
using RIMA;
using RIMA.Environment;
using RIMA.MapDesigner.Room.Runtime;
using RIMA.UI.BuildMode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.BuildMode
{
    /// <summary>
    /// PlayMode integration suite for the editor consolidation (binding decision item 5). Covers the
    /// bug classes that NEED a live scene: F2 sole-owner toggle, legacy IMGUI overlay retired, canvas
    /// overlap hide/restore, working-copy lifecycle, tool exclusivity, iso-grid overlay lifecycle, and
    /// Save no-op safety.
    ///
    /// A minimal scene is built IN CODE (Camera + isometric Grid/Tilemap + WalkabilityMap) instead of
    /// loading PlayableArena_Test01 from disk — that scene is not in the build profile (the existing
    /// RoomFlowTests is [Ignore]'d for the same reason). DirectorMode / BuildModeController /
    /// BuildPlacementController self-bootstrap via RuntimeInitializeOnLoadMethod, so the controllers
    /// come up on their own; the test only supplies the scene prerequisites EnterBuildMode needs
    /// (DirectorMode.Instance + Camera.main, both satisfied here).
    /// </summary>
    public class BuildModeConsolidationPlayModeTests
    {
        private GameObject cameraGo;
        private GameObject gridGo;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            // Camera tagged MainCamera so Camera.main resolves (EnterBuildMode precondition).
            cameraGo = new GameObject("TestMainCamera");
            cameraGo.tag = "MainCamera";
            var cam = cameraGo.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 6f;
            cameraGo.transform.position = new Vector3(0f, 0f, -10f);

            // Isometric Grid + floor Tilemap so BuildPlacementController.ResolveSceneRefs finds a Grid
            // (the iso-grid overlay needs grid.GetCellCenterWorld). Matches project cell layout/size.
            gridGo = new GameObject("TestGrid");
            var grid = gridGo.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSize = new Vector3(0.96f, 0.585f, 1f);

            var tmGo = new GameObject("Floor");
            tmGo.transform.SetParent(gridGo.transform, false);
            var tilemap = tmGo.AddComponent<Tilemap>();
            tmGo.AddComponent<TilemapRenderer>();

            var walk = gridGo.AddComponent<WalkabilityMap>();
            walk.floorTilemap = tilemap;

            // Let the self-bootstrapping controllers (DirectorMode, BuildModeController, etc.) spin up.
            yield return null;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            var bm = BuildModeController.Instance;
            if (bm != null && bm.IsBuildModeActive) bm.ExitBuildMode();
            Time.timeScale = 1f;

            // ExitBuildMode runs a camera-zoom coroutine (~0.35s unscaled) that touches the camera and
            // calls RestoreCameraRig at the end. Destroy the test camera only AFTER it finishes, or the
            // coroutine NREs on the destroyed camera. Wait out the lerp plus a margin.
            yield return new WaitForSecondsRealtime(0.6f);

            if (cameraGo != null) Object.Destroy(cameraGo);
            if (gridGo != null) Object.Destroy(gridGo);
            yield return null;
        }

        private static bool CanEnterBuildMode()
        {
            return DirectorMode.Instance != null && Camera.main != null;
        }

        [UnityTest]
        public IEnumerator F2Toggle_BuildModeController_IsSoleOwner()
        {
            if (!CanEnterBuildMode()) { Assert.Ignore("No DirectorMode/Camera.main in this session."); yield break; }

            var bm = BuildModeController.Instance;
            if (bm.IsBuildModeActive) { bm.ExitBuildMode(); yield return null; }
            Assert.IsFalse(bm.IsBuildModeActive, "Build Mode should start inactive.");

            bm.Toggle();
            yield return null;
            Assert.IsTrue(BuildModeController.IsActive, "Toggle must enter Build Mode.");

            bm.Toggle();
            yield return null;
            Assert.IsFalse(BuildModeController.IsActive, "Second toggle must exit Build Mode.");
        }

        [UnityTest]
        public IEnumerator F2_IsOwnedBy_BuildModeController()
        {
            // Item 1+2: the single-owner registry must report BuildModeController as the F2 owner.
            var bm = BuildModeController.Instance;
            yield return null;
            Assert.IsTrue(InPlayToolKeyRegistry.Owns(UnityEngine.InputSystem.Key.F2, bm),
                "BuildModeController must be the sole registered F2 owner.");
        }

        [UnityTest]
        public IEnumerator LegacyOverlay_IsRetired_NotPresent()
        {
            // Item 2: the legacy IMGUI overlay must NOT self-bootstrap (Bootstrap is behind an
            // off-by-default define), so a fresh play session has no instance polling F2.
            yield return null;
            var legacy = Object.FindObjectOfType<RIMA.DevTools.InPlayMapPaintOverlay>();
            Assert.IsNull(legacy, "Legacy InPlayMapPaintOverlay must not self-activate (F2 conflict retired).");
        }

        [UnityTest]
        public IEnumerator OverlapHide_DisablesOtherCanvas_RestoresOnExit()
        {
            if (!CanEnterBuildMode()) { Assert.Ignore("No DirectorMode/Camera.main in this session."); yield break; }

            var dummyGo = new GameObject("DummyOverlapCanvas");
            var dummy = dummyGo.AddComponent<Canvas>();
            dummy.renderMode = RenderMode.ScreenSpaceOverlay;
            dummy.enabled = true;

            var offGo = new GameObject("PreDisabledCanvas");
            var off = offGo.AddComponent<Canvas>();
            off.renderMode = RenderMode.ScreenSpaceOverlay;
            off.enabled = false;

            yield return null;

            var bm = BuildModeController.Instance;
            if (bm.IsBuildModeActive) { bm.ExitBuildMode(); yield return null; }
            bm.EnterBuildMode();
            yield return null;

            Assert.IsFalse(dummy.enabled, "An enabled foreign canvas must be hidden in Build Mode.");

            bm.ExitBuildMode();
            yield return null;

            Assert.IsTrue(dummy.enabled, "The hidden canvas must be restored exactly on exit.");
            Assert.IsFalse(off.enabled, "A canvas that was already disabled must remain disabled.");

            Object.Destroy(dummyGo);
            Object.Destroy(offGo);
        }

        [UnityTest]
        public IEnumerator Enter_CreatesWorkingCopy_OrNoOpWithoutSource()
        {
            if (!CanEnterBuildMode()) { Assert.Ignore("No DirectorMode/Camera.main in this session."); yield break; }

            var director = Object.FindObjectOfType<RoomRunDirector>();
            var bm = BuildModeController.Instance;
            if (bm.IsBuildModeActive) { bm.ExitBuildMode(); yield return null; }
            bm.EnterBuildMode();
            yield return null;

            if (director != null && director.CurrentTemplate != null)
            {
                // With a live source template the working copy is a DIFFERENT instance (no pollution).
                Assert.IsNotNull(BuildModeController.ActiveWorkingTemplate, "A working copy must exist in Build Mode.");
                Assert.AreNotSame(director.CurrentTemplate, BuildModeController.ActiveWorkingTemplate,
                    "The working copy must be a different instance than the source.");
            }

            bm.ExitBuildMode();
            yield return null;
            Assert.IsNull(BuildModeController.ActiveWorkingTemplate, "The working copy must be gone after exit.");
        }

        [UnityTest]
        public IEnumerator ToolExclusivity_TileHidesPropGhost_PropRestores()
        {
            if (!CanEnterBuildMode()) { Assert.Ignore("No DirectorMode/Camera.main in this session."); yield break; }

            var bm = BuildModeController.Instance;
            if (bm.IsBuildModeActive) { bm.ExitBuildMode(); yield return null; }
            bm.EnterBuildMode();
            yield return null;

            var pc = BuildPlacementController.Instance;
            pc.SelectToolForValidation(1); // Tile
            yield return null;
            Assert.AreEqual(1, pc.ActiveToolForValidation(), "Active tool must be Tile.");
            Assert.IsFalse(pc.HasGhostForValidation(), "Prop ghost must be hidden while the Tile tool is active.");

            pc.SelectToolForValidation(0); // Prop
            yield return null;
            Assert.AreEqual(0, pc.ActiveToolForValidation(), "Active tool must be Prop.");

            bm.ExitBuildMode();
            yield return null;
        }

        [UnityTest]
        public IEnumerator GridOverlay_ActiveInBuildMode_InactiveOnExit()
        {
            if (!CanEnterBuildMode()) { Assert.Ignore("No DirectorMode/Camera.main in this session."); yield break; }

            var bm = BuildModeController.Instance;
            if (bm.IsBuildModeActive) { bm.ExitBuildMode(); yield return null; }
            bm.EnterBuildMode();
            yield return null;
            yield return null; // overlay builds on the first refresh.

            var pc = BuildPlacementController.Instance;
            Assert.IsTrue(pc.GridOverlayActiveForValidation(),
                "Iso-grid overlay must be active in Build Mode (a Grid is present in the test scene).");

            bm.ExitBuildMode();
            yield return null;
            Assert.IsFalse(pc.GridOverlayActiveForValidation(), "Iso-grid overlay must be inactive after exit.");
        }

        [UnityTest]
        public IEnumerator Save_WithoutWorkingCopy_ReturnsFalse()
        {
            var bm = BuildModeController.Instance;
            if (bm.IsBuildModeActive) { bm.ExitBuildMode(); yield return null; }
            Assert.IsFalse(bm.SaveForValidation(), "Save with no working copy must return false (safe no-op).");
        }
    }
}
