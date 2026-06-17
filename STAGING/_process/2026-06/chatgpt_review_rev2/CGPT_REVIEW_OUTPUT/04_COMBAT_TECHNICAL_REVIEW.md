# 04 — Combat Technical Review

## 1. Detection-range fix

`Assets/Scripts/Enemies/BaseMobBehavior.cs`

The change from 8 to 12 and per-frame re-acquire is a valid partial fix for:

- spawn distance greater than old detection range
- player/enemy initialization order
- lost references after reload

It is not sufficient as the only combat activation mechanism.

### Remaining dependency

Both target discovery and multiple damage paths depend on the `Player` tag:

```csharp
GameObject.FindGameObjectWithTag("Player")
```

and:

```csharp
if (!h.CompareTag("Player")) continue;
```

`PlayerController.Awake()` sets the physics layer but does not guarantee the Player tag.

### Required action

- Verify the real runtime player root after the normal CharacterSelect → `_Arena` path.
- Fix the prefab/scene tag at source.
- Add a runtime validation with an explicit error/warning.
- Prefer an encounter/spawner-provided player reference or a shared resolver over repeated global tag searches.
- Keep tag fallback for demo resilience.

Do not assume that the untagged `DemoPlayer` scene object is the final runtime player. Inspect the actual instantiated object.

## 2. Boss target acquisition

`Assets/Scripts/Enemies/Boss/PenitentSovereign.cs`

The boss resolves Player once in `Start()`. Add the same re-acquire resilience used by mobs, or use the shared player resolver. Otherwise a spawn-order race can leave the boss loop permanently idle.

## 3. AttackTokenManager lifecycle

`Assets/Scripts/Combat/AttackTokenManager.cs`

Potential issue:

```csharp
private void OnDestroy()
{
    if (instance == this)
    {
        instance = null;
        _shuttingDown = true;
    }
}
```

`_shuttingDown` is also checked by `Instance`. If the manager is destroyed during a normal scene transition, future token requests may return null permanently. Verify scene lifetime.

Preferred minimal options:

- Set `_shuttingDown = true` only in `OnApplicationQuit`, or
- make the manager persistent with `DontDestroyOnLoad` and verify no duplicate instance.

Test after room/scene transition, not only in one arena session.

## 4. Penitent lethality

`Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs`

Current combo:

- 20
- 25
- 40
- total 85
- 100 default player HP

`AttackTokenManager` allows two melee token owners. This does not necessarily mean two Penitents, because the encounter asset currently shows `maxSimultaneous: 1` for enemyType 3. It does mean Penitent burst can overlap with another melee attacker.

`Assets/Resources/Encounters/Act1_Wave_Pilot.asset`:

- threat budget 10
- opening fraction 0.4
- opening budget 4
- Penitent threat cost 4

Therefore Penitent can plausibly fill the opening budget alone, depending on spawner selection semantics.

### Demo-safe recommendation

- Remove Penitent from the opening wave.
- Introduce it in wave 2/3.
- Reduce full-combo total to approximately 40–50 for the demo.
- Candidate values: `10 / 12 / 20` or similar.
- Increase initial telegraph to around 0.6–0.75 s.
- Add a distinct tell before the third hit.

Do not rebalance the entire game two days before the demo.

## 5. Telegraph reuse

Reuse `Assets/Scripts/Enemies/EnemyTelegraph.cs`. Do not create a second system.

Prioritize:

1. ChainExplosion delayed rings
2. Sovereign's Wrath danger + safe-zone distinction
3. Fracture Charge line
4. Holy Lash cone/arc
5. Fracture Strike
6. Shackle origin flash only if time remains

### Important correctness rule

Lock attack origin/direction/positions when the telegraph begins, and use the same snapshot for damage. Recalculating direction after windup can make damage differ from the warning.

### Safe-zone rule

Wrath safe zone must use a visually distinct signal. Two identical red rings do not communicate where to stand.

### VFX rule

Avoid duplicating CombatJuice and FlashImpact on every small hit. Reserve strong snap effects for major boss events.

## 6. Priority

No CombatJuice, outline, prop dressing, or screenshot work should outrank a failing end-to-end combat chain.
