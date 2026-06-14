# DONE — blue-arc removal + ember tint fix — P5 — 2026-06-14

VERDICT: DONE. Compile 0 errors. CHANGE A proven (one ember arc, blue LineRenderer never fires from
Warblade basic swing). CHANGE B applied (crescent base + additive ember); reads clean warm amber/ember,
NOT muddy — saturated punch would need a neutral/white base slash sprite (user art).

## Files changed
- `Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs` (157 -> 159 lines)
  - Removed the two `owner.EmitSlashArc(...)` blue-LineRenderer calls:
    - immediate path (was `:97`) inside `ExecuteCombo`
    - deferred strike-frame path (was `:112`) inside `ResolvePendingHit`
  - KEPT both `SkillVfx.MeleeArc(...)` ember-sprite calls (now `:99` and `:114`).
- `Assets/Scripts/VFX/SkillVfx.cs` (485 -> 512 lines)
  - `MeleeArc`: prefer `slash_arc_crescent` (more neutral/brighter base) over `slash_arc_main`;
    call `PlaySweep(..., additiveSprite: true)`.
  - `PlaySweep`: new optional `bool additiveSprite` param.
  - New helper `ApplyAdditiveSprite(instance, color)`: sets SpriteRenderer to the shared additive
    material and boosts the vertex color toward ember (R x1.6, G x1.15, B x0.6) so additive output
    glows orange instead of multiply-muddy teal-brown.

## NOT touched (per spec)
- `SlashArcVFX.cs`, the SlashArcVFX prefab(s), and `ChamberSelectBootstrap.AssignSlashArcVFXToPlayer`
  remain intact and LIVE.
- `MarkPulseBehavior.cs:71` (Ravager Brutal Swing) STILL calls `owner.EmitSlashArc(...)`. This is a
  DIFFERENT class's basic swing, not the Warblade swing the task scoped. So the blue LineRenderer is
  still used by Ravager and was correctly left alone. (If the user wants Ravager on the ember sprite
  too, that is a separate follow-up.)

## Verification (in-Editor, Play, _Arena, Warblade via class-set + LoadScene shortcut)
- Compile: editor isCompiling=False, read_console error filter = 0 entries; execute_code ran under
  Roslyn against project assemblies (would fail on any compile error).
- CHANGE A data-proof: wired SlashArcVFX as a Player child + set the `slashArcVFX` field (so the blue
  LineRenderer COULD fire), forced facing East, then drove a full swing via reflection:
    BASELINE                 MeleeArc count=0  | SlashArcLR enabled=False
    OnLMBInput(pressed)      MeleeArc count=0  | SlashArcLR enabled=False
    OnUpdate(dt=0.1) resolve MeleeArc count=1  | SlashArcLR enabled=False
  => exactly ONE ember arc sprite per swing; the wired blue LineRenderer stays disabled through the
  whole swing (proves the EmitSlashArc call is gone, not just a null-field no-op).
- attackStartup=0.08 (>0) so the swing uses the deferred ResolvePendingHit path (the one that fired
  the second blue arc) — exercised and confirmed clean.
- CHANGE B data-proof (fresh MeleeArc instance):
    sprite=slash_arc_crescent
    color RGBA=(1.46, 0.65, 0.08, 0.99)  (ember, R boosted past 1.0; B near 0)
    material=SkillVfx_SharedAdditive  shader=Legacy Shaders/Particles/Additive  (ADDITIVE blend)
    sortLayer=VFX order=20, scale 0.81, rotation 8-way correct (East=0)

## Screenshots (scene_view)
- F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_process/2026-06/bluearc_emberswing_close_2026-06-14.png
  (close framing — the arc reads as a warm amber/golden-ember glow; clearly NOT teal/blue/muddy.
  The white spear shape in the middle is the HitSpark prefab MeleeArc also spawns, not the arc.)
- F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_process/2026-06/bluearc_emberswing_2026-06-14.png
  (wide framing, less useful — gizmos + zoom.)

## CHANGE B readability verdict
The muddy multiply tint is fixed: additive blend over the brighter/lower-chroma crescent sprite makes
the arc glow warm amber/ember rather than greenish-brown. It is clean and readable. It leans
amber/gold rather than a punchy saturated ember-orange because additive over a near-white crescent
washes toward yellow-white and the floor shows through. For a fully saturated ember at #E89020 the
proper fix is a NEUTRAL/WHITE base slash sprite (user art) — both existing sprites are intrinsically
teal (main chroma 26.5, crescent 21.5; both have near-white cores). No new art invented.

## State hygiene
Play STOPPED. activeScene=_Arena, isDirty=False (NOT saved). Static PlayerClassManager.SelectedClass
reset to None (no debug leak). Temp VFX snapshot destroyed. Console: 0 errors; only the known-benign
play-EXIT "Some objects were not cleaned up" teardown warning (already logged benign in CURRENT_STATUS).
