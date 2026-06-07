# A1 WeaponDatabase Clarify — S114 Track A Unblock

_Date: 2026-05-28 | Sonnet read-only audit_

---

## 1. CANONICAL WeaponDatabase

**CANONICAL: `Assets/Resources/WeaponDatabase.asset`**
**GUID: `4ff6c6f519482f54da6847ca1e91ed7a`**

Runtime proof (Inspector serialization — no `Resources.Load` code path):
- `Assets/Prefabs/Player.prefab` line 253: `weaponDatabase: {fileID: 11400000, guid: 4ff6c6f519482f54da6847ca1e91ed7a, type: 2}`
- `Assets/Scenes/Phase2_WeaponAttach_Test.unity` line 629: same guid reference

Both live assets point to `Resources/WeaponDatabase.asset` via the `HandAnchorAttach.weaponDatabase` [SerializeField] field.

**`HandAnchorAttach.cs`** (`Assets/Scripts/Systems/Combat/HandAnchorAttach.cs`) is the sole runtime consumer. It uses `weaponDatabase.GetWeapon(classId, formId)` — no `Resources.Load`, purely serialized field.

---

## 2. Orphan / Duplicate

**ORPHAN: `Assets/ScriptableObjects/Weapons/WeaponDatabaseSO.asset`**
**GUID: `e8f4406d55a44d3d82ba021c7838bc2a`**

Zero references to this GUID in any `.prefab`, `.unity`, or `.cs` file.

Content diff vs canonical:
- Both share the same `Warblade / Base` entry with identical `weaponPrefab` GUID (`837acdf62eb1be9459882c84c0353150`).
- `WeaponDatabaseSO.asset` has an extra `handOffsets[]` array (8 per-direction offsets) — these were later superseded by the `SpriteHandData` system in `HandAnchorAttach`.
- `WeaponDatabase.asset` (canonical) has the newer minimal structure without duplicate `handOffsets` (those now live in `WeaponEntry.handOffsets` in the SO class, but are not set in this asset).

**Verdict: `WeaponDatabaseSO.asset` can be deleted in BLOK A.** It is an orphan with no live references.

**Exception note**: `Assets/_archive~/pre_v2_editor/CreatePlayerPrefab.cs` calls `Resources.Load<WeaponDatabaseSO>("WeaponDatabase")` — this is archived editor code, not runtime, and the path points to the canonical asset anyway.

---

## 3. Canonical Warblade Prefab

Three Warblade prefabs exist:

| Path | Role | Notes |
|------|------|-------|
| `Assets/Prefabs/Characters/Warblade.prefab` | **Character prefab** (PrefabInstance variant of a base prefab guid `64f340ef3dbef474da6aa6b17fe976f4`) | Contains PlayerMovementController, Health, RageSystem, KnockbackReceiver, PlayerController, PlayerAttack as added components. No HandAnchorAttach. |
| `Assets/Prefabs/Player.prefab` | **Canonical runtime player prefab** | Contains HandAnchorAttach wired to `Resources/WeaponDatabase.asset`. This is the live player. |
| `Assets/Prefabs/Combat/Weapons/Warblade.prefab` | **Weapon visual prefab** (old-style) | Has `WeaponSorter` + `OrientationSync` components. No longer used — see Section 4. |
| `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` | **Weapon sprite prefab (canonical)** | fileID `3689238102083876011`, GUID `837acdf62eb1be9459882c84c0353150`. This is what both WeaponDatabase entries reference as `weaponPrefab`. Only a SpriteRenderer child, no scripts. |

**Canonical character prefab for A2 work: `Assets/Prefabs/Player.prefab`**
**Canonical weapon spawn prefab: `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab`**

---

## 4. OrientationSync / WeaponSorter — Runtime Status

**Updated 2026-06-08 audit status:** `OrientationSync` is NO LONGER dead in the canonical runtime player. `Assets/Prefabs/Player.prefab` has `OrientationSync`, and `HandAnchorAttach` calls it after spawning the canonical weapon. `WeaponSorter` remains old-prefab-only unless separately revalidated.

| Script | Path | CS references | Prefab/scene references |
|--------|------|--------------|------------------------|
| `OrientationSync` | `Assets/Scripts/Combat/OrientationSync.cs` | Called by `HandAnchorAttach` in canonical runtime | `Assets/Prefabs/Player.prefab` + old `Assets/Prefabs/Combat/Weapons/Warblade.prefab` |
| `WeaponSorter` | `Assets/Scripts/Combat\WeaponSorter.cs` | Self only | `Assets/Prefabs/Combat/Weapons/Warblade.prefab` only |

