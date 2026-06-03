# Review: WAVE-2 finish — B5 Echo draft-card + B4 puff VFX + review-fixes #2/#3 — CX

ACTIVE RULES: (1) think (2) real issues only, file:line + fix (3) reviewer not writer (4) BLOCKED if can't read.
NLM ACCESS: optional.

## Files
- NEW `Assets/Scripts/CrossClass/CrossClassCatalog.cs` (curated guest set in-code)
- NEW `Assets/Scripts/CrossClass/EchoPuffBurst.cs` (puff VFX)
- `Assets/Scripts/Systems/RewardOffer.cs` (CrossClassEcho type + FromEcho)
- `Assets/Scripts/Skills/DraftManager.cs` (MaybeInjectEchoOffer cadence + OnOfferSelected→Bind)
- `Assets/Scripts/UI/SkillOfferUI.cs` (ECHO card cyan treatment)
- `Assets/Scripts/Core/KeyBindManager.cs` (NEW GameAction CrossClassEcho default C)
- `Assets/Scripts/CrossClass/PlayerCrossClassBinding.cs` (reads GetBinding + rebuild)
- `Assets/Scripts/CrossClass/CrossClassEcho.cs` (RunGuarded cleanup)

## Review focus — PASS/FAIL + file:line
1. **B5 acquisition path:** picking the Echo offer calls `PlayerCrossClassBinding.Bind()` (resolves component on player root). Confirm Bind is actually reached, guest skill gets AddComponented, and the cross-class is then C-activatable. Cadence (rooms 3/6/9, cap 4, no re-offer of bound guest) + "replaces last offer" preserves the 3-card flow without dropping a needed offer.
2. **Curated catalog:** Fireball/Cleave/Earthsplitter registered in SkillDatabase + SupportsEchoOrigin=true (WarStomp excluded since unregistered). Confirm no Bind() abort for offered guests; demo "Echo of Elementalist→Fireball" works.
3. **Fix #2 keybind:** GameAction CrossClassEcho default C; PlayerCrossClassBinding reads via KeyBindManager.GetBinding + OnBindingsChanged rebuild + OnDisable unsubscribe (no leak); duplicate/reserved guard now covers C.
4. **Fix #3 cleanup:** RunGuarded pump guarantees PuffOut + Destroy(gameObject) even if guestSkill throws; no double-teardown; exception is logged (not silently swallowed in a way that hides bugs).
5. **B4 EchoPuffBurst:** self-destroys (no leak), pixelated-particle rules, black+cyan, in/out; VFX sort layer.
6. **No regression:** existing draft pick flow for skill/gold/heal offers intact; Bind()/ExecuteAt from B1-B3 untouched; no per-frame alloc.

## Output
`STATUS: PASS`/`FAIL` + findings. Tight.
