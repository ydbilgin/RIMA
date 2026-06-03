// C12 — LiveToolLauncher (F6).
// One-click launch: serialize current room → write room_current.json → launch Tool.exe + Game.exe.
// Exposed via RimaRoomPainterWindow toolbar button ("Launch Live Tool").
//
// Spec source: T3_TOOL_FULL_DESIGN.md §F6 (C12 + build config row).
// - Validates Builds/RIMA_Tool/RIMA_Tool.exe and Builds/RIMA_Game/RIMA.exe exist.
// - If missing, offers to build (BuildPipeline); user can cancel and build manually.
// - Writes room_current.json via RoomLayoutSerializer.WriteCurrent().
// - Launches both .exe; updates statusbar PID state.
// - Tracks live PIDs so "Stop" / re-launch works correctly.

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RIMA.Editor.RoomPainter.LiveTool
{
    /// <summary>
    /// Static service class that owns the Live Tool launch lifecycle.
    /// Designed to be called from <c>RimaRoomPainterWindow</c> toolbar only.
    /// </summary>
    public static class LiveToolLauncher
    {
        // ── Build output paths (relative to project root) ──────────────────────
        private const string ToolExeRelative  = "Builds/RIMA_Tool/RIMA_Tool.exe";
        private const string GameExeRelative  = "Builds/RIMA_Game/RIMA.exe";

        // ── Scripting define injected into Tool build only ─────────────────────
        private const string ToolScriptingDefine = "RIMA_LIVE_TOOL";

        // ── Process tracking ───────────────────────────────────────────────────
        private static Process _toolProcess;
        private static Process _gameProcess;

        /// <summary>True while at least one managed process is running.</summary>
        public static bool IsRunning =>
            (_toolProcess != null && !_toolProcess.HasExited) ||
            (_gameProcess != null && !_gameProcess.HasExited);

        /// <summary>
        /// Human-readable status for the Room Painter statusbar.
        /// Returns PIDs when running, "Idle" otherwise.
        /// </summary>
        public static string StatusText
        {
            get
            {
                bool toolAlive = _toolProcess != null && !_toolProcess.HasExited;
                bool gameAlive = _gameProcess != null && !_gameProcess.HasExited;

                if (!toolAlive && !gameAlive)
                    return "Live Tool: Idle";

                string toolPart = toolAlive ? "Tool PID " + _toolProcess.Id : "Tool Idle";
                string gamePart = gameAlive ? "Game PID " + _gameProcess.Id : "Game Idle";
                return "Live Tool running — " + toolPart + ", " + gamePart;
            }
        }

        // ── Public API ─────────────────────────────────────────────────────────

        /// <summary>
        /// Main entry point called by the toolbar button.
        /// Sequence: validate → (optional build) → serialize → launch.
        /// </summary>
        public static void Launch()
        {
            // Kill stale managed processes first so re-launch is clean.
            StopAll();

            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string toolExe = Path.Combine(projectRoot, ToolExeRelative);
            string gameExe = Path.Combine(projectRoot, GameExeRelative);

            // Validate both .exe exist; offer to build if missing.
            bool toolMissing = !File.Exists(toolExe);
            bool gameMissing = !File.Exists(gameExe);

            if (toolMissing || gameMissing)
            {
                string missing = (toolMissing ? "RIMA_Tool.exe" : "") +
                                 (toolMissing && gameMissing ? " and " : "") +
                                 (gameMissing ? "RIMA.exe" : "");

                bool doBuild = EditorUtility.DisplayDialog(
                    "Live Tool — Build Required",
                    missing + " not found.\n\nBuild both targets now?\n\n" +
                    "• Yes  — triggers BuildPipeline (may take several minutes).\n" +
                    "• No   — cancel; build manually via RIMA → Live Tool → Build menu, then try again.",
                    "Yes, Build Now",
                    "Cancel");

                if (!doBuild)
                {
                    Debug.Log("[LiveToolLauncher] Launch cancelled — build required first.");
                    return;
                }

                BuildBothTargets(toolExe, gameExe);

                // Re-validate after build.
                if (!File.Exists(toolExe) || !File.Exists(gameExe))
                {
                    EditorUtility.DisplayDialog(
                        "Live Tool — Build Failed",
                        "Build did not produce the expected .exe files.\n" +
                        "Check the Console for BuildPipeline errors.",
                        "OK");
                    return;
                }
            }

            // Serialize current room to StreamingAssets/live/room_current.json.
            try
            {
                RoomLayoutSerializer.WriteCurrent();
                Debug.Log("[LiveToolLauncher] room_current.json written: " + RoomLayoutSerializer.CurrentJsonPath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[LiveToolLauncher] room_current.json write failed: " + ex.Message +
                                 "\nLaunching anyway — reloader will load last saved state.");
            }

            // Launch Tool.exe.
            _toolProcess = StartProcess(toolExe);

            // Launch Game.exe — if multiple displays detected, pass -monitor flag.
            string gameArgs = Display.displays.Length > 1 ? "-monitor 1" : string.Empty;
            _gameProcess = StartProcess(gameExe, gameArgs);

            if (_toolProcess != null)
                Debug.Log("[LiveToolLauncher] Tool.exe launched (PID " + _toolProcess.Id + ").");
            else
                Debug.LogWarning("[LiveToolLauncher] Tool.exe failed to start.");

            if (_gameProcess != null)
                Debug.Log("[LiveToolLauncher] Game.exe launched (PID " + _gameProcess.Id + ").");
            else
                Debug.LogWarning("[LiveToolLauncher] Game.exe failed to start.");
        }

        /// <summary>Terminate both managed processes if still running.</summary>
        public static void StopAll()
        {
            KillProcess(ref _toolProcess);
            KillProcess(ref _gameProcess);
        }

        // ── Menu surface (4-surface visibility rule) ───────────────────────────

        [MenuItem("RIMA/Legacy/Live Tool/Launch Live Tool")]
        public static void LaunchFromMenu() => Launch();

        [MenuItem("RIMA/Legacy/Live Tool/Stop Live Tool")]
        public static void StopFromMenu() => StopAll();

        [MenuItem("RIMA/Legacy/Live Tool/Build Both Targets")]
        public static void BuildBothFromMenu()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            BuildBothTargets(
                Path.Combine(projectRoot, ToolExeRelative),
                Path.Combine(projectRoot, GameExeRelative));
        }

        // ── Private helpers ────────────────────────────────────────────────────

        private static Process StartProcess(string exePath, string args = "")
        {
            if (!File.Exists(exePath))
            {
                Debug.LogError("[LiveToolLauncher] Cannot start — file not found: " + exePath);
                return null;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName  = exePath,
                    Arguments = args,
                    UseShellExecute = true   // lets Windows resolve path + show window normally
                };
                Process proc = Process.Start(psi);
                return proc;
            }
            catch (Exception ex)
            {
                Debug.LogError("[LiveToolLauncher] Failed to start " + exePath + ": " + ex.Message);
                return null;
            }
        }

        private static void KillProcess(ref Process proc)
        {
            if (proc == null) return;
            try
            {
                if (!proc.HasExited)
                {
                    proc.Kill();
                    proc.WaitForExit(2000);
                }
            }
            catch { /* process may have already exited on its own */ }
            finally
            {
                proc.Dispose();
                proc = null;
            }
        }

        /// <summary>
        /// Triggers BuildPipeline for both targets.
        /// Tool build: single ToolMain scene + RIMA_LIVE_TOOL scripting define.
        /// Game build: existing RIMA player build (all scenes in Build Settings).
        /// </summary>
        private static void BuildBothTargets(string toolExePath, string gameExePath)
        {
#if UNITY_EDITOR
            // ── Tool build ─────────────────────────────────────────────────────
            string toolOutputDir = Path.GetDirectoryName(toolExePath);
            Directory.CreateDirectory(toolOutputDir);

            // Collect Tool scenes: ToolMain.unity only.
            // If ToolMain.unity doesn't exist yet (F3 not done), skip gracefully.
            const string toolScenePath = "Assets/Scenes/LiveTool/ToolMain.unity";
            string toolSceneFullPath = Path.Combine(Application.dataPath, "..", toolScenePath);
            string[] toolScenes = File.Exists(toolSceneFullPath)
                ? new[] { toolScenePath }
                : null;

            if (toolScenes == null)
            {
                Debug.LogWarning("[LiveToolLauncher] ToolMain.unity not found — skipping Tool build. " +
                                 "Complete F3 first to produce the Tool scene.");
            }
            else
            {
                // Inject RIMA_LIVE_TOOL define temporarily for Tool build target.
                BuildTargetGroup group = BuildTargetGroup.Standalone;
                string originalDefines = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                bool alreadyDefined = originalDefines.Contains(ToolScriptingDefine);

                if (!alreadyDefined)
                    UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(
                        group,
                        originalDefines.Length > 0
                            ? originalDefines + ";" + ToolScriptingDefine
                            : ToolScriptingDefine);

                try
                {
                    BuildPipeline.BuildPlayer(
                        toolScenes,
                        toolExePath,
                        BuildTarget.StandaloneWindows64,
                        BuildOptions.Development);
                }
                finally
                {
                    // Restore defines unconditionally — even if BuildPlayer throws.
                    if (!alreadyDefined)
                        UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(group, originalDefines);
                }
            }

            // ── Game build ─────────────────────────────────────────────────────
            string gameOutputDir = Path.GetDirectoryName(gameExePath);
            Directory.CreateDirectory(gameOutputDir);

            // Collect scenes from Build Settings (existing pipeline, unchanged).
            EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
            if (buildScenes == null || buildScenes.Length == 0)
            {
                Debug.LogWarning("[LiveToolLauncher] No scenes in Build Settings — Game build skipped.");
            }
            else
            {
                System.Collections.Generic.List<string> enabledScenes =
                    new System.Collections.Generic.List<string>();
                foreach (EditorBuildSettingsScene s in buildScenes)
                {
                    if (s.enabled) enabledScenes.Add(s.path);
                }

                if (enabledScenes.Count == 0)
                {
                    Debug.LogWarning("[LiveToolLauncher] No enabled scenes in Build Settings — Game build skipped.");
                }
                else
                {
                    BuildPipeline.BuildPlayer(
                        enabledScenes.ToArray(),
                        gameExePath,
                        BuildTarget.StandaloneWindows64,
                        BuildOptions.Development);
                }
            }
#endif
        }
    }
}
