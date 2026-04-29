using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class EnemyTelegraphTests
    {
        [Test]
        public void BuildCirclePoints_ClosesLoop()
        {
            Vector3[] points = EnemyTelegraph.BuildCirclePoints(Vector3.zero, 2f, 16);

            Assert.AreEqual(17, points.Length);
            Assert.That(Vector3.Distance(points[0], points[points.Length - 1]), Is.LessThan(0.001f));
        }

        [Test]
        public void BuildLinePoints_ReturnsClosedRectangle()
        {
            Vector3[] points = EnemyTelegraph.BuildLinePoints(Vector3.zero, Vector2.right, 5f, 1f);

            Assert.AreEqual(5, points.Length);
            Assert.That(points[1].x, Is.EqualTo(5f).Within(0.001f));
            Assert.That(points[0].y, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(Vector3.Distance(points[0], points[points.Length - 1]), Is.LessThan(0.001f));
        }

        [Test]
        public void BuildConePoints_StartsAndEndsAtOrigin()
        {
            Vector3 origin = new Vector3(2f, 3f, 0f);
            Vector3[] points = EnemyTelegraph.BuildConePoints(origin, Vector2.up, 4f, 60f, 8);

            Assert.AreEqual(11, points.Length);
            Assert.That(Vector3.Distance(origin, points[0]), Is.LessThan(0.001f));
            Assert.That(Vector3.Distance(origin, points[points.Length - 1]), Is.LessThan(0.001f));
        }
    }
}
