# InputBufferService Fix — S66

Execute every step below completely.

## Context
File: Assets/Scripts/Combat/InputBufferService.cs
Both InputBufferService and PlayerAttack are in the RIMA namespace — no extra using needed.
The class may already have a `private PlayerAttack attack` field. If it does, verify it is populated in Awake via GetComponent<PlayerAttack>(). If not, add it.

## Steps

### 1. Inspect InputBufferService.cs
Read the file and check:
- Is there a `private PlayerAttack attack` field?
- Is it populated in Awake with GetComponent<PlayerAttack>()?
- Is there any compile-breaking namespace or type issue?

### 2. Fix
If the field is missing or not populated in Awake, add:
```csharp
private PlayerAttack _attack;
// In Awake:
_attack = GetComponent<PlayerAttack>();
```
If it already exists and is correctly populated, leave it alone.

### 3. Compile check
Use read_console to confirm no compile errors after the change.

### 4. Commit
If any changes were made, commit: `fix(combat): InputBufferService PlayerAttack GetComponent in Awake`
If no changes needed, report "no change required".
