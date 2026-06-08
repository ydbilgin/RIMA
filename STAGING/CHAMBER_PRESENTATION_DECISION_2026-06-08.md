# CHAMBER PRESENTATION — DECISION (2026-06-08)

Council: cx (feasibility/reuse) + ax-3.1-Pro (deep design) + ax-3.5-Flash (lean) → Opus synthesis.
Process files: `STAGING/_process/2026-06/_council_{cx,q_31pro,q_35flash}_chamber-presentation.md`.
Trigger: live playtest (4 screenshots) — chamber too dark, player too low on screen, world-space prompts, cramped dummy, no HUD, lock bypass.

## Q1 — CAMERA FRAMING — DECISION: FIXED chamber framing, player at ~60% from top
- Chamber is a **presentation/select room**, not exploration → camera is **FIXED-framed**, not exploration-follow (cx + 3.5-Flash; 3.1-Pro agreed fixed is acceptable).
- **Keep room visible** (it's a select room — all 10 figures must be seeable). Do NOT use the pixel-perfect 1:1 ortho (2.8125) the ax pair computed — that's hero-zoom, wrong for a select room. Keep the existing fit-to-room logic (multiplier 1.04, padding 0.35) so the 28-wide diamond fits.
- **Root cause of "player too low":** `ChamberSelectBootstrap:1006` sets `followOffset = chamberBounds.center - player.position` → camera locks to geometric room center while the player spawns at the extreme front edge (`minY+2`, L481) → player sits at the very bottom with void above the diamond's top point.
- **Fix:** (a) move spawn OFF the bottom edge (front-center aisle, `minY+4`/`minY+5` ≈ (14,4-5)); (b) frame so the player at spawn sits at ~**60% from top** (lower third), with figures + portal filling the upper ~60%; (c) expose `[Range(0.45,0.72)] chamberPlayerScreenY = 0.60` SerializeField so the user can nudge live (anti-blind-tweak).
- Reuse `CameraFollow` targeting a fixed anchor (or disable follow for chamber). Do NOT rewrite `CameraFollow`.

## Q2 — INTERACTION PROMPT — DECISION: chamber-local bottom-center SCREEN prompt
- User explicitly wants the prompt at screen bottom-center (Hades-referenced). NOTE: real Hades actually anchors prompts in WORLD space slightly above the object (3.1-Pro) — but honor the user's explicit request for a fixed screen prompt.
- Build a **chamber-local `ScreenSpaceOverlay` canvas** (cx) — do NOT instantiate the combat `HUDController` (it drags HP/resource/minimap wiring). Reuse the layout pattern only.
- Anchor `(0.5,0.15)`, pivot `(0.5,0.5)`, size `260x36`, anchoredPos `(0,0)`. `[G]` key-cap styling (dark rounded box + white glyph), 0.15s alpha fade-in.
- Convert `ShowPrompt(pos, text)` to ignore `pos` and only set text/active (keeps call sites surgical). Remove the world-space TMP prompt (`CreatePromptLabel`/`CreateWorldText`).
- Multi-interactable priority logic = **over-engineering** for this room (3.5-Flash): last-entered-wins is enough.

## Q3 — LIGHTING — DECISION: mild global bump + 1 soft fill + brighter pedestals (cx middle path)
A single global bump is not enough (cx). Keep mood, add readability:
- Global Light2D: `0.92 → 1.10` (keep cool color `(0.78,0.86,1)`). Expose as SerializeField.
- **Add ONE soft fill Point Light2D** at aisle midpoint (spawn→portal): intensity `0.35`, outer radius `10`, color `(0.50,0.68,1,1)`. Expose intensity as SerializeField.
- Pedestal base: `0.42 → 0.70`, radius `2.2 → 3.0`.
- Pedestal occupied: `0.72 → 0.95`. Highlighted: `1.05 → 1.30`, radius `3.2 → 3.8`.
- Locked pedestal intensity floor ≥ `0.45` (locked figure must still read).
- Dummy: dedicated point light `0.8`/r`3.0`.
(Rejected: 3.1-Pro's drop-global-to-0.40 = too dark for the user's complaint; 3.5-Flash's flat-1.25 = kills mood.)

## Q4 — DUMMY + MAP + SPAWN + ECHO HUD
- **Dummy:** move from far-right-edge (`maxX-1`) to right-middle training pocket. Formula: `dummyX = Clamp(axisX + columnOffset + 2, minX+2, maxX-3)`, `dummyY = RoundToInt(Lerp(spawn.y, ExitCell.y, 0.42))`, then `PickNearestWalkableCell`. Keep a clear radius around it.
- **Map size:** PRIMARY = reposition within existing **28×20** (Clamp to maxX-3 already gives breathing room → avoid risky `.asset` regen). OPTIONAL (only if still cramped after repositioning): enlarge `Chamber_CharSelect.asset` to `32×22` preserving special-room type + portal meta.
- **Spawn:** `(14, minY+4..5)` front-center aisle, off the bottom edge.
- **Echo HUD:** chamber-local screen overlay label (NOT full HUDController — it's combat-only, absent in chamber = why no HUD shows). Top-left, anchor `(0,1)`, pivot `(0,1)`, anchoredPos `(12,-12)`, ~`140x20`, cyan-diamond icon + balance, update on change. Reuse `HUDController.BuildEchoDisplay` VISUAL pattern only.

## LOCK BUG — DECISION: centralize unlock policy; starter = {Warblade, Elementalist}
Root cause (cx): hardcoded always-unlocked allow-list `{Warblade, Elementalist, Ranger, Shadowblade}` duplicated in `ChamberSelectBootstrap.IsUnlocked` (1353-1359), `CharacterSelectScreen.IsUnlocked` (1244-1249), a 3rd separate policy in `CharacterSelectController.IsUnlocked` (156-160), AND `PlayerClassManager.SetPrimaryClass` (40-47) accepts ANY class with no validation.
- **Starter unlocked roster = {Warblade, Elementalist}** (matches demo plan "2-sınıf kilit"). Ranger, Shadowblade + the other 6 = LOCKED (cost Echo). ⚠️ FLAG: if user wants Warblade-only, it's a 1-line change.
- **Centralize:** one shared `ClassUnlockPolicy.IsUnlocked(cls)` (PlayerPrefs-aware) used by all 3 `IsUnlocked` sites.
- **Defense in depth:** `PlayerClassManager.SetPrimaryClass` rejects locked classes (log + no-op).
- Add a focused EditMode test: locked class cannot be attuned/selected.

## "ODALARA BAK" (secondary)
Screenshots are all the chamber. cx to REPORT the combat-scene global light value; if also dark (<1.0), note for a follow-up pass — not fixed here (out of screenshot scope).

## Anti-blind-tweak
Expose `chamberPlayerScreenY`, global light intensity, and fill-light intensity as Inspector SerializeFields so the USER converges framing/lighting live without recompiles (lesson: chamber "yakınsamıyor → kör-tweak").
