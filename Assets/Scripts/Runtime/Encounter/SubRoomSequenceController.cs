using System;
using System.Collections;
using System.Collections.Generic;
using RIMA.MapDesigner.Encounter;
using RIMA.MapDesigner.Room.Data;
using RIMA.Save;
using UnityEngine;

namespace RIMA.Runtime.Encounter
{
    public enum SubRoomState { Idle, Loading, Active, Cleared, Transitioning, EncounterComplete }

    public class SubRoomSequenceController : MonoBehaviour
    {
        public static SubRoomSequenceController Active { get; private set; }
        public SubRoomState State { get; private set; }
        public EncounterTemplateSO Template { get; private set; }
        public int CurrentSubRoomIndex { get; private set; }
        public string CurrentSubRoomKey => Template.subRooms[CurrentSubRoomIndex].subRoomKey;
        public bool IsFinalSubRoom => Template.subRooms[CurrentSubRoomIndex].isFinal;

        public event Action<int> OnSubRoomEntered;
        public event Action<int> OnSubRoomCleared;
        public event Action OnEncounterComplete;

        private readonly List<GameObject> spawnedEnemies = new List<GameObject>();
        private readonly List<IntraEncounterDoorTrigger> outgoingTriggers = new List<IntraEncounterDoorTrigger>();
        private IEncounterBank encounterBank;
        private GameObject currentSubRoomRoot;
        private int killCount;
        private int killQuota;

        private void Awake()
        {
            State = SubRoomState.Idle;
        }

        public void StartEncounter(EncounterTemplateSO template)
        {
            if (template == null || template.subRooms == null || template.subRooms.Count == 0)
            {
                Debug.LogError("[SubRoomSequenceController] Cannot start encounter: template is empty.");
                return;
            }

            Template = template;
            CurrentSubRoomIndex = FindEntryIndex(template);
            encounterBank = ResolveEncounterBank();
            Active = this;
            StartCoroutine(LoadSubRoomCoroutine());
        }

        public RoomTemplateSO GetCurrentSubRoom()
        {
            if (Template == null ||
                Template.subRooms == null ||
                CurrentSubRoomIndex < 0 ||
                CurrentSubRoomIndex >= Template.subRooms.Count)
            {
                return null;
            }

            return Template.subRooms[CurrentSubRoomIndex]?.room;
        }

        public void AdvanceSubRoom(string viaFromDoorSocketId)
        {
            if (State != SubRoomState.Cleared || Template == null) return;

            SubRoomLink link = FindLink(viaFromDoorSocketId);
            if (link == null)
            {
                Debug.LogWarning($"[SubRoomSequenceController] No sub-room link from door '{viaFromDoorSocketId}'.");
                return;
            }

            int nextIndex = FindSubRoomIndex(link.toSubRoomKey);
            if (nextIndex < 0)
            {
                Debug.LogWarning($"[SubRoomSequenceController] Target sub-room '{link.toSubRoomKey}' not found.");
                return;
            }

            SaveTransitionCheckpoint(nextIndex);
            State = SubRoomState.Transitioning;
            if (RoomTransitionFX.Instance != null)
            {
                RoomTransitionFX.Instance.DoTransition(() => SwapSubRoomWhileBlack(nextIndex, link.toEntryDoorSocketId));
            }
            else
            {
                SwapSubRoomWhileBlack(nextIndex, link.toEntryDoorSocketId);
            }
        }

        public void OnEnemyKilled(GameObject enemy)
        {
            if (State != SubRoomState.Active) return;

            killCount++;
            spawnedEnemies.Remove(enemy);
            if (killCount >= killQuota)
                MarkCurrentSubRoomCleared();
        }

        public void TerminateEncounter()
        {
            TeardownCurrentSubRoom();
            if (Active == this) Active = null;
            State = SubRoomState.Idle;
            DestroyRuntimeObject(gameObject);
        }

        private IEnumerator LoadSubRoomCoroutine()
        {
            State = SubRoomState.Loading;
            TeardownCurrentSubRoom();
            BuildCurrentSubRoom();
            yield return null;

            killCount = 0;
            killQuota = encounterBank != null
                ? Mathf.Max(0, encounterBank.GetSubRoomKillQuota(Template.encounterId, CurrentSubRoomIndex))
                : 0;

            State = SubRoomState.Active;
            OnSubRoomEntered?.Invoke(CurrentSubRoomIndex);
            if (killQuota == 0)
                MarkCurrentSubRoomCleared();
        }

        private void SwapSubRoomWhileBlack(int nextIndex, string toEntryDoorSocketId)
        {
            CurrentSubRoomIndex = nextIndex;
            State = SubRoomState.Loading;
            TeardownCurrentSubRoom();
            BuildCurrentSubRoom();
            MovePlayerToDoorSocket(toEntryDoorSocketId);

            killCount = 0;
            killQuota = encounterBank != null
                ? Mathf.Max(0, encounterBank.GetSubRoomKillQuota(Template.encounterId, CurrentSubRoomIndex))
                : 0;

            State = SubRoomState.Active;
            OnSubRoomEntered?.Invoke(CurrentSubRoomIndex);
            if (killQuota == 0)
                MarkCurrentSubRoomCleared();
        }

