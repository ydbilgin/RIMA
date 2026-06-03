# Review: A5 draft chain-UI chip — CX

ACTIVE RULES: (1) think before judging (2) real issues only, file:line + fix (3) reviewer not writer (4) BLOCKED if can't read.

NLM ACCESS: optional.

## Context
Draft offer cards now show a cyan "⟂ pairs with {Skill}" chip when the offered skill chains with another offered card or an owned active skill. Detection uses the static `ChainWindowTracker.ChainsWith` API (bidirectional). Makes the Sundered-Beat chains legible at draft time.

## Files
- `Assets/Scripts/UI/SkillOfferUI.cs` (chip detection `FindChainPartner` + render `BuildChainChip`, snapshot `currentOfferNames`)
- `Assets/Scripts/Skills/DraftManager.cs` (new `OwnedActiveSkillNames` accessor — fresh snapshot)

## Review focus — PASS/FAIL + file:line
1. **Detection correctness:** `FindChainPartner` builds context = other offered skillNames (currentOfferNames) + owned actives, and matches via bidirectional `Chains(a,b)=ChainsWith(a,b)||ChainsWith(b,a)`. Confirm it fires for BOTH producer and consumer side, and only for real pairs in the static table (no false positives).
2. **Null / non-skill offers:** gold/heal offers (null `skill`) must get NO chip (no NPE). Confirm the chip is only built in normal-draft `BuildRewardCard`, not replace-mode `BuildSkillCard` or gold/heal.
3. **Snapshot hygiene:** `currentOfferNames` is rebuilt in `Show` and cleared in `ClearCards()` — confirm no stale snapshot across drafts, and built BEFORE cards so each card sees its siblings.
4. **Chip doesn't block input:** chip Image `raycastTarget=false` so the SEC/pick button still receives clicks. Confirm placement doesn't overlap-block the button raycast.
5. **OwnedActiveSkillNames:** returns a FRESH snapshot (not the live mutable list) so the UI can't mutate draft state. Confirm.
6. **Theme:** cyan via RimaUITheme (on-brand seal/synergy in UI — distinct from red/orange enemy break tells). Confirm no hardcoded off-brand color.
7. No regression to the existing draft pick flow; no per-frame alloc in card build (one-time build is fine).

## Output
`STATUS: PASS`/`FAIL` + findings. Tight.
