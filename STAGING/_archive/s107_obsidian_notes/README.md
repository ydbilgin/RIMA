# RIMA Obsidian Notes (S107)

> Generated 2026-05-26, S107 overnight close.
>
> **Vault path (S108 detected):** `F:\Antigravity Projeler\2d roguelite\RIMA` — the project root IS the Obsidian vault (`.obsidian/` config present, vault `e77bb3ba3f42dc9e` registered in `C:\Users\ydbil\AppData\Roaming\obsidian\obsidian.json`, opened 2026-05-26). Notes are already inside the vault at `STAGING/s107_obsidian_notes/` and resolve via Obsidian's wiki-links — no relocation needed.

## Project Summary

**RIMA** = 2D top-down roguelite (Hades-style Elysium aesthetic), pure top-down ~85-90 degree camera, 3/4 sprite styling. ARPG with class system, room-based procgen, reward+portal flow.

**Studio context:** RIMA is one game within LaurethStudio scope. Cross-cutting research goes to both repos.

## Note Map

- [[Cliff_System]] — 3-direction cliff renderer (S, SE, SW), 262 tiles, PPU 64 native (S108: S+SE+E, 187 tiles, hybrid A+B flood-fill)
- [[Walkability_Dash]] — Tilemap-based walkability + VoidBlocker + Dash gap-jump
- [[Reward_Portal_Flow]] — Pattern C MVP → Tarz 2 (3 portal) → D (portal+preview)
- [[S107_Overnight_Log]] — S107 autonomous timeline
- [[Open_Decisions]] — Sabah user kararları beklemede
- [[S108_Cleanup]] — Root + STAGING bulk archive + NLM/Graphify/Obsidian rebuild log

## Key Paths

- Scene: `Assets/Scenes/Test/PlayableArena.unity`
- Cliff sprites: `Assets/Sprites/Environment/KitB_Cliff/` (9 PNG, PPU 64, top-center pivot)
- Cliff code: `Assets/Scripts/Environment/CliffAutoPlacer.cs` + `CliffPlacementRules.cs` + `DeterministicVariantTile.cs`
- Walkability: `Assets/Scripts/Environment/WalkabilityMap.cs` + `IObstacle.cs`
- Player: `Assets/Scripts/Player/PlayerController.cs`
- Portal stack: `Assets/Scripts/Environment/Portal.cs` + `PortalSpawnController.cs` + `PortalSpawnAnchor.cs` + `FanLayoutSolver.cs` + `RoomTypeData.cs`

## Active Locks (Top-Level)

- V1 wall-less Hades Elysium LOCKED (V2 walls legacy only)
- 3-Kit BG architecture (A=floor / B=cliff / C=parallax bg)
- High top-down 3/4 sprite (true iso diamond REVOKED 2026-05-24)
- PPU 64 native (Pixel Perfect Camera DISABLED)
