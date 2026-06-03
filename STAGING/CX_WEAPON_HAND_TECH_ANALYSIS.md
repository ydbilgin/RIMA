# Task: Code-grounded analysis — weapon-to-hand mount for characters WITH visible hands (RIMA, 2D top-down 8-dir)

ACTIVE RULES: (1) think before answering (2) ground EVERY claim in the actual code you read (cite file:line) (3) surgical — analysis only, write NO code, change NO files (4) if a claim can't be verified from code, say so; BLOCKED if files missing.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
  (NLM may currently be auth-broken — if it errors, proceed from code + this file only, don't block.)
Direct-read allowed: the .cs files below, CURRENT_STATUS.md, .claude/PROJECT_RULES.md, this file.

## The question
A reference video (being analyzed separately by Gemini) shows a character with a weapon attached **directly to the hand**. RIMA's user wants to know: since **our characters will have actual VISIBLE hands drawn in the sprite**, does a separately-mounted weapon sprite line up — across all 8 directions AND across animation frames (idle bob, walk cycle, attack) — or does it visibly detach from the drawn hand? And what's the right architecture.

This is a TECH/feasibility analysis. Gemini covers the video; you cover OUR code. Opus will synthesize both.

## READ THESE FILES (they ARE the current weapon-mount system)
- `Assets/Scripts/Combat/OrientationSync.cs` — per-direction handOffsets[8] + weaponRotations[8] + flipY + procedural swing.
- `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` — has TWO modes: `Level1Static` (LIVE) and `Level2SpriteHandData`.
- `Assets/Scripts/Data/WeaponDatabaseSO.cs` (or wherever WeaponEntry/gripOffset/anchorOffset/twoHanded live) — find it.
- The `SpriteHandData` type (per-sprite hand-pixel anchor) — find it; understand how hand pixels are authored/stored and matched to a sprite.

## Analyze (code-grounded, cite file:line)
1. **Level1Static** (the LIVE path): `OrientationSync` only adjusts handOffsets/weaponRotations per FACING DIRECTION (8 values), driven in `HandAnchorAttach.LateUpdate`. Confirm: does anything track the hand position per ANIMATION FRAME (walk-cycle hand bob, idle)? If not, state precisely why a weapon pinned to a static per-direction anchor will DETACH from a drawn hand that moves during a walk/idle animation.
2. **Level2SpriteHandData**: walk through `LateUpdate`'s Level2 branch + `SpritePixelToWorld` + `TryGetCurrentHandData`. Does this solve the per-frame problem (weapon snaps to the hand pixel of the *current* sprite)? What exactly must be authored per sprite (left/right hand pixel coords)? For 8 dirs × N animation frames × multiple weapon forms × 10 eventual classes — quantify the authoring burden and where the data comes from (is there an editor tool? the OnDrawGizmosSelected hints at manual authoring).
3. **Swing handling**: during `BeginSwing`, the weapon sprite rotates procedurally. With a VISIBLE drawn hand, does the rotating weapon stay believable, or does the drawn hand stay still while the weapon swings (mismatch)? Note the locked design intent (weapon HIDDEN during swing, replaced by painterly slash VFX) — does the current code hide the weapon renderer during the swing, or does it keep showing the rotating sprite? Cite the code.
4. **Sort order**: `UpdateWeaponSortOrder` flips weapon behind body for N/NE/NW. With visible hands, is per-direction sort enough, or do some frames need the weapon between fingers (z-fighting with the drawn hand)?
5. **Verdict + options** (rank, with code-cost estimate each):
   - (a) keep weaponless-body + HandAnchor (no visible hands) — contradicts the user's "characters will have hands".
   - (b) Level2 per-frame SpriteHandData (visible hands + pinned weapon) — what tooling/data pipeline is the blocker.
   - (c) bake the weapon INTO each per-direction/per-frame character sprite (no separate weapon object) — implications for weapon-swapping/draft.
   - (d) hybrid: hands drawn, weapon hidden during swing → painterly VFX (only the static held pose needs alignment).
   Which is most production-viable for a 10-min demo (1 class, Warblade) vs the full 10-class vision?

## Deliverable (inline → CODEX_DONE.md)
Tight, code-cited. Per-item findings (1-5), then a ranked verdict with the single recommended path + the concrete next code/tooling step it would require. No code written.
