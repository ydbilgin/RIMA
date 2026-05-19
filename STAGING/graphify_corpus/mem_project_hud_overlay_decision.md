---
name: HUD Overlay 2-Layer Decision
description: LOCKED UI architecture — 3 layers (combat HUD, TAB quick overlay, ESC pause menu). No full-screen build panel during gameplay.
type: project
originSessionId: 7fd24c1a-41d2-407d-8929-dc84b69eb65e
---
## 2026-05-05: UI Overlay Architecture Decision

**Problem:** Existing "Run Codex Build Overlay" concept covers ~70% of screen, too heavy for action gameplay. Genre peers (Hades, Vampire Survivors, StS) use minimal in-combat UI.

**Decision: 3-Layer UI System (APPROVED)**

### Layer 1: Combat HUD (always visible)
- Top-left: HP bar + Resource (Fury/Mana/etc)
- Bottom-center: 6 skill slots (1-5 + Z/ult)
- Top-right: Minimap (3-node lookahead)
- Bottom-left: Active buff/debuff icons (small, max 4)

### Layer 2: Quick Overlay (TAB toggle, game SLOWS but doesn't pause)
- Right ~40% of screen (Hades boon list style)
- Content: Equipped skills (icon + name + tag), Passive echoes (icon + count), Tag synergy progress
- Semi-transparent background, world still visible
- Instant dismiss on release

### Layer 3: Full Pause Menu (ESC)
- Full detail: skill descriptions, build identity, run map, settings
- Rarely opened, can be heavy

**Why:** RIMA is action-first. Player should never lose sight of combat for build info. Quick glance (TAB) covers 90% of "what do I have?" without breaking flow.

**Primary reference:** Hades / Hades 2 UI. NOT Slay the Spire (card game, totally different genre). StS only contributes the 3-choice draft reward pattern — nothing else from it applies to RIMA's HUD/overlay.

**How to apply:** Next session — generate Codex wireframe mockup for TAB overlay in pixel art style (Hades boon list as model), then implement piece by piece in Unity. PixelLab for final icon/frame assets if needed.

**Supersedes:** The heavy "Run Codex Build Overlay" concept is now a REFERENCE ONLY (not target UI). character_menu_concept.png is REJECTED (equipment grid violates RIMA rules).
