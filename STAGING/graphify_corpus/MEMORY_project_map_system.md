---
name: rima-dungeon-map-system
description: "**[STALE 2026-05-18]** STS2-style DungeonMapUI.cs. DungeonGraph logic."
metadata: 
  node_type: memory
  type: project
  originSessionId: 353df54c-5237-4e81-8099-e1b1b26ca443
---

> **STALE 2026-05-18 S91 — superseded by [[map-plan-v1-lock]] + Codex evidence (DungeonGraph.cs):** Live runtime = **12 fixed nodes + up to 2 optional fork** (NOT "12+ nodes, 3 forks" as written here). DungeonMapUI.cs LIVE (audit not implement). MapFragment.cs LIVE. Map Fragment smart spawn rates LIVE'da farkli — `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs:653-669` baseline.

# DUNGEON GRAPH (STS2 MODEL)
* Structure: 12+ nodes, 3 forks (L/R/W)
* Fork Prob: 40% W (Fork 1), 25% W (Fork 3)
* Sequence: Start > Warmup > Fork 1 (3) > Merge > Fork 2 (2) > Elite > Fork 3 (3) > Pre-boss > Boss

# VISIBILITY STATES
* visited: Checked icon
* current: Pulsing icon
* step1: Revealed (icon visible)
* step2: Dark "?" icon
* missed: Unselected branches (dimmed to 40% brightness)

# FRAGMENTS SPAWN (SMART)
* Blind: 100%
* 1-step away: 50%
* 2+ steps: 10%

# UI & RUNTIME
* Input: M key toggle (DungeonMapUI.cs)
* Reward Cleanup: Purge activeReward list on StartRoom
