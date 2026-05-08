using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    using RoomType = global::RIMA.RoomType;

    public class RoomLoader : MonoBehaviour
    {
        public static event Action<RoomConfig, GameObject> OnRoomLoaded;
        public static event Action OnRoomCleared;

        [SerializeField] private RoomRegistry registry;
        [SerializeField] private Grid baseGrid;

        private GameObject _currentInstance;

        public void Load(RoomType type, int depth)
        {
            if (_currentInstance != null) Unload();

            GameObject prefab = registry.GetRandom(type, depth);
            if (prefab == null) { Debug.LogError($"[RoomLoader] No prefab for {type} depth {depth}"); return; }

            _currentInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            RoomConfig config = _currentInstance.GetComponent<RoomConfig>();
            if (config == null) { Debug.LogError("[RoomLoader] Prefab missing RoomConfig"); Destroy(_currentInstance); return; }

            ValidateContract(config);
            OnRoomLoaded?.Invoke(config, _currentInstance);
        }

        public void Unload()
        {
            if (_currentInstance == null) return;
            Destroy(_currentInstance);
            _currentInstance = null;
            OnRoomCleared?.Invoke();
        }

        private void ValidateContract(RoomConfig config)
        {
            if (baseGrid == null) return;
            if (config.cellSize != baseGrid.cellSize)
                Debug.LogWarning($"[RoomLoader] cellSize mismatch: {config.cellSize} vs {baseGrid.cellSize}");
            if (config.gridLayout != baseGrid.cellLayout)
                Debug.LogWarning($"[RoomLoader] gridLayout mismatch: {config.gridLayout} vs {baseGrid.cellLayout}");
        }
    }
}
