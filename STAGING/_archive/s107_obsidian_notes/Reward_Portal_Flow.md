# Reward + Portal Flow (S107 Research Phase)

**Status:** Research locked, implementation phased
**Related:** [[Cliff_System]] [[Walkability_Dash]] [[Open_Decisions]]

## NLM Canonical (existing design)

NLM notebook (`30ddffa5-292f-4248-8e77-68074af901be`) already canonicalizes Map Fragment + Skill Draft as the long-term reward primitives. Current MVP defers these to keep scope small.

## Phased Plan

### Phase 1 — Pattern C MVP (next implement)
- Use existing `SkillOfferGenerator` (in `Assets/Scripts/`)
- Single portal at room exit awards a Skill Draft choice
- Minimal UI — pick-up + 3-choice popup
- Sonnet dispatch ready to wire up

### Phase 2 — Tarz 2 (3 portal fan layout)
- 3 portals fan-laid at room exit edge
- `FanLayoutSolver.cs` already exists (`Assets/Scripts/Environment/`)
- Each portal carries a `RoomTypeData.cs` reference (different next-room types: combat / elite / treasure)
- No preview — portal icon hints room type

### Phase 3 — Pattern D (portal + preview)
- Inpaint room-type icons onto portal sprites
- Preview thumbnail on hover/proximity
- Full canonical flow: choose by preview before committing

## Code Inventory (already in place)

- `Assets/Scripts/Environment/Portal.cs` — portal entity
- `Assets/Scripts/Environment/PortalSpawnController.cs` — spawn manager
- `Assets/Scripts/Environment/PortalSpawnAnchor.cs` — placement anchor (needs reachability check)
- `Assets/Scripts/Environment/FanLayoutSolver.cs` — fan geometry
- `Assets/Scripts/Environment/RoomTypeData.cs` — room type ScriptableObject

## Visual: Yarık Portal

- Vertical rift sprite, 64×128 dikey
- 4 idle frames (animate via PixelLab Create Object MCP or S-XL Web UI)
- Room-type icons inpainted onto each portal variant (Phase 3)
- Cyan #00FFCC consistent with [[Cliff_System]] cyan_glow accent

## Hard Constraint

**Reachability** — `PortalSpawnAnchor` must validate the portal lands in a Player-reachable area (flood-fill from Player start position, intersect with WalkabilityMap). Currently NOT enforced — Sonnet dispatch queued. See [[Open_Decisions]].

## NLM Conflict

NLM canonical = Map Fragment + Skill Draft. Phase 1 ships only Skill Draft (existing generator). Map Fragment defers to Phase 2+. Resolution: phased adoption, not contradicting NLM — just sequencing it.
