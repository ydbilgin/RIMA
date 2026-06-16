# DESIGN — Wall-less Door = Teleport Portal (G2)

> Status: PLAN (read-only design, no code/Unity). Demo 19 June 2026.
> Replaces awkward "doors on a floating island with no walls" with a teleport-portal beam transition.
> Canon-safe: 8-direction sprites untouched; _Arena dev-direct scene; slate/void/ember + ambient 0.22.

## 1. REAL HOOK (verified against code — NOT package class names)

Live exit path (DoorTrigger.cs is `[Obsolete]` LEGACY — IGNORE it):

```
RoomRunExitDoorTrigger.Update()/OnTriggerEnter2D   (RoomRunDirector.cs:1841)
   -> RoomRunDirector.TryEnterDoor(choiceIndex)     (:1820)  lifecycle.MarkAdvancing()
   -> RoomRunDirector.AdvanceTo(choiceIndex)         (:1801)  CurrentNodeId = choices[i].id
   -> RoomRunDirector.BuildCurrentRoom()             (:289)   tears down + rebuilds INSTANTLY
```

Doors are built by `IsoRoomBuilder.BuildExitDoors(doorTypes)` (:827): **N exits = N portal GameObjects**,
one per branch child (`CurrentChoices`). Each gets a `RoomRunExitDoorTrigger` + teal locator ring
(`RoomRunDoorLocatorPulse`). So "N portals = N nodes" ALREADY HOLDS — no count change needed.

KEY FINDING: the live advance is **instant** — `RoomTransitionFX` (the screen-fade singleton) is NOT
called on this path. So the beam/zoom is a *new layer inserted into `TryEnterDoor`*, not a rewrite.

## 2. INSERTION POINT (single, surgical)

`TryEnterDoor(choiceIndex)` becomes: gate `MarkAdvancing()` -> start a **portal transition coroutine**
on RoomRunDirector -> coroutine runs beam+zoom -> on-black calls the EXISTING `AdvanceTo(choiceIndex)`
-> fade/zoom back. Reuse `RoomTransitionFX.Instance.DoTransition(onBlack)` for the black-cover swap so
the rebuild pop is hidden (it already fades audio + footsteps). The beam/zoom plays BEFORE handing to it.

Fallback: if `RoomTransitionFX.Instance == null`, call `AdvanceTo` directly (current behavior) — never
softlock. Reuse existing `DraftAutoCloseTimeoutSec`-style hard timeout pattern around the coroutine.

## 3. TRANSITION CHOREOGRAPHY (timings, ~1.0s total)

| Phase | Dur | What |
|---|---|---|
| T0 collapse | 0.15s | Player input locked (`PlayerController.enabled=false`); player sprite scale-squash toward portal mouth; `EchoPuffBurst.Spawn(playerPos, 0.5f, dissolve:true)` — cyan dissolve-out (already pooled-free, #00FFCC) |
| T1 beam | 0.30s | Player SpriteRenderer hidden; spawn vertical **blue beam** at portal (see VFX); beam rises from floor; `ScreenShakeDriver.Instance?.Shake(0.06f,0.2f)` |
| T2 zoom | 0.25s | Camera `orthographicSize` lerps ~0.7x toward portal arch (zoom IN), camera pos eases toward portal; on `CameraFollow` set a one-shot override target = portal, restore after |
| T3 black | — | `RoomTransitionFX.DoTransition(onBlack=AdvanceTo)` covers the actual room teardown/rebuild |
| T4 arrive | fade-in | New room fades in at fixed ortho 5.0 (ApplyFixedDemoCamera already resets it); `EchoPuffBurst.Spawn(newSpawn, 0.6f, dissolve:false)` cyan materialize-in at player spawn; re-enable input |

Colors: beam = void/arcane cyan `#00FFCC` (matches EchoPuffBurst + door locator teal `(0,1,0.9)`), with
an ember `#E89020` core flash at T1 for the canon ember accent. Use `SkillVfx` engine layer — NO new prefab.

## 4. VFX — REUSE FIRST (engine-layer)

- **Blue beam**: build via a `SkillVfx` helper (proposed `SkillVfx.PortalBeam(pos, color, life)`) using a
  vertical quad/LineRenderer with `SharedAdditiveMaterial` (already additive, sorted on "VFX"). Texture =
  existing `Assets/Resources/Art/Telegraphs/telegraph_line_beam.png` (already in Resources, reusable) OR a
  1px white stretched + additive tint. `ScaleFadeAndDestroy` pattern handles the rise+fade. No asset gen needed.
- **Dissolve/materialize puff**: `EchoPuffBurst.Spawn(pos, scale, dissolve, moteCount)` — EXISTS, cyan,
  PPU64 pixel-snapped, self-destroying. Use dissolve:true on exit, dissolve:false on arrive.
- **Portal arch art**: ALREADY SHIPPED — `Assets/Art/Portals/portal_arch_{combat,elite,boss,chest}_{frontal,angled}.png`
  wired via `IsoRoomBuilder.ResolvePortalSprite`. No new portal sprite needed for demo.
- **Camera zoom**: pure code (lerp ortho + pos), reuse `CameraFollow` override; no asset.

## 5. ASSET / VFX GAPS (small)

1. (optional polish) A dedicated 1-frame **beam core glow** sprite for crisper pixel look — `telegraph_line_beam`
   is a reuse stand-in; PixelLab could make a tall cyan beam strip post-demo. NOT a blocker.
2. (optional) Portal "open mouth" 2-3 frame anim — current arch is static; demo can ship static + beam.

## 6. RISK

- **MEDIUM**: `CameraFollow` zoom override must restore cleanly or rooms render at wrong zoom (regression of
  the BUG-2 fixed 5.0 ortho). Mitigation: store/restore ortho around coroutine; `ApplyFixedDemoCamera` in
  `BuildCurrentRoom` already re-asserts 5.0 on arrive, so a missed restore self-heals next room.
- **LOW**: input-lock must be released on EVERY exit (timeout/exception). Use try/finally re-enable, mirror
  `RoomClearSequence` finally-block discipline.
- **LOW**: Merchant path opens doors via `HandleMerchantRoom` -> same `RoomRunExitDoorTrigger`, so it inherits
  the beam automatically — verify it doesn't fire mid-shop.
- Scope: this is ONE coroutine + one `SkillVfx.PortalBeam` helper + a `TryEnterDoor` edit. No FSM/refactor.
