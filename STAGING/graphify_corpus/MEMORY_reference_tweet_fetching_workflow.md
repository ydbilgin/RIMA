---
name: tweet-fetching-workflow
description: "How to fetch x.com / Twitter tweet content + replies + video when WebFetch returns 402. Fallback chain — yt-dlp first, Playwright second, manual paste last. Verified 2026-05-18 on Win11."
metadata: 
  node_type: memory
  type: reference
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# Tweet Fetching — Working Fallback Chain (2026-05-18 LOCK)

**Problem:** `WebFetch https://x.com/...` returns **HTTP 402 Payment Required** for unauth requests. Nitter public mirrors all dead (`nitter.net` empty, `xcancel.com` 503, `nitter.privacydev.net` ECONNREFUSED). Gemini `-p` can hit `MODEL_CAPACITY_EXHAUSTED` server-side.

**Why this matters:** Tweets are a primary RIMA design reference source (Hades devs, pixel-art devs, Boona-type showcases). Losing them = losing reference material.

## Working chain (try in order)

### 1. yt-dlp — for OP tweet + video (PRIMARY)
Already installed at `/c/Program Files/Python312/Scripts/yt-dlp` (v2026.03.17+).

```bash
cd "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING"
yt-dlp --write-info-json --skip-download --no-warnings \
  "https://x.com/<user>/status/<id>" -o "tweet.%(ext)s"
```

Then read `tweet.info.json` — fields: `title`, `description` (full tweet text), `uploader`, `uploader_id`, `like_count`, `repost_count`, `comment_count`, `upload_date`, `subtitles`.

**Video download (if attached):**
```bash
yt-dlp -f "best[height<=720]" --no-warnings \
  "https://x.com/<user>/status/<id>" -o "tweet_video.%(ext)s"
```

**Frame extraction (for visual analysis):**
```bash
ffmpeg -y -i tweet_video.mp4 -vf "fps=1/3,scale=960:-1" frames/frame_%02d.png -loglevel error
```
Then `Read` 4-5 frames spread across the timeline.

**Limit:** yt-dlp gets the OP tweet only — **no replies**.

### 2. Playwright — for replies / thread (SECONDARY)
Already installed (`playwright install chromium` done). Script template at `STAGING/scrape_tweet_replies.py`.

```bash
cd "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING"
python scrape_tweet_replies.py "https://x.com/<user>/status/<id>" out.json
```

**Limit:** Unauth Playwright also hits x.com's reply-wall — typically returns OP only (count=1). For replies, need logged-in cookies.

**Win11 cookie extraction gotchas (verified 2026-05-18):**
- `browser_cookie3.chrome()` → `RequiresAdminError` (shadowcopy needs admin since Win11 Chrome locks DB)
- `yt-dlp --cookies-from-browser chrome` → "Could not copy Chrome cookie database" (yt-dlp #7271) if Chrome is running
- `browser_cookie3.firefox()` → works without admin BUT returns 0 cookies if user not logged into x.com on Firefox
- `browser_cookie3.edge()` → "Unable to get key for cookie decryption" (DPAPI issue on Win11)

**Working paths for cookies (in order):**
1. **Get cookies.txt LOCALLY Chrome extension** — user installs, navigates to x.com (must be logged in), exports `cookies.txt` (Netscape format) to `STAGING/cookies_x.txt`. Then yt-dlp `--cookies STAGING/cookies_x.txt` OR Playwright `add_cookies()` from parsed file. **One-time setup, reusable.**
2. **Close Chrome → run `yt-dlp --cookies-from-browser chrome ...`** — works because DB lock releases. Tell user to close Chrome briefly.
3. **Manual paste from user** — fastest one-shot fallback.

### 3. Manual paste (FALLBACK)
Ask user to open x.com in browser, copy tweet + replies text, paste into chat. Last resort but guaranteed.

## What NOT to try (all confirmed dead 2026-05-18)
- `WebFetch x.com` → 402
- `WebFetch nitter.net` → empty body
- `WebFetch xcancel.com` → 503
- `WebFetch nitter.privacydev.net` → ECONNREFUSED
- `WebFetch twstalker.com` → socket closed
- `gemini -p "fetch <x.com url>"` → MODEL_CAPACITY_EXHAUSTED (server-side throttle, retry rarely helps)

## Reusable assets
- `STAGING/scrape_tweet_replies.py` — Playwright scraper template
- `STAGING/boona_tweet.info.json` — yt-dlp output sample
- `STAGING/boona_video.mp4` — sample video output
- `STAGING/boona_frames/` — sample frame extraction

## Related
- [[research-delegate-to-agents]] — orchestrator delegates research; but tweet-fetch is mechanical enough to do directly with this chain
- [[codex-parallel-profile-workflow]] — for parallel 3-agent review of a fetched tweet
