# RIMA Rift Threshold System — Door Design Spec (S95)

> **Author:** rima-design (Opus, 2026-05-20)
> **Status:** SPEC LIVE — Codex review pending, then Act 1 PixelLab batch
> **Concept:** "Rift Threshold" — vertical rift seam through wall, gravity-aligned reality tear. NOT a framed door.

---

## 1. Concept — "The Rift Threshold"

The Fracturing is RIMA's core narrative event: reality has been torn open. Sub-rooms are not connected by doors built by architects — they are connected by **tears in the architecture itself**, places where the Fracturing has worn through. The threshold is a **vertical rift of crystalline energy bleeding through a horizontal breach in the wall**.

This is **not a Hades reward door** (no iconography, no choice). It is **not a Diablo portal** (no swirling vortex, no UI prompt). It is a **wound in the world** through which the player walks. Per-Act, the wound bleeds different colors because the corruption beneath each Act has a different signature.

## 2. Asset Decomposition

| Sprite | Type | Layer | Frames | Size | Role |
|---|---|---|---|---|---|
| `arch_opening` (existing Pilot A) | Wall piece | L3 wall | 1 | 64×64 | Architectural hole in the wall. Already produced. |
| `rift_seam_locked` | L6 overlay | L6 accent | 1 still | 64×64 | Dim crystalline seam, gray-cyan, low alpha. Sits inside arch. Direction-invariant (vertical seam). |
| `rift_seam_active` | L6 overlay | L6 accent | 4-frame loop | 64×64 | Pulsing bright rift, glow flicker. Replaces locked variant on unlock. |
| `rift_seam_portal` | L6 overlay | L6 accent | 6-frame loop | 64×64 | Active transition burst (fade-to-black trigger frame). Plays once on player interact. |
| `rift_seam_final` | L6 overlay variant | L6 accent | 4-frame loop | 96×96 | Larger + brighter version for final sub-room's exit — signals macro reward incoming. |

**Total per Act: 4 sprites × 3 Acts = 12 sprites.**
**Animation budget: ~15 frames × 3 Acts = 45 frame-gens.**

Single concept, multi-state via overlay swap and animation. NOT class-multiple — same shape, palette + animation swap per Act.

## 3. Per-Act Visual Variant

