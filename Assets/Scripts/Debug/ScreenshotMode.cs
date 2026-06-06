#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RIMA.DebugTools
{
    public sealed class ScreenshotMode : MonoBehaviour
    {
        public const string OutputFolderName = "screenshots_auto";

        private static ScreenshotMode instance;
        private static readonly List<Entry> Entries = new List<Entry>();
        private static readonly CameraPreset[] Presets =
        {
            new CameraPreset("chamber-wide", PresetKind.Bounds, 1.15f, Vector2.zero),
            new CameraPreset("pedestal-close", PresetKind.NamedObject, 2.2f, new Vector2(0f, 0.25f), "EchoStatue_", "EchoLabel_"),
            new CameraPreset("combat", PresetKind.PlayerEnemy, 4.2f, Vector2.zero),
            new CameraPreset("draft", PresetKind.NamedObject, 3.4f, Vector2.zero, "RewardPickup", "Draft"),
            new CameraPreset("doors", PresetKind.NamedObject, 3.0f, new Vector2(0f, 0.25f), "ExitDoor_", "Door_"),
            new CameraPreset("room-overview", PresetKind.Bounds, 1.35f, Vector2.zero),
        };

        [SerializeField] private bool captureHudVariant = true;
        [SerializeField] private bool captureNoHudVariant = true;
        [SerializeField] private int superSize = 1;

        private int presetIndex;
        private bool isCapturing;

        public static bool IsEnabled { get; private set; }
        public static string CurrentPresetName => Presets[Ensure().presetIndex].name;
        public static int RegisteredCount => Entries.Count;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            Ensure();
        }

        public static void Register(GameObject surface, string label = null)
        {
            if (surface == null) return;
            Prune();

            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].surface == surface) return;
            }

            Entries.Add(new Entry(surface, label));
            ApplyEntry(Entries[Entries.Count - 1]);
        }

        public static void Unregister(GameObject surface)
        {
            if (surface == null) return;

            for (int i = Entries.Count - 1; i >= 0; i--)
            {
                if (Entries[i].surface != surface) continue;
                Entries.RemoveAt(i);
            }
        }

        public static void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
            Prune();
            for (int i = 0; i < Entries.Count; i++)
            {
                ApplyEntry(Entries[i]);
            }
        }

        public static void Toggle()
        {
            SetEnabled(!IsEnabled);
        }

        public static void ApplyPresetByName(string presetName)
        {
            ScreenshotMode mode = Ensure();
            for (int i = 0; i < Presets.Length; i++)
            {
                if (!string.Equals(Presets[i].name, presetName, StringComparison.OrdinalIgnoreCase)) continue;
                mode.presetIndex = i;
                mode.ApplyCurrentPreset();
                return;
            }
        }

        public static void CaptureAllPresets()
        {
            ScreenshotMode mode = Ensure();
            if (!mode.isCapturing)
            {
                mode.StartCoroutine(mode.CaptureAllRoutine());
            }
        }

        public static void CaptureCurrentPreset()
        {
            ScreenshotMode mode = Ensure();
            if (!mode.isCapturing)
            {
                mode.StartCoroutine(mode.CaptureCurrentRoutine());
            }
        }

        private static ScreenshotMode Ensure()
        {
            if (instance != null) return instance;

            instance = FindFirstObjectByType<ScreenshotMode>();
            if (instance != null) return instance;

            GameObject host = new GameObject("[ScreenshotMode]");
            DontDestroyOnLoad(host);
            instance = host.AddComponent<ScreenshotMode>();
            return instance;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.f12Key.wasPressedThisFrame)
            {
                Toggle();
            }

            if (keyboard.f11Key.wasPressedThisFrame)
            {
                CyclePreset();
            }

            if (keyboard.f10Key.wasPressedThisFrame)
            {
                CaptureCurrentPreset();
            }
        }

        private void CyclePreset()
        {
            presetIndex = (presetIndex + 1) % Presets.Length;
            ApplyCurrentPreset();
        }

        private void ApplyCurrentPreset()
        {
            Camera camera = Camera.main;
            if (camera == null) return;

            Presets[presetIndex].Apply(camera);
            Debug.Log($"[ScreenshotMode] Preset {Presets[presetIndex].name}");
        }

        private IEnumerator CaptureAllRoutine()
        {
            isCapturing = true;
            int originalPreset = presetIndex;

            for (int i = 0; i < Presets.Length; i++)
            {
                presetIndex = i;
                ApplyCurrentPreset();
                yield return null;
                yield return CapturePresetVariants(Presets[i].name);
            }

            presetIndex = originalPreset;
            ApplyCurrentPreset();
            isCapturing = false;
        }

        private IEnumerator CaptureCurrentRoutine()
        {
            isCapturing = true;
            ApplyCurrentPreset();
            yield return null;
            yield return CapturePresetVariants(Presets[presetIndex].name);
            isCapturing = false;
        }

        private IEnumerator CapturePresetVariants(string presetName)
        {
            if (captureHudVariant)
            {
                Capture(presetName, "hud");
                yield return null;
            }

            if (captureNoHudVariant)
            {
                CanvasState[] canvasStates = SetCanvasesVisible(false);
                Capture(presetName, "nohud");
                yield return null;
                RestoreCanvases(canvasStates);
            }
        }

        private void Capture(string presetName, string variant)
        {
            string folder = Path.Combine(ProjectRoot(), "STAGING", OutputFolderName);
            Directory.CreateDirectory(folder);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string path = Path.Combine(folder, $"{presetName}_{variant}_{timestamp}.png");
            ScreenCapture.CaptureScreenshot(path, Mathf.Clamp(superSize, 1, 2));
            Debug.Log($"[ScreenshotMode] Captured {path}");
        }

        private static CanvasState[] SetCanvasesVisible(bool visible)
        {
            Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<CanvasState> states = new List<CanvasState>(canvases.Length);
            for (int i = 0; i < canvases.Length; i++)
            {
                Canvas canvas = canvases[i];
                if (canvas == null) continue;
                states.Add(new CanvasState(canvas, canvas.enabled));
                canvas.enabled = visible;
            }

            return states.ToArray();
        }

        private static void RestoreCanvases(CanvasState[] states)
        {
            if (states == null) return;
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].canvas != null)
                {
                    states[i].canvas.enabled = states[i].wasEnabled;
                }
            }
        }

        private static string ProjectRoot()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }

        private static void ApplyEntry(Entry entry)
        {
            if (entry.surface == null) return;
            entry.surface.SetActive(IsEnabled ? false : entry.originalActive);
        }

        private static void Prune()
        {
            for (int i = Entries.Count - 1; i >= 0; i--)
            {
                if (Entries[i].surface == null)
                {
                    Entries.RemoveAt(i);
                }
            }
        }

        private enum PresetKind
        {
            Bounds,
            NamedObject,
            PlayerEnemy
        }

        private readonly struct Entry
        {
            public readonly GameObject surface;
            public readonly bool originalActive;
            public readonly string label;

            public Entry(GameObject surface, string label)
            {
                this.surface = surface;
                this.originalActive = surface.activeSelf;
                this.label = label;
            }
        }

        private readonly struct CanvasState
        {
            public readonly Canvas canvas;
            public readonly bool wasEnabled;

            public CanvasState(Canvas canvas, bool wasEnabled)
            {
                this.canvas = canvas;
                this.wasEnabled = wasEnabled;
            }
        }

        private readonly struct CameraPreset
        {
            public readonly string name;
            private readonly PresetKind kind;
            private readonly float orthoSize;
            private readonly Vector2 offset;
            private readonly string primaryName;
            private readonly string fallbackName;

            public CameraPreset(string name, PresetKind kind, float orthoSize, Vector2 offset, string primaryName = null, string fallbackName = null)
            {
                this.name = name;
                this.kind = kind;
                this.orthoSize = orthoSize;
                this.offset = offset;
                this.primaryName = primaryName;
                this.fallbackName = fallbackName;
            }

            public void Apply(Camera camera)
            {
                camera.orthographic = true;
                Vector3 target = ResolveTarget(camera.transform.position);
                camera.transform.position = new Vector3(target.x + offset.x, target.y + offset.y, camera.transform.position.z);
                camera.orthographicSize = Mathf.Max(1f, ResolveOrtho(camera));
            }

            private float ResolveOrtho(Camera camera)
            {
                if (kind != PresetKind.Bounds) return orthoSize;
                if (!TryResolveBounds(out Bounds bounds)) return orthoSize;

                float aspect = camera.aspect > 0f ? camera.aspect : 16f / 9f;
                float halfHeight = bounds.extents.y * orthoSize;
                float halfWidthAsHeight = bounds.extents.x * orthoSize / aspect;
                return Mathf.Max(halfHeight, halfWidthAsHeight, 1f);
            }

            private Vector3 ResolveTarget(Vector3 fallback)
            {
                if (kind == PresetKind.Bounds && TryResolveBounds(out Bounds bounds))
                {
                    return bounds.center;
                }

                if (kind == PresetKind.PlayerEnemy && TryResolvePlayerEnemy(out Vector3 combatTarget))
                {
                    return combatTarget;
                }

                if (TryFindNamed(primaryName, out Vector3 namedTarget) || TryFindNamed(fallbackName, out namedTarget))
                {
                    return namedTarget;
                }

                return fallback;
            }

            private static bool TryResolveBounds(out Bounds bounds)
            {
                bounds = default;
                Renderer[] renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                bool initialized = false;
                for (int i = 0; i < renderers.Length; i++)
                {
                    Renderer renderer = renderers[i];
                    if (renderer == null || renderer is ParticleSystemRenderer) continue;
                    if (!initialized)
                    {
                        bounds = renderer.bounds;
                        initialized = true;
                    }
                    else
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }

                return initialized;
            }

            private static bool TryResolvePlayerEnemy(out Vector3 target)
            {
                target = default;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                if (player != null && enemy != null)
                {
                    target = (player.transform.position + enemy.transform.position) * 0.5f;
                    return true;
                }

                if (player != null)
                {
                    target = player.transform.position;
                    return true;
                }

                if (enemy != null)
                {
                    target = enemy.transform.position;
                    return true;
                }

                return false;
            }

            private static bool TryFindNamed(string prefix, out Vector3 target)
            {
                target = default;
                if (string.IsNullOrEmpty(prefix)) return false;

                Transform[] transforms = FindObjectsByType<Transform>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                for (int i = 0; i < transforms.Length; i++)
                {
                    Transform transform = transforms[i];
                    if (transform != null && transform.name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        target = transform.position;
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
#endif
