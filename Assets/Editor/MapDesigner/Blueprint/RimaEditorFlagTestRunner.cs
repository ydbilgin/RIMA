#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public static class RimaEditorFlagTestRunner
    {
        private const string FlagPath = "STAGING/RUN_EDITMODE_TESTS.flag";
        private const string DonePath = "STAGING/EDITMODE_TESTS_DONE.txt";
        private const string ResultsPath = "TestResults_EditMode.xml";
        private const string SummaryPath = "TestResults_EditMode.summary.txt";

        private static bool isRunning;

        [InitializeOnLoadMethod]
        private static void RegisterFlagWatcher()
        {
            EditorApplication.update -= RunFromFlagOnEditorUpdate;
            EditorApplication.update += RunFromFlagOnEditorUpdate;
        }

        private static void RunFromFlagOnEditorUpdate()
        {
            if (isRunning || EditorApplication.isCompiling || !File.Exists(FlagPath))
            {
                return;
            }

            File.Delete(FlagPath);
            RunEditModeNoExit();
        }

        public static void RunEditModeNoExit()
        {
            isRunning = true;
            Directory.CreateDirectory("STAGING");
            File.WriteAllText(DonePath, "running\n");

            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            api.RegisterCallbacks(new BatchCallbacks());

            var filter = new Filter { testMode = UnityEditor.TestTools.TestRunner.Api.TestMode.EditMode };
            var settings = new ExecutionSettings(filter)
            {
                runSynchronously = true
            };

            api.Execute(settings);
        }

        private sealed class BatchCallbacks : ICallbacks
        {
            public void RunStarted(ITestAdaptor testsToRun)
            {
            }

            public void RunFinished(ITestResultAdaptor result)
            {
                TestRunnerApi.SaveResultToFile(result, ResultsPath);
                string summary =
                    $"pass={result.PassCount}\n" +
                    $"fail={result.FailCount}\n" +
                    $"skip={result.SkipCount}\n" +
                    $"inconclusive={result.InconclusiveCount}\n" +
                    $"state={result.ResultState}\n";
                File.WriteAllText(SummaryPath, summary);
                File.WriteAllText(DonePath, summary);
                isRunning = false;

                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(result.FailCount == 0 ? 0 : 1);
                }
            }

            public void TestStarted(ITestAdaptor test)
            {
            }

            public void TestFinished(ITestResultAdaptor result)
            {
            }
        }
    }
}
#endif
