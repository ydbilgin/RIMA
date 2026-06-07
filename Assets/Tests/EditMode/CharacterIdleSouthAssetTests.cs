using System.IO;
using NUnit.Framework;
using RIMA;
using UnityEngine;

namespace RIMA.Tests
{
    public class CharacterIdleSouthAssetTests
    {
        private static readonly ClassType[] Classes =
        {
            ClassType.Warblade,
            ClassType.Elementalist,
            ClassType.Ranger,
            ClassType.Shadowblade,
            ClassType.Ronin,
            ClassType.Ravager,
            ClassType.Gunslinger,
            ClassType.Brawler,
            ClassType.Summoner,
            ClassType.Hexer
        };

        [Test]
        public void ClassIdleSouthSprites_AllTenClassPathsExistOnDisk()
        {
            foreach (ClassType classType in Classes)
            {
                string className = classType.ToString();
                string lower = className.ToLowerInvariant();
                string editorPath = $"Assets/Resources/Characters/{className}/{lower}_idle_south.png";
                string resourcesPath = $"Characters/{className}/{lower}_idle_south";

                Assert.IsTrue(File.Exists(editorPath), $"{editorPath} must exist");
                Assert.IsNotNull(Resources.Load<Sprite>(resourcesPath), $"{resourcesPath} must resolve through Resources.Load");
            }
        }
    }
}
