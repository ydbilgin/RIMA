// Phase 1 Demo PlayMode Test Harness — abstract base class.
// All Phase 1 tests inherit from this class.
//
// NOTE: Unity PlayMode Test Framework does NOT support [OneTimeSetUp] returning IEnumerator.
// Scene load uses [UnitySetUp] (per-test). The JSON report is written from [TearDown] on
// the last test; sub-classes call RegisterResult() to accumulate results.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RIMA.Tests.Phase1Demo
{
    /// <summary>
    /// Abstract base for Phase 1 Demo PlayMode tests.
    /// Loads PlayableArena_Test01 before each test (idempotent — reloads only
    /// if the scene is not already active), tears down with a JSON report.
    /// </summary>
    public abstract class Phase1TestHarness : InputTestFixture
    {
        // ── Scene ─────────────────────────────────────────────────────────────────

        protected const string TestSceneName = "PlayableArena_Test01";

        // ── Input devices ─────────────────────────────────────────────────────────

        protected Keyboard kb;
        protected Mouse    mouse;

        // ── Test result accumulator ───────────────────────────────────────────────

        protected static readonly List<TestResultEntry> Results = new List<TestResultEntry>();

        protected static void RegisterResult(string testName, bool passed, string details = "")
        {
            Results.Add(new TestResultEntry
            {
                test    = testName,
                passed  = passed,
                details = details
            });
        }

        // ── Setup ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Synchronous setup: adds virtual input devices AFTER InputTestFixture.Setup() runs.
        /// NUnit calls [SetUp] methods bottom-up (base first, then derived), so base.Setup()
        /// has already re-initialized InputSystem by the time this runs.
        /// </summary>
        [SetUp]
        public void SetUpDevices()
        {
            kb    = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();
        }

        [UnitySetUp]
        public IEnumerator SetUpScene()
        {
            // Only reload if scene changed.
            if (SceneManager.GetActiveScene().name != TestSceneName)
            {
                var op = SceneManager.LoadSceneAsync(TestSceneName, LoadSceneMode.Single);
                if (op == null)
                {
                    Assert.Inconclusive(
                        $"[Phase1Harness] Scene '{TestSceneName}' not found in build settings. " +
                        "Add it via File > Build Settings.");
                    yield break;
                }
                while (!op.isDone) yield return null;
            }

            // One frame for Awake/Start to complete.
            yield return null;
        }

        // ── Teardown ──────────────────────────────────────────────────────────────

        [TearDown]
        public void TearDownWriteReport()
        {
            // InputTestFixture.TearDown() is called by NUnit automatically — do not call manually.
            if (Results.Count == 0) return;

            // Write report after each test so partial results survive if suite aborts.
            WriteReport();
        }

        private static void WriteReport()
        {
            string timestamp  = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string stagingDir = Path.GetFullPath(
                Path.Combine(Application.dataPath, "..", "STAGING"));

            try
            {
                Directory.CreateDirectory(stagingDir);
                string reportPath = Path.Combine(stagingDir,
                    $"AUTO_TEST_REPORT_{timestamp}.json");

                int passCount = 0, failCount = 0;
                var entries   = new List<string>();
                foreach (var r in Results)
                {
                    if (r.passed) passCount++; else failCount++;
                    string det = r.details.Replace("\\", "\\\\").Replace("\"", "'");
                    entries.Add(
                        $"    {{\"test\":\"{r.test}\"," +
                        $"\"passed\":{r.passed.ToString().ToLower()}," +
                        $"\"details\":\"{det}\"}}");
                }

                string json =
                    $"{{\n" +
                    $"  \"generatedAt\": \"{timestamp}\",\n" +
                    $"  \"scene\": \"{TestSceneName}\",\n" +
                    $"  \"pass\": {passCount},\n" +
                    $"  \"fail\": {failCount},\n" +
                    $"  \"results\": [\n" +
                    string.Join(",\n", entries) +
                    $"\n  ]\n}}";

                File.WriteAllText(reportPath, json);
                Debug.Log($"[Phase1Harness] Report written: {reportPath}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Phase1Harness] Could not write report: {ex.Message}");
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        protected GameObject GetPlayer()
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) return go;

#if UNITY_2023_1_OR_NEWER
            var ctrl = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
#else
            var ctrl = UnityEngine.Object.FindObjectOfType<PlayerController>();
#endif
            return ctrl != null ? ctrl.gameObject : null;
        }

        protected void TeleportPlayer(Vector2 pos)
        {
            var player = GetPlayer();
            if (player == null) return;
            player.transform.position = pos;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        protected IEnumerator WaitFrames(int n)
        {
            for (int i = 0; i < n; i++)
                yield return null;
        }

        // ── Data types ────────────────────────────────────────────────────────────

        public struct TestResultEntry
        {
            public string test;
            public bool   passed;
            public string details;
        }
    }
}
