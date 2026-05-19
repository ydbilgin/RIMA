---
name: RIMA SFX v2 Rules
description: Stable Audio v2 improvements and terminology.
type: project
---

# SCRIPT & RUN
* Path: generate_rima_sfx_v2.py
* CMD: conda run -n stable-audio python generate_rima_sfx_v2.py

# IMPROVEMENTS (v2)
* Guidance Scale: 10 (Up from 7)
* Terminology: Use transient, resonance, onset, decay (Avoid genre tags)
* Negatives: 14 specific words (e.g., music)
* Seeds: 42, 1337, 9999
* Format: [Features] > [Context] > [Genre] > "no music"
* Hardware: RTX 5080, steps=100
