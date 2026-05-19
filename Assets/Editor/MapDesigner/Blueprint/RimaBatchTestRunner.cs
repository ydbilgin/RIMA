#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public static class RimaBatchTestRunner
    {
        private const string ResultsPath = "TestResults_EditMode.xml";
        private const string SummaryPath = "TestResults_EditMode.summary.txt";

        public static void RunEditMode()
        {
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
                File.WriteAllText(
                    SummaryPath,
                    $"pass={result.PassCount}\nfail={result.FailCount}\nskip={result.SkipCount}\ninconclusive={result.InconclusiveCount}\nstate={result.ResultState}\n");
                EditorApplication.Exit(result.FailCount == 0 ? 0 : 1);
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
