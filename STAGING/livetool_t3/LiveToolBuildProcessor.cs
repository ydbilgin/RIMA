// TARGET: Assets/Editor/Build/LiveToolBuildProcessor.cs
// ─────────────────────────────────────────────────────────────────────────────
// RIMA Live Editor T3 — LiveToolBuildProcessor
//
// Editor-only build orchestration for the dual-target authoring pipeline:
//   • RIMA_Tool.exe  — the standalone live editor ("the Tool")        → RIMA_LIVE_TOOL define SET
//   • RIMA.exe       — the shipping game ("the Game")                 → RIMA_LIVE_TOOL define UNSET
//
// Mechanism (BUILD CONTRACT §2 "LiveToolBuildProcessor" + §1 Assembly Strategy):
//   The new runtime assembly `RIMA.LiveTool.asmdef` carries
//   `"defineConstraints": ["RIMA_LIVE_TOOL"]`. That assembly therefore compiles
//   into a Player build IFF the Standalone scripting-define set contains
//   RIMA_LIVE_TOOL.
//     - Tool build  → this processor injects the define → RIMA.LiveTool compiles
//                     into RIMA_Tool.exe (the live editor ships).
//     - Game build  → this processor strips the define  → RIMA.LiveTool drops out
//                     → 0 bytes of Tool code in RIMA.exe (spec §3 F6 "0 byte cost").
//   The Editor keeps RIMA_LIVE_TOOL permanently set in Player Settings so the Tool
//   source stays IDE-visible / bakeable; per-build this processor toggles it and
//   restores the original afterwards.
//
// Two coexisting flows talk to this file:
//   1. IPreprocessBuildWithReport / IPostprocessBuildWithReport — fire for EVERY
//      Player build (including the legacy inline `BuildBothTargets()` path inside
//      LiveToolLauncher.cs). The pre-hook inspects the output path: if it is the
//      Tool output it forces the define ON; if it is the Game output it forces it
//      OFF. The post-hook restores the snapshot taken in the pre-hook. This makes
//      the define correct even when a build is started by some other code path.
//   2. LiveToolBuilds.BuildTool() / BuildGame() — explicit, self-contained entry
//      points (menu + API) that set scenes + define and call BuildPipeline
//      directly. These are the canonical way to produce each target and are what
//      the launcher SHOULD migrate to (it currently inlines the same logic).
//
// API uncertainty: see // ASSUMPTION comments. All cross-assembly calls
// (RoomLayoutSerializer.WriteCurrent / RuntimeAssetRegistryBaker.Bake) were
// verified against source at:
//   Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs:17,29
//   Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs:44,63
// ─────────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Cross-assembly: RoomLayoutSerializer + RuntimeAssetRegistryBaker live in the
// RIMA.RoomPainter.Editor assembly, namespace RIMA.Editor.RoomPainter.LiveTool.
// The eventual home asmdef (RIMA.Build.Editor) MUST reference RIMA.RoomPainter.Editor.
using RIMA.Editor.RoomPainter.LiveTool;

namespace RIMA.LiveTool.EditorBuild
{
    /// <summary>
    /// Canonical constants + explicit build entry points for the dual-target
    /// (Tool / Game) pipeline. Keep these literals identical to the ones
    /// LiveToolLauncher.cs already spawns (BUILD CONTRACT §2 "Output paths MUST
    /// equal the literals LiveToolLauncher already spawns").
    /// </summary>
    public static class LiveToolBuilds
    {
        // ── Scripting define ──────────────────────────────────────────────────
        /// <summary>The define that gates the RIMA.LiveTool runtime assembly.</summary>
        public const string ToolDefine = "RIMA_LIVE_TOOL";

        // ── Scenes ────────────────────────────────────────────────────────────
        /// <summary>The single scene shipped in the Tool build.</summary>
        public const string ToolScene = "Assets/Scenes/LiveTool/ToolMain.unity";

        // ── Output paths (relative to project root) ───────────────────────────
        // MUST match LiveToolLauncher.cs:28-29 exactly.
        public const string ToolOutput = "Builds/RIMA_Tool/RIMA_Tool.exe";
        public const string GameOutput = "Builds/RIMA_Game/RIMA.exe";

        // ── Build target ──────────────────────────────────────────────────────
        private const BuildTarget StandaloneTarget = BuildTarget.StandaloneWindows64;

