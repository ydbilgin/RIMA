ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Skills.sh Install + Tweet Screenshot (Dual)

**Amaç:** 2 paralel iş:
1. skills.sh'ten image-inpainting + image-outpainting skill install (Codex profile'larına ek tool)
2. X tweet'i Playwright skill ile screenshot, kaydet (orchestrator görsel inceleyecek)

## Görev 1 — Skills.sh Install

URL: https://www.skills.sh/

Install et:
- `image-inpainting` skill
- `image-outpainting` skill

Skills.sh'in install komutu:
- Site "Install them with a single command" diyor
- Tipik format: `skills install <skill-name>` veya `npx @skills-sh/cli install <name>` veya benzer
- Site'i ziyaret edip exact install command'i bul (homepage veya skills page'de gösterir)

Install location: Codex skills klasörüne ekle. Mevcut skill'ler:
- `~/.codex/skills/` (global)
- `~/.codex-profiles/{laurethayday,laurethgame,yasinderyabilgin,ydbilgin}/skills/` (per-profile)

Default install location'a yükle (genelde global ~/.codex/skills/). Eğer per-profile ise tüm 4 profile'a kopyala.

Test:
- Skill kuruldu mu doğrula (`ls ~/.codex/skills/` veya benzer)
- Skill SKILL.md dosyası okunur mu

Eğer install command bulamıyorsan veya skill mevcut değilse:
- BLOCKED ver, exact reason + manual install steps + alternative skill ismi öner

## Görev 2 — Tweet Screenshot (Playwright)

URL: https://x.com/sakevoid/status/2058431528422535506

Playwright skill (`~/.codex/skills/playwright/` global, all profiles) kullan:
- Headless Chromium aç
- Tweet URL'ye git
- Tweet container yüklenmesini bekle (`article[data-testid="tweet"]` selector veya benzer)
- Eğer login wall veya rate limit gelirse: tam sayfa screenshot yine al, login wall görünür
- Screenshot kaydet: `STAGING/tweet_sakevoid_screenshot.png` (full page veya tweet container)

PNG output:
- Tweet container öncelik (sadece tweet görsel)
- Yoksa full page screenshot

Eğer Playwright X login wall'a takılıyorsa:
- Nitter mirror dene: `nitter.net/sakevoid/status/2058431528422535506` veya başka public nitter instance
- Cached version: `web.archive.org/web/https://x.com/sakevoid/status/2058431528422535506`
- Yoksa screenshot yine al, login wall görünür ve orchestrator karar verir

## Çıktı

### Skills install rapor
`STAGING/skills_install_report.md`:
- Install command used
- image-inpainting: SUCCESS / FAIL + path
- image-outpainting: SUCCESS / FAIL + path
- Test: skill SKILL.md readable
- Per-profile install: YES / NO

### Tweet screenshot
`STAGING/tweet_sakevoid_screenshot.png` (PNG, full tweet veya page)
+ `STAGING/tweet_sakevoid_text_extracted.md`:
- Extracted tweet text (Playwright DOM text)
- Author
- Date
- Thread replies (varsa)
- Image/video URLs (varsa)

## Hard Constraints

- Skills install için sadece skills.sh resmi command kullan (random scripts indirme)
- Playwright headless mode (browser UI açma)
- Screenshot 1080×1920 veya tweet container boyut
- Eğer X login wall: nitter fallback dene, yoksa raw screenshot yine al
- Login wall'a giriş yapma (auth gerek değil)
- Eğer 5 dk içinde tweet yüklenemezse BLOCKED ver

## Git Commit

```bash
git add STAGING/skills_install_report.md STAGING/tweet_sakevoid_screenshot.png STAGING/tweet_sakevoid_text_extracted.md
git commit -m "[Codex] [SKILLS+TWEET] skills.sh install image-inpainting/outpainting + tweet screenshot"
```
