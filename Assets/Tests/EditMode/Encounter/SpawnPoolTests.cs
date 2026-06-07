using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RIMA.Encounter;

namespace RIMA.Tests
{
    /// <summary>
    /// T6 — SeamCrawler spawn havuzundan çıkarıldı (sprite'sız mob, yeşil kutu).
    /// İkinci test: FractureImp (sprite'lı) hâlâ spawn edilebilir olmalı.
    /// </summary>
    public class SpawnPoolTests
    {
        private EncounterWaveSO wave;

        [SetUp]
        public void SetUp()
        {
            wave = Resources.Load<EncounterWaveSO>("Encounters/Act1_Wave_Pilot");
        }

        [TearDown]
        public void TearDown()
        {
            // Resources.Load asset — yönetilmez, TearDown gerekmez.
        }

        [Test]
        public void SeamCrawlerNeverSpawns_NotInPilotWave()
        {
            Assert.IsNotNull(wave, "Act1_Wave_Pilot asset yüklenemedi — Resources/Encounters/ kontrol et.");

            foreach (var entry in wave.entries)
            {
                Assert.AreNotEqual(
                    EncounterEnemyType.SeamCrawler,
                    entry.enemyType,
                    "SeamCrawler spawn havuzunda bulunmamalı (sprite'sız mob; T6 kaldırıldı).");
            }
        }

        [Test]
        public void FractureImp_StillInPilotWave()
        {
            Assert.IsNotNull(wave, "Act1_Wave_Pilot asset yüklenemedi.");

            bool found = false;
            foreach (var entry in wave.entries)
            {
                if (entry.enemyType == EncounterEnemyType.FractureImp)
                {
                    found = true;
                    break;
                }
            }

            Assert.IsTrue(found, "FractureImp spawn havuzunda bulunmalı (sprite'lı; gameplay'de KALIR).");
        }
    }
}
