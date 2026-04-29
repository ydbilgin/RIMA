# Discord Export → Ollama Digest — PixelLab

PixelLab Discord'dan seçili kanalları (mesaj + image + video) lokal dump → Ollama analiz → kanal başına markdown digest + cross-channel MASTER.md.

**Authority weighting:** Staff/Mod/Pro/Verified rol'lü kullanıcılar `[AUTHORITY]` tag'iyle işaretlenir, Ollama prompt'u onları daha ağırlıklı analiz eder, digest'te ayrı **"From Authority"** bölümü çıkar.

---

## Senin Yapacakların (4 adım)

### 1) DiscordChatExporter.Cli indir

GitHub: `Tyrrrz/DiscordChatExporter` releases → en son release → **DiscordChatExporter.Cli.win-x64.zip** (self-contained, .NET kurulumu YOK).

Zip'i şu klasöre aç:
```
Tools/discord_export/dce/
```

İçinde `DiscordChatExporter.Cli.exe` olmalı.

### 2) Discord token al

⚠️ User token. Paylaşma. Git'e commit etme. Ban riski düşük ama var (Discord ToS gri).

1. **Tarayıcıda** Discord aç (discord.com/app), giriş yap
2. **F12** → DevTools → **Network** sekmesi
3. Filter kutusuna `messages` yaz
4. Herhangi bir kanala tıkla → Network'te `messages?limit=...` request görünür
5. Request'e tıkla → **Headers** → **Request Headers** → `authorization` satırını kopyala
6. `Tools/discord_export/token.txt`'ye yapıştır (tek satır, başka şey yok)

### 3) Kanal ID'leri yapıştır

1. Discord → **User Settings** (dişli) → **Advanced** → **Developer Mode: ON**
2. PixelLab sunucusuna gir
3. Kritik kanallara **sağ tık → Copy Channel ID**
4. `Tools/discord_export/channels.txt`'ye yapıştır:

```
123456789012345678  showcase
234567890123456789  help
345678901234567890  tips
456789012345678901  pro-mode
567890123456789012  character-art
```

Format: `<id> <label>`. Label dosya adında kullanılır, sade tut. 4-5 kanal yeter.

### 4) Çalıştır — 2 aşamalı kullanım (önerilen)

İki adım **bilinçli olarak ayrı**: export uzun sürer (gece/iş yaparken), analiz Ollama VRAM'i kullanır (çalışmadığın zaman).

**A) Sadece RAW export (DCE, Ollama'ya değmez):**
```powershell
cd "Tools/discord_export"
.\export.ps1 -DaysBack 30        # son 30 gün dump
```
Sonuç: `STAGING/discord_pixellab/<channel>.json` + `<channel>_media/`. **Ollama açık olmasına gerek yok.** Bilgisayarda başka iş yaparken arka planda dönebilir.

Forum kanalı (`help-questions-support`) thread-merge için ayrıca:
```powershell
.\test-help-channel.ps1          # forum thread'leri tek JSON'a merge
```

**B) Sadece Ollama analiz (export hazırken, manuel başlat):**
```powershell
# Text-only (hızlı, ~dakikalar)
python analyze.py

# + Image vision (yavaş, saatler — image limit YOK, hepsini işler)
python analyze.py --with-images

# + Video keyframes (ffmpeg gerek, çok yavaş)
python analyze.py --with-images --with-videos
```

Ya da tek tuş (export + analiz birlikte):
```powershell
.\run-all.ps1 -DaysBack 30 -WithImages
```

---

## Modeller — neden 14b text + 26b vision?

`qwen2.5:14b` text-only model, image alamaz. Vision için **gemma4:26b** (native multimodal, Q4 quantized 16GB'a sığar, pixel art spatial reasoning'de en iyi). Sığmazsa fallback: `llama3.2-vision:11b` (~8GB rahat).

