#!/usr/bin/env bash
PROMPT=$(cat "/f/Antigravity Projeler/2d roguelite/RIMA/.claude/codex_roomdesigner_f1_compact.txt")
"/c/Users/ydbil/AppData/Roaming/npm/cx.cmd" run laurethgame exec \
  --skip-git-repo-check \
  --color never \
  --dangerously-bypass-approvals-and-sandbox \
  --config model_reasoning_effort=high \
  "$PROMPT" < /dev/null 2>&1
