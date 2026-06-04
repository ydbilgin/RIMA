using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.Systems.Map
{
    public sealed class MapFlowManager : MonoBehaviour
    {
        private const string ResourcePath = "Map/MapList_Act1";
        private const string EditorAssetPath = "Assets/Data/Map/MapList_Act1.asset";

        private static MapFlowManager instance;

        [SerializeField] private MapListSO mapList;
        [SerializeField] private int mapsPerRun = 3;

        private int currentIndex;
        private int mapsCleared;

        public static bool IsMapTransition { get; set; }
        // Lazily recover the singleton if the static ref was never set or got cleared
        // (e.g. a run launched without the normal MainMenu bootstrap). Consumers like
        // RoomClearVictoryTrigger rely on this to unlock exits / spawn rewards.
        public static MapFlowManager ActiveInstance =>
            instance != null ? instance : (instance = FindFirstObjectByType<MapFlowManager>());

        public static MapFlowManager Instance
        {
            get
            {
                EnsureInstance();
                return instance;
            }
        }

        public bool HasMapList => mapList != null && mapList.mapSceneNames != null && mapList.mapSceneNames.Count > 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            EnsureInstance();
        }

        private static void EnsureInstance()
        {
            if (instance != null) return;

            MapFlowManager existing = FindFirstObjectByType<MapFlowManager>();
            if (existing != null)
            {
                instance = existing;
                DontDestroyOnLoad(existing.gameObject);
                return;
            }

            GameObject go = new GameObject("MapFlowManager");
            instance = go.AddComponent<MapFlowManager>();
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMapListIfNeeded();
            ResetRun();
        }

        public void ResetRun()
        {
            LoadMapListIfNeeded();
            currentIndex = ResolveStartIndex();
            mapsCleared = 0;
            IsMapTransition = false;
        }

        public void GoToNextMap()
        {
            LoadMapListIfNeeded();
            if (!HasMapList)
            {
                Debug.LogWarning("[MapFlowManager] No MapListSO assigned; cannot transition.");
                IsMapTransition = false;
                return;
            }

            IsMapTransition = true;
            mapsCleared++;

            if (mapsCleared >= Mathf.Max(1, mapsPerRun))
            {
                IsMapTransition = false;
                RoomLoader.RaiseDemoComplete();
                return;
            }

            currentIndex = PickRandomNextIndex();
            string nextScene = mapList.mapSceneNames[currentIndex];
            if (string.IsNullOrWhiteSpace(nextScene))
            {
                Debug.LogWarning("[MapFlowManager] Next map scene name is empty.");
                IsMapTransition = false;
                return;
            }

            SceneManager.LoadScene(nextScene);
        }

        private void LoadMapListIfNeeded()
        {
            if (mapList != null) return;

            mapList = Resources.Load<MapListSO>(ResourcePath);

#if UNITY_EDITOR
            if (mapList == null)
                mapList = AssetDatabase.LoadAssetAtPath<MapListSO>(EditorAssetPath);
#endif
        }

        private int ResolveStartIndex()
        {
            if (!HasMapList) return 0;

            int index = mapList.mapSceneNames.IndexOf(mapList.startSceneName);
            return index >= 0 ? index : 0;
        }

        private int PickRandomNextIndex()
        {
            int count = mapList.mapSceneNames.Count;
            if (count <= 1) return 0;

            int next = currentIndex;
            for (int guard = 0; guard < 8 && next == currentIndex; guard++)
                next = Random.Range(0, count);

            if (next == currentIndex)
                next = (currentIndex + 1) % count;

            return next;
        }
    }
}
