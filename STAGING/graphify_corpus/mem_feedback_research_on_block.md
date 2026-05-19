---
name: research-on-block-fallback
description: "When stuck on a design/implementation question during autonomous work, research before guessing. Codex/Gemini/NLM/web are all available."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 6463930c-ee28-4abe-b2e6-2c17db7c8cd5
---

# Research-on-Block Fallback (S85 user instruction)

When working autonomously and you hit a question that requires opinion or non-obvious knowledge ("how do other people do this?"), do NOT guess and do NOT pause indefinitely. Instead:

1. **Ask the question explicitly** — in your own head, "what is the unknown? what would experienced devs do?"
2. **Pick the right research channel:**
   - **Codex** (cx_dispatch.py) — for code patterns, library usage, specific implementation idioms
   - **Gemini** (rima-research agent with `gemini -p`) — for design philosophy research, comparing approaches, video synthesis, broad web sweeps
   - **NLM** (notebooklm-mcp-cli) — for RIMA project history; what we've already decided
   - **WebFetch / WebSearch** — for specific docs (Unity API, package docs, blog posts)
3. **Bound the research:** one question per dispatch, short prompt, ask for a specific answer not a survey.
4. **Apply the answer + cite the source** in the file / commit message / status update.

## Why
Past sessions where Claude guessed instead of researching produced rework. The cost of one Gemini query (~10s) is way lower than refactoring after a wrong assumption.

## How to apply
- During autonomous sprint work, when a non-mechanical decision arises (e.g., "should soft alpha shader use multi-sample or single-pass?"), dispatch Gemini before writing code.
- For Unity-specific gotchas, prefer Codex (it understands the codebase context) over Gemini.
- For "what would Hades / Death's Door / Dead Cells do?" prefer Gemini web research.
- If NotebookLM Karar # database has the answer (project decisions already made), prefer NLM.

Related: [[brush-tool-v1-design]], [[karar-143-pipeline]].
