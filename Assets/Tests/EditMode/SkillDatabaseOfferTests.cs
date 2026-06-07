using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace RIMA.Tests
{
    public class SkillDatabaseOfferTests
    {
        private GameObject databaseObject;
        private SkillDatabase database;

        [SetUp]
        public void SetUp()
        {
            databaseObject = new GameObject("SkillDatabase_Test");
            database = databaseObject.AddComponent<SkillDatabase>();
            database.EnsureBuilt();
        }

        [TearDown]
        public void TearDown()
        {
            if (databaseObject != null)
                UnityEngine.Object.DestroyImmediate(databaseObject);
        }

        [Test]
        public void RangerCanonicalSkillsAppearInOffers()
        {
            string[] expected =
            {
                "Aimed Shot",
                "Disengage",
                "Multi Shot",
            };

            var offeredNames = database.GetPool(ClassType.Ranger, ClassType.None)
                .Select(skill => skill.skillName)
                .ToArray();

            CollectionAssert.IsSubsetOf(expected, offeredNames);
        }

        [Test]
        public void RetiredSkillsNeverOffered()
        {
            string[] retired =
            {
                "Backstab",
                "Shadow Step",
                "Fan of Knives",
            };

            foreach (ClassType primary in Enum.GetValues(typeof(ClassType)))
            {
                foreach (ClassType secondary in Enum.GetValues(typeof(ClassType)))
                {
                    var offeredNames = database.GetPool(primary, secondary)
                        .Select(skill => skill.skillName)
                        .ToArray();

                    foreach (string retiredName in retired)
                        CollectionAssert.DoesNotContain(offeredNames, retiredName);
                }
            }
        }
    }
}
