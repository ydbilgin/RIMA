# RIMA Memory Index
All agents read this file first. Open only files relevant to the current task.

## Memory Files
- [feedback_agent_delegation.md](feedback_agent_delegation.md) -- agent role boundaries and PlayMode ownership.
- [feedback_encoding.md](feedback_encoding.md) -- ASCII-only markdown rule.
- [feedback_git_attribution.md](feedback_git_attribution.md) -- commit attribution by agent.
- [feedback_mcp_unity.md](feedback_mcp_unity.md) -- Unity MCP compile waits, test syntax, stuck-job recovery.
- [feedback_pixellab_direction.md](feedback_pixellab_direction.md) -- S43 PixelLab direction offset and remap rule.
- [feedback_temp_files.md](feedback_temp_files.md) -- temporary report/prompt cleanup rule.

## Adding New Memory
Create `MEMORY/<topic>.md` with frontmatter, then add one entry here.

## Not Memory
- Active task state: CURRENT_STATUS.md
- Architecture: SYSTEM_MAP.md
- Locked decisions: MASTER_KARAR_BELGESI.md
- Static agent rules: CODEX.md / ANTIGRAVITY.md / CLAUDE.md
- Completed tasks: ARCHIVE/
