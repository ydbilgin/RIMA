using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RIMA.RoomPainter
{
    [Serializable]
    public sealed class RoomDataDTO
    {
        public string roomId;
        public string displayName;
        public List<RoomData.TileCellRecord> floorCells = new List<RoomData.TileCellRecord>();
        public List<RoomData.TileCellRecord> cliffCells = new List<RoomData.TileCellRecord>();
        public List<WallCell> wallCells = new List<WallCell>();
        public List<RoomData.PropPlacement> propPlacements = new List<RoomData.PropPlacement>();
        public List<RoomData.PortalPlacement> portalPlacements = new List<RoomData.PortalPlacement>();
    }

    public static class RoomDataJson
    {
        public static RoomDataDTO ToDto(RoomData room)
        {
            RoomDataDTO dto = new RoomDataDTO();
            if (room == null)
            {
                return dto;
            }

            room.EnsureDefaults();
            // Fold any legacy wallSegments into wallCells BEFORE copying: the DTO
            // carries wallCells but not wallSegments, so without this an un-migrated
            // (segment-only) room would lose every wall on a JSON round-trip.
            // MigrateSegmentsToCells is idempotent (overwrites by coordinate), so it
            // is safe to run on every write.
            RoomDataMutator.MigrateSegmentsToCells(room);
            dto.roomId = room.roomId;
            dto.displayName = room.displayName;
            dto.floorCells = new List<RoomData.TileCellRecord>(room.floorCells);
            dto.cliffCells = new List<RoomData.TileCellRecord>(room.cliffCells);
            dto.wallCells = new List<WallCell>(room.wallCells);
            dto.propPlacements = new List<RoomData.PropPlacement>(room.propPlacements);
            dto.portalPlacements = new List<RoomData.PortalPlacement>(room.portalPlacements);
            return dto;
        }

        public static void ApplyTo(RoomData room, RoomDataDTO dto)
        {
            if (room == null || dto == null)
            {
                return;
            }

            room.roomId = dto.roomId;
            room.displayName = dto.displayName;
            room.floorCells = dto.floorCells != null
                ? new List<RoomData.TileCellRecord>(dto.floorCells)
                : new List<RoomData.TileCellRecord>();
            room.cliffCells = dto.cliffCells != null
                ? new List<RoomData.TileCellRecord>(dto.cliffCells)
                : new List<RoomData.TileCellRecord>();
            room.wallCells = dto.wallCells != null
                ? new List<WallCell>(dto.wallCells)
                : new List<WallCell>();
            room.propPlacements = dto.propPlacements != null
                ? new List<RoomData.PropPlacement>(dto.propPlacements)
                : new List<RoomData.PropPlacement>();
            room.portalPlacements = dto.portalPlacements != null
                ? new List<RoomData.PortalPlacement>(dto.portalPlacements)
                : new List<RoomData.PortalPlacement>();
            room.EnsureDefaults();
        }

        public static void Write(RoomData room, string path)
        {
            if (room == null || string.IsNullOrEmpty(path))
            {
                return;
            }

            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, JsonUtility.ToJson(ToDto(room), true));
        }

        public static RoomDataDTO Read(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            string json = File.ReadAllText(path);
            return string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<RoomDataDTO>(json);
        }

        public static RoomData ReadRoom(string path)
        {
            RoomDataDTO dto = Read(path);
            if (dto == null)
            {
                return null;
            }

            RoomData room = ScriptableObject.CreateInstance<RoomData>();
            ApplyTo(room, dto);
            return room;
        }
    }
}