        private void BuildCurrentSubRoom()
        {
            RoomTemplateSO room = GetCurrentSubRoom();
            if (room == null)
            {
                Debug.LogError($"[SubRoomSequenceController] Missing room for sub-room index {CurrentSubRoomIndex}.");
                return;
            }

            currentSubRoomRoot = new GameObject($"SubRoom_{CurrentSubRoomIndex}_{CurrentSubRoomKey}");
            currentSubRoomRoot.transform.SetParent(transform, false);

            if (room.prefabRef != null)
            {
                GameObject instance = Instantiate(room.prefabRef, currentSubRoomRoot.transform);
                instance.name = $"RoomPrefab_{room.roomId}";
            }

            PaintBackgroundLayers(room);
            SpawnEnemies(room);
            SpawnOutgoingTriggers(room);
            RefreshCameraBounds(room);
        }

        private void PaintBackgroundLayers(RoomTemplateSO room)
        {
            if (room.backgroundLayers == null || room.backgroundLayers.Count == 0) return;

            GameObject bgRoot = new GameObject("PaintedBackground");
            bgRoot.transform.SetParent(currentSubRoomRoot.transform, false);
            bgRoot.transform.localPosition = new Vector3(
                room.bounds.xMin + room.bounds.width * 0.5f,
                room.bounds.yMin + room.bounds.height * 0.5f,
                0f);

            for (int i = 0; i < room.backgroundLayers.Count; i++)
            {
                BackgroundLayerData layer = room.backgroundLayers[i];
                if (layer == null || !layer.visible || layer.sprite == null) continue;

                GameObject layerGO = new GameObject($"Layer_{i:D2}_{layer.layerName}");
                layerGO.transform.SetParent(bgRoot.transform, false);
                layerGO.transform.localPosition = new Vector3(layer.offset.x, layer.offset.y, 1f);
                layerGO.transform.localScale = new Vector3(layer.scale.x, layer.scale.y, 1f);

                SpriteRenderer sr = layerGO.AddComponent<SpriteRenderer>();
                sr.sprite = layer.sprite;
                sr.sortingOrder = layer.sortingOrder;
                sr.color = layer.tint;
                sr.drawMode = SpriteDrawMode.Simple;
            }
        }

        private void SpawnEnemies(RoomTemplateSO room)
        {
            SubRoomEnemyPlan plan = encounterBank != null
                ? encounterBank.GetSubRoomEnemies(Template.encounterId, CurrentSubRoomIndex)
                : null;

            if (plan?.Enemies == null) return;

            foreach (EnemyAssignment assignment in plan.Enemies)
            {
                if (assignment == null || assignment.enemyPrefab == null) continue;
                if (!TryGetEnemySocket(room, assignment.socketId, out EnemySpawnSocket socket)) continue;

                GameObject enemy = Instantiate(assignment.enemyPrefab, TileToWorld(socket.position), Quaternion.identity, currentSubRoomRoot.transform);
                spawnedEnemies.Add(enemy);

                Health health = enemy.GetComponent<Health>();
                if (health == null) health = enemy.AddComponent<Health>();
                GameObject localEnemy = enemy;
                health.OnDeath.AddListener(() => OnEnemyKilled(localEnemy));

                if (assignment.isElite)
                    EliteAffix.Apply(enemy, EliteAffix.RandomAffix());
            }
        }

        private void SpawnOutgoingTriggers(RoomTemplateSO room)
        {
            SubRoomEntry entry = Template.subRooms[CurrentSubRoomIndex];
            if (entry.links == null) return;

            foreach (SubRoomLink link in entry.links)
            {
                if (link == null) continue;
                if (!TryGetDoorSocket(room, link.fromDoorSocketId, out DoorSocket socket)) continue;

                GameObject triggerGO = new GameObject($"IntraDoorTrigger_{link.fromDoorSocketId}");
                triggerGO.transform.SetParent(currentSubRoomRoot.transform, false);
                triggerGO.transform.position = TileToWorld(socket.position);

                BoxCollider2D col = triggerGO.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = new Vector2(Mathf.Max(1f, socket.widthInTiles), 2f);

                IntraEncounterDoorTrigger trigger = triggerGO.AddComponent<IntraEncounterDoorTrigger>();
                trigger.Configure(link.fromDoorSocketId);
                trigger.SetActive(false);
                outgoingTriggers.Add(trigger);
            }
        }

