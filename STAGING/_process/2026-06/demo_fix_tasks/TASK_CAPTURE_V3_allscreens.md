# TASK — Exhaustive screen capture V3 (ALL possible screens, capture-truth verified)

ACTIVE RULES: (1) think before acting (2) min steps (3) surgical — capture only, NO code/scene edits (4) BLOCKED if a screen unreachable.

NLM ACCESS: gerekirse NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR, raporda console durumunu yaz.

E1 OUTPUT: Sonucu DOSYAYA yaz (DONE + manifest); dönüşün ≤10 satır + yol.

## AMAÇ
RIMA'nın **olabilecek BÜTÜN ekran/state'lerinin** TAZE, GERÇEK screenshot'larını al. Bu görseller hem demo-bitirme kararının kanıtı hem de ChatGPT review paketinin içeriği olacak. Özellikle **düşman/boss combat state'leri** eksiksiz olmalı (telegraph/VFX değerlendirmesi için).

## ⚠️ CAPTURE-TRUTH (kritik — önceki batch'te sahte-dup bug'ı vardı: 08=09, 20=21 aynı SHA)
- Mevcut çalışan metod: `STAGING/_process/2026-06/demo_screenshots/full/` 25 görseli bu metodla alındı (MCP 9.7.3, screenshot+input ÇALIŞIYOR, overlay UI görünüyor). AYNI metodu kullan.
- **HER capture'dan sonra:** dosya SHA256 (veya boyut+ilk-byte) hesapla, bir öncekiyle KARŞILAŞTIR. Aynıysa → ekran gerçekten değişmemiş → navigasyonu tekrar yap, screen state'in değiştiğini doğrula, yeniden çek. ASLA dup kabul etme.
- Manifest tut: `filename | screen-state | sha (ilk 12) | unique?`.
- Mümkünse her capture öncesi 1-2 frame ilerlet (state otursun).

## ÇIKTI KLASÖRÜ
`STAGING/_process/2026-06/demo_screenshots/capture_v3/` (numaralı, açıklayıcı isim: `NN_kategori_state.png`).

## EKRAN ENVANTERİ (en az bunlar; ulaşabildiğin EKSTRA state'i de ekle)
**Menü/giriş:** main menu · character select (varsa 2+ class kartı) · settings · pause · codex · character sheet (Tab)
**Run akışı:** run map (branching, açık) · ilk oda · oda geçiş/portal · minimap (M) · merchant/shop room (standlar) · elite room
**Combat (ÖNEMLİ — düşman/boss):**
  - boş combat (düşman yok)
  - **çok düşmanlı combat** (3+ mob görünür) — birden fazla mob tipiyle
  - **düşman saldırı anı** (varsa mevcut bir attack/telegraph state)
  - full-HP HUD · mid-HP · **low-HP (vignette)** · death screen
  - skill draft / offer ekranı · skill kullanımı (LMB/Q/E/R/F VFX görünürse)
**Boss:** boss room spawn · boss her faz (P0/P1/… ulaşabildiğin) · boss health bar (full + mid) · boss saldırı/residue anı
**Tooling (centerpiece):** Build Mode F2 entry · prop tool (19 prop paleti görünür) · prop yerleştirilmiş · tile tool · paint/erase · Director Mode her tab (spawn/stats/telemetry/prop/light) · free-cam

## NOTLAR
- Mevcut düşman görünmüyor/black-blob ise → AYNEN çek + raporda "enemy invisible/unbound" diye işaretle (bu kararın girdisi).
- Overlay UI bazı metodlarda çıkmayabilir; çıkmıyorsa raporda hangi ekran çıkmadı yaz (sahte-dup üretme).
- Full-flow için `playModeStartScene=MainMenu`; dev-direct gerekirse `_Arena`.

## RAPOR
`STAGING/_process/2026-06/demo_screenshots/capture_v3/DONE_capture_v3.md`:
- Manifest tablosu (filename | state | sha12 | unique)
- Kaç ekran çekildi / kaç tanesi dup-retry gerektirdi / ulaşılamayan ekran
- Düşman/boss combat state durumu (görünür mü, kaç mob tipi, telegraph/VFX var mı YOK mu)
- Console durumu
