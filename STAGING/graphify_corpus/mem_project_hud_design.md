---
name: RIMA HUD Design
description: Approved layout, UI decisions, prompt paths.
type: project
---

# HUD LAYOUT (2026-04-12)
* Top Right: Minimap (12 deg, 60% opacity, parchment)
* Top Left: Room counter (1 line, low opacity)
* Bottom Left: HP bar (thin), Resource (class-specific), LMB/RMB (M1/M2)
* Bottom Center: Skill slots 1-6 (38x38px, dark iron)
* Bottom Right: Gold + Rift Shards (watermark)

# RESOURCE BARS
| Class | Resource | Hex/Color |
|-------|----------|-----------|
| Warblade | Rage | #4AADFF |
| Elementalist | Mana | Purple |
| Shadowblade | Energy | Void green |
| Ranger | Focus | Amber |
| Ronin | Ki | Orange |
| Gunslinger | Heat | Red-orange |
| Ravager | Fury | Dark red |
| Brawler | Stamina | Yellow-green |
| Summoner | Soul | Pale purple |
| Hexer | Curse | Bile green |

# CHARACTER PAGE (TAB)
* Panel: Slide-in L (380px), 75% opacity
* Logic: Non-blocking (Time.timeScale = 1)
* Input: TAB (B key REMOVED)

# DRAFT SCREEN
* Cards: 3x (200x280px), tier banner
* VFX: God-ray + rift wisps

# VISUAL TARGET
* Background: character_menu_concept.png
* Environment: Mossy cobblestone, stone blocks, high contrast shadows
* Lighting: Torch amber (Primary), Rift blue (Secondary)

# TECHNICAL
* HP Bar: 4px strip, 3s fade on full
* HUD Editor: HUDEditorManager.cs (ESC+H, 10px snap), HUDElement.cs
* Save: Application.persistentDataPath/hud_layout.json
* Prompt: GEMINI_UI_PROMPT.md
