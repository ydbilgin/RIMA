# P6 — Weapon polish + cleanup (DONE-with-flag) — 2026-06-14

## VERDICT: DONE-with-flag (Part A fully resolved; Part B one subjective tune FLAGGED for orchestrator)

Console: 0 real errors (only benign teardown warning + screenshot-tooling `CaptureScreenshotAsTexture` render-target artifacts, both known/non-project). Compile clean (no code touched — prefab-only change). Play STOPPED, scene `_Arena` not dirty, no diagnostic objects left.

---

## PART A — White circle identity + cleanup

### What the white circle ACTUALLY is: a Scene-View GIZMO artifact (NOT a game object)
Decisive empirical isolation (edit mode, Player.prefab instantiated, Warblade idle body sprite assigned):
1. Disabled HeroLight2D GameObject → circle PERSISTS.
2. Disabled WeaponSprite GameObject → circle PERSISTS.
3. Disabled Body SpriteRenderer → character gone but a white circle-on-a-stem REMAINS (no active renderer/Light2D within 2.5u of it — verified via FindObjectsByType).
4. **Toggled SceneView.drawGizmos = false → circle GONE, character renders 100% clean.**
5. Game-view capture (Play mode, Camera.main) of the actual player → NO white circle at all.

Conclusion: the white circle is the Light2D's scene-view gizmo/icon billboard ("circle on a stem"), rendered ONLY when scene-view gizmos are ON. The P5 reference `bluearc_emberswing_close_2026-06-14.png` was a scene-view capture WITH gizmos on, so it captured the gizmo. It is a tooling/capture artifact — it does NOT exist in the real game render. (Consistent with memory: `reference_unity_screenshot_method`.) The candidates the brief listed — (a) empty WeaponSprite (null sprite → renders nothing), (b) HandGlowVFX (only spawned via SkillVfx.CastFlash, NOT in the Warblade basic-swing path: MeleeChainBehavior calls SkillVfx.MeleeArc only), (c) HeroLight2D (soft warm point light, and disabling it didn't remove the circle) — all ruled out by the toggle tests.

### Cleanup done: removed dead WeaponSprite placeholder
`Player/HandAnchor/WeaponSprite` (SpriteRenderer, m_Sprite=0, m_WasSpriteAssigned=0, sortingOrder 1) was a genuine dead placeholder: renders nothing, and is referenced by NO live code. Verified:
- Grep "WeaponSprite": only `Player.prefab` + docs + one ARCHIVED creator script (`Assets/_archive~/pre_v2_editor/CreatePlayerPrefab.cs`). No runtime lookup by name.
- `OrientationSync.weaponRenderer` and `HandAnchorAttach.weaponRenderer` are both `{fileID: 0}` (null) in the prefab — they do NOT point at it. The real weapon is `Instantiate(entry.weaponPrefab, handAnchor)` from the Warblade DB entry at Start.

Action: deleted the WeaponSprite child via manage_prefabs (prefab SAVED). Confirmed: 0 "WeaponSprite" refs remain, all 3 removed fileIDs gone, `HandAnchor m_Children: []`. The `Resources/Prefabs/Warblade.prefab` is a VARIANT of Player.prefab with no WeaponSprite override → removal propagates to it automatically (`m_AddedGameObjects: []`).

Proof it no longer renders: `p6_player_idle_clean.png` (scene view, gizmos off, clean Warblade, no blob, HandAnchor childCount=0).

---

## PART B — Weapon mount state (diagnose + sorting verified; positioning FLAGGED)

Live Play-mode state (Warblade variant spawned, weapon = `Warblade_Greatsword(Clone)`):
- Mount transform: `HandAnchor` (Warblade-variant localPos override (0.30, 0.02)).
- Weapon instance localPos (0.20, 0.10), localScale 1, rotation tracks 8-way facing (SE=-45°/315°, E=0°, N=90°). Correct.
- Weapon sprite: 64x16 blade, pivot (11.52, 8.00) ≈ grip at left ~18%, vertically centered → grip is the pivot, blade extends outward. Sensible.
- Sorting: VERIFIED correct. SE → weaponOrder 1 > bodyOrder 0 (FRONT). Code (`UpdateWeaponSortOrder`) sets order body-1 (BEHIND) for N/NE/NW, body+1 (FRONT) otherwise, per-frame in LateUpdate. No fix needed.
- Sorting layer promoted to body's "Entities" so weapon never sits under floor/cliff. Correct.

### Tuning applied: NONE (sorting already correct; no clearly-wrong value found).

### FLAGGED for orchestrator (subjective — not guessed):
The weapon GRIP floats ~0.5 world-units to the RIGHT of the body in every facing (HandAnchor 0.30 + weapon 0.20 = +0.50 from body center); it does NOT visually connect to the character's hand — there is a clear gap. This is an artistic positioning call (how far in, what Y, whether to use OrientationSync.handOffsets per facing — which are tiny ±0.10). Evidence: `p6_weapon_facing_se.png`, `p6_weapon_facing_east.png`, `p6_weapon_facing_north.png`.
Proposed options for orchestrator to choose:
  - (O1) Reduce weapon localPos.x and/or HandAnchor.x so the grip overlaps the body silhouette's hand (e.g. weapon localPos ~ (0.0–0.05, 0.0)); cheapest, single value.
  - (O2) Tune OrientationSync `handOffsets[8]` per facing so the grip sits in the correct hand for each of the 8 directions (more faithful, more work).
  - (O3) Leave as-is for demo (weapon reads as held-out greatsword; acceptable at gameplay zoom).
Note: in the N capture the BODY sprite stayed south-facing (combat-facing override drove weapon+animator params but the body blend didn't visibly turn) — that is animator/facing-sync behavior, OUT OF SCOPE for P6; flagging only for awareness.

---

## Files
- `Assets/Prefabs/Player.prefab` — removed dead `HandAnchor/WeaponSprite` child (GameObject+Transform+SpriteRenderer). Prefab saved. (Warblade variant inherits.)
- No code changed.

## Screenshots (all under STAGING/_process/2026-06/)
- `p6_player_idle_clean.png` — Part A: clean player, no blob (gizmos off).
- `p6_diag_gizmos_off.png` / `p6_diag_with_light2.png` — gizmo on/off A-B comparison (proof white circle = gizmo).
- `p6_weapon_facing_se.png`, `p6_weapon_facing_east.png`, `p6_weapon_facing_north.png` — Part B mount across facings (game render, no blob).
