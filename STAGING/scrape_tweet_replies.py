"""Scrape an x.com tweet + replies via Playwright.
Usage:
    python scrape_tweet_replies.py <tweet_url> <output.json> [cookies.txt]

cookies.txt = Netscape format (export from "Get cookies.txt LOCALLY" Chrome extension).
Without cookies, replies are hidden by x.com (you get OP only).
Tested 2026-05-18 on Win11/Python312/playwright 1.x.
"""
import sys, json, asyncio
from pathlib import Path
from playwright.async_api import async_playwright

def parse_netscape_cookies(path: str):
    cookies = []
    for line in Path(path).read_text(encoding="utf-8").splitlines():
        line = line.strip()
        if not line or line.startswith("#"):
            continue
        parts = line.split("\t")
        if len(parts) < 7:
            continue
        domain, flag, cpath, secure, expiry, name, value = parts[:7]
        try:
            expiry_i = int(expiry)
        except ValueError:
            expiry_i = -1
        c = {
            "name": name,
            "value": value,
            "domain": domain,
            "path": cpath,
            "secure": secure.upper() == "TRUE",
            "httpOnly": False,
        }
        if expiry_i > 0:
            c["expires"] = expiry_i
        cookies.append(c)
    return cookies

async def main(url: str, out: str, cookies_path: str | None):
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True)
        ctx = await browser.new_context(
            viewport={"width": 1280, "height": 2400},
            user_agent="Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0 Safari/537.36",
        )
        if cookies_path and Path(cookies_path).exists():
            cookies = parse_netscape_cookies(cookies_path)
            await ctx.add_cookies(cookies)
            print(f"Loaded {len(cookies)} cookies from {cookies_path}", file=sys.stderr)
        page = await ctx.new_page()
        await page.goto(url, wait_until="domcontentloaded", timeout=60000)
        try:
            await page.wait_for_selector('article[data-testid="tweet"]', timeout=25000)
        except Exception as e:
            print("WARN: no tweet article found:", e, file=sys.stderr)
        # Aggressive scroll to load all replies + nested replies
        prev_count = 0
        for i in range(25):
            await page.mouse.wheel(0, 2500)
            await page.wait_for_timeout(900)
            articles = await page.query_selector_all('article[data-testid="tweet"]')
            if len(articles) == prev_count and i > 5:
                # No new content for a while
                if i > 10:
                    break
            prev_count = len(articles)
        articles = await page.query_selector_all('article[data-testid="tweet"]')
        rows = []
        for art in articles:
            try:
                txt_el = await art.query_selector('div[data-testid="tweetText"]')
                txt = (await txt_el.inner_text()) if txt_el else ""
                user_el = await art.query_selector('div[data-testid="User-Name"]')
                user = (await user_el.inner_text()) if user_el else ""
                time_el = await art.query_selector("time")
                ts = (await time_el.get_attribute("datetime")) if time_el else ""
                rows.append({"user": user.replace("\n", " | "), "time": ts, "text": txt})
            except Exception as e:
                rows.append({"error": str(e)})
        with open(out, "w", encoding="utf-8") as f:
            json.dump({"url": url, "count": len(rows), "items": rows}, f, ensure_ascii=False, indent=2)
        print(f"OK: {len(rows)} tweets/replies -> {out}")
        await browser.close()

if __name__ == "__main__":
    cookies = sys.argv[3] if len(sys.argv) > 3 else None
    asyncio.run(main(sys.argv[1], sys.argv[2], cookies))
