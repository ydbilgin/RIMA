#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections;
using RIMA.CameraSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RIMA
{
    /// <summary>
    /// Phase 1 of the in-game Build Mode (design doc:
    /// STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md, section 4 + Phase 1).
    ///
    /// Quote key (") is the polished Build-Mode alias on top of DirectorMode's raw backquote
    /// toggle. Entering pulls the camera back ("enter build" gesture), pauses gameplay via
    /// DirectorMode's existing Director state, and forces the Build tab. Exiting lerps the
    /// camera back and hands ortho-size ownership back to CameraZoom, which re-quantizes to its
    /// crisp pixel-perfect ratio on re-enable (PPU 64 stays crisp, zero PixelPerfect pop).
    ///
    /// Camera capture/restore copies the shipped precedent in ChamberSelectBootstrap
    /// (ConfigureCameraAndLight ~1454-1483, OnDestroy restore ~199-221): disable CameraZoom +
    /// PixelPerfectCamera, override ortho size, re-target/neutralize CameraFollow, restore on exit.
    ///
    /// SCOPE: camera + input + tab only. No tile/prop/light placement, no save, no palette.
    /// Free-cam panning (WASD / arrows) is DirectorMode.UpdateFreeCamera, which runs automatically
    /// while DirectorMode is in the Director state; this controller does not add a new pan system.
    /// </summary>
    public sealed class BuildModeController : MonoBehaviour
    {
        private static BuildModeController _instance;

        // Lazy find-or-create. Under Enter Play Mode Options = DisableDomainReload, statics survive
        // across play sessions, so a stale fake-null reference could leak. Resolving on demand
        // (alive field -> FindObjectOfType -> create DontDestroyOnLoad) guarantees the quote key
        // always lands on a live controller without scene wiring.
        public static BuildModeController Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindObjectOfType<BuildModeController>();
                if (_instance != null)
                {
                    return _instance;
                }

                GameObject go = new GameObject("BuildModeController");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<BuildModeController>();
                return _instance;
            }
        }

        // Non-creating active check for callers that must not trigger lazy creation.
        public static bool IsActive => _instance != null && _instance.IsBuildModeActive;

        [Header("Build framing")]
        [Tooltip("Camera orthographic size used while Build Mode is active (wider 'enter build' framing).")]
        [SerializeField, Min(1f)] private float buildOverviewOrthoSize = 9f;
        [Tooltip("Seconds to glide the camera ortho size in/out of the build framing.")]
        [SerializeField, Min(0.01f)] private float zoomLerpDuration = 0.35f;

        public bool IsBuildModeActive { get; private set; }

        private Camera buildCamera;
        private CameraZoom disabledCameraZoom;
        private Behaviour disabledLegacyPixelPerfectCamera;
        private PixelPerfectCamera disabledUrpPixelPerfectCamera;
        // Fully qualified: RIMA namespace also has an obsolete RIMA.CameraFollow (Player/), the live
        // rig is RIMA.CameraSystem.CameraFollow (the one with SnapToTarget) — same as ChamberSelect.
        private RIMA.CameraSystem.CameraFollow disabledCameraFollow;

        private float capturedOrthoSize;
        private DirectorModeState capturedDirectorState;
        private DirectorTab capturedDirectorTab;
        private bool stateCaptured;

        private Coroutine zoomRoutine;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }

            // Safety: never leave the rig in the disabled "build" state if this is torn down mid-mode.
            RestoreCameraRig();
        }

        /// <summary>Quote-key entry point. Wired from DirectorMode.Update() as a sibling to backquote.</summary>
        public void Toggle()
        {
            if (IsBuildModeActive)
            {
                ExitBuildMode();
            }
            else
            {
                EnterBuildMode();
            }
        }

        public void EnterBuildMode()
        {
            if (IsBuildModeActive)
            {
                return;
            }

            DirectorMode director = DirectorMode.Instance;
            if (director == null)
            {
                Debug.LogWarning("[BuildMode] DirectorMode.Instance is null; cannot enter Build Mode.");
                return;
            }

            buildCamera = Camera.main;
            if (buildCamera == null)
            {
                Debug.LogWarning("[BuildMode] Camera.main is null; cannot enter Build Mode.");
                return;
            }

            IsBuildModeActive = true;

            // Capture DirectorMode state/tab so EXIT restores exactly what the player had.
            capturedDirectorState = director.State;
            capturedDirectorTab = director.ActiveTab;
            stateCaptured = true;

            // Capture + disable the camera rig (ChamberSelect precedent).
            capturedOrthoSize = buildCamera.orthographicSize;

            CameraZoom zoom = buildCamera.GetComponent<CameraZoom>();
            if (zoom != null && zoom.enabled)
            {
                zoom.enabled = false;
                disabledCameraZoom = zoom;
            }

            Behaviour legacyPpc = FindLegacyPixelPerfectCamera(buildCamera);
            if (legacyPpc != null && legacyPpc.enabled)
            {
                legacyPpc.enabled = false;
                disabledLegacyPixelPerfectCamera = legacyPpc;
            }

            PixelPerfectCamera urpPpc = buildCamera.GetComponent<PixelPerfectCamera>();
            if (urpPpc != null && urpPpc.enabled)
            {
                urpPpc.enabled = false;
                disabledUrpPixelPerfectCamera = urpPpc;
            }

            // Neutralize CameraFollow so DirectorMode.UpdateFreeCamera owns the camera transform
            // while in Build Mode (CameraFollow.LateUpdate would otherwise fight free-cam panning).
            RIMA.CameraSystem.CameraFollow follow = buildCamera.GetComponent<RIMA.CameraSystem.CameraFollow>();
            if (follow != null && follow.enabled)
            {
                follow.enabled = false;
                disabledCameraFollow = follow;
            }

            // Pause gameplay + force the Build tab via DirectorMode's existing API.
            director.SetState(DirectorModeState.Director);
            director.ShowTab(DirectorTab.Build);

            StartZoom(buildOverviewOrthoSize, restoreRigOnComplete: false);
        }

        public void ExitBuildMode()
        {
            if (!IsBuildModeActive)
            {
                return;
            }

            IsBuildModeActive = false;

            // Restore DirectorMode to whatever state/tab the player was in before Build Mode.
            DirectorMode director = DirectorMode.Instance;
            if (director != null && stateCaptured)
            {
                director.SetState(capturedDirectorState);
                director.ShowTab(capturedDirectorTab);
            }
            stateCaptured = false;

            // Lerp ortho back to the captured (already-crisp) size, then hand control to CameraZoom.
            StartZoom(capturedOrthoSize, restoreRigOnComplete: true);
        }

        private void StartZoom(float targetOrthoSize, bool restoreRigOnComplete)
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            if (buildCamera == null)
            {
                if (restoreRigOnComplete)
                {
                    RestoreCameraRig();
                }
                return;
            }

            zoomRoutine = StartCoroutine(ZoomRoutine(targetOrthoSize, restoreRigOnComplete));
        }

        private IEnumerator ZoomRoutine(float targetOrthoSize, bool restoreRigOnComplete)
        {
            float start = buildCamera.orthographicSize;
            float elapsed = 0f;

            // Build Mode runs under Time.timeScale 0, so glide on unscaled time.
            while (elapsed < zoomLerpDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / zoomLerpDuration));
                buildCamera.orthographicSize = Mathf.Lerp(start, targetOrthoSize, t);
                yield return null;
            }

            buildCamera.orthographicSize = targetOrthoSize;
            zoomRoutine = null;

            if (restoreRigOnComplete)
            {
                RestoreCameraRig();
            }
        }

        /// <summary>
        /// Re-enable everything captured on enter. CameraZoom reclaims ortho-size ownership and,
        /// because its last target was already snapped to a crisp pixel ratio, ApplyPixelPerfect
        /// re-quantizes the PixelPerfectCamera with no pop (PPU 64 stays crisp). CameraFollow snaps
        /// to its target so the next frame does not pan across the room.
        /// </summary>
        private void RestoreCameraRig()
        {
            if (disabledUrpPixelPerfectCamera != null)
            {
                disabledUrpPixelPerfectCamera.enabled = true;
                disabledUrpPixelPerfectCamera = null;
            }

            if (disabledLegacyPixelPerfectCamera != null)
            {
                disabledLegacyPixelPerfectCamera.enabled = true;
                disabledLegacyPixelPerfectCamera = null;
            }

            if (disabledCameraFollow != null)
            {
                disabledCameraFollow.enabled = true;
                disabledCameraFollow.SnapToTarget();
                disabledCameraFollow = null;
            }

            if (disabledCameraZoom != null)
            {
                disabledCameraZoom.enabled = true;
                disabledCameraZoom = null;
            }
        }

        // Mirrors ChamberSelectBootstrap.FindLegacyPixelPerfectCamera: the deprecated 2D-Extras
        // PixelPerfectCamera lives in a different type than the URP one, so we resolve it by name.
        private static Behaviour FindLegacyPixelPerfectCamera(Camera camera)
        {
            if (camera == null)
            {
                return null;
            }

            foreach (Behaviour behaviour in camera.GetComponents<Behaviour>())
            {
                if (behaviour != null && behaviour.GetType().FullName == "UnityEngine.U2D.PixelPerfectCamera")
                {
                    return behaviour;
                }
            }

            return null;
        }
    }
}
#endif
