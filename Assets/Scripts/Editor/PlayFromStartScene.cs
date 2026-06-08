using UnityEditor;
using UnityEditor.SceneManagement;

// Editor Play her zaman ilk build sahnesinden (MainMenu) boot etsin —
// hangi sahne açık olursa olsun. Böylece _Arena/oda açıkken Play'e basınca
// doğrudan o odadan değil, gerçek akıştan (MainMenu -> CharSelect -> Chamber -> _Arena) başlar.
// Kapatmak için: menü "RIMA/Play From Main Menu" (tik kalkar -> açık sahneden başlar, hızlı oda iterasyonu).
[InitializeOnLoad]
public static class PlayFromStartScene
{
    const string Pref = "RIMA_PlayFromStartScene";
    const string MenuPath = "RIMA/Play From Main Menu";

    static PlayFromStartScene()
    {
        Apply();
    }

    static void Apply()
    {
        if (!EditorPrefs.GetBool(Pref, true))
        {
            EditorSceneManager.playModeStartScene = null;
            return;
        }

        foreach (var s in EditorBuildSettings.scenes)
        {
            if (!s.enabled) continue;
            var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path);
            if (asset != null) EditorSceneManager.playModeStartScene = asset;
            return;
        }
    }

    [MenuItem(MenuPath)]
    static void Toggle()
    {
        EditorPrefs.SetBool(Pref, !EditorPrefs.GetBool(Pref, true));
        Apply();
    }

    [MenuItem(MenuPath, true)]
    static bool ToggleValidate()
    {
        Menu.SetChecked(MenuPath, EditorPrefs.GetBool(Pref, true));
        return true;
    }
}
