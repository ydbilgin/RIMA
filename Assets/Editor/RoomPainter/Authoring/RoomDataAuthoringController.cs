using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RoomDataAuthoringController
    {
        public const string RoomsFolder = "Assets/Data/Rooms";
        public const string ThumbnailsFolder = "Assets/Data/Rooms/Thumbnails";
        public static string LiveRoomJsonPath =>
            Path.Combine(Application.streamingAssetsPath, "live", "room_current.json");

        private readonly List<RoomLibraryEntry> _libraryEntries = new List<RoomLibraryEntry>();

        public IReadOnlyList<RoomLibraryEntry> LibraryEntries
        {
            get { return _libraryEntries; }
        }

        public RoomData ActiveRoom { get; private set; }
        public string ActiveRoomPath { get; private set; }
        public bool IsDirty { get; private set; }

        public void RefreshLibrary()
        {
            EnsureAssetFolder(RoomsFolder);
            EnsureAssetFolder(ThumbnailsFolder);
            _libraryEntries.Clear();

            string[] guids = AssetDatabase.FindAssets("t:RoomData", new[] { RoomsFolder });
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                RoomData room = AssetDatabase.LoadAssetAtPath<RoomData>(path);
                if (room == null)
                {
                    continue;
                }

                room.EnsureDefaults();
                Texture2D thumbnail = string.IsNullOrEmpty(room.thumbnailPath)
                    ? null
                    : AssetDatabase.LoadAssetAtPath<Texture2D>(room.thumbnailPath);

                _libraryEntries.Add(new RoomLibraryEntry(path, room, thumbnail));
            }

            _libraryEntries.Sort((left, right) =>
                string.Compare(left.DisplayName, right.DisplayName, StringComparison.OrdinalIgnoreCase));
        }

        public RoomData CreateNewRoom()
        {
            EnsureAssetFolder(RoomsFolder);
            RoomData room = ScriptableObject.CreateInstance<RoomData>();
            room.name = "RoomData";
            room.displayName = "New Room";
            room.EnsureDefaults();

            string path = AssetDatabase.GenerateUniqueAssetPath(
                RoomsFolder + "/" + SanitizeFileName(room.displayName) + ".asset");
            AssetDatabase.CreateAsset(room, path);
            AssetDatabase.SaveAssets();

            OpenRoom(room, path);
            MarkDirty();
            RefreshLibrary();
            return room;
        }

        public RoomData DuplicateRoom(RoomData source)
        {
            if (source == null)
            {
                return null;
            }

            string sourcePath = AssetDatabase.GetAssetPath(source);
            if (string.IsNullOrEmpty(sourcePath))
            {
                return null;
            }

            string copyPath = AssetDatabase.GenerateUniqueAssetPath(
                RoomsFolder + "/" + SanitizeFileName(source.displayName) + "_Copy.asset");
            if (!AssetDatabase.CopyAsset(sourcePath, copyPath))
            {
                Debug.LogError("[RoomPainter] Failed to duplicate room asset: " + sourcePath);
                return null;
            }

            RoomData copy = AssetDatabase.LoadAssetAtPath<RoomData>(copyPath);
            if (copy == null)
            {
                return null;
            }

            copy.roomId = Guid.NewGuid().ToString("N");
            copy.displayName = source.displayName + " Copy";
            copy.thumbnailPath = string.Empty;
            copy.EnsureDefaults();
            EditorUtility.SetDirty(copy);
            AssetDatabase.SaveAssets();

            OpenRoom(copy, copyPath);
            MarkDirty();
            RefreshLibrary();
            return copy;
        }

        public void DeleteRoom(RoomData room)
        {
            if (room == null)
            {
                return;
            }

            string path = AssetDatabase.GetAssetPath(room);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!EditorUtility.DisplayDialog("Delete Room", "Delete " + room.displayName + "?", "Delete", "Cancel"))
            {
                return;
            }

            if (ActiveRoom == room)
            {
                ActiveRoom = null;
                ActiveRoomPath = string.Empty;
                IsDirty = false;
            }

            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            RefreshLibrary();
        }

        public void OpenRoom(RoomData room, string path = null)
        {
            if (room == null)
            {
                ActiveRoom = null;
                ActiveRoomPath = string.Empty;
                IsDirty = false;
                return;
            }

            room.EnsureDefaults();
            ActiveRoom = room;
            ActiveRoomPath = string.IsNullOrEmpty(path) ? AssetDatabase.GetAssetPath(room) : path;
            IsDirty = false;
        }

        public void MarkDirty()
        {
            if (ActiveRoom == null)
            {
                return;
            }

            ActiveRoom.EnsureDefaults();
            EditorUtility.SetDirty(ActiveRoom);
            IsDirty = true;
        }

        public void SaveActiveRoom(RoomDataComposer composer)
        {
            if (ActiveRoom == null)
            {
                return;
            }

            ActiveRoom.EnsureDefaults();
            if (composer != null)
            {
                composer.Compose(ActiveRoom);
                string thumbnailPath = RoomThumbnailBaker.Bake(ActiveRoom, composer.PreviewRoot);
                if (!string.IsNullOrEmpty(thumbnailPath))
                {
                    ActiveRoom.thumbnailPath = thumbnailPath;
                }
            }

            EditorUtility.SetDirty(ActiveRoom);
            AssetDatabase.SaveAssets();
            RoomDataJson.Write(ActiveRoom, RoomDataPaths.JsonFor(ActiveRoom.roomId));
            WriteLiveRoom(ActiveRoom);
            AssetDatabase.Refresh();
            IsDirty = false;
            RefreshLibrary();
        }

        public static void WriteLiveRoom(RoomData room)
        {
            if (room == null)
            {
                return;
            }

            RoomDataJson.Write(room, LiveRoomJsonPath);
        }

        public static void EnsureAssetFolder(string assetFolder)
        {
            if (AssetDatabase.IsValidFolder(assetFolder))
            {
                return;
            }

            string[] parts = assetFolder.Split('/');
            if (parts.Length == 0)
            {
                return;
            }

            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        private static string SanitizeFileName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "RoomData";
            }

            char[] invalid = System.IO.Path.GetInvalidFileNameChars();
            string result = value;
            for (int i = 0; i < invalid.Length; i++)
            {
                result = result.Replace(invalid[i], '_');
            }

            return result.Replace(' ', '_');
        }
    }

    public sealed class RoomLibraryEntry
    {
        public readonly string path;
        public readonly RoomData room;
        public readonly Texture2D thumbnail;

        public RoomLibraryEntry(string path, RoomData room, Texture2D thumbnail)
        {
            this.path = path;
            this.room = room;
            this.thumbnail = thumbnail;
        }

        public string DisplayName
        {
            get
            {
                if (room == null)
                {
                    return path;
                }

                return string.IsNullOrEmpty(room.displayName) ? room.name : room.displayName;
            }
        }
    }
}
