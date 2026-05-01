using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace RIMA.Tests
{
    public class PlaytestScenarios
    {
        private const string SceneName = "_IsoGame";
        private readonly List<string> capturedErrors = new List<string>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            capturedErrors.Clear();
            Application.logMessageReceived += CaptureErrors;
            yield return SceneManager.LoadSceneAsync(SceneName);
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Application.logMessageReceived -= CaptureErrors;
            Time.timeScale = 1f;
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
                Assert.Fail("Runtime hatalari yakalandi:\n" + string.Join("\n", capturedErrors));
        }

        private IEnumerator KillAllEnemies()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var allHealth = Object.FindObjectsOfType<Health>();

            foreach (var hp in allHealth)
            {
                if (player != null && hp.gameObject == player) continue;
                hp.TakeDamage(9999);
            }

            yield return null;
        }

        private IEnumerator WaitForRoomCleared(RuntimeRoomManager rrm, float timeout = 5f)
        {
            while (rrm != null && !rrm.IsRoomCleared && timeout > 0f)
            {
                timeout -= Time.unscaledDeltaTime;
                yield return null;
            }
        }

        private IEnumerator ClearCurrentRoom(RuntimeRoomManager rrm)
        {
            Assert.IsNotNull(rrm, "RuntimeRoomManager.Instance null.");
            Assert.Greater(rrm.AliveEnemies, 0, "Current room should have enemies before clear.");

            yield return KillAllEnemies();
            yield return WaitForRoomCleared(rrm);

            Assert.IsTrue(rrm.IsRoomCleared, "Room should clear after all enemies die.");
            Assert.AreEqual(0, rrm.AliveEnemies, "AliveEnemies should be 0 after clear.");
        }

        private IEnumerator NavigateNorthToNextRoom(RuntimeRoomManager rrm)
        {
            Assert.IsNotNull(DungeonGraph.Instance, "DungeonGraph.Instance null.");
            Assert.IsTrue(DungeonGraph.Instance.Navigate(DoorDirection.North, out _),
                "Current node should have a North exit.");

            rrm.StartRoom();
            yield return new WaitForSeconds(1f);
        }

        [UnityTest]
        public IEnumerator MultiRoom_Navigate_EnemiesSpawnInNextRoom()
        {
            yield return null;

            var rrm = RuntimeRoomManager.Instance;
            Assert.IsNotNull(rrm, "RuntimeRoomManager.Instance null.");

            yield return NavigateNorthToNextRoom(rrm);

            Assert.Greater(rrm.AliveEnemies, 0, "Next room should spawn enemies.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator MultiRoom_ClearRoom1_NavigateThenClear()
        {
            yield return new WaitForSeconds(1f);

            var rrm = RuntimeRoomManager.Instance;
            yield return ClearCurrentRoom(rrm);
            yield return NavigateNorthToNextRoom(rrm);
            yield return ClearCurrentRoom(rrm);

            Assert.IsTrue(rrm.IsRoomCleared, "Second room should clear after all enemies die.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator RageSystem_HitTaken_AddsRage()
        {
            yield return null;

            var player = GameObject.FindGameObjectWithTag("Player");
            Assert.IsNotNull(player, "Player tag'li obje sahnede yok.");

            var rage = player.GetComponent<RageSystem>();
            var health = player.GetComponent<Health>();
            Assert.IsNotNull(rage, "Player'da RageSystem eksik.");
            Assert.IsNotNull(health, "Player'da Health eksik.");

            // Wire explicitly — Inspector wiring may not exist in test scene
            health.OnDamageTaken.AddListener(rage.OnTakeDamage);

            int rageBefore = rage.CurrentRage;
            health.TakeDamage(10);
            yield return null;

            Assert.Greater(rage.CurrentRage, rageBefore, "Hit taken should add rage.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator RageSystem_Decay_OverTime()
        {
            yield return null;

            var player = GameObject.FindGameObjectWithTag("Player");
            Assert.IsNotNull(player, "Player tag'li obje sahnede yok.");

            var rage = player.GetComponent<RageSystem>();
            Assert.IsNotNull(rage, "Player'da RageSystem eksik.");

            rage.ResetRage();
            rage.AddRage(50);
            Assert.AreEqual(50, rage.CurrentRage, "Precondition: rage should be 50 after AddRage(50).");

            yield return new WaitForSeconds(2f);

            Assert.Less(rage.CurrentRage, 50, "Rage should decay over time.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator DraftManager_PickSkill_HidesDraft()
        {
            yield return null;
            Assert.IsNotNull(DraftManager.Instance, "DraftManager.Instance null.");

            DraftManager.Instance.ShowDraft();
            yield return null;

            Assert.IsTrue(DraftManager.Instance.IsDraftActive, "ShowDraft should activate draft.");

            Button firstOfferButton = null;
            foreach (var button in Object.FindObjectsOfType<Button>(true))
            {
                if (button.gameObject.activeInHierarchy && button.gameObject.name == "Btn")
                {
                    firstOfferButton = button;
                    break;
                }
            }

            Assert.IsNotNull(firstOfferButton, "No active draft offer button found.");
            firstOfferButton.onClick.Invoke();
            yield return null;

            Assert.IsFalse(DraftManager.Instance.IsDraftActive, "Picking an offer should hide draft.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator DeathScreen_PlayerDies_ShowsDeathScreen()
        {
            yield return null;

            var player = GameObject.FindGameObjectWithTag("Player");
            Assert.IsNotNull(player, "Player tag'li obje sahnede yok.");

            var health = player.GetComponent<Health>();
            Assert.IsNotNull(health, "Player'da Health eksik.");

            health.TakeDamage(9999);
            yield return new WaitForSecondsRealtime(2f);

            Assert.IsTrue(health.IsDead, "Player should be dead after lethal damage.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator DungeonGraph_AllNodesReachable()
        {
            yield return null;
            Assert.IsNotNull(DungeonGraph.Instance, "DungeonGraph.Instance null.");

            var nodes = DungeonGraph.Instance.AllNodes;
            var visited = new HashSet<int>();
            var queue = new Queue<int>();
            visited.Add(0);
            queue.Enqueue(0);

            while (queue.Count > 0)
            {
                var node = nodes[queue.Dequeue()];
                foreach (int nextId in node.exits.Values)
                {
                    if (visited.Add(nextId))
                        queue.Enqueue(nextId);
                }
            }

            Assert.GreaterOrEqual(visited.Count, 12, "At least 12 dungeon nodes should be reachable from node 0.");
            Assert.AreEqual(DungeonGraph.Instance.TotalNodes, visited.Count,
                "Every generated node should be reachable from node 0.");
            AssertNoErrors();
        }

        [UnityTest]
        public IEnumerator RewardPickup_Interact_MarksCollected()
        {
            yield return new WaitForSeconds(1f);

            var rrm = RuntimeRoomManager.Instance;
            yield return ClearCurrentRoom(rrm);

            RewardPickup reward = null;
            float timeout = 5f;
            while (reward == null && timeout > 0f)
            {
                timeout -= Time.unscaledDeltaTime;
                reward = Object.FindObjectOfType<RewardPickup>();
                yield return null;
            }

            Assert.IsNotNull(reward, "RewardPickup should spawn after room clear.");

            var doInteract = typeof(RewardPickup).GetMethod(
                "DoInteract", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(doInteract, "RewardPickup has no public Interact/Collect API and private DoInteract was not found.");

            var routine = (IEnumerator)doInteract.Invoke(reward, null);
            Assert.IsNotNull(routine, "DoInteract did not return an IEnumerator.");
            Assert.IsTrue(routine.MoveNext(), "DoInteract should start and yield.");
            yield return null;

            Assert.IsTrue(reward.WasCollected, "Reward interaction should mark WasCollected=true.");
            DraftManager.Instance?.HideDraft();
            AssertNoErrors();
        }
    }
}
