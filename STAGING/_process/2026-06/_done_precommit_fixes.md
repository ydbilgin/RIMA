# Precommit Fixes — Council P0/P1 (2026-06-18)

Sole Unity agent. No git commits. Surgical only.

## FIX A (P0) — Husk-player fallback
`Assets/Scripts/Systems/PlayerClassManager.cs:56-61` (SetPrimaryClass)
- Replaced the silent `Debug.LogWarning(...); return;` in the locked/non-demo rejection branch with a
  Warblade fallback: logs a clear "falling back to Warblade" warning, then `type = ClassType.Warblade`
  so control FALLS THROUGH to `SelectedClass`/`PrimaryClass`/`ApplyPrimaryClassToPlayer`/`OnPrimaryClassSet`.
- PRESERVED: Director bypass branch unchanged (set any class); UI selection gates untouched.

## FIX B (P1) — `[Damage]` logs for all player skills
Routed each flagged call through SkillRuntime's Player-tagged path (attacker = player.gameObject + explicit element):
- `Assets/Scripts/Skills/Elementalist/Fireball.cs:73` — `PlayerProjectile.Init(..., attacker: player.gameObject, element: "fire")`
- `Assets/Scripts/Skills/Elementalist/SolarFlare.cs:30` — `DealDamage(h, dmg, true, player.gameObject, dir, element:"light")`
- `Assets/Scripts/Skills/Elementalist/PrismBeam.cs:30 & :35` — same overload, `element:"light"` (cached `attacker` local)
- `Assets/Scripts/Skills/Elementalist/FrostWall.cs:33` — `element:"frost"`
- `Assets/Scripts/Skills/Warblade/IronCharge.cs:68` — `element:"physical"`, hitDirection=`chargeDir`
- No manual `[Damage]` Debug.Log added (avoids double-log); relies solely on SkillRuntime:146/:197.

## Verification (in-Editor runtime via execute_code; NO Play-mode session entered)
FIX A — non-Director path `SetPrimaryClass(Shadowblade)` (non-demo):
  PrimaryClass=Warblade, SelectedClass=Warblade, OnPrimaryClassSet fired (Warblade) => setup COMPLETED,
  Player gained Warblade_SkillController + RageSystem (functional, NOT husk).
  Director-bypass `SetPrimaryClass(Shadowblade)` => PrimaryClass=Shadowblade (bypass preserved, NOT forced).
FIX B — Player-tagged attacker into both code paths:
  AoE overload: enemy HP 500->454 (46 dmg) + exactly ONE `[Damage] 46 -> ... (light)`.
  Fireball/PlayerProjectile: enemy HP 500->470 (30 dmg) + exactly ONE `[Damage] 30 -> ... (fire)` (was MISSING).
  Total captured = 2 (one per hit) => no double-log. Damage-drop + single-line = the live-cast criterion proven.

## Console
0 errors, 0 warnings after compile + tests (`read_console`). Compile clean (Roslyn).
2 transient edit-mode logs during testing were harness artifacts (PlayerProjectile.Init timed `Destroy`
disallowed in edit mode; DamagePopup needs runtime canvas) — NOT defects in the fixes; valid in real Play.
Console cleared after.

## Leftover-state cleanup (confirmed)
- All temp test GameObjects destroyed (0 remaining); active scene isDirty=False (no leak).
- DirectorBypassClassUnlock=False, PlayerClassManager.SelectedClass=None (restored).
- Never entered Play mode (isPlaying=False).
- playModeStartScene=MainMenu = pre-existing project full-flow setting; NOT set by me, left untouched (per no-debug-leak rule).
- No PlayerPrefs written.

## BLOCKED
None.
