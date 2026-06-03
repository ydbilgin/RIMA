# Review: A4 ChainWindowTracker — CX

ACTIVE RULES: (1) think before judging (2) real issues only, file:line + fix (3) reviewer not writer (4) BLOCKED if can't read.

NLM ACCESS: optional.

## Context
A real BREAK->EXECUTE chain-window system replaced the crude `CooldownPercent > X` proxy used for skill->skill chain detection. Producers open named windows; consumers read (`IsOpen`) or `Consume` them. Static query API added for the draft chain-UI.

## Files
- `Assets/Scripts/Skills/ChainWindowTracker.cs` (NEW)
- Modified: `Warblade/IronCharge.cs` (opens IronChargeNextHit), `CripplingBlow.cs` (reads IronChargeNextHit IsOpen; opens CripplingExecute), `GravityCleave.cs` (reads IronChargeNextHit IsOpen), `DeathBlow.cs` (Consume CripplingExecute; opens SunderExecute), `SunderMark.cs` (Consume SunderExecute), `WarStomp.cs` (opens WarStompFollowup), `IroncladMomentum.cs` (Consume WarStompFollowup).

## Review focus — PASS/FAIL + file:line
1. **Behavior preservation (the #1 risk):** The old proxy was a READ-ONLY `CooldownPercent` check, so Iron Charge could be chained by BOTH CripplingBlow AND GravityCleave off one charge. Confirm both now use read-only `IsOpen(IronChargeNextHit)` (NOT Consume) so the shared behavior is preserved; and the 1:1 chains (DeathBlow/SunderMark/IroncladMomentum) use `Consume` so one producer cast = exactly one empowered follow-up. Verify no chain became impossible or always-on vs before.
2. **Producer timing:** windows that depended on a successful cast (e.g. DeathBlow opens SunderExecute only after a target/cast succeeds — matching the old proxy that became true only once the skill went on cooldown) are opened on the success path, not unconditionally. Confirm.
3. **Window durations** (DefaultWindow 1.5s; CripplingExecute 5s; SunderExecute 6s; WarStompFollowup 3s) ≈ the old `CooldownPercent>X` on the respective CDs — sanity-check they roughly match the prior feel (no chain made far easier/harder).
4. **Lazy-attach / scope:** `ChainWindowTracker.For(component)` lazily attaches to the player root (mirrors SkillRuntime.State). One per player. No duplicate, no leak. Windows expire correctly (timer/Update).
5. **Static query API** (`ChainsWith`, `ProducerFor`) — correct consumer→producer table, safe to call from UI/editor (no runtime state needed, no null deref).
6. **Untouched legit cooldown uses:** confirm `SkillBase.CooldownPercent` definition + `SkillBarUI` cooldown radial were NOT changed (only chain-detection sites).
7. No regression to existing readers; no per-frame alloc.

## Output
`STATUS: PASS`/`FAIL` + findings. Tight.
