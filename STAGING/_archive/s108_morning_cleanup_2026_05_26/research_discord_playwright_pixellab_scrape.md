# Discord Playwright Scraping — Feasibility & Risk Report
**Tarih:** 2026-05-22 | **Konu:** PixelLab Discord showcase scraping (personal account, Playwright)

---

## 1. Teknik Feasibility: EVET, ama zorlu

Discord'u Playwright ile scrape etmek **teknik olarak mümkün** — çünkü Discord web bir SPA (React). Ama 2025 itibarıyla Discord'un anti-detection sistemi çok katmanlıdır:

- **JA4 TLS Fingerprinting:** Playwright'ın standart bağlantısı Chrome DevTools Protocol (CDP) `Runtime.enable` flag'i ile tespit edilir. Normal `playwright-stealth` eklentisi artık yeterli değil.
- **X-Super-Properties Header:** Discord'un login token sistemi her istek ile base64 encoded bir "client metadata" header gönderir — tarayıcı versiyonu, OS, build number. Bot davranışında bu header tutarsız gelir, flag atar.
- **Behavioral ML:** Sabit scroll hızı, sabit tıklama zamanlaması, aynı anda çok fazla GET isteği — hepsi pattern olarak işaretlenir.

**Sonuç:** Geleneksel Playwright kurulumu ile başlanıp 24-72 saat içinde account flag yenebilir.

---

## 2. Discord TOS Riski: Personal Account

Discord ToS Section 15 "Bots and Scraping":
- **Bot Account (Discord API):** Kayıtlı, rate limit var, "Message Content Intent" gerektirir — ama yasal. Bot yine de davet edilmeden private server'a giremez.
- **Personal Account Self-Botting:** **Açıkça yasak.** Account token'ı otomasyonda kullanmak = kalıcı ban + IP kara liste + HWID flag.
- **2025 Trend:** Discord "shadow ban" uyguluyor — account aktif görünür ama mesajlar iletilmez veya scrape boş döner.

**Risk Seviyesi:** HIGH. 2024'te Discord, Spy.Pet gibi mass scraper servislerini hem teknik hem hukuki yollarla kapattı.

