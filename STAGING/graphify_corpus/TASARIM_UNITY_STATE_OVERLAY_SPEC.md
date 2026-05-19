---
status: REFERENCE
faz: 1
tarih: 2026-05-04
ozet: "Unity state overlay UI spesifikasyonu"
---
# RIMA -- Unity State Overlay Specification
Version: 1.0 | Date: 2026-05-01 | Status: DRAFT

Scope: Visual overlay and pip specifications for all tracked combat states.
Applies to: Unity 2D URP, isometric scene `Assets/Scenes/_IsoGame.unity`, Namespace RIMA.

---

## Architecture Overview

### StateOverlayController (enemy-side)

All enemy prefabs carry a `StateOverlayController` MonoBehaviour. It owns:

- A pool of overlay SpriteRenderers sorted by layer offset.
- A `PipGroupRenderer` child for pip-stack states.
- A world-space decal child (for Scar -- see engineering flag EF-01).

Render layer ordering relative to the base sprite (Order In Layer = 0):

```
Layer offset +0 : base enemy sprite
Layer offset +1 : overlay tint (Broken, Shattered, Opened flash)
Layer offset +2 : decal / pip group (Scar gash, Sundered cracks, Cracked fissures, Hex dots, Mark reticle)
Layer offset +3 : world icon / floating indicator (reserved for future stun/freeze icons)
```

All overlay sprite assets: 128x128 px canvas, PPU=64, produced via `create_spritesheet` endpoint.
`create_character` endpoint is FORBIDDEN for all overlay/state art.

### CasterStateHUD (self-states)

Heat (Gunslinger) and Tension (Ronin) are caster self-states, not enemy overlays.
They use a separate `CasterStateHUD` MonoBehaviour on the player/caster prefab.
This component drives weapon SpriteRenderer swaps and screen-space HUD elements.
It does NOT use `StateOverlayController`.

### PipGroupRenderer

Shared component for any state that uses a dot/pip stack displayed above the enemy head.
Parameters exposed per state:
- `pipSprite` : Sprite asset for a single pip icon
- `pipCount` : current stack count (drives instantiation)
- `arcRadius` : float, arc spread above enemy head
- `colorActive` : Color for filled pip
- `colorInactive` : Color for empty pip slot (if showing max slots)
- `phaseColorMap` : optional Color[] indexed by phase enum value

---

## State Specifications

---

### Sundered (Warblade)

**Owner class:** Warblade -- applied via `WarbladeSkillHandler` on hit events.

**State data type:** `int` stack, range 0-3. Stored on `EnemyStateComponent.sunderStacks`.

**Visual representation:**
Plate-shard crack overlay on the enemy sprite. Three distinct overlay frames:
- Stack 1: single hairline crack across one shoulder plate region
- Stack 2: second crack added, slight darkening of plate area (tint blend ~15% dark)
- Stack 3: both cracks visible plus a large shard-separation gap, plate tint shifts to a deep grey (~30% dark blend)
Cracks are geometric, angular, plate-shaped fragments. NOT organic body fissures (those belong to Brawler Cracked).

**Anchor:** Sprite-local. The crack overlay SpriteRenderer is a child of the enemy sprite object and inherits its transform (including any idle bob animation).

**Update trigger:** On apply (each Sundered stack applied triggers a swap to the next overlay frame). No tick update needed. On stack removal or state clear, revert to stack 0 (no overlay).

**Implementation approach:**
`StateOverlayController` holds a `Sprite[] sunderFrames` array (4 entries: index 0 = null/clear, 1-3 = crack frames). On `SetSunderStacks(int n)`, the overlay SpriteRenderer at layer offset +2 swaps to `sunderFrames[n]`. Uses `Color.Lerp` for tint blend on the base sprite material (a secondary material slot or a `_TintColor` shader property).

**Layering order:** Overlay SpriteRenderer at layer offset +2 relative to base sprite.

---

### Broken (Warblade)

**Owner class:** Warblade -- auto-applied by `WarbladeSkillHandler` when `sunderStacks` reaches 3 and a threshold consume event fires.

**State data type:** `bool`. Stored as `EnemyStateComponent.isBroken`.