        private void MarkCurrentSubRoomCleared()
        {
            if (State != SubRoomState.Active) return;

            State = SubRoomState.Cleared;
            OnSubRoomCleared?.Invoke(CurrentSubRoomIndex);

            if (IsFinalSubRoom)
            {
                State = SubRoomState.EncounterComplete;
                CheckpointManager.Instance.Clear();
                RuntimeRoomManager.Instance?.OnEncounterFinalCleared();
                OnEncounterComplete?.Invoke();
                if (Active == this) Active = null;
                DestroyRuntimeObject(this);
                return;
            }

            foreach (IntraEncounterDoorTrigger trigger in outgoingTriggers)
                if (trigger != null) trigger.SetActive(true);

            HUDController.Instance?.SetRoomStatus("kapı açıldı - devam et");
        }

        private void RefreshCameraBounds(RoomTemplateSO room)
        {
            CameraFollow follow = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;
            if (follow == null) return;

            Renderer floorRenderer = currentSubRoomRoot.GetComponentInChildren<Renderer>();
            if (floorRenderer != null)
            {
                follow.SetBounds(floorRenderer.bounds);
                return;
            }

            RectInt rect = room.cameraBounds.tileRect.width > 0 && room.cameraBounds.tileRect.height > 0
                ? room.cameraBounds.tileRect
                : room.bounds;
            follow.SetBounds(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMax));
        }

        private void MovePlayerToDoorSocket(string socketId)
        {
            RoomTemplateSO room = GetCurrentSubRoom();
            if (room == null || string.IsNullOrEmpty(socketId)) return;
            if (!TryGetDoorSocket(room, socketId, out DoorSocket socket)) return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = TileToWorld(socket.position);
        }

        private void SaveTransitionCheckpoint(int nextIndex)
        {
            if (Template == null || Template.subRooms == null || nextIndex < 0 || nextIndex >= Template.subRooms.Count)
                return;

            SubRoomEntry nextEntry = Template.subRooms[nextIndex];
            RoomTemplateSO nextRoom = nextEntry != null ? nextEntry.room : null;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Health health = player != null ? player.GetComponent<Health>() : null;

            CheckpointManager.Instance.Save(new RIMA.Save.CheckpointData
            {
                playerHealth = health != null ? health.CurrentHP : 0,
                playerMaxHealth = health != null ? health.MaxHP : 0,
                currentRoomId = nextRoom != null ? nextRoom.roomId : nextEntry?.subRoomKey,
                currentActId = Template.biomeId,
                inventory = Array.Empty<string>(),
                equipped = Array.Empty<string>()
            });
        }

        private void TeardownCurrentSubRoom()
        {
            spawnedEnemies.Clear();
            outgoingTriggers.Clear();
            if (currentSubRoomRoot != null)
                DestroyRuntimeObject(currentSubRoomRoot);
            currentSubRoomRoot = null;
        }

        private static void DestroyRuntimeObject(UnityEngine.Object obj)
        {
            if (obj == null) return;
            if (Application.isPlaying)
                Destroy(obj);
            else
                DestroyImmediate(obj);
        }

        private IEncounterBank ResolveEncounterBank()
        {
            MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is IEncounterBank bank)
                    return bank;
            }

            return null;
        }

        private SubRoomLink FindLink(string fromDoorSocketId)
        {
            SubRoomEntry entry = Template.subRooms[CurrentSubRoomIndex];
            if (entry.links == null) return null;

            foreach (SubRoomLink link in entry.links)
            {
                if (link != null && link.fromDoorSocketId == fromDoorSocketId)
                    return link;
            }

            return null;
        }

        private int FindEntryIndex(EncounterTemplateSO template)
        {
            for (int i = 0; i < template.subRooms.Count; i++)
                if (template.subRooms[i] != null && template.subRooms[i].isEntry)
                    return i;
            return 0;
        }

        private int FindSubRoomIndex(string subRoomKey)
        {
            for (int i = 0; i < Template.subRooms.Count; i++)
                if (Template.subRooms[i] != null && Template.subRooms[i].subRoomKey == subRoomKey)
                    return i;
            return -1;
        }

        private static bool TryGetEnemySocket(RoomTemplateSO room, string socketId, out EnemySpawnSocket socket)
        {
            socket = null;
            if (room.enemySpawnSockets == null) return false;
            foreach (EnemySpawnSocket candidate in room.enemySpawnSockets)
            {
                if (candidate != null && candidate.socketId == socketId)
                {
                    socket = candidate;
                    return true;
                }
            }
            return false;
        }

        private static bool TryGetDoorSocket(RoomTemplateSO room, string socketId, out DoorSocket socket)
        {
            socket = null;
            if (room.doorSockets == null) return false;
            foreach (DoorSocket candidate in room.doorSockets)
            {
                if (candidate != null && candidate.socketId == socketId)
                {
                    socket = candidate;
                    return true;
                }
            }
            return false;
        }

        private static Vector3 TileToWorld(Vector2Int tile)
        {
            return new Vector3(tile.x, tile.y, 0f);
        }
    }
}

