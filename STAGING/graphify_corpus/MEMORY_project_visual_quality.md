---
name: rima-visual-standards
description: S86 Update - 64x64 native (Karar
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

> WARNING: DEPRECATED 2026-05-17. 128px references are STALE -- Karar #74 64x64 LIVE LOCK. Low Top-Down + Fractured Epic tone sections KEEP.

> **Active animation spec:** [[weaponless-animation-v1]] (Karar #144 -- silahsiz body + WeaponChild SR).
> Size lock: 64x64 native (Karar #74, 2026-05-12). 128px REVOKED.

# ART DIRECTION LOCK
* Style: Stylized ARPG (Hades-like), dramatic but readable
* Tone: Fractured Epic (Karar #30 + #77) -- Vivid Vulnerability, NOT grimdark despair, NOT cute roguelite
* Atmosphere: "Grimdark-lite" (NOT pure horror/black)
* Silhouette: Functional gear, worn materials, distinct class IDs

# SPRITE SPEC (S86 LOCK)
* Canvas: 64x64px native (Karar #74 LOCK) -- 128px REVOKED
* PPU: 64 (Unity) -- Karar #74
* View: Low Top-Down ~35 deg ARPG (Hades reference, Karar #40/#45/#100)
* Directions: 4-cardinal V1 (S/E/N/W); 8-dir V2 staged (Karar #88 trigger threshold)
* Anchor Pose: Natural idle, slight SW angle
* Elementalist Anchor: Use for Scale/Outline/Density/Readability (NOT color/mood)

# RENDERING
* Outline: Heavy single-tone dark
* Shading: Painterly weathered pixel
* Detail: Low-detailed (PixelLab mode)
* Material: Leather != Cloth != Skin (Readable separation)

# ANIMATION
* Run: 6 frames, 4-cardinal directions (W=flipX for symmetric classes)
* Attack/Skill: 3-segment KF + Interpolation
* Walk: REMOVED (Run only)
