---
description: RIMA Game Design Knowledge Base sorgusu — NotebookLM üzerinden tüm tasarım kararlarına eriş.
allowed-tools: Bash
---

# /nlm [soru] — NotebookLM Knowledge Query

Run the command below and return its output verbatim. Do not add commentary.

```bash
NB="${NLM_NOTEBOOK_ID:-30ddffa5-292f-4248-8e77-68074af901be}"
uvx --from notebooklm-mcp-cli nlm notebook query "$NB" "$ARGUMENTS" 2>&1
```

**Usage:** `/nlm warblade skill stagger nasıl tetiklenir?`