        /// <summary>
        /// Project root = parent of Assets/. Used to absolutize the output paths.
        /// </summary>
        public static string ProjectRoot =>
            Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        public static string ToolOutputAbsolute => Path.Combine(ProjectRoot, ToolOutput);
        public static string GameOutputAbsolute => Path.Combine(ProjectRoot, GameOutput);

        // ── Path classification (drives the auto pre/post hook) ───────────────

        /// <summary>
        /// True if <paramref name="outputPath"/> points at the Tool output file.
        /// Compared case-insensitively on the normalized filename + parent folder
        /// so it is robust to absolute vs relative and slash direction.
        /// </summary>
        public static bool IsToolOutput(string outputPath) => MatchesOutput(outputPath, ToolOutput);

        /// <summary>True if <paramref name="outputPath"/> points at the Game output file.</summary>
        public static bool IsGameOutput(string outputPath) => MatchesOutput(outputPath, GameOutput);

        private static bool MatchesOutput(string outputPath, string canonicalRelative)
        {
            if (string.IsNullOrEmpty(outputPath)) return false;

            // Compare the trailing "<parentFolder>/<file>" segment, which is unique
            // per target and stable regardless of how the path was supplied.
            string normalized = outputPath.Replace('\\', '/');
            string canonical  = canonicalRelative.Replace('\\', '/');

            string canonFile   = Path.GetFileName(canonical);
            string canonParent = Path.GetFileName(Path.GetDirectoryName(canonical) ?? string.Empty);
            string suffix      = (canonParent + "/" + canonFile).ToLowerInvariant();

            return normalized.ToLowerInvariant().EndsWith(suffix, StringComparison.Ordinal);
        }

        // ── Explicit build entry points ───────────────────────────────────────

        /// <summary>
        /// Build the standalone Tool (RIMA_Tool.exe).
        /// Sequence: bake registry → ensure room JSON exists → set RIMA_LIVE_TOOL
        /// → BuildPipeline (single ToolMain scene, Development) → restore defines.
        /// Returns the BuildReport (caller inspects report.summary.result).
        /// </summary>
        public static BuildReport BuildTool()
        {
            EnsureRegistryBaked();
            EnsureRoomJsonExists();

            string toolSceneFull = Path.Combine(ProjectRoot, ToolScene);
            if (!File.Exists(toolSceneFull))
            {
                Debug.LogError(
                    "[LiveToolBuildProcessor] " + ToolScene + " not found — cannot build Tool. " +
                    "Author ToolMain.unity (C5/F3) first.");
                return null;
            }

            string output = ToolOutputAbsolute;
            Directory.CreateDirectory(Path.GetDirectoryName(output));

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes           = new[] { ToolScene },
                locationPathName = output,
                target           = StandaloneTarget,
                targetGroup      = BuildTargetGroup.Standalone,
                // Development build so the Tool can log / attach a debugger; the
                // Tool is an internal authoring artifact, never shipped to players.
                options          = BuildOptions.Development,
            };

            return BuildWithDefine(options, toolDefineOn: true, label: "Tool");
        }

        /// <summary>
        /// Build the shipping Game (RIMA.exe).
        /// Uses the enabled scenes from Build Settings (existing pipeline, unchanged)
        /// and guarantees RIMA_LIVE_TOOL is NOT set → RIMA.LiveTool produces 0 bytes.
        /// </summary>
        public static BuildReport BuildGame()
        {
            string[] scenes = GetEnabledBuildSettingsScenes();
            if (scenes.Length == 0)
            {
                Debug.LogError(
                    "[LiveToolBuildProcessor] No enabled scenes in Build Settings — Game build aborted.");
                return null;
            }

            string output = GameOutputAbsolute;
            Directory.CreateDirectory(Path.GetDirectoryName(output));

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes           = scenes,
                locationPathName = output,
                target           = StandaloneTarget,
                targetGroup      = BuildTargetGroup.Standalone,
                // Dev build keeps the live-reload self-bootstrap alive in the Game
                // (LiveRoomReloader/JsonFileWatcher gate on DEVELOPMENT_BUILD).
                // Drop BuildOptions.Development here to produce a clean release.
                options          = BuildOptions.Development,
            };