**Visual representation:**
Full armor-crack tint across the entire enemy sprite. Tint: deep steel-grey (#4A4A4A at ~40% blend over sprite). A single full-body overlay frame (distinct from the per-stack Sundered frames -- this is a unified "shattered plate" look). A hit-reaction frame lock is active while `isBroken == true`: the enemy uses a staggered/flinched idle pose (separate animator state `Broken_Idle`).

**Anchor:** Sprite-local. The tint is applied via `StateOverlayController` on the base sprite material.

**Update trigger:** On apply (bool set to true) and on clear (bool set to false). No tick. Hit-reaction lock is enforced by the animator state machine: `isBroken` bool drives a transition into `Broken_Idle` state that overrides normal idle.

**Implementation approach:**
`StateOverlayController.SetBroken(bool)` swaps to the full-armor-crack overlay sprite at layer +1 (tint layer) and sets animator parameter `IsBroken`. On clear, restores base material and exits `Broken_Idle`. Mutually exclusive with Sundered overlays: clearing Broken also clears Sundered stack visuals.

**Layering order:** Tint at layer offset +1; crack overlay sprite at layer offset +2.

---

### Cracked / Shattered (Brawler)

**Owner class:** Brawler -- applied via `BrawlerSkillHandler`.

**State data type:**
- Cracked: `int` stack, range 1-3. Stored as `EnemyStateComponent.crackedStacks`.
- Shattered: `bool`. Stored as `EnemyStateComponent.isShattered`. Auto-set when `crackedStacks` reaches 3 and consume fires.

**Visual representation:**
Cracked (stacks 1-3): Organic hairline body fissures radiating from the torso. NOT plate-shard geometry (that is Sundered). These are thin, slightly glowing cracks in the enemy's body/skin/material:
- Stack 1: one fissure line across the torso
- Stack 2: two fissure lines, slightly wider
- Stack 3: three fissure lines, faint inner glow on crack edges (suggest structural instability)
Shattered (bool): Full-body hairline crack overlay covering the entire sprite. Fissures branch across limbs and head. Faint glow on all crack edges. No plate-shard geometry.

**Anchor:** Sprite-local. Child SpriteRenderer inherits parent transform.

**Update trigger:** On apply (stack increment). On Shattered apply (bool true). On clear of either state.

**Implementation approach:**
`StateOverlayController` holds `Sprite[] crackedFrames` (4 entries: 0=clear, 1-3=fissure stages) and `Sprite shatteredFrame`. `SetCrackedStacks(int n)` drives frame swap on layer +2 overlay SpriteRenderer. `SetShattered(bool)` overrides to `shatteredFrame`. Shattered clears Cracked visuals (single unified overlay replaces stack frames).

**Layering order:** Overlay at layer offset +2.

---

### Scar (Shadowblade)

**Owner class:** Shadowblade -- applied via `ShadowbladeSkillHandler` (Scarbinding skill). Each Scar application increments the stack.

**State data type:** `int` stack, max 3 (design default before collapse eligibility). Stored as `EnemyStateComponent.scarStacks`. Collapse trigger clears all stacks to 0.

**Visual representation:**
Persistent diagonal gash decal on the enemy. Per stack, one additional gash decal is placed at a distinct position/angle:
- Stack 1: one diagonal slash mark (top-left to bottom-right bias)
- Stack 2: second slash at a crossed angle
- Stack 3: third slash, completing a three-strike pattern
Each gash uses a 2-frame micro-spritesheet: frame 0 = idle (static gash), frame 1 = pulse (slight brightness/spread on the gash edge). Animation: idle 1.0s -> pulse 0.2s -> idle loop.

**Anchor:** WORLD-SPACE. The Scar decal child GameObject does NOT inherit parent rotation. This is a locked design decision (2026-05-01). The decal must remain axis-aligned in world space even if the enemy sprite rotates or if camera orientation changes.
Implementation: Child GameObject with its own SpriteRenderer. On each Update/LateUpdate, counter-rotate by the parent's world rotation to neutralize inherited rotation. See engineering flag EF-01.

**Update trigger:** On apply (stack increment adds a new decal instance). On consume/collapse (all decal instances cleared). Pulse animation runs continuously on a timer while stack > 0.

**Implementation approach:**
`StateOverlayController` manages a `List<ScarDecalInstance>` where each instance is a child GameObject with:
- SpriteRenderer using the gash micro-spritesheet
- `ScarDecalAnimator` component (drives 2-frame idle/pulse loop)
- `WorldSpaceDecalAnchor` component (counter-rotation logic -- see EF-01)
Adding a stack instantiates a new `ScarDecalInstance` from prefab, positions it at a preset offset for that stack slot. Clearing all stacks destroys all instances.

**Layering order:** Layer offset +2. Because it is world-space, sorting layer must be explicitly set to place it above the enemy base sprite but below UI. Use `SortingLayer: Entities`, `Order: baseOrder + 2`.

---

### Mark (Ranger)

**Owner class:** Ranger -- applied via `RangerSkillHandler`.

**State data type:** `bool` (single mark active) or `int` stack (if design extends to multi-mark). Current design default: bool. Stored as `EnemyStateComponent.isMarked`.

**Visual representation:**
Target reticle icon floating above the enemy head. NOT a gash (that is Scar). NOT a dot-stack (that is Hex pips). The reticle is a circular crosshair icon, subtly animated (slow rotation or gentle pulse):
- Active: reticle visible, slight idle rotation (~15 deg/sec or a 2-frame pulse sprite)
- Cleared: reticle hidden

**Anchor:** World-space, positioned above the enemy's head at a fixed Y offset (+0.75 world units above sprite top). Does not rotate with enemy sprite.

**Update trigger:** On apply (bool true -> show reticle). On clear (bool false -> hide). No tick update needed.

**Implementation approach:**
`StateOverlayController` has a `markReticleObject` child GameObject (pre-instantiated, toggled active/inactive). `SetMark(bool)` calls `SetActive`. The reticle object sits at layer offset +2 in Entities sorting layer. A simple `MarkReticleAnimator` component drives the rotation or 2-frame pulse.

**Layering order:** Layer offset +2. Floats above head via Y offset, not via Z or layer stack.

---

### Heat (Gunslinger)

**Owner class:** Gunslinger (CASTER self-state) -- managed by `GunslingerStateComponent` on the player prefab.

**State data type:** `float` 0-100. Stored as `GunslingerStateComponent.heat`.

**Visual representation:**
Three tiers based on heat value:
- Cool (Heat 0-40): no overlay, normal weapon sprite
- Warm (Heat 41-80): barrel/weapon SpriteRenderer swaps to warm-glow frame (slight orange tint on weapon barrel pixels)
- Hot (Heat 81-100): barrel swaps to hot-glow frame (bright orange/red glow on barrel). At Heat > 80: a screen-space heat distortion post-process effect activates on the muzzle area (FullScreen URP pass or a localized distortion SpriteRenderer with a wavy shader)

**Anchor:** Weapon sprite frames are sprite-local (attached to weapon child SpriteRenderer). Heat distortion at Hot tier is screen-space (post-process or overlay canvas element near the weapon screen position).

**Update trigger:** On tick (Heat value updates each frame from cooldown/firing logic). Tier transitions trigger sprite swap immediately when threshold is crossed.

**Implementation approach:**
`CasterStateHUD` on the Gunslinger prefab:
- Watches `GunslingerStateComponent.heat` each Update.
- Calls `WeaponSpriteController.SetHeatTier(HeatTier)` on tier change (enum: Cool/Warm/Hot).
- `WeaponSpriteController` swaps the weapon child SpriteRenderer's sprite from a `Sprite[3] heatTierFrames` array.
- Heat distortion: a separate `HeatDistortionController` component that enables/disables a URP FullScreenPassRendererFeature (or a local distortion mesh) when tier transitions to/from Hot.

**Layering order:** Weapon sprite is part of the caster character rig (not an enemy overlay). Layer ordering is within the caster's own sprite hierarchy. Heat distortion at Hot tier renders on top of all world geometry (screen-space pass).

---

### Charge (Generic)

**Owner class:** Any class with a charge mechanic. Parameterized via `ChargeIndicatorConfig` ScriptableObject. Applied via `ChargeStateComponent` (generic, attached to caster prefab).

**State data type:** `float` 0-100 or `int` step count (config-driven). Stored as `ChargeStateComponent.chargeValue`.

**Visual representation:**
Pip buildup indicator on the caster. A `PipGroupRenderer` instance configured per class:
- `pipCount`: number of pips to display (e.g., 3 for a 3-charge class)
- `pipShape`: sprite asset per class (e.g., orb, arrow-nock, crystal shard)
- `colorEmpty`: color of unfilled pips
- `colorFilled`: color of filled pips
- `arcRadius`: spread of pip arc above caster head
Filled pips fill left-to-right as charge accumulates. At full charge (all pips filled), a brief pulse animation plays on all pips.

**Anchor:** World-space, above caster head (same pattern as Mark reticle but on the caster object).

**Update trigger:** On tick (if float-based, updates each frame and recalculates filled pip count). On step (if int-based, updates on each charge increment event).

**Implementation approach:**
`ChargeStateComponent` holds a `ChargeIndicatorConfig` reference. On charge value change, calls `PipGroupRenderer.SetFillCount(int filledCount)`. `PipGroupRenderer` instantiates N pip sprites from the config, positions them in a fixed arc, and updates colors. The full-charge pulse is triggered by `PipGroupRenderer.PlayFullPulse()` when `filledCount == pipCount`.
Classes override `ChargeIndicatorConfig` in their own prefab variant.

**Layering order:** World-space above caster head. Sorting layer: Entities, order: casterBaseOrder + 2.

---

### Hex Pips / Debuff / Pressure / Overload (Hexer)

**Owner class:** Hexer -- applied via `HexerSkillHandler`.

**State data type:** `int` stack driving a 3-phase `enum HexPhase { Debuff, Pressure, Overload }`.
- Debuff: stacks 1-2
- Pressure: stacks 3-4
- Overload: stacks 5+
Stored as `EnemyStateComponent.hexStacks` and `EnemyStateComponent.hexPhase`.

**Visual representation:**
Dot-stack pip row above enemy head. NOT a reticle (that is Mark). NOT a gash (that is Scar).
- Each pip is a small circular dot sprite.
- Color shifts per phase:
  - Debuff (1-2): cool purple/violet (#9B59B6 approx)
  - Pressure (3-4): deep magenta (#C0392B-ish purple-red)
  - Overload (5+): bright electric violet/white pulse (#E056FD approx), pips pulse in sync
- Pip count displayed = current `hexStacks` value (no empty-slot ghosts shown).

**Anchor:** World-space, above enemy head at a fixed Y offset. Does not rotate with enemy.

**Update trigger:** On apply (stack increment) and on clear. Phase transition triggers immediate color update on all existing pips.

**Implementation approach:**
`StateOverlayController` holds a `PipGroupRenderer hexPips` reference configured with:
- `pipSprite`: small circular dot asset
- `phaseColorMap`: Color[3] indexed by `HexPhase`
`SetHexStacks(int n)` calls `hexPips.SetFillCount(n)` and `hexPips.SetPhaseColor(hexPhase)`.
At Overload phase, `hexPips.PlayPhasePulse()` drives a continuous sync pulse animation on all pip sprites.

**Layering order:** Layer offset +2 relative to enemy base sprite.

---

### Opened (Ronin)

**Owner class:** Ronin -- the `RoninSkillHandler` sets this state on the enemy when the enemy executes an attack while Ronin is in sheathe stance.

**State data type:** `bool` with auto-expiry. Stored as `EnemyStateComponent.isOpened` + `EnemyStateComponent.openedTimer`.

**Visual representation:**
Brief highlight frame on the enemy sprite. Communicates to the player: "this enemy has an open window right now."
- On apply: enemy sprite flashes to a highlight overlay (bright white/gold edge glow, ~80% blend, single overlay frame).
- Fade: linearly fades out over 0.5-1.0 seconds (design range; final value TBD by feel).
- After fade: overlay hidden, `isOpened` cleared.
NOT a persistent decal. NOT a pip. A transient flash-fade.

**Anchor:** Sprite-local. Inherits enemy sprite transform.

**Update trigger:** On apply (flash starts). On tick (fade progresses each frame via `openedTimer` countdown). On expiry (auto-clear).

**Implementation approach:**
`StateOverlayController.SetOpened(float durationSeconds)` activates the `openedHighlightRenderer` (SpriteRenderer at layer +1) at full alpha and records expiry time. Each Update, `StateOverlayController` lerps the alpha from 1.0 to 0.0 over the duration. On alpha reaching 0, disables the renderer and clears `isOpened`.
The highlight overlay is a single sprite (full enemy silhouette with edge glow treatment), not a tint pass, to give crisp edge definition.

**Layering order:** Layer offset +1 (tint/flash layer).

---

### Tension (Ronin)

**Owner class:** Ronin (CASTER self-state) -- managed by `RoninStateComponent` on the Ronin prefab.

**State data type:** `float` 0-100. Stored as `RoninStateComponent.tension`.

**Visual representation:**
Sword-glow intensity on the Ronin weapon sprite, driven by Tension value:
- Tension 0-33: no glow, normal sword sprite
- Tension 34-66: faint glow frame (soft blue-white edge on blade)
- Tension 67-99: medium glow frame (brighter, wider edge glow)
- Tension 100: full-glow frame (maximum intensity, entire blade lit, optional brief flash animation on reaching 100)

**Anchor:** Sprite-local on the Ronin weapon child SpriteRenderer. Part of the caster's own sprite hierarchy.

**Update trigger:** On tick (Tension value changes continuously from stances/actions). Tier transitions trigger sprite swap immediately.

**Implementation approach:**
`CasterStateHUD` on the Ronin prefab:
- Watches `RoninStateComponent.tension` each Update.
- Evaluates tier (None/Low/Mid/Full) from threshold constants.
- Calls `WeaponSpriteController.SetTensionTier(TensionTier)` on tier change.
- `WeaponSpriteController` swaps `Sprite[4] tensionTierFrames` on the weapon child SpriteRenderer.
- At Tension 100 (Full tier), `WeaponSpriteController.PlayFullTensionFlash()` triggers a 2-3 frame pulse animation before settling on the full-glow idle frame.

**Layering order:** Weapon sprite is within the Ronin character rig. No separate overlay layer needed -- sprite swap on the weapon SpriteRenderer itself.

---

## Engineering Feasibility Flags

The following items require a Unity engineer to confirm before production sign-off on the affected state.

---

**EF-01: World-space Scar decal counter-rotation**
State affected: Scar (Shadowblade)
Issue: The Scar gash decal must NOT rotate with the enemy parent transform. In Unity 2D, a child SpriteRenderer inherits parent rotation. Counter-rotating in LateUpdate is the standard workaround, but it may produce a one-frame lag visible at high rotation speeds.
Confirm:
- Is LateUpdate counter-rotation sufficient, or is a separate root-level GameObject pool required?
- Does the isometric camera rig ever rotate the parent enemy object? If enemies never rotate (only sprites flip), this may be a non-issue.
- Confirm that Sorting Layer order is preserved correctly when the decal is on a separate world-space object vs. a sprite-local child.
Blocker: Scar decal art production should not start until this is confirmed. Design decision (world-space anchor) is locked; implementation path is not.

---

**EF-02: Heat distortion at Hot tier**
State affected: Heat (Gunslinger)
Issue: Muzzle heat distortion at Heat > 80 requires either a URP FullScreenPassRendererFeature or a localized distortion mesh with a custom shader. Both approaches have performance implications on lower-end targets.
Confirm:
- Which distortion approach is approved for the target platform profile?
- Is a FullScreenPassRendererFeature available and budgeted in the current URP render pipeline asset?
- Fallback: if distortion is too costly, a simple screen-space sprite overlay (shimmer animation) is acceptable as a downgrade path. Engineering to flag if the full distortion must be cut.

---

**EF-03: PipGroupRenderer arc positioning in isometric view**
State affected: Hex pips, Mark reticle, Charge indicator
Issue: In an isometric 2D scene, "above the enemy head" in world space may drift visually depending on the enemy sprite's isometric offset. A fixed Y offset in world units may not read as "above head" for all enemy sizes.
Confirm:
- Should pip Y offset be a per-prefab config value on `StateOverlayController`, or driven by a head-bone anchor point?
- For enemies taller than 128px (bosses, large enemies), the pip group may need a larger Y offset. Confirm whether `PipGroupRenderer.arcYOffset` should be exposed per prefab.

---

**EF-04: Opened highlight sprite vs. shader tint**
State affected: Opened (Ronin)
Issue: The spec calls for an edge-glow highlight overlay sprite (full silhouette with glow treatment, at layer +1). This requires a pre-authored sprite asset per enemy type. An alternative is a shader-based outline pass (URP renderer feature or per-material shader property) that generates the edge glow procedurally, which would not require per-enemy art.
Confirm:
- Is a shader-based outline approach preferred (no per-enemy art needed, but requires shader work)?
- Or is a per-enemy overlay sprite acceptable (art cost per enemy but no shader dependency)?
- If shader approach: confirm the outline renderer feature is available in the pipeline asset and does not conflict with EF-02.

---

**EF-05: StateOverlayController material slots**
State affected: Sundered tint, Broken tint, Cracked fissure glow
Issue: Some states (Sundered stack 2-3, Cracked stack 3 fissure glow) require a tint or glow blend on the base enemy sprite material, not just a separate overlay SpriteRenderer. This implies a secondary material slot or a `_TintColor` / `_GlowColor` shader property on the enemy sprite shader.
Confirm:
- Does the current enemy sprite shader (URP 2D Lit or custom) support a `_TintColor` property for blending?
- If not, is a custom sprite shader in scope, or should all tinting be handled by a semi-transparent overlay SpriteRenderer at layer +1 instead (simpler but may look less integrated)?

---

*End of UNITY_STATE_OVERLAY_SPEC.md*

