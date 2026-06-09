using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    [System.Serializable]
    public class RoomNode
    {
        public int id;
        public RoomType roomType;
        public int depth;   // how deep in the run (for map Y position)
        public int lane;    // 0=center, -1=left, 1=right (for map X position)
        /// <summary>direction → hedef node id</summary>
        public Dictionary<DoorDirection, int> exits = new Dictionary<DoorDirection, int>();
        public bool visited;
        public bool revealed; // seen via MapFragment
    }

    /// <summary>
    /// STS2-style dungeon graph — real branching paths.
    ///
    /// Layout (12 rooms + boss):
    ///   [0] Start
    ///   [1] Combat (forced)
    ///   [2]Combat / [3]Elite  ← Fork 1 (risk/reward)
    ///   [4] Chest             ← Merge 1
    ///   [5]Rest / [6]Merchant ← Fork 2 (recovery/buy)
    ///   [7] Elite             ← Merge 2
    ///   [8]Combat / [9]Combat ← Fork 3
    ///   [10] Pre-Boss         ← Merge 3
    ///   [11] BOSS
    ///
    /// Player picks ONE path at each fork. Map reveals room types 1 node ahead.
    /// </summary>
    // ⚠️ LEGACY _IsoGame map subsystem. NAME COLLISION with the live pure class
    // RIMA.MapDesigner.Room.Runtime.DungeonGraph (used by RoomRunDirector, has BuildDemoSequence).
    // Live demo flow does NOT use this MonoBehaviour. Post-demo: rename → LegacyIsoDungeonGraph + delete
    // with the rest of the legacy cluster (OVERLAP_CLEANUP_DECISION_2026-06-09.md, HEDEF 1).
    [System.Obsolete("LEGACY _IsoGame dungeon graph — live is RIMA.MapDesigner.Room.Runtime.DungeonGraph. Post-demo removal.", false)]
    public class DungeonGraph : MonoBehaviour
    {
        public static DungeonGraph Instance { get; private set; }

        [Header("Layout")]
        [SerializeField] private int bossNodeId = 11;

        private List<RoomNode> nodes = new List<RoomNode>();
        private int currentNodeId = 0;

        public RoomNode CurrentNode => nodes[currentNodeId];
        public int CurrentNodeId => currentNodeId;
        public int BossNodeId => bossNodeId;
        public int TotalNodes => nodes.Count;

        // Legacy compat — used by RRM for boss check
        public int BossRoomIndex => bossNodeId;

        // Full node list for map rendering
        public IReadOnlyList<RoomNode> AllNodes => nodes;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
            Generate();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ─── Generation ──────────────────────────────────────────────────────────

        public void Generate()
        {
            nodes.Clear();
            currentNodeId = 0;
            BuildGraph();
            // Reveal start + its exits on map
            nodes[0].visited = true;
            RevealExitsFrom(0);
            LogGraph();
        }

        private void BuildGraph()
        {
            // Fork 2 tipi her run'da değişir
            RoomType fork2A = Random.value < 0.5f ? RoomType.Event : RoomType.Chest;
            RoomType fork2B = Random.value < 0.5f ? RoomType.Merchant : RoomType.Combat;
            RoomType fork3B = Random.value < 0.3f ? RoomType.Elite : RoomType.Combat;

            // STS2 gibi 3-yönlü fork: %40 fork-1, %25 fork-3
            bool triple1 = Random.value < 0.40f;
            bool triple3 = Random.value < 0.25f;

            // ─── Sabit 12 node ───────────────────────────────────
            nodes.Add(MakeNode(0,  RoomType.Combat,   0,  0)); // start
            nodes.Add(MakeNode(1,  RoomType.Combat,   1,  0)); // warmup
            nodes.Add(MakeNode(2,  RoomType.Combat,   2, -1)); // fork1-N
            nodes.Add(MakeNode(3,  RoomType.Elite,    2,  1)); // fork1-E
            nodes.Add(MakeNode(4,  RoomType.Chest,    3,  0)); // merge1
            nodes.Add(MakeNode(5,  fork2A,            4, -1)); // fork2-N
            nodes.Add(MakeNode(6,  fork2B,            4,  1)); // fork2-E
            nodes.Add(MakeNode(7,  RoomType.Elite,    5,  0)); // merge2
            nodes.Add(MakeNode(8,  RoomType.Combat,   6, -1)); // fork3-N
            nodes.Add(MakeNode(9,  fork3B,            6,  1)); // fork3-E
            nodes.Add(MakeNode(10, RoomType.Combat,   7,  0)); // pre-boss merge
            nodes.Add(MakeNode(11, RoomType.Boss,     8,  0)); // boss

            // ─── İsteğe bağlı 3. yol node'ları ─────────────────
            int fork1W = -1;
            if (triple1)
            {
                fork1W = nodes.Count;
                nodes.Add(MakeNode(fork1W, RoomType.Combat, 2, -2)); // fork1-W
            }
            int fork3W = -1;
            if (triple3)
            {
                fork3W = nodes.Count;
                nodes.Add(MakeNode(fork3W, RoomType.Event, 6, -2)); // fork3-W
            }

            // ─── Bağlantılar ─────────────────────────────────────
            nodes[0].exits[DoorDirection.North] = 1;

            nodes[1].exits[DoorDirection.North] = 2;
            nodes[1].exits[DoorDirection.East]  = 3;
            if (triple1) nodes[1].exits[DoorDirection.West] = fork1W;

            nodes[2].exits[DoorDirection.North] = 4;
            nodes[3].exits[DoorDirection.North] = 4;
            if (triple1) nodes[fork1W].exits[DoorDirection.North] = 4;

            nodes[4].exits[DoorDirection.North] = 5;
            nodes[4].exits[DoorDirection.East]  = 6;

            nodes[5].exits[DoorDirection.North] = 7;
            nodes[6].exits[DoorDirection.North] = 7;

            nodes[7].exits[DoorDirection.North] = 8;
            nodes[7].exits[DoorDirection.East]  = 9;
            if (triple3) nodes[7].exits[DoorDirection.West] = fork3W;

            nodes[8].exits[DoorDirection.North]  = 10;
            nodes[9].exits[DoorDirection.North]  = 10;
            if (triple3) nodes[fork3W].exits[DoorDirection.North] = 10;

            nodes[10].exits[DoorDirection.North] = 11;
        }

        /// <summary>
        /// Şu anki konumdan kaç adım ilerisinin haritada görünür (reveal) olduğunu döner.
        /// Tüm dallar reveal edilmişse o derinliği sayar; eksik branch varsa durur.
        /// </summary>
        public int GetRevealedStepsAhead()
        {
            var frontier = new HashSet<int>();
            foreach (var kvp in CurrentNode.exits) frontier.Add(kvp.Value);

            int steps = 0;
            while (frontier.Count > 0 && steps < 5)
            {
                foreach (int id in frontier)
                    if (!nodes[id].revealed) return steps;
                steps++;
                var next = new HashSet<int>();
                foreach (int id in frontier)
                    foreach (var kvp in nodes[id].exits) next.Add(kvp.Value);
                frontier = next;
            }
            return steps;
        }

        public bool HasForwardExits()
        {
            return CurrentNode.exits.Count > 0;
        }

        public bool HasRevealedPathBeyondCurrentExits()
        {
            return GetRevealedStepsAhead() > 1;
        }

        private static RoomNode MakeNode(int id, RoomType type, int depth, int lane)
            => new RoomNode { id = id, roomType = type, depth = depth, lane = lane };

        // ─── Navigation ──────────────────────────────────────────────────────────

        /// <summary>
        /// Player bir kapıdan geçince çağrılır.
        /// Başarılı → grafdaki pozisyonu günceller, nextNode döner.
        /// Başarısız → false (bu yönde çıkış yok).
        /// </summary>
        public bool Navigate(DoorDirection dir, out RoomNode nextNode)
        {
            nextNode = null;
            if (!CurrentNode.exits.TryGetValue(dir, out int nextId)) return false;

            currentNodeId = nextId;
            nextNode = nodes[currentNodeId];
            nextNode.visited = true;
            // Reveal next layer exits on map
            RevealExitsFrom(currentNodeId);

            Debug.Log($"[DungeonGraph] → Node {currentNodeId} ({nextNode.roomType}) via {dir}");
            return true;
        }

        /// <summary>Şu anki odanın çıkışları: direction → hedef oda tipi.</summary>
        public Dictionary<DoorDirection, RoomType> GetCurrentExits()
        {
            var result = new Dictionary<DoorDirection, RoomType>();
            foreach (var kvp in CurrentNode.exits)
                result[kvp.Key] = nodes[kvp.Value].roomType;
            return result;
        }

        public bool IsBossRoom() => CurrentNode.roomType == RoomType.Boss;

        // ─── Map Reveal ─────────────────────────────────────────────────────────

        private void RevealExitsFrom(int nodeId)
        {
            foreach (var kvp in nodes[nodeId].exits)
                nodes[kvp.Value].revealed = true;
        }

        /// <summary>Harita parçası toplandığında çağrılır. steps=1 → 1 adım, steps=2 → 2 adım ilerisini açar.</summary>
        public void RevealAhead(int steps)
        {
            RevealFromNode(currentNodeId, steps);
            Debug.Log($"[DungeonGraph] Revealed {steps} step(s) ahead from node {currentNodeId}.");
        }

        private void RevealFromNode(int nodeId, int stepsLeft)
        {
            if (stepsLeft <= 0) return;
            foreach (var kvp in nodes[nodeId].exits)
            {
                nodes[kvp.Value].revealed = true;
                if (stepsLeft > 1)
                    RevealFromNode(kvp.Value, stepsLeft - 1);
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────

        public static DoorDirection Opposite(DoorDirection d) => d switch
        {
            DoorDirection.North => DoorDirection.South,
            DoorDirection.South => DoorDirection.North,
            DoorDirection.East  => DoorDirection.West,
            DoorDirection.West  => DoorDirection.East,
            _                   => DoorDirection.South
        };

        private void LogGraph()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"[DungeonGraph] {nodes.Count} nodes:");
            foreach (var node in nodes)
            {
                sb.Append($"  [{node.id}] {node.roomType,-10} d={node.depth} exits: ");
                foreach (var kvp in node.exits)
                    sb.Append($"{kvp.Key}→{kvp.Value}({nodes[kvp.Value].roomType})  ");
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
    }
}
