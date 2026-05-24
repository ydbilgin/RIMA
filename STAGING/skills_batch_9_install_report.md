# Skills Batch 9 Install + Agent Sprite Forge Report

## Install Results
| # | Skill | Source (owner/repo) | Claude Code | Codex Global | Per-profile (4) | Status |
|---|---|---|---|---|---|---|
| 1 | subagent-driven-development | obra/superpowers | OK | OK | OK | PASS |
| 2 | write-a-skill | mattpocock/skills | OK | OK | OK | PASS |
| 3 | skill-creator | anthropics/skills | OK | OK | OK | PASS |
| 4 | systematic-debugging | obra/superpowers | OK | OK | OK | PASS |
| 5 | brainstorming | obra/superpowers | OK | OK | OK | PASS |
| 6 | clarity (installed as impeccable) | pbakaus/impeccable | OK | OK | OK | PASS_ALT |
| 7 | verification-before-completion | obra/superpowers | OK | OK | OK | PASS |
| 8 | gpt-image-2 | agentspace-so/agent-skills | OK | OK | OK | PASS |
| 9 | flux-2-klein | agentspace-so/runcomfy-agent-skills | OK | OK | OK | PASS |

## Install Notes
- Official install command used for each package: `npx skills add <owner/repo> -g -y --copy -a claude-code codex -s <skill>`.
- The official installer placed shared Codex/Claude skills under `C:\Users\ydbil\.agents\skills\` and copied Claude Code entries under `C:\Users\ydbil\.claude\skills\`.
- Codex global and per-profile copies were verified under `C:\Users\ydbil\.codex\skills\` and all four profile skill folders: `laurethayday`, `laurethgame`, `yasinderyabilgin`, `ydbilgin`.
- Exact `clarity` was not found. Tried `clarity` and `clarify` against `pbakaus/impeccable`; both failed. Installed `impeccable` as the closest official alternative because its trigger text includes clarify/distill/harden/optimize UI work.
- Failed exact/alternate attempts count: 1 skill name family (`clarity` / `clarify`). Not blocked.

## Agent Sprite Forge Investigation
- Type: skill repository / standalone GitHub repo
- Source: https://github.com/0x0funky/agent-sprite-forge
- Skills found by `npx skills add 0x0funky/agent-sprite-forge -l --full-depth`:
  - `generate2dmap`
  - `generate2dsprite`
- `npx skills find "sprite forge"`: no registry search result.
- GitHub search found primary repo `0x0funky/agent-sprite-forge` plus forks/adaptations.
- Web search found the npaka123 X post: https://x.com/npaka123/status/2058326600396206406
- README summary: Codex skills for game-ready 2D sprites, layered maps, local cleanup, and engine handoff to Godot/Unity/raw 2D workflows. It uses built-in image generation for raw visuals and local Python processors for chroma-key cleanup, frame splitting, alignment, GIF/PNG export, and map/prototype metadata.
- RIMA relevance: high. RIMA is a 2D roguelite with active sprite, room, wall, map, and Unity pipeline work. `generate2dsprite` and `generate2dmap` are directly relevant for prototype-scale asset generation, sprite-sheet cleanup, map prop extraction, and Unity/Godot-style handoff evaluation.
- Install command if approved:
  - `npx skills add 0x0funky/agent-sprite-forge -g -y --copy -a claude-code codex -s generate2dsprite`
  - `npx skills add 0x0funky/agent-sprite-forge -g -y --copy -a claude-code codex -s generate2dmap`
- Recommendation: investigate further. Run a sandbox comparison against the existing RIMA PixelLab/imagegen pipeline before making it a production default.

## Next Step
- Custom `$rima-conventions` skill writing can use `skill-creator` now.
- New image generation skills can be tested: `gpt-image-2` vs existing `$imagegen`, and `flux-2-klein` vs current PixelLab/imagegen flow.
- Agent Sprite Forge is a strong candidate for a controlled prototype test, especially sprite-sheet cleanup and layered map extraction.
