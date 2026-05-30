using System.Collections.Generic;
using System.Text;
using RIMA.Combat;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA
{
    public sealed class RunStats : MonoBehaviour
    {
        private static RunStats instance;

        private readonly HashSet<int> countedKills = new HashSet<int>();
        private Health playerHealth;
        private int kills;
        private int roomsCleared;
        private int roomReached = 1;
        private float runTimeSeconds;
        private bool runStarted;
        private bool frozen;

        public static RunStats Instance
        {
            get
            {
                EnsureInstance();
                return instance;
            }
        }

        public static int Kills => Instance.kills;
        public static float RunTimeSeconds => Instance.runTimeSeconds;
        public static int RoomReached => Instance.GetRoomReached();
        public static int RoomsCleared => Instance.roomsCleared;
        public static string BuildName => Instance.GetBuildName();
        public static string BuildSeed => Instance.GetBuildSeed();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            EnsureInstance();
        }

        private static void EnsureInstance()
        {
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
            if (instance == this) instance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            CombatEventBus.OnKill -= OnKill;
            RoomLoader.OnRoomChanged -= OnRoomChanged;
            RoomLoader.OnRoomLoaded -= OnRoomLoaded;
            RoomLoader.OnRoomCleared -= OnRoomCleared;
            RoomLoader.OnDemoComplete -= Freeze;
            UnhookPlayerHealth();
        }

        private void Update()
        {
            HookPlayerHealthIfNeeded();

            if (runStarted && !frozen)
                runTimeSeconds += Time.unscaledDeltaTime;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ResetRun();
        }

        private void ResetRun()
        {
            countedKills.Clear();
            kills = 0;
            roomsCleared = 0;
            roomReached = 1;
            runTimeSeconds = 0f;
            runStarted = false;
            frozen = false;
            UnhookPlayerHealth();
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
            int reached = roomReached;
            if (RuntimeRoomManager.Instance != null)
                reached = Mathf.Max(reached, RuntimeRoomManager.Instance.CurrentRoom + 1);
            return Mathf.Max(1, reached);
        }

        private string GetBuildName()
        {
            ClassType primary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            ClassType secondary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.SecondaryClass : ClassType.None;
            List<string> skills = GetEquippedSkillNames();

            string className = secondary == ClassType.None ? primary.ToString() : primary + " / " + secondary;
            if (skills.Count == 0) return className + " + 0 skills";
            return className + " + " + string.Join(", ", skills);
        }

        private string GetBuildSeed()
        {
            ClassType primary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            ClassType secondary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.SecondaryClass : ClassType.None;
            List<string> skills = GetEquippedSkillNames();

            StringBuilder builder = new StringBuilder("RIMA-");
            builder.Append(ClassCode(primary));
            if (secondary != ClassType.None)
                builder.Append(ClassCode(secondary));

            int tokenCount = Mathf.Min(skills.Count, 4);
            for (int i = 0; i < tokenCount; i++)
            {
                builder.Append('-');
                builder.Append(SkillToken(skills[i]));
            }

            builder.Append('x');
            builder.Append(skills.Count);
            return builder.ToString();
        }

        private static string ClassCode(ClassType type)
        {
            return type switch
            {
                ClassType.Warblade => "WB",
                ClassType.Elementalist => "EL",
                ClassType.Shadowblade => "SB",
                ClassType.Ranger => "RG",
                ClassType.Ravager => "RV",
                ClassType.Ronin => "RN",
                ClassType.Gunslinger => "GS",
                ClassType.Brawler => "BR",
                ClassType.Summoner => "SM",
                ClassType.Hexer => "HX",
                _ => "NO"
            };
        }

        private static string SkillToken(string skillName)
        {
            if (string.IsNullOrWhiteSpace(skillName)) return "SK";

            StringBuilder token = new StringBuilder(4);
            bool takeNext = true;
            for (int i = 0; i < skillName.Length && token.Length < 4; i++)
            {
                char c = skillName[i];
                if (!char.IsLetterOrDigit(c))
                {
                    takeNext = true;
                    continue;
                }

                if (takeNext || char.IsDigit(c))
                {
                    token.Append(char.ToUpperInvariant(c));
                    takeNext = false;
                }
            }

            if (token.Length == 0)
            {
                for (int i = 0; i < skillName.Length && token.Length < 4; i++)
                {
                    char c = skillName[i];
                    if (char.IsLetterOrDigit(c))
                        token.Append(char.ToUpperInvariant(c));
                }
            }

            return token.Length > 0 ? token.ToString() : "SK";
        }

        private static List<string> GetEquippedSkillNames()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            List<string> names = new List<string>();
            if (player == null) return names;

            ClassType primary = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            switch (primary)
            {
                case ClassType.Elementalist:
                    AddSlots(player.GetComponent<Elementalist_SkillController>()?.GetAllSlots(), names);
                    break;
                case ClassType.Shadowblade:
                    AddSlots(player.GetComponent<Shadowblade_SkillController>()?.GetAllSlots(), names);
                    break;
                case ClassType.Ranger:
                    AddSlots(player.GetComponent<Ranger_SkillController>()?.GetAllSlots(), names);
                    break;
                case ClassType.Ronin:
                    AddSlots(player.GetComponent<RoninController>()?.GetAllSlots(), names);
                    break;
                default:
                    AddSlots(player.GetComponent<Warblade_SkillController>()?.GetAllSlots(), names);
                    break;
            }

            return names;
        }

        private static void AddSlots(SkillBase[] slots, List<string> names)
        {
            if (slots == null) return;

            for (int i = 0; i < slots.Length; i++)
            {
                SkillBase skill = slots[i];
                if (skill == null) continue;

                string skillName = !string.IsNullOrWhiteSpace(skill.skillName)
                    ? skill.skillName
                    : skill.GetType().Name;
                if (!names.Contains(skillName))
                    names.Add(skillName);
            }
        }
    }
}
