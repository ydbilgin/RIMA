using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Runs once per editor session on domain reload.
// Removes any RoomRoot (RoomConfig) scene object that was left in the scene at edit-time
// as an artifact of Task A authoring. Safe no-op after first cleanup.
[InitializeOnLoad]
public static class RemoveStrayRoomRoot
{
    private const string SessionKey = "RIMA_StrayRoomRootCleaned";

    static RemoveStrayRoomRoot()
    {
        if (SessionState.GetBool(SessionKey, false)) return;
        EditorApplication.delayCall += Cleanup;
    }

    private static void Cleanup()
    {
        SessionState.SetBool(SessionKey, true);

        var cfgs = Object.FindObjectsByType<RIMA.Systems.Map.RoomConfig>(FindObjectsSortMode.None);
        int removed = 0;
        foreach (var cfg in cfgs)
        {
            var go = cfg.gameObject;
            if (go.name == "RoomRoot" && go.scene.isLoaded && !PrefabUtility.IsPartOfPrefabInstance(go))
            {
                Debug.Log($"[RemoveStrayRoomRoot] Removing edit-time artifact '{go.name}' from scene.");
                Object.DestroyImmediate(go);
                removed++;
            }
        }

        if (removed > 0)
        {
            EditorSceneManager.SaveOpenScenes();
            Debug.Log($"[RemoveStrayRoomRoot] Done — {removed} object(s) removed, scene saved.");
        }
    }
}
