---
name: pixellab-v3-ui
description: "PixelLab web UI Custom Animation V3 + Custom Frames. MCP'de YOK — user manuel üretir, Claude prompt sağlar."
metadata: 
  node_type: memory
  type: reference
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# PixelLab Custom Animation V3 — WEB UI ONLY

**MCP DURUMU (2026-05-16 confirmed):** V3 Custom Animation ve Custom Frames özellikleri **MCP'de EXPOSE EDİLMEMİŞ**. `animate_character` sadece template_animation_id veya text-based `action_description` kabul ediyor — keyframe slot YOK, "Keep First Frame" toggle YOK, 220×220 resolution kontrolü YOK.

**Workflow ayrımı (S86 LOCK):**
- **Karakterler** → user PixelLab web UI'da V3 ile manuel üretir, Claude prompt formülünü sağlar
- **Object/decal/tile** → MCP `create_object` / `create_tiles_pro` ile dispatch edilebilir
- **Character pro mode** (`create_character mode=pro`) → KULLANMA. User karakter işlerini web UI V3'te kendi yapıyor. Bkz: [[pixellab-character-via-web-ui-v3]]

# PRESET ANIMATIONS (web UI)
* Movement: Idle, Jumping, Running, Walking, Backflip (BETA)
* Movement (BETA): Crouching, Front Flip, Getting Up, Slide
* Combat: Kicking, Punching, Reactions, Fireball (BETA)
* Interactions: Drinking, Picking Up, Pull/Push/Throw Object
* Custom: Custom Animation V3 (BETA, Primary for RIMA)

# CUSTOM ANIMATION V3 SPECS (web UI)
* Frame Count: 4 - 10 (Default: 8)
* Keep First Frame: ON (Idle loops), OFF (Run/Attack transitions)
* Cost: 104 generations total (8 directions)
* Resolution: 220x220px (Resize to 128px via Codex/Script)
* Enhance Action with AI: ENABLED (Preserves identity from ref sprite)

# CUSTOM FRAMES (ADVANCED, web UI)
* Start Frame Slot: Pick frame A from gallery
* End Frame Slot: Pick frame B from gallery
* Logic: A -> [AI Gen] -> B (Single-step interpolation)

# See Also
- [[pixellab-tool-inventory]] — Live MCP endpoint list (2026-05-16)
- [[pixellab-create-character-workflow]] — Web UI Ref Standard workflow
