---
description: RIMA Game Design Knowledge Base sorgusu — NotebookLM üzerinden tüm tasarım kararlarına eriş.
allowed-tools: Bash
---

# /nlm [soru] — NotebookLM Knowledge Query

Run the command below and return its output verbatim. Do not add commentary.

```bash
uvx --from notebooklm-mcp-cli nlm notebook query $(cat .claude/nlm.local 2>/dev/null) "$ARGUMENTS" 2>&1
```

**Usage:** `/nlm warblade skill stagger nasıl tetiklenir?`
