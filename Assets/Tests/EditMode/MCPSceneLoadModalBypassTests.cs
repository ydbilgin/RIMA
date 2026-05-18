using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA.Tests
{
    public sealed class MCPSceneLoadModalBypassTests
    {
        private const string TestDir = "Assets/Tests/_TestArtifacts/MCPSceneModalBypass";
        private const string TargetScenePath = TestDir + "/TargetScene.unity";
        private const string NamedScenePath = TestDir + "/NamedDirtyScene.unity";
        private const string CreatedScenePath = TestDir + "/CreatedByMcp.unity";
        private const string TempDiscardPath = "Assets/_mcp_discard_temp.unity";

        private static readonly Type ManageSceneType = Type.GetType(
            "MCPForUnity.Editor.Tools.ManageScene, MCPForUnity.Editor");

        [SetUp]
        public void SetUp()
        {
            Assume.That(ManageSceneType, Is.Not.Null, "MCPForUnity.Editor.Tools.ManageScene is not loaded.");
            Directory.CreateDirectory(TestDir);
            DeleteSceneAsset(TargetScenePath);
            DeleteSceneAsset(NamedScenePath);
            DeleteSceneAsset(CreatedScenePath);
            DeleteSceneAsset(TempDiscardPath);
            CreateSavedScene(TargetScenePath);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        [TearDown]
        public void TearDown()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            DeleteSceneAsset(TargetScenePath);
            DeleteSceneAsset(NamedScenePath);
            DeleteSceneAsset(CreatedScenePath);
            DeleteSceneAsset(TempDiscardPath);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }

        [Test]
        public void LoadScene_WithDirtyUntitledScene_ForceDiscardTrue_LoadsTarget()
        {
            MakeActiveSceneDirty();

            object response = InvokePrivate("LoadScene", new object[] { TargetScenePath, true });

            AssertSuccess(response);
            Assert.That(EditorSceneManager.GetActiveScene().path, Is.EqualTo(TargetScenePath));
            Assert.That(AssetDatabase.LoadAssetAtPath<SceneAsset>(TempDiscardPath), Is.Null);
        }

        [Test]
        public void LoadScene_WithDirtyNamedScene_ForceDiscardTrue_SavesAndLoads()
        {
            CreateSavedScene(NamedScenePath);
            EditorSceneManager.OpenScene(NamedScenePath, OpenSceneMode.Single);
            new GameObject("SavedBeforeForcedLoad");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            object response = InvokePrivate("LoadScene", new object[] { TargetScenePath, true });

            AssertSuccess(response);
            Assert.That(EditorSceneManager.GetActiveScene().path, Is.EqualTo(TargetScenePath));

            EditorSceneManager.OpenScene(NamedScenePath, OpenSceneMode.Single);
            Assert.That(GameObject.Find("SavedBeforeForcedLoad"), Is.Not.Null);
        }

        [Test]
        public void LoadScene_WithDirtyScene_ForceDiscardFalse_ReturnsError()
        {
            MakeActiveSceneDirty();

            object response = InvokePrivate("LoadScene", new object[] { TargetScenePath, false });

            AssertFailure(response);
            Assert.That(EditorSceneManager.GetActiveScene().path, Is.Empty);
        }

        [Test]
        public void CreateScene_WithDirtyUntitledScene_ForceDiscardTrue_CreatesWithoutModal()
        {
            MakeActiveSceneDirty();
            string fullPath = Path.GetFullPath(CreatedScenePath);

            object response = InvokePrivate("CreateScene", new object[] { fullPath, CreatedScenePath, true });

            AssertSuccess(response);
            Assert.That(EditorSceneManager.GetActiveScene().path, Is.EqualTo(CreatedScenePath));
            Assert.That(AssetDatabase.LoadAssetAtPath<SceneAsset>(CreatedScenePath), Is.Not.Null);
            Assert.That(AssetDatabase.LoadAssetAtPath<SceneAsset>(TempDiscardPath), Is.Null);
        }

        private static void CreateSavedScene(string path)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Assert.That(EditorSceneManager.SaveScene(scene, path), Is.True);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }

        private static void MakeActiveSceneDirty()
        {
            new GameObject("DirtyMarker");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Assert.That(EditorSceneManager.GetActiveScene().isDirty, Is.True);
        }

        private static object InvokePrivate(string methodName, object[] args)
        {
            foreach (MethodInfo method in ManageSceneType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (method.Name == methodName && ParametersMatch(method.GetParameters(), args))
                    return method.Invoke(null, args);
            }

            Assert.Fail($"Could not find private method {methodName} with {args.Length} parameters.");
            return null;
        }

        private static bool ParametersMatch(ParameterInfo[] parameters, object[] args)
        {
            if (parameters.Length != args.Length) return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (args[i] != null && !parameters[i].ParameterType.IsInstanceOfType(args[i]))
                    return false;
            }
            return true;
        }

        private static void AssertSuccess(object response)
        {
            Assert.That(GetSuccess(response), Is.True, GetError(response));
        }

        private static void AssertFailure(object response)
        {
            Assert.That(GetSuccess(response), Is.False);
        }

        private static bool GetSuccess(object response)
        {
            Assert.That(response, Is.Not.Null);
            PropertyInfo property = response.GetType().GetProperty("Success");
            Assert.That(property, Is.Not.Null);
            return (bool)property.GetValue(response);
        }

        private static string GetError(object response)
        {
            if (response == null) return "Response was null.";
            PropertyInfo property = response.GetType().GetProperty("Error");
            return property == null ? response.ToString() : property.GetValue(response) as string;
        }

        private static void DeleteSceneAsset(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) != null)
                AssetDatabase.DeleteAsset(path);
        }
    }
}
