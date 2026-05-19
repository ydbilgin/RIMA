---
name: room-design-pipeline
description: "**[STALE 2026-05-18]** Layout architecture and generation workflow."
metadata: 
  node_type: memory
  type: project
  originSessionId: 353df54c-5237-4e81-8099-e1b1b26ca443
---

> **STALE 2026-05-18 S91 — superseded by [[map-plan-v1-lock]] + `TASARIM/dungeon_act1_map.md` + Codex evidence (DungeonGraph.cs LIVE):** Act 1 = 15 node (Karar #62 LOCKED), oda tipleri LIVE enum (Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor). 19 gun eski "Entry > Guard > Corridor > Ritual > Elite > Boss" akisi YANLIS. RoomTemplate library ([[map-plan-v1-lock]] §7) canonical.

# ARCHITECTURE (2026-04-16)
* Gate System: GateBehavior state machine (Hidden/Locked/Open)
* Dash Lanes: 2x lanes (8+ tiles clear area) per room
* Clearance: 3-tile gap mandatory for North gates
* Act 1 Flow: Entry > Guard > Corridor > Ritual > Elite > Boss

# WORKFLOW
1. Claude: ASCII Specification
2. Kiro: Unity Tilemap generation
3. Manual: Polish/Detail pass

# ROOM TYPES
* Open (Range-fav), Corridor (Melee-fav), Pillared (Assassin-fav), Mixed
* Target: 15 templates per Act
