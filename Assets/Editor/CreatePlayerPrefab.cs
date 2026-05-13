using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using RIMA;

public static class CreatePlayerPrefab
{
    [MenuItem("RIMA/Tools/Create Player Prefab")]
    public static void Create()
    {
        // Root
        var root = new GameObject("Player");
        root.tag = "Player";
        root.layer = LayerMask.NameToLayer("Default");

        // Rigidbody2D
        var rb = root.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Collider
        var col = root.AddComponent<CapsuleCollider2D>();
        col.size = new Vector2(0.5f, 0.5f);
        col.direction = CapsuleDirection2D.Vertical;

        // CombatHandler
        root.AddComponent<CombatHandler>();

        // HandAnchorAttach
        var haa = root.AddComponent<HandAnchorAttach>();

        // PlayerPrefabSetup
        root.AddComponent<PlayerPrefabSetup>();

        // Body SpriteRenderer child
        var body = new GameObject("Body");
        body.transform.SetParent(root.transform, false);
        var sr = body.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 0;

        // HandAnchor child
        var anchor = new GameObject("HandAnchor");
        anchor.transform.SetParent(root.transform, false);
        anchor.transform.localPosition = new Vector3(0.2f, 0.1f, 0f);

        // WeaponSprite child under HandAnchor
        var weaponSprite = new GameObject("WeaponSprite");
        weaponSprite.transform.SetParent(anchor.transform, false);
        var wsr = weaponSprite.AddComponent<SpriteRenderer>();
        wsr.sortingOrder = 1;

        // Wire HandAnchor field via SerializedObject
        var so = new SerializedObject(haa);
        so.FindProperty("handAnchor").objectReferenceValue = anchor.transform;
        so.ApplyModifiedProperties();

        // Try to load WeaponDatabase from Resources
        var db = Resources.Load<WeaponDatabaseSO>("WeaponDatabase");
        if (db != null)
        {
            var so2 = new SerializedObject(haa);
            so2.FindProperty("weaponDatabase").objectReferenceValue = db;
            so2.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogWarning("CreatePlayerPrefab: WeaponDatabase.asset not found in Resources/. Assign manually.");
        }

        // Save prefab
        string dir = "Assets/Prefabs";
        if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
        AssetDatabase.Refresh();
        string path = $"{dir}/Player.prefab";
        PrefabUtility.SaveAsPrefabAsset(root, path);
        Object.DestroyImmediate(root);
        Debug.Log($"CreatePlayerPrefab: Saved {path}");
        AssetDatabase.Refresh();
    }
}
