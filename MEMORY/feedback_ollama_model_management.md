---
name: Ollama model management rule
description: When to unload vs keep loaded; must ask before using gemma4:26b
type: feedback
---

Do NOT unload gemma4:26b from GPU memory just because it is idle. Loaded models are fine to leave resident.

**Why:** gemma4:26b = 17GB, requires GPU to be free (Unity/games/browsers closed). Falls back to CPU/RAM when GPU is occupied -- works but slow. This makes it ideal for overnight long-running tasks.

**How to apply:**
- gemma4:26b daytime = ALWAYS ask user first; they must close Unity/games/browsers before running.
- gemma4:26b overnight = preferred window. If a task is long (many frames, multi-doc synthesis) AND quality gap vs smaller models is significant, proactively suggest: "Bunu gece gemma4:26b ile çalıştıralım, daha iyi sonuc alırız."
- CPU fallback = ollama automatically uses CPU/RAM when GPU is full. Slower but unattended overnight is fine.
- When to suggest overnight: YouTube video batch analysis, multi-chapter research synthesis, frame-by-frame analysis of 10+ videos, anything where gemma4:e4b or qwen2.5vl:7b would miss nuance.
- qwen2.5vl:7b, gemma4:e4b, gemma3:4b: safe to run anytime without asking.
- Only unload if user explicitly asks, or another model needs VRAM and user approves.
