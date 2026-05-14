# WeaponDatabase Level 1 OrbitAttach — execute every step, commit at end

## Context

Animation Spec LOCKED §4.4 (STAGING/animation_system_spec_LOCKED.md): Karar #123 Yol A Level 1 OrbitAttach. WeaponDatabase ScriptableObject — Faz 1 entry: Warblade base greatsword only. Single static HandAnchor, weapon GameObject parented at spawn.

## STEP 1 — WeaponDatabaseSO

Create `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs`:

```csharp
[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "RIMA/Weapon Database")]
public class WeaponDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public class WeaponEntry
    {
        public string classId;          // e.g. "Warblade"
        public string formId;           // e.g. "Base", "T2_Rift" (Karar #124 Faz 2)
        public GameObject weaponPrefab; // instantiated at runtime, parented to HandAnchor
        public Vector3 anchorOffset;    // HandAnchor.localPosition (Level 1 static)
    }

    public WeaponEntry[] entries;

    public WeaponEntry GetWeapon(string classId, string formId = "Base")
    {
        foreach (var e in entries)
            if (e.classId == classId && e.formId == formId) return e;
        return null;
    }
}
```

## STEP 2 — WeaponDatabase asset

Create `Assets/Resources/WeaponDatabase.asset`:
- 1 entry: classId="Warblade", formId="Base", weaponPrefab=null (placeholder, sprite not yet generated), anchorOffset=(0.2f, 0.1f, 0f)

## STEP 3 — HandAnchor + OrbitAttach component

Create `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs`:

```csharp
/// <summary>
/// Level 1 OrbitAttach: static HandAnchor. Weapon parented here at spawn.
/// Karar #123 Yol A Level 1. Level 2 (per-frame AnimationCurve) = Faz 2.
/// </summary>
public class HandAnchorAttach : MonoBehaviour
{
    [SerializeField] private WeaponDatabaseSO weaponDatabase;
    [SerializeField] private string classId = "Warblade";
    [SerializeField] private Transform handAnchor; // assign in Inspector

    private GameObject _weaponInstance;

    private void Start()
    {
        AttachWeapon("Base");
    }

    public void AttachWeapon(string formId)
    {
        if (_weaponInstance != null) Destroy(_weaponInstance);
        var entry = weaponDatabase?.GetWeapon(classId, formId);
        if (entry?.weaponPrefab == null) return;

        _weaponInstance = Instantiate(entry.weaponPrefab, handAnchor);
        _weaponInstance.transform.localPosition = entry.anchorOffset;
        _weaponInstance.transform.localRotation = Quaternion.identity;
    }
}
```

## STEP 4 — Check existing player scripts

Read `Assets/Scripts/Player/` — does a player character script exist? If yes, check if HandAnchorAttach can be integrated or if it should be a standalone component.

## STEP 5 — Compile check

`read_console` — 0 errors.

## STEP 6 — Commit

```bash
git add Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs Assets/Scripts/Systems/Combat/HandAnchorAttach.cs Assets/Resources/WeaponDatabase.asset
git commit -m "[anim-spec] WeaponDatabase Level 1 OrbitAttach + HandAnchorAttach

- WeaponDatabaseSO: classId/formId/prefab/anchorOffset entries (Karar #123 Yol A)
- HandAnchorAttach: static HandAnchor parent spawn (Level 1 — Level 2 Faz 2)
- WeaponDatabase.asset: Warblade Base placeholder entry
- Karar #124 Faz 2 form variation wiring extension point (formId lookup ready)"
```

## STEP 7 — Report

Write `STAGING/weapon_database_level1_report.md`:
```
# WeaponDatabase Level 1 Report

## WeaponDatabaseSO
[created Y/N, GetWeapon method Y/N]

## HandAnchorAttach
[created Y/N, AttachWeapon Y/N]

## WeaponDatabase.asset
[1 Warblade Base entry Y/N]

## Compile
[0 errors Y/N]
```

Append `CODEX_DONE_yasinderyabilgin.md`:
```
## [2026-05-14] WeaponDatabase Level 1 OrbitAttach
- WeaponDatabaseSO: Y/N
- HandAnchorAttach: Y/N
- Compile: Y/N
- Commit: [hash]
```

## Constraints

- Level 1 ONLY: static HandAnchor.localPosition. DO NOT implement AnimationCurve per-frame anchor (Faz 2)
- Karar #124: formId "T2_Rift" lookup ready as a stub — no code needed beyond the ScriptableObject structure
- weaponPrefab null is OK for Faz 1 (placeholder) — weapon sprite not yet generated

## Source References

1. `Assets/Scripts/Player/` — player scripts
2. `STAGING/animation_system_spec_LOCKED.md` §4.4 — implementation spec
3. `C:\Users\ydbil\.ccs\instances\...\memory\project_yol_a_weapon_decouple.md` — Karar #123 Yol A Level 1 detail
