---
name: RIMA SFX Pipeline
description: Stable Audio Open local workflow (RTX 5080).
type: project
---

# SETUP (RTX 5080)
* Model: stable-audio-model (15.7GB)
* Environment: conda stable-audio
* Tool: generate_rima_sfx.py
* Perf: ~9s per 2s sound (100 steps)
* License: Stability AI Community ( cap)

# WORKFLOW
1. Add prompts to generate_rima_sfx.py
2. Generate A/B/C variations
3. Listen > Select > Strip suffix > Import to Unity
4. Logic: PyTorch nightly cu128 + diffusers (Win torchaudio DLL fix)
