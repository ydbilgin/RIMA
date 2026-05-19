---
name: animation-production-notes
description: "Spec for Run (6f), Idle (4f), and Attack (3-seg). Updated S86 for 64x64 + 4-cardinal V1."
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

> WARNING: DEPRECATED 2026-05-17. See [[weaponless-animation-v1]] (Karar #144) + [[pixellab-character-states-workflow]] (Karar #145). 128px->64x64 done, 8-dir->4-cardinal V1.

> **Active animation spec:** [[weaponless-animation-v1]] (Karar #144 — silahsiz body + WeaponChild SR).
> Direction lock: 4-cardinal V1 (S/E/N/W), 8-dir V2 (Karar #53 + Karar #114 staged, Karar #88 trigger threshold).

* **Tool/Frame Locks:**
  - **Run:** V3 + Enhance Action (6f, 4-cardinal, No flips — W=flipX for symmetric classes).
  - **Idle:** V3 + Enhance, KeepFirst ON (4f).
  - **Attack/Multi-phase:** 3-seg KF+Interp (3f source).
  - **Hit/Death:** V3 (3f / 6-8f).
* **Locomotion:** Run only (No Walk).
* **Animator (PlayerAnimator.cs):** 2D Simple Directional Blend Tree. Exit Time OFF (0.05s). `run_stop` state mandatory.
* **Timing:** Impact snap at 40ms; other frames 80-100ms.
* **Size:** 64x64px (PPU=64). Karar #74 LOCK — 128x128 REVOKED.
