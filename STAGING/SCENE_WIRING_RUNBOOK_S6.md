# RIMA — Scene-Wiring Runbook (S6) — GATED work (Unity-MCP-healthy / user session)

> These steps need a RESPONSIVE Unity (this overnight session had UnityMCP `read_console`/play timing out — only
> `refresh_unity` + Editor.log worked). All the demo's CODE is done + compile-clean (PHASE 1 A–D + story + audio).
> What remains = scene/prefab wiring + art + play-verify. Do these in Unity with MCP working, or by hand.
> **Tip:** if MCP keeps timing out, RESTART Unity first (close + reopen the project) — clears the stuck bridge.

## ORDER OF OPERATIONS
A → (F5 play-verify the code) → B → C → D. A & D give the biggest visible payoff.

---

## A. LIGHTING RIG FLIP (biggest visual win) — `Assets/Scenes/Test/PlayableArena.unity`
Goal: kill the "gas-lamp" look → island lit by its own CYAN RIFT (DESIGN_LOCK §2.2). All Light2D, **zero Shadowcaster2D, no bake.**
1. `Global Light 2D`: intensity `0.08 → 0.22`; tint cooler `#1E1B2E`. Keep ApplyToSortingLayers = Floor, Decor_Cliff(12), Decor_Floor(13), Gameplay.
2. ADD `Rune_Pulse_Cyan` (Point Light2D) at rune center (~0,0): color `#00FFCC`, intensity 1.1, inner 0.4 / outer 6, falloff 0.5; add `LightPulse` (±0.2 slow).
3. ADD 2–4 `Rift_Seam_Cyan` (Point Light2D) on floor cracks: `#00FFCC`, intensity 0.5–0.7, inner 0.2 / outer 2–3.
4. Convert `RimLight_*_Cyan` Point → **Freeform** traced along the void-facing cliff edge: intensity 1.2, FalloffIntensity 0.3 (sharp); target Decor_Cliff(12)+Floor edge+Gameplay.
5. DEMOTE warm: keep ONE entrance brazier (4.5 → 1.0, recolor `#FF8033 → #E89020`, keep LightFlicker ±0.1). Delete/disable the other 3 `Brazier_*_WarmLight` + the `Pillar_AmberLight` ring.
6. Reparent ALL lights under one independent `Scene_Lighting` GameObject — NEVER under `RIMA_Cycle2_Dressing`/decor (deactivating decor silently kills lights = the black-cliff root cause).
7. Verify at 640×360: cool-cyan dominant, warm = single ember, cliff void-rim glows cyan, gameplay readable, no warm corner pools. Tune Rune ±0.3 / rim ±0.4.
8. (Per-room mood ramp = future RoomLightingController; build it WITH this rig, not before — see E.)

## B. WEAPON PLAYER-WIRING — `Assets/Prefabs/Characters/Warblade.prefab`
Code is LIVE (OrientationSync swing/flipY, HandAnchorAttach). Missing = the prefab components + DB + sprite import.
1. Import cyan greatsword `31ee0f73` (PixelLab) → PNG, PPU 64, Filter=Point, Compression=None → `Assets/Resources/Weapons/`.
2. On `Warblade.prefab`: add `HandAnchorAttach` (assign `weaponDatabase` = `Assets/Resources/WeaponDatabase.asset`) + `OrientationSync` (assign weaponRenderer = the HandAnchor child SpriteRenderer). Confirm `WeaponDatabase.asset` has a Warblade/Base entry → greatsword prefab.
3. Play-verify: weapon mounts to hand, 8-dir flipY correct, swing fires on attack, slash VFX shows. Tune handOffsets/swing in OrientationSync inspector (A5 feel).

## C. SCREEN-IMAGE WIRING (after images exist)
Code screens (menu/HUD/draft/death/victory) currently self-build with RimaUITheme procedural placeholders.
1. Generate assets per `STAGING/IMAGEGEN_PACK_S6.md` (Priority A first) → import to listed `Resources/UI/RIMA/...` paths (Point filter, no compress, PPU 64).
2. Add `Resources.Load<Sprite>` hooks in the self-building screens (DemoCompleteOverlay, DeathScreenManager, RoomMonologController, HUDController, SkillOfferUI, MenuDungeonBackground) to use the image if present, else keep procedural. (One small cx batch — "bind screen images if present, else fallback".)
3. Skill icons: assign 64×64 icons to `Data/Skills/*.asset` `SkillBase.icon`.

## D. PLAY-VERIFY (F5) — the real combat-feel + demo gate (USER)
Press F5 (open PlayableArena + Play). Verify the night's code in motion:
- Hit-confirm: slash arc + white flash + cyan hitspark fire on hits (VFXRouter entries now populated).
- Juice: hitstop tiers (heavier on finisher/kill), camera kicks OPPOSITE swing, directional shake.
- Input: attack buffer (mash LMB during commit), dash-edge grace near cliff.
- Skills: hitspark/hitstop now fire on SKILL hits too (Batch C parity).
- Story: room-enter monolog lines (R2–R5) + R5 boss title-card + boss phase-2 line at 33% HP.
- Death screen: shows (scale-fix), non-blaming copy, Wishlist CTA. Victory (boss death): run-stats + Wishlist CTA, NO class-select (bypass).
- Audio: dash/hit/finisher/kill SFX (procedural until real clips).
- **A5 = lock the feel** (tune hitstop/shake/buffer numbers to taste). This is THE gate before final art.

## E. DEFERRED-WITH-RIG (don't build blind)
- `RoomLightingProfile` SO + `RoomLightingController` (per-room mood ramp §2.3) — build WITH rig A so light-role refs are real.
- `RoomConnectionProfile` (landmark/continuity §3) — needs landmark art (PixelLab, gated).
- Mob archive-restore (`ARCHIVE/Sprites_Enemies_old/` → FractureImp/ShardWalker/HollowHulk, 0-gen) — kills "colored squares". Autonomous-safe but better done in Unity (sprite assign on prefabs).
- Boss sprite (Penitent Sovereign) — text-card placeholder ships now (§9 #4); real art GATED (PixelLab/RTX-local).

## AUDIO content (DEFERRED, user lane = Sora ChatGPT Plus + Gemini Pro)
Drop real clips at `Assets/Resources/Audio/<Sfx>.wav` (Hit/Death/Dash/Finisher/Shatter/Cast/Gate/Draft/BossIntro) +
`music_demo.*` — AudioManager auto-overrides procedural (loader already built, Batch F). No code change needed.
