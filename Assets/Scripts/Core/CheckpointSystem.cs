using System;
using System.IO;
using UnityEngine;

namespace RIMA
{
    [Serializable]
    public class PlayerState
    {
        public int playerHp;
        public string[] inventory = Array.Empty<string>();
        public int currency;
    }

    [Serializable]
    public class CheckpointData
    {
        public string roomId;
        public int playerHp;
        public string[] inventory = Array.Empty<string>();
        public int currency;
        public string manifestId;
        public string[] manifestRoomIds = Array.Empty<string>();
    }

    public static class CheckpointSystem
    {
        private const string CheckpointFileName = "checkpoint_act1.json";

        public static string CheckpointPath => Path.Combine(Application.persistentDataPath, CheckpointFileName);

        public static void SaveCheckpoint(string roomId, PlayerState state)
        {
            SaveCheckpoint(roomId, state, null);
        }

        public static void SaveCheckpoint(string roomId, PlayerState state, RIMA.Map.MapManifestSO manifest)
        {
            if (state == null) state = new PlayerState();

            CheckpointData data = new CheckpointData
            {
                roomId = roomId,
                playerHp = state.playerHp,
                inventory = state.inventory ?? Array.Empty<string>(),
                currency = state.currency,
                manifestId = manifest != null ? manifest.manifestId : string.Empty,
                manifestRoomIds = GetManifestRoomIds(manifest)
            };

            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(CheckpointPath, json);
                Debug.Log("[CheckpointSystem] Saved checkpoint: " + CheckpointPath);
            }
            catch (IOException ex)
            {
                Debug.LogError("[CheckpointSystem] Checkpoint write failed: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError("[CheckpointSystem] Checkpoint write denied: " + ex.Message);
            }
        }

        public static bool LoadCheckpoint(out CheckpointData data)
        {
            data = null;

            try
            {
                if (!File.Exists(CheckpointPath)) return false;

                string json = File.ReadAllText(CheckpointPath);
                if (string.IsNullOrWhiteSpace(json)) return false;

                data = JsonUtility.FromJson<CheckpointData>(json);
                return data != null && !string.IsNullOrEmpty(data.roomId);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
            {
                Debug.LogError("[CheckpointSystem] Checkpoint load failed: " + ex.Message);
                return false;
            }
        }

        public static PlayerState CapturePlayerState(Transform playerTransform)
        {
            PlayerState state = new PlayerState();

            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }

            if (playerTransform != null)
            {
                Health health = playerTransform.GetComponent<Health>();
                if (health != null) state.playerHp = health.CurrentHP;
            }

            if (PlayerEconomy.Instance != null)
                state.currency = PlayerEconomy.Instance.Gold;

            return state;
        }

        private static string[] GetManifestRoomIds(RIMA.Map.MapManifestSO manifest)
        {
            if (manifest == null || manifest.rooms == null) return Array.Empty<string>();

            string[] ids = new string[manifest.rooms.Length];
            for (int i = 0; i < manifest.rooms.Length; i++)
                ids[i] = manifest.rooms[i] != null ? manifest.rooms[i].roomId : string.Empty;
            return ids;
        }
    }
}
