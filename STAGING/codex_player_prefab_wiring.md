# Player Prefab + HandAnchorAttach Wiring — execute every step, commit at end

## Context

WeaponDatabaseSO + HandAnchorAttach (Karar #123 Yol A Level 1) are compiled. No Player prefab exists yet
that wires HandAnchorAttach → WeaponDatabase.asset. CharacterSelectScreen references "_IsoGame" scene
which was deleted; it needs updating to reference the correct scene.

## Key files (DO NOT read others)

- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Player/PlayerStats.cs`
- `Assets/Scripts/UI/CharacterSelectScreen.cs`
- `Assets/Scripts/Combat/CombatHandler.cs`
- `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs`
- `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs`

## STEP 1 — Read key files

Read all files listed above.

## STEP 2 — Fix CharacterSelectScreen scene reference

In `CharacterSelectScreen.cs`, change:
```csharp
[SerializeField] private string gameSceneName = "_IsoGame";
```
to:
```csharp
[SerializeField] private string gameSceneName = "RoomPipelineTest";
```
(The only scene in Build Settings is now `Demo/RoomPipelineTest.unity`.)

## STEP 3 — Create Player prefab script (PlayerPrefabSetup)

Create `Assets/Scripts/Player/PlayerPrefabSetup.cs`:

```csharp
using UnityEngine;
using RIMA.Systems.Map; // WeaponDatabaseSO is in global namespace but import for clarity

/// <summary>
/// Editor utility: validates Player prefab required components.
/// Karar #123 Yol A Level 1 — HandAnchorAttach must be on the Player root.
/// </summary>
public class PlayerPrefabSetup : MonoBehaviour
{
    [ContextMenu("Validate Player Setup")]
    private void Validate()
    {
        var ha = GetComponent<HandAnchorAttach>();
        if (ha == null) Debug.LogError("PlayerPrefabSetup: HandAnchorAttach missing on Player root.");
        else Debug.Log("PlayerPrefabSetup: HandAnchorAttach OK.");

        var ch = GetComponent<CombatHandler>();
        if (ch == null) Debug.LogWarning("PlayerPrefabSetup: CombatHandler missing.");
        else Debug.Log("PlayerPrefabSetup: CombatHandler OK.");

        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null) Debug.LogWarning("PlayerPrefabSetup: No SpriteRenderer found in children.");
        else Debug.Log($"PlayerPrefabSetup: SpriteRenderer OK ({sr.gameObject.name}).");
    }
}
```

## STEP 4 — Create Player prefab via Unity code execution

Use `execute_code` (UnityMCP) or shell script to create the prefab. If UnityMCP not available, create via C# editor script.

Create `Assets/Editor/CreatePlayerPrefab.cs`:

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

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
```

## STEP 5 — Execute the prefab creation

Run `execute_menu_item` with "RIMA/Tools/Create Player Prefab" OR run the editor script via `execute_code`.

## STEP 6 — Compile check

`read_console` — 0 errors required. Fix any compiler errors.

## STEP 7 — Commit

```bash
git add Assets/Scripts/Player/PlayerPrefabSetup.cs Assets/Scripts/UI/CharacterSelectScreen.cs Assets/Editor/CreatePlayerPrefab.cs Assets/Prefabs/Player.prefab Assets/Prefabs/Player.prefab.meta
git commit -m "[player-prefab] Player.prefab + HandAnchorAttach wiring + CharacterSelectScreen fix

- Player.prefab: Body SR + HandAnchor child + WeaponSprite child + Rigidbody2D + CombatHandler + HandAnchorAttach
- HandAnchorAttach: wired to WeaponDatabase.asset (Resources) + HandAnchor transform (Karar #123 Yol A L1)
- CharacterSelectScreen: scene ref _IsoGame → RoomPipelineTest
- PlayerPrefabSetup: ContextMenu Validate helper"
```

## STEP 8 — Report

Write `STAGING/player_prefab_wiring_report.md`:
```
# Player Prefab Wiring Report

## Player.prefab
[created Y/N, path]

## HandAnchorAttach
[weaponDatabase assigned Y/N, handAnchor transform assigned Y/N]

## CharacterSelectScreen
[scene ref fixed Y/N]

## Compile
[0 errors Y/N]
```

Append `CODEX_DONE_laurethgame.md`:
```
## [2026-05-14] Player Prefab + HandAnchorAttach Wiring
- Player.prefab: Y/N
- HandAnchorAttach wired: Y/N
- CharacterSelectScreen fix: Y/N
- Compile: Y/N
- Commit: [hash]
```

## Constraints

- Player.prefab: Karar #123 Yol A Level 1 — HandAnchorAttach MUST be on root
- weaponPrefab in WeaponDatabase.asset is null (placeholder) — HandAnchorAttach.AttachWeapon returns early when null, NO error
- DO NOT add movement/input logic — PlayerController.cs already exists for that
- NAMESPACE: HandAnchorAttach and WeaponDatabaseSO are in RIMA namespace

## Source References

1. `Assets/Scripts/Player/PlayerController.cs` — existing player script
2. `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` — `weaponDatabase`, `classId`, `handAnchor` SerializeField names
3. `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs` — in RIMA namespace
4. `Assets/Scripts/UI/CharacterSelectScreen.cs` — gameSceneName field to fix
5. `Assets/Scripts/Combat/CombatHandler.cs` — in global namespace