            return BuildWithDefine(options, toolDefineOn: false, label: "Game");
        }

        // ── Shared build core ─────────────────────────────────────────────────

        /// <summary>
        /// Snapshot the Standalone scripting defines, force RIMA_LIVE_TOOL to the
        /// requested state, run BuildPipeline, then restore the snapshot
        /// unconditionally (try/finally — survives BuildPlayer throwing).
        /// </summary>
        private static BuildReport BuildWithDefine(BuildPlayerOptions options, bool toolDefineOn, string label)
        {
            string snapshot = GetStandaloneDefines();
            ApplyToolDefine(toolDefineOn);

            try
            {
                Debug.Log(
                    "[LiveToolBuildProcessor] Building " + label + " → " + options.locationPathName +
                    " (" + ToolDefine + (toolDefineOn ? " SET)" : " UNSET)"));

                BuildReport report = BuildPipeline.BuildPlayer(options);

                BuildResult result = report != null ? report.summary.result : BuildResult.Unknown;
                if (result == BuildResult.Succeeded)
                    Debug.Log("[LiveToolBuildProcessor] " + label + " build SUCCEEDED → " + options.locationPathName);
                else
                    Debug.LogError("[LiveToolBuildProcessor] " + label + " build " + result + " — check Console.");

                return report;
            }
            finally
            {
                SetStandaloneDefines(snapshot);
            }
        }

        /// <summary>
        /// Build both targets in sequence (Tool first, then Game). Mirrors the
        /// legacy LiveToolLauncher.BuildBothTargets but routed through the
        /// canonical, define-correct entry points.
        /// </summary>
        public static void BuildBoth()
        {
            BuildTool();
            BuildGame();
        }

        // ── Pre-requisite guarantees ──────────────────────────────────────────

        /// <summary>
        /// Ensure Assets/Resources/Live/RuntimeAssetRegistry.asset exists and is
        /// fresh. The Tool aborts at runtime if the registry is null
        /// (ToolBootstrap "Registry not baked" banner), so baking pre-build is
        /// mandatory. Bake() is idempotent + safe to call from code
        /// (RuntimeAssetRegistryBaker.cs:63).
        /// </summary>
        internal static void EnsureRegistryBaked()
        {
            try
            {
                // RuntimeAssetRegistryBaker.Bake() : public static RuntimeAssetRegistry
                // (RuntimeAssetRegistryBaker.cs:63). We only need the side effect.
                RuntimeAssetRegistryBaker.Bake();
                Debug.Log("[LiveToolBuildProcessor] RuntimeAssetRegistry baked (pre-build).");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    "[LiveToolBuildProcessor] Registry bake failed (continuing): " + ex.Message +
                    "\nTool will show 'Registry not baked' if the asset is absent.");
            }
        }

        /// <summary>
        /// Ensure StreamingAssets/live/room_current.json exists so the Tool has a
        /// room to open on first launch. WriteCurrent serializes the active Editor
        /// scene (RoomLayoutSerializer.cs:29). Non-fatal if it fails.
        /// </summary>
        internal static void EnsureRoomJsonExists()
        {
            try
            {
                if (File.Exists(RoomLayoutSerializer.CurrentJsonPath)) return; // already present
                RoomLayoutSerializer.WriteCurrent();
                Debug.Log("[LiveToolBuildProcessor] room_current.json written: " +
                          RoomLayoutSerializer.CurrentJsonPath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(
                    "[LiveToolBuildProcessor] room_current.json write failed (continuing): " + ex.Message);
            }
        }

        // ── Scripting-define helpers (Unity 6 NamedBuildTarget API) ───────────

        // ASSUMPTION: Unity 6000.3 (this project) exposes the NamedBuildTarget
        // overloads of PlayerSettings.Get/SetScriptingDefineSymbols. These are the
        // documented non-deprecated API in Unity 6.x. If a downstream Unity were
        // older, swap to the *ForGroup(BuildTargetGroup.Standalone, ...) overload
        // (still present, just obsolete-warned). Verified: 6000.3.6f1.
        private static readonly NamedBuildTarget StandaloneNamed = NamedBuildTarget.Standalone;

        private static string GetStandaloneDefines() =>
            PlayerSettings.GetScriptingDefineSymbols(StandaloneNamed);

        private static void SetStandaloneDefines(string defines) =>
            PlayerSettings.SetScriptingDefineSymbols(StandaloneNamed, defines ?? string.Empty);

        /// <summary>Add or remove RIMA_LIVE_TOOL from the Standalone define set.</summary>
        internal static void ApplyToolDefine(bool on)
        {
            List<string> defines = SplitDefines(GetStandaloneDefines());
            bool present = defines.Contains(ToolDefine);

            if (on && !present) defines.Add(ToolDefine);
            else if (!on && present) defines.RemoveAll(d => d == ToolDefine);
            else return; // already in desired state

            SetStandaloneDefines(string.Join(";", defines));
        }

        private static List<string> SplitDefines(string raw)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(raw)) return list;
            foreach (string part in raw.Split(';'))
            {
                string trimmed = part.Trim();
                if (trimmed.Length > 0 && !list.Contains(trimmed)) list.Add(trimmed);
            }
            return list;
        }

        // ── Scene collection ──────────────────────────────────────────────────

        private static string[] GetEnabledBuildSettingsScenes()
        {
            var scenes = new List<string>();
            foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
                if (s.enabled && !string.IsNullOrEmpty(s.path))
                    scenes.Add(s.path);
            return scenes.ToArray();
        }

        // ── Menu surface (4-surface visibility rule) ──────────────────────────

        [MenuItem("RIMA/Live Tool/Build Tool (RIMA_Tool.exe)")]
        public static void BuildToolFromMenu() => BuildTool();

        [MenuItem("RIMA/Live Tool/Build Game (RIMA.exe)")]
        public static void BuildGameFromMenu() => BuildGame();

        [MenuItem("RIMA/Live Tool/Build Both (Tool + Game)")]
        public static void BuildBothFromMenu() => BuildBoth();

        /// <summary>
        /// Permanently set RIMA_LIVE_TOOL in Player Settings so the Tool source is
        /// IDE-visible + bakeable in the Editor (BUILD CONTRACT §1: "Editor must
        /// set RIMA_LIVE_TOOL globally"). Per-build hooks still toggle/restore it.
        /// </summary>
        [MenuItem("RIMA/Live Tool/Enable Tool Define (Editor)")]
        public static void EnableToolDefineInEditor()
        {
            ApplyToolDefine(true);
            Debug.Log("[LiveToolBuildProcessor] " + ToolDefine +
                      " set in Standalone Player Settings (Editor now compiles RIMA.LiveTool).");
        }
    }

    /// <summary>
    /// Auto-correct the RIMA_LIVE_TOOL define for ANY Player build by inspecting
    /// the build output path. This covers builds started outside the explicit
    /// LiveToolBuilds entry points (e.g. the legacy inline launcher path or a
    /// manual File → Build) so the Game can never accidentally ship Tool code and
    /// the Tool can never ship without it.
    /// </summary>
    public sealed class LiveToolBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        // Run early so the define is correct before script compilation for the build.
        public int callbackOrder => 0;

        // Snapshot taken in pre, restored in post. Static because the two callbacks
        // are separate invocations on (potentially) separate instances.
        private static string _defineSnapshot;
        private static bool _modifiedThisBuild;

        public void OnPreprocessBuild(BuildReport report)
        {
            string output = report != null && report.summary.outputPath != null
                ? report.summary.outputPath
                : string.Empty;

            bool isTool = LiveToolBuilds.IsToolOutput(output);
            bool isGame = LiveToolBuilds.IsGameOutput(output);

            // Unrecognized output (some other build) → leave defines untouched.
            if (!isTool && !isGame)
            {
                _modifiedThisBuild = false;
                return;
            }

            // ASSUMPTION: NamedBuildTarget API present (Unity 6.x). See LiveToolBuilds.
            _defineSnapshot   = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
            _modifiedThisBuild = true;

            // Tool build → define ON; Game build → define OFF.
            LiveToolBuilds.ApplyToolDefine(isTool);

            Debug.Log(
                "[LiveToolBuildProcessor] Pre-build: detected " +
                (isTool ? "TOOL" : "GAME") + " target → " +
                LiveToolBuilds.ToolDefine + (isTool ? " SET." : " UNSET.") +
                " Output: " + output);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (!_modifiedThisBuild) return;

            // Restore the exact pre-build define string so the Editor returns to
            // its normal (RIMA_LIVE_TOOL-on) authoring state.
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, _defineSnapshot ?? string.Empty);
            _modifiedThisBuild = false;

            Debug.Log("[LiveToolBuildProcessor] Post-build: Standalone defines restored.");
        }
    }
}
#endif
