# TASK: Chamber proximity UX — Hades-style G-interact, NO screen swap

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
USER PLAYTEST BUG: In the Attunement Chamber (CharacterSelect scene, runtime-built by `Assets/Scripts/UI/ChamberSelectBootstrap.cs`), walking near an echo statue SWAPS THE WHOLE SCREEN — the classic CharacterSelectScreen canvas pops over the world. User wants Hades-weapon-rack behavior: world stays, only a small interact prompt appears near the statue; press G to attune. Unity Editor is OPEN (UnityMCP).

## Root cause (already diagnosed — verify then fix)
`ChamberSelectBootstrap.Update()`: when `FindNearestStation()` returns a station it calls `InvokeClassicSelect(nearest.classType)` AND `SetClassicOverlayVisible(true)` — the latter shows the full classic canvas. That's the "weird screen".

## Required behavior (design locked — implement exactly)
1. Proximity to a statue shows ONLY the existing world-space `promptLabel` (small, above/near statue):
   - Unlocked: `[G] Bürün — <CLASS NAME>` (class name visible so player knows who they're attuning to)
   - Can unlock: `[G] Kilidi Aç — <cost> SHATTERED ECHO`
   - Cannot unlock: existing condition text unchanged.
   NO full-screen canvas, NO classic overlay, NO camera change on proximity. Leaving range hides the prompt.
2. Interact key = **G** (matches RewardPickup G pattern). Replace the E key check. Update all prompt strings from [E] to [G].
3. KEEP `InvokeClassicSelect(nearest.classType)` if it only syncs selection state (check what it does) — but it must NOT make any canvas visible. If it shows UI as a side effect, sync the state another way (PlayerClassManager/currentClass only).
4. TAB fallback overlay behavior UNCHANGED (user can still open classic screen manually with TAB).
5. Small polish (cheap, do it): when attuned via G, keep the existing attune feedback; optionally a brief highlight on the statue. Do NOT build any new info panel/card — prompt only (user: "sadece G ile interact edebileceğim yer çıkmalı").
6. Door prompt: if door prompt also uses [E], switch to [G] for consistency (check `exitWorld` proximity block).

## Files
- `Assets/Scripts/UI/ChamberSelectBootstrap.cs` (main; surgical)
- Only touch other files if a string/key constant lives elsewhere.

## Verification (MANDATORY, play mode)
- MainMenu → BAŞLA → Chamber: walk-near simulation (move player transform next to a pedestal via execute_code) → assert classic canvas STAYS hidden (CanvasGroup alpha==0) AND promptLabel active with "[G]" text.
- Press-G simulation: if keyboard injection is unreliable headless, invoke the same code path the G-branch calls and assert attune succeeded (currentClass changed, PlayerClassManager.SelectedClass synced).
- TAB toggle still works (invoke its branch; canvas alpha goes 0→1→0).
- Locked statue near: prompt shows unlock text, no canvas.
- Console 0 errors. Do NOT save scenes. Stop play mode when done; leave editor on MainMenu scene.

## Commit
Changed files only. e.g. `fix(chamber): proximity shows G-interact prompt only, no classic screen swap`. Identity ydbilgin, NO Co-Authored-By.

## Output
CODEX_DONE: what InvokeClassicSelect did, evidence per verification item, commit hash.