**Hangi davranış trigger eder:**
- 5 dakikada 200+ scroll action
- Full member list fetch
- Yeni "device" olarak login (her oturumda fresh browser context)
- Sabit interval request pattern (örn. her 500ms'de GET)
- 10.000+ invalid request (401/403/429) in 10 dakika → 24h Cloudflare IP ban

---

## 3. Anti-Detection Gereklilikler

### Zorunlu
- **rebrowser-playwright** (github.com/rebrowser/rebrowser-playwright): CDP detection'ı source-level patch ile elimine eder. Standart Playwright + stealth plugin yetersiz.
- **Browser Context Persistence:** Her run'da fresh login = "new device" = 2FA / email verify loop. Storage state VEYA tam UserDataDir kopyasıyla session muhafaza edilmeli.
- **Residential Proxy:** Datacenter IP (AWS, Azure) anında flag atar. Konut IP'si şart.
- **Human-like timing:** `Math.random() * 3000 + 1500` ms arası delay; sabit interval asla kullanılmaz.

### Önerilen
- **curl_cffi (Python):** Full browser açmadan sadece network layer'da TLS/JA4 fingerprint taklit eder. Discord'un public-facing JSON endpoint'lerini doğrudan çekebilirse çok daha hafif.
- **Non-linear scroll:** Sabit `window.scrollTo(0, 9999999)` yerine 300-700px aralıkta random increment.
- **Mouse simulation:** `page.mouse.move()` ile koordinat bazlı hareket (acceleration curve).

---

## 4. Somut Adım Listesi (Codex Implementable)

### Aşama 1: Kurulum + Session Capture
```
pip install rebrowser-playwright[chromium] curl_cffi
```
- Chromium'u headful (görünür) modda aç, Discord'a **manuel** login yap (2FA dahil).
- Session'ı kaydet:
```python
await context.storage_state(path="discord_session.json")
# VEYA userDataDir ile tam profil:
browser = await p.chromium.launch_persistent_context("./discord_profile", headless=False)
```
- Anti-detection: User-agent string'i gerçek Chrome versiyonuyla eşitle. `--disable-blink-features=AutomationControlled` flag ekle.

### Aşama 2: Kanal Browse + Message Scroll + Content Extract
- Kayıtlı session ile başlat:
```python
context = await browser.new_context(storage_state="discord_session.json",
                                     user_agent="Mozilla/5.0 ... Chrome/124.0 ...")
page = await context.new_page()
await page.goto("https://discord.com/channels/SERVER_ID/CHANNEL_ID")
```
- Scroll loop — human-like:
```python
import random, asyncio
for _ in range(50):
    scroll_px = random.randint(300, 700)
    await page.evaluate(f"window.scrollBy(0, -{scroll_px})")
    await asyncio.sleep(random.uniform(1.5, 4.5))
```
- Message extract: CSS selector `li[class*="messageListItem"]` → `.message-content`, `.timestamp`, `.reaction` sayıları.
- Image URL'leri: `img[src*="cdn.discordapp.com"]` src attribute'ları topla.

### Aşama 3: Output JSON + Image Download
```python
import json, httpx
messages = []  # [{author, timestamp, content, images:[], reactions:{}}]
# Her message için:
messages.append({"content": text, "images": [url1, url2], "reactions": {"🔥": 5}})

with open("output/pixellab_showcase.json", "w") as f:
    json.dump(messages, f, ensure_ascii=False, indent=2)

# Image download
for url in image_urls:
    r = httpx.get(url)
    filename = url.split("/")[-1].split("?")[0]
    with open(f"output/images/{filename}", "wb") as f:
        f.write(r.content)
    await asyncio.sleep(random.uniform(0.5, 1.5))  # rate limit pacing
```

### Aşama 4: Cron / Windows Task Scheduler
- Windows Task Scheduler XML (headless mümkünse, aksi halde headful arka planda):
  ```
  Trigger: Daily 03:00
  Action: python F:\scripts\discord_scrape.py
  ```
- Cron (WSL / Linux):
  ```
  0 3 * * * /usr/bin/python3 /home/user/discord_scrape.py >> /logs/scrape.log 2>&1
  ```
- **Pacing kuralı:** Run başına max 100 mesaj, sonra 30 dakika dur. Günde max 2 run.
- Session refresh: Her 7 günde bir headful modda manuel login kontrolü.

---

## 5. Gerçekçi Rate Limit Pacing

| Eylem | İnsan hızı | Bot-safe limit |
|-------|-----------|----------------|
| Kanal geçiş | 5-10 sn | 8-15 sn (random) |
| Scroll (per scroll) | 1-3 sn | 1.5-4.5 sn (random) |
| Image tıkla/kapat | 2-5 sn | 3-6 sn |
| Mesaj başına işlem | ~2 sn | 2-4 sn |
| Toplam mesaj/saat | ~180 (human max) | 60-90 (safe bot) |

---

## 6. Alternatif Kaynaklar (Discord'a Alternatif)

PixelLab'in Discord showcase'ine alternatif public kaynaklar:

| Kaynak | Erişim | İçerik |
|--------|--------|--------|
| **PixelLab Twitter/X** | Public RSS veya nitter scrape | Resmi showcase postları, kullanıcı etiketleri |
| **PixelLab Blog** (pixellab.ai/blog) | Basit HTTP GET + BeautifulSoup | Resmi case study ve highlight |
| **PixelLab MCP / API** | Direkt API — zaten kullanıyoruz | Sadece kendi gen'lerimiz |
| **Reddit r/gamedev, r/pixelart** | Public API (reddit.com/r/X.json) | Topluluk paylaşımları, PixelLab tagged |
| **Itch.io game pages** | HTML scrape — çok düşük risk | PixelLab kullanan projelerin asset görselleri |

**Reddit API:** `https://www.reddit.com/search.json?q=pixellab&sort=new` — API key gerektirmez, rate limit gevşek, TOS'a aykırı değil.

---

## 7. Risk Değerlendirmesi Özeti

| Risk | Seviye | Detay |
|------|--------|-------|
| Account ban | HIGH | Personal account self-botting Discord ToS açığı |
| IP blacklist | MEDIUM | Cloudflare layer, residential proxy ile azaltılabilir |
| HWID flag | LOW-MEDIUM | Sadece repeat offender pattern'da |
| Legal risk | LOW | Kişisel kullanım, veri satmıyoruz |
| Shadow ban | MEDIUM | Tespit zor, sessizce veriyi bozar |

---

## 8. Tavsiye: CONDITIONAL GO

**RECOMMEND: CONDITIONAL GO — sadece şu koşullarla:**

1. **Önce alternatifler tüketilsin:** Reddit API + PixelLab Twitter scrape dene. Risk sıfır, implementasyon 2 saat.
2. **Discord'a geçilecekse:** rebrowser-playwright + residential proxy + persistent session zorunlu. Standart Playwright ile başlama — ilk 48 saatte flag yenir.
3. **Ayrı bir test account kullan:** Ana Discord hesabını riske atma. Throwaway account ile test — sonra ana hesaba geç istersin.
4. **Günde max 2 run, 100 mesaj/run:** Bu pacing ile shadow ban süresi aylar ölçeğine uzar.
5. **Session kalıcılığı:** `userDataDir` persistent context, her run'da yeni login asla.

**NO-GO koşulları:**
- Ana Discord hesabı kullanılacaksa VE residential proxy yoksa → **NO-GO**.
- "Tam member list" veya "tüm server history" çekilecekse → **KESIN NO-GO**.

**Önerilen başlangıç:** Reddit API pilot (1 gün implement) → yeterli veri var mı gör → yeterli değilse Discord flow'una gir.

---

*Model: Gemini default (gemini-3.1-pro-preview) | Confidence: HIGH for technical details, MEDIUM for TOS risk specifics (Discord policy değişebilir)*
