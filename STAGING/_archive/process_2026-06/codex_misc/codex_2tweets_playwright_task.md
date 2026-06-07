ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — 2 X Tweet Scrape + Screenshot (Playwright)

**Amaç:** 2 X tweet'i Playwright skill ile aç, full content extract et, screenshot al. LaurethStudio + RIMA için Claude orchestrator analiz edecek.

## URLs

1. `https://x.com/npaka123/status/2058326600396206406`
2. `https://x.com/npaka123/status/2058031349878194515`

## Playwright Workflow

`~/.codex/skills/playwright/` skill (all profile available).

Her tweet için:
1. Headless Chromium aç
2. URL'ye git, tweet container yüklenmesini bekle (`article[data-testid="tweet"]` selector)
3. Eğer login wall: screenshot yine al + nitter fallback (`nitter.net/npaka123/status/...` veya `nitter.poast.org`)
4. Tweet text DOM'dan extract et
5. Embed image/video URL'ler varsa kaydet
6. Thread replies (`/with_replies`) varsa scrape
7. Full page screenshot

## Çıktı

`STAGING/tweets_npaka123/`:
- `tweet_2058326600396206406.png` (screenshot)
- `tweet_2058326600396206406_text.md` (extracted text + metadata)
- `tweet_2058031349878194515.png`
- `tweet_2058031349878194515_text.md`

## Text extract format

```markdown
# Tweet [ID]

**Author:** @npaka123
**Date:** [from tweet]
**URL:** [original]

## Content
[Full tweet text, translate from Japanese to English if needed]

## Embedded Media
- [Image URLs, video URLs]

## Thread Replies
[If any, list with author + content]

## Topic Tags
[AI / Claude / Codex / skill / game-dev / workflow / etc.]
```

## Fallback Strategy

1. X direct → headless Chromium (may have login wall)
2. If login wall: nitter.net mirror try
3. If nitter fail: web.archive.org cached version
4. If all fail: BLOCKED report with screenshot of login wall

## Önemli notlar
- Tweet'ler Japanese (npaka123 Japan dev), translate gerek
- Login wall sıklıkla X'te — nitter fallback CRITICAL
- Tweet content RIMA/LaurethStudio için relevant olabilir (Claude/skill ecosystem related — önceki sakevoid tweet'i find-skills'den bahsediyordu)
- Screenshot kalite önemli (1080×high)

## Git commit

```bash
git add STAGING/tweets_npaka123/
git commit -m "[Codex] [TWEET SCRAPE] 2 npaka123 tweets Playwright + screenshot"
```
