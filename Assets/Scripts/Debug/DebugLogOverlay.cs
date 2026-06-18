#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA.DebugTools
{
    // F3 in-game log overlay: shows Debug.Log/Warning/Error on screen during play (demo feedback +
    // live error catching). Mirrors DemoDebugPanel bootstrap so it self-installs after scene load.
    public sealed class DebugLogOverlay : MonoBehaviour
    {
        private const int MaxEntries = 60;
        private const int MaxStackLines = 4;

        private readonly struct LogLine
        {
            public readonly string text;
            public readonly LogType type;

            public LogLine(string text, LogType type)
            {
                this.text = text;
                this.type = type;
            }
        }

        private readonly Queue<LogLine> _lines = new Queue<LogLine>(MaxEntries);
        private bool _visible;
        private Vector2 _scroll;
        private GUIStyle _style;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (FindFirstObjectByType<DebugLogOverlay>() != null) return;

            GameObject overlay = new GameObject("DebugLogOverlay");
            DontDestroyOnLoad(overlay);
            overlay.AddComponent<DebugLogOverlay>();
        }

        private void Awake()
        {
            ScreenshotMode.Register(gameObject, nameof(DebugLogOverlay));
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void OnDestroy()
        {
            ScreenshotMode.Unregister(gameObject);
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.f3Key.wasPressedThisFrame)
            {
                _visible = !_visible;
            }
        }

        private void HandleLog(string message, string stackTrace, LogType type)
        {
            string text = message;
            if ((type == LogType.Error || type == LogType.Exception || type == LogType.Assert) &&
                !string.IsNullOrEmpty(stackTrace))
            {
                text += "\n" + FirstStackLines(stackTrace);
            }

            if (_lines.Count >= MaxEntries) _lines.Dequeue();
            _lines.Enqueue(new LogLine(text, type));
            _scroll.y = float.MaxValue;
        }

        private static string FirstStackLines(string stackTrace)
        {
            string[] split = stackTrace.Split('\n');
            int count = Mathf.Min(MaxStackLines, split.Length);
            return string.Join("\n", split, 0, count).TrimEnd();
        }

        private void OnGUI()
        {
            if (!_visible) return;

            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label) { fontSize = 11, wordWrap = true, richText = false };
            }

            float width = Mathf.Min(420f, Screen.width * 0.42f);
            float height = Mathf.Min(360f, Screen.height * 0.55f);
            float x = Screen.width - width - 12f;
            float y = 12f;

            Color prevBg = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0f, 0f, 0f, 0.78f);
            GUILayout.BeginArea(new Rect(x, y, width, height), GUI.skin.box);
            GUI.backgroundColor = prevBg;

            GUILayout.Label($"RIMA Log (F3) — {_lines.Count}/{MaxEntries}");
            _scroll = GUILayout.BeginScrollView(_scroll);

            foreach (LogLine line in _lines)
            {
                _style.normal.textColor = ColorFor(line.type);
                GUILayout.Label(line.text, _style);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private static Color ColorFor(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    return new Color(1f, 0.36f, 0.32f);
                case LogType.Warning:
                    return new Color(1f, 0.86f, 0.30f);
                default:
                    return Color.white;
            }
        }
    }
}
#endif
