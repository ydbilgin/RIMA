#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using RIMA.CameraSystem;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using RIMA.UI.BuildMode;
using UnityEngine;
using UnityEngine.InputSystem;
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

        // Self-bootstrap so the F2/quote poll in Update() runs without scene wiring. Previously the
        // controller was created lazily by DirectorMode's quote branch; that branch moved here, so
        // nothing else would instantiate it. Mirrors DirectorMode.Bootstrap. Play-mode only, so the
        // edit-mode DontDestroyOnLoad concern never applies. Uses the lazy Instance getter, which
        // find-or-creates a DontDestroyOnLoad controller (DisableDomainReload-safe via the alive check).
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            _ = Instance;
        }

        // Non-creating active check for callers that must not trigger lazy creation.
        public static bool IsActive => _instance != null && _instance.IsBuildModeActive;

        // PHASE 3.1 (audit MAJOR fix): the ONE shared working copy for the whole Build Mode session.
        // Owned here (the lifecycle owner): created on EnterBuildMode (deep-copies the source's
        // walkableGrid bool[] / overlayMask int[] / props List via Object.Instantiate), destroyed on
        // ExitBuildMode + OnDestroy. BOTH tools (prop placement + tile/walkability brush) edit THIS
        // single instance and ALWAYS point WalkabilityMap at it — so the live ground Tilemap, the
        // pathing authority and the prop validator can never disagree, and the source .asset is never
        // mutated/dirtied (disk write-back is Phase 4 on explicit command). DisableDomainReload-safe
        // (DontSave hideFlags + nulled on teardown).
        public RoomTemplateSO WorkingTemplate { get; private set; }

        // Non-creating accessor for the active session's working copy.
        public static RoomTemplateSO ActiveWorkingTemplate =>
            _instance != null && _instance.IsBuildModeActive ? _instance.WorkingTemplate : null;

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

        // OVERLAP FIX: every OTHER root UI canvas we disabled on enter, restored verbatim on exit.
        // Build Mode draws over the live game world; without this the reward/draft screen, HUD and
        // class-select bleed THROUGH and around the build panels (unreadable overlap). We disable
        // Canvas.enabled (NOT the GameObject) so each canvas keeps its full hierarchy/state and a
        // simple re-enable restores it. Our own two overlay canvases + the EventSystem are exempt.
        private readonly List<Canvas> hiddenCanvases = new List<Canvas>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            // CONSOLIDATION item 1+2: claim F2 (+ quote) as the SOLE owner of the Build-Mode toggle.
            // The legacy IMGUI InPlayMapPaintOverlay no longer self-activates or polls F2 (retired
            // behind RIMA_LEGACY_MAPPAINT, off by default), so this controller is the only claimant.
            // The registry surfaces a clear error if anything else ever tries to re-poll these keys.
            InPlayToolKeyRegistry.RegisterExclusive(Key.F2, this);
            InPlayToolKeyRegistry.RegisterExclusive(Key.Quote, this);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }

            // Release our key claims so a re-created controller (DisableDomainReload) can re-own them.
            InPlayToolKeyRegistry.Release(Key.F2, this);
            InPlayToolKeyRegistry.Release(Key.Quote, this);

            // Safety: never leave the rig in the disabled "build" state if this is torn down mid-mode.
            RestoreCameraRig();

            // Safety: never leave other UI canvases hidden if this is torn down mid-mode.
            RestoreOtherUiCanvases();

            // Safety: never leave the Phase 2 placement layer active if torn down mid-mode.
            if (BuildPlacementController.InstanceIfExists != null)
            {
                BuildPlacementController.InstanceIfExists.SetBuildModeActive(false);
            }

            // Safety: never leak the runtime working copy if torn down mid-mode.
            DestroyWorkingTemplate();
        }

        // Layout-independent Build-Mode toggle. F2 is the primary key because its physical position
        // is identical on every keyboard layout (the previous quoteKey did not map to the "
        // a Turkish-layout user presses, so Build Mode never opened). quoteKey is kept for US layouts.
        // Runs on THIS live instance every frame regardless of DirectorMode state; uses members
        // directly (never the lazy Instance getter, which would DontDestroyOnLoad-throw in edit mode).
        private void Update()
        {
            DirectorMode.EnsureRuntimeInstanceForCurrentScene();

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            // Only the registered owner polls the toggle keys (single-owner guard). If a stale
            // duplicate exists under DisableDomainReload, re-claim so the live instance keeps the key.
            if (keyboard.f2Key.wasPressedThisFrame && EnsureOwns(Key.F2))
            {
                Toggle();
            }
            else if (keyboard.quoteKey.wasPressedThisFrame && EnsureOwns(Key.Quote))
            {
                Toggle();
            }
        }

        // Confirm this live instance owns the key; reclaim a key whose recorded owner is gone
        // (DisableDomainReload can leave a stale claim from a previous play session).
        private bool EnsureOwns(Key key)
        {
            if (InPlayToolKeyRegistry.Owns(key, this)) return true;
            return InPlayToolKeyRegistry.RegisterExclusive(key, this);
        }

        /// <summary>Build-Mode toggle entry point (driven by this controller's Update on F2 / quote).</summary>
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

            // Guard: do not enter Build Mode while a draft or overlay is open (centerpiece protection).
            if ((UIManager.Instance != null && UIManager.Instance.IsAnyOverlayOpen) ||
                (DraftManager.Instance != null && (DraftManager.Instance.IsDraftActive || DraftManager.Instance.IsDraftPending)))
                return;

            DirectorMode director = DirectorMode.Instance ?? DirectorMode.EnsureRuntimeInstanceForCurrentScene();
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

            // PHASE 3.1: create the ONE shared working copy before any tool spins up, so both the
            // prop tool and the tile brush edit the same instance for the whole session.
            CreateWorkingTemplate();

            // Phase 2 placement layer: enable the PropRegistry-driven palette + iso-grid ghost +
            // place/erase/rotate/flip/eyedropper/undo. Active ONLY while Build Mode is active.
            // (Builds the palette overlay canvas, which the hide pass below must EXEMPT.)
            BuildPlacementController.Instance.SetBuildModeActive(true);

            // OVERLAP FIX: now that our palette canvas exists, hide every OTHER active root UI canvas
            // so only the Build Mode UI + the live game world are visible (no reward/HUD/class bleed).
            HideOtherUiCanvases();

            StartZoom(buildOverviewOrthoSize, restoreRigOnComplete: false);
        }

        public void ExitBuildMode()
        {
            if (!IsBuildModeActive)
            {
                return;
            }

            IsBuildModeActive = false;

            // OVERLAP FIX: bring back every other UI canvas we hid on enter (reward/HUD/class-select).
            RestoreOtherUiCanvases();

            // Disable the Phase 2 placement layer first (hides ghost + palette, drops undo history).
            // Authored props persist in the room template; only editor-only state is dropped.
            if (BuildPlacementController.InstanceIfExists != null)
            {
                BuildPlacementController.InstanceIfExists.SetBuildModeActive(false);
            }

            // PHASE 3.1: both tools are now down, so destroy the shared working copy. All session
            // edits lived only on this runtime instance; the source .asset was never touched.
            DestroyWorkingTemplate();

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

        // PHASE 3.1: deep-copy the live source template into the session working copy. Object.Instantiate
        // on a ScriptableObject copies the serialized fields by value (walkableGrid bool[], overlayMask
        // int[] and the props List are all duplicated), so every edit lands on the clone and the source
        // .asset stays pristine. hideFlags = DontSave keeps the clone out of any scene/asset save.
        private void CreateWorkingTemplate()
        {
            DestroyWorkingTemplate();

            RoomRunDirector director = FindObjectOfType<RoomRunDirector>();
            RoomTemplateSO source = director != null ? director.CurrentTemplate : null;
            if (source == null)
            {
                Debug.LogWarning("[BuildMode] No active RoomTemplate; tools will operate without a working copy.");
                return;
            }

            WorkingTemplate = Object.Instantiate(source);
            WorkingTemplate.hideFlags = HideFlags.DontSave;
            WorkingTemplate.name = source.name + " (BuildWorkingCopy)";
        }

        /// <summary>
        /// CONSOLIDATION item 3 — explicit Save/Apply. Copies the session working copy back onto its
        /// SOURCE RoomTemplateSO .asset (the live RoomRunDirector.CurrentTemplate) and persists it to
        /// disk. The source is dirtied ONLY here, never during a session (the working copy stays a
        /// DontSave clone until this is called), so unsaved edits are discarded on exit as designed.
        ///
        /// Editor-only write: EditorUtility.CopySerialized restores name + hideFlags afterwards so the
        /// CopySerialized (which would otherwise overwrite them with the clone's "(BuildWorkingCopy)"
        /// name / DontSave flag) cannot orphan the asset. In a player build it is a logged no-op.
        /// Returns true if a write actually happened.
        /// </summary>
        public bool SaveWorkingTemplate()
        {
            if (WorkingTemplate == null)
            {
                Debug.LogWarning("[BuildMode] SaveWorkingTemplate: no working copy to save (enter Build Mode first).");
                return false;
            }

            RoomRunDirector director = FindObjectOfType<RoomRunDirector>();
            RoomTemplateSO source = director != null ? director.CurrentTemplate : null;
            if (source == null)
            {
                Debug.LogWarning("[BuildMode] SaveWorkingTemplate: no source RoomTemplate to write back to.");
                return false;
            }

#if UNITY_EDITOR
            // Preserve the source asset's identity across the field-by-field copy.
            string sourceName = source.name;
            HideFlags sourceFlags = source.hideFlags;

            UnityEditor.EditorUtility.CopySerialized(WorkingTemplate, source);

            source.name = sourceName;
            source.hideFlags = sourceFlags;

            UnityEditor.EditorUtility.SetDirty(source);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log($"[BuildMode] Saved working copy back to source template '{sourceName}'.");
            return true;
#else
            // Player build: no AssetDatabase. Persisting authored rooms at runtime is a post-demo
            // concern (JSON sidecar path); for now Save is a logged no-op outside the Editor.
            Debug.Log("[BuildMode] SaveWorkingTemplate is editor-only; ignored in a player build.");
            return false;
#endif
        }

        /// <summary>Data-proof hook for the regression suite (no screenshot). Mirrors SaveWorkingTemplate.</summary>
        public bool SaveForValidation() => SaveWorkingTemplate();

        private void DestroyWorkingTemplate()
        {
            if (WorkingTemplate == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(WorkingTemplate);
            }
            else
            {
                DestroyImmediate(WorkingTemplate);
            }
            WorkingTemplate = null;
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

        /// <summary>
        /// OVERLAP FIX. Hide every OTHER active root UI canvas in the scene so Build Mode shows only
        /// its own panels over the live game world. Robust rule: FindObjectsOfType&lt;Canvas&gt;(true) ->
        /// keep only ones that are a root canvas, currently enabled, and NOT one of ours; flip
        /// Canvas.enabled = false (never the GameObject, so the canvas keeps its full state) and record
        /// it. Our two overlay canvases (palette + tile brush) are exempt by reference; the EventSystem
        /// is never a Canvas so it is untouched, and the cursor / ghost world objects are not canvases.
        /// </summary>
        private void HideOtherUiCanvases()
        {
            hiddenCanvases.Clear();

            Canvas ownPalette = BuildPlacementController.Instance != null
                ? BuildPlacementController.Instance.OwnCanvas : null;
            Canvas ownBrush = BuildTileBrushController.InstanceIfExists != null
                ? BuildTileBrushController.InstanceIfExists.OwnCanvas : null;

            Canvas[] all = FindObjectsOfType<Canvas>(true);
            foreach (Canvas c in all)
            {
                if (c == null) continue;
                if (!c.isRootCanvas) continue;      // child/nested canvases follow their root.
                if (!c.enabled) continue;           // already off; leave it off on restore.
                if (c == ownPalette || c == ownBrush) continue; // never hide our own UI.

                c.enabled = false;
                hiddenCanvases.Add(c);
            }
        }

        /// <summary>OVERLAP FIX. Re-enable exactly the canvases HideOtherUiCanvases disabled.</summary>
        private void RestoreOtherUiCanvases()
        {
            for (int i = 0; i < hiddenCanvases.Count; i++)
            {
                Canvas c = hiddenCanvases[i];
                if (c != null) c.enabled = true; // tolerant of canvases destroyed while in Build Mode.
            }
            hiddenCanvases.Clear();
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
