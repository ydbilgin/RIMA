# Review: WAVE-2 cross-class Echo (B1+B2+B3) — CX

ACTIVE RULES: (1) think before judging (2) real issues only, file:line + fix (3) reviewer not writer (4) BLOCKED if can't read.

NLM ACCESS: optional.

## Context
A transient "Sundered Echo" actor: pressing key C summons a black+cyan-rim silhouette of a guest class that performs the guest skill (ranged from afar / melee dash-in-strike) then puffs out. Reuses the guest SkillBase on the player via a new `ExecuteAt(origin, aim)` overload. Design = `STAGING/CROSS_CLASS_DESIGN_SPEC.md`.

## Files
- NEW `Assets/Scripts/CrossClass/CrossClassEcho.cs` (actor)
- NEW `Assets/Scripts/CrossClass/PlayerCrossClassBinding.cs` (binding, key C, guest-favor cooldown)
- `Assets/Scripts/CrossClass/CrossClassSkillData.cs` (B1 fields: guestSkillName, EchoArchetype, IsEcho)
- `Assets/Scripts/Skills/Base/SkillBase.cs` (B3 `ExecuteAt` + `SkillOrigin`/`SkillAim`/`SupportsEchoOrigin`)
- `Fireball.cs`, `Cleave.cs`, `Earthsplitter.cs`, `WarStomp.cs` (opted-in: read SkillOrigin/SkillAim)

## Review focus — PASS/FAIL + file:line
1. **B3 regression (the #1 risk):** `ExecuteAt` sets a transient origin/aim override, runs `Execute()`, clears in `finally`. Confirm NON-echo (normal) casts of Fireball/Cleave/Earthsplitter/WarStomp are UNCHANGED — `SkillOrigin`/`SkillAim` must default to `transform.position`/`player.FacingDirection` when no override is set. Any path where the override leaks to a normal cast?
2. **ExecuteAt re-entrancy / exceptions:** if Execute() throws, does `finally` restore state? Can two ExecuteAt overlap and clobber the override?
3. **Self-positioning exclusion:** Blink/IronCharge/BladeRush are NOT opted in (SupportsEchoOrigin=false) → fallback basic hit. Confirm they can't be invoked-at-origin (which would teleport/dash the player wrongly).
4. **B2 actor lifecycle:** silhouette never takes damage / never persists / despawns after lifetime; no leak; null-guards (Resources.Load sprite missing → fallback; no enemy near cursor for melee). Custom-Axis sort (layer "Entities", spriteSortPoint=Pivot, NO manual sortingOrder).
5. **B2 visual:** black body (~0.6α) + cyan #00FFCC rim (~0.45α) — confirm cyan rim used ONLY here as the echo identity (this is allowed; cyan=seal/echo energy). Enemy layer "Enemy" for any echo damage.
6. **B1 binding:** AddComponents guest SkillBase on player root (DraftManager pattern, so rage/resource resolve); guest-favor cooldown; key C doesn't collide with native bindings (check KeyBindManager reserved/used keys).
7. No new asmdef; reuse CombatEventBus/SkillRuntime; no per-frame alloc in the echo Update.

## Output
`STATUS: PASS`/`FAIL` + findings. Tight.
