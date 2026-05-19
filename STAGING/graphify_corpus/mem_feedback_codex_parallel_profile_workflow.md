---
name: codex-parallel-profile-workflow
description: cx_dispatch.py multi-profile parallel — 3 active ChatGPT profiles (laurethgame/laurethayday/yasinderyabilgin) can run concurrent Codex dispatches; assign imagegen/refactor/Unity-MCP to separate profiles
metadata: 
  node_type: memory
  type: feedback
  originSessionId: a12da79a-6b77-423a-8b7c-59af8ccea2f8
---

# Codex Parallel Profile Workflow

User feedback (2026-05-18): "codex için farklı profilleri aynı anda çalıştırabilirsin cx ile biriyle imagegen yapıp biriyle review ya da biriyle unity mcp ile işlem yaptırabilirsin"

## Available profiles (logged in)

| Profile | Email | Suggested role |
|---|---|---|
| `laurethgame` | laurethgame@gmail.com | Imagegen (heavy asset production) |
| `laurethayday` | laurethayday@gmail.com | Refactor / spec implementation |
| `yasinderyabilgin` | yasinderyabilgin@gmail.com | Unity MCP integration / review |

(`ydbilgin` not logged in, ignore)

List via `powershell -NoProfile -Command "& cx accounts"` or `python cx_dispatch.py --help`.

## How to dispatch parallel

```bash
# Profile A — imagegen
python cx_dispatch.py --task-file STAGING/imagegen_task.md --profile laurethgame --effort high --timeout 3000

# Profile B — refactor (parallel)
python cx_dispatch.py --task-file STAGING/refactor_task.md --profile laurethayday --effort xhigh --timeout 3000

# Profile C — Unity integration (parallel)
python cx_dispatch.py --task-file STAGING/unity_integration_task.md --profile yasinderyabilgin --effort medium --timeout 1800
```

Each profile runs independent codex CLI session. No collision.

## Why this matters

- Single-profile sequential = 3× slower for 3 independent tasks
- Profile collision risk (laurethgame double-dispatch) → output file overwrites, partial completion
- User has 3 logged-in profiles → 3× throughput available

## Rule

Whenever 2+ independent Codex tasks need to run, assign different `--profile` flags. If 2 tasks share dependency (e.g., both touch same file), serialize via same profile or wait gate.

## Cross-references

- [[brush-tool-v1]] — past sprint dispatches used laurethgame only (single-profile bottleneck)
- [[research-on-block]] — parallel agents principle generalizes