| Aşama | Model | VRAM |
|---|---|---|
| Text chunk + sentez + executive | `qwen2.5:14b` | ~9GB |
| Image / video frame vision | `gemma4:26b` | ~14GB |
| Vision fallback (VRAM yetersizse) | `llama3.2-vision:11b` | ~8GB |

`run-all.ps1` model eksikse otomatik `ollama pull` yapar (~5-15 dk download).

Video için `ffmpeg` gerek: `winget install Gyan.FFmpeg`

---

## Çıktılar

Hepsi: `STAGING/discord_pixellab/`

```
discord_pixellab/
  showcase.json                  # raw DCE export
  showcase_media/                # indirilen image/video
  help.json
  help_media/
  ...
  digest/
    showcase_text.md             # text digest
    showcase_images.md           # vision digest (Phase 2)
    showcase_videos.md           # video digest (Phase 3)
    ...
    MASTER.md                    # tüm kanalların birleşimi + Authority Index
    for_claude.md                # ⭐ Executive summary (Claude bunu okur)
```

**Claude için:** `for_claude.md` ~30-50 satır, executive summary. Sadece bu okunur, MASTER fallback.
**Sen için:** `MASTER.md` cross-channel authority index + her kanalın detaylı digest'i.

---

## Authority Sistemi

`authority_roles.txt` rol pattern listesi (case-insensitive substring):
- `staff`, `moderator`, `mod`, `admin`, `dev`, `team`, `pixellab`, `verified`, `expert`, `pro`, `contributor`, `helper`, `trusted`...

Bir rol adı bu pattern'lerden birini içeriyorsa o kullanıcı **AUTHORITY**.

**Etkileri:**
1. Mesaj chunk'ında `[AUTHORITY: <rol>]` tag'i ile gider → Ollama text prompt'u "weight higher" der
2. Image/video phase'de **authority-first ordering** (önce yetkili mesajları işler)
3. Synthesis prompt'u "From Authority" başlığı altında verbatim quote eder
4. MASTER.md'de **Cross-Channel Authority Index** çıkar (kim kaç kanalda kaç mesaj)

PixelLab Discord'a girince gerçek rol adları görürsen `authority_roles.txt`'ye eklersin (ör. "PixelLab Team", "Subscriber") — case fark etmez, substring eşleşir.

---

## Pre-flight Sorunları

| Hata | Çözüm |
|---|---|
| `DCE bulunamadi` | Adım 1 yap, zip'i `dce/` altına aç |
| `token.txt yok` | Adım 2 yap |
| `channels.txt'de aktif kanal yok` | Adım 3 yap, # işaretlerini kaldır |
| `Ollama erisilemedi` | Başka terminalde `ollama serve` çalıştır |
| `Eksik Ollama modeli` | `run-all.ps1` otomatik `ollama pull` yapar (~5-15 dk download) |
| `Python yok` | `winget install Python.Python.3.12` |
| `ffmpeg yok` (video phase) | `winget install Gyan.FFmpeg` (sadece video phase için) |

---

## Tekrar Çalıştırma

```powershell
# Sadece export (yeni mesajları çek)
.\export.ps1 -DaysBack 7

# Sadece analiz (mevcut export üstünden tekrar)
python analyze.py --with-images
```

DCE `--reuse-media` flag ile zaten indirilmiş attachment'ları tekrar indirmez. Yeni mesajlar varsa sadece onları çeker.

## Periyodik Plan (full run sonrası)

Haftada 1 son 7 gün → hızlı export + analiz, yeni dev cevap/tip varsa Claude memory'e ekler. `/schedule` ile cron otomatize edilebilir.

---

## Güvenlik

- `token.txt` → `.gitignore`'da
- `dce/` → `.gitignore`'da (zip büyük)
- `STAGING/discord_pixellab/` → asset/dump bölgesi, repo'ya commit edilmez (Unity .gitignore zaten STAGING'i tutar)
- Token sızdırırsa: Discord → Settings → Devices → Logout (token invalidate)
