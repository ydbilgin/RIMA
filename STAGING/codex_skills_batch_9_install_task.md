ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — 9 Skill Batch Install + Agent Sprite Forge Investigate

**Amaç:** RIMA orchestrator için 9 skill install (Claude Code + tüm Codex profile'lara) + Agent Sprite Forge (yeni keşif) skill aratma + install candidate raporu.

## 9 Skill Install Listesi

### Priority Top 7
1. **subagent-driven-development** — Multi-agent koordinasyon (RIMA pattern)
2. **write-a-skill** — Custom skill yazımı
3. **skill-creator** — Skill development framework
4. **systematic-debugging** — Codex/Unity error fix
5. **brainstorming** — Design ideation
6. **clarity** — Concept clarification
7. **verification-before-completion** — Codex çıktı QC

### Image Alternatives
8. **gpt-image-2** — Image gen alternative
9. **flux-2-klein** — Flux 2 cloud

## Install Procedure

Her skill için:
```bash
npx skills add <owner/repo>@<skill-name> -g -y
```

skills.sh'ten exact `owner/repo` bul:
- subagent-driven-development: muhtemelen `anthropics/skills` veya `vercel-labs/agent-skills`
- write-a-skill, skill-creator: aynı anthropics/skills
- systematic-debugging: vercel-labs veya impeccable/skills
- brainstorming, clarity, verification-before-completion: impeccable koleksiyonu
- gpt-image-2, flux-2-klein: image-inpainting ile aynı org (runcomfy veya benzer)

Install location: global `~/.codex/skills/` ve `~/.claude/skills/` (her ikisi).

## Test Protokolü

Her install sonrası:
1. Skill SKILL.md readable mı (`ls ~/.codex/skills/<name>/SKILL.md`)
2. Per-profile copy: 4 profile'a (laurethayday, laurethgame, yasinderyabilgin, ydbilgin)
3. Claude Code skill folder copy: `~/.claude/skills/<name>/`

## Agent Sprite Forge Araştırma

Tweet 1 (npaka123, May 23 2026) "Agent Sprite Forge" tool'undan bahsediyor:
> Codex + Agent Sprite Forge ile RPG town screen oluşturmayı deniyor. 3D harita + billboard karakterler (GRANDIA tarzı).

Bu skill mi yoksa standalone tool mu araştır:
- `npx skills find "sprite forge"`
- Web search: "Agent Sprite Forge npaka123" veya "Agent Sprite Forge Codex"
- GitHub search: agent-sprite-forge
- Eğer skills.sh'te varsa: install candidate olarak raporla
- Eğer standalone repo ise: README link + RIMA için relevance assessment

## Çıktı

`STAGING/skills_batch_9_install_report.md`:

```markdown
# Skills Batch 9 Install + Agent Sprite Forge Report

## Install Results
| # | Skill | Source (owner/repo) | Claude Code | Codex Global | Per-profile (4) | Status |
| 1 | subagent-driven-development | ... | ✅/❌ | ✅/❌ | ✅/❌ | PASS/FAIL |
| ... |

## Agent Sprite Forge Investigation
- Type: skill / standalone tool / unclear
- Source: [URL]
- RIMA relevance: high/medium/low + sebep
- Install command (if available): ...
- Recommendation: install / investigate further / skip

## Next Step
- Custom $rima-conventions skill yazımı için skill-creator hazır
- Yeni image gen skills test edilebilir (gpt-image-2 vs $imagegen karşılaştırma)
```

## Hard Constraints

- Sadece skills.sh resmi `npx skills add` command kullan
- API key arama (ÖZELLİKLE image-gen skill'leri için, $imagegen built-in tool mode rule applies)
- Eğer skill not found: alternative name dene (örn. systematic-debugging → debug, debugging)
- Per-profile install OPTIONAL — Codex global yeterli olabilir, Claude Code zorunlu
- BLOCKED ver eğer 5+ skill install fail ederse

## Git Commit

```bash
git add STAGING/skills_batch_9_install_report.md
git commit -m "[Codex] [SKILLS BATCH 9] install + Agent Sprite Forge investigate"
```