| Act | Wall base | Rift color | Particle accent | Mood |
|---|---|---|---|---|
| Act 1 — Shattered Keep | Granite arch (existing) | Cyan crystalline (#00FFCC) | Cyan glint sparks rising | Cold reality tear, jagged crystal |
| Act 2 — Bleeding Wastes | Bone-wrapped arch | Rust ember (#C8502A) with dark red core | Drifting ember + blood mist | Infected wound, throbbing pulse |
| Act 3 — Core Approach | Void-stone arch | Incandescent gold (#FFD700) with violet bleed (#4F2A6B) | Gold sigil fragments floating | Cosmic seam, reality dissolving |

Same shape, same animation rhythm. Palette + particle layer differ. Honors Karar #150 per-Act material LIVE rule.

## 4. State Mechanic

| State | Trigger | Visual | Collider |
|---|---|---|---|
| `Locked` | Sub-room enters `Loading` or `Active` (combat ongoing) | `rift_seam_locked` overlay (dim, no animation, ~30% alpha) | Disabled (player walks past, no prompt) |
| `Unlocking` | `Cleared` fires from `SubRoomSequenceController` | 0.5s flash + camera pulse + `rift_seam_locked` cross-fades to `rift_seam_active` | Disabled during 0.5s |
| `Unlocked` | After unlocking flash | `rift_seam_active` loop, full glow, bright pulse | Enabled, "Devam et" prompt on player overlap |
| `Activating` | Player presses interact (G key) | `rift_seam_portal` burst plays | Disabled (input lock during fade) |
| `Consumed` | Sub-room torn down | GameObject destroyed by `RoomTransitionFX` swap | — |

**Final sub-room exit special case**: use `rift_seam_final` (96px, brighter) — same state machine, larger visual signal that macro reward is one click away.

## 5. Direction-Independent Solution

**Three options analyzed — Option C ADOPTED:**

**Vertical pivot-symmetric seam:** rift sprite is a **vertical column of crystalline energy** with **radial glow** centered on the sprite.
- Radial glow rotationally symmetric → invariant under wall rotation
- Vertical seam = reality tear, gravity-aligned (top-to-bottom narrative correct)
- Z-rotation 0 only — sprite never rotates. Wall rotates; rift stays vertical
- **Iso-invariant** — looks identical regardless of which wall edge it's on

This is the **RIMA-native answer**: rift is a gravity-aligned reality tear, not directional architecture. Inverse of a Hades door (which IS directional architecture).

## 6. PixelLab Production Recipe

**Endpoint**: `mcp__pixellab__create_object` (stills), `mcp__pixellab__animate_object` (loops).

### Act 1 Locked Seam (still)
```
Vertical crystalline rift seam, dim cyan crystal shard,
centered vertical line, narrow 8-12px wide, soft outer
glow radial gradient, fits inside 64×64 stone arch
opening, low intensity dormant state. Pure transparent
background. Iso fake top-down view (RIMA 30-35°).
Palette: #00FFCC core, #3A4D5C dim shell.
Negative Prompt : horizontal elements, framing, door
ornaments, hinges, lock symbols, rotation perspective.
```

### Act 1 Active Seam (4-frame loop)
```
Same vertical cyan rift seam, now BRIGHT pulsing,
high glow intensity, subtle inner flicker, particles
rising from the seam. 4-frame seamless loop, pulse
breathing rhythm.
```

### Act 1 Portal Burst (6-frame one-shot)
```
Vertical cyan rift expanding outward into screen-fill
white-cyan flash. Frame 1-3: rift widens. Frame 4:
peak brightness. Frame 5-6: fade-out to consumed.
One-shot, plays on player interact.
```

Repeat for Act 2 (rust palette, ember particles) and Act 3 (gold + violet, sigil fragments).

**Budget tally:**
- Act 1: 1 still + 4 frames + 6 frames + 4 frames (final variant) = 15 frame-gens
- Act 2: 15
- Act 3: 15
- **Total = 45 frame-gens** (vertical slice: Act 1 only = 15)

## 7. Sub-Room Transition Workflow

```
Walls_Root (sortingLayer "Walls")
├── arch_opening sprite (existing L3 wall, sortingOrder 100)
└── RiftThreshold GameObject (sortingLayer "Walls", sortingOrder 105 — ON TOP)
    ├── RiftSeam SpriteRenderer (rift_seam_locked initial)
    ├── BoxCollider2D (isTrigger, world-space matched to arch interior)
    ├── IntraEncounterDoorTrigger.cs (EXISTING, no changes needed)
    └── RiftThresholdView.cs (NEW — view-layer only)
```

**Lifecycle:**
1. `SubRoomSequenceController` spawns threshold prefab when sub-room loads
2. `RiftThresholdView.Awake()` registers as `Locked`, applies dim sprite
3. Sub-room `Cleared` event fires → `RiftThresholdView.PlayUnlockSequence()`:
   - 0.5s flash anim
   - Cross-fade sprite from `_locked` to `_active` loop
   - Call `IntraEncounterDoorTrigger.SetActive(true)`
4. Player overlaps + presses G
5. `IntraEncounterDoorTrigger` calls `SubRoomSequenceController.AdvanceSubRoom(fromDoorSocketId)`
6. `RoomTransitionFX.DoTransition(SwapSubRoomWhileBlack)` fires
7. `RiftThresholdView` plays `_portal` burst animation during fade-to-black
8. Threshold GameObject torn down with old sub-room. Next sub-room's threshold spawns new instance at validator-mirrored position

## 8. Codex Review Notes — Tartışmalı Noktalar

Codex review için 6 nokta:

1. **Direction-independent claim**: Vertical rift seam iso-invariant kalır mı arch_opening EW vs NS rotation'da? Mitigation: standardize pivot at sprite center for both arch variants.

2. **45-gen budget vs alternative**: Act 1 only 15 gen verticals slice ile başla, Act 2/3 defer. Veya shader-only solution (~9 stills + shader animation).

3. **State machine complexity**: View 5-state machine + logic 6-state. Source-of-truth = logic, view subscribes events only.

4. **`rift_seam_final` 96px in 64px Pilot A wall grid**: Sprite extends beyond wall — acceptable "glow halo" effect, bounds check needed.

5. **Per-Act asset isolation**: `Act1_ShatteredKeep/walls/rift_thresholds/` subfolder recommended.

6. **Final sub-room visual signal alternative**: Larger sprite vs above-arch icon. Recommend larger sprite (all diegetic, no UI).

## 9. Next Steps

1. **Codex review dispatch** — 6 Review Notes ile explicit review
2. **APPROVE → Act 1 PixelLab batch** — 15 frame-gens (vertical slice)
3. **`RiftThresholdView.cs` implementation** — Codex review state machine boundary lock sonra
4. Act 2/3 production deferred

## Relevant File Paths

- `Assets/Scripts/Runtime/Encounter/IntraEncounterDoorTrigger.cs` — existing, DO NOT modify
- `Assets/Scripts/Core/RoomTransitionFX.cs` — existing singleton
- `Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs` — existing mirror validator, no rule additions needed
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_3_arch_opening.png` — existing wall base, keeps current role
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/rift_thresholds/` — NEW folder for rift seam overlays
