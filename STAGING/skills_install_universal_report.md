# Skills.sh Universal Install Report

## Install Command Used

Official install command pattern from skills.sh:

```bash
npx skills add <repo-url> --skill <skill-name>
```

Commands executed:

```bash
npx skills add https://github.com/vercel-labs/skills --skill find-skills --global --agent claude-code codex --yes --copy
npx skills add https://github.com/agentspace-so/runcomfy-agent-skills --skill image-inpainting --global --agent claude-code codex --yes --copy
npx skills add https://github.com/agentspace-so/runcomfy-agent-skills --skill image-outpainting --global --agent claude-code codex --yes --copy
```

The Skills CLI installed Claude Code at `C:\Users\ydbil\.claude\skills\` and its Codex-compatible global target at `C:\Users\ydbil\.agents\skills\`. The three installed skill folders were then copied into the requested Codex global/profile roots:

- `C:\Users\ydbil\.codex\skills\`
- `C:\Users\ydbil\.codex-profiles\laurethayday\skills\`
- `C:\Users\ydbil\.codex-profiles\laurethgame\skills\`
- `C:\Users\ydbil\.codex-profiles\yasinderyabilgin\skills\`
- `C:\Users\ydbil\.codex-profiles\ydbilgin\skills\`

## find-skills

- Claude Code: SUCCESS - `C:\Users\ydbil\.claude\skills\find-skills\SKILL.md`
- Codex global: SUCCESS - `C:\Users\ydbil\.agents\skills\find-skills\SKILL.md` and `C:\Users\ydbil\.codex\skills\find-skills\SKILL.md`
- Codex per-profile laurethayday: SUCCESS - `C:\Users\ydbil\.codex-profiles\laurethayday\skills\find-skills\SKILL.md`
- Codex per-profile laurethgame: SUCCESS - `C:\Users\ydbil\.codex-profiles\laurethgame\skills\find-skills\SKILL.md`
- Codex per-profile yasinderyabilgin: SUCCESS - `C:\Users\ydbil\.codex-profiles\yasinderyabilgin\skills\find-skills\SKILL.md`
- Codex per-profile ydbilgin: SUCCESS - `C:\Users\ydbil\.codex-profiles\ydbilgin\skills\find-skills\SKILL.md`
- SKILL.md content summary: Discovers installable skills when users ask how to do a task, ask for a skill by domain, ask if a capability exists, or want to extend agent functionality. Uses `npx skills find`, `npx skills add`, leaderboard checks, and source/install-count quality checks.
- Dependencies: `npx skills`; network access to skills.sh/GitHub. No API key or MCP config required by the skill.

## image-inpainting

- Claude Code: SUCCESS - `C:\Users\ydbil\.claude\skills\image-inpainting\SKILL.md`
- Codex global: SUCCESS - `C:\Users\ydbil\.agents\skills\image-inpainting\SKILL.md` and `C:\Users\ydbil\.codex\skills\image-inpainting\SKILL.md`
- Codex per-profile laurethayday: SUCCESS - `C:\Users\ydbil\.codex-profiles\laurethayday\skills\image-inpainting\SKILL.md`
- Codex per-profile laurethgame: SUCCESS - `C:\Users\ydbil\.codex-profiles\laurethgame\skills\image-inpainting\SKILL.md`
- Codex per-profile yasinderyabilgin: SUCCESS - `C:\Users\ydbil\.codex-profiles\yasinderyabilgin\skills\image-inpainting\SKILL.md`
- Codex per-profile ydbilgin: SUCCESS - `C:\Users\ydbil\.codex-profiles\ydbilgin\skills\image-inpainting\SKILL.md`
- SKILL.md content summary: Mask-driven still-image inpainting through RunComfy. Default route is `tongyi-mai/z-image/turbo/inpainting` for masked edits; fallback routes include `google/nano-banana-2/edit`, `openai/gpt-image-2/edit`, and `blackforestlabs/flux-1-kontext/pro/edit` when a mask is not available.
- Dependencies: RunComfy CLI (`npm i -g @runcomfy/cli` or `npx -y @runcomfy/cli --version`) and RunComfy auth via `runcomfy login` or `RUNCOMFY_TOKEN`. `RUNCOMFY_TOKEN` was not set in this shell. No direct `OPENAI_API_KEY` requirement is listed by the skill.

## image-outpainting

- Claude Code: SUCCESS - `C:\Users\ydbil\.claude\skills\image-outpainting\SKILL.md`
- Codex global: SUCCESS - `C:\Users\ydbil\.agents\skills\image-outpainting\SKILL.md` and `C:\Users\ydbil\.codex\skills\image-outpainting\SKILL.md`
- Codex per-profile laurethayday: SUCCESS - `C:\Users\ydbil\.codex-profiles\laurethayday\skills\image-outpainting\SKILL.md`
- Codex per-profile laurethgame: SUCCESS - `C:\Users\ydbil\.codex-profiles\laurethgame\skills\image-outpainting\SKILL.md`
- Codex per-profile yasinderyabilgin: SUCCESS - `C:\Users\ydbil\.codex-profiles\yasinderyabilgin\skills\image-outpainting\SKILL.md`
- Codex per-profile ydbilgin: SUCCESS - `C:\Users\ydbil\.codex-profiles\ydbilgin\skills\image-outpainting\SKILL.md`
- SKILL.md content summary: Still-image outpainting through RunComfy. Extends canvas, uncrops, and changes aspect ratio while preserving original content. Default route is `google/nano-banana-2/edit`; alternatives include `openai/gpt-image-2/edit`, `blackforestlabs/flux-1-kontext/pro/edit`, and brand-specific edit endpoints.
- Dependencies: RunComfy CLI (`npm i -g @runcomfy/cli` or `npx -y @runcomfy/cli --version`) and RunComfy auth via `runcomfy login` or `RUNCOMFY_TOKEN`. `RUNCOMFY_TOKEN` was not set in this shell. No direct `OPENAI_API_KEY` requirement is listed by the skill.

## Compatibility Notes

- `find-skills` works in Claude Code as a discovery workflow for prompts like "how do I do X", "find a skill for X", "is there a skill that can...", and capability-extension requests. It recommends checking install count, source reputation, and GitHub stars before recommending or installing a skill.
- The official CLI registered all three new skills as global skills for both Claude Code and Codex, visible through `npx skills list --global --agent claude-code --json` and `npx skills list --global --agent codex --json`.
- The CLI's Codex global registry path is `C:\Users\ydbil\.agents\skills\`; requested legacy/profile Codex roots were also populated and verified.
- `image-inpainting` and `image-outpainting` depend on RunComfy, not a local OpenAI SDK. They require RunComfy CLI plus login/token before actual generation commands can run.
- Shell compatibility check found `RUNCOMFY_TOKEN` not set. `runcomfy` was not available as a local command in this shell.
- `npx skills find image-inpainting` and `npx skills find image-outpainting` returned no CLI search results in this shell, but skills.sh pages exposed the official install commands and both installs completed successfully.

## Next Step Onerisi

- Test `find-skills` in Claude Code with: `Find a skill for sprite inpainting workflows and recommend the safest install option.`
- For RIMA wall iteration, use `image-inpainting` when a specific wall cell or masked wall area needs local repair. Prepare a source image URL, a grayscale mask URL where white is the editable region, and a prompt that names what must stay unchanged.
- Use `image-outpainting` when a wall or wall-plus-floor image needs canvas expansion before separating usable asset regions. Set the desired aspect ratio explicitly and include preservation language for the original visible wall/floor.
