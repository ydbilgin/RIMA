using System;
using System.IO;
using UnityEngine;

namespace RIMA.Save
{
    public class CheckpointManager : MonoBehaviour
    {
        private const string CheckpointFileName = "checkpoint.json";

        private static CheckpointManager instance;
        private static bool _shuttingDown;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
            _shuttingDown = false;
        }

        public static CheckpointManager Instance
        {
            get
            {
                if (_shuttingDown) return null;
                if (instance != null) return instance;

                instance = FindFirstObjectByType<CheckpointManager>();
                if (instance != null) return instance;

                GameObject go = new GameObject(nameof(CheckpointManager));
                instance = go.AddComponent<CheckpointManager>();
                DontDestroyOnLoad(go);
                return instance;
            }
        }

        public static string CheckpointPath => Path.Combine(Application.persistentDataPath, CheckpointFileName);

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                _shuttingDown = true;
            }
        }

        public void Save(CheckpointData data)
        {
            if (data == null) return;

            data.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            try
            {
                File.WriteAllText(CheckpointPath, JsonUtility.ToJson(data, true));
                Debug.Log("[CheckpointManager] Saved checkpoint: " + CheckpointPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Debug.LogError("[CheckpointManager] Checkpoint write failed: " + ex.Message);
            }
        }

        public CheckpointData Load()
        {
            try
            {
                if (!File.Exists(CheckpointPath)) return null;

                string json = File.ReadAllText(CheckpointPath);
                if (string.IsNullOrWhiteSpace(json)) return null;

                return JsonUtility.FromJson<CheckpointData>(json);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
            {
                Debug.LogError("[CheckpointManager] Checkpoint load failed: " + ex.Message);
                return null;
            }
        }

        public bool HasCheckpoint()
        {
            return File.Exists(CheckpointPath);
        }

        public void Clear()
        {
            try
            {
                if (File.Exists(CheckpointPath))
                    File.Delete(CheckpointPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Debug.LogError("[CheckpointManager] Checkpoint clear failed: " + ex.Message);
            }
        }
    }
}
