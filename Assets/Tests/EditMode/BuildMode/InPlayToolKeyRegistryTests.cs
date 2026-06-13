#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.UI.BuildMode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace RIMA.Tests.BuildMode
{
    /// <summary>
    /// Key-ownership guard regression (consolidation item 1 + decision item 5 first bullet):
    /// first F2 register succeeds, a SECOND owner FAILS with a clear error, the same owner may
    /// re-register, and Release frees the key. This is the runtime contract that prevents the
    /// F2 double-poll conflict from ever recurring.
    /// </summary>
    public class InPlayToolKeyRegistryTests
    {
        private GameObject ownerA;
        private GameObject ownerB;

        [SetUp]
        public void SetUp()
        {
            InPlayToolKeyRegistry.ClearAll();
            ownerA = new GameObject("OwnerA");
            ownerB = new GameObject("OwnerB");
        }

        [TearDown]
        public void TearDown()
        {
            InPlayToolKeyRegistry.ClearAll();
            if (ownerA != null) Object.DestroyImmediate(ownerA);
            if (ownerB != null) Object.DestroyImmediate(ownerB);
        }

        [Test]
        public void FirstRegister_Succeeds_AndIsOwner()
        {
            Assert.IsTrue(InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerA));
            Assert.IsTrue(InPlayToolKeyRegistry.Owns(Key.F2, ownerA));
            Assert.AreSame(ownerA, InPlayToolKeyRegistry.OwnerOf(Key.F2));
        }

        [Test]
        public void SecondOwner_Fails_AndLogsError()
        {
            InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerA);

            // The conflict is reported as a clear error; assert it is logged so a future double-poll
            // is loud, not silent.
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("already owned"));
            Assert.IsFalse(InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerB),
                "A second, different owner must NOT be able to claim an owned key.");
            Assert.IsTrue(InPlayToolKeyRegistry.Owns(Key.F2, ownerA), "Original owner keeps the key.");
        }

        [Test]
        public void SameOwner_ReRegister_IsIdempotentSuccess()
        {
            Assert.IsTrue(InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerA));
            Assert.IsTrue(InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerA),
                "The same owner re-registering its own key must succeed (idempotent).");
        }

        [Test]
        public void Release_FreesKey_ForReclaim()
        {
            InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerA);
            Assert.IsTrue(InPlayToolKeyRegistry.Release(Key.F2, ownerA));
            Assert.IsNull(InPlayToolKeyRegistry.OwnerOf(Key.F2));
            Assert.IsTrue(InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerB),
                "After release, a different owner may claim the key.");
        }

        [Test]
        public void Release_ByNonOwner_IsNoOp()
        {
            InPlayToolKeyRegistry.RegisterExclusive(Key.F2, ownerA);
            Assert.IsFalse(InPlayToolKeyRegistry.Release(Key.F2, ownerB),
                "Releasing a key you do not own is a safe no-op.");
            Assert.IsTrue(InPlayToolKeyRegistry.Owns(Key.F2, ownerA));
        }
    }
}
#endif
