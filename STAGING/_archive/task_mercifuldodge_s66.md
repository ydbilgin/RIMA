# MercifulDodge Implementation — S66

Execute every step below completely.

## Context
- File to modify: Assets/Scripts/Player/PlayerController.cs
- Health component fires OnDamageTaken as Action<int> (confirmed from BaseMobBehavior usage pattern)
- TryDash() already calls attack.TryCancelForDash() — MercifulDodge should bypass this check when active
- When bypassing, also set CommitTimer = 0f to clear the attack commit state

## Design
When player takes damage, a short grace window (0.18s) opens where dash ignores attack commit lock.
This allows dodge-cancel out of an attack when hit.

## Steps

### 1. Read PlayerController.cs
Inspect: TryDash() method, Update() method, Awake() method, existing fields.
Find where `attack.TryCancelForDash()` is called in TryDash().

### 2. Add fields
Add to PlayerController class:
```csharp
private bool _mercifulDodgeActive;
private float _mercifulDodgeExpiry;
```

### 3. Subscribe to Health.OnDamageTaken
In Awake (or Start), get the Health component and subscribe:
```csharp
var health = GetComponent<Health>();
if (health != null)
    health.OnDamageTaken += _ => { _mercifulDodgeActive = true; _mercifulDodgeExpiry = Time.time + 0.18f; };
```
(Use the correct Health event name — read the Health script if OnDamageTaken name differs.)

### 4. Modify TryDash()
At the top of TryDash(), before the attack cancel check:
```csharp
if (_mercifulDodgeActive)
{
    _mercifulDodgeActive = false;
    attack.CommitTimer = 0f;  // clear commit — adjust field name if different
}
```
This bypasses TryCancelForDash() entirely when merciful window is open.

### 5. Add cleanup in Update()
```csharp
if (_mercifulDodgeActive && Time.time > _mercifulDodgeExpiry)
    _mercifulDodgeActive = false;
```

### 6. Adapt to actual field/property names
Read PlayerController.cs and Health.cs to confirm exact field names (CommitTimer, OnDamageTaken, etc.) before writing. Do not assume — verify.

### 7. Compile check
Use read_console to confirm no errors.

### 8. Commit
`feat(combat): MercifulDodge — bypass attack commit lock on damage grace window`
