---
name: feedback_memory_system
type: feedback
trigger: memory, memory guncelle, shared memory, MASTER_KARAR, CURRENT_STATUS
description: Shared project memory update rule; never write Claude local auto-memory
---

When the user says "memory guncelle" (update memory), update **shared memory only**:

1. Design/locked decisions -> `TASARIM/MASTER_KARAR_BELGESI.md` (new numbered entry)
2. Workflow/process rules -> `RIMA/MEMORY/<topic>.md` + update `RIMA/MEMORY/INDEX.md`
3. Update `CURRENT_STATUS.md` if session state changed

**Never** write to Claude local auto-memory (`C:/Users/ydbil/.ccs/...`) for project content.

**Why:** All agents (Codex, Gemini, Claude) read from RIMA/MEMORY/ and TASARIM/. Local auto-memory is invisible to other agents.

**How to apply:** At any point when user says "memory guncelle" or after a decision cycle -- update shared files above, not local memory system.
