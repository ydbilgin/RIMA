# Task: Code-grounded — swing-time weapon visibility (full-hide vs keep-visible-reduced) in OUR system

ACTIVE RULES: (1) think before answering (2) ground claims in code you read (cite file:line) (3) analysis only — write NO code, change NO files (4) BLOCKED if files missing.
NLM ACCESS: optional NLM query may be auth-broken; if it errors, proceed from code + this file. Direct-read: the .cs below + this file.
RESPOND INLINE -> CODEX_DONE.md. This is a Codex (cx) task; an agy web-research task runs in parallel — do not duplicate it.

## Decision to settle
During a melee swing, the user does NOT want to fully REMOVE the weapon sprite (feels illogical). They prefer: weapon stays locked to the hand, and the swing's harshness is masked by REDUCING the weapon's visibility (alpha/blend/smear) + a slash VFX trail — more natural. Evaluate, in OUR code, three options and recommend:
- (H) full hide: `weaponRenderer.enabled=false` during the swing → slash VFX only.
- (V) keep visible: weapon procedurally rotates through the arc (current `OrientationSync.BeginSwing`) + slash VFX overlaid.
- (R) keep visible but REDUCE opacity (e.g. fade weapon `SpriteRenderer.color.a` to ~0.3-0.5 during the swing window) + slash VFX. The user's preferred middle path.

## READ
- `Assets/Scripts/Combat/OrientationSync.cs` — `BeginSwing`, `IsSwinging`, the swing arc in `Update()`.
- `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` — `HandleComboStep`, `weaponRenderer`, where the swing is triggered.
- `Assets/Scripts/VFX/SlashArcVFX.cs` — what it currently renders (LineRenderer? sprite flipbook?), how/when it's triggered, its lifetime vs the swing window.

## Analyze (code-cited)
1. What does `SlashArcVFX` actually draw today, and is it currently triggered on the swing? (The synthesis doc claims a "painterly slash-arc flipbook" is canon, but VFX_STRATEGY notes the code is a LineRenderer — confirm which it is.)
2. For option (R): where would you fade the weapon's `SpriteRenderer.color.a` during the swing? Is the swing window known (`_playerAttack.CurrentSwingWindow` / `OrientationSync.IsSwinging`)? Is a per-frame alpha lerp cheap and localized (no new system)? Cite the exact hook points.
3. For option (H): cost of toggling `weaponRenderer.enabled` around `IsSwinging` — trivial? Any teardown/edge cases (recompile-during-play, dropped swing)?
4. Pixel-art caveat: does procedurally rotating the weapon sprite at arbitrary angles (option V/R, `BeginSwing` uses float degrees) cause double-pixel/jaggy artifacts? Is option R (reduced opacity) enough to MASK that, or does it still read badly?
5. Which option is the smallest, lowest-risk change that gives a natural look for our top-down 8-dir pixel separate-weapon (hand-locked) setup? Give the concrete hook points (file:line) for the recommended one.

## Deliverable (inline -> CODEX_DONE.md)
Per-item findings (1-5, code-cited), then a one-line recommendation (H/V/R) with the exact hook points to implement it. No code written.
