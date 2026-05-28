# PixelLab Object Production Log

## 2026-05-19 S94 — Rift Theme Batch 1

**Pipeline:** Orchestrator (Sonnet/MCP) fires create_object + create_object_state → User V3 web UI interpolates → User exports → Sonnet Unity import.

**Workflow split LOCK:** `feedback_object_state_animation_workflow_split` memory.

### Object 1: wall_rift_small (Sub-Room 1 — subtle wall accent)
- **Object ID:** `22e83d26-3329-4738-9615-4cafd2335a8c`
- Size: 64×64
- View: high top-down (Karar #100 35° angled)
- Cost: 20 gen

States (4):
| Name | State ID | Edit description |
|---|---|---|
| closed | `5cb3162d-2fec-4ddd-98e2-230e56d91ef5` | Dormant, no cyan glow, lifeless fissure |
| opening_25 | `2ca80fc2-aded-4c72-a68a-c617a7eafa5d` | Faint cyan glow seeping |
| opening_75 | `d212eed0-3b8c-4d24-9859-4d07ce4661f5` | Crack widening, void hint, pale cyan rim |
| open | `c96f0e9d-c949-4ed1-ac81-215e18a66908` | Fully open, intense cyan+violet void, energy wisps |

### Object 2: wall_rift_large (Sub-Room 2 — elite/final signature, void portal)
- **Object ID:** `c3729267-7c44-4a83-b728-55b341ec7a74`
- Size: 128×128
- View: high top-down
- Cost: 20 gen

States (4):
| Name | State ID | Edit description |
|---|---|---|
| closed | `6d4f338d-8861-49a1-b720-9107a0132b2f` | Y-shape fracture dormant, no glow |
| fracturing | `0f6a3a48-a7c3-4c2b-9b42-3ea310f50583` | Cyan pulses, sparks at edges, hairline secondary cracks |
| opening | `0c4057dd-3263-44a4-acf8-0f06367d726c` | Fracture widens, stone chunks fall, void swirling |
| open_portal | `c38f5621-0c0b-47af-b433-e377d55354bc` | Full portal showing another world, dramatic energy spill |

### Object 3: floor_rift_scar (Combat zone narrative)
- **Object ID:** `24f477a0-e7d3-4f5b-8d06-a88a5a1e065f`
- Size: 64×64
- View: high top-down
- Cost: 20 gen

States (3 planned, 2 fired + 1 PENDING retry):
| Name | State ID | Edit description |
|---|---|---|
| dormant | `a34a09e9-6c0b-4ece-96e8-202e8d13915c` | Pattern visible, center dark, no glow |
| pulse_1 | `e8927741-6cf0-4be7-a5fe-a9a10880cbe0` | Faint cyan glow at center, edges catching light |
| active | `640ce406-096a-46fd-8126-94dd954b3a72` | Full glow with energy along all cracks |

---

## User next step (V3 web UI workflow)

For each completed object:

1. **PixelLab web UI** → Open object by ID
2. **Custom Frames + Interpolate**:
   - Add states as keyframes in order (closed → opening_25 → opening_75 → open)
   - Insert 6-10 interpolated frames between each pair
   - Easing: ease-in-out (rift "breathes")
3. **Loop strategy**:
   - **Continuous breathe:** closed → open → closed (full cycle, 24-32 frames)
   - **One-shot:** closed → open (triggered, 12-16 frames, hold on `open`)
4. **Export** as PNG sheet (or GIF for preview)
5. **Drop file in STAGING/**, notify orchestrator for Unity import

## Budget

| Item | Cost | Subtotal |
|---|---|---|
| 3 base objects | 20 gen × 3 | 60 gen |
| 10 states fired | (TBD per state) | ~100-200 gen estimated |
| 1 pending state | TBD | TBD |
| **Total batch** | | **~160-260 gen** |

Balance at start: **3740 / 5000 gen** (before this batch).
Estimated after batch complete: ~3480-3580 / 5000.

## Monitoring

Check job status:
```
mcp__pixellab__get_object(object_id="<uuid>")
```

Or list all processing:
```
mcp__pixellab__list_objects(status_filter="processing")
```

Or list completed by tag (when tags applied via metadata):
```
mcp__pixellab__list_objects(status_filter="completed", tags="rift_obj_s94")
```

## Sub-room placement plan (after animation imported)

- **Sub-Room 1**: place `wall_rift_small` animated sprite on top wall, ~(9, 9.5, 0) — subtle accent
- **Sub-Room 2**: place `wall_rift_large` animated sprite on back wall, ~(34, 9.0, 0) — elite signature (Spawn_02 offset +25)
- **Both sub-rooms**: optional `floor_rift_scar` near combat center

---

## 2026-05-19 S94 — Rift Theme Batch 2 (re-run, new IDs)

**Note:** Batch 1 IDs above are from a prior run. Batch 2 below was generated in the same session with fresh objects. Use whichever set renders best in V3 web UI.

### Object 1: wall_rift_small (Batch 2)
- **Object ID:** `2356cebc-52b0-453b-b0ad-78fb4ae18d28`
- Size: 64x64
- View: high top-down
- Cost: 20 gen

States (4):
| Name | State ID | Status |
|---|---|---|
| closed | `8fc0daf1-86b2-4b7a-a881-7111762529c2` | processing |
| opening_25 | `1fb91316-7022-40be-acd4-29d7376aeaa6` | processing |
| opening_75 | `57caba5a-7d38-4b81-9ad2-aa5fc77bf3ef` | processing |
| open | `a5dd2437-f45e-4a09-b8c7-34cbdc5175c5` | processing |

### Object 2: wall_rift_large (Batch 2)
- **Object ID:** `d536f49c-0d2d-4507-9973-9791b86b009f`
- Size: 128x128
- View: high top-down
- Cost: 20 gen

States (4):
| Name | State ID | Status |
|---|---|---|
| closed | `e529d0a2-3c47-4d54-bb0c-1c5c7b6ab061` | processing |
| fracturing | `f9544ccc-db3a-43e3-9adc-adceebda43a4` | processing |
| opening | `734280a8-fdf8-4ae1-bd49-412994a38d5c` | processing |
| open_portal | `29537438-6b52-4996-b17e-c704c64c07ac` | processing |

### Object 3: floor_rift_scar (Batch 2)
- **Object ID:** `c88143c5-d926-473f-b7c0-d20cfcbeb30d`
- Size: 64x64
- View: high top-down
- Cost: 20 gen

States (3 — 2 fired, 1 rate-limited):
| Name | State ID | Status |
|---|---|---|
| dormant | `294039a0-7207-4a05-805b-4376fce2baf0` | processing |
| pulse_1 | `23a2dce1-3ea2-40b7-9b4e-d68cd649bdfd` | processing |
| active | PENDING | rate limit hit (10/10 jobs) — retry after prior jobs complete |

**Retry command for active state:**
```
create_object_state(
  object_id="c88143c5-d926-473f-b7c0-d20cfcbeb30d",
  edit_description="Center glows strongly with cyan #4AF0FF and violet #6B3AFF energy, spilling along the entire radial crack pattern. Each fissure is now a glowing line. Light spreads outward across nearby floor stone painting it with pale cyan #BEFFFF highlights. Small energy wisps rise from the center. Sprite reads as actively channeling rift energy through floor stone."
)
```

## Budget (Batch 2)

| Item | Cost |
|---|---|
| 3 base objects × 20 gen | 60 gen |
| 10 states fired (est. 10 gen each) | ~100 gen |
| 1 pending state (retry) | ~10 gen |
| **Batch 2 total** | ~170 gen |

Balance at batch start: 3800 / 5000 gen.
Estimated after all complete: ~3630 / 5000.
