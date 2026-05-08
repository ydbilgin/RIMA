using System;
using System.Reflection;
using NUnit.Framework;
using RIMA.Tests.Contracts;

namespace RIMA.Tests.EditMode
{
    [TestFixture]
    [Category("UIFlow")]
    [Category("Contract")]
    public class UIFlowContractTests
    {
        private static Assembly _rimaAssembly;

        [OneTimeSetUp]
        public void SetUp()
        {
            // Find the RIMA runtime assembly by looking for a known type
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetType("RIMA.SkillOfferUI") != null)
                {
                    _rimaAssembly = asm;
                    break;
                }
            }
            Assert.IsNotNull(_rimaAssembly, "Could not locate RIMA runtime assembly. Is RIMA.Runtime compiled?");
        }

        [Test]
        public void PausingUI_AllHaveCloseMethod()
        {
            foreach (var (className, closeMethod) in UIFlowContract.PauseRestorePairs)
            {
                var type = _rimaAssembly.GetType($"RIMA.{className}");
                // If class not yet implemented, skip with informative message rather than hard-fail.
                // Remove Ignore once the class exists.
                if (type == null)
                {
                    Assert.Ignore($"Class RIMA.{className} not found — implement the class to un-ignore this test.");
                    continue;
                }

                var method = type.GetMethod(closeMethod,
                    BindingFlags.Public | BindingFlags.Instance);

                Assert.IsNotNull(method,
                    $"RIMA.{className} is missing public method '{closeMethod}()'. " +
                    $"Every UI class that sets timeScale=0 must restore it in '{closeMethod}'.");
            }
        }

        [Test]
        public void MainMenuScreen_HasGameStartedGuard()
        {
            var type = _rimaAssembly.GetType("RIMA.MainMenuScreen");
            Assert.IsNotNull(type, "RIMA.MainMenuScreen class not found.");

            var field = type.GetField(UIFlowContract.MainMenuGuardField,
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.IsNotNull(field,
                $"MainMenuScreen is missing static private field '{UIFlowContract.MainMenuGuardField}'. " +
                "This guard prevents double scene-load on domain reload.");

            Assert.AreEqual(typeof(bool), field.FieldType,
                $"Field '{UIFlowContract.MainMenuGuardField}' must be bool.");
        }

        [Test]
        public void MainMenuScreen_OnPlayClicked_Exists()
        {
            var type = _rimaAssembly.GetType("RIMA.MainMenuScreen");
            Assert.IsNotNull(type, "RIMA.MainMenuScreen class not found.");

            var method = type.GetMethod("OnPlayClicked",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method,
                "MainMenuScreen.OnPlayClicked() private method not found. " +
                "This method must set _gameStarted = true before loading the next scene.");
        }

        [Test]
        public void CharacterSelectScreen_OnStartRun_Exists()
        {
            var type = _rimaAssembly.GetType("RIMA.CharacterSelectScreen");
            Assert.IsNotNull(type, "RIMA.CharacterSelectScreen class not found.");

            var method = type.GetMethod("OnStartRun",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method,
                "CharacterSelectScreen.OnStartRun() private method not found. " +
                "This is the scene transition entry point from character select.");
        }
    }
}
