# DIAG — "mavi-arc #3" (blue arc) — P5 — 2026-06-14

DIAGNOSE ONLY. No code changed, no scene saved. Play entered + STOPPED clean.

## 1. Complaint hunt — what was reported?
No explicit written "blue arc is wrong" complaint exists in STAGING / git log / memory.
The term "blue SlashArc" appears only as confirmation-of-firing in two prior diags:
- `_diag_tooltip_skillflow_2026-06-13.md:20` — "We confirmed LMB fires -> blue SlashArcVFX."
- `_diag_runstart_2a_2026-06-14.md:29` — "confirmed it fires — blue SlashArc."
Backlog item (`CURRENT_STATUS.md:24`): "P5 mavi-arc(re-diag)" — orchestrator flagged the spec as unclear.
So: the original complaint text is NOT FOUND. What the re-diag surfaces instead is a concrete
architecture defect (below) that is almost certainly the thing the user reacted to.

## 2. Architecture — TWO arcs fire on EVERY Warblade swing
`MeleeChainBehavior.ExecuteCombo` (`:97-98`) and `ResolvePendingHit` (`:112-113`) BOTH call,
back-to-back, on the same swing/strike frame:
1. `owner.EmitSlashArc(facing, step)`  -> `PlayerAttack.EmitSlashArc` (`:295-298`) -> `SlashArcVFX.Emit`
2. `SkillVfx.MeleeArc(hitCenter, facing, VfxElement.Physical)`

These are two SEPARATE, visually different VFX overlapping in the same place:

| | SlashArcVFX (LineRenderer) | SkillVfx.MeleeArc (sprite) |
|---|---|---|
| Source | `SlashArcVFX.cs` + prefab `Resources/Prefabs/VFX/SlashArcVFX.prefab` | `SkillVfx.cs:115-127` + sprite `Resources/VFX/Skills/slash_arc_main.png` |
| Color | white core + **cold-blue rim** `(0.45,0.75,1.0)` | sprite tinted **ember-orange** `0xE89020` `(0.91,0.56,0.13)` |
| Sorting | layer VFX, order **5** | layer VFX, order **20** (draws ON TOP of the blue one) |
| Geometry/scale | radius ~1.1 / arc 140deg | sprite scale 0.81 |
| Duration | 0.13-0.20s (per step) | 0.18s |

Provenance: the sprite `MeleeArc` was ADDED on top of the pre-existing `EmitSlashArc` by VFX Faz3
wiring (commit `0a36b7ef` "wire Tier 1 skills ... [visual unverified]") — never visually QA'd, and
the older blue LineRenderer was never removed. Result = doubled-up arc with a blue/orange clash.

Extra wrinkle: the sprite asset `slash_arc_main.png` is intrinsically TEAL/CYAN. Tinting it
"Physical orange" multiplies onto a teal sprite -> muddy result; and in the chamber-spawned flow
the blue LineRenderer overlays it, so the player sees blue-over-muddy-orange.

## 3. In-game observation (Play, Warblade, _Arena via class-set + LoadScene shortcut)
playModeStartScene=MainMenu confirmed; full chamber walk is not MCP-keyboard-drivable, so used the
same documented shortcut as prior diags (set PlayerClassManager.SelectedClass=Warblade -> LoadScene
"_Arena"; this exercises the same spawn path). NOTE: that shortcut does NOT run
`ChamberSelectBootstrap.AssignSlashArcVFXToPlayer`, so `slashArcVFX` was NULL until I replicated the
bootstrap wiring (Instantiate `Prefabs/VFX/SlashArcVFX` as child + reflection-set the field).

Observed (execute_code data-proof):
- Blue LineRenderer arc: enabled, sortingLayer=VFX order=5, 25 pts, worldSpace, additive material
  (URP Particles/Unlit). Gradient = blue rim(0.45,0.75,1.0)->white core->blue rim. Centered/bulging
  correctly toward the passed direction (player(5.28,4.10) -> arc mid x=6.38 for East). Looks fine.
- Sprite arc: sprite=slash_arc_main, color=(0.91,0.56,0.13,~1.0) [orange], sortLayer=VFX order=20,
  scale 0.81. Rotation is correct 8-way: E=0, N=90, W=180, S=270, NE=45 deg; position offsets toward
  facing correctly. Looks fine on its own.
- Both render in front of the player (VFX layer). Each individually is facing-aligned and timed OK.

So neither arc is individually broken (color/pos/scale/rotation/sort/timing all correct). The defect
is that BOTH play together with mismatched color + redundant geometry.

## 4. Verdict
DEFECT IDENTIFIED (architecture, not a rendering bug): redundant **double arc** on every Warblade
basic swing — old blue `SlashArcVFX` (LineRenderer) + new orange-tinted `SkillVfx.MeleeArc` sprite
fire simultaneously, clashing in color and stacking geometry. Almost certainly the source of the
"mavi-arc" complaint, but the EXACT user intent (which one to keep, or recolor) is NOT in writing.

Change surface (for whoever fixes — DO NOT fix here):
- Keep-ONE decision needed. Two clean options:
  - (A) Keep the sprite `SkillVfx.MeleeArc`, drop the blue LineRenderer: remove the two
    `owner.EmitSlashArc(...)` calls in `MeleeChainBehavior.cs:97` and `:112` (PlayerAttack.EmitSlashArc
    + SlashArcVFX.cs + the bootstrap AssignSlashArcVFXToPlayer wiring then become dead).
  - (B) Keep the blue LineRenderer, drop the sprite: remove the two `SkillVfx.MeleeArc(...)` calls at
    `MeleeChainBehavior.cs:98` and `:113`.
- If "blue" is the desired look but via the sprite path: the sprite `slash_arc_main.png` is teal —
  retint `VfxElement.Physical` (or pass Frost) OR keep LineRenderer. (`SkillVfx.Palette` `:178-179`.)

SPEC STILL NEEDS A USER CALL: which arc to keep + desired color. Orchestrator should ask the user
"tek arc mi, hangi renk?" rather than guess.

## 5. State hygiene
Play STOPPED. activeScene=_Arena, isDirty=False (NOT saved). Console: 0 errors; only the known-benign
play-EXIT "Some objects were not cleaned up" teardown warning (already logged benign in
CURRENT_STATUS.md). Static `PlayerClassManager.SelectedClass=Warblade` residual (resets to None on next
real run-start; not persisted). No files modified.
