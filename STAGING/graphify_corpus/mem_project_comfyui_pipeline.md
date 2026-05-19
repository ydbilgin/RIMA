---
name: ComfyUI Local Pipeline
description: Local FLUX generation setup for concept art.
type: project
---
* **Hardware:** RTX 5080, PyTorch 2.11+cu128.
* **Software:** ComfyUI v0.19.1, rembg[gpu].
* **Scripts:** `rima_batch.py`, `rima_generate.py`, `rima_cleanup.py` (watermark removal).
* **Config:** 832x1536 canvas, #CCCCCC BG.
* **Constraint:** Negative prompt: `pixel art, pixelated, 8-bit, 16-bit` (MANDATORY).
