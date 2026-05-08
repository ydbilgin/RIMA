using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace RIMA.Tests.EditMode
{
    [TestFixture]
    [Category("Bootstrap")]
    [Category("Wiring")]
    public class ComponentWiringTests
    {
        private static Assembly _rimaAssembly;
        private static Type _behaviorInterface;

        [OneTimeSetUp]
        public void SetUp()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetType("RIMA.PlayerAttack") != null)
                {
                    _rimaAssembly = asm;
                    break;
                }
            }
            Assert.IsNotNull(_rimaAssembly, "Could not locate RIMA runtime assembly.");
            _behaviorInterface = _rimaAssembly.GetType("RIMA.IBasicAttackBehavior");
        }

        [Test]
        public void PlayerAttack_BasicAttackProfileField_Exists()
        {
            var type = _rimaAssembly.GetType("RIMA.PlayerAttack");
            Assert.IsNotNull(type, "RIMA.PlayerAttack not found.");

            var field = type.GetField("basicAttackProfile",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(field,
                "PlayerAttack is missing private field 'basicAttackProfile'. " +
                "It must be a [SerializeField] so the Inspector can assign the profile asset.");

            // Verify it carries SerializeField attribute (Inspector-assignable)
            var hasSerializeField = field.GetCustomAttributes(typeof(UnityEngine.SerializeField), false).Length > 0;
            Assert.IsTrue(hasSerializeField,
                "PlayerAttack.basicAttackProfile must be decorated with [SerializeField].");
        }

        [Test]
        public void AllBasicAttackBehaviors_AreInRIMANamespace()
        {
            Assert.IsNotNull(_behaviorInterface, "IBasicAttackBehavior not found.");

            var concreteTypes = _rimaAssembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface
                            && _behaviorInterface.IsAssignableFrom(t))
                .ToArray();

            Assert.Greater(concreteTypes.Length, 0,
                "No concrete IBasicAttackBehavior implementations found.");

            foreach (var type in concreteTypes)
            {
                Assert.AreEqual("RIMA", type.Namespace,
                    $"{type.Name} is in namespace '{type.Namespace}' but must be in 'RIMA'. " +
                    "All behavior implementations belong to the root RIMA namespace.");
            }
        }

        [Test]
        public void ClassType_EnumCoversAllClasses()
        {
            var enumType = _rimaAssembly.GetType("RIMA.ClassType");
            Assert.IsNotNull(enumType, "RIMA.ClassType enum not found.");

            var names = Enum.GetNames(enumType);

            string[] required =
            {
                "Warblade", "Elementalist", "Shadowblade",
                "Ranger", "Ravager", "Ronin",
                "Gunslinger", "Brawler", "Summoner", "Hexer"
            };

            foreach (var cls in required)
            {
                Assert.IsTrue(
                    names.Contains(cls),
                    $"ClassType enum is missing '{cls}'. " +
                    "All 10 playable classes must be declared before asset work begins.");
            }
        }
    }
}