The old `Assets/Prefabs/Combat/Weapons/Warblade.prefab` is still an orphan (not referenced by Player.prefab or any scene). That prefab also has the `handOffsets` array inlined, suggesting it predates the `WeaponDatabaseSO`/`HandAnchorAttach` system.

**Recommendation for BLOK A:**
- Do NOT delete `OrientationSync.cs`; it is live in the canonical runtime player.
- `WeaponSorter.cs` and `Assets/Prefabs/Combat/Weapons/Warblade.prefab` can be treated as old-prefab cleanup candidates after revalidation.
- `WeaponSorter` duplicates sorting logic — can be deleted once A2 is wired.

---

## 5. A2 Mount Bridge Start Point

**File: `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs`**
**Field to wire: `weaponDatabase` ([SerializeField] WeaponDatabaseSO)**
**Asset to assign: `Assets/Resources/WeaponDatabase.asset` (already assigned in Player.prefab)**
**Prefab: `Assets/Prefabs/Player.prefab` — handAnchor is wired, `attachMode = Level1Static (0)`**

For A2 (direction-driven mount):
- `HandAnchorAttach.attachMode` needs to switch to `Level2SpriteHandData`
- OR wire `OrientationSync.Sync(FacingDir8)` as the per-frame update driver — called from `PlayerMovementController` or `PlayerController` on facing change
- `WeaponDatabaseSO.WeaponEntry.handOffsets[8]` is populated in `WeaponDatabaseSO.asset` but NOT in `WeaponDatabase.asset` (canonical). **BLOCKED: canonical asset's handOffsets[] array is empty/default** — A2 needs these filled before per-direction weapon positioning works.

---

## 6. BLOCKED Items — Inspector Verify Required

1. **`WeaponDatabase.asset` handOffsets empty**: The canonical `Resources/WeaponDatabase.asset` has no `handOffsets` array serialized (the field exists in `WeaponDatabaseSO.cs` as `public Vector2[] handOffsets = new Vector2[8]` but the asset YAML has no `handOffsets:` key). The orphan `WeaponDatabaseSO.asset` has 8 per-direction offsets filled. **Action required**: Copy those 8 values into the canonical asset in Inspector, OR confirm `HandAnchorAttach` in `Level1Static` mode doesn't use them (correct — Level1Static uses only `anchorOffset`).

2. **`Player.prefab` HandAnchorAttach.handAnchor**: Set to fileID `599438984922063064`. This is a child transform inside Player.prefab's base prefab. Needs Inspector confirm that HandAnchor child GO exists and is correct.

3. **`Player.prefab` HandAnchorAttach.bodyRenderer is null** (line 257: `bodyRenderer: {fileID: 0}`): Level2SpriteHandData mode requires bodyRenderer. Not needed for Level1, but A2 needs this assigned.

---

## 7. PRESERVED DATA — Warblade/Base offsets (extracted from orphan before delete, S114 0.4)

Orphan `WeaponDatabaseSO.asset` deleted 2026-05-28 (0 references confirmed). Its Warblade/Base entry held the only filled copy of these values — preserved here for A2 (canonical `Resources/WeaponDatabase.asset` lacks `handOffsets`). Git history also retains the asset if full recovery needed.

```
classId: Warblade  formId: Base
weaponPrefab: Warblade_Greatsword.prefab (guid 837acdf62eb1be9459882c84c0353150)
anchorOffset: (0.2, 0.1, 0)
gripOffset: (0, 0, 0)
twoHanded: true   orientBetweenHands: true   orientationOffsetDegrees: 0
handOffsets[8] (S, SE, E, NE, N, NW, W, SW order):
  S  ( 0.00, -0.08)
  SE ( 0.08, -0.04)
  E  ( 0.10,  0.00)
  NE ( 0.07,  0.05)
  N  ( 0.00,  0.08)
  NW (-0.07,  0.05)
  W  (-0.10,  0.00)
  SW (-0.08, -0.04)
```

**A2 note:** handOffsets may be superseded by `SpriteHandData` (Section 4) — A2 design decides whether to use these or SpriteHandData. Values preserved either way.
