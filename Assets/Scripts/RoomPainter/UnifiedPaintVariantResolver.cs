using System;
using System.Collections.Generic;
using RIMA.Live;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public static class UnifiedPaintVariantResolver
    {
        private const string GroupPrefix = "rima-material://";

        private static readonly Vector3Int[] RebuildOffsets =
        {
            new Vector3Int(-1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0)
        };

        public readonly struct PaintResolution
        {
            public readonly string assetGuidOrName;
            public readonly string sourceGroupId;

            public PaintResolution(string assetGuidOrName, string sourceGroupId)
            {
                this.assetGuidOrName = assetGuidOrName;
                this.sourceGroupId = sourceGroupId;
            }
        }

        public static string BuildGroupId(DesignerCategory category, string groupKey)
        {
            return string.IsNullOrEmpty(groupKey)
                ? string.Empty
                : GroupPrefix + DesignerCategoryMap.RegistryTag(category) + "/" + groupKey;
        }

        public static bool IsGroupId(string value)
        {
            return !string.IsNullOrEmpty(value) &&
                   value.StartsWith(GroupPrefix, StringComparison.Ordinal);
        }

        public static string GroupKeyFor(RegistryEntry entry, DesignerCategory category)
        {
            if (entry == null)
            {
                return string.Empty;
            }

            string name = !string.IsNullOrEmpty(entry.displayName) ? entry.displayName : entry.guid;
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            string normalized = name.Trim().ToLowerInvariant();
            int end = normalized.Length - 1;
            while (end >= 0 && char.IsDigit(normalized[end]))
            {
                end--;
            }

            if (end >= 0 && end < normalized.Length - 1)
            {
                int prefixLength = end + 1;
                if (normalized[end] == '_' || normalized[end] == '-')
                {
                    prefixLength = end;
                }

                return normalized.Substring(0, prefixLength);
            }

            return normalized;
        }

        public static PaintResolution Resolve(RoomData room, DesignerCategory category, string selectedAssetId, Vector3Int cell)
        {
            if (!IsGroupId(selectedAssetId))
            {
                return new PaintResolution(selectedAssetId, null);
            }

            string resolved = ResolveGroupAsset(room, category, selectedAssetId, cell);
            return new PaintResolution(string.IsNullOrEmpty(resolved) ? selectedAssetId : resolved, selectedAssetId);
        }

        public static void ReResolveAffected(RoomData room, DesignerCategory category, Vector3Int center)
        {
            if (room == null || !DesignerCategoryMap.IsTileCategory(category))
            {
                return;
            }

            room.EnsureDefaults();
            List<RoomData.TileCellRecord> records = RecordsFor(room, category);
            if (records == null || records.Count == 0)
            {
                return;
            }

            for (int i = 0; i < RebuildOffsets.Length; i++)
            {
                Vector3Int target = center + RebuildOffsets[i];
                int index = IndexOf(records, target);
                if (index < 0)
                {
                    continue;
                }

                RoomData.TileCellRecord record = records[index];
                if (!IsGroupId(record.sourceGroupId))
                {
                    continue;
                }

                string resolved = ResolveGroupAsset(room, category, record.sourceGroupId, target);
                if (string.IsNullOrEmpty(resolved))
                {
                    continue;
                }

                record.assetGuidOrName = resolved;
                records[index] = record;
            }
        }

        private static string ResolveGroupAsset(RoomData room, DesignerCategory category, string groupId, Vector3Int cell)
        {
            RuntimeAssetRegistry registry = RuntimeAssetRegistry.Instance;
            if (registry == null)
            {
                return string.Empty;
            }

            string groupKey = ExtractGroupKey(groupId);
            if (string.IsNullOrEmpty(groupKey))
            {
                return string.Empty;
            }

            IReadOnlyList<RegistryEntry> entries = registry.GetByTag(DesignerCategoryMap.RegistryTag(category));
            List<RegistryEntry> matches = CollectMatches(entries, category, groupKey, true);
            if (matches.Count == 0)
            {
                matches = CollectMatches(entries, category, groupKey, false);
            }

            if (matches.Count == 0)
            {
                return string.Empty;
            }

            if (category == DesignerCategory.Floor && TryResolveNumberedFloor(room, matches, cell, out string floorGuid))
            {
                return floorGuid;
            }

            int index = PositiveModulo(SpatialHash(cell, groupKey), matches.Count);
            for (int i = 0; i < matches.Count; i++)
            {
                RegistryEntry candidate = matches[(index + i) % matches.Count];
                if (candidate != null && !string.IsNullOrEmpty(candidate.guid))
                {
                    return candidate.guid;
                }
            }

            return string.Empty;
        }

        private static bool TryResolveNumberedFloor(RoomData room, List<RegistryEntry> matches, Vector3Int cell, out string guid)
        {
            guid = string.Empty;
            int key = FloorWangResolver.ResolveFloorTile(cell, candidate => ContainsCell(room != null ? room.floorCells : null, candidate));
            RegistryEntry matched = FindNumberedEntry(matches, key);
            if (matched != null && !string.IsNullOrEmpty(matched.guid))
            {
                guid = matched.guid;
                return true;
            }

            return false;
        }

        private static RegistryEntry FindNumberedEntry(List<RegistryEntry> matches, int number)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                RegistryEntry entry = matches[i];
                if (entry == null || string.IsNullOrEmpty(entry.displayName))
                {
                    continue;
                }

                if (TryReadTrailingNumber(entry.displayName, out int parsed) && parsed == number)
                {
                    return entry;
                }
            }

            return null;
        }

        private static List<RegistryEntry> CollectMatches(
            IReadOnlyList<RegistryEntry> entries,
            DesignerCategory category,
            string groupKey,
            bool requireTileForTileLayer)
        {
            List<RegistryEntry> matches = new List<RegistryEntry>();
            if (entries == null)
            {
                return matches;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                RegistryEntry entry = entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.guid))
                {
                    continue;
                }

                if (requireTileForTileLayer && DesignerCategoryMap.IsTileCategory(category) && entry.tile == null)
                {
                    continue;
                }

                if (GroupKeyFor(entry, category) == groupKey)
                {
                    matches.Add(entry);
                }
            }

            return matches;
        }

        private static string ExtractGroupKey(string groupId)
        {
            if (!IsGroupId(groupId))
            {
                return string.Empty;
            }

            int slash = groupId.LastIndexOf('/');
            return slash >= 0 && slash < groupId.Length - 1 ? groupId.Substring(slash + 1) : string.Empty;
        }

        private static List<RoomData.TileCellRecord> RecordsFor(RoomData room, DesignerCategory category)
        {
            switch (category)
            {
                case DesignerCategory.Floor:
                    return room.floorCells;
                case DesignerCategory.Cliff:
                    return room.cliffCells;
                default:
                    return null;
            }
        }

        private static bool ContainsCell(List<RoomData.TileCellRecord> records, Vector3Int cell)
        {
            return IndexOf(records, cell) >= 0;
        }

        private static int IndexOf(List<RoomData.TileCellRecord> records, Vector3Int cell)
        {
            if (records == null)
            {
                return -1;
            }

            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].cell == cell)
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool TryReadTrailingNumber(string value, out int number)
        {
            number = 0;
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            int end = value.Length - 1;
            while (end >= 0 && char.IsDigit(value[end]))
            {
                end--;
            }

            if (end == value.Length - 1)
            {
                return false;
            }

            return int.TryParse(value.Substring(end + 1), out number);
        }

        private static int SpatialHash(Vector3Int cell, string salt)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + cell.x;
                hash = hash * 31 + cell.y;
                hash = hash * 31 + cell.z;
                hash = hash * 31 + StableStringHash(salt);
                hash ^= hash << 13;
                hash ^= hash >> 17;
                hash ^= hash << 5;
                return hash;
            }
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }

        private static int StableStringHash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            unchecked
            {
                int hash = 216613626;
                for (int i = 0; i < value.Length; i++)
                {
                    hash = (hash ^ value[i]) * 16777619;
                }

                return hash;
            }
        }
    }
}
