---
name: Local model routing (ollama)
description: Which local model to use for which task. Covers all 6 installed models with task-level routing.
type: feedback
---

## Installed Models

| Model | Size | Vision | Notes |
|---|---|---|---|
| gemma3:4b | 3.3 GB | Unknown | Fastest; likely text-only at this size |
| gemma4:e4b | 9.6 GB | NO | Edge 4-bit quantized -- stripped capabilities, text only |
| gemma4:26b | 17 GB | YES | Fits in user's GPU. High quality vision + text |
| qwen2.5:14b | 9 GB | NO | Strong coder/reasoner, text only |
| qwen2.5vl:7b | 6 GB | YES | Dedicated Vision-Language model, fast |
| deepseek-r1:14b | 9 GB | NO | Chain-of-thought reasoning, text only |

## Task Routing

**Image / Frame analysis (screenshots, video frames, sprites):**
- Fast, good enough: `qwen2.5vl:7b` (purpose-built, 6GB)
- High quality / complex scene: `gemma4:26b`
- NEVER use: gemma4:e4b, qwen2.5:14b, deepseek-r1:14b (no vision)

**YouTube transcript / text synthesis / research summary:**
- Best: `gemma4:26b`
- Alternative: `qwen2.5:14b`
- Quick/cheap: `gemma4:e4b`

**Code generation / code review / Unity C# tasks:**
- Best: `qwen2.5:14b` (strong coder)
- Alternative: `gemma4:26b`

**Deep reasoning / multi-step analysis / design decisions:**
- Best: `deepseek-r1:14b` (chain-of-thought)
- Alternative: `gemma4:26b`

**Quick text tasks (summarize, format, translate):**
- Best: `gemma4:e4b` or `gemma3:4b` (fastest, lowest VRAM)

**PixelLab research synthesis (text + image mix):**
- Text docs: `gemma4:26b`
- Frame captures: `qwen2.5vl:7b`

## Rules

**Why:** gemma4:e4b = edge 4-bit quantized = text-only (vision stripped). qwen2.5vl:7b = dedicated VL model. gemma4:26b = full multimodal, fits user's GPU.

**How to apply:**
- Vision task? -> qwen2.5vl:7b (default) or gemma4:26b (quality)
- Coding? -> qwen2.5:14b
- Reasoning/design? -> deepseek-r1:14b
- Text, fast? -> gemma4:e4b
- Best overall quality (text or vision)? -> gemma4:26b
- gemma4:26b: ALWAYS ask user before running. User must close Unity/games/browsers first. Never run silently.
- qwen2.5vl:7b, gemma4:e4b, gemma3:4b: safe to run without asking
