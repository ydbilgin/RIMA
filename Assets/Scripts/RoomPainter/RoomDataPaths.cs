using System.IO;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public static class RoomDataPaths
    {
        public static string JsonFor(string roomId)
        {
            string fileName = Sanitize(string.IsNullOrEmpty(roomId) ? "room" : roomId) + ".room.json";
#if UNITY_EDITOR
            return ("Assets/Data/Rooms/" + fileName).Replace('\\', '/');
#else
            return Path.Combine(Application.streamingAssetsPath, "Rooms", fileName);
#endif
        }

        // Canonical .asset path for a roomId. Shares the SAME sanitization as JsonFor so the
        // two surfaces (JSON sidecar + .asset) never diverge on file naming for odd ids.
        public static string AssetPathFor(string roomId)
        {
            return "Assets/Data/Rooms/" + Sanitize(string.IsNullOrEmpty(roomId) ? "room" : roomId) + ".asset";
        }

        private static string Sanitize(string value)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            string result = value;
            for (int i = 0; i < invalid.Length; i++)
            {
                result = result.Replace(invalid[i], '_');
            }

            return result.Replace(' ', '_');
        }
    }
}
