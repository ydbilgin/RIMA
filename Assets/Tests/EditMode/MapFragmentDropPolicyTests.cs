using NUnit.Framework;
using RIMA;

namespace RIMA.Tests
{
    public class MapFragmentDropPolicyTests
    {
        [TestCase(0)]
        [TestCase(1)]
        public void DropChance_IsGuaranteed_WhenCurrentRouteIsAtRevealFrontier(int revealedStepsAhead)
        {
            float chance = RuntimeRoomManager.GetMapFragmentDropChance(revealedStepsAhead, 0.35f);

            Assert.AreEqual(1f, chance);
        }

        [Test]
        public void DropChance_UsesConfiguredChance_WhenFutureRouteAlreadyVisible()
        {
            float chance = RuntimeRoomManager.GetMapFragmentDropChance(2, 0.35f);

            Assert.AreEqual(0.35f, chance);
        }

        [Test]
        public void DropChance_ClampsConfiguredChance()
        {
            Assert.AreEqual(1f, RuntimeRoomManager.GetMapFragmentDropChance(2, 2f));
            Assert.AreEqual(0f, RuntimeRoomManager.GetMapFragmentDropChance(2, -1f));
        }
    }
}

