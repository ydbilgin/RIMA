ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Skills.sh Universal Install (Claude + Codex)

**Amaç:** 3 skill'i HEM Claude Code'a HEM tüm Codex profile'larına install et. Skills agent-agnostic (skills.sh hepsini destekliyor), orchestrator capacity artırma.

User insight (2026-05-24 tweet/sakevoid): find-skills Claude Code orchestrator için workflow discovery sağlıyor — tek prompt'la yüzlerce skill'i tarayıp en uygun chain'i kuruyor. Bu massive workflow simplification.

## Install Edilecek 3 Skill

### 1. find-skills (EN ÖNEMLİ)
- URL: https://www.skills.sh/vercel-labs/skills/find-skills
- Author: Vercel Labs
- İşlev: Skill discovery — agent tek prompt yazar, find-skills yüzlerce skill'i tarar, en uygun workflow'u kurar
- **Primary target: Claude Code (orchestrator)**
- Secondary: Codex profile'lar

### 2. image-inpainting
- URL: https://www.skills.sh/ → search "image-inpainting"
- İşlev: Cell-by-cell asset düzeltme, wall variant fix
- Primary target: Claude Code + Codex

### 3. image-outpainting
- URL: https://www.skills.sh/ → search "image-outpainting"
- İşlev: Asset etrafına genişletme (wall + zemin birlikte üretip ayırma)
- Primary target: Claude Code + Codex

## Install Locations

### Claude Code skills location
- `~/.claude/skills/` (Claude Code custom skills folder)
- Veya `~/AppData/Roaming/Claude/skills/` (Windows AppData)
- Eğer Claude Code skill directory mevcut değilse: create it

### Codex skills locations
- Global: `~/.codex/skills/`
- Per-profile (4 profile): `~/.codex-profiles/{laurethayday,laurethgame,yasinderyabilgin,ydbilgin}/skills/`

## Görev Adımları

### Adım 1: skills.sh install command research
Site'i ziyaret et, exact install command'i öğren:
- `https://www.skills.sh/` homepage
- `https://www.skills.sh/vercel-labs/skills/find-skills` skill page (install instructions burada olmalı)

Tipik install pattern'ler:
- `npx skills install <skill-name>`
- `curl -sL https://skills.sh/install.sh | bash`
- `git clone <skill-repo>` + manual copy
- Yine `skills add <name>` veya `skills.sh add <name>`

Bul exact command, raporla.

### Adım 2: Claude Code skill install
1. Claude Code skill directory tespit (`~/.claude/skills/` veya `%APPDATA%\Claude\skills\`)
2. Yoksa create
3. 3 skill install (find-skills + image-inpainting + image-outpainting)
4. Test: skill SKILL.md readable her biri için

### Adım 3: Codex skills install (global + 4 profile)
1. Global install: `~/.codex/skills/`
2. Per-profile install: 4 profile'a kopyala (laurethayday, laurethgame, yasinderyabilgin, ydbilgin)
3. Test: her profile'da skill SKILL.md readable

### Adım 4: Skill compatibility test
- find-skills: Claude Code'da hangi skill discovery patternleri destekliyor doğrula
- image-inpainting: API/SDK gereksinim var mı (OPENAI_API_KEY vs.)
- image-outpainting: aynı

### Adım 5: Rapor yaz

`STAGING/skills_install_universal_report.md`:

```markdown
# Skills.sh Universal Install Report

## Install Command Used
[exact command]

## find-skills
- Claude Code: SUCCESS / FAIL + path
- Codex global: SUCCESS / FAIL
- Codex per-profile (4): SUCCESS / FAIL each
- SKILL.md content summary
- Dependencies: [API keys, env vars, MCP requirements]

## image-inpainting
[same structure]

## image-outpainting
[same structure]

## Compatibility Notes
- find-skills Claude Code'da nasıl çalışır
- image-inpainting model dependency (OpenAI API key?)
- image-outpainting aynı

## Next Step Önerisi
- find-skills nasıl test edilir (sample prompt)
- image-inpainting RIMA wall iterasyonu için nasıl kullanılır
```

## Hard Constraints

- Skills.sh resmi install command kullan (random script çalıştırma)
- OPENAI_API_KEY arama — skill içinde belirtilirse not düş, manual install ETME
- Claude Code skill location FAIL ederse: BLOCKED ver, manual install path öner
- Eğer 3 skill'in herhangi biri install edilemezse: kısmi success raporu, FAIL'leri belirt

## Git Commit

```bash
git add STAGING/skills_install_universal_report.md
git commit -m "[Codex] [SKILLS UNIVERSAL] find-skills + image-inpainting + image-outpainting (Claude + Codex)"
```

## Önemli Notlar

- Tweet konusu zaten user tarafından özetlendi, screenshot gerek YOK (bu görevden çıkar)
- Sadece skill install + rapor
- Eğer find-skills MCP based ise (tweet "küçük bir mcp paketi gibi oturuyor" diyor), MCP config dosyaları edit gerek olabilir — bu durumda dikkatli ol, mevcut MCP config bozma
- find-skills tweet'te "Claude Code içine" diyor — primary install target Claude Code, Codex secondary
