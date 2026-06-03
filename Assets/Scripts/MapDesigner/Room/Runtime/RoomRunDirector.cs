using System.Collections.Generic;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Runtime
{
    public sealed class RoomRunDirector : MonoBehaviour
    {
        [SerializeField] private IsoRoomBuilder builder;
        [SerializeField] private RoomBankSO roomBank;
        [SerializeField] private Transform player;
        [SerializeField] private RoomTemplateSO fallbackTemplate;
        [SerializeField] private int runSeed = 12345;
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private int depthCount = 5;

        private bool warnedMissingPlayer;
        private DungeonGraph graph;

        public DungeonGraph Graph => graph;
        public int CurrentNodeId { get; private set; }
        public DungeonNode CurrentNode => graph?.Get(CurrentNodeId);
        public RIMA.RoomType CurrentRoomType => CurrentNode != null ? CurrentNode.roomType : RIMA.RoomType.Combat;
        public List<DungeonNode> CurrentChoices => graph != null ? graph.ChildrenOf(CurrentNodeId) : new List<DungeonNode>();
        public bool IsRunComplete => CurrentNode == null || CurrentChoices.Count == 0;
        public RoomTemplateSO CurrentTemplate { get; private set; }

        private void Start()
        {
            if (buildOnStart)
            {
                BeginRun();
            }
        }

        public void BeginRun()
        {
            graph = DungeonGraph.Generate(runSeed, depthCount);
            CurrentNodeId = graph.startId;
            BuildCurrentRoom();
        }

        public void BuildCurrentRoom()
        {
            if (builder == null)
            {
                Debug.LogError("[RoomRunDirector] Missing IsoRoomBuilder reference.");
                return;
            }

            if (graph == null)
            {
                Debug.LogWarning("[RoomRunDirector] Missing dungeon graph.");
                return;
            }

            DungeonNode node = CurrentNode;
            if (node == null)
            {
                Debug.LogWarning($"[RoomRunDirector] Missing current node id={CurrentNodeId}.");
                return;
            }

            RoomTemplateSO template = roomBank != null ? roomBank.Pick(CurrentRoomType, runSeed + CurrentNodeId) : null;
            if (template == null)
            {
                template = fallbackTemplate;
            }

            if (template == null)
            {
                Debug.LogError($"[RoomRunDirector] no template for {CurrentRoomType}");
                return;
            }

            CurrentTemplate = template;
            builder.Build(template);

            if (player != null && builder.PlayerSpawnMarker != null)
            {
                player.position = builder.PlayerSpawnMarker.position;
            }
            else if (player == null && !warnedMissingPlayer)
            {
                Debug.LogWarning("[RoomRunDirector] Missing player reference; skipping teleport.");
                warnedMissingPlayer = true;
            }

            Debug.Log($"[RoomRunDirector] Built node id={node.id} depth={node.depth} type={node.roomType} choices={CurrentChoices.Count} template={template.roomId}");

            // Exit doors = this node's branch choices (door count + each door's destination type).
            List<RIMA.RoomType> doorTypes = new List<RIMA.RoomType>();
            foreach (DungeonNode child in CurrentChoices)
            {
                doorTypes.Add(child.roomType);
            }
            builder.BuildExitDoors(doorTypes);

            // TODO: encounter start -> clear -> slow-mo -> reward -> illuminate doors -> walk-to-advance.
        }

        public void AdvanceTo(int choiceIndex)
        {
            if (IsRunComplete)
            {
                Debug.Log("[RoomRunDirector] run complete");
                return;
            }

            List<DungeonNode> choices = CurrentChoices;
            if (choiceIndex < 0 || choiceIndex >= choices.Count)
            {
                Debug.LogWarning($"[RoomRunDirector] Invalid choice index {choiceIndex}; choices={choices.Count}.");
                return;
            }

            CurrentNodeId = choices[choiceIndex].id;
            BuildCurrentRoom();
        }

        [ContextMenu("Advance First Choice")]
        private void DebugAdvance()
        {
            if (!IsRunComplete)
            {
                AdvanceTo(0);
            }
        }
    }
}
