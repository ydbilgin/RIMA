using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [CreateAssetMenu(menuName = "RIMA/Room/RoomTemplate", fileName = "RoomTemplate_New", order = 200)]
    public class RoomTemplateSO : ScriptableObject
    {
        public string schemaVersion = "1.0";
        public string roomId;
        public string biomeId;
        public RIMA.RoomType roomType;
        public RectInt bounds;

        public List<DoorSocket> doorSockets = new List<DoorSocket>();
        public PlayerSpawnSocket playerSpawn;
        public List<EnemySpawnSocket> enemySpawnSockets = new List<EnemySpawnSocket>();
        public CameraBounds cameraBounds;
        public GameObject prefabRef;

        [Header("Painted Background Layers (Map Plan v1 LOCK — S91, Multi-Layer Painter v1 LOCK — S92)")]
        [Tooltip("Stacked painted background sprites, Hades-style. Render order = sortingOrder per layer. Empty list = no painted bg.")]
        public List<BackgroundLayerData> backgroundLayers = new List<BackgroundLayerData>();

        public List<string> encounterTags = new List<string>();
        public List<string> difficultyTags = new List<string>();
        public List<string> blockerTags = new List<string>();

        [Header("Lighting")]
        public RoomLightingProfileSO lightingProfile;

        [Header("Props (Sprint 12)")]
        public List<RIMA.MapDesigner.Props.PropPlacementData> props = new List<RIMA.MapDesigner.Props.PropPlacementData>();

        [Header("Walkable Grid (Sprint 13 — Condition 1 fix)")]
        [Tooltip("Per-tile walkability map. Index = (y * bounds.width) + x. true = walkable, false = wall/blocked. Empty array = full bounds walkable (fallback).")]
        public bool[] walkableGrid;

        [Header("Overlay Mask (Modular Props K2)")]
        [Tooltip("Per-tile overlay map. Index = (y * bounds.width) + x. 0 = none, 1..N = overlay tile index. Empty array = no overlay.")]
        public int[] overlayMask;

        public const string ExitSlotNorthWestId = "door_NW_01";
        public const string ExitSlotNorthId = "door_N_01";
        public const string ExitSlotNorthEastId = "door_NE_01";

        public int ValidExitSlotCount
        {
            get
            {
                DoorSocket[] slots = ResolveExitSlots();
                int count = 0;
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i] != null)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool IsWalkable(Vector2Int tilePos)
        {
            if (walkableGrid == null || walkableGrid.Length == 0)
            {
                return tilePos.x >= bounds.xMin && tilePos.x < bounds.xMax &&
                       tilePos.y >= bounds.yMin && tilePos.y < bounds.yMax;
            }

            int lx = tilePos.x - bounds.xMin;
            int ly = tilePos.y - bounds.yMin;
            if (lx < 0 || lx >= bounds.width || ly < 0 || ly >= bounds.height) return false;

            int idx = (ly * bounds.width) + lx;
            return idx >= 0 && idx < walkableGrid.Length && walkableGrid[idx];
        }

        public int GetOverlayTileIndex(Vector2Int tilePos)
        {
            if (overlayMask == null || overlayMask.Length == 0)
            {
                return 0;
            }

            int lx = tilePos.x - bounds.xMin;
            int ly = tilePos.y - bounds.yMin;
            if (lx < 0 || lx >= bounds.width || ly < 0 || ly >= bounds.height) return 0;

            int idx = (ly * bounds.width) + lx;
            return idx >= 0 && idx < overlayMask.Length ? overlayMask[idx] : 0;
        }

        public DoorSocket[] ResolveExitSlots()
        {
            DoorSocket[] slots = new DoorSocket[3];
            if (doorSockets == null)
            {
                return slots;
            }

            for (int i = 0; i < doorSockets.Count; i++)
            {
                DoorSocket socket = doorSockets[i];
                int slotIndex = ExitSlotIndex(socket);
                if (slotIndex < 0 || slots[slotIndex] != null || !IsValidExitSlot(socket))
                {
                    continue;
                }

                slots[slotIndex] = socket;
            }

            return slots;
        }

        public bool TryResolveExitSlotsForDoorCount(int doorCount, List<DoorSocket> selectedSlots)
        {
            if (selectedSlots == null)
            {
                return false;
            }

            selectedSlots.Clear();
            if (doorCount <= 0)
            {
                return true;
            }

            DoorSocket[] slots = ResolveExitSlots();
            if (CountSlots(slots) < doorCount)
            {
                return false;
            }

            if (doorCount == 1)
            {
                selectedSlots.Add(slots[1] ?? ClosestToHorizontalCenter(slots));
                return selectedSlots[0] != null;
            }

            if (doorCount == 2)
            {
                DoorSocket left = slots[0];
                DoorSocket right = slots[2];
                if (left != null && right != null)
                {
                    selectedSlots.Add(left);
                    selectedSlots.Add(right);
                    return true;
                }

                DoorSocket center = slots[1];
                DoorSocket wing = left ?? right;
                if (center == null || wing == null)
                {
                    return false;
                }

                if (wing.position.x < center.position.x)
                {
                    selectedSlots.Add(wing);
                    selectedSlots.Add(center);
                }
                else
                {
                    selectedSlots.Add(center);
                    selectedSlots.Add(wing);
                }

                return true;
            }

            if (doorCount == 3)
            {
                if (slots[0] == null || slots[1] == null || slots[2] == null)
                {
                    return false;
                }

                selectedSlots.Add(slots[0]);
                selectedSlots.Add(slots[1]);
                selectedSlots.Add(slots[2]);
                return true;
            }

            return false;
        }

        public bool IsValidExitSlot(DoorSocket socket)
        {
            if (socket == null || !socket.isExit || socket.direction != RIMA.DoorDirection.North || ExitSlotIndex(socket) < 0)
            {
                return false;
            }

            return IsWalkable(socket.position)
                && !IsWalkable(socket.position + Vector2Int.up)
                && IsWalkable(socket.position + Vector2Int.down)
                && IsWalkable(socket.position + Vector2Int.down * 2);
        }

        public static int ExitSlotIndex(DoorSocket socket)
        {
            return socket != null ? ExitSlotIndex(socket.socketId) : -1;
        }

        public static int ExitSlotIndex(string socketId)
        {
            switch (socketId)
            {
                case ExitSlotNorthWestId:
                    return 0;
                case ExitSlotNorthId:
                    return 1;
                case ExitSlotNorthEastId:
                    return 2;
                default:
                    return -1;
            }
        }

        public static string ExitSlotLabel(int slotIndex)
        {
            switch (slotIndex)
            {
                case 0:
                    return "NW";
                case 1:
                    return "N";
                case 2:
                    return "NE";
                default:
                    return string.Empty;
            }
        }

        public static string ExitSlotId(int slotIndex)
        {
            switch (slotIndex)
            {
                case 0:
                    return ExitSlotNorthWestId;
                case 1:
                    return ExitSlotNorthId;
                case 2:
                    return ExitSlotNorthEastId;
                default:
                    return string.Empty;
            }
        }

        private static int CountSlots(DoorSocket[] slots)
        {
            int count = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        private DoorSocket ClosestToHorizontalCenter(DoorSocket[] slots)
        {
            float centerX = bounds.xMin + (bounds.width - 1) * 0.5f;
            DoorSocket best = null;
            float bestDistance = float.MaxValue;
            for (int i = 0; i < slots.Length; i++)
            {
                DoorSocket slot = slots[i];
                if (slot == null)
                {
                    continue;
                }

                float distance = Mathf.Abs(slot.position.x - centerX);
                if (best == null || distance < bestDistance)
                {
                    best = slot;
                    bestDistance = distance;
                }
            }

            return best;
        }
    }
}
