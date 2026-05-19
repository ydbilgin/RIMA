---
name: PixelLab Discord Export + Ollama Digest Pipeline
description: Periyodik PixelLab Discord scrape → Ollama (qwen2.5:14b text + qwen2.5vl:7b vision) digest. Forum kanalları için thread-merge gerek. Tools/discord_export/.
type: project
originSessionId: 211c5ab1-b4dc-4efe-ba03-5b2b874a86bb
---
## Amaç
PixelLab Discord'unda kullanıcılar + dev'ler ne paylaşıyor → lokal dump → Ollama analiz → workflow tip / dev cevap / pitfall digest. RIMA pipeline'ına uygun parçaları memory'e al.

## Stack (S42 — 2026-04-26)
- **DCE:** `Tools/discord_export/dce/DiscordChatExporter.Cli.exe` v2.47.1
- **Ollama:** `qwen2.5:14b` (text) + `qwen2.5vl:7b` (vision), localhost:11434
- **Authority:** `authority_roles.txt` — `developer/Moderator/staff` rolleri ağırlıklı analiz
- **Confirmed dev (Kaninen):** PixelLab developer, 27 mesaj/5 kanal — quote'ları altın

## Model Rationale (neden 14b text + 7b vision)
qwen2.5:14b text-only, image alamaz. Multimodal `qwen2.5vl` ailesinde 14b YOK (3b/7b/32b/72b). 32b 16GB VRAM'a sığmaz. 7b en iyi sığan vision. Alternatif `llama3.2-vision:11b` daha kaliteli olabilir (16GB rahat) — vision çıktısı zayıfsa geç. Text 14b doğru: 32b 16GB'a zorlanır, hız yarılanır.

## Kanallar (9 adet, `channels.txt`)
mcp-and-vibe-coding · share-tips-tricks · pixellab-art-gallery · help-questions-support (FORUM) · api-and-sdk · projects · tutorials · helpful-posts · announcements

## KRİTİK: Forum kanalı (help-questions-support)
Forum'da her post = thread. DCE tek JSON'a yazamıyor → klasör/template şart. Çözüm `test-help-channel.ps1`'de:
1. Output template: `<dir>\%C.json` (her thread ayrı dosya, 55 thread bulundu)
2. Bütün thread JSON'ları PowerShell ile merge → tek `help-questions-support.json` (analyze.py formatına uygun)
3. `--include-threads All` flag (tüm forum kanalları için)

`export.ps1`'de zaten `--include-threads All` var ama tek dosya çıkışı forum'da fail eder. Forum kanalları için ayrı işlem gerek.

## PowerShell uyarısı
`Get-Content -Raw` bu makinede çalışmıyor (eski PS sürümü olabilir) → `[IO.File]::ReadAllText()` kullan. ConvertTo-Json `-Depth 100` (default 2 yetmez Discord JSON için).

## Kullanım — 2 Aşamalı (kullanıcı tercihi)
**A) Raw export (Ollama gerek yok, bilgisayarda başka iş yaparken arka planda):**
```powershell
.\export.ps1 -DaysBack 30
.\test-help-channel.ps1   # forum thread merge (help-questions-support)
```
**B) Ollama analiz (kullanıcı manuel başlatır, VRAM kullanır):**
```powershell
python analyze.py --with-images   # text + vision, image limit YOK
```
Tek tuş istersen: `.\run-all.ps1 -DaysBack 30 -WithImages`. Ama kullanıcı **iki adımı ayrı tutmayı** tercih etti — export gece/iş yaparken, analiz manuel.

## Image Limit
**YOK** (default 0 = cap yok, hepsi işlenir). Bilgisayar kasmıyor, donanım yeterli.

## Output (Claude için optimize)
- `_STAGING/discord_pixellab/digest/for_claude.md` — **Claude bunu okur** (~30-50 satır executive summary, qwen2.5:14b son aşamada üretir)
- `_STAGING/discord_pixellab/digest/MASTER.md` — kullanıcı için detaylı (~500+ satır)
- `_STAGING/discord_pixellab/digest/<channel>_text.md` / `_images.md` / `_videos.md` — kanal detayı
- Claude derinleşmek isterse → `for_claude.md`'deki sinyale göre kanal dosyasına iner

## Periyodik Çalıştırma
Tam tarama bir kez yapıldıktan sonra **haftada 1** son 7 günü çek + digest, yeni dev cevap/tip varsa memory'e ekle. `/schedule` ile otomatize edilebilir.

## Dosyalar
- `Tools/discord_export/README.md` — token alma, kanal ID, çalıştırma
- `Tools/discord_export/export.ps1` — full export (--include-threads All)
- `Tools/discord_export/test-help-channel.ps1` — forum thread merge testi
- `Tools/discord_export/analyze.py` — Ollama digest pipeline (chunk + synthesis + vision)
- `Tools/discord_export/run-all.ps1` — export + analyze tek tuş
- `Tools/discord_export/.gitignore` — token.txt ve dce/ git'ten dışarıda
- `Tools/discord_export/channels.txt` — 9 kanal ID dolu

## Output
`_STAGING/discord_pixellab/digest/MASTER.md` — kanal başına text digest + cross-channel authority index

## Ollama Kalite (S42)
**Patch v1 uygulandı (2026-04-26):**
- TEXT_PROMPT: "real prompt = multi-word descriptive" tanımı, UI dropdown VERBATIM zorunlu, authority verbatim quote zorunlu (<=200 char), chat/emoji/vague drop edilir
- SYNTHESIS_PROMPT: dedup `(seen Nx)`, authority quote koruma zorunlu, "From Authority" tek kaynak
- VISION_PROMPT: UI screenshot'ta her dropdown verbatim listele
- CHUNK_SIZE 40 → 60 (daha fazla bağlam, daha az sentez kaybı)
- **YENİ STAGE:** `EXECUTIVE_SUMMARY_PROMPT` → `for_claude.md` (Claude direkt bunu okur, MASTER fallback)

İlk run text-only digest (S42 60 gün) PASS ama gürültü vardı (örn. "Sprite + UI ^^" prompt sayılmıştı). Patch sonrası yeniden run gerek.

## RIMA için Discord-türetilen tip'ler (S42)
Detay: `feedback_pixellab_discord_lessons.md`
1. API > MCP (skeleton + extra method)
2. Skeleton anim = unusual pose (climb/fly)
3. Eye color/detay her anim prompt'unda tekrar
4. South-lock: start frame = last frame ✅ (zaten bizim kuralımız)
5. Recolor için shader, regenerate yok
6. Outfit ayrı layer + custom palette color reduction
7. Quadruped validator + V3/template direction grouping bug'lar var

## Risk
Token = hesap erişimi. `.gitignore` koruyor ama dikkat: `token.txt` asla commit etme, screenshot'a alma.
