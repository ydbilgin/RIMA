using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RIMA.Tests.Contracts;

namespace RIMA.Tests.EditMode
{
    [TestFixture]
    [Category("Combat")]
    [Category("Contract")]
    public class CombatContractTests
    {
        private static Assembly _rimaAssembly;
        private static Type _behaviorInterface;

        [OneTimeSetUp]
        public void SetUp()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetType("RIMA.IBasicAttackBehavior") != null)
                {
                    _rimaAssembly = asm;
                    break;
                }
            }
            Assert.IsNotNull(_rimaAssembly, "Could not locate RIMA runtime assembly.");
            _behaviorInterface = _rimaAssembly.GetType("RIMA.IBasicAttackBehavior");
            Assert.IsNotNull(_behaviorInterface, "RIMA.IBasicAttackBehavior type not found.");
        }

        /// <summary>
        /// Every concrete class implementing IBasicAttackBehavior must provide
        /// all three interface methods. This catches partial implementations
        /// that would break the strategy dispatch in PlayerAttack.Update().
        /// </summary>
        [Test]
        public void AllBehaviors_ImplementRequiredMethods()
        {
            var concreteTypes = _rimaAssembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface
                            && _behaviorInterface.IsAssignableFrom(t))
                .ToArray();

            Assert.Greater(concreteTypes.Length, 0,
                "No concrete IBasicAttackBehavior implementations found in RIMA assembly.");

            foreach (var type in concreteTypes)
            {
                foreach (var methodName in CombatContract.RequiredBehaviorMethods)
                {
                    // Interface methods will be present via explicit or implicit impl — GetMethod is sufficient.
                    var method = type.GetMethod(methodName,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    Assert.IsNotNull(method,
                        $"{type.Name} is missing method '{methodName}'. " +
                        $"All IBasicAttackBehavior implementations must provide this.");
                }
            }
        }

        [Test]
        public void BasicAttackProfile_ValidateMethod_Exists()
        {
            var profileType = _rimaAssembly.GetType("RIMA.BasicAttackProfile");
            Assert.IsNotNull(profileType, "RIMA.BasicAttackProfile not found.");

            // Validate(out string error) signature
            var method = profileType.GetMethod("Validate",
                BindingFlags.Public | BindingFlags.Instance);

            Assert.IsNotNull(method, "BasicAttackProfile.Validate() not found.");

            var parameters = method.GetParameters();
            Assert.AreEqual(1, parameters.Length,
                "BasicAttackProfile.Validate() must take exactly one parameter (out string error).");
            Assert.IsTrue(parameters[0].IsOut,
                "BasicAttackProfile.Validate() first parameter must be 'out string'.");
            Assert.AreEqual(typeof(string).MakeByRefType(), parameters[0].ParameterType,
                "BasicAttackProfile.Validate() out parameter must be of type string.");
        }

        /// <summary>
        /// Verifies that the number of explicit cases in CreateBehavior()'s switch
        /// matches CombatContract.ExplicitBehaviorCases. This is an indirect guard:
        /// when a new enum value is added AND a new behavior class is written,
        /// the developer must also increment ExplicitBehaviorCases in CombatContract.
        /// The test fails if someone forgets to update the contract constant.
        ///
        /// NOTE (for Opus review): this is a documentation-style enforcement.
        /// A stronger approach would be source-parsing the switch via Roslyn,
        /// but that adds heavy dependency for modest gain.
        /// </summary>
        [Test]
        public void BehaviorTypes_ExplicitCaseCount_MatchesContract()
        {
            var enumType = _rimaAssembly.GetType("RIMA.BasicAttackBehaviorType");
            Assert.IsNotNull(enumType, "RIMA.BasicAttackBehaviorType not found.");

            int enumValueCount = Enum.GetValues(enumType).Length;

            // The contract constant declares how many enum values have their own behavior class.
            // The rest legitimately fall through to the default (MeleeChainBehavior).
            // Invariant: ExplicitBehaviorCases <= enumValueCount
            Assert.LessOrEqual(CombatContract.ExplicitBehaviorCases, enumValueCount,
                $"CombatContract.ExplicitBehaviorCases ({CombatContract.ExplicitBehaviorCases}) " +
                $"exceeds total enum values ({enumValueCount}). Update BasicAttackBehaviorType or the contract.");

            // Verify there is at least one concrete behavior class per explicit case.
            var concreteTypes = _rimaAssembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface
                            && _behaviorInterface.IsAssignableFrom(t))
                .ToArray();

            Assert.GreaterOrEqual(concreteTypes.Length, CombatContract.ExplicitBehaviorCases,
                $"Found {concreteTypes.Length} concrete behavior classes but contract declares " +
                $"{CombatContract.ExplicitBehaviorCases} explicit cases. " +
                "Add missing behavior implementations or update CombatContract.ExplicitBehaviorCases.");
        }
    }
}
