using System.Collections.Generic;
using RIMA.Combat;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA
{
    public sealed class RunStats : MonoBehaviour
    {
        private static RunStats instance;
        private static bool _shuttingDown;

        private readonly HashSet<int> countedKills = new HashSet<int>();
        private Health playerHealth;
        private int kills;
        private int roomsCleared;
        private int rewardsCollected;
        private int roomReached = 1;
        private float runTimeSeconds;
        private bool runStarted;
        private bool frozen;
        private bool echoAwarded;
        private int echoAward;

        public static RunStats Instance
        {
            get
            {
                if (_shuttingDown) return null;
                EnsureInstance();
                return instance;
            }
        }

        public static int Kills => Instance.kills;
        public static float RunTimeSeconds => Instance.runTimeSeconds;
        public static int RoomReached => Instance.GetRoomReached();
        public static int RoomsCleared => Instance.roomsCleared;
        public static int RewardsCollected => Instance.rewardsCollected;
        public int KillsForAward => kills;
        public int RoomsClearedForAward => roomsCleared;
        public bool HasEchoAward => echoAwarded;
        public int EchoAward => echoAward;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            EnsureInstance();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
            _shuttingDown = false;
        }

        private static void EnsureInstance()
        {
            if (_shuttingDown) return;
            if (instance != null) return;

            GameObject go = new GameObject("RunStats");
            instance = go.AddComponent<RunStats>();
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            CombatEventBus.OnKill += OnKill;
            RoomLoader.OnRoomChanged += OnRoomChanged;
            RoomLoader.OnRoomLoaded += OnRoomLoaded;
            RoomLoader.OnRoomCleared += OnRoomCleared;
            RoomLoader.OnDemoComplete += Freeze;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            CombatEventBus.OnKill -= OnKill;
            RoomLoader.OnRoomChanged -= OnRoomChanged;
            RoomLoader.OnRoomLoaded -= OnRoomLoaded;
            RoomLoader.OnRoomCleared -= OnRoomCleared;
            RoomLoader.OnDemoComplete -= Freeze;
            UnhookPlayerHealth();
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        private void Update()
        {
            HookPlayerHealthIfNeeded();

            if (runStarted && !frozen)
                runTimeSeconds += Time.unscaledDeltaTime;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!MapFlowManager.IsMapTransition) return;

            MapFlowManager.IsMapTransition = false;
            AdvanceRoom();
        }

        private void ResetRun()
        {
            countedKills.Clear();
            kills = 0;
            roomsCleared = 0;
            rewardsCollected = 0;
            roomReached = 1;
            runTimeSeconds = 0f;
            runStarted = false;
            frozen = false;
            echoAwarded = false;
            echoAward = 0;
            UnhookPlayerHealth();
        }

        public void StartNewRun()
        {
            ResetRun();
            StartRunIfNeeded();
        }

        public void AdvanceRoom()
        {
            StartRunIfNeeded();
            roomReached++;
        }

        private void StartRunIfNeeded()
        {
            if (runStarted) return;
            runStarted = true;
            frozen = false;
        }

        private void OnRoomChanged(int index)
        {
            StartRunIfNeeded();
            roomReached = Mathf.Max(roomReached, index + 1);
        }

        private void OnRoomLoaded(RoomConfig config, GameObject room)
        {
            StartRunIfNeeded();
        }

        private void OnRoomCleared()
        {
            if (!runStarted) StartRunIfNeeded();
            roomsCleared++;
        }

        /// <summary>
        /// Public room-clear bridge for the _Arena RoomRunDirector path, which drives clears through
        /// its own EncounterController.OnRoomCleared (UnityEvent) rather than RoomLoader.OnRoomCleared
        /// (the static event this class subscribes to). Without this bridge roomsCleared stays 0 on the
        /// demo path → Echo award floored, death/victory screen stuck on "ODA 1". Mirrors OnRoomCleared.
        /// </summary>
        public void NotifyRoomCleared()
        {
            OnRoomCleared();
        }

        public void RecordRewardCollected()
        {
            if (!runStarted) StartRunIfNeeded();
            rewardsCollected++;
        }

        public bool TryMarkEchoAwarded(int amount)
        {
            if (echoAwarded) return false;

            echoAwarded = true;
            echoAward = Mathf.Max(0, amount);
            return true;
        }

        private void OnKill(KillEvent e)
        {
            int id = e.victim != null ? e.victim.GetInstanceID() : 0;
            if (id != 0 && !countedKills.Add(id)) return;
            kills++;
        }

        private void HookPlayerHealthIfNeeded()
        {
            if (playerHealth != null) return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.OnDeath.AddListener(Freeze);
        }

        private void UnhookPlayerHealth()
        {
            if (playerHealth != null)
                playerHealth.OnDeath.RemoveListener(Freeze);
            playerHealth = null;
        }

        private void Freeze()
        {
            frozen = true;
        }

        private int GetRoomReached()
        {
            return Mathf.Max(1, roomReached);
        }

    }
}
