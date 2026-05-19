---
name: 128px-pivot-s43-deprecated
description: DEPRECATED 2026-05-16 (S86). Original 128px resolution decision -- overridden by Karar
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

> WARNING: DEPRECATED 2026-05-17. Pivot resolved -- Karar #74 64x64 LIVE LOCK. This memory is historical only.
>
> **STATUS: DEPRECATED 2026-05-16 (S86)**
> **Reason:** Karar #74 (2026-05-12) overrode 128px -> 64x64 chibi. PROJECT_RULES.md S59 LOCK confirms 64x64.
> **Active spec:** [[weaponless-animation-v1]] for 64x64 + 4-cardinal V1 animation. See also Karar #72, #73, #74 in MASTER_KARAR_BELGESI.md.
> This file is kept for cross-reference history only. Do NOT use values below for production decisions.

---

### HISTORICAL RECORD (S43, pre-Karar #74)

* **LOCKED DECISION (2026-04-27):** 128px target for characters, tiles, and assets.
* **Why:**
  - 64px bandwidth is too low for S-XL identity/details.
  - 128px allows 7-head mature proportions (18px/head).
  - Matches Cursemark/Last Epoch style zone.
  - Pivot cost is zero as no final assets were produced in 64px.
* **Spec Changes:**
  - **PPU:** 128.
  - **Tiles:** Ground (128x64), Wall (128x192).
  - **Camera Ortho:** ~1.25-1.5.
  - **PixelLab:** 128 native generation (No upscaling).
* **Matrix (128px baseline):**
  - **Player:** 128 (1.0x).
  - **Imp:** 80-96 (0.7x).
  - **Warden/Caster:** 128-144 (1.1x).
  - **Shard Walker:** 160-176 (1.4x).
  - **Seam Crawler:** 200x96 wide (2.0x).
  - **Bosses:** 256-384 (2-3x).
