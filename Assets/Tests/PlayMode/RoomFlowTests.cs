using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace RIMA.Tests
{
    /// <summary>
    /// PlayMode integration tests — scene yüklenir, gerçek runtime test edilir.
    /// MCP run_tests ile çalıştırılır: Claude otomatik koşar, sonuçları analiz eder.
    /// </summary>
    public class RoomFlowTests
    {
        private const string SceneName = "_IsoGame";
        private readonly List<string> capturedErrors = new List<string>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            capturedErrors.Clear();
            Application.logMessageReceived += CaptureErrors;
            yield return SceneManager.LoadSceneAsync(SceneName);
            yield return null; // Awake/Start için bir kare bekle
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Application.logMessageReceived -= CaptureErrors;
            Time.timeScale = 1f; // Show() 0'a çeker, her testten sonra sıfırla
            yield return null;
        }

        private void CaptureErrors(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception)
                capturedErrors.Add($"[{type}] {condition}");
        }

        private void AssertNoErrors()
        {
            if (capturedErrors.Count > 0)
                Assert.Fail("Runtime hataları yakalandı:\n" + string.Join("\n", capturedErrors));
        }

        // ── Temel başlatma ────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator SceneLoad_NoErrors()
        {
            // Sahne yüklenip 1 saniye oynanıyor, hata olmamalı
            yield return new WaitForSeconds(1f);
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator DraftManager_ExistsAfterLoad()
        {
            yield return null;
            Assert.IsNotNull(DraftManager.Instance, "DraftManager.Instance null — Systems GO'ya bileşen eksik.");
        }

        [UnityTest]
        public IEnumerator DungeonGraph_GeneratesExpectedNodes()
        {
            yield return null;
            Assert.IsNotNull(DungeonGraph.Instance, "DungeonGraph.Instance null.");
            Assert.That(DungeonGraph.Instance.TotalNodes, Is.InRange(12, 14),
                $"DungeonGraph 12-14 node üretmeli (triple fork dahil). Gerçek: {DungeonGraph.Instance.TotalNodes}");
        }

        [UnityTest]
        public IEnumerator DungeonGraph_StartsAtNode0_Visited()
        {
            yield return null;
            Assert.IsNotNull(DungeonGraph.Instance);
            Assert.IsTrue(DungeonGraph.Instance.CurrentNode.visited,
                "Başlangıç odası (node 0) visited=true olmalı.");
        }

        [UnityTest]
        public IEnumerator RuntimeRoomManager_Room1Starts()
        {
            yield return null;
            Assert.IsNotNull(RuntimeRoomManager.Instance, "RuntimeRoomManager.Instance null.");
            Assert.AreEqual(1, RuntimeRoomManager.Instance.CurrentRoom,
                "İlk oda CurrentRoom=1 olmalı.");
        }

        // ── Draft akışı ──────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator Draft_ShowDraft_ActivatesDraft()
        {
            yield return null;
            Assert.IsNotNull(DraftManager.Instance);

            DraftManager.Instance.ShowDraft();
            yield return null;

            Assert.IsTrue(DraftManager.Instance.IsDraftActive,
                "ShowDraft() sonrası IsDraftActive=true olmalı.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator Draft_HideDraft_DeactivatesDraft()
        {
            yield return null;
            DraftManager.Instance.ShowDraft();
            yield return null;
            DraftManager.Instance.HideDraft();
            yield return null;

            Assert.IsFalse(DraftManager.Instance.IsDraftActive,
                "HideDraft() sonrası IsDraftActive=false olmalı.");
            Assert.AreEqual(1f, Time.timeScale, 0.01f,
                "HideDraft() sonrası timeScale=1 olmalı.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator Draft_FallbackOffers_WhenPoolEmpty()
        {
            // SkillData asset'lar yokken gold/heal fallback çalışmalı
            yield return null;

            bool draftShowed = false;
            DraftManager.Instance.OnSkillPicked.AddListener(_ => draftShowed = true);

            // ShowDraft çağrısı exception fırlatmamalı (pool boş = fallback)
            Assert.DoesNotThrow(() => DraftManager.Instance.ShowDraft());
            yield return null;

            AssertNoErrors();
        }

        // ── DungeonMapUI ─────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator DungeonMapUI_ExistsInScene()
        {
            yield return null;
            Assert.IsNotNull(DungeonMapUI.Instance,
                "DungeonMapUI.Instance null — Systems GO'da DungeonMapUI bileşeni eksik.");
        }

        [UnityTest]
        public IEnumerator DungeonMapUI_Toggle_NoErrors()
        {
            yield return null;
            Assert.IsNotNull(DungeonMapUI.Instance);

            DungeonMapUI.Instance.Toggle(); // aç
            yield return null;
            DungeonMapUI.Instance.Toggle(); // kapat
            yield return null;

            AssertNoErrors();
        }

        // ── Rage sistemi ─────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator RageSystem_ExistsOnPlayer()
        {
            yield return null;
            var player = GameObject.FindGameObjectWithTag("Player");
            Assert.IsNotNull(player, "Player tag'li obje sahnede yok.");

            var rage = player.GetComponent<RageSystem>();
            Assert.IsNotNull(rage, "Player'da RageSystem bileşeni eksik.");
        }

        [UnityTest]
        public IEnumerator RageSystem_AddRage_ClampedAt100()
        {
            yield return null;
            var player = GameObject.FindGameObjectWithTag("Player");
            var rage = player.GetComponent<RageSystem>();
            Assert.IsNotNull(rage);

            rage.AddRage(200); // max 100 olmalı
            Assert.LessOrEqual(rage.CurrentRage, 100, "Rage 100'ü geçmemeli.");
            AssertNoErrors();
        }

        // ── Kapı akışı ───────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator DungeonGraph_Navigate_UpdatesCurrentNode()
        {
            yield return null;
            Assert.IsNotNull(DungeonGraph.Instance);

            int startId = DungeonGraph.Instance.CurrentNode.id;
            bool result = DungeonGraph.Instance.Navigate(DoorDirection.North, out var nextNode);

            Assert.IsTrue(result, "Node 0'dan North çıkış olmalı (→ Node 1).");
            Assert.AreNotEqual(startId, DungeonGraph.Instance.CurrentNode.id,
                "Navigate sonrası farklı node'da olmalı.");
            Assert.IsTrue(DungeonGraph.Instance.CurrentNode.visited,
                "Yeni node visited=true olmalı.");
            AssertNoErrors();
        }
    }
}
